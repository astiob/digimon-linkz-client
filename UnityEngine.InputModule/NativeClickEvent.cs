using System;
using System.Runtime.InteropServices;

namespace UnityEngineInternal.Input
{
	[StructLayout(LayoutKind.Explicit, Size = 32)]
	public struct NativeClickEvent
	{
		[FieldOffset(0)]
		public NativeInputEvent baseEvent;

		[FieldOffset(20)]
		public bool isPressed;

		[FieldOffset(24)]
		public int controlIndex;

		[FieldOffset(28)]
		public int clickCount;

		public static NativeClickEvent Press(int deviceId, double time, int controlIndex, int clickCount)
		{
			NativeClickEvent result;
			result.baseEvent = new NativeInputEvent(NativeInputEventType.Click, 32, deviceId, time);
			result.isPressed = true;
			result.controlIndex = controlIndex;
			result.clickCount = clickCount;
			return result;
		}

		public static NativeClickEvent Release(int deviceId, double time, int controlIndex, int clickCount)
		{
			NativeClickEvent result;
			result.baseEvent = new NativeInputEvent(NativeInputEventType.Click, 32, deviceId, time);
			result.isPressed = false;
			result.controlIndex = controlIndex;
			result.clickCount = clickCount;
			return result;
		}
	}
}
