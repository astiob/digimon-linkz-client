using Master;
using System;
using UnityEngine;

public class CMD_ChangePOP_STONE : CMD
{
	[SerializeField]
	private UILabel titleLabel;

	[SerializeField]
	private UILabel infoLabel;

	[SerializeField]
	private UILabel possessionTitle;

	[SerializeField]
	private UILabel possessionNum;

	[SerializeField]
	private UILabel costTitle;

	[SerializeField]
	private UILabel costNum;

	[SerializeField]
	private UILabel ruleButtonLabel;

	[SerializeField]
	private UILabel yesButtonLabel;

	[SerializeField]
	private UILabel noButtonLabel;

	public Action OnPushedYesAction;

	public Action OnPushedNoAction;

	public string Title
	{
		get
		{
			return this.titleLabel.text;
		}
		set
		{
			this.titleLabel.text = value;
		}
	}

	public string Info
	{
		get
		{
			return this.infoLabel.text;
		}
		set
		{
			this.infoLabel.text = value;
		}
	}

	public string BtnTextYes
	{
		get
		{
			return this.yesButtonLabel.text;
		}
		set
		{
			this.yesButtonLabel.text = value;
		}
	}

	public string BtnTextNo
	{
		get
		{
			return this.noButtonLabel.text;
		}
		set
		{
			this.noButtonLabel.text = value;
		}
	}

	private void Start()
	{
		this.possessionTitle.text = StringMaster.GetString("SystemPossession");
		this.costTitle.text = StringMaster.GetString("SystemCost");
		this.ruleButtonLabel.text = StringMaster.GetString("ShopRule-02");
	}

	public void SetDigistone(int possession, int cost)
	{
		this.possessionNum.text = possession.ToString();
		this.costNum.text = cost.ToString();
		if (possession < cost)
		{
			this.costNum.color = Color.red;
			this.yesButtonLabel.text = StringMaster.GetString("SystemButtonGoShop");
		}
	}

	private void OnPushedYesButton()
	{
		if (this.OnPushedYesAction != null && null == GUIManager.CheckTopDialog("CMDWebWindow", null))
		{
			this.OnPushedYesAction();
		}
	}

	private void OnPushedNoButton()
	{
		if (this.OnPushedNoAction != null && null == GUIManager.CheckTopDialog("CMDWebWindow", null))
		{
			this.OnPushedNoAction();
		}
		this.ClosePanel(true);
	}

	private void OnClickedLegalSpecificButton()
	{
		CMDWebWindow cmdwebWindow = GUIMain.ShowCommonDialog(null, "CMDWebWindow") as CMDWebWindow;
		cmdwebWindow.TitleText = StringMaster.GetString("ShopRule-02");
		cmdwebWindow.Url = WebAddress.EXT_ADR_TRADE;
	}
}
