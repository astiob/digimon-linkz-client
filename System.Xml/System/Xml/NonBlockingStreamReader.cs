using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Xml
{
	internal class NonBlockingStreamReader : TextReader
	{
		private const int DefaultBufferSize = 1024;

		private const int DefaultFileBufferSize = 4096;

		private const int MinimumBufferSize = 128;

		private byte[] input_buffer;

		private char[] decoded_buffer;

		private int decoded_count;

		private int pos;

		private int buffer_size;

		private Encoding encoding;

		private Decoder decoder;

		private Stream base_stream;

		private bool mayBlock;

		private StringBuilder line_builder;

		private bool foundCR;

		public NonBlockingStreamReader(Stream stream, Encoding encoding)
		{
			int num = 1024;
			this.base_stream = stream;
			this.input_buffer = new byte[num];
			this.buffer_size = num;
			this.encoding = encoding;
			this.decoder = encoding.GetDecoder();
			this.decoded_buffer = new char[encoding.GetMaxCharCount(num)];
			this.decoded_count = 0;
			this.pos = 0;
		}

		public Encoding Encoding
		{
			get
			{
				return this.encoding;
			}
		}

		public override void Close()
		{
			this.Dispose(true);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.base_stream != null)
			{
				this.base_stream.Close();
			}
			this.input_buffer = null;
			this.decoded_buffer = null;
			this.encoding = null;
			this.decoder = null;
			this.base_stream = null;
			base.Dispose(disposing);
		}

		public void DiscardBufferedData()
		{
			this.pos = (this.decoded_count = 0);
			this.mayBlock = false;
			this.decoder.Reset();
		}

		private int ReadBuffer()
		{
			this.pos = 0;
			this.decoded_count = 0;
			int byteIndex = 0;
			for (;;)
			{
				int num = this.base_stream.Read(this.input_buffer, 0, this.buffer_size);
				if (num == 0)
				{
					break;
				}
				this.mayBlock = (num < this.buffer_size);
				this.decoded_count += this.decoder.GetChars(this.input_buffer, byteIndex, num, this.decoded_buffer, 0);
				byteIndex = 0;
				if (this.decoded_count != 0)
				{
					goto Block_2;
				}
			}
			return 0;
			Block_2:
			return this.decoded_count;
		}

		public override int Peek()
		{
			if (this.base_stream == null)
			{
				throw new ObjectDisposedException("StreamReader", "Cannot read from a closed StreamReader");
			}
			if (this.pos >= this.decoded_count && (this.mayBlock || this.ReadBuffer() == 0))
			{
				return -1;
			}
			return (int)this.decoded_buffer[this.pos];
		}

		public override int Read()
		{
			if (this.base_stream == null)
			{
				throw new ObjectDisposedException("StreamReader", "Cannot read from a closed StreamReader");
			}
			if (this.pos >= this.decoded_count && this.ReadBuffer() == 0)
			{
				return -1;
			}
			return (int)this.decoded_buffer[this.pos++];
		}

		public override int Read([In] [Out] char[] dest_buffer, int index, int count)
		{
			if (this.base_stream == null)
			{
				throw new ObjectDisposedException("StreamReader", "Cannot read from a closed StreamReader");
			}
			if (dest_buffer == null)
			{
				throw new ArgumentNullException("dest_buffer");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "< 0");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "< 0");
			}
			if (index > dest_buffer.Length - count)
			{
				throw new ArgumentException("index + count > dest_buffer.Length");
			}
			int num = 0;
			if (this.pos >= this.decoded_count && this.ReadBuffer() == 0)
			{
				return (num <= 0) ? 0 : num;
			}
			int num2 = Math.Min(this.decoded_count - this.pos, count);
			Array.Copy(this.decoded_buffer, this.pos, dest_buffer, index, num2);
			this.pos += num2;
			index += num2;
			count -= num2;
			return num + num2;
		}

		private int FindNextEOL()
		{
			while (this.pos < this.decoded_count)
			{
				char c = this.decoded_buffer[this.pos];
				if (c == '\n')
				{
					this.pos++;
					int num = (!this.foundCR) ? (this.pos - 1) : (this.pos - 2);
					if (num < 0)
					{
						num = 0;
					}
					this.foundCR = false;
					return num;
				}
				if (this.foundCR)
				{
					this.foundCR = false;
					return this.pos - 1;
				}
				this.foundCR = (c == '\r');
				this.pos++;
			}
			return -1;
		}

		public override string ReadLine()
		{
			if (this.base_stream == null)
			{
				throw new ObjectDisposedException("StreamReader", "Cannot read from a closed StreamReader");
			}
			if (this.pos >= this.decoded_count && this.ReadBuffer() == 0)
			{
				return null;
			}
			int num = this.pos;
			int num2 = this.FindNextEOL();
			if (num2 < this.decoded_count && num2 >= num)
			{
				return new string(this.decoded_buffer, num, num2 - num);
			}
			if (this.line_builder == null)
			{
				this.line_builder = new StringBuilder();
			}
			else
			{
				this.line_builder.Length = 0;
			}
			for (;;)
			{
				if (this.foundCR)
				{
					this.decoded_count--;
				}
				this.line_builder.Append(new string(this.decoded_buffer, num, this.decoded_count - num));
				if (this.ReadBuffer() == 0)
				{
					break;
				}
				num = this.pos;
				num2 = this.FindNextEOL();
				if (num2 < this.decoded_count && num2 >= num)
				{
					goto Block_11;
				}
			}
			if (this.line_builder.Capacity > 32768)
			{
				StringBuilder stringBuilder = this.line_builder;
				this.line_builder = null;
				return stringBuilder.ToString(0, stringBuilder.Length);
			}
			return this.line_builder.ToString(0, this.line_builder.Length);
			Block_11:
			this.line_builder.Append(new string(this.decoded_buffer, num, num2 - num));
			if (this.line_builder.Capacity > 32768)
			{
				StringBuilder stringBuilder2 = this.line_builder;
				this.line_builder = null;
				return stringBuilder2.ToString(0, stringBuilder2.Length);
			}
			return this.line_builder.ToString(0, this.line_builder.Length);
		}

		public override string ReadToEnd()
		{
			if (this.base_stream == null)
			{
				throw new ObjectDisposedException("StreamReader", "Cannot read from a closed StreamReader");
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = this.decoded_buffer.Length;
			char[] array = new char[num];
			int charCount;
			while ((charCount = this.Read(array, 0, num)) != 0)
			{
				stringBuilder.Append(array, 0, charCount);
			}
			return stringBuilder.ToString();
		}
	}
}
