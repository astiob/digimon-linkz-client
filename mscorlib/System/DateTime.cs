using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>Represents an instant in time, typically expressed as a date and time of day. </summary>
	/// <filterpriority>1</filterpriority>
	[Serializable]
	[StructLayout(LayoutKind.Auto)]
	public struct DateTime : IFormattable, IConvertible, IComparable, IComparable<DateTime>, IEquatable<DateTime>
	{
		private const int dp400 = 146097;

		private const int dp100 = 36524;

		private const int dp4 = 1461;

		private const long w32file_epoch = 504911232000000000L;

		private const long MAX_VALUE_TICKS = 3155378975999999999L;

		internal const long UnixEpoch = 621355968000000000L;

		private const long ticks18991230 = 599264352000000000L;

		private const double OAMinValue = -657435.0;

		private const double OAMaxValue = 2958466.0;

		private const string formatExceptionMessage = "String was not recognized as a valid DateTime.";

		private TimeSpan ticks;

		private DateTimeKind kind;

		/// <summary>Represents the largest possible value of <see cref="T:System.DateTime" />. This field is read-only.</summary>
		/// <filterpriority>1</filterpriority>
		public static readonly DateTime MaxValue = new DateTime(false, new TimeSpan(3155378975999999999L));

		/// <summary>Represents the smallest possible value of <see cref="T:System.DateTime" />. This field is read-only.</summary>
		/// <filterpriority>1</filterpriority>
		public static readonly DateTime MinValue = new DateTime(false, new TimeSpan(0L));

		private static readonly string[] ParseTimeFormats = new string[]
		{
			"H:m:s.fffffffzzz",
			"H:m:s.fffffff",
			"H:m:s tt zzz",
			"H:m:szzz",
			"H:m:s",
			"H:mzzz",
			"H:m",
			"H tt",
			"H'時'm'分's'秒'"
		};

		private static readonly string[] ParseYearDayMonthFormats = new string[]
		{
			"yyyy/M/dT",
			"M/yyyy/dT",
			"yyyy'年'M'月'd'日",
			"yyyy/d/MMMM",
			"yyyy/MMM/d",
			"d/MMMM/yyyy",
			"MMM/d/yyyy",
			"d/yyyy/MMMM",
			"MMM/yyyy/d",
			"yy/d/M"
		};

		private static readonly string[] ParseYearMonthDayFormats = new string[]
		{
			"yyyy/M/dT",
			"M/yyyy/dT",
			"yyyy'年'M'月'd'日",
			"yyyy/MMMM/d",
			"yyyy/d/MMM",
			"MMMM/d/yyyy",
			"d/MMM/yyyy",
			"MMMM/yyyy/d",
			"d/yyyy/MMM",
			"yy/MMMM/d",
			"yy/d/MMM",
			"MMM/yy/d"
		};

		private static readonly string[] ParseDayMonthYearFormats = new string[]
		{
			"yyyy/M/dT",
			"M/yyyy/dT",
			"yyyy'年'M'月'd'日",
			"yyyy/MMMM/d",
			"yyyy/d/MMM",
			"d/MMMM/yyyy",
			"MMM/d/yyyy",
			"MMMM/yyyy/d",
			"d/yyyy/MMM",
			"d/MMMM/yy",
			"yy/MMM/d",
			"d/yy/MMM",
			"yy/d/MMM",
			"MMM/d/yy",
			"MMM/yy/d"
		};

		private static readonly string[] ParseMonthDayYearFormats = new string[]
		{
			"yyyy/M/dT",
			"M/yyyy/dT",
			"yyyy'年'M'月'd'日",
			"yyyy/MMMM/d",
			"yyyy/d/MMM",
			"MMMM/d/yyyy",
			"d/MMM/yyyy",
			"MMMM/yyyy/d",
			"d/yyyy/MMM",
			"MMMM/d/yy",
			"MMM/yy/d",
			"d/MMM/yy",
			"yy/MMM/d",
			"d/yy/MMM",
			"yy/d/MMM"
		};

		private static readonly string[] MonthDayShortFormats = new string[]
		{
			"MMMM/d",
			"d/MMM",
			"yyyy/MMMM"
		};

		private static readonly string[] DayMonthShortFormats = new string[]
		{
			"d/MMMM",
			"MMM/yy",
			"yyyy/MMMM"
		};

		private static readonly int[] daysmonth = new int[]
		{
			0,
			31,
			28,
			31,
			30,
			31,
			30,
			31,
			31,
			30,
			31,
			30,
			31
		};

		private static readonly int[] daysmonthleap = new int[]
		{
			0,
			31,
			29,
			31,
			30,
			31,
			30,
			31,
			31,
			30,
			31,
			30,
			31
		};

		private static object to_local_time_span_object;

		private static long last_now;

		/// <summary>Initializes a new instance of the <see cref="T:System.DateTime" /> structure to a specified number of ticks.</summary>
		/// <param name="ticks">A date and time expressed in the number of 100-nanosecond intervals that have elapsed since January 1, 0001 at 00:00:00.000 in the Gregorian calendar. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="ticks" /> is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />. </exception>
		public DateTime(long ticks)
		{
			this.ticks = new TimeSpan(ticks);
			if (ticks < DateTime.MinValue.Ticks || ticks > DateTime.MaxValue.Ticks)
			{
				string text = Locale.GetText("Value {0} is outside the valid range [{1},{2}].", new object[]
				{
					ticks,
					DateTime.MinValue.Ticks,
					DateTime.MaxValue.Ticks
				});
				throw new ArgumentOutOfRangeException("ticks", text);
			}
			this.kind = DateTimeKind.Unspecified;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.DateTime" /> structure to the specified year, month, and day.</summary>
		/// <param name="year">The year (1 through 9999). </param>
		/// <param name="month">The month (1 through 12). </param>
		/// <param name="day">The day (1 through the number of days in <paramref name="month" />). </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> is less than 1 or greater than 9999.-or- <paramref name="month" /> is less than 1 or greater than 12.-or- <paramref name="day" /> is less than 1 or greater than the number of days in <paramref name="month" />. </exception>
		/// <exception cref="T:System.ArgumentException">The specified parameters evaluate to less than <see cref="F:System.DateTime.MinValue" /> or more than <see cref="F:System.DateTime.MaxValue" />. </exception>
		public DateTime(int year, int month, int day)
		{
			this = new DateTime(year, month, day, 0, 0, 0, 0);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.DateTime" /> structure to the specified year, month, day, hour, minute, and second.</summary>
		/// <param name="year">The year (1 through 9999). </param>
		/// <param name="month">The month (1 through 12). </param>
		/// <param name="day">The day (1 through the number of days in <paramref name="month" />). </param>
		/// <param name="hour">The hours (0 through 23). </param>
		/// <param name="minute">The minutes (0 through 59). </param>
		/// <param name="second">The seconds (0 through 59). </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> is less than 1 or greater than 9999. -or- <paramref name="month" /> is less than 1 or greater than 12. -or- <paramref name="day" /> is less than 1 or greater than the number of days in <paramref name="month" />.-or- <paramref name="hour" /> is less than 0 or greater than 23. -or- <paramref name="minute" /> is less than 0 or greater than 59. -or- <paramref name="second" /> is less than 0 or greater than 59. </exception>
		/// <exception cref="T:System.ArgumentException">The specified parameters evaluate to less than <see cref="F:System.DateTime.MinValue" /> or more than <see cref="F:System.DateTime.MaxValue" />. </exception>
		public DateTime(int year, int month, int day, int hour, int minute, int second)
		{
			this = new DateTime(year, month, day, hour, minute, second, 0);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.DateTime" /> structure to the specified year, month, day, hour, minute, second, and millisecond.</summary>
		/// <param name="year">The year (1 through 9999). </param>
		/// <param name="month">The month (1 through 12). </param>
		/// <param name="day">The day (1 through the number of days in <paramref name="month" />). </param>
		/// <param name="hour">The hours (0 through 23). </param>
		/// <param name="minute">The minutes (0 through 59). </param>
		/// <param name="second">The seconds (0 through 59). </param>
		/// <param name="millisecond">The milliseconds (0 through 999). </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> is less than 1 or greater than 9999.-or- <paramref name="month" /> is less than 1 or greater than 12.-or- <paramref name="day" /> is less than 1 or greater than the number of days in <paramref name="month" />.-or- <paramref name="hour" /> is less than 0 or greater than 23.-or- <paramref name="minute" /> is less than 0 or greater than 59.-or- <paramref name="second" /> is less than 0 or greater than 59.-or- <paramref name="millisecond" /> is less than 0 or greater than 999. </exception>
		/// <exception cref="T:System.ArgumentException">The specified parameters evaluate to less than <see cref="F:System.DateTime.MinValue" /> or more than <see cref="F:System.DateTime.MaxValue" />. </exception>
		public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
		{
			if (year < 1 || year > 9999 || month < 1 || month > 12 || day < 1 || day > DateTime.DaysInMonth(year, month) || hour < 0 || hour > 23 || minute < 0 || minute > 59 || second < 0 || second > 59 || millisecond < 0 || millisecond > 999)
			{
				throw new ArgumentOutOfRangeException("Parameters describe an unrepresentable DateTime.");
			}
			this.ticks = new TimeSpan(DateTime.AbsoluteDays(year, month, day), hour, minute, second, millisecond);
			this.kind = DateTimeKind.Unspecified;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.DateTime" /> structure to the specified year, month, and day for the specified calendar.</summary>
		/// <param name="year">The year (1 through the number of years in <paramref name="calendar" />). </param>
		/// <param name="month">The month (1 through the number of months in <paramref name="calendar" />). </param>
		/// <param name="day">The day (1 through the number of days in <paramref name="month" />). </param>
		/// <param name="calendar">The calendar that is used to interpret <paramref name="year" />, <paramref name="month" />, and <paramref name="day" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="calendar" /> is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> is not in the range supported by <paramref name="calendar" />.-or- <paramref name="month" /> is less than 1 or greater than the number of months in <paramref name="calendar" />.-or- <paramref name="day" /> is less than 1 or greater than the number of days in <paramref name="month" />. </exception>
		/// <exception cref="T:System.ArgumentException">The specified parameters evaluate to less than <see cref="F:System.DateTime.MinValue" /> or more than <see cref="F:System.DateTime.MaxValue" />. </exception>
		public DateTime(int year, int month, int day, Calendar calendar)
		{
			this = new DateTime(year, month, day, 0, 0, 0, 0, calendar);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.DateTime" /> structure to the specified year, month, day, hour, minute, and second for the specified calendar.</summary>
		/// <param name="year">The year (1 through the number of years in <paramref name="calendar" />). </param>
		/// <param name="month">The month (1 through the number of months in <paramref name="calendar" />). </param>
		/// <param name="day">The day (1 through the number of days in <paramref name="month" />). </param>
		/// <param name="hour">The hours (0 through 23). </param>
		/// <param name="minute">The minutes (0 through 59). </param>
		/// <param name="second">The seconds (0 through 59). </param>
		/// <param name="calendar">The calendar that is used to interpret <paramref name="year" />, <paramref name="month" />, and <paramref name="day" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="calendar" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> is not in the range supported by <paramref name="calendar" />.-or- <paramref name="month" /> is less than 1 or greater than the number of months in <paramref name="calendar" />.-or- <paramref name="day" /> is less than 1 or greater than the number of days in <paramref name="month" />.-or- <paramref name="hour" /> is less than 0 or greater than 23 -or- <paramref name="minute" /> is less than 0 or greater than 59. -or- <paramref name="second" /> is less than 0 or greater than 59. </exception>
		/// <exception cref="T:System.ArgumentException">The specified parameters evaluate to less than <see cref="F:System.DateTime.MinValue" /> or more than <see cref="F:System.DateTime.MaxValue" />. </exception>
		public DateTime(int year, int month, int day, int hour, int minute, int second, Calendar calendar)
		{
			this = new DateTime(year, month, day, hour, minute, second, 0, calendar);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.DateTime" /> structure to the specified year, month, day, hour, minute, second, and millisecond for the specified calendar.</summary>
		/// <param name="year">The year (1 through the number of years in <paramref name="calendar" />). </param>
		/// <param name="month">The month (1 through the number of months in <paramref name="calendar" />). </param>
		/// <param name="day">The day (1 through the number of days in <paramref name="month" />). </param>
		/// <param name="hour">The hours (0 through 23). </param>
		/// <param name="minute">The minutes (0 through 59). </param>
		/// <param name="second">The seconds (0 through 59). </param>
		/// <param name="millisecond">The milliseconds (0 through 999). </param>
		/// <param name="calendar">The calendar that is used to interpret <paramref name="year" />, <paramref name="month" />, and <paramref name="day" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="calendar" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> is not in the range supported by <paramref name="calendar" />.-or- <paramref name="month" /> is less than 1 or greater than the number of months in <paramref name="calendar" />.-or- <paramref name="day" /> is less than 1 or greater than the number of days in <paramref name="month" />.-or- <paramref name="hour" /> is less than 0 or greater than 23.-or- <paramref name="minute" /> is less than 0 or greater than 59.-or- <paramref name="second" /> is less than 0 or greater than 59.-or- <paramref name="millisecond" /> is less than 0 or greater than 999. </exception>
		/// <exception cref="T:System.ArgumentException">The specified parameters evaluate to less than <see cref="F:System.DateTime.MinValue" /> or more than <see cref="F:System.DateTime.MaxValue" />. </exception>
		public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, Calendar calendar)
		{
			if (calendar == null)
			{
				throw new ArgumentNullException("calendar");
			}
			this.ticks = calendar.ToDateTime(year, month, day, hour, minute, second, millisecond).ticks;
			this.kind = DateTimeKind.Unspecified;
		}

		internal DateTime(bool check, TimeSpan value)
		{
			if (check && (value.Ticks < DateTime.MinValue.Ticks || value.Ticks > DateTime.MaxValue.Ticks))
			{
				throw new ArgumentOutOfRangeException();
			}
			this.ticks = value;
			this.kind = DateTimeKind.Unspecified;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.DateTime" /> structure to a specified number of ticks and to Coordinated Universal Time (UTC) or local time.</summary>
		/// <param name="ticks">A date and time expressed in the number of 100-nanosecond intervals that have elapsed since January 1, 0001 at 00:00:00.000 in the Gregorian calendar. </param>
		/// <param name="kind">One of the enumeration values that indicates whether <paramref name="ticks" /> specifies a local time, Coordinated Universal Time (UTC), or neither.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="ticks" /> is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="kind" /> is not one of the <see cref="T:System.DateTimeKind" /> values.</exception>
		public DateTime(long ticks, DateTimeKind kind)
		{
			this = new DateTime(ticks);
			this.CheckDateTimeKind(kind);
			this.kind = kind;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.DateTime" /> structure to the specified year, month, day, hour, minute, second, and Coordinated Universal Time (UTC) or local time.</summary>
		/// <param name="year">The year (1 through 9999). </param>
		/// <param name="month">The month (1 through 12). </param>
		/// <param name="day">The day (1 through the number of days in <paramref name="month" />). </param>
		/// <param name="hour">The hours (0 through 23). </param>
		/// <param name="minute">The minutes (0 through 59). </param>
		/// <param name="second">The seconds (0 through 59). </param>
		/// <param name="kind">One of the enumeration values that indicates whether <paramref name="year" />, <paramref name="month" />, <paramref name="day" />, <paramref name="hour" />, <paramref name="minute" /> and <paramref name="second" /> specify a local time, Coordinated Universal Time (UTC), or neither.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> is less than 1 or greater than 9999. -or- <paramref name="month" /> is less than 1 or greater than 12. -or- <paramref name="day" /> is less than 1 or greater than the number of days in <paramref name="month" />.-or- <paramref name="hour" /> is less than 0 or greater than 23. -or- <paramref name="minute" /> is less than 0 or greater than 59. -or- <paramref name="second" /> is less than 0 or greater than 59. </exception>
		/// <exception cref="T:System.ArgumentException">The specified time parameters evaluate to less than <see cref="F:System.DateTime.MinValue" /> or more than <see cref="F:System.DateTime.MaxValue" />. -or-<paramref name="kind" /> is not one of the <see cref="T:System.DateTimeKind" /> values.</exception>
		public DateTime(int year, int month, int day, int hour, int minute, int second, DateTimeKind kind)
		{
			this = new DateTime(year, month, day, hour, minute, second);
			this.CheckDateTimeKind(kind);
			this.kind = kind;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.DateTime" /> structure to the specified year, month, day, hour, minute, second, millisecond, and Coordinated Universal Time (UTC) or local time.</summary>
		/// <param name="year">The year (1 through 9999). </param>
		/// <param name="month">The month (1 through 12). </param>
		/// <param name="day">The day (1 through the number of days in <paramref name="month" />). </param>
		/// <param name="hour">The hours (0 through 23). </param>
		/// <param name="minute">The minutes (0 through 59). </param>
		/// <param name="second">The seconds (0 through 59). </param>
		/// <param name="millisecond">The milliseconds (0 through 999). </param>
		/// <param name="kind">One of the enumeration values that indicates whether <paramref name="year" />, <paramref name="month" />, <paramref name="day" />, <paramref name="hour" />, <paramref name="minute" />, <paramref name="second" />, and <paramref name="millisecond" /> specify a local time, Coordinated Universal Time (UTC), or neither.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> is less than 1 or greater than 9999.-or- <paramref name="month" /> is less than 1 or greater than 12.-or- <paramref name="day" /> is less than 1 or greater than the number of days in <paramref name="month" />.-or- <paramref name="hour" /> is less than 0 or greater than 23.-or- <paramref name="minute" /> is less than 0 or greater than 59.-or- <paramref name="second" /> is less than 0 or greater than 59.-or- <paramref name="millisecond" /> is less than 0 or greater than 999. </exception>
		/// <exception cref="T:System.ArgumentException">The specified time parameters evaluate to less than <see cref="F:System.DateTime.MinValue" /> or more than <see cref="F:System.DateTime.MaxValue" />. -or-<paramref name="kind" /> is not one of the <see cref="T:System.DateTimeKind" /> values.</exception>
		public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind kind)
		{
			this = new DateTime(year, month, day, hour, minute, second, millisecond);
			this.CheckDateTimeKind(kind);
			this.kind = kind;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.DateTime" /> structure to the specified year, month, day, hour, minute, second, millisecond, and Coordinated Universal Time (UTC) or local time for the specified calendar.</summary>
		/// <param name="year">The year (1 through the number of years in <paramref name="calendar" />). </param>
		/// <param name="month">The month (1 through the number of months in <paramref name="calendar" />). </param>
		/// <param name="day">The day (1 through the number of days in <paramref name="month" />). </param>
		/// <param name="hour">The hours (0 through 23). </param>
		/// <param name="minute">The minutes (0 through 59). </param>
		/// <param name="second">The seconds (0 through 59). </param>
		/// <param name="millisecond">The milliseconds (0 through 999). </param>
		/// <param name="calendar">The calendar that is used to interpret <paramref name="year" />, <paramref name="month" />, and <paramref name="day" />.</param>
		/// <param name="kind">One of the enumeration values that indicates whether <paramref name="year" />, <paramref name="month" />, <paramref name="day" />, <paramref name="hour" />, <paramref name="minute" />, <paramref name="second" />, and <paramref name="millisecond" /> specify a local time, Coordinated Universal Time (UTC), or neither.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="calendar" /> is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> is not in the range supported by <paramref name="calendar" />.-or- <paramref name="month" /> is less than 1 or greater than the number of months in <paramref name="calendar" />.-or- <paramref name="day" /> is less than 1 or greater than the number of days in <paramref name="month" />.-or- <paramref name="hour" /> is less than 0 or greater than 23.-or- <paramref name="minute" /> is less than 0 or greater than 59.-or- <paramref name="second" /> is less than 0 or greater than 59.-or- <paramref name="millisecond" /> is less than 0 or greater than 999. </exception>
		/// <exception cref="T:System.ArgumentException">The specified time parameters evaluate to less than <see cref="F:System.DateTime.MinValue" /> or more than <see cref="F:System.DateTime.MaxValue" />. -or-<paramref name="kind" /> is not one of the <see cref="T:System.DateTimeKind" /> values.</exception>
		public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, Calendar calendar, DateTimeKind kind)
		{
			this = new DateTime(year, month, day, hour, minute, second, millisecond, calendar);
			this.CheckDateTimeKind(kind);
			this.kind = kind;
		}

		static DateTime()
		{
			if (MonoTouchAOTHelper.FalseFlag)
			{
				GenericComparer<DateTime> genericComparer = new GenericComparer<DateTime>();
				GenericEqualityComparer<DateTime> genericEqualityComparer = new GenericEqualityComparer<DateTime>();
			}
		}

		/// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>The return value for this member is not used.</returns>
		/// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify null.)</param>
		/// <exception cref="T:System.InvalidCastException">In all cases.</exception>
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			throw new InvalidCastException();
		}

		/// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>The return value for this member is not used.</returns>
		/// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify null.)</param>
		/// <exception cref="T:System.InvalidCastException">In all cases.</exception>
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			throw new InvalidCastException();
		}

		/// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>The return value for this member is not used.</returns>
		/// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify null.)</param>
		/// <exception cref="T:System.InvalidCastException">In all cases.</exception>
		char IConvertible.ToChar(IFormatProvider provider)
		{
			throw new InvalidCastException();
		}

		/// <summary>Returns the current <see cref="T:System.DateTime" /> object.</summary>
		/// <returns>The current <see cref="T:System.DateTime" /> object.</returns>
		/// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify null.)</param>
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			return this;
		}

		/// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>The return value for this member is not used.</returns>
		/// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface or null.</param>
		/// <exception cref="T:System.InvalidCastException">In all cases.</exception>
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			throw new InvalidCastException();
		}

		/// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>The return value for this member is not used.</returns>
		/// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify null.)</param>
		/// <exception cref="T:System.InvalidCastException">In all cases.</exception>
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			throw new InvalidCastException();
		}

		/// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>The return value for this member is not used.</returns>
		/// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify null.)</param>
		/// <exception cref="T:System.InvalidCastException">In all cases.</exception>
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			throw new InvalidCastException();
		}

		/// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>The return value for this member is not used.</returns>
		/// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify null.)</param>
		/// <exception cref="T:System.InvalidCastException">In all cases.</exception>
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			throw new InvalidCastException();
		}

		/// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>The return value for this member is not used.</returns>
		/// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify null.)</param>
		/// <exception cref="T:System.InvalidCastException">In all cases.</exception>
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			throw new InvalidCastException();
		}

		/// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>The return value for this member is not used.</returns>
		/// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify null.)</param>
		/// <exception cref="T:System.InvalidCastException">In all cases.</exception>
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			throw new InvalidCastException();
		}

		/// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>The return value for this member is not used.</returns>
		/// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify null.)</param>
		/// <exception cref="T:System.InvalidCastException">In all cases. </exception>
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			throw new InvalidCastException();
		}

		/// <summary>Converts the current <see cref="T:System.DateTime" /> object to an object of a specified type.</summary>
		/// <returns>An object of the type specified by the <paramref name="type" /> parameter, with a value equivalent to the current <see cref="T:System.DateTime" /> object.</returns>
		/// <param name="type">The desired type. </param>
		/// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify null.)</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is null. </exception>
		/// <exception cref="T:System.InvalidCastException">This conversion is not supported for the <see cref="T:System.DateTime" /> type.</exception>
		object IConvertible.ToType(Type targetType, IFormatProvider provider)
		{
			if (targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}
			if (targetType == typeof(DateTime))
			{
				return this;
			}
			if (targetType == typeof(string))
			{
				return this.ToString(provider);
			}
			if (targetType == typeof(object))
			{
				return this;
			}
			throw new InvalidCastException();
		}

		/// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>The return value for this member is not used.</returns>
		/// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify null.)</param>
		/// <exception cref="T:System.InvalidCastException">In all cases.</exception>
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			throw new InvalidCastException();
		}

		/// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>The return value for this member is not used.</returns>
		/// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify null.)</param>
		/// <exception cref="T:System.InvalidCastException">In all cases.</exception>
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			throw new InvalidCastException();
		}

		/// <summary>This conversion is not supported. Attempting to use this method throws an <see cref="T:System.InvalidCastException" />.</summary>
		/// <returns>The return value for this member is not used.</returns>
		/// <param name="provider">An object that implements the <see cref="T:System.IFormatProvider" /> interface. (This parameter is not used; specify null.)</param>
		/// <exception cref="T:System.InvalidCastException">In all cases.</exception>
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			throw new InvalidCastException();
		}

		private static int AbsoluteDays(int year, int month, int day)
		{
			int num = 0;
			int i = 1;
			int[] array = (!DateTime.IsLeapYear(year)) ? DateTime.daysmonth : DateTime.daysmonthleap;
			while (i < month)
			{
				num += array[i++];
			}
			return day - 1 + num + 365 * (year - 1) + (year - 1) / 4 - (year - 1) / 100 + (year - 1) / 400;
		}

		private int FromTicks(DateTime.Which what)
		{
			int num = 1;
			int[] array = DateTime.daysmonth;
			int i = this.ticks.Days;
			int num2 = i / 146097;
			i -= num2 * 146097;
			int num3 = i / 36524;
			if (num3 == 4)
			{
				num3 = 3;
			}
			i -= num3 * 36524;
			int num4 = i / 1461;
			i -= num4 * 1461;
			int num5 = i / 365;
			if (num5 == 4)
			{
				num5 = 3;
			}
			if (what == DateTime.Which.Year)
			{
				return num2 * 400 + num3 * 100 + num4 * 4 + num5 + 1;
			}
			i -= num5 * 365;
			if (what == DateTime.Which.DayYear)
			{
				return i + 1;
			}
			if (num5 == 3 && (num3 == 3 || num4 != 24))
			{
				array = DateTime.daysmonthleap;
			}
			while (i >= array[num])
			{
				i -= array[num++];
			}
			if (what == DateTime.Which.Month)
			{
				return num;
			}
			return i + 1;
		}

		/// <summary>Gets the date component of this instance.</summary>
		/// <returns>A new <see cref="T:System.DateTime" /> with the same date as this instance, and the time value set to 12:00:00 midnight (00:00:00).</returns>
		/// <filterpriority>1</filterpriority>
		public DateTime Date
		{
			get
			{
				return new DateTime(this.Year, this.Month, this.Day)
				{
					kind = this.kind
				};
			}
		}

		/// <summary>Gets the month component of the date represented by this instance.</summary>
		/// <returns>The month component, expressed as a value between 1 and 12.</returns>
		/// <filterpriority>1</filterpriority>
		public int Month
		{
			get
			{
				return this.FromTicks(DateTime.Which.Month);
			}
		}

		/// <summary>Gets the day of the month represented by this instance.</summary>
		/// <returns>The day component, expressed as a value between 1 and 31.</returns>
		/// <filterpriority>1</filterpriority>
		public int Day
		{
			get
			{
				return this.FromTicks(DateTime.Which.Day);
			}
		}

		/// <summary>Gets the day of the week represented by this instance.</summary>
		/// <returns>A <see cref="T:System.DayOfWeek" /> enumerated constant that indicates the day of the week of this <see cref="T:System.DateTime" /> value. </returns>
		/// <filterpriority>1</filterpriority>
		public DayOfWeek DayOfWeek
		{
			get
			{
				return (this.ticks.Days + DayOfWeek.Monday) % (DayOfWeek)7;
			}
		}

		/// <summary>Gets the day of the year represented by this instance.</summary>
		/// <returns>The day of the year, expressed as a value between 1 and 366.</returns>
		/// <filterpriority>1</filterpriority>
		public int DayOfYear
		{
			get
			{
				return this.FromTicks(DateTime.Which.DayYear);
			}
		}

		/// <summary>Gets the time of day for this instance.</summary>
		/// <returns>A <see cref="T:System.TimeSpan" /> that represents the fraction of the day that has elapsed since midnight.</returns>
		/// <filterpriority>1</filterpriority>
		public TimeSpan TimeOfDay
		{
			get
			{
				return new TimeSpan(this.ticks.Ticks % 864000000000L);
			}
		}

		/// <summary>Gets the hour component of the date represented by this instance.</summary>
		/// <returns>The hour component, expressed as a value between 0 and 23.</returns>
		/// <filterpriority>1</filterpriority>
		public int Hour
		{
			get
			{
				return this.ticks.Hours;
			}
		}

		/// <summary>Gets the minute component of the date represented by this instance.</summary>
		/// <returns>The minute component, expressed as a value between 0 and 59.</returns>
		/// <filterpriority>1</filterpriority>
		public int Minute
		{
			get
			{
				return this.ticks.Minutes;
			}
		}

		/// <summary>Gets the seconds component of the date represented by this instance.</summary>
		/// <returns>The seconds, between 0 and 59.</returns>
		/// <filterpriority>1</filterpriority>
		public int Second
		{
			get
			{
				return this.ticks.Seconds;
			}
		}

		/// <summary>Gets the milliseconds component of the date represented by this instance.</summary>
		/// <returns>The milliseconds component, expressed as a value between 0 and 999.</returns>
		/// <filterpriority>1</filterpriority>
		public int Millisecond
		{
			get
			{
				return this.ticks.Milliseconds;
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern long GetTimeMonotonic();

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern long GetNow();

		/// <summary>Gets a <see cref="T:System.DateTime" /> object that is set to the current date and time on this computer, expressed as the local time.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> whose value is the current local date and time.</returns>
		/// <filterpriority>1</filterpriority>
		public static DateTime Now
		{
			get
			{
				long now = DateTime.GetNow();
				DateTime dateTime = new DateTime(now);
				if (now - DateTime.last_now > 600000000L)
				{
					DateTime.to_local_time_span_object = TimeZone.CurrentTimeZone.GetLocalTimeDiff(dateTime);
					DateTime.last_now = now;
				}
				DateTime result = dateTime + (TimeSpan)DateTime.to_local_time_span_object;
				result.kind = DateTimeKind.Local;
				return result;
			}
		}

		/// <summary>Gets the number of ticks that represent the date and time of this instance.</summary>
		/// <returns>The number of ticks that represent the date and time of this instance. The value is between DateTime.MinValue.Ticks and DateTime.MaxValue.Ticks.</returns>
		/// <filterpriority>1</filterpriority>
		public long Ticks
		{
			get
			{
				return this.ticks.Ticks;
			}
		}

		/// <summary>Gets the current date.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> set to today's date, with the time component set to 00:00:00.</returns>
		/// <filterpriority>1</filterpriority>
		public static DateTime Today
		{
			get
			{
				DateTime now = DateTime.Now;
				return new DateTime(now.Year, now.Month, now.Day)
				{
					kind = now.kind
				};
			}
		}

		/// <summary>Gets a <see cref="T:System.DateTime" /> object that is set to the current date and time on this computer, expressed as the Coordinated Universal Time (UTC).</summary>
		/// <returns>A <see cref="T:System.DateTime" /> whose value is the current UTC date and time.</returns>
		/// <filterpriority>1</filterpriority>
		public static DateTime UtcNow
		{
			get
			{
				return new DateTime(DateTime.GetNow(), DateTimeKind.Utc);
			}
		}

		/// <summary>Gets the year component of the date represented by this instance.</summary>
		/// <returns>The year, between 1 and 9999.</returns>
		/// <filterpriority>1</filterpriority>
		public int Year
		{
			get
			{
				return this.FromTicks(DateTime.Which.Year);
			}
		}

		/// <summary>Gets a value that indicates whether the time represented by this instance is based on local time, Coordinated Universal Time (UTC), or neither.</summary>
		/// <returns>One of the T:System.DateTimeKind values. The default is <see cref="F:System.DateTimeKind.Unspecified" />.</returns>
		/// <filterpriority>1</filterpriority>
		public DateTimeKind Kind
		{
			get
			{
				return this.kind;
			}
		}

		/// <summary>Returns a new <see cref="T:System.DateTime" /> that adds the value of the specified <see cref="T:System.TimeSpan" /> to the value of this instance.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> whose value is the sum of the date and time represented by this instance and the time interval represented by <paramref name="value" />.</returns>
		/// <param name="value">A <see cref="T:System.TimeSpan" /> object that represents a positive or negative time interval. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTime" /> is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />. </exception>
		/// <filterpriority>2</filterpriority>
		public DateTime Add(TimeSpan value)
		{
			DateTime result = this.AddTicks(value.Ticks);
			result.kind = this.kind;
			return result;
		}

		/// <summary>Returns a new <see cref="T:System.DateTime" /> that adds the specified number of days to the value of this instance.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> whose value is the sum of the date and time represented by this instance and the number of days represented by <paramref name="value" />.</returns>
		/// <param name="value">A number of whole and fractional days. The <paramref name="value" /> parameter can be negative or positive. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTime" /> is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />. </exception>
		/// <filterpriority>2</filterpriority>
		public DateTime AddDays(double value)
		{
			return this.AddMilliseconds(Math.Round(value * 86400000.0));
		}

		/// <summary>Returns a new <see cref="T:System.DateTime" /> that adds the specified number of ticks to the value of this instance.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> whose value is the sum of the date and time represented by this instance and the time represented by <paramref name="value" />.</returns>
		/// <param name="value">A number of 100-nanosecond ticks. The <paramref name="value" /> parameter can be positive or negative. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTime" /> is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />. </exception>
		/// <filterpriority>2</filterpriority>
		public DateTime AddTicks(long value)
		{
			if (value + this.ticks.Ticks > 3155378975999999999L || value + this.ticks.Ticks < 0L)
			{
				throw new ArgumentOutOfRangeException();
			}
			return new DateTime(value + this.ticks.Ticks)
			{
				kind = this.kind
			};
		}

		/// <summary>Returns a new <see cref="T:System.DateTime" /> that adds the specified number of hours to the value of this instance.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> whose value is the sum of the date and time represented by this instance and the number of hours represented by <paramref name="value" />.</returns>
		/// <param name="value">A number of whole and fractional hours. The <paramref name="value" /> parameter can be negative or positive. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTime" /> is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />. </exception>
		/// <filterpriority>2</filterpriority>
		public DateTime AddHours(double value)
		{
			return this.AddMilliseconds(value * 3600000.0);
		}

		/// <summary>Returns a new <see cref="T:System.DateTime" /> that adds the specified number of milliseconds to the value of this instance.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> whose value is the sum of the date and time represented by this instance and the number of milliseconds represented by <paramref name="value" />.</returns>
		/// <param name="value">A number of whole and fractional milliseconds. The <paramref name="value" /> parameter can be negative or positive. Note that this value is rounded to the nearest integer.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTime" /> is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />. </exception>
		/// <filterpriority>2</filterpriority>
		public DateTime AddMilliseconds(double value)
		{
			if (value * 10000.0 > 9.2233720368547758E+18 || value * 10000.0 < -9.2233720368547758E+18)
			{
				throw new ArgumentOutOfRangeException();
			}
			long value2 = (long)Math.Round(value * 10000.0);
			return this.AddTicks(value2);
		}

		private DateTime AddRoundedMilliseconds(double ms)
		{
			if (ms * 10000.0 > 9.2233720368547758E+18 || ms * 10000.0 < -9.2233720368547758E+18)
			{
				throw new ArgumentOutOfRangeException();
			}
			long value = (long)(ms += ((ms <= 0.0) ? -0.5 : 0.5)) * 10000L;
			return this.AddTicks(value);
		}

		/// <summary>Returns a new <see cref="T:System.DateTime" /> that adds the specified number of minutes to the value of this instance.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> whose value is the sum of the date and time represented by this instance and the number of minutes represented by <paramref name="value" />.</returns>
		/// <param name="value">A number of whole and fractional minutes. The <paramref name="value" /> parameter can be negative or positive. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTime" /> is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />. </exception>
		/// <filterpriority>2</filterpriority>
		public DateTime AddMinutes(double value)
		{
			return this.AddMilliseconds(value * 60000.0);
		}

		/// <summary>Returns a new <see cref="T:System.DateTime" /> that adds the specified number of months to the value of this instance.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> whose value is the sum of the date and time represented by this instance and <paramref name="months" />.</returns>
		/// <param name="months">A number of months. The <paramref name="months" /> parameter can be negative or positive. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTime" /> is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />.-or- <paramref name="months" /> is less than -120,000 or greater than 120,000. </exception>
		/// <filterpriority>2</filterpriority>
		public DateTime AddMonths(int months)
		{
			int num = this.Day;
			int num2 = this.Month + months % 12;
			int num3 = this.Year + months / 12;
			if (num2 < 1)
			{
				num2 = 12 + num2;
				num3--;
			}
			else if (num2 > 12)
			{
				num2 -= 12;
				num3++;
			}
			int num4 = DateTime.DaysInMonth(num3, num2);
			if (num > num4)
			{
				num = num4;
			}
			DateTime dateTime = new DateTime(num3, num2, num);
			dateTime.kind = this.kind;
			return dateTime.Add(this.TimeOfDay);
		}

		/// <summary>Returns a new <see cref="T:System.DateTime" /> that adds the specified number of seconds to the value of this instance.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> whose value is the sum of the date and time represented by this instance and the number of seconds represented by <paramref name="value" />.</returns>
		/// <param name="value">A number of whole and fractional seconds. The <paramref name="value" /> parameter can be negative or positive. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTime" /> is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />. </exception>
		/// <filterpriority>2</filterpriority>
		public DateTime AddSeconds(double value)
		{
			return this.AddMilliseconds(value * 1000.0);
		}

		/// <summary>Returns a new <see cref="T:System.DateTime" /> that adds the specified number of years to the value of this instance.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> whose value is the sum of the date and time represented by this instance and the number of years represented by <paramref name="value" />.</returns>
		/// <param name="value">A number of years. The <paramref name="value" /> parameter can be negative or positive. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="value" /> or the resulting <see cref="T:System.DateTime" /> is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />. </exception>
		/// <filterpriority>2</filterpriority>
		public DateTime AddYears(int value)
		{
			return this.AddMonths(value * 12);
		}

		/// <summary>Compares two instances of <see cref="T:System.DateTime" /> and returns an integer that indicates whether the first instance is earlier than, the same as, or later than the second instance.</summary>
		/// <returns>A signed number indicating the relative values of <paramref name="t1" /> and <paramref name="t2" />.Value Type Condition Less than zero <paramref name="t1" /> is earlier than <paramref name="t2" />. Zero <paramref name="t1" /> is the same as <paramref name="t2" />. Greater than zero <paramref name="t1" /> is later than <paramref name="t2" />. </returns>
		/// <param name="t1">The first object to compare. </param>
		/// <param name="t2">The second object to compare.</param>
		/// <filterpriority>1</filterpriority>
		public static int Compare(DateTime t1, DateTime t2)
		{
			if (t1.ticks < t2.ticks)
			{
				return -1;
			}
			if (t1.ticks > t2.ticks)
			{
				return 1;
			}
			return 0;
		}

		/// <summary>Compares the value of this instance to a specified object that contains a specified <see cref="T:System.DateTime" /> value, and returns an integer that indicates whether this instance is earlier than, the same as, or later than the specified <see cref="T:System.DateTime" /> value.</summary>
		/// <returns>A signed number indicating the relative values of this instance and <paramref name="value" />.Value Description Less than zero This instance is earlier than <paramref name="value" />. Zero This instance is the same as <paramref name="value" />. Greater than zero This instance is later than <paramref name="value" />, or <paramref name="value" /> is null. </returns>
		/// <param name="value">A boxed object to compare, or null. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="value" /> is not a <see cref="T:System.DateTime" />. </exception>
		/// <filterpriority>2</filterpriority>
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is DateTime))
			{
				throw new ArgumentException(Locale.GetText("Value is not a System.DateTime"));
			}
			return DateTime.Compare(this, (DateTime)value);
		}

		/// <summary>Indicates whether this instance of <see cref="T:System.DateTime" /> is within the Daylight Saving Time range for the current time zone.</summary>
		/// <returns>true if <see cref="P:System.DateTime.Kind" /> is <see cref="F:System.DateTimeKind.Local" /> or <see cref="F:System.DateTimeKind.Unspecified" /> and the value of this instance of <see cref="T:System.DateTime" /> is within the Daylight Saving Time range for the current time zone. false if <see cref="P:System.DateTime.Kind" /> is <see cref="F:System.DateTimeKind.Utc" />.</returns>
		/// <filterpriority>2</filterpriority>
		public bool IsDaylightSavingTime()
		{
			return this.kind != DateTimeKind.Utc && TimeZone.CurrentTimeZone.IsDaylightSavingTime(this);
		}

		/// <summary>Compares the value of this instance to a specified <see cref="T:System.DateTime" /> value and returns an integer that indicates whether this instance is earlier than, the same as, or later than the specified <see cref="T:System.DateTime" /> value.</summary>
		/// <returns>A signed number indicating the relative values of this instance and the <paramref name="value" /> parameter.Value Description Less than zero This instance is earlier than <paramref name="value" />. Zero This instance is the same as <paramref name="value" />. Greater than zero This instance is later than <paramref name="value" />. </returns>
		/// <param name="value">The object to compare to the current instance. </param>
		/// <filterpriority>2</filterpriority>
		public int CompareTo(DateTime value)
		{
			return DateTime.Compare(this, value);
		}

		/// <summary>Returns a value indicating whether this instance is equal to the specified <see cref="T:System.DateTime" /> instance.</summary>
		/// <returns>true if the <paramref name="value" /> parameter equals the value of this instance; otherwise, false.</returns>
		/// <param name="value">The object to compare to this instance. </param>
		/// <filterpriority>2</filterpriority>
		public bool Equals(DateTime value)
		{
			return value.ticks == this.ticks;
		}

		/// <summary>Serializes the current <see cref="T:System.DateTime" /> object to a 64-bit binary value that subsequently can be used to recreate the <see cref="T:System.DateTime" /> object.</summary>
		/// <returns>A 64-bit signed integer that encodes the <see cref="P:System.DateTime.Kind" /> and <see cref="P:System.DateTime.Ticks" /> properties. </returns>
		/// <filterpriority>2</filterpriority>
		public long ToBinary()
		{
			DateTimeKind dateTimeKind = this.kind;
			if (dateTimeKind == DateTimeKind.Utc)
			{
				return this.Ticks | 4611686018427387904L;
			}
			if (dateTimeKind != DateTimeKind.Local)
			{
				return this.Ticks;
			}
			return this.ToUniversalTime().Ticks | long.MinValue;
		}

		/// <summary>Deserializes a 64-bit binary value and recreates an original serialized <see cref="T:System.DateTime" /> object.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> object that is equivalent to the <see cref="T:System.DateTime" /> object that was serialized by the <see cref="M:System.DateTime.ToBinary" /> method.</returns>
		/// <param name="dateData">A 64-bit signed integer that encodes the <see cref="P:System.DateTime.Kind" /> property in a 2-bit field and the <see cref="P:System.DateTime.Ticks" /> property in a 62-bit field. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="dateData" /> is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static DateTime FromBinary(long dateData)
		{
			ulong num = (ulong)dateData >> 62;
			if (num == (ulong)0)
			{
				return new DateTime(dateData, DateTimeKind.Unspecified);
			}
			if (num != (ulong)1)
			{
				DateTime dateTime = new DateTime(dateData & 4611686018427387903L, DateTimeKind.Utc);
				return dateTime.ToLocalTime();
			}
			return new DateTime(dateData ^ 4611686018427387904L, DateTimeKind.Utc);
		}

		/// <summary>Creates a new <see cref="T:System.DateTime" /> object that has the same number of ticks as the specified <see cref="T:System.DateTime" />, but is designated as either local time, Coordinated Universal Time (UTC), or neither, as indicated by the specified <see cref="T:System.DateTimeKind" /> value.</summary>
		/// <returns>A new object that has the same number of ticks as the object represented by the <paramref name="value" /> parameter and the <see cref="T:System.DateTimeKind" /> value specified by the <paramref name="kind" /> parameter.</returns>
		/// <param name="value">A <see cref="T:System.DateTime" /> object.</param>
		/// <param name="kind">One of the <see cref="T:System.DateTimeKind" /> values.</param>
		/// <filterpriority>2</filterpriority>
		public static DateTime SpecifyKind(DateTime value, DateTimeKind kind)
		{
			return new DateTime(value.Ticks, kind);
		}

		/// <summary>Returns the number of days in the specified month and year.</summary>
		/// <returns>The number of days in <paramref name="month" /> for the specified <paramref name="year" />.For example, if <paramref name="month" /> equals 2 for February, the return value is 28 or 29 depending upon whether <paramref name="year" /> is a leap year.</returns>
		/// <param name="year">The year. </param>
		/// <param name="month">The month (a number ranging from 1 to 12). </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="month" /> is less than 1 or greater than 12.-or-<paramref name="year" /> is less than 1 or greater than 9999.</exception>
		/// <filterpriority>1</filterpriority>
		public static int DaysInMonth(int year, int month)
		{
			if (month < 1 || month > 12)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (year < 1 || year > 9999)
			{
				throw new ArgumentOutOfRangeException();
			}
			int[] array = (!DateTime.IsLeapYear(year)) ? DateTime.daysmonth : DateTime.daysmonthleap;
			return array[month];
		}

		/// <summary>Returns a value indicating whether this instance is equal to a specified object.</summary>
		/// <returns>true if <paramref name="value" /> is an instance of <see cref="T:System.DateTime" /> and equals the value of this instance; otherwise, false.</returns>
		/// <param name="value">An object to compare to this instance. </param>
		/// <filterpriority>2</filterpriority>
		public override bool Equals(object value)
		{
			return value is DateTime && ((DateTime)value).ticks == this.ticks;
		}

		/// <summary>Returns a value indicating whether two instances of <see cref="T:System.DateTime" /> are equal.</summary>
		/// <returns>true if the two values are equal; otherwise, false.</returns>
		/// <param name="t1">The first object to compare.</param>
		/// <param name="t2">The second object to compare.</param>
		/// <filterpriority>1</filterpriority>
		public static bool Equals(DateTime t1, DateTime t2)
		{
			return t1.ticks == t2.ticks;
		}

		/// <summary>Converts the specified Windows file time to an equivalent local time.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> object that represents a local time equivalent to the date and time represented by the <paramref name="fileTime" /> parameter.</returns>
		/// <param name="fileTime">A Windows file time expressed in ticks. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="fileTime" /> is less than 0 or represents a time greater than <see cref="F:System.DateTime.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static DateTime FromFileTime(long fileTime)
		{
			if (fileTime < 0L)
			{
				throw new ArgumentOutOfRangeException("fileTime", "< 0");
			}
			DateTime dateTime = new DateTime(504911232000000000L + fileTime);
			return dateTime.ToLocalTime();
		}

		/// <summary>Converts the specified Windows file time to an equivalent UTC time.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> object that represents a UTC time equivalent to the date and time represented by the <paramref name="fileTime" /> parameter.</returns>
		/// <param name="fileTime">A Windows file time expressed in ticks. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="fileTime" /> is less than 0 or represents a time greater than <see cref="F:System.DateTime.MaxValue" />. </exception>
		/// <filterpriority>1</filterpriority>
		public static DateTime FromFileTimeUtc(long fileTime)
		{
			if (fileTime < 0L)
			{
				throw new ArgumentOutOfRangeException("fileTime", "< 0");
			}
			return new DateTime(504911232000000000L + fileTime);
		}

		/// <summary>Returns a <see cref="T:System.DateTime" /> equivalent to the specified OLE Automation date.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> that represents the same date and time as <paramref name="d" />.</returns>
		/// <param name="d">An OLE Automation date value. </param>
		/// <exception cref="T:System.ArgumentException">The date is not a valid OLE Automation date value. </exception>
		/// <filterpriority>1</filterpriority>
		public static DateTime FromOADate(double d)
		{
			if (d <= -657435.0 || d >= 2958466.0)
			{
				throw new ArgumentException("d", "[-657435,2958466]");
			}
			DateTime result = new DateTime(599264352000000000L);
			if (d < 0.0)
			{
				double num = Math.Ceiling(d);
				result = result.AddRoundedMilliseconds(num * 86400000.0);
				double num2 = num - d;
				result = result.AddRoundedMilliseconds(num2 * 86400000.0);
			}
			else
			{
				result = result.AddRoundedMilliseconds(d * 86400000.0);
			}
			return result;
		}

		/// <summary>Converts the value of this instance to all the string representations supported by the standard date and time format specifiers.</summary>
		/// <returns>A string array where each element is the representation of the value of this instance formatted with one of the standard date and time format specifiers.</returns>
		/// <filterpriority>2</filterpriority>
		public string[] GetDateTimeFormats()
		{
			return this.GetDateTimeFormats(CultureInfo.CurrentCulture);
		}

		/// <summary>Converts the value of this instance to all the string representations supported by the specified standard date and time format specifier.</summary>
		/// <returns>A string array where each element is the representation of the value of this instance formatted with the <paramref name="format" /> standard date and time format specifier.</returns>
		/// <param name="format">A standard date and time format string (see Remarks). </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="format" /> is not a valid standard date and time format specifier character.</exception>
		/// <filterpriority>2</filterpriority>
		public string[] GetDateTimeFormats(char format)
		{
			if ("dDgGfFmMrRstTuUyY".IndexOf(format) < 0)
			{
				throw new FormatException("Invalid format character.");
			}
			return new string[]
			{
				this.ToString(format.ToString())
			};
		}

		/// <summary>Converts the value of this instance to all the string representations supported by the standard date and time format specifiers and the specified culture-specific formatting information.</summary>
		/// <returns>A string array where each element is the representation of the value of this instance formatted with one of the standard date and time format specifiers.</returns>
		/// <param name="provider">An object that supplies culture-specific formatting information about this instance. </param>
		/// <filterpriority>2</filterpriority>
		public string[] GetDateTimeFormats(IFormatProvider provider)
		{
			DateTimeFormatInfo provider2 = (DateTimeFormatInfo)provider.GetFormat(typeof(DateTimeFormatInfo));
			ArrayList arrayList = new ArrayList();
			foreach (char format in "dDgGfFmMrRstTuUyY")
			{
				arrayList.AddRange(this.GetDateTimeFormats(format, provider2));
			}
			return arrayList.ToArray(typeof(string)) as string[];
		}

		/// <summary>Converts the value of this instance to all the string representations supported by the specified standard date and time format specifier and culture-specific formatting information.</summary>
		/// <returns>A string array where each element is the representation of the value of this instance formatted with one of the standard date and time format specifiers.</returns>
		/// <param name="format">A standard date and time format string (see Remarks). </param>
		/// <param name="provider">An object that supplies culture-specific formatting information about this instance. </param>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="format" /> is not a valid standard date and time format specifier character.</exception>
		/// <filterpriority>2</filterpriority>
		public string[] GetDateTimeFormats(char format, IFormatProvider provider)
		{
			if ("dDgGfFmMrRstTuUyY".IndexOf(format) < 0)
			{
				throw new FormatException("Invalid format character.");
			}
			bool adjustutc = false;
			if (format == 'U')
			{
				adjustutc = true;
			}
			DateTimeFormatInfo dateTimeFormatInfo = (DateTimeFormatInfo)provider.GetFormat(typeof(DateTimeFormatInfo));
			return this.GetDateTimeFormats(adjustutc, dateTimeFormatInfo.GetAllRawDateTimePatterns(format), dateTimeFormatInfo);
		}

		private string[] GetDateTimeFormats(bool adjustutc, string[] patterns, DateTimeFormatInfo dfi)
		{
			string[] array = new string[patterns.Length];
			DateTime dt = (!adjustutc) ? this : this.ToUniversalTime();
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = DateTimeUtils.ToString(dt, patterns[i], dfi);
			}
			return array;
		}

		private void CheckDateTimeKind(DateTimeKind kind)
		{
			if (kind != DateTimeKind.Unspecified && kind != DateTimeKind.Utc && kind != DateTimeKind.Local)
			{
				throw new ArgumentException("Invalid DateTimeKind value.", "kind");
			}
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			return (int)this.ticks.Ticks;
		}

		/// <summary>Returns the <see cref="T:System.TypeCode" /> for value type <see cref="T:System.DateTime" />.</summary>
		/// <returns>The enumerated constant, <see cref="F:System.TypeCode.DateTime" />.</returns>
		/// <filterpriority>2</filterpriority>
		public TypeCode GetTypeCode()
		{
			return TypeCode.DateTime;
		}

		/// <summary>Returns an indication whether the specified year is a leap year.</summary>
		/// <returns>true if <paramref name="year" /> is a leap year; otherwise, false.</returns>
		/// <param name="year">A 4-digit year. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> is less than 1 or greater than 9999.</exception>
		/// <filterpriority>1</filterpriority>
		public static bool IsLeapYear(int year)
		{
			if (year < 1 || year > 9999)
			{
				throw new ArgumentOutOfRangeException();
			}
			return (year % 4 == 0 && year % 100 != 0) || year % 400 == 0;
		}

		/// <summary>Converts the specified string representation of a date and time to its <see cref="T:System.DateTime" /> equivalent.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> equivalent to the date and time contained in <paramref name="s" />.</returns>
		/// <param name="s">A string containing a date and time to convert. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> does not contain a valid string representation of a date and time. </exception>
		/// <filterpriority>1</filterpriority>
		public static DateTime Parse(string s)
		{
			return DateTime.Parse(s, null);
		}

		/// <summary>Converts the specified string representation of a date and time to its <see cref="T:System.DateTime" /> equivalent using the specified culture-specific format information.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> equivalent to the date and time contained in <paramref name="s" /> as specified by <paramref name="provider" />.</returns>
		/// <param name="s">A string containing a date and time to convert. </param>
		/// <param name="provider">An object that supplies culture-specific format information about <paramref name="s" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> does not contain a valid string representation of a date and time. </exception>
		/// <filterpriority>1</filterpriority>
		public static DateTime Parse(string s, IFormatProvider provider)
		{
			return DateTime.Parse(s, provider, DateTimeStyles.AllowWhiteSpaces);
		}

		/// <summary>Converts the specified string representation of a date and time to its <see cref="T:System.DateTime" /> equivalent using the specified culture-specific format information and formatting style.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> equivalent to the date and time contained in <paramref name="s" /> as specified by <paramref name="provider" /> and <paramref name="styles" />.</returns>
		/// <param name="s">A string containing a date and time to convert. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />. </param>
		/// <param name="styles">A bitwise combination of the enumeration values that indicates the style elements that can be present in <paramref name="s" /> for the parse operation to succeed and that defines how to interpret the parsed date in relation to the current time zone or the current date. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> does not contain a valid string representation of a date and time. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="styles" /> contains an invalid combination of <see cref="T:System.Globalization.DateTimeStyles" /> values. For example, both <see cref="F:System.Globalization.DateTimeStyles.AssumeLocal" /> and <see cref="F:System.Globalization.DateTimeStyles.AssumeUniversal" />.</exception>
		/// <filterpriority>1</filterpriority>
		public static DateTime Parse(string s, IFormatProvider provider, DateTimeStyles styles)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			Exception ex = null;
			DateTime result;
			DateTimeOffset dateTimeOffset;
			if (!DateTime.CoreParse(s, provider, styles, out result, out dateTimeOffset, true, ref ex))
			{
				throw ex;
			}
			return result;
		}

		internal static bool CoreParse(string s, IFormatProvider provider, DateTimeStyles styles, out DateTime result, out DateTimeOffset dto, bool setExceptionOnError, ref Exception exception)
		{
			dto = new DateTimeOffset(0L, TimeSpan.Zero);
			if (s == null || s.Length == 0)
			{
				if (setExceptionOnError)
				{
					exception = new FormatException("String was not recognized as a valid DateTime.");
				}
				result = DateTime.MinValue;
				return false;
			}
			if (provider == null)
			{
				provider = CultureInfo.CurrentCulture;
			}
			DateTimeFormatInfo instance = DateTimeFormatInfo.GetInstance(provider);
			string[] array = DateTime.YearMonthDayFormats(instance, setExceptionOnError, ref exception);
			if (array == null)
			{
				result = DateTime.MinValue;
				return false;
			}
			bool flag = false;
			foreach (string firstPart in array)
			{
				bool flag2 = false;
				if (DateTime._DoParse(s, firstPart, string.Empty, false, out result, out dto, instance, styles, true, ref flag2, ref flag))
				{
					return true;
				}
				if (flag2)
				{
					for (int j = 0; j < DateTime.ParseTimeFormats.Length; j++)
					{
						if (DateTime._DoParse(s, firstPart, DateTime.ParseTimeFormats[j], false, out result, out dto, instance, styles, true, ref flag2, ref flag))
						{
							return true;
						}
					}
				}
			}
			int num = instance.MonthDayPattern.IndexOf('d');
			int num2 = instance.MonthDayPattern.IndexOf('M');
			if (num == -1 || num2 == -1)
			{
				result = DateTime.MinValue;
				if (setExceptionOnError)
				{
					exception = new FormatException(Locale.GetText("Order of month and date is not defined by {0}", new object[]
					{
						instance.MonthDayPattern
					}));
				}
				return false;
			}
			bool flag3 = num < num2;
			string[] array2 = (!flag3) ? DateTime.MonthDayShortFormats : DateTime.DayMonthShortFormats;
			for (int k = 0; k < array2.Length; k++)
			{
				bool flag4 = false;
				if (DateTime._DoParse(s, array2[k], string.Empty, false, out result, out dto, instance, styles, true, ref flag4, ref flag))
				{
					return true;
				}
			}
			for (int l = 0; l < DateTime.ParseTimeFormats.Length; l++)
			{
				string firstPart2 = DateTime.ParseTimeFormats[l];
				bool flag5 = false;
				if (DateTime._DoParse(s, firstPart2, string.Empty, false, out result, out dto, instance, styles, false, ref flag5, ref flag))
				{
					return true;
				}
				if (flag5)
				{
					for (int m = 0; m < array2.Length; m++)
					{
						if (DateTime._DoParse(s, firstPart2, array2[m], false, out result, out dto, instance, styles, false, ref flag5, ref flag))
						{
							return true;
						}
					}
					foreach (string text in array)
					{
						if (text[text.Length - 1] != 'T')
						{
							if (DateTime._DoParse(s, firstPart2, text, false, out result, out dto, instance, styles, false, ref flag5, ref flag))
							{
								return true;
							}
						}
					}
				}
			}
			if (DateTime.ParseExact(s, instance.GetAllDateTimePatternsInternal(), instance, styles, out result, false, ref flag, setExceptionOnError, ref exception))
			{
				return true;
			}
			if (!setExceptionOnError)
			{
				return false;
			}
			exception = new FormatException("String was not recognized as a valid DateTime.");
			return false;
		}

		/// <summary>Converts the specified string representation of a date and time to its <see cref="T:System.DateTime" /> equivalent using the specified format and culture-specific format information. The format of the string representation must match the specified format exactly.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> equivalent to the date and time contained in <paramref name="s" /> as specified by <paramref name="format" /> and <paramref name="provider" />.</returns>
		/// <param name="s">A string that contains a date and time to convert. </param>
		/// <param name="format">A format specifier that defines the required format of <paramref name="s" />. </param>
		/// <param name="provider">An object that supplies culture-specific format information about <paramref name="s" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> or <paramref name="format" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> or <paramref name="format" /> is an empty string. -or- <paramref name="s" /> does not contain a date and time that corresponds to the pattern specified in <paramref name="format" />. </exception>
		/// <filterpriority>2</filterpriority>
		public static DateTime ParseExact(string s, string format, IFormatProvider provider)
		{
			return DateTime.ParseExact(s, format, provider, DateTimeStyles.None);
		}

		private static string[] YearMonthDayFormats(DateTimeFormatInfo dfi, bool setExceptionOnError, ref Exception exc)
		{
			int num = dfi.ShortDatePattern.IndexOf('d');
			int num2 = dfi.ShortDatePattern.IndexOf('M');
			int num3 = dfi.ShortDatePattern.IndexOf('y');
			if (num == -1 || num2 == -1 || num3 == -1)
			{
				if (setExceptionOnError)
				{
					exc = new FormatException(Locale.GetText("Order of year, month and date is not defined by {0}", new object[]
					{
						dfi.ShortDatePattern
					}));
				}
				return null;
			}
			if (num3 < num2)
			{
				if (num2 < num)
				{
					return DateTime.ParseYearMonthDayFormats;
				}
				if (num3 < num)
				{
					return DateTime.ParseYearDayMonthFormats;
				}
				if (setExceptionOnError)
				{
					exc = new FormatException(Locale.GetText("Order of date, year and month defined by {0} is not supported", new object[]
					{
						dfi.ShortDatePattern
					}));
				}
				return null;
			}
			else
			{
				if (num < num2)
				{
					return DateTime.ParseDayMonthYearFormats;
				}
				if (num < num3)
				{
					return DateTime.ParseMonthDayYearFormats;
				}
				if (setExceptionOnError)
				{
					exc = new FormatException(Locale.GetText("Order of month, year and date defined by {0} is not supported", new object[]
					{
						dfi.ShortDatePattern
					}));
				}
				return null;
			}
		}

		private static int _ParseNumber(string s, int valuePos, int min_digits, int digits, bool leadingzero, bool sloppy_parsing, out int num_parsed)
		{
			int num = 0;
			if (sloppy_parsing)
			{
				leadingzero = false;
			}
			if (!leadingzero)
			{
				int num2 = 0;
				int i = valuePos;
				while (i < s.Length && i < digits + valuePos)
				{
					if (!char.IsDigit(s[i]))
					{
						break;
					}
					num2++;
					i++;
				}
				digits = num2;
			}
			if (digits < min_digits)
			{
				num_parsed = -1;
				return 0;
			}
			if (s.Length - valuePos < digits)
			{
				num_parsed = -1;
				return 0;
			}
			for (int i = valuePos; i < digits + valuePos; i++)
			{
				char c = s[i];
				if (!char.IsDigit(c))
				{
					num_parsed = -1;
					return 0;
				}
				num = num * 10 + (int)((byte)(c - '0'));
			}
			num_parsed = digits;
			return num;
		}

		private static int _ParseEnum(string s, int sPos, string[] values, string[] invValues, bool exact, out int num_parsed)
		{
			for (int i = values.Length - 1; i >= 0; i--)
			{
				if (!exact && invValues[i].Length > values[i].Length)
				{
					if (invValues[i].Length > 0 && DateTime._ParseString(s, sPos, 0, invValues[i], out num_parsed))
					{
						return i;
					}
					if (values[i].Length > 0 && DateTime._ParseString(s, sPos, 0, values[i], out num_parsed))
					{
						return i;
					}
				}
				else
				{
					if (values[i].Length > 0 && DateTime._ParseString(s, sPos, 0, values[i], out num_parsed))
					{
						return i;
					}
					if (!exact && invValues[i].Length > 0 && DateTime._ParseString(s, sPos, 0, invValues[i], out num_parsed))
					{
						return i;
					}
				}
			}
			num_parsed = -1;
			return -1;
		}

		private static bool _ParseString(string s, int sPos, int maxlength, string value, out int num_parsed)
		{
			if (maxlength <= 0)
			{
				maxlength = value.Length;
			}
			if (sPos + maxlength <= s.Length && string.Compare(s, sPos, value, 0, maxlength, true, CultureInfo.InvariantCulture) == 0)
			{
				num_parsed = maxlength;
				return true;
			}
			num_parsed = -1;
			return false;
		}

		private static bool _ParseAmPm(string s, int valuePos, int num, DateTimeFormatInfo dfi, bool exact, out int num_parsed, ref int ampm)
		{
			num_parsed = -1;
			if (ampm != -1)
			{
				return false;
			}
			if (DateTime.IsLetter(s, valuePos))
			{
				DateTimeFormatInfo invariantInfo = DateTimeFormatInfo.InvariantInfo;
				if ((!exact && DateTime._ParseString(s, valuePos, num, invariantInfo.PMDesignator, out num_parsed)) || (dfi.PMDesignator != string.Empty && DateTime._ParseString(s, valuePos, num, dfi.PMDesignator, out num_parsed)))
				{
					ampm = 1;
				}
				else
				{
					if ((exact || !DateTime._ParseString(s, valuePos, num, invariantInfo.AMDesignator, out num_parsed)) && !DateTime._ParseString(s, valuePos, num, dfi.AMDesignator, out num_parsed))
					{
						return false;
					}
					if (exact || num_parsed != 0)
					{
						ampm = 0;
					}
				}
				return true;
			}
			if (dfi.AMDesignator != string.Empty)
			{
				return false;
			}
			if (exact)
			{
				ampm = 0;
			}
			num_parsed = 0;
			return true;
		}

		private static bool _ParseTimeSeparator(string s, int sPos, DateTimeFormatInfo dfi, bool exact, out int num_parsed)
		{
			return DateTime._ParseString(s, sPos, 0, dfi.TimeSeparator, out num_parsed) || (!exact && DateTime._ParseString(s, sPos, 0, ":", out num_parsed));
		}

		private static bool _ParseDateSeparator(string s, int sPos, DateTimeFormatInfo dfi, bool exact, out int num_parsed)
		{
			num_parsed = -1;
			if (exact && s[sPos] != '/')
			{
				return false;
			}
			if (DateTime._ParseTimeSeparator(s, sPos, dfi, exact, out num_parsed) || char.IsDigit(s[sPos]) || char.IsLetter(s[sPos]))
			{
				return false;
			}
			num_parsed = 1;
			return true;
		}

		private static bool IsLetter(string s, int pos)
		{
			return pos < s.Length && char.IsLetter(s[pos]);
		}

		private static bool _DoParse(string s, string firstPart, string secondPart, bool exact, out DateTime result, out DateTimeOffset dto, DateTimeFormatInfo dfi, DateTimeStyles style, bool firstPartIsDate, ref bool incompleteFormat, ref bool longYear)
		{
			bool flag = false;
			bool flag2 = false;
			bool sloppy_parsing = false;
			dto = new DateTimeOffset(0L, TimeSpan.Zero);
			bool flag3 = !exact && secondPart != null;
			incompleteFormat = false;
			int num = 0;
			string text = firstPart;
			bool flag4 = false;
			DateTimeFormatInfo invariantInfo = DateTimeFormatInfo.InvariantInfo;
			if (text.Length == 1)
			{
				text = DateTimeUtils.GetStandardPattern(text[0], dfi, out flag, out flag2);
			}
			result = new DateTime(0L);
			if (text == null)
			{
				return false;
			}
			if (s == null)
			{
				return false;
			}
			if ((style & DateTimeStyles.AllowLeadingWhite) != DateTimeStyles.None)
			{
				text = text.TrimStart(null);
				s = s.TrimStart(null);
			}
			if ((style & DateTimeStyles.AllowTrailingWhite) != DateTimeStyles.None)
			{
				text = text.TrimEnd(null);
				s = s.TrimEnd(null);
			}
			if (flag2)
			{
				dfi = invariantInfo;
			}
			if ((style & DateTimeStyles.AllowInnerWhite) != DateTimeStyles.None)
			{
				sloppy_parsing = true;
			}
			string text2 = text;
			int length = text.Length;
			int num2 = 0;
			int num3 = 0;
			if (length == 0)
			{
				return false;
			}
			int num4 = -1;
			int num5 = -1;
			int num6 = -1;
			int num7 = -1;
			int num8 = -1;
			int num9 = -1;
			int num10 = -1;
			double num11 = -1.0;
			int num12 = -1;
			int num13 = -1;
			int num14 = -1;
			int num15 = -1;
			bool flag5 = true;
			while (num != s.Length)
			{
				int num16 = 0;
				if (flag3 && num2 + num3 == 0)
				{
					bool flag6 = DateTime.IsLetter(s, num);
					if (flag6)
					{
						if (s[num] == 'Z')
						{
							num16 = 1;
						}
						else
						{
							DateTime._ParseString(s, num, 0, "GMT", out num16);
						}
						if (num16 > 0 && !DateTime.IsLetter(s, num + num16))
						{
							num += num16;
							flag = true;
							continue;
						}
					}
					if (!flag4 && DateTime._ParseAmPm(s, num, 0, dfi, exact, out num16, ref num12))
					{
						if (DateTime.IsLetter(s, num + num16))
						{
							num12 = -1;
						}
						else if (num16 > 0)
						{
							num += num16;
							continue;
						}
					}
					if (!flag4 && num5 == -1 && flag6)
					{
						num5 = DateTime._ParseEnum(s, num, dfi.RawDayNames, invariantInfo.RawDayNames, exact, out num16);
						if (num5 == -1)
						{
							num5 = DateTime._ParseEnum(s, num, dfi.RawAbbreviatedDayNames, invariantInfo.RawAbbreviatedDayNames, exact, out num16);
						}
						if (num5 != -1 && !DateTime.IsLetter(s, num + num16))
						{
							num += num16;
							continue;
						}
						num5 = -1;
					}
					if (char.IsWhiteSpace(s[num]) || s[num] == ',')
					{
						num++;
						continue;
					}
					num16 = 0;
				}
				if (num2 + num3 >= length)
				{
					if (flag3 && num3 == 0)
					{
						flag4 = (flag5 && firstPart[firstPart.Length - 1] == 'T');
						if (flag5 || !(text == string.Empty))
						{
							num2 = 0;
							if (flag5)
							{
								text = secondPart;
							}
							else
							{
								text = string.Empty;
							}
							text2 = text;
							length = text2.Length;
							flag5 = false;
							continue;
						}
					}
					IL_EA9:
					if (num2 + 1 < length && text2[num2] == '.' && text2[num2 + 1] == 'F')
					{
						num2++;
						while (num2 < length && text2[num2] == 'F')
						{
							num2++;
						}
					}
					while (num2 < length && text2[num2] == 'K')
					{
						num2++;
					}
					if (num2 < length)
					{
						return false;
					}
					if (s.Length > num)
					{
						if (num == 0)
						{
							return false;
						}
						if (char.IsDigit(s[num]) && char.IsDigit(s[num - 1]))
						{
							return false;
						}
						if (char.IsLetter(s[num]) && char.IsLetter(s[num - 1]))
						{
							return false;
						}
						incompleteFormat = true;
						return false;
					}
					else
					{
						if (num8 == -1)
						{
							num8 = 0;
						}
						if (num9 == -1)
						{
							num9 = 0;
						}
						if (num10 == -1)
						{
							num10 = 0;
						}
						if (num11 == -1.0)
						{
							num11 = 0.0;
						}
						if (num4 == -1 && num6 == -1 && num7 == -1)
						{
							if ((style & DateTimeStyles.NoCurrentDateDefault) != DateTimeStyles.None)
							{
								num4 = 1;
								num6 = 1;
								num7 = 1;
							}
							else
							{
								num4 = DateTime.Today.Day;
								num6 = DateTime.Today.Month;
								num7 = DateTime.Today.Year;
							}
						}
						if (num4 == -1)
						{
							num4 = 1;
						}
						if (num6 == -1)
						{
							num6 = 1;
						}
						if (num7 == -1)
						{
							if ((style & DateTimeStyles.NoCurrentDateDefault) != DateTimeStyles.None)
							{
								num7 = 1;
							}
							else
							{
								num7 = DateTime.Today.Year;
							}
						}
						if (num12 == 0 && num8 == 12)
						{
							num8 = 0;
						}
						if (num12 == 1 && (!flag3 || num8 < 12))
						{
							num8 += 12;
						}
						if (num7 < 1 || num7 > 9999 || num6 < 1 || num6 > 12 || num4 < 1 || num4 > DateTime.DaysInMonth(num7, num6) || num8 < 0 || num8 > 23 || num9 < 0 || num9 > 59 || num10 < 0 || num10 > 59)
						{
							return false;
						}
						result = new DateTime(num7, num6, num4, num8, num9, num10, 0);
						result = result.AddSeconds(num11);
						if (num5 != -1 && num5 != (int)result.DayOfWeek)
						{
							return false;
						}
						if (num13 == -1)
						{
							if (result != DateTime.MinValue)
							{
								try
								{
									dto = new DateTimeOffset(result);
								}
								catch
								{
								}
							}
						}
						else
						{
							if (num15 == -1)
							{
								num15 = 0;
							}
							if (num14 == -1)
							{
								num14 = 0;
							}
							if (num13 == 1)
							{
								num14 = -num14;
								num15 = -num15;
							}
							try
							{
								dto = new DateTimeOffset(result, new TimeSpan(num14, num15, 0));
							}
							catch
							{
							}
						}
						bool flag7 = (style & DateTimeStyles.AdjustToUniversal) != DateTimeStyles.None;
						if (num13 != -1)
						{
							long num17 = (result.ticks - dto.Offset).Ticks;
							if (num17 < 0L)
							{
								num17 += 864000000000L;
							}
							result = new DateTime(false, new TimeSpan(num17));
							result.kind = DateTimeKind.Utc;
							if ((style & DateTimeStyles.RoundtripKind) != DateTimeStyles.None)
							{
								result = result.ToLocalTime();
							}
						}
						else if (flag || (style & DateTimeStyles.AssumeUniversal) != DateTimeStyles.None)
						{
							result.kind = DateTimeKind.Utc;
						}
						else if ((style & DateTimeStyles.AssumeLocal) != DateTimeStyles.None)
						{
							result.kind = DateTimeKind.Local;
						}
						bool flag8 = !flag7 && (style & DateTimeStyles.RoundtripKind) == DateTimeStyles.None;
						if (result.kind != DateTimeKind.Unspecified)
						{
							if (flag7)
							{
								result = result.ToUniversalTime();
							}
							else if (flag8)
							{
								result = result.ToLocalTime();
							}
						}
						return true;
					}
				}
				else
				{
					bool leadingzero = true;
					if (text2[num2] == '\'')
					{
						num3 = 1;
						while (num2 + num3 < length)
						{
							if (text2[num2 + num3] == '\'')
							{
								break;
							}
							if (num == s.Length || s[num] != text2[num2 + num3])
							{
								return false;
							}
							num++;
							num3++;
						}
						num2 += num3 + 1;
						num3 = 0;
					}
					else if (text2[num2] == '"')
					{
						num3 = 1;
						while (num2 + num3 < length)
						{
							if (text2[num2 + num3] == '"')
							{
								break;
							}
							if (num == s.Length || s[num] != text2[num2 + num3])
							{
								return false;
							}
							num++;
							num3++;
						}
						num2 += num3 + 1;
						num3 = 0;
					}
					else if (text2[num2] == '\\')
					{
						num2 += num3 + 1;
						num3 = 0;
						if (num2 >= length)
						{
							return false;
						}
						if (s[num] != text2[num2])
						{
							return false;
						}
						num++;
						num2++;
					}
					else if (text2[num2] == '%')
					{
						num2++;
					}
					else if (char.IsWhiteSpace(s[num]) || (s[num] == ',' && ((!exact && text2[num2] == '/') || char.IsWhiteSpace(text2[num2]))))
					{
						num++;
						num3 = 0;
						if (exact && (style & DateTimeStyles.AllowInnerWhite) == DateTimeStyles.None)
						{
							if (!char.IsWhiteSpace(text2[num2]))
							{
								return false;
							}
							num2++;
						}
						else
						{
							int i;
							for (i = num; i < s.Length; i++)
							{
								if (!char.IsWhiteSpace(s[i]) && s[i] != ',')
								{
									break;
								}
							}
							num = i;
							for (i = num2; i < text2.Length; i++)
							{
								if (!char.IsWhiteSpace(text2[i]) && text2[i] != ',')
								{
									break;
								}
							}
							num2 = i;
							if (!exact && num2 < text2.Length && text2[num2] == '/' && !DateTime._ParseDateSeparator(s, num, dfi, exact, out num16))
							{
								num2++;
							}
						}
					}
					else if (num2 + num3 + 1 < length && text2[num2 + num3 + 1] == text2[num2 + num3])
					{
						num3++;
					}
					else
					{
						char c = text2[num2];
						switch (c)
						{
						case 'F':
							leadingzero = false;
							goto IL_A82;
						case 'G':
							if (s[num] != 'G')
							{
								return false;
							}
							if (num2 + 2 < length && num + 2 < s.Length && text2[num2 + 1] == 'M' && s[num + 1] == 'M' && text2[num2 + 2] == 'T' && s[num + 2] == 'T')
							{
								flag = true;
								num3 = 2;
								num16 = 3;
							}
							else
							{
								num3 = 0;
								num16 = 1;
							}
							break;
						case 'H':
							if (num8 != -1 || (!flag3 && num12 >= 0))
							{
								return false;
							}
							if (num3 == 0)
							{
								num8 = DateTime._ParseNumber(s, num, 1, 2, false, sloppy_parsing, out num16);
							}
							else
							{
								num8 = DateTime._ParseNumber(s, num, 1, 2, true, sloppy_parsing, out num16);
							}
							if (num8 >= 24)
							{
								return false;
							}
							break;
						default:
							switch (c)
							{
							case 's':
								if (num10 != -1)
								{
									return false;
								}
								if (num3 == 0)
								{
									num10 = DateTime._ParseNumber(s, num, 1, 2, false, sloppy_parsing, out num16);
								}
								else
								{
									num10 = DateTime._ParseNumber(s, num, 1, 2, true, sloppy_parsing, out num16);
								}
								if (num10 >= 60)
								{
									return false;
								}
								break;
							case 't':
								if (!DateTime._ParseAmPm(s, num, (num3 <= 0) ? 1 : 0, dfi, exact, out num16, ref num12))
								{
									return false;
								}
								break;
							default:
								switch (c)
								{
								case 'd':
									if ((num3 < 2 && num4 != -1) || (num3 >= 2 && num5 != -1))
									{
										return false;
									}
									if (num3 == 0)
									{
										num4 = DateTime._ParseNumber(s, num, 1, 2, false, sloppy_parsing, out num16);
									}
									else if (num3 == 1)
									{
										num4 = DateTime._ParseNumber(s, num, 1, 2, true, sloppy_parsing, out num16);
									}
									else if (num3 == 2)
									{
										num5 = DateTime._ParseEnum(s, num, dfi.RawAbbreviatedDayNames, invariantInfo.RawAbbreviatedDayNames, exact, out num16);
									}
									else
									{
										num5 = DateTime._ParseEnum(s, num, dfi.RawDayNames, invariantInfo.RawDayNames, exact, out num16);
									}
									break;
								default:
									if (c != '/')
									{
										if (c != ':')
										{
											if (c != 'Z')
											{
												if (c != 'm')
												{
													if (s[num] != text2[num2])
													{
														return false;
													}
													num3 = 0;
													num16 = 1;
												}
												else
												{
													if (num9 != -1)
													{
														return false;
													}
													if (num3 == 0)
													{
														num9 = DateTime._ParseNumber(s, num, 1, 2, false, sloppy_parsing, out num16);
													}
													else
													{
														num9 = DateTime._ParseNumber(s, num, 1, 2, true, sloppy_parsing, out num16);
													}
													if (num9 >= 60)
													{
														return false;
													}
												}
											}
											else
											{
												if (s[num] != 'Z')
												{
													return false;
												}
												num3 = 0;
												num16 = 1;
												flag = true;
											}
										}
										else if (!DateTime._ParseTimeSeparator(s, num, dfi, exact, out num16))
										{
											return false;
										}
									}
									else
									{
										if (!DateTime._ParseDateSeparator(s, num, dfi, exact, out num16))
										{
											return false;
										}
										num3 = 0;
									}
									break;
								case 'f':
									goto IL_A82;
								case 'h':
									if (num8 != -1)
									{
										return false;
									}
									if (num3 == 0)
									{
										num8 = DateTime._ParseNumber(s, num, 1, 2, false, sloppy_parsing, out num16);
									}
									else
									{
										num8 = DateTime._ParseNumber(s, num, 1, 2, true, sloppy_parsing, out num16);
									}
									if (num8 > 12)
									{
										return false;
									}
									if (num8 == 12)
									{
										num8 = 0;
									}
									break;
								}
								break;
							case 'y':
								if (num7 != -1)
								{
									return false;
								}
								if (num3 == 0)
								{
									num7 = DateTime._ParseNumber(s, num, 1, 2, false, sloppy_parsing, out num16);
								}
								else if (num3 < 3)
								{
									num7 = DateTime._ParseNumber(s, num, 1, 2, true, sloppy_parsing, out num16);
								}
								else
								{
									num7 = DateTime._ParseNumber(s, num, (!exact) ? 3 : 4, 4, false, sloppy_parsing, out num16);
									if (num7 >= 1000 && num16 == 4 && !longYear && s.Length > 4 + num)
									{
										int num18 = 0;
										int num19 = DateTime._ParseNumber(s, num, 5, 5, false, sloppy_parsing, out num18);
										longYear = (num19 > 9999);
									}
									num3 = 3;
								}
								if (num16 <= 2)
								{
									num7 += ((num7 >= 30) ? 1900 : 2000);
								}
								break;
							case 'z':
								if (num13 != -1)
								{
									return false;
								}
								if (s[num] == '+')
								{
									num13 = 0;
								}
								else
								{
									if (s[num] != '-')
									{
										return false;
									}
									num13 = 1;
								}
								num++;
								if (num3 == 0)
								{
									num14 = DateTime._ParseNumber(s, num, 1, 2, false, sloppy_parsing, out num16);
								}
								else if (num3 == 1)
								{
									num14 = DateTime._ParseNumber(s, num, 1, 2, true, sloppy_parsing, out num16);
								}
								else
								{
									num14 = DateTime._ParseNumber(s, num, 1, 2, true, true, out num16);
									num += num16;
									if (num16 < 0)
									{
										return false;
									}
									num16 = 0;
									if ((num < s.Length && char.IsDigit(s[num])) || DateTime._ParseTimeSeparator(s, num, dfi, exact, out num16))
									{
										num += num16;
										num15 = DateTime._ParseNumber(s, num, 1, 2, true, sloppy_parsing, out num16);
										if (num16 < 0)
										{
											return false;
										}
									}
									else
									{
										if (!flag3)
										{
											return false;
										}
										num16 = 0;
									}
								}
								break;
							}
							break;
						case 'K':
							if (s[num] == 'Z')
							{
								num++;
								flag = true;
							}
							else if (s[num] == '+' || s[num] == '-')
							{
								if (num13 != -1)
								{
									return false;
								}
								if (s[num] == '+')
								{
									num13 = 0;
								}
								else if (s[num] == '-')
								{
									num13 = 1;
								}
								num++;
								num14 = DateTime._ParseNumber(s, num, 0, 2, true, sloppy_parsing, out num16);
								num += num16;
								if (num16 < 0)
								{
									return false;
								}
								if (char.IsDigit(s[num]))
								{
									num16 = 0;
								}
								else if (!DateTime._ParseString(s, num, 0, dfi.TimeSeparator, out num16))
								{
									return false;
								}
								num += num16;
								num15 = DateTime._ParseNumber(s, num, 0, 2, true, sloppy_parsing, out num16);
								num3 = 2;
								if (num16 < 0)
								{
									return false;
								}
							}
							break;
						case 'M':
							if (num6 != -1)
							{
								return false;
							}
							if (flag3)
							{
								num16 = -1;
								if (num3 == 0 || num3 == 3)
								{
									num6 = DateTime._ParseNumber(s, num, 1, 2, false, sloppy_parsing, out num16);
								}
								if (num3 > 1 && num16 == -1)
								{
									num6 = DateTime._ParseEnum(s, num, dfi.RawMonthNames, invariantInfo.RawMonthNames, exact, out num16) + 1;
								}
								if (num3 > 1 && num16 == -1)
								{
									num6 = DateTime._ParseEnum(s, num, dfi.RawAbbreviatedMonthNames, invariantInfo.RawAbbreviatedMonthNames, exact, out num16) + 1;
								}
							}
							else if (num3 == 0)
							{
								num6 = DateTime._ParseNumber(s, num, 1, 2, false, sloppy_parsing, out num16);
							}
							else if (num3 == 1)
							{
								num6 = DateTime._ParseNumber(s, num, 1, 2, true, sloppy_parsing, out num16);
							}
							else if (num3 == 2)
							{
								num6 = DateTime._ParseEnum(s, num, dfi.RawAbbreviatedMonthNames, invariantInfo.RawAbbreviatedMonthNames, exact, out num16) + 1;
							}
							else
							{
								num6 = DateTime._ParseEnum(s, num, dfi.RawMonthNames, invariantInfo.RawMonthNames, exact, out num16) + 1;
							}
							break;
						}
						IL_DF3:
						if (num16 < 0)
						{
							return false;
						}
						num += num16;
						if (!exact && !flag3)
						{
							c = text2[num2];
							if (c == 'F' || c == 'f' || c == 'm' || c == 's' || c == 'z')
							{
								if (s.Length > num && s[num] == 'Z' && (num2 + 1 == text2.Length || text2[num2 + 1] != 'Z'))
								{
									flag = true;
									num++;
								}
							}
						}
						num2 = num2 + num3 + 1;
						num3 = 0;
						continue;
						IL_A82:
						if (num3 > 6 || num11 != -1.0)
						{
							return false;
						}
						double num20 = (double)DateTime._ParseNumber(s, num, 0, num3 + 1, leadingzero, sloppy_parsing, out num16);
						if (num16 == -1)
						{
							return false;
						}
						num11 = num20 / Math.Pow(10.0, (double)num16);
						goto IL_DF3;
					}
				}
			}
			goto IL_EA9;
		}

		/// <summary>Converts the specified string representation of a date and time to its <see cref="T:System.DateTime" /> equivalent using the specified format, culture-specific format information, and style. The format of the string representation must match the specified format exactly or an exception is thrown.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> equivalent to the date and time contained in <paramref name="s" /> as specified by <paramref name="format" />, <paramref name="provider" />, and <paramref name="style" />.</returns>
		/// <param name="s">A string containing a date and time to convert. </param>
		/// <param name="format">A format specifier that defines the required format of <paramref name="s" />. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />. </param>
		/// <param name="style">A bitwise combination of the enumeration values that provides additional information about <paramref name="s" />, about style elements that may be present in <paramref name="s" />, or about the conversion from <paramref name="s" /> to a <see cref="T:System.DateTime" /> value. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> or <paramref name="format" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> or <paramref name="format" /> is an empty string. -or- <paramref name="s" /> does not contain a date and time that corresponds to the pattern specified in <paramref name="format" />. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="style" /> contains an invalid combination of <see cref="T:System.Globalization.DateTimeStyles" /> values. For example, both <see cref="F:System.Globalization.DateTimeStyles.AssumeLocal" /> and <see cref="F:System.Globalization.DateTimeStyles.AssumeUniversal" />.</exception>
		/// <filterpriority>2</filterpriority>
		public static DateTime ParseExact(string s, string format, IFormatProvider provider, DateTimeStyles style)
		{
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			return DateTime.ParseExact(s, new string[]
			{
				format
			}, provider, style);
		}

		/// <summary>Converts the specified string representation of a date and time to its <see cref="T:System.DateTime" /> equivalent using the specified array of formats, culture-specific format information, and style. The format of the string representation must match at least one of the specified formats exactly or an exception is thrown.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> equivalent to the date and time contained in <paramref name="s" /> as specified by <paramref name="formats" />, <paramref name="provider" />, and <paramref name="style" />.</returns>
		/// <param name="s">A string containing one or more dates and times to convert. </param>
		/// <param name="formats">An array of allowable formats of <paramref name="s" />. </param>
		/// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies culture-specific format information about <paramref name="s" />. </param>
		/// <param name="style">A bitwise combination of <see cref="T:System.Globalization.DateTimeStyles" /> values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="s" /> or <paramref name="formats" /> is null. </exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="s" /> is an empty string. -or- an element of <paramref name="formats" /> is an empty string. -or- <paramref name="s" /> does not contain a date and time that corresponds to any element of <paramref name="formats" />. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="style" /> contains an invalid combination of <see cref="T:System.Globalization.DateTimeStyles" /> values. For example, both <see cref="F:System.Globalization.DateTimeStyles.AssumeLocal" /> and <see cref="F:System.Globalization.DateTimeStyles.AssumeUniversal" />.</exception>
		/// <filterpriority>2</filterpriority>
		public static DateTime ParseExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles style)
		{
			DateTimeFormatInfo instance = DateTimeFormatInfo.GetInstance(provider);
			DateTime.CheckStyle(style);
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (formats == null)
			{
				throw new ArgumentNullException("formats");
			}
			if (formats.Length == 0)
			{
				throw new FormatException("Format specifier was invalid.");
			}
			bool flag = false;
			Exception ex = null;
			DateTime result;
			if (!DateTime.ParseExact(s, formats, instance, style, out result, true, ref flag, true, ref ex))
			{
				throw ex;
			}
			return result;
		}

		private static void CheckStyle(DateTimeStyles style)
		{
			if ((style & DateTimeStyles.RoundtripKind) != DateTimeStyles.None && ((style & DateTimeStyles.AdjustToUniversal) != DateTimeStyles.None || (style & DateTimeStyles.AssumeLocal) != DateTimeStyles.None || (style & DateTimeStyles.AssumeUniversal) != DateTimeStyles.None))
			{
				throw new ArgumentException("The DateTimeStyles value RoundtripKind cannot be used with the values AssumeLocal, Asersal or AdjustToUniversal.", "style");
			}
			if ((style & DateTimeStyles.AssumeUniversal) != DateTimeStyles.None && (style & DateTimeStyles.AssumeLocal) != DateTimeStyles.None)
			{
				throw new ArgumentException("The DateTimeStyles values AssumeLocal and AssumeUniversal cannot be used together.", "style");
			}
		}

		/// <summary>Converts the specified string representation of a date and time to its <see cref="T:System.DateTime" /> equivalent and returns a value that indicates whether the conversion succeeded.</summary>
		/// <returns>true if the <paramref name="s" /> parameter was converted successfully; otherwise, false.</returns>
		/// <param name="s">A string containing a date and time to convert. </param>
		/// <param name="result">When this method returns, contains the <see cref="T:System.DateTime" /> value equivalent to the date and time contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.DateTime.MinValue" /> if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is null, is an empty string (""), or does not contain a valid string representation of a date and time. This parameter is passed uninitialized. </param>
		/// <filterpriority>1</filterpriority>
		public static bool TryParse(string s, out DateTime result)
		{
			if (s != null)
			{
				try
				{
					Exception ex = null;
					DateTimeOffset dateTimeOffset;
					return DateTime.CoreParse(s, null, DateTimeStyles.AllowWhiteSpaces, out result, out dateTimeOffset, false, ref ex);
				}
				catch
				{
				}
			}
			result = DateTime.MinValue;
			return false;
		}

		/// <summary>Converts the specified string representation of a date and time to its <see cref="T:System.DateTime" /> equivalent using the specified culture-specific format information and formatting style, and returns a value that indicates whether the conversion succeeded.</summary>
		/// <returns>true if the <paramref name="s" /> parameter was converted successfully; otherwise, false.</returns>
		/// <param name="s">A string containing a date and time to convert. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information about <paramref name="s" />. </param>
		/// <param name="styles">A bitwise combination of enumeration values that defines how to interpret the parsed date in relation to the current time zone or the current date. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
		/// <param name="result">When this method returns, contains the <see cref="T:System.DateTime" /> value equivalent to the date and time contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.DateTime.MinValue" /> if the conversion failed. The conversion fails if the <paramref name="s" /> parameter is null, is an empty string (""), or does not contain a valid string representation of a date and time. This parameter is passed uninitialized. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="styles" /> is not a valid <see cref="T:System.Globalization.DateTimeStyles" /> value.-or-<paramref name="styles" /> contains an invalid combination of <see cref="T:System.Globalization.DateTimeStyles" /> values (for example, both <see cref="F:System.Globalization.DateTimeStyles.AssumeLocal" /> and <see cref="F:System.Globalization.DateTimeStyles.AssumeUniversal" />).</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="provider" /> is a neutral culture and cannot be used in a parsing operation.</exception>
		/// <filterpriority>1</filterpriority>
		public static bool TryParse(string s, IFormatProvider provider, DateTimeStyles styles, out DateTime result)
		{
			if (s != null)
			{
				try
				{
					Exception ex = null;
					DateTimeOffset dateTimeOffset;
					return DateTime.CoreParse(s, provider, styles, out result, out dateTimeOffset, false, ref ex);
				}
				catch
				{
				}
			}
			result = DateTime.MinValue;
			return false;
		}

		/// <summary>Converts the specified string representation of a date and time to its <see cref="T:System.DateTime" /> equivalent using the specified format, culture-specific format information, and style. The format of the string representation must match the specified format exactly. The method returns a value that indicates whether the conversion succeeded.</summary>
		/// <returns>true if <paramref name="s" /> was converted successfully; otherwise, false.</returns>
		/// <param name="s">A string containing a date and time to convert. </param>
		/// <param name="format">The required format of <paramref name="s" />. </param>
		/// <param name="provider">An <see cref="T:System.IFormatProvider" /> object that supplies culture-specific formatting information about <paramref name="s" />. </param>
		/// <param name="style">A bitwise combination of one or more enumeration values that indicate the permitted format of <paramref name="s" />. </param>
		/// <param name="result">When this method returns, contains the <see cref="T:System.DateTime" /> value equivalent to the date and time contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.DateTime.MinValue" /> if the conversion failed. The conversion fails if either the <paramref name="s" /> or <paramref name="format" /> parameter is null, is an empty string, or does not contain a date and time that correspond to the pattern specified in <paramref name="format" />. This parameter is passed uninitialized. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="styles" /> is not a valid <see cref="T:System.Globalization.DateTimeStyles" /> value.-or-<paramref name="styles" /> contains an invalid combination of <see cref="T:System.Globalization.DateTimeStyles" /> values (for example, both <see cref="F:System.Globalization.DateTimeStyles.AssumeLocal" /> and <see cref="F:System.Globalization.DateTimeStyles.AssumeUniversal" />).</exception>
		/// <filterpriority>1</filterpriority>
		public static bool TryParseExact(string s, string format, IFormatProvider provider, DateTimeStyles style, out DateTime result)
		{
			return DateTime.TryParseExact(s, new string[]
			{
				format
			}, provider, style, out result);
		}

		/// <summary>Converts the specified string representation of a date and time to its <see cref="T:System.DateTime" /> equivalent using the specified array of formats, culture-specific format information, and style. The format of the string representation must match at least one of the specified formats exactly. The method returns a value that indicates whether the conversion succeeded.</summary>
		/// <returns>true if the <paramref name="s" /> parameter was converted successfully; otherwise, false.</returns>
		/// <param name="s">A string containing one or more dates and times to convert. </param>
		/// <param name="formats">An array of allowable formats of <paramref name="s" />. </param>
		/// <param name="provider">An object that supplies culture-specific format information about <paramref name="s" />. </param>
		/// <param name="style">A bitwise combination of enumeration values that indicates the permitted format of <paramref name="s" />. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.</param>
		/// <param name="result">When this method returns, contains the <see cref="T:System.DateTime" /> value equivalent to the date and time contained in <paramref name="s" />, if the conversion succeeded, or <see cref="F:System.DateTime.MinValue" /> if the conversion failed. The conversion fails if <paramref name="s" /> or <paramref name="formats" /> is null, <paramref name="s" /> or an element of <paramref name="formats" /> is an empty string, or the format of <paramref name="s" /> is not exactly as specified by at least one of the format patterns in <paramref name="formats" />. This parameter is passed uninitialized. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="styles" /> is not a valid <see cref="T:System.Globalization.DateTimeStyles" /> value.-or-<paramref name="styles" /> contains an invalid combination of <see cref="T:System.Globalization.DateTimeStyles" /> values (for example, both <see cref="F:System.Globalization.DateTimeStyles.AssumeLocal" /> and <see cref="F:System.Globalization.DateTimeStyles.AssumeUniversal" />).</exception>
		/// <filterpriority>1</filterpriority>
		public static bool TryParseExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles style, out DateTime result)
		{
			bool result2;
			try
			{
				DateTimeFormatInfo instance = DateTimeFormatInfo.GetInstance(provider);
				bool flag = false;
				Exception ex = null;
				result2 = DateTime.ParseExact(s, formats, instance, style, out result, true, ref flag, false, ref ex);
			}
			catch
			{
				result = DateTime.MinValue;
				result2 = false;
			}
			return result2;
		}

		private static bool ParseExact(string s, string[] formats, DateTimeFormatInfo dfi, DateTimeStyles style, out DateTime ret, bool exact, ref bool longYear, bool setExceptionOnError, ref Exception exception)
		{
			bool flag = false;
			for (int i = 0; i < formats.Length; i++)
			{
				string text = formats[i];
				if (text == null || text == string.Empty)
				{
					break;
				}
				DateTime dateTime;
				DateTimeOffset dateTimeOffset;
				if (DateTime._DoParse(s, formats[i], null, exact, out dateTime, out dateTimeOffset, dfi, style, false, ref flag, ref longYear))
				{
					ret = dateTime;
					return true;
				}
			}
			if (setExceptionOnError)
			{
				exception = new FormatException("Invalid format string");
			}
			ret = DateTime.MinValue;
			return false;
		}

		/// <summary>Subtracts the specified date and time from this instance.</summary>
		/// <returns>A <see cref="T:System.TimeSpan" /> interval equal to the date and time represented by this instance minus the date and time represented by <paramref name="value" />.</returns>
		/// <param name="value">An instance of <see cref="T:System.DateTime" />. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The result is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />. </exception>
		/// <filterpriority>2</filterpriority>
		public TimeSpan Subtract(DateTime value)
		{
			return new TimeSpan(this.ticks.Ticks) - value.ticks;
		}

		/// <summary>Subtracts the specified duration from this instance.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> equal to the date and time represented by this instance minus the time interval represented by <paramref name="value" />.</returns>
		/// <param name="value">An instance of <see cref="T:System.TimeSpan" />. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The result is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />. </exception>
		/// <filterpriority>2</filterpriority>
		public DateTime Subtract(TimeSpan value)
		{
			TimeSpan value2 = new TimeSpan(this.ticks.Ticks) - value;
			return new DateTime(true, value2)
			{
				kind = this.kind
			};
		}

		/// <summary>Converts the value of the current <see cref="T:System.DateTime" /> object to a Windows file time.</summary>
		/// <returns>The value of the current <see cref="T:System.DateTime" /> object expressed as a Windows file time.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The resulting file time would represent a date and time before 12:00 midnight January 1, 1601 C.E. UTC. </exception>
		/// <filterpriority>2</filterpriority>
		public long ToFileTime()
		{
			DateTime dateTime = this.ToUniversalTime();
			if (dateTime.Ticks < 504911232000000000L)
			{
				throw new ArgumentOutOfRangeException("file time is not valid");
			}
			return dateTime.Ticks - 504911232000000000L;
		}

		/// <summary>Converts the value of the current <see cref="T:System.DateTime" /> object to a Windows file time.</summary>
		/// <returns>The value of the current <see cref="T:System.DateTime" /> object expressed as a Windows file time.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The resulting file time would represent a date and time before 12:00 midnight January 1, 1601 C.E. UTC. </exception>
		/// <filterpriority>2</filterpriority>
		public long ToFileTimeUtc()
		{
			if (this.Ticks < 504911232000000000L)
			{
				throw new ArgumentOutOfRangeException("file time is not valid");
			}
			return this.Ticks - 504911232000000000L;
		}

		/// <summary>Converts the value of the current <see cref="T:System.DateTime" /> object to its equivalent long date string representation.</summary>
		/// <returns>A string that contains the long date string representation of the current <see cref="T:System.DateTime" /> object.</returns>
		/// <filterpriority>2</filterpriority>
		public string ToLongDateString()
		{
			return this.ToString("D");
		}

		/// <summary>Converts the value of the current <see cref="T:System.DateTime" /> object to its equivalent long time string representation.</summary>
		/// <returns>A string that contains the long time string representation of the current <see cref="T:System.DateTime" /> object.</returns>
		/// <filterpriority>2</filterpriority>
		public string ToLongTimeString()
		{
			return this.ToString("T");
		}

		/// <summary>Converts the value of this instance to the equivalent OLE Automation date.</summary>
		/// <returns>A double-precision floating-point number that contains an OLE Automation date equivalent to the value of this instance.</returns>
		/// <exception cref="T:System.OverflowException">The value of this instance cannot be represented as an OLE Automation date. </exception>
		/// <filterpriority>2</filterpriority>
		public double ToOADate()
		{
			long num = this.Ticks;
			if (num == 0L)
			{
				return 0.0;
			}
			if (num < 31242239136000000L)
			{
				return -657434.999;
			}
			TimeSpan timeSpan = new TimeSpan(this.Ticks - 599264352000000000L);
			double num2 = timeSpan.TotalDays;
			if (num < 599264352000000000L)
			{
				double num3 = Math.Ceiling(num2);
				num2 = num3 - 2.0 - (num2 - num3);
			}
			else if (num2 >= 2958466.0)
			{
				num2 = 2958465.99999999;
			}
			return num2;
		}

		/// <summary>Converts the value of the current <see cref="T:System.DateTime" /> object to its equivalent short date string representation.</summary>
		/// <returns>A string that contains the short date string representation of the current <see cref="T:System.DateTime" /> object.</returns>
		/// <filterpriority>2</filterpriority>
		public string ToShortDateString()
		{
			return this.ToString("d");
		}

		/// <summary>Converts the value of the current <see cref="T:System.DateTime" /> object to its equivalent short time string representation.</summary>
		/// <returns>A string that contains the short time string representation of the current <see cref="T:System.DateTime" /> object.</returns>
		/// <filterpriority>2</filterpriority>
		public string ToShortTimeString()
		{
			return this.ToString("t");
		}

		/// <summary>Converts the value of the current <see cref="T:System.DateTime" /> object to its equivalent string representation.</summary>
		/// <returns>A string representation of the value of the current <see cref="T:System.DateTime" /> object.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The date and time is outside the range of dates supported by the calendar used by the current culture. </exception>
		/// <filterpriority>1</filterpriority>
		public override string ToString()
		{
			return this.ToString("G", null);
		}

		/// <summary>Converts the value of the current <see cref="T:System.DateTime" /> object to its equivalent string representation using the specified culture-specific format information.</summary>
		/// <returns>A string representation of value of the current <see cref="T:System.DateTime" /> object as specified by <paramref name="provider" />.</returns>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The date and time is outside the range of dates supported by the calendar used by <paramref name="provider" />. </exception>
		/// <filterpriority>1</filterpriority>
		public string ToString(IFormatProvider provider)
		{
			return this.ToString(null, provider);
		}

		/// <summary>Converts the value of the current <see cref="T:System.DateTime" /> object to its equivalent string representation using the specified format.</summary>
		/// <returns>A string representation of value of the current <see cref="T:System.DateTime" /> object as specified by <paramref name="format" />.</returns>
		/// <param name="format">A standard or custom date and time format string. </param>
		/// <exception cref="T:System.FormatException">The length of <paramref name="format" /> is 1, and it is not one of the format specifier characters defined for <see cref="T:System.Globalization.DateTimeFormatInfo" />.-or- <paramref name="format" /> does not contain a valid custom format pattern. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The date and time is outside the range of dates supported by the calendar used by the current culture.</exception>
		/// <filterpriority>1</filterpriority>
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		/// <summary>Converts the value of the current <see cref="T:System.DateTime" /> object to its equivalent string representation using the specified format and culture-specific format information.</summary>
		/// <returns>A string representation of value of the current <see cref="T:System.DateTime" /> object as specified by <paramref name="format" /> and <paramref name="provider" />.</returns>
		/// <param name="format">A standard or custom date and time format string. </param>
		/// <param name="provider">An object that supplies culture-specific formatting information. </param>
		/// <exception cref="T:System.FormatException">The length of <paramref name="format" /> is 1, and it is not one of the format specifier characters defined for <see cref="T:System.Globalization.DateTimeFormatInfo" />.-or- <paramref name="format" /> does not contain a valid custom format pattern. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The date and time is outside the range of dates supported by the calendar used by <paramref name="provider" />. </exception>
		/// <filterpriority>1</filterpriority>
		public string ToString(string format, IFormatProvider provider)
		{
			DateTimeFormatInfo instance = DateTimeFormatInfo.GetInstance(provider);
			if (format == null || format == string.Empty)
			{
				format = "G";
			}
			bool flag = false;
			bool flag2 = false;
			if (format.Length == 1)
			{
				char c = format[0];
				format = DateTimeUtils.GetStandardPattern(c, instance, out flag, out flag2);
				if (c == 'U')
				{
					return DateTimeUtils.ToString(this.ToUniversalTime(), format, instance);
				}
				if (format == null)
				{
					throw new FormatException("format is not one of the format specifier characters defined for DateTimeFormatInfo");
				}
			}
			return DateTimeUtils.ToString(this, format, instance);
		}

		/// <summary>Converts the value of the current <see cref="T:System.DateTime" /> object to local time.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> object whose <see cref="P:System.DateTime.Kind" /> property is <see cref="F:System.DateTimeKind.Local" />, and whose value is the local time equivalent to the value of the current <see cref="T:System.DateTime" /> object, or <see cref="F:System.DateTime.MaxValue" /> if the converted value is too large to be represented by a <see cref="T:System.DateTime" /> object, or <see cref="F:System.DateTime.MinValue" /> if the converted value is too small to be represented as a <see cref="T:System.DateTime" /> object.</returns>
		/// <filterpriority>2</filterpriority>
		public DateTime ToLocalTime()
		{
			return TimeZone.CurrentTimeZone.ToLocalTime(this);
		}

		/// <summary>Converts the value of the current <see cref="T:System.DateTime" /> object to Coordinated Universal Time (UTC).</summary>
		/// <returns>A <see cref="T:System.DateTime" /> object whose <see cref="P:System.DateTime.Kind" /> property is <see cref="F:System.DateTimeKind.Utc" />, and whose value is the UTC equivalent to the value of the current <see cref="T:System.DateTime" /> object, or <see cref="F:System.DateTime.MaxValue" /> if the converted value is too large to be represented by a <see cref="T:System.DateTime" /> object, or <see cref="F:System.DateTime.MinValue" /> if the converted value is too small to be represented by a <see cref="T:System.DateTime" /> object.</returns>
		/// <filterpriority>2</filterpriority>
		public DateTime ToUniversalTime()
		{
			return TimeZone.CurrentTimeZone.ToUniversalTime(this);
		}

		/// <summary>Adds a specified time interval to a specified date and time, yielding a new date and time.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> that is the sum of the values of <paramref name="d" /> and <paramref name="t" />.</returns>
		/// <param name="d">A <see cref="T:System.DateTime" />. </param>
		/// <param name="t">A <see cref="T:System.TimeSpan" />. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTime" /> is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />. </exception>
		/// <filterpriority>3</filterpriority>
		public static DateTime operator +(DateTime d, TimeSpan t)
		{
			return new DateTime(true, d.ticks + t)
			{
				kind = d.kind
			};
		}

		/// <summary>Determines whether two specified instances of <see cref="T:System.DateTime" /> are equal.</summary>
		/// <returns>true if <paramref name="d1" /> and <paramref name="d2" /> represent the same date and time; otherwise, false.</returns>
		/// <param name="d1">A <see cref="T:System.DateTime" />. </param>
		/// <param name="d2">A <see cref="T:System.DateTime" />. </param>
		/// <filterpriority>3</filterpriority>
		public static bool operator ==(DateTime d1, DateTime d2)
		{
			return d1.ticks == d2.ticks;
		}

		/// <summary>Determines whether one specified <see cref="T:System.DateTime" /> is greater than another specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>true if <paramref name="t1" /> is greater than <paramref name="t2" />; otherwise, false.</returns>
		/// <param name="t1">A <see cref="T:System.DateTime" />. </param>
		/// <param name="t2">A <see cref="T:System.DateTime" />. </param>
		/// <filterpriority>3</filterpriority>
		public static bool operator >(DateTime t1, DateTime t2)
		{
			return t1.ticks > t2.ticks;
		}

		/// <summary>Determines whether one specified <see cref="T:System.DateTime" /> is greater than or equal to another specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>true if <paramref name="t1" /> is greater than or equal to <paramref name="t2" />; otherwise, false.</returns>
		/// <param name="t1">A <see cref="T:System.DateTime" />. </param>
		/// <param name="t2">A <see cref="T:System.DateTime" />. </param>
		/// <filterpriority>3</filterpriority>
		public static bool operator >=(DateTime t1, DateTime t2)
		{
			return t1.ticks >= t2.ticks;
		}

		/// <summary>Determines whether two specified instances of <see cref="T:System.DateTime" /> are not equal.</summary>
		/// <returns>true if <paramref name="d1" /> and <paramref name="d2" /> do not represent the same date and time; otherwise, false.</returns>
		/// <param name="d1">A <see cref="T:System.DateTime" />. </param>
		/// <param name="d2">A <see cref="T:System.DateTime" />. </param>
		/// <filterpriority>3</filterpriority>
		public static bool operator !=(DateTime d1, DateTime d2)
		{
			return d1.ticks != d2.ticks;
		}

		/// <summary>Determines whether one specified <see cref="T:System.DateTime" /> is less than another specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>true if <paramref name="t1" /> is less than <paramref name="t2" />; otherwise, false.</returns>
		/// <param name="t1">A <see cref="T:System.DateTime" />. </param>
		/// <param name="t2">A <see cref="T:System.DateTime" />. </param>
		/// <filterpriority>3</filterpriority>
		public static bool operator <(DateTime t1, DateTime t2)
		{
			return t1.ticks < t2.ticks;
		}

		/// <summary>Determines whether one specified <see cref="T:System.DateTime" /> is less than or equal to another specified <see cref="T:System.DateTime" />.</summary>
		/// <returns>true if <paramref name="t1" /> is less than or equal to <paramref name="t2" />; otherwise, false.</returns>
		/// <param name="t1">A <see cref="T:System.DateTime" />. </param>
		/// <param name="t2">A <see cref="T:System.DateTime" />. </param>
		/// <filterpriority>3</filterpriority>
		public static bool operator <=(DateTime t1, DateTime t2)
		{
			return t1.ticks <= t2.ticks;
		}

		/// <summary>Subtracts a specified date and time from another specified date and time and returns a time interval.</summary>
		/// <returns>A <see cref="T:System.TimeSpan" /> that is the time interval between <paramref name="d1" /> and <paramref name="d2" />; that is, <paramref name="d1" /> minus <paramref name="d2" />.</returns>
		/// <param name="d1">A <see cref="T:System.DateTime" /> (the minuend). </param>
		/// <param name="d2">A <see cref="T:System.DateTime" /> (the subtrahend). </param>
		/// <filterpriority>3</filterpriority>
		public static TimeSpan operator -(DateTime d1, DateTime d2)
		{
			return new TimeSpan((d1.ticks - d2.ticks).Ticks);
		}

		/// <summary>Subtracts a specified time interval from a specified date and time and returns a new date and time.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> whose value is the value of <paramref name="d" /> minus the value of <paramref name="t" />.</returns>
		/// <param name="d">A <see cref="T:System.DateTime" />. </param>
		/// <param name="t">A <see cref="T:System.TimeSpan" />. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTime" /> is less than <see cref="F:System.DateTime.MinValue" /> or greater than <see cref="F:System.DateTime.MaxValue" />. </exception>
		/// <filterpriority>3</filterpriority>
		public static DateTime operator -(DateTime d, TimeSpan t)
		{
			return new DateTime(true, d.ticks - t)
			{
				kind = d.kind
			};
		}

		private enum Which
		{
			Day,
			DayYear,
			Month,
			Year
		}
	}
}
