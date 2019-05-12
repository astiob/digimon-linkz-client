using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.XR.Tango
{
	[NativeConditional("PLATFORM_ANDROID")]
	[UsedByNativeCode]
	[NativeHeader("Runtime/AR/Tango/TangoScriptApi.h")]
	internal static class TangoInputTracking
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Internal_TryGetPoseAtTime(double time, ScreenOrientation screenOrientation, CoordinateFrame baseFrame, CoordinateFrame targetFrame, out PoseData pose);

		internal static bool TryGetPoseAtTime(out PoseData pose, CoordinateFrame baseFrame, CoordinateFrame targetFrame, double time, ScreenOrientation screenOrientation)
		{
			return TangoInputTracking.Internal_TryGetPoseAtTime(time, screenOrientation, baseFrame, targetFrame, out pose);
		}

		internal static bool TryGetPoseAtTime(out PoseData pose, CoordinateFrame baseFrame, CoordinateFrame targetFrame, double time = 0.0)
		{
			return TangoInputTracking.Internal_TryGetPoseAtTime(time, Screen.orientation, baseFrame, targetFrame, out pose);
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal static event Action<CoordinateFrame> trackingAcquired;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal static event Action<CoordinateFrame> trackingLost;

		[UsedByNativeCode]
		private static void InvokeTangoTrackingEvent(TangoInputTracking.TrackingStateEventType eventType, CoordinateFrame frame)
		{
			Action<CoordinateFrame> action;
			if (eventType != TangoInputTracking.TrackingStateEventType.TrackingAcquired)
			{
				if (eventType != TangoInputTracking.TrackingStateEventType.TrackingLost)
				{
					throw new ArgumentException("TrackingEventHandler - Invalid EventType: " + eventType);
				}
				action = TangoInputTracking.trackingLost;
			}
			else
			{
				action = TangoInputTracking.trackingAcquired;
			}
			if (action != null)
			{
				action(frame);
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static TangoInputTracking()
		{
			TangoInputTracking.trackingAcquired = null;
			TangoInputTracking.trackingLost = null;
		}

		private enum TrackingStateEventType
		{
			TrackingAcquired,
			TrackingLost
		}
	}
}
