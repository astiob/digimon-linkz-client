using System;
using UnityEngine.XR;

namespace UnityEngine.VR
{
	[Obsolete("VRDevice has been moved and renamed.  Use UnityEngine.XR.XRDevice instead (UnityUpgradable) -> UnityEngine.XR.XRDevice", true)]
	public static class VRDevice
	{
		public static bool isPresent
		{
			get
			{
				throw new NotSupportedException("VRDevice has been moved and renamed.  Use UnityEngine.XR.XRDevice instead.");
			}
		}

		public static UserPresenceState userPresence
		{
			get
			{
				throw new NotSupportedException("VRDevice has been moved and renamed.  Use UnityEngine.XR.XRDevice instead.");
			}
		}

		[Obsolete("family is deprecated.  Use XRSettings.loadedDeviceName instead.", true)]
		public static string family
		{
			get
			{
				throw new NotSupportedException("VRDevice has been moved and renamed.  Use UnityEngine.XR.XRDevice instead.");
			}
		}

		public static string model
		{
			get
			{
				throw new NotSupportedException("VRDevice has been moved and renamed.  Use UnityEngine.XR.XRDevice instead.");
			}
		}

		public static float refreshRate
		{
			get
			{
				throw new NotSupportedException("VRDevice has been moved and renamed.  Use UnityEngine.XR.XRDevice instead.");
			}
		}

		public static TrackingSpaceType GetTrackingSpaceType()
		{
			throw new NotSupportedException("VRDevice has been moved and renamed.  Use UnityEngine.XR.XRDevice instead.");
		}

		public static bool SetTrackingSpaceType(TrackingSpaceType trackingSpaceType)
		{
			throw new NotSupportedException("VRDevice has been moved and renamed.  Use UnityEngine.XR.XRDevice instead.");
		}

		public static IntPtr GetNativePtr()
		{
			throw new NotSupportedException("VRDevice has been moved and renamed.  Use UnityEngine.XR.XRDevice instead.");
		}

		[Obsolete("DisableAutoVRCameraTracking has been moved and renamed.  Use UnityEngine.XR.XRDevice.DisableAutoXRCameraTracking instead (UnityUpgradable) -> UnityEngine.XR.XRDevice.DisableAutoXRCameraTracking(*)", true)]
		public static void DisableAutoVRCameraTracking(Camera camera, bool disabled)
		{
			throw new NotSupportedException("VRDevice has been moved and renamed.  Use UnityEngine.XR.XRDevice instead.");
		}
	}
}
