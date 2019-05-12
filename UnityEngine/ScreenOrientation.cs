using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Describes screen orientation.</para>
	/// </summary>
	public enum ScreenOrientation
	{
		Unknown,
		/// <summary>
		///   <para>Portrait orientation.</para>
		/// </summary>
		Portrait,
		/// <summary>
		///   <para>Portrait orientation, upside down.</para>
		/// </summary>
		PortraitUpsideDown,
		/// <summary>
		///   <para>Landscape orientation, counter-clockwise from the portrait orientation.</para>
		/// </summary>
		LandscapeLeft,
		/// <summary>
		///   <para>Landscape orientation, clockwise from the portrait orientation.</para>
		/// </summary>
		LandscapeRight,
		/// <summary>
		///   <para>Auto-rotates the screen as necessary toward any of the enabled orientations.</para>
		/// </summary>
		AutoRotation,
		Landscape = 3
	}
}
