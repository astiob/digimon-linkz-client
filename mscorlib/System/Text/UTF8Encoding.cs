using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Text
{
	/// <summary>Represents a UTF-8 encoding of Unicode characters.</summary>
	/// <filterpriority>1</filterpriority>
	[MonoTODO("Serialization format not compatible with .NET")]
	[ComVisible(true)]
	[MonoTODO("EncoderFallback is not handled")]
	[Serializable]
	public class UTF8Encoding : Encoding
	{
		internal const int UTF8_CODE_PAGE = 65001;

		private bool emitIdentifier;

		/// <summary>Initializes a new instance of the <see cref="T:System.Text.UTF8Encoding" /> class.</summary>
		public UTF8Encoding() : this(false, false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Text.UTF8Encoding" /> class. A parameter specifies whether to provide a Unicode byte order mark.</summary>
		/// <param name="encoderShouldEmitUTF8Identifier">true to specify that a Unicode byte order mark is provided; otherwise, false. </param>
		public UTF8Encoding(bool encoderShouldEmitUTF8Identifier) : this(encoderShouldEmitUTF8Identifier, false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Text.UTF8Encoding" /> class. Parameters specify whether to provide a Unicode byte order mark and whether to throw an exception when an invalid encoding is detected.</summary>
		/// <param name="encoderShouldEmitUTF8Identifier">true to specify that a Unicode byte order mark is provided; otherwise, false. </param>
		/// <param name="throwOnInvalidBytes">true to specify that an exception be thrown when an invalid encoding is detected; otherwise, false. </param>
		public UTF8Encoding(bool encoderShouldEmitUTF8Identifier, bool throwOnInvalidBytes) : base(65001)
		{
			this.emitIdentifier = encoderShouldEmitUTF8Identifier;
			if (throwOnInvalidBytes)
			{
				base.SetFallbackInternal(null, DecoderFallback.ExceptionFallback);
			}
			else
			{
				base.SetFallbackInternal(null, DecoderFallback.StandardSafeFallback);
			}
			this.web_name = (this.body_name = (this.header_name = "utf-8"));
			this.encoding_name = "Unicode (UTF-8)";
			this.is_browser_save = true;
			this.is_browser_display = true;
			this.is_mail_news_display = true;
			this.is_mail_news_save = true;
			this.windows_code_page = 1200;
		}

		private unsafe static int InternalGetByteCount(char[] chars, int index, int count, ref char leftOver, bool flush)
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
			if (index != chars.Length)
			{
				fixed (char* ptr = ref (chars != null && chars.Length != 0) ? ref chars[0] : ref *null)
				{
					return UTF8Encoding.InternalGetByteCount(ptr + index, count, ref leftOver, flush);
				}
			}
			if (flush && leftOver != '\0')
			{
				leftOver = '\0';
				return 3;
			}
			return 0;
		}

		private unsafe static int InternalGetByteCount(char* chars, int count, ref char leftOver, bool flush)
		{
			int num = 0;
			char* ptr = chars + count;
			while (chars < ptr)
			{
				if (leftOver == '\0')
				{
					while (chars < ptr)
					{
						if (*chars < '\u0080')
						{
							num++;
						}
						else if (*chars < 'ࠀ')
						{
							num += 2;
						}
						else if (*chars < '\ud800' || *chars > '\udfff')
						{
							num += 3;
						}
						else if (*chars <= '\udbff')
						{
							if (chars + 1 >= ptr || chars[1] < '\udc00' || chars[1] > '\udfff')
							{
								leftOver = *chars;
								chars++;
								break;
							}
							num += 4;
							chars++;
						}
						else
						{
							num += 3;
							leftOver = '\0';
						}
						chars++;
					}
				}
				else
				{
					if (*chars >= '\udc00' && *chars <= '\udfff')
					{
						num += 4;
						chars++;
					}
					else
					{
						num += 3;
					}
					leftOver = '\0';
				}
			}
			if (flush && leftOver != '\0')
			{
				num += 3;
				leftOver = '\0';
			}
			return num;
		}

		/// <summary>Calculates the number of bytes produced by encoding a set of characters from the specified character array.</summary>
		/// <returns>The number of bytes produced by encoding the specified characters.</returns>
		/// <param name="chars">The character array containing the set of characters to encode. </param>
		/// <param name="index">The index of the first character to encode. </param>
		/// <param name="count">The number of characters to encode. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="chars" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is less than zero.-or- <paramref name="index" /> and <paramref name="count" /> do not denote a valid range in <paramref name="chars" />.-or- The resulting number of bytes is greater than the maximum number that can be returned as an integer. </exception>
		/// <exception cref="T:System.ArgumentException">Error detection is enabled, and <paramref name="chars" /> contains an invalid sequence of characters. </exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public override int GetByteCount(char[] chars, int index, int count)
		{
			char c = '\0';
			return UTF8Encoding.InternalGetByteCount(chars, index, count, ref c, true);
		}

		/// <summary>Calculates the number of bytes produced by encoding a set of characters starting at the specified character pointer.</summary>
		/// <returns>The number of bytes produced by encoding the specified characters.</returns>
		/// <param name="chars">A pointer to the first character to encode. </param>
		/// <param name="count">The number of characters to encode. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="chars" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="count" /> is less than zero.-or- The resulting number of bytes is greater than the maximum number that can be returned as an integer. </exception>
		/// <exception cref="T:System.ArgumentException">Error detection is enabled, and <paramref name="chars" /> contains an invalid sequence of characters. </exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		[ComVisible(false)]
		public unsafe override int GetByteCount(char* chars, int count)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars");
			}
			if (count == 0)
			{
				return 0;
			}
			char c = '\0';
			return UTF8Encoding.InternalGetByteCount(chars, count, ref c, true);
		}

		private unsafe static int InternalGetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex, ref char leftOver, bool flush)
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
			if (charIndex == chars.Length)
			{
				if (flush && leftOver != '\0')
				{
					leftOver = '\0';
				}
				return 0;
			}
			fixed (char* ptr = ref (chars != null && chars.Length != 0) ? ref chars[0] : ref *null)
			{
				if (bytes.Length == byteIndex)
				{
					return UTF8Encoding.InternalGetBytes(ptr + charIndex, charCount, null, 0, ref leftOver, flush);
				}
				fixed (byte* ptr2 = ref (bytes != null && bytes.Length != 0) ? ref bytes[0] : ref *null)
				{
					return UTF8Encoding.InternalGetBytes(ptr + charIndex, charCount, ptr2 + byteIndex, bytes.Length - byteIndex, ref leftOver, flush);
				}
			}
		}

		private unsafe static int InternalGetBytes(char* chars, int count, byte* bytes, int bcount, ref char leftOver, bool flush)
		{
			char* ptr = chars + count;
			byte* ptr2 = bytes + bcount;
			while (chars < ptr)
			{
				if (leftOver == '\0')
				{
					while (chars < ptr)
					{
						int num = (int)(*chars);
						if (num < 128)
						{
							if (bytes >= ptr2)
							{
								goto IL_29B;
							}
							*(bytes++) = (byte)num;
						}
						else if (num < 2048)
						{
							if (bytes + 1 >= ptr2)
							{
								goto IL_29B;
							}
							*bytes = (byte)(192 | num >> 6);
							bytes[1] = (byte)(128 | (num & 63));
							bytes += 2;
						}
						else if (num < 55296 || num > 57343)
						{
							if (bytes + 2 >= ptr2)
							{
								goto IL_29B;
							}
							*bytes = (byte)(224 | num >> 12);
							bytes[1] = (byte)(128 | (num >> 6 & 63));
							bytes[2] = (byte)(128 | (num & 63));
							bytes += 3;
						}
						else
						{
							if (num <= 56319)
							{
								leftOver = *chars;
								chars++;
								break;
							}
							if (bytes + 2 >= ptr2)
							{
								goto IL_29B;
							}
							*bytes = (byte)(224 | num >> 12);
							bytes[1] = (byte)(128 | (num >> 6 & 63));
							bytes[2] = (byte)(128 | (num & 63));
							bytes += 3;
							leftOver = '\0';
						}
						chars++;
					}
					continue;
				}
				if (*chars >= '\udc00' && *chars <= '\udfff')
				{
					int num2 = 65536 + (int)(*chars) - 56320 + (int)((int)(leftOver - '\ud800') << 10);
					if (bytes + 3 >= ptr2)
					{
						goto IL_29B;
					}
					*bytes = (byte)(240 | num2 >> 18);
					bytes[1] = (byte)(128 | (num2 >> 12 & 63));
					bytes[2] = (byte)(128 | (num2 >> 6 & 63));
					bytes[3] = (byte)(128 | (num2 & 63));
					bytes += 4;
					chars++;
				}
				else
				{
					int num3 = (int)leftOver;
					if (bytes + 2 >= ptr2)
					{
						goto IL_29B;
					}
					*bytes = (byte)(224 | num3 >> 12);
					bytes[1] = (byte)(128 | (num3 >> 6 & 63));
					bytes[2] = (byte)(128 | (num3 & 63));
					bytes += 3;
				}
				leftOver = '\0';
				continue;
				IL_29B:
				throw new ArgumentException("Insufficient Space", "bytes");
			}
			if (flush && leftOver != '\0')
			{
				int num4 = (int)leftOver;
				if (bytes + 2 >= ptr2)
				{
					goto IL_29B;
				}
				*bytes = (byte)(224 | num4 >> 12);
				bytes[1] = (byte)(128 | (num4 >> 6 & 63));
				bytes[2] = (byte)(128 | (num4 & 63));
				bytes += 3;
				leftOver = '\0';
			}
			return (int)((long)(bytes - (ptr2 - bcount)));
		}

		/// <summary>Encodes a set of characters from the specified character array into the specified byte array.</summary>
		/// <returns>The actual number of bytes written into <paramref name="bytes" />.</returns>
		/// <param name="chars">The character array containing the set of characters to encode. </param>
		/// <param name="charIndex">The index of the first character to encode. </param>
		/// <param name="charCount">The number of characters to encode. </param>
		/// <param name="bytes">The byte array to contain the resulting sequence of bytes. </param>
		/// <param name="byteIndex">The index at which to start writing the resulting sequence of bytes. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="chars" /> is null.-or- <paramref name="bytes" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="charIndex" /> or <paramref name="charCount" /> or <paramref name="byteIndex" /> is less than zero.-or- <paramref name="charIndex" /> and <paramref name="charCount" /> do not denote a valid range in <paramref name="chars" />.-or- <paramref name="byteIndex" /> is not a valid index in <paramref name="bytes" />. </exception>
		/// <exception cref="T:System.ArgumentException">Error detection is enabled, and <paramref name="chars" /> contains an invalid sequence of characters.-or- <paramref name="bytes" /> does not have enough capacity from <paramref name="byteIndex" /> to the end of the array to accommodate the resulting bytes. </exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			char c = '\0';
			return UTF8Encoding.InternalGetBytes(chars, charIndex, charCount, bytes, byteIndex, ref c, true);
		}

		/// <summary>Encodes a set of characters from the specified <see cref="T:System.String" /> into the specified byte array.</summary>
		/// <returns>The actual number of bytes written into <paramref name="bytes" />.</returns>
		/// <param name="s">The <see cref="T:System.String" /> containing the set of characters to encode. </param>
		/// <param name="charIndex">The index of the first character to encode. </param>
		/// <param name="charCount">The number of characters to encode. </param>
		/// <param name="bytes">The byte array to contain the resulting sequence of bytes. </param>
		/// <param name="byteIndex">The index at which to start writing the resulting sequence of bytes. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null.-or- <paramref name="bytes" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="charIndex" /> or <paramref name="charCount" /> or <paramref name="byteIndex" /> is less than zero.-or- <paramref name="charIndex" /> and <paramref name="charCount" /> do not denote a valid range in <paramref name="chars" />.-or- <paramref name="byteIndex" /> is not a valid index in <paramref name="bytes" />. </exception>
		/// <exception cref="T:System.ArgumentException">Error detection is enabled, and <paramref name="s" /> contains an invalid sequence of characters.-or- <paramref name="bytes" /> does not have enough capacity from <paramref name="byteIndex" /> to the end of the array to accommodate the resulting bytes. </exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public unsafe override int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex)
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
			if (charIndex == s.Length)
			{
				return 0;
			}
			fixed (char* ptr = s + RuntimeHelpers.OffsetToStringData / 2)
			{
				char c = '\0';
				if (bytes.Length == byteIndex)
				{
					return UTF8Encoding.InternalGetBytes(ptr + charIndex, charCount, null, 0, ref c, true);
				}
				fixed (byte* ptr2 = ref (bytes != null && bytes.Length != 0) ? ref bytes[0] : ref *null)
				{
					return UTF8Encoding.InternalGetBytes(ptr + charIndex, charCount, ptr2 + byteIndex, bytes.Length - byteIndex, ref c, true);
				}
			}
		}

		/// <summary>Encodes a set of characters starting at the specified character pointer into a sequence of bytes that are stored starting at the specified byte pointer.</summary>
		/// <returns>The actual number of bytes written at the location indicated by <paramref name="bytes" />.</returns>
		/// <param name="chars">A pointer to the first character to encode. </param>
		/// <param name="charCount">The number of characters to encode. </param>
		/// <param name="bytes">A pointer to the location at which to start writing the resulting sequence of bytes. </param>
		/// <param name="byteCount">The maximum number of bytes to write. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="chars" /> is null.-or- <paramref name="bytes" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="charCount" /> or <paramref name="byteCount" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">Error detection is enabled, and <paramref name="chars" /> contains an invalid sequence of characters.-or- <paramref name="byteCount" /> is less than the resulting number of bytes. </exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		[ComVisible(false)]
		public unsafe override int GetBytes(char* chars, int charCount, byte* bytes, int byteCount)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars");
			}
			if (charCount < 0)
			{
				throw new IndexOutOfRangeException("charCount");
			}
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (byteCount < 0)
			{
				throw new IndexOutOfRangeException("charCount");
			}
			if (charCount == 0)
			{
				return 0;
			}
			char c = '\0';
			if (byteCount == 0)
			{
				return UTF8Encoding.InternalGetBytes(chars, charCount, null, 0, ref c, true);
			}
			return UTF8Encoding.InternalGetBytes(chars, charCount, bytes, byteCount, ref c, true);
		}

		private unsafe static int InternalGetCharCount(byte[] bytes, int index, int count, uint leftOverBits, uint leftOverCount, object provider, ref DecoderFallbackBuffer fallbackBuffer, ref byte[] bufferArg, bool flush)
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
				return 0;
			}
			fixed (byte* ptr = ref (bytes != null && bytes.Length != 0) ? ref bytes[0] : ref *null)
			{
				return UTF8Encoding.InternalGetCharCount(ptr + index, count, leftOverBits, leftOverCount, provider, ref fallbackBuffer, ref bufferArg, flush);
			}
		}

		private unsafe static int InternalGetCharCount(byte* bytes, int count, uint leftOverBits, uint leftOverCount, object provider, ref DecoderFallbackBuffer fallbackBuffer, ref byte[] bufferArg, bool flush)
		{
			int i = 0;
			int num = 0;
			if (leftOverCount == 0u)
			{
				int num2 = i + count;
				while (i < num2)
				{
					if (bytes[i] >= 128)
					{
						break;
					}
					num++;
					i++;
					count--;
				}
			}
			uint num3 = leftOverBits;
			uint num4 = leftOverCount & 15u;
			uint num5 = leftOverCount >> 4 & 15u;
			while (count > 0)
			{
				uint num6 = (uint)bytes[i++];
				count--;
				if (num5 == 0u)
				{
					if (num6 < 128u)
					{
						num++;
					}
					else if ((num6 & 224u) == 192u)
					{
						num3 = (num6 & 31u);
						num4 = 1u;
						num5 = 2u;
					}
					else if ((num6 & 240u) == 224u)
					{
						num3 = (num6 & 15u);
						num4 = 1u;
						num5 = 3u;
					}
					else if ((num6 & 248u) == 240u)
					{
						num3 = (num6 & 7u);
						num4 = 1u;
						num5 = 4u;
					}
					else if ((num6 & 252u) == 248u)
					{
						num3 = (num6 & 3u);
						num4 = 1u;
						num5 = 5u;
					}
					else if ((num6 & 254u) == 252u)
					{
						num3 = (num6 & 3u);
						num4 = 1u;
						num5 = 6u;
					}
					else
					{
						num += UTF8Encoding.Fallback(provider, ref fallbackBuffer, ref bufferArg, bytes, (long)(i - 1), 1u);
					}
				}
				else if ((num6 & 192u) == 128u)
				{
					num3 = (num3 << 6 | (num6 & 63u));
					if ((num4 += 1u) >= num5)
					{
						if (num3 < 65536u)
						{
							bool flag = false;
							switch (num5)
							{
							case 2u:
								flag = (num3 <= 127u);
								break;
							case 3u:
								flag = (num3 <= 2047u);
								break;
							case 4u:
								flag = (num3 <= 65535u);
								break;
							case 5u:
								flag = (num3 <= 2097151u);
								break;
							case 6u:
								flag = (num3 <= 67108863u);
								break;
							}
							if (flag)
							{
								num += UTF8Encoding.Fallback(provider, ref fallbackBuffer, ref bufferArg, bytes, (long)i - (long)((ulong)num4), num4);
							}
							else if ((num3 & 63488u) == 55296u)
							{
								num += UTF8Encoding.Fallback(provider, ref fallbackBuffer, ref bufferArg, bytes, (long)i - (long)((ulong)num4), num4);
							}
							else
							{
								num++;
							}
						}
						else if (num3 < 1114112u)
						{
							num += 2;
						}
						else
						{
							num += UTF8Encoding.Fallback(provider, ref fallbackBuffer, ref bufferArg, bytes, (long)i - (long)((ulong)num4), num4);
						}
						num5 = 0u;
					}
				}
				else
				{
					num += UTF8Encoding.Fallback(provider, ref fallbackBuffer, ref bufferArg, bytes, (long)i - (long)((ulong)num4), num4);
					num5 = 0u;
					i--;
					count++;
				}
			}
			if (flush && num5 != 0u)
			{
				num += UTF8Encoding.Fallback(provider, ref fallbackBuffer, ref bufferArg, bytes, (long)i - (long)((ulong)num4), num4);
			}
			return num;
		}

		private unsafe static int Fallback(object provider, ref DecoderFallbackBuffer buffer, ref byte[] bufferArg, byte* bytes, long index, uint size)
		{
			if (buffer == null)
			{
				DecoderFallback decoderFallback = provider as DecoderFallback;
				if (decoderFallback != null)
				{
					buffer = decoderFallback.CreateFallbackBuffer();
				}
				else
				{
					buffer = ((Decoder)provider).FallbackBuffer;
				}
			}
			if (bufferArg == null)
			{
				bufferArg = new byte[1];
			}
			int num = 0;
			int num2 = 0;
			while ((long)num2 < (long)((ulong)size))
			{
				bufferArg[0] = bytes[(int)index + num2];
				buffer.Fallback(bufferArg, 0);
				num += buffer.Remaining;
				buffer.Reset();
				num2++;
			}
			return num;
		}

		private unsafe static void Fallback(object provider, ref DecoderFallbackBuffer buffer, ref byte[] bufferArg, byte* bytes, long byteIndex, uint size, char* chars, ref int charIndex)
		{
			if (buffer == null)
			{
				DecoderFallback decoderFallback = provider as DecoderFallback;
				if (decoderFallback != null)
				{
					buffer = decoderFallback.CreateFallbackBuffer();
				}
				else
				{
					buffer = ((Decoder)provider).FallbackBuffer;
				}
			}
			if (bufferArg == null)
			{
				bufferArg = new byte[1];
			}
			int num = 0;
			while ((long)num < (long)((ulong)size))
			{
				bufferArg[0] = bytes[byteIndex + (long)num];
				buffer.Fallback(bufferArg, 0);
				while (buffer.Remaining > 0)
				{
					chars[charIndex++] = buffer.GetNextChar();
				}
				buffer.Reset();
				num++;
			}
		}

		/// <summary>Calculates the number of characters produced by decoding a sequence of bytes from the specified byte array.</summary>
		/// <returns>The number of characters produced by decoding the specified sequence of bytes.</returns>
		/// <param name="bytes">The byte array containing the sequence of bytes to decode. </param>
		/// <param name="index">The index of the first byte to decode. </param>
		/// <param name="count">The number of bytes to decode. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is less than zero.-or- <paramref name="index" /> and <paramref name="count" /> do not denote a valid range in <paramref name="bytes" />.-or- The resulting number of bytes is greater than the maximum number that can be returned as an integer. </exception>
		/// <exception cref="T:System.ArgumentException">Error detection is enabled, and <paramref name="bytes" /> contains an invalid sequence of bytes. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public override int GetCharCount(byte[] bytes, int index, int count)
		{
			DecoderFallbackBuffer decoderFallbackBuffer = null;
			byte[] array = null;
			return UTF8Encoding.InternalGetCharCount(bytes, index, count, 0u, 0u, base.DecoderFallback, ref decoderFallbackBuffer, ref array, true);
		}

		/// <summary>Calculates the number of characters produced by decoding a sequence of bytes starting at the specified byte pointer.</summary>
		/// <returns>The number of characters produced by decoding the specified sequence of bytes.</returns>
		/// <param name="bytes"></param>
		/// <param name="count"></param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="count" /> is less than zero.-or- The resulting number of bytes is greater than the maximum number that can be returned as an integer. </exception>
		/// <exception cref="T:System.ArgumentException">Error detection is enabled, and <paramref name="bytes" /> contains an invalid sequence of bytes. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		[ComVisible(false)]
		public unsafe override int GetCharCount(byte* bytes, int count)
		{
			DecoderFallbackBuffer decoderFallbackBuffer = null;
			byte[] array = null;
			return UTF8Encoding.InternalGetCharCount(bytes, count, 0u, 0u, base.DecoderFallback, ref decoderFallbackBuffer, ref array, true);
		}

		private unsafe static int InternalGetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex, ref uint leftOverBits, ref uint leftOverCount, object provider, ref DecoderFallbackBuffer fallbackBuffer, ref byte[] bufferArg, bool flush)
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
			if (charIndex == chars.Length)
			{
				return 0;
			}
			fixed (char* ptr = ref (chars != null && chars.Length != 0) ? ref chars[0] : ref *null)
			{
				if (byteCount == 0 || byteIndex == bytes.Length)
				{
					return UTF8Encoding.InternalGetChars(null, 0, ptr + charIndex, chars.Length - charIndex, ref leftOverBits, ref leftOverCount, provider, ref fallbackBuffer, ref bufferArg, flush);
				}
				fixed (byte* ptr2 = ref (bytes != null && bytes.Length != 0) ? ref bytes[0] : ref *null)
				{
					return UTF8Encoding.InternalGetChars(ptr2 + byteIndex, byteCount, ptr + charIndex, chars.Length - charIndex, ref leftOverBits, ref leftOverCount, provider, ref fallbackBuffer, ref bufferArg, flush);
				}
			}
		}

		private unsafe static int InternalGetChars(byte* bytes, int byteCount, char* chars, int charCount, ref uint leftOverBits, ref uint leftOverCount, object provider, ref DecoderFallbackBuffer fallbackBuffer, ref byte[] bufferArg, bool flush)
		{
			int num = 0;
			int i = 0;
			int num2 = num;
			if (leftOverCount == 0u)
			{
				int num3 = i + byteCount;
				while (i < num3)
				{
					if (bytes[i] >= 128)
					{
						break;
					}
					chars[num2] = (char)bytes[i];
					num2++;
					i++;
					byteCount--;
				}
			}
			uint num4 = leftOverBits;
			uint num5 = leftOverCount & 15u;
			uint num6 = leftOverCount >> 4 & 15u;
			int num7 = i + byteCount;
			while (i < num7)
			{
				uint num8 = (uint)bytes[i];
				if (num6 == 0u)
				{
					if (num8 < 128u)
					{
						if (num2 >= charCount)
						{
							throw new ArgumentException(Encoding._("Arg_InsufficientSpace"), "chars");
						}
						chars[num2++] = (char)num8;
					}
					else if ((num8 & 224u) == 192u)
					{
						num4 = (num8 & 31u);
						num5 = 1u;
						num6 = 2u;
					}
					else if ((num8 & 240u) == 224u)
					{
						num4 = (num8 & 15u);
						num5 = 1u;
						num6 = 3u;
					}
					else if ((num8 & 248u) == 240u)
					{
						num4 = (num8 & 7u);
						num5 = 1u;
						num6 = 4u;
					}
					else if ((num8 & 252u) == 248u)
					{
						num4 = (num8 & 3u);
						num5 = 1u;
						num6 = 5u;
					}
					else if ((num8 & 254u) == 252u)
					{
						num4 = (num8 & 3u);
						num5 = 1u;
						num6 = 6u;
					}
					else
					{
						UTF8Encoding.Fallback(provider, ref fallbackBuffer, ref bufferArg, bytes, (long)i, 1u, chars, ref num2);
					}
				}
				else if ((num8 & 192u) == 128u)
				{
					num4 = (num4 << 6 | (num8 & 63u));
					if ((num5 += 1u) >= num6)
					{
						if (num4 < 65536u)
						{
							bool flag = false;
							switch (num6)
							{
							case 2u:
								flag = (num4 <= 127u);
								break;
							case 3u:
								flag = (num4 <= 2047u);
								break;
							case 4u:
								flag = (num4 <= 65535u);
								break;
							case 5u:
								flag = (num4 <= 2097151u);
								break;
							case 6u:
								flag = (num4 <= 67108863u);
								break;
							}
							if (flag)
							{
								UTF8Encoding.Fallback(provider, ref fallbackBuffer, ref bufferArg, bytes, (long)i - (long)((ulong)num5), num5, chars, ref num2);
							}
							else if ((num4 & 63488u) == 55296u)
							{
								UTF8Encoding.Fallback(provider, ref fallbackBuffer, ref bufferArg, bytes, (long)i - (long)((ulong)num5), num5, chars, ref num2);
							}
							else
							{
								if (num2 >= charCount)
								{
									throw new ArgumentException(Encoding._("Arg_InsufficientSpace"), "chars");
								}
								chars[num2++] = (char)num4;
							}
						}
						else if (num4 < 1114112u)
						{
							if (num2 + 2 > charCount)
							{
								throw new ArgumentException(Encoding._("Arg_InsufficientSpace"), "chars");
							}
							num4 -= 65536u;
							chars[num2++] = (char)((num4 >> 10) + 55296u);
							chars[num2++] = (char)((num4 & 1023u) + 56320u);
						}
						else
						{
							UTF8Encoding.Fallback(provider, ref fallbackBuffer, ref bufferArg, bytes, (long)i - (long)((ulong)num5), num5, chars, ref num2);
						}
						num6 = 0u;
					}
				}
				else
				{
					UTF8Encoding.Fallback(provider, ref fallbackBuffer, ref bufferArg, bytes, (long)i - (long)((ulong)num5), num5, chars, ref num2);
					num6 = 0u;
					i--;
				}
				i++;
			}
			if (flush && num6 != 0u)
			{
				UTF8Encoding.Fallback(provider, ref fallbackBuffer, ref bufferArg, bytes, (long)i - (long)((ulong)num5), num5, chars, ref num2);
			}
			leftOverBits = num4;
			leftOverCount = (num5 | num6 << 4);
			return num2 - num;
		}

		/// <summary>Decodes a sequence of bytes from the specified byte array into the specified character array.</summary>
		/// <returns>The actual number of characters written into <paramref name="chars" />.</returns>
		/// <param name="bytes">The byte array containing the sequence of bytes to decode. </param>
		/// <param name="byteIndex">The index of the first byte to decode. </param>
		/// <param name="byteCount">The number of bytes to decode. </param>
		/// <param name="chars">The character array to contain the resulting set of characters. </param>
		/// <param name="charIndex">The index at which to start writing the resulting set of characters. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is null.-or- <paramref name="chars" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="byteIndex" /> or <paramref name="byteCount" /> or <paramref name="charIndex" /> is less than zero.-or- <paramref name="byteindex" /> and <paramref name="byteCount" /> do not denote a valid range in <paramref name="bytes" />.-or- <paramref name="charIndex" /> is not a valid index in <paramref name="chars" />. </exception>
		/// <exception cref="T:System.ArgumentException">Error detection is enabled, and <paramref name="bytes" /> contains an invalid sequence of bytes.-or- <paramref name="chars" /> does not have enough capacity from <paramref name="charIndex" /> to the end of the array to accommodate the resulting characters. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
		{
			uint num = 0u;
			uint num2 = 0u;
			DecoderFallbackBuffer decoderFallbackBuffer = null;
			byte[] array = null;
			return UTF8Encoding.InternalGetChars(bytes, byteIndex, byteCount, chars, charIndex, ref num, ref num2, base.DecoderFallback, ref decoderFallbackBuffer, ref array, true);
		}

		/// <summary>Decodes a sequence of bytes starting at the specified byte pointer into a set of characters that are stored starting at the specified character pointer.</summary>
		/// <returns>The actual number of characters written at the location indicated by <paramref name="chars" />.</returns>
		/// <param name="bytes">A pointer to the first byte to decode. </param>
		/// <param name="byteCount">The number of bytes to decode. </param>
		/// <param name="chars">A pointer to the location at which to start writing the resulting set of characters. </param>
		/// <param name="charCount">The maximum number of characters to write. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is null.-or- <paramref name="chars" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="byteCount" /> or <paramref name="charCount" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">Error detection is enabled, and <paramref name="bytes" /> contains an invalid sequence of bytes.-or- <paramref name="charCount" /> is less than the resulting number of characters. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(false)]
		[CLSCompliant(false)]
		public unsafe override int GetChars(byte* bytes, int byteCount, char* chars, int charCount)
		{
			DecoderFallbackBuffer decoderFallbackBuffer = null;
			byte[] array = null;
			uint num = 0u;
			uint num2 = 0u;
			return UTF8Encoding.InternalGetChars(bytes, byteCount, chars, charCount, ref num, ref num2, base.DecoderFallback, ref decoderFallbackBuffer, ref array, true);
		}

		/// <summary>Calculates the maximum number of bytes produced by encoding the specified number of characters.</summary>
		/// <returns>The maximum number of bytes produced by encoding the specified number of characters.</returns>
		/// <param name="charCount">The number of characters to encode. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="charCount" /> is less than zero.-or- The resulting number of bytes is greater than the maximum number that can be returned as an integer. </exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public override int GetMaxByteCount(int charCount)
		{
			if (charCount < 0)
			{
				throw new ArgumentOutOfRangeException("charCount", Encoding._("ArgRange_NonNegative"));
			}
			return charCount * 4;
		}

		/// <summary>Calculates the maximum number of characters produced by decoding the specified number of bytes.</summary>
		/// <returns>The maximum number of characters produced by decoding the specified number of bytes.</returns>
		/// <param name="byteCount">The number of bytes to decode. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="byteCount" /> is less than zero.-or- The resulting number of bytes is greater than the maximum number that can be returned as an integer. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public override int GetMaxCharCount(int byteCount)
		{
			if (byteCount < 0)
			{
				throw new ArgumentOutOfRangeException("byteCount", Encoding._("ArgRange_NonNegative"));
			}
			return byteCount;
		}

		/// <summary>Obtains a decoder that converts a UTF-8 encoded sequence of bytes into a sequence of Unicode characters.</summary>
		/// <returns>A <see cref="T:System.Text.Decoder" /> that converts a UTF-8 encoded sequence of bytes into a sequence of Unicode characters.</returns>
		/// <filterpriority>1</filterpriority>
		public override Decoder GetDecoder()
		{
			return new UTF8Encoding.UTF8Decoder(base.DecoderFallback);
		}

		/// <summary>Obtains an encoder that converts a sequence of Unicode characters into a UTF-8 encoded sequence of bytes.</summary>
		/// <returns>A <see cref="T:System.Text.Encoder" /> that converts a sequence of Unicode characters into a UTF-8 encoded sequence of bytes.</returns>
		/// <filterpriority>1</filterpriority>
		public override Encoder GetEncoder()
		{
			return new UTF8Encoding.UTF8Encoder(this.emitIdentifier);
		}

		/// <summary>Returns a Unicode byte order mark encoded in UTF-8 format, if the constructor for this instance requests a byte order mark.</summary>
		/// <returns>A byte array containing the Unicode byte order mark, if the constructor for this instance requests a byte order mark. Otherwise, this method returns a byte array of length zero.</returns>
		/// <filterpriority>1</filterpriority>
		public override byte[] GetPreamble()
		{
			if (this.emitIdentifier)
			{
				return new byte[]
				{
					239,
					187,
					191
				};
			}
			return new byte[0];
		}

		/// <summary>Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Text.UTF8Encoding" /> object.</summary>
		/// <returns>true if <paramref name="value" /> is an instance of <see cref="T:System.Text.UTF8Encoding" /> and is equal to the current object; otherwise, false.</returns>
		/// <param name="value">The <see cref="T:System.Object" /> to compare with the current instance. </param>
		/// <filterpriority>2</filterpriority>
		public override bool Equals(object value)
		{
			UTF8Encoding utf8Encoding = value as UTF8Encoding;
			return utf8Encoding != null && (this.codePage == utf8Encoding.codePage && this.emitIdentifier == utf8Encoding.emitIdentifier && base.DecoderFallback.Equals(utf8Encoding.DecoderFallback)) && base.EncoderFallback.Equals(utf8Encoding.EncoderFallback);
		}

		/// <summary>Returns the hash code for the current instance.</summary>
		/// <returns>The hash code for the current instance.</returns>
		/// <filterpriority>1</filterpriority>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>Calculates the number of bytes produced by encoding the characters in the specified <see cref="T:System.String" />.</summary>
		/// <returns>The number of bytes produced by encoding the specified characters.</returns>
		/// <param name="chars">The <see cref="T:System.String" /> containing the set of characters to encode. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="chars" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The resulting number of bytes is greater than the maximum number that can be returned as an integer. </exception>
		/// <exception cref="T:System.ArgumentException">Error detection is enabled, and <paramref name="chars" /> contains an invalid sequence of characters. </exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public override int GetByteCount(string chars)
		{
			return base.GetByteCount(chars);
		}

		/// <summary>Decodes a range of bytes from a byte array into a string.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the results of decoding the specified sequence of bytes.</returns>
		/// <param name="bytes">The byte array containing the sequence of bytes to decode. </param>
		/// <param name="index">The index of the first byte to decode. </param>
		/// <param name="count">The number of bytes to decode. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is less than zero.-or- <paramref name="index" /> and <paramref name="count" /> do not denote a valid range in <paramref name="bytes" />. </exception>
		/// <exception cref="T:System.ArgumentException">Error detection is enabled, and <paramref name="bytes" /> contains an invalid sequence of bytes. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(false)]
		public override string GetString(byte[] bytes, int index, int count)
		{
			return base.GetString(bytes, index, count);
		}

		[Serializable]
		private class UTF8Decoder : Decoder
		{
			private uint leftOverBits;

			private uint leftOverCount;

			public UTF8Decoder(DecoderFallback fallback)
			{
				base.Fallback = fallback;
				this.leftOverBits = 0u;
				this.leftOverCount = 0u;
			}

			public override int GetCharCount(byte[] bytes, int index, int count)
			{
				DecoderFallbackBuffer decoderFallbackBuffer = null;
				byte[] array = null;
				return UTF8Encoding.InternalGetCharCount(bytes, index, count, this.leftOverBits, this.leftOverCount, this, ref decoderFallbackBuffer, ref array, false);
			}

			public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
			{
				DecoderFallbackBuffer decoderFallbackBuffer = null;
				byte[] array = null;
				return UTF8Encoding.InternalGetChars(bytes, byteIndex, byteCount, chars, charIndex, ref this.leftOverBits, ref this.leftOverCount, this, ref decoderFallbackBuffer, ref array, false);
			}
		}

		[Serializable]
		private class UTF8Encoder : Encoder
		{
			private char leftOverForCount;

			private char leftOverForConv;

			public UTF8Encoder(bool emitIdentifier)
			{
				this.leftOverForCount = '\0';
				this.leftOverForConv = '\0';
			}

			public override int GetByteCount(char[] chars, int index, int count, bool flush)
			{
				return UTF8Encoding.InternalGetByteCount(chars, index, count, ref this.leftOverForCount, flush);
			}

			public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex, bool flush)
			{
				return UTF8Encoding.InternalGetBytes(chars, charIndex, charCount, bytes, byteIndex, ref this.leftOverForConv, flush);
			}

			public unsafe override int GetByteCount(char* chars, int count, bool flush)
			{
				return UTF8Encoding.InternalGetByteCount(chars, count, ref this.leftOverForCount, flush);
			}

			public unsafe override int GetBytes(char* chars, int charCount, byte* bytes, int byteCount, bool flush)
			{
				return UTF8Encoding.InternalGetBytes(chars, charCount, bytes, byteCount, ref this.leftOverForConv, flush);
			}
		}
	}
}
