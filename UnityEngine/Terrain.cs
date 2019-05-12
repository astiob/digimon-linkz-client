using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;

namespace UnityEngine
{
	/// <summary>
	///   <para>The Terrain component renders the terrain.</para>
	/// </summary>
	public sealed class Terrain : Behaviour
	{
		public extern TerrainRenderFlags editorRenderFlags { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The Terrain Data that stores heightmaps, terrain textures, detail meshes and trees.</para>
		/// </summary>
		public extern TerrainData terrainData { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The maximum distance at which trees are rendered.</para>
		/// </summary>
		public extern float treeDistance { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Distance from the camera where trees will be rendered as billboards only.</para>
		/// </summary>
		public extern float treeBillboardDistance { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Total distance delta that trees will use to transition from billboard orientation to mesh orientation.</para>
		/// </summary>
		public extern float treeCrossFadeLength { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Maximum number of trees rendered at full LOD.</para>
		/// </summary>
		public extern int treeMaximumFullLODCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Detail objects will be displayed up to this distance.</para>
		/// </summary>
		public extern float detailObjectDistance { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Density of detail objects.</para>
		/// </summary>
		public extern float detailObjectDensity { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Collect Detail patches from memory.</para>
		/// </summary>
		public extern bool collectDetailPatches { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>An approximation of how many pixels the terrain will pop in the worst case when switching lod.</para>
		/// </summary>
		public extern float heightmapPixelError { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Lets you essentially lower the heightmap resolution used for rendering.</para>
		/// </summary>
		public extern int heightmapMaximumLOD { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Heightmap patches beyond basemap distance will use a precomputed low res basemap.</para>
		/// </summary>
		public extern float basemapDistance { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[Obsolete("use basemapDistance", true)]
		public float splatmapDistance
		{
			get
			{
				return this.basemapDistance;
			}
			set
			{
				this.basemapDistance = value;
			}
		}

		/// <summary>
		///   <para>The index of the baked lightmap applied to this terrain.</para>
		/// </summary>
		public extern int lightmapIndex { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The index of the realtime lightmap applied to this terrain.</para>
		/// </summary>
		public extern int realtimeLightmapIndex { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The UV scale &amp; offset used for a baked lightmap.</para>
		/// </summary>
		public Vector4 lightmapScaleOffset
		{
			get
			{
				Vector4 result;
				this.INTERNAL_get_lightmapScaleOffset(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_lightmapScaleOffset(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_lightmapScaleOffset(out Vector4 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_lightmapScaleOffset(ref Vector4 value);

		/// <summary>
		///   <para>The UV scale &amp; offset used for a realtime lightmap.</para>
		/// </summary>
		public Vector4 realtimeLightmapScaleOffset
		{
			get
			{
				Vector4 result;
				this.INTERNAL_get_realtimeLightmapScaleOffset(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_realtimeLightmapScaleOffset(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_realtimeLightmapScaleOffset(out Vector4 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_realtimeLightmapScaleOffset(ref Vector4 value);

		/// <summary>
		///   <para>Should terrain cast shadows?.</para>
		/// </summary>
		public extern bool castShadows { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>How reflection probes are used for terrain. See Rendering.ReflectionProbeUsage.</para>
		/// </summary>
		public extern ReflectionProbeUsage reflectionProbeUsage { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetClosestReflectionProbesInternal(object result);

		public void GetClosestReflectionProbes(List<ReflectionProbeBlendInfo> result)
		{
			this.GetClosestReflectionProbesInternal(result);
		}

		/// <summary>
		///   <para>The type of the material used to render the terrain. Could be one of the built-in types or custom. See Terrain.MaterialType.</para>
		/// </summary>
		public extern Terrain.MaterialType materialType { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The custom material used to render the terrain.</para>
		/// </summary>
		public extern Material materialTemplate { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The specular color of the terrain.</para>
		/// </summary>
		public Color legacySpecular
		{
			get
			{
				Color result;
				this.INTERNAL_get_legacySpecular(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_legacySpecular(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_legacySpecular(out Color value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_legacySpecular(ref Color value);

		/// <summary>
		///   <para>The shininess value of the terrain.</para>
		/// </summary>
		public extern float legacyShininess { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Specify if terrain heightmap should be drawn.</para>
		/// </summary>
		public extern bool drawHeightmap { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Specify if terrain trees and details should be drawn.</para>
		/// </summary>
		public extern bool drawTreesAndFoliage { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Samples the height at the given position defined in world space, relative to the terrain space.</para>
		/// </summary>
		/// <param name="worldPosition"></param>
		public float SampleHeight(Vector3 worldPosition)
		{
			return Terrain.INTERNAL_CALL_SampleHeight(this, ref worldPosition);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float INTERNAL_CALL_SampleHeight(Terrain self, ref Vector3 worldPosition);

		/// <summary>
		///   <para>Update the terrain's LOD and vegetation information after making changes with TerrainData.SetHeightsDelayLOD.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ApplyDelayedHeightmapModification();

		/// <summary>
		///   <para>Adds a tree instance to the terrain.</para>
		/// </summary>
		/// <param name="instance"></param>
		public void AddTreeInstance(TreeInstance instance)
		{
			Terrain.INTERNAL_CALL_AddTreeInstance(this, ref instance);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_AddTreeInstance(Terrain self, ref TreeInstance instance);

		/// <summary>
		///   <para>Lets you setup the connection between neighboring Terrains.</para>
		/// </summary>
		/// <param name="left"></param>
		/// <param name="top"></param>
		/// <param name="right"></param>
		/// <param name="bottom"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetNeighbors(Terrain left, Terrain top, Terrain right, Terrain bottom);

		/// <summary>
		///   <para>Get the position of the terrain.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Vector3 GetPosition();

		/// <summary>
		///   <para>Flushes any change done in the terrain so it takes effect.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Flush();

		internal void RemoveTrees(Vector2 position, float radius, int prototypeIndex)
		{
			Terrain.INTERNAL_CALL_RemoveTrees(this, ref position, radius, prototypeIndex);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_RemoveTrees(Terrain self, ref Vector2 position, float radius, int prototypeIndex);

		/// <summary>
		///   <para>The active terrain. This is a convenience function to get to the main terrain in the scene.</para>
		/// </summary>
		public static extern Terrain activeTerrain { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The active terrains in the scene.</para>
		/// </summary>
		public static extern Terrain[] activeTerrains { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Creates a Terrain including collider from TerrainData.</para>
		/// </summary>
		/// <param name="assignTerrain"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern GameObject CreateTerrainGameObject(TerrainData assignTerrain);

		/// <summary>
		///   <para>The type of the material used to render a terrain object. Could be one of the built-in types or custom.</para>
		/// </summary>
		public enum MaterialType
		{
			/// <summary>
			///   <para>A built-in material that uses the standard physically-based lighting model. Inputs supported: smoothness, metallic / specular, normal.</para>
			/// </summary>
			BuiltInStandard,
			/// <summary>
			///   <para>A built-in material that uses the legacy Lambert (diffuse) lighting model and has optional normal map support.</para>
			/// </summary>
			BuiltInLegacyDiffuse,
			/// <summary>
			///   <para>A built-in material that uses the legacy BlinnPhong (specular) lighting model and has optional normal map support.</para>
			/// </summary>
			BuiltInLegacySpecular,
			/// <summary>
			///   <para>Use a custom material given by Terrain.materialTemplate.</para>
			/// </summary>
			Custom
		}
	}
}
