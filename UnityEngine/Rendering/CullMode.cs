using System;

namespace UnityEngine.Rendering
{
	/// <summary>
	///   <para>Backface culling mode.</para>
	/// </summary>
	public enum CullMode
	{
		/// <summary>
		///   <para>Disable culling.</para>
		/// </summary>
		Off,
		/// <summary>
		///   <para>Cull front-facing geometry.</para>
		/// </summary>
		Front,
		/// <summary>
		///   <para>Cull back-facing geometry.</para>
		/// </summary>
		Back
	}
}
