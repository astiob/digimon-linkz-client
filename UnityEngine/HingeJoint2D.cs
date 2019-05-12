using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Joint that allows a Rigidbody2D object to rotate around a point in space or a point on another object.</para>
	/// </summary>
	public sealed class HingeJoint2D : AnchoredJoint2D
	{
		/// <summary>
		///   <para>Should the joint be rotated automatically by a motor torque?</para>
		/// </summary>
		public extern bool useMotor { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Should limits be placed on the range of rotation?</para>
		/// </summary>
		public extern bool useLimits { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Parameters for the motor force applied to the joint.</para>
		/// </summary>
		public JointMotor2D motor
		{
			get
			{
				JointMotor2D result;
				this.INTERNAL_get_motor(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_motor(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_motor(out JointMotor2D value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_motor(ref JointMotor2D value);

		/// <summary>
		///   <para>Limit of angular rotation (in degrees) on the joint.</para>
		/// </summary>
		public JointAngleLimits2D limits
		{
			get
			{
				JointAngleLimits2D result;
				this.INTERNAL_get_limits(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_limits(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_limits(out JointAngleLimits2D value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_limits(ref JointAngleLimits2D value);

		/// <summary>
		///   <para>Gets the state of the joint limit.</para>
		/// </summary>
		public extern JointLimitState2D limitState { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The angle (in degrees) referenced between the two bodies used as the constraint for the joint.</para>
		/// </summary>
		public extern float referenceAngle { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The current joint angle (in degrees) with respect to the reference angle.</para>
		/// </summary>
		public extern float jointAngle { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The current joint speed.</para>
		/// </summary>
		public extern float jointSpeed { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Gets the reaction force of the joint given the specified timestep.</para>
		/// </summary>
		/// <param name="timeStep">The time to calculate the reaction force for.</param>
		public Vector2 GetReactionForce(float timeStep)
		{
			Vector2 result;
			HingeJoint2D.HingeJoint2D_CUSTOM_INTERNAL_GetReactionForce(this, timeStep, out result);
			return result;
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void HingeJoint2D_CUSTOM_INTERNAL_GetReactionForce(HingeJoint2D joint, float timeStep, out Vector2 value);

		/// <summary>
		///   <para>Gets the reaction torque of the joint given the specified timestep.</para>
		/// </summary>
		/// <param name="timeStep">The time to calculate the reaction torque for.</param>
		public float GetReactionTorque(float timeStep)
		{
			return HingeJoint2D.INTERNAL_CALL_GetReactionTorque(this, timeStep);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float INTERNAL_CALL_GetReactionTorque(HingeJoint2D self, float timeStep);

		/// <summary>
		///   <para>Gets the motor torque of the joint given the specified timestep.</para>
		/// </summary>
		/// <param name="timeStep">The time to calculate the motor torque for.</param>
		public float GetMotorTorque(float timeStep)
		{
			return HingeJoint2D.INTERNAL_CALL_GetMotorTorque(this, timeStep);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float INTERNAL_CALL_GetMotorTorque(HingeJoint2D self, float timeStep);
	}
}
