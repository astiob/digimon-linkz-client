using System;

namespace Microsoft.Win32
{
	/// <summary>Specifies optional behavior when retrieving name/value pairs from a registry key.</summary>
	[Flags]
	public enum RegistryValueOptions
	{
		/// <summary>No optional behavior is specified.</summary>
		None = 0,
		/// <summary>A value of type <see cref="F:Microsoft.Win32.RegistryValueKind.ExpandString" /> is retrieved without expanding its embedded environment variables. </summary>
		DoNotExpandEnvironmentNames = 1
	}
}
