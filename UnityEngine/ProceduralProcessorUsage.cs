using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>The global Substance engine processor usage (as used for the ProceduralMaterial.substanceProcessorUsage property).</para>
	/// </summary>
	public enum ProceduralProcessorUsage
	{
		/// <summary>
		///   <para>Exact control of processor usage is not available.</para>
		/// </summary>
		Unsupported,
		/// <summary>
		///   <para>A single physical processor core is used for ProceduralMaterial generation.</para>
		/// </summary>
		One,
		/// <summary>
		///   <para>Half of all physical processor cores are used for ProceduralMaterial generation.</para>
		/// </summary>
		Half,
		/// <summary>
		///   <para>All physical processor cores are used for ProceduralMaterial generation.</para>
		/// </summary>
		All
	}
}
