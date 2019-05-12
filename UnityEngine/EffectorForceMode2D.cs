using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>The mode used to apply Effector2D forces.</para>
	/// </summary>
	public enum EffectorForceMode2D
	{
		/// <summary>
		///   <para>The force is applied at a constant rate.</para>
		/// </summary>
		Constant,
		/// <summary>
		///   <para>The force is applied inverse-linear relative to a point.</para>
		/// </summary>
		InverseLinear,
		/// <summary>
		///   <para>The force is applied inverse-squared relative to a point.</para>
		/// </summary>
		InverseSquared
	}
}
