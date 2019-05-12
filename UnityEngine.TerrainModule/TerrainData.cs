using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[NativeHeader("TerrainScriptingClasses.h")]
	[NativeHeader("Modules/Terrain/Public/TerrainDataScriptingInterface.h")]
	public sealed class TerrainData : Object
	{
		private const string k_ScriptingInterfaceName = "TerrainDataScriptingInterface";

		private const string k_ScriptingInterfacePrefix = "TerrainDataScriptingInterface::";

		private const string k_HeightmapPrefix = "GetHeightmap().";

		private const string k_DetailDatabasePrefix = "GetDetailDatabase().";

		private const string k_TreeDatabasePrefix = "GetTreeDatabase().";

		private const string k_SplatDatabasePrefix = "GetSplatDatabase().";

		private static readonly int k_MaximumResolution = TerrainData.GetBoundaryValue(TerrainData.BoundaryValueType.MaxHeightmapRes);

		private static readonly int k_MinimumDetailResolutionPerPatch = TerrainData.GetBoundaryValue(TerrainData.BoundaryValueType.MinDetailResPerPatch);

		private static readonly int k_MaximumDetailResolutionPerPatch = TerrainData.GetBoundaryValue(TerrainData.BoundaryValueType.MaxDetailResPerPatch);

		private static readonly int k_MaximumDetailPatchCount = TerrainData.GetBoundaryValue(TerrainData.BoundaryValueType.MaxDetailPatchCount);

		private static readonly int k_MinimumAlphamapResolution = TerrainData.GetBoundaryValue(TerrainData.BoundaryValueType.MinAlphamapRes);

		private static readonly int k_MaximumAlphamapResolution = TerrainData.GetBoundaryValue(TerrainData.BoundaryValueType.MaxAlphamapRes);

		private static readonly int k_MinimumBaseMapResolution = TerrainData.GetBoundaryValue(TerrainData.BoundaryValueType.MinBaseMapRes);

		private static readonly int k_MaximumBaseMapResolution = TerrainData.GetBoundaryValue(TerrainData.BoundaryValueType.MaxBaseMapRes);

		public TerrainData()
		{
			TerrainData.Internal_Create(this);
		}

		[ThreadSafe]
		[StaticAccessor("TerrainDataScriptingInterface", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetBoundaryValue(TerrainData.BoundaryValueType type);

		[FreeFunction("TerrainDataScriptingInterface::Create")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_Create([Writable] TerrainData terrainData);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool HasUser(GameObject user);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void AddUser(GameObject user);

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void RemoveUser(GameObject user);

		public extern int heightmapWidth { [NativeName("GetHeightmap().GetWidth")] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern int heightmapHeight { [NativeName("GetHeightmap().GetHeight")] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public int heightmapResolution
		{
			get
			{
				return this.internalHeightmapResolution;
			}
			set
			{
				int internalHeightmapResolution = value;
				if (value < 0 || value > TerrainData.k_MaximumResolution)
				{
					Debug.LogWarning("heightmapResolution is clamped to the range of [0, " + TerrainData.k_MaximumResolution + "].");
					internalHeightmapResolution = Math.Min(TerrainData.k_MaximumResolution, Math.Max(value, 0));
				}
				this.internalHeightmapResolution = internalHeightmapResolution;
			}
		}

		private extern int internalHeightmapResolution { [NativeName("GetHeightmap().GetResolution")] [MethodImpl(MethodImplOptions.InternalCall)] get; [NativeName("GetHeightmap().SetResolution")] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public Vector3 heightmapScale
		{
			[NativeName("GetHeightmap().GetScale")]
			get
			{
				Vector3 result;
				this.get_heightmapScale_Injected(out result);
				return result;
			}
		}

		public Vector3 size
		{
			[NativeName("GetHeightmap().GetSize")]
			get
			{
				Vector3 result;
				this.get_size_Injected(out result);
				return result;
			}
			[NativeName("GetHeightmap().SetSize")]
			set
			{
				this.set_size_Injected(ref value);
			}
		}

		public Bounds bounds
		{
			[NativeName("GetHeightmap().CalculateBounds")]
			get
			{
				Bounds result;
				this.get_bounds_Injected(out result);
				return result;
			}
		}

		public extern float thickness { [NativeName("GetHeightmap().GetThickness")] [MethodImpl(MethodImplOptions.InternalCall)] get; [NativeName("GetHeightmap().SetThickness")] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[NativeName("GetHeightmap().GetHeight")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float GetHeight(int x, int y);

		[NativeName("GetHeightmap().GetInterpolatedHeight")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float GetInterpolatedHeight(float x, float y);

		public float[,] GetHeights(int xBase, int yBase, int width, int height)
		{
			if (xBase < 0 || yBase < 0 || xBase + width < 0 || yBase + height < 0 || xBase + width > this.heightmapWidth || yBase + height > this.heightmapHeight)
			{
				throw new ArgumentException("Trying to access out-of-bounds terrain height information.");
			}
			return this.Internal_GetHeights(xBase, yBase, width, height);
		}

		[FreeFunction("TerrainDataScriptingInterface::GetHeights", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern float[,] Internal_GetHeights(int xBase, int yBase, int width, int height);

		public void SetHeights(int xBase, int yBase, float[,] heights)
		{
			if (heights == null)
			{
				throw new NullReferenceException();
			}
			if (xBase + heights.GetLength(1) > this.heightmapWidth || xBase + heights.GetLength(1) < 0 || yBase + heights.GetLength(0) < 0 || xBase < 0 || yBase < 0 || yBase + heights.GetLength(0) > this.heightmapHeight)
			{
				throw new ArgumentException(UnityString.Format("X or Y base out of bounds. Setting up to {0}x{1} while map size is {2}x{3}", new object[]
				{
					xBase + heights.GetLength(1),
					yBase + heights.GetLength(0),
					this.heightmapWidth,
					this.heightmapHeight
				}));
			}
			this.Internal_SetHeights(xBase, yBase, heights.GetLength(1), heights.GetLength(0), heights);
		}

		[FreeFunction("TerrainDataScriptingInterface::SetHeights", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetHeights(int xBase, int yBase, int width, int height, float[,] heights);

		public void SetHeightsDelayLOD(int xBase, int yBase, float[,] heights)
		{
			if (heights == null)
			{
				throw new ArgumentNullException("heights");
			}
			int length = heights.GetLength(0);
			int length2 = heights.GetLength(1);
			if (xBase < 0 || xBase + length2 < 0 || xBase + length2 > this.heightmapWidth)
			{
				throw new ArgumentException(UnityString.Format("X out of bounds - trying to set {0}-{1} but the terrain ranges from 0-{2}", new object[]
				{
					xBase,
					xBase + length2,
					this.heightmapWidth
				}));
			}
			if (yBase < 0 || yBase + length < 0 || yBase + length > this.heightmapHeight)
			{
				throw new ArgumentException(UnityString.Format("Y out of bounds - trying to set {0}-{1} but the terrain ranges from 0-{2}", new object[]
				{
					yBase,
					yBase + length,
					this.heightmapHeight
				}));
			}
			this.Internal_SetHeightsDelayLOD(xBase, yBase, length2, length, heights);
		}

		[FreeFunction("TerrainDataScriptingInterface::SetHeightsDelayLOD", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetHeightsDelayLOD(int xBase, int yBase, int width, int height, float[,] heights);

		[NativeName("GetHeightmap().GetSteepness")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float GetSteepness(float x, float y);

		[NativeName("GetHeightmap().GetInterpolatedNormal")]
		public Vector3 GetInterpolatedNormal(float x, float y)
		{
			Vector3 result;
			this.GetInterpolatedNormal_Injected(x, y, out result);
			return result;
		}

		[NativeName("GetHeightmap().GetAdjustedSize")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int GetAdjustedSize(int size);

		public extern float wavingGrassStrength { [NativeName("GetDetailDatabase().GetWavingGrassStrength")] [MethodImpl(MethodImplOptions.InternalCall)] get; [FreeFunction("TerrainDataScriptingInterface::SetWavingGrassStrength", HasExplicitThis = true)] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern float wavingGrassAmount { [NativeName("GetDetailDatabase().GetWavingGrassAmount")] [MethodImpl(MethodImplOptions.InternalCall)] get; [FreeFunction("TerrainDataScriptingInterface::SetWavingGrassAmount", HasExplicitThis = true)] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern float wavingGrassSpeed { [NativeName("GetDetailDatabase().GetWavingGrassSpeed")] [MethodImpl(MethodImplOptions.InternalCall)] get; [FreeFunction("TerrainDataScriptingInterface::SetWavingGrassSpeed", HasExplicitThis = true)] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public Color wavingGrassTint
		{
			[NativeName("GetDetailDatabase().GetWavingGrassTint")]
			get
			{
				Color result;
				this.get_wavingGrassTint_Injected(out result);
				return result;
			}
			[FreeFunction("TerrainDataScriptingInterface::SetWavingGrassTint", HasExplicitThis = true)]
			set
			{
				this.set_wavingGrassTint_Injected(ref value);
			}
		}

		public extern int detailWidth { [NativeName("GetDetailDatabase().GetWidth")] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern int detailHeight { [NativeName("GetDetailDatabase().GetHeight")] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public void SetDetailResolution(int detailResolution, int resolutionPerPatch)
		{
			if (detailResolution < 0)
			{
				Debug.LogWarning("detailResolution must not be negative.");
				detailResolution = 0;
			}
			if (resolutionPerPatch < TerrainData.k_MinimumDetailResolutionPerPatch || resolutionPerPatch > TerrainData.k_MaximumDetailResolutionPerPatch)
			{
				Debug.LogWarning(string.Concat(new object[]
				{
					"resolutionPerPatch is clamped to the range of [",
					TerrainData.k_MinimumDetailResolutionPerPatch,
					", ",
					TerrainData.k_MaximumDetailResolutionPerPatch,
					"]."
				}));
				resolutionPerPatch = Math.Min(TerrainData.k_MaximumDetailResolutionPerPatch, Math.Max(resolutionPerPatch, TerrainData.k_MinimumDetailResolutionPerPatch));
			}
			int num = detailResolution / resolutionPerPatch;
			if (num > TerrainData.k_MaximumDetailPatchCount)
			{
				Debug.LogWarning("Patch count (detailResolution / resolutionPerPatch) is clamped to the range of [0, " + TerrainData.k_MaximumDetailPatchCount + "].");
				num = Math.Min(TerrainData.k_MaximumDetailPatchCount, Math.Max(num, 0));
			}
			this.Internal_SetDetailResolution(num, resolutionPerPatch);
		}

		[NativeName("GetDetailDatabase().SetDetailResolution")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetDetailResolution(int patchCount, int resolutionPerPatch);

		public extern int detailResolution { [NativeName("GetDetailDatabase().GetResolution")] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		internal extern int detailResolutionPerPatch { [NativeName("GetDetailDatabase().GetResolutionPerPatch")] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[NativeName("GetDetailDatabase().ResetDirtyDetails")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void ResetDirtyDetails();

		[FreeFunction("TerrainDataScriptingInterface::RefreshPrototypes", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RefreshPrototypes();

		public extern DetailPrototype[] detailPrototypes { [FreeFunction("TerrainDataScriptingInterface::GetDetailPrototypes", HasExplicitThis = true)] [MethodImpl(MethodImplOptions.InternalCall)] get; [FreeFunction("TerrainDataScriptingInterface::SetDetailPrototypes", HasExplicitThis = true)] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[FreeFunction("TerrainDataScriptingInterface::GetSupportedLayers", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int[] GetSupportedLayers(int xBase, int yBase, int totalWidth, int totalHeight);

		[FreeFunction("TerrainDataScriptingInterface::GetDetailLayer", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int[,] GetDetailLayer(int xBase, int yBase, int width, int height, int layer);

		public void SetDetailLayer(int xBase, int yBase, int layer, int[,] details)
		{
			this.Internal_SetDetailLayer(xBase, yBase, details.GetLength(1), details.GetLength(0), layer, details);
		}

		[FreeFunction("TerrainDataScriptingInterface::SetDetailLayer", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetDetailLayer(int xBase, int yBase, int totalWidth, int totalHeight, int detailIndex, int[,] data);

		public TreeInstance[] treeInstances
		{
			get
			{
				return this.Internal_GetTreeInstances();
			}
			set
			{
				this.Internal_SetTreeInstances(value);
			}
		}

		[NativeName("GetTreeDatabase().GetInstances")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern TreeInstance[] Internal_GetTreeInstances();

		[FreeFunction("TerrainDataScriptingInterface::SetTreeInstances", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetTreeInstances([NotNull] TreeInstance[] instances);

		public TreeInstance GetTreeInstance(int index)
		{
			if (index < 0 || index >= this.treeInstanceCount)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return this.Internal_GetTreeInstance(index);
		}

		[FreeFunction("TerrainDataScriptingInterface::GetTreeInstance", HasExplicitThis = true)]
		private TreeInstance Internal_GetTreeInstance(int index)
		{
			TreeInstance result;
			this.Internal_GetTreeInstance_Injected(index, out result);
			return result;
		}

		[NativeThrows]
		[FreeFunction("TerrainDataScriptingInterface::SetTreeInstance", HasExplicitThis = true)]
		public void SetTreeInstance(int index, TreeInstance instance)
		{
			this.SetTreeInstance_Injected(index, ref instance);
		}

		public extern int treeInstanceCount { [NativeName("GetTreeDatabase().GetInstances().size")] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern TreePrototype[] treePrototypes { [FreeFunction("TerrainDataScriptingInterface::GetTreePrototypes", HasExplicitThis = true)] [MethodImpl(MethodImplOptions.InternalCall)] get; [FreeFunction("TerrainDataScriptingInterface::SetTreePrototypes", HasExplicitThis = true)] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[NativeName("GetTreeDatabase().RemoveTreePrototype")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void RemoveTreePrototype(int index);

		[NativeName("GetTreeDatabase().RecalculateTreePositions")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void RecalculateTreePositions();

		[NativeName("GetDetailDatabase().RemoveDetailPrototype")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void RemoveDetailPrototype(int index);

		[NativeName("GetTreeDatabase().NeedUpgradeScaledPrototypes")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool NeedUpgradeScaledTreePrototypes();

		[FreeFunction("TerrainDataScriptingInterface::UpgradeScaledTreePrototype", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void UpgradeScaledTreePrototype();

		public extern int alphamapLayers { [NativeName("GetSplatDatabase().GetDepth")] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public float[,,] GetAlphamaps(int x, int y, int width, int height)
		{
			if (x < 0 || y < 0 || width < 0 || height < 0)
			{
				throw new ArgumentException("Invalid argument for GetAlphaMaps");
			}
			return this.Internal_GetAlphamaps(x, y, width, height);
		}

		[FreeFunction("TerrainDataScriptingInterface::GetAlphamaps", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern float[,,] Internal_GetAlphamaps(int x, int y, int width, int height);

		public int alphamapResolution
		{
			get
			{
				return this.Internal_alphamapResolution;
			}
			set
			{
				int internal_alphamapResolution = value;
				if (value < TerrainData.k_MinimumAlphamapResolution || value > TerrainData.k_MaximumAlphamapResolution)
				{
					Debug.LogWarning(string.Concat(new object[]
					{
						"alphamapResolution is clamped to the range of [",
						TerrainData.k_MinimumAlphamapResolution,
						", ",
						TerrainData.k_MaximumAlphamapResolution,
						"]."
					}));
					internal_alphamapResolution = Math.Min(TerrainData.k_MaximumAlphamapResolution, Math.Max(value, TerrainData.k_MinimumAlphamapResolution));
				}
				this.Internal_alphamapResolution = internal_alphamapResolution;
			}
		}

		[RequiredByNativeCode]
		[NativeName("GetSplatDatabase().GetAlphamapResolution")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern float GetAlphamapResolutionInternal();

		private extern int Internal_alphamapResolution { [NativeName("GetSplatDatabase().GetAlphamapResolution")] [MethodImpl(MethodImplOptions.InternalCall)] get; [NativeName("GetSplatDatabase().SetAlphamapResolution")] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public int alphamapWidth
		{
			get
			{
				return this.alphamapResolution;
			}
		}

		public int alphamapHeight
		{
			get
			{
				return this.alphamapResolution;
			}
		}

		public int baseMapResolution
		{
			get
			{
				return this.Internal_baseMapResolution;
			}
			set
			{
				int internal_baseMapResolution = value;
				if (value < TerrainData.k_MinimumBaseMapResolution || value > TerrainData.k_MaximumBaseMapResolution)
				{
					Debug.LogWarning(string.Concat(new object[]
					{
						"baseMapResolution is clamped to the range of [",
						TerrainData.k_MinimumBaseMapResolution,
						", ",
						TerrainData.k_MaximumBaseMapResolution,
						"]."
					}));
					internal_baseMapResolution = Math.Min(TerrainData.k_MaximumBaseMapResolution, Math.Max(value, TerrainData.k_MinimumBaseMapResolution));
				}
				this.Internal_baseMapResolution = internal_baseMapResolution;
			}
		}

		private extern int Internal_baseMapResolution { [NativeName("GetSplatDatabase().GetBaseMapResolution")] [MethodImpl(MethodImplOptions.InternalCall)] get; [NativeName("GetSplatDatabase().SetBaseMapResolution")] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public void SetAlphamaps(int x, int y, float[,,] map)
		{
			if (map.GetLength(2) != this.alphamapLayers)
			{
				throw new Exception(UnityString.Format("Float array size wrong (layers should be {0})", new object[]
				{
					this.alphamapLayers
				}));
			}
			this.Internal_SetAlphamaps(x, y, map.GetLength(1), map.GetLength(0), map);
		}

		[FreeFunction("TerrainDataScriptingInterface::SetAlphamaps", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetAlphamaps(int x, int y, int width, int height, float[,,] map);

		[NativeName("GetSplatDatabase().RecalculateBasemapIfDirty")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void RecalculateBasemapIfDirty();

		[NativeName("GetSplatDatabase().SetBasemapDirty")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetBasemapDirty(bool dirty);

		[NativeName("GetSplatDatabase().GetAlphaTexture")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Texture2D GetAlphamapTexture(int index);

		private extern int alphamapTextureCount { [NativeName("GetSplatDatabase().GetAlphaTextureCount")] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public Texture2D[] alphamapTextures
		{
			get
			{
				Texture2D[] array = new Texture2D[this.alphamapTextureCount];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this.GetAlphamapTexture(i);
				}
				return array;
			}
		}

		public extern SplatPrototype[] splatPrototypes { [FreeFunction("TerrainDataScriptingInterface::GetSplatPrototypes", HasExplicitThis = true)] [MethodImpl(MethodImplOptions.InternalCall)] get; [FreeFunction("TerrainDataScriptingInterface::SetSplatPrototypes", HasExplicitThis = true)] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[NativeName("GetTreeDatabase().AddTree")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void AddTree(ref TreeInstance tree);

		[NativeName("GetTreeDatabase().RemoveTrees")]
		internal int RemoveTrees(Vector2 position, float radius, int prototypeIndex)
		{
			return this.RemoveTrees_Injected(ref position, radius, prototypeIndex);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_heightmapScale_Injected(out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_size_Injected(out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_size_Injected(ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_bounds_Injected(out Bounds ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetInterpolatedNormal_Injected(float x, float y, out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_wavingGrassTint_Injected(out Color ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_wavingGrassTint_Injected(ref Color value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_GetTreeInstance_Injected(int index, out TreeInstance ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetTreeInstance_Injected(int index, ref TreeInstance instance);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int RemoveTrees_Injected(ref Vector2 position, float radius, int prototypeIndex);

		private enum BoundaryValueType
		{
			MaxHeightmapRes,
			MinDetailResPerPatch,
			MaxDetailResPerPatch,
			MaxDetailPatchCount,
			MinAlphamapRes,
			MaxAlphamapRes,
			MinBaseMapRes,
			MaxBaseMapRes
		}
	}
}
