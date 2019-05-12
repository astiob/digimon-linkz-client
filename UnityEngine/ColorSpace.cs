using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Valid color spaces.</para>
	/// </summary>
	public enum ColorSpace
	{
		/// <summary>
		///   <para>Uninitialized colorspace.</para>
		/// </summary>
		Uninitialized = -1,
		/// <summary>
		///   <para>Lightmap has been baked for gamma rendering.</para>
		/// </summary>
		Gamma,
		/// <summary>
		///   <para>Lightmap has been baked for linear rendering.</para>
		/// </summary>
		Linear
	}
}
