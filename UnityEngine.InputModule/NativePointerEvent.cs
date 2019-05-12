using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UnityEngineInternal.Input
{
	[StructLayout(LayoutKind.Explicit, Size = 80)]
	public struct NativePointerEvent
	{
		[FieldOffset(0)]
		public NativeInputEvent baseEvent;

		[FieldOffset(20)]
		public int pointerId;

		[FieldOffset(24)]
		public Vector3 position;

		[FieldOffset(36)]
		public Vector3 delta;

		[FieldOffset(48)]
		public float pressure;

		[FieldOffset(52)]
		public float twist;

		[FieldOffset(56)]
		public Vector2 tilt;

		[FieldOffset(64)]
		public Vector3 radius;

		[FieldOffset(76)]
		public int displayIndex;

		public static NativePointerEvent Down(int deviceId, double time, int pointerId, Vector3 position, Vector3 delta = default(Vector3), float pressure = 1f, float twist = 1f, Vector2 tilt = default(Vector2), Vector3 radius = default(Vector3), int displayIndex = 0)
		{
			NativePointerEvent result;
			result.baseEvent = new NativeInputEvent(NativeInputEventType.PointerDown, 80, deviceId, time);
			result.pointerId = pointerId;
			result.position = position;
			result.delta = delta;
			result.pressure = pressure;
			result.twist = twist;
			result.tilt = tilt;
			result.radius = radius;
			result.displayIndex = displayIndex;
			return result;
		}

		public static NativePointerEvent Move(int deviceId, double time, int pointerId, Vector3 position, Vector3 delta = default(Vector3), float pressure = 1f, float twist = 1f, Vector2 tilt = default(Vector2), Vector3 radius = default(Vector3), int displayIndex = 0)
		{
			NativePointerEvent result;
			result.baseEvent = new NativeInputEvent(NativeInputEventType.PointerMove, 80, deviceId, time);
			result.pointerId = pointerId;
			result.position = position;
			result.delta = delta;
			result.pressure = pressure;
			result.twist = twist;
			result.tilt = tilt;
			result.radius = radius;
			result.displayIndex = displayIndex;
			return result;
		}

		public static NativePointerEvent Up(int deviceId, double time, int pointerId, Vector3 position, Vector3 delta = default(Vector3), float pressure = 1f, float twist = 1f, Vector2 tilt = default(Vector2), Vector3 radius = default(Vector3), int displayIndex = 0)
		{
			NativePointerEvent result;
			result.baseEvent = new NativeInputEvent(NativeInputEventType.PointerUp, 80, deviceId, time);
			result.pointerId = pointerId;
			result.position = position;
			result.delta = delta;
			result.pressure = pressure;
			result.twist = twist;
			result.tilt = tilt;
			result.radius = radius;
			result.displayIndex = displayIndex;
			return result;
		}

		public static NativePointerEvent Cancelled(int deviceId, double time, int pointerId, Vector3 position, Vector3 delta = default(Vector3), float pressure = 1f, float twist = 1f, Vector2 tilt = default(Vector2), Vector3 radius = default(Vector3), int displayIndex = 0)
		{
			NativePointerEvent result;
			result.baseEvent = new NativeInputEvent(NativeInputEventType.PointerCancelled, 80, deviceId, time);
			result.pointerId = pointerId;
			result.position = position;
			result.delta = delta;
			result.pressure = pressure;
			result.twist = twist;
			result.tilt = tilt;
			result.radius = radius;
			result.displayIndex = displayIndex;
			return result;
		}
	}
}
