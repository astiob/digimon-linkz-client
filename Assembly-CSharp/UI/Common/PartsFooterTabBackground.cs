using NGUI.Extensions;
using System;
using UnityEngine;

namespace UI.Common
{
	public sealed class PartsFooterTabBackground : UIWidgetStretch
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

		protected override bool SetBottomAnchor(UIWidget widget, UIPanel uiPanel)
		{
			widget.bottomAnchor.target = uiPanel.transform;
			widget.bottomAnchor.Set(0f, -50f);
			return true;
		}

		protected override void SetAnchorExcute(UIWidget widget)
		{
			widget.updateAnchors = UIRect.AnchorUpdate.OnUpdate;
		}
	}
}
