using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UnityEngineInternal.Input
{
	[StructLayout(LayoutKind.Explicit, Size = 24)]
	public struct NativeKeyEvent
	{
		[FieldOffset(0)]
		public NativeInputEvent baseEvent;

		[FieldOffset(20)]
		public KeyCode key;

		public static NativeKeyEvent Down(int deviceId, double time, KeyCode key)
		{
			NativeKeyEvent result;
			result.baseEvent = new NativeInputEvent(NativeInputEventType.KeyDown, 24, deviceId, time);
			result.key = key;
			return result;
		}

		public static NativeKeyEvent Up(int deviceId, double time, KeyCode key)
		{
			NativeKeyEvent result;
			result.baseEvent = new NativeInputEvent(NativeInputEventType.KeyUp, 24, deviceId, time);
			result.key = key;
			return result;
		}
	}
}
