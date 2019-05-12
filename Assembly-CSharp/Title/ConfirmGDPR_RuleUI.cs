using SwitchParts;
using System;
using UnityEngine;

namespace Title
{
	public sealed class ConfirmGDPR_RuleUI : MonoBehaviour
	{
		[SerializeField]
		private CMD_ConfirmGDPR rootUI;

		[SerializeField]
		private ConfirmGDPR_WebView parentUI;

		[SerializeField]
		private TabSwitch tab;

		[SerializeField]
		private ConfirmGDPR_CheckAgree checkBox;

		[SerializeField]
		private Rect webViewRect;

		private bool checkBoxStatus;

		private string webURL;

		private WebViewControl webViewControl;

		private void PushCheckBox(bool enable)
		{
			this.checkBoxStatus = enable;
			this.rootUI.UpdateAcceptSelectedButtonStatus();
		}

		private void PushTab()
		{
			if (!this.tab.Status())
			{
				this.parentUI.SetTabStatus(this.tab);
				if (!this.checkBox.gameObject.activeSelf)
				{
					this.checkBox.gameObject.SetActive(true);
				}
				this.checkBox.SetPushAction(new Action<bool>(this.PushCheckBox));
				this.checkBox.SetCheckBoxStatus(this.checkBoxStatus);
				this.webViewControl.SetWebViewRect(this.webViewRect.x, this.webViewRect.y, this.webViewRect.width, this.webViewRect.height);
				this.webViewControl.SetWebViewPosition();
				this.webViewControl.OpenWebView(this.webURL);
			}
		}

		public void SetWebView(WebViewControl control)
		{
			this.webViewControl = control;
		}

		public void SetURL(string url)
		{
			this.webURL = url;
		}

		public bool GetCheckBoxStatus()
		{
			return this.checkBoxStatus;
		}
	}
}
