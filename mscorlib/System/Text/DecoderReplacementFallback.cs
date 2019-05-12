using System;

namespace System.Text
{
	/// <summary>Provides a failure-handling mechanism, called a fallback, for an encoded input byte sequence that cannot be converted to an output character. The fallback emits a user-specified replacement string instead of a decoded input byte sequence. This class cannot be inherited.</summary>
	/// <filterpriority>2</filterpriority>
	[Serializable]
	public sealed class DecoderReplacementFallback : DecoderFallback
	{
		private string replacement;

		/// <summary>Initializes a new instance of the <see cref="T:System.Text.DecoderReplacementFallback" /> class. </summary>
		public DecoderReplacementFallback() : this("?")
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Text.DecoderReplacementFallback" /> class using a specified replacement string.</summary>
		/// <param name="replacement">A string that is emitted in a decoding operation in place of an input byte sequence that cannot be decoded.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="replacement" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="replacement" /> contains an invalid surrogate pair. In other words, the surrogate pair does not consist of one high surrogate component followed by one low surrogate component.</exception>
		[MonoTODO]
		public DecoderReplacementFallback(string replacement)
		{
			if (replacement == null)
			{
				throw new ArgumentNullException();
			}
			this.replacement = replacement;
		}

		/// <summary>Gets the replacement string that is the value of the <see cref="T:System.Text.DecoderReplacementFallback" /> object.</summary>
		/// <returns>A substitute string that is emitted in place of an input byte sequence that cannot be decoded.</returns>
		/// <filterpriority>2</filterpriority>
		public string DefaultString
		{
			get
			{
				return this.replacement;
			}
		}

		/// <summary>Gets the number of characters in the replacement string for the <see cref="T:System.Text.DecoderReplacementFallback" /> object.</summary>
		/// <returns>The number of characters in the string that is emitted in place of a byte sequence that cannot be decoded, that is, the length of the string returned by the <see cref="P:System.Text.DecoderReplacementFallback.DefaultString" /> property.</returns>
		/// <filterpriority>2</filterpriority>
		public override int MaxCharCount
		{
			get
			{
				return this.replacement.Length;
			}
		}

		/// <summary>Creates a <see cref="T:System.Text.DecoderFallbackBuffer" /> object that is initialized with the replacement string of this <see cref="T:System.Text.DecoderReplacementFallback" /> object.</summary>
		/// <returns>A <see cref="T:System.Text.DecoderFallbackBuffer" /> object that specifies a string to use instead of the original decoding operation input.</returns>
		/// <filterpriority>2</filterpriority>
		public override DecoderFallbackBuffer CreateFallbackBuffer()
		{
			return new DecoderReplacementFallbackBuffer(this);
		}

		/// <summary>Indicates whether the value of a specified object is equal to the <see cref="T:System.Text.DecoderReplacementFallback" /> object.</summary>
		/// <returns>true if <paramref name="value" /> is a <see cref="T:System.Text.DecoderReplacementFallback" /> object having a <see cref="P:System.Text.DecoderReplacementFallback.DefaultString" /> property that is equal to the <see cref="P:System.Text.DecoderReplacementFallback.DefaultString" /> property of the current <see cref="T:System.Text.DecoderReplacementFallback" /> object; otherwise, false. </returns>
		/// <param name="value">A <see cref="T:System.Text.DecoderReplacementFallback" /> object.</param>
		/// <filterpriority>2</filterpriority>
		public override bool Equals(object value)
		{
			DecoderReplacementFallback decoderReplacementFallback = value as DecoderReplacementFallback;
			return decoderReplacementFallback != null && this.replacement == decoderReplacementFallback.replacement;
		}

		/// <summary>Retrieves the hash code for the value of the <see cref="T:System.Text.DecoderReplacementFallback" /> object.</summary>
		/// <returns>The hash code of the value of the object.</returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			return this.replacement.GetHashCode();
		}
	}
}
