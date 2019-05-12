using System;

namespace System.Net.NetworkInformation
{
	/// <summary>Specifies the operational state of a network interface.</summary>
	public enum OperationalStatus
	{
		/// <summary>The network interface is up; it can transmit data packets.</summary>
		Up = 1,
		/// <summary>The network interface is unable to transmit data packets.</summary>
		Down,
		/// <summary>The network interface is running tests.</summary>
		Testing,
		/// <summary>The network interface status is not known.</summary>
		Unknown,
		/// <summary>The network interface is not in a condition to transmit data packets; it is waiting for an external event.</summary>
		Dormant,
		/// <summary>The network interface is unable to transmit data packets because of a missing component, typically a hardware component.</summary>
		NotPresent,
		/// <summary>The network interface is unable to transmit data packets because it runs on top of one or more other interfaces, and at least one of these "lower layer" interfaces is down.</summary>
		LowerLayerDown
	}
}
