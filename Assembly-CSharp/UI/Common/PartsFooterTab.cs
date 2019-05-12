using NGUI.Extensions;
using System;
using UnityEngine;

namespace UI.Common
{
	public sealed class PartsFooterTab : UIWidgetStretch, IUISafeAreaChildren
	{
		public void SetAnchorTarget(Transform safeArea)
		{
			UIWidget component = base.GetComponent<UIWidget>();
			component.leftAnchor.target = safeArea;
			component.leftAnchor.Set(0f, 0f);
			component.rightAnchor.target = safeArea;
			component.rightAnchor.Set(1f, 0f);
			component.topAnchor.target = safeArea;
			component.topAnchor.Set(0f, (float)component.height);
			component.bottomAnchor.target = safeArea;
			component.bottomAnchor.Set(0f, 0f);
			component.ResetAnchors();
		}
	}
}
