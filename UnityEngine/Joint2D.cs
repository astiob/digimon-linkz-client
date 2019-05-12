using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Parent class for joints to connect Rigidbody2D objects.</para>
	/// </summary>
	public class Joint2D : Behaviour
	{
		/// <summary>
		///   <para>The Rigidbody2D object to which the other end of the joint is attached (ie, the object without the joint component).</para>
		/// </summary>
		public extern Rigidbody2D connectedBody { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Should rigid bodies connected with this joint collide?</para>
		/// </summary>
		public extern bool enableCollision { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
