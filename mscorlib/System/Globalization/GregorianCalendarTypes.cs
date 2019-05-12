using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	/// <summary>Defines the different language versions of the Gregorian calendar.</summary>
	[ComVisible(true)]
	[Serializable]
	public enum GregorianCalendarTypes
	{
		/// <summary>Refers to the localized version of the Gregorian calendar, based on the language of the <see cref="T:System.Globalization.CultureInfo" /> that uses the <see cref="T:System.Globalization.DateTimeFormatInfo" />.</summary>
		Localized = 1,
		/// <summary>Refers to the U.S. English version of the Gregorian calendar.</summary>
		USEnglish,
		/// <summary>Refers to the Middle East French version of the Gregorian calendar.</summary>
		MiddleEastFrench = 9,
		/// <summary>Refers to the Arabic version of the Gregorian calendar.</summary>
		Arabic,
		/// <summary>Refers to the transliterated English version of the Gregorian calendar.</summary>
		TransliteratedEnglish,
		/// <summary>Refers to the transliterated French version of the Gregorian calendar.</summary>
		TransliteratedFrench
	}
}
