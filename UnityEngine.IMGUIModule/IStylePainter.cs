using System;

namespace UnityEngine
{
	internal interface IStylePainter
	{
		void DrawRect(RectStylePainterParameters painterParams);

		void DrawTexture(TextureStylePainterParameters painterParams);

		void DrawText(TextStylePainterParameters painterParams);

		Vector2 GetCursorPosition(CursorPositionStylePainterParameters painterParams);

		Rect currentWorldClip { get; set; }

		Vector2 mousePosition { get; set; }

		Matrix4x4 currentTransform { get; set; }

		Event repaintEvent { get; set; }

		float opacity { get; set; }

		float ComputeTextWidth(TextStylePainterParameters painterParams);

		float ComputeTextHeight(TextStylePainterParameters painterParams);
	}
}
