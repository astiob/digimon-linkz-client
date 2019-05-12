using System;

namespace System.Globalization
{
	/// <summary>Represents the Persian calendar.</summary>
	[Serializable]
	public class PersianCalendar : Calendar
	{
		internal const long M_MinTicks = 196036416000000000L;

		internal const int M_MinYear = 1;

		internal const int epoch = 226895;

		/// <summary>Represents the current era. This field is constant.</summary>
		public static readonly int PersianEra = 1;

		private static DateTime PersianMin = new DateTime(622, 3, 21, 0, 0, 0);

		private static DateTime PersianMax = new DateTime(9999, 12, 31, 11, 59, 59);

		/// <summary>Initializes a new instance of the <see cref="T:System.Globalization.PersianCalendar" /> class. </summary>
		public PersianCalendar()
		{
			this.M_AbbrEraNames = new string[]
			{
				"A.P."
			};
			this.M_EraNames = new string[]
			{
				"Anno Persico"
			};
			if (this.twoDigitYearMax == 99)
			{
				this.twoDigitYearMax = 1410;
			}
		}

		/// <summary>Gets the list of eras in a <see cref="T:System.Globalization.PersianCalendar" /> object.</summary>
		/// <returns>An array of integers that represents the eras in a <see cref="T:System.Globalization.PersianCalendar" /> object. The array consists of a single element having a value of <see cref="F:System.Globalization.PersianCalendar.PersianEra" />.</returns>
		public override int[] Eras
		{
			get
			{
				return new int[]
				{
					PersianCalendar.PersianEra
				};
			}
		}

		/// <summary>Gets or sets the last year of a 100-year range that can be represented by a 2-digit year.</summary>
		/// <returns>The last year of a 100-year range that can be represented by a 2-digit year.</returns>
		/// <exception cref="T:System.InvalidOperationException">This calendar is read-only.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value in a set operation is less than 100 or greater than 9378.</exception>
		public override int TwoDigitYearMax
		{
			get
			{
				return this.twoDigitYearMax;
			}
			set
			{
				base.CheckReadOnly();
				base.M_ArgumentInRange("value", value, 100, this.M_MaxYear);
				this.twoDigitYearMax = value;
			}
		}

		internal void M_CheckDateTime(DateTime time)
		{
			if (time.Ticks < 196036416000000000L)
			{
				throw new ArgumentOutOfRangeException("time", "Only positive Persian years are supported.");
			}
		}

		internal void M_CheckEra(ref int era)
		{
			if (era == 0)
			{
				era = PersianCalendar.PersianEra;
			}
			if (era != PersianCalendar.PersianEra)
			{
				throw new ArgumentException("Era value was not valid.");
			}
		}

		internal override void M_CheckYE(int year, ref int era)
		{
			this.M_CheckEra(ref era);
			if (year < 1 || year > this.M_MaxYear)
			{
				throw new ArgumentOutOfRangeException("year", "Only Persian years between 1 and 9378, inclusive, are supported.");
			}
		}

		internal void M_CheckYME(int year, int month, ref int era)
		{
			this.M_CheckYE(year, ref era);
			if (month < 1 || month > 12)
			{
				throw new ArgumentOutOfRangeException("month", "Month must be between one and twelve.");
			}
			if (year == this.M_MaxYear && month > 10)
			{
				throw new ArgumentOutOfRangeException("month", "Months in year 9378 must be between one and ten.");
			}
		}

		internal void M_CheckYMDE(int year, int month, int day, ref int era)
		{
			this.M_CheckYME(year, month, ref era);
			base.M_ArgumentInRange("day", day, 1, this.GetDaysInMonth(year, month, era));
			if (year == this.M_MaxYear && month == 10 && day > 10)
			{
				throw new ArgumentOutOfRangeException("day", "Days in month 10 of year 9378 must be between one and ten.");
			}
		}

		internal int fixed_from_dmy(int day, int month, int year)
		{
			int num = 226894;
			num += 365 * (year - 1);
			num += (8 * year + 21) / 33;
			if (month <= 7)
			{
				num += 31 * (month - 1);
			}
			else
			{
				num += 30 * (month - 1) + 6;
			}
			return num + day;
		}

		internal int year_from_fixed(int date)
		{
			return (33 * (date - 226895) + 3) / 12053 + 1;
		}

		internal void my_from_fixed(out int month, out int year, int date)
		{
			year = this.year_from_fixed(date);
			int num = date - this.fixed_from_dmy(1, 1, year);
			if (num < 216)
			{
				month = num / 31 + 1;
			}
			else
			{
				month = (num - 6) / 30 + 1;
			}
		}

