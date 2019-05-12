using NGUI.Extensions;
using System;
using UnityEngine;

namespace UI.Common
{
	public sealed class PartsFooterTabTopParts : UIWidgetStretch
	{
		protected override bool SetBottomAnchor(UIWidget widget, UIPanel uiPanel)
		{
			widget.height = Mathf.CeilToInt(uiPanel.GetWindowSize().x * 0.5f + 0.5f);
			return true;
		}
	}
}
