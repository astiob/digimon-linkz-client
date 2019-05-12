using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[NativeHeader("TerrainScriptingClasses.h")]
	[NativeHeader("Runtime/Interfaces/ITerrainManager.h")]
	[NativeHeader("Modules/Terrain/Public/Terrain.h")]
	[UsedByNativeCode]
	[StaticAccessor("GetITerrainManager()", StaticAccessorType.Arrow)]
	public sealed class Terrain : Behaviour
	{
		public extern TerrainData terrainData { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern float treeDistance { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern float treeBillboardDistance { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern float treeCrossFadeLength { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern int treeMaximumFullLODCount { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern float detailObjectDistance { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern float detailObjectDensity { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern float heightmapPixelError { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern int heightmapMaximumLOD { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern float basemapDistance { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[Obsolete("splatmapDistance is deprecated, please use basemapDistance instead. (UnityUpgradable) -> basemapDistance", true)]
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

		[NativeProperty("StaticLightmapIndexInt")]
		public extern int lightmapIndex { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[NativeProperty("DynamicLightmapIndexInt")]
		public extern int realtimeLightmapIndex { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[NativeProperty("StaticLightmapST")]
		public Vector4 lightmapScaleOffset
		{
			get
			{
				Vector4 result;
				this.get_lightmapScaleOffset_Injected(out result);
				return result;
			}
			set
			{
				this.set_lightmapScaleOffset_Injected(ref value);
			}
		}

		[NativeProperty("DynamicLightmapST")]
		public Vector4 realtimeLightmapScaleOffset
		{
			get
			{
				Vector4 result;
				this.get_realtimeLightmapScaleOffset_Injected(out result);
				return result;
			}
			set
			{
				this.set_realtimeLightmapScaleOffset_Injected(ref value);
			}
		}

		[NativeProperty("GarbageCollectRenderers")]
		public extern bool freeUnusedRenderingResources { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern bool castShadows { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern ReflectionProbeUsage reflectionProbeUsage { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void GetClosestReflectionProbes(List<ReflectionProbeBlendInfo> result);

		public extern Terrain.MaterialType materialType { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern Material materialTemplate { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public Color legacySpecular
		{
			get
			{
				Color result;
				this.get_legacySpecular_Injected(out result);
				return result;
			}
			set
			{
				this.set_legacySpecular_Injected(ref value);
			}
		}

		public extern float legacyShininess { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern bool drawHeightmap { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern bool drawTreesAndFoliage { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public Vector3 patchBoundsMultiplier
		{
			get
			{
				Vector3 result;
				this.get_patchBoundsMultiplier_Injected(out result);
				return result;
			}
			set
			{
				this.set_patchBoundsMultiplier_Injected(ref value);
			}
		}

		public float SampleHeight(Vector3 worldPosition)
		{
			return this.SampleHeight_Injected(ref worldPosition);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ApplyDelayedHeightmapModification();

		public void AddTreeInstance(TreeInstance instance)
		{
			this.AddTreeInstance_Injected(ref instance);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetNeighbors(Terrain left, Terrain top, Terrain right, Terrain bottom);

		public extern float treeLODBiasMultiplier { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern bool collectDetailPatches { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern TerrainRenderFlags editorRenderFlags { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public Vector3 GetPosition()
		{
			Vector3 result;
			this.GetPosition_Injected(out result);
			return result;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Flush();

		internal void RemoveTrees(Vector2 position, float radius, int prototypeIndex)
		{
			this.RemoveTrees_Injected(ref position, radius, prototypeIndex);
		}

		[NativeMethod("CopySplatMaterialCustomProps")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetSplatMaterialPropertyBlock(MaterialPropertyBlock properties);

		public void GetSplatMaterialPropertyBlock(MaterialPropertyBlock dest)
		{
			if (dest == null)
			{
				throw new ArgumentNullException("dest");
			}
			this.Internal_GetSplatMaterialPropertyBlock(dest);
		}

		[NativeMethod("GetSplatMaterialCustomProps")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_GetSplatMaterialPropertyBlock(MaterialPropertyBlock dest);

		public static extern Terrain activeTerrain { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[NativeProperty("ActiveTerrainsScriptingArray")]
		public static extern Terrain[] activeTerrains { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[UsedByNativeCode]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern GameObject CreateTerrainGameObject(TerrainData assignTerrain);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_lightmapScaleOffset_Injected(out Vector4 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_lightmapScaleOffset_Injected(ref Vector4 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_realtimeLightmapScaleOffset_Injected(out Vector4 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_realtimeLightmapScaleOffset_Injected(ref Vector4 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_legacySpecular_Injected(out Color ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_legacySpecular_Injected(ref Color value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_patchBoundsMultiplier_Injected(out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_patchBoundsMultiplier_Injected(ref Vector3 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern float SampleHeight_Injected(ref Vector3 worldPosition);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AddTreeInstance_Injected(ref TreeInstance instance);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetPosition_Injected(out Vector3 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void RemoveTrees_Injected(ref Vector2 position, float radius, int prototypeIndex);

		public enum MaterialType
		{
			BuiltInStandard,
			BuiltInLegacyDiffuse,
			BuiltInLegacySpecular,
			Custom
		}
	}
}
