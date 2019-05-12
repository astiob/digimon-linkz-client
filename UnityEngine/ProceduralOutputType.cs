using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>The type of generated image in a ProceduralMaterial.</para>
	/// </summary>
	public enum ProceduralOutputType
	{
		/// <summary>
		///   <para>Undefined type.</para>
		/// </summary>
		Unknown,
		/// <summary>
		///   <para>Diffuse map.</para>
		/// </summary>
		Diffuse,
		/// <summary>
		///   <para>Normal (Bump) map.</para>
		/// </summary>
		Normal,
		/// <summary>
		///   <para>Height map.</para>
		/// </summary>
		Height,
		/// <summary>
		///   <para>Emissive map.</para>
		/// </summary>
		Emissive,
		/// <summary>
		///   <para>Specular map.</para>
		/// </summary>
		Specular,
		/// <summary>
		///   <para>Opacity (Tranparency) map.</para>
		/// </summary>
		Opacity,
		/// <summary>
		///   <para>Smoothness map (formerly referred to as Glossiness).</para>
		/// </summary>
		Smoothness,
		/// <summary>
		///   <para>Ambient occlusion map.</para>
		/// </summary>
		AmbientOcclusion,
		/// <summary>
		///   <para>Detail mask map.</para>
		/// </summary>
		DetailMask,
		/// <summary>
		///   <para>Metalness map.</para>
		/// </summary>
		Metallic,
		/// <summary>
		///   <para>Roughness map.</para>
		/// </summary>
		Roughness
	}
}
