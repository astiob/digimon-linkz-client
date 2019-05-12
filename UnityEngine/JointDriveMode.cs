using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>The ConfigurableJoint attempts to attain position / velocity targets based on this flag.</para>
	/// </summary>
	[Flags]
	public enum JointDriveMode
	{
		/// <summary>
		///   <para>Don't apply any forces to reach the target.</para>
		/// </summary>
		None = 0,
		/// <summary>
		///   <para>Try to reach the specified target position.</para>
		/// </summary>
		Position = 1,
		/// <summary>
		///   <para>Try to reach the specified target velocity.</para>
		/// </summary>
		Velocity = 2,
		/// <summary>
		///   <para>Try to reach the specified target position and velocity.</para>
		/// </summary>
		PositionAndVelocity = 3
	}
}
