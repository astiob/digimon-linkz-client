using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Rendering;

namespace UnityEngine
{
	[NativeHeader("Runtime/Graphics/Mesh/MeshScriptBindings.h")]
	public sealed class Mesh : Object
	{
		public Mesh()
		{
			Mesh.Internal_Create(this);
		}

		[FreeFunction("MeshScripting::CreateMesh")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_Create([Writable] Mesh mono);

		[FreeFunction("MeshScripting::MeshFromInstaceId")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Mesh FromInstanceID(int id);

		public extern IndexFormat indexFormat { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[FreeFunction(Name = "MeshScripting::GetIndexStart", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern uint GetIndexStartImpl(int submesh);

		[FreeFunction(Name = "MeshScripting::GetIndexCount", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern uint GetIndexCountImpl(int submesh);

		[FreeFunction(Name = "MeshScripting::GetBaseVertex", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern uint GetBaseVertexImpl(int submesh);

		[FreeFunction(Name = "AllocExtractMeshTrianglesFromScript", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int[] GetTrianglesImpl(int submesh, bool applyBaseVertex);

		[FreeFunction(Name = "AllocExtractMeshIndicesFromScript", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int[] GetIndicesImpl(int submesh, bool applyBaseVertex);

		[FreeFunction(Name = "SetMeshIndicesFromScript", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetIndicesImpl(int submesh, MeshTopology topology, Array indices, int arraySize, bool calculateBounds, int baseVertex);

		[FreeFunction(Name = "ExtractMeshTrianglesToArrayFromScript", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetTrianglesNonAllocImpl(Array values, int submesh, bool applyBaseVertex);

		[FreeFunction(Name = "ExtractMeshIndicesToArrayFromScript", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetIndicesNonAllocImpl(Array values, int submesh, bool applyBaseVertex);

		[FreeFunction(Name = "MeshScripting::PrintErrorCantAccessChannel", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void PrintErrorCantAccessChannel(Mesh.InternalShaderChannel ch);

		[FreeFunction(Name = "MeshScripting::HasChannel", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool HasChannel(Mesh.InternalShaderChannel ch);

		[FreeFunction(Name = "SetMeshComponentFromArrayFromScript", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetArrayForChannelImpl(Mesh.InternalShaderChannel channel, Mesh.InternalVertexChannelType format, int dim, Array values, int arraySize);

		[FreeFunction(Name = "AllocExtractMeshComponentFromScript", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Array GetAllocArrayFromChannelImpl(Mesh.InternalShaderChannel channel, Mesh.InternalVertexChannelType format, int dim);

		[FreeFunction(Name = "ExtractMeshComponentFromScript", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetArrayFromChannelImpl(Mesh.InternalShaderChannel channel, Mesh.InternalVertexChannelType format, int dim, Array values);

		public extern int vertexBufferCount { [FreeFunction(Name = "MeshScripting::GetVertexBufferCount", HasExplicitThis = true)] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[NativeThrows]
		[FreeFunction(Name = "MeshScripting::GetNativeVertexBufferPtr", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern IntPtr GetNativeVertexBufferPtr(int index);

		[FreeFunction(Name = "MeshScripting::GetNativeIndexBufferPtr", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern IntPtr GetNativeIndexBufferPtr();

		public extern int blendShapeCount { [NativeMethod(Name = "GetBlendShapeChannelCount")] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[FreeFunction(Name = "MeshScripting::ClearBlendShapes", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ClearBlendShapes();

		[FreeFunction(Name = "MeshScripting::GetBlendShapeName", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern string GetBlendShapeName(int shapeIndex);

		[FreeFunction(Name = "MeshScripting::GetBlendShapeIndex", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetBlendShapeIndex(string blendShapeName);

		[FreeFunction(Name = "MeshScripting::GetBlendShapeFrameCount", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetBlendShapeFrameCount(int shapeIndex);

		[FreeFunction(Name = "MeshScripting::GetBlendShapeFrameWeight", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float GetBlendShapeFrameWeight(int shapeIndex, int frameIndex);

		[FreeFunction(Name = "GetBlendShapeFrameVerticesFromScript", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void GetBlendShapeFrameVertices(int shapeIndex, int frameIndex, Vector3[] deltaVertices, Vector3[] deltaNormals, Vector3[] deltaTangents);

		[FreeFunction(Name = "AddBlendShapeFrameFromScript", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void AddBlendShapeFrame(string shapeName, float frameWeight, Vector3[] deltaVertices, Vector3[] deltaNormals, Vector3[] deltaTangents);

		[FreeFunction(Name = "MeshScripting::GetBoneWeightCount", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetBoneWeightCount();

		public extern BoneWeight[] boneWeights { [FreeFunction(Name = "MeshScripting::AllocExtractBoneWeights", HasExplicitThis = true)] [MethodImpl(MethodImplOptions.InternalCall)] get; [FreeFunction(Name = "MeshScripting::SetBoneWeights", HasExplicitThis = true)] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetBindposeCount();

		public extern Matrix4x4[] bindposes { [FreeFunction(Name = "MeshScripting::AllocExtractBindPoses", HasExplicitThis = true)] [MethodImpl(MethodImplOptions.InternalCall)] get; [FreeFunction(Name = "MeshScripting::SetBindPoses", HasExplicitThis = true)] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[FreeFunction(Name = "MeshScripting::ExtractBoneWeightsIntoArray", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetBoneWeightsNonAllocImpl(Array values);

		[FreeFunction(Name = "MeshScripting::ExtractBindPosesIntoArray", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetBindposesNonAllocImpl(Array values);

		public extern bool isReadable { [NativeMethod("GetIsReadable")] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		internal extern bool canAccess { [NativeMethod("CanAccessFromScript")] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern int vertexCount { [NativeMethod("GetVertexCount")] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern int subMeshCount { [NativeMethod(Name = "GetSubMeshCount")] [MethodImpl(MethodImplOptions.InternalCall)] get; [FreeFunction(Name = "MeshScripting::SetSubMeshCount", HasExplicitThis = true)] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public Bounds bounds
		{
			get
			{
				Bounds result;
				this.get_bounds_Injected(out result);
				return result;
			}
			set
			{
				this.set_bounds_Injected(ref value);
			}
		}

		[NativeMethod("Clear")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ClearImpl(bool keepVertexLayout);

		[NativeMethod("RecalculateBounds")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void RecalculateBoundsImpl();

		[NativeMethod("RecalculateNormals")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void RecalculateNormalsImpl();

		[NativeMethod("RecalculateTangents")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void RecalculateTangentsImpl();

		[NativeMethod("MarkDynamic")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void MarkDynamicImpl();

		[NativeMethod("UploadMeshData")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void UploadMeshDataImpl(bool markNoLogerReadable);

		[FreeFunction(Name = "MeshScripting::GetPrimitiveType", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern MeshTopology GetTopologyImpl(int submesh);

		[FreeFunction(Name = "MeshScripting::CombineMeshes", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void CombineMeshesImpl(CombineInstance[] combine, bool mergeSubMeshes, bool useMatrices, bool hasLightmapData);

		internal Mesh.InternalShaderChannel GetUVChannel(int uvIndex)
		{
			if (uvIndex < 0 || uvIndex > 3)
			{
				throw new ArgumentException("GetUVChannel called for bad uvIndex", "uvIndex");
			}
			return Mesh.InternalShaderChannel.TexCoord0 + uvIndex;
		}

		internal static int DefaultDimensionForChannel(Mesh.InternalShaderChannel channel)
		{
			int result;
			if (channel == Mesh.InternalShaderChannel.Vertex || channel == Mesh.InternalShaderChannel.Normal)
			{
				result = 3;
			}
			else if (channel >= Mesh.InternalShaderChannel.TexCoord0 && channel <= Mesh.InternalShaderChannel.TexCoord3)
			{
				result = 2;
			}
			else
			{
				if (channel != Mesh.InternalShaderChannel.Tangent && channel != Mesh.InternalShaderChannel.Color)
				{
					throw new ArgumentException("DefaultDimensionForChannel called for bad channel", "channel");
				}
				result = 4;
			}
			return result;
		}

		private T[] GetAllocArrayFromChannel<T>(Mesh.InternalShaderChannel channel, Mesh.InternalVertexChannelType format, int dim)
		{
			if (this.canAccess)
			{
				if (this.HasChannel(channel))
				{
					return (T[])this.GetAllocArrayFromChannelImpl(channel, format, dim);
				}
			}
			else
			{
				this.PrintErrorCantAccessChannel(channel);
			}
			return new T[0];
		}

		private T[] GetAllocArrayFromChannel<T>(Mesh.InternalShaderChannel channel)
		{
			return this.GetAllocArrayFromChannel<T>(channel, Mesh.InternalVertexChannelType.Float, Mesh.DefaultDimensionForChannel(channel));
		}

		private void SetSizedArrayForChannel(Mesh.InternalShaderChannel channel, Mesh.InternalVertexChannelType format, int dim, Array values, int valuesCount)
		{
			if (this.canAccess)
			{
				this.SetArrayForChannelImpl(channel, format, dim, values, valuesCount);
			}
			else
			{
				this.PrintErrorCantAccessChannel(channel);
			}
		}

		private void SetArrayForChannel<T>(Mesh.InternalShaderChannel channel, Mesh.InternalVertexChannelType format, int dim, T[] values)
		{
			this.SetSizedArrayForChannel(channel, format, dim, values, NoAllocHelpers.SafeLength(values));
		}

		private void SetArrayForChannel<T>(Mesh.InternalShaderChannel channel, T[] values)
		{
			this.SetSizedArrayForChannel(channel, Mesh.InternalVertexChannelType.Float, Mesh.DefaultDimensionForChannel(channel), values, NoAllocHelpers.SafeLength(values));
		}

		private void SetListForChannel<T>(Mesh.InternalShaderChannel channel, Mesh.InternalVertexChannelType format, int dim, List<T> values)
		{
			this.SetSizedArrayForChannel(channel, format, dim, NoAllocHelpers.ExtractArrayFromList(values), NoAllocHelpers.SafeLength<T>(values));
		}

		private void SetListForChannel<T>(Mesh.InternalShaderChannel channel, List<T> values)
		{
			this.SetSizedArrayForChannel(channel, Mesh.InternalVertexChannelType.Float, Mesh.DefaultDimensionForChannel(channel), NoAllocHelpers.ExtractArrayFromList(values), NoAllocHelpers.SafeLength<T>(values));
		}

		private void GetListForChannel<T>(List<T> buffer, int capacity, Mesh.InternalShaderChannel channel, int dim)
		{
			this.GetListForChannel<T>(buffer, capacity, channel, dim, Mesh.InternalVertexChannelType.Float);
		}

		private void GetListForChannel<T>(List<T> buffer, int capacity, Mesh.InternalShaderChannel channel, int dim, Mesh.InternalVertexChannelType channelType)
		{
			buffer.Clear();
			if (!this.canAccess)
			{
				this.PrintErrorCantAccessChannel(channel);
			}
			else if (this.HasChannel(channel))
			{
				this.PrepareUserBuffer<T>(buffer, capacity);
				this.GetArrayFromChannelImpl(channel, channelType, dim, NoAllocHelpers.ExtractArrayFromList(buffer));
			}
		}

		private void PrepareUserBuffer<T>(List<T> buffer, int capacity)
		{
			buffer.Clear();
			if (buffer.Capacity < capacity)
			{
				buffer.Capacity = capacity;
			}
			NoAllocHelpers.ResizeList(buffer, capacity);
		}

		public Vector3[] vertices
		{
			get
			{
				return this.GetAllocArrayFromChannel<Vector3>(Mesh.InternalShaderChannel.Vertex);
			}
			set
			{
				this.SetArrayForChannel<Vector3>(Mesh.InternalShaderChannel.Vertex, value);
			}
		}

		public Vector3[] normals
		{
			get
			{
				return this.GetAllocArrayFromChannel<Vector3>(Mesh.InternalShaderChannel.Normal);
			}
			set
			{
				this.SetArrayForChannel<Vector3>(Mesh.InternalShaderChannel.Normal, value);
			}
		}

		public Vector4[] tangents
		{
			get
			{
				return this.GetAllocArrayFromChannel<Vector4>(Mesh.InternalShaderChannel.Tangent);
			}
			set
			{
				this.SetArrayForChannel<Vector4>(Mesh.InternalShaderChannel.Tangent, value);
			}
		}

		public Vector2[] uv
		{
			get
			{
				return this.GetAllocArrayFromChannel<Vector2>(Mesh.InternalShaderChannel.TexCoord0);
			}
			set
			{
				this.SetArrayForChannel<Vector2>(Mesh.InternalShaderChannel.TexCoord0, value);
			}
		}

		public Vector2[] uv2
		{
			get
			{
				return this.GetAllocArrayFromChannel<Vector2>(Mesh.InternalShaderChannel.TexCoord1);
			}
			set
			{
				this.SetArrayForChannel<Vector2>(Mesh.InternalShaderChannel.TexCoord1, value);
			}
		}

		public Vector2[] uv3
		{
			get
			{
				return this.GetAllocArrayFromChannel<Vector2>(Mesh.InternalShaderChannel.TexCoord2);
			}
			set
			{
				this.SetArrayForChannel<Vector2>(Mesh.InternalShaderChannel.TexCoord2, value);
			}
		}

		public Vector2[] uv4
		{
			get
			{
				return this.GetAllocArrayFromChannel<Vector2>(Mesh.InternalShaderChannel.TexCoord3);
			}
			set
			{
				this.SetArrayForChannel<Vector2>(Mesh.InternalShaderChannel.TexCoord3, value);
			}
		}

		public Color[] colors
		{
			get
			{
				return this.GetAllocArrayFromChannel<Color>(Mesh.InternalShaderChannel.Color);
			}
			set
			{
				this.SetArrayForChannel<Color>(Mesh.InternalShaderChannel.Color, value);
			}
		}

		public Color32[] colors32
		{
			get
			{
				return this.GetAllocArrayFromChannel<Color32>(Mesh.InternalShaderChannel.Color, Mesh.InternalVertexChannelType.Color, 1);
			}
			set
			{
				this.SetArrayForChannel<Color32>(Mesh.InternalShaderChannel.Color, Mesh.InternalVertexChannelType.Color, 1, value);
			}
		}

		public void GetVertices(List<Vector3> vertices)
		{
			if (vertices == null)
			{
				throw new ArgumentNullException("The result vertices list cannot be null.", "vertices");
			}
			this.GetListForChannel<Vector3>(vertices, this.vertexCount, Mesh.InternalShaderChannel.Vertex, Mesh.DefaultDimensionForChannel(Mesh.InternalShaderChannel.Vertex));
		}

		public void SetVertices(List<Vector3> inVertices)
		{
			this.SetListForChannel<Vector3>(Mesh.InternalShaderChannel.Vertex, inVertices);
		}

		public void GetNormals(List<Vector3> normals)
		{
			if (normals == null)
			{
				throw new ArgumentNullException("The result normals list cannot be null.", "normals");
			}
			this.GetListForChannel<Vector3>(normals, this.vertexCount, Mesh.InternalShaderChannel.Normal, Mesh.DefaultDimensionForChannel(Mesh.InternalShaderChannel.Normal));
		}

		public void SetNormals(List<Vector3> inNormals)
		{
			this.SetListForChannel<Vector3>(Mesh.InternalShaderChannel.Normal, inNormals);
		}

		public void GetTangents(List<Vector4> tangents)
		{
			if (tangents == null)
			{
				throw new ArgumentNullException("The result tangents list cannot be null.", "tangents");
			}
			this.GetListForChannel<Vector4>(tangents, this.vertexCount, Mesh.InternalShaderChannel.Tangent, Mesh.DefaultDimensionForChannel(Mesh.InternalShaderChannel.Tangent));
		}

		public void SetTangents(List<Vector4> inTangents)
		{
			this.SetListForChannel<Vector4>(Mesh.InternalShaderChannel.Tangent, inTangents);
		}

		public void GetColors(List<Color> colors)
		{
			if (colors == null)
			{
				throw new ArgumentNullException("The result colors list cannot be null.", "colors");
			}
			this.GetListForChannel<Color>(colors, this.vertexCount, Mesh.InternalShaderChannel.Color, Mesh.DefaultDimensionForChannel(Mesh.InternalShaderChannel.Color));
		}

		public void SetColors(List<Color> inColors)
		{
			this.SetListForChannel<Color>(Mesh.InternalShaderChannel.Color, inColors);
		}

		public void GetColors(List<Color32> colors)
		{
			if (colors == null)
			{
				throw new ArgumentNullException("The result colors list cannot be null.", "colors");
			}
			this.GetListForChannel<Color32>(colors, this.vertexCount, Mesh.InternalShaderChannel.Color, 1, Mesh.InternalVertexChannelType.Color);
		}

		public void SetColors(List<Color32> inColors)
		{
			this.SetListForChannel<Color32>(Mesh.InternalShaderChannel.Color, Mesh.InternalVertexChannelType.Color, 1, inColors);
		}

		private void SetUvsImpl<T>(int uvIndex, int dim, List<T> uvs)
		{
			if (uvIndex < 0 || uvIndex > 3)
			{
				Debug.LogError("The uv index is invalid (must be in [0..3]");
			}
			else
			{
				this.SetListForChannel<T>(this.GetUVChannel(uvIndex), Mesh.InternalVertexChannelType.Float, dim, uvs);
			}
		}

		public void SetUVs(int channel, List<Vector2> uvs)
		{
			this.SetUvsImpl<Vector2>(channel, 2, uvs);
		}

		public void SetUVs(int channel, List<Vector3> uvs)
		{
			this.SetUvsImpl<Vector3>(channel, 3, uvs);
		}

		public void SetUVs(int channel, List<Vector4> uvs)
		{
			this.SetUvsImpl<Vector4>(channel, 4, uvs);
		}

		private void GetUVsImpl<T>(int uvIndex, List<T> uvs, int dim)
		{
			if (uvs == null)
			{
				throw new ArgumentNullException("The result uvs list cannot be null.", "uvs");
			}
			if (uvIndex < 0 || uvIndex > 3)
			{
				throw new IndexOutOfRangeException("Specified uv index is out of range. Must be in the range [0, 3].");
			}
			this.GetListForChannel<T>(uvs, this.vertexCount, this.GetUVChannel(uvIndex), dim);
		}

		public void GetUVs(int channel, List<Vector2> uvs)
		{
			this.GetUVsImpl<Vector2>(channel, uvs, 2);
		}

		public void GetUVs(int channel, List<Vector3> uvs)
		{
			this.GetUVsImpl<Vector3>(channel, uvs, 3);
		}

		public void GetUVs(int channel, List<Vector4> uvs)
		{
			this.GetUVsImpl<Vector4>(channel, uvs, 4);
		}

		private void PrintErrorCantAccessIndices()
		{
			Debug.LogError(string.Format("Not allowed to access triangles/indices on mesh '{0}' (isReadable is false; Read/Write must be enabled in import settings)", base.name));
		}

		private bool CheckCanAccessSubmesh(int submesh, bool errorAboutTriangles)
		{
			bool result;
			if (!this.canAccess)
			{
				this.PrintErrorCantAccessIndices();
				result = false;
			}
			else if (submesh < 0 || submesh >= this.subMeshCount)
			{
				Debug.LogError(string.Format("Failed getting {0}. Submesh index is out of bounds.", (!errorAboutTriangles) ? "indices" : "triangles"), this);
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		private bool CheckCanAccessSubmeshTriangles(int submesh)
		{
			return this.CheckCanAccessSubmesh(submesh, true);
		}

		private bool CheckCanAccessSubmeshIndices(int submesh)
		{
			return this.CheckCanAccessSubmesh(submesh, false);
		}

		public int[] triangles
		{
			get
			{
				int[] result;
				if (this.canAccess)
				{
					result = this.GetTrianglesImpl(-1, true);
				}
				else
				{
					this.PrintErrorCantAccessIndices();
					result = new int[0];
				}
				return result;
			}
			set
			{
				if (this.canAccess)
				{
					this.SetTrianglesImpl(-1, value, NoAllocHelpers.SafeLength(value), true, 0);
				}
				else
				{
					this.PrintErrorCantAccessIndices();
				}
			}
		}

		public int[] GetTriangles(int submesh)
		{
			return this.GetTriangles(submesh, true);
		}

		public int[] GetTriangles(int submesh, [UnityEngine.Internal.DefaultValue("true")] bool applyBaseVertex)
		{
			return (!this.CheckCanAccessSubmeshTriangles(submesh)) ? new int[0] : this.GetTrianglesImpl(submesh, applyBaseVertex);
		}

		public void GetTriangles(List<int> triangles, int submesh)
		{
			this.GetTriangles(triangles, submesh, true);
		}

		public void GetTriangles(List<int> triangles, int submesh, [UnityEngine.Internal.DefaultValue("true")] bool applyBaseVertex)
		{
			if (triangles == null)
			{
				throw new ArgumentNullException("The result triangles list cannot be null.", "triangles");
			}
			if (submesh < 0 || submesh >= this.subMeshCount)
			{
				throw new IndexOutOfRangeException("Specified sub mesh is out of range. Must be greater or equal to 0 and less than subMeshCount.");
			}
			this.PrepareUserBuffer<int>(triangles, (int)this.GetIndexCount(submesh));
			this.GetTrianglesNonAllocImpl(NoAllocHelpers.ExtractArrayFromList(triangles), submesh, applyBaseVertex);
		}

		public int[] GetIndices(int submesh)
		{
			return this.GetIndices(submesh, true);
		}

		public int[] GetIndices(int submesh, [UnityEngine.Internal.DefaultValue("true")] bool applyBaseVertex)
		{
			return (!this.CheckCanAccessSubmeshIndices(submesh)) ? new int[0] : this.GetIndicesImpl(submesh, applyBaseVertex);
		}

		public void GetIndices(List<int> indices, int submesh)
		{
			this.GetIndices(indices, submesh, true);
		}

		public void GetIndices(List<int> indices, int submesh, [UnityEngine.Internal.DefaultValue("true")] bool applyBaseVertex)
		{
			if (indices == null)
			{
				throw new ArgumentNullException("The result indices list cannot be null.", "indices");
			}
			if (submesh < 0 || submesh >= this.subMeshCount)
			{
				throw new IndexOutOfRangeException("Specified sub mesh is out of range. Must be greater or equal to 0 and less than subMeshCount.");
			}
			this.PrepareUserBuffer<int>(indices, (int)this.GetIndexCount(submesh));
			this.GetIndicesNonAllocImpl(NoAllocHelpers.ExtractArrayFromList(indices), submesh, applyBaseVertex);
		}

		public uint GetIndexStart(int submesh)
		{
			if (submesh < 0 || submesh >= this.subMeshCount)
			{
				throw new IndexOutOfRangeException("Specified sub mesh is out of range. Must be greater or equal to 0 and less than subMeshCount.");
			}
			return this.GetIndexStartImpl(submesh);
		}

		public uint GetIndexCount(int submesh)
		{
			if (submesh < 0 || submesh >= this.subMeshCount)
			{
				throw new IndexOutOfRangeException("Specified sub mesh is out of range. Must be greater or equal to 0 and less than subMeshCount.");
			}
			return this.GetIndexCountImpl(submesh);
		}

		public uint GetBaseVertex(int submesh)
		{
			if (submesh < 0 || submesh >= this.subMeshCount)
			{
				throw new IndexOutOfRangeException("Specified sub mesh is out of range. Must be greater or equal to 0 and less than subMeshCount.");
			}
			return this.GetBaseVertexImpl(submesh);
		}

		private void SetTrianglesImpl(int submesh, Array triangles, int arraySize, bool calculateBounds, int baseVertex)
		{
			this.SetIndicesImpl(submesh, MeshTopology.Triangles, triangles, arraySize, calculateBounds, baseVertex);
		}

		public void SetTriangles(int[] triangles, int submesh)
		{
			this.SetTriangles(triangles, submesh, true, 0);
		}

		public void SetTriangles(int[] triangles, int submesh, bool calculateBounds)
		{
			this.SetTriangles(triangles, submesh, calculateBounds, 0);
		}

		public void SetTriangles(int[] triangles, int submesh, [UnityEngine.Internal.DefaultValue("true")] bool calculateBounds, [UnityEngine.Internal.DefaultValue("0")] int baseVertex)
		{
			if (this.CheckCanAccessSubmeshTriangles(submesh))
			{
				this.SetTrianglesImpl(submesh, triangles, NoAllocHelpers.SafeLength(triangles), calculateBounds, baseVertex);
			}
		}

		public void SetTriangles(List<int> triangles, int submesh)
		{
			this.SetTriangles(triangles, submesh, true, 0);
		}

		public void SetTriangles(List<int> triangles, int submesh, bool calculateBounds)
		{
			this.SetTriangles(triangles, submesh, calculateBounds, 0);
		}

		public void SetTriangles(List<int> triangles, int submesh, [UnityEngine.Internal.DefaultValue("true")] bool calculateBounds, [UnityEngine.Internal.DefaultValue("0")] int baseVertex)
		{
			if (this.CheckCanAccessSubmeshTriangles(submesh))
			{
				this.SetTrianglesImpl(submesh, NoAllocHelpers.ExtractArrayFromList(triangles), NoAllocHelpers.SafeLength<int>(triangles), calculateBounds, baseVertex);
			}
		}

		public void SetIndices(int[] indices, MeshTopology topology, int submesh)
		{
			this.SetIndices(indices, topology, submesh, true, 0);
		}

		public void SetIndices(int[] indices, MeshTopology topology, int submesh, bool calculateBounds)
		{
			this.SetIndices(indices, topology, submesh, calculateBounds, 0);
		}

		public void SetIndices(int[] indices, MeshTopology topology, int submesh, [UnityEngine.Internal.DefaultValue("true")] bool calculateBounds, [UnityEngine.Internal.DefaultValue("0")] int baseVertex)
		{
			if (this.CheckCanAccessSubmeshIndices(submesh))
			{
				this.SetIndicesImpl(submesh, topology, indices, NoAllocHelpers.SafeLength(indices), calculateBounds, baseVertex);
			}
		}

		public void GetBindposes(List<Matrix4x4> bindposes)
		{
			if (bindposes == null)
			{
				throw new ArgumentNullException("The result bindposes list cannot be null.", "bindposes");
			}
			this.PrepareUserBuffer<Matrix4x4>(bindposes, this.GetBindposeCount());
			this.GetBindposesNonAllocImpl(NoAllocHelpers.ExtractArrayFromList(bindposes));
		}

		public void GetBoneWeights(List<BoneWeight> boneWeights)
		{
			if (boneWeights == null)
			{
				throw new ArgumentNullException("The result boneWeights list cannot be null.", "boneWeights");
			}
			this.PrepareUserBuffer<BoneWeight>(boneWeights, this.GetBoneWeightCount());
			this.GetBoneWeightsNonAllocImpl(NoAllocHelpers.ExtractArrayFromList(boneWeights));
		}

		public void Clear(bool keepVertexLayout)
		{
			this.ClearImpl(keepVertexLayout);
		}

		public void Clear()
		{
			this.ClearImpl(true);
		}

		public void RecalculateBounds()
		{
			if (this.canAccess)
			{
				this.RecalculateBoundsImpl();
			}
			else
			{
				Debug.LogError(string.Format("Not allowed to call RecalculateBounds() on mesh '{0}'", base.name));
			}
		}

		public void RecalculateNormals()
		{
			if (this.canAccess)
			{
				this.RecalculateNormalsImpl();
			}
			else
			{
				Debug.LogError(string.Format("Not allowed to call RecalculateNormals() on mesh '{0}'", base.name));
			}
		}

		public void RecalculateTangents()
		{
			if (this.canAccess)
			{
				this.RecalculateTangentsImpl();
			}
			else
			{
				Debug.LogError(string.Format("Not allowed to call RecalculateTangents() on mesh '{0}'", base.name));
			}
		}

		public void MarkDynamic()
		{
			if (this.canAccess)
			{
				this.MarkDynamicImpl();
			}
		}

		public void UploadMeshData(bool markNoLogerReadable)
		{
			if (this.canAccess)
			{
				this.UploadMeshDataImpl(markNoLogerReadable);
			}
		}

		public MeshTopology GetTopology(int submesh)
		{
			MeshTopology result;
			if (submesh < 0 || submesh >= this.subMeshCount)
			{
				Debug.LogError(string.Format("Failed getting topology. Submesh index is out of bounds.", new object[0]), this);
				result = MeshTopology.Triangles;
			}
			else
			{
				result = this.GetTopologyImpl(submesh);
			}
			return result;
		}

		public void CombineMeshes(CombineInstance[] combine, [UnityEngine.Internal.DefaultValue("true")] bool mergeSubMeshes, [UnityEngine.Internal.DefaultValue("true")] bool useMatrices, [UnityEngine.Internal.DefaultValue("false")] bool hasLightmapData)
		{
			this.CombineMeshesImpl(combine, mergeSubMeshes, useMatrices, hasLightmapData);
		}

		public void CombineMeshes(CombineInstance[] combine, bool mergeSubMeshes, bool useMatrices)
		{
			this.CombineMeshesImpl(combine, mergeSubMeshes, useMatrices, false);
		}

		public void CombineMeshes(CombineInstance[] combine, bool mergeSubMeshes)
		{
			this.CombineMeshesImpl(combine, mergeSubMeshes, true, false);
		}

		public void CombineMeshes(CombineInstance[] combine)
		{
			this.CombineMeshesImpl(combine, true, true, false);
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("This method is no longer supported (UnityUpgradable)", true)]
		public void Optimize()
		{
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_bounds_Injected(out Bounds ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_bounds_Injected(ref Bounds value);

		internal enum InternalShaderChannel
		{
			Vertex,
			Normal,
			Color,
			TexCoord0,
			TexCoord1,
			TexCoord2,
			TexCoord3,
			Tangent
		}

		internal enum InternalVertexChannelType
		{
			Float,
			Color = 2
		}
	}
}
