using Master;
using System;
using UnityEngine;

public sealed class CMD_InstallingPOP : CMD
{
	private Action _successCallback;

	[Header("タイトルラベル")]
	[SerializeField]
	private UILabel titleLabel;

	[Header("チップテクスチャ")]
	[SerializeField]
	private UITexture chipTexture;

	[Header("チップランクプライト")]
	[SerializeField]
	private UISprite rankSprite;

	[Header("はいラベル")]
	[SerializeField]
	private UILabel yesLabel;

	[Header("いいえラベル")]
	[SerializeField]
	private UILabel noLabel;

	[Header("チップ名ラベル")]
	[SerializeField]
	private UILabel chipNameLabel;

	[Header("チップ説明ラベル")]
	[SerializeField]
	private UILabel chipDescLabel;

	[Header("チップ装着メッセージラベル")]
	[SerializeField]
	private UILabel chipAlertLabel;

	private GameWebAPI.ReqDataCS_ChipEquipLogic equip;

	public string chipName
	{
		get
		{
			return this.chipNameLabel.text;
		}
		set
		{
			this.chipNameLabel.text = value;
		}
	}

	public string chipDetail
	{
		get
		{
			return this.chipDescLabel.text;
		}
		set
		{
			this.chipDescLabel.text = value;
		}
	}

	public Action successCallback
	{
		get
		{
			return this._successCallback;
		}
		set
		{
			this._successCallback = value;
		}
	}

	public static CMD_InstallingPOP Create(GameWebAPI.ReqDataCS_ChipEquipLogic equip, GameWebAPI.RespDataMA_ChipM.Chip chip, Action successCallback)
	{
		CMD_InstallingPOP cmd_InstallingPOP = GUIMain.ShowCommonDialog(null, "CMD_InstallingPOP", null) as CMD_InstallingPOP;
		cmd_InstallingPOP.equip = equip;
		cmd_InstallingPOP.chipName = chip.name;
		cmd_InstallingPOP.chipDetail = chip.detail;
		cmd_InstallingPOP.SetTexture(chip.GetIconPath());
		cmd_InstallingPOP.SetRankSprite(chip.rank);
		cmd_InstallingPOP.successCallback = successCallback;
		return cmd_InstallingPOP;
	}

	public void SetTexture(string path)
	{
		AssetDataMng.Instance().LoadObjectASync(path, delegate(UnityEngine.Object obj)
		{
			if (obj != null)
			{
				Texture2D mainTexture = obj as Texture2D;
				this.chipTexture.mainTexture = mainTexture;
			}
		});
	}

	public void SetRankSprite(string rank)
	{
		this.rankSprite.spriteName = ChipTools.GetRankPath(rank);
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.Show(f, sizeX, sizeY, aT);
		this.SetupLocalize();
	}

	private void SetupLocalize()
	{
		this.titleLabel.text = StringMaster.GetString("ChipInstallingConfirmTitle");
		this.yesLabel.text = StringMaster.GetString("SystemButtonYes");
		this.noLabel.text = StringMaster.GetString("SystemButtonNo");
		this.chipAlertLabel.text = StringMaster.GetString("ChipInstallingConfirmInfo");
	}

	private void OnTapYes()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		this.equip.act = 1;
		GameWebAPI.ChipEquipLogic request = ChipDataMng.RequestAPIChipEquip(this.equip, new Action<int>(this.EndAttachment));
		base.StartCoroutine(request.Run(null, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
			GUIMain.BarrierOFF();
		}, null));
	}

	private void EndAttachment(int resultCode)
	{
		if (resultCode == 1)
		{
			this.successCallback();
			RestrictionInput.EndLoad();
			base.ClosePanel(true);
		}
		else
		{
			RestrictionInput.EndLoad();
			ChipTools.CheckResultCode(resultCode);
			this.DispErrorModal(resultCode);
		}
	}

	private void DispErrorModal(string title, string info)
	{
		AlertManager.ShowModalMessage(delegate(int modal)
		{
		}, title, info, AlertManager.ButtonActionType.Close, false);
	}

	private void DispErrorModal(int resultCode)
	{
		string @string = StringMaster.GetString("SystemDataMismatchTitle");
		string message = string.Format(StringMaster.GetString("ChipDataMismatchMesage"), resultCode);
		AlertManager.ShowModalMessage(delegate(int modal)
		{
			this.<ClosePanel>__BaseCallProxy0(true);
		}, @string, message, AlertManager.ButtonActionType.Close, false);
	}

	private void OnTapNo()
	{
		base.ClosePanel(true);
	}
}
