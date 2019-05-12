using System;

namespace System.Runtime.InteropServices.ComTypes
{
	/// <summary>Describes a connection that exists to a given connection point.</summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct CONNECTDATA
	{
		/// <summary>Represents a pointer to the IUnknown interface on a connected advisory sink. The caller must call IUnknown::Release on this pointer when the CONNECTDATA structure is no longer needed.</summary>
		[MarshalAs(UnmanagedType.Interface)]
		public object pUnk;

		/// <summary>Represents a connection token that is returned from a call to <see cref="M:System.Runtime.InteropServices.ComTypes.IConnectionPoint.Advise(System.Object,System.Int32@)" />.</summary>
		public int dwCookie;
	}
}
