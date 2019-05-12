using System;

namespace UnityEngine.UI
{
	public interface ILayoutElement
	{
		void CalculateLayoutInputHorizontal();

		void CalculateLayoutInputVertical();

		float minWidth { get; }

		float preferredWidth { get; }

		float flexibleWidth { get; }

		float minHeight { get; }

		float preferredHeight { get; }

		float flexibleHeight { get; }

		int layoutPriority { get; }
	}
}
