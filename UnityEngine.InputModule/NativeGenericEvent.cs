using System;
using System.Runtime.InteropServices;

namespace UnityEngineInternal.Input
{
	[StructLayout(LayoutKind.Explicit, Size = 36)]
	public struct NativeGenericEvent
	{
		[FieldOffset(0)]
		public NativeInputEvent baseEvent;

		[FieldOffset(20)]
		public int controlIndex;

		[FieldOffset(24)]
		public int rawValue;

		[FieldOffset(28)]
		public double scaledValue;

		public static NativeGenericEvent Value(int deviceId, double time, int controlIndex, int rawValue, double scaledValue)
		{
			NativeGenericEvent result;
			result.baseEvent = new NativeInputEvent(NativeInputEventType.Generic, 36, deviceId, time);
			result.controlIndex = controlIndex;
			result.rawValue = rawValue;
			result.scaledValue = scaledValue;
			return result;
		}
	}
}
