using Evolution;
using Monster;
using System;

namespace User
{
	public static class UserInventory
	{
		public static int GetNumberExceptProtectedAssets(MasterDataMng.AssetCategory category, string assetsValue)
		{
			int num = -1;
			if (category != MasterDataMng.AssetCategory.MONSTER)
			{
				if (category != MasterDataMng.AssetCategory.CHIP)
				{
					num = UserInventory.GetNumber(category, assetsValue);
				}
				else if (ChipDataMng.userChipData != null && ChipDataMng.userChipData.userChipList != null)
				{
					if (string.IsNullOrEmpty(assetsValue) || "0" == assetsValue)
					{
						num = 0;
						for (int i = 0; i < ChipDataMng.userChipData.userChipList.Length; i++)
						{
							if (ChipDataMng.userChipData.userChipList[i].userMonsterId == 0)
							{
								num++;
							}
						}
					}
					else
					{
						int num2 = 0;
						if (int.TryParse(assetsValue, out num2))
						{
							num = 0;
							for (int j = 0; j < ChipDataMng.userChipData.userChipList.Length; j++)
							{
								GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChipList = ChipDataMng.userChipData.userChipList[j];
								if (userChipList.chipId == num2 && userChipList.userMonsterId == 0)
								{
									num++;
								}
							}
						}
					}
				}
			}
			else
			{
				Debug.LogError("未実装");
			}
			Debug.Assert(-1 != num, "アセット所持数の取得に失敗.");
			return num;
		}

		public static int GetNumber(MasterDataMng.AssetCategory category, string assetsValue)
		{
			int num = -1;
			switch (category)
			{
			case MasterDataMng.AssetCategory.GATHA_TICKET:
				Debug.LogError("未実装");
				break;
			default:
				if (category != MasterDataMng.AssetCategory.MONSTER)
				{
					if (category != MasterDataMng.AssetCategory.ITEM)
					{
						num = UserInventory.GetNumber(category);
					}
					else
					{
						int itemId = 0;
						if (int.TryParse(assetsValue, out itemId))
						{
							num = Singleton<UserDataMng>.Instance.GetUserItemNumByItemId(itemId);
						}
					}
				}
				else
				{
					int num2 = 0;
					if (string.IsNullOrEmpty(assetsValue) || "0" == assetsValue)
					{
						num = ClassSingleton<MonsterUserDataMng>.Instance.GetMonsterNum();
					}
					else if (int.TryParse(assetsValue, out num2))
					{
						Debug.LogError("未実装");
					}
				}
				break;
			case MasterDataMng.AssetCategory.SOUL:
				num = EvolutionMaterialData.GetUserEvolutionMaterialNum(assetsValue);
				break;
			case MasterDataMng.AssetCategory.CHIP:
				if (ChipDataMng.userChipData != null && ChipDataMng.userChipData.userChipList != null)
				{
					if (string.IsNullOrEmpty(assetsValue) || "0" == assetsValue)
					{
						num = ChipDataMng.userChipData.userChipList.Length;
					}
					else
					{
						int num3 = 0;
						if (int.TryParse(assetsValue, out num3))
						{
							num = 0;
							for (int i = 0; i < ChipDataMng.userChipData.userChipList.Length; i++)
							{
								if (ChipDataMng.userChipData.userChipList[i].chipId == num3)
								{
									num++;
								}
							}
						}
					}
				}
				break;
			case MasterDataMng.AssetCategory.DUNGEON_TICKET:
				Debug.LogError("未実装");
				break;
			}
			Debug.Assert(-1 != num, "アセット所持数の取得に失敗.");
			return num;
		}

