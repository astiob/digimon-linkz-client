using Evolution;
using Master;
using System;
using UnityEngine;

public class CaptureBonusItem : MonoBehaviour
{
	[SerializeField]
	private UITexture iconTexture;

	[SerializeField]
	private UISprite iconSprite;

	[SerializeField]
	private UILabel textMore;

	public string DialogDataSet(GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM masterUpgradeCategory, MasterDataMng.AssetCategory assetCategoryId, GameWebAPI.RespDataGA_ExecGacha.GachaRewardsData RewardsData)
	{
		string text = string.Empty;
		if (masterUpgradeCategory != null)
		{
			text = masterUpgradeCategory.assetTitle;
		}
		string arg = string.Empty;
		this.iconTexture.gameObject.SetActive(false);
		this.iconSprite.gameObject.SetActive(false);
		this.textMore.gameObject.SetActive(false);
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
			break;
		}
		case MasterDataMng.AssetCategory.DIGI_STONE:
			arg = text;
			this.iconSprite.gameObject.SetActive(true);
			this.iconSprite.spriteName = this.GetSpriteName(assetCategoryId);
			break;
		case MasterDataMng.AssetCategory.LINK_POINT:
			arg = text;
			this.iconSprite.gameObject.SetActive(true);
			this.iconSprite.spriteName = this.GetSpriteName(assetCategoryId);
			break;
		case MasterDataMng.AssetCategory.TIP:
			this.iconSprite.gameObject.SetActive(true);
			this.iconSprite.spriteName = this.GetSpriteName(assetCategoryId);
			arg = text;
			break;
		default:
			switch (assetCategoryId)
			{
			case MasterDataMng.AssetCategory.MEAT:
				this.iconSprite.gameObject.SetActive(true);
				this.iconSprite.spriteName = this.GetSpriteName(assetCategoryId);
				arg = text;
				goto IL_2FA;
			case MasterDataMng.AssetCategory.SOUL:
			{
				GameWebAPI.RespDataMA_GetSoulM.SoulM soul = MasterDataMng.Instance().RespDataMA_SoulM.GetSoul(RewardsData.assetValue);
				arg = soul.soulName;
				NGUIUtil.ChangeUITextureFromFile(this.iconTexture, this.GetTexturePath(assetCategoryId, RewardsData.assetValue), false);
				this.iconTexture.gameObject.SetActive(true);
				goto IL_2FA;
			}
			case MasterDataMng.AssetCategory.FACILITY_KEY:
				arg = text;
				goto IL_2FA;
			case MasterDataMng.AssetCategory.CHIP:
			{
				GameWebAPI.RespDataMA_ChipM.Chip chipMainData = ChipDataMng.GetChipMainData(RewardsData.assetValue);
				ChipDataMng.MakePrefabByChipData(chipMainData, this.iconSprite.gameObject, this.iconSprite.transform.localPosition, this.iconSprite.transform.localScale, null, -1, -1, true);
				arg = chipMainData.name;
				goto IL_2FA;
			}
			}
			arg = StringMaster.GetString("Present-10");
			break;
		case MasterDataMng.AssetCategory.ITEM:
		{
			GameWebAPI.RespDataMA_GetItemM.ItemM itemM = MasterDataMng.Instance().RespDataMA_ItemM.GetItemM(RewardsData.assetValue);
			if (itemM != null)
			{
				arg = itemM.name;
				NGUIUtil.ChangeUITextureFromFile(this.iconTexture, this.GetTexturePath(assetCategoryId, RewardsData.assetValue), false);
				this.iconTexture.gameObject.SetActive(true);
			}
			break;
		}
		}
		IL_2FA:
		return string.Format(StringMaster.GetString("CaptureBonusItem"), arg, RewardsData.count);
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

	public void ActivateMore()
	{
		this.iconTexture.gameObject.SetActive(false);
		this.iconSprite.gameObject.SetActive(false);
		this.textMore.gameObject.SetActive(true);
	}
}
