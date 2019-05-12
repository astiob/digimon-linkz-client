using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Describes when an AudioSource or AudioListener is updated.</para>
	/// </summary>
	public enum AudioVelocityUpdateMode
	{
		/// <summary>
		///   <para>Updates the source or listener in the fixed update loop if it is attached to a Rigidbody, dynamic otherwise.</para>
		/// </summary>
		Auto,
		/// <summary>
		///   <para>Updates the source or listener in the fixed update loop.</para>
		/// </summary>
		Fixed,
		/// <summary>
		///   <para>Updates the source or listener in the dynamic update loop.</para>
		/// </summary>
		Dynamic
	}
}
