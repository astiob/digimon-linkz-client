using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Provides control over the casing of names when exported to a type library.</summary>
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("fa1f3615-acb9-486d-9eac-1bef87e36b09")]
	[ComVisible(true)]
	public interface ITypeLibExporterNameProvider
	{
		/// <summary>Returns a list of names to control the casing of.</summary>
		/// <returns>An array of strings, where each element contains the name of a type to control casing for.</returns>
		[return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_BSTR)]
		string[] GetNames();
	}
}
