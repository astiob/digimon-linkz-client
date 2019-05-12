using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Shadow casting options for a Light.</para>
	/// </summary>
	public enum LightShadows
	{
		/// <summary>
		///   <para>Do not cast shadows (default).</para>
		/// </summary>
		None,
		/// <summary>
		///   <para>Cast "hard" shadows (with no shadow filtering).</para>
		/// </summary>
		Hard,
		/// <summary>
		///   <para>Cast "soft" shadows (with 4x PCF filtering).</para>
		/// </summary>
		Soft
	}
}
