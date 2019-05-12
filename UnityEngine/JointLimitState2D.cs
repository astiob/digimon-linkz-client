using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Represents the state of a joint limit.</para>
	/// </summary>
	public enum JointLimitState2D
	{
		/// <summary>
		///   <para>Represents a state where the joint limit is inactive.</para>
		/// </summary>
		Inactive,
		/// <summary>
		///   <para>Represents a state where the joint limit is at the specified lower limit.</para>
		/// </summary>
		LowerLimit,
		/// <summary>
		///   <para>Represents a state where the joint limit is at the specified upper limit.</para>
		/// </summary>
		UpperLimit,
		/// <summary>
		///   <para>Represents a state where the joint limit is at the specified lower and upper limits (they are identical).</para>
		/// </summary>
		EqualLimits
	}
}
