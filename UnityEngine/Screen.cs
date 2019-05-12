using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	/// <summary>
	///   <para>Access to display information.</para>
	/// </summary>
	public sealed class Screen
	{
		/// <summary>
		///   <para>All fullscreen resolutions supported by the monitor (Read Only).</para>
		/// </summary>
		public static extern Resolution[] resolutions { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Should the cursor be locked?</para>
		/// </summary>
		[Obsolete("Property lockCursor has been deprecated. Use Cursor.lockState and Cursor.visible instead.")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool lockCursor
		{
			get
			{
				return CursorLockMode.None == Cursor.lockState;
			}
			set
			{
				if (value)
				{
					Cursor.visible = false;
					Cursor.lockState = CursorLockMode.Locked;
				}
				else
				{
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;
				}
			}
		}

		/// <summary>
		///   <para>The current screen resolution (Read Only).</para>
		/// </summary>
		public static extern Resolution currentResolution { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Switches the screen resolution.</para>
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="fullscreen"></param>
		/// <param name="preferredRefreshRate"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetResolution(int width, int height, bool fullscreen, [UnityEngine.Internal.DefaultValue("0")] int preferredRefreshRate);

		/// <summary>
		///   <para>Switches the screen resolution.</para>
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="fullscreen"></param>
		/// <param name="preferredRefreshRate"></param>
		[ExcludeFromDocs]
		public static void SetResolution(int width, int height, bool fullscreen)
		{
			int preferredRefreshRate = 0;
			Screen.SetResolution(width, height, fullscreen, preferredRefreshRate);
		}

		/// <summary>
		///   <para>The current width of the screen window in pixels (Read Only).</para>
		/// </summary>
		public static extern int width { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The current height of the screen window in pixels (Read Only).</para>
		/// </summary>
		public static extern int height { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The current DPI of the screen / device (Read Only).</para>
		/// </summary>
		public static extern float dpi { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Is the game running fullscreen?</para>
		/// </summary>
		public static extern bool fullScreen { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Allow auto-rotation to portrait?</para>
		/// </summary>
		public static extern bool autorotateToPortrait { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Allow auto-rotation to portrait, upside down?</para>
		/// </summary>
		public static extern bool autorotateToPortraitUpsideDown { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Allow auto-rotation to landscape left?</para>
		/// </summary>
		public static extern bool autorotateToLandscapeLeft { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Allow auto-rotation to landscape right?</para>
		/// </summary>
		public static extern bool autorotateToLandscapeRight { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Specifies logical orientation of the screen.</para>
		/// </summary>
		public static extern ScreenOrientation orientation { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>A power saving setting, allowing the screen to dim some time after the last active user interaction.</para>
		/// </summary>
		public static extern int sleepTimeout { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
