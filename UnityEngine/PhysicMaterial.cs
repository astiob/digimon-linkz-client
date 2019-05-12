using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Physics material describes how to handle colliding objects (friction, bounciness).</para>
	/// </summary>
	public sealed class PhysicMaterial : Object
	{
		/// <summary>
		///   <para>Creates a new material.</para>
		/// </summary>
		public PhysicMaterial()
		{
			PhysicMaterial.Internal_CreateDynamicsMaterial(this, null);
		}

		/// <summary>
		///   <para>Creates a new material named name.</para>
		/// </summary>
		/// <param name="name"></param>
		public PhysicMaterial(string name)
		{
			PhysicMaterial.Internal_CreateDynamicsMaterial(this, name);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_CreateDynamicsMaterial([Writable] PhysicMaterial mat, string name);

		/// <summary>
		///   <para>The friction used when already moving.  This value has to be between 0 and 1.</para>
		/// </summary>
		public extern float dynamicFriction { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The friction coefficient used when an object is lying on a surface.</para>
		/// </summary>
		public extern float staticFriction { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>How bouncy is the surface? A value of 0 will not bounce. A value of 1 will bounce without any loss of energy.</para>
		/// </summary>
		public extern float bounciness { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[Obsolete("Use PhysicMaterial.bounciness instead", true)]
		public float bouncyness
		{
			get
			{
				return this.bounciness;
			}
			set
			{
				this.bounciness = value;
			}
		}

		/// <summary>
		///   <para>The direction of anisotropy. Anisotropic friction is enabled if the vector is not zero.</para>
		/// </summary>
		[Obsolete("Anisotropic friction is no longer supported since Unity 5.0.", true)]
		public Vector3 frictionDirection2
		{
			get
			{
				return Vector3.zero;
			}
			set
			{
			}
		}

		/// <summary>
		///   <para>If anisotropic friction is enabled, dynamicFriction2 will be applied along frictionDirection2.</para>
		/// </summary>
		[Obsolete("Anisotropic friction is no longer supported since Unity 5.0.", true)]
		public extern float dynamicFriction2 { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>If anisotropic friction is enabled, staticFriction2 will be applied along frictionDirection2.</para>
		/// </summary>
		[Obsolete("Anisotropic friction is no longer supported since Unity 5.0.", true)]
		public extern float staticFriction2 { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Determines how the friction is combined.</para>
		/// </summary>
		public extern PhysicMaterialCombine frictionCombine { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Determines how the bounciness is combined.</para>
		/// </summary>
		public extern PhysicMaterialCombine bounceCombine { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[Obsolete("Anisotropic friction is no longer supported since Unity 5.0.", true)]
		public Vector3 frictionDirection
		{
			get
			{
				return Vector3.zero;
			}
			set
			{
			}
		}
	}
}
