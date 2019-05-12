using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	/// <summary>
	///   <para>WebCam Textures are textures onto which the live video input is rendered.</para>
	/// </summary>
	public sealed class WebCamTexture : Texture
	{
		/// <summary>
		///   <para>Create a WebCamTexture.</para>
		/// </summary>
		/// <param name="deviceName">The name of the video input device to be used.</param>
		/// <param name="requestedWidth">The requested width of the texture.</param>
		/// <param name="requestedHeight">The requested height of the texture.</param>
		/// <param name="requestedFPS">The requested frame rate of the texture.</param>
		public WebCamTexture(string deviceName, int requestedWidth, int requestedHeight, int requestedFPS)
		{
			WebCamTexture.Internal_CreateWebCamTexture(this, deviceName, requestedWidth, requestedHeight, requestedFPS);
		}

		/// <summary>
		///   <para>Create a WebCamTexture.</para>
		/// </summary>
		/// <param name="deviceName">The name of the video input device to be used.</param>
		/// <param name="requestedWidth">The requested width of the texture.</param>
		/// <param name="requestedHeight">The requested height of the texture.</param>
		/// <param name="requestedFPS">The requested frame rate of the texture.</param>
		public WebCamTexture(string deviceName, int requestedWidth, int requestedHeight)
		{
			WebCamTexture.Internal_CreateWebCamTexture(this, deviceName, requestedWidth, requestedHeight, 0);
		}

		/// <summary>
		///   <para>Create a WebCamTexture.</para>
		/// </summary>
		/// <param name="deviceName">The name of the video input device to be used.</param>
		/// <param name="requestedWidth">The requested width of the texture.</param>
		/// <param name="requestedHeight">The requested height of the texture.</param>
		/// <param name="requestedFPS">The requested frame rate of the texture.</param>
		public WebCamTexture(string deviceName)
		{
			WebCamTexture.Internal_CreateWebCamTexture(this, deviceName, 0, 0, 0);
		}

		/// <summary>
		///   <para>Create a WebCamTexture.</para>
		/// </summary>
		/// <param name="deviceName">The name of the video input device to be used.</param>
		/// <param name="requestedWidth">The requested width of the texture.</param>
		/// <param name="requestedHeight">The requested height of the texture.</param>
		/// <param name="requestedFPS">The requested frame rate of the texture.</param>
		public WebCamTexture(int requestedWidth, int requestedHeight, int requestedFPS)
		{
			WebCamTexture.Internal_CreateWebCamTexture(this, string.Empty, requestedWidth, requestedHeight, requestedFPS);
		}

		/// <summary>
		///   <para>Create a WebCamTexture.</para>
		/// </summary>
		/// <param name="deviceName">The name of the video input device to be used.</param>
		/// <param name="requestedWidth">The requested width of the texture.</param>
		/// <param name="requestedHeight">The requested height of the texture.</param>
		/// <param name="requestedFPS">The requested frame rate of the texture.</param>
		public WebCamTexture(int requestedWidth, int requestedHeight)
		{
			WebCamTexture.Internal_CreateWebCamTexture(this, string.Empty, requestedWidth, requestedHeight, 0);
		}

		/// <summary>
		///   <para>Create a WebCamTexture.</para>
		/// </summary>
		/// <param name="deviceName">The name of the video input device to be used.</param>
		/// <param name="requestedWidth">The requested width of the texture.</param>
		/// <param name="requestedHeight">The requested height of the texture.</param>
		/// <param name="requestedFPS">The requested frame rate of the texture.</param>
		public WebCamTexture()
		{
			WebCamTexture.Internal_CreateWebCamTexture(this, string.Empty, 0, 0, 0);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_CreateWebCamTexture([Writable] WebCamTexture self, string scriptingDevice, int requestedWidth, int requestedHeight, int maxFramerate);

		/// <summary>
		///   <para>Starts the camera.</para>
		/// </summary>
		public void Play()
		{
			WebCamTexture.INTERNAL_CALL_Play(this);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Play(WebCamTexture self);

		/// <summary>
		///   <para>Pauses the camera.</para>
		/// </summary>
		public void Pause()
		{
			WebCamTexture.INTERNAL_CALL_Pause(this);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Pause(WebCamTexture self);

		/// <summary>
		///   <para>Stops the camera.</para>
		/// </summary>
		public void Stop()
		{
			WebCamTexture.INTERNAL_CALL_Stop(this);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Stop(WebCamTexture self);

		/// <summary>
		///   <para>Returns if the camera is currently playing.</para>
		/// </summary>
		public extern bool isPlaying { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Set this to specify the name of the device to use.</para>
		/// </summary>
		public extern string deviceName { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Set the requested frame rate of the camera device (in frames per second).</para>
		/// </summary>
		public extern float requestedFPS { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Set the requested width of the camera device.</para>
		/// </summary>
		public extern int requestedWidth { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Set the requested height of the camera device.</para>
		/// </summary>
		public extern int requestedHeight { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Return a list of available devices.</para>
		/// </summary>
		public static extern WebCamDevice[] devices { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns pixel color at coordinates (x, y).</para>
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Color GetPixel(int x, int y);

		/// <summary>
		///   <para>Get a block of pixel colors.</para>
		/// </summary>
		public Color[] GetPixels()
		{
			return this.GetPixels(0, 0, this.width, this.height);
		}

		/// <summary>
		///   <para>Get a block of pixel colors.</para>
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="blockWidth"></param>
		/// <param name="blockHeight"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Color[] GetPixels(int x, int y, int blockWidth, int blockHeight);

		/// <summary>
		///   <para>Returns the pixels data in raw format.</para>
		/// </summary>
		/// <param name="colors">Optional array to receive pixel data.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Color32[] GetPixels32([DefaultValue("null")] Color32[] colors);

		[ExcludeFromDocs]
		public Color32[] GetPixels32()
		{
			Color32[] colors = null;
			return this.GetPixels32(colors);
		}

		/// <summary>
		///   <para>Returns an clockwise angle (in degrees), which can be used to rotate a polygon so camera contents are shown in correct orientation.</para>
		/// </summary>
		public extern int videoRotationAngle { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns if the texture image is vertically flipped.</para>
		/// </summary>
		public extern bool videoVerticallyMirrored { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Did the video buffer update this frame?</para>
		/// </summary>
		public extern bool didUpdateThisFrame { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }
	}
}
