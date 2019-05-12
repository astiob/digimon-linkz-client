using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Rendering
{
	[UsedByNativeCode]
	public struct CameraProperties
	{
		private const int kNumLayers = 32;

		private Rect screenRect;

		private Vector3 viewDir;

		private float projectionNear;

		private float projectionFar;

		private float cameraNear;

		private float cameraFar;

		private float cameraAspect;

		private Matrix4x4 cameraToWorld;

		private Matrix4x4 actualWorldToClip;

		private Matrix4x4 cameraClipToWorld;

		private Matrix4x4 cameraWorldToClip;

		private Matrix4x4 implicitProjection;

		private Matrix4x4 stereoWorldToClipLeft;

		private Matrix4x4 stereoWorldToClipRight;

		private Matrix4x4 worldToCamera;

		private Vector3 up;

		private Vector3 right;

		private Vector3 transformDirection;

		private Vector3 cameraEuler;

		private Vector3 velocity;

		private float farPlaneWorldSpaceLength;

		private uint rendererCount;

		private CameraProperties.<_shadowCullPlanes>__FixedBuffer1 _shadowCullPlanes;

		private CameraProperties.<_cameraCullPlanes>__FixedBuffer2 _cameraCullPlanes;

		private float baseFarDistance;

		private Vector3 shadowCullCenter;

		private CameraProperties.<layerCullDistances>__FixedBuffer3 layerCullDistances;

		private int layerCullSpherical;

		private CoreCameraValues coreCameraValues;

		private uint cameraType;

		public unsafe Plane GetShadowCullingPlane(int index)
		{
			if (index < 0 || index >= 6)
			{
				throw new IndexOutOfRangeException("Invalid plane index");
			}
			fixed (float* ptr = &this._shadowCullPlanes.FixedElementField)
			{
				return new Plane(new Vector3(ptr[(IntPtr)(index * 4) * 4], ptr[(IntPtr)(index * 4 + 1) * 4], ptr[(IntPtr)(index * 4 + 2) * 4]), ptr[(IntPtr)(index * 4 + 3) * 4]);
			}
		}

		public unsafe void SetShadowCullingPlane(int index, Plane plane)
		{
			if (index < 0 || index >= 6)
			{
				throw new IndexOutOfRangeException("Invalid plane index");
			}
			fixed (float* ptr = &this._shadowCullPlanes.FixedElementField)
			{
				ptr[(IntPtr)(index * 4) * 4] = plane.normal.x;
				ptr[(IntPtr)(index * 4 + 1) * 4] = plane.normal.y;
				ptr[(IntPtr)(index * 4 + 2) * 4] = plane.normal.z;
				ptr[(IntPtr)(index * 4 + 3) * 4] = plane.distance;
			}
		}

		public unsafe Plane GetCameraCullingPlane(int index)
		{
			if (index < 0 || index >= 6)
			{
				throw new IndexOutOfRangeException("Invalid plane index");
			}
			fixed (float* ptr = &this._cameraCullPlanes.FixedElementField)
			{
				return new Plane(new Vector3(ptr[(IntPtr)(index * 4) * 4], ptr[(IntPtr)(index * 4 + 1) * 4], ptr[(IntPtr)(index * 4 + 2) * 4]), ptr[(IntPtr)(index * 4 + 3) * 4]);
			}
		}

		public unsafe void SetCameraCullingPlane(int index, Plane plane)
		{
			if (index < 0 || index >= 6)
			{
				throw new IndexOutOfRangeException("Invalid plane index");
			}
			fixed (float* ptr = &this._cameraCullPlanes.FixedElementField)
			{
				ptr[(IntPtr)(index * 4) * 4] = plane.normal.x;
				ptr[(IntPtr)(index * 4 + 1) * 4] = plane.normal.y;
				ptr[(IntPtr)(index * 4 + 2) * 4] = plane.normal.z;
				ptr[(IntPtr)(index * 4 + 3) * 4] = plane.distance;
			}
		}

		[UnsafeValueType]
		[CompilerGenerated]
		[StructLayout(LayoutKind.Sequential, Size = 96)]
		public struct <_shadowCullPlanes>__FixedBuffer1
		{
			public float FixedElementField;
		}

		[UnsafeValueType]
		[CompilerGenerated]
		[StructLayout(LayoutKind.Sequential, Size = 96)]
		public struct <_cameraCullPlanes>__FixedBuffer2
		{
			public float FixedElementField;
		}

		[UnsafeValueType]
		[CompilerGenerated]
		[StructLayout(LayoutKind.Sequential, Size = 128)]
		public struct <layerCullDistances>__FixedBuffer3
		{
			public float FixedElementField;
		}
	}
}
