using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Overrides the global Physics.queriesHitTriggers.</para>
	/// </summary>
	public enum QueryTriggerInteraction
	{
		/// <summary>
		///   <para>Queries use the global Physics.queriesHitTriggers setting.</para>
		/// </summary>
		UseGlobal,
		/// <summary>
		///   <para>Queries never report Trigger hits.</para>
		/// </summary>
		Ignore,
		/// <summary>
		///   <para>Queries always report Trigger hits.</para>
		/// </summary>
		Collide
	}
}
