using System;

namespace UnityEngine.Rendering
{
	/// <summary>
	///   <para>Blend operation.</para>
	/// </summary>
	public enum BlendOp
	{
		/// <summary>
		///   <para>Add (s + d).</para>
		/// </summary>
		Add,
		/// <summary>
		///   <para>Subtract.</para>
		/// </summary>
		Subtract,
		/// <summary>
		///   <para>Reverse subtract.</para>
		/// </summary>
		ReverseSubtract,
		/// <summary>
		///   <para>Min.</para>
		/// </summary>
		Min,
		/// <summary>
		///   <para>Max.</para>
		/// </summary>
		Max,
		/// <summary>
		///   <para>Logical Clear (0).</para>
		/// </summary>
		LogicalClear,
		/// <summary>
		///   <para>Logical SET (1) (D3D11.1 only).</para>
		/// </summary>
		LogicalSet,
		/// <summary>
		///   <para>Logical Copy (s) (D3D11.1 only).</para>
		/// </summary>
		LogicalCopy,
		/// <summary>
		///   <para>Logical inverted Copy (!s) (D3D11.1 only).</para>
		/// </summary>
		LogicalCopyInverted,
		/// <summary>
		///   <para>Logical No-op (d) (D3D11.1 only).</para>
		/// </summary>
		LogicalNoop,
		/// <summary>
		///   <para>Logical Inverse (!d) (D3D11.1 only).</para>
		/// </summary>
		LogicalInvert,
		/// <summary>
		///   <para>Logical AND (s &amp; d) (D3D11.1 only).</para>
		/// </summary>
		LogicalAnd,
		/// <summary>
		///   <para>Logical NAND !(s &amp; d). D3D11.1 only.</para>
		/// </summary>
		LogicalNand,
		/// <summary>
		///   <para>Logical OR (s | d) (D3D11.1 only).</para>
		/// </summary>
		LogicalOr,
		/// <summary>
		///   <para>Logical NOR !(s | d) (D3D11.1 only).</para>
		/// </summary>
		LogicalNor,
		/// <summary>
		///   <para>Logical XOR (s XOR d) (D3D11.1 only).</para>
		/// </summary>
		LogicalXor,
		/// <summary>
		///   <para>Logical Equivalence !(s XOR d) (D3D11.1 only).</para>
		/// </summary>
		LogicalEquivalence,
		/// <summary>
		///   <para>Logical reverse AND (s &amp; !d) (D3D11.1 only).</para>
		/// </summary>
		LogicalAndReverse,
		/// <summary>
		///   <para>Logical inverted AND (!s &amp; d) (D3D11.1 only).</para>
		/// </summary>
		LogicalAndInverted,
		/// <summary>
		///   <para>Logical reverse OR (s | !d) (D3D11.1 only).</para>
		/// </summary>
		LogicalOrReverse,
		/// <summary>
		///   <para>Logical inverted OR (!s | d) (D3D11.1 only).</para>
		/// </summary>
		LogicalOrInverted,
		/// <summary>
		///   <para>Multiply (Advanced OpenGL blending).</para>
		/// </summary>
		Multiply,
		/// <summary>
		///   <para>Screen (Advanced OpenGL blending).</para>
		/// </summary>
		Screen,
		/// <summary>
		///   <para>Overlay (Advanced OpenGL blending).</para>
		/// </summary>
		Overlay,
		/// <summary>
		///   <para>Darken (Advanced OpenGL blending).</para>
		/// </summary>
		Darken,
		/// <summary>
		///   <para>Lighten (Advanced OpenGL blending).</para>
		/// </summary>
		Lighten,
		/// <summary>
		///   <para>Color dodge (Advanced OpenGL blending).</para>
		/// </summary>
		ColorDodge,
		/// <summary>
		///   <para>Color burn (Advanced OpenGL blending).</para>
		/// </summary>
		ColorBurn,
		/// <summary>
		///   <para>Hard light (Advanced OpenGL blending).</para>
		/// </summary>
		HardLight,
		/// <summary>
		///   <para>Soft light (Advanced OpenGL blending).</para>
		/// </summary>
		SoftLight,
		/// <summary>
		///   <para>Difference (Advanced OpenGL blending).</para>
		/// </summary>
		Difference,
		/// <summary>
		///   <para>Exclusion (Advanced OpenGL blending).</para>
		/// </summary>
		Exclusion,
		/// <summary>
		///   <para>HSL Hue (Advanced OpenGL blending).</para>
		/// </summary>
		HSLHue,
		/// <summary>
		///   <para>HSL saturation (Advanced OpenGL blending).</para>
		/// </summary>
		HSLSaturation,
		/// <summary>
		///   <para>HSL color (Advanced OpenGL blending).</para>
		/// </summary>
		HSLColor,
		/// <summary>
		///   <para>HSL luminosity (Advanced OpenGL blending).</para>
		/// </summary>
		HSLLuminosity
	}
}
