using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>A text string displayed in a GUI.</para>
	/// </summary>
	public sealed class GUIText : GUIElement
	{
		/// <summary>
		///   <para>The text to display.</para>
		/// </summary>
		public extern string text { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The Material to use for rendering.</para>
		/// </summary>
		public extern Material material { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_GetPixelOffset(out Vector2 output);

		private void Internal_SetPixelOffset(Vector2 p)
		{
			GUIText.INTERNAL_CALL_Internal_SetPixelOffset(this, ref p);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Internal_SetPixelOffset(GUIText self, ref Vector2 p);

		/// <summary>
		///   <para>The pixel offset of the text.</para>
		/// </summary>
		public Vector2 pixelOffset
		{
			get
			{
				Vector2 result;
				this.Internal_GetPixelOffset(out result);
				return result;
			}
			set
			{
				this.Internal_SetPixelOffset(value);
			}
		}

		/// <summary>
		///   <para>The font used for the text.</para>
		/// </summary>
		public extern Font font { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The alignment of the text.</para>
		/// </summary>
		public extern TextAlignment alignment { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The anchor of the text.</para>
		/// </summary>
		public extern TextAnchor anchor { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The line spacing multiplier.</para>
		/// </summary>
		public extern float lineSpacing { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The tab width multiplier.</para>
		/// </summary>
		public extern float tabSize { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The font size to use (for dynamic fonts).</para>
		/// </summary>
		public extern int fontSize { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The font style to use (for dynamic fonts).</para>
		/// </summary>
		public extern FontStyle fontStyle { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Enable HTML-style tags for Text Formatting Markup.</para>
		/// </summary>
		public extern bool richText { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The color used to render the text.</para>
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
	}
}
