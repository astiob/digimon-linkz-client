using Evolution;
using FarmData;
using Monster;
using MonsterIcon;
using MonsterIconExtensions;
using System;
using UnityEngine;

namespace UI.Common
{
	public sealed class UIAssetsIcon : MonoBehaviour
	{
		[SerializeField]
		private UISprite iconSprite;

		[SerializeField]
		private UITexture iconTexture;

		[SerializeField]
		private bool iconPixelPerfect;

		[SerializeField]
		private UIAssetsIcon.MonsterIconPartsFlag monsterIconParts;

		private MasterDataMng.AssetCategory assetsCategory;

		private string assetValue;

		private bool IsSprite(MasterDataMng.AssetCategory category)
		{
			bool result = false;
			switch (category)
			{
			case MasterDataMng.AssetCategory.DIGI_STONE:
			case MasterDataMng.AssetCategory.LINK_POINT:
			case MasterDataMng.AssetCategory.TIP:
			case MasterDataMng.AssetCategory.EXP:
				break;
			default:
				if (category != MasterDataMng.AssetCategory.MEAT)
				{
					return result;
				}
				break;
			}
			result = true;
			return result;
		}

		private bool IsTexture(MasterDataMng.AssetCategory category)
		{
			bool result = false;
			switch (category)
			{
			case MasterDataMng.AssetCategory.ITEM:
			case MasterDataMng.AssetCategory.GATHA_TICKET:
			case MasterDataMng.AssetCategory.SOUL:
			case MasterDataMng.AssetCategory.FACILITY_KEY:
			case MasterDataMng.AssetCategory.CHIP:
			case MasterDataMng.AssetCategory.DUNGEON_TICKET:
			case MasterDataMng.AssetCategory.TITLE:
				result = true;
				break;
			}
			return result;
		}

		private bool SetSprite(MasterDataMng.AssetCategory category, UISprite sprite)
		{
			bool result = true;
			switch (category)
			{
			case MasterDataMng.AssetCategory.DIGI_STONE:
				sprite.spriteName = "Common02_LB_Stone";
				break;
			case MasterDataMng.AssetCategory.LINK_POINT:
				sprite.spriteName = "Common02_LB_Link";
				break;
			case MasterDataMng.AssetCategory.TIP:
				sprite.spriteName = "Common02_LB_Chip";
				break;
			case MasterDataMng.AssetCategory.EXP:
				sprite.spriteName = "Common02_Drop_Exp";
				break;
			default:
				if (category != MasterDataMng.AssetCategory.MEAT)
				{
					if (category != MasterDataMng.AssetCategory.TITLE)
					{
						result = false;
					}
					else
					{
						sprite.spriteName = string.Format("Common02_item_title_{0}", CountrySetting.GetCountryPrefix(CountrySetting.CountryCode.EN));
					}
				}
				else
				{
					sprite.spriteName = "Common02_item_meat";
				}
				break;
			}
			return result;
		}

		private bool SetTexture(MasterDataMng.AssetCategory category, string assetsValue, UITexture texture)
		{
			bool result = true;
			string text = string.Empty;
			switch (category)
			{
			case MasterDataMng.AssetCategory.GATHA_TICKET:
				break;
			default:
				if (category != MasterDataMng.AssetCategory.ITEM)
				{
					result = false;
				}
				else
				{
					GameWebAPI.RespDataMA_GetItemM.ItemM itemM = MasterDataMng.Instance().RespDataMA_ItemM.GetItemM(assetsValue);
					if (itemM != null)
					{
						text = itemM.GetLargeImagePath();
					}
				}
				break;
			case MasterDataMng.AssetCategory.SOUL:
			{
				GameWebAPI.RespDataMA_GetSoulM.SoulM soul = MasterDataMng.Instance().RespDataMA_SoulM.GetSoul(assetsValue);
				if (soul != null)
				{
					text = ClassSingleton<EvolutionData>.Instance.GetEvolveItemIconPathByID(assetsValue);
				}
				break;
			}
			case MasterDataMng.AssetCategory.FACILITY_KEY:
			{
				FacilityM facilityMasterByReleaseId = FarmDataManager.GetFacilityMasterByReleaseId(assetsValue);
				if (facilityMasterByReleaseId != null)
				{
					text = facilityMasterByReleaseId.GetIconPath();
				}
				break;
			}
			case MasterDataMng.AssetCategory.DUNGEON_TICKET:
			{
				GameWebAPI.RespDataMA_DungeonTicketMaster.DungeonTicketM ticketMaster = MasterDataMng.Instance().RespDataMA_DungeonTicketMaster.GetTicketMaster(assetsValue);
				if (ticketMaster != null)
				{
					text = ticketMaster.img;
				}
				break;
			}
			}
			if (!string.IsNullOrEmpty(text))
			{
				NGUIUtil.ChangeUITextureFromFile(texture, text, false);
			}
			return result;
		}

