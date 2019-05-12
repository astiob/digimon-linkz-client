using System;

namespace UnityEngine.VR
{
	[Obsolete("VRNode has been moved and renamed.  Use UnityEngine.XR.XRNode instead (UnityUpgradable) -> UnityEngine.XR.XRNode", true)]
	public enum VRNode
	{
		LeftEye,
		RightEye,
		CenterEye,
		Head,
		LeftHand,
		RightHand,
		GameController,
		TrackingReference,
		HardwareTracker
	}
}
