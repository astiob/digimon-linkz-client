using System;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	/// <summary>Specifies the type of user interface (UI) the trust manager should use for trust decisions. </summary>
	[ComVisible(true)]
	public enum TrustManagerUIContext
	{
		/// <summary>An Install UI.</summary>
		Install,
		/// <summary>An Upgrade UI.</summary>
		Upgrade,
		/// <summary>A Run UI.</summary>
		Run
	}
}
