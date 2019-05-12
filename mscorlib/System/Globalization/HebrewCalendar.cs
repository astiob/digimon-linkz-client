using System;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	/// <summary>Represents the Hebrew calendar.</summary>
	[MonoTODO("Serialization format not compatible with.NET")]
	[ComVisible(true)]
	[Serializable]
	public class HebrewCalendar : Calendar
	{
		internal const long M_MinTicks = 499147488000000000L;

		internal const long M_MaxTicks = 706783967999999999L;

		internal const int M_MinYear = 5343;

		/// <summary>Represents the current era. This field is constant.</summary>
		public static readonly int HebrewEra = 1;

		private static DateTime Min = new DateTime(1583, 1, 1, 0, 0, 0);

		private static DateTime Max = new DateTime(2239, 9, 29, 11, 59, 59);

		/// <summary>Initializes a new instance of the <see cref="T:System.Globalization.HebrewCalendar" /> class.</summary>
		public HebrewCalendar()
		{
			this.M_AbbrEraNames = new string[]
			{
				"A.M."
			};
			this.M_EraNames = new string[]
			{
				"Anno Mundi"
			};
			if (this.twoDigitYearMax == 99)
			{
				this.twoDigitYearMax = 5790;
			}
		}

		internal override int M_MaxYear
		{
			get
			{
				return 6000;
			}
		}

		/// <summary>Gets the list of eras in the <see cref="T:System.Globalization.HebrewCalendar" />.</summary>
		/// <returns>An array of integers that represents the eras in the <see cref="T:System.Globalization.HebrewCalendar" /> type. The return value is always an array containing one element equal to <see cref="F:System.Globalization.HebrewCalendar.HebrewEra" />. </returns>
		public override int[] Eras
		{
			get
			{
				return new int[]
				{
					HebrewCalendar.HebrewEra
				};
			}
		}

		/// <summary>Gets or sets the last year of a 100-year range that can be represented by a 2-digit year.</summary>
		/// <returns>The last year of a 100-year range that can be represented by a 2-digit year.</returns>
		/// <exception cref="T:System.InvalidOperationException">The current <see cref="T:System.Globalization.HebrewCalendar" /> object is read-only.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">In a set operation, the Hebrew calendar year value is less than 5343 but is not 99, or the year value is greater than 5999. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override int TwoDigitYearMax
		{
			get
			{
				return this.twoDigitYearMax;
			}
			set
			{
				base.CheckReadOnly();
				base.M_ArgumentInRange("value", value, 5343, this.M_MaxYear);
				this.twoDigitYearMax = value;
			}
		}

		internal void M_CheckDateTime(DateTime time)
		{
			if (time.Ticks < 499147488000000000L || time.Ticks > 706783967999999999L)
			{
				throw new ArgumentOutOfRangeException("time", "Only hebrew years between 5343 and 6000, inclusive, are supported.");
			}
		}

		internal void M_CheckEra(ref int era)
		{
			if (era == 0)
			{
				era = HebrewCalendar.HebrewEra;
			}
			if (era != HebrewCalendar.HebrewEra)
			{
				throw new ArgumentException("Era value was not valid.");
			}
		}

		internal override void M_CheckYE(int year, ref int era)
		{
			this.M_CheckEra(ref era);
			if (year < 5343 || year > this.M_MaxYear)
			{
				throw new ArgumentOutOfRangeException("year", "Only hebrew years between 5343 and 6000, inclusive, are supported.");
			}
		}

		internal void M_CheckYME(int year, int month, ref int era)
		{
			this.M_CheckYE(year, ref era);
			int num = CCHebrewCalendar.last_month_of_year(year);
			if (month < 1 || month > num)
			{
				StringWriter stringWriter = new StringWriter();
				stringWriter.Write("Month must be between 1 and {0}.", num);
				throw new ArgumentOutOfRangeException("month", stringWriter.ToString());
			}
		}

		internal void M_CheckYMDE(int year, int month, int day, ref int era)
		{
			this.M_CheckYME(year, month, ref era);
			base.M_ArgumentInRange("day", day, 1, this.GetDaysInMonth(year, month, era));
		}

		/// <summary>Returns a <see cref="T:System.DateTime" /> that is the specified number of months away from the specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>The <see cref="T:System.DateTime" /> that results from adding the specified number of months to the specified <see cref="T:System.DateTime" />.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to which to add <paramref name="months" />. </param>
		/// <param name="months">The number of months to add. </param>
		/// <exception cref="T:System.ArgumentException">The resulting <see cref="T:System.DateTime" /> is outside the supported range. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="months" /> is less than -120,000 or greater than 120,000. </exception>
		public override DateTime AddMonths(DateTime time, int months)
		{
			DateTime dateTime;
			if (months == 0)
			{
				dateTime = time;
			}
			else
			{
				int date = CCFixed.FromDateTime(time);
				int day;
				int num;
				int num2;
				CCHebrewCalendar.dmy_from_fixed(out day, out num, out num2, date);
				num = this.M_Month(num, num2);
				if (months < 0)
				{
					while (months < 0)
					{
						if (num + months > 0)
						{
							num += months;
							months = 0;
						}
						else
						{
							months += num;
							num2--;
							num = this.GetMonthsInYear(num2);
						}
					}
				}
				else
				{
					while (months > 0)
					{
						int monthsInYear = this.GetMonthsInYear(num2);
						if (num + months <= monthsInYear)
						{
							num += months;
							months = 0;
						}
						else
						{
							months -= monthsInYear - num + 1;
							num = 1;
							num2++;
						}
					}
				}
				dateTime = this.ToDateTime(num2, num, day, 0, 0, 0, 0).Add(time.TimeOfDay);
			}
			this.M_CheckDateTime(dateTime);
			return dateTime;
		}

		/// <summary>Returns a <see cref="T:System.DateTime" /> that is the specified number of years away from the specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>The <see cref="T:System.DateTime" /> that results from adding the specified number of years to the specified <see cref="T:System.DateTime" />.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to which to add <paramref name="years" />. </param>
		/// <param name="years">The number of years to add. </param>
		/// <exception cref="T:System.ArgumentException">The resulting <see cref="T:System.DateTime" /> is outside the supported range. </exception>
		public override DateTime AddYears(DateTime time, int years)
		{
			int date = CCFixed.FromDateTime(time);
			int day;
			int month;
			int num;
			CCHebrewCalendar.dmy_from_fixed(out day, out month, out num, date);
			num += years;
			date = CCHebrewCalendar.fixed_from_dmy(day, month, num);
			DateTime dateTime = CCFixed.ToDateTime(date).Add(time.TimeOfDay);
			this.M_CheckDateTime(dateTime);
			return dateTime;
		}

		/// <summary>Returns the day of the month in the specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>An integer from 1 to 30 that represents the day of the month in the specified <see cref="T:System.DateTime" />.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to read. </param>
		public override int GetDayOfMonth(DateTime time)
		{
			this.M_CheckDateTime(time);
			int date = CCFixed.FromDateTime(time);
			return CCHebrewCalendar.day_from_fixed(date);
		}

		/// <summary>Returns the day of the week in the specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>A <see cref="T:System.DayOfWeek" /> value that represents the day of the week in the specified <see cref="T:System.DateTime" />.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to read. </param>
		public override DayOfWeek GetDayOfWeek(DateTime time)
		{
			this.M_CheckDateTime(time);
			int date = CCFixed.FromDateTime(time);
			return CCFixed.day_of_week(date);
		}

		/// <summary>Returns the day of the year in the specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>An integer from 1 to 385 that represents the day of the year in the specified <see cref="T:System.DateTime" />.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to read. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="time" /> is earlier than September 17, 1583 in the Gregorian calendar, or greater than <see cref="P:System.Globalization.HebrewCalendar.MaxSupportedDateTime" />. </exception>
		public override int GetDayOfYear(DateTime time)
		{
			this.M_CheckDateTime(time);
			int num = CCFixed.FromDateTime(time);
			int year = CCHebrewCalendar.year_from_fixed(num);
			int num2 = CCHebrewCalendar.fixed_from_dmy(1, 7, year);
			return num - num2 + 1;
		}

		internal int M_CCMonth(int month, int year)
		{
			if (month <= 6)
			{
				return 6 + month;
			}
			int num = CCHebrewCalendar.last_month_of_year(year);
			if (num == 12)
			{
				return month - 6;
			}
			return (month > 7) ? (month - 7) : (6 + month);
		}

		internal int M_Month(int ccmonth, int year)
		{
			if (ccmonth >= 7)
			{
				return ccmonth - 6;
			}
			int num = CCHebrewCalendar.last_month_of_year(year);
			return ccmonth + ((num != 12) ? 7 : 6);
		}

		/// <summary>Returns the number of days in the specified month in the specified year in the specified era.</summary>
		/// <returns>The number of days in the specified month in the specified year in the specified era.</returns>
		/// <param name="year">An integer that represents the year. </param>
		/// <param name="month">An integer from 1 to 13 that represents the month. </param>
		/// <param name="era">An integer that represents the era. Specify either <see cref="F:System.Globalization.HebrewCalendar.HebrewEra" /> or <see cref="F:System.Globalization.Calendar.CurrentEra" />.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" />, <paramref name="month" />, or <paramref name="era" /> is outside the range supported by the current <see cref="T:System.Globalization.HebrewCalendar" /> object. </exception>
		public override int GetDaysInMonth(int year, int month, int era)
		{
			this.M_CheckYME(year, month, ref era);
			int month2 = this.M_CCMonth(month, year);
			return CCHebrewCalendar.last_day_of_month(month2, year);
		}

		/// <summary>Returns the number of days in the specified year in the specified era.</summary>
		/// <returns>The number of days in the specified year in the specified era.</returns>
		/// <param name="year">An integer that represents the year. </param>
		/// <param name="era">An integer that represents the era. Specify either <see cref="F:System.Globalization.HebrewCalendar.HebrewEra" /> or <see cref="F:System.Globalization.Calendar.CurrentEra" />.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> or <paramref name="era" /> is outside the range supported by the current <see cref="T:System.Globalization.HebrewCalendar" /> object. </exception>
		public override int GetDaysInYear(int year, int era)
		{
			this.M_CheckYE(year, ref era);
			int num = CCHebrewCalendar.fixed_from_dmy(1, 7, year);
			int num2 = CCHebrewCalendar.fixed_from_dmy(1, 7, year + 1);
			return num2 - num;
		}

		/// <summary>Returns the era in the specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>An integer that represents the era in the specified <see cref="T:System.DateTime" />. The return value is always <see cref="F:System.Globalization.HebrewCalendar.HebrewEra" />.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to read. </param>
		public override int GetEra(DateTime time)
		{
			this.M_CheckDateTime(time);
			return HebrewCalendar.HebrewEra;
		}

		/// <summary>Calculates the leap month for a specified year and era.</summary>
		/// <returns>A positive integer that indicates the leap month in the specified year and era. The return value is 7 if the <paramref name="year" /> and <paramref name="era" /> parameters specify a leap year, or 0 if the year is not a leap year.</returns>
		/// <param name="year">A year.</param>
		/// <param name="era">An era. Specify either <see cref="F:System.Globalization.HebrewCalendar.HebrewEra" /> or <see cref="F:System.Globalization.Calendar.CurrentEra" />.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="era" /> is not <see cref="F:System.Globalization.HebrewCalendar.HebrewEra" /> or <see cref="F:System.Globalization.Calendar.CurrentEra" />.-or-<paramref name="year" /> is less than the Hebrew calendar year 5343 or greater than the Hebrew calendar year 5999.</exception>
		public override int GetLeapMonth(int year, int era)
		{
			return (!this.IsLeapMonth(year, 7, era)) ? 0 : 7;
		}

		/// <summary>Returns the month in the specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>An integer from 1 to 13 that represents the month in the specified <see cref="T:System.DateTime" />.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to read. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="time" /> is less than <see cref="P:System.Globalization.HebrewCalendar.MinSupportedDateTime" /> or greater than <see cref="P:System.Globalization.HebrewCalendar.MaxSupportedDateTime" />.</exception>
		public override int GetMonth(DateTime time)
		{
			this.M_CheckDateTime(time);
			int date = CCFixed.FromDateTime(time);
			int ccmonth;
			int year;
			CCHebrewCalendar.my_from_fixed(out ccmonth, out year, date);
			return this.M_Month(ccmonth, year);
		}

		/// <summary>Returns the number of months in the specified year in the specified era.</summary>
		/// <returns>The number of months in the specified year in the specified era. The return value is either 12 in a common year, or 13 in a leap year.</returns>
		/// <param name="year">An integer that represents the year. </param>
		/// <param name="era">An integer that represents the era. Specify either <see cref="F:System.Globalization.HebrewCalendar.HebrewEra" /> or <see cref="F:System.Globalization.Calendar.CurrentEra" />.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> or <paramref name="era" /> is outside the range supported by the current <see cref="T:System.Globalization.HebrewCalendar" /> object. </exception>
		public override int GetMonthsInYear(int year, int era)
		{
			this.M_CheckYE(year, ref era);
			return CCHebrewCalendar.last_month_of_year(year);
		}

		/// <summary>Returns the year in the specified <see cref="T:System.DateTime" /> value.</summary>
		/// <returns>An integer that represents the year in the specified <see cref="T:System.DateTime" /> value.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to read. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="time" /> is outside the range supported by the current <see cref="T:System.Globalization.HebrewCalendar" /> object. </exception>
		public override int GetYear(DateTime time)
		{
			this.M_CheckDateTime(time);
			int date = CCFixed.FromDateTime(time);
			return CCHebrewCalendar.year_from_fixed(date);
		}

		/// <summary>Determines whether the specified date in the specified era is a leap day.</summary>
		/// <returns>true if the specified day is a leap day; otherwise, false.</returns>
		/// <param name="year">An integer that represents the year. </param>
		/// <param name="month">An integer from 1 to 13 that represents the month. </param>
		/// <param name="day">An integer from 1 to 30 that represents the day. </param>
		/// <param name="era">An integer that represents the era. Specify either <see cref="F:System.Globalization.HebrewCalendar.HebrewEra" /> or <see cref="F:System.Globalization.Calendar.CurrentEra" />.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" />, <paramref name="month" />, <paramref name="day" />, or <paramref name="era" /> is outside the range supported by this calendar. </exception>
		public override bool IsLeapDay(int year, int month, int day, int era)
		{
			this.M_CheckYMDE(year, month, day, ref era);
			return this.IsLeapYear(year) && (month == 7 || (month == 6 && day == 30));
		}

		/// <summary>Determines whether the specified month in the specified year in the specified era is a leap month.</summary>
		/// <returns>true if the specified month is a leap month; otherwise, false.</returns>
		/// <param name="year">An integer that represents the year. </param>
		/// <param name="month">An integer from 1 to 13 that represents the month. </param>
		/// <param name="era">An integer that represents the era. Specify either <see cref="F:System.Globalization.HebrewCalendar.HebrewEra" /> or <see cref="F:System.Globalization.Calendar.CurrentEra" />.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" />, <paramref name="month" />, or <paramref name="era" /> is outside the range supported by this calendar. </exception>
		public override bool IsLeapMonth(int year, int month, int era)
		{
			this.M_CheckYME(year, month, ref era);
			return this.IsLeapYear(year) && month == 7;
		}

		/// <summary>Determines whether the specified year in the specified era is a leap year.</summary>
		/// <returns>true if the specified year is a leap year; otherwise, false.</returns>
		/// <param name="year">An integer that represents the year. </param>
		/// <param name="era">An integer that represents the era. Specify either <see cref="F:System.Globalization.HebrewCalendar.HebrewEra" /> or <see cref="F:System.Globalization.Calendar.CurrentEra" />.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> or <paramref name="era" /> is outside the range supported by this calendar. </exception>
		public override bool IsLeapYear(int year, int era)
		{
			this.M_CheckYE(year, ref era);
			return CCHebrewCalendar.is_leap_year(year);
		}

		/// <summary>Returns a <see cref="T:System.DateTime" /> that is set to the specified date and time in the specified era.</summary>
		/// <returns>The <see cref="T:System.DateTime" /> that is set to the specified date and time in the current era.</returns>
		/// <param name="year">An integer that represents the year. </param>
		/// <param name="month">An integer from 1 to 13 that represents the month. </param>
		/// <param name="day">An integer from 1 to 30 that represents the day. </param>
		/// <param name="hour">An integer from 0 to 23 that represents the hour. </param>
		/// <param name="minute">An integer from 0 to 59 that represents the minute. </param>
		/// <param name="second">An integer from 0 to 59 that represents the second. </param>
		/// <param name="millisecond">An integer from 0 to 999 that represents the millisecond. </param>
		/// <param name="era">An integer that represents the era. Specify either <see cref="F:System.Globalization.HebrewCalendar.HebrewEra" /> or <see cref="F:System.Globalization.Calendar.CurrentEra" />.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" />, <paramref name="month" />, <paramref name="day" /> or <paramref name="era" /> is outside the range supported by the current <see cref="T:System.Globalization.HebrewCalendar" /> object.-or- <paramref name="hour" /> is less than 0 or greater than 23.-or- <paramref name="minute" /> is less than 0 or greater than 59.-or- <paramref name="second" /> is less than 0 or greater than 59.-or- <paramref name="millisecond" /> is less than 0 or greater than 999. </exception>
		public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
		{
			this.M_CheckYMDE(year, month, day, ref era);
			base.M_CheckHMSM(hour, minute, second, millisecond);
			int month2 = this.M_CCMonth(month, year);
			int date = CCHebrewCalendar.fixed_from_dmy(day, month2, year);
			return CCFixed.ToDateTime(date, hour, minute, second, (double)millisecond);
		}

		/// <summary>Converts the specified year to a 4-digit year by using the <see cref="P:System.Globalization.HebrewCalendar.TwoDigitYearMax" /> property to determine the appropriate century.</summary>
		/// <returns>If the <paramref name="year" /> parameter is a 2-digit year, the return value is the corresponding 4-digit year. If the <paramref name="year" /> parameter is a 4-digit year, the return value is the unchanged <paramref name="year" /> parameter.</returns>
		/// <param name="year">A 2-digit year from 0 through 99, or a 4-digit Hebrew calendar year from 5343 through 5999.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> is less than 0.-or-<paramref name="year" /> is less than <see cref="P:System.Globalization.HebrewCalendar.MinSupportedDateTime" /> or greater than <see cref="P:System.Globalization.HebrewCalendar.MaxSupportedDateTime" />. </exception>
		public override int ToFourDigitYear(int year)
		{
			base.M_ArgumentInRange("year", year, 0, this.M_MaxYear - 1);
			int num = this.twoDigitYearMax % 100;
			int num2 = this.twoDigitYearMax - num;
			if (year >= 100)
			{
				return year;
			}
			if (year <= num)
			{
				return num2 + year;
			}
			return num2 + year - 100;
		}

		/// <summary>Gets the earliest date and time supported by the <see cref="T:System.Globalization.HebrewCalendar" /> type.</summary>
		/// <returns>The earliest date and time supported by the <see cref="T:System.Globalization.HebrewCalendar" /> type, which is equivalent to the first moment of January, 1, 1583 C.E. in the Gregorian calendar.</returns>
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return HebrewCalendar.Min;
			}
		}

		/// <summary>Gets the latest date and time supported by the <see cref="T:System.Globalization.HebrewCalendar" /> type.</summary>
		/// <returns>The latest date and time supported by the <see cref="T:System.Globalization.HebrewCalendar" /> type, which is equivalent to the last moment of September, 29, 2239 C.E. in the Gregorian calendar.</returns>
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return HebrewCalendar.Max;
			}
		}
	}
}
