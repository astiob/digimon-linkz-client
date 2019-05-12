using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Text
{
	/// <summary>Represents a character encoding.</summary>
	/// <filterpriority>1</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public abstract class Encoding : ICloneable
	{
		internal int codePage;

		internal int windows_code_page;

		private bool is_readonly = true;

		private DecoderFallback decoder_fallback;

		private EncoderFallback encoder_fallback;

		private static Assembly i18nAssembly;

		private static bool i18nDisabled;

		private static EncodingInfo[] encoding_infos;

		private static readonly object[] encodings = new object[]
		{
			20127,
			"ascii",
			"us_ascii",
			"us",
			"ansi_x3.4_1968",
			"ansi_x3.4_1986",
			"cp367",
			"csascii",
			"ibm367",
			"iso_ir_6",
			"iso646_us",
			"iso_646.irv:1991",
			65000,
			"utf_7",
			"csunicode11utf7",
			"unicode_1_1_utf_7",
			"unicode_2_0_utf_7",
			"x_unicode_1_1_utf_7",
			"x_unicode_2_0_utf_7",
			65001,
			"utf_8",
			"unicode_1_1_utf_8",
			"unicode_2_0_utf_8",
			"x_unicode_1_1_utf_8",
			"x_unicode_2_0_utf_8",
			1200,
			"utf_16",
			"UTF_16LE",
			"ucs_2",
			"unicode",
			"iso_10646_ucs2",
			1201,
			"unicodefffe",
			"utf_16be",
			12000,
			"utf_32",
			"UTF_32LE",
			"ucs_4",
			12001,
			"UTF_32BE",
			28591,
			"iso_8859_1",
			"latin1"
		};

		internal string body_name;

		internal string encoding_name;

		internal string header_name;

		internal bool is_mail_news_display;

		internal bool is_mail_news_save;

		internal bool is_browser_save;

		internal bool is_browser_display;

		internal string web_name;

		private static volatile Encoding asciiEncoding;

		private static volatile Encoding bigEndianEncoding;

		private static volatile Encoding defaultEncoding;

		private static volatile Encoding utf7Encoding;

		private static volatile Encoding utf8EncodingWithMarkers;

		private static volatile Encoding utf8EncodingWithoutMarkers;

		private static volatile Encoding unicodeEncoding;

		private static volatile Encoding isoLatin1Encoding;

		private static volatile Encoding utf8EncodingUnsafe;

		private static volatile Encoding utf32Encoding;

		private static volatile Encoding bigEndianUTF32Encoding;

		private static readonly object lockobj = new object();

		/// <summary>Initializes a new instance of the <see cref="T:System.Text.Encoding" /> class.</summary>
		protected Encoding()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Text.Encoding" /> class that corresponds to the specified code page.</summary>
		/// <param name="codePage">The code page identifier of the preferred encoding.-or- 0, to use the default encoding. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="codePage" /> is less than zero. </exception>
		protected Encoding(int codePage)
		{
			this.windows_code_page = codePage;
			this.codePage = codePage;
			if (codePage != 1200 && codePage != 1201 && codePage != 12000 && codePage != 12001 && codePage != 65000 && codePage != 65001)
			{
				if (codePage != 20127 && codePage != 54936)
				{
					this.decoder_fallback = DecoderFallback.ReplacementFallback;
					this.encoder_fallback = EncoderFallback.ReplacementFallback;
				}
				else
				{
					this.decoder_fallback = DecoderFallback.ReplacementFallback;
					this.encoder_fallback = EncoderFallback.ReplacementFallback;
				}
			}
			else
			{
				this.decoder_fallback = DecoderFallback.StandardSafeFallback;
				this.encoder_fallback = EncoderFallback.StandardSafeFallback;
			}
		}

		internal static string _(string arg)
		{
			return arg;
		}

		/// <summary>When overridden in a derived class, gets a value indicating whether the current encoding is read-only.</summary>
		/// <returns>true if the current <see cref="T:System.Text.Encoding" /> is read-only; otherwise, false. The default is true.</returns>
		/// <filterpriority>2</filterpriority>
		[ComVisible(false)]
		public bool IsReadOnly
		{
			get
			{
				return this.is_readonly;
			}
		}

		/// <summary>When overridden in a derived class, gets a value indicating whether the current encoding uses single-byte code points.</summary>
		/// <returns>true if the current <see cref="T:System.Text.Encoding" /> uses single-byte code points; otherwise, false.</returns>
		/// <filterpriority>2</filterpriority>
		[ComVisible(false)]
		public virtual bool IsSingleByte
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Text.DecoderFallback" /> object for the current <see cref="T:System.Text.Encoding" /> object.</summary>
		/// <returns>The <see cref="T:System.Text.DecoderFallback" /> object for the current <see cref="T:System.Text.Encoding" /> object. </returns>
		/// <exception cref="T:System.ArgumentNullException">The value in a set operation is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">A value cannot be assigned in a set operation because the current <see cref="T:System.Text.Encoding" /> object is read-only.</exception>
		/// <filterpriority>2</filterpriority>
		[ComVisible(false)]
		public DecoderFallback DecoderFallback
		{
			get
			{
				return this.decoder_fallback;
			}
			set
			{
				if (this.IsReadOnly)
				{
					throw new InvalidOperationException("This Encoding is readonly.");
				}
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				this.decoder_fallback = value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Text.EncoderFallback" /> object for the current <see cref="T:System.Text.Encoding" /> object.</summary>
		/// <returns>The <see cref="T:System.Text.EncoderFallback" /> object for the current <see cref="T:System.Text.Encoding" /> object. </returns>
		/// <exception cref="T:System.ArgumentNullException">The value in a set operation is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">A value cannot be assigned in a set operation because the current <see cref="T:System.Text.Encoding" /> object is read-only.</exception>
		/// <filterpriority>2</filterpriority>
		[ComVisible(false)]
		public EncoderFallback EncoderFallback
		{
			get
			{
				return this.encoder_fallback;
			}
			set
			{
				if (this.IsReadOnly)
				{
					throw new InvalidOperationException("This Encoding is readonly.");
				}
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				this.encoder_fallback = value;
			}
		}

		internal void SetFallbackInternal(EncoderFallback e, DecoderFallback d)
		{
			if (e != null)
			{
				this.encoder_fallback = e;
			}
			if (d != null)
			{
				this.decoder_fallback = d;
			}
		}

		/// <summary>Converts an entire byte array from one encoding to another.</summary>
		/// <returns>An array of type <see cref="T:System.Byte" /> containing the results of converting <paramref name="bytes" /> from <paramref name="srcEncoding" /> to <paramref name="dstEncoding" />.</returns>
		/// <param name="srcEncoding">The encoding format of <paramref name="bytes" />. </param>
		/// <param name="dstEncoding">The target encoding format. </param>
		/// <param name="bytes"></param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="srcEncoding" /> is null.-or- <paramref name="dstEncoding" /> is null.-or- <paramref name="bytes" /> is null. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-srcEncoding.<see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-dstEncoding.<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public static byte[] Convert(Encoding srcEncoding, Encoding dstEncoding, byte[] bytes)
		{
			if (srcEncoding == null)
			{
				throw new ArgumentNullException("srcEncoding");
			}
			if (dstEncoding == null)
			{
				throw new ArgumentNullException("dstEncoding");
			}
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			return dstEncoding.GetBytes(srcEncoding.GetChars(bytes, 0, bytes.Length));
		}

		/// <summary>Converts a range of bytes in a byte array from one encoding to another.</summary>
		/// <returns>An array of type <see cref="T:System.Byte" /> containing the result of converting a range of bytes in <paramref name="bytes" /> from <paramref name="srcEncoding" /> to <paramref name="dstEncoding" />.</returns>
		/// <param name="srcEncoding">The encoding of the source array, <paramref name="bytes" />. </param>
		/// <param name="dstEncoding">The encoding of the output array. </param>
		/// <param name="bytes">The array of bytes to convert. </param>
		/// <param name="index">The index of the first element of <paramref name="bytes" /> to convert. </param>
		/// <param name="count">The number of bytes to convert. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="srcEncoding" /> is null.-or- <paramref name="dstEncoding" /> is null.-or- <paramref name="bytes" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> and <paramref name="count" /> do not specify a valid range in the byte array. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-srcEncoding.<see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-dstEncoding.<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public static byte[] Convert(Encoding srcEncoding, Encoding dstEncoding, byte[] bytes, int index, int count)
		{
			if (srcEncoding == null)
			{
				throw new ArgumentNullException("srcEncoding");
			}
			if (dstEncoding == null)
			{
				throw new ArgumentNullException("dstEncoding");
			}
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (index < 0 || index > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("index", Encoding._("ArgRange_Array"));
			}
			if (count < 0 || bytes.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("count", Encoding._("ArgRange_Array"));
			}
			return dstEncoding.GetBytes(srcEncoding.GetChars(bytes, index, count));
		}

		/// <summary>Determines whether the specified <see cref="T:System.Object" /> is equal to the current instance.</summary>
		/// <returns>true if <paramref name="value" /> is an instance of <see cref="T:System.Text.Encoding" /> and is equal to the current instance; otherwise, false. </returns>
		/// <param name="value">The <see cref="T:System.Object" /> to compare with the current instance. </param>
		/// <filterpriority>2</filterpriority>
		public override bool Equals(object value)
		{
			Encoding encoding = value as Encoding;
			return encoding != null && (this.codePage == encoding.codePage && this.DecoderFallback.Equals(encoding.DecoderFallback)) && this.EncoderFallback.Equals(encoding.EncoderFallback);
		}

		/// <summary>When overridden in a derived class, calculates the number of bytes produced by encoding a set of characters from the specified character array.</summary>
		/// <returns>The number of bytes produced by encoding the specified characters.</returns>
		/// <param name="chars">The character array containing the set of characters to encode. </param>
		/// <param name="index">The index of the first character to encode. </param>
		/// <param name="count">The number of characters to encode. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="chars" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is less than zero.-or- <paramref name="index" /> and <paramref name="count" /> do not denote a valid range in <paramref name="chars" />. </exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public abstract int GetByteCount(char[] chars, int index, int count);

		/// <summary>When overridden in a derived class, calculates the number of bytes produced by encoding the characters in the specified string.</summary>
		/// <returns>The number of bytes produced by encoding the specified characters.</returns>
		/// <param name="s">The string containing the set of characters to encode. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public unsafe virtual int GetByteCount(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (s.Length == 0)
			{
				return 0;
			}
			fixed (char* chars = s + RuntimeHelpers.OffsetToStringData / 2)
			{
				return this.GetByteCount(chars, s.Length);
			}
		}

		/// <summary>When overridden in a derived class, calculates the number of bytes produced by encoding all the characters in the specified character array.</summary>
		/// <returns>The number of bytes produced by encoding all the characters in the specified character array.</returns>
		/// <param name="chars">The character array containing the characters to encode. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="chars" /> is null. </exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public virtual int GetByteCount(char[] chars)
		{
			if (chars != null)
			{
				return this.GetByteCount(chars, 0, chars.Length);
			}
			throw new ArgumentNullException("chars");
		}

		/// <summary>When overridden in a derived class, encodes a set of characters from the specified character array into the specified byte array.</summary>
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
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="bytes" /> does not have enough capacity from <paramref name="byteIndex" /> to the end of the array to accommodate the resulting bytes. </exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public abstract int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex);

		/// <summary>When overridden in a derived class, encodes a set of characters from the specified string into the specified byte array.</summary>
		/// <returns>The actual number of bytes written into <paramref name="bytes" />.</returns>
		/// <param name="s">The string containing the set of characters to encode. </param>
		/// <param name="charIndex">The index of the first character to encode. </param>
		/// <param name="charCount">The number of characters to encode. </param>
		/// <param name="bytes">The byte array to contain the resulting sequence of bytes. </param>
		/// <param name="byteIndex">The index at which to start writing the resulting sequence of bytes. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null.-or- <paramref name="bytes" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="charIndex" /> or <paramref name="charCount" /> or <paramref name="byteIndex" /> is less than zero.-or- <paramref name="charIndex" /> and <paramref name="charCount" /> do not denote a valid range in <paramref name="chars" />.-or- <paramref name="byteIndex" /> is not a valid index in <paramref name="bytes" />. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="bytes" /> does not have enough capacity from <paramref name="byteIndex" /> to the end of the array to accommodate the resulting bytes. </exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public unsafe virtual int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (charIndex < 0 || charIndex > s.Length)
			{
				throw new ArgumentOutOfRangeException("charIndex", Encoding._("ArgRange_Array"));
			}
			if (charCount < 0 || charIndex > s.Length - charCount)
			{
				throw new ArgumentOutOfRangeException("charCount", Encoding._("ArgRange_Array"));
			}
			if (byteIndex < 0 || byteIndex > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("byteIndex", Encoding._("ArgRange_Array"));
			}
			if (charCount == 0 || bytes.Length == byteIndex)
			{
				return 0;
			}
			fixed (char* ptr = s + RuntimeHelpers.OffsetToStringData / 2)
			{
				fixed (byte* ptr2 = ref (bytes != null && bytes.Length != 0) ? ref bytes[0] : ref *null)
				{
					return this.GetBytes(ptr + charIndex, charCount, ptr2 + byteIndex, bytes.Length - byteIndex);
				}
			}
		}

		/// <summary>When overridden in a derived class, encodes all the characters in the specified string into a sequence of bytes.</summary>
		/// <returns>A byte array containing the results of encoding the specified set of characters.</returns>
		/// <param name="s"></param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public unsafe virtual byte[] GetBytes(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (s.Length == 0)
			{
				return new byte[0];
			}
			int byteCount = this.GetByteCount(s);
			if (byteCount == 0)
			{
				return new byte[0];
			}
			fixed (char* chars = s + RuntimeHelpers.OffsetToStringData / 2)
			{
				byte[] array = new byte[byteCount];
				fixed (byte* bytes = ref (array != null && array.Length != 0) ? ref array[0] : ref *null)
				{
					this.GetBytes(chars, s.Length, bytes, byteCount);
					return array;
				}
			}
		}

		/// <summary>When overridden in a derived class, encodes a set of characters from the specified character array into a sequence of bytes.</summary>
		/// <returns>A byte array containing the results of encoding the specified set of characters.</returns>
		/// <param name="chars">The character array containing the set of characters to encode. </param>
		/// <param name="index">The index of the first character to encode. </param>
		/// <param name="count">The number of characters to encode. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="chars" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is less than zero.-or- <paramref name="index" /> and <paramref name="count" /> do not denote a valid range in <paramref name="chars" />. </exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public virtual byte[] GetBytes(char[] chars, int index, int count)
		{
			int byteCount = this.GetByteCount(chars, index, count);
			byte[] array = new byte[byteCount];
			this.GetBytes(chars, index, count, array, 0);
			return array;
		}

		/// <summary>When overridden in a derived class, encodes all the characters in the specified character array into a sequence of bytes.</summary>
		/// <returns>A byte array containing the results of encoding the specified set of characters.</returns>
		/// <param name="chars">The character array containing the characters to encode. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="chars" /> is null. </exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public virtual byte[] GetBytes(char[] chars)
		{
			int byteCount = this.GetByteCount(chars, 0, chars.Length);
			byte[] array = new byte[byteCount];
			this.GetBytes(chars, 0, chars.Length, array, 0);
			return array;
		}

		/// <summary>When overridden in a derived class, calculates the number of characters produced by decoding a sequence of bytes from the specified byte array.</summary>
		/// <returns>The number of characters produced by decoding the specified sequence of bytes.</returns>
		/// <param name="bytes">The byte array containing the sequence of bytes to decode. </param>
		/// <param name="index">The index of the first byte to decode. </param>
		/// <param name="count">The number of bytes to decode. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is less than zero.-or- <paramref name="index" /> and <paramref name="count" /> do not denote a valid range in <paramref name="bytes" />. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public abstract int GetCharCount(byte[] bytes, int index, int count);

		/// <summary>When overridden in a derived class, calculates the number of characters produced by decoding all the bytes in the specified byte array.</summary>
		/// <returns>The number of characters produced by decoding the specified sequence of bytes.</returns>
		/// <param name="bytes">The byte array containing the sequence of bytes to decode. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is null. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public virtual int GetCharCount(byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			return this.GetCharCount(bytes, 0, bytes.Length);
		}

		/// <summary>When overridden in a derived class, decodes a sequence of bytes from the specified byte array into the specified character array.</summary>
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
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="chars" /> does not have enough capacity from <paramref name="charIndex" /> to the end of the array to accommodate the resulting characters. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public abstract int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex);

		/// <summary>When overridden in a derived class, decodes a sequence of bytes from the specified byte array into a set of characters.</summary>
		/// <returns>A character array containing the results of decoding the specified sequence of bytes.</returns>
		/// <param name="bytes">The byte array containing the sequence of bytes to decode. </param>
		/// <param name="index">The index of the first byte to decode. </param>
		/// <param name="count">The number of bytes to decode. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is less than zero.-or- <paramref name="index" /> and <paramref name="count" /> do not denote a valid range in <paramref name="bytes" />. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public virtual char[] GetChars(byte[] bytes, int index, int count)
		{
			int charCount = this.GetCharCount(bytes, index, count);
			char[] array = new char[charCount];
			this.GetChars(bytes, index, count, array, 0);
			return array;
		}

		/// <summary>When overridden in a derived class, decodes all the bytes in the specified byte array into a set of characters.</summary>
		/// <returns>A character array containing the results of decoding the specified sequence of bytes.</returns>
		/// <param name="bytes">The byte array containing the sequence of bytes to decode. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is null. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public virtual char[] GetChars(byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			int charCount = this.GetCharCount(bytes, 0, bytes.Length);
			char[] array = new char[charCount];
			this.GetChars(bytes, 0, bytes.Length, array, 0);
			return array;
		}

		/// <summary>When overridden in a derived class, obtains a decoder that converts an encoded sequence of bytes into a sequence of characters.</summary>
		/// <returns>A <see cref="T:System.Text.Decoder" /> that converts an encoded sequence of bytes into a sequence of characters.</returns>
		/// <filterpriority>1</filterpriority>
		public virtual Decoder GetDecoder()
		{
			return new Encoding.ForwardingDecoder(this);
		}

		/// <summary>When overridden in a derived class, obtains an encoder that converts a sequence of Unicode characters into an encoded sequence of bytes.</summary>
		/// <returns>A <see cref="T:System.Text.Encoder" /> that converts a sequence of Unicode characters into an encoded sequence of bytes.</returns>
		/// <filterpriority>1</filterpriority>
		public virtual Encoder GetEncoder()
		{
			return new Encoding.ForwardingEncoder(this);
		}

		private static object InvokeI18N(string name, params object[] args)
		{
			object obj = Encoding.lockobj;
			object result;
			lock (obj)
			{
				if (Encoding.i18nDisabled)
				{
					result = null;
				}
				else
				{
					if (Encoding.i18nAssembly == null)
					{
						try
						{
							try
							{
								Encoding.i18nAssembly = Assembly.Load("I18N, Version=2.0.0.0, Culture=neutral, PublicKeyToken=0738eb9f132ed756");
							}
							catch (NotImplementedException)
							{
								Encoding.i18nDisabled = true;
								return null;
							}
							if (Encoding.i18nAssembly == null)
							{
								return null;
							}
						}
						catch (SystemException)
						{
							return null;
						}
					}
					Type type;
					try
					{
						type = Encoding.i18nAssembly.GetType("I18N.Common.Manager");
					}
					catch (NotImplementedException)
					{
						Encoding.i18nDisabled = true;
						return null;
					}
					if (type == null)
					{
						result = null;
					}
					else
					{
						object obj2;
						try
						{
							obj2 = type.InvokeMember("PrimaryManager", BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty, null, null, null, null, null, null);
							if (obj2 == null)
							{
								return null;
							}
						}
						catch (MissingMethodException)
						{
							return null;
						}
						catch (SecurityException)
						{
							return null;
						}
						catch (NotImplementedException)
						{
							Encoding.i18nDisabled = true;
							return null;
						}
						try
						{
							result = type.InvokeMember(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, null, obj2, args, null, null, null);
						}
						catch (MissingMethodException)
						{
							result = null;
						}
						catch (SecurityException)
						{
							result = null;
						}
					}
				}
			}
			return result;
		}

		/// <summary>Returns the encoding associated with the specified code page identifier.</summary>
		/// <returns>The <see cref="T:System.Text.Encoding" /> associated with the specified code page.</returns>
		/// <param name="codepage">The code page identifier of the preferred encoding.-or- 0, to use the default encoding. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="codepage" /> is less than zero or greater than 65535. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="codepage" /> is not supported by the underlying platform. </exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="codepage" /> is not supported by the underlying platform. </exception>
		/// <filterpriority>1</filterpriority>
		public static Encoding GetEncoding(int codepage)
		{
			if (codepage < 0 || codepage > 65535)
			{
				throw new ArgumentOutOfRangeException("codepage", "Valid values are between 0 and 65535, inclusive.");
			}
			int num = codepage;
			if (num == 1200)
			{
				return Encoding.Unicode;
			}
			if (num == 1201)
			{
				return Encoding.BigEndianUnicode;
			}
			if (num == 12000)
			{
				return Encoding.UTF32;
			}
			if (num == 12001)
			{
				return Encoding.BigEndianUTF32;
			}
			if (num == 65000)
			{
				return Encoding.UTF7;
			}
			if (num == 65001)
			{
				return Encoding.UTF8;
			}
			if (num == 0)
			{
				return Encoding.Default;
			}
			if (num == 20127)
			{
				return Encoding.ASCII;
			}
			if (num == 28591)
			{
				return Encoding.ISOLatin1;
			}
			Encoding encoding = (Encoding)Encoding.InvokeI18N("GetEncoding", new object[]
			{
				codepage
			});
			if (encoding != null)
			{
				encoding.is_readonly = true;
				return encoding;
			}
			string text = "System.Text.CP" + codepage.ToString();
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			Type type = executingAssembly.GetType(text);
			if (type != null)
			{
				encoding = (Encoding)Activator.CreateInstance(type);
				encoding.is_readonly = true;
				return encoding;
			}
			type = Type.GetType(text);
			if (type != null)
			{
				encoding = (Encoding)Activator.CreateInstance(type);
				encoding.is_readonly = true;
				return encoding;
			}
			throw new NotSupportedException(string.Format("CodePage {0} not supported", codepage.ToString()));
		}

		/// <summary>When overridden in a derived class, creates a shallow copy of the current <see cref="T:System.Text.Encoding" /> object.</summary>
		/// <returns>A copy of the current <see cref="T:System.Text.Encoding" /> object.</returns>
		/// <filterpriority>2</filterpriority>
		[ComVisible(false)]
		public virtual object Clone()
		{
			Encoding encoding = (Encoding)base.MemberwiseClone();
			encoding.is_readonly = false;
			return encoding;
		}

		/// <summary>Returns the encoding associated with the specified code page identifier. Parameters specify an error handler for characters that cannot be encoded and byte sequences that cannot be decoded.</summary>
		/// <returns>The <see cref="T:System.Text.Encoding" /> object associated with the specified code page.</returns>
		/// <param name="codepage">The code page identifier of the preferred encoding.-or- 0, to use the default encoding. </param>
		/// <param name="encoderFallback">A <see cref="T:System.Text.EncoderFallback" /> object that provides an error handling procedure when a character cannot be encoded with the current encoding. </param>
		/// <param name="decoderFallback">A <see cref="T:System.Text.DecoderFallback" /> object that provides an error handling procedure when a byte sequence cannot be decoded with the current encoding. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="codepage" /> is less than zero or greater than 65535. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="codepage" /> is not supported by the underlying platform. </exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="codepage" /> is not supported by the underlying platform. </exception>
		/// <filterpriority>1</filterpriority>
		public static Encoding GetEncoding(int codepage, EncoderFallback encoderFallback, DecoderFallback decoderFallback)
		{
			if (encoderFallback == null)
			{
				throw new ArgumentNullException("encoderFallback");
			}
			if (decoderFallback == null)
			{
				throw new ArgumentNullException("decoderFallback");
			}
			Encoding encoding = Encoding.GetEncoding(codepage).Clone() as Encoding;
			encoding.is_readonly = false;
			encoding.encoder_fallback = encoderFallback;
			encoding.decoder_fallback = decoderFallback;
			return encoding;
		}

		/// <summary>Returns the encoding associated with the specified code page name. Parameters specify an error handler for characters that cannot be encoded and byte sequences that cannot be decoded.</summary>
		/// <returns>The <see cref="T:System.Text.Encoding" /> object associated with the specified code page.</returns>
		/// <param name="name">The code page name of the preferred encoding. </param>
		/// <param name="encoderFallback">A <see cref="T:System.Text.EncoderFallback" /> object that provides an error handling procedure when a character cannot be encoded with the current encoding. </param>
		/// <param name="decoderFallback">A <see cref="T:System.Text.DecoderFallback" /> object that provides an error handling procedure when a byte sequence cannot be decoded with the current encoding. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> is not a valid code page name.-or- The code page indicated by <paramref name="name" /> is not supported by the underlying platform. </exception>
		/// <filterpriority>1</filterpriority>
		public static Encoding GetEncoding(string name, EncoderFallback encoderFallback, DecoderFallback decoderFallback)
		{
			if (encoderFallback == null)
			{
				throw new ArgumentNullException("encoderFallback");
			}
			if (decoderFallback == null)
			{
				throw new ArgumentNullException("decoderFallback");
			}
			Encoding encoding = Encoding.GetEncoding(name).Clone() as Encoding;
			encoding.is_readonly = false;
			encoding.encoder_fallback = encoderFallback;
			encoding.decoder_fallback = decoderFallback;
			return encoding;
		}

		/// <summary>Returns an array containing all encodings.</summary>
		/// <returns>An array of type <see cref="T:System.Text.EncodingInfo" /> containing all encodings.</returns>
		/// <filterpriority>1</filterpriority>
		public static EncodingInfo[] GetEncodings()
		{
			if (Encoding.encoding_infos == null)
			{
				int[] array = new int[]
				{
					37,
					437,
					500,
					708,
					850,
					852,
					855,
					857,
					858,
					860,
					861,
					862,
					863,
					864,
					865,
					866,
					869,
					870,
					874,
					875,
					932,
					936,
					949,
					950,
					1026,
					1047,
					1140,
					1141,
					1142,
					1143,
					1144,
					1145,
					1146,
					1147,
					1148,
					1149,
					1200,
					1201,
					1250,
					1251,
					1252,
					1253,
					1254,
					1255,
					1256,
					1257,
					1258,
					10000,
					10079,
					12000,
					12001,
					20127,
					20273,
					20277,
					20278,
					20280,
					20284,
					20285,
					20290,
					20297,
					20420,
					20424,
					20866,
					20871,
					21025,
					21866,
					28591,
					28592,
					28593,
					28594,
					28595,
					28596,
					28597,
					28598,
					28599,
					28605,
					38598,
					50220,
					50221,
					50222,
					51932,
					51949,
					54936,
					57002,
					57003,
					57004,
					57005,
					57006,
					57007,
					57008,
					57009,
					57010,
					57011,
					65000,
					65001
				};
				Encoding.encoding_infos = new EncodingInfo[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					Encoding.encoding_infos[i] = new EncodingInfo(array[i]);
				}
			}
			return Encoding.encoding_infos;
		}

		/// <summary>Gets a value indicating whether the current encoding is always normalized, using the default normalization form.</summary>
		/// <returns>true if the current <see cref="T:System.Text.Encoding" /> is always normalized; otherwise, false. The default is false.</returns>
		/// <filterpriority>2</filterpriority>
		[ComVisible(false)]
		public bool IsAlwaysNormalized()
		{
			return this.IsAlwaysNormalized(NormalizationForm.FormC);
		}

		/// <summary>When overridden in a derived class, gets a value indicating whether the current encoding is always normalized, using the specified normalization form.</summary>
		/// <returns>true if the current <see cref="T:System.Text.Encoding" /> object is always normalized using the specified <see cref="T:System.Text.NormalizationForm" /> value; otherwise, false. The default is false.</returns>
		/// <param name="form">One of the <see cref="T:System.Text.NormalizationForm" /> values. </param>
		/// <filterpriority>2</filterpriority>
		[ComVisible(false)]
		public virtual bool IsAlwaysNormalized(NormalizationForm form)
		{
			return form == NormalizationForm.FormC && this is ASCIIEncoding;
		}

		/// <summary>Returns an encoding associated with the specified code page name.</summary>
		/// <returns>The <see cref="T:System.Text.Encoding" /> associated with the specified code page.</returns>
		/// <param name="name">The code page name of the preferred encoding. Any value returned by <see cref="P:System.Text.Encoding.WebName" /> is a valid input. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> is not a valid code page name.-or- The code page indicated by <paramref name="name" /> is not supported by the underlying platform. </exception>
		/// <filterpriority>1</filterpriority>
		public static Encoding GetEncoding(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			string text = name.ToLowerInvariant().Replace('-', '_');
			int codepage = 0;
			for (int i = 0; i < Encoding.encodings.Length; i++)
			{
				object obj = Encoding.encodings[i];
				if (obj is int)
				{
					codepage = (int)obj;
				}
				else if (text == (string)Encoding.encodings[i])
				{
					return Encoding.GetEncoding(codepage);
				}
			}
			Encoding encoding = (Encoding)Encoding.InvokeI18N("GetEncoding", new object[]
			{
				name
			});
			if (encoding != null)
			{
				return encoding;
			}
			string text2 = "System.Text.ENC" + text;
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			Type type = executingAssembly.GetType(text2);
			if (type != null)
			{
				return (Encoding)Activator.CreateInstance(type);
			}
			type = Type.GetType(text2);
			if (type != null)
			{
				return (Encoding)Activator.CreateInstance(type);
			}
			throw new ArgumentException(string.Format("Encoding name '{0}' not supported", name), "name");
		}

		/// <summary>Returns the hash code for the current instance.</summary>
		/// <returns>The hash code for the current instance.</returns>
		/// <filterpriority>1</filterpriority>
		public override int GetHashCode()
		{
			return this.DecoderFallback.GetHashCode() << 24 + this.EncoderFallback.GetHashCode() << 16 + this.codePage;
		}

		/// <summary>When overridden in a derived class, calculates the maximum number of bytes produced by encoding the specified number of characters.</summary>
		/// <returns>The maximum number of bytes produced by encoding the specified number of characters.</returns>
		/// <param name="charCount">The number of characters to encode. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="charCount" /> is less than zero. </exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public abstract int GetMaxByteCount(int charCount);

		/// <summary>When overridden in a derived class, calculates the maximum number of characters produced by decoding the specified number of bytes.</summary>
		/// <returns>The maximum number of characters produced by decoding the specified number of bytes.</returns>
		/// <param name="byteCount">The number of bytes to decode. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="byteCount" /> is less than zero. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public abstract int GetMaxCharCount(int byteCount);

		/// <summary>When overridden in a derived class, returns a sequence of bytes that specifies the encoding used.</summary>
		/// <returns>A byte array containing a sequence of bytes that specifies the encoding used.-or- A byte array of length zero, if a preamble is not required.</returns>
		/// <filterpriority>1</filterpriority>
		public virtual byte[] GetPreamble()
		{
			return new byte[0];
		}

		/// <summary>When overridden in a derived class, decodes a sequence of bytes from the specified byte array into a string.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the results of decoding the specified sequence of bytes.</returns>
		/// <param name="bytes">The byte array containing the sequence of bytes to decode. </param>
		/// <param name="index">The index of the first byte to decode. </param>
		/// <param name="count">The number of bytes to decode. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is less than zero.-or- <paramref name="index" /> and <paramref name="count" /> do not denote a valid range in <paramref name="bytes" />. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public virtual string GetString(byte[] bytes, int index, int count)
		{
			return new string(this.GetChars(bytes, index, count));
		}

		/// <summary>When overridden in a derived class, decodes all the bytes in the specified byte array into a string.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the results of decoding the specified sequence of bytes.</returns>
		/// <param name="bytes">The byte array containing the sequence of bytes to decode. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is null. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		public virtual string GetString(byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			return this.GetString(bytes, 0, bytes.Length);
		}

		/// <summary>When overridden in a derived class, gets a name for the current encoding that can be used with mail agent body tags.</summary>
		/// <returns>A name for the current <see cref="T:System.Text.Encoding" /> that can be used with mail agent body tags.-or- An empty string (""), if the current <see cref="T:System.Text.Encoding" /> cannot be used.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual string BodyName
		{
			get
			{
				return this.body_name;
			}
		}

		/// <summary>When overridden in a derived class, gets the code page identifier of the current <see cref="T:System.Text.Encoding" />.</summary>
		/// <returns>The code page identifier of the current <see cref="T:System.Text.Encoding" />.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual int CodePage
		{
			get
			{
				return this.codePage;
			}
		}

		/// <summary>When overridden in a derived class, gets the human-readable description of the current encoding.</summary>
		/// <returns>The human-readable description of the current <see cref="T:System.Text.Encoding" />.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual string EncodingName
		{
			get
			{
				return this.encoding_name;
			}
		}

		/// <summary>When overridden in a derived class, gets a name for the current encoding that can be used with mail agent header tags.</summary>
		/// <returns>A name for the current <see cref="T:System.Text.Encoding" /> to use with mail agent header tags.-or- An empty string (""), if the current <see cref="T:System.Text.Encoding" /> cannot be used.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual string HeaderName
		{
			get
			{
				return this.header_name;
			}
		}

		/// <summary>When overridden in a derived class, gets a value indicating whether the current encoding can be used by browser clients for displaying content.</summary>
		/// <returns>true if the current <see cref="T:System.Text.Encoding" /> can be used by browser clients for displaying content; otherwise, false.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual bool IsBrowserDisplay
		{
			get
			{
				return this.is_browser_display;
			}
		}

		/// <summary>When overridden in a derived class, gets a value indicating whether the current encoding can be used by browser clients for saving content.</summary>
		/// <returns>true if the current <see cref="T:System.Text.Encoding" /> can be used by browser clients for saving content; otherwise, false.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual bool IsBrowserSave
		{
			get
			{
				return this.is_browser_save;
			}
		}

		/// <summary>When overridden in a derived class, gets a value indicating whether the current encoding can be used by mail and news clients for displaying content.</summary>
		/// <returns>true if the current <see cref="T:System.Text.Encoding" /> can be used by mail and news clients for displaying content; otherwise, false.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual bool IsMailNewsDisplay
		{
			get
			{
				return this.is_mail_news_display;
			}
		}

		/// <summary>When overridden in a derived class, gets a value indicating whether the current encoding can be used by mail and news clients for saving content.</summary>
		/// <returns>true if the current <see cref="T:System.Text.Encoding" /> can be used by mail and news clients for saving content; otherwise, false.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual bool IsMailNewsSave
		{
			get
			{
				return this.is_mail_news_save;
			}
		}

		/// <summary>When overridden in a derived class, gets the name registered with the Internet Assigned Numbers Authority (IANA) for the current encoding.</summary>
		/// <returns>The IANA name for the current <see cref="T:System.Text.Encoding" />.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual string WebName
		{
			get
			{
				return this.web_name;
			}
		}

		/// <summary>When overridden in a derived class, gets the Windows operating system code page that most closely corresponds to the current encoding.</summary>
		/// <returns>The Windows operating system code page that most closely corresponds to the current <see cref="T:System.Text.Encoding" />.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual int WindowsCodePage
		{
			get
			{
				return this.windows_code_page;
			}
		}

		/// <summary>Gets an encoding for the ASCII (7-bit) character set.</summary>
		/// <returns>An encoding for the ASCII (7-bit) character set.</returns>
		/// <filterpriority>1</filterpriority>
		public static Encoding ASCII
		{
			get
			{
				if (Encoding.asciiEncoding == null)
				{
					object obj = Encoding.lockobj;
					lock (obj)
					{
						if (Encoding.asciiEncoding == null)
						{
							Encoding.asciiEncoding = new ASCIIEncoding();
						}
					}
				}
				return Encoding.asciiEncoding;
			}
		}

		/// <summary>Gets an encoding for the UTF-16 format using the big endian byte order.</summary>
		/// <returns>An encoding object for the UTF-16 format using the big endian byte order.</returns>
		/// <filterpriority>1</filterpriority>
		public static Encoding BigEndianUnicode
		{
			get
			{
				if (Encoding.bigEndianEncoding == null)
				{
					object obj = Encoding.lockobj;
					lock (obj)
					{
						if (Encoding.bigEndianEncoding == null)
						{
							Encoding.bigEndianEncoding = new UnicodeEncoding(true, true);
						}
					}
				}
				return Encoding.bigEndianEncoding;
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern string InternalCodePage(ref int code_page);

		/// <summary>Gets an encoding for the operating system's current ANSI code page.</summary>
		/// <returns>An encoding for the operating system's current ANSI code page.</returns>
		/// <filterpriority>1</filterpriority>
		public static Encoding Default
		{
			get
			{
				if (Encoding.defaultEncoding == null)
				{
					object obj = Encoding.lockobj;
					lock (obj)
					{
						if (Encoding.defaultEncoding == null)
						{
							int num = 1;
							string name = Encoding.InternalCodePage(ref num);
							try
							{
								if (num == -1)
								{
									Encoding.defaultEncoding = Encoding.GetEncoding(name);
								}
								else
								{
									num &= 268435455;
									switch (num)
									{
									case 1:
										num = 20127;
										break;
									case 2:
										num = 65000;
										break;
									case 3:
										num = 65001;
										break;
									case 4:
										num = 1200;
										break;
									case 5:
										num = 1201;
										break;
									case 6:
										num = 28591;
										break;
									}
									Encoding.defaultEncoding = Encoding.GetEncoding(num);
								}
							}
							catch (NotSupportedException)
							{
								Encoding.defaultEncoding = Encoding.UTF8Unmarked;
							}
							catch (ArgumentException)
							{
								Encoding.defaultEncoding = Encoding.UTF8Unmarked;
							}
							Encoding.defaultEncoding.is_readonly = true;
						}
					}
				}
				return Encoding.defaultEncoding;
			}
		}

		private static Encoding ISOLatin1
		{
			get
			{
				if (Encoding.isoLatin1Encoding == null)
				{
					object obj = Encoding.lockobj;
					lock (obj)
					{
						if (Encoding.isoLatin1Encoding == null)
						{
							Encoding.isoLatin1Encoding = new Latin1Encoding();
						}
					}
				}
				return Encoding.isoLatin1Encoding;
			}
		}

		/// <summary>Gets an encoding for the UTF-7 format.</summary>
		/// <returns>A <see cref="T:System.Text.Encoding" /> for the UTF-7 format.</returns>
		/// <filterpriority>1</filterpriority>
		public static Encoding UTF7
		{
			get
			{
				if (Encoding.utf7Encoding == null)
				{
					object obj = Encoding.lockobj;
					lock (obj)
					{
						if (Encoding.utf7Encoding == null)
						{
							Encoding.utf7Encoding = new UTF7Encoding();
						}
					}
				}
				return Encoding.utf7Encoding;
			}
		}

		/// <summary>Gets an encoding for the UTF-8 format.</summary>
		/// <returns>An encoding for the UTF-8 format.</returns>
		/// <filterpriority>1</filterpriority>
		public static Encoding UTF8
		{
			get
			{
				if (Encoding.utf8EncodingWithMarkers == null)
				{
					object obj = Encoding.lockobj;
					lock (obj)
					{
						if (Encoding.utf8EncodingWithMarkers == null)
						{
							Encoding.utf8EncodingWithMarkers = new UTF8Encoding(true);
						}
					}
				}
				return Encoding.utf8EncodingWithMarkers;
			}
		}

		internal static Encoding UTF8Unmarked
		{
			get
			{
				if (Encoding.utf8EncodingWithoutMarkers == null)
				{
					object obj = Encoding.lockobj;
					lock (obj)
					{
						if (Encoding.utf8EncodingWithoutMarkers == null)
						{
							Encoding.utf8EncodingWithoutMarkers = new UTF8Encoding(false, false);
						}
					}
				}
				return Encoding.utf8EncodingWithoutMarkers;
			}
		}

		internal static Encoding UTF8UnmarkedUnsafe
		{
			get
			{
				if (Encoding.utf8EncodingUnsafe == null)
				{
					object obj = Encoding.lockobj;
					lock (obj)
					{
						if (Encoding.utf8EncodingUnsafe == null)
						{
							Encoding.utf8EncodingUnsafe = new UTF8Encoding(false, false);
							Encoding.utf8EncodingUnsafe.is_readonly = false;
							Encoding.utf8EncodingUnsafe.DecoderFallback = new DecoderReplacementFallback(string.Empty);
							Encoding.utf8EncodingUnsafe.is_readonly = true;
						}
					}
				}
				return Encoding.utf8EncodingUnsafe;
			}
		}

		/// <summary>Gets an encoding for the UTF-16 format using the little endian byte order.</summary>
		/// <returns>An encoding for the UTF-16 format using the little endian byte order.</returns>
		/// <filterpriority>1</filterpriority>
		public static Encoding Unicode
		{
			get
			{
				if (Encoding.unicodeEncoding == null)
				{
					object obj = Encoding.lockobj;
					lock (obj)
					{
						if (Encoding.unicodeEncoding == null)
						{
							Encoding.unicodeEncoding = new UnicodeEncoding(false, true);
						}
					}
				}
				return Encoding.unicodeEncoding;
			}
		}

		/// <summary>Gets an encoding for the UTF-32 format using the little endian byte order.</summary>
		/// <returns>An encoding object for the UTF-32 format using the little endian byte order.</returns>
		/// <filterpriority>1</filterpriority>
		public static Encoding UTF32
		{
			get
			{
				if (Encoding.utf32Encoding == null)
				{
					object obj = Encoding.lockobj;
					lock (obj)
					{
						if (Encoding.utf32Encoding == null)
						{
							Encoding.utf32Encoding = new UTF32Encoding(false, true);
						}
					}
				}
				return Encoding.utf32Encoding;
			}
		}

		internal static Encoding BigEndianUTF32
		{
			get
			{
				if (Encoding.bigEndianUTF32Encoding == null)
				{
					object obj = Encoding.lockobj;
					lock (obj)
					{
						if (Encoding.bigEndianUTF32Encoding == null)
						{
							Encoding.bigEndianUTF32Encoding = new UTF32Encoding(true, true);
						}
					}
				}
				return Encoding.bigEndianUTF32Encoding;
			}
		}

		/// <summary>When overridden in a derived class, calculates the number of bytes produced by encoding a set of characters starting at the specified character pointer.</summary>
		/// <returns>The number of bytes produced by encoding the specified characters.</returns>
		/// <param name="chars">A pointer to the first character to encode. </param>
		/// <param name="count">The number of characters to encode. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="chars" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="count" /> is less than zero. </exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		[ComVisible(false)]
		public unsafe virtual int GetByteCount(char* chars, int count)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			char[] array = new char[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = chars[i];
			}
			return this.GetByteCount(array);
		}

		/// <summary>When overridden in a derived class, calculates the number of characters produced by decoding a sequence of bytes starting at the specified byte pointer.</summary>
		/// <returns>The number of characters produced by decoding the specified sequence of bytes.</returns>
		/// <param name="bytes">A pointer to the first byte to decode. </param>
		/// <param name="count">The number of bytes to decode. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="count" /> is less than zero. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(false)]
		[CLSCompliant(false)]
		public unsafe virtual int GetCharCount(byte* bytes, int count)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			byte[] array = new byte[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = bytes[i];
			}
			return this.GetCharCount(array, 0, count);
		}

		/// <summary>When overridden in a derived class, decodes a sequence of bytes starting at the specified byte pointer into a set of characters that are stored starting at the specified character pointer.</summary>
		/// <returns>The actual number of characters written at the location indicated by the <paramref name="chars" /> parameter.</returns>
		/// <param name="bytes">A pointer to the first byte to decode. </param>
		/// <param name="byteCount">The number of bytes to decode. </param>
		/// <param name="chars">A pointer to the location at which to start writing the resulting set of characters. </param>
		/// <param name="charCount">The maximum number of characters to write. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is null.-or- <paramref name="chars" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="byteCount" /> or <paramref name="charCount" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="charCount" /> is less than the resulting number of characters. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.DecoderFallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		[ComVisible(false)]
		public unsafe virtual int GetChars(byte* bytes, int byteCount, char* chars, int charCount)
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
			byte[] array = new byte[byteCount];
			for (int i = 0; i < byteCount; i++)
			{
				array[i] = bytes[i];
			}
			char[] chars2 = this.GetChars(array, 0, byteCount);
			int num = chars2.Length;
			if (num > charCount)
			{
				throw new ArgumentException("charCount is less than the number of characters produced", "charCount");
			}
			for (int j = 0; j < num; j++)
			{
				chars[j] = chars2[j];
			}
			return num;
		}

		/// <summary>When overridden in a derived class, encodes a set of characters starting at the specified character pointer into a sequence of bytes that are stored starting at the specified byte pointer.</summary>
		/// <returns>The actual number of bytes written at the location indicated by the <paramref name="bytes" /> parameter.</returns>
		/// <param name="chars">A pointer to the first character to encode. </param>
		/// <param name="charCount">The number of characters to encode. </param>
		/// <param name="bytes">A pointer to the location at which to start writing the resulting sequence of bytes. </param>
		/// <param name="byteCount">The maximum number of bytes to write. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="chars" /> is null.-or- <paramref name="bytes" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="charCount" /> or <paramref name="byteCount" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="byteCount" /> is less than the resulting number of bytes. </exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">A fallback occurred (see Understanding Encodings for complete explanation)-and-<see cref="P:System.Text.Encoding.EncoderFallback" /> is set to <see cref="T:System.Text.EncoderExceptionFallback" />.</exception>
		/// <filterpriority>1</filterpriority>
		[CLSCompliant(false)]
		[ComVisible(false)]
		public unsafe virtual int GetBytes(char* chars, int charCount, byte* bytes, int byteCount)
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
			char[] array = new char[charCount];
			for (int i = 0; i < charCount; i++)
			{
				array[i] = chars[i];
			}
			byte[] bytes2 = this.GetBytes(array, 0, charCount);
			int num = bytes2.Length;
			if (num > byteCount)
			{
				throw new ArgumentException("byteCount is less that the number of bytes produced", "byteCount");
			}
			for (int j = 0; j < num; j++)
			{
				bytes[j] = bytes2[j];
			}
			return bytes2.Length;
		}

		private sealed class ForwardingDecoder : Decoder
		{
			private Encoding encoding;

			public ForwardingDecoder(Encoding enc)
			{
				this.encoding = enc;
				DecoderFallback decoderFallback = this.encoding.DecoderFallback;
				if (decoderFallback != null)
				{
					base.Fallback = decoderFallback;
				}
			}

			public override int GetCharCount(byte[] bytes, int index, int count)
			{
				return this.encoding.GetCharCount(bytes, index, count);
			}

			public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
			{
				return this.encoding.GetChars(bytes, byteIndex, byteCount, chars, charIndex);
			}
		}

		private sealed class ForwardingEncoder : Encoder
		{
			private Encoding encoding;

			public ForwardingEncoder(Encoding enc)
			{
				this.encoding = enc;
				EncoderFallback encoderFallback = this.encoding.EncoderFallback;
				if (encoderFallback != null)
				{
					base.Fallback = encoderFallback;
				}
			}

			public override int GetByteCount(char[] chars, int index, int count, bool flush)
			{
				return this.encoding.GetByteCount(chars, index, count);
			}

			public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteCount, bool flush)
			{
				return this.encoding.GetBytes(chars, charIndex, charCount, bytes, byteCount);
			}
		}
	}
}