		internal void dmy_from_fixed(out int day, out int month, out int year, int date)
		{
			year = this.year_from_fixed(date);
			day = date - this.fixed_from_dmy(1, 1, year);
			if (day < 216)
			{
				month = day / 31 + 1;
				day = day % 31 + 1;
			}
			else
			{
				month = (day - 6) / 30 + 1;
				day = (day - 6) % 30 + 1;
			}
		}

		internal bool is_leap_year(int year)
		{
			return (25 * year + 11) % 33 < 8;
		}

		/// <summary>Returns a <see cref="T:System.DateTime" /> that is offset the specified number of months from the specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> that represents the date yielded by adding the number of months specified by the <paramref name="months" /> parameter to the date specified by the <paramref name="time" /> parameter.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to which to add months. </param>
		/// <param name="months">The positive or negative number of months to add. </param>
		/// <exception cref="T:System.ArgumentException">The resulting <see cref="T:System.DateTime" /> is outside the supported range. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="months" /> is less than -120,000 or greater than 120,000. </exception>
		public override DateTime AddMonths(DateTime time, int months)
		{
			int date = CCFixed.FromDateTime(time);
			int day;
			int num;
			int num2;
			this.dmy_from_fixed(out day, out num, out num2, date);
			num += months;
			num2 += CCMath.div_mod(out num, num, 12);
			date = this.fixed_from_dmy(day, num, num2);
			DateTime dateTime = CCFixed.ToDateTime(date).Add(time.TimeOfDay);
			this.M_CheckDateTime(dateTime);
			return dateTime;
		}

		/// <summary>Returns a <see cref="T:System.DateTime" /> that is offset the specified number of years from the specified <see cref="T:System.DateTime" /> object.</summary>
		/// <returns>The <see cref="T:System.DateTime" /> that results from adding the specified number of years to the specified <see cref="T:System.DateTime" /> object.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to which to add years. </param>
		/// <param name="years">The positive or negative number of years to add. </param>
		/// <exception cref="T:System.ArgumentException">The resulting <see cref="T:System.DateTime" /> is outside the supported range. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="years" /> is less than -10,000 or greater than 10,000. </exception>
		public override DateTime AddYears(DateTime time, int years)
		{
			int date = CCFixed.FromDateTime(time);
			int day;
			int month;
			int num;
			this.dmy_from_fixed(out day, out month, out num, date);
			num += years;
			date = this.fixed_from_dmy(day, month, num);
			DateTime dateTime = CCFixed.ToDateTime(date).Add(time.TimeOfDay);
			this.M_CheckDateTime(dateTime);
			return dateTime;
		}

		/// <summary>Returns the day of the month in the specified <see cref="T:System.DateTime" /> object.</summary>
		/// <returns>An integer from 1 through 31 that represents the day of the month in the specified <see cref="T:System.DateTime" /> object.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to read. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="time" /> parameter represents a date less than <see cref="P:System.Globalization.PersianCalendar.MinSupportedDateTime" /> or greater than <see cref="P:System.Globalization.PersianCalendar.MaxSupportedDateTime" />.</exception>
		public override int GetDayOfMonth(DateTime time)
		{
			this.M_CheckDateTime(time);
			int date = CCFixed.FromDateTime(time);
			int result;
			int num;
			int num2;
			this.dmy_from_fixed(out result, out num, out num2, date);
			return result;
		}

		/// <summary>Returns the day of the week in the specified <see cref="T:System.DateTime" /> object.</summary>
		/// <returns>A <see cref="T:System.DayOfWeek" /> value that represents the day of the week in the specified <see cref="T:System.DateTime" /> object.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to read. </param>
		public override DayOfWeek GetDayOfWeek(DateTime time)
		{
			this.M_CheckDateTime(time);
			int date = CCFixed.FromDateTime(time);
			return CCFixed.day_of_week(date);
		}

		/// <summary>Returns the day of the year in the specified <see cref="T:System.DateTime" /> object.</summary>
		/// <returns>An integer from 1 through 366 that represents the day of the year in the specified <see cref="T:System.DateTime" /> object.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to read. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="time" /> parameter represents a date less than <see cref="P:System.Globalization.PersianCalendar.MinSupportedDateTime" /> or greater than <see cref="P:System.Globalization.PersianCalendar.MaxSupportedDateTime" />.</exception>
		public override int GetDayOfYear(DateTime time)
		{
			this.M_CheckDateTime(time);
			int num = CCFixed.FromDateTime(time);
			int year = this.year_from_fixed(num);
			int num2 = this.fixed_from_dmy(1, 1, year);
			return num - num2 + 1;
		}

