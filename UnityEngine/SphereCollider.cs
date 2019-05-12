using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>A sphere-shaped primitive collider.</para>
	/// </summary>
	public sealed class SphereCollider : Collider
	{
		/// <summary>
		///   <para>The center of the sphere in the object's local space.</para>
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
		///   <para>The radius of the sphere measured in the object's local space.</para>
		/// </summary>
		public extern float radius { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
