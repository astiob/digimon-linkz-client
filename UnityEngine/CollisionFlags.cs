using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>CollisionFlags is a bitmask returned by CharacterController.Move.</para>
	/// </summary>
	public enum CollisionFlags
	{
		/// <summary>
		///   <para>CollisionFlags is a bitmask returned by CharacterController.Move.</para>
		/// </summary>
		None,
		/// <summary>
		///   <para>CollisionFlags is a bitmask returned by CharacterController.Move.</para>
		/// </summary>
		Sides,
		/// <summary>
		///   <para>CollisionFlags is a bitmask returned by CharacterController.Move.</para>
		/// </summary>
		Above,
		/// <summary>
		///   <para>CollisionFlags is a bitmask returned by CharacterController.Move.</para>
		/// </summary>
		Below = 4,
		CollidedSides = 1,
		CollidedAbove,
		CollidedBelow = 4
	}
}
