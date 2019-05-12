using System;

namespace UnityEngine.VR
{
	public enum VRDeviceType
	{
		None,
		Stereo,
		Split,
		Oculus,
		[Obsolete("Enum member VRDeviceType.Morpheus has been deprecated. Use VRDeviceType.PlayStationVR instead (UnityUpgradable) -> PlayStationVR", true)]
		Morpheus,
		PlayStationVR = 4
	}
}
