using Master;
using System;
using UI.Common;
using UnityEngine;

public sealed class CMD_ChangePOP_STONE : CMD, IPayConfirmNotice
{
	[SerializeField]
	private UILabel titleLabel;

	[SerializeField]
	private UILabel infoLabel;

	[SerializeField]
	private UILabel possessionNum;

	[SerializeField]
	private UILabel costNum;

	[SerializeField]
	private UILabel yesButtonLabel;

	[SerializeField]
	private UILabel noButtonLabel;

	[SerializeField]
	private GameObject ruleButtonObject;

	public Action OnPushedYesAction;

	public Action OnPushedNoAction;

	private object useDetail;

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
		if (this.ruleButtonObject != null)
		{
			this.ruleButtonObject.SetActive(true);
		}
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
		CMDWebWindow cmdwebWindow = GUIMain.ShowCommonDialog(null, "CMDWebWindow", null) as CMDWebWindow;
		cmdwebWindow.TitleText = StringMaster.GetString("ShopRule-02");
		cmdwebWindow.Url = WebAddress.EXT_ADR_TRADE;
	}

	public void SetUseDetail(object detail)
	{
		this.useDetail = detail;
	}

	public object GetUseDetail()
	{
		return this.useDetail;
	}
}
