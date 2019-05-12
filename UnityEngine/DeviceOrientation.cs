using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Describes physical orientation of the device as determined by the OS.</para>
	/// </summary>
	public enum DeviceOrientation
	{
		/// <summary>
		///   <para>The orientation of the device cannot be determined.</para>
		/// </summary>
		Unknown,
		/// <summary>
		///   <para>The device is in portrait mode, with the device held upright and the home button at the bottom.</para>
		/// </summary>
		Portrait,
		/// <summary>
		///   <para>The device is in portrait mode but upside down, with the device held upright and the home button at the top.</para>
		/// </summary>
		PortraitUpsideDown,
		/// <summary>
		///   <para>The device is in landscape mode, with the device held upright and the home button on the right side.</para>
		/// </summary>
		LandscapeLeft,
		/// <summary>
		///   <para>The device is in landscape mode, with the device held upright and the home button on the left side.</para>
		/// </summary>
		LandscapeRight,
		/// <summary>
		///   <para>The device is held parallel to the ground with the screen facing upwards.</para>
		/// </summary>
		FaceUp,
		/// <summary>
		///   <para>The device is held parallel to the ground with the screen facing downwards.</para>
		/// </summary>
		FaceDown
	}
}
