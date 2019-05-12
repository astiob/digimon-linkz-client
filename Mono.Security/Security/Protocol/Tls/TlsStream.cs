using System;
using System.IO;

namespace Mono.Security.Protocol.Tls
{
	internal class TlsStream : Stream
	{
		private const int temp_size = 4;

		private bool canRead;

		private bool canWrite;

		private MemoryStream buffer;

		private byte[] temp;

		public TlsStream()
		{
			this.buffer = new MemoryStream(0);
			this.canRead = false;
			this.canWrite = true;
		}

		public TlsStream(byte[] data)
		{
			if (data != null)
			{
				this.buffer = new MemoryStream(data);
			}
			else
			{
				this.buffer = new MemoryStream();
			}
			this.canRead = true;
			this.canWrite = false;
		}

		public bool EOF
		{
			get
			{
				return this.Position >= this.Length;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return this.canWrite;
			}
		}

		public override bool CanRead
		{
			get
			{
				return this.canRead;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return this.buffer.CanSeek;
			}
		}

		public override long Position
		{
			get
			{
				return this.buffer.Position;
			}
			set
			{
				this.buffer.Position = value;
			}
		}

		public override long Length
		{
			get
			{
				return this.buffer.Length;
			}
		}

		private byte[] ReadSmallValue(int length)
		{
			if (length > 4)
			{
				throw new ArgumentException("8 bytes maximum");
			}
			if (this.temp == null)
			{
				this.temp = new byte[4];
			}
			if (this.Read(this.temp, 0, length) != length)
			{
				throw new TlsException(string.Format("buffer underrun", new object[0]));
			}
			return this.temp;
		}

		public new byte ReadByte()
		{
			byte[] array = this.ReadSmallValue(1);
			return array[0];
		}

		public short ReadInt16()
		{
			byte[] array = this.ReadSmallValue(2);
			return (short)((int)array[0] << 8 | (int)array[1]);
		}

		public int ReadInt24()
		{
			byte[] array = this.ReadSmallValue(3);
			return (int)array[0] << 16 | (int)array[1] << 8 | (int)array[2];
		}

		public int ReadInt32()
		{
			byte[] array = this.ReadSmallValue(4);
			return (int)array[0] << 24 | (int)array[1] << 16 | (int)array[2] << 8 | (int)array[3];
		}

		public byte[] ReadBytes(int count)
		{
			byte[] result = new byte[count];
			if (this.Read(result, 0, count) != count)
			{
				throw new TlsException("buffer underrun");
			}
			return result;
		}

		public void Write(byte value)
		{
			if (this.temp == null)
			{
				this.temp = new byte[4];
			}
			this.temp[0] = value;
			this.Write(this.temp, 0, 1);
		}

		public void Write(short value)
		{
			if (this.temp == null)
			{
				this.temp = new byte[4];
			}
			this.temp[0] = (byte)(value >> 8);
			this.temp[1] = (byte)value;
			this.Write(this.temp, 0, 2);
		}

		public void WriteInt24(int value)
		{
			if (this.temp == null)
			{
				this.temp = new byte[4];
			}
			this.temp[0] = (byte)(value >> 16);
			this.temp[1] = (byte)(value >> 8);
			this.temp[2] = (byte)value;
			this.Write(this.temp, 0, 3);
		}

		public void Write(int value)
		{
			if (this.temp == null)
			{
				this.temp = new byte[4];
			}
			this.temp[0] = (byte)(value >> 24);
			this.temp[1] = (byte)(value >> 16);
			this.temp[2] = (byte)(value >> 8);
			this.temp[3] = (byte)value;
			this.Write(this.temp, 0, 4);
		}

		public void Write(ulong value)
		{
			this.Write((int)(value >> 32));
			this.Write((int)value);
		}

		public void Write(byte[] buffer)
		{
			this.Write(buffer, 0, buffer.Length);
		}

		public void Reset()
		{
			this.buffer.SetLength(0L);
			this.buffer.Position = 0L;
		}

		public byte[] ToArray()
		{
			return this.buffer.ToArray();
		}

		public override void Flush()
		{
			this.buffer.Flush();
		}

		public override void SetLength(long length)
		{
			this.buffer.SetLength(length);
		}

		public override long Seek(long offset, SeekOrigin loc)
		{
			return this.buffer.Seek(offset, loc);
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this.canRead)
			{
				return this.buffer.Read(buffer, offset, count);
			}
			throw new InvalidOperationException("Read operations are not allowed by this stream");
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.canWrite)
			{
				this.buffer.Write(buffer, offset, count);
				return;
			}
			throw new InvalidOperationException("Write operations are not allowed by this stream");
		}
	}
}
