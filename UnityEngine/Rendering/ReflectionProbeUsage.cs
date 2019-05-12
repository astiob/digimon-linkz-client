using System;

namespace UnityEngine.Rendering
{
	/// <summary>
	///   <para>Reflection Probe usage.</para>
	/// </summary>
	public enum ReflectionProbeUsage
	{
		/// <summary>
		///   <para>Reflection probes are disabled, skybox will be used for reflection.</para>
		/// </summary>
		Off,
		/// <summary>
		///   <para>Reflection probes are enabled. Blending occurs only between probes, useful in indoor environments. The renderer will use default reflection if there are no reflection probes nearby, but no blending between default reflection and probe will occur.</para>
		/// </summary>
		BlendProbes,
		/// <summary>
		///   <para>Reflection probes are enabled. Blending occurs between probes or probes and default reflection, useful for outdoor environments.</para>
		/// </summary>
		BlendProbesAndSkybox,
		/// <summary>
		///   <para>Reflection probes are enabled, but no blending will occur between probes when there are two overlapping volumes.</para>
		/// </summary>
		Simple
	}
}
