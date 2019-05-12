using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Blend weights.</para>
	/// </summary>
	public enum BlendWeights
	{
		/// <summary>
		///   <para>One bone affects each vertex.</para>
		/// </summary>
		OneBone = 1,
		/// <summary>
		///   <para>Two bones affect each vertex.</para>
		/// </summary>
		TwoBones,
		/// <summary>
		///   <para>Four bones affect each vertex.</para>
		/// </summary>
		FourBones = 4
	}
}
