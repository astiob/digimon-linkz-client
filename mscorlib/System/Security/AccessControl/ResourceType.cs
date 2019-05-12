using System;

namespace System.Security.AccessControl
{
	/// <summary>Specifies the defined native object types.</summary>
	public enum ResourceType
	{
		/// <summary>An unknown object type.</summary>
		Unknown,
		/// <summary>A file or directory.</summary>
		FileObject,
		/// <summary>A Windows service.</summary>
		Service,
		/// <summary>A printer.</summary>
		Printer,
		/// <summary>A registry key.</summary>
		RegistryKey,
		/// <summary>A network share.</summary>
		LMShare,
		/// <summary>A local kernel object.</summary>
		KernelObject,
		/// <summary>A window station or desktop object on the local computer.</summary>
		WindowObject,
		/// <summary>A directory service (DS) object or a property set or property of a directory service object.</summary>
		DSObject,
		/// <summary>A directory service object and all of its property sets and properties.</summary>
		DSObjectAll,
		/// <summary>An object defined by a provider.</summary>
		ProviderDefined,
		/// <summary>A Windows Management Instrumentation (WMI) object.</summary>
		WmiGuidObject,
		/// <summary>An object for a registry entry under WOW64.</summary>
		RegistryWow6432Key
	}
}
