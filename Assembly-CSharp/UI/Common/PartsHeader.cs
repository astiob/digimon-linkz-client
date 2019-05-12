using NGUI.Extensions;
using System;
using UnityEngine;

namespace UI.Common
{
	public sealed class PartsHeader : UIWidgetStretch, IUISafeAreaChildren
	{
		protected override bool SetLeftAnchor(UIWidget widget, UIPanel uiPanel)
		{
			if (this.stretchLeft.fit)
			{
				widget.width = Mathf.CeilToInt(uiPanel.GetWindowSize().x * 0.5f + 0.5f);
			}
			return true;
		}

		protected override bool SetRightAnchor(UIWidget widget, UIPanel uiPanel)
		{
			if (this.stretchRight.fit)
			{
				widget.width = Mathf.CeilToInt(uiPanel.GetWindowSize().x * 0.5f + 0.5f);
			}
			return true;
		}

		public void SetAnchorTarget(Transform safeArea)
		{
			UIWidget component = base.GetComponent<UIWidget>();
			component.topAnchor.target = safeArea;
			component.topAnchor.Set(1f, 0f);
			component.bottomAnchor.target = safeArea;
			component.bottomAnchor.Set(1f, (float)component.height * -1f);
			component.ResetAnchors();
		}
	}
}
