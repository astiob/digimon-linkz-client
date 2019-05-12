using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	[NativeHeader("Modules/Physics2D/HingeJoint2D.h")]
	public sealed class HingeJoint2D : AnchoredJoint2D
	{
		public extern bool useMotor { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern bool useLimits { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public JointMotor2D motor
		{
			get
			{
				JointMotor2D result;
				this.get_motor_Injected(out result);
				return result;
			}
			set
			{
				this.set_motor_Injected(ref value);
			}
		}

		public JointAngleLimits2D limits
		{
			get
			{
				JointAngleLimits2D result;
				this.get_limits_Injected(out result);
				return result;
			}
			set
			{
				this.set_limits_Injected(ref value);
			}
		}

		public extern JointLimitState2D limitState { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern float referenceAngle { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern float jointAngle { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern float jointSpeed { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float GetMotorTorque(float timeStep);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_motor_Injected(out JointMotor2D ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_motor_Injected(ref JointMotor2D value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_limits_Injected(out JointAngleLimits2D ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_limits_Injected(ref JointAngleLimits2D value);
	}
}