		/// <summary>Returns the number of days in the specified month of the specified year and era.</summary>
		/// <returns>The number of days in the specified month of the specified year and era.</returns>
		/// <param name="year">An integer from 1 through 9378 that represents the year. </param>
		/// <param name="month">An integer that represents the month, and ranges from 1 through 12 if <paramref name="year" /> is not 9378, or 1 through 10 if <paramref name="year" /> is 9378.</param>
		/// <param name="era">An integer from 0 through 1 that represents the era. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" />, <paramref name="month" />, or <paramref name="era" /> is outside the range supported by this calendar. </exception>
		public override int GetDaysInMonth(int year, int month, int era)
		{
			this.M_CheckYME(year, month, ref era);
			if (month <= 6)
			{
				return 31;
			}
			if (month == 12 && !this.is_leap_year(year))
			{
				return 29;
			}
			return 30;
		}

		/// <summary>Returns the number of days in the specified year of the specified era.</summary>
		/// <returns>The number of days in the specified year and era. The number of days is 365 in a common year or 366 in a leap year.</returns>
		/// <param name="year">An integer from 1 through 9378 that represents the year. </param>
		/// <param name="era">An integer from 0 through 1 that represents the era. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> or <paramref name="era" /> is outside the range supported by this calendar. </exception>
		public override int GetDaysInYear(int year, int era)
		{
			this.M_CheckYE(year, ref era);
			return (!this.is_leap_year(year)) ? 365 : 366;
		}

		/// <summary>Returns the era in the specified <see cref="T:System.DateTime" /> object.</summary>
		/// <returns>Always returns <see cref="F:System.Globalization.PersianCalendar.PersianEra" />.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to read. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="time" /> parameter represents a date less than <see cref="P:System.Globalization.PersianCalendar.MinSupportedDateTime" /> or greater than <see cref="P:System.Globalization.PersianCalendar.MaxSupportedDateTime" />.</exception>
		public override int GetEra(DateTime time)
		{
			this.M_CheckDateTime(time);
			return PersianCalendar.PersianEra;
		}

		/// <summary>Returns the leap month for a specified year and era.</summary>
		/// <returns>The return value is always 0.</returns>
		/// <param name="year">An integer from 1 through 9378 that represents the year to convert. </param>
		/// <param name="era">An integer from 0 through 1 that represents the era. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> or <paramref name="era" /> is outside the range supported by this calendar. </exception>
		public override int GetLeapMonth(int year, int era)
		{
			return 0;
		}

		/// <summary>Returns the month in the specified <see cref="T:System.DateTime" /> object.</summary>
		/// <returns>An integer from 1 through 12 that represents the month in the specified <see cref="T:System.DateTime" /> object.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to read. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="time" /> parameter represents a date less than <see cref="P:System.Globalization.PersianCalendar.MinSupportedDateTime" /> or greater than <see cref="P:System.Globalization.PersianCalendar.MaxSupportedDateTime" />.</exception>
		public override int GetMonth(DateTime time)
		{
			this.M_CheckDateTime(time);
			int date = CCFixed.FromDateTime(time);
			int result;
			int num;
			this.my_from_fixed(out result, out num, date);
			return result;
		}

		/// <summary>Returns the number of months in the specified year of the specified era.</summary>
		/// <returns>Returns 10 if the <paramref name="year" /> parameter is 9378; otherwise, always returns 12.</returns>
		/// <param name="year">An integer from 1 through 9378 that represents the year. </param>
		/// <param name="era">An integer from 0 through 1 that represents the era. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> or <paramref name="era" /> is outside the range supported by this calendar. </exception>
		public override int GetMonthsInYear(int year, int era)
		{
			this.M_CheckYE(year, ref era);
			return 12;
		}

		/// <summary>Returns the year in the specified <see cref="T:System.DateTime" /> object.</summary>
		/// <returns>An integer from 1 through 9378 that represents the year in the specified <see cref="T:System.DateTime" />. </returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to read. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="time" /> parameter represents a date less than <see cref="P:System.Globalization.PersianCalendar.MinSupportedDateTime" /> or greater than <see cref="P:System.Globalization.PersianCalendar.MaxSupportedDateTime" />.</exception>
		public override int GetYear(DateTime time)
		{
			this.M_CheckDateTime(time);
			int date = CCFixed.FromDateTime(time);
			return this.year_from_fixed(date);
		}

		/// <summary>Determines whether the specified date is a leap day.</summary>
		/// <returns>true if the specified day is a leap day; otherwise, false.</returns>
		/// <param name="year">An integer from 1 through 9378 that represents the year. </param>
		/// <param name="month">An integer that represents the month and ranges from 1 through 12 if <paramref name="year" /> is not 9378, or 1 through 10 if <paramref name="year" /> is 9378.</param>
		/// <param name="day">An integer from 1 through 31 that represents the day. </param>
		/// <param name="era">An integer from 0 through 1 that represents the era. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" />, <paramref name="month" />, <paramref name="day" />, or <paramref name="era" /> is outside the range supported by this calendar. </exception>
		public override bool IsLeapDay(int year, int month, int day, int era)
		{
			this.M_CheckYMDE(year, month, day, ref era);
			return this.is_leap_year(year) && month == 12 && day == 30;
		}

