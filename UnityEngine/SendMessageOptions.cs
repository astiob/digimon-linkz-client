using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Options for how to send a message.</para>
	/// </summary>
	public enum SendMessageOptions
	{
		/// <summary>
		///   <para>A receiver is required for SendMessage.</para>
		/// </summary>
		RequireReceiver,
		/// <summary>
		///   <para>No receiver is required for SendMessage.</para>
		/// </summary>
		DontRequireReceiver
	}
}
