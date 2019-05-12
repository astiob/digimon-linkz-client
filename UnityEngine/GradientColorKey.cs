using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Color key used by Gradient.</para>
	/// </summary>
	public struct GradientColorKey
	{
		/// <summary>
		///   <para>Color of key.</para>
		/// </summary>
		public Color color;

		/// <summary>
		///   <para>Time of the key (0 - 1).</para>
		/// </summary>
		public float time;

		/// <summary>
		///   <para>Gradient color key.</para>
		/// </summary>
		/// <param name="color">Color of key.</param>
		/// <param name="time">Time of the key (0 - 1).</param>
		/// <param name="col"></param>
		public GradientColorKey(Color col, float time)
		{
			this.color = col;
			this.time = time;
		}
	}
}
