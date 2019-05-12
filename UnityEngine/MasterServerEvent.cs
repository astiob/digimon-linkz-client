using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Describes status messages from the master server as returned in MonoBehaviour.OnMasterServerEvent|OnMasterServerEvent.</para>
	/// </summary>
	public enum MasterServerEvent
	{
		/// <summary>
		///   <para>Registration failed because an empty game name was given.</para>
		/// </summary>
		RegistrationFailedGameName,
		/// <summary>
		///   <para>Registration failed because an empty game type was given.</para>
		/// </summary>
		RegistrationFailedGameType,
		/// <summary>
		///   <para>Registration failed because no server is running.</para>
		/// </summary>
		RegistrationFailedNoServer,
		/// <summary>
		///   <para>Registration to master server succeeded, received confirmation.</para>
		/// </summary>
		RegistrationSucceeded,
		/// <summary>
		///   <para>Received a host list from the master server.</para>
		/// </summary>
		HostListReceived
	}
}
