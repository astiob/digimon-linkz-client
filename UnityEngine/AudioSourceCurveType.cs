using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>This defines the curve type of the different custom curves that can be queried and set within the AudioSource.</para>
	/// </summary>
	public enum AudioSourceCurveType
	{
		/// <summary>
		///   <para>Custom Volume Rolloff.</para>
		/// </summary>
		CustomRolloff,
		/// <summary>
		///   <para>The Spatial Blend.</para>
		/// </summary>
		SpatialBlend,
		/// <summary>
		///   <para>Reverb Zone Mix.</para>
		/// </summary>
		ReverbZoneMix,
		/// <summary>
		///   <para>The 3D Spread.</para>
		/// </summary>
		Spread
	}
}
