using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Rigidbody interpolation mode.</para>
	/// </summary>
	public enum RigidbodyInterpolation
	{
		/// <summary>
		///   <para>No Interpolation.</para>
		/// </summary>
		None,
		/// <summary>
		///   <para>Interpolation will always lag a little bit behind but can be smoother than extrapolation.</para>
		/// </summary>
		Interpolate,
		/// <summary>
		///   <para>Extrapolation will predict the position of the rigidbody based on the current velocity.</para>
		/// </summary>
		Extrapolate
	}
}
