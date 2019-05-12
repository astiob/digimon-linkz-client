using System;

namespace System.Globalization
{
	/// <summary>Represents time in divisions, such as months, days, and years. Years are calculated as for the Japanese calendar, while days and months are calculated using the lunisolar calendar.</summary>
	[Serializable]
	public class JapaneseLunisolarCalendar : EastAsianLunisolarCalendar
	{
		/// <summary>Specifies the current era.</summary>
		public const int JapaneseEra = 1;

		internal static readonly CCEastAsianLunisolarEraHandler era_handler;

		private static DateTime JapanMin = new DateTime(1960, 1, 28, 0, 0, 0);

		private static DateTime JapanMax = new DateTime(2050, 1, 22, 23, 59, 59);

		/// <summary>Initializes a new instance of the <see cref="T:System.Globalization.JapaneseLunisolarCalendar" /> class. </summary>
		[MonoTODO]
		public JapaneseLunisolarCalendar() : base(JapaneseLunisolarCalendar.era_handler)
		{
		}

		static JapaneseLunisolarCalendar()
		{
			JapaneseLunisolarCalendar.era_handler = new CCEastAsianLunisolarEraHandler();
			JapaneseLunisolarCalendar.era_handler.appendEra(3, CCGregorianCalendar.fixed_from_dmy(25, 12, 1926), CCGregorianCalendar.fixed_from_dmy(7, 1, 1989));
			JapaneseLunisolarCalendar.era_handler.appendEra(4, CCGregorianCalendar.fixed_from_dmy(8, 1, 1989));
		}

		internal override int ActualCurrentEra
		{
			get
			{
				return 4;
			}
		}

		/// <summary>Gets the eras that are relevant to the <see cref="T:System.Globalization.JapaneseLunisolarCalendar" /> object.</summary>
		/// <returns>An array of 32-bit signed integers that specify the relevant eras.</returns>
		public override int[] Eras
		{
			get
			{
				return (int[])JapaneseLunisolarCalendar.era_handler.Eras.Clone();
			}
		}

		/// <summary>Retrieves the era that corresponds to the specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>An integer that represents the era specified in the <paramref name="time" /> parameter.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to read. </param>
		public override int GetEra(DateTime time)
		{
			int date = CCFixed.FromDateTime(time);
			int result;
			JapaneseLunisolarCalendar.era_handler.EraYear(out result, date);
			return result;
		}

		/// <summary>Gets the minimum date and time supported by the <see cref="T:System.Globalization.JapaneseLunisolarCalendar" /> class.</summary>
		/// <returns>The earliest date and time supported by the <see cref="T:System.Globalization.JapaneseLunisolarCalendar" /> class, which is equivalent to the first moment of January 28, 1960 C.E. in the Gregorian calendar.</returns>
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return JapaneseLunisolarCalendar.JapanMin;
			}
		}

		/// <summary>Gets the maximum date and time supported by the <see cref="T:System.Globalization.JapaneseLunisolarCalendar" /> class.</summary>
		/// <returns>The latest date and time supported by the <see cref="T:System.Globalization.JapaneseLunisolarCalendar" /> class, which is equivalent to the last moment of January 22, 2050 C.E. in the Gregorian calendar.</returns>
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return JapaneseLunisolarCalendar.JapanMax;
			}
		}
	}
}
