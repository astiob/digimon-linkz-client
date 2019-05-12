using Master;
using System;
using UnityEngine;

public class ChipSphereStatus : MonoBehaviour
{
	[Header("MenuType")]
	[SerializeField]
	private CMD_ChipSphere.MenuType myMenuType;

	[Header("Lookのゲームオブジェクト")]
	[SerializeField]
	private GameObject lookGO;

	[SerializeField]
	[Header("アイコン右下のレア度")]
	private UISprite rareSprite;

	[Header("アイコンのlookテクスチャ")]
	[SerializeField]
	private UITexture chipLookTexture;

	[Header("アイコンのテクスチャ")]
	[SerializeField]
	private UITexture chipTexture;

	[SerializeField]
	[Header("アイコンの個数/名前/進化段階名")]
	private UILabel chipNameLabel;

	[Header("右下のメッセージ")]
	[SerializeField]
	private UILabel messageLabel;

	[SerializeField]
	[Header("ボタンのラベル")]
	private UILabel buttonLabel;

	[SerializeField]
	[Header("強化ボタンのラベル")]
	private UILabel reinforcementButtonLabel;

	[Header("ボタンのスプライト")]
	[SerializeField]
	private UISprite buttonSprite;

	[SerializeField]
	[Header("強化ボタンのスプライト")]
	private UISprite reinforcementButtonSprite;

	[SerializeField]
	[Header("ボタンのコライダー")]
	private BoxCollider buttonCollider;

	[SerializeField]
	[Header("強化ボタンのコライダー")]
	private BoxCollider reinforcementButtonCollider;

	private Color defaultButtonLabelEffectColor;

	private Color defaultReinforcementButtonLabelEffectColor;

	private int extendItemCount { get; set; }

	private int ejectItemCount { get; set; }

	public void SetButtonOff()
	{
		if (this.buttonCollider != null)
		{
			this.buttonCollider.enabled = false;
		}
		if (this.reinforcementButtonCollider != null)
		{
			this.reinforcementButtonCollider.enabled = false;
		}
	}

	private void Awake()
	{
		if (this.buttonLabel != null)
		{
			this.defaultButtonLabelEffectColor = this.buttonLabel.effectColor;
		}
		if (this.reinforcementButtonLabel != null)
		{
			this.defaultReinforcementButtonLabelEffectColor = this.reinforcementButtonLabel.effectColor;
		}
		this.SetupLocalize();
	}

	private void SetupLocalize()
	{
		switch (this.myMenuType)
		{
		case CMD_ChipSphere.MenuType.Detail:
			this.buttonLabel.text = StringMaster.GetString("ChipInstalling-09");
			if (this.reinforcementButtonLabel != null)
			{
				this.reinforcementButtonLabel.text = StringMaster.GetString("ReinforcementTitle");
			}
			break;
		}
	}

	public void Refresh(int extendItemCount, int ejectItemCount)
	{
		this.extendItemCount = extendItemCount;
		this.ejectItemCount = ejectItemCount;
	}

