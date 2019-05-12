using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Depth texture generation mode for Camera.</para>
	/// </summary>
	[Flags]
	public enum DepthTextureMode
	{
		/// <summary>
		///   <para>Do not generate depth texture (Default).</para>
		/// </summary>
		None = 0,
		/// <summary>
		///   <para>Generate a depth texture.</para>
		/// </summary>
		Depth = 1,
		/// <summary>
		///   <para>Generate a depth + normals texture.</para>
		/// </summary>
		DepthNormals = 2
	}
}
