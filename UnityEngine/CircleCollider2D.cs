using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Collider for 2D physics representing an circle.</para>
	/// </summary>
	public sealed class CircleCollider2D : Collider2D
	{
		/// <summary>
		///   <para>Radius of the circle.</para>
		/// </summary>
		public extern float radius { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
