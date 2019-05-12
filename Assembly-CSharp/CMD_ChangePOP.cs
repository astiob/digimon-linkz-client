using Master;
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

	public static CMD_ChangePOP CreateExtendChipPOP(int possessionNum, int consumePrice, Action onPushedYesAction)
	{
		CMD_ChangePOP cmd_ChangePOP = GUIMain.ShowCommonDialog(null, "CMD_ChangePOP", null) as CMD_ChangePOP;
		cmd_ChangePOP.Title = StringMaster.GetString("ChipInstalling-05");
		cmd_ChangePOP.Info = string.Format(StringMaster.GetString("ChipInstalling-06"), consumePrice);
		cmd_ChangePOP.OnPushedYesAction = onPushedYesAction;
		cmd_ChangePOP.SetPoint(possessionNum, consumePrice);
		return cmd_ChangePOP;
	}

	public static CMD_ChangePOP CreateEjectChipPOP(int possessionNum, int consumePrice, Action onPushedYesAction)
	{
		CMD_ChangePOP cmd_ChangePOP = GUIMain.ShowCommonDialog(null, "CMD_ChangePOP", null) as CMD_ChangePOP;
		cmd_ChangePOP.Title = StringMaster.GetString("ChipInstalling-09");
		cmd_ChangePOP.Info = string.Format(StringMaster.GetString("ChipInstalling-12"), consumePrice);
		cmd_ChangePOP.OnPushedYesAction = onPushedYesAction;
		cmd_ChangePOP.SetPoint(possessionNum, consumePrice);
		return cmd_ChangePOP;
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
	}

	private void OnPushedNoButton()
	{
		if (this.OnPushedNoAction != null)
		{
			this.OnPushedNoAction();
		}
		this.ClosePanel(true);
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
}
