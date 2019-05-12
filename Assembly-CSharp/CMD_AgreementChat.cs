using Master;
using System;
using UnityEngine;

public class CMD_AgreementChat : CMD_AgreementConsent
{
	[SerializeField]
	private GameObject chatServiceButton;

	[SerializeField]
	private UILabel chatServiceButtonLabel;

	private void Start()
	{
		this.titleLabel.text = StringMaster.GetString("AgreementChatTitle");
		this.infoLabel.text = StringMaster.GetString("AgreementChatInfo");
		this.ruleButtonLabel.text = StringMaster.GetString("AgreementTitle");
		this.chatServiceButton.SetActive(false);
		this.infoLabel.height += 50;
		this.yesButtonLabel.text = StringMaster.GetString("AgreementYesButtonText");
		this.noButtonLabel.text = StringMaster.GetString("AgreementNoButtonText");
	}

	protected override void OnAgreementOK()
	{
		base.Close(true);
	}

	private void OnClickedAgreementChat()
	{
	}
}
