using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class EdgeCollider2D : Collider2D
	{
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Reset();

		public extern int edgeCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern int pointCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern Vector2[] points { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
