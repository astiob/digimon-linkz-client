using System;

namespace System.Runtime.InteropServices.ComTypes
{
	/// <summary>Describes a variable, constant, or data member.</summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct VARDESC
	{
		/// <summary>Indicates the member ID of a variable.</summary>
		public int memid;

		/// <summary>This field is reserved for future use.</summary>
		public string lpstrSchema;

		/// <summary>Contains information about a variable.</summary>
		public VARDESC.DESCUNION desc;

		/// <summary>Contains the variable type.</summary>
		public ELEMDESC elemdescVar;

		/// <summary>Defines the properties of a variable.</summary>
		public short wVarFlags;

		/// <summary>Defines how to marshal a variable.</summary>
		public VARKIND varkind;

		/// <summary>Contains information about a variable.</summary>
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
