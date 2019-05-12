using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Different types of synchronization for the NetworkView component.</para>
	/// </summary>
	public enum NetworkStateSynchronization
	{
		/// <summary>
		///   <para>No state data will be synchronized.</para>
		/// </summary>
		Off,
		/// <summary>
		///   <para>All packets are sent reliable and ordered.</para>
		/// </summary>
		ReliableDeltaCompressed,
		/// <summary>
		///   <para>Brute force unreliable state sending.</para>
		/// </summary>
		Unreliable
	}
}
