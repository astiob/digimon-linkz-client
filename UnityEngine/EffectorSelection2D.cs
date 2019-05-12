using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Selects the source and/or target to be used by an Effector2D.</para>
	/// </summary>
	public enum EffectorSelection2D
	{
		/// <summary>
		///   <para>The source/target is defined by the Rigidbody2D.</para>
		/// </summary>
		Rigidbody,
		/// <summary>
		///   <para>The source/target is defined by the Collider2D.</para>
		/// </summary>
		Collider
	}
}
