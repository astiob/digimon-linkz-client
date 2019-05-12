using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace System
{
	/// <summary>Represents any time zone in the world.</summary>
	[Serializable]
	public sealed class TimeZoneInfo : ISerializable, IDeserializationCallback, IEquatable<TimeZoneInfo>
	{
		private const int BUFFER_SIZE = 16384;

		private TimeSpan baseUtcOffset;

		private string daylightDisplayName;

		private string displayName;

		private string id;

		private static TimeZoneInfo local;

		private string standardDisplayName;

		private bool disableDaylightSavingTime;

		private static TimeZoneInfo utc;

		private static string timeZoneDirectory;

		private TimeZoneInfo.AdjustmentRule[] adjustmentRules;

		private static List<TimeZoneInfo> systemTimeZones;

		private TimeZoneInfo(string id, TimeSpan baseUtcOffset, string displayName, string standardDisplayName, string daylightDisplayName, TimeZoneInfo.AdjustmentRule[] adjustmentRules, bool disableDaylightSavingTime)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			if (id == string.Empty)
			{
				throw new ArgumentException("id parameter is an empty string");
			}
			if (baseUtcOffset.Ticks % 600000000L != 0L)
			{
				throw new ArgumentException("baseUtcOffset parameter does not represent a whole number of minutes");
			}
			if (baseUtcOffset > new TimeSpan(14, 0, 0) || baseUtcOffset < new TimeSpan(-14, 0, 0))
			{
				throw new ArgumentOutOfRangeException("baseUtcOffset parameter is greater than 14 hours or less than -14 hours");
			}
			if (adjustmentRules != null && adjustmentRules.Length != 0)
			{
				TimeZoneInfo.AdjustmentRule adjustmentRule = null;
				foreach (TimeZoneInfo.AdjustmentRule adjustmentRule2 in adjustmentRules)
				{
					if (adjustmentRule2 == null)
					{
						throw new InvalidTimeZoneException("one or more elements in adjustmentRules are null");
					}
					if (baseUtcOffset + adjustmentRule2.DaylightDelta < new TimeSpan(-14, 0, 0) || baseUtcOffset + adjustmentRule2.DaylightDelta > new TimeSpan(14, 0, 0))
					{
						throw new InvalidTimeZoneException("Sum of baseUtcOffset and DaylightDelta of one or more object in adjustmentRules array is greater than 14 or less than -14 hours;");
					}
					if (adjustmentRule != null && adjustmentRule.DateStart > adjustmentRule2.DateStart)
					{
						throw new InvalidTimeZoneException("adjustment rules specified in adjustmentRules parameter are not in chronological order");
					}
					if (adjustmentRule != null && adjustmentRule.DateEnd > adjustmentRule2.DateStart)
					{
						throw new InvalidTimeZoneException("some adjustment rules in the adjustmentRules parameter overlap");
					}
					if (adjustmentRule != null && adjustmentRule.DateEnd == adjustmentRule2.DateStart)
					{
						throw new InvalidTimeZoneException("a date can have multiple adjustment rules applied to it");
					}
					adjustmentRule = adjustmentRule2;
				}
			}
			this.id = id;
			this.baseUtcOffset = baseUtcOffset;
			this.displayName = (displayName ?? id);
			this.standardDisplayName = (standardDisplayName ?? id);
			this.daylightDisplayName = daylightDisplayName;
			this.disableDaylightSavingTime = disableDaylightSavingTime;
			this.adjustmentRules = adjustmentRules;
		}

		/// <summary>Gets the time difference between the current time zone's standard time and Coordinated Universal Time (UTC).</summary>
		/// <returns>A <see cref="T:System.TimeSpan" /> object that indicates the time difference between the current time zone's standard time and Coordinated Universal Time (UTC).</returns>
		public TimeSpan BaseUtcOffset
		{
			get
			{
				return this.baseUtcOffset;
			}
		}

		/// <summary>Gets the localized display name for the current time zone's daylight saving time.</summary>
		/// <returns>The display name for the time zone's localized daylight saving time.</returns>
		public string DaylightName
		{
			get
			{
				if (this.disableDaylightSavingTime)
				{
					return string.Empty;
				}
				return this.daylightDisplayName;
			}
		}

		/// <summary>Gets the localized general display name that represents the time zone.</summary>
		/// <returns>The time zone's localized general display name.</returns>
		public string DisplayName
		{
			get
			{
				return this.displayName;
			}
		}

		/// <summary>Gets the time zone identifier.</summary>
		/// <returns>The time zone identifier.</returns>
		public string Id
		{
			get
			{
				return this.id;
			}
		}

		/// <summary>Gets a <see cref="T:System.TimeZoneInfo" /> object that represents the local time zone.</summary>
		/// <returns>A <see cref="T:System.TimeZoneInfo" /> object that represents the local time zone.</returns>
		public static TimeZoneInfo Local
		{
			get
			{
				if (TimeZoneInfo.local == null)
				{
					try
					{
						TimeZoneInfo.local = TimeZoneInfo.FindSystemTimeZoneByFileName("Local", "/etc/localtime");
					}
					catch
					{
						try
						{
							TimeZoneInfo.local = TimeZoneInfo.FindSystemTimeZoneByFileName("Local", Path.Combine(TimeZoneInfo.TimeZoneDirectory, "localtime"));
						}
						catch
						{
							throw new TimeZoneNotFoundException();
						}
					}
				}
				return TimeZoneInfo.local;
			}
		}

		/// <summary>Gets the localized display name for the time zone's standard time.</summary>
		/// <returns>The localized display name of the time zone's standard time.</returns>
		public string StandardName
		{
			get
			{
				return this.standardDisplayName;
			}
		}

		/// <summary>Gets a value indicating whether the time zone has any daylight saving time rules.</summary>
		/// <returns>true if the time zone supports daylight saving time; otherwise, false.</returns>
		public bool SupportsDaylightSavingTime
		{
			get
			{
				return !this.disableDaylightSavingTime;
			}
		}

		/// <summary>Gets a <see cref="T:System.TimeZoneInfo" /> object that represents the Coordinated Universal Time (UTC) zone.</summary>
		/// <returns>A <see cref="T:System.TimeZoneInfo" /> object that represents the Coordinated Universal Time (UTC) zone.</returns>
		public static TimeZoneInfo Utc
		{
			get
			{
				if (TimeZoneInfo.utc == null)
				{
					TimeZoneInfo.utc = TimeZoneInfo.CreateCustomTimeZone("UTC", new TimeSpan(0L), "UTC", "UTC");
				}
				return TimeZoneInfo.utc;
			}
		}

		private static string TimeZoneDirectory
		{
			get
			{
				if (TimeZoneInfo.timeZoneDirectory == null)
				{
					TimeZoneInfo.timeZoneDirectory = "/usr/share/zoneinfo";
				}
				return TimeZoneInfo.timeZoneDirectory;
			}
			set
			{
				TimeZoneInfo.ClearCachedData();
				TimeZoneInfo.timeZoneDirectory = value;
			}
		}

		/// <summary>Clears cached time zone data.</summary>
		/// <filterpriority>2</filterpriority>
		public static void ClearCachedData()
		{
			TimeZoneInfo.local = null;
			TimeZoneInfo.utc = null;
			TimeZoneInfo.systemTimeZones = null;
		}

		/// <summary>Converts a time to the time in a particular time zone.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> value that represents the date and time in the destination time zone.</returns>
		/// <param name="dateTime">The date and time to convert.   </param>
		/// <param name="destinationTimeZone">The time zone to convert <paramref name="dateTime" /> to.</param>
		/// <exception cref="T:System.ArgumentException">The value of the <paramref name="dateTime" /> parameter represents an invalid time.</exception>
		/// <exception cref="T:System.ArgumentNullException">The value of the <paramref name="destinationTimeZone" /> parameter is null.</exception>
		public static DateTime ConvertTime(DateTime dateTime, TimeZoneInfo destinationTimeZone)
		{
			return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.Local, destinationTimeZone);
		}

		/// <summary>Converts a time from one time zone to another.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> value that represents the date and time in the destination time zone that corresponds to the <paramref name="dateTime" /> parameter in the source time zone.</returns>
		/// <param name="dateTime">The date and time to convert.</param>
		/// <param name="sourceTimeZone">The time zone of <paramref name="dateTime" />.</param>
		/// <param name="destinationTimeZone">The time zone to convert <paramref name="dateTime" /> to.</param>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.DateTime.Kind" /> property of the <paramref name="dateTime" /> parameter is <see cref="F:System.DateTimeKind.Local" />, but the <paramref name="sourceTimeZone" /> parameter does not equal <see cref="F:System.DateTimeKind.Local" />.-or-The <see cref="P:System.DateTime.Kind" /> property of the <paramref name="dateTime" /> parameter is <see cref="F:System.DateTimeKind.Utc" />, but the <paramref name="sourceTimeZone" /> parameter does not equal <see cref="P:System.TimeZoneInfo.Utc" />.-or-The <paramref name="dateTime" /> parameter is an invalid time (that is, it represents a time that does not exist because of a time zone's adjustment rules).</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="sourceTimeZone" /> parameter is null.-or-The <paramref name="destinationTimeZone" /> parameter is null.</exception>
		public static DateTime ConvertTime(DateTime dateTime, TimeZoneInfo sourceTimeZone, TimeZoneInfo destinationTimeZone)
		{
			if (dateTime.Kind == DateTimeKind.Local && sourceTimeZone != TimeZoneInfo.Local)
			{
				throw new ArgumentException("Kind propery of dateTime is Local but the sourceTimeZone does not equal TimeZoneInfo.Local");
			}
			if (dateTime.Kind == DateTimeKind.Utc && sourceTimeZone != TimeZoneInfo.Utc)
			{
				throw new ArgumentException("Kind propery of dateTime is Utc but the sourceTimeZone does not equal TimeZoneInfo.Utc");
			}
			if (sourceTimeZone.IsInvalidTime(dateTime))
			{
				throw new ArgumentException("dateTime parameter is an invalid time");
			}
			if (sourceTimeZone == null)
			{
				throw new ArgumentNullException("sourceTimeZone");
			}
			if (destinationTimeZone == null)
			{
				throw new ArgumentNullException("destinationTimeZone");
			}
			if (dateTime.Kind == DateTimeKind.Local && sourceTimeZone == TimeZoneInfo.Local && destinationTimeZone == TimeZoneInfo.Local)
			{
				return dateTime;
			}
			DateTime dateTime2 = TimeZoneInfo.ConvertTimeToUtc(dateTime);
			if (destinationTimeZone == TimeZoneInfo.Utc)
			{
				return dateTime2;
			}
			return TimeZoneInfo.ConvertTimeFromUtc(dateTime2, destinationTimeZone);
		}

		/// <summary>Converts a time to the time in a particular time zone.</summary>
		/// <returns>A <see cref="T:System.DateTimeOffset" /> value that represents the date and time in the destination time zone.</returns>
		/// <param name="dateTimeOffset">The date and time to convert.   </param>
		/// <param name="destinationTimeZone">The time zone to convert <paramref name="dateTime" /> to.</param>
		/// <exception cref="T:System.ArgumentNullException">The value of the <paramref name="destinationTimeZone" /> parameter is null.</exception>
		public static DateTimeOffset ConvertTime(DateTimeOffset dateTimeOffset, TimeZoneInfo destinationTimeZone)
		{
			throw new NotImplementedException();
		}

		/// <summary>Converts a time to the time in another time zone based on the time zone's identifier.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> value that represents the date and time in the destination time zone.</returns>
		/// <param name="dateTime">The date and time to convert.</param>
		/// <param name="destinationTimeZoneId">The identifier of the destination time zone.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="destinationTimeZoneId" /> is null.</exception>
		/// <exception cref="T:System.InvalidTimeZoneException">The time zone identifier was found, but the registry data is corrupted.</exception>
		/// <exception cref="T:System.Security.SecurityException">The process does not have the permissions required to read from the registry key that contains the time zone information.</exception>
		/// <exception cref="T:System.TimeZoneNotFoundException">The <paramref name="destinationTimeZoneId" /> identifier was not found on the local system.</exception>
		public static DateTime ConvertTimeBySystemTimeZoneId(DateTime dateTime, string destinationTimeZoneId)
		{
			return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.FindSystemTimeZoneById(destinationTimeZoneId));
		}

		/// <summary>Converts a time from one time zone to another based on time zone identifiers.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> value that represents the date and time in the destination time zone that corresponds to the <paramref name="dateTime" /> parameter in the source time zone.</returns>
		/// <param name="dateTime">The date and time to convert.</param>
		/// <param name="sourceTimeZoneId">The identifier of the source time zone. </param>
		/// <param name="destinationTimeZoneId">The identifier of the destination time zone.</param>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.DateTime.Kind" /> property of the <paramref name="dateTime" /> parameter does not correspond to the source time zone.-or-<paramref name="dateTime" /> is an invalid time in the source time zone.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="sourceTimeZoneId" /> is null.-or-<paramref name="destinationTimeZoneId" /> is null.</exception>
		/// <exception cref="T:System.InvalidTimeZoneException">The time zone identifier was found, but the registry data is corrupted.</exception>
		/// <exception cref="T:System.Security.SecurityException">The process does not have the permissions required to read from the registry key that contains the time zone information.</exception>
		/// <exception cref="T:System.TimeZoneNotFoundException">The <paramref name="sourceTimeZoneId" /> identifier was not found on the local system.-or-The <paramref name="destinationTimeZoneId" /> identifier was not found on the local system.</exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have the permissions required to read from the registry keys that hold time zone data.</exception>
		public static DateTime ConvertTimeBySystemTimeZoneId(DateTime dateTime, string sourceTimeZoneId, string destinationTimeZoneId)
		{
			return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.FindSystemTimeZoneById(sourceTimeZoneId), TimeZoneInfo.FindSystemTimeZoneById(destinationTimeZoneId));
		}

		/// <summary>Converts a time to the time in another time zone based on the time zone's identifier.</summary>
		/// <returns>A <see cref="T:System.DateTimeOffset" /> value that represents the date and time in the destination time zone.</returns>
		/// <param name="dateTimeOffset">The date and time to convert.</param>
		/// <param name="destinationTimeZoneId">The identifier of the destination time zone.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="destinationTimeZoneId" /> is null.</exception>
		/// <exception cref="T:System.InvalidTimeZoneException">The time zone identifier was found but the registry data is corrupted.</exception>
		/// <exception cref="T:System.Security.SecurityException">The process does not have the permissions required to read from the registry key that contains the time zone information.</exception>
		/// <exception cref="T:System.TimeZoneNotFoundException">The <paramref name="destinationTimeZoneId" /> identifier was not found on the local system.</exception>
		public static DateTimeOffset ConvertTimeBySystemTimeZoneId(DateTimeOffset dateTimeOffset, string destinationTimeZoneId)
		{
			return TimeZoneInfo.ConvertTime(dateTimeOffset, TimeZoneInfo.FindSystemTimeZoneById(destinationTimeZoneId));
		}

		private DateTime ConvertTimeFromUtc(DateTime dateTime)
		{
			if (dateTime.Kind == DateTimeKind.Local)
			{
				throw new ArgumentException("Kind property of dateTime is Local");
			}
			if (this == TimeZoneInfo.Utc)
			{
				return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
			}
			if (this == TimeZoneInfo.Local)
			{
				return DateTime.SpecifyKind(dateTime.ToLocalTime(), DateTimeKind.Unspecified);
			}
			TimeZoneInfo.AdjustmentRule applicableRule = this.GetApplicableRule(dateTime);
			if (this.IsDaylightSavingTime(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)))
			{
				return DateTime.SpecifyKind(dateTime + this.BaseUtcOffset + applicableRule.DaylightDelta, DateTimeKind.Unspecified);
			}
			return DateTime.SpecifyKind(dateTime + this.BaseUtcOffset, DateTimeKind.Unspecified);
		}

		/// <summary>Converts a Coordinated Universal Time (UTC) to the time in a specified time zone.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> value that represents the date and time in the destination time zone. Its <see cref="P:System.DateTime.Kind" /> property is <see cref="F:System.DateTimeKind.Utc" /> if <paramref name="destinationTimeZone" /> is <see cref="P:System.TimeZoneInfo.Utc" />; otherwise, its <see cref="P:System.DateTime.Kind" /> property is <see cref="F:System.DateTimeKind.Unspecified" />.</returns>
		/// <param name="dateTime">The Coordinated Universal Time (UTC).</param>
		/// <param name="destinationTimeZone">The time zone to convert <paramref name="dateTime" /> to.</param>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.DateTime.Kind" /> property of <paramref name="dateTime" /> is <see cref="F:System.DateTimeKind.Local" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="destinationTimeZone" /> is null.</exception>
		public static DateTime ConvertTimeFromUtc(DateTime dateTime, TimeZoneInfo destinationTimeZone)
		{
			if (destinationTimeZone == null)
			{
				throw new ArgumentNullException("destinationTimeZone");
			}
			return destinationTimeZone.ConvertTimeFromUtc(dateTime);
		}

		/// <summary>Converts the current date and time to Coordinated Universal Time (UTC).</summary>
		/// <returns>A <see cref="T:System.DateTime" /> value that represents the Coordinated Universal Time (UTC) that corresponds to the <paramref name="dateTime" /> parameter. The <see cref="T:System.DateTime" /> value's <see cref="P:System.DateTime.Kind" /> property is always set to <see cref="F:System.DateTimeKind.Utc" />.</returns>
		/// <param name="dateTime">The date and time to convert.</param>
		/// <exception cref="T:System.ArgumentException">TimeZoneInfo.Local.IsInvalidDateTime(<paramref name="dateTime" />) returns true.</exception>
		public static DateTime ConvertTimeToUtc(DateTime dateTime)
		{
			if (dateTime.Kind == DateTimeKind.Utc)
			{
				return dateTime;
			}
			return DateTime.SpecifyKind(dateTime.ToUniversalTime(), DateTimeKind.Utc);
		}

		/// <summary>Converts the time in a specified time zone to Coordinated Universal Time (UTC).</summary>
		/// <returns>A <see cref="T:System.DateTime" /> object that represents the Coordinated Universal Time (UTC) that corresponds to the <paramref name="dateTime" /> parameter. The <see cref="T:System.DateTime" /> object's <see cref="P:System.DateTime.Kind" /> property is always set to <see cref="F:System.DateTimeKind.Utc" />.</returns>
		/// <param name="dateTime">The date and time to convert.</param>
		/// <param name="sourceTimeZone">The time zone of <paramref name="dateTime" />.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="dateTime" />.Kind is <see cref="F:System.DateTimeKind.Utc" /> and <paramref name="sourceTimeZone" /> does not equal <see cref="P:System.TimeZoneInfo.Utc" />.-or-<paramref name="dateTime" />.Kind is <see cref="F:System.DateTimeKind.Local" /> and <paramref name="sourceTimeZone" /> does not equal <see cref="P:System.TimeZoneInfo.Local" />.-or-<paramref name="sourceTimeZone" />.IsInvalidDateTime(<paramref name="dateTime" />) returns true.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="sourceTimeZone" /> is null.</exception>
		public static DateTime ConvertTimeToUtc(DateTime dateTime, TimeZoneInfo sourceTimeZone)
		{
			if (sourceTimeZone == null)
			{
				throw new ArgumentNullException("sourceTimeZone");
			}
			if (dateTime.Kind == DateTimeKind.Utc && sourceTimeZone != TimeZoneInfo.Utc)
			{
				throw new ArgumentException("Kind propery of dateTime is Utc but the sourceTimeZone does not equal TimeZoneInfo.Utc");
			}
			if (dateTime.Kind == DateTimeKind.Local && sourceTimeZone != TimeZoneInfo.Local)
			{
				throw new ArgumentException("Kind propery of dateTime is Local but the sourceTimeZone does not equal TimeZoneInfo.Local");
			}
			if (sourceTimeZone.IsInvalidTime(dateTime))
			{
				throw new ArgumentException("dateTime parameter is an invalid time");
			}
			if (dateTime.Kind == DateTimeKind.Utc && sourceTimeZone == TimeZoneInfo.Utc)
			{
				return dateTime;
			}
			if (dateTime.Kind == DateTimeKind.Utc)
			{
				return dateTime;
			}
			if (dateTime.Kind == DateTimeKind.Local)
			{
				return TimeZoneInfo.ConvertTimeToUtc(dateTime);
			}
			if (sourceTimeZone.IsAmbiguousTime(dateTime) || !sourceTimeZone.IsDaylightSavingTime(dateTime))
			{
				return DateTime.SpecifyKind(dateTime - sourceTimeZone.BaseUtcOffset, DateTimeKind.Utc);
			}
			TimeZoneInfo.AdjustmentRule applicableRule = sourceTimeZone.GetApplicableRule(dateTime);
			return DateTime.SpecifyKind(dateTime - sourceTimeZone.BaseUtcOffset - applicableRule.DaylightDelta, DateTimeKind.Utc);
		}

		/// <summary>Creates a custom time zone with a specified identifier, an offset from Coordinated Universal Time (UTC), a display name, and a standard time display name.</summary>
		/// <returns>A <see cref="T:System.TimeZoneInfo" /> object that represents the new time zone.</returns>
		/// <param name="id">The time zone's identifier.</param>
		/// <param name="baseUtcOffset">A <see cref="T:System.TimeSpan" /> object that represents the time difference between this time zone and Coordinated Universal Time (UTC).</param>
		/// <param name="displayName">The display name of the new time zone.   </param>
		/// <param name="standardDisplayName">The name of the new time zone's standard time.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="id" /> parameter is null.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="id" /> parameter is an empty string ("").-or-The <paramref name="baseUtcOffset" /> parameter does not represent a whole number of minutes.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="baseUtcOffset" /> parameter is greater than 14 hours or less than -14 hours.</exception>
		/// <filterpriority>2</filterpriority>
		public static TimeZoneInfo CreateCustomTimeZone(string id, TimeSpan baseUtcOffset, string displayName, string standardDisplayName)
		{
			return TimeZoneInfo.CreateCustomTimeZone(id, baseUtcOffset, displayName, standardDisplayName, null, null, true);
		}

		/// <summary>Creates a custom time zone with a specified identifier, an offset from Coordinated Universal Time (UTC), a display name, a standard time name, a daylight saving time name, and daylight saving time rules.</summary>
		/// <returns>A <see cref="T:System.TimeZoneInfo" /> object that represents the new time zone.</returns>
		/// <param name="id">The time zone's identifier.</param>
		/// <param name="baseUtcOffset">A <see cref="T:System.TimeSpan" /> object that represents the time difference between this time zone and Coordinated Universal Time (UTC).</param>
		/// <param name="displayName">The display name of the new time zone.   </param>
		/// <param name="standardDisplayName">The new time zone's standard time name.</param>
		/// <param name="daylightDisplayName">The daylight saving time name of the new time zone.   </param>
		/// <param name="adjustmentRules">An array of <see cref="T:System.TimeZoneInfo.AdjustmentRule" /> objects that augment the base UTC offset for a particular period. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="id" /> parameter is null.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="id" /> parameter is an empty string ("").-or-The <paramref name="baseUtcOffset" /> parameter does not represent a whole number of minutes.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="baseUtcOffset" /> parameter is greater than 14 hours or less than -14 hours.</exception>
		/// <exception cref="T:System.InvalidTimeZoneException">The adjustment rules specified in the <paramref name="adjustmentRules" /> parameter overlap.-or-The adjustment rules specified in the <paramref name="adjustmentRules" /> parameter are not in chronological order.-or-One or more elements in <paramref name="adjustmentRules" /> are null.-or-A date can have multiple adjustment rules applied to it.-or-The sum of the <paramref name="baseUtcOffset" /> parameter and the <see cref="P:System.TimeZoneInfo.AdjustmentRule.DaylightDelta" /> value of one or more objects in the <paramref name="adjustmentRules" /> array is greater than 14 hours or less than -14 hours.</exception>
		/// <filterpriority>2</filterpriority>
		public static TimeZoneInfo CreateCustomTimeZone(string id, TimeSpan baseUtcOffset, string displayName, string standardDisplayName, string daylightDisplayName, TimeZoneInfo.AdjustmentRule[] adjustmentRules)
		{
			return TimeZoneInfo.CreateCustomTimeZone(id, baseUtcOffset, displayName, standardDisplayName, daylightDisplayName, adjustmentRules, false);
		}

		/// <summary>Creates a custom time zone with a specified identifier, an offset from Coordinated Universal Time (UTC), a display name, a standard time name, a daylight saving time name, daylight saving time rules, and a value that indicates whether the returned object reflects daylight saving time information.</summary>
		/// <returns>A <see cref="T:System.TimeZoneInfo" /> object that represents the new time zone. If the <paramref name="disableDaylightSavingTime" /> parameter is true, the returned object has no daylight saving time data.</returns>
		/// <param name="id">The time zone's identifier.</param>
		/// <param name="baseUtcOffset">A <see cref="T:System.TimeSpan" /> object that represents the time difference between this time zone and Coordinated Universal Time (UTC).</param>
		/// <param name="displayName">The display name of the new time zone.   </param>
		/// <param name="standardDisplayName">The standard time name of the new time zone.</param>
		/// <param name="daylightDisplayName">The daylight saving time name of the new time zone.   </param>
		/// <param name="adjustmentRules">An array of <see cref="T:System.TimeZoneInfo.AdjustmentRule" /> objects that augment the base UTC offset for a particular period.</param>
		/// <param name="disableDaylightSavingTime">true to discard any daylight saving time-related information present in <paramref name="adjustmentRules" /> with the new object; otherwise, false.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="id" /> parameter is null.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="id" /> parameter is an empty string ("").-or-The <paramref name="baseUtcOffset" /> parameter does not represent a whole number of minutes.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="baseUtcOffset" /> parameter is greater than 14 hours or less than -14 hours.</exception>
		/// <exception cref="T:System.InvalidTimeZoneException">The adjustment rules specified in the <paramref name="adjustmentRules" /> parameter overlap.-or-The adjustment rules specified in the <paramref name="adjustmentRules" /> parameter are not in chronological order.-or-One or more elements in <paramref name="adjustmentRules" /> are null.-or-A date can have multiple adjustment rules applied to it.-or-The sum of the <paramref name="baseUtcOffset" /> parameter and the <see cref="P:System.TimeZoneInfo.AdjustmentRule.DaylightDelta" /> value of one or more objects in the <paramref name="adjustmentRules" /> array is greater than 14 hours or less than -14 hours.</exception>
		/// <filterpriority>2</filterpriority>
		public static TimeZoneInfo CreateCustomTimeZone(string id, TimeSpan baseUtcOffset, string displayName, string standardDisplayName, string daylightDisplayName, TimeZoneInfo.AdjustmentRule[] adjustmentRules, bool disableDaylightSavingTime)
		{
			return new TimeZoneInfo(id, baseUtcOffset, displayName, standardDisplayName, daylightDisplayName, adjustmentRules, disableDaylightSavingTime);
		}

		/// <summary>Determines whether the current <see cref="T:System.TimeZoneInfo" /> object and another <see cref="T:System.TimeZoneInfo" /> object are equal.</summary>
		/// <returns>true if the two <see cref="T:System.TimeZoneInfo" /> objects are equal; otherwise, false.</returns>
		/// <param name="other">A second <see cref="T:System.TimeZoneInfo" /> object to compare with the current <see cref="T:System.TimeZoneInfo" /> object.  </param>
		/// <filterpriority>2</filterpriority>
		public bool Equals(TimeZoneInfo other)
		{
			return other != null && other.Id == this.Id && this.HasSameRules(other);
		}

		/// <summary>Retrieves a <see cref="T:System.TimeZoneInfo" /> object from the registry based on its identifier.</summary>
		/// <returns>A <see cref="T:System.TimeZoneInfo" /> object whose identifier is the value of the <paramref name="id" /> parameter.</returns>
		/// <param name="id">The time zone identifier, which corresponds to the <see cref="P:System.TimeZoneInfo.Id" /> property.      </param>
		/// <exception cref="T:System.OutOfMemoryException">The system does not have enough memory to hold information about the time zone.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="id" /> parameter is null.</exception>
		/// <exception cref="T:System.TimeZoneNotFoundException">The time zone identifier specified by <paramref name="id" /> was not found. This means that a registry key whose name matches <paramref name="id" /> does not exist, or that the key exists but does not contain any time zone data.</exception>
		/// <exception cref="T:System.Security.SecurityException">The process does not have the permissions required to read from the registry key that contains the time zone information.</exception>
		/// <exception cref="T:System.InvalidTimeZoneException">The time zone identifier was found, but the registry data is corrupted.</exception>
		public static TimeZoneInfo FindSystemTimeZoneById(string id)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			string filepath = Path.Combine(TimeZoneInfo.TimeZoneDirectory, id);
			return TimeZoneInfo.FindSystemTimeZoneByFileName(id, filepath);
		}

		private static TimeZoneInfo FindSystemTimeZoneByFileName(string id, string filepath)
		{
			if (!File.Exists(filepath))
			{
				throw new TimeZoneNotFoundException();
			}
			byte[] array = new byte[16384];
			int length;
			using (FileStream fileStream = File.OpenRead(filepath))
			{
				length = fileStream.Read(array, 0, 16384);
			}
			if (!TimeZoneInfo.ValidTZFile(array, length))
			{
				throw new InvalidTimeZoneException("TZ file too big for the buffer");
			}
			TimeZoneInfo result;
			try
			{
				result = TimeZoneInfo.ParseTZBuffer(id, array, length);
			}
			catch (Exception ex)
			{
				throw new InvalidTimeZoneException(ex.Message);
			}
			return result;
		}

		/// <summary>Deserializes a string to re-create an original serialized <see cref="T:System.TimeZoneInfo" /> object.</summary>
		/// <returns>A <see cref="T:System.TimeZoneInfo" /> object.</returns>
		/// <param name="source">The string representation of the serialized <see cref="T:System.TimeZoneInfo" /> object.   </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="source" /> parameter is <see cref="F:System.String.Empty" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> parameter is a null string.</exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The source parameter cannot be deserialized back into a <see cref="T:System.TimeZoneInfo" /> object.</exception>
		public static TimeZoneInfo FromSerializedString(string source)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves an array of <see cref="T:System.TimeZoneInfo.AdjustmentRule" /> objects that apply to the current <see cref="T:System.TimeZoneInfo" /> object.</summary>
		/// <returns>An array of <see cref="T:System.TimeZoneInfo.AdjustmentRule" /> objects for this time zone.</returns>
		/// <exception cref="T:System.OutOfMemoryException">The system does not have enough memory to make an in-memory copy of the adjustment rules.</exception>
		/// <filterpriority>2</filterpriority>
		public TimeZoneInfo.AdjustmentRule[] GetAdjustmentRules()
		{
			if (this.disableDaylightSavingTime)
			{
				return new TimeZoneInfo.AdjustmentRule[0];
			}
			return (TimeZoneInfo.AdjustmentRule[])this.adjustmentRules.Clone();
		}

		/// <summary>Returns information about the possible dates and times that an ambiguous date and time can be mapped to.</summary>
		/// <returns>An array of <see cref="T:System.TimeSpan" /> objects that represents possible Coordinated Universal Time (UTC) offsets that a particular date and time can be mapped to.</returns>
		/// <param name="dateTime">A date and time.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="dateTime" /> is not an ambiguous time.</exception>
		/// <filterpriority>2</filterpriority>
		public TimeSpan[] GetAmbiguousTimeOffsets(DateTime dateTime)
		{
			if (!this.IsAmbiguousTime(dateTime))
			{
				throw new ArgumentException("dateTime is not an ambiguous time");
			}
			TimeZoneInfo.AdjustmentRule applicableRule = this.GetApplicableRule(dateTime);
			return new TimeSpan[]
			{
				this.baseUtcOffset,
				this.baseUtcOffset + applicableRule.DaylightDelta
			};
		}

		/// <summary>Returns information about the possible dates and times that an ambiguous date and time can be mapped to.</summary>
		/// <returns>An array of <see cref="T:System.TimeSpan" /> objects that represents possible Coordinated Universal Time (UTC) offsets that a particular date and time can be mapped to.</returns>
		/// <param name="dateTimeOffset">A date and time.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="dateTime" /> is not an ambiguous time.</exception>
		/// <filterpriority>2</filterpriority>
		public TimeSpan[] GetAmbiguousTimeOffsets(DateTimeOffset dateTimeOffset)
		{
			if (!this.IsAmbiguousTime(dateTimeOffset))
			{
				throw new ArgumentException("dateTimeOffset is not an ambiguous time");
			}
			throw new NotImplementedException();
		}

		/// <summary>Serves as a hash function for hashing algorithms and data structures such as hash tables.</summary>
		/// <returns>A 32-bit signed integer that serves as the hash code for this <see cref="T:System.TimeZoneInfo" /> object.</returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			int num = this.Id.GetHashCode();
			foreach (TimeZoneInfo.AdjustmentRule adjustmentRule in this.GetAdjustmentRules())
			{
				num ^= adjustmentRule.GetHashCode();
			}
			return num;
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotImplementedException();
		}

		/// <summary>Returns a sorted collection of all the time zones about which information is available on the local system.</summary>
		/// <returns>A read-only collection of <see cref="T:System.TimeZoneInfo" /> objects.</returns>
		/// <exception cref="T:System.OutOfMemoryException">There is insufficient memory to store all time zone information.</exception>
		/// <exception cref="T:System.Security.SecurityException">The user does not have permission to read from the registry keys that contain time zone information.</exception>
		public static ReadOnlyCollection<TimeZoneInfo> GetSystemTimeZones()
		{
			if (TimeZoneInfo.systemTimeZones == null)
			{
				TimeZoneInfo.systemTimeZones = new List<TimeZoneInfo>();
				string[] array = new string[]
				{
					"Africa",
					"America",
					"Antarctica",
					"Arctic",
					"Asia",
					"Atlantic",
					"Brazil",
					"Canada",
					"Chile",
					"Europe",
					"Indian",
					"Mexico",
					"Mideast",
					"Pacific",
					"US"
				};
				foreach (string text in array)
				{
					try
					{
						foreach (string path in Directory.GetFiles(Path.Combine(TimeZoneInfo.TimeZoneDirectory, text)))
						{
							try
							{
								string text2 = string.Format("{0}/{1}", text, Path.GetFileName(path));
								TimeZoneInfo.systemTimeZones.Add(TimeZoneInfo.FindSystemTimeZoneById(text2));
							}
							catch (ArgumentNullException)
							{
							}
							catch (TimeZoneNotFoundException)
							{
							}
							catch (InvalidTimeZoneException)
							{
							}
							catch (Exception)
							{
								throw;
							}
						}
					}
					catch
					{
					}
				}
			}
			return new ReadOnlyCollection<TimeZoneInfo>(TimeZoneInfo.systemTimeZones);
		}

		/// <summary>Calculates the offset or difference between the time in this time zone and Coordinated Universal Time (UTC) for a particular date and time.</summary>
		/// <returns>A <see cref="T:System.TimeSpan" /> object that indicates the time difference between the two time zones.</returns>
		/// <param name="dateTime">The date and time to determine the offset for.   </param>
		public TimeSpan GetUtcOffset(DateTime dateTime)
		{
			if (this.IsDaylightSavingTime(dateTime))
			{
				TimeZoneInfo.AdjustmentRule applicableRule = this.GetApplicableRule(dateTime);
				return this.BaseUtcOffset + applicableRule.DaylightDelta;
			}
			return this.BaseUtcOffset;
		}

		/// <summary>Calculates the offset or difference between the time in this time zone and Coordinated Universal Time (UTC) for a particular date and time.</summary>
		/// <returns>A <see cref="T:System.TimeSpan" /> object that indicates the time difference between Coordinated Universal Time (UTC) and the current time zone.</returns>
		/// <param name="dateTimeOffset">The date and time to determine the offset for.</param>
		public TimeSpan GetUtcOffset(DateTimeOffset dateTimeOffset)
		{
			throw new NotImplementedException();
		}

		/// <summary>Indicates whether the current object and another <see cref="T:System.TimeZoneInfo" /> object have the same adjustment rules.</summary>
		/// <returns>true if the two time zones have identical adjustment rules and an identical base offset; otherwise, false.</returns>
		/// <param name="other">A second <see cref="T:System.TimeZoneInfo" /> object to compare with the current <see cref="T:System.TimeZoneInfo" /> object.   </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="other" /> parameter is null.</exception>
		public bool HasSameRules(TimeZoneInfo other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			if (this.adjustmentRules == null != (other.adjustmentRules == null))
			{
				return false;
			}
			if (this.adjustmentRules == null)
			{
				return true;
			}
			if (this.BaseUtcOffset != other.BaseUtcOffset)
			{
				return false;
			}
			if (this.adjustmentRules.Length != other.adjustmentRules.Length)
			{
				return false;
			}
			for (int i = 0; i < this.adjustmentRules.Length; i++)
			{
				if (!this.adjustmentRules[i].Equals(other.adjustmentRules[i]))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>Determines whether a particular date and time in a particular time zone is ambiguous and can be mapped to two or more Coordinated Universal Time (UTC) times.</summary>
		/// <returns>true if the <paramref name="dateTime" /> parameter is ambiguous; otherwise, false.</returns>
		/// <param name="dateTime">A date and time value.   </param>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.DateTime.Kind" /> property of the <paramref name="dateTime" /> value is <see cref="F:System.DateTimeKind.Local" /> and <paramref name="dateTime" /> is an invalid time.</exception>
		/// <filterpriority>2</filterpriority>
		public bool IsAmbiguousTime(DateTime dateTime)
		{
			if (dateTime.Kind == DateTimeKind.Local && this.IsInvalidTime(dateTime))
			{
				throw new ArgumentException("Kind is Local and time is Invalid");
			}
			if (this == TimeZoneInfo.Utc)
			{
				return false;
			}
			if (dateTime.Kind == DateTimeKind.Utc)
			{
				dateTime = this.ConvertTimeFromUtc(dateTime);
			}
			if (dateTime.Kind == DateTimeKind.Local && this != TimeZoneInfo.Local)
			{
				dateTime = TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.Local, this);
			}
			TimeZoneInfo.AdjustmentRule applicableRule = this.GetApplicableRule(dateTime);
			DateTime dateTime2 = TimeZoneInfo.TransitionPoint(applicableRule.DaylightTransitionEnd, dateTime.Year);
			return dateTime > dateTime2 - applicableRule.DaylightDelta && dateTime <= dateTime2;
		}

		/// <summary>Determines whether a particular date and time in a particular time zone is ambiguous and can be mapped to two or more Coordinated Universal Time (UTC) times.</summary>
		/// <returns>true if the <paramref name="dateTimeOffset" /> parameter is ambiguous in the current time zone; otherwise, false.</returns>
		/// <param name="dateTimeOffset">A date and time.</param>
		/// <filterpriority>2</filterpriority>
		public bool IsAmbiguousTime(DateTimeOffset dateTimeOffset)
		{
			throw new NotImplementedException();
		}

		/// <summary>Indicates whether a specified date and time falls in the range of daylight saving time for the time zone of the current <see cref="T:System.TimeZoneInfo" /> object.</summary>
		/// <returns>true if the <paramref name="dateTime" /> parameter is a daylight saving time; otherwise, false.</returns>
		/// <param name="dateTime">A date and time value.   </param>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.DateTime.Kind" /> property of the <paramref name="dateTime" /> value is <see cref="F:System.DateTimeKind.Local" /> and <paramref name="dateTime" /> is an invalid time.</exception>
		public bool IsDaylightSavingTime(DateTime dateTime)
		{
			if (dateTime.Kind == DateTimeKind.Local && this.IsInvalidTime(dateTime))
			{
				throw new ArgumentException("dateTime is invalid and Kind is Local");
			}
			if (this == TimeZoneInfo.Utc)
			{
				return false;
			}
			if (!this.SupportsDaylightSavingTime)
			{
				return false;
			}
			if ((dateTime.Kind == DateTimeKind.Local || dateTime.Kind == DateTimeKind.Unspecified) && this == TimeZoneInfo.Local)
			{
				return dateTime.IsDaylightSavingTime();
			}
			if (dateTime.Kind == DateTimeKind.Local && this != TimeZoneInfo.Utc)
			{
				return this.IsDaylightSavingTime(DateTime.SpecifyKind(dateTime.ToUniversalTime(), DateTimeKind.Utc));
			}
			TimeZoneInfo.AdjustmentRule applicableRule = this.GetApplicableRule(dateTime.Date);
			if (applicableRule == null)
			{
				return false;
			}
			DateTime dateTime2 = TimeZoneInfo.TransitionPoint(applicableRule.DaylightTransitionStart, dateTime.Year);
			DateTime dateTime3 = TimeZoneInfo.TransitionPoint(applicableRule.DaylightTransitionEnd, dateTime.Year + ((applicableRule.DaylightTransitionStart.Month >= applicableRule.DaylightTransitionEnd.Month) ? 1 : 0));
			if (dateTime.Kind == DateTimeKind.Utc)
			{
				dateTime2 -= this.BaseUtcOffset;
				dateTime3 -= this.BaseUtcOffset + applicableRule.DaylightDelta;
			}
			return dateTime >= dateTime2 && dateTime < dateTime3;
		}

		/// <summary>Indicates whether a specified date and time falls in the range of daylight saving time for the time zone of the current <see cref="T:System.TimeZoneInfo" /> object.</summary>
		/// <returns>true if the <paramref name="dateTimeOffset" /> parameter is a daylight saving time; otherwise, false.</returns>
		/// <param name="dateTimeOffset">A date and time value.</param>
		public bool IsDaylightSavingTime(DateTimeOffset dateTimeOffset)
		{
			throw new NotImplementedException();
		}

		/// <summary>Indicates whether a particular date and time is invalid.</summary>
		/// <returns>true if <paramref name="dateTime" /> is invalid; otherwise, false.</returns>
		/// <param name="dateTime">A date and time value.   </param>
		/// <filterpriority>2</filterpriority>
		public bool IsInvalidTime(DateTime dateTime)
		{
			if (dateTime.Kind == DateTimeKind.Utc)
			{
				return false;
			}
			if (dateTime.Kind == DateTimeKind.Local && this != TimeZoneInfo.Local)
			{
				return false;
			}
			TimeZoneInfo.AdjustmentRule applicableRule = this.GetApplicableRule(dateTime);
			DateTime dateTime2 = TimeZoneInfo.TransitionPoint(applicableRule.DaylightTransitionStart, dateTime.Year);
			return dateTime >= dateTime2 && dateTime < dateTime2 + applicableRule.DaylightDelta;
		}

		public void OnDeserialization(object sender)
		{
			throw new NotImplementedException();
		}

		/// <summary>Converts the current <see cref="T:System.TimeZoneInfo" /> object to a serialized string.</summary>
		/// <returns>A string that represents the current <see cref="T:System.TimeZoneInfo" /> object.</returns>
		public string ToSerializedString()
		{
			throw new NotImplementedException();
		}

		/// <summary>Returns the current <see cref="T:System.TimeZoneInfo" /> object's display name.</summary>
		/// <returns>The value of the <see cref="P:System.TimeZoneInfo.DisplayName" /> property of the current <see cref="T:System.TimeZoneInfo" /> object.</returns>
		public override string ToString()
		{
			return this.DisplayName;
		}

		private TimeZoneInfo.AdjustmentRule GetApplicableRule(DateTime dateTime)
		{
			DateTime d = dateTime;
			if (dateTime.Kind == DateTimeKind.Local && this != TimeZoneInfo.Local)
			{
				d = d.ToUniversalTime() + this.BaseUtcOffset;
			}
			if (dateTime.Kind == DateTimeKind.Utc && this != TimeZoneInfo.Utc)
			{
				d += this.BaseUtcOffset;
			}
			foreach (TimeZoneInfo.AdjustmentRule adjustmentRule in this.adjustmentRules)
			{
				if (adjustmentRule.DateStart > d.Date)
				{
					return null;
				}
				if (!(adjustmentRule.DateEnd < d.Date))
				{
					return adjustmentRule;
				}
			}
			return null;
		}

		private static DateTime TransitionPoint(TimeZoneInfo.TransitionTime transition, int year)
		{
			if (transition.IsFixedDateRule)
			{
				return new DateTime(year, transition.Month, transition.Day) + transition.TimeOfDay.TimeOfDay;
			}
			DateTime dateTime = new DateTime(year, transition.Month, 1);
			DayOfWeek dayOfWeek = dateTime.DayOfWeek;
			int num = 1 + (transition.Week - 1) * 7 + (transition.DayOfWeek - dayOfWeek) % 7;
			if (num > DateTime.DaysInMonth(year, transition.Month))
			{
				num -= 7;
			}
			return new DateTime(year, transition.Month, num) + transition.TimeOfDay.TimeOfDay;
		}

		private static bool ValidTZFile(byte[] buffer, int length)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < 4; i++)
			{
				stringBuilder.Append((char)buffer[i]);
			}
			return !(stringBuilder.ToString() != "TZif") && length < 16384;
		}

		private static int SwapInt32(int i)
		{
			return (i >> 24 & 255) | (i >> 8 & 65280) | (i << 8 & 16711680) | i << 24;
		}

		private static int ReadBigEndianInt32(byte[] buffer, int start)
		{
			int num = BitConverter.ToInt32(buffer, start);
			if (!BitConverter.IsLittleEndian)
			{
				return num;
			}
			return TimeZoneInfo.SwapInt32(num);
		}

		private static TimeZoneInfo ParseTZBuffer(string id, byte[] buffer, int length)
		{
			int num = TimeZoneInfo.ReadBigEndianInt32(buffer, 20);
			int num2 = TimeZoneInfo.ReadBigEndianInt32(buffer, 24);
			int num3 = TimeZoneInfo.ReadBigEndianInt32(buffer, 28);
			int num4 = TimeZoneInfo.ReadBigEndianInt32(buffer, 32);
			int num5 = TimeZoneInfo.ReadBigEndianInt32(buffer, 36);
			int num6 = TimeZoneInfo.ReadBigEndianInt32(buffer, 40);
			if (length < 44 + num4 * 5 + num5 * 6 + num6 + num3 * 8 + num2 + num)
			{
				throw new InvalidTimeZoneException();
			}
			Dictionary<int, string> abbreviations = TimeZoneInfo.ParseAbbreviations(buffer, 44 + 4 * num4 + num4 + 6 * num5, num6);
			Dictionary<int, TimeZoneInfo.TimeType> dictionary = TimeZoneInfo.ParseTimesTypes(buffer, 44 + 4 * num4 + num4, num5, abbreviations);
			List<KeyValuePair<DateTime, TimeZoneInfo.TimeType>> list = TimeZoneInfo.ParseTransitions(buffer, 44, num4, dictionary);
			if (dictionary.Count == 0)
			{
				throw new InvalidTimeZoneException();
			}
			if (dictionary.Count == 1 && dictionary[0].IsDst)
			{
				throw new InvalidTimeZoneException();
			}
			TimeSpan timeSpan = new TimeSpan(0L);
			TimeSpan timeSpan2 = new TimeSpan(0L);
			string text = null;
			string a = null;
			bool flag = false;
			DateTime d = DateTime.MinValue;
			List<TimeZoneInfo.AdjustmentRule> list2 = new List<TimeZoneInfo.AdjustmentRule>();
			for (int i = 0; i < list.Count; i++)
			{
				KeyValuePair<DateTime, TimeZoneInfo.TimeType> keyValuePair = list[i];
				DateTime key = keyValuePair.Key;
				TimeZoneInfo.TimeType value = keyValuePair.Value;
				if (!value.IsDst)
				{
					if (text != value.Name || timeSpan.TotalSeconds != (double)value.Offset)
					{
						text = value.Name;
						a = null;
						timeSpan = new TimeSpan(0, 0, value.Offset);
						list2 = new List<TimeZoneInfo.AdjustmentRule>();
						flag = false;
					}
					if (flag)
					{
						d += timeSpan;
						DateTime d2 = key + timeSpan + timeSpan2;
						if (d2.Date == new DateTime(d2.Year, 1, 1) && d2.Year > d.Year)
						{
							d2 -= new TimeSpan(24, 0, 0);
						}
						DateTime dateStart;
						if (d.Month < 7)
						{
							dateStart = new DateTime(d.Year, 1, 1);
						}
						else
						{
							dateStart = new DateTime(d.Year, 7, 1);
						}
						DateTime dateEnd;
						if (d2.Month >= 7)
						{
							dateEnd = new DateTime(d2.Year, 12, 31);
						}
						else
						{
							dateEnd = new DateTime(d2.Year, 6, 30);
						}
						TimeZoneInfo.TransitionTime transitionTime = TimeZoneInfo.TransitionTime.CreateFixedDateRule(new DateTime(1, 1, 1) + d.TimeOfDay, d.Month, d.Day);
						TimeZoneInfo.TransitionTime transitionTime2 = TimeZoneInfo.TransitionTime.CreateFixedDateRule(new DateTime(1, 1, 1) + d2.TimeOfDay, d2.Month, d2.Day);
						if (transitionTime != transitionTime2)
						{
							list2.Add(TimeZoneInfo.AdjustmentRule.CreateAdjustmentRule(dateStart, dateEnd, timeSpan2, transitionTime, transitionTime2));
						}
					}
					flag = false;
				}
				else
				{
					if (a != value.Name || timeSpan2.TotalSeconds != (double)value.Offset - timeSpan.TotalSeconds)
					{
						a = value.Name;
						timeSpan2 = new TimeSpan(0, 0, value.Offset) - timeSpan;
					}
					d = key;
					flag = true;
				}
			}
			if (list2.Count == 0)
			{
				TimeZoneInfo.TimeType timeType = dictionary[0];
				if (text == null)
				{
					text = timeType.Name;
					timeSpan = new TimeSpan(0, 0, timeType.Offset);
				}
				return TimeZoneInfo.CreateCustomTimeZone(id, timeSpan, id, text);
			}
			return TimeZoneInfo.CreateCustomTimeZone(id, timeSpan, id, text, a, TimeZoneInfo.ValidateRules(list2).ToArray());
		}

		private static List<TimeZoneInfo.AdjustmentRule> ValidateRules(List<TimeZoneInfo.AdjustmentRule> adjustmentRules)
		{
			TimeZoneInfo.AdjustmentRule adjustmentRule = null;
			foreach (TimeZoneInfo.AdjustmentRule adjustmentRule2 in adjustmentRules.ToArray())
			{
				if (adjustmentRule != null && adjustmentRule.DateEnd > adjustmentRule2.DateStart)
				{
					adjustmentRules.Remove(adjustmentRule2);
				}
				adjustmentRule = adjustmentRule2;
			}
			return adjustmentRules;
		}

		private static Dictionary<int, string> ParseAbbreviations(byte[] buffer, int index, int count)
		{
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < count; i++)
			{
				char c = (char)buffer[index + i];
				if (c != '\0')
				{
					stringBuilder.Append(c);
				}
				else
				{
					dictionary.Add(num, stringBuilder.ToString());
					for (int j = 1; j < stringBuilder.Length; j++)
					{
						dictionary.Add(num + j, stringBuilder.ToString(j, stringBuilder.Length - j));
					}
					num = i + 1;
					stringBuilder = new StringBuilder();
				}
			}
			return dictionary;
		}

		private static Dictionary<int, TimeZoneInfo.TimeType> ParseTimesTypes(byte[] buffer, int index, int count, Dictionary<int, string> abbreviations)
		{
			Dictionary<int, TimeZoneInfo.TimeType> dictionary = new Dictionary<int, TimeZoneInfo.TimeType>(count);
			for (int i = 0; i < count; i++)
			{
				int offset = TimeZoneInfo.ReadBigEndianInt32(buffer, index + 6 * i);
				byte b = buffer[index + 6 * i + 4];
				byte key = buffer[index + 6 * i + 5];
				dictionary.Add(i, new TimeZoneInfo.TimeType(offset, b != 0, abbreviations[(int)key]));
			}
			return dictionary;
		}

		private static List<KeyValuePair<DateTime, TimeZoneInfo.TimeType>> ParseTransitions(byte[] buffer, int index, int count, Dictionary<int, TimeZoneInfo.TimeType> time_types)
		{
			List<KeyValuePair<DateTime, TimeZoneInfo.TimeType>> list = new List<KeyValuePair<DateTime, TimeZoneInfo.TimeType>>(count);
			for (int i = 0; i < count; i++)
			{
				int num = TimeZoneInfo.ReadBigEndianInt32(buffer, index + 4 * i);
				DateTime key = TimeZoneInfo.DateTimeFromUnixTime((long)num);
				byte key2 = buffer[index + 4 * count + i];
				list.Add(new KeyValuePair<DateTime, TimeZoneInfo.TimeType>(key, time_types[(int)key2]));
			}
			return list;
		}

		private static DateTime DateTimeFromUnixTime(long unix_time)
		{
			DateTime dateTime = new DateTime(1970, 1, 1);
			return dateTime.AddSeconds((double)unix_time);
		}

		/// <summary>Provides information about a time zone adjustment, such as the transition to and from daylight saving time.</summary>
		/// <filterpriority>2</filterpriority>
		[Serializable]
		public sealed class AdjustmentRule : ISerializable, IDeserializationCallback, IEquatable<TimeZoneInfo.AdjustmentRule>
		{
			private DateTime dateEnd;

			private DateTime dateStart;

			private TimeSpan daylightDelta;

			private TimeZoneInfo.TransitionTime daylightTransitionEnd;

			private TimeZoneInfo.TransitionTime daylightTransitionStart;

			private AdjustmentRule(DateTime dateStart, DateTime dateEnd, TimeSpan daylightDelta, TimeZoneInfo.TransitionTime daylightTransitionStart, TimeZoneInfo.TransitionTime daylightTransitionEnd)
			{
				if (dateStart.Kind != DateTimeKind.Unspecified || dateEnd.Kind != DateTimeKind.Unspecified)
				{
					throw new ArgumentException("the Kind property of dateStart or dateEnd parameter does not equal DateTimeKind.Unspecified");
				}
				if (daylightTransitionStart == daylightTransitionEnd)
				{
					throw new ArgumentException("daylightTransitionStart parameter cannot equal daylightTransitionEnd parameter");
				}
				if (dateStart.Ticks % 864000000000L != 0L || dateEnd.Ticks % 864000000000L != 0L)
				{
					throw new ArgumentException("dateStart or dateEnd parameter includes a time of day value");
				}
				if (dateEnd < dateStart)
				{
					throw new ArgumentOutOfRangeException("dateEnd is earlier than dateStart");
				}
				if (daylightDelta > new TimeSpan(14, 0, 0) || daylightDelta < new TimeSpan(-14, 0, 0))
				{
					throw new ArgumentOutOfRangeException("daylightDelta is less than -14 or greater than 14 hours");
				}
				if (daylightDelta.Ticks % 10000000L != 0L)
				{
					throw new ArgumentOutOfRangeException("daylightDelta parameter does not represent a whole number of seconds");
				}
				this.dateStart = dateStart;
				this.dateEnd = dateEnd;
				this.daylightDelta = daylightDelta;
				this.daylightTransitionStart = daylightTransitionStart;
				this.daylightTransitionEnd = daylightTransitionEnd;
			}

			/// <summary>Gets the date when the adjustment rule ceases to be in effect.</summary>
			/// <returns>A <see cref="T:System.DateTime" /> value that indicates the end date of the adjustment rule.</returns>
			public DateTime DateEnd
			{
				get
				{
					return this.dateEnd;
				}
			}

			/// <summary>Gets the date when the adjustment rule takes effect.</summary>
			/// <returns>A <see cref="T:System.DateTime" /> value that indicates when the adjustment rule takes effect.</returns>
			public DateTime DateStart
			{
				get
				{
					return this.dateStart;
				}
			}

			/// <summary>Gets the amount of time that is required to form the time zone's daylight saving time. This amount of time is added to the time zone's offset from Coordinated Universal Time (UTC).</summary>
			/// <returns>A <see cref="T:System.TimeSpan" /> object that indicates the amount of time to add to the standard time changes as a result of the adjustment rule.</returns>
			public TimeSpan DaylightDelta
			{
				get
				{
					return this.daylightDelta;
				}
			}

			/// <summary>Gets information about the annual transition from daylight saving time back to standard time.</summary>
			/// <returns>A <see cref="T:System.TimeZoneInfo.TransitionTime" /> object that defines the annual transition from daylight saving time back to the time zone's standard time.</returns>
			public TimeZoneInfo.TransitionTime DaylightTransitionEnd
			{
				get
				{
					return this.daylightTransitionEnd;
				}
			}

			/// <summary>Gets information about the annual transition from standard time to daylight saving time.</summary>
			/// <returns>A <see cref="T:System.TimeZoneInfo.TransitionTime" /> object that defines the annual transition from a time zone's standard time to daylight saving time.</returns>
			public TimeZoneInfo.TransitionTime DaylightTransitionStart
			{
				get
				{
					return this.daylightTransitionStart;
				}
			}

			/// <summary>Creates a new adjustment rule for a particular time zone.</summary>
			/// <returns>A <see cref="T:System.TimeZoneInfo.AdjustmentRule" /> object that represents the new adjustment rule.</returns>
			/// <param name="dateStart">The effective date of the adjustment rule. If the value of the <paramref name="dateStart" /> parameter is DateTime.MinValue.Date, this is the first adjustment rule in effect for a time zone.   </param>
			/// <param name="dateEnd">The last date that the adjustment rule is in force. If the value of the <paramref name="dateEnd" /> parameter is DateTime.MaxValue.Date, the adjustment rule has no end date.</param>
			/// <param name="daylightDelta">The time change that results from the adjustment. This value is added to the time zone's <see cref="P:System.TimeZoneInfo.BaseUtcOffset" /> property to obtain the correct daylight offset from Coordinated Universal Time (UTC). This value can range from -14 to 14. </param>
			/// <param name="daylightTransitionStart">A <see cref="T:System.TimeZoneInfo.TransitionTime" /> object that defines the start of daylight saving time.</param>
			/// <param name="daylightTransitionEnd">A <see cref="T:System.TimeZoneInfo.TransitionTime" /> object that defines the end of daylight saving time.   </param>
			/// <exception cref="T:System.ArgumentException">The <see cref="P:System.DateTime.Kind" /> property of the <paramref name="dateStart" /> or <paramref name="dateEnd" /> parameter does not equal <see cref="F:System.DateTimeKind.Unspecified" />.-or-The <paramref name="daylightTransitionStart" /> parameter is equal to the <paramref name="daylightTransitionEnd" /> parameter.-or-The <paramref name="dateStart" /> or <paramref name="dateEnd" /> parameter includes a time of day value.</exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///   <paramref name="dateEnd" /> is earlier than <paramref name="dateStart" />.-or-<paramref name="daylightDelta" /> is less than -14 or greater than 14.-or-The <see cref="P:System.TimeSpan.Milliseconds" /> property of the <paramref name="daylightDelta" /> parameter is not equal to 0.-or-The <see cref="P:System.TimeSpan.Ticks" /> property of the <paramref name="daylightDelta" /> parameter does not equal a whole number of seconds.</exception>
			public static TimeZoneInfo.AdjustmentRule CreateAdjustmentRule(DateTime dateStart, DateTime dateEnd, TimeSpan daylightDelta, TimeZoneInfo.TransitionTime daylightTransitionStart, TimeZoneInfo.TransitionTime daylightTransitionEnd)
			{
				return new TimeZoneInfo.AdjustmentRule(dateStart, dateEnd, daylightDelta, daylightTransitionStart, daylightTransitionEnd);
			}

			/// <summary>Determines whether the current <see cref="T:System.TimeZoneInfo.AdjustmentRule" /> object is equal to a second <see cref="T:System.TimeZoneInfo.AdjustmentRule" /> object.</summary>
			/// <returns>true if both <see cref="T:System.TimeZoneInfo.AdjustmentRule" /> objects have equal values; otherwise, false.</returns>
			/// <param name="other">A second <see cref="T:System.TimeZoneInfo.AdjustmentRule" /> object.</param>
			/// <filterpriority>2</filterpriority>
			public bool Equals(TimeZoneInfo.AdjustmentRule other)
			{
				return this.dateStart == other.dateStart && this.dateEnd == other.dateEnd && this.daylightDelta == other.daylightDelta && this.daylightTransitionStart == other.daylightTransitionStart && this.daylightTransitionEnd == other.daylightTransitionEnd;
			}

			/// <summary>Serves as a hash function for hashing algorithms and data structures such as hash tables.</summary>
			/// <returns>A 32-bit signed integer that serves as the hash code for the current <see cref="T:System.TimeZoneInfo.AdjustmentRule" /> object.</returns>
			/// <filterpriority>2</filterpriority>
			public override int GetHashCode()
			{
				return this.dateStart.GetHashCode() ^ this.dateEnd.GetHashCode() ^ this.daylightDelta.GetHashCode() ^ this.daylightTransitionStart.GetHashCode() ^ this.daylightTransitionEnd.GetHashCode();
			}

			public void GetObjectData(SerializationInfo info, StreamingContext context)
			{
				throw new NotImplementedException();
			}

			public void OnDeserialization(object sender)
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>Provides information about a specific time change, such as the change from daylight saving time to standard time or vice versa, in a particular time zone.</summary>
		/// <filterpriority>2</filterpriority>
		[Serializable]
		public struct TransitionTime : ISerializable, IDeserializationCallback, IEquatable<TimeZoneInfo.TransitionTime>
		{
			private DateTime timeOfDay;

			private int month;

			private int day;

			private int week;

			private DayOfWeek dayOfWeek;

			private bool isFixedDateRule;

			private TransitionTime(DateTime timeOfDay, int month, int day)
			{
				this = new TimeZoneInfo.TransitionTime(timeOfDay, month);
				if (day < 1 || day > 31)
				{
					throw new ArgumentOutOfRangeException("day parameter is less than 1 or greater than 31");
				}
				this.day = day;
				this.isFixedDateRule = true;
			}

			private TransitionTime(DateTime timeOfDay, int month, int week, DayOfWeek dayOfWeek)
			{
				this = new TimeZoneInfo.TransitionTime(timeOfDay, month);
				if (week < 1 || week > 5)
				{
					throw new ArgumentOutOfRangeException("week parameter is less than 1 or greater than 5");
				}
				if (dayOfWeek != DayOfWeek.Sunday && dayOfWeek != DayOfWeek.Monday && dayOfWeek != DayOfWeek.Tuesday && dayOfWeek != DayOfWeek.Wednesday && dayOfWeek != DayOfWeek.Thursday && dayOfWeek != DayOfWeek.Friday && dayOfWeek != DayOfWeek.Saturday)
				{
					throw new ArgumentOutOfRangeException("dayOfWeek parameter is not a member od DayOfWeek enumeration");
				}
				this.week = week;
				this.dayOfWeek = dayOfWeek;
				this.isFixedDateRule = false;
			}

			private TransitionTime(DateTime timeOfDay, int month)
			{
				if (timeOfDay.Year != 1 || timeOfDay.Month != 1 || timeOfDay.Day != 1)
				{
					throw new ArgumentException("timeOfDay parameter has a non-default date component");
				}
				if (timeOfDay.Kind != DateTimeKind.Unspecified)
				{
					throw new ArgumentException("timeOfDay parameter Kind's property is not DateTimeKind.Unspecified");
				}
				if (timeOfDay.Ticks % 10000L != 0L)
				{
					throw new ArgumentException("timeOfDay parameter does not represent a whole number of milliseconds");
				}
				if (month < 1 || month > 12)
				{
					throw new ArgumentOutOfRangeException("month parameter is less than 1 or greater than 12");
				}
				this.timeOfDay = timeOfDay;
				this.month = month;
				this.week = -1;
				this.dayOfWeek = (DayOfWeek)(-1);
				this.day = -1;
				this.isFixedDateRule = false;
			}

			/// <summary>Gets the hour, minute, and second at which the time change occurs.</summary>
			/// <returns>A <see cref="T:System.DateTime" /> value that indicates the time of day at which the time change occurs.</returns>
			public DateTime TimeOfDay
			{
				get
				{
					return this.timeOfDay;
				}
			}

			/// <summary>Gets the month in which the time change occurs.</summary>
			/// <returns>The month in which the time change occurs.</returns>
			public int Month
			{
				get
				{
					return this.month;
				}
			}

			/// <summary>Gets the day on which the time change occurs.</summary>
			/// <returns>The day on which the time change occurs.</returns>
			public int Day
			{
				get
				{
					return this.day;
				}
			}

			/// <summary>Gets the week of the month in which a time change occurs.</summary>
			/// <returns>The week of the month in which the time change occurs.</returns>
			public int Week
			{
				get
				{
					return this.week;
				}
			}

			/// <summary>Gets the day of the week on which the time change occurs.</summary>
			/// <returns>The day of the week on which the time change occurs.</returns>
			public DayOfWeek DayOfWeek
			{
				get
				{
					return this.dayOfWeek;
				}
			}

			/// <summary>Gets a value indicating whether the time change occurs at a fixed date and time (such as November 1) or a floating date and time (such as the last Sunday of October).</summary>
			/// <returns>true if the time change rule is fixed-date; false if the time change rule is floating-date.</returns>
			public bool IsFixedDateRule
			{
				get
				{
					return this.isFixedDateRule;
				}
			}

			/// <summary>Defines a time change that uses a fixed-date rule.</summary>
			/// <returns>A <see cref="T:System.TimeZoneInfo.TransitionTime" /> object that contains data about the time change.</returns>
			/// <param name="timeOfDay">The time at which the time change occurs.</param>
			/// <param name="month">The month in which the time change occurs.</param>
			/// <param name="day">The day of the month on which the time change occurs.</param>
			/// <exception cref="T:System.ArgumentException">The <paramref name="timeOfDay" /> parameter has a non-default date component.-or-The <paramref name="timeOfDay" /> parameter's <see cref="P:System.DateTime.Kind" /> property is not <see cref="F:System.DateTimeKind.Unspecified" />.-or-The <paramref name="timeOfDay" /> parameter does not represent a whole number of milliseconds.</exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="month" /> parameter is less than 1 or greater than 12.-or-The <paramref name="day" /> parameter is less than 1 or greater than 31.</exception>
			public static TimeZoneInfo.TransitionTime CreateFixedDateRule(DateTime timeOfDay, int month, int day)
			{
				return new TimeZoneInfo.TransitionTime(timeOfDay, month, day);
			}

			/// <summary>Defines a time change that uses a floating-date rule.</summary>
			/// <returns>A <see cref="T:System.TimeZoneInfo.TransitionTime" /> object that contains data about the time change.</returns>
			/// <param name="timeOfDay">The time at which the time change occurs.</param>
			/// <param name="month">The month in which the time change occurs.</param>
			/// <param name="week">The week of the month in which the time change occurs.</param>
			/// <param name="dayOfWeek">The day of the week on which the time change occurs.</param>
			/// <exception cref="T:System.ArgumentException">The <paramref name="timeOfDay" /> parameter has a non-default date component.-or-The <paramref name="timeOfDay" /> parameter does not represent a whole number of milliseconds.-or-The <paramref name="timeOfDay" /> parameter's <see cref="P:System.DateTime.Kind" /> property is not <see cref="F:System.DateTimeKind.Unspecified" />.</exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///   <paramref name="month" /> is less than 1 or greater than 12.-or-<paramref name="week" /> is less than 1 or greater than 5.-or-The <paramref name="dayOfWeek" /> parameter is not a member of the <see cref="T:System.DayOfWeek" /> enumeration.</exception>
			public static TimeZoneInfo.TransitionTime CreateFloatingDateRule(DateTime timeOfDay, int month, int week, DayOfWeek dayOfWeek)
			{
				return new TimeZoneInfo.TransitionTime(timeOfDay, month, week, dayOfWeek);
			}

			public void GetObjectData(SerializationInfo info, StreamingContext context)
			{
				throw new NotImplementedException();
			}

			/// <summary>Determines whether an object has identical values to the current <see cref="T:System.TimeZoneInfo.TransitionTime" /> object.</summary>
			/// <returns>true if the two objects are equal; otherwise, false.</returns>
			/// <param name="obj">An object to compare with the current <see cref="T:System.TimeZoneInfo.TransitionTime" /> object.   </param>
			/// <filterpriority>2</filterpriority>
			public override bool Equals(object other)
			{
				return other is TimeZoneInfo.TransitionTime && this == (TimeZoneInfo.TransitionTime)other;
			}

			/// <summary>Determines whether the current <see cref="T:System.TimeZoneInfo.TransitionTime" /> object has identical values to a second <see cref="T:System.TimeZoneInfo.TransitionTime" /> object.</summary>
			/// <returns>true if the two objects have identical property values; otherwise, false.</returns>
			/// <param name="other">The second <see cref="T:System.TimeZoneInfo.TransitionTime" /> object.   </param>
			/// <filterpriority>2</filterpriority>
			public bool Equals(TimeZoneInfo.TransitionTime other)
			{
				return this == other;
			}

			/// <summary>Serves as a hash function for hashing algorithms and data structures such as hash tables.</summary>
			/// <returns>A 32-bit signed integer that serves as the hash code for this <see cref="T:System.TimeZoneInfo.TransitionTime" /> object.</returns>
			/// <filterpriority>2</filterpriority>
			public override int GetHashCode()
			{
				return this.day ^ (int)this.dayOfWeek ^ this.month ^ (int)this.timeOfDay.Ticks ^ this.week;
			}

			public void OnDeserialization(object sender)
			{
				throw new NotImplementedException();
			}

			/// <summary>Determines whether two specified <see cref="T:System.TimeZoneInfo.TransitionTime" /> objects are equal.</summary>
			/// <returns>true if <paramref name="left" /> and <paramref name="right" /> have identical values; otherwise, false.</returns>
			/// <param name="left">The first <see cref="T:System.TimeZoneInfo.TransitionTime" /> object.</param>
			/// <param name="right">The second <see cref="T:System.TimeZoneInfo.TransitionTime" /> object.</param>
			public static bool operator ==(TimeZoneInfo.TransitionTime t1, TimeZoneInfo.TransitionTime t2)
			{
				return t1.day == t2.day && t1.dayOfWeek == t2.dayOfWeek && t1.isFixedDateRule == t2.isFixedDateRule && t1.month == t2.month && t1.timeOfDay == t2.timeOfDay && t1.week == t2.week;
			}

			/// <summary>Determines whether two specified <see cref="T:System.TimeZoneInfo.TransitionTime" /> objects are not equal.</summary>
			/// <returns>true if <paramref name="left" /> and <paramref name="right" /> have any different member values; otherwise, false.</returns>
			/// <param name="left">The first <see cref="T:System.TimeZoneInfo.TransitionTime" /> object.</param>
			/// <param name="right">The second <see cref="T:System.TimeZoneInfo.TransitionTime" /> object.</param>
			public static bool operator !=(TimeZoneInfo.TransitionTime t1, TimeZoneInfo.TransitionTime t2)
			{
				return !(t1 == t2);
			}
		}

		private struct TimeType
		{
			public readonly int Offset;

			public readonly bool IsDst;

			public string Name;

			public TimeType(int offset, bool is_dst, string abbrev)
			{
				this.Offset = offset;
				this.IsDst = is_dst;
				this.Name = abbrev;
			}

			public override string ToString()
			{
				return string.Concat(new object[]
				{
					"offset: ",
					this.Offset,
					"s, is_dst: ",
					this.IsDst,
					", zone name: ",
					this.Name
				});
			}
		}
	}
}
