using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>How the Light is rendered.</para>
	/// </summary>
	public enum LightRenderMode
	{
		/// <summary>
		///   <para>Automatically choose the render mode.</para>
		/// </summary>
		Auto,
		/// <summary>
		///   <para>Force the Light to be a pixel light.</para>
		/// </summary>
		ForcePixel,
		/// <summary>
		///   <para>Force the Light to be a vertex light.</para>
		/// </summary>
		ForceVertex
	}
}
