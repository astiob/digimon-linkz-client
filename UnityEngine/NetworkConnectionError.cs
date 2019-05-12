using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Possible status messages returned by Network.Connect and in MonoBehaviour.OnFailedToConnect|OnFailedToConnect in case the error was not immediate.</para>
	/// </summary>
	public enum NetworkConnectionError
	{
		/// <summary>
		///   <para>No error occurred.</para>
		/// </summary>
		NoError,
		/// <summary>
		///   <para>We presented an RSA public key which does not match what the system we connected to is using.</para>
		/// </summary>
		RSAPublicKeyMismatch = 21,
		/// <summary>
		///   <para>The server is using a password and has refused our connection because we did not set the correct password.</para>
		/// </summary>
		InvalidPassword = 23,
		/// <summary>
		///   <para>Connection attempt failed, possibly because of internal connectivity problems.</para>
		/// </summary>
		ConnectionFailed = 15,
		/// <summary>
		///   <para>The server is at full capacity, failed to connect.</para>
		/// </summary>
		TooManyConnectedPlayers = 18,
		/// <summary>
		///   <para>We are banned from the system we attempted to connect to (likely temporarily).</para>
		/// </summary>
		ConnectionBanned = 22,
		/// <summary>
		///   <para>We are already connected to this particular server (can happen after fast disconnect/reconnect).</para>
		/// </summary>
		AlreadyConnectedToServer = 16,
		/// <summary>
		///   <para>Cannot connect to two servers at once. Close the connection before connecting again.</para>
		/// </summary>
		AlreadyConnectedToAnotherServer = -1,
		/// <summary>
		///   <para>Internal error while attempting to initialize network interface. Socket possibly already in use.</para>
		/// </summary>
		CreateSocketOrThreadFailure = -2,
		/// <summary>
		///   <para>Incorrect parameters given to Connect function.</para>
		/// </summary>
		IncorrectParameters = -3,
		/// <summary>
		///   <para>No host target given in Connect.</para>
		/// </summary>
		EmptyConnectTarget = -4,
		/// <summary>
		///   <para>Client could not connect internally to same network NAT enabled server.</para>
		/// </summary>
		InternalDirectConnectFailed = -5,
		/// <summary>
		///   <para>The NAT target we are trying to connect to is not connected to the facilitator server.</para>
		/// </summary>
		NATTargetNotConnected = 69,
		/// <summary>
		///   <para>Connection lost while attempting to connect to NAT target.</para>
		/// </summary>
		NATTargetConnectionLost = 71,
		/// <summary>
		///   <para>NAT punchthrough attempt has failed. The cause could be a too restrictive NAT implementation on either endpoints.</para>
		/// </summary>
		NATPunchthroughFailed = 73
	}
}
