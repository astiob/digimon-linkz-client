using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Use <see cref="T:System.Runtime.InteropServices.ComTypes.VARDESC" /> instead.</summary>
	[Obsolete]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct VARDESC
	{
		/// <summary>Indicates the member ID of a variable.</summary>
		public int memid;

		/// <summary>This field is reserved for future use.</summary>
		public string lpstrSchema;

		/// <summary>Contains the variable type.</summary>
		public ELEMDESC elemdescVar;

		/// <summary>Defines the properties of a variable.</summary>
		public short wVarFlags;

		/// <summary>Defines how a variable should be marshaled.</summary>
		public VarEnum varkind;

		/// <summary>Use <see cref="T:System.Runtime.InteropServices.ComTypes.VARDESC.DESCUNION" /> instead.</summary>
		[ComVisible(false)]
		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
		public struct DESCUNION
		{
			/// <summary>Describes a symbolic constant.</summary>
			[FieldOffset(0)]
			public IntPtr lpvarValue;

			/// <summary>Indicates the offset of this variable within the instance.</summary>
			[FieldOffset(0)]
			public int oInst;
		}
	}
}
