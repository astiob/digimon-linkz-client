using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>The type of a ProceduralProperty.</para>
	/// </summary>
	public enum ProceduralPropertyType
	{
		/// <summary>
		///   <para>Procedural boolean property. Use with ProceduralMaterial.GetProceduralBoolean.</para>
		/// </summary>
		Boolean,
		/// <summary>
		///   <para>Procedural float property. Use with ProceduralMaterial.GetProceduralFloat.</para>
		/// </summary>
		Float,
		/// <summary>
		///   <para>Procedural Vector2 property. Use with ProceduralMaterial.GetProceduralVector.</para>
		/// </summary>
		Vector2,
		/// <summary>
		///   <para>Procedural Vector3 property. Use with ProceduralMaterial.GetProceduralVector.</para>
		/// </summary>
		Vector3,
		/// <summary>
		///   <para>Procedural Vector4 property. Use with ProceduralMaterial.GetProceduralVector.</para>
		/// </summary>
		Vector4,
		/// <summary>
		///   <para>Procedural Color property without alpha. Use with ProceduralMaterial.GetProceduralColor.</para>
		/// </summary>
		Color3,
		/// <summary>
		///   <para>Procedural Color property with alpha. Use with ProceduralMaterial.GetProceduralColor.</para>
		/// </summary>
		Color4,
		/// <summary>
		///   <para>Procedural Enum property. Use with ProceduralMaterial.GetProceduralEnum.</para>
		/// </summary>
		Enum,
		/// <summary>
		///   <para>Procedural Texture property. Use with ProceduralMaterial.GetProceduralTexture.</para>
		/// </summary>
		Texture
	}
}
