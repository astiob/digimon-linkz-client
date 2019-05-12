using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Rendering;

namespace UnityEngine
{
	/// <summary>
	///   <para>The reflection probe is used to capture the surroundings into a texture which is passed to the shaders and used for reflections.</para>
	/// </summary>
	public sealed class ReflectionProbe : Behaviour
	{
		/// <summary>
		///   <para>Reflection probe type.</para>
		/// </summary>
		public extern ReflectionProbeType type { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Should this reflection probe use HDR rendering?</para>
		/// </summary>
		public extern bool hdr { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The size of the box area in which reflections will be applied to the objects. Measured in the probes's local space.</para>
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
		///   <para>The center of the box area in which reflections will be applied to the objects. Measured in the probes's local space.</para>
		/// </summary>
		public Vector3 center
		{
			get
			{
				Vector3 result;
				this.INTERNAL_get_center(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_center(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_center(out Vector3 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_center(ref Vector3 value);

		/// <summary>
		///   <para>The near clipping plane distance when rendering the probe.</para>
		/// </summary>
		public extern float nearClipPlane { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The far clipping plane distance when rendering the probe.</para>
		/// </summary>
		public extern float farClipPlane { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Shadow drawing distance when rendering the probe.</para>
		/// </summary>
		public extern float shadowDistance { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Resolution of the underlying reflection texture in pixels.</para>
		/// </summary>
		public extern int resolution { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>This is used to render parts of the reflecion probe's surrounding selectively.</para>
		/// </summary>
		public extern int cullingMask { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>How the reflection probe clears the background.</para>
		/// </summary>
		public extern ReflectionProbeClearFlags clearFlags { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The color with which the texture of reflection probe will be cleared.</para>
		/// </summary>
		public Color backgroundColor
		{
			get
			{
				Color result;
				this.INTERNAL_get_backgroundColor(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_backgroundColor(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_backgroundColor(out Color value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_backgroundColor(ref Color value);

		/// <summary>
		///   <para>The intensity modifier that is applied to the texture of reflection probe in the shader.</para>
		/// </summary>
		public extern float intensity { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Distance around probe used for blending (used in deferred probes).</para>
		/// </summary>
		public extern float blendDistance { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Should this reflection probe use box projection?</para>
		/// </summary>
		public extern bool boxProjection { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The bounding volume of the reflection probe (Read Only).</para>
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
		///   <para>Should reflection probe texture be generated in the Editor (ReflectionProbeMode.Baked) or should probe use custom specified texure (ReflectionProbeMode.Custom)?</para>
		/// </summary>
		public extern ReflectionProbeMode mode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Reflection probe importance.</para>
		/// </summary>
		public extern int importance { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Sets the way the probe will refresh.
		///
		/// See Also: ReflectionProbeRefreshMode.</para>
		/// </summary>
		public extern ReflectionProbeRefreshMode refreshMode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Sets this probe time-slicing mode
		///
		/// See Also: ReflectionProbeTimeSlicingMode.</para>
		/// </summary>
		public extern ReflectionProbeTimeSlicingMode timeSlicingMode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Reference to the baked texture of the reflection probe's surrounding.</para>
		/// </summary>
		public extern Texture bakedTexture { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Reference to the baked texture of the reflection probe's surrounding. Use this to assign custom reflection texture.</para>
		/// </summary>
		public extern Texture customBakedTexture { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Texture which is passed to the shader of the objects in the vicinity of the reflection probe (Read Only).</para>
		/// </summary>
		public extern Texture texture { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Refreshes the probe's cubemap.</para>
		/// </summary>
		/// <param name="targetTexture">Target RendeTexture in which rendering should be done. Specifying null will update the probe's default texture.</param>
		/// <returns>
		///   <para>
		///     An integer representing a RenderID which can subsequently be used to check if the probe has finished rendering while rendering in time-slice mode.
		///
		///     See Also: IsFinishedRendering
		///     See Also: timeSlicingMode
		///   </para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int RenderProbe([DefaultValue("null")] RenderTexture targetTexture);

		[ExcludeFromDocs]
		public int RenderProbe()
		{
			RenderTexture targetTexture = null;
			return this.RenderProbe(targetTexture);
		}

		/// <summary>
		///   <para>Checks if a probe has finished a time-sliced render.</para>
		/// </summary>
		/// <param name="renderId">An integer representing the RenderID as returned by the RenderProbe method.</param>
		/// <returns>
		///   <para>
		///     True if the render has finished, false otherwise.
		///
		///     See Also: timeSlicingMode
		///   </para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsFinishedRendering(int renderId);

		/// <summary>
		///   <para>Utility method to blend 2 cubemaps into a target render texture.</para>
		/// </summary>
		/// <param name="src">Cubemap to blend from.</param>
		/// <param name="dst">Cubemap to blend to.</param>
		/// <param name="blend">Blend weight.</param>
		/// <param name="target">RenderTexture which will hold the result of the blend.</param>
		/// <returns>
		///   <para>Returns trues if cubemaps were blended, false otherwise.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool BlendCubemap(Texture src, Texture dst, float blend, RenderTexture target);
	}
}
