using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Settings for a Rigidbody2D's initial sleep state.</para>
	/// </summary>
	public enum RigidbodySleepMode2D
	{
		/// <summary>
		///   <para>Rigidbody2D never automatically sleeps.</para>
		/// </summary>
		NeverSleep,
		/// <summary>
		///   <para>Rigidbody2D is initially awake.</para>
		/// </summary>
		StartAwake,
		/// <summary>
		///   <para>Rigidbody2D is initially asleep.</para>
		/// </summary>
		StartAsleep
	}
}
