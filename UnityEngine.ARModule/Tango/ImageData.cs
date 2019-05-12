using System;
using System.Collections.Generic;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.XR.Tango
{
	internal struct ImageData
	{
		public uint width;

		public uint height;

		public int format;

		public long timestampNs;

		public List<byte> planeData;

		public List<ImageData.PlaneInfo> planeInfos;

		public ImageData.CameraMetadata metadata;

		[NativeHeader("Runtime/AR/Tango/TangoScriptApi.h")]
		[UsedByNativeCode]
		public struct PlaneInfo
		{
			public int size;

			public int rowStride;

			public int pixelStride;

			public uint offset;
		}

		public struct CameraMetadata
		{
			public long timestampNs;

			public long frameNumber;

			public long exposureDurationNs;

			public int sensitivityIso;

			public float lensAperture;

			public int colorCorrectionMode;

			public float colorCorrectionGains0;

			public float colorCorrectionGains1;

			public float colorCorrectionGains2;

			public float colorCorrectionGains3;

			public float colorCorrectionTransform0;

			public float colorCorrectionTransform1;

			public float colorCorrectionTransform2;

			public float colorCorrectionTransform3;

			public float colorCorrectionTransform4;

			public float colorCorrectionTransform5;

			public float colorCorrectionTransform6;

			public float colorCorrectionTransform7;

			public float colorCorrectionTransform8;

			public float sensorNeutralColorPoint0;

			public float sensorNeutralColorPoint1;

			public float sensorNeutralColorPoint2;
		}
	}
}
