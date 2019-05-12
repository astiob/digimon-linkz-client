using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Different methods for how the GUI system handles text being too large to fit the rectangle allocated.</para>
	/// </summary>
	public enum TextClipping
	{
		/// <summary>
		///   <para>Text flows freely outside the element.</para>
		/// </summary>
		Overflow,
		/// <summary>
		///   <para>Text gets clipped to be inside the element.</para>
		/// </summary>
		Clip
	}
}