		public static int GetNumber(MasterDataMng.AssetCategory category)
		{
			int result = 0;
			switch (category)
			{
			case MasterDataMng.AssetCategory.MONSTER:
				result = ClassSingleton<MonsterUserDataMng>.Instance.GetMonsterNum();
				break;
			case MasterDataMng.AssetCategory.DIGI_STONE:
				if (DataMng.Instance().RespDataUS_PlayerInfo != null && DataMng.Instance().RespDataUS_PlayerInfo.playerInfo != null)
				{
					result = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point;
				}
				break;
			case MasterDataMng.AssetCategory.LINK_POINT:
				if (DataMng.Instance().RespDataUS_PlayerInfo != null && DataMng.Instance().RespDataUS_PlayerInfo.playerInfo != null)
				{
					int.TryParse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.friendPoint, out result);
				}
				break;
			case MasterDataMng.AssetCategory.TIP:
				if (DataMng.Instance().RespDataUS_PlayerInfo != null && DataMng.Instance().RespDataUS_PlayerInfo.playerInfo != null)
				{
					int.TryParse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney, out result);
				}
				break;
			default:
				switch (category)
				{
				case MasterDataMng.AssetCategory.CHIP:
					if (ChipDataMng.userChipData != null && ChipDataMng.userChipData.userChipList != null)
					{
						result = ChipDataMng.userChipData.userChipList.Length;
					}
					break;
				default:
					if (category == MasterDataMng.AssetCategory.MEAT)
					{
						if (DataMng.Instance().RespDataUS_PlayerInfo != null && DataMng.Instance().RespDataUS_PlayerInfo.playerInfo != null)
						{
							int.TryParse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.meatNum, out result);
						}
					}
					break;
				case MasterDataMng.AssetCategory.TITLE:
					if (TitleDataMng.userTitleList != null)
					{
						result = TitleDataMng.userTitleList.Count;
					}
					break;
				}
				break;
			}
			return result;
		}

		public static bool CheckOverNumber(MasterDataMng.AssetCategory category, int addCount)
		{
			int num = -1;
			int num2 = -1;
			switch (category)
			{
			case MasterDataMng.AssetCategory.MONSTER:
				if (DataMng.Instance().RespDataUS_PlayerInfo != null && DataMng.Instance().RespDataUS_PlayerInfo.playerInfo != null && int.TryParse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.unitLimitMax, out num))
				{
					num2 = ClassSingleton<MonsterUserDataMng>.Instance.GetMonsterNum();
				}
				break;
			case MasterDataMng.AssetCategory.DIGI_STONE:
				if (DataMng.Instance().RespDataUS_PlayerInfo != null && DataMng.Instance().RespDataUS_PlayerInfo.playerInfo != null)
				{
					num = ConstValue.MAX_DIGISTONE_COUNT;
					num2 = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point;
				}
				break;
			case MasterDataMng.AssetCategory.LINK_POINT:
				if (DataMng.Instance().RespDataUS_PlayerInfo != null && DataMng.Instance().RespDataUS_PlayerInfo.playerInfo != null && int.TryParse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.friendPoint, out num2))
				{
					num = ConstValue.MAX_LINK_POINT_COUNT;
				}
				break;
			case MasterDataMng.AssetCategory.TIP:
				if (DataMng.Instance().RespDataUS_PlayerInfo != null && DataMng.Instance().RespDataUS_PlayerInfo.playerInfo != null && int.TryParse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney, out num2))
				{
					num = ConstValue.MAX_CLUSTER_COUNT;
				}
				break;
			default:
				if (category != MasterDataMng.AssetCategory.MEAT)
				{
					if (category != MasterDataMng.AssetCategory.CHIP)
					{
						num2 = 0;
						num = addCount;
					}
					else if (DataMng.Instance().RespDataUS_PlayerInfo != null && DataMng.Instance().RespDataUS_PlayerInfo.playerInfo != null && ChipDataMng.userChipData != null)
					{
						num = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.chipLimitMax;
						if (ChipDataMng.userChipData.userChipList != null)
						{
							num2 = ChipDataMng.userChipData.userChipList.Length;
						}
						else
						{
							num2 = 0;
						}
					}
				}
				else if (DataMng.Instance().RespDataUS_PlayerInfo != null && DataMng.Instance().RespDataUS_PlayerInfo.playerInfo != null && int.TryParse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.meatLimitMax, out num))
				{
					int.TryParse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.meatNum, out num2);
				}
				break;
			}
			Debug.Assert(num2 != -1 && -1 != num, "所持上限の確認用パラメータの取得失敗.");
			return num < num2 + addCount;
		}

		public static void CalculateNumber(MasterDataMng.AssetCategory category, string assetsValue, int value)
		{
			switch (category)
			{
			case MasterDataMng.AssetCategory.DIGI_STONE:
				if (DataMng.Instance().RespDataUS_PlayerInfo != null && DataMng.Instance().RespDataUS_PlayerInfo.playerInfo != null)
				{
					DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point -= value;
					Debug.Assert(0 <= DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point, "デジストーンが負の値になりました");
				}
				break;
			case MasterDataMng.AssetCategory.LINK_POINT:
				if (DataMng.Instance().RespDataUS_PlayerInfo != null && DataMng.Instance().RespDataUS_PlayerInfo.playerInfo != null)
				{
					int num = 0;
					if (int.TryParse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.friendPoint, out num))
					{
						num -= value;
						DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.friendPoint = num.ToString();
						Debug.Assert(0 <= num, "リンクポイントが負の値になりました");
					}
				}
				break;
			case MasterDataMng.AssetCategory.ITEM:
			{
				int itemId = 0;
				if (int.TryParse(assetsValue, out itemId))
				{
					Singleton<UserDataMng>.Instance.UpdateUserItemNum(itemId, -value);
					Debug.Assert(0 <= Singleton<UserDataMng>.Instance.GetUserItemNumByItemId(itemId), "アイテム所持数が負の値になりました");
				}
				break;
			}
			}
		}
	}
}
