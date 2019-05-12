using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	/// <summary>
	///   <para>A class that allows creating or modifying meshes from scripts.</para>
	/// </summary>
	public sealed class Mesh : Object
	{
		/// <summary>
		///   <para>Creates an empty mesh.</para>
		/// </summary>
		public Mesh()
		{
			Mesh.Internal_Create(this);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_Create([Writable] Mesh mono);

		/// <summary>
		///   <para>Clears all vertex data and all triangle indices.</para>
		/// </summary>
		/// <param name="keepVertexLayout"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Clear([DefaultValue("true")] bool keepVertexLayout);

		[ExcludeFromDocs]
		public void Clear()
		{
			bool keepVertexLayout = true;
			this.Clear(keepVertexLayout);
		}

		/// <summary>
		///   <para>Returns state of the Read/Write Enabled checkbox when model was imported.</para>
		/// </summary>
		public extern bool isReadable { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		internal extern bool canAccess { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns a copy of the vertex positions or assigns a new vertex positions array.</para>
		/// </summary>
		public extern Vector3[] vertices { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public void SetVertices(List<Vector3> inVertices)
		{
			this.SetVerticesInternal(inVertices);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetVerticesInternal(object vertices);

		/// <summary>
		///   <para>The normals of the mesh.</para>
		/// </summary>
		public extern Vector3[] normals { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public void SetNormals(List<Vector3> inNormals)
		{
			this.SetNormalsInternal(inNormals);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetNormalsInternal(object normals);

		/// <summary>
		///   <para>The tangents of the mesh.</para>
		/// </summary>
		public extern Vector4[] tangents { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public void SetTangents(List<Vector4> inTangents)
		{
			this.SetTangentsInternal(inTangents);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetTangentsInternal(object tangents);

		/// <summary>
		///   <para>The base texture coordinates of the mesh.</para>
		/// </summary>
		public extern Vector2[] uv { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The second texture coordinate set of the mesh, if present.</para>
		/// </summary>
		public extern Vector2[] uv2 { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The third texture coordinate set of the mesh, if present.</para>
		/// </summary>
		public extern Vector2[] uv3 { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The fourth texture coordinate set of the mesh, if present.</para>
		/// </summary>
		public extern Vector2[] uv4 { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public void SetUVs(int channel, List<Vector2> uvs)
		{
			this.SetUVInternal(uvs, channel, 2);
		}

		public void SetUVs(int channel, List<Vector3> uvs)
		{
			this.SetUVInternal(uvs, channel, 3);
		}

		public void SetUVs(int channel, List<Vector4> uvs)
		{
			this.SetUVInternal(uvs, channel, 4);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetUVInternal(object uvs, int channel, int dim);

		/// <summary>
		///   <para>The bounding volume of the mesh.</para>
		/// </summary>
		public Bounds bounds
		{
			get
			{
				Bounds result;
				this.INTERNAL_get_bounds(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_bounds(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_bounds(out Bounds value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_bounds(ref Bounds value);

		/// <summary>
		///   <para>Vertex colors of the mesh.</para>
		/// </summary>
		public extern Color[] colors { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public void SetColors(List<Color> inColors)
		{
			this.SetColorsInternal(inColors);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetColorsInternal(object colors);

		/// <summary>
		///   <para>Vertex colors of the mesh.</para>
		/// </summary>
		public extern Color32[] colors32 { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public void SetColors(List<Color32> inColors)
		{
			this.SetColors32Internal(inColors);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetColors32Internal(object colors);

		/// <summary>
		///   <para>Recalculate the bounding volume of the mesh from the vertices.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RecalculateBounds();

		/// <summary>
		///   <para>Recalculates the normals of the mesh from the triangles and vertices.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RecalculateNormals();

		/// <summary>
		///   <para>Optimizes the mesh for display.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Optimize();

		/// <summary>
		///   <para>An array containing all triangles in the mesh.</para>
		/// </summary>
		public extern int[] triangles { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Returns the triangle list for the submesh.</para>
		/// </summary>
		/// <param name="submesh"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int[] GetTriangles(int submesh);

		/// <summary>
		///   <para>Sets the triangle list for the submesh.</para>
		/// </summary>
		/// <param name="inTriangles"></param>
		/// <param name="submesh"></param>
		/// <param name="triangles"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetTriangles(int[] triangles, int submesh);

		public void SetTriangles(List<int> inTriangles, int submesh)
		{
			this.SetTrianglesInternal(inTriangles, submesh);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetTrianglesInternal(object triangles, int submesh);

		/// <summary>
		///   <para>Returns the index buffer for the submesh.</para>
		/// </summary>
		/// <param name="submesh"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int[] GetIndices(int submesh);

		/// <summary>
		///   <para>Sets the index buffer for the submesh.</para>
		/// </summary>
		/// <param name="indices"></param>
		/// <param name="topology"></param>
		/// <param name="submesh"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetIndices(int[] indices, MeshTopology topology, int submesh);

		/// <summary>
		///   <para>Gets the topology of a submesh.</para>
		/// </summary>
		/// <param name="submesh"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern MeshTopology GetTopology(int submesh);

		/// <summary>
		///   <para>Returns the number of vertices in the mesh (Read Only).</para>
		/// </summary>
		public extern int vertexCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The number of submeshes. Every material has a separate triangle list.</para>
		/// </summary>
		public extern int subMeshCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Combines several meshes into this mesh.</para>
		/// </summary>
		/// <param name="combine">Descriptions of the meshes to combine.</param>
		/// <param name="mergeSubMeshes">Should all meshes be combined into a single submesh?</param>
		/// <param name="useMatrices">Should the transforms supplied in the CombineInstance array be used or ignored?</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void CombineMeshes(CombineInstance[] combine, [DefaultValue("true")] bool mergeSubMeshes, [DefaultValue("true")] bool useMatrices);

		[ExcludeFromDocs]
		public void CombineMeshes(CombineInstance[] combine, bool mergeSubMeshes)
		{
			bool useMatrices = true;
			this.CombineMeshes(combine, mergeSubMeshes, useMatrices);
		}

		[ExcludeFromDocs]
		public void CombineMeshes(CombineInstance[] combine)
		{
			bool useMatrices = true;
			bool mergeSubMeshes = true;
			this.CombineMeshes(combine, mergeSubMeshes, useMatrices);
		}

		/// <summary>
		///   <para>The bone weights of each vertex.</para>
		/// </summary>
		public extern BoneWeight[] boneWeights { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The bind poses. The bind pose at each index refers to the bone with the same index.</para>
		/// </summary>
		public extern Matrix4x4[] bindposes { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Optimize mesh for frequent updates.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void MarkDynamic();

		/// <summary>
		///   <para>Upload previously done mesh modifications to the graphics API.</para>
		/// </summary>
		/// <param name="markNoLogerReadable">Frees up system memory copy of mesh data when set to true.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void UploadMeshData(bool markNoLogerReadable);

		/// <summary>
		///   <para>Returns BlendShape count on this mesh.</para>
		/// </summary>
		public extern int blendShapeCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns name of BlendShape by given index.</para>
		/// </summary>
		/// <param name="index"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string GetBlendShapeName(int index);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetBlendShapeIndex(string blendShapeName);
	}
}
