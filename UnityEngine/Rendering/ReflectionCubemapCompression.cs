using System;

namespace UnityEngine.Rendering
{
	/// <summary>
	///   <para>Determines how Unity will compress baked reflection cubemap.</para>
	/// </summary>
	public enum ReflectionCubemapCompression
	{
		/// <summary>
		///   <para>Baked Reflection cubemap will be left uncompressed.</para>
		/// </summary>
		Uncompressed,
		/// <summary>
		///   <para>Baked Reflection cubemap will be compressed.</para>
		/// </summary>
		Compressed,
		/// <summary>
		///   <para>Baked Reflection cubemap will be compressed if compression format is suitable.</para>
		/// </summary>
		Auto
	}
}
