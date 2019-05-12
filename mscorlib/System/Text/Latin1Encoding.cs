using System;
using System.Runtime.CompilerServices;

namespace System.Text
{
	[Serializable]
	internal class Latin1Encoding : Encoding
	{
		internal const int ISOLATIN_CODE_PAGE = 28591;

		public Latin1Encoding() : base(28591)
		{
		}

		public override bool IsSingleByte
		{
			get
			{
				return true;
			}
		}

		public override bool IsAlwaysNormalized(NormalizationForm form)
		{
			return form == NormalizationForm.FormC;
		}

		public override int GetByteCount(char[] chars, int index, int count)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars");
			}
			if (index < 0 || index > chars.Length)
			{
				throw new ArgumentOutOfRangeException("index", Encoding._("ArgRange_Array"));
			}
			if (count < 0 || count > chars.Length - index)
			{
				throw new ArgumentOutOfRangeException("count", Encoding._("ArgRange_Array"));
			}
			return count;
		}

		public override int GetByteCount(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			return s.Length;
		}

		public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			EncoderFallbackBuffer encoderFallbackBuffer = null;
			char[] array = null;
			return this.GetBytes(chars, charIndex, charCount, bytes, byteIndex, ref encoderFallbackBuffer, ref array);
		}

		private int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex, ref EncoderFallbackBuffer buffer, ref char[] fallback_chars)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars");
			}
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (charIndex < 0 || charIndex > chars.Length)
			{
				throw new ArgumentOutOfRangeException("charIndex", Encoding._("ArgRange_Array"));
			}
			if (charCount < 0 || charCount > chars.Length - charIndex)
			{
				throw new ArgumentOutOfRangeException("charCount", Encoding._("ArgRange_Array"));
			}
			if (byteIndex < 0 || byteIndex > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("byteIndex", Encoding._("ArgRange_Array"));
			}
			if (bytes.Length - byteIndex < charCount)
			{
				throw new ArgumentException(Encoding._("Arg_InsufficientSpace"));
			}
			int num = charCount;
			while (num-- > 0)
			{
				char c = chars[charIndex++];
				if (c < 'Ā')
				{
					bytes[byteIndex++] = (byte)c;
				}
				else if (c >= '！' && c <= '～')
				{
					bytes[byteIndex++] = (byte)(c - 'ﻠ');
				}
				else
				{
					if (buffer == null)
					{
						buffer = base.EncoderFallback.CreateFallbackBuffer();
					}
					if (char.IsSurrogate(c) && num > 1 && char.IsSurrogate(chars[charIndex]))
					{
						buffer.Fallback(c, chars[charIndex], charIndex++ - 1);
					}
					else
					{
						buffer.Fallback(c, charIndex - 1);
					}
					if (fallback_chars == null || fallback_chars.Length < buffer.Remaining)
					{
						fallback_chars = new char[buffer.Remaining];
					}
					for (int i = 0; i < fallback_chars.Length; i++)
					{
						fallback_chars[i] = buffer.GetNextChar();
					}
					byteIndex += this.GetBytes(fallback_chars, 0, fallback_chars.Length, bytes, byteIndex, ref buffer, ref fallback_chars);
				}
			}
			return charCount;
		}

		public override int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			EncoderFallbackBuffer encoderFallbackBuffer = null;
			char[] array = null;
			return this.GetBytes(s, charIndex, charCount, bytes, byteIndex, ref encoderFallbackBuffer, ref array);
		}

		private int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex, ref EncoderFallbackBuffer buffer, ref char[] fallback_chars)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (charIndex < 0 || charIndex > s.Length)
			{
				throw new ArgumentOutOfRangeException("charIndex", Encoding._("ArgRange_StringIndex"));
			}
			if (charCount < 0 || charCount > s.Length - charIndex)
			{
				throw new ArgumentOutOfRangeException("charCount", Encoding._("ArgRange_StringRange"));
			}
			if (byteIndex < 0 || byteIndex > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("byteIndex", Encoding._("ArgRange_Array"));
			}
			if (bytes.Length - byteIndex < charCount)
			{
				throw new ArgumentException(Encoding._("Arg_InsufficientSpace"));
			}
			int num = charCount;
			while (num-- > 0)
			{
				char c = s[charIndex++];
				if (c < 'Ā')
				{
					bytes[byteIndex++] = (byte)c;
				}
				else if (c >= '！' && c <= '～')
				{
					bytes[byteIndex++] = (byte)(c - 'ﻠ');
				}
				else
				{
					if (buffer == null)
					{
						buffer = base.EncoderFallback.CreateFallbackBuffer();
					}
					if (char.IsSurrogate(c) && num > 1 && char.IsSurrogate(s[charIndex]))
					{
						buffer.Fallback(c, s[charIndex], charIndex++ - 1);
					}
					else
					{
						buffer.Fallback(c, charIndex - 1);
					}
					if (fallback_chars == null || fallback_chars.Length < buffer.Remaining)
					{
						fallback_chars = new char[buffer.Remaining];
					}
					for (int i = 0; i < fallback_chars.Length; i++)
					{
						fallback_chars[i] = buffer.GetNextChar();
					}
					byteIndex += this.GetBytes(fallback_chars, 0, fallback_chars.Length, bytes, byteIndex, ref buffer, ref fallback_chars);
				}
			}
			return charCount;
		}

		public override int GetCharCount(byte[] bytes, int index, int count)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (index < 0 || index > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("index", Encoding._("ArgRange_Array"));
			}
			if (count < 0 || count > bytes.Length - index)
			{
				throw new ArgumentOutOfRangeException("count", Encoding._("ArgRange_Array"));
			}
			return count;
		}

		public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (chars == null)
			{
				throw new ArgumentNullException("chars");
			}
			if (byteIndex < 0 || byteIndex > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("byteIndex", Encoding._("ArgRange_Array"));
			}
			if (byteCount < 0 || byteCount > bytes.Length - byteIndex)
			{
				throw new ArgumentOutOfRangeException("byteCount", Encoding._("ArgRange_Array"));
			}
			if (charIndex < 0 || charIndex > chars.Length)
			{
				throw new ArgumentOutOfRangeException("charIndex", Encoding._("ArgRange_Array"));
			}
			if (chars.Length - charIndex < byteCount)
			{
				throw new ArgumentException(Encoding._("Arg_InsufficientSpace"));
			}
			int num = byteCount;
			while (num-- > 0)
			{
				chars[charIndex++] = (char)bytes[byteIndex++];
			}
			return byteCount;
		}

		public override int GetMaxByteCount(int charCount)
		{
			if (charCount < 0)
			{
				throw new ArgumentOutOfRangeException("charCount", Encoding._("ArgRange_NonNegative"));
			}
			return charCount;
		}

		public override int GetMaxCharCount(int byteCount)
		{
			if (byteCount < 0)
			{
				throw new ArgumentOutOfRangeException("byteCount", Encoding._("ArgRange_NonNegative"));
			}
			return byteCount;
		}

		public unsafe override string GetString(byte[] bytes, int index, int count)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (index < 0 || index > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("index", Encoding._("ArgRange_Array"));
			}
			if (count < 0 || count > bytes.Length - index)
			{
				throw new ArgumentOutOfRangeException("count", Encoding._("ArgRange_Array"));
			}
			if (count == 0)
			{
				return string.Empty;
			}
			fixed (byte* ptr = ref (bytes != null && bytes.Length != 0) ? ref bytes[0] : ref *null)
			{
				string text = string.InternalAllocateStr(count);
				fixed (string text2 = text)
				{
					fixed (char* ptr2 = text2 + RuntimeHelpers.OffsetToStringData / 2)
					{
						byte* ptr3 = ptr + index;
						byte* ptr4 = ptr3 + count;
						char* ptr5 = ptr2;
						while (ptr3 < ptr4)
						{
							*(ptr5++) = (char)(*(ptr3++));
						}
						text2 = null;
						return text;
					}
				}
			}
		}

		public override string GetString(byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			return this.GetString(bytes, 0, bytes.Length);
		}

		public override string BodyName
		{
			get
			{
				return "iso-8859-1";
			}
		}

		public override string EncodingName
		{
			get
			{
				return "Western European (ISO)";
			}
		}

		public override string HeaderName
		{
			get
			{
				return "iso-8859-1";
			}
		}

		public override bool IsBrowserDisplay
		{
			get
			{
				return true;
			}
		}

		public override bool IsBrowserSave
		{
			get
			{
				return true;
			}
		}

		public override bool IsMailNewsDisplay
		{
			get
			{
				return true;
			}
		}

		public override bool IsMailNewsSave
		{
			get
			{
				return true;
			}
		}

		public override string WebName
		{
			get
			{
				return "iso-8859-1";
			}
		}
	}
}
