using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Option for who will receive an RPC, used by NetworkView.RPC.</para>
	/// </summary>
	public enum RPCMode
	{
		/// <summary>
		///   <para>Sends to the server only.</para>
		/// </summary>
		Server,
		/// <summary>
		///   <para>Sends to everyone except the sender.</para>
		/// </summary>
		Others,
		/// <summary>
		///   <para>Sends to everyone except the sender and adds to the buffer.</para>
		/// </summary>
		OthersBuffered = 5,
		/// <summary>
		///   <para>Sends to everyone.</para>
		/// </summary>
		All = 2,
		/// <summary>
		///   <para>Sends to everyone and adds to the buffer.</para>
		/// </summary>
		AllBuffered = 6
	}
}
