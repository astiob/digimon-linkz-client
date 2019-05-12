using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>A Canvas placable element that can be used to modify children Alpha, Raycasting, Enabled state.</para>
	/// </summary>
	public sealed class CanvasGroup : Component, ICanvasRaycastFilter
	{
		/// <summary>
		///   <para>Set the alpha of the group.</para>
		/// </summary>
		public extern float alpha { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Is the group interactable (are the elements beneath the group enabled).</para>
		/// </summary>
		public extern bool interactable { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Does this group block raycasting (allow collision).</para>
		/// </summary>
		public extern bool blocksRaycasts { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Should the group ignore parent groups?</para>
		/// </summary>
		public extern bool ignoreParentGroups { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Returns true if the Group allows raycasts.</para>
		/// </summary>
		/// <param name="sp"></param>
		/// <param name="eventCamera"></param>
		public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
		{
			return this.blocksRaycasts;
		}
	}
}
