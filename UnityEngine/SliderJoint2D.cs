using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Joint that restricts the motion of a Rigidbody2D object to a single line.</para>
	/// </summary>
	public sealed class SliderJoint2D : AnchoredJoint2D
	{
		/// <summary>
		///   <para>The angle of the line in space (in degrees).</para>
		/// </summary>
		public extern float angle { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Should a motor force be applied automatically to the Rigidbody2D?</para>
		/// </summary>
		public extern bool useMotor { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Should motion limits be used?</para>
		/// </summary>
		public extern bool useLimits { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Parameters for a motor force that is applied automatically to the Rigibody2D along the line.</para>
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
		///   <para>Restrictions on how far the joint can slide in each direction along the line.</para>
		/// </summary>
		public JointTranslationLimits2D limits
		{
			get
			{
				JointTranslationLimits2D result;
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
		private extern void INTERNAL_get_limits(out JointTranslationLimits2D value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_limits(ref JointTranslationLimits2D value);

		/// <summary>
		///   <para>Gets the state of the joint limit.</para>
		/// </summary>
		public extern JointLimitState2D limitState { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The angle (in degrees) referenced between the two bodies used as the constraint for the joint.</para>
		/// </summary>
		public extern float referenceAngle { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The current joint translation.</para>
		/// </summary>
		public extern float jointTranslation { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The current joint speed.</para>
		/// </summary>
		public extern float jointSpeed { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Gets the motor force of the joint given the specified timestep.</para>
		/// </summary>
		/// <param name="timeStep">The time to calculate the motor force for.</param>
		public float GetMotorForce(float timeStep)
		{
			return SliderJoint2D.INTERNAL_CALL_GetMotorForce(this, timeStep);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float INTERNAL_CALL_GetMotorForce(SliderJoint2D self, float timeStep);
	}
}
