using System;

namespace UnityEngine.XR.Tango
{
	internal enum CoordinateFrame
	{
		GlobalWGS84,
		AreaDescription,
		StartOfService,
		PreviousDevicePose,
		Device,
		IMU,
		Display,
		CameraColor,
		CameraDepth,
		CameraFisheye,
		UUID,
		Invalid,
		MaxCoordinateFrameType
	}
}
