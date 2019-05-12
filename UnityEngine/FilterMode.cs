using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Filtering mode for textures. Corresponds to the settings in a.</para>
	/// </summary>
	public enum FilterMode
	{
		/// <summary>
		///   <para>Point filtering - texture pixels become blocky up close.</para>
		/// </summary>
		Point,
		/// <summary>
		///   <para>Bilinear filtering - texture samples are averaged.</para>
		/// </summary>
		Bilinear,
		/// <summary>
		///   <para>Trilinear filtering - texture samples are averaged and also blended between mipmap levels.</para>
		/// </summary>
		Trilinear
	}
}
