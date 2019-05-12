using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Rendering path of a Camera.</para>
	/// </summary>
	public enum RenderingPath
	{
		/// <summary>
		///   <para>Use Player Settings.</para>
		/// </summary>
		UsePlayerSettings = -1,
		/// <summary>
		///   <para>Vertex Lit.</para>
		/// </summary>
		VertexLit,
		/// <summary>
		///   <para>Forward Rendering.</para>
		/// </summary>
		Forward,
		/// <summary>
		///   <para>Deferred Lighting (Legacy).
		/// </para>
		/// </summary>
		DeferredLighting,
		/// <summary>
		///   <para>Deferred Shading.
		/// </para>
		/// </summary>
		DeferredShading
	}
}
