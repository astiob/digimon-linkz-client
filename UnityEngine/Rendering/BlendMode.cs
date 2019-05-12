using System;

namespace UnityEngine.Rendering
{
	/// <summary>
	///   <para>Blend mode for controlling the blending.</para>
	/// </summary>
	public enum BlendMode
	{
		/// <summary>
		///   <para>Blend factor is  (0, 0, 0, 0).</para>
		/// </summary>
		Zero,
		/// <summary>
		///   <para>Blend factor is (1, 1, 1, 1).</para>
		/// </summary>
		One,
		/// <summary>
		///   <para>Blend factor is (Rd, Gd, Bd, Ad).</para>
		/// </summary>
		DstColor,
		/// <summary>
		///   <para>Blend factor is (Rs, Gs, Bs, As).</para>
		/// </summary>
		SrcColor,
		/// <summary>
		///   <para>Blend factor is (1 - Rd, 1 - Gd, 1 - Bd, 1 - Ad).</para>
		/// </summary>
		OneMinusDstColor,
		/// <summary>
		///   <para>Blend factor is (As, As, As, As).</para>
		/// </summary>
		SrcAlpha,
		/// <summary>
		///   <para>Blend factor is (1 - Rs, 1 - Gs, 1 - Bs, 1 - As).</para>
		/// </summary>
		OneMinusSrcColor,
		/// <summary>
		///   <para>Blend factor is (Ad, Ad, Ad, Ad).</para>
		/// </summary>
		DstAlpha,
		/// <summary>
		///   <para>Blend factor is (1 - Ad, 1 - Ad, 1 - Ad, 1 - Ad).</para>
		/// </summary>
		OneMinusDstAlpha,
		/// <summary>
		///   <para>Blend factor is (f, f, f, 1); where f = min(As, 1 - Ad).</para>
		/// </summary>
		SrcAlphaSaturate,
		/// <summary>
		///   <para>Blend factor is (1 - As, 1 - As, 1 - As, 1 - As).</para>
		/// </summary>
		OneMinusSrcAlpha
	}
}
