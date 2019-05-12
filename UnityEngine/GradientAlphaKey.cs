using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Alpha key used by Gradient.</para>
	/// </summary>
	public struct GradientAlphaKey
	{
		/// <summary>
		///   <para>Alpha channel of key.</para>
		/// </summary>
		public float alpha;

		/// <summary>
		///   <para>Time of the key (0 - 1).</para>
		/// </summary>
		public float time;

		/// <summary>
		///   <para>Gradient alpha key.</para>
		/// </summary>
		/// <param name="alpha">Alpha of key (0 - 1).</param>
		/// <param name="time">Time of the key (0 - 1).</param>
		public GradientAlphaKey(float alpha, float time)
		{
			this.alpha = alpha;
			this.time = time;
		}
	}
}
