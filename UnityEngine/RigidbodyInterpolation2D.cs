using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Interpolation mode for Rigidbody2D objects.</para>
	/// </summary>
	public enum RigidbodyInterpolation2D
	{
		/// <summary>
		///   <para>Do not apply any smoothing to the object's movement.</para>
		/// </summary>
		None,
		/// <summary>
		///   <para>Smooth movement based on the object's positions in previous frames.</para>
		/// </summary>
		Interpolate,
		/// <summary>
		///   <para>Smooth an object's movement based on an estimate of its position in the next frame.</para>
		/// </summary>
		Extrapolate
	}
}
