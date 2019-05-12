using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	[RequireComponent(typeof(Transform), typeof(Rigidbody2D))]
	[NativeHeader("Modules/Physics2D/Joint2D.h")]
	public class Joint2D : Behaviour
	{
		public extern Rigidbody2D attachedRigidbody { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern Rigidbody2D connectedBody { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern bool enableCollision { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern float breakForce { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern float breakTorque { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public Vector2 reactionForce
		{
			[NativeMethod("GetReactionForceFixedTime")]
			get
			{
				Vector2 result;
				this.get_reactionForce_Injected(out result);
				return result;
			}
		}

		public extern float reactionTorque { [NativeMethod("GetReactionTorqueFixedTime")] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public Vector2 GetReactionForce(float timeStep)
		{
			Vector2 result;
			this.GetReactionForce_Injected(timeStep, out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float GetReactionTorque(float timeStep);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_reactionForce_Injected(out Vector2 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetReactionForce_Injected(float timeStep, out Vector2 ret);
	}
}
