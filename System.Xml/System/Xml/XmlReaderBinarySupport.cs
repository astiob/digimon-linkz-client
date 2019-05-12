using System;
using System.Text;

namespace System.Xml
{
	internal class XmlReaderBinarySupport
	{
		private XmlReader reader;

		private XmlReaderBinarySupport.CharGetter getter;

		private byte[] base64Cache = new byte[3];

		private int base64CacheStartsAt;

		private XmlReaderBinarySupport.CommandState state;

		private StringBuilder textCache;

		private bool hasCache;

		private bool dontReset;

		public XmlReaderBinarySupport(XmlReader reader)
		{
			this.reader = reader;
			this.Reset();
		}

		public XmlReaderBinarySupport.CharGetter Getter
		{
			get
			{
				return this.getter;
			}
			set
			{
				this.getter = value;
			}
		}

		public void Reset()
		{
			if (!this.dontReset)
			{
				this.dontReset = true;
				if (this.hasCache)
				{
					XmlNodeType nodeType = this.reader.NodeType;
					if (nodeType == XmlNodeType.Text || nodeType == XmlNodeType.CDATA || nodeType == XmlNodeType.Whitespace || nodeType == XmlNodeType.SignificantWhitespace)
					{
						this.reader.Read();
					}
					switch (this.state)
					{
					case XmlReaderBinarySupport.CommandState.ReadElementContentAsBase64:
					case XmlReaderBinarySupport.CommandState.ReadElementContentAsBinHex:
						this.reader.Read();
						break;
					}
				}
				this.base64CacheStartsAt = -1;
				this.state = XmlReaderBinarySupport.CommandState.None;
				this.hasCache = false;
				this.dontReset = false;
			}
		}

		private InvalidOperationException StateError(XmlReaderBinarySupport.CommandState action)
		{
			return new InvalidOperationException(string.Format("Invalid attempt to read binary content by {0}, while once binary reading was started by {1}", action, this.state));
		}

		private void CheckState(bool element, XmlReaderBinarySupport.CommandState action)
		{
			if (this.state == XmlReaderBinarySupport.CommandState.None)
			{
				if (this.textCache == null)
				{
					this.textCache = new StringBuilder();
				}
				else
				{
					this.textCache.Length = 0;
				}
				if (action == XmlReaderBinarySupport.CommandState.None)
				{
					return;
				}
				if (this.reader.ReadState != ReadState.Interactive)
				{
					return;
				}
				XmlNodeType nodeType = this.reader.NodeType;
				switch (nodeType)
				{
				case XmlNodeType.Element:
					if (element)
					{
						if (!this.reader.IsEmptyElement)
						{
							this.reader.Read();
						}
						this.state = action;
						return;
					}
					goto IL_C6;
				default:
					if (nodeType != XmlNodeType.Whitespace && nodeType != XmlNodeType.SignificantWhitespace)
					{
						goto IL_C6;
					}
					break;
				case XmlNodeType.Text:
				case XmlNodeType.CDATA:
					break;
				}
				if (!element)
				{
					this.state = action;
					return;
				}
				IL_C6:
				throw new XmlException((!element) ? "Reader is not positioned on a text node." : "Reader is not positioned on an element.");
			}
			else
			{
				if (this.state == action)
				{
					return;
				}
				throw this.StateError(action);
			}
		}

		public int ReadElementContentAsBase64(byte[] buffer, int offset, int length)
		{
			this.CheckState(true, XmlReaderBinarySupport.CommandState.ReadElementContentAsBase64);
			return this.ReadBase64(buffer, offset, length);
		}

		public int ReadContentAsBase64(byte[] buffer, int offset, int length)
		{
			this.CheckState(false, XmlReaderBinarySupport.CommandState.ReadContentAsBase64);
			return this.ReadBase64(buffer, offset, length);
		}

		public int ReadElementContentAsBinHex(byte[] buffer, int offset, int length)
		{
			this.CheckState(true, XmlReaderBinarySupport.CommandState.ReadElementContentAsBinHex);
			return this.ReadBinHex(buffer, offset, length);
		}

		public int ReadContentAsBinHex(byte[] buffer, int offset, int length)
		{
			this.CheckState(false, XmlReaderBinarySupport.CommandState.ReadContentAsBinHex);
			return this.ReadBinHex(buffer, offset, length);
		}

