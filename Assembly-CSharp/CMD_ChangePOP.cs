using Master;
using System;
using UnityEngine;

public class CMD_ChangePOP : CMD
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
	private UISprite possessionIconSprite;

	[SerializeField]
	private UITexture possessionIconTexture;

	[SerializeField]
	private UILabel costTitle;

	[SerializeField]
	private UILabel costNum;

	[SerializeField]
	private UISprite costIconSprite;

	[SerializeField]
	private UITexture costIconTexture;

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

	public static CMD_ChangePOP CreateExtendChipPOP(int possessionNum, int consumePrice, Action onPushedYesAction)
	{
		CMD_ChangePOP cmd_ChangePOP = GUIMain.ShowCommonDialog(null, "CMD_ChangePOP") as CMD_ChangePOP;
		cmd_ChangePOP.Title = StringMaster.GetString("ChipInstalling-05");
		cmd_ChangePOP.Info = string.Format(StringMaster.GetString("ChipInstalling-06"), consumePrice);
		cmd_ChangePOP.OnPushedYesAction = onPushedYesAction;
		cmd_ChangePOP.SetPoint(possessionNum, consumePrice);
		return cmd_ChangePOP;
	}

	public static CMD_ChangePOP CreateEjectChipPOP(int possessionNum, int consumePrice, Action onPushedYesAction)
	{
		CMD_ChangePOP cmd_ChangePOP = GUIMain.ShowCommonDialog(null, "CMD_ChangePOP") as CMD_ChangePOP;
		cmd_ChangePOP.Title = StringMaster.GetString("ChipInstalling-09");
		cmd_ChangePOP.Info = string.Format(StringMaster.GetString("ChipInstalling-12"), consumePrice);
		cmd_ChangePOP.OnPushedYesAction = onPushedYesAction;
		cmd_ChangePOP.SetPoint(possessionNum, consumePrice);
		return cmd_ChangePOP;
	}

	private void Start()
	{
		this.possessionTitle.text = StringMaster.GetString("SystemPossession");
		this.costTitle.text = StringMaster.GetString("SystemCost");
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
}
