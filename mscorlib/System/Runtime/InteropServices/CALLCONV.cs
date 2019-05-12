using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Use <see cref="T:System.Runtime.InteropServices.ComTypes.CALLCONV" /> instead.</summary>
	[Obsolete]
	[Serializable]
	public enum CALLCONV
	{
		/// <summary>Indicates that the Cdecl calling convention is used for a method.</summary>
		CC_CDECL = 1,
		/// <summary>Indicates that the Pascal calling convention is used for a method.</summary>
		CC_PASCAL,
		/// <summary>Indicates that the Mscpascal calling convention is used for a method.</summary>
		CC_MSCPASCAL = 2,
		/// <summary>Indicates that the Macpascal calling convention is used for a method.</summary>
		CC_MACPASCAL,
		/// <summary>Indicates that the Stdcall calling convention is used for a method.</summary>
		CC_STDCALL,
		/// <summary>This value is reserved for future use.</summary>
		CC_RESERVED,
		/// <summary>Indicates that the Syscall calling convention is used for a method.</summary>
		CC_SYSCALL,
		/// <summary>Indicates that the Mpwcdecl calling convention is used for a method.</summary>
		CC_MPWCDECL,
		/// <summary>Indicates that the Mpwpascal calling convention is used for a method.</summary>
		CC_MPWPASCAL,
		/// <summary>Indicates the end of the <see cref="T:System.Runtime.InteropServices.CALLCONV" /> enumeration.</summary>
		CC_MAX
	}
}
