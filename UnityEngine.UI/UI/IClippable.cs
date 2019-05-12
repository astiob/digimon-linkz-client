using System;

namespace UnityEngine.UI
{
	public interface IClippable
	{
		void RecalculateClipping();

		RectTransform rectTransform { get; }

		void Cull(Rect clipRect, bool validRect);

		void SetClipRect(Rect value, bool validRect);
	}
}
