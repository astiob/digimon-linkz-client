using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	public sealed class AndroidInput
	{
		private AndroidInput()
		{
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Touch GetSecondaryTouch(int index);

		public static extern int touchCountSecondary { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public static extern bool secondaryTouchEnabled { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public static extern int secondaryTouchWidth { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public static extern int secondaryTouchHeight { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }
	}
}
