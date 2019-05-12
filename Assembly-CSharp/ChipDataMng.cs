using Chip;
using Monster;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ChipDataMng
{
	public const int DEFAULT_USER_MONSTER_ID = 0;

	private static ChipSlotData userChipSlotData;

	private static Dictionary<int, GameWebAPI.RespDataMA_ChipM.Chip> ChipM;

	private static Dictionary<int, GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[]> ChipEffectM;

	public static GameWebAPI.RespDataCS_ChipListLogic userChipData { get; set; }

	public static void ClearCache()
	{
		if (ChipDataMng.ChipM != null)
		{
			ChipDataMng.ChipM.Clear();
			ChipDataMng.ChipM = null;
		}
		if (ChipDataMng.ChipEffectM != null)
		{
			ChipDataMng.ChipEffectM.Clear();
			ChipDataMng.ChipEffectM = null;
		}
		if (ChipDataMng.userChipSlotData == null)
		{
			ChipDataMng.userChipSlotData = new ChipSlotData();
		}
		ChipDataMng.userChipSlotData.Initialize();
	}

	public static GameWebAPI.RespDataCS_ChipListLogic.UserChipList GetUserChip(int userChipId)
	{
		global::Debug.Assert(null != ChipDataMng.userChipData, "所持チップ情報がNULL (userChipData)");
		global::Debug.Assert(null != ChipDataMng.userChipData.userChipList, "所持チップ一覧がNULL (userChipData.userChipList)");
		GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChipList = ChipDataMng.userChipData.userChipList.FirstOrDefault((GameWebAPI.RespDataCS_ChipListLogic.UserChipList c) => c.userChipId == userChipId);
		global::Debug.Assert(null != userChipList, "所持していないユーザーチップID (" + userChipId + ")");
		return userChipList;
	}

	public static GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] GetMonsterChipList(string userMonsterId)
	{
		GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] result = null;
		if (ChipDataMng.userChipData.userChipList != null)
		{
			result = ChipDataMng.userChipData.userChipList.Where((GameWebAPI.RespDataCS_ChipListLogic.UserChipList c) => c.userMonsterId == int.Parse(userMonsterId)).ToArray<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>();
		}
		return result;
	}

	public static void DeleteUserChipData(int userChipId)
	{
		List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList> list = new List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>(ChipDataMng.userChipData.userChipList);
		if (ChipDataMng.userChipData != null)
		{
			for (int i = 0; i < ChipDataMng.userChipData.userChipList.Length; i++)
			{
				if (ChipDataMng.userChipData.userChipList[i].userChipId == userChipId)
				{
					list.RemoveAt(i);
					ChipDataMng.userChipData.userChipList = list.ToArray();
					ChipDataMng.userChipData.count--;
					break;
				}
			}
		}
	}

	public static void DeleteEquipChip(string[] userMonsterIdList)
	{
		foreach (string userMonsterId in userMonsterIdList)
		{
			GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] monsterChipList = ChipDataMng.GetMonsterChipList(userMonsterId);
			if (monsterChipList != null)
			{
				foreach (GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChipList in monsterChipList)
				{
					ChipDataMng.DeleteUserChipData(userChipList.userChipId);
				}
			}
		}
	}

	public static void AddUserChipDataList(List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList> ucList)
	{
		List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList> list;
		if (ChipDataMng.userChipData.userChipList != null)
		{
			list = new List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>(ChipDataMng.userChipData.userChipList);
		}
		else
		{
			list = new List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>();
		}
		for (int i = 0; i < ucList.Count; i++)
		{
			list.Add(ucList[i]);
			ChipDataMng.userChipData.count++;
		}
		ChipDataMng.userChipData.userChipList = list.ToArray();
	}

	public static Dictionary<int, GameWebAPI.RespDataMA_ChipM.Chip> GetDictionaryChipM()
	{
		if (ChipDataMng.ChipM == null)
		{
			ChipDataMng.ChipM = MasterDataMng.Instance().RespDataMA_ChipMaster.chipM.ToDictionary((GameWebAPI.RespDataMA_ChipM.Chip x) => int.Parse(x.chipId));
		}
		return ChipDataMng.ChipM;
	}

	public static GameWebAPI.RespDataMA_ChipM.Chip GetChipMaster(int chipId)
	{
		global::Debug.Assert(ChipDataMng.ChipM.ContainsKey(chipId), "存在しないチップID (" + chipId + ")");
		return ChipDataMng.ChipM[chipId];
	}

	public static GameWebAPI.RespDataMA_ChipM.Chip GetChipMaster(string chipId)
	{
		return ChipDataMng.GetChipMaster(chipId);
	}

	public static Dictionary<int, GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[]> GetDictionaryChipEffectM()
	{
		if (ChipDataMng.ChipEffectM == null)
		{
			ChipDataMng.ChipEffectM = new Dictionary<int, GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[]>();
			KeyValuePair<int, GameWebAPI.RespDataMA_ChipM.Chip> chip;
			foreach (KeyValuePair<int, GameWebAPI.RespDataMA_ChipM.Chip> chip2 in ChipDataMng.GetDictionaryChipM())
			{
				chip = chip2;
				ChipDataMng.ChipEffectM[chip.Key] = MasterDataMng.Instance().RespDataMA_ChipEffectMaster.chipEffectM.Where((GameWebAPI.RespDataMA_ChipEffectM.ChipEffect e) => e.chipId == chip.Value.chipId).ToArray<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
			}
		}
		return ChipDataMng.ChipEffectM;
	}

	public static GameWebAPI.RespDataMA_ChipM.Chip GetChipMainData(string chipId)
	{
		GameWebAPI.RespDataMA_ChipM.Chip chip = null;
		ChipDataMng.GetDictionaryChipM().TryGetValue(int.Parse(chipId), out chip);
		chip = ChipDataMng.SetChipIconPath(chip);
		chip = ChipDataMng.SetChipSellPrice(chip);
		return chip;
	}

	public static GameWebAPI.RespDataMA_ChipM.Chip GetChipMainData(GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChip)
	{
		GameWebAPI.RespDataMA_ChipM.Chip chip = null;
		ChipDataMng.GetDictionaryChipM().TryGetValue(userChip.chipId, out chip);
		chip = ChipDataMng.SetChipIconPath(chip);
		chip = ChipDataMng.SetChipSellPrice(chip);
		return chip;
	}

	public static GameWebAPI.RespDataMA_ChipEffectM.ChipEffect GetChipEffectDataToId(string chipEffectId)
	{
		GameWebAPI.RespDataMA_ChipEffectM.ChipEffect result = null;
		GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] chipEffectM = MasterDataMng.Instance().RespDataMA_ChipEffectMaster.chipEffectM;
		foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect in chipEffectM)
		{
			if (chipEffect.chipEffectId == chipEffectId)
			{
				result = chipEffect;
				break;
			}
		}
		return result;
	}

	public static GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] GetChipEffectData(string chipId)
	{
		GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] result = null;
		ChipDataMng.GetDictionaryChipEffectM().TryGetValue(int.Parse(chipId), out result);
		return result;
	}

	public static GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] GetChipEffectData(GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChip)
	{
		GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[] result = null;
		ChipDataMng.GetDictionaryChipEffectM().TryGetValue(userChip.chipId, out result);
		return result;
	}

	public static GameWebAPI.RespDataMA_ChipM.Chip GetChipEnhancedData(string chipId)
	{
		GameWebAPI.RespDataMA_ChipM.Chip beforeChip = null;
		ChipDataMng.GetDictionaryChipM().TryGetValue(int.Parse(chipId), out beforeChip);
		IEnumerable<GameWebAPI.RespDataMA_ChipM.Chip> source = MasterDataMng.Instance().RespDataMA_ChipMaster.chipM.Where((GameWebAPI.RespDataMA_ChipM.Chip c) => c.chipGroupId == beforeChip.chipGroupId && int.Parse(c.rank) > int.Parse(beforeChip.rank));
		GameWebAPI.RespDataMA_ChipM.Chip chip = null;
		if (source.Count<GameWebAPI.RespDataMA_ChipM.Chip>() > 0)
		{
			chip = source.OrderBy((GameWebAPI.RespDataMA_ChipM.Chip c) => int.Parse(c.rank)).ToArray<GameWebAPI.RespDataMA_ChipM.Chip>()[0];
			chip = ChipDataMng.SetChipIconPath(chip);
			chip = ChipDataMng.SetChipSellPrice(chip);
		}
		return chip;
	}

	public static APIRequestTask RequestAPIChipList(bool requestRetry = true)
	{
		GameWebAPI.ReqDataCS_ChipListLogic reqDataCS_ChipListLogic = new GameWebAPI.ReqDataCS_ChipListLogic();
		reqDataCS_ChipListLogic.OnReceived = delegate(GameWebAPI.RespDataCS_ChipListLogic response)
		{
			ChipDataMng.userChipData = response;
		};
		GameWebAPI.ReqDataCS_ChipListLogic request = reqDataCS_ChipListLogic;
		return new APIRequestTask(request, requestRetry);
	}

	public static GameWebAPI.ChipEquipLogic RequestAPIChipEquip(GameWebAPI.ReqDataCS_ChipEquipLogic equip, Action<int, GameWebAPI.RequestMonsterList> callBack = null)
	{
		return new GameWebAPI.ChipEquipLogic
		{
			SetSendData = delegate(GameWebAPI.ReqDataCS_ChipEquipLogic param)
			{
				param.act = equip.act;
				param.dispNum = equip.dispNum;
				param.type = equip.type;
				param.userChipId = equip.userChipId;
				param.userMonsterId = equip.userMonsterId;
			},
			OnReceived = delegate(GameWebAPI.RespDataCS_ChipEquipLogic resData)
			{
				GameWebAPI.RequestMonsterList arg = null;
				if (resData.resultCode == 1)
				{
					GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChip = ChipDataMng.GetUserChip(equip.userChipId);
					userChip.userChipId = equip.userChipId;
					userChip.userMonsterId = ((equip.GetActEnum() != GameWebAPI.ReqDataCS_ChipEquipLogic.ACT.ATTACH) ? 0 : equip.userMonsterId);
					MonsterData md = ClassSingleton<MonsterUserDataMng>.Instance.GetUserMonster(equip.userMonsterId.ToString());
					md.GetChipEquip().UpdateEquipList(equip);
					if (equip.GetActEnum() == GameWebAPI.ReqDataCS_ChipEquipLogic.ACT.REMOVE)
					{
						Singleton<UserDataMng>.Instance.UpdateUserItemNum(7, -1);
					}
					GameWebAPI.RequestMonsterList requestMonsterList = new GameWebAPI.RequestMonsterList();
					requestMonsterList.SetSendData = delegate(GameWebAPI.ReqDataUS_GetMonsterList param)
					{
						param.userMonsterIds = new int[]
						{
							int.Parse(md.userMonster.userMonsterId)
						};
					};
					requestMonsterList.OnReceived = delegate(GameWebAPI.RespDataUS_GetMonsterList response)
					{
						ClassSingleton<MonsterUserDataMng>.Instance.UpdateUserMonsterData(response.userMonsterList);
					};
					arg = requestMonsterList;
				}
				if (callBack != null)
				{
					callBack(resData.resultCode, arg);
				}
			}
		};
	}

	public static GameWebAPI.ChipFusionLogic RequestAPIChipFusion(int baseChipId, int[] materialChipIds, Action<int, GameWebAPI.RequestMonsterList> callBack = null)
	{
		return new GameWebAPI.ChipFusionLogic
		{
			SetSendData = delegate(GameWebAPI.ReqDataCS_ChipFusionLogic param)
			{
				param.baseChip = baseChipId;
				param.materialChip = materialChipIds;
			},
			OnReceived = delegate(GameWebAPI.RespDataCS_ChipFusionLogic resData)
			{
				GameWebAPI.RequestMonsterList arg = null;
				if (resData.resultCode == 1)
				{
					GameWebAPI.RespDataCS_ChipListLogic.UserChipList afterChip = ChipDataMng.GetUserChip(baseChipId);
					afterChip.chipId = resData.userChip.chipId;
					List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList> list = ChipDataMng.userChipData.userChipList.ToList<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>();
					foreach (int materialChip in materialChipIds)
					{
						list.RemoveAll((GameWebAPI.RespDataCS_ChipListLogic.UserChipList c) => c.userChipId == materialChip);
					}
					ChipDataMng.userChipData.userChipList = list.ToArray();
					if (afterChip.IsUse())
					{
						GameWebAPI.RequestMonsterList requestMonsterList = new GameWebAPI.RequestMonsterList();
						requestMonsterList.SetSendData = delegate(GameWebAPI.ReqDataUS_GetMonsterList param)
						{
							param.userMonsterIds = new int[]
							{
								afterChip.userMonsterId
							};
						};
						requestMonsterList.OnReceived = delegate(GameWebAPI.RespDataUS_GetMonsterList response)
						{
							ClassSingleton<MonsterUserDataMng>.Instance.UpdateUserMonsterData(response.userMonsterList);
						};
						arg = requestMonsterList;
					}
				}
				if (callBack != null)
				{
					callBack(resData.resultCode, arg);
				}
			}
		};
	}

	public static GameWebAPI.ChipUnlockExtraSlotLogic RequestAPIChipUnlockExtraSlot(MonsterData md, int dispNum, int useEjectItemCnt, Action<int> callBack = null)
	{
		return new GameWebAPI.ChipUnlockExtraSlotLogic
		{
			SetSendData = delegate(GameWebAPI.ReqDataCS_ChipUnlockExtraSlotLogic param)
			{
				param.dispNum = dispNum;
				param.userMonsterId = int.Parse(md.userMonster.userMonsterId);
			},
			OnReceived = delegate(GameWebAPI.RespDataCS_ChipUnlockExtraSlotLogic resData)
			{
				if (resData.resultCode == 1)
				{
					ChipClientSlotInfo slotInfo = ChipDataMng.userChipSlotData.GetSlotInfo(md.userMonster.userMonsterId);
					slotInfo.GetSlotNum().extraSlotNum++;
					Singleton<UserDataMng>.Instance.UpdateUserItemNum(6, useEjectItemCnt * -1);
				}
				if (callBack != null)
				{
					callBack(resData.resultCode);
				}
			}
		};
	}

	public static GameWebAPI.ChipSellLogic RequestAPIChipSell(int[] sellUserChipIdList, Action<int> callBack = null)
	{
		return new GameWebAPI.ChipSellLogic
		{
			SetSendData = delegate(GameWebAPI.ReqDataCS_ChipSellLogic param)
			{
				param.materialChip = sellUserChipIdList;
			},
			OnReceived = delegate(GameWebAPI.RespDataCS_ChipSellLogic resData)
			{
				if (resData.resultCode == 1)
				{
					foreach (int userChipId in sellUserChipIdList)
					{
						ChipDataMng.DeleteUserChipData(userChipId);
					}
				}
				if (callBack != null)
				{
					callBack(resData.resultCode);
				}
			}
		};
	}

	public static APIRequestTask RequestAPIMonsterSlotInfoList(bool requestRetry = true)
	{
		return ChipAPIRequest.RequestAPIMonsterSlotInfoList(null, requestRetry);
	}

	public static GameWebAPI.MonsterSlotInfoListLogic RequestAPIMonsterSlotInfo(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList[] addMonsterList)
	{
		int[] array = null;
		if (0 < addMonsterList.Length)
		{
			array = new int[addMonsterList.Length];
			for (int i = 0; i < addMonsterList.Length; i++)
			{
				array[i] = int.Parse(addMonsterList[i].userMonsterId);
			}
		}
		return ChipAPIRequest.RequestAPIMonsterSlotInfo(array);
	}

	public static GameWebAPI.RespDataMA_ChipM.Chip SetChipIconPath(GameWebAPI.RespDataMA_ChipM.Chip chip)
	{
		if (string.IsNullOrEmpty(chip.GetIconPath()))
		{
			chip.SetIconPath(string.Format("ChipThumbnail/{0}", chip.icon));
		}
		return chip;
	}

	public static GameWebAPI.RespDataMA_ChipM.Chip SetChipSellPrice(GameWebAPI.RespDataMA_ChipM.Chip chip)
	{
		chip.SetSellPrice(int.Parse(chip.rank) * ConstValue.BASE_CHIP_SELL_PRICE);
		return chip;
	}

	public static void MakePrefabByChipData(GameWebAPI.RespDataMA_ChipM.Chip masterChip, GameObject emptyObject, Vector3 position, Vector3 scale, Action<ChipIcon> actCB = null, int texSizeWidth = -1, int texSizeHeight = -1, bool colliderEnable = true)
	{
		Action<UnityEngine.Object> actEnd = delegate(UnityEngine.Object obj)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(obj) as GameObject;
			gameObject.transform.SetParent(emptyObject.transform.parent);
			gameObject.transform.localPosition = position;
			gameObject.transform.localScale = scale;
			BoxCollider component = gameObject.GetComponent<BoxCollider>();
			if (component != null)
			{
				component.enabled = colliderEnable;
			}
			UIWidget component2 = emptyObject.GetComponent<UIWidget>();
			DepthController component3 = gameObject.GetComponent<DepthController>();
			component3.AddWidgetDepth(component2.depth);
			ChipIcon component4 = gameObject.GetComponent<ChipIcon>();
			component4.SetData(masterChip, texSizeWidth, texSizeHeight);
			if (actCB != null)
			{
				actCB(component4);
			}
		};
		AssetDataMng.Instance().LoadObjectASync("UICommon/ListParts/ListPartsChip", actEnd);
	}

	public static ChipSlotData GetUserChipSlotData()
	{
		return ChipDataMng.userChipSlotData;
	}
}
