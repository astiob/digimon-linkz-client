using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>A texture image used in a 2D GUI.</para>
	/// </summary>
	public sealed class GUITexture : GUIElement
	{
		/// <summary>
		///   <para>The color of the GUI texture.</para>
		/// </summary>
		public Color color
		{
			get
			{
				Color result;
				this.INTERNAL_get_color(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_color(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_color(out Color value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_color(ref Color value);

		/// <summary>
		///   <para>The texture used for drawing.</para>
		/// </summary>
		public extern Texture texture { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Pixel inset used for pixel adjustments for size and position.</para>
		/// </summary>
		public Rect pixelInset
		{
			get
			{
				Rect result;
				this.INTERNAL_get_pixelInset(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_pixelInset(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_pixelInset(out Rect value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_pixelInset(ref Rect value);

		/// <summary>
		///   <para>The border defines the number of pixels from the edge that are not affected by scale.</para>
		/// </summary>
		public extern RectOffset border { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
