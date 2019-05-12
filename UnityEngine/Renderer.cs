using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;

namespace UnityEngine
{
	/// <summary>
	///   <para>General functionality for all renderers.</para>
	/// </summary>
	public class Renderer : Component
	{
		internal extern Transform staticBatchRootTransform { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		internal extern int staticBatchIndex { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetSubsetIndex(int index, int subSetIndexForMaterial);

		/// <summary>
		///   <para>Has this renderer been statically batched with any other renderers?</para>
		/// </summary>
		public extern bool isPartOfStaticBatch { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Matrix that transforms a point from world space into local space (Read Only).</para>
		/// </summary>
		public Matrix4x4 worldToLocalMatrix
		{
			get
			{
				Matrix4x4 result;
				this.INTERNAL_get_worldToLocalMatrix(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_worldToLocalMatrix(out Matrix4x4 value);

		/// <summary>
		///   <para>Matrix that transforms a point from local space into world space (Read Only).</para>
		/// </summary>
		public Matrix4x4 localToWorldMatrix
		{
			get
			{
				Matrix4x4 result;
				this.INTERNAL_get_localToWorldMatrix(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_localToWorldMatrix(out Matrix4x4 value);

		/// <summary>
		///   <para>Makes the rendered 3D object visible if enabled.</para>
		/// </summary>
		public extern bool enabled { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Does this object cast shadows?</para>
		/// </summary>
		public extern ShadowCastingMode shadowCastingMode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[Obsolete("Property castShadows has been deprecated. Use shadowCastingMode instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public extern bool castShadows { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Does this object receive shadows?</para>
		/// </summary>
		public extern bool receiveShadows { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Returns the first instantiated Material assigned to the renderer.</para>
		/// </summary>
		public extern Material material { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The shared material of this object.</para>
		/// </summary>
		public extern Material sharedMaterial { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Returns all the instantiated materials of this object.</para>
		/// </summary>
		public extern Material[] materials { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>All the shared materials of this object.</para>
		/// </summary>
		public extern Material[] sharedMaterials { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The bounding volume of the renderer (Read Only).</para>
		/// </summary>
		public Bounds bounds
		{
			get
			{
				Bounds result;
				this.INTERNAL_get_bounds(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_bounds(out Bounds value);

		/// <summary>
		///   <para>The index of the baked lightmap applied to this renderer.</para>
		/// </summary>
		public extern int lightmapIndex { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The index of the realtime lightmap applied to this renderer.</para>
		/// </summary>
		public extern int realtimeLightmapIndex { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The UV scale &amp; offset used for a lightmap.</para>
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
		///   <para>Is this renderer visible in any camera? (Read Only)</para>
		/// </summary>
		public extern bool isVisible { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Should light probes be used for this Renderer?</para>
		/// </summary>
		public extern bool useLightProbes { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>If set, Renderer will use this Transform's position to find the light or reflection probe.</para>
		/// </summary>
		public extern Transform probeAnchor { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Should reflection probes be used for this Renderer?</para>
		/// </summary>
		public extern ReflectionProbeUsage reflectionProbeUsage { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Lets you add per-renderer material parameters without duplicating a material.</para>
		/// </summary>
		/// <param name="properties"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetPropertyBlock(MaterialPropertyBlock properties);

		/// <summary>
		///   <para>Get per-renderer material property block.</para>
		/// </summary>
		/// <param name="dest"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void GetPropertyBlock(MaterialPropertyBlock dest);

		/// <summary>
		///   <para>Name of the Renderer's sorting layer.</para>
		/// </summary>
		public extern string sortingLayerName { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Unique ID of the Renderer's sorting layer.</para>
		/// </summary>
		public extern int sortingLayerID { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Renderer's order within a sorting layer.</para>
		/// </summary>
		public extern int sortingOrder { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetClosestReflectionProbesInternal(object result);

		public void GetClosestReflectionProbes(List<ReflectionProbeBlendInfo> result)
		{
			this.GetClosestReflectionProbesInternal(result);
		}
	}
}
