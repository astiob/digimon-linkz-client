using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Globalization
{
	/// <summary>Provides information about a specific culture (called a "locale" for unmanaged code development). The information includes the names for the culture, the writing system, the calendar used, and formatting for dates and sort strings.</summary>
	[ComVisible(true)]
	[Serializable]
	public class CultureInfo : ICloneable, IFormatProvider
	{
		private const int NumOptionalCalendars = 5;

		private const int GregorianTypeMask = 16777215;

		private const int CalendarTypeBits = 24;

		private const int InvariantCultureId = 127;

		private static volatile CultureInfo invariant_culture_info;

		private static object shared_table_lock = new object();

		internal static int BootstrapCultureID;

		private bool m_isReadOnly;

		private int cultureID;

		[NonSerialized]
		private int parent_lcid;

		[NonSerialized]
		private int specific_lcid;

		[NonSerialized]
		private int datetime_index;

		[NonSerialized]
		private int number_index;

		private bool m_useUserOverride;

		[NonSerialized]
		private volatile NumberFormatInfo numInfo;

		private volatile DateTimeFormatInfo dateTimeInfo;

		private volatile TextInfo textInfo;

		private string m_name;

		[NonSerialized]
		private string displayname;

		[NonSerialized]
		private string englishname;

		[NonSerialized]
		private string nativename;

		[NonSerialized]
		private string iso3lang;

		[NonSerialized]
		private string iso2lang;

		[NonSerialized]
		private string icu_name;

		[NonSerialized]
		private string win3lang;

		[NonSerialized]
		private string territory;

		private volatile CompareInfo compareInfo;

		[NonSerialized]
		private unsafe readonly int* calendar_data;

		[NonSerialized]
		private unsafe readonly void* textinfo_data;

		[NonSerialized]
		private Calendar[] optional_calendars;

		[NonSerialized]
		private CultureInfo parent_culture;

		private int m_dataItem;

		private Calendar calendar;

		[NonSerialized]
		private bool constructed;

		[NonSerialized]
		internal byte[] cached_serialized_form;

		private static readonly string MSG_READONLY = "This instance is read only";

		private static Hashtable shared_by_number;

		private static Hashtable shared_by_name;

		/// <summary>Initializes a new instance of the <see cref="T:System.Globalization.CultureInfo" /> class based on the culture specified by the culture identifier.</summary>
		/// <param name="culture">A predefined <see cref="T:System.Globalization.CultureInfo" /> identifier, <see cref="P:System.Globalization.CultureInfo.LCID" /> property of an existing <see cref="T:System.Globalization.CultureInfo" /> object, or Windows-only culture identifier. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="culture" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="culture" /> is not a valid culture identifier. -or-In .NET Compact Framework applications, <paramref name="culture" /> is not supported by the operating system of the device. </exception>
		public CultureInfo(int culture) : this(culture, true)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Globalization.CultureInfo" /> class based on the culture specified by the culture identifier and on the Boolean that specifies whether to use the user-selected culture settings from the system.</summary>
		/// <param name="culture">A predefined <see cref="T:System.Globalization.CultureInfo" /> identifier, <see cref="P:System.Globalization.CultureInfo.LCID" /> property of an existing <see cref="T:System.Globalization.CultureInfo" /> object, or Windows-only culture identifier. </param>
		/// <param name="useUserOverride">A Boolean that denotes whether to use the user-selected culture settings (true) or the default culture settings (false). </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="culture" /> is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="culture" /> is not a valid culture identifier.-or-In .NET Compact Framework applications, <paramref name="culture" /> is not supported by the operating system of the device.  </exception>
		public CultureInfo(int culture, bool useUserOverride) : this(culture, useUserOverride, false)
		{
		}

		private CultureInfo(int culture, bool useUserOverride, bool read_only)
		{
			if (culture < 0)
			{
				throw new ArgumentOutOfRangeException("culture", "Positive number required.");
			}
			this.constructed = true;
			this.m_isReadOnly = read_only;
			this.m_useUserOverride = useUserOverride;
			if (culture == 127)
			{
				this.ConstructInvariant(read_only);
				return;
			}
			if (!this.ConstructInternalLocaleFromLcid(culture))
			{
				throw new ArgumentException(string.Format("Culture ID {0} (0x{0:X4}) is not a supported culture.", culture), "culture");
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Globalization.CultureInfo" /> class based on the culture specified by name.</summary>
		/// <param name="name">A predefined <see cref="T:System.Globalization.CultureInfo" /> name, <see cref="P:System.Globalization.CultureInfo.Name" /> of an existing <see cref="T:System.Globalization.CultureInfo" />, or Windows-only culture name. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> is not a valid culture name. -or-In .NET Compact Framework applications, <paramref name="culture" /> is not supported by the operating system of the device.</exception>
		public CultureInfo(string name) : this(name, true)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Globalization.CultureInfo" /> class based on the culture specified by name and on the Boolean that specifies whether to use the user-selected culture settings from the system.</summary>
		/// <param name="name">A predefined <see cref="T:System.Globalization.CultureInfo" /> name, <see cref="P:System.Globalization.CultureInfo.Name" /> of an existing <see cref="T:System.Globalization.CultureInfo" />, or Windows-only culture name. </param>
		/// <param name="useUserOverride">A Boolean that denotes whether to use the user-selected culture settings (true) or the default culture settings (false). </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> is not a valid culture name. -or-In .NET Compact Framework applications, <paramref name="culture" /> is not supported by the operating system of the device.</exception>
		public CultureInfo(string name, bool useUserOverride) : this(name, useUserOverride, false)
		{
		}

		private CultureInfo(string name, bool useUserOverride, bool read_only)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.constructed = true;
			this.m_isReadOnly = read_only;
			this.m_useUserOverride = useUserOverride;
			if (name.Length == 0)
			{
				this.ConstructInvariant(read_only);
				return;
			}
			if (!this.ConstructInternalLocaleFromName(name.ToLowerInvariant()))
			{
				throw new ArgumentException("Culture name " + name + " is not supported.", "name");
			}
		}

		private CultureInfo()
		{
			this.constructed = true;
		}

		static CultureInfo()
		{
			CultureInfo.invariant_culture_info = new CultureInfo(127, false, true);
		}

		/// <summary>Gets the <see cref="T:System.Globalization.CultureInfo" /> that is culture-independent (invariant).</summary>
		/// <returns>The <see cref="T:System.Globalization.CultureInfo" /> that is culture-independent (invariant).</returns>
		public static CultureInfo InvariantCulture
		{
			get
			{
				return CultureInfo.invariant_culture_info;
			}
		}

		/// <summary>Creates a <see cref="T:System.Globalization.CultureInfo" /> object that represents the specific culture that is associated with the specified name.</summary>
		/// <returns>A <see cref="T:System.Globalization.CultureInfo" /> object that represents:The invariant culture, if <paramref name="name" /> is an empty string ("").-or- The specific culture associated with <paramref name="name" />, if <paramref name="name" /> is a neutral culture.-or- The culture specified by <paramref name="name" />, if <paramref name="name" /> is already a specific culture.</returns>
		/// <param name="name">A predefined <see cref="T:System.Globalization.CultureInfo" /> name or the name of an existing <see cref="T:System.Globalization.CultureInfo" /> object. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> is not a valid culture name.-or- The culture specified by <paramref name="name" /> does not have a specific culture associated with it. </exception>
		/// <exception cref="T:System.NullReferenceException">
		///   <paramref name="name" /> is null. </exception>
		public static CultureInfo CreateSpecificCulture(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name == string.Empty)
			{
				return CultureInfo.InvariantCulture;
			}
			CultureInfo cultureInfo = new CultureInfo();
			if (!CultureInfo.ConstructInternalLocaleFromSpecificName(cultureInfo, name.ToLowerInvariant()))
			{
				throw new ArgumentException("Culture name " + name + " is not supported.", name);
			}
			return cultureInfo;
		}

		/// <summary>Gets the <see cref="T:System.Globalization.CultureInfo" /> that represents the culture used by the current thread.</summary>
		/// <returns>The <see cref="T:System.Globalization.CultureInfo" /> that represents the culture used by the current thread.</returns>
		public static CultureInfo CurrentCulture
		{
			get
			{
				return Thread.CurrentThread.CurrentCulture;
			}
		}

		/// <summary>Gets the <see cref="T:System.Globalization.CultureInfo" /> that represents the current culture used by the Resource Manager to look up culture-specific resources at run time.</summary>
		/// <returns>The <see cref="T:System.Globalization.CultureInfo" /> that represents the current culture used by the Resource Manager to look up culture-specific resources at run time.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static CultureInfo CurrentUICulture
		{
			get
			{
				return Thread.CurrentThread.CurrentUICulture;
			}
		}

		internal static CultureInfo ConstructCurrentCulture()
		{
			CultureInfo cultureInfo = new CultureInfo();
			if (!CultureInfo.ConstructInternalLocaleFromCurrentLocale(cultureInfo))
			{
				cultureInfo = CultureInfo.InvariantCulture;
			}
			CultureInfo.BootstrapCultureID = cultureInfo.cultureID;
			return cultureInfo;
		}

		internal static CultureInfo ConstructCurrentUICulture()
		{
			return CultureInfo.ConstructCurrentCulture();
		}

		internal string Territory
		{
			get
			{
				return this.territory;
			}
		}

		/// <summary>Gets the culture identifier for the current <see cref="T:System.Globalization.CultureInfo" />.</summary>
		/// <returns>The culture identifier for the current <see cref="T:System.Globalization.CultureInfo" />.</returns>
		public virtual int LCID
		{
			get
			{
				return this.cultureID;
			}
		}

		/// <summary>Gets the culture name in the format "&lt;languagecode2&gt;-&lt;country/regioncode2&gt;".</summary>
		/// <returns>The culture name in the format "&lt;languagecode2&gt;-&lt;country/regioncode2&gt;", where &lt;languagecode2&gt; is a lowercase two-letter code derived from ISO 639-1 and &lt;country/regioncode2&gt; is an uppercase two-letter code derived from ISO 3166.</returns>
		public virtual string Name
		{
			get
			{
				return this.m_name;
			}
		}

		/// <summary>Gets the culture name, consisting of the language, the country/region, and the optional script, that the culture is set to display.</summary>
		/// <returns>The culture name. consisting of the full name of the language, the full name of the country/region, and the optional script. The format is discussed in the description of the <see cref="T:System.Globalization.CultureInfo" /> class.</returns>
		public virtual string NativeName
		{
			get
			{
				if (!this.constructed)
				{
					this.Construct();
				}
				return this.nativename;
			}
		}

		/// <summary>Gets the default calendar used by the culture.</summary>
		/// <returns>A <see cref="T:System.Globalization.Calendar" /> that represents the default calendar used by the culture.</returns>
		public virtual Calendar Calendar
		{
			get
			{
				return this.DateTimeFormat.Calendar;
			}
		}

		/// <summary>Gets the list of calendars that can be used by the culture.</summary>
		/// <returns>An array of type <see cref="T:System.Globalization.Calendar" /> that represents the calendars that can be used by the culture represented by the current <see cref="T:System.Globalization.CultureInfo" />.</returns>
		public virtual Calendar[] OptionalCalendars
		{
			get
			{
				if (this.optional_calendars == null)
				{
					lock (this)
					{
						if (this.optional_calendars == null)
						{
							this.ConstructCalendars();
						}
					}
				}
				return this.optional_calendars;
			}
		}

		/// <summary>Gets the <see cref="T:System.Globalization.CultureInfo" /> that represents the parent culture of the current <see cref="T:System.Globalization.CultureInfo" />.</summary>
		/// <returns>The <see cref="T:System.Globalization.CultureInfo" /> that represents the parent culture of the current <see cref="T:System.Globalization.CultureInfo" />.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public virtual CultureInfo Parent
		{
			get
			{
				if (this.parent_culture == null)
				{
					if (!this.constructed)
					{
						this.Construct();
					}
					if (this.parent_lcid == this.cultureID)
					{
						return null;
					}
					if (this.parent_lcid == 127)
					{
						this.parent_culture = CultureInfo.InvariantCulture;
					}
					else if (this.cultureID == 127)
					{
						this.parent_culture = this;
					}
					else
					{
						this.parent_culture = new CultureInfo(this.parent_lcid);
					}
				}
				return this.parent_culture;
			}
		}

		/// <summary>Gets the <see cref="T:System.Globalization.TextInfo" /> that defines the writing system associated with the culture.</summary>
		/// <returns>The <see cref="T:System.Globalization.TextInfo" /> that defines the writing system associated with the culture.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public virtual TextInfo TextInfo
		{
			get
			{
				if (this.textInfo == null)
				{
					if (!this.constructed)
					{
						this.Construct();
					}
					lock (this)
					{
						if (this.textInfo == null)
						{
							this.textInfo = this.CreateTextInfo(this.m_isReadOnly);
						}
					}
				}
				return this.textInfo;
			}
		}

		/// <summary>Gets the ISO 639-2 three-letter code for the language of the current <see cref="T:System.Globalization.CultureInfo" />.</summary>
		/// <returns>The ISO 639-2 three-letter code for the language of the current <see cref="T:System.Globalization.CultureInfo" />.</returns>
		public virtual string ThreeLetterISOLanguageName
		{
			get
			{
				if (!this.constructed)
				{
					this.Construct();
				}
				return this.iso3lang;
			}
		}

		/// <summary>Gets the three-letter code for the language as defined in the Windows API.</summary>
		/// <returns>The three-letter code for the language as defined in the Windows API.</returns>
		public virtual string ThreeLetterWindowsLanguageName
		{
			get
			{
				if (!this.constructed)
				{
					this.Construct();
				}
				return this.win3lang;
			}
		}

		/// <summary>Gets the ISO 639-1 two-letter code for the language of the current <see cref="T:System.Globalization.CultureInfo" />.</summary>
		/// <returns>The ISO 639-1 two-letter code for the language of the current <see cref="T:System.Globalization.CultureInfo" />.</returns>
		public virtual string TwoLetterISOLanguageName
		{
			get
			{
				if (!this.constructed)
				{
					this.Construct();
				}
				return this.iso2lang;
			}
		}

		/// <summary>Gets a value indicating whether the current <see cref="T:System.Globalization.CultureInfo" /> uses the user-selected culture settings.</summary>
		/// <returns>true if the current <see cref="T:System.Globalization.CultureInfo" /> uses the user-selected culture settings; otherwise, false.</returns>
		public bool UseUserOverride
		{
			get
			{
				return this.m_useUserOverride;
			}
		}

		internal string IcuName
		{
			get
			{
				if (!this.constructed)
				{
					this.Construct();
				}
				return this.icu_name;
			}
		}

		/// <summary>Refreshes cached culture-related information.</summary>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public void ClearCachedData()
		{
			Thread.CurrentThread.CurrentCulture = null;
			Thread.CurrentThread.CurrentUICulture = null;
		}

		/// <summary>Creates a copy of the current <see cref="T:System.Globalization.CultureInfo" />.</summary>
		/// <returns>A copy of the current <see cref="T:System.Globalization.CultureInfo" />.</returns>
		public virtual object Clone()
		{
			if (!this.constructed)
			{
				this.Construct();
			}
			CultureInfo cultureInfo = (CultureInfo)base.MemberwiseClone();
			cultureInfo.m_isReadOnly = false;
			cultureInfo.cached_serialized_form = null;
			if (!this.IsNeutralCulture)
			{
				cultureInfo.NumberFormat = (NumberFormatInfo)this.NumberFormat.Clone();
				cultureInfo.DateTimeFormat = (DateTimeFormatInfo)this.DateTimeFormat.Clone();
			}
			return cultureInfo;
		}

		/// <summary>Determines whether the specified object is the same culture as the current <see cref="T:System.Globalization.CultureInfo" />.</summary>
		/// <returns>true if <paramref name="value" /> is the same culture as the current <see cref="T:System.Globalization.CultureInfo" />; otherwise, false.</returns>
		/// <param name="value">The object to compare with the current <see cref="T:System.Globalization.CultureInfo" />. </param>
		public override bool Equals(object value)
		{
			CultureInfo cultureInfo = value as CultureInfo;
			return cultureInfo != null && cultureInfo.cultureID == this.cultureID;
		}

		/// <summary>Gets the list of supported cultures filtered by the specified <see cref="T:System.Globalization.CultureTypes" /> parameter.</summary>
		/// <returns>An array of type <see cref="T:System.Globalization.CultureInfo" /> that contains the cultures specified by the <paramref name="types" /> parameter. The array of cultures is unsorted.</returns>
		/// <param name="types">A bitwise combination of <see cref="T:System.Globalization.CultureTypes" /> values that filter the cultures to retrieve. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="types" /> specifies an invalid combination of <see cref="T:System.Globalization.CultureTypes" /> values.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static CultureInfo[] GetCultures(CultureTypes types)
		{
			bool flag = (types & CultureTypes.NeutralCultures) != (CultureTypes)0;
			bool specific = (types & CultureTypes.SpecificCultures) != (CultureTypes)0;
			bool installed = (types & CultureTypes.InstalledWin32Cultures) != (CultureTypes)0;
			CultureInfo[] array = CultureInfo.internal_get_cultures(flag, specific, installed);
			if (flag && array.Length > 0 && array[0] == null)
			{
				array[0] = (CultureInfo)CultureInfo.InvariantCulture.Clone();
			}
			return array;
		}

		/// <summary>Serves as a hash function for the current <see cref="T:System.Globalization.CultureInfo" />, suitable for hashing algorithms and data structures, such as a hash table.</summary>
		/// <returns>A hash code for the current <see cref="T:System.Globalization.CultureInfo" />.</returns>
		public override int GetHashCode()
		{
			return this.cultureID;
		}

		/// <summary>Returns a read-only wrapper around the specified <see cref="T:System.Globalization.CultureInfo" />.</summary>
		/// <returns>A read-only <see cref="T:System.Globalization.CultureInfo" /> wrapper around <paramref name="ci" />.</returns>
		/// <param name="ci">The <see cref="T:System.Globalization.CultureInfo" /> to wrap. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="ci" /> is null. </exception>
		public static CultureInfo ReadOnly(CultureInfo ci)
		{
			if (ci == null)
			{
				throw new ArgumentNullException("ci");
			}
			if (ci.m_isReadOnly)
			{
				return ci;
			}
			CultureInfo cultureInfo = (CultureInfo)ci.Clone();
			cultureInfo.m_isReadOnly = true;
			if (cultureInfo.numInfo != null)
			{
				cultureInfo.numInfo = NumberFormatInfo.ReadOnly(cultureInfo.numInfo);
			}
			if (cultureInfo.dateTimeInfo != null)
			{
				cultureInfo.dateTimeInfo = DateTimeFormatInfo.ReadOnly(cultureInfo.dateTimeInfo);
			}
			if (cultureInfo.textInfo != null)
			{
				cultureInfo.textInfo = TextInfo.ReadOnly(cultureInfo.textInfo);
			}
			return cultureInfo;
		}

		/// <summary>Returns a string containing the name of the current <see cref="T:System.Globalization.CultureInfo" /> in the format "&lt;languagecode2&gt;-&lt;country/regioncode2&gt;".</summary>
		/// <returns>A string containing the name of the current <see cref="T:System.Globalization.CultureInfo" />.</returns>
		public override string ToString()
		{
			return this.m_name;
		}

		/// <summary>Gets the <see cref="T:System.Globalization.CompareInfo" /> that defines how to compare strings for the culture.</summary>
		/// <returns>The <see cref="T:System.Globalization.CompareInfo" /> that defines how to compare strings for the culture.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public virtual CompareInfo CompareInfo
		{
			get
			{
				if (this.compareInfo == null)
				{
					if (!this.constructed)
					{
						this.Construct();
					}
					lock (this)
					{
						if (this.compareInfo == null)
						{
							this.compareInfo = new CompareInfo(this);
						}
					}
				}
				return this.compareInfo;
			}
		}

		internal static bool IsIDNeutralCulture(int lcid)
		{
			bool result;
			if (!CultureInfo.internal_is_lcid_neutral(lcid, out result))
			{
				throw new ArgumentException(string.Format("Culture id 0x{:x4} is not supported.", lcid));
			}
			return result;
		}

		/// <summary>Gets a value indicating whether the current <see cref="T:System.Globalization.CultureInfo" /> represents a neutral culture.</summary>
		/// <returns>true if the current <see cref="T:System.Globalization.CultureInfo" /> represents a neutral culture; otherwise, false.</returns>
		public virtual bool IsNeutralCulture
		{
			get
			{
				if (!this.constructed)
				{
					this.Construct();
				}
				return this.cultureID != 127 && ((this.cultureID & 65280) == 0 || this.specific_lcid == 0);
			}
		}

		internal void CheckNeutral()
		{
			if (this.IsNeutralCulture)
			{
				throw new NotSupportedException("Culture \"" + this.m_name + "\" is a neutral culture. It can not be used in formatting and parsing and therefore cannot be set as the thread's current culture.");
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Globalization.NumberFormatInfo" /> that defines the culturally appropriate format of displaying numbers, currency, and percentage.</summary>
		/// <returns>A <see cref="T:System.Globalization.NumberFormatInfo" /> that defines the culturally appropriate format of displaying numbers, currency, and percentage.</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is set to null. </exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Globalization.CultureInfo" /> is for a neutral culture. </exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Globalization.CultureInfo.NumberFormat" /> property or any of the <see cref="T:System.Globalization.NumberFormatInfo" /> properties is set, and the <see cref="T:System.Globalization.CultureInfo" /> is read-only. </exception>
		public virtual NumberFormatInfo NumberFormat
		{
			get
			{
				if (!this.constructed)
				{
					this.Construct();
				}
				this.CheckNeutral();
				if (this.numInfo == null)
				{
					lock (this)
					{
						if (this.numInfo == null)
						{
							this.numInfo = new NumberFormatInfo(this.m_isReadOnly);
							this.construct_number_format();
						}
					}
				}
				return this.numInfo;
			}
			set
			{
				if (!this.constructed)
				{
					this.Construct();
				}
				if (this.m_isReadOnly)
				{
					throw new InvalidOperationException(CultureInfo.MSG_READONLY);
				}
				if (value == null)
				{
					throw new ArgumentNullException("NumberFormat");
				}
				this.numInfo = value;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Globalization.DateTimeFormatInfo" /> that defines the culturally appropriate format of displaying dates and times.</summary>
		/// <returns>A <see cref="T:System.Globalization.DateTimeFormatInfo" /> that defines the culturally appropriate format of displaying dates and times.</returns>
		/// <exception cref="T:System.ArgumentNullException">The property is set to null. </exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Globalization.CultureInfo" /> is for a neutral culture. </exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Globalization.CultureInfo.DateTimeFormat" /> property or any of the <see cref="T:System.Globalization.DateTimeFormatInfo" /> properties is set, and the <see cref="T:System.Globalization.CultureInfo" /> is read-only. </exception>
		public virtual DateTimeFormatInfo DateTimeFormat
		{
			get
			{
				if (!this.constructed)
				{
					this.Construct();
				}
				this.CheckNeutral();
				if (this.dateTimeInfo == null)
				{
					lock (this)
					{
						if (this.dateTimeInfo == null)
						{
							this.dateTimeInfo = new DateTimeFormatInfo(this.m_isReadOnly);
							this.construct_datetime_format();
							if (this.optional_calendars != null)
							{
								this.dateTimeInfo.Calendar = this.optional_calendars[0];
							}
						}
					}
				}
				return this.dateTimeInfo;
			}
			set
			{
				if (!this.constructed)
				{
					this.Construct();
				}
				if (this.m_isReadOnly)
				{
					throw new InvalidOperationException(CultureInfo.MSG_READONLY);
				}
				if (value == null)
				{
					throw new ArgumentNullException("DateTimeFormat");
				}
				this.dateTimeInfo = value;
			}
		}

		/// <summary>Gets the culture name in the format "&lt;languagefull&gt; (&lt;country/regionfull&gt;)" in the language of the localized version of .NET Framework.</summary>
		/// <returns>The culture name in the format "&lt;languagefull&gt; (&lt;country/regionfull&gt;)" in the language of the localized version of .NET Framework, where &lt;languagefull&gt; is the full name of the language and &lt;country/regionfull&gt; is the full name of the country/region.</returns>
		public virtual string DisplayName
		{
			get
			{
				if (!this.constructed)
				{
					this.Construct();
				}
				return this.displayname;
			}
		}

		/// <summary>Gets the culture name in the format "&lt;languagefull&gt; (&lt;country/regionfull&gt;)" in English.</summary>
		/// <returns>The culture name in the format "&lt;languagefull&gt; (&lt;country/regionfull&gt;)" in English, where &lt;languagefull&gt; is the full name of the language and &lt;country/regionfull&gt; is the full name of the country/region.</returns>
		public virtual string EnglishName
		{
			get
			{
				if (!this.constructed)
				{
					this.Construct();
				}
				return this.englishname;
			}
		}

		/// <summary>Gets the <see cref="T:System.Globalization.CultureInfo" /> that represents the culture installed with the operating system.</summary>
		/// <returns>The <see cref="T:System.Globalization.CultureInfo" /> that represents the culture installed with the operating system.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static CultureInfo InstalledUICulture
		{
			get
			{
				return CultureInfo.GetCultureInfo(CultureInfo.BootstrapCultureID);
			}
		}

		/// <summary>Gets a value indicating whether the current <see cref="T:System.Globalization.CultureInfo" /> is read-only.</summary>
		/// <returns>true if the current <see cref="T:System.Globalization.CultureInfo" /> is read-only; otherwise, false. The default is false.</returns>
		public bool IsReadOnly
		{
			get
			{
				return this.m_isReadOnly;
			}
		}

		/// <summary>Gets an object that defines how to format the specified type.</summary>
		/// <returns>The value of the <see cref="P:System.Globalization.CultureInfo.NumberFormat" /> property, which is a <see cref="T:System.Globalization.NumberFormatInfo" /> containing the default number format information for the current <see cref="T:System.Globalization.CultureInfo" />, if <paramref name="formatType" /> is the <see cref="T:System.Type" /> object for the <see cref="T:System.Globalization.NumberFormatInfo" /> class.-or- The value of the <see cref="P:System.Globalization.CultureInfo.DateTimeFormat" /> property, which is a <see cref="T:System.Globalization.DateTimeFormatInfo" /> containing the default date and time format information for the current <see cref="T:System.Globalization.CultureInfo" />, if <paramref name="formatType" /> is the <see cref="T:System.Type" /> object for the <see cref="T:System.Globalization.DateTimeFormatInfo" /> class.-or- null, if <paramref name="formatType" /> is any other object.</returns>
		/// <param name="formatType">The <see cref="T:System.Type" /> for which to get a formatting object. This method only supports the <see cref="T:System.Globalization.NumberFormatInfo" /> and <see cref="T:System.Globalization.DateTimeFormatInfo" /> types. </param>
		public virtual object GetFormat(Type formatType)
		{
			object result = null;
			if (formatType == typeof(NumberFormatInfo))
			{
				result = this.NumberFormat;
			}
			else if (formatType == typeof(DateTimeFormatInfo))
			{
				result = this.DateTimeFormat;
			}
			return result;
		}

		private void Construct()
		{
			this.construct_internal_locale_from_lcid(this.cultureID);
			this.constructed = true;
		}

		private bool ConstructInternalLocaleFromName(string locale)
		{
			string text = locale;
			if (text != null)
			{
				if (CultureInfo.<>f__switch$map19 == null)
				{
					CultureInfo.<>f__switch$map19 = new Dictionary<string, int>(2)
					{
						{
							"zh-hans",
							0
						},
						{
							"zh-hant",
							1
						}
					};
				}
				int num;
				if (CultureInfo.<>f__switch$map19.TryGetValue(text, out num))
				{
					if (num != 0)
					{
						if (num == 1)
						{
							locale = "zh-cht";
						}
					}
					else
					{
						locale = "zh-chs";
					}
				}
			}
			return this.construct_internal_locale_from_name(locale);
		}

		private bool ConstructInternalLocaleFromLcid(int lcid)
		{
			return this.construct_internal_locale_from_lcid(lcid);
		}

		private static bool ConstructInternalLocaleFromSpecificName(CultureInfo ci, string name)
		{
			return CultureInfo.construct_internal_locale_from_specific_name(ci, name);
		}

		private static bool ConstructInternalLocaleFromCurrentLocale(CultureInfo ci)
		{
			return CultureInfo.construct_internal_locale_from_current_locale(ci);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool construct_internal_locale_from_lcid(int lcid);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool construct_internal_locale_from_name(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool construct_internal_locale_from_specific_name(CultureInfo ci, string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool construct_internal_locale_from_current_locale(CultureInfo ci);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CultureInfo[] internal_get_cultures(bool neutral, bool specific, bool installed);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void construct_datetime_format();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void construct_number_format();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool internal_is_lcid_neutral(int lcid, out bool is_neutral);

		private void ConstructInvariant(bool read_only)
		{
			this.cultureID = 127;
			this.numInfo = NumberFormatInfo.InvariantInfo;
			this.dateTimeInfo = DateTimeFormatInfo.InvariantInfo;
			if (!read_only)
			{
				this.numInfo = (NumberFormatInfo)this.numInfo.Clone();
				this.dateTimeInfo = (DateTimeFormatInfo)this.dateTimeInfo.Clone();
			}
			this.textInfo = this.CreateTextInfo(read_only);
			this.m_name = string.Empty;
			this.displayname = (this.englishname = (this.nativename = "Invariant Language (Invariant Country)"));
			this.iso3lang = "IVL";
			this.iso2lang = "iv";
			this.icu_name = "en_US_POSIX";
			this.win3lang = "IVL";
		}

		private TextInfo CreateTextInfo(bool readOnly)
		{
			return new TextInfo(this, this.cultureID, this.textinfo_data, readOnly);
		}

		private static void insert_into_shared_tables(CultureInfo c)
		{
			if (CultureInfo.shared_by_number == null)
			{
				CultureInfo.shared_by_number = new Hashtable();
				CultureInfo.shared_by_name = new Hashtable();
			}
			CultureInfo.shared_by_number[c.cultureID] = c;
			CultureInfo.shared_by_name[c.m_name] = c;
		}

		/// <summary>Retrieves a cached, read-only instance of a culture using the specified culture identifier.</summary>
		/// <returns>A read-only <see cref="T:System.Globalization.CultureInfo" /> object.</returns>
		/// <param name="culture">A culture identifier.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="culture" /> is less than zero.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="culture" /> specifies a culture that is not supported.</exception>
		public static CultureInfo GetCultureInfo(int culture)
		{
			object obj = CultureInfo.shared_table_lock;
			CultureInfo result;
			lock (obj)
			{
				CultureInfo cultureInfo;
				if (CultureInfo.shared_by_number != null)
				{
					cultureInfo = (CultureInfo.shared_by_number[culture] as CultureInfo);
					if (cultureInfo != null)
					{
						return cultureInfo;
					}
				}
				cultureInfo = new CultureInfo(culture, false, true);
				CultureInfo.insert_into_shared_tables(cultureInfo);
				result = cultureInfo;
			}
			return result;
		}

		/// <summary>Retrieves a cached, read-only instance of a culture using the specified culture name. </summary>
		/// <returns>A read-only <see cref="T:System.Globalization.CultureInfo" /> object.</returns>
		/// <param name="name">The name of a culture.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> specifies a culture that is not supported.</exception>
		public static CultureInfo GetCultureInfo(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			object obj = CultureInfo.shared_table_lock;
			CultureInfo result;
			lock (obj)
			{
				CultureInfo cultureInfo;
				if (CultureInfo.shared_by_name != null)
				{
					cultureInfo = (CultureInfo.shared_by_name[name] as CultureInfo);
					if (cultureInfo != null)
					{
						return cultureInfo;
					}
				}
				cultureInfo = new CultureInfo(name, false, true);
				CultureInfo.insert_into_shared_tables(cultureInfo);
				result = cultureInfo;
			}
			return result;
		}

		/// <summary>Retrieves a cached, read-only instance of a culture. Parameters specify a culture that is initialized with the <see cref="T:System.Globalization.TextInfo" /> and <see cref="T:System.Globalization.CompareInfo" /> objects specified by another culture.</summary>
		/// <returns>A read-only <see cref="T:System.Globalization.CultureInfo" /> object.</returns>
		/// <param name="name">The name of a culture.</param>
		/// <param name="altName">The name of a culture that supplies the <see cref="T:System.Globalization.TextInfo" /> and <see cref="T:System.Globalization.CompareInfo" /> objects used to initialize <paramref name="name" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> or <paramref name="altName" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> or <paramref name="altName" /> specifies a culture that is not supported.</exception>
		[MonoTODO("Currently it ignores the altName parameter")]
		public static CultureInfo GetCultureInfo(string name, string altName)
		{
			if (name == null)
			{
				throw new ArgumentNullException("null");
			}
			if (altName == null)
			{
				throw new ArgumentNullException("null");
			}
			return CultureInfo.GetCultureInfo(name);
		}

		/// <summary>Deprecated. Retrieves a read-only <see cref="T:System.Globalization.CultureInfo" /> object having linguistic characteristics that are identified by the specified RFC 4646 language tag. </summary>
		/// <returns>A read-only <see cref="T:System.Globalization.CultureInfo" /> object.</returns>
		/// <param name="name">The name of a language as specified by the RFC 4646 standard.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> does not correspond to a supported culture.</exception>
		public static CultureInfo GetCultureInfoByIetfLanguageTag(string name)
		{
			if (name != null)
			{
				if (CultureInfo.<>f__switch$map1A == null)
				{
					CultureInfo.<>f__switch$map1A = new Dictionary<string, int>(2)
					{
						{
							"zh-Hans",
							0
						},
						{
							"zh-Hant",
							1
						}
					};
				}
				int num;
				if (CultureInfo.<>f__switch$map1A.TryGetValue(name, out num))
				{
					if (num == 0)
					{
						return CultureInfo.GetCultureInfo("zh-CHS");
					}
					if (num == 1)
					{
						return CultureInfo.GetCultureInfo("zh-CHT");
					}
				}
			}
			return CultureInfo.GetCultureInfo(name);
		}

		internal static CultureInfo CreateCulture(string name, bool reference)
		{
			bool flag = name.Length == 0;
			bool useUserOverride;
			bool read_only;
			if (reference)
			{
				useUserOverride = !flag;
				read_only = false;
			}
			else
			{
				read_only = false;
				useUserOverride = !flag;
			}
			return new CultureInfo(name, useUserOverride, read_only);
		}

		internal unsafe void ConstructCalendars()
		{
			if (this.calendar_data == null)
			{
				this.optional_calendars = new Calendar[]
				{
					new GregorianCalendar(GregorianCalendarTypes.Localized)
				};
				return;
			}
			this.optional_calendars = new Calendar[5];
			for (int i = 0; i < 5; i++)
			{
				int num = this.calendar_data[i];
				Calendar calendar;
				switch (num >> 24)
				{
				case 0:
				{
					GregorianCalendarTypes type = (GregorianCalendarTypes)(num & 16777215);
					calendar = new GregorianCalendar(type);
					break;
				}
				case 1:
					calendar = new HijriCalendar();
					break;
				case 2:
					calendar = new ThaiBuddhistCalendar();
					break;
				default:
					throw new Exception("invalid calendar type:  " + num);
				}
				this.optional_calendars[i] = calendar;
			}
		}
	}
}
