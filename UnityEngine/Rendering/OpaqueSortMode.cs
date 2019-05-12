using System;

namespace UnityEngine.Rendering
{
	/// <summary>
	///   <para>Opaque object sorting mode of a Camera.</para>
	/// </summary>
	public enum OpaqueSortMode
	{
		/// <summary>
		///   <para>Default opaque sorting mode.</para>
		/// </summary>
		Default,
		/// <summary>
		///   <para>Do rough front-to-back sorting of opaque objects.</para>
		/// </summary>
		FrontToBack,
		/// <summary>
		///   <para>Do not sort opaque objects by distance.</para>
		/// </summary>
		NoDistanceSort
	}
}
