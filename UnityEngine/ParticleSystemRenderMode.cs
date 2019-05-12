using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>The rendering mode for particle systems (Shuriken).</para>
	/// </summary>
	public enum ParticleSystemRenderMode
	{
		/// <summary>
		///   <para>Render particles as billboards facing the player. (Default)</para>
		/// </summary>
		Billboard,
		/// <summary>
		///   <para>Stretch particles in the direction of motion.</para>
		/// </summary>
		Stretch,
		/// <summary>
		///   <para>Render particles as billboards always facing up along the y-Axis.</para>
		/// </summary>
		HorizontalBillboard,
		/// <summary>
		///   <para>Render particles as billboards always facing the player, but not pitching along the x-Axis.</para>
		/// </summary>
		VerticalBillboard,
		/// <summary>
		///   <para>Render particles as meshes.</para>
		/// </summary>
		Mesh
	}
}
