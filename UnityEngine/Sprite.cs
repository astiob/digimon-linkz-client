using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	/// <summary>
	///   <para>Represents a Sprite object for use in 2D gameplay.</para>
	/// </summary>
	public sealed class Sprite : Object
	{
		public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, [DefaultValue("100.0f")] float pixelsPerUnit, [DefaultValue("0")] uint extrude, [DefaultValue("SpriteMeshType.Tight")] SpriteMeshType meshType, [DefaultValue("Vector4.zero")] Vector4 border)
		{
			return Sprite.INTERNAL_CALL_Create(texture, ref rect, ref pivot, pixelsPerUnit, extrude, meshType, ref border);
		}

		[ExcludeFromDocs]
		public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit, uint extrude, SpriteMeshType meshType)
		{
			Vector4 zero = Vector4.zero;
			return Sprite.INTERNAL_CALL_Create(texture, ref rect, ref pivot, pixelsPerUnit, extrude, meshType, ref zero);
		}

		[ExcludeFromDocs]
		public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit, uint extrude)
		{
			Vector4 zero = Vector4.zero;
			SpriteMeshType meshType = SpriteMeshType.Tight;
			return Sprite.INTERNAL_CALL_Create(texture, ref rect, ref pivot, pixelsPerUnit, extrude, meshType, ref zero);
		}

		/// <summary>
		///   <para>Create a new Sprite object.</para>
		/// </summary>
		/// <param name="texture">Texture from which to obtain the sprite graphic.</param>
		/// <param name="rect">Rectangular section of the texture to use for the sprite.</param>
		/// <param name="pivot">Sprite's pivot point relative to its graphic rectangle.</param>
		/// <param name="pixelsToUnits">Scaling to map pixels in the image to world space units.</param>
		/// <param name="pixelsPerUnit"></param>
		/// <param name="extrude"></param>
		/// <param name="meshType"></param>
		/// <param name="border"></param>
		[ExcludeFromDocs]
		public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit)
		{
			Vector4 zero = Vector4.zero;
			SpriteMeshType meshType = SpriteMeshType.Tight;
			uint extrude = 0u;
			return Sprite.INTERNAL_CALL_Create(texture, ref rect, ref pivot, pixelsPerUnit, extrude, meshType, ref zero);
		}

		[ExcludeFromDocs]
		public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot)
		{
			Vector4 zero = Vector4.zero;
			SpriteMeshType meshType = SpriteMeshType.Tight;
			uint extrude = 0u;
			float pixelsPerUnit = 100f;
			return Sprite.INTERNAL_CALL_Create(texture, ref rect, ref pivot, pixelsPerUnit, extrude, meshType, ref zero);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Sprite INTERNAL_CALL_Create(Texture2D texture, ref Rect rect, ref Vector2 pivot, float pixelsPerUnit, uint extrude, SpriteMeshType meshType, ref Vector4 border);

		/// <summary>
		///   <para>Bounds of the Sprite, specified by its center and extents in world space units.</para>
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
		///   <para>Location of the Sprite on the original Texture, specified in pixels.</para>
		/// </summary>
		public Rect rect
		{
			get
			{
				Rect result;
				this.INTERNAL_get_rect(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_rect(out Rect value);

		/// <summary>
		///   <para>The number of pixels in the sprite that correspond to one unit in world space. (Read Only)</para>
		/// </summary>
		public extern float pixelsPerUnit { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Get the reference to the used texture. If packed this will point to the atlas, if not packed will point to the source sprite.</para>
		/// </summary>
		public extern Texture2D texture { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns the texture that contains the alpha channel from the source texture. Unity generates this texture under the hood for sprites that have alpha in the source, and need to be compressed using techniques like ETC1.
		///
		/// Returns NULL if there is no associated alpha texture for the source sprite. This is the case if the sprite has not been setup to use ETC1 compression.</para>
		/// </summary>
		public extern Texture2D associatedAlphaSplitTexture { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Get the rectangle this sprite uses on its texture. Raises an exception if this sprite is tightly packed in an atlas.</para>
		/// </summary>
		public Rect textureRect
		{
			get
			{
				Rect result;
				this.INTERNAL_get_textureRect(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_textureRect(out Rect value);

		/// <summary>
		///   <para>Gets the offset of the rectangle this sprite uses on its texture to the original sprite bounds. If sprite mesh type is FullRect, offset is zero.</para>
		/// </summary>
		public Vector2 textureRectOffset
		{
			get
			{
				Vector2 result;
				Sprite.Internal_GetTextureRectOffset(this, out result);
				return result;
			}
		}

		/// <summary>
		///   <para>Returns true if this Sprite is packed in an atlas.</para>
		/// </summary>
		public extern bool packed { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>If Sprite is packed (see Sprite.packed), returns its SpritePackingMode.</para>
		/// </summary>
		public extern SpritePackingMode packingMode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>If Sprite is packed (see Sprite.packed), returns its SpritePackingRotation.</para>
		/// </summary>
		public extern SpritePackingRotation packingRotation { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_GetTextureRectOffset(Sprite sprite, out Vector2 output);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_GetPivot(Sprite sprite, out Vector2 output);

		/// <summary>
		///   <para>Location of the Sprite's center point in the Rect on the original Texture, specified in pixels.</para>
		/// </summary>
		public Vector2 pivot
		{
			get
			{
				Vector2 result;
				Sprite.Internal_GetPivot(this, out result);
				return result;
			}
		}

		/// <summary>
		///   <para>Returns the border sizes of the sprite.</para>
		/// </summary>
		public Vector4 border
		{
			get
			{
				Vector4 result;
				this.INTERNAL_get_border(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_border(out Vector4 value);

		/// <summary>
		///   <para>Returns a copy of the array containing sprite mesh vertex positions.</para>
		/// </summary>
		public extern Vector2[] vertices { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns a copy of the array containing sprite mesh triangles.</para>
		/// </summary>
		public extern ushort[] triangles { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The base texture coordinates of the sprite mesh.</para>
		/// </summary>
		public extern Vector2[] uv { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Sets up new Sprite geometry.</para>
		/// </summary>
		/// <param name="vertices">Array of vertex positions in Sprite Rect space.</param>
		/// <param name="triangles">Array of sprite mesh triangle indices.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void OverrideGeometry(Vector2[] vertices, ushort[] triangles);
	}
}
