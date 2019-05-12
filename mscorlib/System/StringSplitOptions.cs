using System;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>Specifies whether applicable <see cref="Overload:System.String.Split" /> method overloads include or omit empty substrings from the return value.</summary>
	/// <filterpriority>1</filterpriority>
	[ComVisible(false)]
	[Flags]
	public enum StringSplitOptions
	{
		/// <summary>The return value includes array elements that contain an empty string</summary>
		None = 0,
		/// <summary>The return value does not include array elements that contain an empty string</summary>
		RemoveEmptyEntries = 1
	}
}