		private bool SetMonsterIcon(string assetsValue, Transform parent, int iconSize, int depth, int monsterIconOption)
		{
			bool result = false;
			MonsterClientMaster monsterMasterByMonsterId = MonsterMaster.GetMonsterMasterByMonsterId(assetsValue);
			if (monsterMasterByMonsterId != null)
			{
				MonsterIcon monsterIcon = MonsterIconFactory.CreateIcon(monsterIconOption);
				if (monsterIcon != null)
				{
					monsterIcon.Initialize(parent, iconSize, depth);
					monsterIcon.Message.SetSortTextColor(Color.white);
					monsterIcon.SetMonsterImage(monsterMasterByMonsterId);
					result = true;
				}
			}
			return result;
		}

		private bool SetChipIcon(string assetsValue, GameObject parent)
		{
			bool result = false;
			GameWebAPI.RespDataMA_ChipM.Chip chipMainData = ChipDataMng.GetChipMainData(assetsValue);
			if (chipMainData != null)
			{
				Transform transform = parent.transform;
				ChipDataMng.MakePrefabByChipData(chipMainData, parent, transform.localPosition, transform.localScale, null, -1, -1, true);
				result = true;
			}
			return result;
		}

		private int GetMonsterIconPartsFlag()
		{
			int num = 0;
			num |= ((!this.monsterIconParts.showMessage) ? 0 : 1);
			num |= ((!this.monsterIconParts.showNew) ? 0 : 2);
			num |= ((!this.monsterIconParts.showLock) ? 0 : 4);
			num |= ((!this.monsterIconParts.showArousal) ? 0 : 8);
			num |= ((!this.monsterIconParts.showMedal) ? 0 : 16);
			num |= ((!this.monsterIconParts.showPlayerNo) ? 0 : 32);
			return num | ((!this.monsterIconParts.showGimmick) ? 0 : 64);
		}

		public void SetAssetsCategory(MasterDataMng.AssetCategory category)
		{
			this.SetAssetsCategory(category, string.Empty);
		}

		public void SetAssetsCategory(MasterDataMng.AssetCategory category, string assetsValue)
		{
			this.assetsCategory = category;
			this.assetValue = assetsValue;
			if (this.IsSprite(category))
			{
				this.iconSprite.enabled = true;
				this.iconTexture.enabled = false;
			}
			else if (this.IsTexture(category))
			{
				this.iconTexture.enabled = true;
				this.iconSprite.enabled = false;
			}
			else
			{
				this.iconSprite.enabled = false;
				this.iconTexture.enabled = false;
			}
		}

		public void SetIcon()
		{
			global::Debug.Assert(this.assetsCategory != (MasterDataMng.AssetCategory)0, "アセットカテゴリーIDが設定されていません");
			if (this.iconSprite.enabled)
			{
				if (this.SetSprite(this.assetsCategory, this.iconSprite) && this.iconPixelPerfect)
				{
					this.iconSprite.MakePixelPerfect();
				}
			}
			else if (this.iconTexture.enabled)
			{
				if (this.SetTexture(this.assetsCategory, this.assetValue, this.iconTexture) && this.iconPixelPerfect)
				{
					this.iconTexture.MakePixelPerfect();
				}
			}
			else if (this.assetsCategory == MasterDataMng.AssetCategory.CHIP)
			{
				this.SetChipIcon(this.assetValue, this.iconSprite.gameObject);
			}
			else if (this.assetsCategory == MasterDataMng.AssetCategory.MONSTER)
			{
				this.SetMonsterIcon(this.assetValue, this.iconSprite.transform, (int)this.iconSprite.localSize.x, this.iconSprite.depth, this.GetMonsterIconPartsFlag());
			}
		}

		[Serializable]
		private struct MonsterIconPartsFlag
		{
			public bool showMessage;

			public bool showNew;

			public bool showLock;

			public bool showArousal;

			public bool showMedal;

			public bool showPlayerNo;

			public bool showGimmick;
		}
	}
}
