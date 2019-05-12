using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Level of obstacle avoidance.</para>
	/// </summary>
	public enum ObstacleAvoidanceType
	{
		/// <summary>
		///   <para>Disable avoidance.</para>
		/// </summary>
		NoObstacleAvoidance,
		/// <summary>
		///   <para>Enable simple avoidance. Low performance impact.</para>
		/// </summary>
		LowQualityObstacleAvoidance,
		/// <summary>
		///   <para>Medium avoidance. Medium performance impact.</para>
		/// </summary>
		MedQualityObstacleAvoidance,
		/// <summary>
		///   <para>Good avoidance. High performance impact.</para>
		/// </summary>
		GoodQualityObstacleAvoidance,
		/// <summary>
		///   <para>Enable highest precision. Highest performance impact.</para>
		/// </summary>
		HighQualityObstacleAvoidance
	}
}
