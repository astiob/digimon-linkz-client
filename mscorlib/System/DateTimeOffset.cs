using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	/// <summary>Represents a point in time, typically expressed as a date and time of day, relative to Coordinated Universal Time (UTC).</summary>
	/// <filterpriority>2</filterpriority>
	[Serializable]
	[StructLayout(LayoutKind.Auto)]
	public struct DateTimeOffset : IFormattable, IComparable, ISerializable, IComparable<DateTimeOffset>, IEquatable<DateTimeOffset>, IDeserializationCallback
	{
		/// <summary>Represents the greatest possible value of <see cref="T:System.DateTimeOffset" />. This field is read-only.</summary>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <see cref="F:System.DateTime.MaxValue" /> is outside the range of the current culture's or specified culture's default calendar.</exception>
		public static readonly DateTimeOffset MaxValue = new DateTimeOffset(DateTime.MaxValue, TimeSpan.Zero);

		/// <summary>Represents the earliest possible <see cref="T:System.DateTimeOffset" /> value. This field is read-only.</summary>
		public static readonly DateTimeOffset MinValue = new DateTimeOffset(DateTime.MinValue, TimeSpan.Zero);

		private DateTime dt;

		private TimeSpan utc_offset;

		/// <summary>Initializes a new instance of the <see cref="T:System.DateTimeOffset" /> structure using the specified <see cref="T:System.DateTime" /> value.</summary>
		/// <param name="dateTime">A date and time.   </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The Coordinated Universal Time (UTC) date and time that results from applying the offset is earlier than <see cref="F:System.DateTimeOffset.MinValue" />.-or-The UTC date and time that results from applying the offset is later than <see cref="F:System.DateTimeOffset.MaxValue" />.</exception>
		public DateTimeOffset(DateTime dateTime)
		{
			this.dt = dateTime;
			if (dateTime.Kind == DateTimeKind.Utc)
			{
				this.utc_offset = TimeSpan.Zero;
			}
			else
			{
				this.utc_offset = TimeZone.CurrentTimeZone.GetUtcOffset(dateTime);
			}
			if (this.UtcDateTime < DateTime.MinValue || this.UtcDateTime > DateTime.MaxValue)
			{
				throw new ArgumentOutOfRangeException("The UTC date and time that results from applying the offset is earlier than MinValue or later than MaxValue.");
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.DateTimeOffset" /> structure using the specified <see cref="T:System.DateTime" /> value and offset.</summary>
		/// <param name="dateTime">A date and time.   </param>
		/// <param name="offset">The time's offset from Coordinated Universal Time (UTC).</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="dateTime.Kind" /> equals <see cref="F:System.DateTimeKind.Utc" /> and <paramref name="offset" /> does not equal zero.-or-<paramref name="dateTime.Kind" /> equals <see cref="F:System.DateTimeKind.Local" /> and <paramref name="offset" /> does not equal the offset of the system's local time zone.-or-<paramref name="offset" /> is not specified in whole minutes.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than -14 hours or greater than 14 hours.-or-<see cref="P:System.DateTimeOffset.UtcDateTime" /> is less than <see cref="F:System.DateTimeOffset.MinValue" /> or greater than <see cref="F:System.DateTimeOffset.MaxValue" />. </exception>
		public DateTimeOffset(DateTime dateTime, TimeSpan offset)
		{
			if (dateTime.Kind == DateTimeKind.Utc && offset != TimeSpan.Zero)
			{
				throw new ArgumentException("dateTime.Kind equals Utc and offset does not equal zero.");
			}
			if (dateTime.Kind == DateTimeKind.Local && offset != TimeZone.CurrentTimeZone.GetUtcOffset(dateTime))
			{
				throw new ArgumentException("dateTime.Kind equals Local and offset does not equal the offset of the system's local time zone.");
			}
			if (offset.Ticks % 600000000L != 0L)
			{
				throw new ArgumentException("offset is not specified in whole minutes.");
			}
			if (offset < new TimeSpan(-14, 0, 0) || offset > new TimeSpan(14, 0, 0))
			{
				throw new ArgumentOutOfRangeException("offset is less than -14 hours or greater than 14 hours.");
			}
			this.dt = dateTime;
			this.utc_offset = offset;
			if (this.UtcDateTime < DateTime.MinValue || this.UtcDateTime > DateTime.MaxValue)
			{
				throw new ArgumentOutOfRangeException("The UtcDateTime property is earlier than MinValue or later than MaxValue.");
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.DateTimeOffset" /> structure using the specified number of ticks and offset.</summary>
		/// <param name="ticks">A date and time expressed as the number of 100-nanosecond intervals that have elapsed since 12:00:00 midnight on January 1, 0001.</param>
		/// <param name="offset">The time's offset from Coordinated Universal Time (UTC).</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="offset" /> is not specified in whole minutes.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <see cref="P:System.DateTimeOffset.UtcDateTime" /> property is earlier than <see cref="F:System.DateTimeOffset.MinValue" /> or later than <see cref="F:System.DateTimeOffset.MaxValue" />.-or-<paramref name="ticks" /> is less than DateTimeOffset.MinValue.Ticks or greater than DateTimeOffset.MaxValue.Ticks.-or-<paramref name="Offset" /> s less than -14 hours or greater than 14 hours.</exception>
		public DateTimeOffset(long ticks, TimeSpan offset)
		{
			this = new DateTimeOffset(new DateTime(ticks), offset);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.DateTimeOffset" /> structure using the specified year, month, day, hour, minute, second, and offset.</summary>
		/// <param name="year">The year (1 through 9999).</param>
		/// <param name="month">The month (1 through 12).</param>
		/// <param name="day">The day (1 through the number of days in <paramref name="month" />).</param>
		/// <param name="hour">The hours (0 through 23).   </param>
		/// <param name="minute">The minutes (0 through 59).</param>
		/// <param name="second">The seconds (0 through 59).</param>
		/// <param name="offset">The time's offset from Coordinated Universal Time (UTC).</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="offset" /> does not represent whole minutes.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> is less than one or greater than 9999.-or-<paramref name="month" /> is less than one or greater than 12.-or-<paramref name="day" /> is less than one or greater than the number of days in <paramref name="month" />.-or-<paramref name="hour" /> is less than zero or greater than 23.-or-<paramref name="minute" /> is less than 0 or greater than 59.-or-<paramref name="second" /> is less than 0 or greater than 59.-or-<paramref name="offset" /> is less than -14 hours or greater than 14 hours.-or-The <see cref="P:System.DateTimeOffset.UtcDateTime" /> property is earlier than <see cref="F:System.DateTimeOffset.MinValue" /> or later than <see cref="F:System.DateTimeOffset.MaxValue" />.</exception>
		public DateTimeOffset(int year, int month, int day, int hour, int minute, int second, TimeSpan offset)
		{
			this = new DateTimeOffset(new DateTime(year, month, day, hour, minute, second), offset);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.DateTimeOffset" /> structure using the specified year, month, day, hour, minute, second, millisecond, and offset.</summary>
		/// <param name="year">The year (1 through 9999).</param>
		/// <param name="month">The month (1 through 12).</param>
		/// <param name="day">The day (1 through the number of days in <paramref name="month" />).</param>
		/// <param name="hour">The hours (0 through 23).   </param>
		/// <param name="minute">The minutes (0 through 59).</param>
		/// <param name="second">The seconds (0 through 59).</param>
		/// <param name="millisecond">The milliseconds (0 through 999).</param>
		/// <param name="offset">The time's offset from Coordinated Universal Time (UTC).</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="offset" /> does not represent whole minutes.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> is less than one or greater than 9999.-or-<paramref name="month" /> is less than one or greater than 12.-or-<paramref name="day" /> is less than one or greater than the number of days in <paramref name="month" />.-or-<paramref name="hour" /> is less than zero or greater than 23.-or-<paramref name="minute" /> is less than 0 or greater than 59.-or-<paramref name="second" /> is less than 0 or greater than 59.-or-<paramref name="millisecond" /> is less than 0 or greater than 999.-or-<paramref name="offset" /> is less than -14 or greater than 14.-or-The <see cref="P:System.DateTimeOffset.UtcDateTime" /> property is earlier than <see cref="F:System.DateTimeOffset.MinValue" /> or later than <see cref="F:System.DateTimeOffset.MaxValue" />.</exception>
		public DateTimeOffset(int year, int month, int day, int hour, int minute, int second, int millisecond, TimeSpan offset)
		{
			this = new DateTimeOffset(new DateTime(year, month, day, hour, minute, second, millisecond), offset);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.DateTimeOffset" /> structure using the specified year, month, day, hour, minute, second, millisecond, and offset of a specified calendar.</summary>
		/// <param name="year">The year.</param>
		/// <param name="month">The month (1 through 12).</param>
		/// <param name="day">The day (1 through the number of days in <paramref name="month" />).</param>
		/// <param name="hour">The hours (0 through 23).   </param>
		/// <param name="minute">The minutes (0 through 59).</param>
		/// <param name="second">The seconds (0 through 59).</param>
		/// <param name="millisecond">The milliseconds (0 through 999).</param>
		/// <param name="calendar">The calendar whose time is defined.</param>
		/// <param name="offset">The time's offset from Coordinated Universal Time (UTC).</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="offset" /> does not represent whole minutes.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="calendar" /> cannot be null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="year" /> is less than the <paramref name="calendar" /> parameter's MinSupportedDateTime.Year or greater than MaxSupportedDateTime.Year.-or-<paramref name="month" /> is either less than or greater than the number of months in <paramref name="year" /> in the <paramref name="calendar" />. -or-<paramref name="day" /> is less than one or greater than the number of days in <paramref name="month" />.-or-<paramref name="hour" /> is less than zero or greater than 23.-or-<paramref name="minute" /> is less than 0 or greater than 59.-or-<paramref name="second" /> is less than 0 or greater than 59.-or-<paramref name="millisecond" /> is less than 0 or greater than 999.-or-<paramref name="offset" /> is less than -14 hours or greater than 14 hours.-or-The <paramref name="year" />, <paramref name="month" />, and <paramref name="day" /> parameters cannot be represented as a date and time value.-or-The <see cref="P:System.DateTimeOffset.UtcDateTime" /> property is earlier than <see cref="F:System.DateTimeOffset.MinValue" /> or later than <see cref="F:System.DateTimeOffset.MaxValue" />.</exception>
		public DateTimeOffset(int year, int month, int day, int hour, int minute, int second, int millisecond, Calendar calendar, TimeSpan offset)
		{
			this = new DateTimeOffset(new DateTime(year, month, day, hour, minute, second, millisecond, calendar), offset);
		}

		private DateTimeOffset(SerializationInfo info, StreamingContext context)
		{
			DateTime dateTime = (DateTime)info.GetValue("DateTime", typeof(DateTime));
			short @int = info.GetInt16("OffsetMinutes");
			this.utc_offset = TimeSpan.FromMinutes((double)@int);
			this.dt = dateTime.Add(this.utc_offset);
		}

		static DateTimeOffset()
		{
			if (MonoTouchAOTHelper.FalseFlag)
			{
				GenericComparer<DateTimeOffset> genericComparer = new GenericComparer<DateTimeOffset>();
				GenericEqualityComparer<DateTimeOffset> genericEqualityComparer = new GenericEqualityComparer<DateTimeOffset>();
			}
		}

		/// <summary>Compares the value of the current <see cref="T:System.DateTimeOffset" /> object with another object of the same type.</summary>
		/// <returns>A 32-bit signed integer that indicates whether the current <see cref="T:System.DateTimeOffset" /> object is less than, equal to, or greater than <paramref name="obj" />. The return values of the method are interpreted as follows:Return ValueDescriptionLess than zeroThe current <see cref="T:System.DateTimeOffset" /> object is less than (earlier than) <paramref name="obj" />.ZeroThe current <see cref="T:System.DateTimeOffset" /> object is equal to (the same point in time as) <paramref name="obj" />.Greater than zeroThe current <see cref="T:System.DateTimeOffset" /> object is greater than (later than) <paramref name="obj" />.</returns>
		/// <param name="obj">The object to compare with the current <see cref="T:System.DateTimeOffset" /> object.</param>
		int IComparable.CompareTo(object obj)
		{
			return this.CompareTo((DateTimeOffset)obj);
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the data required to serialize the current <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object to populate with data.</param>
		/// <param name="context">The destination for this serialization (see <see cref="T:System.Runtime.Serialization.StreamingContext" />).</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is null.</exception>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			DateTime dateTime = new DateTime(this.dt.Ticks);
			DateTime value = dateTime.Subtract(this.utc_offset);
			info.AddValue("DateTime", value);
			info.AddValue("OffsetMinutes", (short)this.utc_offset.TotalMinutes);
		}

		/// <summary>Runs when the deserialization of an object has been completed.</summary>
		/// <param name="sender">The object that initiated the callback. The functionality for this parameter is not currently implemented.</param>
		[MonoTODO]
		void IDeserializationCallback.OnDeserialization(object sender)
		{
		}

		/// <summary>Adds a specified time interval to a <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>A <see cref="T:System.DateTimeOffset" /> object whose value is the sum of the date and time represented by the current <see cref="T:System.DateTimeOffset" /> object and the time interval represented by <paramref name="timeSpan" />.</returns>
		/// <param name="timeSpan">A <see cref="T:System.TimeSpan" /> object that represents a positive or a negative time interval.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTimeOffset" /> value is less than <see cref="F:System.DateTimeOffset.MinValue" />.-or- The resulting <see cref="T:System.DateTimeOffset" /> value is greater than <see cref="F:System.DateTimeOffset.MaxValue" />.</exception>
		public DateTimeOffset Add(TimeSpan timeSpan)
		{
			return new DateTimeOffset(this.dt.Add(timeSpan), this.utc_offset);
		}

		/// <summary>Adds a specified number of whole and fractional days to the current <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>A <see cref="T:System.DateTimeOffset" /> object whose value is the sum of the date and time represented by the current <see cref="T:System.DateTimeOffset" /> object and the number of days represented by <paramref name="days" />.</returns>
		/// <param name="days">A number of whole and fractional days. The number can be negative or positive.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTimeOffset" /> value is less than <see cref="F:System.DateTimeOffset.MinValue" />.-or- The resulting <see cref="T:System.DateTimeOffset" /> value is greater than <see cref="F:System.DateTimeOffset.MaxValue" />.</exception>
		public DateTimeOffset AddDays(double days)
		{
			return new DateTimeOffset(this.dt.AddDays(days), this.utc_offset);
		}

		/// <summary>Adds a specified number of whole and fractional hours to the current <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>A <see cref="T:System.DateTimeOffset" /> object whose value is the sum of the date and time represented by the current <see cref="T:System.DateTimeOffset" /> object and the number of hours represented by <paramref name="hours" />.</returns>
		/// <param name="hours">A number of whole and fractional hours. The number can be negative or positive.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTimeOffset" /> value is less than <see cref="F:System.DateTimeOffset.MinValue" />.-or- The resulting <see cref="T:System.DateTimeOffset" /> value is greater than <see cref="F:System.DateTimeOffset.MaxValue" />.</exception>
		public DateTimeOffset AddHours(double hours)
		{
			return new DateTimeOffset(this.dt.AddHours(hours), this.utc_offset);
		}

		/// <summary>Adds a specified number of milliseconds to the current <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>A <see cref="T:System.DateTimeOffset" /> object whose value is the sum of the date and time represented by the current <see cref="T:System.DateTimeOffset" /> object and the number of whole milliseconds represented by <paramref name="milliseconds" />.</returns>
		/// <param name="milliseconds">A number of whole and fractional milliseconds. The number can be negative or positive.   </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTimeOffset" /> value is less than <see cref="F:System.DateTimeOffset.MinValue" />.-or- The resulting <see cref="T:System.DateTimeOffset" /> value is greater than <see cref="F:System.DateTimeOffset.MaxValue" />.</exception>
		public DateTimeOffset AddMilliseconds(double milliseconds)
		{
			return new DateTimeOffset(this.dt.AddMilliseconds(milliseconds), this.utc_offset);
		}

		/// <summary>Adds a specified number of whole and fractional minutes to the current <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>A <see cref="T:System.DateTimeOffset" /> object whose value is the sum of the date and time represented by the current <see cref="T:System.DateTimeOffset" /> object and the number of minutes represented by <paramref name="minutes" />.</returns>
		/// <param name="minutes">A number of whole and fractional minutes. The number can be negative or positive.   </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTimeOffset" /> value is less than <see cref="F:System.DateTimeOffset.MinValue" />.-or- The resulting <see cref="T:System.DateTimeOffset" /> value is greater than <see cref="F:System.DateTimeOffset.MaxValue" />.</exception>
		public DateTimeOffset AddMinutes(double minutes)
		{
			return new DateTimeOffset(this.dt.AddMinutes(minutes), this.utc_offset);
		}

		/// <summary>Adds a specified number of months to the current <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>A <see cref="T:System.DateTimeOffset" /> object whose value is the sum of the date and time represented by the current <see cref="T:System.DateTimeOffset" /> object and the number of months represented by <paramref name="months" />.</returns>
		/// <param name="months">A number of whole months. The number can be negative or positive.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTimeOffset" /> value is less than <see cref="F:System.DateTimeOffset.MinValue" />.-or- The resulting <see cref="T:System.DateTimeOffset" /> value is greater than <see cref="F:System.DateTimeOffset.MaxValue" />.</exception>
		public DateTimeOffset AddMonths(int months)
		{
			return new DateTimeOffset(this.dt.AddMonths(months), this.utc_offset);
		}

		/// <summary>Adds a specified number of whole and fractional seconds to the current <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>A <see cref="T:System.DateTimeOffset" /> object whose value is the sum of the date and time represented by the current <see cref="T:System.DateTimeOffset" /> object and the number of seconds represented by <paramref name="seconds" />.</returns>
		/// <param name="seconds">A number of whole and fractional seconds. The number can be negative or positive.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTimeOffset" /> value is less than <see cref="F:System.DateTimeOffset.MinValue" />.-or- The resulting <see cref="T:System.DateTimeOffset" /> value is greater than <see cref="F:System.DateTimeOffset.MaxValue" />.</exception>
		public DateTimeOffset AddSeconds(double seconds)
		{
			return new DateTimeOffset(this.dt.AddSeconds(seconds), this.utc_offset);
		}

		/// <summary>Adds a specified number of ticks to the current <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>A <see cref="T:System.DateTimeOffset" /> object whose value is the sum of the date and time represented by the current <see cref="T:System.DateTimeOffset" /> object and the number of ticks represented by <paramref name="ticks" />.</returns>
		/// <param name="ticks">A number of 100-nanosecond ticks. The number can be negative or positive.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTimeOffset" /> value is less than <see cref="F:System.DateTimeOffset.MinValue" />.-or- The resulting <see cref="T:System.DateTimeOffset" /> value is greater than <see cref="F:System.DateTimeOffset.MaxValue" />.</exception>
		public DateTimeOffset AddTicks(long ticks)
		{
			return new DateTimeOffset(this.dt.AddTicks(ticks), this.utc_offset);
		}

		/// <summary>Adds a specified number of years to the <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>A <see cref="T:System.DateTimeOffset" /> object whose value is the sum of the date and time represented by the current <see cref="T:System.DateTimeOffset" /> object and the number of years represented by <paramref name="years" />.</returns>
		/// <param name="years">A number of years. The number can be negative or positive.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTimeOffset" /> value is less than <see cref="F:System.DateTimeOffset.MinValue" />.-or- The resulting <see cref="T:System.DateTimeOffset" /> value is greater than <see cref="F:System.DateTimeOffset.MaxValue" />.</exception>
		public DateTimeOffset AddYears(int years)
		{
			return new DateTimeOffset(this.dt.AddYears(years), this.utc_offset);
		}

		/// <summary>Compares two <see cref="T:System.DateTimeOffset" /> objects and indicates whether the first is earlier than the second, equal to the second, or later than the second.</summary>
		/// <returns>A signed integer that indicates whether the value of the <paramref name="first" /> parameter is earlier than, later than, or the same time as the value of the <paramref name="second" /> parameter, as the following table shows.Return valueConditionLess than zero<paramref name="first" /> is earlier than <paramref name="second" />.Zero<paramref name="first" /> is equal to <paramref name="second" />.Greater than zero<paramref name="first" /> is later than <paramref name="second" />.</returns>
		/// <param name="first">The first <see cref="T:System.DateTimeOffset" /> object.</param>
		/// <param name="second">The second <see cref="T:System.DateTimeOffset" /> object.</param>
		public static int Compare(DateTimeOffset first, DateTimeOffset second)
		{
			return first.CompareTo(second);
		}

		/// <summary>Compares the current <see cref="T:System.DateTimeOffset" /> object to a specified <see cref="T:System.DateTimeOffset" /> object and indicates whether the current object is earlier than, the same as, or later than the second <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>A signed integer that indicates the relationship between the current <see cref="T:System.DateTimeOffset" /> object and <paramref name="other" />, as the following table shows.Return ValueDescriptionLess than zeroThe current <see cref="T:System.DateTimeOffset" /> object is earlier than <paramref name="other" />.ZeroThe current <see cref="T:System.DateTimeOffset" /> object is the same as <paramref name="other" />.Greater than zero.The current <see cref="T:System.DateTimeOffset" /> object is later than <paramref name="other" />.</returns>
		/// <param name="other">A <see cref="T:System.DateTimeOffset" /> object to compare with the current <see cref="T:System.DateTimeOffset" /> object.</param>
		public int CompareTo(DateTimeOffset other)
		{
			return this.UtcDateTime.CompareTo(other.UtcDateTime);
		}

		/// <summary>Determines whether the current <see cref="T:System.DateTimeOffset" /> object represents the same point in time as a specified <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>true if both <see cref="T:System.DateTimeOffset" /> objects have the same <see cref="P:System.DateTimeOffset.UtcDateTime" /> value; otherwise, false.</returns>
		/// <param name="other">A <see cref="T:System.DateTimeOffset" /> object to compare to the current <see cref="T:System.DateTimeOffset" /> object.   </param>
		public bool Equals(DateTimeOffset other)
		{
			return this.UtcDateTime == other.UtcDateTime;
		}

		/// <summary>Determines whether a <see cref="T:System.DateTimeOffset" /> object represents the same point in time as a specified object.</summary>
		/// <returns>true if the <paramref name="obj" /> parameter is a <see cref="T:System.DateTimeOffset" /> object and represents the same point in time as the current <see cref="T:System.DateTimeOffset" /> object; otherwise, false.</returns>
		/// <param name="obj">The object to compare to the current <see cref="T:System.DateTimeOffset" /> object.</param>
		public override bool Equals(object obj)
		{
			return obj is DateTimeOffset && this.UtcDateTime == ((DateTimeOffset)obj).UtcDateTime;
		}

		/// <summary>Determines whether two specified <see cref="T:System.DateTimeOffset" /> objects represent the same point in time.</summary>
		/// <returns>true if the two <see cref="T:System.DateTimeOffset" /> objects have the same <see cref="P:System.DateTimeOffset.UtcDateTime" /> value; otherwise, false.</returns>
		/// <param name="first">The first <see cref="T:System.DateTimeOffset" /> object.</param>
		/// <param name="second">The second <see cref="T:System.DateTimeOffset" /> object.</param>
		public static bool Equals(DateTimeOffset first, DateTimeOffset second)
		{
			return first.Equals(second);
		}

		/// <summary>Determines whether the current <see cref="T:System.DateTimeOffset" /> object represents the same time and has the same offset as a specified <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>true if the current <see cref="T:System.DateTimeOffset" /> object and <paramref name="other" /> have the same date and time value and the same <see cref="P:System.DateTimeOffset.Offset" /> value; otherwise, false.</returns>
		/// <param name="other">The <see cref="T:System.DateTimeOffset" /> object to compare to the current <see cref="T:System.DateTimeOffset" /> object. </param>
		public bool EqualsExact(DateTimeOffset other)
		{
			return this.dt == other.dt && this.utc_offset == other.utc_offset;
		}

		/// <summary>Converts the specified Windows file time to an equivalent local time.</summary>
		/// <returns>A <see cref="T:System.DateTimeOffset" /> object that represents the date and time of <paramref name="fileTime" /> with the offset set to the local time offset.</returns>
		/// <param name="fileTime">A Windows file time, expressed in ticks.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="filetime" /> is less than zero.-or-<paramref name="filetime" /> is greater than DateTimeOffset.MaxValue.Ticks.</exception>
		public static DateTimeOffset FromFileTime(long fileTime)
		{
			if (fileTime < 0L || fileTime > DateTimeOffset.MaxValue.Ticks)
			{
				throw new ArgumentOutOfRangeException("fileTime is less than zero or greater than DateTimeOffset.MaxValue.Ticks.");
			}
			return new DateTimeOffset(DateTime.FromFileTime(fileTime), TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.FromFileTime(fileTime)));
		}

		/// <summary>Returns the hash code for the current <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			return this.dt.GetHashCode() ^ this.utc_offset.GetHashCode();
		}

		/// <summary>Converts the specified string representation of a date, time, and offset to its <see cref="T:System.DateTimeOffset" /> equivalent.</summary>
		/// <returns>A <see cref="T:System.DateTimeOffset" /> object that is equivalent to the date and time that is contained in <paramref name="input" />.</returns>
		/// <param name="input">A string that contains a date and time to convert.</param>
		/// <exception cref="T:System.ArgumentException">The offset is greater than 14 hours or less than -14 hours.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="input" /> is null.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="input" /> does not contain a valid string representation of a date and time.-or-<paramref name="input" /> contains the string representation of an offset value without a date or time.</exception>
		public static DateTimeOffset Parse(string input)
		{
			return DateTimeOffset.Parse(input, null);
		}

		/// <summary>Converts the specified string representation of a date and time to its <see cref="T:System.DateTimeOffset" /> equivalent using the specified culture-specific format information.</summary>
		/// <returns>A <see cref="T:System.DateTimeOffset" /> equivalent to the date and time that is contained in <paramref name="input" /> as specified by <paramref name="formatProvider" />.</returns>
		/// <param name="input">A string that contains a date and time to convert.   </param>
		/// <param name="formatProvider">An <see cref="T:System.IFormatProvider" /> object that provides culture-specific format information about <paramref name="input" />.</param>
		/// <exception cref="T:System.ArgumentException">The offset is greater than 14 hours or less than -14 hours.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="input" /> is null.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="input" /> does not contain a valid string representation of a date and time.-or-<paramref name="input" /> contains the string representation of an offset value without a date or time.</exception>
		public static DateTimeOffset Parse(string input, IFormatProvider formatProvider)
		{
			return DateTimeOffset.Parse(input, formatProvider, DateTimeStyles.AllowWhiteSpaces);
		}

		/// <summary>Converts the specified string representation of a date and time to its <see cref="T:System.DateTimeOffset" /> equivalent using the specified culture-specific format information and formatting style.</summary>
		/// <returns>A <see cref="T:System.DateTimeOffset" /> object that is equivalent to the date and time that is contained in <paramref name="input" /> as specified by <paramref name="formatProvider" /> and <paramref name="styles" />.</returns>
		/// <param name="input">A string that contains a date and time to convert.   </param>
		/// <param name="formatProvider">An <see cref="T:System.IFormatProvider" /> object that provides culture-specific format information about <paramref name="input" />.</param>
		/// <param name="styles">A bitwise combination of <see cref="T:System.Globalization.DateTimeStyles" /> values that indicates the permitted format of <paramref name="input" />. A typical value to specify is <see cref="F:System.Globalization.DateTimeStyles.None" />.   </param>
		/// <exception cref="T:System.ArgumentException">The offset is greater than 14 hours or less than -14 hours.-or-<paramref name="styles" /> is not a valid <see cref="T:System.Globalization.DateTimeStyles" /> value.-or-<paramref name="styles" /> includes an unsupported <see cref="T:System.Globalization.DateTimeStyles" /> value.-or-<paramref name="styles" /> includes <see cref="T:System.Globalization.DateTimeStyles" /> values that cannot be used together.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="input" /> is null.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="input" /> does not contain a valid string representation of a date and time.-or-<paramref name="input" /> contains the string representation of an offset value without a date or time.</exception>
		public static DateTimeOffset Parse(string input, IFormatProvider formatProvider, DateTimeStyles styles)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			Exception ex = null;
			DateTime dateTime;
			DateTimeOffset result;
			if (!DateTime.CoreParse(input, formatProvider, styles, out dateTime, out result, true, ref ex))
			{
				throw ex;
			}
			if (dateTime.Ticks != 0L && result.Ticks == 0L)
			{
				throw new ArgumentOutOfRangeException("The UTC representation falls outside the 1-9999 year range");
			}
			return result;
		}

		/// <summary>Converts the specified string representation of a date and time to its <see cref="T:System.DateTimeOffset" /> equivalent using the specified format and culture-specific format information. The format of the string representation must match the specified format exactly.</summary>
		/// <returns>A <see cref="T:System.DateTimeOffset" /> object that is equivalent to the date and time that is contained in <paramref name="input" /> as specified by <paramref name="format" /> and <paramref name="formatProvider" />.</returns>
		/// <param name="input">A string that contains a date and time to convert.</param>
		/// <param name="format">A format specifier that defines the expected format of <paramref name="input" />.</param>
		/// <param name="formatProvider">An <see cref="T:System.IFormatProvider" /> object that supplies culture-specific formatting information about <paramref name="input" />.</param>
		/// <exception cref="T:System.ArgumentException">The offset is greater than 14 hours or less than -14 hours.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="input" /> is null.-or-<paramref name="format" /> is null.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="input" /> is an empty string ("").-or-<paramref name="input" /> does not contain a valid string representation of a date and time.-or-<paramref name="format" /> is an empty string.</exception>
		public static DateTimeOffset ParseExact(string input, string format, IFormatProvider formatProvider)
		{
			return DateTimeOffset.ParseExact(input, format, formatProvider, DateTimeStyles.AssumeLocal);
		}

		/// <summary>Converts the specified string representation of a date and time to its <see cref="T:System.DateTimeOffset" /> equivalent using the specified format, culture-specific format information, and style. The format of the string representation must match the specified format exactly.</summary>
		/// <returns>A <see cref="T:System.DateTimeOffset" /> equivalent to the date and time that is contained in the <paramref name="input" /> parameter as specified by the <paramref name="format" />, <paramref name="formatProvider" />, and <paramref name="styles" /> parameters.</returns>
		/// <param name="input">A string that contains a date and time to convert.</param>
		/// <param name="format">A format specifier that defines the expected format of <paramref name="input" />.</param>
		/// <param name="formatProvider">An <see cref="T:System.IFormatProvider" /> object that supplies culture-specific formatting information about <paramref name="input" />.</param>
		/// <param name="styles">A bitwise combination of <see cref="T:System.Globalization.DateTimeStyles" /> values that indicates the permitted format of <paramref name="input" />.</param>
		/// <exception cref="T:System.ArgumentException">The offset is greater than 14 hours or less than -14 hours.-or-The <paramref name="styles" /> parameter includes an unsupported value.-or-The <paramref name="styles" /> parameter contains <see cref="T:System.Globalization.DateTimeStyles" /> values that cannot be used together.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="input" /> is null.-or-<paramref name="format" /> is null.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="input" /> is an empty string ("").-or-<paramref name="input" /> does not contain a valid string representation of a date and time.-or-<paramref name="format" /> is an empty string.</exception>
		public static DateTimeOffset ParseExact(string input, string format, IFormatProvider formatProvider, DateTimeStyles styles)
		{
			if (format == null)
			{
				throw new ArgumentNullException("format");
			}
			if (format == string.Empty)
			{
				throw new FormatException("format is an empty string");
			}
			return DateTimeOffset.ParseExact(input, new string[]
			{
				format
			}, formatProvider, styles);
		}

		/// <summary>Converts the specified string representation of a date and time to its <see cref="T:System.DateTimeOffset" /> equivalent using the specified formats, culture-specific format information, and style. The format of the string representation must match one of the specified formats exactly.</summary>
		/// <returns>A <see cref="T:System.DateTimeOffset" /> equivalent to the date and time that is contained in the <paramref name="input" /> parameter as specified by the <paramref name="formats" />, <paramref name="formatProvider" />, and <paramref name="styles" /> parameters.</returns>
		/// <param name="input">A string that contains a date and time to convert.</param>
		/// <param name="formats">An array of format specifiers that define the expected formats of <paramref name="input" />.</param>
		/// <param name="formatProvider">An <see cref="T:System.IFormatProvider" /> object that supplies culture-specific formatting information about <paramref name="input" />.</param>
		/// <param name="styles">A bitwise combination of <see cref="T:System.Globalization.DateTimeStyles" /> values that indicates the permitted format of <paramref name="input" />.</param>
		/// <exception cref="T:System.ArgumentException">The offset is greater than 14 hours or less than -14 hours.-or-<paramref name="styles" /> includes an unsupported value.-or-The <paramref name="styles" /> parameter contains <see cref="T:System.Globalization.DateTimeStyles" /> values that cannot be used together.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="input" /> is null.</exception>
		/// <exception cref="T:System.FormatException">
		///   <paramref name="input" /> is an empty string ("").-or-<paramref name="input" /> does not contain a valid string representation of a date and time.-or-No element of <paramref name="formats" /> contains a valid format specifier.</exception>
		public static DateTimeOffset ParseExact(string input, string[] formats, IFormatProvider formatProvider, DateTimeStyles styles)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (input == string.Empty)
			{
				throw new FormatException("input is an empty string");
			}
			if (formats == null)
			{
				throw new ArgumentNullException("formats");
			}
			if (formats.Length == 0)
			{
				throw new FormatException("Invalid format specifier");
			}
			if ((styles & DateTimeStyles.AssumeLocal) != DateTimeStyles.None && (styles & DateTimeStyles.AssumeUniversal) != DateTimeStyles.None)
			{
				throw new ArgumentException("styles parameter contains incompatible flags");
			}
			DateTimeOffset result;
			if (!DateTimeOffset.ParseExact(input, formats, DateTimeFormatInfo.GetInstance(formatProvider), styles, out result))
			{
				throw new FormatException("Invalid format string");
			}
			return result;
		}

		private static bool ParseExact(string input, string[] formats, DateTimeFormatInfo dfi, DateTimeStyles styles, out DateTimeOffset ret)
		{
			foreach (string text in formats)
			{
				if (text == null || text == string.Empty)
				{
					throw new FormatException("Invalid format string");
				}
				DateTimeOffset dateTimeOffset;
				if (DateTimeOffset.DoParse(input, text, false, out dateTimeOffset, dfi, styles))
				{
					ret = dateTimeOffset;
					return true;
				}
			}
			ret = DateTimeOffset.MinValue;
			return false;
		}

		private static bool DoParse(string input, string format, bool exact, out DateTimeOffset result, DateTimeFormatInfo dfi, DateTimeStyles styles)
		{
			if ((styles & DateTimeStyles.AllowLeadingWhite) != DateTimeStyles.None)
			{
				format = format.TrimStart(null);
				input = input.TrimStart(null);
			}
			if ((styles & DateTimeStyles.AllowTrailingWhite) != DateTimeStyles.None)
			{
				format = format.TrimEnd(null);
				input = input.TrimEnd(null);
			}
			bool allow_leading_white = false;
			if ((styles & DateTimeStyles.AllowInnerWhite) != DateTimeStyles.None)
			{
				allow_leading_white = true;
			}
			bool flag = false;
			bool flag2 = false;
			if (format.Length == 1)
			{
				format = DateTimeUtils.GetStandardPattern(format[0], dfi, out flag, out flag2, true);
			}
			int num = -1;
			int num2 = -1;
			int num3 = -1;
			int num4 = -1;
			int num5 = -1;
			int num6 = -1;
			int num7 = -1;
			double num8 = -1.0;
			int num9 = -1;
			TimeSpan timeSpan = TimeSpan.MinValue;
			result = DateTimeOffset.MinValue;
			int i = 0;
			int num10 = 0;
			while (i < format.Length)
			{
				char c = format[i];
				char c2 = c;
				int num11;
				switch (c2)
				{
				case 's':
					num11 = DateTimeUtils.CountRepeat(format, i, c);
					if (num7 != -1 || num11 > 2)
					{
						return false;
					}
					num10 += DateTimeOffset.ParseNumber(input, num10, 2, num11 == 2, allow_leading_white, out num7);
					break;
				case 't':
				{
					num11 = DateTimeUtils.CountRepeat(format, i, c);
					if (num5 != -1 || num11 > 2)
					{
						return false;
					}
					int num12 = num10;
					string input2 = input;
					int pos = num10;
					string[] enums;
					if (num11 == 1)
					{
						string[] array = new string[2];
						array[0] = new string(dfi.AMDesignator[0], 1);
						enums = array;
						array[1] = new string(dfi.PMDesignator[0], 0);
					}
					else
					{
						string[] array2 = new string[2];
						array2[0] = dfi.AMDesignator;
						enums = array2;
						array2[1] = dfi.PMDesignator;
					}
					num10 = num12 + DateTimeOffset.ParseEnum(input2, pos, enums, allow_leading_white, out num9);
					if (num9 == -1)
					{
						return false;
					}
					if (num4 == -1)
					{
						num4 = num9 * 12;
					}
					else
					{
						num5 = num4 + num9 * 12;
					}
					break;
				}
				default:
					switch (c2)
					{
					case 'd':
						num11 = DateTimeUtils.CountRepeat(format, i, c);
						if (num3 != -1 || num11 > 4)
						{
							return false;
						}
						if (num11 <= 2)
						{
							num10 += DateTimeOffset.ParseNumber(input, num10, 2, num11 == 2, allow_leading_white, out num3);
						}
						else
						{
							num10 += DateTimeOffset.ParseEnum(input, num10, (num11 != 3) ? dfi.DayNames : dfi.AbbreviatedDayNames, allow_leading_white, out num9);
						}
						break;
					default:
						switch (c2)
						{
						case 'F':
						{
							num11 = DateTimeUtils.CountRepeat(format, i, c);
							int num14;
							int num13 = DateTimeOffset.ParseNumber(input, num10, num11, true, allow_leading_white, out num9, out num14);
							if (num9 == -1)
							{
								num10 += DateTimeOffset.ParseNumber(input, num10, num14, true, allow_leading_white, out num9);
							}
							else
							{
								num10 += num13;
							}
							if (num8 >= 0.0 || num11 > 7 || num9 == -1)
							{
								return false;
							}
							num8 = (double)num9 / Math.Pow(10.0, (double)num14);
							break;
						}
						default:
							if (c2 != ' ')
							{
								if (c2 != '%')
								{
									if (c2 != '/')
									{
										if (c2 != ':')
										{
											if (c2 != 'M')
											{
												if (c2 != '\\')
												{
													if (c2 != 'm')
													{
														num11 = 1;
														num10 += DateTimeOffset.ParseChar(input, num10, format[i], allow_leading_white, out num9);
														if (num9 == -1)
														{
															return false;
														}
													}
													else
													{
														num11 = DateTimeUtils.CountRepeat(format, i, c);
														if (num6 != -1 || num11 > 2)
														{
															return false;
														}
														num10 += DateTimeOffset.ParseNumber(input, num10, 2, num11 == 2, allow_leading_white, out num6);
													}
												}
												else
												{
													num11 = 2;
													num10 += DateTimeOffset.ParseChar(input, num10, format[i + 1], allow_leading_white, out num9);
													if (num9 == -1)
													{
														return false;
													}
												}
											}
											else
											{
												num11 = DateTimeUtils.CountRepeat(format, i, c);
												if (num2 != -1 || num11 > 4)
												{
													return false;
												}
												if (num11 <= 2)
												{
													num10 += DateTimeOffset.ParseNumber(input, num10, 2, num11 == 2, allow_leading_white, out num2);
												}
												else
												{
													num10 += DateTimeOffset.ParseEnum(input, num10, (num11 != 3) ? dfi.MonthNames : dfi.AbbreviatedMonthNames, allow_leading_white, out num2);
													num2++;
												}
											}
										}
										else
										{
											num11 = 1;
											num10 += DateTimeOffset.ParseEnum(input, num10, new string[]
											{
												dfi.TimeSeparator
											}, false, out num9);
											if (num9 == -1)
											{
												return false;
											}
										}
									}
									else
									{
										num11 = 1;
										num10 += DateTimeOffset.ParseEnum(input, num10, new string[]
										{
											dfi.DateSeparator
										}, false, out num9);
										if (num9 == -1)
										{
											return false;
										}
									}
								}
								else
								{
									num11 = 1;
									if (i != 0)
									{
										return false;
									}
								}
							}
							else
							{
								num11 = 1;
								num10 += DateTimeOffset.ParseChar(input, num10, ' ', false, out num9);
								if (num9 == -1)
								{
									return false;
								}
							}
							break;
						case 'H':
							num11 = DateTimeUtils.CountRepeat(format, i, c);
							if (num5 != -1 || num11 > 2)
							{
								return false;
							}
							num10 += DateTimeOffset.ParseNumber(input, num10, 2, num11 == 2, allow_leading_white, out num5);
							break;
						}
						break;
					case 'f':
						num11 = DateTimeUtils.CountRepeat(format, i, c);
						num10 += DateTimeOffset.ParseNumber(input, num10, num11, true, allow_leading_white, out num9);
						if (num8 >= 0.0 || num11 > 7 || num9 == -1)
						{
							return false;
						}
						num8 = (double)num9 / Math.Pow(10.0, (double)num11);
						break;
					case 'h':
						num11 = DateTimeUtils.CountRepeat(format, i, c);
						if (num5 != -1 || num11 > 2)
						{
							return false;
						}
						num10 += DateTimeOffset.ParseNumber(input, num10, 2, num11 == 2, allow_leading_white, out num9);
						if (num9 == -1)
						{
							return false;
						}
						if (num4 == -1)
						{
							num4 = num9;
						}
						else
						{
							num5 = num4 + num9;
						}
						break;
					}
					break;
				case 'y':
					if (num != -1)
					{
						return false;
					}
					num11 = DateTimeUtils.CountRepeat(format, i, c);
					if (num11 <= 2)
					{
						num10 += DateTimeOffset.ParseNumber(input, num10, 2, num11 == 2, allow_leading_white, out num);
						if (num != -1)
						{
							num += DateTime.Now.Year - DateTime.Now.Year % 100;
						}
					}
					else if (num11 <= 4)
					{
						int num15;
						num10 += DateTimeOffset.ParseNumber(input, num10, 5, false, allow_leading_white, out num, out num15);
						if (num15 < num11 || (num15 > num11 && (double)num / Math.Pow(10.0, (double)(num15 - 1)) < 1.0))
						{
							return false;
						}
					}
					else
					{
						num10 += DateTimeOffset.ParseNumber(input, num10, num11, true, allow_leading_white, out num);
					}
					break;
				case 'z':
				{
					num11 = DateTimeUtils.CountRepeat(format, i, c);
					if (timeSpan != TimeSpan.MinValue || num11 > 3)
					{
						return false;
					}
					int num16 = 0;
					num9 = 0;
					int num17;
					num10 += DateTimeOffset.ParseEnum(input, num10, new string[]
					{
						"-",
						"+"
					}, allow_leading_white, out num17);
					int num18;
					num10 += DateTimeOffset.ParseNumber(input, num10, 2, num11 != 1, false, out num18);
					if (num11 == 3)
					{
						num10 += DateTimeOffset.ParseEnum(input, num10, new string[]
						{
							dfi.TimeSeparator
						}, false, out num9);
						num10 += DateTimeOffset.ParseNumber(input, num10, 2, true, false, out num16);
					}
					if (num18 == -1 || num16 == -1 || num17 == -1)
					{
						return false;
					}
					if (num17 == 0)
					{
						num17 = -1;
					}
					timeSpan = new TimeSpan(num17 * num18, num17 * num16, 0);
					break;
				}
				}
				i += num11;
			}
			if (timeSpan == TimeSpan.MinValue && (styles & DateTimeStyles.AssumeLocal) != DateTimeStyles.None)
			{
				timeSpan = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
			}
			if (timeSpan == TimeSpan.MinValue && (styles & DateTimeStyles.AssumeUniversal) != DateTimeStyles.None)
			{
				timeSpan = TimeSpan.Zero;
			}
			if (num5 < 0)
			{
				num5 = 0;
			}
			if (num6 < 0)
			{
				num6 = 0;
			}
			if (num7 < 0)
			{
				num7 = 0;
			}
			if (num8 < 0.0)
			{
				num8 = 0.0;
			}
			if (num > 0 && num2 > 0 && num3 > 0)
			{
				result = new DateTimeOffset(num, num2, num3, num5, num6, num7, 0, timeSpan);
				result = result.AddSeconds(num8);
				if ((styles & DateTimeStyles.AdjustToUniversal) != DateTimeStyles.None)
				{
					result = result.ToUniversalTime();
				}
				return true;
			}
			return false;
		}

		private static int ParseNumber(string input, int pos, int digits, bool leading_zero, bool allow_leading_white, out int result)
		{
			int num;
			return DateTimeOffset.ParseNumber(input, pos, digits, leading_zero, allow_leading_white, out result, out num);
		}

		private static int ParseNumber(string input, int pos, int digits, bool leading_zero, bool allow_leading_white, out int result, out int digit_parsed)
		{
			int num = 0;
			digit_parsed = 0;
			result = 0;
			while (allow_leading_white && pos < input.Length && input[pos] == ' ')
			{
				num++;
				pos++;
			}
			while (pos < input.Length && char.IsDigit(input[pos]) && digits > 0)
			{
				result = 10 * result + (int)((byte)(input[pos] - '0'));
				pos++;
				num++;
				digit_parsed++;
				digits--;
			}
			if (leading_zero && digits > 0)
			{
				result = -1;
			}
			if (digit_parsed == 0)
			{
				result = -1;
			}
			return num;
		}

		private static int ParseEnum(string input, int pos, string[] enums, bool allow_leading_white, out int result)
		{
			int num = 0;
			result = -1;
			while (allow_leading_white && pos < input.Length && input[pos] == ' ')
			{
				num++;
				pos++;
			}
			for (int i = 0; i < enums.Length; i++)
			{
				if (input.Substring(pos).StartsWith(enums[i]))
				{
					result = i;
					break;
				}
			}
			if (result >= 0)
			{
				num += enums[result].Length;
			}
			return num;
		}

		private static int ParseChar(string input, int pos, char c, bool allow_leading_white, out int result)
		{
			int num = 0;
			result = -1;
			while (allow_leading_white && pos < input.Length && input[pos] == ' ')
			{
				pos++;
				num++;
			}
			if (pos < input.Length && input[pos] == c)
			{
				result = (int)c;
				num++;
			}
			return num;
		}

		/// <summary>Subtracts a <see cref="T:System.DateTimeOffset" /> value that represents a specific date and time from the current <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>A <see cref="T:System.TimeSpan" /> object that specifies the interval between the two <see cref="T:System.DateTimeOffset" /> objects.</returns>
		/// <param name="value">A <see cref="T:System.DateTimeOffset" /> object.</param>
		public TimeSpan Subtract(DateTimeOffset value)
		{
			return this.UtcDateTime - value.UtcDateTime;
		}

		/// <summary>Subtracts a specified time interval from the current <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>A <see cref="T:System.DateTimeOffset" /> object that is equal to the date and time represented by the current <see cref="T:System.DateTimeOffset" /> object, minus the time interval represented by <paramref name="value" />.</returns>
		/// <param name="value">A <see cref="T:System.TimeSpan" /> object that represents a time interval.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTimeOffset" /> value is less than <see cref="F:System.DateTimeOffset.MinValue" />.-or- The resulting <see cref="T:System.DateTimeOffset" /> value is greater than <see cref="F:System.DateTimeOffset.MaxValue" />.</exception>
		public DateTimeOffset Subtract(TimeSpan value)
		{
			return this.Add(-value);
		}

		/// <summary>Converts the value of the current <see cref="T:System.DateTimeOffset" /> object to a Windows file time.</summary>
		/// <returns>The value of the current <see cref="T:System.DateTimeOffset" /> object, expressed as a Windows file time.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The resulting file time would represent a date and time before midnight on January 1, 1601 C.E. Coordinated Universal Time (UTC).</exception>
		public long ToFileTime()
		{
			return this.UtcDateTime.ToFileTime();
		}

		/// <summary>Converts the current <see cref="T:System.DateTimeOffset" /> object to a <see cref="T:System.DateTimeOffset" /> object that represents the local time.</summary>
		/// <returns>A <see cref="T:System.DateTimeOffset" /> object that represents the date and time of the current <see cref="T:System.DateTimeOffset" /> object converted to local time.</returns>
		public DateTimeOffset ToLocalTime()
		{
			return new DateTimeOffset(this.UtcDateTime.ToLocalTime(), TimeZone.CurrentTimeZone.GetUtcOffset(this.UtcDateTime.ToLocalTime()));
		}

		/// <summary>Converts the value of the current <see cref="T:System.DateTimeOffset" /> object to the date and time specified by an offset value.</summary>
		/// <returns>A <see cref="T:System.DateTimeOffset" /> object that is equal to the original <see cref="T:System.DateTimeOffset" /> object (that is, their <see cref="M:System.DateTimeOffset.ToUniversalTime" /> methods return identical points in time) but whose <see cref="P:System.DateTimeOffset.Offset" /> property is set to <paramref name="offset" />.</returns>
		/// <param name="offset">The offset to convert the <see cref="T:System.DateTimeOffset" /> value to.   </param>
		/// <exception cref="T:System.ArgumentException">The resulting <see cref="T:System.DateTimeOffset" /> object has a <see cref="P:System.DateTimeOffset.DateTime" /> value earlier than <see cref="F:System.DateTimeOffset.MinValue" />.-or-The resulting <see cref="T:System.DateTimeOffset" /> object has a <see cref="P:System.DateTimeOffset.DateTime" /> value later than <see cref="F:System.DateTimeOffset.MaxValue" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> is less than -14 hours.-or-<paramref name="offset" /> is greater than 14 hours.</exception>
		public DateTimeOffset ToOffset(TimeSpan offset)
		{
			return new DateTimeOffset(this.dt - this.utc_offset + offset, offset);
		}

		/// <summary>Converts the value of the current <see cref="T:System.DateTimeOffset" /> object to its equivalent string representation.</summary>
		/// <returns>A string representation of a <see cref="T:System.DateTimeOffset" /> object that includes the offset appended at the end of the string.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The date and time is outside the range of dates supported by the calendar used by the current culture. </exception>
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		/// <summary>Converts the value of the current <see cref="T:System.DateTimeOffset" /> object to its equivalent string representation using the specified culture-specific formatting information.</summary>
		/// <returns>A string representation of the value of the current <see cref="T:System.DateTimeOffset" /> object, as specified by <paramref name="formatProvider" />.</returns>
		/// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The date and time is outside the range of dates supported by the calendar used by <paramref name="formatProvider" />. </exception>
		public string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		/// <summary>Converts the value of the current <see cref="T:System.DateTimeOffset" /> object to its equivalent string representation using the specified format.</summary>
		/// <returns>A string representation of the value of the current <see cref="T:System.DateTimeOffset" /> object, as specified by <paramref name="format" />.</returns>
		/// <param name="format">A format string.</param>
		/// <exception cref="T:System.FormatException">The length of <paramref name="format" /> is one, and it is not one of the standard format specifier characters defined for <see cref="T:System.Globalization.DateTimeFormatInfo" />. -or-<paramref name="format" /> does not contain a valid custom format pattern.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The date and time is outside the range of dates supported by the calendar used by the current culture. </exception>
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		/// <summary>Converts the value of the current <see cref="T:System.DateTimeOffset" /> object to its equivalent string representation using the specified format and culture-specific format information.</summary>
		/// <returns>A string representation of the value of the current <see cref="T:System.DateTimeOffset" /> object, as specified by <paramref name="format" /> and <paramref name="provider" />.</returns>
		/// <param name="format">A format string.</param>
		/// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
		/// <exception cref="T:System.FormatException">The length of <paramref name="format" /> is one, and it is not one of the standard format specifier characters defined for <see cref="T:System.Globalization.DateTimeFormatInfo" />.-or-<paramref name="format" /> does not contain a valid custom format pattern. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The date and time is outside the range of dates supported by the calendar used by <paramref name="formatProvider" />. </exception>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			DateTimeFormatInfo instance = DateTimeFormatInfo.GetInstance(formatProvider);
			if (format == null || format == string.Empty)
			{
				format = instance.ShortDatePattern + " " + instance.LongTimePattern + " zzz";
			}
			bool flag = false;
			bool flag2 = false;
			if (format.Length == 1)
			{
				char format2 = format[0];
				try
				{
					format = DateTimeUtils.GetStandardPattern(format2, instance, out flag, out flag2, true);
				}
				catch
				{
					format = null;
				}
				if (format == null)
				{
					throw new FormatException("format is not one of the format specifier characters defined for DateTimeFormatInfo");
				}
			}
			return (!flag) ? DateTimeUtils.ToString(this.DateTime, new TimeSpan?(this.Offset), format, instance) : DateTimeUtils.ToString(this.UtcDateTime, new TimeSpan?(TimeSpan.Zero), format, instance);
		}

		/// <summary>Converts the current <see cref="T:System.DateTimeOffset" /> object to a <see cref="T:System.DateTimeOffset" /> value that represents the Coordinated Universal Time (UTC).</summary>
		/// <returns>A <see cref="T:System.DateTimeOffset" /> object that represents the date and time of the current <see cref="T:System.DateTimeOffset" /> object converted to Coordinated Universal Time (UTC).</returns>
		public DateTimeOffset ToUniversalTime()
		{
			return new DateTimeOffset(this.UtcDateTime, TimeSpan.Zero);
		}

		/// <summary>Tries to converts a specified string representation of a date and time to its <see cref="T:System.DateTimeOffset" /> equivalent, and returns a value that indicates whether the conversion succeeded.</summary>
		/// <returns>true if the <paramref name="input" /> parameter is successfully converted; otherwise, false.</returns>
		/// <param name="input">A string that contains a date and time to convert.</param>
		/// <param name="result">When the method returns, contains the <see cref="T:System.DateTimeOffset" /> equivalent to the date and time of <paramref name="input" />, if the conversion succeeded, or <see cref="F:System.DateTimeOffset.MinValue" />, if the conversion failed. The conversion fails if the <paramref name="input" /> parameter is null or does not contain a valid string representation of a date and time. This parameter is passed uninitialized.</param>
		public static bool TryParse(string input, out DateTimeOffset result)
		{
			bool result2;
			try
			{
				result = DateTimeOffset.Parse(input);
				result2 = true;
			}
			catch
			{
				result = DateTimeOffset.MinValue;
				result2 = false;
			}
			return result2;
		}

		/// <summary>Tries to convert a specified string representation of a date and time to its <see cref="T:System.DateTimeOffset" /> equivalent, and returns a value that indicates whether the conversion succeeded.</summary>
		/// <returns>true if the <paramref name="input" /> parameter is successfully converted; otherwise, false.</returns>
		/// <param name="input">A string that contains a date and time to convert.</param>
		/// <param name="formatProvider">An <see cref="T:System.IFormatProvider" /> object that provides culture-specific formatting information about <paramref name="input" />.</param>
		/// <param name="styles">A bitwise combination of <see cref="T:System.Globalization.DateTimeStyles" /> values that indicates the permitted format of <paramref name="input" />. </param>
		/// <param name="result">When the method returns, contains the <see cref="T:System.DateTimeOffset" /> value equivalent to the date and time of <paramref name="input" />, if the conversion succeeded, or <see cref="F:System.DateTimeOffset.MinValue" />, if the conversion failed. The conversion fails if the <paramref name="input" /> parameter is null or does not contain a valid string representation of a date and time. This parameter is passed uninitialized.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="styles" /> includes an undefined <see cref="T:System.Globalization.DateTimeStyles" /> value.-or-<see cref="F:System.Globalization.DateTimeStyles.NoCurrentDateDefault" />  is not supported.-or-<paramref name="styles" /> includes mutually exclusive <see cref="T:System.Globalization.DateTimeStyles" /> values.</exception>
		public static bool TryParse(string input, IFormatProvider formatProvider, DateTimeStyles styles, out DateTimeOffset result)
		{
			bool result2;
			try
			{
				result = DateTimeOffset.Parse(input, formatProvider, styles);
				result2 = true;
			}
			catch
			{
				result = DateTimeOffset.MinValue;
				result2 = false;
			}
			return result2;
		}

		/// <summary>Converts the specified string representation of a date and time to its <see cref="T:System.DateTimeOffset" /> equivalent using the specified format, culture-specific format information, and style. The format of the string representation must match the specified format exactly.</summary>
		/// <returns>true if the <paramref name="input" /> parameter is successfully converted; otherwise, false.</returns>
		/// <param name="input">A string that contains a date and time to convert.</param>
		/// <param name="format">A format specifier that defines the required format of <paramref name="input" />.</param>
		/// <param name="formatProvider">An <see cref="T:System.IFormatProvider" /> object that supplies culture-specific formatting information about <paramref name="input" />.</param>
		/// <param name="styles">A bitwise combination of <see cref="T:System.Globalization.DateTimeStyles" /> values that indicates the permitted format of input. A typical value to specify is None.</param>
		/// <param name="result">When the method returns, contains the <see cref="T:System.DateTimeOffset" /> equivalent to the date and time of <paramref name="input" />, if the conversion succeeded, or <see cref="F:System.DateTimeOffset.MinValue" />, if the conversion failed. The conversion fails if the <paramref name="input" /> parameter is null, or does not contain a valid string representation of a date and time in the expected format defined by <paramref name="format" /> and <paramref name="provider" />. This parameter is passed uninitialized.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="styles" /> includes an undefined <see cref="T:System.Globalization.DateTimeStyles" /> value.-or-<see cref="F:System.Globalization.DateTimeStyles.NoCurrentDateDefault" />  is not supported.-or-<paramref name="styles" /> includes mutually exclusive <see cref="T:System.Globalization.DateTimeStyles" /> values.</exception>
		public static bool TryParseExact(string input, string format, IFormatProvider formatProvider, DateTimeStyles styles, out DateTimeOffset result)
		{
			bool result2;
			try
			{
				result = DateTimeOffset.ParseExact(input, format, formatProvider, styles);
				result2 = true;
			}
			catch
			{
				result = DateTimeOffset.MinValue;
				result2 = false;
			}
			return result2;
		}

		/// <summary>Converts the specified string representation of a date and time to its <see cref="T:System.DateTimeOffset" /> equivalent using the specified array of formats, culture-specific format information, and style. The format of the string representation must match one of the specified formats exactly.</summary>
		/// <returns>true if the <paramref name="input" /> parameter is successfully converted; otherwise, false.</returns>
		/// <param name="input">A string that contains a date and time to convert.</param>
		/// <param name="formats">An array that defines the expected formats of <paramref name="input" />.</param>
		/// <param name="formatProvider">An <see cref="T:System.IFormatProvider" /> object that supplies culture-specific formatting information about <paramref name="input" />.</param>
		/// <param name="styles">A bitwise combination of <see cref="T:System.Globalization.DateTimeStyles" /> values that indicates the permitted format of input. A typical value to specify is None.</param>
		/// <param name="result">When the method returns, contains the <see cref="T:System.DateTimeOffset" /> equivalent to the date and time of <paramref name="input" />, if the conversion succeeded, or <see cref="F:System.DateTimeOffset.MinValue" />, if the conversion failed. The conversion fails if the <paramref name="input" /> does not contain a valid string representation of a date and time, or does not contain the date and time in the expected format defined by <paramref name="format" />, or if <paramref name="formats" /> is null. This parameter is passed uninitialized.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="styles" /> includes an undefined <see cref="T:System.Globalization.DateTimeStyles" /> value.-or-<see cref="F:System.Globalization.DateTimeStyles.NoCurrentDateDefault" />  is not supported.-or-<paramref name="styles" /> includes mutually exclusive <see cref="T:System.Globalization.DateTimeStyles" /> values.</exception>
		public static bool TryParseExact(string input, string[] formats, IFormatProvider formatProvider, DateTimeStyles styles, out DateTimeOffset result)
		{
			bool result2;
			try
			{
				result = DateTimeOffset.ParseExact(input, formats, formatProvider, styles);
				result2 = true;
			}
			catch
			{
				result = DateTimeOffset.MinValue;
				result2 = false;
			}
			return result2;
		}

		/// <summary>Gets a <see cref="T:System.DateTime" /> value that represents the date component of the current <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> value that represents the date component of the current <see cref="T:System.DateTimeOffset" /> object.</returns>
		public DateTime Date
		{
			get
			{
				return DateTime.SpecifyKind(this.dt.Date, DateTimeKind.Unspecified);
			}
		}

		/// <summary>Gets a <see cref="T:System.DateTime" /> value that represents the date and time of the current <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> value that represents the date and time of the current <see cref="T:System.DateTimeOffset" /> object.</returns>
		public DateTime DateTime
		{
			get
			{
				return DateTime.SpecifyKind(this.dt, DateTimeKind.Unspecified);
			}
		}

		/// <summary>Gets the day of the month represented by the current <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>The day component of the current <see cref="T:System.DateTimeOffset" /> object, expressed as a value between 1 and 31.</returns>
		public int Day
		{
			get
			{
				return this.dt.Day;
			}
		}

		/// <summary>Gets the day of the week represented by the current <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>One of the <see cref="T:System.DayOfWeek" /> values that indicates the day of the week of the current <see cref="T:System.DateTimeOffset" /> object.</returns>
		public DayOfWeek DayOfWeek
		{
			get
			{
				return this.dt.DayOfWeek;
			}
		}

		/// <summary>Gets the day of the year represented by the current <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>The day of the year of the current <see cref="T:System.DateTimeOffset" /> object, expressed as a value between 1 and 366.</returns>
		public int DayOfYear
		{
			get
			{
				return this.dt.DayOfYear;
			}
		}

		/// <summary>Gets the hour component of the time represented by the current <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>The hour component of the current <see cref="T:System.DateTimeOffset" /> object. This property uses a 24-hour clock; the value ranges from 0 to 23.</returns>
		public int Hour
		{
			get
			{
				return this.dt.Hour;
			}
		}

		/// <summary>Gets a <see cref="T:System.DateTime" /> value that represents the local date and time of the current <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> value that represents the local date and time of the current <see cref="T:System.DateTimeOffset" /> object.</returns>
		public DateTime LocalDateTime
		{
			get
			{
				return this.UtcDateTime.ToLocalTime();
			}
		}

		/// <summary>Gets the millisecond component of the time represented by the current <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>The millisecond component of the current <see cref="T:System.DateTimeOffset" /> object, expressed as an integer between 0 and 999.</returns>
		public int Millisecond
		{
			get
			{
				return this.dt.Millisecond;
			}
		}

		/// <summary>Gets the minute component of the time represented by the current <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>The minute component of the current <see cref="T:System.DateTimeOffset" /> object, expressed as an integer between 0 and 59.</returns>
		public int Minute
		{
			get
			{
				return this.dt.Minute;
			}
		}

		/// <summary>Gets the month component of the date represented by the current <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>The month component of the current <see cref="T:System.DateTimeOffset" /> object, expressed as an integer between 1 and 12.</returns>
		public int Month
		{
			get
			{
				return this.dt.Month;
			}
		}

		/// <summary>Gets a <see cref="T:System.DateTimeOffset" /> object that is set to the current date and time on the current computer, with the offset set to the local time's offset from Coordinated Universal Time (UTC).</summary>
		/// <returns>A <see cref="T:System.DateTimeOffset" /> object whose date and time is the current local time and whose offset is the local time zone's offset from Coordinated Universal Time (UTC).</returns>
		public static DateTimeOffset Now
		{
			get
			{
				return new DateTimeOffset(DateTime.Now);
			}
		}

		/// <summary>Gets the time's offset from Coordinated Universal Time (UTC). </summary>
		/// <returns>A <see cref="T:System.TimeSpan" /> object that represents the difference between the current <see cref="T:System.DateTimeOffset" /> object's time value and Coordinated Universal Time (UTC).</returns>
		public TimeSpan Offset
		{
			get
			{
				return this.utc_offset;
			}
		}

		/// <summary>Gets the second component of the clock time represented by the current <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>The second component of the <see cref="T:System.DateTimeOffset" /> object, expressed as an integer value between 0 and 59.</returns>
		public int Second
		{
			get
			{
				return this.dt.Second;
			}
		}

		/// <summary>Gets the number of ticks that represents the date and time of the current <see cref="T:System.DateTimeOffset" /> object in clock time.</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that represents the number of ticks in the <see cref="T:System.DateTimeOffset" /> object's clock time.</returns>
		public long Ticks
		{
			get
			{
				return this.dt.Ticks;
			}
		}

		/// <summary>Gets the time of day for the current <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>A <see cref="T:System.TimeSpan" /> object that represents the time interval of the current date that has elapsed since midnight.</returns>
		public TimeSpan TimeOfDay
		{
			get
			{
				return this.dt.TimeOfDay;
			}
		}

		/// <summary>Gets a <see cref="T:System.DateTime" /> value that represents the Coordinated Universal Time (UTC) date and time of the current <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> value that represents the Coordinated Universal Time (UTC) date and time of the current <see cref="T:System.DateTimeOffset" /> object.</returns>
		public DateTime UtcDateTime
		{
			get
			{
				return DateTime.SpecifyKind(this.dt - this.utc_offset, DateTimeKind.Utc);
			}
		}

		/// <summary>Gets a <see cref="T:System.DateTimeOffset" /> object whose date and time are set to the current Coordinated Universal Time (UTC) date and time and whose offset is <see cref="F:System.TimeSpan.Zero" />.</summary>
		/// <returns>A <see cref="T:System.DateTimeOffset" /> value whose date and time is the current Coordinated Universal Time (UTC) and whose offset is <see cref="F:System.TimeSpan.Zero" />.</returns>
		public static DateTimeOffset UtcNow
		{
			get
			{
				return new DateTimeOffset(DateTime.UtcNow);
			}
		}

		/// <summary>Gets the number of ticks that represents the date and time of the current <see cref="T:System.DateTimeOffset" /> object in Coordinated Universal Time (UTC).</summary>
		/// <returns>An <see cref="T:System.Int64" /> value that represents the number of ticks in the <see cref="T:System.DateTimeOffset" /> object's Coordinated Universal Time (UTC).</returns>
		public long UtcTicks
		{
			get
			{
				return this.UtcDateTime.Ticks;
			}
		}

		/// <summary>Gets the year component of the date represented by the current <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>The year component of the current <see cref="T:System.DateTimeOffset" /> object, expressed as an integer value between 0 and 9999.</returns>
		public int Year
		{
			get
			{
				return this.dt.Year;
			}
		}

		/// <summary>Adds a specified time interval to a <see cref="T:System.DateTimeOffset" /> object that has a specified date and time, and yields a <see cref="T:System.DateTimeOffset" /> object that has new a date and time.</summary>
		/// <returns>A <see cref="T:System.DateTimeOffset" /> object whose value is the sum of the values of <paramref name="dateTimeTz" /> and <paramref name="timeSpan" />.</returns>
		/// <param name="dateTimeTz">A <see cref="T:System.DateTimeOffset" /> object.</param>
		/// <param name="timeSpan">A <see cref="T:System.TimeSpan" /> object that specifies the time interval to add to the <see cref="T:System.DateTimeOffset" /> object.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTimeOffset" /> value is less than <see cref="F:System.DateTimeOffset.MinValue" />.-or- The resulting <see cref="T:System.DateTimeOffset" /> value is greater than <see cref="F:System.DateTimeOffset.MaxValue" />.</exception>
		public static DateTimeOffset operator +(DateTimeOffset dateTimeTz, TimeSpan timeSpan)
		{
			return dateTimeTz.Add(timeSpan);
		}

		/// <summary>Determines whether two specified <see cref="T:System.DateTimeOffset" /> objects represent the same point in time.</summary>
		/// <returns>true if both <see cref="T:System.DateTimeOffset" /> objects have the same <see cref="P:System.DateTimeOffset.UtcDateTime" /> value; otherwise, false.</returns>
		/// <param name="left">The first <see cref="T:System.DateTimeOffset" /> object.</param>
		/// <param name="right">The second <see cref="T:System.DateTimeOffset" /> object.</param>
		public static bool operator ==(DateTimeOffset left, DateTimeOffset right)
		{
			return left.Equals(right);
		}

		/// <summary>Determines whether one specified <see cref="T:System.DateTimeOffset" /> object is greater than (or later than) a second specified <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>true if the <see cref="P:System.DateTimeOffset.UtcDateTime" /> value of <paramref name="left" /> is later than the <see cref="P:System.DateTimeOffset.UtcDateTime" /> value of <paramref name="right" />; otherwise, false.</returns>
		/// <param name="left">The first <see cref="T:System.DateTimeOffset" /> object.</param>
		/// <param name="right">The second <see cref="T:System.DateTimeOffset" /> object.</param>
		public static bool operator >(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime > right.UtcDateTime;
		}

		/// <summary>Determines whether one specified <see cref="T:System.DateTimeOffset" /> object is greater than or equal to a second specified <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>true if the <see cref="P:System.DateTimeOffset.UtcDateTime" /> value of <paramref name="left" /> is the same as or later than the <see cref="P:System.DateTimeOffset.UtcDateTime" /> value of <paramref name="right" />; otherwise, false.</returns>
		/// <param name="left">The first <see cref="T:System.DateTimeOffset" /> object.</param>
		/// <param name="right">The second <see cref="T:System.DateTimeOffset" /> object.</param>
		public static bool operator >=(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime >= right.UtcDateTime;
		}

		/// <summary>Defines an implicit conversion of a <see cref="T:System.DateTime" /> object to a <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>A <see cref="T:System.DateTimeOffset" /> object.</returns>
		/// <param name="dateTime">A <see cref="T:System.DateTime" /> object. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The Coordinated Universal Time (UTC) date and time that results from applying the offset is earlier than <see cref="F:System.DateTimeOffset.MinValue" />.-or-The UTC date and time that results from applying the offset is later than <see cref="F:System.DateTimeOffset.MaxValue" />.</exception>
		public static implicit operator DateTimeOffset(DateTime dateTime)
		{
			return new DateTimeOffset(dateTime);
		}

		/// <summary>Determines whether two specified <see cref="T:System.DateTimeOffset" /> objects refer to different points in time.</summary>
		/// <returns>true if <paramref name="left" /> and <paramref name="right" /> do not have the same <see cref="P:System.DateTimeOffset.UtcDateTime" /> value; otherwise, false.</returns>
		/// <param name="left">The first <see cref="T:System.DateTimeOffset" /> object.</param>
		/// <param name="right">The second <see cref="T:System.DateTimeOffset" /> object.</param>
		public static bool operator !=(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime != right.UtcDateTime;
		}

		/// <summary>Determines whether one specified <see cref="T:System.DateTimeOffset" /> object is less than a second specified <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>true if the <see cref="P:System.DateTimeOffset.UtcDateTime" /> value of <paramref name="left" /> is earlier than the <see cref="P:System.DateTimeOffset.UtcDateTime" /> value of <paramref name="right" />; otherwise, false.</returns>
		/// <param name="left">The first <see cref="T:System.DateTimeOffset" /> object.</param>
		/// <param name="right">The second <see cref="T:System.DateTimeOffset" /> object.</param>
		public static bool operator <(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime < right.UtcDateTime;
		}

		/// <summary>Determines whether one specified <see cref="T:System.DateTimeOffset" /> object is less than a second specified <see cref="T:System.DateTimeOffset" /> object.</summary>
		/// <returns>true if the <see cref="P:System.DateTimeOffset.UtcDateTime" /> value of <paramref name="left" /> is earlier than the <see cref="P:System.DateTimeOffset.UtcDateTime" /> value of <paramref name="right" />; otherwise, false.</returns>
		/// <param name="left">The first <see cref="T:System.DateTimeOffset" /> object.</param>
		/// <param name="right">The second <see cref="T:System.DateTimeOffset" /> object.</param>
		public static bool operator <=(DateTimeOffset left, DateTimeOffset right)
		{
			return left.UtcDateTime <= right.UtcDateTime;
		}

		/// <summary>Subtracts one <see cref="T:System.DateTimeOffset" /> object from another and yields a time interval.</summary>
		/// <returns>A <see cref="T:System.TimeSpan" /> object that represents the difference between <paramref name="left" /> and <paramref name="right" />.</returns>
		/// <param name="left">A <see cref="T:System.DateTimeOffset" /> object (the minuend).   </param>
		/// <param name="right">A second <see cref="T:System.DateTimeOffset" /> object (the subtrahend).</param>
		public static TimeSpan operator -(DateTimeOffset left, DateTimeOffset right)
		{
			return left.Subtract(right);
		}

		/// <summary>Subtracts a specified time interval from a specified date and time, and yields a new date and time.</summary>
		/// <returns>A <see cref="T:System.DateTimeOffset" /> object equal to the value of <paramref name="dateTimeTz" /> minus <paramref name="timeSpan" />.</returns>
		/// <param name="dateTimeTz">A <see cref="T:System.DateTimeOffset" /> object that represents a particular date and time.</param>
		/// <param name="timeSpan">A time interval.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The resulting <see cref="T:System.DateTimeOffset" /> value is less than <see cref="F:System.DateTimeOffset.MinValue" /> or greater than <see cref="F:System.DateTimeOffset.MaxValue" />.</exception>
		public static DateTimeOffset operator -(DateTimeOffset dateTimeTz, TimeSpan timeSpan)
		{
			return dateTimeTz.Subtract(timeSpan);
		}
	}
}
