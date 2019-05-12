using System;

namespace System.Text
{
	/// <summary>Provides a failure-handling mechanism, called a fallback, for an encoded input byte sequence that cannot be converted to an output character. </summary>
	/// <filterpriority>2</filterpriority>
	[Serializable]
	public abstract class DecoderFallback
	{
		private static DecoderFallback exception_fallback = new DecoderExceptionFallback();

		private static DecoderFallback replacement_fallback = new DecoderReplacementFallback();

		private static DecoderFallback standard_safe_fallback = new DecoderReplacementFallback("�");

		/// <summary>Gets an object that throws an exception when an input byte sequence cannot be decoded.</summary>
		/// <returns>A type derived from the <see cref="T:System.Text.DecoderFallback" /> class. The default value is a <see cref="T:System.Text.DecoderExceptionFallback" /> object.</returns>
		/// <filterpriority>1</filterpriority>
		public static DecoderFallback ExceptionFallback
		{
			get
			{
				return DecoderFallback.exception_fallback;
			}
		}

		/// <summary>When overridden in a derived class, gets the maximum number of characters the current <see cref="T:System.Text.DecoderFallback" /> object can return.</summary>
		/// <returns>The maximum number of characters the current <see cref="T:System.Text.DecoderFallback" /> object can return.</returns>
		/// <filterpriority>2</filterpriority>
		public abstract int MaxCharCount { get; }

		/// <summary>Gets an object that outputs a substitute string in place of an input byte sequence that cannot be decoded.</summary>
		/// <returns>A type derived from the <see cref="T:System.Text.DecoderFallback" /> class. The default value is a <see cref="T:System.Text.DecoderReplacementFallback" /> object that emits the QUESTION MARK character ("?", U+003F) in place of unknown byte sequences. </returns>
		/// <filterpriority>1</filterpriority>
		public static DecoderFallback ReplacementFallback
		{
			get
			{
				return DecoderFallback.replacement_fallback;
			}
		}

		internal static DecoderFallback StandardSafeFallback
		{
			get
			{
				return DecoderFallback.standard_safe_fallback;
			}
		}

		/// <summary>When overridden in a derived class, initializes a new instance of the <see cref="T:System.Text.DecoderFallbackBuffer" /> class. </summary>
		/// <returns>An object that provides a fallback buffer for a decoder.</returns>
		/// <filterpriority>2</filterpriority>
		public abstract DecoderFallbackBuffer CreateFallbackBuffer();
	}
}
