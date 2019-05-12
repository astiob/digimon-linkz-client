using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Cubemap face.</para>
	/// </summary>
	public enum CubemapFace
	{
		/// <summary>
		///   <para>Cubemap face is unknown or unspecified.</para>
		/// </summary>
		Unknown = -1,
		/// <summary>
		///   <para>Right facing side (+x).</para>
		/// </summary>
		PositiveX,
		/// <summary>
		///   <para>Left facing side (-x).</para>
		/// </summary>
		NegativeX,
		/// <summary>
		///   <para>Upwards facing side (+y).</para>
		/// </summary>
		PositiveY,
		/// <summary>
		///   <para>Downward facing side (-y).</para>
		/// </summary>
		NegativeY,
		/// <summary>
		///   <para>Forward facing side (+z).</para>
		/// </summary>
		PositiveZ,
		/// <summary>
		///   <para>Backward facing side (-z).</para>
		/// </summary>
		NegativeZ
	}
}
