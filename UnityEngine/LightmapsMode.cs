using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Lightmap (and lighting) configuration mode, controls how lightmaps interact with lighting and what kind of information they store.</para>
	/// </summary>
	public enum LightmapsMode
	{
		/// <summary>
		///   <para>Light intensity (no directional information), encoded as 1 lightmap.</para>
		/// </summary>
		NonDirectional,
		/// <summary>
		///   <para>Directional information for direct light is combined with directional information for indirect light, encoded as 2 lightmaps.</para>
		/// </summary>
		CombinedDirectional,
		/// <summary>
		///   <para>Directional information for direct light is stored separately from directional information for indirect light, encoded as 4 lightmaps.</para>
		/// </summary>
		SeparateDirectional
	}
}
