using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Constrains movement for a ConfigurableJoint along the 6 axes.</para>
	/// </summary>
	public enum ConfigurableJointMotion
	{
		/// <summary>
		///   <para>Motion along the axis will be locked.</para>
		/// </summary>
		Locked,
		/// <summary>
		///   <para>Motion along the axis will be limited by the respective limit.</para>
		/// </summary>
		Limited,
		/// <summary>
		///   <para>Motion along the axis will be completely free and completely unconstrained.</para>
		/// </summary>
		Free
	}
}
