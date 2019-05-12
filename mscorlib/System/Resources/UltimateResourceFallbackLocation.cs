using System;
using System.Runtime.InteropServices;

namespace System.Resources
{
	/// <summary>Specifies the assembly for the <see cref="T:System.Resources.ResourceManager" /> class to use to retrieve neutral resources by using the Packaging and Deploying Resources.</summary>
	[ComVisible(true)]
	[Serializable]
	public enum UltimateResourceFallbackLocation
	{
		/// <summary>Fallback resources are located in the main assembly.</summary>
		MainAssembly,
		/// <summary>Fallback resources are located in a satellite assembly in the location specified by the <see cref="P:System.Resources.NeutralResourcesLanguageAttribute.Location" /> property.</summary>
		Satellite
	}
}
