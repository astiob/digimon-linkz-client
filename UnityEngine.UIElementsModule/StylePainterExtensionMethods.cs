using System;

namespace UnityEngine.Experimental.UIElements
{
	internal static class StylePainterExtensionMethods
	{
		internal static TextureStylePainterParameters GetDefaultTextureParameters(this IStylePainter painter, VisualElement ve)
		{
			IStyle style = ve.style;
			return new TextureStylePainterParameters
			{
				layout = ve.layout,
				color = Color.white,
				texture = style.backgroundImage,
				scaleMode = style.backgroundSize,
				borderLeftWidth = style.borderLeftWidth,
				borderTopWidth = style.borderTopWidth,
				borderRightWidth = style.borderRightWidth,
				borderBottomWidth = style.borderBottomWidth,
				borderTopLeftRadius = style.borderTopLeftRadius,
				borderTopRightRadius = style.borderTopRightRadius,
				borderBottomRightRadius = style.borderBottomRightRadius,
				borderBottomLeftRadius = style.borderBottomLeftRadius,
				sliceLeft = style.sliceLeft,
				sliceTop = style.sliceTop,
				sliceRight = style.sliceRight,
				sliceBottom = style.sliceBottom
			};
		}

		internal static RectStylePainterParameters GetDefaultRectParameters(this IStylePainter painter, VisualElement ve)
		{
			IStyle style = ve.style;
			return new RectStylePainterParameters
			{
				layout = ve.layout,
				color = style.backgroundColor,
				borderLeftWidth = style.borderLeftWidth,
				borderTopWidth = style.borderTopWidth,
				borderRightWidth = style.borderRightWidth,
				borderBottomWidth = style.borderBottomWidth,
				borderTopLeftRadius = style.borderTopLeftRadius,
				borderTopRightRadius = style.borderTopRightRadius,
				borderBottomRightRadius = style.borderBottomRightRadius,
				borderBottomLeftRadius = style.borderBottomLeftRadius
			};
		}

		internal static TextStylePainterParameters GetDefaultTextParameters(this IStylePainter painter, VisualElement ve)
		{
			IStyle style = ve.style;
			return new TextStylePainterParameters
			{
				layout = ve.contentRect,
				text = ve.text,
				font = style.font,
				fontSize = style.fontSize,
				fontStyle = style.fontStyle,
				fontColor = style.textColor.GetSpecifiedValueOrDefault(Color.black),
				anchor = style.textAlignment,
				wordWrap = style.wordWrap,
				wordWrapWidth = ((!style.wordWrap) ? 0f : ve.contentRect.width),
				richText = false,
				clipping = style.textClipping
			};
		}

		internal static CursorPositionStylePainterParameters GetDefaultCursorPositionParameters(this IStylePainter painter, VisualElement ve)
		{
			IStyle style = ve.style;
			return new CursorPositionStylePainterParameters
			{
				layout = ve.contentRect,
				text = ve.text,
				font = style.font,
				fontSize = style.fontSize,
				fontStyle = style.fontStyle,
				anchor = style.textAlignment,
				wordWrapWidth = ((!style.wordWrap) ? 0f : ve.contentRect.width),
				richText = false,
				cursorIndex = 0
			};
		}

		internal static void DrawBackground(this IStylePainter painter, VisualElement ve)
		{
			IStyle style = ve.style;
			if (style.backgroundColor != Color.clear)
			{
				RectStylePainterParameters defaultRectParameters = painter.GetDefaultRectParameters(ve);
				defaultRectParameters.borderLeftWidth = 0f;
				defaultRectParameters.borderTopWidth = 0f;
				defaultRectParameters.borderRightWidth = 0f;
				defaultRectParameters.borderBottomWidth = 0f;
				painter.DrawRect(defaultRectParameters);
			}
			if (style.backgroundImage.value != null)
			{
				TextureStylePainterParameters defaultTextureParameters = painter.GetDefaultTextureParameters(ve);
				defaultTextureParameters.borderLeftWidth = 0f;
				defaultTextureParameters.borderTopWidth = 0f;
				defaultTextureParameters.borderRightWidth = 0f;
				defaultTextureParameters.borderBottomWidth = 0f;
				painter.DrawTexture(defaultTextureParameters);
			}
		}

		internal static void DrawBorder(this IStylePainter painter, VisualElement ve)
		{
			IStyle style = ve.style;
			if (style.borderColor != Color.clear && (style.borderLeftWidth > 0f || style.borderTopWidth > 0f || style.borderRightWidth > 0f || style.borderBottomWidth > 0f))
			{
				RectStylePainterParameters defaultRectParameters = painter.GetDefaultRectParameters(ve);
				defaultRectParameters.color = style.borderColor;
				painter.DrawRect(defaultRectParameters);
			}
		}

		internal static void DrawText(this IStylePainter painter, VisualElement ve)
		{
			if (!string.IsNullOrEmpty(ve.text) && ve.contentRect.width > 0f && ve.contentRect.height > 0f)
			{
				painter.DrawText(painter.GetDefaultTextParameters(ve));
			}
		}
	}
}
