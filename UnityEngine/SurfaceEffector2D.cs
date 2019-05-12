using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Applies tangent forces along the surfaces of colliders.</para>
	/// </summary>
	public sealed class SurfaceEffector2D : Effector2D
	{
		/// <summary>
		///   <para>The speed to be maintained along the surface.</para>
		/// </summary>
		public extern float speed { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The speed variation (from zero to the variation) added to base speed to be applied.</para>
		/// </summary>
		public extern float speedVariation { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The scale of the impulse force applied while attempting to reach the surface speed.</para>
		/// </summary>
		public extern float forceScale { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Should the impulse force but applied to the contact point?</para>
		/// </summary>
		public extern bool useContactForce { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Should friction be used for any contact with the surface?</para>
		/// </summary>
		public extern bool useFriction { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Should bounce be used for any contact with the surface?</para>
		/// </summary>
		public extern bool useBounce { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
