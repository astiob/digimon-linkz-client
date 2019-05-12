using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Defines a set of flags used when registering assemblies.</summary>
	[ComVisible(true)]
	[Flags]
	public enum AssemblyRegistrationFlags
	{
		/// <summary>Indicates no special settings.</summary>
		None = 0,
		/// <summary>Indicates that the code base key for the assembly should be set in the registry.</summary>
		SetCodeBase = 1
	}
}
