using System;
using UnityEngine;

namespace NGUI.Extensions
{
	[RequireComponent(typeof(UIWidget))]
	public class UIWidgetStretch : MonoBehaviour
	{
		[SerializeField]
		protected UIWidgetStretch.StretchInfo stretchLeft = default(UIWidgetStretch.StretchInfo);

		[SerializeField]
		protected UIWidgetStretch.StretchInfo stretchRight = default(UIWidgetStretch.StretchInfo);

		[SerializeField]
		protected UIWidgetStretch.StretchInfo stretchTop = default(UIWidgetStretch.StretchInfo);

		[SerializeField]
		protected UIWidgetStretch.StretchInfo stretchBottom = default(UIWidgetStretch.StretchInfo);

		private void Awake()
		{
			UIWidget component = base.GetComponent<UIWidget>();
			if (null != component)
			{
				UICoverSafeArea uicoverSafeArea = UnityEngine.Object.FindObjectOfType<UICoverSafeArea>();
				if (null != uicoverSafeArea)
				{
					UIPanel component2 = uicoverSafeArea.GetComponent<UIPanel>();
					if (null != component2)
					{
						this.FitAnchorPoint(component, component2);
						this.SetAnchorExcute(component);
					}
				}
			}
		}

		private void Start()
		{
			this.OnStart();
		}

		private void FitAnchorPoint(UIWidget widget, UIPanel uiPanel)
		{
			bool flag = this.SetLeftAnchor(widget, uiPanel);
			bool flag2 = this.SetRightAnchor(widget, uiPanel);
			bool flag3 = this.SetTopAnchor(widget, uiPanel);
			bool flag4 = this.SetBottomAnchor(widget, uiPanel);
			if (flag || flag2 || flag3 || flag4)
			{
				widget.ResetAnchors();
				widget.UpdateAnchors();
			}
		}

		protected virtual bool SetLeftAnchor(UIWidget widget, UIPanel uiPanel)
		{
			bool result = false;
			if (this.stretchLeft.fit)
			{
				if (this.stretchLeft.useUIRootWidget)
				{
					widget.leftAnchor.target = uiPanel.transform;
					widget.leftAnchor.Set(0f, 0f);
				}
				else
				{
					widget.width = Mathf.CeilToInt(uiPanel.GetWindowSize().x);
				}
				result = true;
			}
			return result;
		}

		protected virtual bool SetRightAnchor(UIWidget widget, UIPanel uiPanel)
		{
			bool result = false;
			if (this.stretchRight.fit)
			{
				if (this.stretchRight.useUIRootWidget)
				{
					widget.rightAnchor.target = uiPanel.transform;
					widget.rightAnchor.Set(1f, 0f);
				}
				else
				{
					widget.width = Mathf.CeilToInt(uiPanel.GetWindowSize().x);
				}
				result = true;
			}
			return result;
		}

		protected virtual bool SetTopAnchor(UIWidget widget, UIPanel uiPanel)
		{
			bool result = false;
			if (this.stretchTop.fit)
			{
				if (this.stretchTop.useUIRootWidget)
				{
					widget.topAnchor.target = uiPanel.transform;
					widget.topAnchor.Set(1f, 0f);
				}
				else
				{
					widget.height = Mathf.CeilToInt(uiPanel.GetWindowSize().y);
				}
				result = true;
			}
			return result;
		}

		protected virtual bool SetBottomAnchor(UIWidget widget, UIPanel uiPanel)
		{
			bool result = false;
			if (this.stretchBottom.fit)
			{
				if (this.stretchBottom.useUIRootWidget)
				{
					widget.bottomAnchor.target = uiPanel.transform;
					widget.bottomAnchor.Set(0f, 0f);
				}
				else
				{
					widget.height = Mathf.CeilToInt(uiPanel.GetWindowSize().y);
				}
				result = true;
			}
			return result;
		}

		protected virtual void SetAnchorExcute(UIWidget widget)
		{
			widget.updateAnchors = UIRect.AnchorUpdate.OnStart;
		}

		protected virtual void OnStart()
		{
		}

		[Serializable]
		protected struct StretchInfo
		{
			public bool fit;

			public bool useUIRootWidget;
		}
	}
}
