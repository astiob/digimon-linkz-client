using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Vertex class used by a Canvas for managing vertices.</para>
	/// </summary>
	public struct UIVertex
	{
		/// <summary>
		///   <para>Vertex position.</para>
		/// </summary>
		public Vector3 position;

		/// <summary>
		///   <para>Normal.</para>
		/// </summary>
		public Vector3 normal;

		/// <summary>
		///   <para>Vertex color.</para>
		/// </summary>
		public Color32 color;

		/// <summary>
		///   <para>UV0.</para>
		/// </summary>
		public Vector2 uv0;

		/// <summary>
		///   <para>UV1.</para>
		/// </summary>
		public Vector2 uv1;

		/// <summary>
		///   <para>Tangent.</para>
		/// </summary>
		public Vector4 tangent;

		private static readonly Color32 s_DefaultColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		private static readonly Vector4 s_DefaultTangent = new Vector4(1f, 0f, 0f, -1f);

		/// <summary>
		///   <para>Simple UIVertex with sensible settings for use in the UI system.</para>
		/// </summary>
		public static UIVertex simpleVert = new UIVertex
		{
			position = Vector3.zero,
			normal = Vector3.back,
			tangent = UIVertex.s_DefaultTangent,
			color = UIVertex.s_DefaultColor,
			uv0 = Vector2.zero,
			uv1 = Vector2.zero
		};
	}
}
