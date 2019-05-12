using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Use these flags to constrain motion of the Rigidbody2D.</para>
	/// </summary>
	[Flags]
	public enum RigidbodyConstraints2D
	{
		/// <summary>
		///   <para>No constraints.</para>
		/// </summary>
		None = 0,
		/// <summary>
		///   <para>Freeze motion along the X-axis.</para>
		/// </summary>
		FreezePositionX = 1,
		/// <summary>
		///   <para>Freeze motion along the Y-axis.</para>
		/// </summary>
		FreezePositionY = 2,
		/// <summary>
		///   <para>Freeze rotation along the Z-axis.</para>
		/// </summary>
		FreezeRotation = 4,
		/// <summary>
		///   <para>Freeze motion along the X-axis and Y-axis.</para>
		/// </summary>
		FreezePosition = 3,
		/// <summary>
		///   <para>Freeze rotation and motion along all axes.</para>
		/// </summary>
		FreezeAll = 7
	}
}
