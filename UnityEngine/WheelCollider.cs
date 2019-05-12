using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>A special collider for vehicle wheels.</para>
	/// </summary>
	public sealed class WheelCollider : Collider
	{
		/// <summary>
		///   <para>The center of the wheel, measured in the object's local space.</para>
		/// </summary>
		public Vector3 center
		{
			get
			{
				Vector3 result;
				this.INTERNAL_get_center(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_center(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_center(out Vector3 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_center(ref Vector3 value);

		/// <summary>
		///   <para>The radius of the wheel, measured in local space.</para>
		/// </summary>
		public extern float radius { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Maximum extension distance of wheel suspension, measured in local space.</para>
		/// </summary>
		public extern float suspensionDistance { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The parameters of wheel's suspension. The suspension attempts to reach a target position by applying a linear force and a damping force.</para>
		/// </summary>
		public JointSpring suspensionSpring
		{
			get
			{
				JointSpring result;
				this.INTERNAL_get_suspensionSpring(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_suspensionSpring(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_suspensionSpring(out JointSpring value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_suspensionSpring(ref JointSpring value);

		/// <summary>
		///   <para>Application point of the suspension and tire forces measured from the base of the resting wheel.</para>
		/// </summary>
		public extern float forceAppPointDistance { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The mass of the wheel, expressed in kilograms. Must be larger than zero. Typical values would be in range (20,80).</para>
		/// </summary>
		public extern float mass { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The damping rate of the wheel. Must be larger than zero.</para>
		/// </summary>
		public extern float wheelDampingRate { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Properties of tire friction in the direction the wheel is pointing in.</para>
		/// </summary>
		public WheelFrictionCurve forwardFriction
		{
			get
			{
				WheelFrictionCurve result;
				this.INTERNAL_get_forwardFriction(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_forwardFriction(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_forwardFriction(out WheelFrictionCurve value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_forwardFriction(ref WheelFrictionCurve value);

		/// <summary>
		///   <para>Properties of tire friction in the sideways direction.</para>
		/// </summary>
		public WheelFrictionCurve sidewaysFriction
		{
			get
			{
				WheelFrictionCurve result;
				this.INTERNAL_get_sidewaysFriction(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_sidewaysFriction(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_sidewaysFriction(out WheelFrictionCurve value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_sidewaysFriction(ref WheelFrictionCurve value);

		/// <summary>
		///   <para>Motor torque on the wheel axle expressed in Newton metres. Positive or negative depending on direction.</para>
		/// </summary>
		public extern float motorTorque { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Brake torque expressed in Newton metres.</para>
		/// </summary>
		public extern float brakeTorque { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Steering angle in degrees, always around the local y-axis.</para>
		/// </summary>
		public extern float steerAngle { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Indicates whether the wheel currently collides with something (Read Only).</para>
		/// </summary>
		public extern bool isGrounded { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Configure vehicle sub-stepping parameters.</para>
		/// </summary>
		/// <param name="speedThreshold">The speed threshold of the sub-stepping algorithm.</param>
		/// <param name="stepsBelowThreshold">Amount of simulation sub-steps when vehicle's speed is below speedThreshold.</param>
		/// <param name="stepsAboveThreshold">Amount of simulation sub-steps when vehicle's speed is above speedThreshold.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ConfigureVehicleSubsteps(float speedThreshold, int stepsBelowThreshold, int stepsAboveThreshold);

		/// <summary>
		///   <para>The mass supported by this WheelCollider.</para>
		/// </summary>
		public extern float sprungMass { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetGroundHit(out WheelHit hit);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void GetWorldPose(out Vector3 pos, out Quaternion quat);

		/// <summary>
		///   <para>Current wheel axle rotation speed, in rotations per minute (Read Only).</para>
		/// </summary>
		public extern float rpm { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }
	}
}
