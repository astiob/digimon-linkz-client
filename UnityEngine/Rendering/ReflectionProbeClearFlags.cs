using System;

namespace UnityEngine.Rendering
{
	/// <summary>
	///   <para>Values for ReflectionProbe.clearFlags, determining what to clear when rendering a ReflectionProbe.</para>
	/// </summary>
	public enum ReflectionProbeClearFlags
	{
		/// <summary>
		///   <para>Clear with the skybox.</para>
		/// </summary>
		Skybox = 1,
		/// <summary>
		///   <para>Clear with a background color.</para>
		/// </summary>
		SolidColor
	}
}
