using System;
using System.Runtime.CompilerServices;

namespace System.Threading
{
	internal sealed class NativeEventCalls
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr CreateEvent_internal(bool manual, bool initial, string name, out bool created);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool SetEvent_internal(IntPtr handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool ResetEvent_internal(IntPtr handle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void CloseEvent_internal(IntPtr handle);
	}
}
