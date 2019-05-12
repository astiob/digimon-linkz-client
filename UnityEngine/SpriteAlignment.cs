using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>How a Sprite's graphic rectangle is aligned with its pivot point.</para>
	/// </summary>
	public enum SpriteAlignment
	{
		/// <summary>
		///   <para>Pivot is at the center of the graphic rectangle.</para>
		/// </summary>
		Center,
		/// <summary>
		///   <para>Pivot is at the top left corner of the graphic rectangle.</para>
		/// </summary>
		TopLeft,
		/// <summary>
		///   <para>Pivot is at the center of the top edge of the graphic rectangle.</para>
		/// </summary>
		TopCenter,
		/// <summary>
		///   <para>Pivot is at the top right corner of the graphic rectangle.</para>
		/// </summary>
		TopRight,
		/// <summary>
		///   <para>Pivot is at the center of the left edge of the graphic rectangle.</para>
		/// </summary>
		LeftCenter,
		/// <summary>
		///   <para>Pivot is at the center of the right edge of the graphic rectangle.</para>
		/// </summary>
		RightCenter,
		/// <summary>
		///   <para>Pivot is at the bottom left corner of the graphic rectangle.</para>
		/// </summary>
		BottomLeft,
		/// <summary>
		///   <para>Pivot is at the center of the bottom edge of the graphic rectangle.</para>
		/// </summary>
		BottomCenter,
		/// <summary>
		///   <para>Pivot is at the bottom right corner of the graphic rectangle.</para>
		/// </summary>
		BottomRight,
		/// <summary>
		///   <para>Pivot is at a custom position within the graphic rectangle.</para>
		/// </summary>
		Custom
	}
}
