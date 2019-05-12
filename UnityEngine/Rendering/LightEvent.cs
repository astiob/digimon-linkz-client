using System;

namespace UnityEngine.Rendering
{
	/// <summary>
	///   <para>Defines a place in light's rendering to attach Rendering.CommandBuffer objects to.</para>
	/// </summary>
	public enum LightEvent
	{
		/// <summary>
		///   <para>Before shadowmap is rendered.</para>
		/// </summary>
		BeforeShadowMap,
		/// <summary>
		///   <para>After shadowmap is rendered.</para>
		/// </summary>
		AfterShadowMap,
		/// <summary>
		///   <para>Before directional light screenspace shadow mask is computed.</para>
		/// </summary>
		BeforeScreenspaceMask,
		/// <summary>
		///   <para>After directional light screenspace shadow mask is computed.</para>
		/// </summary>
		AfterScreenspaceMask
	}
}
