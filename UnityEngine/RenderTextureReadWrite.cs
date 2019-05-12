using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Color space conversion mode of a RenderTexture.</para>
	/// </summary>
	public enum RenderTextureReadWrite
	{
		/// <summary>
		///   <para>The correct color space for the current position in the rendering pipeline.</para>
		/// </summary>
		Default,
		/// <summary>
		///   <para>No sRGB reads or writes to this render texture.</para>
		/// </summary>
		Linear,
		/// <summary>
		///   <para>sRGB reads and writes to this render texture.</para>
		/// </summary>
		sRGB
	}
}
