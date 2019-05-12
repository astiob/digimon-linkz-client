using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Applies "platform" behaviour such as one-way collisions etc.</para>
	/// </summary>
	public sealed class PlatformEffector2D : Effector2D
	{
		/// <summary>
		///   <para>Should the one-way collision behaviour be used?</para>
		/// </summary>
		public extern bool useOneWay { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Should friction be used on the platform sides?</para>
		/// </summary>
		public extern bool useSideFriction { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Should bounce be used on the platform sides?</para>
		/// </summary>
		public extern bool useSideBounce { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The angle of an arc that defines the surface of the platform centered of the local 'up' of the effector.</para>
		/// </summary>
		public extern float surfaceArc { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The angle of an arc that defines the sides of the platform centered on the local 'left' and 'right' of the effector. Any collision normals within this arc are considered for the 'side' behaviours.</para>
		/// </summary>
		public extern float sideArc { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
