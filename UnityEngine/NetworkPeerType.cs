using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Describes the status of the network interface peer type as returned by Network.peerType.</para>
	/// </summary>
	public enum NetworkPeerType
	{
		/// <summary>
		///   <para>No client connection running. Server not initialized.</para>
		/// </summary>
		Disconnected,
		/// <summary>
		///   <para>Running as server.</para>
		/// </summary>
		Server,
		/// <summary>
		///   <para>Running as client.</para>
		/// </summary>
		Client,
		/// <summary>
		///   <para>Attempting to connect to a server.</para>
		/// </summary>
		Connecting
	}
}
