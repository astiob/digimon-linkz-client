using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.XR
{
	[RequiredByNativeCode]
	public static class InputTracking
	{
		public static Vector3 GetLocalPosition(XRNode node)
		{
			Vector3 result;
			InputTracking.INTERNAL_CALL_GetLocalPosition(node, out result);
			return result;
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_GetLocalPosition(XRNode node, out Vector3 value);

		public static Quaternion GetLocalRotation(XRNode node)
		{
			Quaternion result;
			InputTracking.INTERNAL_CALL_GetLocalRotation(node, out result);
			return result;
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_GetLocalRotation(XRNode node, out Quaternion value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Recenter();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetNodeName(ulong uniqueID);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetNodeStatesInternal(object nodeStates);

		public static void GetNodeStates(List<XRNodeState> nodeStates)
		{
			if (nodeStates == null)
			{
				throw new ArgumentNullException("nodeStates");
			}
			nodeStates.Clear();
			InputTracking.GetNodeStatesInternal(nodeStates);
		}

		public static extern bool disablePositionalTracking { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<XRNodeState> trackingAcquired;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<XRNodeState> trackingLost;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<XRNodeState> nodeAdded;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<XRNodeState> nodeRemoved;

		[RequiredByNativeCode]
		private static void InvokeTrackingEvent(InputTracking.TrackingStateEventType eventType, XRNode nodeType, long uniqueID, bool tracked)
		{
			XRNodeState obj = default(XRNodeState);
			obj.uniqueID = (ulong)uniqueID;
			obj.nodeType = nodeType;
			obj.tracked = tracked;
			Action<XRNodeState> action;
			switch (eventType)
			{
			case InputTracking.TrackingStateEventType.NodeAdded:
				action = InputTracking.nodeAdded;
				break;
			case InputTracking.TrackingStateEventType.NodeRemoved:
				action = InputTracking.nodeRemoved;
				break;
			case InputTracking.TrackingStateEventType.TrackingAcquired:
				action = InputTracking.trackingAcquired;
				break;
			case InputTracking.TrackingStateEventType.TrackingLost:
				action = InputTracking.trackingLost;
				break;
			default:
				throw new ArgumentException("TrackingEventHandler - Invalid EventType: " + eventType);
			}
			if (action != null)
			{
				action(obj);
			}
		}

		// Note: this type is marked as 'beforefieldinit'.
		static InputTracking()
		{
			InputTracking.trackingAcquired = null;
			InputTracking.trackingLost = null;
			InputTracking.nodeAdded = null;
			InputTracking.nodeRemoved = null;
		}

		private enum TrackingStateEventType
		{
			NodeAdded,
			NodeRemoved,
			TrackingAcquired,
			TrackingLost
		}
	}
}
