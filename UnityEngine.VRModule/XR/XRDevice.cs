using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.XR
{
	public static class XRDevice
	{
		public static extern bool isPresent { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public static extern UserPresenceState userPresence { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[Obsolete("family is deprecated.  Use XRSettings.loadedDeviceName instead.")]
		public static extern string family { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public static extern string model { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public static extern float refreshRate { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern TrackingSpaceType GetTrackingSpaceType();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool SetTrackingSpaceType(TrackingSpaceType trackingSpaceType);

		public static IntPtr GetNativePtr()
		{
			IntPtr result;
			XRDevice.INTERNAL_CALL_GetNativePtr(out result);
			return result;
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_GetNativePtr(out IntPtr value);

		public static void DisableAutoXRCameraTracking(Camera camera, bool disabled)
		{
			if (camera == null)
			{
				throw new ArgumentNullException("camera");
			}
			XRDevice.DisableAutoXRCameraTrackingInternal(camera, disabled);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DisableAutoXRCameraTrackingInternal(Camera camera, bool disabled);

		public static extern float fovZoomFactor { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
