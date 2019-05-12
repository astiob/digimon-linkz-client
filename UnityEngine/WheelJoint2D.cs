using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>The wheel joint allows the simulation of wheels by providing a constraining suspension motion with an optional motor.</para>
	/// </summary>
	public sealed class WheelJoint2D : AnchoredJoint2D
	{
		/// <summary>
		///   <para>Set the joint suspension configuration.</para>
		/// </summary>
		public JointSuspension2D suspension
		{
			get
			{
				JointSuspension2D result;
				this.INTERNAL_get_suspension(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_suspension(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_suspension(out JointSuspension2D value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_suspension(ref JointSuspension2D value);

		/// <summary>
		///   <para>Should a motor force be applied automatically to the Rigidbody2D?</para>
		/// </summary>
		public extern bool useMotor { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

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
		///   <para>The current joint translation.</para>
		/// </summary>
		public extern float jointTranslation { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The current joint speed.</para>
		/// </summary>
		public extern float jointSpeed { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Gets the motor torque of the joint given the specified timestep.</para>
		/// </summary>
		/// <param name="timeStep">The time to calculate the motor torque for.</param>
		public float GetMotorTorque(float timeStep)
		{
			return WheelJoint2D.INTERNAL_CALL_GetMotorTorque(this, timeStep);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float INTERNAL_CALL_GetMotorTorque(WheelJoint2D self, float timeStep);
	}
}
