using System;

namespace System.Runtime.InteropServices.ComTypes
{
	/// <summary>Contains the type description and process transfer information for a variable, function, or a function parameter.</summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct ELEMDESC
	{
		/// <summary>Identifies the type of the element.</summary>
		public TYPEDESC tdesc;

		/// <summary>Contains information about an element.</summary>
		public ELEMDESC.DESCUNION desc;

		/// <summary>Contains information about an element. </summary>
		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
		public struct DESCUNION
		{
			/// <summary>Contains information for remoting the element.</summary>
			[FieldOffset(0)]
			public IDLDESC idldesc;

			/// <summary>Contains information about the parameter.</summary>
			[FieldOffset(0)]
			public PARAMDESC paramdesc;
		}
	}
}
