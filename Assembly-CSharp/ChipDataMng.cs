using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChipDataMng
{
	public const int DEFAULT_USER_MONSTER_ID = 0;

	public static Dictionary<int, GameWebAPI.RespDataMA_ChipM.Chip> ChipM;

	public static Dictionary<int, GameWebAPI.RespDataMA_ChipEffectM.ChipEffect[]> ChipEffectM;

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
	}

	public static GameWebAPI.RespDataCS_ChipListLogic.UserChipList GetUserChipDataByUserChipId(int userChipId)
	{
		GameWebAPI.RespDataCS_ChipListLogic.UserChipList result = null;
		if (ChipDataMng.userChipData.userChipList != null)
		{
			result = ChipDataMng.userChipData.userChipList.FirstOrDefault((GameWebAPI.RespDataCS_ChipListLogic.UserChipList c) => c.userChipId == userChipId);
		}
		return result;
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
					GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChipDataByUserChipId = ChipDataMng.GetUserChipDataByUserChipId(equip.userChipId);
					userChipDataByUserChipId.userChipId = equip.userChipId;
					userChipDataByUserChipId.userMonsterId = ((equip.GetActEnum() != GameWebAPI.ReqDataCS_ChipEquipLogic.ACT.ATTACH) ? 0 : equip.userMonsterId);
					MonsterData md = MonsterDataMng.Instance().GetMonsterDataByUserMonsterID(equip.userMonsterId.ToString(), false);
					md.UpdateSlotInfo(equip);
					md.InitChipInfo();
					if (equip.GetActEnum() == GameWebAPI.ReqDataCS_ChipEquipLogic.ACT.REMOVE)
					{
						Singleton<UserDataMng>.Instance.UpdateUserItemNum(7, -1);
					}
					arg = new GameWebAPI.RequestMonsterList
					{
						SetSendData = delegate(GameWebAPI.ReqDataUS_GetMonsterList param)
						{
							param.userMonsterIds = new int[]
							{
								int.Parse(md.userMonster.userMonsterId)
							};
						},
						OnReceived = delegate(GameWebAPI.RespDataUS_GetMonsterList response)
						{
							md.DuplicateUserMonster(response.userMonsterList[0]);
							MonsterDataMng.Instance().RefreshUserMonsterByUserMonsterList(response.userMonsterList);
						}
					};
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
					GameWebAPI.RespDataCS_ChipListLogic.UserChipList afterChip = ChipDataMng.GetUserChipDataByUserChipId(baseChipId);
					afterChip.chipId = resData.userChip.chipId;
					List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList> list = ChipDataMng.userChipData.userChipList.ToList<GameWebAPI.RespDataCS_ChipListLogic.UserChipList>();
					foreach (int materialChip in materialChipIds)
					{
						list.RemoveAll((GameWebAPI.RespDataCS_ChipListLogic.UserChipList c) => c.userChipId == materialChip);
					}
					ChipDataMng.userChipData.userChipList = list.ToArray();
					if (afterChip.userMonsterId != 0)
					{
						UnityEngine.Debug.Log("モンスター情報を更新します");
						MonsterData md = MonsterDataMng.Instance().GetMonsterDataByUserMonsterID(afterChip.userMonsterId.ToString(), false);
						arg = new GameWebAPI.RequestMonsterList
						{
							SetSendData = delegate(GameWebAPI.ReqDataUS_GetMonsterList param)
							{
								param.userMonsterIds = new int[]
								{
									afterChip.userMonsterId
								};
							},
							OnReceived = delegate(GameWebAPI.RespDataUS_GetMonsterList response)
							{
								md.DuplicateUserMonster(response.userMonsterList[0]);
								MonsterDataMng.Instance().RefreshUserMonsterByUserMonsterList(response.userMonsterList);
							}
						};
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
					GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.SlotInfo slotInfo = MonsterDataMng.Instance().userMonsterSlotInfoListLogic.slotInfo.FirstOrDefault((GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.SlotInfo c) => c.userMonsterId == int.Parse(md.userMonster.userMonsterId));
					slotInfo.manage.extraSlotNum++;
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
		GameWebAPI.MonsterSlotInfoListLogic monsterSlotInfoListLogic = new GameWebAPI.MonsterSlotInfoListLogic();
		monsterSlotInfoListLogic.OnReceived = delegate(GameWebAPI.RespDataCS_MonsterSlotInfoListLogic response)
		{
			MonsterDataMng.Instance().userMonsterSlotInfoListLogic = response;
		};
		GameWebAPI.MonsterSlotInfoListLogic request = monsterSlotInfoListLogic;
		return new APIRequestTask(request, requestRetry);
	}

	public static GameWebAPI.MonsterSlotInfoListLogic RequestAPIMonsterSlotInfo(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList[] addMonsterList, Action<int> callBack = null)
	{
		List<int> userMonsterIdList = new List<int>();
		foreach (GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonsterList in addMonsterList)
		{
			userMonsterIdList.Add(int.Parse(userMonsterList.userMonsterId));
		}
		return new GameWebAPI.MonsterSlotInfoListLogic
		{
			SetSendData = delegate(GameWebAPI.ReqDataCS_MonsterSlotInfoListLogic param)
			{
				param.userMonsterId = userMonsterIdList.ToArray();
			},
			OnReceived = delegate(GameWebAPI.RespDataCS_MonsterSlotInfoListLogic resData)
			{
				if (resData.resultCode == 1)
				{
					if (MonsterDataMng.Instance().userMonsterSlotInfoListLogic != null && MonsterDataMng.Instance().userMonsterSlotInfoListLogic.slotInfo != null && MonsterDataMng.Instance().userMonsterSlotInfoListLogic.slotInfo.Count<GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.SlotInfo>() > 0)
					{
						UnityEngine.Debug.Log(MonsterDataMng.Instance().userMonsterSlotInfoListLogic.slotInfo.Count<GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.SlotInfo>());
						List<GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.SlotInfo> list = new List<GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.SlotInfo>();
						GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.SlotInfo[] slotInfo = MonsterDataMng.Instance().userMonsterSlotInfoListLogic.slotInfo;
						list.AddRange(resData.slotInfo);
						foreach (GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.SlotInfo slotInfo2 in slotInfo)
						{
							bool flag = true;
							foreach (GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.SlotInfo slotInfo3 in list)
							{
								if (slotInfo3.userMonsterId == slotInfo2.userMonsterId)
								{
									flag = false;
									break;
								}
							}
							if (flag)
							{
								list.Add(slotInfo2);
							}
						}
						MonsterDataMng.Instance().userMonsterSlotInfoListLogic.slotInfo = list.ToArray();
					}
					else
					{
						UnityEngine.Debug.Log("B");
						MonsterDataMng.Instance().userMonsterSlotInfoListLogic = resData;
					}
				}
				if (callBack != null)
				{
					callBack(resData.resultCode);
				}
			}
		};
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
}
