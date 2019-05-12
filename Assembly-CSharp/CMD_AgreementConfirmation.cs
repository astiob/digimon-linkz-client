using Master;
using System;
using UnityEngine;

public class CMD_AgreementConfirmation : CMD
{
	[SerializeField]
	private UILabel titleLabel;

	[SerializeField]
	private UILabel infoLabelUpper;

	[SerializeField]
	private UILabel infoLabelLower;

	[SerializeField]
	private UILabel ruleButtonLabel;

	[SerializeField]
	private UILabel yesButtonLabel;

	[SerializeField]
	private UILabel noButtonLabel;

	private Action<bool> actionClosed;

	private void Start()
	{
		this.titleLabel.text = StringMaster.GetString("AgreementMinorsTitle");
		this.infoLabelUpper.text = StringMaster.GetString("AgreementMinorsInfo1");
		this.infoLabelLower.text = StringMaster.GetString("AgreementMinorsInfo2");
		this.ruleButtonLabel.text = StringMaster.GetString("AgreementTitle");
		this.yesButtonLabel.text = StringMaster.GetString("AgreementYesButtonText");
		this.noButtonLabel.text = StringMaster.GetString("AgreementNoButtonText");
	}

	public void SetActionAgreementPopupClosed(Action<bool> action)
	{
		this.actionClosed = action;
	}

	private void Close(bool agreement)
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

	private void OnAgreementOK()
	{
		this.Close(true);
	}

	private void OnClickedTermsOfUse()
	{
		CMDWebWindow cmdwebWindow = GUIMain.ShowCommonDialog(null, "CMDWebWindow", null) as CMDWebWindow;
		cmdwebWindow.TitleText = StringMaster.GetString("AgreementTitle");
		cmdwebWindow.Url = WebAddress.EXT_ADR_AGREE;
	}
}
