using System;
using System.Runtime.InteropServices;

namespace UnityEngineInternal.Input
{
	[StructLayout(LayoutKind.Explicit, Size = 24)]
	public struct NativeTextEvent
	{
		[FieldOffset(0)]
		public NativeInputEvent baseEvent;

		[FieldOffset(20)]
		public int utf32Character;

		public static NativeTextEvent Character(int deviceId, double time, int utf32)
		{
			NativeTextEvent result;
			result.baseEvent = new NativeInputEvent(NativeInputEventType.Text, 24, deviceId, time);
			result.utf32Character = utf32;
			return result;
		}
	}
}
