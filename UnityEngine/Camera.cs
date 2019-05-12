using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;
using UnityEngine.Rendering;

namespace UnityEngine
{
	/// <summary>
	///   <para>A Camera is a device through which the player views the world.</para>
	/// </summary>
	public sealed class Camera : Behaviour
	{
		/// <summary>
		///   <para>Event that is fired before any camera starts culling.</para>
		/// </summary>
		public static Camera.CameraCallback onPreCull;

		/// <summary>
		///   <para>Event that is fired before any camera starts rendering.</para>
		/// </summary>
		public static Camera.CameraCallback onPreRender;

		/// <summary>
		///   <para>Event that is fired after any camera finishes rendering.</para>
		/// </summary>
		public static Camera.CameraCallback onPostRender;

		[Obsolete("use Camera.fieldOfView instead.")]
		public float fov
		{
			get
			{
				return this.fieldOfView;
			}
			set
			{
				this.fieldOfView = value;
			}
		}

		[Obsolete("use Camera.nearClipPlane instead.")]
		public float near
		{
			get
			{
				return this.nearClipPlane;
			}
			set
			{
				this.nearClipPlane = value;
			}
		}

		[Obsolete("use Camera.farClipPlane instead.")]
		public float far
		{
			get
			{
				return this.farClipPlane;
			}
			set
			{
				this.farClipPlane = value;
			}
		}

