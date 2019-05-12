using System;

namespace System.Text
{
	/// <summary>Represents a UTF-32 encoding of Unicode characters.</summary>
	/// <filterpriority>1</filterpriority>
	[Serializable]
	public sealed class UTF32Encoding : Encoding
	{
		internal const int UTF32_CODE_PAGE = 12000;

		internal const int BIG_UTF32_CODE_PAGE = 12001;

		private bool bigEndian;

		private bool byteOrderMark;

		/// <summary>Initializes a new instance of the <see cref="T:System.Text.UTF32Encoding" /> class.</summary>
		public UTF32Encoding() : this(false, true, false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Text.UTF32Encoding" /> class. Parameters specify whether to use the big endian byte order and whether to provide a Unicode byte order mark.</summary>
		/// <param name="bigEndian">true to use the big endian byte order (most significant byte first), or false to use the little endian byte order (least significant byte first). </param>
		/// <param name="byteOrderMark">true to specify that a Unicode byte order mark is provided; otherwise, false. </param>
		public UTF32Encoding(bool bigEndian, bool byteOrderMark) : this(bigEndian, byteOrderMark, false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Text.UTF32Encoding" /> class. Parameters specify whether to use the big endian byte order, whether to provide a Unicode byte order mark, and whether to throw an exception when an invalid encoding is detected.</summary>
		/// <param name="bigEndian">true to use the big endian byte order (most significant byte first), or false to use the little endian byte order (least significant byte first). </param>
		/// <param name="byteOrderMark">true to specify that a Unicode byte order mark is provided; otherwise, false. </param>
		/// <param name="throwOnInvalidCharacters">true to specify that an exception should be thrown when an invalid encoding is detected; otherwise, false. </param>
		public UTF32Encoding(bool bigEndian, bool byteOrderMark, bool throwOnInvalidCharacters) : base((!bigEndian) ? 12000 : 12001)
		{
			this.bigEndian = bigEndian;
			this.byteOrderMark = byteOrderMark;
			if (throwOnInvalidCharacters)
			{
				base.SetFallbackInternal(EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback);
			}
			else
			{
				base.SetFallbackInternal(new EncoderReplacementFallback("�"), new DecoderReplacementFallback("�"));
			}
			if (bigEndian)
			{
				this.body_name = "utf-32BE";
				this.encoding_name = "UTF-32 (Big-Endian)";
				this.header_name = "utf-32BE";
				this.web_name = "utf-32BE";
			}
			else
			{
				this.body_name = "utf-32";
				this.encoding_name = "UTF-32";
				this.header_name = "utf-32";
				this.web_name = "utf-32";
			}
			this.windows_code_page = 12000;
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
		[MonoTODO("handle fallback")]
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
			int num = 0;
			for (int i = index; i < index + count; i++)
			{
				if (char.IsSurrogate(chars[i]))
				{
					if (i + 1 < chars.Length && char.IsSurrogate(chars[i + 1]))
					{
						num += 4;
					}
					else
					{
						num += 4;
					}
				}
				else
				{
					num += 4;
				}
			}
			return num;
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
		[MonoTODO("handle fallback")]
		public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
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
			if (bytes.Length - byteIndex < charCount * 4)
			{
				throw new ArgumentException(Encoding._("Arg_InsufficientSpace"));
			}
			int num = byteIndex;
			while (charCount-- > 0)
			{
				char c = chars[charIndex++];
				if (char.IsSurrogate(c))
				{
					if (charCount-- > 0)
					{
						int num2 = (int)('Ѐ' * (c - '\ud800')) + 65536 + (int)chars[charIndex++] - 56320;
						if (this.bigEndian)
						{
							for (int i = 0; i < 4; i++)
							{
								bytes[num + 3 - i] = (byte)(num2 % 256);
								num2 >>= 8;
							}
							num += 4;
						}
						else
						{
							for (int j = 0; j < 4; j++)
							{
								bytes[num++] = (byte)(num2 % 256);
								num2 >>= 8;
							}
						}
					}
					else if (this.bigEndian)
					{
						bytes[num++] = 0;
						bytes[num++] = 0;
						bytes[num++] = 0;
						bytes[num++] = 63;
					}
					else
					{
						bytes[num++] = 63;
						bytes[num++] = 0;
						bytes[num++] = 0;
						bytes[num++] = 0;
					}
				}
				else if (this.bigEndian)
				{
					bytes[num++] = 0;
					bytes[num++] = 0;
					bytes[num++] = (byte)(c >> 8);
					bytes[num++] = (byte)c;
				}
				else
				{
					bytes[num++] = (byte)c;
					bytes[num++] = (byte)(c >> 8);
					bytes[num++] = 0;
					bytes[num++] = 0;
				}
			}
			return num - byteIndex;
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
			return count / 4;
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
			if (chars.Length - charIndex < byteCount / 4)
			{
				throw new ArgumentException(Encoding._("Arg_InsufficientSpace"));
			}
			int num = charIndex;
			if (this.bigEndian)
			{
				while (byteCount >= 4)
				{
					chars[num++] = (char)((int)bytes[byteIndex] << 24 | (int)bytes[byteIndex + 1] << 16 | (int)bytes[byteIndex + 2] << 8 | (int)bytes[byteIndex + 3]);
					byteIndex += 4;
					byteCount -= 4;
				}
			}
			else
			{
				while (byteCount >= 4)
				{
					chars[num++] = (char)((int)bytes[byteIndex] | (int)bytes[byteIndex + 1] << 8 | (int)bytes[byteIndex + 2] << 16 | (int)bytes[byteIndex + 3] << 24);
					byteIndex += 4;
					byteCount -= 4;
				}
			}
			return num - charIndex;
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
			return byteCount / 4;
		}

		/// <summary>Obtains a decoder that converts a UTF-32 encoded sequence of bytes into a sequence of Unicode characters.</summary>
		/// <returns>A <see cref="T:System.Text.Decoder" /> that converts a UTF-32 encoded sequence of bytes into a sequence of Unicode characters.</returns>
		/// <filterpriority>1</filterpriority>
		public override Decoder GetDecoder()
		{
			return new UTF32Encoding.UTF32Decoder(this.bigEndian);
		}

		/// <summary>Returns a Unicode byte order mark encoded in UTF-32 format, if the constructor for this instance requests a byte order mark.</summary>
		/// <returns>A byte array containing the Unicode byte order mark, if the constructor for this instance requests a byte order mark. Otherwise, this method returns a byte array of length zero.</returns>
		/// <filterpriority>1</filterpriority>
		public override byte[] GetPreamble()
		{
			if (this.byteOrderMark)
			{
				byte[] array = new byte[4];
				if (this.bigEndian)
				{
					array[2] = 254;
					array[3] = byte.MaxValue;
				}
				else
				{
					array[0] = byte.MaxValue;
					array[1] = 254;
				}
				return array;
			}
			return new byte[0];
		}

		/// <summary>Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Text.UTF32Encoding" /> object.</summary>
		/// <returns>true if <paramref name="value" /> is an instance of <see cref="T:System.Text.UTF32Encoding" /> and is equal to the current object; otherwise, false.</returns>
		/// <param name="value">The <see cref="T:System.Object" /> to compare with the current object. </param>
		/// <filterpriority>2</filterpriority>
		public override bool Equals(object value)
		{
			UTF32Encoding utf32Encoding = value as UTF32Encoding;
			return utf32Encoding != null && (this.codePage == utf32Encoding.codePage && this.bigEndian == utf32Encoding.bigEndian && this.byteOrderMark == utf32Encoding.byteOrderMark) && base.Equals(value);
		}

		/// <summary>Returns the hash code for the current instance.</summary>
		/// <returns>The hash code for the current <see cref="T:System.Text.UTF32Encoding" /> object.</returns>
		/// <filterpriority>1</filterpriority>
		public override int GetHashCode()
		{
			int num = base.GetHashCode();
			if (this.bigEndian)
			{
				num ^= 31;
			}
			if (this.byteOrderMark)
			{
				num ^= 63;
			}
			return num;
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
		public unsafe override int GetByteCount(char* chars, int count)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars");
			}
			return count * 4;
		}

		/// <summary>Calculates the number of bytes produced by encoding the characters in the specified <see cref="T:System.String" />.</summary>
		/// <returns>The number of bytes produced by encoding the specified characters.</returns>
		/// <param name="s">The <see cref="T:System.String" /> containing the set of characters to encode. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The resulting number of bytes is greater than the maximum number that can be returned as an integer. </exception>
		/// <exception cref="T:System.ArgumentException">Error detection is enabled, and <paramref name="s" /> contains an invalid sequence of characters. </exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public override int GetByteCount(string s)
		{
			return base.GetByteCount(s);
		}

		/// <summary>Encodes a set of characters starting at the specified character pointer into a sequence of bytes that are stored starting at the specified byte pointer.</summary>
		/// <returns>The actual number of bytes written at the location indicated by the <paramref name="bytes" /> parameter.</returns>
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
		public unsafe override int GetBytes(char* chars, int charCount, byte* bytes, int byteCount)
		{
			return base.GetBytes(chars, charCount, bytes, byteCount);
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
		public override int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			return base.GetBytes(s, charIndex, charCount, bytes, byteIndex);
		}

		/// <summary>Calculates the number of characters produced by decoding a sequence of bytes starting at the specified byte pointer.</summary>
		/// <returns>The number of characters produced by decoding the specified sequence of bytes.</returns>
		/// <param name="bytes">A pointer to the first byte to decode. </param>
		/// <param name="count">The number of bytes to decode. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="count" /> is less than zero.-or- The resulting number of bytes is greater than the maximum number that can be returned as an integer. </exception>
		/// <exception cref="T:System.ArgumentException">Error detection is enabled, and <paramref name="bytes" /> contains an invalid sequence of bytes. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		public unsafe override int GetCharCount(byte* bytes, int count)
		{
			return base.GetCharCount(bytes, count);
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
		[CLSCompliant(false)]
		public unsafe override int GetChars(byte* bytes, int byteCount, char* chars, int charCount)
		{
			return base.GetChars(bytes, byteCount, chars, charCount);
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
		public override string GetString(byte[] bytes, int index, int count)
		{
			return base.GetString(bytes, index, count);
		}

		/// <summary>Obtains an encoder that converts a sequence of Unicode characters into a UTF-32 encoded sequence of bytes.</summary>
		/// <returns>A <see cref="T:System.Text.Encoder" /> that converts a sequence of Unicode characters into a UTF-32 encoded sequence of bytes.</returns>
		/// <filterpriority>1</filterpriority>
		public override Encoder GetEncoder()
		{
			return base.GetEncoder();
		}

		private sealed class UTF32Decoder : Decoder
		{
			private bool bigEndian;

			private int leftOverByte;

			private int leftOverLength;

			public UTF32Decoder(bool bigEndian)
			{
				this.bigEndian = bigEndian;
				this.leftOverByte = -1;
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
				if (this.leftOverByte != -1)
				{
					return (count + 1) / 4;
				}
				return count / 4;
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
				int num = charIndex;
				int num2 = this.leftOverByte;
				int num3 = chars.Length;
				int num4 = 4 - this.leftOverLength;
				if (this.leftOverLength > 0 && byteCount > num4)
				{
					if (this.bigEndian)
					{
						for (int i = 0; i < num4; i++)
						{
							num2 += (int)bytes[byteIndex++] << 4 - byteCount--;
						}
					}
					else
					{
						for (int j = 0; j < num4; j++)
						{
							num2 += (int)bytes[byteIndex++] << byteCount--;
						}
					}
					if ((num2 > 65535 && num + 1 < num3) || num < num3)
					{
						throw new ArgumentException(Encoding._("Arg_InsufficientSpace"));
					}
					if (num2 > 65535)
					{
						chars[num++] = (char)((num2 - 10000) / 1024 + 55296);
						chars[num++] = (char)((num2 - 10000) % 1024 + 56320);
					}
					else
					{
						chars[num++] = (char)num2;
					}
					this.leftOverLength = 0;
				}
				while (byteCount > 3)
				{
					char c;
					if (this.bigEndian)
					{
						c = (char)((int)bytes[byteIndex++] << 24 | (int)bytes[byteIndex++] << 16 | (int)bytes[byteIndex++] << 8 | (int)bytes[byteIndex++]);
					}
					else
					{
						c = (char)((int)bytes[byteIndex++] | (int)bytes[byteIndex++] << 8 | (int)bytes[byteIndex++] << 16 | (int)bytes[byteIndex++] << 24);
					}
					byteCount -= 4;
					if (num >= num3)
					{
						throw new ArgumentException(Encoding._("Arg_InsufficientSpace"));
					}
					chars[num++] = c;
				}
				if (byteCount > 0)
				{
					this.leftOverLength = byteCount;
					num2 = 0;
					if (this.bigEndian)
					{
						for (int k = 0; k < byteCount; k++)
						{
							num2 += (int)bytes[byteIndex++] << 4 - byteCount--;
						}
					}
					else
					{
						for (int l = 0; l < byteCount; l++)
						{
							num2 += (int)bytes[byteIndex++] << byteCount--;
						}
					}
					this.leftOverByte = num2;
				}
				return num - charIndex;
			}
		}
	}
}
