using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization.Formatters
{
	/// <summary>Specifies the level of automatic deserialization for .NET Framework remoting.</summary>
	[ComVisible(true)]
	public enum TypeFilterLevel
	{
		/// <summary>The low deserialization level for .NET Framework remoting. It supports types associated with basic remoting functionality.</summary>
		Low = 2,
		/// <summary>The full deserialization level for .NET Framework remoting. It supports all types that remoting supports in all situations.</summary>
		Full
	}
}
