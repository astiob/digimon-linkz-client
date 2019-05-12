using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Joint that keeps two Rigidbody2D objects a fixed distance apart.</para>
	/// </summary>
	public sealed class DistanceJoint2D : AnchoredJoint2D
	{
		/// <summary>
		///   <para>The distance separating the two ends of the joint.</para>
		/// </summary>
		public extern float distance { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Whether to maintain a maximum distance only or not.  If not then the absolute distance will be maintained instead.</para>
		/// </summary>
		public extern bool maxDistanceOnly { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Gets the reaction force of the joint given the specified timestep.</para>
		/// </summary>
		/// <param name="timeStep">The time to calculate the reaction force for.</param>
		public Vector2 GetReactionForce(float timeStep)
		{
			Vector2 result;
			DistanceJoint2D.DistanceJoint2D_CUSTOM_INTERNAL_GetReactionForce(this, timeStep, out result);
			return result;
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DistanceJoint2D_CUSTOM_INTERNAL_GetReactionForce(DistanceJoint2D joint, float timeStep, out Vector2 value);

		/// <summary>
		///   <para>Gets the reaction torque of the joint given the specified timestep.</para>
		/// </summary>
		/// <param name="timeStep">The time to calculate the reaction torque for.</param>
		public float GetReactionTorque(float timeStep)
		{
			return DistanceJoint2D.INTERNAL_CALL_GetReactionTorque(this, timeStep);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float INTERNAL_CALL_GetReactionTorque(DistanceJoint2D self, float timeStep);
	}
}
