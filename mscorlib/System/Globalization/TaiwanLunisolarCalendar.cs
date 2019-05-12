using System;

namespace System.Globalization
{
	/// <summary>Represents the Taiwan lunisolar calendar. As for the Taiwan calendar, years are calculated using the Gregorian calendar, while days and months are calculated using the lunisolar calendar.</summary>
	[Serializable]
	public class TaiwanLunisolarCalendar : EastAsianLunisolarCalendar
	{
		private const int TaiwanEra = 1;

		internal static readonly CCEastAsianLunisolarEraHandler era_handler;

		private static DateTime TaiwanMin = new DateTime(1912, 2, 18);

		private static DateTime TaiwanMax = new DateTime(2051, 2, 10, 23, 59, 59, 999);

		/// <summary>Initializes a new instance of the <see cref="T:System.Globalization.TaiwanLunisolarCalendar" /> class. </summary>
		[MonoTODO]
		public TaiwanLunisolarCalendar() : base(TaiwanLunisolarCalendar.era_handler)
		{
		}

		static TaiwanLunisolarCalendar()
		{
			TaiwanLunisolarCalendar.era_handler = new CCEastAsianLunisolarEraHandler();
			TaiwanLunisolarCalendar.era_handler.appendEra(1, CCFixed.FromDateTime(TaiwanLunisolarCalendar.TaiwanMin), CCFixed.FromDateTime(TaiwanLunisolarCalendar.TaiwanMax));
		}

		/// <summary>Gets the eras that are relevant to the current <see cref="T:System.Globalization.TaiwanLunisolarCalendar" /> object.</summary>
		/// <returns>An array that consists of a single element having a value that is always the current era.</returns>
		public override int[] Eras
		{
			get
			{
				return (int[])TaiwanLunisolarCalendar.era_handler.Eras.Clone();
			}
		}

		/// <summary>Retrieves the era that corresponds to the specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>An integer that represents the era specified in the <paramref name="time" /> parameter.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to read. </param>
		public override int GetEra(DateTime time)
		{
			int date = CCFixed.FromDateTime(time);
			int result;
			TaiwanLunisolarCalendar.era_handler.EraYear(out result, date);
			return result;
		}

		/// <summary>Gets the minimum date and time supported by the <see cref="T:System.Globalization.TaiwanLunisolarCalendar" /> class.</summary>
		/// <returns>The earliest date and time supported by the <see cref="T:System.Globalization.TaiwanLunisolarCalendar" /> class, which is equivalent to the first moment of February 18, 1912 C.E. in the Gregorian calendar.</returns>
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return TaiwanLunisolarCalendar.TaiwanMin;
			}
		}

		/// <summary>Gets the maximum date and time supported by the <see cref="T:System.Globalization.TaiwanLunisolarCalendar" /> class.</summary>
		/// <returns>The latest date and time supported by the <see cref="T:System.Globalization.TaiwanLunisolarCalendar" /> class, which is equivalent to the last moment of February 10, 2051 C.E. in the Gregorian calendar.</returns>
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return TaiwanLunisolarCalendar.TaiwanMax;
			}
		}
	}
}
