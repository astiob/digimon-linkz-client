using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Globalization
{
	/// <summary>Defines how <see cref="T:System.DateTime" /> values are formatted and displayed, depending on the culture.</summary>
	[ComVisible(true)]
	[Serializable]
	public sealed class DateTimeFormatInfo : ICloneable, IFormatProvider
	{
		private const string _RoundtripPattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK";

		private static readonly string MSG_READONLY = "This instance is read only";

		private static readonly string MSG_ARRAYSIZE_MONTH = "An array with exactly 13 elements is needed";

		private static readonly string MSG_ARRAYSIZE_DAY = "An array with exactly 7 elements is needed";

		private static readonly string[] INVARIANT_ABBREVIATED_DAY_NAMES = new string[]
		{
			"Sun",
			"Mon",
			"Tue",
			"Wed",
			"Thu",
			"Fri",
			"Sat"
		};

		private static readonly string[] INVARIANT_DAY_NAMES = new string[]
		{
			"Sunday",
			"Monday",
			"Tuesday",
			"Wednesday",
			"Thursday",
			"Friday",
			"Saturday"
		};

		private static readonly string[] INVARIANT_ABBREVIATED_MONTH_NAMES = new string[]
		{
			"Jan",
			"Feb",
			"Mar",
			"Apr",
			"May",
			"Jun",
			"Jul",
			"Aug",
			"Sep",
			"Oct",
			"Nov",
			"Dec",
			string.Empty
		};

		private static readonly string[] INVARIANT_MONTH_NAMES = new string[]
		{
			"January",
			"February",
			"March",
			"April",
			"May",
			"June",
			"July",
			"August",
			"September",
			"October",
			"November",
			"December",
			string.Empty
		};

		private static readonly string[] INVARIANT_SHORT_DAY_NAMES = new string[]
		{
			"Su",
			"Mo",
			"Tu",
			"We",
			"Th",
			"Fr",
			"Sa"
		};

		private static DateTimeFormatInfo theInvariantDateTimeFormatInfo;

		private bool m_isReadOnly;

		private string amDesignator;

		private string pmDesignator;

		private string dateSeparator;

		private string timeSeparator;

		private string shortDatePattern;

		private string longDatePattern;

		private string shortTimePattern;

		private string longTimePattern;

		private string monthDayPattern;

		private string yearMonthPattern;

		private string fullDateTimePattern;

		private string _RFC1123Pattern;

		private string _SortableDateTimePattern;

		private string _UniversalSortableDateTimePattern;

		private int firstDayOfWeek;

		private Calendar calendar;

		private int calendarWeekRule;

		private string[] abbreviatedDayNames;

		private string[] dayNames;

		private string[] monthNames;

		private string[] abbreviatedMonthNames;

		private string[] allShortDatePatterns;

		private string[] allLongDatePatterns;

		private string[] allShortTimePatterns;

		private string[] allLongTimePatterns;

		private string[] monthDayPatterns;

		private string[] yearMonthPatterns;

		private string[] shortDayNames;

		private int nDataItem;

		private bool m_useUserOverride;

		private bool m_isDefaultCalendar;

		private int CultureID;

		private bool bUseCalendarInfo;

		private string generalShortTimePattern;

		private string generalLongTimePattern;

		private string[] m_eraNames;

		private string[] m_abbrevEraNames;

		private string[] m_abbrevEnglishEraNames;

		private string[] m_dateWords;

		private int[] optionalCalendars;

		private string[] m_superShortDayNames;

		private string[] genitiveMonthNames;

		private string[] m_genitiveAbbreviatedMonthNames;

		private string[] leapYearMonthNames;

		private DateTimeFormatFlags formatFlags;

		private string m_name;

		private volatile string[] all_date_time_patterns;

		internal DateTimeFormatInfo(bool read_only)
		{
			this.m_isReadOnly = read_only;
			this.amDesignator = "AM";
			this.pmDesignator = "PM";
			this.dateSeparator = "/";
			this.timeSeparator = ":";
			this.shortDatePattern = "MM/dd/yyyy";
			this.longDatePattern = "dddd, dd MMMM yyyy";
			this.shortTimePattern = "HH:mm";
			this.longTimePattern = "HH:mm:ss";
			this.monthDayPattern = "MMMM dd";
			this.yearMonthPattern = "yyyy MMMM";
			this.fullDateTimePattern = "dddd, dd MMMM yyyy HH:mm:ss";
			this._RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
			this._SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
			this._UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
			this.firstDayOfWeek = 0;
			this.calendar = new GregorianCalendar();
			this.calendarWeekRule = 0;
			this.abbreviatedDayNames = DateTimeFormatInfo.INVARIANT_ABBREVIATED_DAY_NAMES;
			this.dayNames = DateTimeFormatInfo.INVARIANT_DAY_NAMES;
			this.abbreviatedMonthNames = DateTimeFormatInfo.INVARIANT_ABBREVIATED_MONTH_NAMES;
			this.monthNames = DateTimeFormatInfo.INVARIANT_MONTH_NAMES;
			this.m_genitiveAbbreviatedMonthNames = DateTimeFormatInfo.INVARIANT_ABBREVIATED_MONTH_NAMES;
			this.genitiveMonthNames = DateTimeFormatInfo.INVARIANT_MONTH_NAMES;
			this.shortDayNames = DateTimeFormatInfo.INVARIANT_SHORT_DAY_NAMES;
		}

		/// <summary>Initializes a new writable instance of the <see cref="T:System.Globalization.DateTimeFormatInfo" /> class that is culture-independent (invariant).</summary>
		public DateTimeFormatInfo() : this(false)
		{
		}

		/// <summary>Returns the <see cref="T:System.Globalization.DateTimeFormatInfo" /> associated with the specified <see cref="T:System.IFormatProvider" />.</summary>
		/// <returns>A <see cref="T:System.Globalization.DateTimeFormatInfo" /> associated with the specified <see cref="T:System.IFormatProvider" />.</returns>
		/// <param name="provider">The <see cref="T:System.IFormatProvider" /> that gets the <see cref="T:System.Globalization.DateTimeFormatInfo" />.-or- null to get <see cref="P:System.Globalization.DateTimeFormatInfo.CurrentInfo" />. </param>
		public static DateTimeFormatInfo GetInstance(IFormatProvider provider)
		{
			if (provider != null)
			{
				DateTimeFormatInfo dateTimeFormatInfo = (DateTimeFormatInfo)provider.GetFormat(typeof(DateTimeFormatInfo));
				if (dateTimeFormatInfo != null)
				{
					return dateTimeFormatInfo;
				}
			}
			return DateTimeFormatInfo.CurrentInfo;
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Globalization.DateTimeFormatInfo" /> object is read-only.</summary>
		/// <returns>true if the <see cref="T:System.Globalization.DateTimeFormatInfo" /> object is read-only; otherwise, false.</returns>
		public bool IsReadOnly
		{
			get
			{
				return this.m_isReadOnly;
			}
		}

		/// <summary>Returns a read-only <see cref="T:System.Globalization.DateTimeFormatInfo" /> wrapper.</summary>
		/// <returns>A read-only <see cref="T:System.Globalization.DateTimeFormatInfo" /> wrapper around <paramref name="dtfi" />.</returns>
		/// <param name="dtfi">The <see cref="T:System.Globalization.DateTimeFormatInfo" /> to wrap. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="dtfi" /> is null. </exception>
		public static DateTimeFormatInfo ReadOnly(DateTimeFormatInfo dtfi)
		{
			DateTimeFormatInfo dateTimeFormatInfo = (DateTimeFormatInfo)dtfi.Clone();
			dateTimeFormatInfo.m_isReadOnly = true;
			return dateTimeFormatInfo;
		}

		/// <summary>Creates a shallow copy of the <see cref="T:System.Globalization.DateTimeFormatInfo" />.</summary>
		/// <returns>A new <see cref="T:System.Globalization.DateTimeFormatInfo" /> copied from the original <see cref="T:System.Globalization.DateTimeFormatInfo" />.</returns>
		public object Clone()
		{
			DateTimeFormatInfo dateTimeFormatInfo = (DateTimeFormatInfo)base.MemberwiseClone();
			dateTimeFormatInfo.m_isReadOnly = false;
			return dateTimeFormatInfo;
		}

		/// <summary>Returns an object of the specified type that provides a <see cref="T:System.DateTime" /> formatting service.</summary>
		/// <returns>The current <see cref="T:System.Globalization.DateTimeFormatInfo" />, if <paramref name="formatType" /> is the same as the type of the current <see cref="T:System.Globalization.DateTimeFormatInfo" />; otherwise, null.</returns>
		/// <param name="formatType">The <see cref="T:System.Type" /> of the required formatting service. </param>
		public object GetFormat(Type formatType)
		{
			return (formatType != base.GetType()) ? null : this;
		}

		/// <summary>Returns the string containing the abbreviated name of the specified era, if an abbreviation exists.</summary>
		/// <returns>A string containing the abbreviated name of the specified era, if an abbreviation exists.-or- A string containing the full name of the era, if an abbreviation does not exist.</returns>
		/// <param name="era">The integer representing the era. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="era" /> does not represent a valid era in the calendar specified in the <see cref="P:System.Globalization.DateTimeFormatInfo.Calendar" /> property. </exception>
		public string GetAbbreviatedEraName(int era)
		{
			if (era < 0 || era >= this.calendar.AbbreviatedEraNames.Length)
			{
				throw new ArgumentOutOfRangeException("era", era.ToString());
			}
			return this.calendar.AbbreviatedEraNames[era];
		}

		/// <summary>Returns the culture-specific abbreviated name of the specified month based on the culture associated with the current <see cref="T:System.Globalization.DateTimeFormatInfo" /> object.</summary>
		/// <returns>The culture-specific abbreviated name of the month represented by <paramref name="month" />.</returns>
		/// <param name="month">An integer from 1 through 13 representing the name of the month to retrieve. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="month" /> is less than 1 or greater than 13. </exception>
		public string GetAbbreviatedMonthName(int month)
		{
			if (month < 1 || month > 13)
			{
				throw new ArgumentOutOfRangeException();
			}
			return this.abbreviatedMonthNames[month - 1];
		}

		/// <summary>Returns the integer representing the specified era.</summary>
		/// <returns>The integer representing the era, if <paramref name="eraName" /> is valid; otherwise, -1.</returns>
		/// <param name="eraName">The string containing the name of the era. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="eraName" /> is null. </exception>
		public int GetEra(string eraName)
		{
			if (eraName == null)
			{
				throw new ArgumentNullException();
			}
			string[] array = this.calendar.EraNames;
			for (int i = 0; i < array.Length; i++)
			{
				if (CultureInfo.InvariantCulture.CompareInfo.Compare(eraName, array[i], CompareOptions.IgnoreCase) == 0)
				{
					return this.calendar.Eras[i];
				}
			}
			array = this.calendar.AbbreviatedEraNames;
			for (int j = 0; j < array.Length; j++)
			{
				if (CultureInfo.InvariantCulture.CompareInfo.Compare(eraName, array[j], CompareOptions.IgnoreCase) == 0)
				{
					return this.calendar.Eras[j];
				}
			}
			return -1;
		}

		/// <summary>Returns the string containing the name of the specified era.</summary>
		/// <returns>A string containing the name of the era.</returns>
		/// <param name="era">The integer representing the era. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="era" /> does not represent a valid era in the calendar specified in the <see cref="P:System.Globalization.DateTimeFormatInfo.Calendar" /> property. </exception>
		public string GetEraName(int era)
		{
			if (era < 0 || era > this.calendar.EraNames.Length)
			{
				throw new ArgumentOutOfRangeException("era", era.ToString());
			}
			return this.calendar.EraNames[era - 1];
		}

		/// <summary>Returns the culture-specific full name of the specified month based on the culture associated with the current <see cref="T:System.Globalization.DateTimeFormatInfo" /> object.</summary>
		/// <returns>The culture-specific full name of the month represented by <paramref name="month" />.</returns>
		/// <param name="month">An integer from 1 through 13 representing the name of the month to retrieve. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="month" /> is less than 1 or greater than 13. </exception>
		public string GetMonthName(int month)
		{
			if (month < 1 || month > 13)
			{
				throw new ArgumentOutOfRangeException();
			}
			return this.monthNames[month - 1];
		}

		/// <summary>Gets or sets a one-dimensional array of type <see cref="T:System.String" /> containing the culture-specific abbreviated names of the days of the week.</summary>
		/// <returns>A one-dimensional array of type <see cref="T:System.String" /> containing the culture-specific abbreviated names of the days of the week. The array for <see cref="P:System.Globalization.DateTimeFormatInfo.InvariantInfo" /> contains "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", and "Sat".</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.ArgumentException">The property is being set to an array that is multidimensional or that has a length that is not exactly 7. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.DateTimeFormatInfo" /> is read-only. </exception>
		public string[] AbbreviatedDayNames
		{
			get
			{
				return (string[])this.RawAbbreviatedDayNames.Clone();
			}
			set
			{
				this.RawAbbreviatedDayNames = value;
			}
		}

		internal string[] RawAbbreviatedDayNames
		{
			get
			{
				return this.abbreviatedDayNames;
			}
			set
			{
				if (this.IsReadOnly)
				{
					throw new InvalidOperationException(DateTimeFormatInfo.MSG_READONLY);
				}
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				if (value.GetLength(0) != 7)
				{
					throw new ArgumentException(DateTimeFormatInfo.MSG_ARRAYSIZE_DAY);
				}
				this.abbreviatedDayNames = (string[])value.Clone();
			}
		}

		/// <summary>Gets or sets a one-dimensional array of type <see cref="T:System.String" /> containing the culture-specific abbreviated names of the months.</summary>
		/// <returns>A one-dimensional array of type <see cref="T:System.String" /> containing the culture-specific abbreviated names of the months. In a 12-month calendar, the 13th element of the array is an empty string. The array for <see cref="P:System.Globalization.DateTimeFormatInfo.InvariantInfo" /> contains "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", and "".</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.ArgumentException">The property is being set to an array that is multidimensional or that has a length that is not exactly 13. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.DateTimeFormatInfo" /> is read-only. </exception>
		public string[] AbbreviatedMonthNames
		{
			get
			{
				return (string[])this.RawAbbreviatedMonthNames.Clone();
			}
			set
			{
				this.RawAbbreviatedMonthNames = value;
			}
		}

		internal string[] RawAbbreviatedMonthNames
		{
			get
			{
				return this.abbreviatedMonthNames;
			}
			set
			{
				if (this.IsReadOnly)
				{
					throw new InvalidOperationException(DateTimeFormatInfo.MSG_READONLY);
				}
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				if (value.GetLength(0) != 13)
				{
					throw new ArgumentException(DateTimeFormatInfo.MSG_ARRAYSIZE_MONTH);
				}
				this.abbreviatedMonthNames = (string[])value.Clone();
			}
		}

		/// <summary>Gets or sets a one-dimensional array of type <see cref="T:System.String" /> containing the culture-specific full names of the days of the week.</summary>
		/// <returns>A one-dimensional array of type <see cref="T:System.String" /> containing the culture-specific full names of the days of the week. The array for <see cref="P:System.Globalization.DateTimeFormatInfo.InvariantInfo" /> contains "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", and "Saturday".</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.ArgumentException">The property is being set to an array that is multidimensional or that has a length that is not exactly 7. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.DateTimeFormatInfo" /> is read-only. </exception>
		public string[] DayNames
		{
			get
			{
				return (string[])this.RawDayNames.Clone();
			}
			set
			{
				this.RawDayNames = value;
			}
		}

		internal string[] RawDayNames
		{
			get
			{
				return this.dayNames;
			}
			set
			{
				if (this.IsReadOnly)
				{
					throw new InvalidOperationException(DateTimeFormatInfo.MSG_READONLY);
				}
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				if (value.GetLength(0) != 7)
				{
					throw new ArgumentException(DateTimeFormatInfo.MSG_ARRAYSIZE_DAY);
				}
				this.dayNames = (string[])value.Clone();
			}
		}

		/// <summary>Gets or sets a one-dimensional array of type <see cref="T:System.String" /> containing the culture-specific full names of the months.</summary>
		/// <returns>A one-dimensional array of type <see cref="T:System.String" /> containing the culture-specific full names of the months. In a 12-month calendar, the 13th element of the array is an empty string. The array for <see cref="P:System.Globalization.DateTimeFormatInfo.InvariantInfo" /> contains "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", and "".</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.ArgumentException">The property is being set to an array that is multidimensional or that has a length that is not exactly 13. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.DateTimeFormatInfo" /> is read-only. </exception>
		public string[] MonthNames
		{
			get
			{
				return (string[])this.RawMonthNames.Clone();
			}
			set
			{
				this.RawMonthNames = value;
			}
		}

		internal string[] RawMonthNames
		{
			get
			{
				return this.monthNames;
			}
			set
			{
				if (this.IsReadOnly)
				{
					throw new InvalidOperationException(DateTimeFormatInfo.MSG_READONLY);
				}
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				if (value.GetLength(0) != 13)
				{
					throw new ArgumentException(DateTimeFormatInfo.MSG_ARRAYSIZE_MONTH);
				}
				this.monthNames = (string[])value.Clone();
			}
		}

		/// <summary>Gets or sets the string designator for hours that are "ante meridiem" (before noon).</summary>
		/// <returns>The string designator for hours that are "ante meridiem" (before noon). The default for <see cref="P:System.Globalization.DateTimeFormatInfo.InvariantInfo" /> is "AM".</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.DateTimeFormatInfo" /> is read-only. </exception>
		public string AMDesignator
		{
			get
			{
				return this.amDesignator;
			}
			set
			{
				if (this.IsReadOnly)
				{
					throw new InvalidOperationException(DateTimeFormatInfo.MSG_READONLY);
				}
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				this.amDesignator = value;
			}
		}

		/// <summary>Gets or sets the string designator for hours that are "post meridiem" (after noon).</summary>
		/// <returns>The string designator for hours that are "post meridiem" (after noon). The default for <see cref="P:System.Globalization.DateTimeFormatInfo.InvariantInfo" /> is "PM".</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.DateTimeFormatInfo" /> is read-only. </exception>
		public string PMDesignator
		{
			get
			{
				return this.pmDesignator;
			}
			set
			{
				if (this.IsReadOnly)
				{
					throw new InvalidOperationException(DateTimeFormatInfo.MSG_READONLY);
				}
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				this.pmDesignator = value;
			}
		}

		/// <summary>Gets or sets the string that separates the components of a date, that is, the year, month, and day.</summary>
		/// <returns>The string that separates the components of a date, that is, the year, month, and day. The default for <see cref="P:System.Globalization.DateTimeFormatInfo.InvariantInfo" /> is "/".</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.DateTimeFormatInfo" /> is read-only. </exception>
		public string DateSeparator
		{
			get
			{
				return this.dateSeparator;
			}
			set
			{
				if (this.IsReadOnly)
				{
					throw new InvalidOperationException(DateTimeFormatInfo.MSG_READONLY);
				}
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				this.dateSeparator = value;
			}
		}

		/// <summary>Gets or sets the string that separates the components of time, that is, the hour, minutes, and seconds.</summary>
		/// <returns>The string that separates the components of time. The default for <see cref="P:System.Globalization.DateTimeFormatInfo.InvariantInfo" /> is ":".</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.DateTimeFormatInfo" /> is read-only. </exception>
		public string TimeSeparator
		{
			get
			{
				return this.timeSeparator;
			}
			set
			{
				if (this.IsReadOnly)
				{
					throw new InvalidOperationException(DateTimeFormatInfo.MSG_READONLY);
				}
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				this.timeSeparator = value;
			}
		}

		/// <summary>Gets or sets the format pattern for a long date value, which is associated with the "D" format pattern.</summary>
		/// <returns>The format pattern for a long date value, which is associated with the "D" format pattern.</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.DateTimeFormatInfo" /> is read-only. </exception>
		public string LongDatePattern
		{
			get
			{
				return this.longDatePattern;
			}
			set
			{
				if (this.IsReadOnly)
				{
					throw new InvalidOperationException(DateTimeFormatInfo.MSG_READONLY);
				}
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				this.longDatePattern = value;
			}
		}

		/// <summary>Gets or sets the format pattern for a short date value, which is associated with the "d" format pattern.</summary>
		/// <returns>The format pattern for a short date value, which is associated with the "d" format pattern.</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.DateTimeFormatInfo" /> object is read-only. </exception>
		public string ShortDatePattern
		{
			get
			{
				return this.shortDatePattern;
			}
			set
			{
				if (this.IsReadOnly)
				{
					throw new InvalidOperationException(DateTimeFormatInfo.MSG_READONLY);
				}
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				this.shortDatePattern = value;
			}
		}

		/// <summary>Gets or sets the format pattern for a short time value, which is associated with the "t" format pattern.</summary>
		/// <returns>The format pattern for a short time value, which is associated with the "t" format pattern.</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.DateTimeFormatInfo" /> is read-only. </exception>
		public string ShortTimePattern
		{
			get
			{
				return this.shortTimePattern;
			}
			set
			{
				if (this.IsReadOnly)
				{
					throw new InvalidOperationException(DateTimeFormatInfo.MSG_READONLY);
				}
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				this.shortTimePattern = value;
			}
		}

		/// <summary>Gets or sets the format pattern for a long time value, which is associated with the "T" format pattern.</summary>
		/// <returns>The format pattern for a long time value, which is associated with the "T" format pattern.</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.DateTimeFormatInfo" /> is read-only. </exception>
		public string LongTimePattern
		{
			get
			{
				return this.longTimePattern;
			}
			set
			{
				if (this.IsReadOnly)
				{
					throw new InvalidOperationException(DateTimeFormatInfo.MSG_READONLY);
				}
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				this.longTimePattern = value;
			}
		}

		/// <summary>Gets or sets the format pattern for a month and day value, which is associated with the "m" and "M" format patterns.</summary>
		/// <returns>The format pattern for a month and day value, which is associated with the "m" and "M" format patterns.</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.DateTimeFormatInfo" /> is read-only. </exception>
		public string MonthDayPattern
		{
			get
			{
				return this.monthDayPattern;
			}
			set
			{
				if (this.IsReadOnly)
				{
					throw new InvalidOperationException(DateTimeFormatInfo.MSG_READONLY);
				}
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				this.monthDayPattern = value;
			}
		}

		/// <summary>Gets or sets the format pattern for a year and month value, which is associated with the "y" and "Y" format patterns.</summary>
		/// <returns>The format pattern for a year and month value, which is associated with the "y" and "Y" format patterns.</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.DateTimeFormatInfo" /> is read-only. </exception>
		public string YearMonthPattern
		{
			get
			{
				return this.yearMonthPattern;
			}
			set
			{
				if (this.IsReadOnly)
				{
					throw new InvalidOperationException(DateTimeFormatInfo.MSG_READONLY);
				}
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				this.yearMonthPattern = value;
			}
		}

		/// <summary>Gets or sets the format pattern for a long date and long time value, which is associated with the "F" format pattern.</summary>
		/// <returns>The format pattern for a long date and long time value, which is associated with the "F" format pattern.</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.DateTimeFormatInfo" /> is read-only. </exception>
		public string FullDateTimePattern
		{
			get
			{
				if (this.fullDateTimePattern != null)
				{
					return this.fullDateTimePattern;
				}
				return this.longDatePattern + " " + this.longTimePattern;
			}
			set
			{
				if (this.IsReadOnly)
				{
					throw new InvalidOperationException(DateTimeFormatInfo.MSG_READONLY);
				}
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				this.fullDateTimePattern = value;
			}
		}

		/// <summary>Gets a read-only <see cref="T:System.Globalization.DateTimeFormatInfo" /> object that formats values based on the current culture.</summary>
		/// <returns>A read-only <see cref="T:System.Globalization.DateTimeFormatInfo" /> object based on the <see cref="T:System.Globalization.CultureInfo" /> object for the current thread.</returns>
		public static DateTimeFormatInfo CurrentInfo
		{
			get
			{
				return Thread.CurrentThread.CurrentCulture.DateTimeFormat;
			}
		}

		/// <summary>Gets the default read-only <see cref="T:System.Globalization.DateTimeFormatInfo" /> that is culture-independent (invariant).</summary>
		/// <returns>The default read-only <see cref="T:System.Globalization.DateTimeFormatInfo" /> object that is culture-independent (invariant).</returns>
		public static DateTimeFormatInfo InvariantInfo
		{
			get
			{
				if (DateTimeFormatInfo.theInvariantDateTimeFormatInfo == null)
				{
					DateTimeFormatInfo.theInvariantDateTimeFormatInfo = DateTimeFormatInfo.ReadOnly(new DateTimeFormatInfo());
					DateTimeFormatInfo.theInvariantDateTimeFormatInfo.FillInvariantPatterns();
				}
				return DateTimeFormatInfo.theInvariantDateTimeFormatInfo;
			}
		}

		/// <summary>Gets or sets the first day of the week.</summary>
		/// <returns>A <see cref="T:System.DayOfWeek" /> value representing the first day of the week. The default for <see cref="P:System.Globalization.DateTimeFormatInfo.InvariantInfo" /> is <see cref="F:System.DayOfWeek.Sunday" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The property is being set to a value that is not a valid <see cref="T:System.DayOfWeek" /> value. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.DateTimeFormatInfo" /> is read-only. </exception>
		public DayOfWeek FirstDayOfWeek
		{
			get
			{
				return (DayOfWeek)this.firstDayOfWeek;
			}
			set
			{
				if (this.IsReadOnly)
				{
					throw new InvalidOperationException(DateTimeFormatInfo.MSG_READONLY);
				}
				if (value < DayOfWeek.Sunday || value > DayOfWeek.Saturday)
				{
					throw new ArgumentOutOfRangeException();
				}
				this.firstDayOfWeek = (int)value;
			}
		}

		/// <summary>Gets or sets the calendar to use for the current culture.</summary>
		/// <returns>The <see cref="T:System.Globalization.Calendar" /> indicating the calendar to use for the current culture. The default for <see cref="P:System.Globalization.DateTimeFormatInfo.InvariantInfo" /> is the <see cref="T:System.Globalization.GregorianCalendar" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is being set to null. </exception>
		/// <exception cref="T:System.ArgumentException">The property is being set to a <see cref="T:System.Globalization.Calendar" /> that is not valid for the current culture. </exception>
		/// <exception cref="T:System.InvalidOperationException">The property is being set and the <see cref="T:System.Globalization.DateTimeFormatInfo" /> is read-only. </exception>
		public Calendar Calendar
		{
			get
			{
				return this.calendar;
			}
			set
			{
				if (this.IsReadOnly)
				{
					throw new InvalidOperationException(DateTimeFormatInfo.MSG_READONLY);
				}
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				this.calendar = value;
			}
		}

		/// <summary>Gets or sets a value that specifies which rule is used to determine the first calendar week of the year.</summary>
		/// <returns>A <see cref="T:System.Globalization.CalendarWeekRule" /> value that determines the first calendar week of the year. The default for <see cref="P:System.Globalization.DateTimeFormatInfo.InvariantInfo" /> is <see cref="F:System.Globalization.CalendarWeekRule.FirstDay" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The property is being set to a value that is not a valid <see cref="T:System.Globalization.CalendarWeekRule" /> value. </exception>
		public CalendarWeekRule CalendarWeekRule
		{
			get
			{
				return (CalendarWeekRule)this.calendarWeekRule;
			}
			set
			{
				if (this.IsReadOnly)
				{
					throw new InvalidOperationException(DateTimeFormatInfo.MSG_READONLY);
				}
				this.calendarWeekRule = (int)value;
			}
		}

		/// <summary>Gets the format pattern for a time value, which is based on the Internet Engineering Task Force (IETF) Request for Comments (RFC) 1123 specification and is associated with the "r" and "R" format patterns.</summary>
		/// <returns>The format pattern for a time value, which is based on the IETF RFC 1123 specification and is associated with the "r" and "R" format patterns.</returns>
		public string RFC1123Pattern
		{
			get
			{
				return this._RFC1123Pattern;
			}
		}

		internal string RoundtripPattern
		{
			get
			{
				return "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK";
			}
		}

		/// <summary>Gets the format pattern for a sortable date and time value, which is associated with the "s" format pattern.</summary>
		/// <returns>The format pattern for a sortable date and time value, which is associated with the "s" format pattern.</returns>
		public string SortableDateTimePattern
		{
			get
			{
				return this._SortableDateTimePattern;
			}
		}

		/// <summary>Gets the format pattern for a universal sortable date and time value, which is associated with the "u" format pattern.</summary>
		/// <returns>The format pattern for a universal sortable date and time value, which is associated with the "u" format pattern.</returns>
		public string UniversalSortableDateTimePattern
		{
			get
			{
				return this._UniversalSortableDateTimePattern;
			}
		}

		/// <summary>Returns all the standard patterns in which date and time values can be formatted.</summary>
		/// <returns>An array containing the standard patterns in which date and time values can be formatted.</returns>
		public string[] GetAllDateTimePatterns()
		{
			return (string[])this.GetAllDateTimePatternsInternal().Clone();
		}

		internal string[] GetAllDateTimePatternsInternal()
		{
			this.FillAllDateTimePatterns();
			return this.all_date_time_patterns;
		}

		private void FillAllDateTimePatterns()
		{
			if (this.all_date_time_patterns != null)
			{
				return;
			}
			ArrayList arrayList = new ArrayList();
			arrayList.AddRange(this.GetAllRawDateTimePatterns('d'));
			arrayList.AddRange(this.GetAllRawDateTimePatterns('D'));
			arrayList.AddRange(this.GetAllRawDateTimePatterns('g'));
			arrayList.AddRange(this.GetAllRawDateTimePatterns('G'));
			arrayList.AddRange(this.GetAllRawDateTimePatterns('f'));
			arrayList.AddRange(this.GetAllRawDateTimePatterns('F'));
			arrayList.AddRange(this.GetAllRawDateTimePatterns('m'));
			arrayList.AddRange(this.GetAllRawDateTimePatterns('M'));
			arrayList.AddRange(this.GetAllRawDateTimePatterns('r'));
			arrayList.AddRange(this.GetAllRawDateTimePatterns('R'));
			arrayList.AddRange(this.GetAllRawDateTimePatterns('s'));
			arrayList.AddRange(this.GetAllRawDateTimePatterns('t'));
			arrayList.AddRange(this.GetAllRawDateTimePatterns('T'));
			arrayList.AddRange(this.GetAllRawDateTimePatterns('u'));
			arrayList.AddRange(this.GetAllRawDateTimePatterns('U'));
			arrayList.AddRange(this.GetAllRawDateTimePatterns('y'));
			arrayList.AddRange(this.GetAllRawDateTimePatterns('Y'));
			this.all_date_time_patterns = (string[])arrayList.ToArray(typeof(string));
		}

		/// <summary>Returns all the standard patterns in which date and time values can be formatted using the specified format pattern.</summary>
		/// <returns>An array containing the standard patterns in which date and time values can be formatted using the specified format pattern.</returns>
		/// <param name="format">A standard format pattern. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="format" /> is not a valid standard format pattern. </exception>
		public string[] GetAllDateTimePatterns(char format)
		{
			return (string[])this.GetAllRawDateTimePatterns(format).Clone();
		}

		internal string[] GetAllRawDateTimePatterns(char format)
		{
			string[] array;
			switch (format)
			{
			case 'R':
				goto IL_2CB;
			default:
				switch (format)
				{
				case 'r':
					goto IL_2CB;
				case 's':
					return new string[]
					{
						this.SortableDateTimePattern
					};
				case 't':
					if (this.allShortTimePatterns != null && this.allShortTimePatterns.Length > 0)
					{
						return this.allShortTimePatterns;
					}
					return new string[]
					{
						this.ShortTimePattern
					};
				case 'u':
					return new string[]
					{
						this.UniversalSortableDateTimePattern
					};
				default:
					switch (format)
					{
					case 'D':
						if (this.allLongDatePatterns != null && this.allLongDatePatterns.Length > 0)
						{
							return this.allLongDatePatterns;
						}
						return new string[]
						{
							this.LongDatePattern
						};
					default:
						switch (format)
						{
						case 'd':
							if (this.allShortDatePatterns != null && this.allShortDatePatterns.Length > 0)
							{
								return this.allShortDatePatterns;
							}
							return new string[]
							{
								this.ShortDatePattern
							};
						default:
							if (format != 'M' && format != 'm')
							{
								throw new ArgumentException("Format specifier was invalid.");
							}
							if (this.monthDayPatterns != null && this.monthDayPatterns.Length > 0)
							{
								return this.monthDayPatterns;
							}
							return new string[]
							{
								this.MonthDayPattern
							};
						case 'f':
							array = this.PopulateCombinedList(this.allLongDatePatterns, this.allShortTimePatterns);
							if (array != null && array.Length > 0)
							{
								return array;
							}
							return new string[]
							{
								this.LongDatePattern + " " + this.ShortTimePattern
							};
						case 'g':
							array = this.PopulateCombinedList(this.allShortDatePatterns, this.allShortTimePatterns);
							if (array != null && array.Length > 0)
							{
								return array;
							}
							return new string[]
							{
								this.ShortDatePattern + " " + this.ShortTimePattern
							};
						}
						break;
					case 'F':
						break;
					case 'G':
						array = this.PopulateCombinedList(this.allShortDatePatterns, this.allLongTimePatterns);
						if (array != null && array.Length > 0)
						{
							return array;
						}
						return new string[]
						{
							this.ShortDatePattern + " " + this.LongTimePattern
						};
					}
					break;
				case 'y':
					goto IL_29B;
				}
				break;
			case 'T':
				if (this.allLongTimePatterns != null && this.allLongTimePatterns.Length > 0)
				{
					return this.allLongTimePatterns;
				}
				return new string[]
				{
					this.LongTimePattern
				};
			case 'U':
				break;
			case 'Y':
				goto IL_29B;
			}
			array = this.PopulateCombinedList(this.allLongDatePatterns, this.allLongTimePatterns);
			if (array != null && array.Length > 0)
			{
				return array;
			}
			return new string[]
			{
				this.LongDatePattern + " " + this.LongTimePattern
			};
			IL_29B:
			if (this.yearMonthPatterns != null && this.yearMonthPatterns.Length > 0)
			{
				return this.yearMonthPatterns;
			}
			return new string[]
			{
				this.YearMonthPattern
			};
			IL_2CB:
			return new string[]
			{
				this.RFC1123Pattern
			};
		}

		/// <summary>Returns the culture-specific full name of the specified day of the week based on the culture associated with the current <see cref="T:System.Globalization.DateTimeFormatInfo" /> object.</summary>
		/// <returns>The culture-specific full name of the day of the week represented by <paramref name="dayofweek" />.</returns>
		/// <param name="dayofweek">A <see cref="T:System.DayOfWeek" /> value. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="dayofweek" /> is not a valid <see cref="T:System.DayOfWeek" /> value. </exception>
		public string GetDayName(DayOfWeek dayofweek)
		{
			if (dayofweek < DayOfWeek.Sunday || dayofweek > DayOfWeek.Saturday)
			{
				throw new ArgumentOutOfRangeException();
			}
			return this.dayNames[(int)dayofweek];
		}

		/// <summary>Returns the culture-specific abbreviated name of the specified day of the week based on the culture associated with the current <see cref="T:System.Globalization.DateTimeFormatInfo" /> object.</summary>
		/// <returns>The culture-specific abbreviated name of the day of the week represented by <paramref name="dayofweek" />.</returns>
		/// <param name="dayofweek">A <see cref="T:System.DayOfWeek" /> value. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="dayofweek" /> is not a valid <see cref="T:System.DayOfWeek" /> value. </exception>
		public string GetAbbreviatedDayName(DayOfWeek dayofweek)
		{
			if (dayofweek < DayOfWeek.Sunday || dayofweek > DayOfWeek.Saturday)
			{
				throw new ArgumentOutOfRangeException();
			}
			return this.abbreviatedDayNames[(int)dayofweek];
		}

		private void FillInvariantPatterns()
		{
			this.allShortDatePatterns = new string[]
			{
				"MM/dd/yyyy"
			};
			this.allLongDatePatterns = new string[]
			{
				"dddd, dd MMMM yyyy"
			};
			this.allLongTimePatterns = new string[]
			{
				"HH:mm:ss"
			};
			this.allShortTimePatterns = new string[]
			{
				"HH:mm",
				"hh:mm tt",
				"H:mm",
				"h:mm tt"
			};
			this.monthDayPatterns = new string[]
			{
				"MMMM dd"
			};
			this.yearMonthPatterns = new string[]
			{
				"yyyy MMMM"
			};
		}

		private string[] PopulateCombinedList(string[] dates, string[] times)
		{
			if (dates != null && times != null)
			{
				string[] array = new string[dates.Length * times.Length];
				int num = 0;
				foreach (string str in dates)
				{
					foreach (string str2 in times)
					{
						array[num++] = str + " " + str2;
					}
				}
				return array;
			}
			return null;
		}

		/// <summary>Gets or sets a string array of abbreviated month names associated with the current <see cref="T:System.Globalization.DateTimeFormatInfo" /> object.</summary>
		/// <returns>A string array of abbreviated month names.</returns>
		/// <exception cref="T:System.ArgumentNullException">In a set operation, the value array or one of the elements of the value array is null.</exception>
		[MonoTODO("Returns only the English month abbreviated names")]
		[ComVisible(false)]
		public string[] AbbreviatedMonthGenitiveNames
		{
			get
			{
				return this.m_genitiveAbbreviatedMonthNames;
			}
			set
			{
				this.m_genitiveAbbreviatedMonthNames = value;
			}
		}

		/// <summary>Gets or sets a string array of month names associated with the current <see cref="T:System.Globalization.DateTimeFormatInfo" /> object.</summary>
		/// <returns>A string array of month names.</returns>
		/// <exception cref="T:System.ArgumentNullException">In a set operation, the value array or one of the elements of the value array is null.</exception>
		[MonoTODO("Returns only the English moth names")]
		[ComVisible(false)]
		public string[] MonthGenitiveNames
		{
			get
			{
				return this.genitiveMonthNames;
			}
			set
			{
				this.genitiveMonthNames = value;
			}
		}

		/// <summary>Gets the native name of the calendar associated with the current <see cref="T:System.Globalization.DateTimeFormatInfo" /> object.</summary>
		/// <returns>The native name of the calendar used in the culture associated with the current <see cref="T:System.Globalization.DateTimeFormatInfo" /> object if that name is available, or the empty string ("") if the native calendar name is not available.</returns>
		[ComVisible(false)]
		[MonoTODO("Returns an empty string as if the calendar name wasn't available")]
		public string NativeCalendarName
		{
			get
			{
				return string.Empty;
			}
		}

		/// <summary>Gets or sets a string array of the shortest unique abbreviated day names associated with the current <see cref="T:System.Globalization.DateTimeFormatInfo" /> object.</summary>
		/// <returns>A string array of day names.</returns>
		/// <exception cref="T:System.ArgumentNullException">In a set operation, the value array or one of the elements of the value array is null.</exception>
		[ComVisible(false)]
		public string[] ShortestDayNames
		{
			get
			{
				return this.shortDayNames;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				if (value.Length != 7)
				{
					throw new ArgumentException("Array must have 7 entries");
				}
				for (int i = 0; i < 7; i++)
				{
					if (value[i] == null)
					{
						throw new ArgumentNullException(string.Format("Element {0} is null", i));
					}
				}
				this.shortDayNames = value;
			}
		}

		/// <summary>Obtains the shortest abbreviated day name for a specified day of the week associated with the current <see cref="T:System.Globalization.DateTimeFormatInfo" /> object.</summary>
		/// <returns>The abbreviated name of the week that corresponds to the <paramref name="dayOfWeek" /> parameter.</returns>
		/// <param name="dayOfWeek">One of the <see cref="T:System.DayOfWeek" /> values.</param>
		[ComVisible(false)]
		public string GetShortestDayName(DayOfWeek dayOfWeek)
		{
			if (dayOfWeek < DayOfWeek.Sunday || dayOfWeek > DayOfWeek.Saturday)
			{
				throw new ArgumentOutOfRangeException();
			}
			return this.shortDayNames[(int)dayOfWeek];
		}

		/// <summary>Sets all the custom date and time format strings that correspond to a specified standard format string.</summary>
		/// <param name="patterns">An array of custom format strings.</param>
		/// <param name="format">The standard format string associated with the custom format strings specified in the <paramref name="patterns" /> parameter. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="patterns" /> is a zero-length array.-or-<paramref name="format" /> is not a valid standard format string or is a standard format string whose patterns cannot be set.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="patterns" /> is null.-or-<paramref name="patterns" /> has an array element whose value is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">This <see cref="T:System.Globalization.DateTimeFormatInfo" /> object is read-only.</exception>
		[ComVisible(false)]
		public void SetAllDateTimePatterns(string[] patterns, char format)
		{
			if (patterns == null)
			{
				throw new ArgumentNullException("patterns");
			}
			if (patterns.Length == 0)
			{
				throw new ArgumentException("patterns", "The argument patterns must not be of zero-length");
			}
			if (format != 'D')
			{
				if (format != 'M')
				{
					if (format != 'T')
					{
						if (format != 'Y')
						{
							if (format == 'd')
							{
								this.allShortDatePatterns = patterns;
								return;
							}
							if (format == 'm')
							{
								goto IL_7C;
							}
							if (format == 't')
							{
								this.allShortTimePatterns = patterns;
								return;
							}
							if (format != 'y')
							{
								throw new ArgumentException("format", "Format specifier is invalid");
							}
						}
						this.yearMonthPatterns = patterns;
						return;
					}
					this.allLongTimePatterns = patterns;
					return;
				}
				IL_7C:
				this.monthDayPatterns = patterns;
			}
			else
			{
				this.allLongDatePatterns = patterns;
			}
		}
	}
}
