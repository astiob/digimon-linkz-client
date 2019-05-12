using SwitchParts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Title
{
	public sealed class CMD_ConfirmGDPR : CMD
	{
		[SerializeField]
		private ConfirmGDPR_WebView webView;

		[SerializeField]
		private ButtonSwitch acceptSelectedButton;

		private ConfirmGDPR_Network network;

		private List<ConfirmGDPR_Rule> rules;

		private bool isCancel;

		public static CMD_ConfirmGDPR Create(ConfirmGDPR_Network network)
		{
			CMD_ConfirmGDPR cmd_ConfirmGDPR = null;
			GameWebAPI.ResponseGdprInfo.Details[] details = network.Details;
			List<ConfirmGDPR_Rule> list = new List<ConfirmGDPR_Rule>();
			for (int i = 0; i < details.Length; i++)
			{
				ConfirmGDPR_Network.GDPRWebPageType webPageType = network.GetWebPageType(details[i]);
				if (network.IsConfirmWebPage(webPageType))
				{
					list.Add(new ConfirmGDPR_Rule(webPageType, details[i].url));
				}
			}
			if (0 < list.Count)
			{
				cmd_ConfirmGDPR = (GUIMain.ShowCommonDialog(null, "CMD_ConfirmGDPR", null) as CMD_ConfirmGDPR);
				cmd_ConfirmGDPR.network = network;
				cmd_ConfirmGDPR.rules = list;
			}
			return cmd_ConfirmGDPR;
		}

		private void Start()
		{
			string webPageURL = this.network.GetWebPageURL(ConfirmGDPR_Network.GDPRWebPageType.TOP_PAGE);
			this.webView.SetRuleSetting(webPageURL, this.rules);
			this.SetActiveAcceptSelectedButton(false);
		}

		private void SetActiveAcceptSelectedButton(bool active)
		{
			this.acceptSelectedButton.Switch(active);
			GUICollider component = this.acceptSelectedButton.GetComponent<GUICollider>();
			if (null != component)
			{
				component.activeCollider = active;
			}
		}

		private void PushAdRejectAllButton()
		{
			ConfirmGDPR_RuleUI[] ruleSettingList = this.webView.GetRuleSettingList();
			Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
			for (int i = 0; i < ruleSettingList.Length; i++)
			{
				if (ruleSettingList[i].gameObject.activeSelf)
				{
					dictionary.Add(this.rules[i].adjustEventKey, false);
				}
			}
			this.SendAgreementFlag(dictionary);
		}

		private void PushAcceptSelectedButton()
		{
			if (this.acceptSelectedButton.Status())
			{
				ConfirmGDPR_RuleUI[] ruleSettingList = this.webView.GetRuleSettingList();
				Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
				for (int i = 0; i < ruleSettingList.Length; i++)
				{
					if (ruleSettingList[i].gameObject.activeSelf)
					{
						dictionary.Add(this.rules[i].adjustEventKey, ruleSettingList[i].GetCheckBoxStatus());
					}
				}
				this.SendAgreementFlag(dictionary);
			}
		}

		private void PushAcceptAllButton()
		{
			ConfirmGDPR_RuleUI[] ruleSettingList = this.webView.GetRuleSettingList();
			Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
			for (int i = 0; i < ruleSettingList.Length; i++)
			{
				if (ruleSettingList[i].gameObject.activeSelf)
				{
					dictionary.Add(this.rules[i].adjustEventKey, true);
				}
			}
			this.SendAgreementFlag(dictionary);
		}

		private void SendAgreementFlag(Dictionary<string, bool> agreements)
		{
			AdjustWrapper.Instance.TrackEventGDPR(agreements);
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			APIRequestTask requestConfirmed = this.network.GetRequestConfirmed();
			base.StartCoroutine(this.network.Send(requestConfirmed, new Action(this.OnSended), new Func<Exception, APIRequestTask, IEnumerator>(this.OnAlertSend)));
		}

		private void OnSended()
		{
			this.webView.CloseWebView();
			RestrictionInput.EndLoad();
			base.ClosePanel(true);
		}

		private IEnumerator OnAlertSend(Exception ex, APIRequestTask request)
		{
			this.webView.CloseWebView();
			return request.OnAlert(ex);
		}

		protected override void WindowOpened()
		{
			base.WindowOpened();
			this.webView.SetWebViewPositionOffset();
			this.webView.OpenSummaryWebPage();
		}

		public void UpdateAcceptSelectedButtonStatus()
		{
			bool activeAcceptSelectedButton = false;
			ConfirmGDPR_RuleUI[] ruleSettingList = this.webView.GetRuleSettingList();
			for (int i = 0; i < ruleSettingList.Length; i++)
			{
				if (ruleSettingList[i].gameObject.activeSelf && ruleSettingList[i].GetCheckBoxStatus())
				{
					activeAcceptSelectedButton = true;
					break;
				}
			}
			this.SetActiveAcceptSelectedButton(activeAcceptSelectedButton);
		}

		public override void ClosePanel(bool animation = true)
		{
			this.webView.CloseWebView();
			this.isCancel = true;
			base.ClosePanel(true);
		}

		public bool IsCancel()
		{
			return this.isCancel;
		}
	}
}
