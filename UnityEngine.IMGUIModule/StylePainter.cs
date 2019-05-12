using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[UsedByNativeCode]
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class StylePainter : IStylePainter
	{
		[NonSerialized]
		internal IntPtr m_Ptr;

		private Color m_OpacityColor = Color.white;

		public StylePainter()
		{
			this.Init();
		}

		public StylePainter(Vector2 pos) : this()
		{
			this.mousePosition = pos;
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Init();

		internal void DrawRect_Internal(Rect screenRect, Color color, Vector4 borderWidths, Vector4 borderRadiuses)
		{
			StylePainter.INTERNAL_CALL_DrawRect_Internal(this, ref screenRect, ref color, ref borderWidths, ref borderRadiuses);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_DrawRect_Internal(StylePainter self, ref Rect screenRect, ref Color color, ref Vector4 borderWidths, ref Vector4 borderRadiuses);

		internal void DrawTexture_Internal(Rect screenRect, Texture texture, Rect sourceRect, Color color, Vector4 borderWidths, Vector4 borderRadiuses, int leftBorder, int topBorder, int rightBorder, int bottomBorder)
		{
			StylePainter.INTERNAL_CALL_DrawTexture_Internal(this, ref screenRect, texture, ref sourceRect, ref color, ref borderWidths, ref borderRadiuses, leftBorder, topBorder, rightBorder, bottomBorder);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_DrawTexture_Internal(StylePainter self, ref Rect screenRect, Texture texture, ref Rect sourceRect, ref Color color, ref Vector4 borderWidths, ref Vector4 borderRadiuses, int leftBorder, int topBorder, int rightBorder, int bottomBorder);

		internal void DrawText_Internal(Rect screenRect, string text, Font font, int fontSize, FontStyle fontStyle, Color fontColor, TextAnchor anchor, bool wordWrap, float wordWrapWidth, bool richText, TextClipping textClipping)
		{
			StylePainter.INTERNAL_CALL_DrawText_Internal(this, ref screenRect, text, font, fontSize, fontStyle, ref fontColor, anchor, wordWrap, wordWrapWidth, richText, textClipping);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_DrawText_Internal(StylePainter self, ref Rect screenRect, string text, Font font, int fontSize, FontStyle fontStyle, ref Color fontColor, TextAnchor anchor, bool wordWrap, float wordWrapWidth, bool richText, TextClipping textClipping);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float ComputeTextWidth_Internal(string text, float width, bool wordWrap, Font font, int fontSize, FontStyle fontStyle, TextAnchor anchor, bool richText);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float ComputeTextHeight_Internal(string text, float width, bool wordWrap, Font font, int fontSize, FontStyle fontStyle, TextAnchor anchor, bool richText);

		public Vector2 GetCursorPosition_Internal(string text, Font font, int fontSize, FontStyle fontStyle, TextAnchor anchor, float wordWrapWidth, bool richText, Rect screenRect, int cursorPosition)
		{
			Vector2 result;
			StylePainter.INTERNAL_CALL_GetCursorPosition_Internal(this, text, font, fontSize, fontStyle, anchor, wordWrapWidth, richText, ref screenRect, cursorPosition, out result);
			return result;
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_GetCursorPosition_Internal(StylePainter self, string text, Font font, int fontSize, FontStyle fontStyle, TextAnchor anchor, float wordWrapWidth, bool richText, ref Rect screenRect, int cursorPosition, out Vector2 value);

		public void DrawRect(RectStylePainterParameters painterParams)
		{
			Rect layout = painterParams.layout;
			Color color = painterParams.color;
			Vector4 borderWidths = new Vector4(painterParams.borderLeftWidth, painterParams.borderTopWidth, painterParams.borderRightWidth, painterParams.borderBottomWidth);
			Vector4 borderRadiuses = new Vector4(painterParams.borderTopLeftRadius, painterParams.borderTopRightRadius, painterParams.borderBottomRightRadius, painterParams.borderBottomLeftRadius);
			this.DrawRect_Internal(layout, color * this.m_OpacityColor, borderWidths, borderRadiuses);
		}

		public void DrawTexture(TextureStylePainterParameters painterParams)
		{
			Rect layout = painterParams.layout;
			Texture texture = painterParams.texture;
			Color color = painterParams.color;
			ScaleMode scaleMode = painterParams.scaleMode;
			int sliceLeft = painterParams.sliceLeft;
			int sliceTop = painterParams.sliceTop;
			int sliceRight = painterParams.sliceRight;
			int sliceBottom = painterParams.sliceBottom;
			Rect screenRect = layout;
			Rect sourceRect = new Rect(0f, 0f, 1f, 1f);
			float num = (float)texture.width / (float)texture.height;
			float num2 = layout.width / layout.height;
			if (scaleMode != ScaleMode.StretchToFill)
			{
				if (scaleMode != ScaleMode.ScaleAndCrop)
				{
					if (scaleMode == ScaleMode.ScaleToFit)
					{
						if (num2 > num)
						{
							float num3 = num / num2;
							screenRect = new Rect(layout.xMin + layout.width * (1f - num3) * 0.5f, layout.yMin, num3 * layout.width, layout.height);
						}
						else
						{
							float num4 = num2 / num;
							screenRect = new Rect(layout.xMin, layout.yMin + layout.height * (1f - num4) * 0.5f, layout.width, num4 * layout.height);
						}
					}
				}
				else if (num2 > num)
				{
					float num5 = num / num2;
					sourceRect = new Rect(0f, (1f - num5) * 0.5f, 1f, num5);
				}
				else
				{
					float num6 = num2 / num;
					sourceRect = new Rect(0.5f - num6 * 0.5f, 0f, num6, 1f);
				}
			}
			Vector4 borderWidths = new Vector4(painterParams.borderLeftWidth, painterParams.borderTopWidth, painterParams.borderRightWidth, painterParams.borderBottomWidth);
			Vector4 borderRadiuses = new Vector4(painterParams.borderTopLeftRadius, painterParams.borderTopRightRadius, painterParams.borderBottomRightRadius, painterParams.borderBottomLeftRadius);
			this.DrawTexture_Internal(screenRect, texture, sourceRect, color * this.m_OpacityColor, borderWidths, borderRadiuses, sliceLeft, sliceTop, sliceRight, sliceBottom);
		}

		public void DrawText(TextStylePainterParameters painterParams)
		{
			Rect layout = painterParams.layout;
			string text = painterParams.text;
			Font font = painterParams.font;
			int fontSize = painterParams.fontSize;
			FontStyle fontStyle = painterParams.fontStyle;
			Color fontColor = painterParams.fontColor;
			TextAnchor anchor = painterParams.anchor;
			bool wordWrap = painterParams.wordWrap;
			float wordWrapWidth = painterParams.wordWrapWidth;
			bool richText = painterParams.richText;
			TextClipping clipping = painterParams.clipping;
			this.DrawText_Internal(layout, text, font, fontSize, fontStyle, fontColor * this.m_OpacityColor, anchor, wordWrap, wordWrapWidth, richText, clipping);
		}

		public Vector2 GetCursorPosition(CursorPositionStylePainterParameters painterParams)
		{
			Font font = painterParams.font;
			Vector2 result;
			if (font == null)
			{
				Debug.LogError("StylePainter: Can't process a null font.");
				result = Vector2.zero;
			}
			else
			{
				string text = painterParams.text;
				int fontSize = painterParams.fontSize;
				FontStyle fontStyle = painterParams.fontStyle;
				TextAnchor anchor = painterParams.anchor;
				float wordWrapWidth = painterParams.wordWrapWidth;
				bool richText = painterParams.richText;
				Rect layout = painterParams.layout;
				int cursorIndex = painterParams.cursorIndex;
				result = this.GetCursorPosition_Internal(text, font, fontSize, fontStyle, anchor, wordWrapWidth, richText, layout, cursorIndex);
			}
			return result;
		}

		public float ComputeTextWidth(TextStylePainterParameters painterParams)
		{
			string text = painterParams.text;
			float wordWrapWidth = painterParams.wordWrapWidth;
			bool wordWrap = painterParams.wordWrap;
			Font font = painterParams.font;
			int fontSize = painterParams.fontSize;
			FontStyle fontStyle = painterParams.fontStyle;
			TextAnchor anchor = painterParams.anchor;
			bool richText = painterParams.richText;
			return this.ComputeTextWidth_Internal(text, wordWrapWidth, wordWrap, font, fontSize, fontStyle, anchor, richText);
		}

		public float ComputeTextHeight(TextStylePainterParameters painterParams)
		{
			string text = painterParams.text;
			float wordWrapWidth = painterParams.wordWrapWidth;
			bool wordWrap = painterParams.wordWrap;
			Font font = painterParams.font;
			int fontSize = painterParams.fontSize;
			FontStyle fontStyle = painterParams.fontStyle;
			TextAnchor anchor = painterParams.anchor;
			bool richText = painterParams.richText;
			return this.ComputeTextHeight_Internal(text, wordWrapWidth, wordWrap, font, fontSize, fontStyle, anchor, richText);
		}

		public Matrix4x4 currentTransform { get; set; }

		public Vector2 mousePosition { get; set; }

		public Rect currentWorldClip { get; set; }

		public Event repaintEvent { get; set; }

		public float opacity
		{
			get
			{
				return this.m_OpacityColor.a;
			}
			set
			{
				this.m_OpacityColor.a = value;
			}
		}
	}
}
