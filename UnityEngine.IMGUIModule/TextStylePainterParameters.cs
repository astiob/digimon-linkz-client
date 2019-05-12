using System;

namespace UnityEngine
{
	internal struct TextStylePainterParameters
	{
		public Rect layout;

		public string text;

		public Font font;

		public int fontSize;

		public FontStyle fontStyle;

		public Color fontColor;

		public TextAnchor anchor;

		public bool wordWrap;

		public float wordWrapWidth;

		public bool richText;

		public TextClipping clipping;
	}
}
