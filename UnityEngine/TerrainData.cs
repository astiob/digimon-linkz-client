using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>The TerrainData class stores heightmaps, detail mesh positions, tree instances, and terrain texture alpha maps.</para>
	/// </summary>
	public sealed class TerrainData : Object
	{
		public TerrainData()
		{
			this.Internal_Create(this);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void Internal_Create([Writable] TerrainData terrainData);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool HasUser(GameObject user);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void AddUser(GameObject user);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void RemoveUser(GameObject user);

		/// <summary>
		///   <para>Width of the terrain in samples (Read Only).</para>
		/// </summary>
		public extern int heightmapWidth { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Height of the terrain in samples (Read Only).</para>
		/// </summary>
		public extern int heightmapHeight { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Resolution of the heightmap.</para>
		/// </summary>
		public extern int heightmapResolution { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The size of each heightmap sample.</para>
		/// </summary>
		public Vector3 heightmapScale
		{
			get
			{
				Vector3 result;
				this.INTERNAL_get_heightmapScale(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_heightmapScale(out Vector3 value);

		/// <summary>
		///   <para>The total size in world units of the terrain.</para>
		/// </summary>
		public Vector3 size
		{
			get
			{
				Vector3 result;
				this.INTERNAL_get_size(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_size(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_size(out Vector3 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_size(ref Vector3 value);

		/// <summary>
		///   <para>The thickness of the terrain used for collision detection.</para>
		/// </summary>
		public extern float thickness { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Gets the height at a certain point x,y.</para>
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float GetHeight(int x, int y);

		/// <summary>
		///   <para>Gets an interpolated height at a point x,y.</para>
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float GetInterpolatedHeight(float x, float y);

		/// <summary>
		///   <para>Get an array of heightmap samples.</para>
		/// </summary>
		/// <param name="xBase">First x index of heightmap samples to retrieve.</param>
		/// <param name="yBase">First y index of heightmap samples to retrieve.</param>
		/// <param name="width">Number of samples to retrieve along the heightmap's x axis.</param>
		/// <param name="height">Number of samples to retrieve along the heightmap's y axis.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float[,] GetHeights(int xBase, int yBase, int width, int height);

		/// <summary>
		///   <para>Set an array of heightmap samples.</para>
		/// </summary>
		/// <param name="xBase">First x index of heightmap samples to set.</param>
		/// <param name="yBase">First y index of heightmap samples to set.</param>
		/// <param name="heights">Array of heightmap samples to set (values range from 0 to 1, array indexed as [y,x]).</param>
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

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetHeights(int xBase, int yBase, int width, int height, float[,] heights);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetHeightsDelayLOD(int xBase, int yBase, int width, int height, float[,] heights);

		/// <summary>
		///   <para>Set an array of heightmap samples.</para>
		/// </summary>
		/// <param name="xBase">First x index of heightmap samples to set.</param>
		/// <param name="yBase">First y index of heightmap samples to set.</param>
		/// <param name="heights">Array of heightmap samples to set (values range from 0 to 1, array indexed as [y,x]).</param>
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

		/// <summary>
		///   <para>Gets the gradient of the terrain at point &amp;amp;amp;amp;lt;x,y&amp;amp;amp;amp;gt;.</para>
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float GetSteepness(float x, float y);

		/// <summary>
		///   <para>Get an interpolated normal at a given location.</para>
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Vector3 GetInterpolatedNormal(float x, float y);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int GetAdjustedSize(int size);

		/// <summary>
		///   <para>Strength of the waving grass in the terrain.</para>
		/// </summary>
		public extern float wavingGrassStrength { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Amount of waving grass in the terrain.</para>
		/// </summary>
		public extern float wavingGrassAmount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Speed of the waving grass.</para>
		/// </summary>
		public extern float wavingGrassSpeed { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Color of the waving grass that the terrain has.</para>
		/// </summary>
		public Color wavingGrassTint
		{
			get
			{
				Color result;
				this.INTERNAL_get_wavingGrassTint(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_wavingGrassTint(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_wavingGrassTint(out Color value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_wavingGrassTint(ref Color value);

		/// <summary>
		///   <para>Detail width of the TerrainData.</para>
		/// </summary>
		public extern int detailWidth { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Detail height of the TerrainData.</para>
		/// </summary>
		public extern int detailHeight { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Set the resolution of the detail map.</para>
		/// </summary>
		/// <param name="detailResolution">Specifies the number of pixels in the detail resolution map. A larger detailResolution, leads to more accurate detail object painting.</param>
		/// <param name="resolutionPerPatch">Specifies the size in pixels of each individually rendered detail patch. A larger number reduces draw calls, but might increase triangle count since detail patches are culled on a per batch basis. A recommended value is 16. If you use a very large detail object distance and your grass is very sparse, it makes sense to increase the value.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetDetailResolution(int detailResolution, int resolutionPerPatch);

		/// <summary>
		///   <para>Detail Resolution of the TerrainData.</para>
		/// </summary>
		public extern int detailResolution { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		internal extern int detailResolutionPerPatch { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void ResetDirtyDetails();

		/// <summary>
		///   <para>Reloads all the values of the available prototypes (ie, detail mesh assets) in the TerrainData Object.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RefreshPrototypes();

		/// <summary>
		///   <para>Contains the detail texture/meshes that the terrain has.</para>
		/// </summary>
		public extern DetailPrototype[] detailPrototypes { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Returns an array of all supported detail layer indices in the area.</para>
		/// </summary>
		/// <param name="xBase"></param>
		/// <param name="yBase"></param>
		/// <param name="totalWidth"></param>
		/// <param name="totalHeight"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int[] GetSupportedLayers(int xBase, int yBase, int totalWidth, int totalHeight);

		/// <summary>
		///   <para>Returns a 2D array of the detail object density in the specific location.</para>
		/// </summary>
		/// <param name="xBase"></param>
		/// <param name="yBase"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="layer"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int[,] GetDetailLayer(int xBase, int yBase, int width, int height, int layer);

		/// <summary>
		///   <para>Sets the detail layer density map.</para>
		/// </summary>
		/// <param name="xBase"></param>
		/// <param name="yBase"></param>
		/// <param name="layer"></param>
		/// <param name="details"></param>
		public void SetDetailLayer(int xBase, int yBase, int layer, int[,] details)
		{
			this.Internal_SetDetailLayer(xBase, yBase, details.GetLength(1), details.GetLength(0), layer, details);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetDetailLayer(int xBase, int yBase, int totalWidth, int totalHeight, int detailIndex, int[,] data);

		/// <summary>
		///   <para>Contains the current trees placed in the terrain.</para>
		/// </summary>
		public extern TreeInstance[] treeInstances { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Get the tree instance at the specified index. It is used as a faster version of treeInstances[index] as this function doesn't create the entire tree instances array.</para>
		/// </summary>
		/// <param name="index">The index of the tree instance.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern TreeInstance GetTreeInstance(int index);

		/// <summary>
		///   <para>Set the tree instance with new parameters at the specified index. However, TreeInstance.prototypeIndex and TreeInstance.position can not be changed otherwise an ArgumentException will be thrown.</para>
		/// </summary>
		/// <param name="index">The index of the tree instance.</param>
		/// <param name="instance">The new TreeInstance value.</param>
		public void SetTreeInstance(int index, TreeInstance instance)
		{
			TerrainData.INTERNAL_CALL_SetTreeInstance(this, index, ref instance);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_SetTreeInstance(TerrainData self, int index, ref TreeInstance instance);

		/// <summary>
		///   <para>Returns the number of tree instances.</para>
		/// </summary>
		public extern int treeInstanceCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The list of tree prototypes this are the ones available in the inspector.</para>
		/// </summary>
		public extern TreePrototype[] treePrototypes { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void RemoveTreePrototype(int index);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void RecalculateTreePositions();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void RemoveDetailPrototype(int index);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool NeedUpgradeScaledTreePrototypes();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void UpgradeScaledTreePrototype();

		/// <summary>
		///   <para>Number of alpha map layers.</para>
		/// </summary>
		public extern int alphamapLayers { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns the alpha map at a position x, y given a width and height.</para>
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float[,,] GetAlphamaps(int x, int y, int width, int height);

		/// <summary>
		///   <para>Resolution of the alpha map.</para>
		/// </summary>
		public extern int alphamapResolution { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Width of the alpha map.</para>
		/// </summary>
		public extern int alphamapWidth { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Height of the alpha map.</para>
		/// </summary>
		public extern int alphamapHeight { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Resolution of the base map used for rendering far patches on the terrain.</para>
		/// </summary>
		public extern int baseMapResolution { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Assign all splat values in the given map area.</para>
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="map"></param>
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

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_SetAlphamaps(int x, int y, int width, int height, float[,,] map);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void RecalculateBasemapIfDirty();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetBasemapDirty(bool dirty);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Texture2D GetAlphamapTexture(int index);

		private extern int alphamapTextureCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Alpha map textures used by the Terrain. Used by Terrain Inspector for undo.</para>
		/// </summary>
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

		/// <summary>
		///   <para>Splat texture used by the terrain.</para>
		/// </summary>
		public extern SplatPrototype[] splatPrototypes { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void AddTree(out TreeInstance tree);

		internal int RemoveTrees(Vector2 position, float radius, int prototypeIndex)
		{
			return TerrainData.INTERNAL_CALL_RemoveTrees(this, ref position, radius, prototypeIndex);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int INTERNAL_CALL_RemoveTrees(TerrainData self, ref Vector2 position, float radius, int prototypeIndex);
	}
}
