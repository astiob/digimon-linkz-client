using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>NPOT Texture2D|textures support.</para>
	/// </summary>
	public enum NPOTSupport
	{
		/// <summary>
		///   <para>NPOT textures are not supported. Will be upscaled/padded at loading time.</para>
		/// </summary>
		None,
		/// <summary>
		///   <para>Limited NPOT support: no mip-maps and clamp TextureWrapMode|wrap mode will be forced.</para>
		/// </summary>
		Restricted,
		/// <summary>
		///   <para>Full NPOT support.</para>
		/// </summary>
		Full
	}
}
