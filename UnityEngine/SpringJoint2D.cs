using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Joint that attempts to keep two Rigidbody2D objects a set distance apart by applying a force between them.</para>
	/// </summary>
	public sealed class SpringJoint2D : AnchoredJoint2D
	{
		/// <summary>
		///   <para>The distance the spring will try to keep between the two objects.</para>
		/// </summary>
		public extern float distance { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The amount by which the spring force is reduced in proportion to the movement speed.</para>
		/// </summary>
		public extern float dampingRatio { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The frequency at which the spring oscillates around the distance distance between the objects.</para>
		/// </summary>
		public extern float frequency { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Gets the reaction force of the joint given the specified timestep.</para>
		/// </summary>
		/// <param name="timeStep">The time to calculate the reaction force for.</param>
		public Vector2 GetReactionForce(float timeStep)
		{
			Vector2 result;
			SpringJoint2D.SpringJoint2D_CUSTOM_INTERNAL_GetReactionForce(this, timeStep, out result);
			return result;
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SpringJoint2D_CUSTOM_INTERNAL_GetReactionForce(SpringJoint2D joint, float timeStep, out Vector2 value);

		/// <summary>
		///   <para>Gets the reaction torque of the joint given the specified timestep.</para>
		/// </summary>
		/// <param name="timeStep">The time to calculate the reaction torque for.</param>
		public float GetReactionTorque(float timeStep)
		{
			return SpringJoint2D.INTERNAL_CALL_GetReactionTorque(this, timeStep);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float INTERNAL_CALL_GetReactionTorque(SpringJoint2D self, float timeStep);
	}
}
