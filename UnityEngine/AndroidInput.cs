using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>AndroidInput provides support for off-screen touch input, such as a touchpad.</para>
	/// </summary>
	public sealed class AndroidInput
	{
		private AndroidInput()
		{
		}

		/// <summary>
		///   <para>Returns object representing status of a specific touch on a secondary touchpad (Does not allocate temporary variables).</para>
		/// </summary>
		/// <param name="index"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Touch GetSecondaryTouch(int index);

		/// <summary>
		///   <para>Number of secondary touches. Guaranteed not to change throughout the frame. (Read Only).</para>
		/// </summary>
		public static extern int touchCountSecondary { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Property indicating whether the system provides secondary touch input.</para>
		/// </summary>
		public static extern bool secondaryTouchEnabled { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Property indicating the width of the secondary touchpad.</para>
		/// </summary>
		public static extern int secondaryTouchWidth { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Property indicating the height of the secondary touchpad.</para>
		/// </summary>
		public static extern int secondaryTouchHeight { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }
	}
}
