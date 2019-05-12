using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Applies both linear and angular (torque) forces continuously to the rigidbody each physics update.</para>
	/// </summary>
	public sealed class ConstantForce2D : PhysicsUpdateBehaviour2D
	{
		/// <summary>
		///   <para>The linear force applied to the rigidbody each physics update.</para>
		/// </summary>
		public Vector2 force
		{
			get
			{
				Vector2 result;
				this.INTERNAL_get_force(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_force(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_force(out Vector2 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_force(ref Vector2 value);

		/// <summary>
		///   <para>The linear force, relative to the rigid-body coordinate system, applied each physics update.</para>
		/// </summary>
		public Vector2 relativeForce
		{
			get
			{
				Vector2 result;
				this.INTERNAL_get_relativeForce(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_relativeForce(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_relativeForce(out Vector2 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_relativeForce(ref Vector2 value);

		/// <summary>
		///   <para>The torque applied to the rigidbody each physics update.</para>
		/// </summary>
		public extern float torque { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
