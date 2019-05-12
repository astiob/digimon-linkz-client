using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Priority of a thread.</para>
	/// </summary>
	public enum ThreadPriority
	{
		/// <summary>
		///   <para>Lowest thread priority.</para>
		/// </summary>
		Low,
		/// <summary>
		///   <para>Below normal thread priority.</para>
		/// </summary>
		BelowNormal,
		/// <summary>
		///   <para>Normal thread priority.</para>
		/// </summary>
		Normal,
		/// <summary>
		///   <para>Highest thread priority.</para>
		/// </summary>
		High = 4
	}
}
