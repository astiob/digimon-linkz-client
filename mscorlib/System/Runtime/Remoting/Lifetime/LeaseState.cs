using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Lifetime
{
	/// <summary>Indicates the possible lease states of a lifetime lease.</summary>
	[ComVisible(true)]
	[Serializable]
	public enum LeaseState
	{
		/// <summary>The lease is not initialized.</summary>
		Null,
		/// <summary>The lease has been created, but is not yet active.</summary>
		Initial,
		/// <summary>The lease is active and has not expired.</summary>
		Active,
		/// <summary>The lease has expired and is seeking sponsorship.</summary>
		Renewing,
		/// <summary>The lease has expired and cannot be renewed.</summary>
		Expired
	}
}
