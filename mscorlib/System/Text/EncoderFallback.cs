using System;

namespace System.Text
{
	/// <summary>Provides a failure-handling mechanism, called a fallback, for an input character that cannot be converted to an encoded output byte sequence. </summary>
	/// <filterpriority>2</filterpriority>
	[Serializable]
	public abstract class EncoderFallback
	{
		private static EncoderFallback exception_fallback = new EncoderExceptionFallback();

		private static EncoderFallback replacement_fallback = new EncoderReplacementFallback();

		private static EncoderFallback standard_safe_fallback = new EncoderReplacementFallback("�");

		/// <summary>Gets an object that throws an exception when an input character cannot be encoded.</summary>
		/// <returns>A type derived from the <see cref="T:System.Text.EncoderFallback" /> class. The default value is a <see cref="T:System.Text.EncoderExceptionFallback" /> object.</returns>
		/// <filterpriority>1</filterpriority>
		public static EncoderFallback ExceptionFallback
		{
			get
			{
				return EncoderFallback.exception_fallback;
			}
		}

		/// <summary>When overridden in a derived class, gets the maximum number of characters the current <see cref="T:System.Text.EncoderFallback" /> object can return.</summary>
		/// <returns>The maximum number of characters the current <see cref="T:System.Text.EncoderFallback" /> object can return.</returns>
		/// <filterpriority>2</filterpriority>
		public abstract int MaxCharCount { get; }

		/// <summary>Gets an object that outputs a substitute string in place of an input character that cannot be encoded.</summary>
		/// <returns>A type derived from the <see cref="T:System.Text.EncoderFallback" /> class. The default value is a <see cref="T:System.Text.EncoderReplacementFallback" /> object that replaces unknown input characters with the QUESTION MARK character ("?", U+003F).</returns>
		/// <filterpriority>1</filterpriority>
		public static EncoderFallback ReplacementFallback
		{
			get
			{
				return EncoderFallback.replacement_fallback;
			}
		}

		internal static EncoderFallback StandardSafeFallback
		{
			get
			{
				return EncoderFallback.standard_safe_fallback;
			}
		}

		/// <summary>When overridden in a derived class, initializes a new instance of the <see cref="T:System.Text.EncoderFallbackBuffer" /> class. </summary>
		/// <returns>An object that provides a fallback buffer for an encoder.</returns>
		/// <filterpriority>2</filterpriority>
		public abstract EncoderFallbackBuffer CreateFallbackBuffer();
	}
}
