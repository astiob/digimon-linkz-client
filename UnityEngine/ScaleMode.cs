using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Scaling mode to draw textures with.</para>
	/// </summary>
	public enum ScaleMode
	{
		/// <summary>
		///   <para>Stretches the texture to fill the complete rectangle passed in to GUI.DrawTexture.</para>
		/// </summary>
		StretchToFill,
		/// <summary>
		///   <para>Scales the texture, maintaining aspect ratio, so it completely covers the position rectangle passed to GUI.DrawTexture. If the texture is being draw to a rectangle with a different aspect ratio than the original, the image is cropped.</para>
		/// </summary>
		ScaleAndCrop,
		/// <summary>
		///   <para>Scales the texture, maintaining aspect ratio, so it completely fits withing the position rectangle passed to GUI.DrawTexture.</para>
		/// </summary>
		ScaleToFit
	}
}
