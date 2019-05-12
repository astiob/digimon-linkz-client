using System;

namespace UnityEngine.Networking
{
	/// <summary>
	///   <para>Possible transport layer erors.</para>
	/// </summary>
	public enum NetworkError
	{
		/// <summary>
		///   <para>Everything good so far.</para>
		/// </summary>
		Ok,
		/// <summary>
		///   <para>Host doesn't exist.</para>
		/// </summary>
		WrongHost,
		/// <summary>
		///   <para>Connection doesn't exist.</para>
		/// </summary>
		WrongConnection,
		/// <summary>
		///   <para>Channel doesn't exist.</para>
		/// </summary>
		WrongChannel,
		/// <summary>
		///   <para>No internal resources ro acomplish request.</para>
		/// </summary>
		NoResources,
		/// <summary>
		///   <para>Obsolete.</para>
		/// </summary>
		BadMessage,
		/// <summary>
		///   <para>Timeout happened.</para>
		/// </summary>
		Timeout,
		/// <summary>
		///   <para>Sending message too long to fit internal buffers, or user doesn't present buffer with length enouf to contain receiving message.</para>
		/// </summary>
		MessageToLong,
		/// <summary>
		///   <para>Operation is not supported.</para>
		/// </summary>
		WrongOperation,
		/// <summary>
		///   <para>Different version of protocol on ends of connection.</para>
		/// </summary>
		VersionMismatch,
		/// <summary>
		///   <para>Two ends of connection have different agreement about channels, channels qos and network parameters.</para>
		/// </summary>
		CRCMismatch,
		/// <summary>
		///   <para>The address supplied to connect to was invalid or could not be resolved.</para>
		/// </summary>
		DNSFailure
	}
}
