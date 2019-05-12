using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Format of a RenderTexture.</para>
	/// </summary>
	public enum RenderTextureFormat
	{
		/// <summary>
		///   <para>Color render texture format, 8 bits per channel.</para>
		/// </summary>
		ARGB32,
		/// <summary>
		///   <para>A depth render texture format.</para>
		/// </summary>
		Depth,
		/// <summary>
		///   <para>Color render texture format, 16 bit floating point per channel.</para>
		/// </summary>
		ARGBHalf,
		/// <summary>
		///   <para>A native shadowmap render texture format.</para>
		/// </summary>
		Shadowmap,
		/// <summary>
		///   <para>Color render texture format.</para>
		/// </summary>
		RGB565,
		/// <summary>
		///   <para>Color render texture format, 4 bit per channel.</para>
		/// </summary>
		ARGB4444,
		/// <summary>
		///   <para>Color render texture format, 1 bit for Alpha channel, 5 bits for Red, Green and Blue channels.</para>
		/// </summary>
		ARGB1555,
		/// <summary>
		///   <para>Default color render texture format: will be chosen accordingly to Frame Buffer format and Platform.</para>
		/// </summary>
		Default,
		/// <summary>
		///   <para>Color render texture format. 10 bits for colors, 2 bits for alpha.</para>
		/// </summary>
		ARGB2101010,
		/// <summary>
		///   <para>Default HDR color render texture format: will be chosen accordingly to Frame Buffer format and Platform.</para>
		/// </summary>
		DefaultHDR,
		/// <summary>
		///   <para>Color render texture format, 32 bit floating point per channel.</para>
		/// </summary>
		ARGBFloat = 11,
		/// <summary>
		///   <para>Two color (RG) render texture format, 32 bit floating point per channel.</para>
		/// </summary>
		RGFloat,
		/// <summary>
		///   <para>Two color (RG) render texture format, 16 bit floating point per channel.</para>
		/// </summary>
		RGHalf,
		/// <summary>
		///   <para>Scalar (R) render texture format, 32 bit floating point.</para>
		/// </summary>
		RFloat,
		/// <summary>
		///   <para>Scalar (R) render texture format, 16 bit floating point.</para>
		/// </summary>
		RHalf,
		/// <summary>
		///   <para>Scalar (R) render texture format, 8 bit fixed point.</para>
		/// </summary>
		R8,
		/// <summary>
		///   <para>Four channel (ARGB) render texture format, 32 bit signed integer per channel.</para>
		/// </summary>
		ARGBInt,
		/// <summary>
		///   <para>Two channel (RG) render texture format, 32 bit signed integer per channel.</para>
		/// </summary>
		RGInt,
		/// <summary>
		///   <para>Scalar (R) render texture format, 32 bit signed integer.</para>
		/// </summary>
		RInt
	}
}
