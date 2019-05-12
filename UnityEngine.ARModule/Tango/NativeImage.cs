using System;
using System.Collections.Generic;

namespace UnityEngine.XR.Tango
{
	internal struct NativeImage
	{
		public uint width;

		public uint height;

		public int format;

		public long timestampNs;

		public IntPtr planeData;

		public IntPtr nativePtr;

		public List<ImageData.PlaneInfo> planeInfos;

		public ImageData.CameraMetadata metadata;
	}
}
