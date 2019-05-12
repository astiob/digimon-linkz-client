using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngineInternal.Input
{
	[NativeHeader("Modules/Input/Private/InputInternal.h")]
	[NativeHeader("Modules/Input/Private/InputModuleBindings.h")]
	[NativeConditional("ENABLE_NEW_INPUT_SYSTEM")]
	public class NativeInputSystem
	{
		public static NativeUpdateCallback onUpdate;

		public static NativeEventCallback onEvents;

		private static NativeDeviceDiscoveredCallback s_OnDeviceDiscoveredCallback;

		public static event NativeDeviceDiscoveredCallback onDeviceDiscovered
		{
			add
			{
				NativeInputSystem.s_OnDeviceDiscoveredCallback = (NativeDeviceDiscoveredCallback)Delegate.Combine(NativeInputSystem.s_OnDeviceDiscoveredCallback, value);
				NativeInputSystem.hasDeviceDiscoveredCallback = (NativeInputSystem.s_OnDeviceDiscoveredCallback != null);
			}
			remove
			{
				NativeInputSystem.s_OnDeviceDiscoveredCallback = (NativeDeviceDiscoveredCallback)Delegate.Remove(NativeInputSystem.s_OnDeviceDiscoveredCallback, value);
				NativeInputSystem.hasDeviceDiscoveredCallback = (NativeInputSystem.s_OnDeviceDiscoveredCallback != null);
			}
		}

		[RequiredByNativeCode]
		internal static void NotifyUpdate(NativeInputUpdateType updateType)
		{
			NativeUpdateCallback nativeUpdateCallback = NativeInputSystem.onUpdate;
			if (nativeUpdateCallback != null)
			{
				nativeUpdateCallback(updateType);
			}
		}

		[RequiredByNativeCode]
		internal static void NotifyEvents(int eventCount, IntPtr eventData)
		{
			NativeEventCallback nativeEventCallback = NativeInputSystem.onEvents;
			if (nativeEventCallback != null)
			{
				nativeEventCallback(eventCount, eventData);
			}
		}

		[RequiredByNativeCode]
		internal static void NotifyDeviceDiscovered(NativeInputDeviceInfo deviceInfo)
		{
			NativeDeviceDiscoveredCallback nativeDeviceDiscoveredCallback = NativeInputSystem.s_OnDeviceDiscoveredCallback;
			if (nativeDeviceDiscoveredCallback != null)
			{
				nativeDeviceDiscoveredCallback(deviceInfo);
			}
		}

		public static extern double zeroEventTime { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public static extern bool hasDeviceDiscoveredCallback { [MethodImpl(MethodImplOptions.InternalCall)] set; } = false;

		public static void SendInput<TInputEvent>(TInputEvent inputEvent) where TInputEvent : struct
		{
			NativeInputSystem.SendInput(UnsafeUtility.AddressOf<TInputEvent>(ref inputEvent));
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SendInput(IntPtr inputEvent);

		public static bool SendOutput<TOutputEvent>(int deviceId, int type, TOutputEvent outputEvent) where TOutputEvent : struct
		{
			return NativeInputSystem.SendOutput(deviceId, type, UnsafeUtility.SizeOf<TOutputEvent>(), UnsafeUtility.AddressOf<TOutputEvent>(ref outputEvent));
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool SendOutput(int deviceId, int type, int sizeInBytes, IntPtr data);

		public static string GetDeviceConfiguration(int deviceId)
		{
			return null;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetControlConfiguration(int deviceId, int controlIndex);

		public static void SetPollingFrequency(float hertz)
		{
			if (hertz < 1f)
			{
				throw new ArgumentException("Polling frequency cannot be less than 1Hz");
			}
			NativeInputSystem.SetPollingFrequencyInternal(hertz);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetPollingFrequencyInternal(float hertz);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SendEvents();

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Update(NativeInputUpdateType updateType);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int ReportNewInputDevice(string descriptor);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ReportInputDeviceDisconnect(int nativeDeviceId);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ReportInputDeviceReconnect(int nativeDeviceId);
	}
}
