using System;

namespace UnityEngine.XR.Tango
{
	internal struct NativePointCloud
	{
		public uint version;

		public double timestamp;

		public uint numPoints;

		public IntPtr points;

		public IntPtr nativePtr;
	}
}
