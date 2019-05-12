using System;

namespace UnityEngine.Rendering
{
	/// <summary>
	///   <para>Defines a place in camera's rendering to attach Rendering.CommandBuffer objects to.</para>
	/// </summary>
	public enum CameraEvent
	{
		/// <summary>
		///   <para>Before camera's depth texture is generated.</para>
		/// </summary>
		BeforeDepthTexture,
		/// <summary>
		///   <para>After camera's depth texture is generated.</para>
		/// </summary>
		AfterDepthTexture,
		/// <summary>
		///   <para>Before camera's depth+normals texture is generated.</para>
		/// </summary>
		BeforeDepthNormalsTexture,
		/// <summary>
		///   <para>After camera's depth+normals texture is generated.</para>
		/// </summary>
		AfterDepthNormalsTexture,
		/// <summary>
		///   <para>Before deferred rendering G-buffer is rendered.</para>
		/// </summary>
		BeforeGBuffer,
		/// <summary>
		///   <para>After deferred rendering G-buffer is rendered.</para>
		/// </summary>
		AfterGBuffer,
		/// <summary>
		///   <para>Before lighting pass in deferred rendering.</para>
		/// </summary>
		BeforeLighting,
		/// <summary>
		///   <para>After lighting pass in deferred rendering.</para>
		/// </summary>
		AfterLighting,
		/// <summary>
		///   <para>Before final geometry pass in deferred lighting.</para>
		/// </summary>
		BeforeFinalPass,
		/// <summary>
		///   <para>After final geometry pass in deferred lighting.</para>
		/// </summary>
		AfterFinalPass,
		/// <summary>
		///   <para>Before opaque objects in forward rendering.</para>
		/// </summary>
		BeforeForwardOpaque,
		/// <summary>
		///   <para>After opaque objects in forward rendering.</para>
		/// </summary>
		AfterForwardOpaque,
		/// <summary>
		///   <para>Before image effects that happen between opaque &amp; transparent objects.</para>
		/// </summary>
		BeforeImageEffectsOpaque,
		/// <summary>
		///   <para>After image effects that happen between opaque &amp; transparent objects.</para>
		/// </summary>
		AfterImageEffectsOpaque,
		/// <summary>
		///   <para>Before skybox is drawn.</para>
		/// </summary>
		BeforeSkybox,
		/// <summary>
		///   <para>After skybox is drawn.</para>
		/// </summary>
		AfterSkybox,
		/// <summary>
		///   <para>Before transparent objects in forward rendering.</para>
		/// </summary>
		BeforeForwardAlpha,
		/// <summary>
		///   <para>After transparent objects in forward rendering.</para>
		/// </summary>
		AfterForwardAlpha,
		/// <summary>
		///   <para>Before image effects.</para>
		/// </summary>
		BeforeImageEffects,
		/// <summary>
		///   <para>After image effects.</para>
		/// </summary>
		AfterImageEffects,
		/// <summary>
		///   <para>After camera has done rendering everything.</para>
		/// </summary>
		AfterEverything,
		/// <summary>
		///   <para>Before reflections pass in deferred rendering.</para>
		/// </summary>
		BeforeReflections,
		/// <summary>
		///   <para>After reflections pass in deferred rendering.</para>
		/// </summary>
		AfterReflections
	}
}
