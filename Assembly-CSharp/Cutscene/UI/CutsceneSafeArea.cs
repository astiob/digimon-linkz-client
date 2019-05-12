using NGUI.Extensions;
using System;
using UnityEngine;

namespace Cutscene.UI
{
	public sealed class CutsceneSafeArea : MonoBehaviour
	{
		[SerializeField]
		private UIWidget cutsceneUIWidget;

		private void Awake()
		{
			UIRoot component = base.GetComponent<UIRoot>();
			UIRoot uiroot = GUIMain.GetUIRoot();
			if (null != uiroot)
			{
				component.manualWidth = uiroot.manualWidth;
				component.manualHeight = uiroot.manualHeight;
				if (null != this.cutsceneUIWidget)
				{
					UICoverSafeArea component2 = uiroot.GetComponent<UICoverSafeArea>();
					if (null != component2)
					{
						UICoverSafeArea.AnchorPoint anchorPoint = component2.GetAnchorPoint();
						this.cutsceneUIWidget.SetAnchor(base.gameObject, Mathf.CeilToInt(anchorPoint.left), Mathf.CeilToInt(anchorPoint.bottom), Mathf.CeilToInt(anchorPoint.right), Mathf.CeilToInt(anchorPoint.top));
						this.cutsceneUIWidget.updateAnchors = UIRect.AnchorUpdate.OnEnable;
					}
				}
			}
		}
	}
}
