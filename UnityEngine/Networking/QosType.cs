using System;

namespace UnityEngine.Networking
{
	/// <summary>
	///   <para>Descibed allowed types of quality of service for channels.</para>
	/// </summary>
	public enum QosType
	{
		/// <summary>
		///   <para>Just sending message, no grants.</para>
		/// </summary>
		Unreliable,
		/// <summary>
		///   <para>The same as unreliable, but big message (up to 32 fragment per message) can be sent.</para>
		/// </summary>
		UnreliableFragmented,
		/// <summary>
		///   <para>The same as unrelaible but all unorder messages will be dropped. Example: VoIP.</para>
		/// </summary>
		UnreliableSequenced,
		/// <summary>
		///   <para>Channel will be configured as relaiable, each message sent in this channel will be delivered or connection will be disconnected.</para>
		/// </summary>
		Reliable,
		/// <summary>
		///   <para>Same as reliable, but big messages are allowed (up to 32 fragment with fragmentsize each for message).</para>
		/// </summary>
		ReliableFragmented,
		/// <summary>
		///   <para>The same as reliable, but with granting message order.</para>
		/// </summary>
		ReliableSequenced,
		/// <summary>
		///   <para>Unreliable, only last message in send buffer will be sent, only most recent message in reading buffer will be delivered.</para>
		/// </summary>
		StateUpdate,
		/// <summary>
		///   <para>The same as StateUpdate, but reliable.</para>
		/// </summary>
		ReliableStateUpdate,
		/// <summary>
		///   <para>Reliable message will resend almost with each frame, without waiting  delivery notification. usefull for important urgent short messages, like a shoot.</para>
		/// </summary>
		AllCostDelivery
	}
}
