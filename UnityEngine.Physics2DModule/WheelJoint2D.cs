using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	[NativeHeader("Modules/Physics2D/WheelJoint2D.h")]
	public sealed class WheelJoint2D : AnchoredJoint2D
	{
		public JointSuspension2D suspension
		{
			get
			{
				JointSuspension2D result;
				this.get_suspension_Injected(out result);
				return result;
			}
			set
			{
				this.set_suspension_Injected(ref value);
			}
		}

		public extern bool useMotor { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

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

		public extern float jointTranslation { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern float jointLinearSpeed { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern float jointSpeed { [NativeMethod("GetJointAngularSpeed")] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern float jointAngle { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float GetMotorTorque(float timeStep);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_suspension_Injected(out JointSuspension2D ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_suspension_Injected(ref JointSuspension2D value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_motor_Injected(out JointMotor2D ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_motor_Injected(ref JointMotor2D value);
	}
}
