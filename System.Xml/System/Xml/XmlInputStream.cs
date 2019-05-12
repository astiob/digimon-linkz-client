using System;
using System.IO;
using System.Text;

namespace System.Xml
{
	internal class XmlInputStream : Stream
	{
		public static readonly Encoding StrictUTF8;

		private Encoding enc;

		private Stream stream;

		private byte[] buffer;

		private int bufLength;

		private int bufPos;

		private static XmlException encodingException = new XmlException("invalid encoding specification.");

		public XmlInputStream(Stream stream)
		{
			this.Initialize(stream);
		}

		static XmlInputStream()
		{
			XmlInputStream.StrictUTF8 = new UTF8Encoding(false, true);
		}

		private static string GetStringFromBytes(byte[] bytes, int index, int count)
		{
			return Encoding.ASCII.GetString(bytes, index, count);
		}

		private void Initialize(Stream stream)
		{
			this.buffer = new byte[64];
			this.stream = stream;
			this.enc = XmlInputStream.StrictUTF8;
			this.bufLength = stream.Read(this.buffer, 0, this.buffer.Length);
			if (this.bufLength == -1 || this.bufLength == 0)
			{
				return;
			}
			int i = this.ReadByteSpecial();
			int num = i;
			if (num != 254)
			{
				if (num != 255)
				{
					if (num != 60)
					{
						if (num != 239)
						{
							this.bufPos = 0;
						}
						else
						{
							i = this.ReadByteSpecial();
							if (i == 187)
							{
								i = this.ReadByteSpecial();
								if (i != 191)
								{
									this.bufPos = 0;
								}
							}
							else
							{
								this.buffer[--this.bufPos] = 239;
							}
						}
					}
					else
					{
						if (this.bufLength >= 5 && XmlInputStream.GetStringFromBytes(this.buffer, 1, 4) == "?xml")
						{
							this.bufPos += 4;
							i = this.SkipWhitespace();
							if (i == 118)
							{
								while (i >= 0)
								{
									i = this.ReadByteSpecial();
									if (i == 48)
									{
										this.ReadByteSpecial();
										break;
									}
								}
								i = this.SkipWhitespace();
							}
							if (i == 101)
							{
								int num2 = this.bufLength - this.bufPos;
								if (num2 >= 7 && XmlInputStream.GetStringFromBytes(this.buffer, this.bufPos, 7) == "ncoding")
								{
									this.bufPos += 7;
									i = this.SkipWhitespace();
									if (i != 61)
									{
										throw XmlInputStream.encodingException;
									}
									i = this.SkipWhitespace();
									int num3 = i;
									StringBuilder stringBuilder = new StringBuilder();
									for (;;)
									{
										i = this.ReadByteSpecial();
										if (i == num3)
										{
											break;
										}
										if (i < 0)
										{
											goto Block_19;
										}
										stringBuilder.Append((char)i);
									}
									string text = stringBuilder.ToString();
									if (!XmlChar.IsValidIANAEncoding(text))
									{
										throw XmlInputStream.encodingException;
									}
									this.enc = Encoding.GetEncoding(text);
									goto IL_272;
									Block_19:
									throw XmlInputStream.encodingException;
								}
							}
						}
						IL_272:
						this.bufPos = 0;
					}
				}
				else
				{
					i = this.ReadByteSpecial();
					if (i == 254)
					{
						this.enc = Encoding.Unicode;
					}
					else
					{
						this.bufPos = 0;
					}
				}
			}
			else
			{
				i = this.ReadByteSpecial();
				if (i == 255)
				{
					this.enc = Encoding.BigEndianUnicode;
					return;
				}
				this.bufPos = 0;
			}
		}

		private int ReadByteSpecial()
		{
			if (this.bufLength > this.bufPos)
			{
				return (int)this.buffer[this.bufPos++];
			}
			byte[] dst = new byte[this.buffer.Length * 2];
			Buffer.BlockCopy(this.buffer, 0, dst, 0, this.bufLength);
			int num = this.stream.Read(dst, this.bufLength, this.buffer.Length);
			if (num == -1 || num == 0)
			{
				return -1;
			}
			this.bufLength += num;
			this.buffer = dst;
			return (int)this.buffer[this.bufPos++];
		}

		private int SkipWhitespace()
		{
			int num;
			for (;;)
			{
				num = this.ReadByteSpecial();
				char c = (char)num;
				switch (c)
				{
				case '\t':
					break;
				case '\n':
					break;
				default:
					if (c != ' ')
					{
						return num;
					}
					break;
				case '\r':
					break;
				}
			}
			return num;
		}

		public Encoding ActualEncoding
		{
			get
			{
				return this.enc;
			}
		}

		public override bool CanRead
		{
			get
			{
				return this.bufLength > this.bufPos || this.stream.CanRead;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		public override long Length
		{
			get
			{
				return this.stream.Length;
			}
		}

		public override long Position
		{
			get
			{
				return this.stream.Position - (long)this.bufLength + (long)this.bufPos;
			}
			set
			{
				if (value < (long)this.bufLength)
				{
					this.bufPos = (int)value;
				}
				else
				{
					this.stream.Position = value - (long)this.bufLength;
				}
			}
		}

		public override void Close()
		{
			this.stream.Close();
		}

		public override void Flush()
		{
			this.stream.Flush();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			int result;
			if (count <= this.bufLength - this.bufPos)
			{
				Buffer.BlockCopy(this.buffer, this.bufPos, buffer, offset, count);
				this.bufPos += count;
				result = count;
			}
			else
			{
				int num = this.bufLength - this.bufPos;
				if (this.bufLength > this.bufPos)
				{
					Buffer.BlockCopy(this.buffer, this.bufPos, buffer, offset, num);
					this.bufPos += num;
				}
				result = num + this.stream.Read(buffer, offset + num, count - num);
			}
			return result;
		}

		public override int ReadByte()
		{
			if (this.bufLength > this.bufPos)
			{
				return (int)this.buffer[this.bufPos++];
			}
			return this.stream.ReadByte();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			int num = this.bufLength - this.bufPos;
			if (origin != SeekOrigin.Current)
			{
				return this.stream.Seek(offset, origin);
			}
			if (offset < (long)num)
			{
				return (long)this.buffer[(int)(checked((IntPtr)(unchecked((long)this.bufPos + offset))))];
			}
			return this.stream.Seek(offset - (long)num, origin);
		}

		public override void SetLength(long value)
		{
			this.stream.SetLength(value);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}
	}
}
