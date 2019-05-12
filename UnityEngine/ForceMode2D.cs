using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Option for how to apply a force using Rigidbody2D.AddForce.</para>
	/// </summary>
	public enum ForceMode2D
	{
		/// <summary>
		///   <para>Add a force to the Rigidbody2D, using its mass.</para>
		/// </summary>
		Force,
		/// <summary>
		///   <para>Add an instant force impulse to the rigidbody2D, using its mass.</para>
		/// </summary>
		Impulse
	}
}
