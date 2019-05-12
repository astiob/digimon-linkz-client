using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.VR
{
	/// <summary>
	///   <para>VR Input tracking data.</para>
	/// </summary>
	public sealed class InputTracking
	{
		/// <summary>
		///   <para>The current position of the requested VRNode.</para>
		/// </summary>
		/// <param name="node">Node index.</param>
		/// <returns>
		///   <para>Position of node local to its tracking space.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Vector3 GetLocalPosition(VRNode node);

		/// <summary>
		///   <para>The current rotation of the requested VRNode.</para>
		/// </summary>
		/// <param name="node">Node index.</param>
		/// <returns>
		///   <para>Rotation of node local to its tracking space.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Quaternion GetLocalRotation(VRNode node);

		/// <summary>
		///   <para>Center tracking on the current pose.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Recenter();
	}
}