	public void SetupDetail(ChipSphereIconButton.Parameter parameter)
	{
		switch (parameter.menuType)
		{
		case CMD_ChipSphere.MenuType.Empty:
			NGUIUtil.ChangeUITextureFromFileASync(this.chipTexture, "ChipThumbnail/Chip_Empty", false, null);
			NGUITools.SetActiveSelf(this.chipTexture.gameObject, true);
			NGUITools.SetActiveSelf(this.chipNameLabel.gameObject, false);
			NGUITools.SetActiveSelf(this.rareSprite.gameObject, false);
			NGUITools.SetActiveSelf(this.chipLookTexture.gameObject, false);
			NGUITools.SetActiveSelf(this.lookGO, false);
			this.messageLabel.text = StringMaster.GetString("ChipInstalling-03");
			this.buttonLabel.text = StringMaster.GetString("ChipInstalling-04");
			this.chipNameLabel.color = Color.white;
			this.myMenuType = parameter.menuType;
			this.buttonSprite.spriteName = "Common02_Btn_Green";
			this.buttonCollider.enabled = true;
			this.buttonLabel.color = Color.white;
			this.buttonLabel.effectColor = this.defaultButtonLabelEffectColor;
			break;
		case CMD_ChipSphere.MenuType.Extendable:
		{
			NGUIUtil.ChangeUITextureFromFileASync(this.chipTexture, "ChipThumbnail/Chip_NotOpen", true, null);
			NGUITools.SetActiveSelf(this.lookGO, true);
			NGUITools.SetActiveSelf(this.rareSprite.gameObject, false);
			NGUITools.SetActiveSelf(this.chipNameLabel.gameObject, true);
			NGUITools.SetActiveSelf(this.chipLookTexture.gameObject, false);
			NGUITools.SetActiveSelf(this.chipTexture.gameObject, true);
			int itemCount = parameter.itemCount;
			this.chipNameLabel.text = string.Format("×{0}", itemCount);
			this.buttonLabel.text = StringMaster.GetString("ChipInstalling-08");
			global::Debug.LogFormat("{0} >= {1}, isOpened:{2}", new object[]
			{
				this.extendItemCount,
				itemCount,
				parameter.isOpened
			});
			bool flag = this.extendItemCount >= itemCount;
			if (flag)
			{
				this.chipNameLabel.color = Color.white;
			}
			else
			{
				this.chipNameLabel.color = Color.red;
			}
			if (flag && parameter.isExtendable)
			{
				this.messageLabel.text = StringMaster.GetString("ChipInstalling-07");
				this.buttonSprite.spriteName = "Common02_Btn_Green";
				this.buttonCollider.enabled = true;
				this.buttonLabel.color = Color.white;
				this.buttonLabel.effectColor = this.defaultButtonLabelEffectColor;
			}
			else
			{
				this.messageLabel.text = StringMaster.GetString("ChipInstalling-11");
				this.buttonSprite.spriteName = "Common02_Btn_Gray";
				this.buttonCollider.enabled = false;
				this.buttonLabel.color = ConstValue.DEACTIVE_BUTTON_LABEL;
				this.buttonLabel.effectColor = ConstValue.DEACTIVE_BUTTON_LABEL;
			}
			this.myMenuType = parameter.menuType;
			break;
		}
		case CMD_ChipSphere.MenuType.NotYet:
		{
			this.chipNameLabel.text = parameter.chipName;
			NGUITools.SetActiveSelf(this.rareSprite.gameObject, false);
			string text = string.Format(StringMaster.GetString("ChipInstalling-10"), parameter.chipDetail);
			this.messageLabel.text = text;
			break;
		}
		case CMD_ChipSphere.MenuType.Detail:
		{
			this.chipNameLabel.text = parameter.chipName;
			this.messageLabel.text = parameter.chipDetail;
			NGUIUtil.ChangeUITextureFromFileASync(this.chipTexture, parameter.chipIconPath, false, null);
			NGUITools.SetActiveSelf(this.chipTexture.gameObject, true);
			this.rareSprite.spriteName = ChipTools.GetRankPath(parameter.chipRank);
			NGUITools.SetActiveSelf(this.rareSprite.gameObject, true);
			if (this.ejectItemCount > 0)
			{
				this.buttonSprite.spriteName = "Common02_Btn_Red";
				this.buttonCollider.enabled = true;
				this.buttonLabel.color = Color.white;
				this.buttonLabel.effectColor = this.defaultButtonLabelEffectColor;
			}
			else
			{
				this.buttonSprite.spriteName = "Common02_Btn_Gray";
				this.buttonCollider.enabled = false;
				this.buttonLabel.color = ConstValue.DEACTIVE_BUTTON_LABEL;
				this.buttonLabel.effectColor = ConstValue.DEACTIVE_BUTTON_LABEL;
			}
			GameWebAPI.RespDataMA_ChipM.Chip chipEnhancedData = ChipDataMng.GetChipEnhancedData(parameter.ConvertChipId());
			if (chipEnhancedData != null)
			{
				if (this.reinforcementButtonSprite != null)
				{
					this.reinforcementButtonSprite.spriteName = "Common02_Btn_Green";
				}
				if (this.reinforcementButtonCollider != null)
				{
					this.reinforcementButtonCollider.enabled = true;
				}
				if (this.reinforcementButtonLabel != null)
				{
					this.reinforcementButtonLabel.color = Color.white;
				}
				if (this.buttonLabel != null)
				{
					this.buttonLabel.effectColor = this.defaultReinforcementButtonLabelEffectColor;
				}
			}
			else
			{
				if (this.reinforcementButtonSprite != null)
				{
					this.reinforcementButtonSprite.spriteName = "Common02_Btn_Gray";
				}
				if (this.reinforcementButtonCollider != null)
				{
					this.reinforcementButtonCollider.enabled = false;
				}
				if (this.reinforcementButtonLabel != null)
				{
					this.reinforcementButtonLabel.color = ConstValue.DEACTIVE_BUTTON_LABEL;
				}
				if (this.buttonLabel != null)
				{
					this.buttonLabel.effectColor = ConstValue.DEACTIVE_BUTTON_LABEL;
				}
			}
			break;
		}
		}
	}
}
