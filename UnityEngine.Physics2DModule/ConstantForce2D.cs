using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	[NativeHeader("Modules/Physics2D/ConstantForce2D.h")]
	[RequireComponent(typeof(Rigidbody2D))]
	public sealed class ConstantForce2D : PhysicsUpdateBehaviour2D
	{
		public Vector2 force
		{
			get
			{
				Vector2 result;
				this.get_force_Injected(out result);
				return result;
			}
			set
			{
				this.set_force_Injected(ref value);
			}
		}

		public Vector2 relativeForce
		{
			get
			{
				Vector2 result;
				this.get_relativeForce_Injected(out result);
				return result;
			}
			set
			{
				this.set_relativeForce_Injected(ref value);
			}
		}

		public extern float torque { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_force_Injected(out Vector2 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_force_Injected(ref Vector2 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_relativeForce_Injected(out Vector2 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_relativeForce_Injected(ref Vector2 value);
	}
}