		public int ReadBase64(byte[] buffer, int offset, int length)
		{
			if (offset < 0)
			{
				throw XmlReaderBinarySupport.CreateArgumentOutOfRangeException("offset", offset, "Offset must be non-negative integer.");
			}
			if (length < 0)
			{
				throw XmlReaderBinarySupport.CreateArgumentOutOfRangeException("length", length, "Length must be non-negative integer.");
			}
			if (buffer.Length < offset + length)
			{
				throw new ArgumentOutOfRangeException("buffer length is smaller than the sum of offset and length.");
			}
			if (this.reader.IsEmptyElement)
			{
				return 0;
			}
			if (length == 0)
			{
				return 0;
			}
			int num = offset;
			int num2 = offset + length;
			if (this.base64CacheStartsAt >= 0)
			{
				for (int i = this.base64CacheStartsAt; i < 3; i++)
				{
					buffer[num++] = this.base64Cache[this.base64CacheStartsAt++];
					if (num == num2)
					{
						return num2 - offset;
					}
				}
			}
			for (int j = 0; j < 3; j++)
			{
				this.base64Cache[j] = 0;
			}
			this.base64CacheStartsAt = -1;
			int num3 = (int)Math.Ceiling(1.3333333333333333 * (double)length);
			int num4 = num3 % 4;
			if (num4 > 0)
			{
				num3 += 4 - num4;
			}
			char[] array = new char[num3];
			int num5 = (this.getter == null) ? this.ReadValueChunk(array, 0, num3) : this.getter(array, 0, num3);
			for (int k = 0; k < num5 - 3; k++)
			{
				if ((k = this.SkipIgnorableBase64Chars(array, num5, k)) == num5)
				{
					break;
				}
				byte b = (byte)(this.GetBase64Byte(array[k]) << 2);
				if (num < num2)
				{
					buffer[num] = b;
				}
				else
				{
					if (this.base64CacheStartsAt < 0)
					{
						this.base64CacheStartsAt = 0;
					}
					this.base64Cache[0] = b;
				}
				if (++k == num5)
				{
					break;
				}
				if ((k = this.SkipIgnorableBase64Chars(array, num5, k)) == num5)
				{
					break;
				}
				b = this.GetBase64Byte(array[k]);
				byte b2 = (byte)(b >> 4);
				if (num < num2)
				{
					int num6 = num;
					buffer[num6] += b2;
					num++;
				}
				else
				{
					byte[] array2 = this.base64Cache;
					int num7 = 0;
					array2[num7] += b2;
				}
				b2 = (byte)((b & 15) << 4);
				if (num < num2)
				{
					buffer[num] = b2;
				}
				else
				{
					if (this.base64CacheStartsAt < 0)
					{
						this.base64CacheStartsAt = 1;
					}
					this.base64Cache[1] = b2;
				}
				if (++k == num5)
				{
					break;
				}
				if ((k = this.SkipIgnorableBase64Chars(array, num5, k)) == num5)
				{
					break;
				}
				b = this.GetBase64Byte(array[k]);
				b2 = (byte)(b >> 2);
				if (num < num2)
				{
					int num8 = num;
					buffer[num8] += b2;
					num++;
				}
				else
				{
					byte[] array3 = this.base64Cache;
					int num9 = 1;
					array3[num9] += b2;
				}
				b2 = (byte)((b & 3) << 6);
				if (num < num2)
				{
					buffer[num] = b2;
				}
				else
				{
					if (this.base64CacheStartsAt < 0)
					{
						this.base64CacheStartsAt = 2;
					}
					this.base64Cache[2] = b2;
				}
				if (++k == num5)
				{
					break;
				}
				if ((k = this.SkipIgnorableBase64Chars(array, num5, k)) == num5)
				{
					break;
				}
				b2 = this.GetBase64Byte(array[k]);
				if (num < num2)
				{
					int num10 = num;
					buffer[num10] += b2;
					num++;
				}
				else
				{
					byte[] array4 = this.base64Cache;
					int num11 = 2;
					array4[num11] += b2;
				}
			}
			int num12 = Math.Min(num2 - offset, num - offset);
			if (num12 < length && num5 > 0)
			{
				return num12 + this.ReadBase64(buffer, offset + num12, length - num12);
			}
			return num12;
		}

