using System;

namespace UnityEngine.Rendering
{
	/// <summary>
	///   <para>Graphics device API type.</para>
	/// </summary>
	public enum GraphicsDeviceType
	{
		/// <summary>
		///   <para>OpenGL 2.x graphics API.</para>
		/// </summary>
		OpenGL2,
		/// <summary>
		///   <para>Direct3D 9 graphics API.</para>
		/// </summary>
		Direct3D9,
		/// <summary>
		///   <para>Direct3D 11 graphics API.</para>
		/// </summary>
		Direct3D11,
		/// <summary>
		///   <para>PlayStation 3 graphics API.</para>
		/// </summary>
		PlayStation3,
		/// <summary>
		///   <para>No graphics API.</para>
		/// </summary>
		Null,
		/// <summary>
		///   <para>Xbox 360 graphics API.</para>
		/// </summary>
		Xbox360 = 6,
		/// <summary>
		///   <para>OpenGL ES 2.0 graphics API.</para>
		/// </summary>
		OpenGLES2 = 8,
		/// <summary>
		///   <para>OpenGL ES 3.0 graphics API.</para>
		/// </summary>
		OpenGLES3 = 11,
		/// <summary>
		///   <para>PlayStation Vita graphics API.</para>
		/// </summary>
		PlayStationVita,
		/// <summary>
		///   <para>PlayStation 4 graphics API.</para>
		/// </summary>
		PlayStation4,
		/// <summary>
		///   <para>Xbox One graphics API.</para>
		/// </summary>
		XboxOne,
		/// <summary>
		///   <para>PlayStation Mobile (PSM) graphics API.</para>
		/// </summary>
		PlayStationMobile,
		/// <summary>
		///   <para>iOS Metal graphics API.</para>
		/// </summary>
		Metal,
		/// <summary>
		///   <para>OpenGL (Core profile - GL3 or later) graphics API.</para>
		/// </summary>
		OpenGLCore,
		/// <summary>
		///   <para>Direct3D 12 graphics API.</para>
		/// </summary>
		Direct3D12,
		/// <summary>
		///   <para>Nintendo 3DS graphics API.</para>
		/// </summary>
		Nintendo3DS
	}
}
