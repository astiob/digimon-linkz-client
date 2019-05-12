using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	/// <summary>
	///   <para>Render textures are textures that can be rendered to.</para>
	/// </summary>
	public sealed class RenderTexture : Texture
	{
		/// <summary>
		///   <para>Creates a new RenderTexture object.</para>
		/// </summary>
		/// <param name="width">Texture width in pixels.</param>
		/// <param name="height">Texture height in pixels.</param>
		/// <param name="depth">Number of bits in depth buffer (0, 16 or 24). Note that only 24 bit depth has stencil buffer.</param>
		/// <param name="format">Texture color format.</param>
		/// <param name="readWrite">How or if color space conversions should be done on texture read/write.</param>
		public RenderTexture(int width, int height, int depth, RenderTextureFormat format, RenderTextureReadWrite readWrite)
		{
			RenderTexture.Internal_CreateRenderTexture(this);
			this.width = width;
			this.height = height;
			this.depth = depth;
			this.format = format;
			bool sRGB = readWrite == RenderTextureReadWrite.sRGB;
			if (readWrite == RenderTextureReadWrite.Default)
			{
				sRGB = (QualitySettings.activeColorSpace == ColorSpace.Linear);
			}
			RenderTexture.Internal_SetSRGBReadWrite(this, sRGB);
		}

		/// <summary>
		///   <para>Creates a new RenderTexture object.</para>
		/// </summary>
		/// <param name="width">Texture width in pixels.</param>
		/// <param name="height">Texture height in pixels.</param>
		/// <param name="depth">Number of bits in depth buffer (0, 16 or 24). Note that only 24 bit depth has stencil buffer.</param>
		/// <param name="format">Texture color format.</param>
		/// <param name="readWrite">How or if color space conversions should be done on texture read/write.</param>
		public RenderTexture(int width, int height, int depth, RenderTextureFormat format)
		{
			RenderTexture.Internal_CreateRenderTexture(this);
			this.width = width;
			this.height = height;
			this.depth = depth;
			this.format = format;
			RenderTexture.Internal_SetSRGBReadWrite(this, QualitySettings.activeColorSpace == ColorSpace.Linear);
		}

		/// <summary>
		///   <para>Creates a new RenderTexture object.</para>
		/// </summary>
		/// <param name="width">Texture width in pixels.</param>
		/// <param name="height">Texture height in pixels.</param>
		/// <param name="depth">Number of bits in depth buffer (0, 16 or 24). Note that only 24 bit depth has stencil buffer.</param>
		/// <param name="format">Texture color format.</param>
		/// <param name="readWrite">How or if color space conversions should be done on texture read/write.</param>
		public RenderTexture(int width, int height, int depth)
		{
			RenderTexture.Internal_CreateRenderTexture(this);
			this.width = width;
			this.height = height;
			this.depth = depth;
			this.format = RenderTextureFormat.Default;
			RenderTexture.Internal_SetSRGBReadWrite(this, QualitySettings.activeColorSpace == ColorSpace.Linear);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_CreateRenderTexture([Writable] RenderTexture rt);

		/// <summary>
		///   <para>Allocate a temporary render texture.</para>
		/// </summary>
		/// <param name="width">Width in pixels.</param>
		/// <param name="height">Height in pixels.</param>
		/// <param name="depthBuffer">Depth buffer bits (0, 16 or 24). Note that only 24 bit depth has stencil buffer.</param>
		/// <param name="format">Render texture format.</param>
		/// <param name="readWrite">sRGB handling mode.</param>
		/// <param name="antiAliasing">Anti-aliasing (1,2,4,8).</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern RenderTexture GetTemporary(int width, int height, [DefaultValue("0")] int depthBuffer, [DefaultValue("RenderTextureFormat.Default")] RenderTextureFormat format, [DefaultValue("RenderTextureReadWrite.Default")] RenderTextureReadWrite readWrite, [DefaultValue("1")] int antiAliasing);

		[ExcludeFromDocs]
		public static RenderTexture GetTemporary(int width, int height, int depthBuffer, RenderTextureFormat format, RenderTextureReadWrite readWrite)
		{
			int antiAliasing = 1;
			return RenderTexture.GetTemporary(width, height, depthBuffer, format, readWrite, antiAliasing);
		}

		[ExcludeFromDocs]
		public static RenderTexture GetTemporary(int width, int height, int depthBuffer, RenderTextureFormat format)
		{
			int antiAliasing = 1;
			RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default;
			return RenderTexture.GetTemporary(width, height, depthBuffer, format, readWrite, antiAliasing);
		}

		[ExcludeFromDocs]
		public static RenderTexture GetTemporary(int width, int height, int depthBuffer)
		{
			int antiAliasing = 1;
			RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default;
			RenderTextureFormat format = RenderTextureFormat.Default;
			return RenderTexture.GetTemporary(width, height, depthBuffer, format, readWrite, antiAliasing);
		}

		[ExcludeFromDocs]
		public static RenderTexture GetTemporary(int width, int height)
		{
			int antiAliasing = 1;
			RenderTextureReadWrite readWrite = RenderTextureReadWrite.Default;
			RenderTextureFormat format = RenderTextureFormat.Default;
			int depthBuffer = 0;
			return RenderTexture.GetTemporary(width, height, depthBuffer, format, readWrite, antiAliasing);
		}

		/// <summary>
		///   <para>Release a temporary texture allocated with GetTemporary.</para>
		/// </summary>
		/// <param name="temp"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ReleaseTemporary(RenderTexture temp);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Internal_GetWidth(RenderTexture mono);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_SetWidth(RenderTexture mono, int width);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Internal_GetHeight(RenderTexture mono);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_SetHeight(RenderTexture mono, int width);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_SetSRGBReadWrite(RenderTexture mono, bool sRGB);

		/// <summary>
		///   <para>The width of the render texture in pixels.</para>
		/// </summary>
		public override int width
		{
			get
			{
				return RenderTexture.Internal_GetWidth(this);
			}
			set
			{
				RenderTexture.Internal_SetWidth(this, value);
			}
		}

		/// <summary>
		///   <para>The height of the render texture in pixels.</para>
		/// </summary>
		public override int height
		{
			get
			{
				return RenderTexture.Internal_GetHeight(this);
			}
			set
			{
				RenderTexture.Internal_SetHeight(this, value);
			}
		}

		/// <summary>
		///   <para>The precision of the render texture's depth buffer in bits (0, 16, 24 are supported).</para>
		/// </summary>
		public extern int depth { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern bool isPowerOfTwo { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Does this render texture use sRGB read / write (Read Only).</para>
		/// </summary>
		public extern bool sRGB { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The color format of the render texture.</para>
		/// </summary>
		public extern RenderTextureFormat format { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Use mipmaps on a render texture?</para>
		/// </summary>
		public extern bool useMipMap { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Should mipmap levels be generated automatically?</para>
		/// </summary>
		public extern bool generateMips { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>If enabled, this Render Texture will be used as a Cubemap.</para>
		/// </summary>
		public extern bool isCubemap { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>If enabled, this Render Texture will be used as a Texture3D.</para>
		/// </summary>
		public extern bool isVolume { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Volume extent of a 3D render texture.</para>
		/// </summary>
		public extern int volumeDepth { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The antialiasing level for the RenderTexture.</para>
		/// </summary>
		public extern int antiAliasing { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Enable random access write into this render texture on Shader Model 5.0 level shaders.</para>
		/// </summary>
		public extern bool enableRandomWrite { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Actually creates the RenderTexture.</para>
		/// </summary>
		public bool Create()
		{
			return RenderTexture.INTERNAL_CALL_Create(this);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool INTERNAL_CALL_Create(RenderTexture self);

		/// <summary>
		///   <para>Releases the RenderTexture.</para>
		/// </summary>
		public void Release()
		{
			RenderTexture.INTERNAL_CALL_Release(this);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Release(RenderTexture self);

		/// <summary>
		///   <para>Is the render texture actually created?</para>
		/// </summary>
		public bool IsCreated()
		{
			return RenderTexture.INTERNAL_CALL_IsCreated(this);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool INTERNAL_CALL_IsCreated(RenderTexture self);

		/// <summary>
		///   <para>Discards the contents of the RenderTexture.</para>
		/// </summary>
		/// <param name="discardColor">Should the colour buffer be discarded?</param>
		/// <param name="discardDepth">Should the depth buffer be discarded?</param>
		public void DiscardContents()
		{
			RenderTexture.INTERNAL_CALL_DiscardContents(this);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_DiscardContents(RenderTexture self);

		/// <summary>
		///   <para>Discards the contents of the RenderTexture.</para>
		/// </summary>
		/// <param name="discardColor">Should the colour buffer be discarded?</param>
		/// <param name="discardDepth">Should the depth buffer be discarded?</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void DiscardContents(bool discardColor, bool discardDepth);

		/// <summary>
		///   <para>Indicate that there's a RenderTexture restore operation expected.</para>
		/// </summary>
		public void MarkRestoreExpected()
		{
			RenderTexture.INTERNAL_CALL_MarkRestoreExpected(this);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_MarkRestoreExpected(RenderTexture self);

		/// <summary>
		///   <para>Color buffer of the render texture (Read Only).</para>
		/// </summary>
		public RenderBuffer colorBuffer
		{
			get
			{
				RenderBuffer result;
				this.GetColorBuffer(out result);
				return result;
			}
		}

		/// <summary>
		///   <para>Depth/stencil buffer of the render texture (Read Only).</para>
		/// </summary>
		public RenderBuffer depthBuffer
		{
			get
			{
				RenderBuffer result;
				this.GetDepthBuffer(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetColorBuffer(out RenderBuffer res);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetDepthBuffer(out RenderBuffer res);

		/// <summary>
		///   <para>Assigns this RenderTexture as a global shader property named propertyName.</para>
		/// </summary>
		/// <param name="propertyName"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetGlobalShaderProperty(string propertyName);

		/// <summary>
		///   <para>Currently active render texture.</para>
		/// </summary>
		public static extern RenderTexture active { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[Obsolete("Use SystemInfo.supportsRenderTextures instead.")]
		public static extern bool enabled { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_GetTexelOffset(RenderTexture tex, out Vector2 output);

		public Vector2 GetTexelOffset()
		{
			Vector2 result;
			RenderTexture.Internal_GetTexelOffset(this, out result);
			return result;
		}

		/// <summary>
		///   <para>Does a RenderTexture have stencil buffer?</para>
		/// </summary>
		/// <param name="rt">Render texture, or null for main screen.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool SupportsStencil(RenderTexture rt);

		[Obsolete("SetBorderColor is no longer supported.", true)]
		public void SetBorderColor(Color color)
		{
		}
	}
}
