using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class CircleCollider2D : Collider2D
	{
		public extern float radius { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
