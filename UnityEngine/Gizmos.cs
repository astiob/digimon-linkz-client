using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	/// <summary>
	///   <para>Gizmos are used to give visual debugging or setup aids in the scene view.</para>
	/// </summary>
	public sealed class Gizmos
	{
		/// <summary>
		///   <para>Draws a ray starting at from to from + direction.</para>
		/// </summary>
		/// <param name="r"></param>
		/// <param name="from"></param>
		/// <param name="direction"></param>
		public static void DrawRay(Ray r)
		{
			Gizmos.DrawLine(r.origin, r.origin + r.direction);
		}

		/// <summary>
		///   <para>Draws a ray starting at from to from + direction.</para>
		/// </summary>
		/// <param name="r"></param>
		/// <param name="from"></param>
		/// <param name="direction"></param>
		public static void DrawRay(Vector3 from, Vector3 direction)
		{
			Gizmos.DrawLine(from, from + direction);
		}

		/// <summary>
		///   <para>Draws a line starting at from towards to.</para>
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		public static void DrawLine(Vector3 from, Vector3 to)
		{
			Gizmos.INTERNAL_CALL_DrawLine(ref from, ref to);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_DrawLine(ref Vector3 from, ref Vector3 to);

		/// <summary>
		///   <para>Draws a wireframe sphere with center and radius.</para>
		/// </summary>
		/// <param name="center"></param>
		/// <param name="radius"></param>
		public static void DrawWireSphere(Vector3 center, float radius)
		{
			Gizmos.INTERNAL_CALL_DrawWireSphere(ref center, radius);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_DrawWireSphere(ref Vector3 center, float radius);

		/// <summary>
		///   <para>Draws a solid sphere with center and radius.</para>
		/// </summary>
		/// <param name="center"></param>
		/// <param name="radius"></param>
		public static void DrawSphere(Vector3 center, float radius)
		{
			Gizmos.INTERNAL_CALL_DrawSphere(ref center, radius);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_DrawSphere(ref Vector3 center, float radius);

		/// <summary>
		///   <para>Draw a wireframe box with center and size.</para>
		/// </summary>
		/// <param name="center"></param>
		/// <param name="size"></param>
		public static void DrawWireCube(Vector3 center, Vector3 size)
		{
			Gizmos.INTERNAL_CALL_DrawWireCube(ref center, ref size);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_DrawWireCube(ref Vector3 center, ref Vector3 size);

		/// <summary>
		///   <para>Draw a solid box with center and size.</para>
		/// </summary>
		/// <param name="center"></param>
		/// <param name="size"></param>
		public static void DrawCube(Vector3 center, Vector3 size)
		{
			Gizmos.INTERNAL_CALL_DrawCube(ref center, ref size);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_DrawCube(ref Vector3 center, ref Vector3 size);

		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation)
		{
			Vector3 one = Vector3.one;
			Gizmos.DrawMesh(mesh, position, rotation, one);
		}

		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Vector3 position)
		{
			Vector3 one = Vector3.one;
			Quaternion identity = Quaternion.identity;
			Gizmos.DrawMesh(mesh, position, identity, one);
		}

		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh)
		{
			Vector3 one = Vector3.one;
			Quaternion identity = Quaternion.identity;
			Vector3 zero = Vector3.zero;
			Gizmos.DrawMesh(mesh, zero, identity, one);
		}

		/// <summary>
		///   <para>Draws a mesh.</para>
		/// </summary>
		/// <param name="mesh">Mesh to draw as a gizmo.</param>
		/// <param name="position">Position (default is zero).</param>
		/// <param name="rotation">Rotation (default is no rotation).</param>
		/// <param name="scale">Scale (default is no scale).</param>
		/// <param name="submeshIndex">Submesh to draw (default is -1, which draws whole mesh).</param>
		public static void DrawMesh(Mesh mesh, [DefaultValue("Vector3.zero")] Vector3 position, [DefaultValue("Quaternion.identity")] Quaternion rotation, [DefaultValue("Vector3.one")] Vector3 scale)
		{
			Gizmos.DrawMesh(mesh, -1, position, rotation, scale);
		}

		/// <summary>
		///   <para>Draws a mesh.</para>
		/// </summary>
		/// <param name="mesh">Mesh to draw as a gizmo.</param>
		/// <param name="position">Position (default is zero).</param>
		/// <param name="rotation">Rotation (default is no rotation).</param>
		/// <param name="scale">Scale (default is no scale).</param>
		/// <param name="submeshIndex">Submesh to draw (default is -1, which draws whole mesh).</param>
		public static void DrawMesh(Mesh mesh, int submeshIndex, [DefaultValue("Vector3.zero")] Vector3 position, [DefaultValue("Quaternion.identity")] Quaternion rotation, [DefaultValue("Vector3.one")] Vector3 scale)
		{
			Gizmos.INTERNAL_CALL_DrawMesh(mesh, submeshIndex, ref position, ref rotation, ref scale);
		}

		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, int submeshIndex, Vector3 position, Quaternion rotation)
		{
			Vector3 one = Vector3.one;
			Gizmos.INTERNAL_CALL_DrawMesh(mesh, submeshIndex, ref position, ref rotation, ref one);
		}

		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, int submeshIndex, Vector3 position)
		{
			Vector3 one = Vector3.one;
			Quaternion identity = Quaternion.identity;
			Gizmos.INTERNAL_CALL_DrawMesh(mesh, submeshIndex, ref position, ref identity, ref one);
		}

		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, int submeshIndex)
		{
			Vector3 one = Vector3.one;
			Quaternion identity = Quaternion.identity;
			Vector3 zero = Vector3.zero;
			Gizmos.INTERNAL_CALL_DrawMesh(mesh, submeshIndex, ref zero, ref identity, ref one);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_DrawMesh(Mesh mesh, int submeshIndex, ref Vector3 position, ref Quaternion rotation, ref Vector3 scale);

		[ExcludeFromDocs]
		public static void DrawWireMesh(Mesh mesh, Vector3 position, Quaternion rotation)
		{
			Vector3 one = Vector3.one;
			Gizmos.DrawWireMesh(mesh, position, rotation, one);
		}

		[ExcludeFromDocs]
		public static void DrawWireMesh(Mesh mesh, Vector3 position)
		{
			Vector3 one = Vector3.one;
			Quaternion identity = Quaternion.identity;
			Gizmos.DrawWireMesh(mesh, position, identity, one);
		}

		[ExcludeFromDocs]
		public static void DrawWireMesh(Mesh mesh)
		{
			Vector3 one = Vector3.one;
			Quaternion identity = Quaternion.identity;
			Vector3 zero = Vector3.zero;
			Gizmos.DrawWireMesh(mesh, zero, identity, one);
		}

		/// <summary>
		///   <para>Draws a wireframe mesh.</para>
		/// </summary>
		/// <param name="mesh">Mesh to draw as a gizmo.</param>
		/// <param name="position">Position (default is zero).</param>
		/// <param name="rotation">Rotation (default is no rotation).</param>
		/// <param name="scale">Scale (default is no scale).</param>
		/// <param name="submeshIndex">Submesh to draw (default is -1, which draws whole mesh).</param>
		public static void DrawWireMesh(Mesh mesh, [DefaultValue("Vector3.zero")] Vector3 position, [DefaultValue("Quaternion.identity")] Quaternion rotation, [DefaultValue("Vector3.one")] Vector3 scale)
		{
			Gizmos.DrawWireMesh(mesh, -1, position, rotation, scale);
		}

		/// <summary>
		///   <para>Draws a wireframe mesh.</para>
		/// </summary>
		/// <param name="mesh">Mesh to draw as a gizmo.</param>
		/// <param name="position">Position (default is zero).</param>
		/// <param name="rotation">Rotation (default is no rotation).</param>
		/// <param name="scale">Scale (default is no scale).</param>
		/// <param name="submeshIndex">Submesh to draw (default is -1, which draws whole mesh).</param>
		public static void DrawWireMesh(Mesh mesh, int submeshIndex, [DefaultValue("Vector3.zero")] Vector3 position, [DefaultValue("Quaternion.identity")] Quaternion rotation, [DefaultValue("Vector3.one")] Vector3 scale)
		{
			Gizmos.INTERNAL_CALL_DrawWireMesh(mesh, submeshIndex, ref position, ref rotation, ref scale);
		}

		[ExcludeFromDocs]
		public static void DrawWireMesh(Mesh mesh, int submeshIndex, Vector3 position, Quaternion rotation)
		{
			Vector3 one = Vector3.one;
			Gizmos.INTERNAL_CALL_DrawWireMesh(mesh, submeshIndex, ref position, ref rotation, ref one);
		}

		[ExcludeFromDocs]
		public static void DrawWireMesh(Mesh mesh, int submeshIndex, Vector3 position)
		{
			Vector3 one = Vector3.one;
			Quaternion identity = Quaternion.identity;
			Gizmos.INTERNAL_CALL_DrawWireMesh(mesh, submeshIndex, ref position, ref identity, ref one);
		}

		[ExcludeFromDocs]
		public static void DrawWireMesh(Mesh mesh, int submeshIndex)
		{
			Vector3 one = Vector3.one;
			Quaternion identity = Quaternion.identity;
			Vector3 zero = Vector3.zero;
			Gizmos.INTERNAL_CALL_DrawWireMesh(mesh, submeshIndex, ref zero, ref identity, ref one);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_DrawWireMesh(Mesh mesh, int submeshIndex, ref Vector3 position, ref Quaternion rotation, ref Vector3 scale);

		/// <summary>
		///   <para>Draw an icon at a position in the scene view.</para>
		/// </summary>
		/// <param name="center"></param>
		/// <param name="name"></param>
		/// <param name="allowScaling"></param>
		public static void DrawIcon(Vector3 center, string name, [DefaultValue("true")] bool allowScaling)
		{
			Gizmos.INTERNAL_CALL_DrawIcon(ref center, name, allowScaling);
		}

		/// <summary>
		///   <para>Draw an icon at a position in the scene view.</para>
		/// </summary>
		/// <param name="center"></param>
		/// <param name="name"></param>
		/// <param name="allowScaling"></param>
		[ExcludeFromDocs]
		public static void DrawIcon(Vector3 center, string name)
		{
			bool allowScaling = true;
			Gizmos.INTERNAL_CALL_DrawIcon(ref center, name, allowScaling);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_DrawIcon(ref Vector3 center, string name, bool allowScaling);

		/// <summary>
		///   <para>Draw a texture in the scene.</para>
		/// </summary>
		/// <param name="screenRect">The size and position of the texture on the "screen" defined by the XY plane.</param>
		/// <param name="texture">The texture to be displayed.</param>
		/// <param name="mat">An optional material to apply the texture.</param>
		/// <param name="leftBorder">Inset from the rectangle's left edge.</param>
		/// <param name="rightBorder">Inset from the rectangle's right edge.</param>
		/// <param name="topBorder">Inset from the rectangle's top edge.</param>
		/// <param name="bottomBorder">Inset from the rectangle's bottom edge.</param>
		[ExcludeFromDocs]
		public static void DrawGUITexture(Rect screenRect, Texture texture)
		{
			Material mat = null;
			Gizmos.DrawGUITexture(screenRect, texture, mat);
		}

		/// <summary>
		///   <para>Draw a texture in the scene.</para>
		/// </summary>
		/// <param name="screenRect">The size and position of the texture on the "screen" defined by the XY plane.</param>
		/// <param name="texture">The texture to be displayed.</param>
		/// <param name="mat">An optional material to apply the texture.</param>
		/// <param name="leftBorder">Inset from the rectangle's left edge.</param>
		/// <param name="rightBorder">Inset from the rectangle's right edge.</param>
		/// <param name="topBorder">Inset from the rectangle's top edge.</param>
		/// <param name="bottomBorder">Inset from the rectangle's bottom edge.</param>
		public static void DrawGUITexture(Rect screenRect, Texture texture, [DefaultValue("null")] Material mat)
		{
			Gizmos.DrawGUITexture(screenRect, texture, 0, 0, 0, 0, mat);
		}

		/// <summary>
		///   <para>Draw a texture in the scene.</para>
		/// </summary>
		/// <param name="screenRect">The size and position of the texture on the "screen" defined by the XY plane.</param>
		/// <param name="texture">The texture to be displayed.</param>
		/// <param name="mat">An optional material to apply the texture.</param>
		/// <param name="leftBorder">Inset from the rectangle's left edge.</param>
		/// <param name="rightBorder">Inset from the rectangle's right edge.</param>
		/// <param name="topBorder">Inset from the rectangle's top edge.</param>
		/// <param name="bottomBorder">Inset from the rectangle's bottom edge.</param>
		public static void DrawGUITexture(Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder, [DefaultValue("null")] Material mat)
		{
			Gizmos.INTERNAL_CALL_DrawGUITexture(ref screenRect, texture, leftBorder, rightBorder, topBorder, bottomBorder, mat);
		}

		/// <summary>
		///   <para>Draw a texture in the scene.</para>
		/// </summary>
		/// <param name="screenRect">The size and position of the texture on the "screen" defined by the XY plane.</param>
		/// <param name="texture">The texture to be displayed.</param>
		/// <param name="mat">An optional material to apply the texture.</param>
		/// <param name="leftBorder">Inset from the rectangle's left edge.</param>
		/// <param name="rightBorder">Inset from the rectangle's right edge.</param>
		/// <param name="topBorder">Inset from the rectangle's top edge.</param>
		/// <param name="bottomBorder">Inset from the rectangle's bottom edge.</param>
		[ExcludeFromDocs]
		public static void DrawGUITexture(Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder)
		{
			Material mat = null;
			Gizmos.INTERNAL_CALL_DrawGUITexture(ref screenRect, texture, leftBorder, rightBorder, topBorder, bottomBorder, mat);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_DrawGUITexture(ref Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Material mat);

		/// <summary>
		///   <para>Sets the color for the gizmos that will be drawn next.</para>
		/// </summary>
		public static Color color
		{
			get
			{
				Color result;
				Gizmos.INTERNAL_get_color(out result);
				return result;
			}
			set
			{
				Gizmos.INTERNAL_set_color(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_get_color(out Color value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_set_color(ref Color value);

		/// <summary>
		///   <para>Set the gizmo matrix used to draw all gizmos.</para>
		/// </summary>
		public static Matrix4x4 matrix
		{
			get
			{
				Matrix4x4 result;
				Gizmos.INTERNAL_get_matrix(out result);
				return result;
			}
			set
			{
				Gizmos.INTERNAL_set_matrix(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_get_matrix(out Matrix4x4 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_set_matrix(ref Matrix4x4 value);

		/// <summary>
		///   <para>Draw a camera frustum using the currently set Gizmos.matrix for it's location and rotation.</para>
		/// </summary>
		/// <param name="center">The apex of the truncated pyramid.</param>
		/// <param name="fov">Vertical field of view (ie, the angle at the apex in degrees).</param>
		/// <param name="maxRange">Distance of the frustum's far plane.</param>
		/// <param name="minRange">Distance of the frustum's near plane.</param>
		/// <param name="aspect">Width/height ratio.</param>
		public static void DrawFrustum(Vector3 center, float fov, float maxRange, float minRange, float aspect)
		{
			Gizmos.INTERNAL_CALL_DrawFrustum(ref center, fov, maxRange, minRange, aspect);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_DrawFrustum(ref Vector3 center, float fov, float maxRange, float minRange, float aspect);
	}
}
