using System;

namespace System.Text
{
	/// <summary>Provides a buffer that allows a fallback handler to return an alternate string to a decoder when it cannot decode an input byte sequence. </summary>
	/// <filterpriority>2</filterpriority>
	public abstract class DecoderFallbackBuffer
	{
		/// <summary>When overridden in a derived class, gets the number of characters in the current <see cref="T:System.Text.DecoderFallbackBuffer" /> object that remain to be processed.</summary>
		/// <returns>The number of characters in the current fallback buffer that have not yet been processed.</returns>
		/// <filterpriority>1</filterpriority>
		public abstract int Remaining { get; }

		/// <summary>When overridden in a derived class, prepares the fallback buffer to handle the specified input byte sequence.</summary>
		/// <returns>true if the fallback buffer can process <paramref name="bytesUnknown" />; false if the fallback buffer ignores <paramref name="bytesUnknown" />.</returns>
		/// <param name="bytesUnknown">An input array of bytes.</param>
		/// <param name="index">The index position of a byte in <paramref name="bytesUnknown" />.</param>
		/// <filterpriority>1</filterpriority>
		public abstract bool Fallback(byte[] bytesUnknown, int index);

		/// <summary>When overridden in a derived class, retrieves the next character in the fallback buffer.</summary>
		/// <returns>The next character in the fallback buffer.</returns>
		/// <filterpriority>2</filterpriority>
		public abstract char GetNextChar();

		/// <summary>When overridden in a derived class, causes the next call to the <see cref="M:System.Text.DecoderFallbackBuffer.GetNextChar" /> method to access the data buffer character position that is prior to the current character position. </summary>
		/// <returns>true if the <see cref="M:System.Text.DecoderFallbackBuffer.MovePrevious" /> operation was successful; otherwise, false.</returns>
		/// <filterpriority>1</filterpriority>
		public abstract bool MovePrevious();

		/// <summary>Initializes all data and state information pertaining to this fallback buffer.</summary>
		/// <filterpriority>1</filterpriority>
		public virtual void Reset()
		{
		}
	}
}
