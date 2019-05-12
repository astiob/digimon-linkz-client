using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Use <see cref="T:System.Runtime.InteropServices.ComTypes.PARAMDESC" /> instead.</summary>
	[Obsolete]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct PARAMDESC
	{
		/// <summary>Represents a pointer to a value that is being passed between processes.</summary>
		public IntPtr lpVarValue;

		/// <summary>Represents bitmask values that describe the structure element, parameter, or return value.</summary>
		public PARAMFLAG wParamFlags;
	}
}
