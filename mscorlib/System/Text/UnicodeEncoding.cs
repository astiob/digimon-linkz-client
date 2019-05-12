using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Text
{
	/// <summary>Represents a UTF-16 encoding of Unicode characters.</summary>
	/// <filterpriority>1</filterpriority>
	[ComVisible(true)]
	[MonoTODO("Serialization format not compatible with .NET")]
	[Serializable]
	public class UnicodeEncoding : Encoding
	{
		internal const int UNICODE_CODE_PAGE = 1200;

		internal const int BIG_UNICODE_CODE_PAGE = 1201;

		/// <summary>Represents the Unicode version 2.0 character size in bytes. This field is a constant.</summary>
		/// <filterpriority>1</filterpriority>
		public const int CharSize = 2;

		private bool bigEndian;

		private bool byteOrderMark;

		/// <summary>Initializes a new instance of the <see cref="T:System.Text.UnicodeEncoding" /> class.</summary>
		public UnicodeEncoding() : this(false, true)
		{
			this.bigEndian = false;
			this.byteOrderMark = true;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Text.UnicodeEncoding" /> class. Parameters specify whether to use the big endian byte order and whether to provide a Unicode byte order mark.</summary>
		/// <param name="bigEndian">true to use the big endian byte order (most significant byte first), or false to use the little endian byte order (least significant byte first). </param>
		/// <param name="byteOrderMark">true to specify that a Unicode byte order mark is provided; otherwise, false. </param>
		public UnicodeEncoding(bool bigEndian, bool byteOrderMark) : this(bigEndian, byteOrderMark, false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Text.UnicodeEncoding" /> class. Parameters specify whether to use the big endian byte order, whether to provide a Unicode byte order mark, and whether to throw an exception when an invalid encoding is detected.</summary>
		/// <param name="bigEndian">true to use the big endian byte order (most significant byte first); false to use the little endian byte order (least significant byte first). </param>
		/// <param name="byteOrderMark">true to specify that a Unicode byte order mark is provided; otherwise, false. </param>
		/// <param name="throwOnInvalidBytes">true to specify that an exception should be thrown when an invalid encoding is detected; otherwise, false. </param>
		public UnicodeEncoding(bool bigEndian, bool byteOrderMark, bool throwOnInvalidBytes) : base((!bigEndian) ? 1200 : 1201)
		{
			if (throwOnInvalidBytes)
			{
				base.SetFallbackInternal(null, new DecoderExceptionFallback());
			}
			else
			{
				base.SetFallbackInternal(null, new DecoderReplacementFallback("�"));
			}
			this.bigEndian = bigEndian;
			this.byteOrderMark = byteOrderMark;
			if (bigEndian)
			{
				this.body_name = "unicodeFFFE";
				this.encoding_name = "Unicode (Big-Endian)";
				this.header_name = "unicodeFFFE";
				this.is_browser_save = false;
				this.web_name = "unicodeFFFE";
			}
			else
			{
				this.body_name = "utf-16";
				this.encoding_name = "Unicode";
				this.header_name = "utf-16";
				this.is_browser_save = true;
				this.web_name = "utf-16";
			}
			this.windows_code_page = 1200;
		}

		/// <summary>Calculates the number of bytes produced by encoding a set of characters from the specified character array.</summary>
		/// <returns>The number of bytes produced by encoding the specified characters.</returns>
		/// <param name="chars">The character array containing the set of characters to encode. </param>
		/// <param name="index">The index of the first character to encode. </param>
		/// <param name="count">The number of characters to encode. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="chars" /> is null (Nothing). </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is less than zero.-or- <paramref name="index" /> and <paramref name="count" /> do not denote a valid range in <paramref name="chars" />.-or- The resulting number of bytes is greater than the maximum number that can be returned as an integer. </exception>
		/// <exception cref="T:System.ArgumentException">Error detection is enabled, and <paramref name="chars" /> contains an invalid sequence of characters. </exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for fuller explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
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
			return count * 2;
		}

		/// <summary>Calculates the number of bytes produced by encoding the characters in the specified <see cref="T:System.String" />.</summary>
		/// <returns>The number of bytes produced by encoding the specified characters.</returns>
		/// <param name="s">The <see cref="T:System.String" /> containing the set of characters to encode. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null (Nothing). </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The resulting number of bytes is greater than the maximum number that can be returned as an integer. </exception>
		/// <exception cref="T:System.ArgumentException">Error detection is enabled, and <paramref name="s" /> contains an invalid sequence of characters. </exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for fuller explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public override int GetByteCount(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			return s.Length * 2;
		}

		/// <summary>Calculates the number of bytes produced by encoding a set of characters starting at the specified character pointer.</summary>
		/// <returns>The number of bytes produced by encoding the specified characters.</returns>
		/// <param name="chars">A pointer to the first character to encode. </param>
		/// <param name="count">The number of characters to encode. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="chars" /> is null (Nothing in Visual Basic .NET). </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="count" /> is less than zero.-or- The resulting number of bytes is greater than the maximum number that can be returned as an integer. </exception>
		/// <exception cref="T:System.ArgumentException">Error detection is enabled and <paramref name="chars" /> contains an invalid sequence of characters. </exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for fuller explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(false)]
		[CLSCompliant(false)]
		public unsafe override int GetByteCount(char* chars, int count)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			return count * 2;
		}

		/// <summary>Encodes a set of characters from the specified character array into the specified byte array.</summary>
		/// <returns>The actual number of bytes written into <paramref name="bytes" />.</returns>
		/// <param name="chars">The character array containing the set of characters to encode. </param>
		/// <param name="charIndex">The index of the first character to encode. </param>
		/// <param name="charCount">The number of characters to encode. </param>
		/// <param name="bytes">The byte array to contain the resulting sequence of bytes. </param>
		/// <param name="byteIndex">The index at which to start writing the resulting sequence of bytes. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="chars" /> is null (Nothing).-or- <paramref name="bytes" /> is null (Nothing). </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="charIndex" /> or <paramref name="charCount" /> or <paramref name="byteIndex" /> is less than zero.-or- <paramref name="charIndex" /> and <paramref name="charCount" /> do not denote a valid range in <paramref name="chars" />.-or- <paramref name="byteIndex" /> is not a valid index in <paramref name="bytes" />. </exception>
		/// <exception cref="T:System.ArgumentException">Error detection is enabled, and <paramref name="chars" /> contains an invalid sequence of characters.-or- <paramref name="bytes" /> does not have enough capacity from <paramref name="byteIndex" /> to the end of the array to accommodate the resulting bytes. </exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for fuller explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public unsafe override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
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
			if (charCount == 0)
			{
				return 0;
			}
			int byteCount = bytes.Length - byteIndex;
			if (bytes.Length == 0)
			{
				bytes = new byte[1];
			}
			fixed (char* ptr = ref (chars != null && chars.Length != 0) ? ref chars[0] : ref *null)
			{
				fixed (byte* ptr2 = ref (bytes != null && bytes.Length != 0) ? ref bytes[0] : ref *null)
				{
					return this.GetBytesInternal(ptr + charIndex, charCount, ptr2 + byteIndex, byteCount);
				}
			}
		}

		/// <summary>Encodes a set of characters from the specified <see cref="T:System.String" /> into the specified byte array.</summary>
		/// <returns>The actual number of bytes written into <paramref name="bytes" />.</returns>
		/// <param name="s">The <see cref="T:System.String" /> containing the set of characters to encode. </param>
		/// <param name="charIndex">The index of the first character to encode. </param>
		/// <param name="charCount">The number of characters to encode. </param>
		/// <param name="bytes">The byte array to contain the resulting sequence of bytes. </param>
		/// <param name="byteIndex">The index at which to start writing the resulting sequence of bytes. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null (Nothing).-or- <paramref name="bytes" /> is null (Nothing). </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="charIndex" /> or <paramref name="charCount" /> or <paramref name="byteIndex" /> is less than zero.-or- <paramref name="charIndex" /> and <paramref name="charCount" /> do not denote a valid range in <paramref name="chars" />.-or- <paramref name="byteIndex" /> is not a valid index in <paramref name="bytes" />. </exception>
		/// <exception cref="T:System.ArgumentException">Error detection is enabled, and <paramref name="s" /> contains an invalid sequence of characters.-or- <paramref name="bytes" /> does not have enough capacity from <paramref name="byteIndex" /> to the end of the array to accommodate the resulting bytes. </exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for fuller explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
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
			if (charCount == 0)
			{
				return 0;
			}
			int byteCount = bytes.Length - byteIndex;
			if (bytes.Length == 0)
			{
				bytes = new byte[1];
			}
			fixed (char* ptr = s + RuntimeHelpers.OffsetToStringData / 2)
			{
				fixed (byte* ptr2 = ref (bytes != null && bytes.Length != 0) ? ref bytes[0] : ref *null)
				{
					return this.GetBytesInternal(ptr + charIndex, charCount, ptr2 + byteIndex, byteCount);
				}
			}
		}

		/// <summary>Encodes a set of characters starting at the specified character pointer into a sequence of bytes that are stored starting at the specified byte pointer.</summary>
		/// <returns>The actual number of bytes written at the location indicated by the <paramref name="bytes" /> parameter.</returns>
		/// <param name="chars">A pointer to the first character to encode. </param>
		/// <param name="charCount">The number of characters to encode. </param>
		/// <param name="bytes">A pointer to the location at which to start writing the resulting sequence of bytes. </param>
		/// <param name="byteCount">The maximum number of bytes to write. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="chars" /> is null (Nothing).-or- <paramref name="bytes" /> is null (Nothing). </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="charCount" /> or <paramref name="byteCount" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">Error detection is enabled, and <paramref name="chars" /> contains an invalid sequence of characters.-or- <paramref name="byteCount" /> is less than the resulting number of bytes. </exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for fuller explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		[ComVisible(false)]
		public unsafe override int GetBytes(char* chars, int charCount, byte* bytes, int byteCount)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (chars == null)
			{
				throw new ArgumentNullException("chars");
			}
			if (charCount < 0)
			{
				throw new ArgumentOutOfRangeException("charCount");
			}
			if (byteCount < 0)
			{
				throw new ArgumentOutOfRangeException("byteCount");
			}
			return this.GetBytesInternal(chars, charCount, bytes, byteCount);
		}

		private unsafe int GetBytesInternal(char* chars, int charCount, byte* bytes, int byteCount)
		{
			int num = charCount * 2;
			if (byteCount < num)
			{
				throw new ArgumentException(Encoding._("Arg_InsufficientSpace"));
			}
			UnicodeEncoding.CopyChars((byte*)chars, bytes, num, this.bigEndian);
			return num;
		}

		/// <summary>Calculates the number of characters produced by decoding a sequence of bytes from the specified byte array.</summary>
		/// <returns>The number of characters produced by decoding the specified sequence of bytes.</returns>
		/// <param name="bytes">The byte array containing the sequence of bytes to decode. </param>
		/// <param name="index">The index of the first byte to decode. </param>
		/// <param name="count">The number of bytes to decode. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is null (Nothing). </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is less than zero.-or- <paramref name="index" /> and <paramref name="count" /> do not denote a valid range in <paramref name="bytes" />.-or- The resulting number of bytes is greater than the maximum number that can be returned as an integer. </exception>
		/// <exception cref="T:System.ArgumentException">Error detection is enabled, and <paramref name="bytes" /> contains an invalid sequence of bytes. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for fuller explanation)-and-<see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
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
			return count / 2;
		}

		/// <summary>Calculates the number of characters produced by decoding a sequence of bytes starting at the specified byte pointer.</summary>
		/// <returns>The number of characters produced by decoding the specified sequence of bytes.</returns>
		/// <param name="bytes">A pointer to the first byte to decode. </param>
		/// <param name="count">The number of bytes to decode. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is null (Nothing). </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="count" /> is less than zero.-or- The resulting number of bytes is greater than the maximum number that can be returned as an integer. </exception>
		/// <exception cref="T:System.ArgumentException">Error detection is enabled, and <paramref name="bytes" /> contains an invalid sequence of bytes. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for fuller explanation)-and-<see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(false)]
		[CLSCompliant(false)]
		public unsafe override int GetCharCount(byte* bytes, int count)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			return count / 2;
		}

		/// <summary>Decodes a sequence of bytes from the specified byte array into the specified character array.</summary>
		/// <returns>The actual number of characters written into <paramref name="chars" />.</returns>
		/// <param name="bytes">The byte array containing the sequence of bytes to decode. </param>
		/// <param name="byteIndex">The index of the first byte to decode. </param>
		/// <param name="byteCount">The number of bytes to decode. </param>
		/// <param name="chars">The character array to contain the resulting set of characters. </param>
		/// <param name="charIndex">The index at which to start writing the resulting set of characters. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is null (Nothing).-or- <paramref name="chars" /> is null (Nothing). </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="byteIndex" /> or <paramref name="byteCount" /> or <paramref name="charIndex" /> is less than zero.-or- <paramref name="byteindex" /> and <paramref name="byteCount" /> do not denote a valid range in <paramref name="bytes" />.-or- <paramref name="charIndex" /> is not a valid index in <paramref name="chars" />. </exception>
		/// <exception cref="T:System.ArgumentException">Error detection is enabled, and <paramref name="bytes" /> contains an invalid sequence of bytes.-or- <paramref name="chars" /> does not have enough capacity from <paramref name="charIndex" /> to the end of the array to accommodate the resulting characters. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for fuller explanation)-and-<see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public unsafe override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
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
			if (byteCount == 0)
			{
				return 0;
			}
			int charCount = chars.Length - charIndex;
			if (chars.Length == 0)
			{
				chars = new char[1];
			}
			fixed (byte* ptr = ref (bytes != null && bytes.Length != 0) ? ref bytes[0] : ref *null)
			{
				fixed (char* ptr2 = ref (chars != null && chars.Length != 0) ? ref chars[0] : ref *null)
				{
					return this.GetCharsInternal(ptr + byteIndex, byteCount, ptr2 + charIndex, charCount);
				}
			}
		}

		/// <summary>Decodes a sequence of bytes starting at the specified byte pointer into a set of characters that are stored starting at the specified character pointer.</summary>
		/// <returns>The actual number of characters written at the location indicated by the <paramref name="chars" /> parameter.</returns>
		/// <param name="bytes">A pointer to the first byte to decode. </param>
		/// <param name="byteCount">The number of bytes to decode. </param>
		/// <param name="chars">A pointer to the location at which to start writing the resulting set of characters. </param>
		/// <param name="charCount">The maximum number of characters to write. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is null (Nothing).-or- <paramref name="chars" /> is null (Nothing). </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="byteCount" /> or <paramref name="charCount" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">Error detection is enabled, and <paramref name="bytes" /> contains an invalid sequence of bytes.-or- <paramref name="charCount" /> is less than the resulting number of characters. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for fuller explanation)-and-<see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(false)]
		[CLSCompliant(false)]
		public unsafe override int GetChars(byte* bytes, int byteCount, char* chars, int charCount)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (chars == null)
			{
				throw new ArgumentNullException("chars");
			}
			if (charCount < 0)
			{
				throw new ArgumentOutOfRangeException("charCount");
			}
			if (byteCount < 0)
			{
				throw new ArgumentOutOfRangeException("byteCount");
			}
			return this.GetCharsInternal(bytes, byteCount, chars, charCount);
		}

		/// <summary>Decodes a range of bytes from a byte array into a string.</summary>
		/// <returns>A <see cref="T:System.String" /> object containing the results of decoding the specified sequence of bytes.</returns>
		/// <param name="bytes">The byte array containing the sequence of bytes to decode. </param>
		/// <param name="index">The index of the first byte to decode. </param>
		/// <param name="count">The number of bytes to decode. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is null (Nothing). </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is less than zero.-or- <paramref name="index" /> and <paramref name="count" /> do not denote a valid range in <paramref name="bytes" />. </exception>
		/// <exception cref="T:System.ArgumentException">Error detection is enabled, and <paramref name="bytes" /> contains an invalid sequence of bytes. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for fuller explanation)-and-<see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(false)]
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
			int num = count / 2;
			string text = string.InternalAllocateStr(num);
			fixed (byte* ptr = ref (bytes != null && bytes.Length != 0) ? ref bytes[0] : ref *null)
			{
				fixed (string text2 = text)
				{
					fixed (char* chars = text2 + RuntimeHelpers.OffsetToStringData / 2)
					{
						this.GetCharsInternal(ptr + index, count, chars, num);
						text2 = null;
						ptr = null;
						return text;
					}
				}
			}
		}

		private unsafe int GetCharsInternal(byte* bytes, int byteCount, char* chars, int charCount)
		{
			int num = byteCount / 2;
			if (charCount < num)
			{
				throw new ArgumentException(Encoding._("Arg_InsufficientSpace"));
			}
			UnicodeEncoding.CopyChars(bytes, (byte*)chars, byteCount, this.bigEndian);
			return num;
		}

		/// <summary>Obtains an encoder that converts a sequence of Unicode characters into a UTF-16 encoded sequence of bytes.</summary>
		/// <returns>A <see cref="T:System.Text.Encoder" /> object that converts a sequence of Unicode characters into a UTF-16 encoded sequence of bytes.</returns>
		/// <filterpriority>1</filterpriority>
		[ComVisible(false)]
		public override Encoder GetEncoder()
		{
			return base.GetEncoder();
		}

		/// <summary>Calculates the maximum number of bytes produced by encoding the specified number of characters.</summary>
		/// <returns>The maximum number of bytes produced by encoding the specified number of characters.</returns>
		/// <param name="charCount">The number of characters to encode. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="charCount" /> is less than zero.-or- The resulting number of bytes is greater than the maximum number that can be returned as an integer. </exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for fuller explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public override int GetMaxByteCount(int charCount)
		{
			if (charCount < 0)
			{
				throw new ArgumentOutOfRangeException("charCount", Encoding._("ArgRange_NonNegative"));
			}
			return charCount * 2;
		}

		/// <summary>Calculates the maximum number of characters produced by decoding the specified number of bytes.</summary>
		/// <returns>The maximum number of characters produced by decoding the specified number of bytes.</returns>
		/// <param name="byteCount">The number of bytes to decode. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="byteCount" /> is less than zero.-or- The resulting number of bytes is greater than the maximum number that can be returned as an integer. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for fuller explanation)-and-<see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public override int GetMaxCharCount(int byteCount)
		{
			if (byteCount < 0)
			{
				throw new ArgumentOutOfRangeException("byteCount", Encoding._("ArgRange_NonNegative"));
			}
			return byteCount / 2;
		}

		/// <summary>Obtains a decoder that converts a UTF-16 encoded sequence of bytes into a sequence of Unicode characters.</summary>
		/// <returns>A <see cref="T:System.Text.Decoder" /> that converts a UTF-16 encoded sequence of bytes into a sequence of Unicode characters.</returns>
		/// <filterpriority>1</filterpriority>
		public override Decoder GetDecoder()
		{
			return new UnicodeEncoding.UnicodeDecoder(this.bigEndian);
		}

		/// <summary>Returns a Unicode byte order mark encoded in UTF-16 format, if the constructor for this instance requests a byte order mark.</summary>
		/// <returns>A byte array containing the Unicode byte order mark, if the constructor for this instance requests a byte order mark. Otherwise, this method returns a byte array of length zero.</returns>
		/// <filterpriority>1</filterpriority>
		public override byte[] GetPreamble()
		{
			if (this.byteOrderMark)
			{
				byte[] array = new byte[2];
				if (this.bigEndian)
				{
					array[0] = 254;
					array[1] = byte.MaxValue;
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

		/// <summary>Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Text.UnicodeEncoding" /> object.</summary>
		/// <returns>true if <paramref name="value" /> is an instance of <see cref="T:System.Text.UnicodeEncoding" /> and is equal to the current object; otherwise, false.</returns>
		/// <param name="value">The <see cref="T:System.Object" /> to compare with the current object. </param>
		/// <filterpriority>2</filterpriority>
		public override bool Equals(object value)
		{
			UnicodeEncoding unicodeEncoding = value as UnicodeEncoding;
			return unicodeEncoding != null && (this.codePage == unicodeEncoding.codePage && this.bigEndian == unicodeEncoding.bigEndian) && this.byteOrderMark == unicodeEncoding.byteOrderMark;
		}

		/// <summary>Returns the hash code for the current instance.</summary>
		/// <returns>The hash code for the current <see cref="T:System.Text.UnicodeEncoding" /> object.</returns>
		/// <filterpriority>1</filterpriority>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		private unsafe static void CopyChars(byte* src, byte* dest, int count, bool bigEndian)
		{
			if (BitConverter.IsLittleEndian != bigEndian)
			{
				string.memcpy(dest, src, count & -2);
				return;
			}
			switch (count)
			{
			case 0:
				return;
			case 1:
				return;
			case 2:
				goto IL_220;
			case 3:
				goto IL_220;
			case 4:
				goto IL_1F1;
			case 5:
				goto IL_1F1;
			case 6:
				goto IL_1F1;
			case 7:
				goto IL_1F1;
			case 8:
				break;
			case 9:
				break;
			case 10:
				break;
			case 11:
				break;
			case 12:
				break;
			case 13:
				break;
			case 14:
				break;
			case 15:
				break;
			default:
				do
				{
					*dest = src[1];
					dest[1] = *src;
					dest[2] = src[3];
					dest[3] = src[2];
					dest[4] = src[5];
					dest[5] = src[4];
					dest[6] = src[7];
					dest[7] = src[6];
					dest[8] = src[9];
					dest[9] = src[8];
					dest[10] = src[11];
					dest[11] = src[10];
					dest[12] = src[13];
					dest[13] = src[12];
					dest[14] = src[15];
					dest[15] = src[14];
					dest += 16;
					src += 16;
					count -= 16;
				}
				while ((count & -16) != 0);
				switch (count)
				{
				case 0:
					return;
				case 1:
					return;
				case 2:
					goto IL_220;
				case 3:
					goto IL_220;
				case 4:
					goto IL_1F1;
				case 5:
					goto IL_1F1;
				case 6:
					goto IL_1F1;
				case 7:
					goto IL_1F1;
				}
				break;
			}
			*dest = src[1];
			dest[1] = *src;
			dest[2] = src[3];
			dest[3] = src[2];
			dest[4] = src[5];
			dest[5] = src[4];
			dest[6] = src[7];
			dest[7] = src[6];
			dest += 8;
			src += 8;
			if ((count & 4) == 0)
			{
				goto IL_217;
			}
			IL_1F1:
			*dest = src[1];
			dest[1] = *src;
			dest[2] = src[3];
			dest[3] = src[2];
			dest += 4;
			src += 4;
			IL_217:
			if ((count & 2) == 0)
			{
				return;
			}
			IL_220:
			*dest = src[1];
			dest[1] = *src;
		}

		private sealed class UnicodeDecoder : Decoder
		{
			private bool bigEndian;

			private int leftOverByte;

			public UnicodeDecoder(bool bigEndian)
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
					return (count + 1) / 2;
				}
				return count / 2;
			}

			public unsafe override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
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
				if (byteCount == 0)
				{
					return 0;
				}
				int num = this.leftOverByte;
				int num2;
				if (num != -1)
				{
					num2 = (byteCount + 1) / 2;
				}
				else
				{
					num2 = byteCount / 2;
				}
				if (chars.Length - charIndex < num2)
				{
					throw new ArgumentException(Encoding._("Arg_InsufficientSpace"));
				}
				if (num != -1)
				{
					if (this.bigEndian)
					{
						chars[charIndex] = (char)(num << 8 | (int)bytes[byteIndex]);
					}
					else
					{
						chars[charIndex] = (char)((int)bytes[byteIndex] << 8 | num);
					}
					charIndex++;
					byteIndex++;
					byteCount--;
				}
				if ((byteCount & -2) != 0)
				{
					fixed (byte* ptr = ref (bytes != null && bytes.Length != 0) ? ref bytes[0] : ref *null)
					{
						fixed (char* ptr2 = ref (chars != null && chars.Length != 0) ? ref chars[0] : ref *null)
						{
							UnicodeEncoding.CopyChars(ptr + byteIndex, (byte*)(ptr2 + charIndex), byteCount, this.bigEndian);
						}
					}
				}
				if ((byteCount & 1) == 0)
				{
					this.leftOverByte = -1;
				}
				else
				{
					this.leftOverByte = (int)bytes[byteCount + byteIndex - 1];
				}
				return num2;
			}
		}
	}
}
