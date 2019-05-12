using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>How the material should interact with lightmaps and lightprobes.</para>
	/// </summary>
	[Flags]
	public enum MaterialGlobalIlluminationFlags
	{
		/// <summary>
		///   <para>The emissive lighting should not affect Global Illumination at all.</para>
		/// </summary>
		None = 0,
		/// <summary>
		///   <para>The emissive lighting should affect Global Illumination. It should emit lighting into realtime lightmaps and realtime lightprobes.</para>
		/// </summary>
		RealtimeEmissive = 1,
		/// <summary>
		///   <para>The emissive lighting should affect Global Illumination. It should emit lighting into baked lightmaps and baked lightprobes.</para>
		/// </summary>
		BakedEmissive = 2,
		/// <summary>
		///   <para>The emissive lighting is guaranteed to be black. This lets the lightmapping system know that it doesn't have to extract emissive lighting information from the material and can simply assume it is completely black.</para>
		/// </summary>
		EmissiveIsBlack = 4
	}
}
