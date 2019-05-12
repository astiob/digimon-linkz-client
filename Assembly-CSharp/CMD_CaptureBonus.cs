using Evolution;
using Master;
using System;
using UnityEngine;

public sealed class CMD_CaptureBonus : CMD
{
	[SerializeField]
	private UILabel titleLabel;

	[SerializeField]
	private UILabel itemDetileLabel;

	[SerializeField]
	private UILabel presentBoxLabel;

	[SerializeField]
	private UITexture iconTexture;

	[SerializeField]
	private UISprite iconSprite;

	public static CMD_CaptureBonus Create(string title, string info, Action<int> onCloseAction = null)
	{
		return GUIMain.ShowCommonDialog(onCloseAction, "CMD_CaptureBonus") as CMD_CaptureBonus;
	}

	public void DialogDataSet(GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM masterUpgradeCategory, MasterDataMng.AssetCategory assetCategoryId, GameWebAPI.RespDataGA_ExecGacha.GachaRewardsData RewardsData)
	{
		string text = string.Empty;
		if (masterUpgradeCategory != null)
		{
			text = masterUpgradeCategory.assetTitle;
		}
		string arg = string.Empty;
		this.iconTexture.gameObject.SetActive(false);
		this.iconSprite.gameObject.SetActive(false);
		switch (assetCategoryId)
		{
		case MasterDataMng.AssetCategory.MONSTER:
		{
			MonsterData monsterData = MonsterDataMng.Instance().CreateMonsterDataByMID(RewardsData.assetValue);
			GUIMonsterIcon guimonsterIcon = GUIMonsterIcon.MakePrefabByMonsterData(MonsterDataMng.Instance().CreateMonsterDataByMID(RewardsData.assetValue), Vector3.one, Vector3.zero, this.iconSprite.transform, true, false);
			guimonsterIcon.ResizeIcon(this.iconSprite.width, this.iconSprite.height);
			if (null != guimonsterIcon)
			{
				DepthController depthController = guimonsterIcon.GetDepthController();
				if (null != depthController)
				{
					depthController.AddWidgetDepth(guimonsterIcon.transform, this.iconSprite.depth + 10);
					this.iconSprite.gameObject.SetActive(true);
				}
			}
			arg = monsterData.monsterMG.monsterName;
			goto IL_2FC;
		}
		case MasterDataMng.AssetCategory.DIGI_STONE:
			arg = text;
			this.iconSprite.gameObject.SetActive(true);
			this.iconSprite.spriteName = this.GetSpriteName(assetCategoryId);
			goto IL_2FC;
		case MasterDataMng.AssetCategory.LINK_POINT:
			arg = text;
			this.iconSprite.gameObject.SetActive(true);
			this.iconSprite.spriteName = this.GetSpriteName(assetCategoryId);
			goto IL_2FC;
		case MasterDataMng.AssetCategory.TIP:
			this.iconSprite.gameObject.SetActive(true);
			this.iconSprite.spriteName = this.GetSpriteName(assetCategoryId);
			arg = text;
			goto IL_2FC;
		case MasterDataMng.AssetCategory.ITEM:
		{
			GameWebAPI.RespDataMA_GetItemM.ItemM itemM = MasterDataMng.Instance().RespDataMA_ItemM.GetItemM(RewardsData.assetValue);
			if (itemM != null)
			{
				arg = itemM.name;
				NGUIUtil.ChangeUITextureFromFile(this.iconTexture, this.GetTexturePath(assetCategoryId, RewardsData.assetValue), false);
				this.iconTexture.gameObject.SetActive(true);
			}
			goto IL_2FC;
		}
		case MasterDataMng.AssetCategory.MEAT:
			this.iconSprite.gameObject.SetActive(true);
			this.iconSprite.spriteName = this.GetSpriteName(assetCategoryId);
			arg = text;
			goto IL_2FC;
		case MasterDataMng.AssetCategory.SOUL:
		{
			GameWebAPI.RespDataMA_GetSoulM.SoulM soul = MasterDataMng.Instance().RespDataMA_SoulM.GetSoul(RewardsData.assetValue);
			arg = soul.soulName;
			NGUIUtil.ChangeUITextureFromFile(this.iconTexture, this.GetTexturePath(assetCategoryId, RewardsData.assetValue), false);
			this.iconTexture.gameObject.SetActive(true);
			goto IL_2FC;
		}
		case MasterDataMng.AssetCategory.FACILITY_KEY:
			arg = text;
			goto IL_2FC;
		case MasterDataMng.AssetCategory.CHIP:
		{
			GameWebAPI.RespDataMA_ChipM.Chip chipMainData = ChipDataMng.GetChipMainData(RewardsData.assetValue);
			ChipDataMng.MakePrefabByChipData(chipMainData, this.iconSprite.gameObject, this.iconSprite.transform.localPosition, this.iconSprite.transform.localScale, null, -1, -1, true);
			arg = chipMainData.name;
			goto IL_2FC;
		}
		}
		arg = StringMaster.GetString("Present-10");
		IL_2FC:
		this.titleLabel.text = StringMaster.GetString("CaptureBonusTitle");
		this.presentBoxLabel.text = StringMaster.GetString("CaptureBonusText");
		this.itemDetileLabel.text = string.Format(StringMaster.GetString("CaptureBonusItem"), arg, RewardsData.count);
	}

	private string GetSpriteName(MasterDataMng.AssetCategory assetCategoryId)
	{
		string result = string.Empty;
		switch (assetCategoryId)
		{
		case MasterDataMng.AssetCategory.DIGI_STONE:
			result = "Common02_LB_Stone";
			break;
		case MasterDataMng.AssetCategory.LINK_POINT:
			result = "Common02_LB_Link";
			break;
		case MasterDataMng.AssetCategory.TIP:
			result = "Common02_LB_Chip";
			break;
		default:
			if (assetCategoryId == MasterDataMng.AssetCategory.MEAT)
			{
				result = "Common02_item_meat";
			}
			break;
		}
		return result;
	}

	private string GetTexturePath(MasterDataMng.AssetCategory assetCategoryId, string objectId)
	{
		string result = string.Empty;
		if (assetCategoryId != MasterDataMng.AssetCategory.ITEM)
		{
			if (assetCategoryId == MasterDataMng.AssetCategory.SOUL)
			{
				result = ClassSingleton<EvolutionData>.Instance.GetEvolveItemIconPathByID(objectId);
			}
		}
		else
		{
			GameWebAPI.RespDataMA_GetItemM.ItemM itemM = MasterDataMng.Instance().RespDataMA_ItemM.GetItemM(objectId);
			if (itemM != null)
			{
				result = itemM.GetLargeImagePath();
			}
		}
		return result;
	}

	protected override void Awake()
	{
		base.Awake();
	}

	private void Start()
	{
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.Show(f, sizeX, sizeY, aT);
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void ClosePanel(bool animation = true)
	{
		base.ClosePanel(animation);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}
