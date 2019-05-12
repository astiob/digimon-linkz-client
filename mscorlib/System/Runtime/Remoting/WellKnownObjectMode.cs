using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting
{
	/// <summary>Defines how well-known objects are activated.</summary>
	[ComVisible(true)]
	[Serializable]
	public enum WellKnownObjectMode
	{
		/// <summary>Every incoming message is serviced by the same object instance.</summary>
		Singleton = 1,
		/// <summary>Every incoming message is serviced by a new object instance.</summary>
		SingleCall
	}
}
