using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Describes how physic materials of colliding objects are combined.</para>
	/// </summary>
	public enum PhysicMaterialCombine
	{
		/// <summary>
		///   <para>Averages the friction/bounce of the two colliding materials.</para>
		/// </summary>
		Average,
		/// <summary>
		///   <para>Uses the smaller friction/bounce of the two colliding materials.</para>
		/// </summary>
		Minimum = 2,
		/// <summary>
		///   <para>Multiplies the friction/bounce of the two colliding materials.</para>
		/// </summary>
		Multiply = 1,
		/// <summary>
		///   <para>Uses the larger friction/bounce of the two colliding materials.</para>
		/// </summary>
		Maximum = 3
	}
}
