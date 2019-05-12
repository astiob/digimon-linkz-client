using System;

namespace UnityEngine.Rendering
{
	/// <summary>
	///   <para>Specifies the operation that's performed on the stencil buffer when rendering.</para>
	/// </summary>
	public enum StencilOp
	{
		/// <summary>
		///   <para>Keeps the current stencil value.</para>
		/// </summary>
		Keep,
		/// <summary>
		///   <para>Sets the stencil buffer value to zero.</para>
		/// </summary>
		Zero,
		/// <summary>
		///   <para>Replace the stencil buffer value with reference value (specified in the shader).</para>
		/// </summary>
		Replace,
		/// <summary>
		///   <para>Increments the current stencil buffer value. Clamps to the maximum representable unsigned value.</para>
		/// </summary>
		IncrementSaturate,
		/// <summary>
		///   <para>Decrements the current stencil buffer value. Clamps to 0.</para>
		/// </summary>
		DecrementSaturate,
		/// <summary>
		///   <para>Bitwise inverts the current stencil buffer value.</para>
		/// </summary>
		Invert,
		/// <summary>
		///   <para>Increments the current stencil buffer value. Wraps stencil buffer value to zero when incrementing the maximum representable unsigned value.</para>
		/// </summary>
		IncrementWrap,
		/// <summary>
		///   <para>Decrements the current stencil buffer value. Wraps stencil buffer value to the maximum representable unsigned value when decrementing a stencil buffer value of zero.</para>
		/// </summary>
		DecrementWrap
	}
}
