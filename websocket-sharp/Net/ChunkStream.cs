using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace WebSocketSharp.Net
{
	internal class ChunkStream
	{
		private int chunkRead;

		private List<ChunkStream.Chunk> chunks;

		private int chunkSize;

		private bool gotit;

		private StringBuilder saved;

		private bool sawCR;

		private ChunkStream.State state;

		private int trailerState;

		internal WebHeaderCollection headers;

		public ChunkStream(byte[] buffer, int offset, int size, WebHeaderCollection headers) : this(headers)
		{
			this.Write(buffer, offset, size);
		}

		public ChunkStream(WebHeaderCollection headers)
		{
			this.headers = headers;
			this.saved = new StringBuilder();
			this.chunks = new List<ChunkStream.Chunk>();
			this.chunkSize = -1;
		}

		public int ChunkLeft
		{
			get
			{
				return this.chunkSize - this.chunkRead;
			}
		}

		public bool WantMore
		{
			get
			{
				return this.chunkRead != this.chunkSize || this.chunkSize != 0 || this.state != ChunkStream.State.None;
			}
		}

		private ChunkStream.State GetChunkSize(byte[] buffer, ref int offset, int size)
		{
			char c = '\0';
			while (offset < size)
			{
				c = (char)buffer[offset++];
				if (c == '\r')
				{
					if (this.sawCR)
					{
						ChunkStream.ThrowProtocolViolation("2 CR found.");
					}
					this.sawCR = true;
				}
				else
				{
					if (this.sawCR && c == '\n')
					{
						break;
					}
					if (c == ' ')
					{
						this.gotit = true;
					}
					if (!this.gotit)
					{
						this.saved.Append(c);
					}
					if (this.saved.Length > 20)
					{
						ChunkStream.ThrowProtocolViolation("Chunk size too long.");
					}
				}
			}
			if (!this.sawCR || c != '\n')
			{
				if (offset < size)
				{
					ChunkStream.ThrowProtocolViolation("Missing \\n.");
				}
				try
				{
					if (this.saved.Length > 0)
					{
						this.chunkSize = int.Parse(ChunkStream.RemoveChunkExtension(this.saved.ToString()), NumberStyles.HexNumber);
					}
				}
				catch (Exception)
				{
					ChunkStream.ThrowProtocolViolation("Cannot parse chunk size.");
				}
				return ChunkStream.State.None;
			}
			this.chunkRead = 0;
			try
			{
				this.chunkSize = int.Parse(ChunkStream.RemoveChunkExtension(this.saved.ToString()), NumberStyles.HexNumber);
			}
			catch (Exception)
			{
				ChunkStream.ThrowProtocolViolation("Cannot parse chunk size.");
			}
			if (this.chunkSize == 0)
			{
				this.trailerState = 2;
				return ChunkStream.State.Trailer;
			}
			return ChunkStream.State.Body;
		}

		private void InternalWrite(byte[] buffer, ref int offset, int size)
		{
			if (this.state == ChunkStream.State.None)
			{
				this.state = this.GetChunkSize(buffer, ref offset, size);
				if (this.state == ChunkStream.State.None)
				{
					return;
				}
				this.saved.Length = 0;
				this.sawCR = false;
				this.gotit = false;
			}
			if (this.state == ChunkStream.State.Body && offset < size)
			{
				this.state = this.ReadBody(buffer, ref offset, size);
				if (this.state == ChunkStream.State.Body)
				{
					return;
				}
			}
			if (this.state == ChunkStream.State.BodyFinished && offset < size)
			{
				this.state = this.ReadCRLF(buffer, ref offset, size);
				if (this.state == ChunkStream.State.BodyFinished)
				{
					return;
				}
				this.sawCR = false;
			}
			if (this.state == ChunkStream.State.Trailer && offset < size)
			{
				this.state = this.ReadTrailer(buffer, ref offset, size);
				if (this.state == ChunkStream.State.Trailer)
				{
					return;
				}
				this.saved.Length = 0;
				this.sawCR = false;
				this.gotit = false;
			}
			if (offset < size)
			{
				this.InternalWrite(buffer, ref offset, size);
			}
		}

		private ChunkStream.State ReadBody(byte[] buffer, ref int offset, int size)
		{
			if (this.chunkSize == 0)
			{
				return ChunkStream.State.BodyFinished;
			}
			int num = size - offset;
			if (num + this.chunkRead > this.chunkSize)
			{
				num = this.chunkSize - this.chunkRead;
			}
			byte[] array = new byte[num];
			Buffer.BlockCopy(buffer, offset, array, 0, num);
			this.chunks.Add(new ChunkStream.Chunk(array));
			offset += num;
			this.chunkRead += num;
			return (this.chunkRead != this.chunkSize) ? ChunkStream.State.Body : ChunkStream.State.BodyFinished;
		}

		private ChunkStream.State ReadCRLF(byte[] buffer, ref int offset, int size)
		{
			if (!this.sawCR)
			{
				if ((ushort)buffer[offset++] != 13)
				{
					ChunkStream.ThrowProtocolViolation("Expecting \\r.");
				}
				this.sawCR = true;
				if (offset == size)
				{
					return ChunkStream.State.BodyFinished;
				}
			}
			if (this.sawCR && (ushort)buffer[offset++] != 10)
			{
				ChunkStream.ThrowProtocolViolation("Expecting \\n.");
			}
			return ChunkStream.State.None;
		}

		private int ReadFromChunks(byte[] buffer, int offset, int size)
		{
			int count = this.chunks.Count;
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				ChunkStream.Chunk chunk = this.chunks[i];
				if (chunk != null)
				{
					if (chunk.Offset == chunk.Bytes.Length)
					{
						this.chunks[i] = null;
					}
					else
					{
						num += chunk.Read(buffer, offset + num, size - num);
						if (num == size)
						{
							break;
						}
					}
				}
			}
			return num;
		}

		private ChunkStream.State ReadTrailer(byte[] buffer, ref int offset, int size)
		{
			if (this.trailerState == 2 && (ushort)buffer[offset] == 13 && this.saved.Length == 0)
			{
				offset++;
				if (offset < size && (ushort)buffer[offset] == 10)
				{
					offset++;
					return ChunkStream.State.None;
				}
				offset--;
			}
			int num = this.trailerState;
			string text = "\r\n\r";
			while (offset < size && num < 4)
			{
				char c = (char)buffer[offset++];
				if ((num == 0 || num == 2) && c == '\r')
				{
					num++;
				}
				else if ((num == 1 || num == 3) && c == '\n')
				{
					num++;
				}
				else if (num > 0)
				{
					this.saved.Append(text.Substring(0, (this.saved.Length != 0) ? num : (num - 2)));
					num = 0;
					if (this.saved.Length > 4196)
					{
						ChunkStream.ThrowProtocolViolation("Error reading trailer (too long).");
					}
				}
			}
			if (num < 4)
			{
				this.trailerState = num;
				if (offset < size)
				{
					ChunkStream.ThrowProtocolViolation("Error reading trailer.");
				}
				return ChunkStream.State.Trailer;
			}
			StringReader stringReader = new StringReader(this.saved.ToString());
			string text2;
			while ((text2 = stringReader.ReadLine()) != null && text2 != "")
			{
				this.headers.Add(text2);
			}
			return ChunkStream.State.None;
		}

		private static string RemoveChunkExtension(string input)
		{
			int num = input.IndexOf(';');
			if (num == -1)
			{
				return input;
			}
			return input.Substring(0, num);
		}

		private static void ThrowProtocolViolation(string message)
		{
			WebException ex = new WebException(message, null, WebExceptionStatus.ServerProtocolViolation, null);
			throw ex;
		}

		public int Read(byte[] buffer, int offset, int size)
		{
			return this.ReadFromChunks(buffer, offset, size);
		}

		public void ResetBuffer()
		{
			this.chunkSize = -1;
			this.chunkRead = 0;
			this.chunks.Clear();
		}

		public void Write(byte[] buffer, int offset, int size)
		{
			this.InternalWrite(buffer, ref offset, size);
		}

		public void WriteAndReadBack(byte[] buffer, int offset, int size, ref int read)
		{
			if (offset + read > 0)
			{
				this.Write(buffer, offset, offset + read);
			}
			read = this.Read(buffer, offset, size);
		}

		private enum State
		{
			None,
			Body,
			BodyFinished,
			Trailer
		}

		private class Chunk
		{
			public byte[] Bytes;

			public int Offset;

			public Chunk(byte[] chunk)
			{
				this.Bytes = chunk;
			}

			public int Read(byte[] buffer, int offset, int size)
			{
				int num = (size <= this.Bytes.Length - this.Offset) ? size : (this.Bytes.Length - this.Offset);
				Buffer.BlockCopy(this.Bytes, this.Offset, buffer, offset, num);
				this.Offset += num;
				return num;
			}
		}
	}
}
