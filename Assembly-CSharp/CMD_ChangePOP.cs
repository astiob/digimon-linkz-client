using Master;
using SwitchParts;
using System;
using UI.Common;
using UnityEngine;

public sealed class CMD_ChangePOP : CMD, IPayConfirmNotice
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
	private UISprite possessionIconSprite;

	[SerializeField]
	private UITexture possessionIconTexture;

	[SerializeField]
	private UISprite costIconSprite;

	[SerializeField]
	private UITexture costIconTexture;

	[SerializeField]
	private UIAssetsIcon assetsNumberIcon;

	[SerializeField]
	private UIAssetsIcon assetsCostIcon;

	[SerializeField]
	private UIAssetsNumber assetPossesionNumber;

	[SerializeField]
	private UIAssetsNumber assetCostNumber;

	[SerializeField]
	private ButtonSwitch yesButton;

	public Action OnPushedYesAction;

	public Action OnPushedNoAction;

	private Action<UnityEngine.Object> onPushYesButton;

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

	public static CMD_ChangePOP CreateExtendChipPOP(int possessionNum, int consumePrice, Action onPushedYesAction)
	{
		CMD_ChangePOP cmd_ChangePOP = GUIMain.ShowCommonDialog(null, "CMD_ChangePOP", null) as CMD_ChangePOP;
		cmd_ChangePOP.Title = StringMaster.GetString("ChipInstalling-05");
		cmd_ChangePOP.Info = string.Format(StringMaster.GetString("ChipInstalling-06"), consumePrice);
		cmd_ChangePOP.OnPushedYesAction = onPushedYesAction;
		cmd_ChangePOP.SetPoint(possessionNum, consumePrice);
		return cmd_ChangePOP;
	}

	public static CMD_Confirm CreateEjectChipPOP(Action onPushedYesAction)
	{
		CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(delegate(int i)
		{
			if (i == 0)
			{
				onPushedYesAction();
			}
		}, "CMD_Confirm", null) as CMD_Confirm;
		cmd_Confirm.Title = StringMaster.GetString("ChipInstalling-09");
		cmd_Confirm.Info = StringMaster.GetString("ChipInstalling-12");
		return cmd_Confirm;
	}

	public void SetPoint(int possession, int cost)
	{
		this.possessionNum.text = possession.ToString();
		this.costNum.text = cost.ToString();
	}

	public void SetSpriteIcon(string spriteName)
	{
		this.possessionIconSprite.spriteName = spriteName;
		this.costIconSprite.spriteName = spriteName;
	}

	public void SetTextureIcon(string filePath)
	{
		this.possessionIconSprite.gameObject.SetActive(false);
		this.costIconSprite.gameObject.SetActive(false);
		this.possessionIconTexture.enabled = true;
		this.costIconTexture.enabled = true;
		NGUIUtil.ChangeUITextureFromFile(this.possessionIconTexture, filePath, false);
		NGUIUtil.ChangeUITextureFromFile(this.costIconTexture, filePath, false);
	}

	private void OnPushedYesButton()
	{
		if (this.OnPushedYesAction != null)
		{
			this.OnPushedYesAction();
		}
		if (this.onPushYesButton != null)
		{
			this.onPushYesButton(this);
		}
	}

	private void OnPushedNoButton()
	{
		if (this.OnPushedNoAction != null)
		{
			this.OnPushedNoAction();
		}
		this.ClosePanel(true);
	}

	private void SetAssetCostAndPossession(int category, int possession, int cost)
	{
		this.assetPossesionNumber.SetNumber(category, possession);
		this.assetCostNumber.SetNumber(category, cost);
		if (possession < cost)
		{
			this.costNum.color = Color.red;
			this.yesButton.Switch(false);
			GUICollider component = this.yesButton.GetComponent<GUICollider>();
			component.activeCollider = false;
		}
	}

	public void SetUseDetail(object detail)
	{
		this.useDetail = detail;
	}

	public object GetUseDetail()
	{
		return this.useDetail;
	}

	public void SetAsset(MasterDataMng.AssetCategory category, string assetsValue)
	{
		this.assetsNumberIcon.SetAssetsCategory(category, assetsValue);
		this.assetsNumberIcon.SetIcon();
		this.assetsCostIcon.SetAssetsCategory(category, assetsValue);
		this.assetsCostIcon.SetIcon();
	}

	public void SetAssetIcon(int category, string assetsValue)
	{
		this.assetsNumberIcon.SetAssetsCategory((MasterDataMng.AssetCategory)category, assetsValue);
		this.assetsNumberIcon.SetIcon();
		this.assetsCostIcon.SetAssetsCategory((MasterDataMng.AssetCategory)category, assetsValue);
		this.assetsCostIcon.SetIcon();
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
		int userInventoryNumber = this.assetPossesionNumber.GetUserInventoryNumber(category);
		this.SetAssetCostAndPossession(category, userInventoryNumber, cost);
	}

	public void SetAssetValue(int category, int assetsValue, int cost)
	{
		int userInventoryNumber = this.assetPossesionNumber.GetUserInventoryNumber(category, assetsValue.ToString());
		this.SetAssetCostAndPossession(category, userInventoryNumber, cost);
	}
}