		private byte GetBase64Byte(char ch)
		{
			if (ch == '+')
			{
				return 62;
			}
			if (ch == '/')
			{
				return 63;
			}
			if (ch >= 'A' && ch <= 'Z')
			{
				return (byte)(ch - 'A');
			}
			if (ch >= 'a' && ch <= 'z')
			{
				return (byte)(ch - 'a' + '\u001a');
			}
			if (ch >= '0' && ch <= '9')
			{
				return (byte)(ch - '0' + '4');
			}
			throw new XmlException("Invalid Base64 character was found.");
		}

		private int SkipIgnorableBase64Chars(char[] chars, int charsLength, int i)
		{
			while (chars[i] == '=' || XmlChar.IsWhitespace((int)chars[i]))
			{
				if (charsLength == ++i)
				{
					break;
				}
			}
			return i;
		}

		private static Exception CreateArgumentOutOfRangeException(string name, object value, string message)
		{
			return new ArgumentOutOfRangeException(message);
		}

		public int ReadBinHex(byte[] buffer, int offset, int length)
		{
			if (offset < 0)
			{
				throw XmlReaderBinarySupport.CreateArgumentOutOfRangeException("offset", offset, "Offset must be non-negative integer.");
			}
			if (length < 0)
			{
				throw XmlReaderBinarySupport.CreateArgumentOutOfRangeException("length", length, "Length must be non-negative integer.");
			}
			if (buffer.Length < offset + length)
			{
				throw new ArgumentOutOfRangeException("buffer length is smaller than the sum of offset and length.");
			}
			if (length == 0)
			{
				return 0;
			}
			char[] array = new char[length * 2];
			int charLength = (this.getter == null) ? this.ReadValueChunk(array, 0, length * 2) : this.getter(array, 0, length * 2);
			return XmlConvert.FromBinHexString(array, offset, charLength, buffer);
		}

		public int ReadValueChunk(char[] buffer, int offset, int length)
		{
			XmlReaderBinarySupport.CommandState commandState = this.state;
			if (this.state == XmlReaderBinarySupport.CommandState.None)
			{
				this.CheckState(false, XmlReaderBinarySupport.CommandState.None);
			}
			if (offset < 0)
			{
				throw XmlReaderBinarySupport.CreateArgumentOutOfRangeException("offset", offset, "Offset must be non-negative integer.");
			}
			if (length < 0)
			{
				throw XmlReaderBinarySupport.CreateArgumentOutOfRangeException("length", length, "Length must be non-negative integer.");
			}
			if (buffer.Length < offset + length)
			{
				throw new ArgumentOutOfRangeException("buffer length is smaller than the sum of offset and length.");
			}
			if (length == 0)
			{
				return 0;
			}
			if (!this.hasCache && this.reader.IsEmptyElement)
			{
				return 0;
			}
			bool flag = true;
			while (flag && this.textCache.Length < length)
			{
				XmlNodeType nodeType = this.reader.NodeType;
				if (nodeType != XmlNodeType.Text && nodeType != XmlNodeType.CDATA && nodeType != XmlNodeType.Whitespace && nodeType != XmlNodeType.SignificantWhitespace)
				{
					flag = false;
				}
				else
				{
					if (this.hasCache)
					{
						XmlNodeType nodeType2 = this.reader.NodeType;
						if (nodeType2 != XmlNodeType.Text && nodeType2 != XmlNodeType.CDATA && nodeType2 != XmlNodeType.Whitespace && nodeType2 != XmlNodeType.SignificantWhitespace)
						{
							flag = false;
						}
						else
						{
							this.Read();
						}
					}
					this.textCache.Append(this.reader.Value);
					this.hasCache = true;
				}
			}
			this.state = commandState;
			int num = this.textCache.Length;
			if (num > length)
			{
				num = length;
			}
			string text = this.textCache.ToString(0, num);
			this.textCache.Remove(0, text.Length);
			text.CopyTo(0, buffer, offset, text.Length);
			if (num < length && flag)
			{
				return num + this.ReadValueChunk(buffer, offset + num, length - num);
			}
			return num;
		}

		private bool Read()
		{
			this.dontReset = true;
			bool result = this.reader.Read();
			this.dontReset = false;
			return result;
		}

		public enum CommandState
		{
			None,
			ReadElementContentAsBase64,
			ReadContentAsBase64,
			ReadElementContentAsBinHex,
			ReadContentAsBinHex
		}

		public delegate int CharGetter(char[] buffer, int offset, int length);
	}
}
