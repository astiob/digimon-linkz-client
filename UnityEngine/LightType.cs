using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>The type of a Light.</para>
	/// </summary>
	public enum LightType
	{
		/// <summary>
		///   <para>The light is a spot light.</para>
		/// </summary>
		Spot,
		/// <summary>
		///   <para>The light is a directional light.</para>
		/// </summary>
		Directional,
		/// <summary>
		///   <para>The light is a point light.</para>
		/// </summary>
		Point,
		/// <summary>
		///   <para>The light is an area light. It affects only lightmaps and lightprobes.</para>
		/// </summary>
		Area
	}
}
