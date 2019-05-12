using System;
using System.Runtime.InteropServices;

namespace System.Resources
{
	/// <summary>Informs the <see cref="T:System.Resources.ResourceManager" /> of the neutral culture of an assembly. This class cannot be inherited.</summary>
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly)]
	public sealed class NeutralResourcesLanguageAttribute : Attribute
	{
		private string culture;

		private UltimateResourceFallbackLocation loc;

		/// <summary>Initializes a new instance of the <see cref="T:System.Resources.NeutralResourcesLanguageAttribute" /> class.</summary>
		/// <param name="cultureName">The name of the culture that the current assembly's neutral resources were written in. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="cultureName" /> parameter is null. </exception>
		public NeutralResourcesLanguageAttribute(string cultureName)
		{
			if (cultureName == null)
			{
				throw new ArgumentNullException("culture is null");
			}
			this.culture = cultureName;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Resources.NeutralResourcesLanguageAttribute" /> class with the specified ultimate resource fallback location.</summary>
		/// <param name="cultureName">The name of the culture that the current assembly's neutral resources were written in.</param>
		/// <param name="location">An <see cref="T:System.Resources.UltimateResourceFallbackLocation" /> enumeration value indicating the location from which to retrieve neutral fallback resources.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="cultureName" /> is null.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="location" /> is not a member of <see cref="T:System.Resources.UltimateResourceFallbackLocation" />.</exception>
		public NeutralResourcesLanguageAttribute(string cultureName, UltimateResourceFallbackLocation location)
		{
			if (cultureName == null)
			{
				throw new ArgumentNullException("culture is null");
			}
			this.culture = cultureName;
			this.loc = location;
		}

		/// <summary>Gets the culture name.</summary>
		/// <returns>A <see cref="T:System.String" /> with the name of the default culture for the main assembly.</returns>
		public string CultureName
		{
			get
			{
				return this.culture;
			}
		}

		/// <summary>Gets the location for the <see cref="T:System.Resources.ResourceManager" /> class to use to retrieve neutral resources by using the resource fallback process.</summary>
		/// <returns>The value of the <see cref="T:System.Resources.UltimateResourceFallbackLocation" /> enumeration that indicates the location (main assembly or satellite) from which to retrieve neutral resources.</returns>
		public UltimateResourceFallbackLocation Location
		{
			get
			{
				return this.loc;
			}
		}
	}
}
