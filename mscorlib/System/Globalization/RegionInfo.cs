using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	/// <summary>Contains information about the country/region.</summary>
	[ComVisible(true)]
	[Serializable]
	public class RegionInfo
	{
		private static RegionInfo currentRegion;

		private int lcid;

		private int regionId;

		private string iso2Name;

		private string iso3Name;

		private string win3Name;

		private string englishName;

		private string currencySymbol;

		private string isoCurrencySymbol;

		private string currencyEnglishName;

		/// <summary>Initializes a new instance of the <see cref="T:System.Globalization.RegionInfo" /> class based on the country/region associated with the specified culture identifier.</summary>
		/// <param name="culture">A culture identifier. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="culture" /> specifies either an invariant, custom, or neutral culture.</exception>
		public RegionInfo(int culture)
		{
			if (!this.GetByTerritory(CultureInfo.GetCultureInfo(culture)))
			{
				throw new ArgumentException(string.Format("Region ID {0} (0x{0:X4}) is not a supported region.", culture), "culture");
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Globalization.RegionInfo" /> class based on the country/region or specific culture, specified by name.</summary>
		/// <param name="name">A string containing one of the two-letter codes defined in ISO 3166 for country/region.-or-Beginning in .NET Framework version 2.0, a string containing the culture name for a specific culture, custom culture, or Windows-only culture. If the culture name is not in RFC 4646 format, your application should specify the entire culture name, not just the country/region. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="name" /> is not a valid country/region name or specific culture name.</exception>
		public RegionInfo(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException();
			}
			if (this.construct_internal_region_from_name(name.ToUpperInvariant()))
			{
				this.lcid = name.GetHashCode();
				return;
			}
			if (!this.GetByTerritory(CultureInfo.GetCultureInfo(name)))
			{
				throw new ArgumentException(string.Format("Region name {0} is not supported.", name), "name");
			}
		}

		/// <summary>Gets the <see cref="T:System.Globalization.RegionInfo" /> that represents the country/region used by the current thread.</summary>
		/// <returns>The <see cref="T:System.Globalization.RegionInfo" /> that represents the country/region used by the current thread.</returns>
		public static RegionInfo CurrentRegion
		{
			get
			{
				if (RegionInfo.currentRegion == null)
				{
					CultureInfo currentCulture = CultureInfo.CurrentCulture;
					if (currentCulture == null || CultureInfo.BootstrapCultureID == 127)
					{
						return null;
					}
					RegionInfo.currentRegion = new RegionInfo(CultureInfo.BootstrapCultureID);
				}
				return RegionInfo.currentRegion;
			}
		}

		private bool GetByTerritory(CultureInfo ci)
		{
			if (ci == null)
			{
				throw new Exception("INTERNAL ERROR: should not happen.");
			}
			if (ci.IsNeutralCulture || ci.Territory == null)
			{
				return false;
			}
			this.lcid = ci.LCID;
			return this.construct_internal_region_from_name(ci.Territory.ToUpperInvariant());
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool construct_internal_region_from_name(string name);

		/// <summary>Gets the name, in English, of the currency used in the country/region.</summary>
		/// <returns>The name, in English, of the currency used in the country/region.</returns>
		[ComVisible(false)]
		public virtual string CurrencyEnglishName
		{
			get
			{
				return this.currencyEnglishName;
			}
		}

		/// <summary>Gets the currency symbol associated with the country/region.</summary>
		/// <returns>The currency symbol associated with the country/region.</returns>
		public virtual string CurrencySymbol
		{
			get
			{
				return this.currencySymbol;
			}
		}

		/// <summary>Gets the full name of the country/region in the language of the localized version of .NET Framework.</summary>
		/// <returns>The full name of the country/region in the language of the localized version of .NET Framework.</returns>
		[MonoTODO("DisplayName currently only returns the EnglishName")]
		public virtual string DisplayName
		{
			get
			{
				return this.englishName;
			}
		}

		/// <summary>Gets the full name of the country/region in English.</summary>
		/// <returns>The full name of the country/region in English.</returns>
		public virtual string EnglishName
		{
			get
			{
				return this.englishName;
			}
		}

		/// <summary>Gets a unique identification number for a geographical region, country, city, or location.</summary>
		/// <returns>A 32-bit signed number that uniquely identifies a geographical location.</returns>
		[ComVisible(false)]
		public virtual int GeoId
		{
			get
			{
				return this.regionId;
			}
		}

		/// <summary>Gets a value indicating whether the country/region uses the metric system for measurements.</summary>
		/// <returns>true if the country/region uses the metric system for measurements; otherwise, false.</returns>
		public virtual bool IsMetric
		{
			get
			{
				string text = this.iso2Name;
				if (text != null)
				{
					if (RegionInfo.<>f__switch$map1B == null)
					{
						RegionInfo.<>f__switch$map1B = new Dictionary<string, int>(2)
						{
							{
								"US",
								0
							},
							{
								"UK",
								0
							}
						};
					}
					int num;
					if (RegionInfo.<>f__switch$map1B.TryGetValue(text, out num))
					{
						if (num == 0)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		/// <summary>Gets the three-character ISO 4217 currency symbol associated with the country/region.</summary>
		/// <returns>The three-character ISO 4217 currency symbol associated with the country/region.</returns>
		public virtual string ISOCurrencySymbol
		{
			get
			{
				return this.isoCurrencySymbol;
			}
		}

		/// <summary>Gets the name of a country/region formatted in the native language of the country/region.</summary>
		/// <returns>The native name of the country/region formatted in the language associated with the ISO 3166 country/region code. </returns>
		[ComVisible(false)]
		public virtual string NativeName
		{
			get
			{
				return this.DisplayName;
			}
		}

		/// <summary>Gets the name of the currency used in the country/region, formatted in the native language of the country/region. </summary>
		/// <returns>The native name of the currency used in the country/region, formatted in the language associated with the ISO 3166 country/region code. </returns>
		[ComVisible(false)]
		[MonoTODO("Not implemented")]
		public virtual string CurrencyNativeName
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>Gets the name or ISO 3166 two-letter country/region code for the current <see cref="T:System.Globalization.RegionInfo" /> object.</summary>
		/// <returns>The value specified by the <paramref name="name" /> parameter of the <see cref="M:System.Globalization.RegionInfo.#ctor(System.String)" /> constructor. The return value is in uppercase.-or-The two-letter code defined in ISO 3166 for the country/region specified by the <paramref name="culture" /> parameter of the <see cref="M:System.Globalization.RegionInfo.#ctor(System.Int32)" /> constructor. The return value is in uppercase.</returns>
		public virtual string Name
		{
			get
			{
				return this.iso2Name;
			}
		}

		/// <summary>Gets the three-letter code defined in ISO 3166 for the country/region.</summary>
		/// <returns>The three-letter code defined in ISO 3166 for the country/region.</returns>
		public virtual string ThreeLetterISORegionName
		{
			get
			{
				return this.iso3Name;
			}
		}

		/// <summary>Gets the three-letter code assigned by Windows to the country/region represented by this <see cref="T:System.Globalization.RegionInfo" />.</summary>
		/// <returns>The three-letter code assigned by Windows to the country/region represented by this <see cref="T:System.Globalization.RegionInfo" />.</returns>
		public virtual string ThreeLetterWindowsRegionName
		{
			get
			{
				return this.win3Name;
			}
		}

		/// <summary>Gets the two-letter code defined in ISO 3166 for the country/region.</summary>
		/// <returns>The two-letter code defined in ISO 3166 for the country/region.</returns>
		public virtual string TwoLetterISORegionName
		{
			get
			{
				return this.iso2Name;
			}
		}

		/// <summary>Determines whether the specified object is the same instance as the current <see cref="T:System.Globalization.RegionInfo" />.</summary>
		/// <returns>true if the <paramref name="value" /> parameter is a <see cref="T:System.Globalization.RegionInfo" /> object and its <see cref="P:System.Globalization.RegionInfo.Name" /> property is the same as the <see cref="P:System.Globalization.RegionInfo.Name" /> property of the current <see cref="T:System.Globalization.RegionInfo" /> object; otherwise, false.</returns>
		/// <param name="value">The object to compare with the current <see cref="T:System.Globalization.RegionInfo" />. </param>
		public override bool Equals(object value)
		{
			RegionInfo regionInfo = value as RegionInfo;
			return regionInfo != null && this.lcid == regionInfo.lcid;
		}

		/// <summary>Serves as a hash function for the current <see cref="T:System.Globalization.RegionInfo" />, suitable for hashing algorithms and data structures, such as a hash table.</summary>
		/// <returns>A hash code for the current <see cref="T:System.Globalization.RegionInfo" />.</returns>
		public override int GetHashCode()
		{
			return (int)((ulong)int.MinValue + (ulong)((long)((long)this.regionId << 3)) + (ulong)((long)this.regionId));
		}

		/// <summary>Returns a string containing the culture name or ISO 3166 two-letter country/region codes specified for the current <see cref="T:System.Globalization.RegionInfo" />.</summary>
		/// <returns>A string containing the culture name or ISO 3166 two-letter country/region codes defined for the current <see cref="T:System.Globalization.RegionInfo" />.</returns>
		public override string ToString()
		{
			return this.Name;
		}
	}
}
