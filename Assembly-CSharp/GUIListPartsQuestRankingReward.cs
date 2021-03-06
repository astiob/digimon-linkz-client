﻿using Evolution;
using FarmData;
using Master;
using Monster;
using System;
using System.Linq;
using UnityEngine;

public sealed class GUIListPartsQuestRankingReward : MonoBehaviour
{
	[SerializeField]
	private UISprite iconSprite;

	[SerializeField]
	private UITexture iconTexture;

	[SerializeField]
	private UILabel itemNumLabel;

	private GUIMonsterIcon monsterIcon;

	private UIBasicSprite[] uiBasicSprites;

	public int depth
	{
		get
		{
			if (this.iconSprite != null)
			{
				return this.iconSprite.depth;
			}
			if (this.iconTexture != null)
			{
				return this.iconTexture.depth;
			}
			return 0;
		}
	}

	public string itemName { get; private set; }

	private void Awake()
	{
		this.iconSprite.enabled = false;
		this.iconTexture.enabled = false;
	}

	public void SetItem(string assetCategoryId, string objectId, string num, bool isLoadASync = false, Action callback = null)
	{
		GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM assetCategory = MasterDataMng.Instance().RespDataMA_AssetCategoryM.GetAssetCategory(assetCategoryId);
		MasterDataMng.AssetCategory assetCategory2 = (MasterDataMng.AssetCategory)assetCategory.assetCategoryId.ToInt32();
		this.itemName = this.GetPresentName(assetCategory, objectId);
		string arg;
		if (assetCategory2 == MasterDataMng.AssetCategory.TIP)
		{
			arg = StringFormat.Cluster(num);
		}
		else
		{
			arg = num;
		}
		if (this.itemNumLabel != null)
		{
			if (assetCategory2 == MasterDataMng.AssetCategory.FACILITY_KEY)
			{
				this.itemNumLabel.text = this.itemName;
			}
			else
			{
				this.itemNumLabel.text = string.Format(StringMaster.GetString("SystemItemCount"), string.Empty, arg);
			}
		}
		switch (assetCategory2)
		{
		case MasterDataMng.AssetCategory.MONSTER:
			if (null != this.monsterIcon)
			{
				UnityEngine.Object.Destroy(this.monsterIcon.gameObject);
			}
			this.monsterIcon = GUIMonsterIcon.MakePrefabByMonsterData(MonsterDataMng.Instance().CreateMonsterDataByMID(objectId), Vector3.one, Vector3.zero, this.iconSprite.transform, true, false);
			this.monsterIcon.ResizeIcon(this.iconSprite.width, this.iconSprite.height);
			if (null != this.monsterIcon)
			{
				DepthController depthController = this.monsterIcon.GetDepthController();
				if (null != depthController)
				{
					depthController.AddWidgetDepth(this.monsterIcon.transform, this.iconSprite.depth + 10);
				}
			}
			if (callback != null)
			{
				callback();
			}
			break;
		case MasterDataMng.AssetCategory.DIGI_STONE:
		case MasterDataMng.AssetCategory.LINK_POINT:
		case MasterDataMng.AssetCategory.TIP:
		case MasterDataMng.AssetCategory.EXP:
		case MasterDataMng.AssetCategory.MEAT:
		case MasterDataMng.AssetCategory.TITLE:
			this.iconSprite.enabled = true;
			this.iconSprite.spriteName = this.GetSpriteName(assetCategory2);
			if (callback != null)
			{
				callback();
			}
			break;
		case MasterDataMng.AssetCategory.ITEM:
		case MasterDataMng.AssetCategory.SOUL:
		case MasterDataMng.AssetCategory.FACILITY_KEY:
		case MasterDataMng.AssetCategory.DUNGEON_TICKET:
			this.iconTexture.enabled = true;
			if (isLoadASync)
			{
				NGUIUtil.ChangeUITextureFromFileASync(this.iconTexture, this.GetTexturePath(assetCategory2, objectId), false, callback);
			}
			else
			{
				NGUIUtil.ChangeUITextureFromFile(this.iconTexture, this.GetTexturePath(assetCategory2, objectId), false);
				if (callback != null)
				{
					callback();
				}
			}
			if (assetCategory2 == MasterDataMng.AssetCategory.FACILITY_KEY)
			{
				this.iconSprite.enabled = true;
				this.iconSprite.spriteName = "Common02_item_flame";
			}
			break;
		case MasterDataMng.AssetCategory.CHIP:
		{
			GameWebAPI.RespDataMA_ChipM.Chip chipMainData = ChipDataMng.GetChipMainData(objectId);
			ChipDataMng.MakePrefabByChipData(chipMainData, this.iconSprite.gameObject, this.iconSprite.transform.localPosition, this.iconSprite.transform.localScale, delegate(ChipIcon result)
			{
				if (callback != null)
				{
					callback();
				}
			}, this.iconSprite.width, this.iconSprite.height, true);
			break;
		}
		}
		this.uiBasicSprites = base.transform.GetComponentsInParent<UIBasicSprite>(true);
	}

