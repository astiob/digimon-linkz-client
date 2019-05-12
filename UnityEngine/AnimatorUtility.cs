using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Various utilities for animator manipulation.</para>
	/// </summary>
	public sealed class AnimatorUtility
	{
		/// <summary>
		///   <para>This function will remove all transform hierarchy under GameObject, the animator will write directly transform matrices into the skin mesh matrices saving alot of CPU cycles.</para>
		/// </summary>
		/// <param name="go">GameObject to Optimize.</param>
		/// <param name="exposedTransforms">List of transform name to expose.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void OptimizeTransformHierarchy(GameObject go, string[] exposedTransforms);

		/// <summary>
		///   <para>This function will recreate all transform hierarchy under GameObject.</para>
		/// </summary>
		/// <param name="go">GameObject to Deoptimize.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DeoptimizeTransformHierarchy(GameObject go);
	}
}
