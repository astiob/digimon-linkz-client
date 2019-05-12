using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Activation
{
	/// <summary>Defines the appropriate position for a <see cref="T:System.Activator" /> in the chain of activators.</summary>
	[ComVisible(true)]
	[Serializable]
	public enum ActivatorLevel
	{
		/// <summary>Constructs a blank object and runs the constructor.</summary>
		Construction = 4,
		/// <summary>Finds or creates a suitable context.</summary>
		Context = 8,
		/// <summary>Finds or creates a <see cref="T:System.AppDomain" />.</summary>
		AppDomain = 12,
		/// <summary>Starts a process.</summary>
		Process = 16,
		/// <summary>Finds a suitable computer.</summary>
		Machine = 20
	}
}
