using System;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>An enumeration used with the <see cref="T:System.LoaderOptimizationAttribute" /> class to specify loader optimizations for an executable.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public enum LoaderOptimization
	{
		/// <summary>Indicates that no optimizations for sharing internal resources are specified. If the default domain or hosting interface specified an optimization, then the loader uses that; otherwise, the loader uses <see cref="F:System.LoaderOptimization.SingleDomain" />.</summary>
		NotSpecified,
		/// <summary>Indicates that the application will probably have a single domain, and loader must not share internal resources across application domains. </summary>
		SingleDomain,
		/// <summary>Indicates that the application will probably have many domains that use the same code, and the loader must share maximal internal resources across application domains. </summary>
		MultiDomain,
		/// <summary>Indicates that the application will probably host unique code in multiple domains, and the loader must share resources across application domains only for globally available (strong-named) assemblies that have been added to the global assembly cache. </summary>
		MultiDomainHost,
		/// <summary>Do not use. This mask selects the domain-related values, screening out the unused <see cref="F:System.LoaderOptimization.DisallowBindings" /> flag.</summary>
		[Obsolete]
		DomainMask = 3,
		/// <summary>Ignored by the common language runtime.</summary>
		[Obsolete]
		DisallowBindings
	}
}
