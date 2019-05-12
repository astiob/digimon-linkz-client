using System;

namespace System.Runtime.CompilerServices
{
	/// <summary>Specifies the preferred default binding for a dependent assembly.</summary>
	[Serializable]
	public enum LoadHint
	{
		/// <summary>No preference specified.</summary>
		Default,
		/// <summary>The dependency is always loaded.</summary>
		Always,
		/// <summary>The dependency is sometimes loaded.</summary>
		Sometimes
	}
}
