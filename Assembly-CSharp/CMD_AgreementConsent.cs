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

	private Action<bool> actionClosed;

	private void Start()
	{
		this.titleLabel.text = StringMaster.GetString("AgreementTitle");
		this.infoLabel.text = StringMaster.GetString("AgreementInfo");
		this.ruleButtonLabel.text = StringMaster.GetString("AgreementTitle");
		this.yesButtonLabel.text = StringMaster.GetString("AgreementYesButtonText");
		this.noButtonLabel.text = StringMaster.GetString("AgreementNoButtonText");
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

	private void OnAgreementNG()
	{
		this.Close(false);
	}

	protected virtual void OnAgreementOK()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
		TutorialStatusSave tutorialStatusSave = new TutorialStatusSave();
		tutorialStatusSave.SetSendData = delegate(TutorialStatusSaveQuery param)
		{
			param.statusId = 10;
		};
		TutorialStatusSave request = tutorialStatusSave;
		base.StartCoroutine(request.Run(delegate()
		{
			RestrictionInput.EndLoad();
			this.Close(true);
		}, null, null));
	}

	private void OnClickedTermsOfUse()
	{
		CMDWebWindow cmdwebWindow = GUIMain.ShowCommonDialog(null, "CMDWebWindow") as CMDWebWindow;
		cmdwebWindow.TitleText = StringMaster.GetString("AgreementTitle");
		cmdwebWindow.Url = WebAddress.EXT_ADR_AGREE;
	}
}
