using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>RenderMode for the Canvas.</para>
	/// </summary>
	public enum RenderMode
	{
		/// <summary>
		///   <para>Render at the end of the scene using a 2D Canvas.</para>
		/// </summary>
		ScreenSpaceOverlay,
		/// <summary>
		///   <para>Render using the Camera configured on the Canvas.</para>
		/// </summary>
		ScreenSpaceCamera,
		/// <summary>
		///   <para>Render using any Camera in the scene that can render the layer.</para>
		/// </summary>
		WorldSpace
	}
}
