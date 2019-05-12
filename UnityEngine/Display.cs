using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Provides access to a display / screen for rendering operations. </para>
	/// </summary>
	public sealed class Display
	{
		internal IntPtr nativeDisplay;

		/// <summary>
		///   <para>The list of currently connected Displays. Contains at least one (main) display.</para>
		/// </summary>
		public static Display[] displays = new Display[]
		{
			new Display()
		};

		private static Display _mainDisplay = Display.displays[0];

		internal Display()
		{
			this.nativeDisplay = new IntPtr(0);
		}

		internal Display(IntPtr nativeDisplay)
		{
			this.nativeDisplay = nativeDisplay;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static Display()
		{
			Display.onDisplaysUpdated = null;
		}

		public static event Display.DisplaysUpdatedDelegate onDisplaysUpdated;

		/// <summary>
		///   <para>Rendering Width.</para>
		/// </summary>
		public int renderingWidth
		{
			get
			{
				int result = 0;
				int num = 0;
				Display.GetRenderingExtImpl(this.nativeDisplay, out result, out num);
				return result;
			}
		}

		/// <summary>
		///   <para>Rendering Height.</para>
		/// </summary>
		public int renderingHeight
		{
			get
			{
				int num = 0;
				int result = 0;
				Display.GetRenderingExtImpl(this.nativeDisplay, out num, out result);
				return result;
			}
		}

		/// <summary>
		///   <para>System Width.</para>
		/// </summary>
		public int systemWidth
		{
			get
			{
				int result = 0;
				int num = 0;
				Display.GetSystemExtImpl(this.nativeDisplay, out result, out num);
				return result;
			}
		}

		/// <summary>
		///   <para>System Height.</para>
		/// </summary>
		public int systemHeight
		{
			get
			{
				int num = 0;
				int result = 0;
				Display.GetSystemExtImpl(this.nativeDisplay, out num, out result);
				return result;
			}
		}

		/// <summary>
		///   <para>Color RenderBuffer.</para>
		/// </summary>
		public RenderBuffer colorBuffer
		{
			get
			{
				RenderBuffer result;
				RenderBuffer renderBuffer;
				Display.GetRenderingBuffersImpl(this.nativeDisplay, out result, out renderBuffer);
				return result;
			}
		}

		/// <summary>
		///   <para>Depth RenderBuffer.</para>
		/// </summary>
		public RenderBuffer depthBuffer
		{
			get
			{
				RenderBuffer renderBuffer;
				RenderBuffer result;
				Display.GetRenderingBuffersImpl(this.nativeDisplay, out renderBuffer, out result);
				return result;
			}
		}

		/// <summary>
		///   <para>Activate an external display. Eg. Secondary Monitors connected to the System.</para>
		/// </summary>
		public void Activate()
		{
			Display.ActivateDisplayImpl(this.nativeDisplay, 0, 0, 60);
		}

		/// <summary>
		///   <para>This overloaded function available for Windows allows specifying desired Window Width, Height and Refresh Rate.</para>
		/// </summary>
		/// <param name="width">Desired Width of the Window (for Windows only. On Linux and Mac uses Screen Width).</param>
		/// <param name="height">Desired Height of the Window (for Windows only. On Linux and Mac uses Screen Height).</param>
		/// <param name="refreshRate">Desired Refresh Rate.</param>
		public void Activate(int width, int height, int refreshRate)
		{
			Display.ActivateDisplayImpl(this.nativeDisplay, width, height, refreshRate);
		}

		/// <summary>
		///   <para>This Windows only function can be used to set Size and Position of the Screen when Multi-Display is enabled.</para>
		/// </summary>
		/// <param name="width">Change Window Width (Windows Only).</param>
		/// <param name="height">Change Window Height (Windows Only).</param>
		/// <param name="x">Change Window Position X (Windows Only).</param>
		/// <param name="y">Change Window Position Y (Windows Only).</param>
		public void SetParams(int width, int height, int x, int y)
		{
			Display.SetParamsImpl(this.nativeDisplay, width, height, x, y);
		}

		/// <summary>
		///   <para>Sets Rendering resolution for the display.</para>
		/// </summary>
		/// <param name="w">Rendering width.</param>
		/// <param name="h">Rendering height.</param>
		public void SetRenderingResolution(int w, int h)
		{
			Display.SetRenderingResolutionImpl(this.nativeDisplay, w, h);
		}

		/// <summary>
		///   <para>Check if MultiDisplayLicense is enabled.</para>
		/// </summary>
		public static bool MultiDisplayLicense()
		{
			return Display.MultiDisplayLicenseImpl();
		}

		/// <summary>
		///   <para>Query relative mouse coordinates.</para>
		/// </summary>
		/// <param name="inputMouseCoordinates">Mouse Input Position as Coordinates.</param>
		public static Vector3 RelativeMouseAt(Vector3 inputMouseCoordinates)
		{
			int num = 0;
			int num2 = 0;
			int x = (int)inputMouseCoordinates.x;
			int y = (int)inputMouseCoordinates.y;
			Vector3 result;
			result.z = (float)Display.RelativeMouseAtImpl(x, y, out num, out num2);
			result.x = (float)num;
			result.y = (float)num2;
			return result;
		}

		/// <summary>
		///   <para>Main Display.
		/// </para>
		/// </summary>
		public static Display main
		{
			get
			{
				return Display._mainDisplay;
			}
		}

		private static void RecreateDisplayList(IntPtr[] nativeDisplay)
		{
			Display.displays = new Display[nativeDisplay.Length];
			for (int i = 0; i < nativeDisplay.Length; i++)
			{
				Display.displays[i] = new Display(nativeDisplay[i]);
			}
			Display._mainDisplay = Display.displays[0];
		}

		private static void FireDisplaysUpdated()
		{
			if (Display.onDisplaysUpdated != null)
			{
				Display.onDisplaysUpdated();
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetSystemExtImpl(IntPtr nativeDisplay, out int w, out int h);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetRenderingExtImpl(IntPtr nativeDisplay, out int w, out int h);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetRenderingBuffersImpl(IntPtr nativeDisplay, out RenderBuffer color, out RenderBuffer depth);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetRenderingResolutionImpl(IntPtr nativeDisplay, int w, int h);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ActivateDisplayImpl(IntPtr nativeDisplay, int width, int height, int refreshRate);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetParamsImpl(IntPtr nativeDisplay, int width, int height, int x, int y);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool MultiDisplayLicenseImpl();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int RelativeMouseAtImpl(int x, int y, out int rx, out int ry);

		public delegate void DisplaysUpdatedDelegate();
	}
}
