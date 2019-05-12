using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Collider for 2D physics representing an arbitrary set of connected edges (lines) defined by its vertices.</para>
	/// </summary>
	public sealed class EdgeCollider2D : Collider2D
	{
		/// <summary>
		///   <para>Reset to a single edge consisting of two points.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Reset();

		/// <summary>
		///   <para>Gets the number of edges.</para>
		/// </summary>
		public extern int edgeCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Gets the number of points.</para>
		/// </summary>
		public extern int pointCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Get or set the points defining multiple continuous edges.</para>
		/// </summary>
		public extern Vector2[] points { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
