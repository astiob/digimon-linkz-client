using Master;
using System;
using System.Collections.Generic;

namespace Evolution
{
	public static class VersionUpMaterialData
	{
		private static Dictionary<string, GameWebAPI.RespDataMA_GetSoulM.SoulM> soulMDic;

		public static void Initialize()
		{
			VersionUpMaterialData.soulMDic = new Dictionary<string, GameWebAPI.RespDataMA_GetSoulM.SoulM>();
			GameWebAPI.RespDataMA_GetSoulM.SoulM[] soulM = MasterDataMng.Instance().RespDataMA_SoulM.soulM;
			for (int i = 0; i < soulM.Length; i++)
			{
				VersionUpMaterialData.soulMDic.Add(soulM[i].soulId, soulM[i]);
			}
		}

		public static GameWebAPI.RespDataMA_GetSoulM.SoulM GetSoulMasterBySoulId(string soulId)
		{
			if (!VersionUpMaterialData.soulMDic.ContainsKey(soulId))
			{
				Debug.Log("==================== soulId = " + soulId + " Master Not Found!!");
				return null;
			}
			return VersionUpMaterialData.soulMDic[soulId];
		}

		public static List<HaveSoulData> GetVersionUpAlMightyMaterial()
		{
			List<HaveSoulData> list = new List<HaveSoulData>();
			GameWebAPI.RespDataMA_GetSoulM respDataMA_SoulM = MasterDataMng.Instance().RespDataMA_SoulM;
			GameWebAPI.RespDataMA_GetSoulM.SoulM[] soulM = MasterDataMng.Instance().RespDataMA_SoulM.soulM;
			for (int i = 0; i < soulM.Length; i++)
			{
				if (respDataMA_SoulM.IsVersionUpGroup(soulM[i]) && respDataMA_SoulM.IsVersionUpAlMighty(soulM[i]))
				{
					GameWebAPI.UserSoulData[] userSoulData = DataMng.Instance().RespDataUS_SoulInfo.userSoulData;
					for (int j = 0; j < userSoulData.Length; j++)
					{
						if (soulM[i].soulId == userSoulData[j].soulId)
						{
							int num = int.Parse(userSoulData[j].num);
							if (num > 0)
							{
								list.Add(new HaveSoulData
								{
									soulM = soulM[i],
									haveNum = num,
									curUsedNum = 0
								});
							}
						}
					}
				}
			}
			return list;
		}

		public static bool CanChangeToAlmighty(List<HaveSoulData> hsdL, string soulId, int needNum, ref HaveSoulData selectAlMighty)
		{
			GameWebAPI.RespDataMA_GetSoulM respDataMA_SoulM = MasterDataMng.Instance().RespDataMA_SoulM;
			GameWebAPI.RespDataMA_GetSoulM.SoulM soulMasterBySoulId = VersionUpMaterialData.GetSoulMasterBySoulId(soulId);
			if (respDataMA_SoulM.IsVersionUpGroup(soulMasterBySoulId) && !respDataMA_SoulM.IsVersionUpAlMighty(soulMasterBySoulId))
			{
				for (int i = 0; i < hsdL.Count; i++)
				{
					if (hsdL[i].haveNum - hsdL[i].curUsedNum >= needNum)
					{
						selectAlMighty = hsdL[i];
						return true;
					}
				}
			}
			return false;
		}

		private static bool IsMaxLevel(GameWebAPI.RespDataMA_GetMonsterMS.MonsterM monsterM, GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster)
		{
			int num = int.Parse(userMonster.level);
			int num2 = int.Parse(monsterM.maxLevel);
			return num2 <= num;
		}

