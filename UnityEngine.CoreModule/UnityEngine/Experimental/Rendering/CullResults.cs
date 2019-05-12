using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Rendering
{
	[UsedByNativeCode]
	public struct CullResults
	{
		public List<VisibleLight> visibleLights;

		public List<VisibleLight> visibleOffscreenVertexLights;

		public List<VisibleReflectionProbe> visibleReflectionProbes;

		public FilterResults visibleRenderers;

		internal IntPtr cullResults;

		private void Init()
		{
			this.visibleLights = new List<VisibleLight>();
			this.visibleOffscreenVertexLights = new List<VisibleLight>();
			this.visibleReflectionProbes = new List<VisibleReflectionProbe>();
			this.visibleRenderers = default(FilterResults);
			this.cullResults = IntPtr.Zero;
		}

		public static bool GetCullingParameters(Camera camera, out ScriptableCullingParameters cullingParameters)
		{
			return CullResults.GetCullingParameters_Internal(camera, false, out cullingParameters, sizeof(ScriptableCullingParameters));
		}

		public static bool GetCullingParameters(Camera camera, bool stereoAware, out ScriptableCullingParameters cullingParameters)
		{
			return CullResults.GetCullingParameters_Internal(camera, stereoAware, out cullingParameters, sizeof(ScriptableCullingParameters));
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetCullingParameters_Internal(Camera camera, bool stereoAware, out ScriptableCullingParameters cullingParameters, int managedCullingParametersSize);

		internal static void Internal_Cull(ref ScriptableCullingParameters parameters, ScriptableRenderContext renderLoop, ref CullResults results)
		{
			CullResults.INTERNAL_CALL_Internal_Cull(ref parameters, ref renderLoop, ref results);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Internal_Cull(ref ScriptableCullingParameters parameters, ref ScriptableRenderContext renderLoop, ref CullResults results);

		public static CullResults Cull(ref ScriptableCullingParameters parameters, ScriptableRenderContext renderLoop)
		{
			CullResults result = default(CullResults);
			CullResults.Cull(ref parameters, renderLoop, ref result);
			return result;
		}

		public static void Cull(ref ScriptableCullingParameters parameters, ScriptableRenderContext renderLoop, ref CullResults results)
		{
			if (results.visibleLights == null || results.visibleOffscreenVertexLights == null || results.visibleReflectionProbes == null)
			{
				results.Init();
			}
			CullResults.Internal_Cull(ref parameters, renderLoop, ref results);
		}

		public static bool Cull(Camera camera, ScriptableRenderContext renderLoop, out CullResults results)
		{
			results.cullResults = IntPtr.Zero;
			results.visibleLights = null;
			results.visibleOffscreenVertexLights = null;
			results.visibleReflectionProbes = null;
			results.visibleRenderers = default(FilterResults);
			ScriptableCullingParameters scriptableCullingParameters;
			bool result;
			if (!CullResults.GetCullingParameters(camera, out scriptableCullingParameters))
			{
				result = false;
			}
			else
			{
				results = CullResults.Cull(ref scriptableCullingParameters, renderLoop);
				result = true;
			}
			return result;
		}

		public bool GetShadowCasterBounds(int lightIndex, out Bounds outBounds)
		{
			return CullResults.GetShadowCasterBounds(this.cullResults, lightIndex, out outBounds);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetShadowCasterBounds(IntPtr cullResults, int lightIndex, out Bounds bounds);

		public int GetLightIndicesCount()
		{
			return CullResults.GetLightIndicesCount(this.cullResults);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int GetLightIndicesCount(IntPtr cullResults);

		public void FillLightIndices(ComputeBuffer computeBuffer)
		{
			CullResults.FillLightIndices(this.cullResults, computeBuffer);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void FillLightIndices(IntPtr cullResults, ComputeBuffer computeBuffer);

		public int[] GetLightIndexMap()
		{
			return CullResults.GetLightIndexMap(this.cullResults);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int[] GetLightIndexMap(IntPtr cullResults);

		public void SetLightIndexMap(int[] mapping)
		{
			CullResults.SetLightIndexMap(this.cullResults, mapping);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetLightIndexMap(IntPtr cullResults, int[] mapping);

		public bool ComputeSpotShadowMatricesAndCullingPrimitives(int activeLightIndex, out Matrix4x4 viewMatrix, out Matrix4x4 projMatrix, out ShadowSplitData shadowSplitData)
		{
			return CullResults.ComputeSpotShadowMatricesAndCullingPrimitives(this.cullResults, activeLightIndex, out viewMatrix, out projMatrix, out shadowSplitData);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool ComputeSpotShadowMatricesAndCullingPrimitives(IntPtr cullResults, int activeLightIndex, out Matrix4x4 viewMatrix, out Matrix4x4 projMatrix, out ShadowSplitData shadowSplitData);

		public bool ComputePointShadowMatricesAndCullingPrimitives(int activeLightIndex, CubemapFace cubemapFace, float fovBias, out Matrix4x4 viewMatrix, out Matrix4x4 projMatrix, out ShadowSplitData shadowSplitData)
		{
			return CullResults.ComputePointShadowMatricesAndCullingPrimitives(this.cullResults, activeLightIndex, cubemapFace, fovBias, out viewMatrix, out projMatrix, out shadowSplitData);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool ComputePointShadowMatricesAndCullingPrimitives(IntPtr cullResults, int activeLightIndex, CubemapFace cubemapFace, float fovBias, out Matrix4x4 viewMatrix, out Matrix4x4 projMatrix, out ShadowSplitData shadowSplitData);

		public bool ComputeDirectionalShadowMatricesAndCullingPrimitives(int activeLightIndex, int splitIndex, int splitCount, Vector3 splitRatio, int shadowResolution, float shadowNearPlaneOffset, out Matrix4x4 viewMatrix, out Matrix4x4 projMatrix, out ShadowSplitData shadowSplitData)
		{
			return CullResults.ComputeDirectionalShadowMatricesAndCullingPrimitives(this.cullResults, activeLightIndex, splitIndex, splitCount, splitRatio, shadowResolution, shadowNearPlaneOffset, out viewMatrix, out projMatrix, out shadowSplitData);
		}

		private static bool ComputeDirectionalShadowMatricesAndCullingPrimitives(IntPtr cullResults, int activeLightIndex, int splitIndex, int splitCount, Vector3 splitRatio, int shadowResolution, float shadowNearPlaneOffset, out Matrix4x4 viewMatrix, out Matrix4x4 projMatrix, out ShadowSplitData shadowSplitData)
		{
			return CullResults.INTERNAL_CALL_ComputeDirectionalShadowMatricesAndCullingPrimitives(cullResults, activeLightIndex, splitIndex, splitCount, ref splitRatio, shadowResolution, shadowNearPlaneOffset, out viewMatrix, out projMatrix, out shadowSplitData);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool INTERNAL_CALL_ComputeDirectionalShadowMatricesAndCullingPrimitives(IntPtr cullResults, int activeLightIndex, int splitIndex, int splitCount, ref Vector3 splitRatio, int shadowResolution, float shadowNearPlaneOffset, out Matrix4x4 viewMatrix, out Matrix4x4 projMatrix, out ShadowSplitData shadowSplitData);
	}
}
