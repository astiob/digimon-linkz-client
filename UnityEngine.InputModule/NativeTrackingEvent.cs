using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UnityEngineInternal.Input
{
	[StructLayout(LayoutKind.Explicit, Size = 104)]
	public struct NativeTrackingEvent
	{
		[FieldOffset(0)]
		public NativeInputEvent baseEvent;

		[FieldOffset(20)]
		public int nodeId;

		[FieldOffset(24)]
		public NativeTrackingEvent.Flags availableFields;

		[FieldOffset(28)]
		public Vector3 localPosition;

		[FieldOffset(40)]
		public Quaternion localRotation;

		[FieldOffset(56)]
		public Vector3 velocity;

		[FieldOffset(68)]
		public Vector3 angularVelocity;

		[FieldOffset(80)]
		public Vector3 acceleration;

		[FieldOffset(92)]
		public Vector3 angularAcceleration;

		public static NativeTrackingEvent Create(int deviceId, double time, int nodeId, Vector3 position, Quaternion rotation)
		{
			return new NativeTrackingEvent
			{
				baseEvent = new NativeInputEvent(NativeInputEventType.Tracking, 104, deviceId, time),
				nodeId = nodeId,
				availableFields = (NativeTrackingEvent.Flags.PositionAvailable | NativeTrackingEvent.Flags.OrientationAvailable),
				localPosition = position,
				localRotation = rotation
			};
		}

		[Flags]
		public enum Flags : uint
		{
			PositionAvailable = 1u,
			OrientationAvailable = 2u,
			VelocityAvailable = 4u,
			AngularVelocityAvailable = 8u,
			AccelerationAvailable = 16u,
			AngularAccelerationAvailable = 32u
		}
	}
}
