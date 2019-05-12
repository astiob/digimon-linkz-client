using System;

namespace UnityEngine.Rendering
{
	/// <summary>
	///   <para>How shadows are cast from this object.</para>
	/// </summary>
	public enum ShadowCastingMode
	{
		/// <summary>
		///   <para>No shadows are cast from this object.</para>
		/// </summary>
		Off,
		/// <summary>
		///   <para>Shadows are cast from this object.</para>
		/// </summary>
		On,
		/// <summary>
		///   <para>Shadows are cast from this object, treating it as two-sided.</para>
		/// </summary>
		TwoSided,
		/// <summary>
		///   <para>Object casts shadows, but is otherwise invisible in the scene.</para>
		/// </summary>
		ShadowsOnly
	}
}
