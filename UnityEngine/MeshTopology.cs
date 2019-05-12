using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Topology of Mesh faces.</para>
	/// </summary>
	public enum MeshTopology
	{
		/// <summary>
		///   <para>Mesh is made from triangles.</para>
		/// </summary>
		Triangles,
		/// <summary>
		///   <para>Mesh is made from quads.</para>
		/// </summary>
		Quads = 2,
		/// <summary>
		///   <para>Mesh is made from lines.</para>
		/// </summary>
		Lines,
		/// <summary>
		///   <para>Mesh is a line strip.</para>
		/// </summary>
		LineStrip,
		/// <summary>
		///   <para>Mesh is made from points.</para>
		/// </summary>
		Points
	}
}
