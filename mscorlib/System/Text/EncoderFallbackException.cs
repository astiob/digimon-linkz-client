using System;

namespace System.Text
{
	/// <summary>The exception that is thrown when an encoder fallback operation fails. This class cannot be inherited.</summary>
	/// <filterpriority>2</filterpriority>
	[Serializable]
	public sealed class EncoderFallbackException : ArgumentException
	{
		private const string defaultMessage = "Failed to decode the input byte sequence to Unicode characters.";

		private char char_unknown;

		private char char_unknown_high;

		private char char_unknown_low;

		private int index;

		/// <summary>Initializes a new instance of the <see cref="T:System.Text.EncoderFallbackException" /> class.</summary>
		public EncoderFallbackException() : this(null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Text.EncoderFallbackException" /> class. A parameter specifies the error message.</summary>
		/// <param name="message">An error message.</param>
		public EncoderFallbackException(string message)
		{
			this.index = -1;
			base..ctor(message);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Text.EncoderFallbackException" /> class. Parameters specify the error message and the inner exception that is the cause of this exception.</summary>
		/// <param name="message">An error message.</param>
		/// <param name="innerException">The exception that caused this exception.</param>
		public EncoderFallbackException(string message, Exception innerException)
		{
			this.index = -1;
			base..ctor(message, innerException);
		}

		internal EncoderFallbackException(char charUnknown, int index)
		{
			this.index = -1;
			base..ctor(null);
			this.char_unknown = charUnknown;
			this.index = index;
		}

		internal EncoderFallbackException(char charUnknownHigh, char charUnknownLow, int index)
		{
			this.index = -1;
			base..ctor(null);
			this.char_unknown_high = charUnknownHigh;
			this.char_unknown_low = charUnknownLow;
			this.index = index;
		}

		/// <summary>Gets the input character that caused the exception.</summary>
		/// <returns>The character that cannot be encoded.</returns>
		/// <filterpriority>2</filterpriority>
		public char CharUnknown
		{
			get
			{
				return this.char_unknown;
			}
		}

		/// <summary>Gets the high component character of the surrogate pair that caused the exception.</summary>
		/// <returns>The high component character of the surrogate pair that cannot be encoded.</returns>
		/// <filterpriority>2</filterpriority>
		public char CharUnknownHigh
		{
			get
			{
				return this.char_unknown_high;
			}
		}

		/// <summary>Gets the low component character of the surrogate pair that caused the exception.</summary>
		/// <returns>The low component character of the surrogate pair that cannot be encoded.</returns>
		/// <filterpriority>2</filterpriority>
		public char CharUnknownLow
		{
			get
			{
				return this.char_unknown_low;
			}
		}

		/// <summary>Gets the index position in the input buffer of the character that caused the exception.</summary>
		/// <returns>The index position in the input buffer of the character that cannot be encoded.</returns>
		/// <filterpriority>1</filterpriority>
		[MonoTODO]
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		/// <summary>Indicates whether the input that caused the exception is a surrogate pair.</summary>
		/// <returns>true if the input was a surrogate pair; otherwise, false.</returns>
		/// <filterpriority>2</filterpriority>
		[MonoTODO]
		public bool IsUnknownSurrogate()
		{
			throw new NotImplementedException();
		}
	}
}
