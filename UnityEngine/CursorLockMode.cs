using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>How the cursor should behave.</para>
	/// </summary>
	public enum CursorLockMode
	{
		/// <summary>
		///   <para>Cursor behavior is unmodified.</para>
		/// </summary>
		None,
		/// <summary>
		///   <para>Lock cursor to the center of the game window.</para>
		/// </summary>
		Locked,
		/// <summary>
		///   <para>Confine cursor to the game window.</para>
		/// </summary>
		Confined
	}
}