		private static bool CheckMaterialNum(GameWebAPI.RespDataMA_GetMonsterMS.MonsterM monsterM, List<HaveSoulData> almightyHsdL)
		{
			List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> monsterVersionUpList = ClassSingleton<EvolutionData>.Instance.GetMonsterVersionUpList(monsterM.monsterId);
			int monsterEvolutionMaterialId = monsterVersionUpList[0].monsterEvolutionMaterialId;
			bool result = true;
			GameWebAPI.MonsterEvolutionMaterialMaster.Material evolutionMaterial = EvolutionMaterialData.GetEvolutionMaterial(monsterEvolutionMaterialId);
			for (int i = 1; i <= 7; i++)
			{
				string assetValue = evolutionMaterial.GetAssetValue(i);
				string assetNum = evolutionMaterial.GetAssetNum(i);
				int num = assetNum.ToInt32();
				GameWebAPI.UserSoulData userSoulDataBySID = VersionUpMaterialData.GetUserSoulDataBySID(assetValue);
				int num2 = userSoulDataBySID.num.ToInt32();
				if (num > num2)
				{
					HaveSoulData haveSoulData = null;
					bool flag = VersionUpMaterialData.CanChangeToAlmighty(almightyHsdL, assetValue, num, ref haveSoulData);
					if (!flag)
					{
						result = false;
						break;
					}
				}
			}
			return result;
		}

		private static bool IsEnoughCluster(GameWebAPI.RespDataMA_GetMonsterMS.MonsterM monsterM)
		{
			int num = CalculatorUtil.CalcClusterForVersionUp(monsterM.monsterId);
			int num2 = int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney);
			return num2 >= num;
		}

		public static bool CanVersionUp(GameWebAPI.RespDataMA_GetMonsterMS.MonsterM monsterM, GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster, List<HaveSoulData> almightyHsdL)
		{
			return VersionUpMaterialData.IsMaxLevel(monsterM, userMonster) && VersionUpMaterialData.CheckMaterialNum(monsterM, almightyHsdL) && VersionUpMaterialData.IsEnoughCluster(monsterM);
		}

		public static bool CanVersionUpWithoutMaterial(GameWebAPI.RespDataMA_GetMonsterMS.MonsterM monsterM, GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster)
		{
			return VersionUpMaterialData.IsMaxLevel(monsterM, userMonster) && VersionUpMaterialData.IsEnoughCluster(monsterM);
		}

		public static void SetVersionUpCondition(List<HaveSoulData> almightyHsdL, GUIMonsterIcon monsterIcon, MonsterData monsterData, bool isOnlyDim = false)
		{
			if (VersionUpMaterialData.CanVersionUp(monsterData.monsterM, monsterData.userMonster, almightyHsdL))
			{
				if (isOnlyDim)
				{
					return;
				}
				monsterIcon.SortMess = StringMaster.GetString("CharaIcon-05");
				monsterIcon.SetSortMessageColor(ConstValue.DIGIMON_YELLOW);
			}
			else
			{
				monsterIcon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.NOTACTIVE);
				if (isOnlyDim)
				{
					return;
				}
				monsterIcon.SortMess = StringMaster.GetString("CharaIcon-06");
				monsterIcon.SetSortMessageColor(ConstValue.DIGIMON_BLUE);
			}
		}

		public static List<int> VersionUpPostProcess(EvolutionData.MonsterEvolveData med)
		{
			List<int> list = new List<int>();
			string text = med.mem.effectMonsterId;
			if (text == "0")
			{
				text = "1";
			}
			string monsterGroupId = MonsterDataMng.Instance().GetMonsterMasterByMonsterId(text).monsterGroupId;
			int item = int.Parse(monsterGroupId);
			list.Add(item);
			return list;
		}

		private static GameWebAPI.UserSoulData GetUserSoulDataBySID(string soulId)
		{
			GameWebAPI.UserSoulData userSoulData = EvolutionMaterialData.GetUserEvolutionMaterial(soulId);
			if (userSoulData == null)
			{
				userSoulData = new GameWebAPI.UserSoulData();
				userSoulData.soulId = soulId;
				userSoulData.num = "0";
			}
			return userSoulData;
		}
	}
}
