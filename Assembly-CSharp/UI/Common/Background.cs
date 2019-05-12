using NGUI.Extensions;
using System;
using UnityEngine;

namespace UI.Common
{
	public sealed class Background : UIWidgetStretch
	{
		protected override bool SetLeftAnchor(UIWidget widget, UIPanel uiPanel)
		{
			widget.width = Mathf.CeilToInt(uiPanel.GetWindowSize().x);
			return true;
		}

		protected override bool SetRightAnchor(UIWidget widget, UIPanel uiPanel)
		{
			return true;
		}
	}
}
