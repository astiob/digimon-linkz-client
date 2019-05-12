using Master;
using System;
using TutorialRequestHeader;
using UnityEngine;

public class CMD_AgreementConsent : CMD
{
	[SerializeField]
	protected UILabel titleLabel;

	[SerializeField]
	protected UILabel infoLabel;

	[SerializeField]
	protected UILabel ruleButtonLabel;

	[SerializeField]
	protected UILabel yesButtonLabel;

	[SerializeField]
	protected UILabel noButtonLabel;

	[SerializeField]
	protected GameObject ruleButtonObject;

	[SerializeField]
	protected GameObject multiLanguageRuleButtonObject;

	private Action<bool> actionClosed;

	private void Start()
	{
		this.titleLabel.text = StringMaster.GetString("AgreementTitle");
		this.infoLabel.text = StringMaster.GetString("AgreementInfo");
		this.ruleButtonLabel.text = StringMaster.GetString("AgreementTitle");
		this.yesButtonLabel.text = StringMaster.GetString("AgreementYesButtonText");
		this.noButtonLabel.text = StringMaster.GetString("AgreementNoButtonText");
		if (this.ruleButtonObject != null)
		{
			this.ruleButtonObject.SetActive(true);
		}
		if (this.multiLanguageRuleButtonObject != null)
		{
			this.multiLanguageRuleButtonObject.SetActive(false);
		}
	}

	public void SetActionAgreementPopupClosed(Action<bool> action)
	{
		this.actionClosed = action;
	}

	protected void Close(bool agreement)
	{
		if (this.actionClosed != null)
		{
			this.actionClosed(agreement);
			this.actionClosed = null;
		}
		base.ClosePanel(true);
	}

	public override void ClosePanel(bool animation = true)
	{
		this.Close(false);
	}

	private void OnAgreementNG()
	{
		this.Close(false);
	}

	protected virtual void OnAgreementOK()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
		GameWebAPI.RequestUS_UserUpdatePolicy request = new GameWebAPI.RequestUS_UserUpdatePolicy();
		APIRequestTask apirequestTask = new APIRequestTask(request, true);
		GameWebAPI.RespDataCM_Login.TutorialStatus tutorialStatus = DataMng.Instance().RespDataCM_Login.tutorialStatus;
		if ("0" == tutorialStatus.endFlg && "0" == tutorialStatus.statusId)
		{
			TutorialStatusSave tutorialStatusSave = new TutorialStatusSave();
			tutorialStatusSave.SetSendData = delegate(TutorialStatusSaveQuery param)
			{
				param.statusId = 10;
			};
			TutorialStatusSave request2 = tutorialStatusSave;
			apirequestTask.Add(new APIRequestTask(request2, true));
		}
		base.StartCoroutine(apirequestTask.Run(delegate
		{
			RestrictionInput.EndLoad();
			this.Close(true);
		}, null, null));
	}

	private void OnClickedTermsOfUse()
	{
		CMDWebWindow cmdwebWindow = GUIMain.ShowCommonDialog(null, "CMDWebWindow", null) as CMDWebWindow;
		cmdwebWindow.TitleText = StringMaster.GetString("AgreementTitle");
		cmdwebWindow.Url = WebAddress.EXT_ADR_AGREE;
	}

	private void OnClickedPrivacyOfUse()
	{
	}
}
