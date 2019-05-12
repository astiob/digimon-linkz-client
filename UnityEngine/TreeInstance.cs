using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Contains information about a tree placed in the Terrain game object.</para>
	/// </summary>
	public struct TreeInstance
	{
		/// <summary>
		///   <para>Position of the tree.</para>
		/// </summary>
		public Vector3 position;

		/// <summary>
		///   <para>Width scale of this instance (compared to the prototype's size).</para>
		/// </summary>
		public float widthScale;

		/// <summary>
		///   <para>Height scale of this instance (compared to the prototype's size).</para>
		/// </summary>
		public float heightScale;

		/// <summary>
		///   <para>Rotation of the tree on X-Z plane (in radians).</para>
		/// </summary>
		public float rotation;

		/// <summary>
		///   <para>Color of this instance.</para>
		/// </summary>
		public Color32 color;

		/// <summary>
		///   <para>Lightmap color calculated for this instance.</para>
		/// </summary>
		public Color32 lightmapColor;

		/// <summary>
		///   <para>Index of this instance in the TerrainData.treePrototypes array.</para>
		/// </summary>
		public int prototypeIndex;

		internal float temporaryDistance;
	}
}
