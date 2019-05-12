using SwitchParts;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Title
{
	public class ConfirmGDPR_WebView : MonoBehaviour
	{
		[SerializeField]
		private CMD rootUI;

		[SerializeField]
		private Rect summaryWebViewRect;

		[SerializeField]
		private ConfirmGDPR_RuleUI[] ruleUis;

		[SerializeField]
		private TabSwitch[] tabSwitches;

		[SerializeField]
		private TabSwitch summaryTab;

		[SerializeField]
		private GameObject checkBoxObject;

		[SerializeField]
		private WebViewControl webViewControl;

		private string summaryWebURL;

		private void Start()
		{
			UIRoot uiroot = GUIMain.GetUIRoot();
			if (null != uiroot)
			{
				Vector3 localScale = uiroot.transform.localScale;
				this.webViewControl.SetScreenScale(localScale.x, localScale.y);
			}
			this.webViewControl.SetOrthoCamera(GUIMain.GetOrthoCamera());
		}

		private void PushSummaryButton()
		{
			if (!this.summaryTab.Status())
			{
				this.OpenSummaryWebPage();
			}
		}

		public void SetRuleSetting(string url, List<ConfirmGDPR_Rule> rules)
		{
			global::Debug.Assert(0 < this.ruleUis.Length, "Rule UI Listが未設定。Inspectorを確認してください。");
			this.summaryWebURL = url;
			int num = Mathf.Min(rules.Count, this.ruleUis.Length);
			for (int i = 0; i < num; i++)
			{
				this.ruleUis[i].SetWebView(this.webViewControl);
				this.ruleUis[i].SetURL(rules[i].url);
			}
			for (int j = num; j < this.ruleUis.Length; j++)
			{
				this.ruleUis[j].gameObject.SetActive(false);
			}
		}

		public void SetWebViewPositionOffset()
		{
			Vector3 localPosition = this.rootUI.transform.localPosition;
			this.webViewControl.SetWebViewPositionOffset(localPosition.x, localPosition.y);
		}

		public void OpenSummaryWebPage()
		{
			this.SetTabStatus(this.summaryTab);
			if (this.checkBoxObject.activeSelf)
			{
				this.checkBoxObject.SetActive(false);
			}
			this.webViewControl.SetWebViewRect(this.summaryWebViewRect.x, this.summaryWebViewRect.y, this.summaryWebViewRect.width, this.summaryWebViewRect.height);
			this.webViewControl.SetWebViewPosition();
			this.webViewControl.OpenWebView(this.summaryWebURL);
		}

		public ConfirmGDPR_RuleUI[] GetRuleSettingList()
		{
			return this.ruleUis;
		}

		public void SetTabStatus(TabSwitch activeTab)
		{
			activeTab.Switch(true);
			for (int i = 0; i < this.tabSwitches.Length; i++)
			{
				if (activeTab != this.tabSwitches[i])
				{
					this.tabSwitches[i].Switch(false);
				}
			}
		}

		public void CloseWebView()
		{
			this.webViewControl.DeleteWebView();
		}
	}
}
