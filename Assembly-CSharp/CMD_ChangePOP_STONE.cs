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

	[SerializeField]
	private UIAssetsNumber assetPossessionNumber;

	[SerializeField]
	private UIAssetsNumber assetCostNumber;

	public Action OnPushedYesAction;

	public Action OnPushedNoAction;

	private Action<UnityEngine.Object> onPushYesButton;

	private bool beEnough;

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
			this.ruleButtonObject.SetActive(false);
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
		if (null == GUIManager.CheckTopDialog("CMDWebWindow", null))
		{
			if (this.OnPushedYesAction != null)
			{
				this.OnPushedYesAction();
			}
			if (this.onPushYesButton != null)
			{
				if (this.beEnough)
				{
					this.onPushYesButton(this);
				}
				else
				{
					base.SetWindowClosedAction(delegate
					{
						GUIMain.ShowCommonDialog(delegate(int n)
						{
							GUIPlayerStatus.RefreshParams_S(false);
						}, "CMD_Shop", null);
					});
					this.ClosePanel(true);
				}
			}
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

	public void SetAssetIcon(int category, string assetsValue)
	{
	}

	public void SetPushActionYesButton(Action<UnityEngine.Object> action)
	{
		this.onPushYesButton = action;
	}

	public void SetMessage(string title, string info)
	{
		this.Title = title;
		this.Info = info;
	}

	public void SetAssetValue(int category, int cost)
	{
		int userInventoryNumber = this.assetPossessionNumber.GetUserInventoryNumber(category);
		this.assetPossessionNumber.SetNumber(category, userInventoryNumber);
		this.assetCostNumber.SetNumber(category, cost);
		this.beEnough = (userInventoryNumber >= cost);
		if (!this.beEnough)
		{
			this.costNum.color = Color.red;
			this.yesButtonLabel.text = StringMaster.GetString("SystemButtonGoShop");
		}
	}
}
