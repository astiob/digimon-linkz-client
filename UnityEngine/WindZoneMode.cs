using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Modes a Wind Zone can have, either Spherical or Directional.</para>
	/// </summary>
	public enum WindZoneMode
	{
		/// <summary>
		///   <para>Wind zone only has an effect inside the radius, and has a falloff from the center towards the edge.</para>
		/// </summary>
		Directional,
		/// <summary>
		///   <para>Wind zone affects the entire scene in one direction.</para>
		/// </summary>
		Spherical
	}
}
