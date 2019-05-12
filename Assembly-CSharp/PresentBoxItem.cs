using Evolution;
using FarmData;
using Master;
using Monster;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public sealed class PresentBoxItem : MonoBehaviour
{
	[SerializeField]
	private UILabel itemNameLabel;

	[SerializeField]
	private UISprite iconSprite;

	[SerializeField]
	private UITexture iconTexture;

	private GUIMonsterIcon monsterIcon;

	private ChipIcon chipIcon;

	private UIBasicSprite[] uiBasicSprites;

	private bool isASyncBusy;

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

	private void ReleaseIcons()
	{
		this.iconSprite.enabled = false;
		this.iconTexture.enabled = false;
		if (null != this.monsterIcon)
		{
			UnityEngine.Object.Destroy(this.monsterIcon.gameObject);
			this.monsterIcon = null;
		}
		if (this.chipIcon != null)
		{
			UnityEngine.Object.Destroy(this.chipIcon.gameObject);
			this.chipIcon = null;
		}
	}

	public IEnumerator SetItemWithWaitASync(string assetCategoryId, string objectId, string num, bool isLoadASync = false, Action callback = null)
	{
		while (this.isASyncBusy)
		{
			yield return null;
		}
		this.SetItem(assetCategoryId, objectId, num, isLoadASync, callback);
		yield break;
	}

	public void SetItem(string assetCategoryId, string objectId, string num, bool isLoadASync = false, Action callback = null)
	{
		this.ReleaseIcons();
		GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM assetCategory = MasterDataMng.Instance().RespDataMA_AssetCategoryM.GetAssetCategory(assetCategoryId);
		MasterDataMng.AssetCategory assetCategory2 = (MasterDataMng.AssetCategory)assetCategory.assetCategoryId.ToInt32();
		this.itemName = this.GetPresentName(assetCategory, objectId, false);
		string arg;
		if (assetCategory2 == MasterDataMng.AssetCategory.TIP)
		{
			arg = StringFormat.Cluster(num);
		}
		else
		{
			arg = num;
		}
		if (this.itemNameLabel != null)
		{
			if (assetCategory2 == MasterDataMng.AssetCategory.FACILITY_KEY || assetCategory2 == MasterDataMng.AssetCategory.TITLE)
			{
				this.itemNameLabel.text = this.itemName;
			}
			else
			{
				this.itemNameLabel.text = string.Format(StringMaster.GetString("SystemItemCount"), this.itemName, arg);
			}
		}
		switch (assetCategory2)
		{
		case MasterDataMng.AssetCategory.MONSTER:
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
				this.isASyncBusy = true;
				NGUIUtil.ChangeUITextureFromFileASync(this.iconTexture, this.GetTexturePath(assetCategory2, objectId), false, delegate
				{
					this.isASyncBusy = false;
					if (callback != null)
					{
						callback();
					}
				});
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
			this.isASyncBusy = true;
			GameWebAPI.RespDataMA_ChipM.Chip chipMainData = ChipDataMng.GetChipMainData(objectId);
			ChipDataMng.MakePrefabByChipData(chipMainData, this.iconSprite.gameObject, this.iconSprite.transform.localPosition, this.iconSprite.transform.localScale, delegate(ChipIcon result)
			{
				this.isASyncBusy = false;
				this.chipIcon = result;
				if (callback != null)
				{
					callback();
				}
			}, this.iconSprite.width, this.iconSprite.height, true);
			break;
		}
		case MasterDataMng.AssetCategory.TITLE:
			this.iconSprite.enabled = true;
			this.iconSprite.spriteName = this.GetSpriteName(assetCategory2);
			if (callback != null)
			{
				callback();
			}
			break;
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

	public string GetPresentName(GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM masterAssetCategory, string objectId, bool showDetail = false)
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
		case MasterDataMng.AssetCategory.TITLE:
		{
			GameWebAPI.RespDataMA_TitleMaster.TitleM titleM = TitleDataMng.GetDictionaryTitleM()[int.Parse(objectId)];
			if (titleM != null)
			{
				result = titleM.name;
			}
			break;
		}
		}
		if (showDetail)
		{
			switch (masterAssetCategory.assetCategoryId.ToInt32())
			{
			case 14:
			{
				GameWebAPI.RespDataMA_GetSoulM.SoulM soulMasterBySoulId = VersionUpMaterialData.GetSoulMasterBySoulId(objectId);
				result = soulMasterBySoulId.soulName;
				break;
			}
			case 17:
			{
				int chipId = int.Parse(objectId);
				GameWebAPI.RespDataMA_ChipM.Chip chipMaster = ChipDataMng.GetChipMaster(chipId);
				result = chipMaster.name;
				break;
			}
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
