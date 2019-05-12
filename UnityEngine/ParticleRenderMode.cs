using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>The rendering mode for legacy particles.</para>
	/// </summary>
	public enum ParticleRenderMode
	{
		/// <summary>
		///   <para>Render the particles as billboards facing the player. (Default)</para>
		/// </summary>
		Billboard,
		/// <summary>
		///   <para>Stretch particles in the direction of motion.</para>
		/// </summary>
		Stretch = 3,
		/// <summary>
		///   <para>Sort the particles back-to-front and render as billboards.</para>
		/// </summary>
		SortedBillboard = 2,
		/// <summary>
		///   <para>Render the particles as billboards always facing up along the y-Axis.</para>
		/// </summary>
		HorizontalBillboard = 4,
		/// <summary>
		///   <para>Render the particles as billboards always facing the player, but not pitching along the x-Axis.</para>
		/// </summary>
		VerticalBillboard
	}
}
