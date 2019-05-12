using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Status of path.</para>
	/// </summary>
	public enum NavMeshPathStatus
	{
		/// <summary>
		///   <para>The path terminates at the destination.</para>
		/// </summary>
		PathComplete,
		/// <summary>
		///   <para>The path cannot reach the destination.</para>
		/// </summary>
		PathPartial,
		/// <summary>
		///   <para>The path is invalid.</para>
		/// </summary>
		PathInvalid
	}
}
