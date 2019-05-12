using System;
using System.Runtime.InteropServices;

namespace System.Resources
{
	/// <summary>Instructs the <see cref="T:System.Resources.ResourceManager" /> to ask for a particular version of a satellite assembly to simplify updates of the main assembly of an application.</summary>
	[AttributeUsage(AttributeTargets.Assembly)]
	[ComVisible(true)]
	public sealed class SatelliteContractVersionAttribute : Attribute
	{
		private Version ver;

		/// <summary>Initializes a new instance of the <see cref="T:System.Resources.SatelliteContractVersionAttribute" /> class.</summary>
		/// <param name="version">A <see cref="T:System.String" /> with the version of the satellite assemblies to load. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="version" /> parameter is null. </exception>
		public SatelliteContractVersionAttribute(string version)
		{
			this.ver = new Version(version);
		}

		/// <summary>Gets the version of the satellite assemblies with the required resources.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the version of the satellite assemblies with the required resources.</returns>
		public string Version
		{
			get
			{
				return this.ver.ToString();
			}
		}
	}
}
