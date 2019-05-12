using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Values for Camera.clearFlags, determining what to clear when rendering a Camera.</para>
	/// </summary>
	public enum CameraClearFlags
	{
		/// <summary>
		///   <para>Clear with the skybox.</para>
		/// </summary>
		Skybox = 1,
		Color,
		/// <summary>
		///   <para>Clear with a background color.</para>
		/// </summary>
		SolidColor = 2,
		/// <summary>
		///   <para>Clear only the depth buffer.</para>
		/// </summary>
		Depth,
		/// <summary>
		///   <para>Don't clear anything.</para>
		/// </summary>
		Nothing
	}
}
