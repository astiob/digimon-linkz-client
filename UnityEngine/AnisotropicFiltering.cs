using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Anisotropic filtering mode.</para>
	/// </summary>
	public enum AnisotropicFiltering
	{
		/// <summary>
		///   <para>Disable anisotropic filtering for all textures.</para>
		/// </summary>
		Disable,
		/// <summary>
		///   <para>Enable anisotropic filtering, as set for each texture.</para>
		/// </summary>
		Enable,
		/// <summary>
		///   <para>Enable anisotropic filtering for all textures.</para>
		/// </summary>
		ForceEnable
	}
}
