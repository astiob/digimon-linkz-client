using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Controls how collisions are detected when a Rigidbody2D moves.</para>
	/// </summary>
	public enum CollisionDetectionMode2D
	{
		/// <summary>
		///   <para>This mode is obsolete.  You should use Discrete mode.</para>
		/// </summary>
		[Obsolete("Enum member CollisionDetectionMode2D.None has been deprecated. Use CollisionDetectionMode2D.Discrete instead (UnityUpgradable) -> Discrete", true)]
		None,
		/// <summary>
		///   <para>When a Rigidbody2D moves, only collisions at the new position are detected.</para>
		/// </summary>
		Discrete = 0,
		/// <summary>
		///   <para>Ensures that all collisions are detected when a Rigidbody2D moves.</para>
		/// </summary>
		Continuous
	}
}
