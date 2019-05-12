using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Wrapping modes for text that reaches the vertical boundary.</para>
	/// </summary>
	public enum VerticalWrapMode
	{
		/// <summary>
		///   <para>Text will be clipped when reaching the vertical boundary.</para>
		/// </summary>
		Truncate,
		/// <summary>
		///   <para>Text well continue to generate when reaching vertical boundary.</para>
		/// </summary>
		Overflow
	}
}
