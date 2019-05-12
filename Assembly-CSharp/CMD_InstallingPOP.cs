using Master;
using System;
using UnityEngine;

public sealed class CMD_InstallingPOP : CMD
{
	private Action _successCallback;

	[SerializeField]
	[Header("タイトルラベル")]
	private UILabel titleLabel;

	[Header("チップテクスチャ")]
	[SerializeField]
	private UITexture chipTexture;

	[SerializeField]
	[Header("チップランクプライト")]
	private UISprite rankSprite;

	[Header("はいラベル")]
	[SerializeField]
	private UILabel yesLabel;

	[SerializeField]
	[Header("いいえラベル")]
	private UILabel noLabel;

	[SerializeField]
	[Header("チップ名ラベル")]
	private UILabel chipNameLabel;

	[SerializeField]
	[Header("チップ説明ラベル")]
	private UILabel chipDescLabel;

	[SerializeField]
	[Header("チップ装着メッセージラベル")]
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
		CMD_InstallingPOP cmd_InstallingPOP = GUIMain.ShowCommonDialog(null, "CMD_InstallingPOP") as CMD_InstallingPOP;
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
		GameWebAPI.ChipEquipLogic request = ChipDataMng.RequestAPIChipEquip(this.equip, new Action<int, GameWebAPI.RequestMonsterList>(this.EndAttachment));
		Action<Exception> onFailed = delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
			GUIMain.BarrierOFF();
		};
		base.StartCoroutine(request.Run(null, onFailed, null));
	}

	private void EndAttachment(int resultCode, GameWebAPI.RequestMonsterList subRequest)
	{
		if (resultCode == 1)
		{
			this.successCallback();
			base.StartCoroutine(subRequest.Run(delegate()
			{
				RestrictionInput.EndLoad();
				base.ClosePanel(true);
			}, delegate(Exception noop)
			{
				RestrictionInput.EndLoad();
				GUIMain.BarrierOFF();
			}, null));
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
			base.ClosePanel(true);
		}, @string, message, AlertManager.ButtonActionType.Close, false);
	}

	private void OnTapNo()
	{
		base.ClosePanel(true);
	}
}
