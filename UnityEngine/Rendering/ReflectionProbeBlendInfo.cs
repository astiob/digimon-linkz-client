using System;

namespace UnityEngine.Rendering
{
	/// <summary>
	///   <para>ReflectionProbeBlendInfo contains information required for blending probes.</para>
	/// </summary>
	public struct ReflectionProbeBlendInfo
	{
		/// <summary>
		///   <para>Reflection Probe used in blending.</para>
		/// </summary>
		public ReflectionProbe probe;

		/// <summary>
		///   <para>Specifies the weight used in the interpolation between two probes, value varies from 0.0 to 1.0.</para>
		/// </summary>
		public float weight;
	}
}
