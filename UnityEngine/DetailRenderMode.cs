using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Render mode for detail prototypes.</para>
	/// </summary>
	public enum DetailRenderMode
	{
		/// <summary>
		///   <para>The detail prototype will be rendered as billboards that are always facing the camera.</para>
		/// </summary>
		GrassBillboard,
		/// <summary>
		///   <para>Will show the prototype using diffuse shading.</para>
		/// </summary>
		VertexLit,
		/// <summary>
		///   <para>The detail prototype will use the grass shader.</para>
		/// </summary>
		Grass
	}
}
