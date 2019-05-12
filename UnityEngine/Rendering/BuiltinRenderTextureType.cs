using System;

namespace UnityEngine.Rendering
{
	/// <summary>
	///   <para>Built-in temporary render textures produced during camera's rendering.</para>
	/// </summary>
	public enum BuiltinRenderTextureType
	{
		None,
		/// <summary>
		///   <para>Currently active render target.</para>
		/// </summary>
		CurrentActive,
		/// <summary>
		///   <para>Target texture of currently rendering camera.</para>
		/// </summary>
		CameraTarget,
		/// <summary>
		///   <para>Camera's depth texture.</para>
		/// </summary>
		Depth,
		/// <summary>
		///   <para>Camera's depth+normals texture.</para>
		/// </summary>
		DepthNormals,
		/// <summary>
		///   <para>Deferred lighting (normals+specular) G-buffer.</para>
		/// </summary>
		PrepassNormalsSpec = 7,
		/// <summary>
		///   <para>Deferred lighting light buffer.</para>
		/// </summary>
		PrepassLight,
		/// <summary>
		///   <para>Deferred lighting HDR specular light buffer (Xbox 360 only).</para>
		/// </summary>
		PrepassLightSpec,
		/// <summary>
		///   <para>Deferred shading G-buffer #0 (typically diffuse color).</para>
		/// </summary>
		GBuffer0,
		/// <summary>
		///   <para>Deferred shading G-buffer #1 (typically specular + roughness).</para>
		/// </summary>
		GBuffer1,
		/// <summary>
		///   <para>Deferred shading G-buffer #2 (typically normals).</para>
		/// </summary>
		GBuffer2,
		/// <summary>
		///   <para>Deferred shading G-buffer #3 (typically emission/lighting).</para>
		/// </summary>
		GBuffer3,
		/// <summary>
		///   <para>Reflections gathered from default reflection and reflections probes.</para>
		/// </summary>
		Reflections
	}
}