		/// <summary>
		///   <para>The field of view of the camera in degrees.</para>
		/// </summary>
		public extern float fieldOfView { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The near clipping plane distance.</para>
		/// </summary>
		public extern float nearClipPlane { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The far clipping plane distance.</para>
		/// </summary>
		public extern float farClipPlane { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Rendering path.</para>
		/// </summary>
		public extern RenderingPath renderingPath { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Actually used rendering path (Read Only).</para>
		/// </summary>
		public extern RenderingPath actualRenderingPath { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>High dynamic range rendering.</para>
		/// </summary>
		public extern bool hdr { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern string[] GetHDRWarnings();

		/// <summary>
		///   <para>Camera's half-size when in orthographic mode.</para>
		/// </summary>
		public extern float orthographicSize { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Is the camera orthographic (true) or perspective (false)?</para>
		/// </summary>
		public extern bool orthographic { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Opaque object sorting mode.</para>
		/// </summary>
		public extern OpaqueSortMode opaqueSortMode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Transparent object sorting mode.</para>
		/// </summary>
		public extern TransparencySortMode transparencySortMode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Camera's depth in the camera rendering order.</para>
		/// </summary>
		public extern float depth { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The aspect ratio (width divided by height).</para>
		/// </summary>
		public extern float aspect { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>This is used to render parts of the scene selectively.</para>
		/// </summary>
		public extern int cullingMask { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		internal static extern int PreviewCullingLayer { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Mask to select which layers can trigger events on the camera.</para>
		/// </summary>
		public extern int eventMask { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The color with which the screen will be cleared.</para>
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
		///   <para>Where on the screen is the camera rendered in normalized coordinates.</para>
		/// </summary>
		public Rect rect
		{
			get
			{
				Rect result;
				this.INTERNAL_get_rect(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_rect(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_rect(out Rect value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_rect(ref Rect value);

		/// <summary>
		///   <para>Where on the screen is the camera rendered in pixel coordinates.</para>
		/// </summary>
		public Rect pixelRect
		{
			get
			{
				Rect result;
				this.INTERNAL_get_pixelRect(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_pixelRect(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_pixelRect(out Rect value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_pixelRect(ref Rect value);

		/// <summary>
		///   <para>Destination render texture.</para>
		/// </summary>
		public extern RenderTexture targetTexture { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetTargetBuffersImpl(out RenderBuffer color, out RenderBuffer depth);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetTargetBuffersMRTImpl(RenderBuffer[] color, out RenderBuffer depth);

		/// <summary>
		///   <para>Sets the Camera to render to the chosen buffers of one or more RenderTextures.</para>
		/// </summary>
		/// <param name="colorBuffer">The RenderBuffer(s) to which color information will be rendered.</param>
		/// <param name="depthBuffer">The RenderBuffer to which depth information will be rendered.</param>
		public void SetTargetBuffers(RenderBuffer colorBuffer, RenderBuffer depthBuffer)
		{
			this.SetTargetBuffersImpl(out colorBuffer, out depthBuffer);
		}

		/// <summary>
		///   <para>Sets the Camera to render to the chosen buffers of one or more RenderTextures.</para>
		/// </summary>
		/// <param name="colorBuffer">The RenderBuffer(s) to which color information will be rendered.</param>
		/// <param name="depthBuffer">The RenderBuffer to which depth information will be rendered.</param>
		public void SetTargetBuffers(RenderBuffer[] colorBuffer, RenderBuffer depthBuffer)
		{
			this.SetTargetBuffersMRTImpl(colorBuffer, out depthBuffer);
		}

		/// <summary>
		///   <para>How wide is the camera in pixels (Read Only).</para>
		/// </summary>
		public extern int pixelWidth { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>How tall is the camera in pixels (Read Only).</para>
		/// </summary>
		public extern int pixelHeight { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Matrix that transforms from camera space to world space (Read Only).</para>
		/// </summary>
		public Matrix4x4 cameraToWorldMatrix
		{
			get
			{
				Matrix4x4 result;
				this.INTERNAL_get_cameraToWorldMatrix(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_cameraToWorldMatrix(out Matrix4x4 value);

		/// <summary>
		///   <para>Matrix that transforms from world to camera space.</para>
		/// </summary>
		public Matrix4x4 worldToCameraMatrix
		{
			get
			{
				Matrix4x4 result;
				this.INTERNAL_get_worldToCameraMatrix(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_worldToCameraMatrix(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_worldToCameraMatrix(out Matrix4x4 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_worldToCameraMatrix(ref Matrix4x4 value);

		/// <summary>
		///   <para>Make the rendering position reflect the camera's position in the scene.</para>
		/// </summary>
		public void ResetWorldToCameraMatrix()
		{
			Camera.INTERNAL_CALL_ResetWorldToCameraMatrix(this);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_ResetWorldToCameraMatrix(Camera self);

		/// <summary>
		///   <para>Set a custom projection matrix.</para>
		/// </summary>
		public Matrix4x4 projectionMatrix
		{
			get
			{
				Matrix4x4 result;
				this.INTERNAL_get_projectionMatrix(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_projectionMatrix(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_projectionMatrix(out Matrix4x4 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_projectionMatrix(ref Matrix4x4 value);

		/// <summary>
		///   <para>Make the projection reflect normal camera's parameters.</para>
		/// </summary>
		public void ResetProjectionMatrix()
		{
			Camera.INTERNAL_CALL_ResetProjectionMatrix(this);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_ResetProjectionMatrix(Camera self);

		/// <summary>
		///   <para>Revert the aspect ratio to the screen's aspect ratio.</para>
		/// </summary>
		public void ResetAspect()
		{
			Camera.INTERNAL_CALL_ResetAspect(this);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_ResetAspect(Camera self);

		/// <summary>
		///   <para>Get the world-space speed of the camera (Read Only).</para>
		/// </summary>
		public Vector3 velocity
		{
			get
			{
				Vector3 result;
				this.INTERNAL_get_velocity(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_velocity(out Vector3 value);

		/// <summary>
		///   <para>How the camera clears the background.</para>
		/// </summary>
		public extern CameraClearFlags clearFlags { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Stereoscopic rendering.</para>
		/// </summary>
		public extern bool stereoEnabled { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Distance between the virtual eyes.</para>
		/// </summary>
		public extern float stereoSeparation { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Distance to a point where virtual eyes converge.</para>
		/// </summary>
		public extern float stereoConvergence { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Identifies what kind of camera this is.</para>
		/// </summary>
		public extern CameraType cameraType { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Render only once and use resulting image for both eyes.</para>
		/// </summary>
		public extern bool stereoMirrorMode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Set the target display for this Camera.</para>
		/// </summary>
		public extern int targetDisplay { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Transforms position from world space into screen space.</para>
		/// </summary>
		/// <param name="position"></param>
		public Vector3 WorldToScreenPoint(Vector3 position)
		{
			return Camera.INTERNAL_CALL_WorldToScreenPoint(this, ref position);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Vector3 INTERNAL_CALL_WorldToScreenPoint(Camera self, ref Vector3 position);

		/// <summary>
		///   <para>Transforms position from world space into viewport space.</para>
		/// </summary>
		/// <param name="position"></param>
		public Vector3 WorldToViewportPoint(Vector3 position)
		{
			return Camera.INTERNAL_CALL_WorldToViewportPoint(this, ref position);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Vector3 INTERNAL_CALL_WorldToViewportPoint(Camera self, ref Vector3 position);

		/// <summary>
		///   <para>Transforms position from viewport space into world space.</para>
		/// </summary>
		/// <param name="position"></param>
		public Vector3 ViewportToWorldPoint(Vector3 position)
		{
			return Camera.INTERNAL_CALL_ViewportToWorldPoint(this, ref position);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Vector3 INTERNAL_CALL_ViewportToWorldPoint(Camera self, ref Vector3 position);

		/// <summary>
		///   <para>Transforms position from screen space into world space.</para>
		/// </summary>
		/// <param name="position"></param>
		public Vector3 ScreenToWorldPoint(Vector3 position)
		{
			return Camera.INTERNAL_CALL_ScreenToWorldPoint(this, ref position);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Vector3 INTERNAL_CALL_ScreenToWorldPoint(Camera self, ref Vector3 position);

		/// <summary>
		///   <para>Transforms position from screen space into viewport space.</para>
		/// </summary>
		/// <param name="position"></param>
		public Vector3 ScreenToViewportPoint(Vector3 position)
		{
			return Camera.INTERNAL_CALL_ScreenToViewportPoint(this, ref position);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Vector3 INTERNAL_CALL_ScreenToViewportPoint(Camera self, ref Vector3 position);

		/// <summary>
		///   <para>Transforms position from viewport space into screen space.</para>
		/// </summary>
		/// <param name="position"></param>
		public Vector3 ViewportToScreenPoint(Vector3 position)
		{
			return Camera.INTERNAL_CALL_ViewportToScreenPoint(this, ref position);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Vector3 INTERNAL_CALL_ViewportToScreenPoint(Camera self, ref Vector3 position);

		/// <summary>
		///   <para>Returns a ray going from camera through a viewport point.</para>
		/// </summary>
		/// <param name="position"></param>
		public Ray ViewportPointToRay(Vector3 position)
		{
			return Camera.INTERNAL_CALL_ViewportPointToRay(this, ref position);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Ray INTERNAL_CALL_ViewportPointToRay(Camera self, ref Vector3 position);

		/// <summary>
		///   <para>Returns a ray going from camera through a screen point.</para>
		/// </summary>
		/// <param name="position"></param>
		public Ray ScreenPointToRay(Vector3 position)
		{
			return Camera.INTERNAL_CALL_ScreenPointToRay(this, ref position);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Ray INTERNAL_CALL_ScreenPointToRay(Camera self, ref Vector3 position);

		/// <summary>
		///   <para>The first enabled camera tagged "MainCamera" (Read Only).</para>
		/// </summary>
		public static extern Camera main { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The camera we are currently rendering with, for low-level render control only (Read Only).</para>
		/// </summary>
		public static extern Camera current { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns all enabled cameras in the scene.</para>
		/// </summary>
		public static extern Camera[] allCameras { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The number of cameras in the current scene.</para>
		/// </summary>
		public static extern int allCamerasCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Fills an array of Camera with the current cameras in the scene, without allocating a new array.</para>
		/// </summary>
		/// <param name="cameras">An array to be filled up with cameras currently in the scene.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetAllCameras(Camera[] cameras);

		private static void FireOnPreCull(Camera cam)
		{
			if (Camera.onPreCull != null)
			{
				Camera.onPreCull(cam);
			}
		}

		private static void FireOnPreRender(Camera cam)
		{
			if (Camera.onPreRender != null)
			{
				Camera.onPreRender(cam);
			}
		}

		private static void FireOnPostRender(Camera cam)
		{
			if (Camera.onPostRender != null)
			{
				Camera.onPostRender(cam);
			}
		}

		/// <summary>
		///   <para>Render the camera manually.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Render();

		/// <summary>
		///   <para>Render the camera with shader replacement.</para>
		/// </summary>
		/// <param name="shader"></param>
		/// <param name="replacementTag"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RenderWithShader(Shader shader, string replacementTag);

		/// <summary>
		///   <para>Make the camera render with shader replacement.</para>
		/// </summary>
		/// <param name="shader"></param>
		/// <param name="replacementTag"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetReplacementShader(Shader shader, string replacementTag);

		/// <summary>
		///   <para>Remove shader replacement from camera.</para>
		/// </summary>
		public void ResetReplacementShader()
		{
			Camera.INTERNAL_CALL_ResetReplacementShader(this);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_ResetReplacementShader(Camera self);

		/// <summary>
		///   <para>Whether or not the Camera will use occlusion culling during rendering.</para>
		/// </summary>
		public extern bool useOcclusionCulling { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RenderDontRestore();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetupCurrent(Camera cur);

		[ExcludeFromDocs]
		public bool RenderToCubemap(Cubemap cubemap)
		{
			int faceMask = 63;
			return this.RenderToCubemap(cubemap, faceMask);
		}

		/// <summary>
		///   <para>Render into a static cubemap from this camera.</para>
		/// </summary>
		/// <param name="cubemap">The cube map to render to.</param>
		/// <param name="faceMask">A bitmask which determines which of the six faces are rendered to.</param>
		/// <returns>
		///   <para>False is rendering fails, else true.</para>
		/// </returns>
		public bool RenderToCubemap(Cubemap cubemap, [DefaultValue("63")] int faceMask)
		{
			return this.Internal_RenderToCubemapTexture(cubemap, faceMask);
		}

		[ExcludeFromDocs]
		public bool RenderToCubemap(RenderTexture cubemap)
		{
			int faceMask = 63;
			return this.RenderToCubemap(cubemap, faceMask);
		}

		/// <summary>
		///   <para>Render into a cubemap from this camera.</para>
		/// </summary>
		/// <param name="faceMask">A bitfield indicating which cubemap faces should be rendered into.</param>
		/// <param name="cubemap">The texture to render to.</param>
		/// <returns>
		///   <para>False is rendering fails, else true.</para>
		/// </returns>
		public bool RenderToCubemap(RenderTexture cubemap, [DefaultValue("63")] int faceMask)
		{
			return this.Internal_RenderToCubemapRT(cubemap, faceMask);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool Internal_RenderToCubemapRT(RenderTexture cubemap, int faceMask);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool Internal_RenderToCubemapTexture(Cubemap cubemap, int faceMask);

		/// <summary>
		///   <para>Per-layer culling distances.</para>
		/// </summary>
		public extern float[] layerCullDistances { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>How to perform per-layer culling for a Camera.</para>
		/// </summary>
		public extern bool layerCullSpherical { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Makes this camera's settings match other camera.</para>
		/// </summary>
		/// <param name="other"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void CopyFrom(Camera other);

		/// <summary>
		///   <para>How and if camera generates a depth texture.</para>
		/// </summary>
		public extern DepthTextureMode depthTextureMode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Should the camera clear the stencil buffer after the deferred light pass?</para>
		/// </summary>
		public extern bool clearStencilAfterLightingPass { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern bool IsFiltered(GameObject go);

		/// <summary>
		///   <para>Add a command buffer to be executed at a specified place.</para>
		/// </summary>
		/// <param name="evt">When to execute the command buffer during rendering.</param>
		/// <param name="buffer">The buffer to execute.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void AddCommandBuffer(CameraEvent evt, CommandBuffer buffer);

		/// <summary>
		///   <para>Remove command buffer from execution at a specified place.</para>
		/// </summary>
		/// <param name="evt">When to execute the command buffer during rendering.</param>
		/// <param name="buffer">The buffer to execute.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RemoveCommandBuffer(CameraEvent evt, CommandBuffer buffer);

		/// <summary>
		///   <para>Remove command buffers from execution at a specified place.</para>
		/// </summary>
		/// <param name="evt">When to execute the command buffer during rendering.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RemoveCommandBuffers(CameraEvent evt);

		/// <summary>
		///   <para>Remove all command buffers set on this camera.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RemoveAllCommandBuffers();

		/// <summary>
		///   <para>Get command buffers to be executed at a specified place.</para>
		/// </summary>
		/// <param name="evt">When to execute the command buffer during rendering.</param>
		/// <returns>
		///   <para>Array of command buffers.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern CommandBuffer[] GetCommandBuffers(CameraEvent evt);

		/// <summary>
		///   <para>Number of command buffers set up on this camera (Read Only).</para>
		/// </summary>
		public extern int commandBufferCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		internal GameObject RaycastTry(Ray ray, float distance, int layerMask, [DefaultValue("QueryTriggerInteraction.UseGlobal")] QueryTriggerInteraction queryTriggerInteraction)
		{
			return Camera.INTERNAL_CALL_RaycastTry(this, ref ray, distance, layerMask, queryTriggerInteraction);
		}

		[ExcludeFromDocs]
		internal GameObject RaycastTry(Ray ray, float distance, int layerMask)
		{
			QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
			return Camera.INTERNAL_CALL_RaycastTry(this, ref ray, distance, layerMask, queryTriggerInteraction);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern GameObject INTERNAL_CALL_RaycastTry(Camera self, ref Ray ray, float distance, int layerMask, QueryTriggerInteraction queryTriggerInteraction);

		internal GameObject RaycastTry2D(Ray ray, float distance, int layerMask)
		{
			return Camera.INTERNAL_CALL_RaycastTry2D(this, ref ray, distance, layerMask);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern GameObject INTERNAL_CALL_RaycastTry2D(Camera self, ref Ray ray, float distance, int layerMask);

		/// <summary>
		///   <para>Calculates and returns oblique near-plane projection matrix.</para>
		/// </summary>
		/// <param name="clipPlane">Vector4 that describes a clip plane.</param>
		/// <returns>
		///   <para>Oblique near-plane projection matrix.</para>
		/// </returns>
		public Matrix4x4 CalculateObliqueMatrix(Vector4 clipPlane)
		{
			return Camera.INTERNAL_CALL_CalculateObliqueMatrix(this, ref clipPlane);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Matrix4x4 INTERNAL_CALL_CalculateObliqueMatrix(Camera self, ref Vector4 clipPlane);

		internal void OnlyUsedForTesting1()
		{
		}

		internal void OnlyUsedForTesting2()
		{
		}

		/// <summary>
		///   <para>Delegate type for camera callbacks.</para>
		/// </summary>
		/// <param name="cam"></param>
		public delegate void CameraCallback(Camera cam);
	}
}
