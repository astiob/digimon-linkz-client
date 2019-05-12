using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>A script interface for a.</para>
	/// </summary>
	public sealed class Projector : Behaviour
	{
		/// <summary>
		///   <para>The near clipping plane distance.</para>
		/// </summary>
		public extern float nearClipPlane { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The far clipping plane distance.</para>
		/// </summary>
		public extern float farClipPlane { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The field of view of the projection in degrees.</para>
		/// </summary>
		public extern float fieldOfView { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The aspect ratio of the projection.</para>
		/// </summary>
		public extern float aspectRatio { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Is the projection orthographic (true) or perspective (false)?</para>
		/// </summary>
		public extern bool orthographic { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Projection's half-size when in orthographic mode.</para>
		/// </summary>
		public extern float orthographicSize { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Which object layers are ignored by the projector.</para>
		/// </summary>
		public extern int ignoreLayers { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The material that will be projected onto every object.</para>
		/// </summary>
		public extern Material material { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
