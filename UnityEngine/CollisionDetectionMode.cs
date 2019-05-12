using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>The collision detection mode constants used for Rigidbody.collisionDetectionMode.</para>
	/// </summary>
	public enum CollisionDetectionMode
	{
		/// <summary>
		///   <para>Continuous collision detection is off for this Rigidbody.</para>
		/// </summary>
		Discrete,
		/// <summary>
		///   <para>Continuous collision detection is on for colliding with static mesh geometry.</para>
		/// </summary>
		Continuous,
		/// <summary>
		///   <para>Continuous collision detection is on for colliding with static and dynamic geometry.</para>
		/// </summary>
		ContinuousDynamic
	}
}
