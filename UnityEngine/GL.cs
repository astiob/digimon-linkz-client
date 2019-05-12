using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	/// <summary>
	///   <para>Low-level graphics library.</para>
	/// </summary>
	public sealed class GL
	{
		/// <summary>
		///   <para>Mode for Begin: draw triangles.</para>
		/// </summary>
		public const int TRIANGLES = 4;

		/// <summary>
		///   <para>Mode for Begin: draw triangle strip.</para>
		/// </summary>
		public const int TRIANGLE_STRIP = 5;

		/// <summary>
		///   <para>Mode for Begin: draw quads.</para>
		/// </summary>
		public const int QUADS = 7;

		/// <summary>
		///   <para>Mode for Begin: draw lines.</para>
		/// </summary>
		public const int LINES = 1;

		/// <summary>
		///   <para>Submit a vertex.</para>
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Vertex3(float x, float y, float z);

		/// <summary>
		///   <para>Submit a vertex.</para>
		/// </summary>
		/// <param name="v"></param>
		public static void Vertex(Vector3 v)
		{
			GL.INTERNAL_CALL_Vertex(ref v);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Vertex(ref Vector3 v);

		/// <summary>
		///   <para>Sets current vertex color.</para>
		/// </summary>
		/// <param name="c"></param>
		public static void Color(Color c)
		{
			GL.INTERNAL_CALL_Color(ref c);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Color(ref Color c);

		/// <summary>
		///   <para>Sets current texture coordinate (v.x,v.y,v.z) for all texture units.</para>
		/// </summary>
		/// <param name="v"></param>
		public static void TexCoord(Vector3 v)
		{
			GL.INTERNAL_CALL_TexCoord(ref v);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_TexCoord(ref Vector3 v);

		/// <summary>
		///   <para>Sets current texture coordinate (x,y) for all texture units.</para>
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void TexCoord2(float x, float y);

		/// <summary>
		///   <para>Sets current texture coordinate (x,y,z) for all texture units.</para>
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void TexCoord3(float x, float y, float z);

		/// <summary>
		///   <para>Sets current texture coordinate (x,y) for the actual texture unit.</para>
		/// </summary>
		/// <param name="unit"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void MultiTexCoord2(int unit, float x, float y);

		/// <summary>
		///   <para>Sets current texture coordinate (x,y,z) to the actual texture unit.</para>
		/// </summary>
		/// <param name="unit"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void MultiTexCoord3(int unit, float x, float y, float z);

		/// <summary>
		///   <para>Sets current texture coordinate (v.x,v.y,v.z) to the actual texture unit.</para>
		/// </summary>
		/// <param name="unit"></param>
		/// <param name="v"></param>
		public static void MultiTexCoord(int unit, Vector3 v)
		{
			GL.INTERNAL_CALL_MultiTexCoord(unit, ref v);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_MultiTexCoord(int unit, ref Vector3 v);

		/// <summary>
		///   <para>Begin drawing 3D primitives.</para>
		/// </summary>
		/// <param name="mode">Primitives to draw: can be TRIANGLES, TRIANGLE_STRIP, QUADS or LINES.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Begin(int mode);

		/// <summary>
		///   <para>End drawing 3D primitives.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void End();

		/// <summary>
		///   <para>Helper function to set up an ortho perspective transform.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void LoadOrtho();

		/// <summary>
		///   <para>Setup a matrix for pixel-correct rendering.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void LoadPixelMatrix();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void LoadPixelMatrixArgs(float left, float right, float bottom, float top);

		/// <summary>
		///   <para>Setup a matrix for pixel-correct rendering.</para>
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <param name="bottom"></param>
		/// <param name="top"></param>
		public static void LoadPixelMatrix(float left, float right, float bottom, float top)
		{
			GL.LoadPixelMatrixArgs(left, right, bottom, top);
		}

		/// <summary>
		///   <para>Set the rendering viewport.</para>
		/// </summary>
		/// <param name="pixelRect"></param>
		public static void Viewport(Rect pixelRect)
		{
			GL.INTERNAL_CALL_Viewport(ref pixelRect);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Viewport(ref Rect pixelRect);

		/// <summary>
		///   <para>Load an arbitrary matrix to the current projection matrix.</para>
		/// </summary>
		/// <param name="mat"></param>
		public static void LoadProjectionMatrix(Matrix4x4 mat)
		{
			GL.INTERNAL_CALL_LoadProjectionMatrix(ref mat);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_LoadProjectionMatrix(ref Matrix4x4 mat);

		/// <summary>
		///   <para>Load the identity matrix to the current modelview matrix.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void LoadIdentity();

		/// <summary>
		///   <para>The current modelview matrix.</para>
		/// </summary>
		public static Matrix4x4 modelview
		{
			get
			{
				Matrix4x4 result;
				GL.INTERNAL_get_modelview(out result);
				return result;
			}
			set
			{
				GL.INTERNAL_set_modelview(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_get_modelview(out Matrix4x4 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_set_modelview(ref Matrix4x4 value);

		/// <summary>
		///   <para>Multiplies the current modelview matrix with the one specified.</para>
		/// </summary>
		/// <param name="mat"></param>
		public static void MultMatrix(Matrix4x4 mat)
		{
			GL.INTERNAL_CALL_MultMatrix(ref mat);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_MultMatrix(ref Matrix4x4 mat);

		/// <summary>
		///   <para>Saves both projection and modelview matrices to the matrix stack.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void PushMatrix();

		/// <summary>
		///   <para>Restores both projection and modelview matrices off the top of the matrix stack.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void PopMatrix();

		/// <summary>
		///   <para>Compute GPU projection matrix from camera's projection matrix.</para>
		/// </summary>
		/// <param name="proj"></param>
		/// <param name="renderIntoTexture"></param>
		public static Matrix4x4 GetGPUProjectionMatrix(Matrix4x4 proj, bool renderIntoTexture)
		{
			return GL.INTERNAL_CALL_GetGPUProjectionMatrix(ref proj, renderIntoTexture);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Matrix4x4 INTERNAL_CALL_GetGPUProjectionMatrix(ref Matrix4x4 proj, bool renderIntoTexture);

		/// <summary>
		///   <para>Should rendering be done in wireframe?</para>
		/// </summary>
		public static extern bool wireframe { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public static extern bool sRGBWrite { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Select whether to invert the backface culling (true) or not (false).</para>
		/// </summary>
		public static extern bool invertCulling { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[WrapperlessIcall]
		[Obsolete("Use invertCulling property")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetRevertBackfacing(bool revertBackFaces);

		[ExcludeFromDocs]
		public static void Clear(bool clearDepth, bool clearColor, Color backgroundColor)
		{
			float depth = 1f;
			GL.Clear(clearDepth, clearColor, backgroundColor, depth);
		}

		/// <summary>
		///   <para>Clear the current render buffer.</para>
		/// </summary>
		/// <param name="clearDepth">Should the depth buffer be cleared?</param>
		/// <param name="clearColor">Should the color buffer be cleared?</param>
		/// <param name="backgroundColor">The color to clear with, used only if clearColor is true.</param>
		/// <param name="depth">The depth to clear Z buffer with, used only if clearDepth is true.</param>
		public static void Clear(bool clearDepth, bool clearColor, Color backgroundColor, [DefaultValue("1.0f")] float depth)
		{
			GL.Internal_Clear(clearDepth, clearColor, backgroundColor, depth);
		}

		private static void Internal_Clear(bool clearDepth, bool clearColor, Color backgroundColor, float depth)
		{
			GL.INTERNAL_CALL_Internal_Clear(clearDepth, clearColor, ref backgroundColor, depth);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Internal_Clear(bool clearDepth, bool clearColor, ref Color backgroundColor, float depth);

		/// <summary>
		///   <para>Clear the current render buffer with camera's skybox.</para>
		/// </summary>
		/// <param name="clearDepth">Should the depth buffer be cleared?</param>
		/// <param name="camera">Camera to get projection parameters and skybox from.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ClearWithSkybox(bool clearDepth, Camera camera);

		/// <summary>
		///   <para>Invalidate the internally cached renderstates.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void InvalidateState();

		/// <summary>
		///   <para>Send a user-defined event to a native code plugin.</para>
		/// </summary>
		/// <param name="eventID">User defined id to send to the callback.</param>
		/// <param name="callback">Native code callback to queue for Unity's renderer to invoke.</param>
		[Obsolete("IssuePluginEvent(eventID) is deprecated. Use IssuePluginEvent(callback, eventID) instead.")]
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void IssuePluginEvent(int eventID);

		/// <summary>
		///   <para>Send a user-defined event to a native code plugin.</para>
		/// </summary>
		/// <param name="eventID">User defined id to send to the callback.</param>
		/// <param name="callback">Native code callback to queue for Unity's renderer to invoke.</param>
		public static void IssuePluginEvent(IntPtr callback, int eventID)
		{
			if (callback == IntPtr.Zero)
			{
				throw new ArgumentException("Null callback specified.");
			}
			GL.IssuePluginEventInternal(callback, eventID);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void IssuePluginEventInternal(IntPtr callback, int eventID);

		/// <summary>
		///   <para>Resolves the render target for subsequent operations sampling from it.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void RenderTargetBarrier();
	}
}
