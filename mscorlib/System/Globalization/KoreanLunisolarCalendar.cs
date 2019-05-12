using System;

namespace System.Globalization
{
	/// <summary>Represents time in divisions, such as months, days, and years. Years are calculated using the Gregorian calendar, while days and months are calculated using the lunisolar calendar.</summary>
	[Serializable]
	public class KoreanLunisolarCalendar : EastAsianLunisolarCalendar
	{
		/// <summary>Specifies the Gregorian era that corresponds to the current <see cref="T:System.Globalization.KoreanLunisolarCalendar" /> object.</summary>
		public const int GregorianEra = 1;

		internal static readonly CCEastAsianLunisolarEraHandler era_handler;

		private static DateTime KoreanMin = new DateTime(918, 2, 14, 0, 0, 0);

		private static DateTime KoreanMax = new DateTime(2051, 2, 10, 23, 59, 59);

		/// <summary>Initializes a new instance of the <see cref="T:System.Globalization.KoreanLunisolarCalendar" /> class. </summary>
		[MonoTODO]
		public KoreanLunisolarCalendar() : base(KoreanLunisolarCalendar.era_handler)
		{
		}

		static KoreanLunisolarCalendar()
		{
			KoreanLunisolarCalendar.era_handler = new CCEastAsianLunisolarEraHandler();
			KoreanLunisolarCalendar.era_handler.appendEra(1, CCFixed.FromDateTime(new DateTime(1, 1, 1)));
		}

		/// <summary>Gets the eras that correspond to the range of dates and times supported by the current <see cref="T:System.Globalization.KoreanLunisolarCalendar" /> object.</summary>
		/// <returns>An array of 32-bit signed integers that specify the relevant eras. The return value for a <see cref="T:System.Globalization.KoreanLunisolarCalendar" /> object is always an array containing one element equal to the <see cref="F:System.Globalization.KoreanLunisolarCalendar.GregorianEra" /> value.</returns>
		public override int[] Eras
		{
			get
			{
				return (int[])KoreanLunisolarCalendar.era_handler.Eras.Clone();
			}
		}

		/// <summary>Retrieves the era that corresponds to the specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>An integer that represents the era specified by the <paramref name="time" /> parameter. The return value for a <see cref="T:System.Globalization.KoreanLunisolarCalendar" /> object is always the <see cref="F:System.Globalization.KoreanLunisolarCalendar.GregorianEra" /> value.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to read. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="time" /> represents a date and time less than <see cref="P:System.Globalization.KoreanLunisolarCalendar.MinSupportedDateTime" /> or greater than <see cref="P:System.Globalization.KoreanLunisolarCalendar.MaxSupportedDateTime" />.</exception>
		public override int GetEra(DateTime time)
		{
			int date = CCFixed.FromDateTime(time);
			int result;
			KoreanLunisolarCalendar.era_handler.EraYear(out result, date);
			return result;
		}

		/// <summary>Gets the minimum date and time supported by the <see cref="T:System.Globalization.KoreanLunisolarCalendar" /> class.</summary>
		/// <returns>The earliest date and time supported by the <see cref="T:System.Globalization.KoreanLunisolarCalendar" /> class, which is equivalent to the first moment of February 14, 918 C.E. in the Gregorian calendar.</returns>
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return KoreanLunisolarCalendar.KoreanMin;
			}
		}

		/// <summary>Gets the maximum date and time supported by the <see cref="T:System.Globalization.KoreanLunisolarCalendar" /> class.</summary>
		/// <returns>The latest date and time supported by the <see cref="T:System.Globalization.KoreanLunisolarCalendar" /> class, which is equivalent to the last  moment of February 10, 2051 C.E. in the Gregorian calendar.</returns>
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return KoreanLunisolarCalendar.KoreanMax;
			}
		}
	}
}
