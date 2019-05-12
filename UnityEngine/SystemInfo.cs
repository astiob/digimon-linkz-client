using System;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;

namespace UnityEngine
{
	/// <summary>
	///   <para>Access system information.</para>
	/// </summary>
	public sealed class SystemInfo
	{
		/// <summary>
		///   <para>Operating system name with version (Read Only).</para>
		/// </summary>
		public static extern string operatingSystem { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Processor name (Read Only).</para>
		/// </summary>
		public static extern string processorType { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Number of processors present (Read Only).</para>
		/// </summary>
		public static extern int processorCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Amount of system memory present (Read Only).</para>
		/// </summary>
		public static extern int systemMemorySize { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Amount of video memory present (Read Only).</para>
		/// </summary>
		public static extern int graphicsMemorySize { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The name of the graphics device (Read Only).</para>
		/// </summary>
		public static extern string graphicsDeviceName { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The vendor of the graphics device (Read Only).</para>
		/// </summary>
		public static extern string graphicsDeviceVendor { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The identifier code of the graphics device (Read Only).</para>
		/// </summary>
		public static extern int graphicsDeviceID { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The identifier code of the graphics device vendor (Read Only).</para>
		/// </summary>
		public static extern int graphicsDeviceVendorID { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The graphics API type used by the graphics device (Read Only).</para>
		/// </summary>
		public static extern GraphicsDeviceType graphicsDeviceType { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The graphics API type and driver version used by the graphics device (Read Only).</para>
		/// </summary>
		public static extern string graphicsDeviceVersion { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Graphics device shader capability level (Read Only).</para>
		/// </summary>
		public static extern int graphicsShaderLevel { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[Obsolete("graphicsPixelFillrate is no longer supported in Unity 5.0+.")]
		public static int graphicsPixelFillrate
		{
			get
			{
				return -1;
			}
		}

		[Obsolete("Vertex program support is required in Unity 5.0+")]
		public static bool supportsVertexPrograms
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		///   <para>Is graphics device using multi-threaded rendering (Read Only)?</para>
		/// </summary>
		public static extern bool graphicsMultiThreaded { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Are built-in shadows supported? (Read Only)</para>
		/// </summary>
		public static extern bool supportsShadows { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Are render textures supported? (Read Only)</para>
		/// </summary>
		public static extern bool supportsRenderTextures { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Are cubemap render textures supported? (Read Only)</para>
		/// </summary>
		public static extern bool supportsRenderToCubemap { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Are image effects supported? (Read Only)</para>
		/// </summary>
		public static extern bool supportsImageEffects { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Are 3D (volume) textures supported? (Read Only)</para>
		/// </summary>
		public static extern bool supports3DTextures { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Are compute shaders supported? (Read Only)</para>
		/// </summary>
		public static extern bool supportsComputeShaders { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Is GPU draw call instancing supported? (Read Only)</para>
		/// </summary>
		public static extern bool supportsInstancing { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Are sparse textures supported? (Read Only)</para>
		/// </summary>
		public static extern bool supportsSparseTextures { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>How many simultaneous render targets (MRTs) are supported? (Read Only)</para>
		/// </summary>
		public static extern int supportedRenderTargetCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Is the stencil buffer supported? (Read Only)</para>
		/// </summary>
		public static extern int supportsStencil { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Is render texture format supported?</para>
		/// </summary>
		/// <param name="format">The format to look up.</param>
		/// <returns>
		///   <para>True if the format is supported.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool SupportsRenderTextureFormat(RenderTextureFormat format);

		/// <summary>
		///   <para>Is texture format supported on this device?</para>
		/// </summary>
		/// <param name="format">The TextureFormat format to look up.</param>
		/// <returns>
		///   <para>True if the format is supported.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool SupportsTextureFormat(TextureFormat format);

		/// <summary>
		///   <para>What NPOT (ie, non-power of two resolution) support does the GPU provide? (Read Only)</para>
		/// </summary>
		public static extern NPOTSupport npotSupport { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>A unique device identifier. It is guaranteed to be unique for every device (Read Only).</para>
		/// </summary>
		public static extern string deviceUniqueIdentifier { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The user defined name of the device (Read Only).</para>
		/// </summary>
		public static extern string deviceName { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The model of the device (Read Only).</para>
		/// </summary>
		public static extern string deviceModel { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Is an accelerometer available on the device?</para>
		/// </summary>
		public static extern bool supportsAccelerometer { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Is a gyroscope available on the device?</para>
		/// </summary>
		public static extern bool supportsGyroscope { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Is the device capable of reporting its location?</para>
		/// </summary>
		public static extern bool supportsLocationService { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Is the device capable of providing the user haptic feedback by vibration?</para>
		/// </summary>
		public static extern bool supportsVibration { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns the kind of device the application is running on.</para>
		/// </summary>
		public static extern DeviceType deviceType { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public static extern int maxTextureSize { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }
	}
}
