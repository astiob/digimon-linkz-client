using System;
using System.Runtime.InteropServices;

namespace System.Text
{
	/// <summary>Converts a sequence of encoded bytes into a set of characters.</summary>
	/// <filterpriority>1</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public abstract class Decoder
	{
		private DecoderFallback fallback = new DecoderReplacementFallback();

		private DecoderFallbackBuffer fallback_buffer;

		/// <summary>Gets or sets a <see cref="T:System.Text.DecoderFallback" /> object for the current <see cref="T:System.Text.Decoder" /> object.</summary>
		/// <returns>A <see cref="T:System.Text.DecoderFallback" /> object.</returns>
		/// <exception cref="T:System.ArgumentNullException">The value in a set operation is null (Nothing).</exception>
		/// <exception cref="T:System.ArgumentException">A new value cannot be assigned in a set operation because the current <see cref="T:System.Text.DecoderFallbackBuffer" /> object contains data that has not been decoded yet. </exception>
		/// <filterpriority>1</filterpriority>
		[ComVisible(false)]
		public DecoderFallback Fallback
		{
			get
			{
				return this.fallback;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				this.fallback = value;
				this.fallback_buffer = null;
			}
		}

		/// <summary>Gets the <see cref="T:System.Text.DecoderFallbackBuffer" /> object associated with the current <see cref="T:System.Text.Decoder" /> object.</summary>
		/// <returns>A <see cref="T:System.Text.DecoderFallbackBuffer" /> object.</returns>
		/// <filterpriority>1</filterpriority>
		[ComVisible(false)]
		public DecoderFallbackBuffer FallbackBuffer
		{
			get
			{
				if (this.fallback_buffer == null)
				{
					this.fallback_buffer = this.fallback.CreateFallbackBuffer();
				}
				return this.fallback_buffer;
			}
		}

		/// <summary>When overridden in a derived class, calculates the number of characters produced by decoding a sequence of bytes from the specified byte array.</summary>
		/// <returns>The number of characters produced by decoding the specified sequence of bytes and any bytes in the internal buffer.</returns>
		/// <param name="bytes">The byte array containing the sequence of bytes to decode. </param>
		/// <param name="index">The index of the first byte to decode. </param>
		/// <param name="count">The number of bytes to decode. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is null (Nothing). </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is less than zero.-or- <paramref name="index" /> and <paramref name="count" /> do not denote a valid range in <paramref name="bytes" />. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for fuller explanation)-and-<see cref="P:System.Text.Decoder.Fallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <filterpriority>2</filterpriority>
		public abstract int GetCharCount(byte[] bytes, int index, int count);

		/// <summary>When overridden in a derived class, decodes a sequence of bytes from the specified byte array and any bytes in the internal buffer into the specified character array.</summary>
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
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="chars" /> does not have enough capacity from <paramref name="charIndex" /> to the end of the array to accommodate the resulting characters. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for fuller explanation)-and-<see cref="P:System.Text.Decoder.Fallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <filterpriority>2</filterpriority>
		public abstract int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex);

		/// <summary>When overridden in a derived class, calculates the number of characters produced by decoding a sequence of bytes from the specified byte array. A parameter indicates whether to clear the internal state of the decoder after the calculation.</summary>
		/// <returns>The number of characters produced by decoding the specified sequence of bytes and any bytes in the internal buffer.</returns>
		/// <param name="bytes">The byte array containing the sequence of bytes to decode. </param>
		/// <param name="index">The index of the first byte to decode. </param>
		/// <param name="count">The number of bytes to decode. </param>
		/// <param name="flush">true to simulate clearing the internal state of the encoder after the calculation; otherwise, false. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is null (Nothing). </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is less than zero.-or- <paramref name="index" /> and <paramref name="count" /> do not denote a valid range in <paramref name="bytes" />. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for fuller explanation)-and-<see cref="P:System.Text.Decoder.Fallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <filterpriority>2</filterpriority>
		[ComVisible(false)]
		public virtual int GetCharCount(byte[] bytes, int index, int count, bool flush)
		{
			if (flush)
			{
				this.Reset();
			}
			return this.GetCharCount(bytes, index, count);
		}

		/// <summary>When overridden in a derived class, calculates the number of characters produced by decoding a sequence of bytes starting at the specified byte pointer. A parameter indicates whether to clear the internal state of the decoder after the calculation.</summary>
		/// <returns>The number of characters produced by decoding the specified sequence of bytes and any bytes in the internal buffer.</returns>
		/// <param name="bytes">A pointer to the first byte to decode. </param>
		/// <param name="count">The number of bytes to decode. </param>
		/// <param name="flush">true to simulate clearing the internal state of the encoder after the calculation; otherwise, false. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is null (Nothing in Visual Basic .NET). </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="count" /> is less than zero. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for fuller explanation)-and-<see cref="P:System.Text.Decoder.Fallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <filterpriority>2</filterpriority>
		[CLSCompliant(false)]
		[ComVisible(false)]
		public unsafe virtual int GetCharCount(byte* bytes, int count, bool flush)
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
			Marshal.Copy((IntPtr)((void*)bytes), array, 0, count);
			return this.GetCharCount(array, 0, count, flush);
		}

		/// <summary>When overridden in a derived class, decodes a sequence of bytes from the specified byte array and any bytes in the internal buffer into the specified character array. A parameter indicates whether to clear the internal state of the decoder after the conversion.</summary>
		/// <returns>The actual number of characters written into the <paramref name="chars" /> parameter.</returns>
		/// <param name="bytes">The byte array containing the sequence of bytes to decode. </param>
		/// <param name="byteIndex">The index of the first byte to decode. </param>
		/// <param name="byteCount">The number of bytes to decode. </param>
		/// <param name="chars">The character array to contain the resulting set of characters. </param>
		/// <param name="charIndex">The index at which to start writing the resulting set of characters. </param>
		/// <param name="flush">true to clear the internal state of the decoder after the conversion; otherwise, false. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is null (Nothing).-or- <paramref name="chars" /> is null (Nothing). </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="byteIndex" /> or <paramref name="byteCount" /> or <paramref name="charIndex" /> is less than zero.-or- <paramref name="byteindex" /> and <paramref name="byteCount" /> do not denote a valid range in <paramref name="bytes" />.-or- <paramref name="charIndex" /> is not a valid index in <paramref name="chars" />. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="chars" /> does not have enough capacity from <paramref name="charIndex" /> to the end of the array to accommodate the resulting characters. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for fuller explanation)-and-<see cref="P:System.Text.Decoder.Fallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <filterpriority>2</filterpriority>
		public virtual int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex, bool flush)
		{
			this.CheckArguments(bytes, byteIndex, byteCount);
			this.CheckArguments(chars, charIndex);
			if (flush)
			{
				this.Reset();
			}
			return this.GetChars(bytes, byteIndex, byteCount, chars, charIndex);
		}

		/// <summary>When overridden in a derived class, decodes a sequence of bytes starting at the specified byte pointer and any bytes in the internal buffer into a set of characters that are stored starting at the specified character pointer. A parameter indicates whether to clear the internal state of the decoder after the conversion.</summary>
		/// <returns>The actual number of characters written at the location indicated by the <paramref name="chars" /> parameter.</returns>
		/// <param name="bytes">A pointer to the first byte to decode. </param>
		/// <param name="byteCount">The number of bytes to decode. </param>
		/// <param name="chars">A pointer to the location at which to start writing the resulting set of characters. </param>
		/// <param name="charCount">The maximum number of characters to write. </param>
		/// <param name="flush">true to clear the internal state of the decoder after the conversion; otherwise, false. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="bytes" /> is null (Nothing).-or- <paramref name="chars" /> is null (Nothing). </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="byteCount" /> or <paramref name="charCount" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="charCount" /> is less than the resulting number of characters. </exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for fuller explanation)-and-<see cref="P:System.Text.Decoder.Fallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <filterpriority>2</filterpriority>
		[ComVisible(false)]
		[CLSCompliant(false)]
		public unsafe virtual int GetChars(byte* bytes, int byteCount, char* chars, int charCount, bool flush)
		{
			this.CheckArguments(chars, charCount, bytes, byteCount);
			char[] array = new char[charCount];
			Marshal.Copy((IntPtr)((void*)chars), array, 0, charCount);
			byte[] array2 = new byte[byteCount];
			Marshal.Copy((IntPtr)((void*)bytes), array2, 0, byteCount);
			return this.GetChars(array2, 0, byteCount, array, 0, flush);
		}

		/// <summary>When overridden in a derived class, sets the decoder back to its initial state.</summary>
		/// <filterpriority>2</filterpriority>
		[ComVisible(false)]
		public virtual void Reset()
		{
			if (this.fallback_buffer != null)
			{
				this.fallback_buffer.Reset();
			}
		}

		/// <summary>Converts a buffer of encoded bytes to Unicode characters and stores the result in another buffer.</summary>
		/// <param name="bytes">The address of a buffer that contains the byte sequences to convert.</param>
		/// <param name="byteCount">The number of bytes in <paramref name="bytes" /> to convert.</param>
		/// <param name="chars">The address of a buffer to store the converted characters.</param>
		/// <param name="charCount">The maximum number of characters in <paramref name="chars" /> to use in the conversion.</param>
		/// <param name="flush">true to indicate no further data is to be converted; otherwise, false.</param>
		/// <param name="bytesUsed">When this method returns, contains the number of bytes that were produced by the conversion. This parameter is passed uninitialized.</param>
		/// <param name="charsUsed">When this method returns, contains the number of characters from <paramref name="chars" /> that were used in the conversion. This parameter is passed uninitialized.</param>
		/// <param name="completed">When this method returns, contains true if all the characters specified by <paramref name="byteCount" /> were converted; otherwise, false. This parameter is passed uninitialized.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="chars" /> or <paramref name="bytes" /> is null (Nothing).</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="charCount" /> or <paramref name="byteCount" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">The output buffer is too small to contain any of the converted input. The output buffer should be greater than or equal to the size indicated by the <see cref="Overload:System.Text.Decoder.GetCharCount" /> method.</exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for fuller explanation)-and-<see cref="P:System.Text.Decoder.Fallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <filterpriority>2</filterpriority>
		[CLSCompliant(false)]
		[ComVisible(false)]
		public unsafe virtual void Convert(byte* bytes, int byteCount, char* chars, int charCount, bool flush, out int bytesUsed, out int charsUsed, out bool completed)
		{
			this.CheckArguments(chars, charCount, bytes, byteCount);
			bytesUsed = byteCount;
			for (;;)
			{
				charsUsed = this.GetCharCount(bytes, bytesUsed, flush);
				if (charsUsed <= charCount)
				{
					break;
				}
				flush = false;
				bytesUsed >>= 1;
			}
			completed = (bytesUsed == byteCount);
			charsUsed = this.GetChars(bytes, bytesUsed, chars, charCount, flush);
		}

		/// <summary>Converts an array of encoded bytes to Unicode characters and stores the result in a byte array.</summary>
		/// <param name="bytes">A byte array to convert.</param>
		/// <param name="byteIndex">The first element of <paramref name="bytes" /> to convert.</param>
		/// <param name="byteCount">The number of elements of <paramref name="bytes" /> to convert.</param>
		/// <param name="chars">An array to store the converted characters.</param>
		/// <param name="charIndex">The first element of <paramref name="chars" /> in which data is stored.</param>
		/// <param name="charCount">The maximum number of elements of <paramref name="chars" /> to use in the conversion.</param>
		/// <param name="flush">true to indicate that no further data is to be converted; otherwise, false.</param>
		/// <param name="bytesUsed">When this method returns, contains the number of bytes that were used in the conversion. This parameter is passed uninitialized.</param>
		/// <param name="charsUsed">When this method returns, contains the number of characters from <paramref name="chars" /> that were produced by the conversion. This parameter is passed uninitialized.</param>
		/// <param name="completed">When this method returns, contains true if all the characters specified by <paramref name="byteCount" /> were converted; otherwise, false. This parameter is passed uninitialized.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="chars" /> or <paramref name="bytes" /> is null (Nothing).</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="charIndex" />, <paramref name="charCount" />, <paramref name="byteIndex" />, or <paramref name="byteCount" /> is less than zero.-or-The length of <paramref name="chars" /> - <paramref name="charIndex" /> is less than <paramref name="charCount" />.-or-The length of <paramref name="bytes" /> - <paramref name="byteIndex" /> is less than <paramref name="byteCount" />.</exception>
		/// <exception cref="T:System.ArgumentException">The output buffer is too small to contain any of the converted input. The output buffer should be greater than or equal to the size indicated by the <see cref="Overload:System.Text.Decoder.GetCharCount" /> method.</exception>
		/// <exception cref="T:System.Text.DecoderFallbackException">A fallback occurred (see Understanding Encodings for fuller explanation)-and-<see cref="P:System.Text.Decoder.Fallback" /> is set to <see cref="T:System.Text.DecoderExceptionFallback" />.</exception>
		/// <filterpriority>2</filterpriority>
		[ComVisible(false)]
		public virtual void Convert(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex, int charCount, bool flush, out int bytesUsed, out int charsUsed, out bool completed)
		{
			this.CheckArguments(bytes, byteIndex, byteCount);
			this.CheckArguments(chars, charIndex);
			if (charCount < 0 || chars.Length < charIndex + charCount)
			{
				throw new ArgumentOutOfRangeException("charCount");
			}
			bytesUsed = byteCount;
			for (;;)
			{
				charsUsed = this.GetCharCount(bytes, byteIndex, bytesUsed, flush);
				if (charsUsed <= charCount)
				{
					break;
				}
				flush = false;
				bytesUsed >>= 1;
			}
			completed = (bytesUsed == byteCount);
			charsUsed = this.GetChars(bytes, byteIndex, bytesUsed, chars, charIndex, flush);
		}

		private void CheckArguments(char[] chars, int charIndex)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars");
			}
			if (charIndex < 0 || chars.Length <= charIndex)
			{
				throw new ArgumentOutOfRangeException("charIndex");
			}
		}

		private void CheckArguments(byte[] bytes, int byteIndex, int byteCount)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (byteIndex < 0 || bytes.Length <= byteIndex)
			{
				throw new ArgumentOutOfRangeException("byteIndex");
			}
			if (byteCount < 0 || bytes.Length < byteIndex + byteCount)
			{
				throw new ArgumentOutOfRangeException("byteCount");
			}
		}

		private unsafe void CheckArguments(char* chars, int charCount, byte* bytes, int byteCount)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars");
			}
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (charCount < 0)
			{
				throw new ArgumentOutOfRangeException("charCount");
			}
			if (byteCount < 0)
			{
				throw new ArgumentOutOfRangeException("byteCount");
			}
		}
	}
}
