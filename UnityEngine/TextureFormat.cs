using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Format used when creating textures from scripts.</para>
	/// </summary>
	public enum TextureFormat
	{
		/// <summary>
		///   <para>Alpha-only texture format.</para>
		/// </summary>
		Alpha8 = 1,
		/// <summary>
		///   <para>A 16 bits/pixel texture format. Texture stores color with an alpha channel.</para>
		/// </summary>
		ARGB4444,
		/// <summary>
		///   <para>A color texture format.</para>
		/// </summary>
		RGB24,
		/// <summary>
		///   <para>Color with alpha texture format, 8-bits per channel.</para>
		/// </summary>
		RGBA32,
		/// <summary>
		///   <para>Color with an alpha channel texture format.</para>
		/// </summary>
		ARGB32,
		/// <summary>
		///   <para>A 16 bit color texture format.</para>
		/// </summary>
		RGB565 = 7,
		/// <summary>
		///   <para>A 16 bit color texture format that only has a red channel.</para>
		/// </summary>
		R16 = 9,
		/// <summary>
		///   <para>Compressed color texture format.</para>
		/// </summary>
		DXT1,
		/// <summary>
		///   <para>Compressed color with alpha channel texture format.</para>
		/// </summary>
		DXT5 = 12,
		/// <summary>
		///   <para>Color and alpha  texture format, 4 bit per channel.</para>
		/// </summary>
		RGBA4444,
		/// <summary>
		///   <para>Format returned by iPhone camera.</para>
		/// </summary>
		BGRA32,
		/// <summary>
		///   <para>Scalar (R)  texture format, 16 bit floating point.</para>
		/// </summary>
		RHalf,
		/// <summary>
		///   <para>Two color (RG)  texture format, 16 bit floating point per channel.</para>
		/// </summary>
		RGHalf,
		/// <summary>
		///   <para>RGB color and alpha texture format, 16 bit floating point per channel.</para>
		/// </summary>
		RGBAHalf,
		/// <summary>
		///   <para>Scalar (R) texture format, 32 bit floating point.</para>
		/// </summary>
		RFloat,
		/// <summary>
		///   <para>Two color (RG)  texture format, 32 bit floating point per channel.</para>
		/// </summary>
		RGFloat,
		/// <summary>
		///   <para>RGB color and alpha etxture format,  32-bit floats per channel.</para>
		/// </summary>
		RGBAFloat,
		/// <summary>
		///   <para>A format that uses the YUV color space and is often used for video encoding.  Currently, this texture format is only useful for native code plugins as there is no support for texture importing or pixel access for this format.  YUY2 is implemented for Direct3D 9, Direct3D 11, and Xbox One.</para>
		/// </summary>
		YUY2,
		/// <summary>
		///   <para>Compressed color texture format with crunch compression for small storage sizes.</para>
		/// </summary>
		DXT1Crunched = 28,
		/// <summary>
		///   <para>Compressed color with alpha channel texture format with crunch compression for small storage sizes.</para>
		/// </summary>
		DXT5Crunched,
		/// <summary>
		///   <para>PowerVR (iOS) 2 bits/pixel compressed color texture format.</para>
		/// </summary>
		PVRTC_RGB2,
		/// <summary>
		///   <para>PowerVR (iOS) 2 bits/pixel compressed with alpha channel texture format.</para>
		/// </summary>
		PVRTC_RGBA2,
		/// <summary>
		///   <para>PowerVR (iOS) 4 bits/pixel compressed color texture format.</para>
		/// </summary>
		PVRTC_RGB4,
		/// <summary>
		///   <para>PowerVR (iOS) 4 bits/pixel compressed with alpha channel texture format.</para>
		/// </summary>
		PVRTC_RGBA4,
		/// <summary>
		///   <para>ETC (GLES2.0) 4 bits/pixel compressed RGB texture format.</para>
		/// </summary>
		ETC_RGB4,
		/// <summary>
		///   <para>ATC (ATITC) 4 bits/pixel compressed RGB texture format.</para>
		/// </summary>
		ATC_RGB4,
		/// <summary>
		///   <para>ATC (ATITC) 8 bits/pixel compressed RGB texture format.</para>
		/// </summary>
		ATC_RGBA8,
		/// <summary>
		///   <para>ETC2  EAC (GL ES 3.0) 4 bitspixel compressed unsigned single-channel texture format.</para>
		/// </summary>
		EAC_R = 41,
		/// <summary>
		///   <para>ETC2  EAC (GL ES 3.0) 4 bitspixel compressed signed single-channel texture format.</para>
		/// </summary>
		EAC_R_SIGNED,
		/// <summary>
		///   <para>ETC2  EAC (GL ES 3.0) 8 bitspixel compressed unsigned dual-channel (RG) texture format.</para>
		/// </summary>
		EAC_RG,
		/// <summary>
		///   <para>ETC2  EAC (GL ES 3.0) 8 bitspixel compressed signed dual-channel (RG) texture format.</para>
		/// </summary>
		EAC_RG_SIGNED,
		/// <summary>
		///   <para>ETC2 (GL ES 3.0) 4 bits/pixel compressed RGB texture format.</para>
		/// </summary>
		ETC2_RGB,
		/// <summary>
		///   <para>ETC2 (GL ES 3.0) 4 bits/pixel RGB+1-bit alpha texture format.</para>
		/// </summary>
		ETC2_RGBA1,
		/// <summary>
		///   <para>ETC2 (GL ES 3.0) 8 bits/pixel compressed RGBA texture format.</para>
		/// </summary>
		ETC2_RGBA8,
		/// <summary>
		///   <para>ASTC (4x4 pixel block in 128 bits) compressed RGB texture format.</para>
		/// </summary>
		ASTC_RGB_4x4,
		/// <summary>
		///   <para>ASTC (5x5 pixel block in 128 bits) compressed RGB texture format.</para>
		/// </summary>
		ASTC_RGB_5x5,
		/// <summary>
		///   <para>ASTC (6x6 pixel block in 128 bits) compressed RGB texture format.</para>
		/// </summary>
		ASTC_RGB_6x6,
		/// <summary>
		///   <para>ASTC (8x8 pixel block in 128 bits) compressed RGB texture format.</para>
		/// </summary>
		ASTC_RGB_8x8,
		/// <summary>
		///   <para>ASTC (10x10 pixel block in 128 bits) compressed RGB texture format.</para>
		/// </summary>
		ASTC_RGB_10x10,
		/// <summary>
		///   <para>ASTC (12x12 pixel block in 128 bits) compressed RGB texture format.</para>
		/// </summary>
		ASTC_RGB_12x12,
		/// <summary>
		///   <para>ASTC (4x4 pixel block in 128 bits) compressed RGBA texture format.</para>
		/// </summary>
		ASTC_RGBA_4x4,
		/// <summary>
		///   <para>ASTC (5x5 pixel block in 128 bits) compressed RGBA texture format.</para>
		/// </summary>
		ASTC_RGBA_5x5,
		/// <summary>
		///   <para>ASTC (6x6 pixel block in 128 bits) compressed RGBA texture format.</para>
		/// </summary>
		ASTC_RGBA_6x6,
		/// <summary>
		///   <para>ASTC (8x8 pixel block in 128 bits) compressed RGBA texture format.</para>
		/// </summary>
		ASTC_RGBA_8x8,
		/// <summary>
		///   <para>ASTC (10x10 pixel block in 128 bits) compressed RGBA texture format.</para>
		/// </summary>
		ASTC_RGBA_10x10,
		/// <summary>
		///   <para>ASTC (12x12 pixel block in 128 bits) compressed RGBA texture format.</para>
		/// </summary>
		ASTC_RGBA_12x12,
		/// <summary>
		///   <para>ETC (Nintendo 3DS) 4 bits/pixel compressed RGB texture format.</para>
		/// </summary>
		ETC_RGB4_3DS,
		/// <summary>
		///   <para>ETC (Nintendo 3DS) 4 bitspixel RGB + 4 bitspixel Alpha compressed texture format.</para>
		/// </summary>
		ETC_RGBA8_3DS
	}
}
