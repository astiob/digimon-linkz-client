using System;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	/// <summary>Represents time in divisions, such as weeks, months, and years.</summary>
	[ComVisible(true)]
	[Serializable]
	public abstract class Calendar : ICloneable
	{
		/// <summary>Represents the current era of the current calendar. </summary>
		public const int CurrentEra = 0;

		[NonSerialized]
		private bool m_isReadOnly;

		[NonSerialized]
		internal int twoDigitYearMax;

		[NonSerialized]
		private int M_MaxYearValue;

		[NonSerialized]
		internal string[] M_AbbrEraNames;

		[NonSerialized]
		internal string[] M_EraNames;

		internal int m_currentEraValue;

		/// <summary>Initializes a new instance of the <see cref="T:System.Globalization.Calendar" /> class.</summary>
		protected Calendar()
		{
			this.twoDigitYearMax = 99;
		}

		internal virtual int M_DaysInWeek
		{
			get
			{
				return 7;
			}
		}

		internal string M_ValidValues(object a, object b)
		{
			StringWriter stringWriter = new StringWriter();
			stringWriter.Write("Valid values are between {0} and {1}, inclusive.", a, b);
			return stringWriter.ToString();
		}

		internal void M_ArgumentInRange(string param, int arg, int a, int b)
		{
			if (a <= arg && arg <= b)
			{
				return;
			}
			throw new ArgumentOutOfRangeException(param, this.M_ValidValues(a, b));
		}

		internal void M_CheckHMSM(int hour, int minute, int second, int milliseconds)
		{
			this.M_ArgumentInRange("hour", hour, 0, 23);
			this.M_ArgumentInRange("minute", minute, 0, 59);
			this.M_ArgumentInRange("second", second, 0, 59);
			this.M_ArgumentInRange("milliseconds", milliseconds, 0, 999999);
		}

		/// <summary>When overridden in a derived class, gets the list of eras in the current calendar.</summary>
		/// <returns>An array of integers that represents the eras in the current calendar.</returns>
		public abstract int[] Eras { get; }

		/// <summary>Gets a value indicating whether the current calendar is solar-based, lunar-based, or a combination of both.</summary>
		/// <returns>One of the <see cref="T:System.Globalization.CalendarAlgorithmType" /> values.</returns>
		[ComVisible(false)]
		public virtual CalendarAlgorithmType AlgorithmType
		{
			get
			{
				return CalendarAlgorithmType.Unknown;
			}
		}

		/// <summary>Gets the latest date and time supported by this <see cref="T:System.Globalization.Calendar" /> object.</summary>
		/// <returns>The latest date and time supported by this calendar. The default is <see cref="F:System.DateTime.MaxValue" />.</returns>
		[ComVisible(false)]
		public virtual DateTime MaxSupportedDateTime
		{
			get
			{
				return DateTime.MaxValue;
			}
		}

		/// <summary>Gets the earliest date and time supported by this <see cref="T:System.Globalization.Calendar" /> object.</summary>
		/// <returns>The earliest date and time supported by this calendar. The default is <see cref="F:System.DateTime.MinValue" />.</returns>
		[ComVisible(false)]
		public virtual DateTime MinSupportedDateTime
		{
			get
			{
				return DateTime.MinValue;
			}
		}

		/// <summary>Creates a new object that is a copy of the current <see cref="T:System.Globalization.Calendar" /> object.</summary>
		/// <returns>A new instance of <see cref="T:System.Object" /> that is the memberwise clone of the current <see cref="T:System.Globalization.Calendar" /> object.</returns>
		[ComVisible(false)]
		public virtual object Clone()
		{
			Calendar calendar = (Calendar)base.MemberwiseClone();
			calendar.m_isReadOnly = false;
			return calendar;
		}

		/// <summary>Calculates the leap month for a specified year.</summary>
		/// <returns>A positive integer that indicates the leap month in the specified year.-or-Zero if this calendar does not support a leap month or if the <paramref name="year" /> parameter does not represent a leap year.</returns>
		/// <param name="year">A year.</param>
		[ComVisible(false)]
		public virtual int GetLeapMonth(int year)
		{
			return this.GetLeapMonth(year, this.GetEra(this.ToDateTime(year, 1, 1, 0, 0, 0, 0)));
		}

		/// <summary>Calculates the leap month for a specified year and era.</summary>
		/// <returns>A positive integer that indicates the leap month in the specified year and era.-or-Zero if this calendar does not support a leap month or if the <paramref name="year" /> and <paramref name="era" /> parameters do not specify a leap year.</returns>
		/// <param name="year">A year.</param>
		/// <param name="era">An era.</param>
		[ComVisible(false)]
		public virtual int GetLeapMonth(int year, int era)
		{
			int monthsInYear = this.GetMonthsInYear(year, era);
			for (int i = 1; i <= monthsInYear; i++)
			{
				if (this.IsLeapMonth(year, i, era))
				{
					return i;
				}
			}
			return 0;
		}

		/// <summary>Gets a value indicating whether this <see cref="T:System.Globalization.Calendar" /> object is read-only.</summary>
		/// <returns>true if this <see cref="T:System.Globalization.Calendar" /> object is read-only; otherwise, false.</returns>
		[ComVisible(false)]
		public bool IsReadOnly
		{
			get
			{
				return this.m_isReadOnly;
			}
		}

		/// <summary>Returns a read-only version of the specified <see cref="T:System.Globalization.Calendar" /> object.</summary>
		/// <returns>The <see cref="T:System.Globalization.Calendar" /> object specified by the <paramref name="calendar" /> parameter, if <paramref name="calendar" /> is read-only.-or-A read-only memberwise clone of the <see cref="T:System.Globalization.Calendar" /> object specified by <paramref name="calendar" />, if <paramref name="calendar" /> is not read-only.</returns>
		/// <param name="calendar">A <see cref="T:System.Globalization.Calendar" /> object.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="calendar" /> is null.</exception>
		[ComVisible(false)]
		public static Calendar ReadOnly(Calendar calendar)
		{
			if (calendar.m_isReadOnly)
			{
				return calendar;
			}
			Calendar calendar2 = (Calendar)calendar.Clone();
			calendar2.m_isReadOnly = true;
			return calendar2;
		}

		internal void CheckReadOnly()
		{
			if (this.m_isReadOnly)
			{
				throw new InvalidOperationException("This Calendar is read-only.");
			}
		}

		internal virtual int M_MaxYear
		{
			get
			{
				if (this.M_MaxYearValue == 0)
				{
					this.M_MaxYearValue = this.GetYear(DateTime.MaxValue);
				}
				return this.M_MaxYearValue;
			}
		}

		internal virtual void M_CheckYE(int year, ref int era)
		{
		}

		/// <summary>Gets or sets the last year of a 100-year range that can be represented by a 2-digit year.</summary>
		/// <returns>The last year of a 100-year range that can be represented by a 2-digit year.</returns>
		/// <exception cref="T:System.InvalidOperationException">The current <see cref="T:System.Globalization.Calendar" /> object is read-only.</exception>
		public virtual int TwoDigitYearMax
		{
			get
			{
				return this.twoDigitYearMax;
			}
			set
			{
				this.CheckReadOnly();
				this.M_ArgumentInRange("year", value, 100, this.M_MaxYear);
				int num = 0;
				this.M_CheckYE(value, ref num);
				this.twoDigitYearMax = value;
			}
		}

		/// <summary>Returns a <see cref="T:System.DateTime" /> that is the specified number of days away from the specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>The <see cref="T:System.DateTime" /> that results from adding the specified number of days to the specified <see cref="T:System.DateTime" />.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to which to add days. </param>
		/// <param name="days">The number of days to add. </param>
		/// <exception cref="T:System.ArgumentException">The resulting <see cref="T:System.DateTime" /> is outside the supported range of this calendar. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="days" /> is outside the supported range of the <see cref="T:System.DateTime" /> return value. </exception>
		public virtual DateTime AddDays(DateTime time, int days)
		{
			return time.Add(TimeSpan.FromDays((double)days));
		}

		/// <summary>Returns a <see cref="T:System.DateTime" /> that is the specified number of hours away from the specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>The <see cref="T:System.DateTime" /> that results from adding the specified number of hours to the specified <see cref="T:System.DateTime" />.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to which to add hours. </param>
		/// <param name="hours">The number of hours to add. </param>
		/// <exception cref="T:System.ArgumentException">The resulting <see cref="T:System.DateTime" /> is outside the supported range of this calendar. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="hours" /> is outside the supported range of the <see cref="T:System.DateTime" /> return value. </exception>
		public virtual DateTime AddHours(DateTime time, int hours)
		{
			return time.Add(TimeSpan.FromHours((double)hours));
		}

		/// <summary>Returns a <see cref="T:System.DateTime" /> that is the specified number of milliseconds away from the specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>The <see cref="T:System.DateTime" /> that results from adding the specified number of milliseconds to the specified <see cref="T:System.DateTime" />.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to add milliseconds to. </param>
		/// <param name="milliseconds">The number of milliseconds to add.</param>
		/// <exception cref="T:System.ArgumentException">The resulting <see cref="T:System.DateTime" /> is outside the supported range of this calendar. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="milliseconds" /> is outside the supported range of the <see cref="T:System.DateTime" /> return value. </exception>
		public virtual DateTime AddMilliseconds(DateTime time, double milliseconds)
		{
			return time.Add(TimeSpan.FromMilliseconds(milliseconds));
		}

		/// <summary>Returns a <see cref="T:System.DateTime" /> that is the specified number of minutes away from the specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>The <see cref="T:System.DateTime" /> that results from adding the specified number of minutes to the specified <see cref="T:System.DateTime" />.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to which to add minutes. </param>
		/// <param name="minutes">The number of minutes to add. </param>
		/// <exception cref="T:System.ArgumentException">The resulting <see cref="T:System.DateTime" /> is outside the supported range of this calendar. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="minutes" /> is outside the supported range of the <see cref="T:System.DateTime" /> return value. </exception>
		public virtual DateTime AddMinutes(DateTime time, int minutes)
		{
			return time.Add(TimeSpan.FromMinutes((double)minutes));
		}

		/// <summary>When overridden in a derived class, returns a <see cref="T:System.DateTime" /> that is the specified number of months away from the specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>The <see cref="T:System.DateTime" /> that results from adding the specified number of months to the specified <see cref="T:System.DateTime" />.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to which to add months. </param>
		/// <param name="months">The number of months to add. </param>
		/// <exception cref="T:System.ArgumentException">The resulting <see cref="T:System.DateTime" /> is outside the supported range of this calendar. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="months" /> is outside the supported range of the <see cref="T:System.DateTime" /> return value. </exception>
		public abstract DateTime AddMonths(DateTime time, int months);

		/// <summary>Returns a <see cref="T:System.DateTime" /> that is the specified number of seconds away from the specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>The <see cref="T:System.DateTime" /> that results from adding the specified number of seconds to the specified <see cref="T:System.DateTime" />.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to which to add seconds. </param>
		/// <param name="seconds">The number of seconds to add. </param>
		/// <exception cref="T:System.ArgumentException">The resulting <see cref="T:System.DateTime" /> is outside the supported range of this calendar. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="seconds" /> is outside the supported range of the <see cref="T:System.DateTime" /> return value. </exception>
		public virtual DateTime AddSeconds(DateTime time, int seconds)
		{
			return time.Add(TimeSpan.FromSeconds((double)seconds));
		}

		/// <summary>Returns a <see cref="T:System.DateTime" /> that is the specified number of weeks away from the specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>The <see cref="T:System.DateTime" /> that results from adding the specified number of weeks to the specified <see cref="T:System.DateTime" />.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to which to add weeks. </param>
		/// <param name="weeks">The number of weeks to add. </param>
		/// <exception cref="T:System.ArgumentException">The resulting <see cref="T:System.DateTime" /> is outside the supported range of this calendar. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="weeks" /> is outside the supported range of the <see cref="T:System.DateTime" /> return value. </exception>
		public virtual DateTime AddWeeks(DateTime time, int weeks)
		{
			return time.AddDays((double)(weeks * this.M_DaysInWeek));
		}

		/// <summary>When overridden in a derived class, returns a <see cref="T:System.DateTime" /> that is the specified number of years away from the specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>The <see cref="T:System.DateTime" /> that results from adding the specified number of years to the specified <see cref="T:System.DateTime" />.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to which to add years. </param>
		/// <param name="years">The number of years to add. </param>
		/// <exception cref="T:System.ArgumentException">The resulting <see cref="T:System.DateTime" /> is outside the supported range of this calendar. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="years" /> is outside the supported range of the <see cref="T:System.DateTime" /> return value. </exception>
		public abstract DateTime AddYears(DateTime time, int years);

		/// <summary>When overridden in a derived class, returns the day of the month in the specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>A positive integer that represents the day of the month in the <paramref name="time" /> parameter.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to read. </param>
		public abstract int GetDayOfMonth(DateTime time);

		/// <summary>When overridden in a derived class, returns the day of the week in the specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>A <see cref="T:System.DayOfWeek" /> value that represents the day of the week in the <paramref name="time" /> parameter.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to read. </param>
		public abstract DayOfWeek GetDayOfWeek(DateTime time);

		/// <summary>When overridden in a derived class, returns the day of the year in the specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>A positive integer that represents the day of the year in the <paramref name="time" /> parameter.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to read. </param>
		public abstract int GetDayOfYear(DateTime time);

		/// <summary>Returns the number of days in the specified month and year of the current era.</summary>
		/// <returns>The number of days in the specified month in the specified year in the current era.</returns>
		/// <param name="year">An integer that represents the year. </param>
		/// <param name="month">A positive integer that represents the month. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> is outside the range supported by the calendar.-or- <paramref name="month" /> is outside the range supported by the calendar. </exception>
		public virtual int GetDaysInMonth(int year, int month)
		{
			return this.GetDaysInMonth(year, month, 0);
		}

		/// <summary>When overridden in a derived class, returns the number of days in the specified month, year, and era.</summary>
		/// <returns>The number of days in the specified month in the specified year in the specified era.</returns>
		/// <param name="year">An integer that represents the year. </param>
		/// <param name="month">A positive integer that represents the month. </param>
		/// <param name="era">An integer that represents the era. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> is outside the range supported by the calendar.-or- <paramref name="month" /> is outside the range supported by the calendar.-or- <paramref name="era" /> is outside the range supported by the calendar. </exception>
		public abstract int GetDaysInMonth(int year, int month, int era);

		/// <summary>Returns the number of days in the specified year of the current era.</summary>
		/// <returns>The number of days in the specified year in the current era.</returns>
		/// <param name="year">An integer that represents the year. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> is outside the range supported by the calendar. </exception>
		public virtual int GetDaysInYear(int year)
		{
			return this.GetDaysInYear(year, 0);
		}

		/// <summary>When overridden in a derived class, returns the number of days in the specified year and era.</summary>
		/// <returns>The number of days in the specified year in the specified era.</returns>
		/// <param name="year">An integer that represents the year. </param>
		/// <param name="era">An integer that represents the era. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> is outside the range supported by the calendar.-or- <paramref name="era" /> is outside the range supported by the calendar. </exception>
		public abstract int GetDaysInYear(int year, int era);

		/// <summary>When overridden in a derived class, returns the era in the specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>An integer that represents the era in <paramref name="time" />.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to read. </param>
		public abstract int GetEra(DateTime time);

		/// <summary>Returns the hours value in the specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>An integer from 0 to 23 that represents the hour in <paramref name="time" />.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to read. </param>
		public virtual int GetHour(DateTime time)
		{
			return time.TimeOfDay.Hours;
		}

		/// <summary>Returns the milliseconds value in the specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>A double-precision floating-point number from 0 to 999 that represents the milliseconds in the <paramref name="time" /> parameter.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to read. </param>
		public virtual double GetMilliseconds(DateTime time)
		{
			return (double)time.TimeOfDay.Milliseconds;
		}

		/// <summary>Returns the minutes value in the specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>An integer from 0 to 59 that represents the minutes in <paramref name="time" />.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to read. </param>
		public virtual int GetMinute(DateTime time)
		{
			return time.TimeOfDay.Minutes;
		}

		/// <summary>When overridden in a derived class, returns the month in the specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>A positive integer that represents the month in <paramref name="time" />.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to read. </param>
		public abstract int GetMonth(DateTime time);

		/// <summary>Returns the number of months in the specified year in the current era.</summary>
		/// <returns>The number of months in the specified year in the current era.</returns>
		/// <param name="year">An integer that represents the year. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> is outside the range supported by the calendar. </exception>
		public virtual int GetMonthsInYear(int year)
		{
			return this.GetMonthsInYear(year, 0);
		}

		/// <summary>When overridden in a derived class, returns the number of months in the specified year in the specified era.</summary>
		/// <returns>The number of months in the specified year in the specified era.</returns>
		/// <param name="year">An integer that represents the year. </param>
		/// <param name="era">An integer that represents the era. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> is outside the range supported by the calendar.-or- <paramref name="era" /> is outside the range supported by the calendar. </exception>
		public abstract int GetMonthsInYear(int year, int era);

		/// <summary>Returns the seconds value in the specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>An integer from 0 to 59 that represents the seconds in <paramref name="time" />.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to read. </param>
		public virtual int GetSecond(DateTime time)
		{
			return time.TimeOfDay.Seconds;
		}

		internal int M_DiffDays(DateTime timeA, DateTime timeB)
		{
			long num = timeA.Ticks - timeB.Ticks;
			if (num >= 0L)
			{
				return (int)(num / 864000000000L);
			}
			num += 1L;
			return -1 + (int)(num / 864000000000L);
		}

		internal DateTime M_GetFirstDayOfSecondWeekOfYear(int year, CalendarWeekRule rule, DayOfWeek firstDayOfWeek)
		{
			DateTime time = this.ToDateTime(year, 1, 1, 0, 0, 0, 0);
			int dayOfWeek = (int)this.GetDayOfWeek(time);
			int num = 0;
			switch (rule)
			{
			case CalendarWeekRule.FirstDay:
				if (firstDayOfWeek > (DayOfWeek)dayOfWeek)
				{
					num += firstDayOfWeek - (DayOfWeek)dayOfWeek;
				}
				else
				{
					num += firstDayOfWeek + this.M_DaysInWeek - (DayOfWeek)dayOfWeek;
				}
				break;
			case CalendarWeekRule.FirstFullWeek:
				num = this.M_DaysInWeek;
				if (firstDayOfWeek >= (DayOfWeek)dayOfWeek)
				{
					num += firstDayOfWeek - (DayOfWeek)dayOfWeek;
				}
				else
				{
					num += firstDayOfWeek + this.M_DaysInWeek - (DayOfWeek)dayOfWeek;
				}
				break;
			case CalendarWeekRule.FirstFourDayWeek:
			{
				int num2 = (dayOfWeek + 3) % this.M_DaysInWeek;
				num = 3;
				if (firstDayOfWeek > (DayOfWeek)num2)
				{
					num += firstDayOfWeek - (DayOfWeek)num2;
				}
				else
				{
					num += firstDayOfWeek + this.M_DaysInWeek - (DayOfWeek)num2;
				}
				break;
			}
			}
			return this.AddDays(time, num);
		}

		/// <summary>Returns the week of the year that includes the date in the specified <see cref="T:System.DateTime" /> value.</summary>
		/// <returns>A positive integer that represents the week of the year that includes the date in the <paramref name="time" /> parameter.</returns>
		/// <param name="time">A date and time value.</param>
		/// <param name="rule">An enumeration value that defines a calendar week. </param>
		/// <param name="firstDayOfWeek">An enumeration value that represents the first day of the week. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="time" /> is earlier than <see cref="P:System.Globalization.Calendar.MinSupportedDateTime" /> or later than <see cref="P:System.Globalization.Calendar.MaxSupportedDateTime" />.-or-<paramref name="firstDayOfWeek" /> is not a valid <see cref="T:System.DayOfWeek" /> value.-or- <paramref name="rule" /> is not a valid <see cref="T:System.Globalization.CalendarWeekRule" /> value. </exception>
		public virtual int GetWeekOfYear(DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek)
		{
			if (firstDayOfWeek < DayOfWeek.Sunday || DayOfWeek.Saturday < firstDayOfWeek)
			{
				throw new ArgumentOutOfRangeException("firstDayOfWeek", "Value is not a valid day of week.");
			}
			int num = this.GetYear(time);
			int num2;
			for (;;)
			{
				DateTime timeB = this.M_GetFirstDayOfSecondWeekOfYear(num, rule, firstDayOfWeek);
				num2 = this.M_DiffDays(time, timeB) + this.M_DaysInWeek;
				if (num2 >= 0)
				{
					break;
				}
				num--;
			}
			return 1 + num2 / this.M_DaysInWeek;
		}

		/// <summary>When overridden in a derived class, returns the year in the specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>An integer that represents the year in <paramref name="time" />.</returns>
		/// <param name="time">The <see cref="T:System.DateTime" /> to read. </param>
		public abstract int GetYear(DateTime time);

		/// <summary>Determines whether the specified date in the current era is a leap day.</summary>
		/// <returns>true if the specified day is a leap day; otherwise, false.</returns>
		/// <param name="year">An integer that represents the year. </param>
		/// <param name="month">A positive integer that represents the month. </param>
		/// <param name="day">A positive integer that represents the day. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> is outside the range supported by the calendar.-or- <paramref name="month" /> is outside the range supported by the calendar.-or- <paramref name="day" /> is outside the range supported by the calendar. </exception>
		public virtual bool IsLeapDay(int year, int month, int day)
		{
			return this.IsLeapDay(year, month, day, 0);
		}

		/// <summary>When overridden in a derived class, determines whether the specified date in the specified era is a leap day.</summary>
		/// <returns>true if the specified day is a leap day; otherwise, false.</returns>
		/// <param name="year">An integer that represents the year. </param>
		/// <param name="month">A positive integer that represents the month. </param>
		/// <param name="day">A positive integer that represents the day. </param>
		/// <param name="era">An integer that represents the era. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> is outside the range supported by the calendar.-or- <paramref name="month" /> is outside the range supported by the calendar.-or- <paramref name="day" /> is outside the range supported by the calendar.-or- <paramref name="era" /> is outside the range supported by the calendar. </exception>
		public abstract bool IsLeapDay(int year, int month, int day, int era);

		/// <summary>Determines whether the specified month in the specified year in the current era is a leap month.</summary>
		/// <returns>true if the specified month is a leap month; otherwise, false.</returns>
		/// <param name="year">An integer that represents the year. </param>
		/// <param name="month">A positive integer that represents the month. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> is outside the range supported by the calendar.-or- <paramref name="month" /> is outside the range supported by the calendar. </exception>
		public virtual bool IsLeapMonth(int year, int month)
		{
			return this.IsLeapMonth(year, month, 0);
		}

		/// <summary>When overridden in a derived class, determines whether the specified month in the specified year in the specified era is a leap month.</summary>
		/// <returns>true if the specified month is a leap month; otherwise, false.</returns>
		/// <param name="year">An integer that represents the year. </param>
		/// <param name="month">A positive integer that represents the month. </param>
		/// <param name="era">An integer that represents the era. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> is outside the range supported by the calendar.-or- <paramref name="month" /> is outside the range supported by the calendar.-or- <paramref name="era" /> is outside the range supported by the calendar. </exception>
		public abstract bool IsLeapMonth(int year, int month, int era);

		/// <summary>Determines whether the specified year in the current era is a leap year.</summary>
		/// <returns>true if the specified year is a leap year; otherwise, false.</returns>
		/// <param name="year">An integer that represents the year. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> is outside the range supported by the calendar. </exception>
		public virtual bool IsLeapYear(int year)
		{
			return this.IsLeapYear(year, 0);
		}

		/// <summary>When overridden in a derived class, determines whether the specified year in the specified era is a leap year.</summary>
		/// <returns>true if the specified year is a leap year; otherwise, false.</returns>
		/// <param name="year">An integer that represents the year. </param>
		/// <param name="era">An integer that represents the era. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> is outside the range supported by the calendar.-or- <paramref name="era" /> is outside the range supported by the calendar. </exception>
		public abstract bool IsLeapYear(int year, int era);

		/// <summary>Returns a <see cref="T:System.DateTime" /> that is set to the specified date and time in the current era.</summary>
		/// <returns>The <see cref="T:System.DateTime" /> that is set to the specified date and time in the current era.</returns>
		/// <param name="year">An integer that represents the year. </param>
		/// <param name="month">A positive integer that represents the month. </param>
		/// <param name="day">A positive integer that represents the day. </param>
		/// <param name="hour">An integer from 0 to 23 that represents the hour. </param>
		/// <param name="minute">An integer from 0 to 59 that represents the minute. </param>
		/// <param name="second">An integer from 0 to 59 that represents the second. </param>
		/// <param name="millisecond">An integer from 0 to 999 that represents the millisecond. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> is outside the range supported by the calendar.-or- <paramref name="month" /> is outside the range supported by the calendar.-or- <paramref name="day" /> is outside the range supported by the calendar.-or- <paramref name="hour" /> is less than zero or greater than 23.-or- <paramref name="minute" /> is less than zero or greater than 59.-or- <paramref name="second" /> is less than zero or greater than 59.-or- <paramref name="millisecond" /> is less than zero or greater than 999. </exception>
		public virtual DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
		{
			return this.ToDateTime(year, month, day, hour, minute, second, millisecond, 0);
		}

		/// <summary>When overridden in a derived class, returns a <see cref="T:System.DateTime" /> that is set to the specified date and time in the specified era.</summary>
		/// <returns>The <see cref="T:System.DateTime" /> that is set to the specified date and time in the current era.</returns>
		/// <param name="year">An integer that represents the year. </param>
		/// <param name="month">A positive integer that represents the month. </param>
		/// <param name="day">A positive integer that represents the day. </param>
		/// <param name="hour">An integer from 0 to 23 that represents the hour. </param>
		/// <param name="minute">An integer from 0 to 59 that represents the minute. </param>
		/// <param name="second">An integer from 0 to 59 that represents the second. </param>
		/// <param name="millisecond">An integer from 0 to 999 that represents the millisecond. </param>
		/// <param name="era">An integer that represents the era. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> is outside the range supported by the calendar.-or- <paramref name="month" /> is outside the range supported by the calendar.-or- <paramref name="day" /> is outside the range supported by the calendar.-or- <paramref name="hour" /> is less than zero or greater than 23.-or- <paramref name="minute" /> is less than zero or greater than 59.-or- <paramref name="second" /> is less than zero or greater than 59.-or- <paramref name="millisecond" /> is less than zero or greater than 999.-or- <paramref name="era" /> is outside the range supported by the calendar. </exception>
		public abstract DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era);

		/// <summary>Converts the specified year to a four-digit year by using the <see cref="P:System.Globalization.Calendar.TwoDigitYearMax" /> property to determine the appropriate century.</summary>
		/// <returns>An integer that contains the four-digit representation of <paramref name="year" />.</returns>
		/// <param name="year">A two-digit or four-digit integer that represents the year to convert. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> is outside the range supported by the calendar. </exception>
		public virtual int ToFourDigitYear(int year)
		{
			if (year < 0)
			{
				throw new ArgumentOutOfRangeException("year", "Non-negative number required.");
			}
			if (year <= 99)
			{
				int num = this.TwoDigitYearMax % 100;
				int num2 = year - num;
				year = this.TwoDigitYearMax + num2 + ((num2 > 0) ? -100 : 0);
			}
			int num3 = 0;
			this.M_CheckYE(year, ref num3);
			return year;
		}

		internal string[] AbbreviatedEraNames
		{
			get
			{
				if (this.M_AbbrEraNames == null || this.M_AbbrEraNames.Length != this.Eras.Length)
				{
					throw new Exception("Internal: M_AbbrEraNames wrong initialized!");
				}
				return (string[])this.M_AbbrEraNames.Clone();
			}
			set
			{
				this.CheckReadOnly();
				if (value.Length != this.Eras.Length)
				{
					StringWriter stringWriter = new StringWriter();
					stringWriter.Write("Array length must be equal Eras length {0}.", this.Eras.Length);
					throw new ArgumentException(stringWriter.ToString());
				}
				this.M_AbbrEraNames = (string[])value.Clone();
			}
		}

		internal string[] EraNames
		{
			get
			{
				if (this.M_EraNames == null || this.M_EraNames.Length != this.Eras.Length)
				{
					throw new Exception("Internal: M_EraNames not initialized!");
				}
				return (string[])this.M_EraNames.Clone();
			}
			set
			{
				this.CheckReadOnly();
				if (value.Length != this.Eras.Length)
				{
					StringWriter stringWriter = new StringWriter();
					stringWriter.Write("Array length must be equal Eras length {0}.", this.Eras.Length);
					throw new ArgumentException(stringWriter.ToString());
				}
				this.M_EraNames = (string[])value.Clone();
			}
		}
	}
}
