using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>(Legacy Particles) Particle animators move your particles over time, you use them to apply wind, drag &amp; color cycling to your particle emitters.</para>
	/// </summary>
	public sealed class ParticleAnimator : Component
	{
		/// <summary>
		///   <para>Do particles cycle their color over their lifetime?</para>
		/// </summary>
		public extern bool doesAnimateColor { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>World space axis the particles rotate around.</para>
		/// </summary>
		public Vector3 worldRotationAxis
		{
			get
			{
				Vector3 result;
				this.INTERNAL_get_worldRotationAxis(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_worldRotationAxis(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_worldRotationAxis(out Vector3 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_worldRotationAxis(ref Vector3 value);

		/// <summary>
		///   <para>Local space axis the particles rotate around.</para>
		/// </summary>
		public Vector3 localRotationAxis
		{
			get
			{
				Vector3 result;
				this.INTERNAL_get_localRotationAxis(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_localRotationAxis(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_localRotationAxis(out Vector3 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_localRotationAxis(ref Vector3 value);

		/// <summary>
		///   <para>How the particle sizes grow over their lifetime.</para>
		/// </summary>
		public extern float sizeGrow { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>A random force added to particles every frame.</para>
		/// </summary>
		public Vector3 rndForce
		{
			get
			{
				Vector3 result;
				this.INTERNAL_get_rndForce(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_rndForce(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_rndForce(out Vector3 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_rndForce(ref Vector3 value);

		/// <summary>
		///   <para>The force being applied to particles every frame.</para>
		/// </summary>
		public Vector3 force
		{
			get
			{
				Vector3 result;
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
		private extern void INTERNAL_get_force(out Vector3 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_force(ref Vector3 value);

		/// <summary>
		///   <para>How much particles are slowed down every frame.</para>
		/// </summary>
		public extern float damping { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Does the GameObject of this particle animator auto destructs?</para>
		/// </summary>
		public extern bool autodestruct { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Colors the particles will cycle through over their lifetime.</para>
		/// </summary>
		public extern Color[] colorAnimation { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
