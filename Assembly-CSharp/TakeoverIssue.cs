using Master;
using System;
using System.Collections;
using UnityEngine;

public class TakeoverIssue : MonoBehaviour
{
	[SerializeField]
	private UILabel takeoverCodeLabel;

	[SerializeField]
	private UILabel expireTimeLabel;

	[SerializeField]
	private UILabel userCodeLabel;

	[SerializeField]
	private UISprite userCodeCopyButtonSprite;

	[SerializeField]
	private UILabel userCodeCopyButtonLabel;

	[SerializeField]
	private BoxCollider userCodeCopyButtonCollider;

	[SerializeField]
	private UISprite takeoverCodeCopyButtonSprite;

	[SerializeField]
	private UILabel takeoverCodeCopyButtonLabel;

	[SerializeField]
	private BoxCollider takeoverCodeCopyButtonCollider;

	[SerializeField]
	private UISprite issueButtonSprite;

	[SerializeField]
	private UILabel issueButtonLabel;

	[SerializeField]
	private BoxCollider issueButtonCollider;

	[SerializeField]
	private UILabel titleLabel;

	[SerializeField]
	private UILabel topLabel;

	[SerializeField]
	private UILabel underLabel;

	[SerializeField]
	private UILabel issueCodeLabel;

	[SerializeField]
	private UILabel userCodeText;

	[SerializeField]
	private UILabel takeOverCodeLabel;

	[SerializeField]
	private UILabel[] copyLabels;

	public void Initialize()
	{
		this.titleLabel.text = StringMaster.GetString("TakeOver-17");
		this.topLabel.text = StringMaster.GetString("TakeOver-24");
		this.underLabel.text = StringMaster.GetString("TakeOver-05");
		this.issueCodeLabel.text = StringMaster.GetString("TakeOver-07");
		this.userCodeText.text = StringMaster.GetString("TakeOver-03");
		this.takeOverCodeLabel.text = StringMaster.GetString("TakeOver-04");
		foreach (UILabel uilabel in this.copyLabels)
		{
			uilabel.text = StringMaster.GetString("TakeOver-26");
		}
		this.UserCodeCopyButtonSetActive(false);
		this.TakeoverCopyButtonSetActive(false);
		this.IssueButtonSetActive(true);
	}

	private void DisplayTakeoverCode()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
		base.StartCoroutine(this.IssueTakeoverCode(delegate(string TakeoverCode, string ExpireTime)
		{
			this.takeoverCodeLabel.text = TakeoverCode;
			this.expireTimeLabel.text = StringMaster.GetString("TakeOver-08") + ExpireTime;
			this.userCodeLabel.text = DataMng.Instance().RespDataCM_Login.playerInfo.userCode;
			this.UserCodeCopyButtonSetActive(true);
			this.TakeoverCopyButtonSetActive(true);
			this.IssueButtonSetActive(false);
			RestrictionInput.EndLoad();
		}));
	}

	private IEnumerator IssueTakeoverCode(Action<string, string> OnIssuedResult)
	{
		GameWebAPI.Request_CM_TakeoverIssue request = new GameWebAPI.Request_CM_TakeoverIssue
		{
			OnReceived = delegate(GameWebAPI.RespDataCM_TakeoverIssue response)
			{
				OnIssuedResult(response.transferCode, response.expireTime);
			}
		};
		return request.Run(null, null, null);
	}

	private void CopyTakeoverCodeToClipboard()
	{
		Clipboard.Text = this.takeoverCodeLabel.text;
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
		cmd_ModalMessage.Title = StringMaster.GetString("SystemCopy");
		cmd_ModalMessage.Info = StringMaster.GetString("TakeOver-09");
	}

	private void CopyUserCodeToClipboard()
	{
		Clipboard.Text = this.userCodeLabel.text;
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
		cmd_ModalMessage.Title = StringMaster.GetString("SystemCopy");
		cmd_ModalMessage.Info = StringMaster.GetString("MyProfile-06");
	}

	private void UserCodeCopyButtonSetActive(bool IsEnable)
	{
		this.userCodeCopyButtonSprite.spriteName = ((!IsEnable) ? "Common02_Btn_BaseG" : "Common02_Btn_BaseON");
		this.userCodeCopyButtonLabel.color = ((!IsEnable) ? Color.gray : Color.white);
		this.userCodeCopyButtonCollider.enabled = IsEnable;
	}

	private void TakeoverCopyButtonSetActive(bool IsEnable)
	{
		this.takeoverCodeCopyButtonSprite.spriteName = ((!IsEnable) ? "Common02_Btn_BaseG" : "Common02_Btn_BaseON");
		this.takeoverCodeCopyButtonLabel.color = ((!IsEnable) ? Color.gray : Color.white);
		this.takeoverCodeCopyButtonCollider.enabled = IsEnable;
	}

	private void IssueButtonSetActive(bool IsEnable)
	{
		this.issueButtonSprite.spriteName = ((!IsEnable) ? "Common02_Btn_BaseG" : "Common02_Btn_BaseON");
		this.issueButtonLabel.color = ((!IsEnable) ? Color.gray : Color.white);
		this.issueButtonCollider.enabled = IsEnable;
	}

	public void OnClickedCopyUserCode()
	{
		if (!string.IsNullOrEmpty(this.userCodeLabel.text))
		{
			this.CopyUserCodeToClipboard();
		}
	}

	public void OnClickedInssueButton()
	{
		this.DisplayTakeoverCode();
	}

	public void OnClickedCopyTakeoverCode()
	{
		if (!string.IsNullOrEmpty(this.takeoverCodeLabel.text))
		{
			this.CopyTakeoverCodeToClipboard();
		}
	}
}
