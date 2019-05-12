using System;

namespace System.Text
{
	/// <summary>Provides a buffer that allows a fallback handler to return an alternate string to an encoder when it cannot encode an input character. </summary>
	/// <filterpriority>2</filterpriority>
	public abstract class EncoderFallbackBuffer
	{
		/// <summary>When overridden in a derived class, gets the number of characters in the current <see cref="T:System.Text.EncoderFallbackBuffer" /> object that remain to be processed.</summary>
		/// <returns>The number of characters in the current fallback buffer that have not yet been processed.</returns>
		/// <filterpriority>1</filterpriority>
		public abstract int Remaining { get; }

		/// <summary>When overridden in a derived class, prepares the fallback buffer to handle the specified input character. </summary>
		/// <returns>true if the fallback buffer can process <paramref name="charUnknown" />; false if the fallback buffer ignores <paramref name="charUnknown" />.</returns>
		/// <param name="charUnknown">An input character.</param>
		/// <param name="index">The index position of the character in the input buffer.</param>
		/// <filterpriority>1</filterpriority>
		public abstract bool Fallback(char charUnknown, int index);

		/// <summary>When overridden in a derived class, prepares the fallback buffer to handle the specified surrogate pair.</summary>
		/// <returns>True if the fallback buffer can process <paramref name="charUnknownHigh" /> and <paramref name="charUnknownLow" />; false if fallback buffer ignores the surrogate pair.</returns>
		/// <param name="charUnknownHigh">The high surrogate of the input pair.</param>
		/// <param name="charUnknownLow">The low surrogate of the input pair.</param>
		/// <param name="index">The index position of the surrogate pair in the input buffer.</param>
		/// <filterpriority>1</filterpriority>
		public abstract bool Fallback(char charUnknownHigh, char charUnknownLow, int index);

		/// <summary>When overridden in a derived class, retrieves the next character in the fallback buffer.</summary>
		/// <returns>The next character in the fallback buffer.</returns>
		/// <filterpriority>2</filterpriority>
		public abstract char GetNextChar();

		/// <summary>When overridden in a derived class, causes the next call to the <see cref="M:System.Text.EncoderFallbackBuffer.GetNextChar" /> method to access the data buffer character position that is prior to the current character position. </summary>
		/// <returns>true if the <see cref="M:System.Text.EncoderFallbackBuffer.MovePrevious" /> operation was successful; otherwise, false.</returns>
		/// <filterpriority>1</filterpriority>
		public abstract bool MovePrevious();

		/// <summary>Initializes all data and state information pertaining to this fallback buffer.</summary>
		/// <filterpriority>1</filterpriority>
		public virtual void Reset()
		{
			while (this.GetNextChar() != '\0')
			{
			}
		}
	}
}
