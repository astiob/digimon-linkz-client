using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Element that can be used for screen rendering.</para>
	/// </summary>
	public sealed class Canvas : Behaviour
	{
		public static event Canvas.WillRenderCanvases willRenderCanvases;

		/// <summary>
		///   <para>Is the Canvas in World or Overlay mode?</para>
		/// </summary>
		public extern RenderMode renderMode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Is this the root Canvas?</para>
		/// </summary>
		public extern bool isRootCanvas { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Camera used for sizing the Canvas when in Screen Space - Camera. Also used as the Camera that events will be sent through for a World Space [[Canvas].</para>
		/// </summary>
		public extern Camera worldCamera { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Get the render rect for the Canvas.</para>
		/// </summary>
		public Rect pixelRect
		{
			get
			{
				Rect result;
				this.INTERNAL_get_pixelRect(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_pixelRect(out Rect value);

		/// <summary>
		///   <para>Used to scale the entire canvas, while still making it fit the screen. Only applies with renderMode is Screen Space.</para>
		/// </summary>
		public extern float scaleFactor { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The number of pixels per unit that is considered the default.</para>
		/// </summary>
		public extern float referencePixelsPerUnit { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Allows for nested canvases to override pixelPerfect settings inherited from parent canvases.</para>
		/// </summary>
		public extern bool overridePixelPerfect { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Force elements in the canvas to be aligned with pixels. Only applies with renderMode is Screen Space.</para>
		/// </summary>
		public extern bool pixelPerfect { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>How far away from the camera is the Canvas generated.</para>
		/// </summary>
		public extern float planeDistance { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The render order in which the canvas is being emitted to the scene.</para>
		/// </summary>
		public extern int renderOrder { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Override the sorting of canvas.</para>
		/// </summary>
		public extern bool overrideSorting { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Canvas' order within a sorting layer.</para>
		/// </summary>
		public extern int sortingOrder { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Unique ID of the Canvas' sorting layer.</para>
		/// </summary>
		public extern int sortingLayerID { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Cached calculated value based upon SortingLayerID.</para>
		/// </summary>
		public extern int cachedSortingLayerValue { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Name of the Canvas' sorting layer.</para>
		/// </summary>
		public extern string sortingLayerName { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Returns the default material that can be used for rendering normal elements on the Canvas.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Material GetDefaultCanvasMaterial();

		/// <summary>
		///   <para>Returns the default material that can be used for rendering text elements on the Canvas.</para>
		/// </summary>
		[WrapperlessIcall]
		[Obsolete("Shared default material now used for text and general UI elements, call Canvas.GetDefaultCanvasMaterial()")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Material GetDefaultCanvasTextMaterial();

		private static void SendWillRenderCanvases()
		{
			if (Canvas.willRenderCanvases != null)
			{
				Canvas.willRenderCanvases();
			}
		}

		/// <summary>
		///   <para>Force all canvases to update their content.</para>
		/// </summary>
		public static void ForceUpdateCanvases()
		{
			Canvas.SendWillRenderCanvases();
		}

		public delegate void WillRenderCanvases();
	}
}
