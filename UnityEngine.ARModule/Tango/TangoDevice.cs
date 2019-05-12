using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.XR.Tango
{
	[NativeConditional("PLATFORM_ANDROID")]
	[NativeHeader("Runtime/AR/Tango/TangoScriptApi.h")]
	internal static class TangoDevice
	{
		private static ARBackgroundRenderer m_BackgroundRenderer = null;

		private static string m_AreaDescriptionUUID = "";

		[CompilerGenerated]
		private static Action <>f__mg$cache0;

		[CompilerGenerated]
		private static Action <>f__mg$cache1;

		internal static extern CoordinateFrame baseCoordinateFrame { [NativeConditional(false)] [MethodImpl(MethodImplOptions.InternalCall)] get; [NativeThrows] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool Connect(string[] boolKeys, bool[] boolValues, string[] intKeys, int[] intValues, string[] longKeys, long[] longValues, string[] doubleKeys, double[] doubleValues, string[] stringKeys, string[] stringValues);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Disconnect();

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool TryGetHorizontalFov(out float fovOut);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool TryGetVerticalFov(out float fovOut);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetRenderMode(ARRenderMode mode);

		internal static extern uint depthCameraRate { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		internal static extern bool synchronizeFramerateWithColorCamera { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetBackgroundMaterial(Material material);

		internal static bool TryGetLatestPointCloud(ref PointCloudData pointCloudData)
		{
			if (pointCloudData.points == null)
			{
				pointCloudData.points = new List<Vector4>();
			}
			pointCloudData.points.Clear();
			return TangoDevice.TryGetLatestPointCloudInternal(pointCloudData.points, out pointCloudData.version, out pointCloudData.timestamp);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool TryGetLatestPointCloudInternal(List<Vector4> pointCloudData, out uint version, out double timestamp);

		internal static bool TryGetLatestImageData(ref ImageData image)
		{
			if (image.planeData == null)
			{
				image.planeData = new List<byte>();
			}
			if (image.planeInfos == null)
			{
				image.planeInfos = new List<ImageData.PlaneInfo>();
			}
			image.planeData.Clear();
			return TangoDevice.TryGetLatestImageDataInternal(image.planeData, image.planeInfos, out image.width, out image.height, out image.format, out image.timestampNs, out image.metadata);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool TryGetLatestImageDataInternal(List<byte> imageData, List<ImageData.PlaneInfo> planeInfos, out uint width, out uint height, out int format, out long timestampNs, out ImageData.CameraMetadata metadata);

		internal static extern bool isServiceConnected { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		internal static extern bool isServiceAvailable { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		internal static bool TryAcquireLatestPointCloud(ref NativePointCloud pointCloud)
		{
			return TangoDevice.Internal_TryAcquireLatestPointCloud(out pointCloud.version, out pointCloud.timestamp, out pointCloud.numPoints, out pointCloud.points, out pointCloud.nativePtr);
		}

		internal static void ReleasePointCloud(IntPtr pointCloudNativePtr)
		{
			TangoDevice.Internal_ReleasePointCloud(pointCloudNativePtr);
		}

		internal static bool TryAcquireLatestImageBuffer(ref NativeImage nativeImage)
		{
			if (nativeImage.planeInfos == null)
			{
				nativeImage.planeInfos = new List<ImageData.PlaneInfo>();
			}
			return TangoDevice.Internal_TryAcquireLatestImageBuffer(nativeImage.planeInfos, out nativeImage.width, out nativeImage.height, out nativeImage.format, out nativeImage.timestampNs, out nativeImage.planeData, out nativeImage.nativePtr, out nativeImage.metadata);
		}

		internal static void ReleaseImageBuffer(IntPtr imageBufferNativePtr)
		{
			TangoDevice.Internal_ReleaseImageBuffer(imageBufferNativePtr);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Internal_TryAcquireLatestImageBuffer(List<ImageData.PlaneInfo> planeInfos, out uint width, out uint height, out int format, out long timestampNs, out IntPtr planeData, out IntPtr nativePtr, out ImageData.CameraMetadata metadata);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Internal_TryAcquireLatestPointCloud(out uint version, out double timestamp, out uint numPoints, out IntPtr points, out IntPtr nativePtr);

		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_ReleasePointCloud(IntPtr pointCloudPtr);

		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_ReleaseImageBuffer(IntPtr imageBufferPtr);

		internal static string areaDescriptionUUID
		{
			get
			{
				return TangoDevice.m_AreaDescriptionUUID;
			}
			set
			{
				TangoDevice.m_AreaDescriptionUUID = value;
			}
		}

		internal static ARBackgroundRenderer backgroundRenderer
		{
			get
			{
				return TangoDevice.m_BackgroundRenderer;
			}
			set
			{
				if (value != null)
				{
					if (TangoDevice.m_BackgroundRenderer != null)
					{
						ARBackgroundRenderer backgroundRenderer = TangoDevice.m_BackgroundRenderer;
						if (TangoDevice.<>f__mg$cache0 == null)
						{
							TangoDevice.<>f__mg$cache0 = new Action(TangoDevice.OnBackgroundRendererChanged);
						}
						backgroundRenderer.backgroundRendererChanged -= TangoDevice.<>f__mg$cache0;
					}
					TangoDevice.m_BackgroundRenderer = value;
					ARBackgroundRenderer backgroundRenderer2 = TangoDevice.m_BackgroundRenderer;
					if (TangoDevice.<>f__mg$cache1 == null)
					{
						TangoDevice.<>f__mg$cache1 = new Action(TangoDevice.OnBackgroundRendererChanged);
					}
					backgroundRenderer2.backgroundRendererChanged += TangoDevice.<>f__mg$cache1;
					TangoDevice.OnBackgroundRendererChanged();
				}
			}
		}

		private static void OnBackgroundRendererChanged()
		{
			TangoDevice.SetBackgroundMaterial(TangoDevice.m_BackgroundRenderer.backgroundMaterial);
			TangoDevice.SetRenderMode(TangoDevice.m_BackgroundRenderer.mode);
		}

		internal static bool Connect(TangoConfig config)
		{
			string[] boolKeys;
			bool[] boolValues;
			TangoDevice.CopyDictionaryToArrays<bool>(config.m_boolParams, out boolKeys, out boolValues);
			string[] intKeys;
			int[] intValues;
			TangoDevice.CopyDictionaryToArrays<int>(config.m_intParams, out intKeys, out intValues);
			string[] longKeys;
			long[] longValues;
			TangoDevice.CopyDictionaryToArrays<long>(config.m_longParams, out longKeys, out longValues);
			string[] doubleKeys;
			double[] doubleValues;
			TangoDevice.CopyDictionaryToArrays<double>(config.m_doubleParams, out doubleKeys, out doubleValues);
			string[] stringKeys;
			string[] stringValues;
			TangoDevice.CopyDictionaryToArrays<string>(config.m_stringParams, out stringKeys, out stringValues);
			return TangoDevice.Connect(boolKeys, boolValues, intKeys, intValues, longKeys, longValues, doubleKeys, doubleValues, stringKeys, stringValues);
		}

		private static void CopyDictionaryToArrays<T>(Dictionary<string, T> dictionary, out string[] keys, out T[] values)
		{
			if (dictionary.Count == 0)
			{
				keys = null;
				values = null;
			}
			else
			{
				keys = new string[dictionary.Count];
				values = new T[dictionary.Count];
				int num = 0;
				foreach (KeyValuePair<string, T> keyValuePair in dictionary)
				{
					keys[num] = keyValuePair.Key;
					values[num++] = keyValuePair.Value;
				}
			}
		}
	}
}