		/// <summary>Determines whether the specified month in the specified year and era is a leap month.</summary>
		/// <returns>Always returns false because the <see cref="T:System.Globalization.PersianCalendar" /> class does not support the notion of a leap month.</returns>
		/// <param name="year">An integer from 1 through 9378 that represents the year. </param>
		/// <param name="month">An integer that represents the month and ranges from 1 through 12 if <paramref name="year" /> is not 9378, or 1 through 10 if <paramref name="year" /> is 9378.</param>
		/// <param name="era">An integer from 0 through 1 that represents the era. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" />, <paramref name="month" />, or <paramref name="era" /> is outside the range supported by this calendar. </exception>
		public override bool IsLeapMonth(int year, int month, int era)
		{
			this.M_CheckYME(year, month, ref era);
			return false;
		}

		/// <summary>Determines whether the specified year in the specified era is a leap year.</summary>
		/// <returns>true if the specified year is a leap year; otherwise, false.</returns>
		/// <param name="year">An integer from 1 through 9378 that represents the year. </param>
		/// <param name="era">An integer from 0 through 1 that represents the era. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> or <paramref name="era" /> is outside the range supported by this calendar. </exception>
		public override bool IsLeapYear(int year, int era)
		{
			this.M_CheckYE(year, ref era);
			return this.is_leap_year(year);
		}

		/// <summary>Returns a <see cref="T:System.DateTime" /> that is set to the specified date, time, and era.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> that is set to the specified date and time in the current era.</returns>
		/// <param name="year">An integer from 1 through 9378 that represents the year. </param>
		/// <param name="month">An integer from 1 through 12 that represents the month. </param>
		/// <param name="day">An integer from 1 through 31 that represents the day. </param>
		/// <param name="hour">An integer from 0 through 23 that represents the hour. </param>
		/// <param name="minute">An integer from 0 through 59 that represents the minute. </param>
		/// <param name="second">An integer from 0 through 59 that represents the second. </param>
		/// <param name="millisecond">An integer from 0 through 999 that represents the millisecond. </param>
		/// <param name="era">An integer from 0 through 1 that represents the era. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" />, <paramref name="month" />, <paramref name="day" />, <paramref name="hour" />, <paramref name="minute" />, <paramref name="second" />, <paramref name="millisecond" />, or <paramref name="era" /> is outside the range supported by this calendar.</exception>
		public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
		{
			this.M_CheckYMDE(year, month, day, ref era);
			base.M_CheckHMSM(hour, minute, second, millisecond);
			int date = this.fixed_from_dmy(day, month, year);
			return CCFixed.ToDateTime(date, hour, minute, second, (double)millisecond);
		}

		/// <summary>Converts the specified year to a four-digit year representation.</summary>
		/// <returns>An integer that contains the four-digit representation of <paramref name="year" />.</returns>
		/// <param name="year">An integer from 1 through 9378 that represents the year to convert. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> is less than 0 or greater than 9378. </exception>
		public override int ToFourDigitYear(int year)
		{
			base.M_ArgumentInRange("year", year, 0, 99);
			int num = this.twoDigitYearMax % 100;
			int num2 = this.twoDigitYearMax - num;
			if (year <= num)
			{
				return num2 + year;
			}
			return num2 + year - 100;
		}

		/// <summary>Gets a value indicating whether the current calendar is solar-based, lunar-based, or lunisolar-based.</summary>
		/// <returns>Always returns <see cref="F:System.Globalization.CalendarAlgorithmType.SolarCalendar" />.</returns>
		public override CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.SolarCalendar;
			}
		}

		/// <summary>Gets the earliest date and time supported by the <see cref="T:System.Globalization.PersianCalendar" /> class.</summary>
		/// <returns>The earliest date and time supported by the <see cref="T:System.Globalization.PersianCalendar" /> class, which is equivalent to the first moment of March 21, 622 C.E. in the Gregorian calendar.</returns>
		public override DateTime MinSupportedDateTime
		{
			get
			{
				return PersianCalendar.PersianMin;
			}
		}

		/// <summary>Gets the latest date and time supported by the <see cref="T:System.Globalization.PersianCalendar" /> class.</summary>
		/// <returns>The latest date and time supported by the <see cref="T:System.Globalization.PersianCalendar" /> class, which is equivalent to the last moment of December 31, 9999 C.E. in the Gregorian calendar.</returns>
		public override DateTime MaxSupportedDateTime
		{
			get
			{
				return PersianCalendar.PersianMax;
			}
		}
	}
}
