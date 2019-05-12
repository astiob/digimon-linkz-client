using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Fog mode to use.</para>
	/// </summary>
	public enum FogMode
	{
		/// <summary>
		///   <para>Linear fog.</para>
		/// </summary>
		Linear = 1,
		/// <summary>
		///   <para>Exponential fog.</para>
		/// </summary>
		Exponential,
		/// <summary>
		///   <para>Exponential squared fog (default).</para>
		/// </summary>
		ExponentialSquared
	}
}
