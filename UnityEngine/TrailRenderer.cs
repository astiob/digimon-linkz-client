using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>The trail renderer is used to make trails behind objects in the scene as they move about.</para>
	/// </summary>
	public sealed class TrailRenderer : Renderer
	{
		/// <summary>
		///   <para>How long does the trail take to fade out.</para>
		/// </summary>
		public extern float time { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The width of the trail at the spawning point.</para>
		/// </summary>
		public extern float startWidth { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The width of the trail at the end of the trail.</para>
		/// </summary>
		public extern float endWidth { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Does the GameObject of this trail renderer auto destructs?</para>
		/// </summary>
		public extern bool autodestruct { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
