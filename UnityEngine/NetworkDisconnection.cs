using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>The reason a disconnect event occured, like in MonoBehaviour.OnDisconnectedFromServer|OnDisconnectedFromServer.</para>
	/// </summary>
	public enum NetworkDisconnection
	{
		/// <summary>
		///   <para>The connection to the system has been lost, no reliable packets could be delivered.</para>
		/// </summary>
		LostConnection = 20,
		/// <summary>
		///   <para>The connection to the system has been closed.</para>
		/// </summary>
		Disconnected = 19
	}
}
