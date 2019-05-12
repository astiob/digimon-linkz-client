using System;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	/// <summary>Specifies how to match versions when locating application trusts in a collection.</summary>
	[ComVisible(true)]
	public enum ApplicationVersionMatch
	{
		/// <summary>Match on the exact version.</summary>
		MatchExactVersion,
		/// <summary>Match on all versions.</summary>
		MatchAllVersions
	}
}
