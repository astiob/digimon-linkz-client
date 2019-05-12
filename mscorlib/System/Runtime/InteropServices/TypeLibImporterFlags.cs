using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Indicates how an assembly should be produced.</summary>
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum TypeLibImporterFlags
	{
		/// <summary>Generates a primary interop assembly. See <see cref="T:System.Runtime.InteropServices.PrimaryInteropAssemblyAttribute" /> for details. A keyfile must be specified.</summary>
		PrimaryInteropAssembly = 1,
		/// <summary>Imports all interfaces as interfaces that suppress the common language runtime's stack crawl for <see cref="F:System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode" /> permission. Be sure you understand the responsibilities associated with suppressing this security check.</summary>
		UnsafeInterfaces = 2,
		/// <summary>Imports all SAFEARRAYs as <see cref="T:System.Array" /> rather than a typed, single dimensional, zero-based managed array. This option is useful when dealing with multi dimensional, non zero-based SAFEARRAYs which otherwise can not be accessed unless you edit the resulting assembly using the ILDASM and ILASM tools.</summary>
		SafeArrayAsSystemArray = 4,
		/// <summary>Transforms [out, retval] parameters of methods on dispatch-only interfaces (dispinterfaces) into return values.</summary>
		TransformDispRetVals = 8,
		/// <summary>Specifies no flags. This is the default.</summary>
		None = 0,
		/// <summary>Not used.</summary>
		PreventClassMembers = 16,
		/// <summary>Imports a type library for any platform.</summary>
		ImportAsAgnostic = 2048,
		/// <summary>Imports a type library for the Itanuim platform.</summary>
		ImportAsItanium = 1024,
		/// <summary>Imports a type library for the X86 64 bit platform.</summary>
		ImportAsX64 = 512,
		/// <summary>Imports a type library for the X86 platform.</summary>
		ImportAsX86 = 256,
		/// <summary>Specifies the use of reflection only loading.</summary>
		ReflectionOnlyLoading = 4096,
		/// <summary>Specifies the use of serailizable classes.</summary>
		SerializableValueClasses = 32
	}
}
