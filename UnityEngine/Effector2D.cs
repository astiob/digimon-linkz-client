using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>A base class for all 2D effectors.</para>
	/// </summary>
	public class Effector2D : Behaviour
	{
		/// <summary>
		///   <para>Should the collider-mask be used or the global collision matrix?</para>
		/// </summary>
		public extern bool useColliderMask { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The mask used to select specific layers allowed to interact with the effector.</para>
		/// </summary>
		public extern int colliderMask { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		internal extern bool requiresCollider { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		internal extern bool designedForTrigger { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		internal extern bool designedForNonTrigger { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }
	}
}