	public void SetColor(Color value)
	{
		if (this.uiBasicSprites != null)
		{
			foreach (UIBasicSprite uibasicSprite in this.uiBasicSprites)
			{
				uibasicSprite.color = value;
			}
		}
	}

	private string GetPresentName(GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM masterAssetCategory, string objectId)
	{
		string result = StringMaster.GetString("Present-10");
		if (masterAssetCategory != null)
		{
			result = masterAssetCategory.assetTitle;
		}
		MasterDataMng.AssetCategory assetCategory = (MasterDataMng.AssetCategory)masterAssetCategory.assetCategoryId.ToInt32();
		switch (assetCategory)
		{
		case MasterDataMng.AssetCategory.FACILITY_KEY:
		{
			FacilityKeyM facilityKeyMaster = FarmDataManager.GetFacilityKeyMaster(objectId);
			if (facilityKeyMaster != null)
			{
				result = facilityKeyMaster.facilityKeyName;
			}
			break;
		}
		default:
			if (assetCategory != MasterDataMng.AssetCategory.MONSTER)
			{
				if (assetCategory == MasterDataMng.AssetCategory.ITEM)
				{
					GameWebAPI.RespDataMA_GetItemM.ItemM itemM = MasterDataMng.Instance().RespDataMA_ItemM.GetItemM(objectId);
					if (itemM != null)
					{
						result = itemM.name;
					}
				}
			}
			else
			{
				GameWebAPI.RespDataMA_GetMonsterMS.MonsterM simple = MonsterMaster.GetMonsterMasterByMonsterId(objectId).Simple;
				if (simple != null)
				{
					GameWebAPI.RespDataMA_GetMonsterMG.MonsterM group = MonsterMaster.GetMonsterMasterByMonsterGroupId(simple.monsterGroupId).Group;
					if (group != null)
					{
						result = group.monsterName;
					}
				}
			}
			break;
		case MasterDataMng.AssetCategory.DUNGEON_TICKET:
		{
			GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM dungeonTicketM = MasterDataMng.Instance().RespDataMA_DungeonTicketMaster.dungeonTicketM.FirstOrDefault((GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM x) => objectId == x.dungeonTicketId);
			if (dungeonTicketM != null)
			{
				result = dungeonTicketM.name;
			}
			break;
		}
		}
		return result;
	}

	private string GetTexturePath(MasterDataMng.AssetCategory assetCategoryId, string objectId)
	{
		string result = string.Empty;
		switch (assetCategoryId)
		{
		case MasterDataMng.AssetCategory.SOUL:
			result = ClassSingleton<EvolutionData>.Instance.GetEvolveItemIconPathByID(objectId);
			break;
		default:
			if (assetCategoryId == MasterDataMng.AssetCategory.ITEM)
			{
				GameWebAPI.RespDataMA_GetItemM.ItemM itemM = MasterDataMng.Instance().RespDataMA_ItemM.GetItemM(objectId);
				if (itemM != null)
				{
					result = itemM.GetLargeImagePath();
				}
			}
			break;
		case MasterDataMng.AssetCategory.FACILITY_KEY:
		{
			FacilityConditionM[] facilityCondition = FarmDataManager.GetFacilityCondition(objectId);
			FacilityConditionM facilityConditionM = facilityCondition.FirstOrDefault((FacilityConditionM x) => int.Parse(x.conditionType) == 1);
			FacilityM facilityMasterByReleaseId = FarmDataManager.GetFacilityMasterByReleaseId(facilityConditionM.releaseId);
			result = facilityMasterByReleaseId.GetIconPath();
			break;
		}
		case MasterDataMng.AssetCategory.DUNGEON_TICKET:
		{
			GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM dungeonTicketM = MasterDataMng.Instance().RespDataMA_DungeonTicketMaster.dungeonTicketM.FirstOrDefault((GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM x) => objectId == x.dungeonTicketId);
			if (dungeonTicketM != null)
			{
				result = dungeonTicketM.img;
			}
			break;
		}
		}
		return result;
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
		case MasterDataMng.AssetCategory.EXP:
			result = "Common02_Drop_Exp";
			break;
		default:
			if (assetCategoryId != MasterDataMng.AssetCategory.MEAT)
			{
				if (assetCategoryId == MasterDataMng.AssetCategory.TITLE)
				{
					result = "Common02_item_title";
				}
			}
			else
			{
				result = "Common02_item_meat";
			}
			break;
		}
		return result;
	}
}
