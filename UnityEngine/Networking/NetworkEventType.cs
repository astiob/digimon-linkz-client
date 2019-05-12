using System;

namespace UnityEngine.Networking
{
	/// <summary>
	///   <para>Type of events returned from Receive() function.</para>
	/// </summary>
	public enum NetworkEventType
	{
		/// <summary>
		///   <para>New data come in.</para>
		/// </summary>
		DataEvent,
		/// <summary>
		///   <para>New connection has been connected.</para>
		/// </summary>
		ConnectEvent,
		/// <summary>
		///   <para>Connection has been disconnected.</para>
		/// </summary>
		DisconnectEvent,
		/// <summary>
		///   <para>Nothing happened.</para>
		/// </summary>
		Nothing,
		/// <summary>
		///   <para>Broadcast discovery event received. To obtain sender connection info and possible complimentary message from him call GetBroadcastConnectionInfo() and GetBroadcastConnectionMessage() functions.</para>
		/// </summary>
		BroadcastEvent
	}
}
