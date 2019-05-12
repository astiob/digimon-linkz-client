using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Renders particles on to the screen (Shuriken).</para>
	/// </summary>
	public sealed class ParticleSystemRenderer : Renderer
	{
		/// <summary>
		///   <para>How particles are drawn.</para>
		/// </summary>
		public extern ParticleSystemRenderMode renderMode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>How much are the particles stretched in their direction of motion.</para>
		/// </summary>
		public extern float lengthScale { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>How much are the particles strectched depending on "how fast they move".</para>
		/// </summary>
		public extern float velocityScale { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>How much are the particles strected depending on the Camera's speed.</para>
		/// </summary>
		public extern float cameraVelocityScale { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Clamp the maximum particle size.</para>
		/// </summary>
		public extern float maxParticleSize { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Mesh used as particle instead of billboarded texture.</para>
		/// </summary>
		public extern Mesh mesh { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
