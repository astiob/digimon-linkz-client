using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Asset type that defines the surface properties of a Collider2D.</para>
	/// </summary>
	public sealed class PhysicsMaterial2D : Object
	{
		public PhysicsMaterial2D()
		{
			PhysicsMaterial2D.Internal_Create(this, null);
		}

		public PhysicsMaterial2D(string name)
		{
			PhysicsMaterial2D.Internal_Create(this, name);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_Create([Writable] PhysicsMaterial2D mat, string name);

		/// <summary>
		///   <para>The degree of elasticity during collisions.</para>
		/// </summary>
		public extern float bounciness { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Coefficient of friction.</para>
		/// </summary>
		public extern float friction { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
