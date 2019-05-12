using Master;
using Monster;
using System;
using System.Collections.Generic;

namespace Evolution
{
	public sealed class EvolutionData : ClassSingleton<EvolutionData>
	{
		private Dictionary<string, List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution>> monsterEvolutionDic;

		private Dictionary<string, List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution>> nextMonsterEvolutionTable;

		private Dictionary<string, List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution>> nextMonsterVersionUpTable;

		private Dictionary<string, List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution>> baseMonsterEvolutionTable;

		private void GetBeforeEvolutionList(string monsterId, ref List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> evolutionList)
		{
			GameWebAPI.RespDataMA_GetMonsterEvolutionM respDataMA_MonsterEvolutionM = MasterDataMng.Instance().RespDataMA_MonsterEvolutionM;
			for (int i = 0; i < respDataMA_MonsterEvolutionM.monsterEvolutionM.Length; i++)
			{
				if (respDataMA_MonsterEvolutionM.monsterEvolutionM[i].nextMonsterId == monsterId && respDataMA_MonsterEvolutionM.monsterEvolutionM[i].IsEvolution())
				{
					evolutionList.Add(respDataMA_MonsterEvolutionM.monsterEvolutionM[i]);
				}
			}
		}

		private void GetBeforeEvolutionChildList(string monsterId, ref List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> evolutionList)
		{
			GameWebAPI.RespDataMA_GetMonsterEvolutionRouteM respDataMA_MonsterEvolutionRouteM = MasterDataMng.Instance().RespDataMA_MonsterEvolutionRouteM;
			for (int i = 0; i < respDataMA_MonsterEvolutionRouteM.monsterEvolutionRouteM.Length; i++)
			{
				GameWebAPI.RespDataMA_GetMonsterEvolutionRouteM.MonsterEvolutionRouteM master = respDataMA_MonsterEvolutionRouteM.monsterEvolutionRouteM[i];
				if (master.childhood2MonsterId == monsterId && !evolutionList.Exists((GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution x) => x.baseMonsterId == master.childhood1MonsterId))
				{
					GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution item = new GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution
					{
						baseMonsterId = master.childhood1MonsterId,
						type = "1"
					};
					evolutionList.Add(item);
				}
				else if (master.growthMonsterId == monsterId && !evolutionList.Exists((GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution x) => x.baseMonsterId == master.childhood1MonsterId))
				{
					GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution item2 = new GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution
					{
						baseMonsterId = master.childhood2MonsterId,
						type = "1"
					};
					evolutionList.Add(item2);
				}
			}
		}

		private void GetAfterEvolutionChildList(string monsterId, ref List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> evolutionList)
		{
			GameWebAPI.RespDataMA_GetMonsterEvolutionRouteM respDataMA_MonsterEvolutionRouteM = MasterDataMng.Instance().RespDataMA_MonsterEvolutionRouteM;
			for (int i = 0; i < respDataMA_MonsterEvolutionRouteM.monsterEvolutionRouteM.Length; i++)
			{
				GameWebAPI.RespDataMA_GetMonsterEvolutionRouteM.MonsterEvolutionRouteM master = respDataMA_MonsterEvolutionRouteM.monsterEvolutionRouteM[i];
				if (master.childhood1MonsterId == monsterId && !evolutionList.Exists((GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution x) => x.nextMonsterId == master.childhood2MonsterId))
				{
					GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution item = new GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution
					{
						nextMonsterId = master.childhood2MonsterId,
						type = "1"
					};
					evolutionList.Add(item);
				}
				else if (master.childhood2MonsterId == monsterId && !evolutionList.Exists((GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution x) => x.nextMonsterId == master.childhood2MonsterId))
				{
					GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution item2 = new GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution
					{
						nextMonsterId = master.growthMonsterId,
						type = "1"
					};
					evolutionList.Add(item2);
				}
			}
		}

		private void GetNextVersionUpList(string monsterId, ref List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> versionUpList)
		{
			GameWebAPI.RespDataMA_GetMonsterEvolutionM respDataMA_MonsterEvolutionM = MasterDataMng.Instance().RespDataMA_MonsterEvolutionM;
			for (int i = 0; i < respDataMA_MonsterEvolutionM.monsterEvolutionM.Length; i++)
			{
				if (respDataMA_MonsterEvolutionM.monsterEvolutionM[i].baseMonsterId == monsterId && respDataMA_MonsterEvolutionM.monsterEvolutionM[i].IsVersionUp())
				{
					versionUpList.Add(respDataMA_MonsterEvolutionM.monsterEvolutionM[i]);
				}
			}
		}

		public bool CheckMaterialNum(int evolutionMaterialId)
		{
			bool result = true;
			GameWebAPI.MonsterEvolutionMaterialMaster.Material evolutionMaterial = EvolutionMaterialData.GetEvolutionMaterial(evolutionMaterialId);
			for (int i = 1; i <= 7; i++)
			{
				string assetValue = evolutionMaterial.GetAssetValue(i);
				string assetNum = evolutionMaterial.GetAssetNum(i);
				int num = assetNum.ToInt32();
				GameWebAPI.UserSoulData userSoulDataBySID = this.GetUserSoulDataBySID(assetValue);
				int num2 = userSoulDataBySID.num.ToInt32();
				if (num > num2)
				{
					result = false;
					break;
				}
			}
			return result;
		}

		private bool AnyClearEvolutionCondition(List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> masterList, bool isMaxLevel)
		{
			bool result = false;
			int num = int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney);
			for (int i = 0; i < masterList.Count; i++)
			{
				string nextMonsterId = masterList[i].nextMonsterId;
				string effectType = masterList[i].effectType;
				if (effectType == "1" || effectType == "3" || effectType == "4" || effectType == "5")
				{
					if (isMaxLevel && this.CheckMaterialNum(masterList[i].monsterEvolutionMaterialId))
					{
						int num2 = EvolutionData.CalcClusterForEvolve(nextMonsterId);
						if (num2 <= num)
						{
							result = true;
							break;
						}
					}
				}
				else if (this.CheckMaterialNum(masterList[i].monsterEvolutionMaterialId))
				{
					int num3 = EvolutionData.CalcClusterForModeChange(nextMonsterId);
					if (num3 <= num)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		public void Initialize()
		{
			GameWebAPI.RespDataMA_GetMonsterEvolutionM respDataMA_MonsterEvolutionM = MasterDataMng.Instance().RespDataMA_MonsterEvolutionM;
			this.monsterEvolutionDic = this.MakeEvolutionDic(respDataMA_MonsterEvolutionM.monsterEvolutionM);
			if (this.baseMonsterEvolutionTable != null)
			{
				this.baseMonsterEvolutionTable.Clear();
			}
			else
			{
				this.baseMonsterEvolutionTable = new Dictionary<string, List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution>>();
			}
			if (this.nextMonsterVersionUpTable != null)
			{
				this.nextMonsterVersionUpTable.Clear();
			}
			else
			{
				this.nextMonsterVersionUpTable = new Dictionary<string, List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution>>();
			}
		}

		public string GetEvolutionEffectType(string baseMonsterId, string nextMonsterId)
		{
			MonsterClientMaster monsterMasterByMonsterId = MonsterMaster.GetMonsterMasterByMonsterId(baseMonsterId);
			return this.GetEvolutionEffectType(baseMonsterId, monsterMasterByMonsterId.Group.growStep, nextMonsterId);
		}

		public string GetEvolutionEffectType(string baseMonsterId, string growStep, string nextMonsterId)
		{
			string result = "1";
			List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> afterMonsterEvolutionList = this.GetAfterMonsterEvolutionList(baseMonsterId, growStep);
			if (afterMonsterEvolutionList != null && 0 < afterMonsterEvolutionList.Count)
			{
				for (int i = 0; i < afterMonsterEvolutionList.Count; i++)
				{
					if (afterMonsterEvolutionList[i].nextMonsterId == nextMonsterId)
					{
						result = afterMonsterEvolutionList[i].effectType;
						break;
					}
				}
			}
			return result;
		}

		private Dictionary<string, List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution>> MakeEvolutionDic(GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution[] evoS)
		{
			Dictionary<string, List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution>> dictionary = new Dictionary<string, List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution>>();
			for (int i = 0; i < evoS.Length; i++)
			{
				if (evoS[i].IsEvolution())
				{
					List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> list = null;
					if (!dictionary.TryGetValue(evoS[i].baseMonsterId, out list))
					{
						list = new List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution>();
						dictionary.Add(evoS[i].baseMonsterId, list);
					}
					list.Add(evoS[i]);
				}
			}
			return dictionary;
		}

		public List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> GetEvoList(string monsterId)
		{
			if (!this.monsterEvolutionDic.ContainsKey(monsterId))
			{
				return new List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution>();
			}
			return this.monsterEvolutionDic[monsterId];
		}

		public List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> GetBeforeMonsterEvolutionList(string monsterId, string growStep)
		{
			List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> list = null;
			if (!this.baseMonsterEvolutionTable.TryGetValue(monsterId, out list))
			{
				int growStep2 = (int)MonsterGrowStepData.ToGrowStep(growStep);
				list = new List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution>();
				if (!MonsterGrowStepData.IsEggScope(growStep2))
				{
					if (MonsterGrowStepData.IsChild1Scope(growStep2) || MonsterGrowStepData.IsChild2Scope(growStep2) || MonsterGrowStepData.IsGrowingScope(growStep2))
					{
						this.GetBeforeEvolutionChildList(monsterId, ref list);
					}
					else
					{
						this.GetBeforeEvolutionList(monsterId, ref list);
					}
				}
				this.baseMonsterEvolutionTable.Add(monsterId, list);
			}
			return list;
		}

		public List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> GetAfterMonsterEvolutionList(string monsterId, string growStep)
		{
			List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> list = null;
			if (!this.monsterEvolutionDic.TryGetValue(monsterId, out list))
			{
				int growStep2 = (int)MonsterGrowStepData.ToGrowStep(growStep);
				list = new List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution>();
				if (MonsterGrowStepData.IsEggScope(growStep2) || MonsterGrowStepData.IsChild1Scope(growStep2) || MonsterGrowStepData.IsChild2Scope(growStep2))
				{
					this.GetAfterEvolutionChildList(monsterId, ref list);
					this.monsterEvolutionDic.Add(monsterId, list);
				}
			}
			return list;
		}

		public List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> GetMonsterVersionUpList(string monsterId)
		{
			List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> list = null;
			if (!this.nextMonsterVersionUpTable.TryGetValue(monsterId, out list))
			{
				list = new List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution>();
				this.GetNextVersionUpList(monsterId, ref list);
				this.nextMonsterVersionUpTable.Add(monsterId, list);
			}
			return list;
		}

		public List<EvolutionData.MonsterEvolveData> GetEvolveListByMonsterData(MonsterData md)
		{
			List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> evoList = this.GetEvoList(md.monsterM.monsterId);
			List<EvolutionData.MonsterEvolveData> result = new List<EvolutionData.MonsterEvolveData>();
			this.GetEvolutionDataList(md, evoList, ref result);
			return result;
		}

		public void EvolvePostProcess(List<EvolutionData.MonsterEvolveItem> meiL)
		{
			for (int i = 0; i < meiL.Count; i++)
			{
				if (meiL[i].catId == ConstValue.EVOLVE_ITEM_SOUL)
				{
					int num = int.Parse(meiL[i].sd_item.num) - meiL[i].need_num;
					meiL[i].sd_item.num = num.ToString();
				}
			}
		}

		private bool CanEvolve(MonsterData monsterData)
		{
			string monsterId = monsterData.userMonster.monsterId;
			int num = int.Parse(monsterData.userMonster.level);
			int num2 = int.Parse(monsterData.monsterM.maxLevel);
			bool isMaxLevel = num2 <= num;
			List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> evoList = this.GetEvoList(monsterId);
			return this.AnyClearEvolutionCondition(evoList, isMaxLevel);
		}

		public bool CanEvolution(string monsterId, bool isMaxLevel)
		{
			List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> evoList = this.GetEvoList(monsterId);
			return this.AnyClearEvolutionCondition(evoList, isMaxLevel);
		}

		public void CheckEvolveable(GUIMonsterIcon monsterIcon, MonsterData monsterData, bool isOnlyDim = false)
		{
			bool flag = this.CanEvolve(monsterData);
			if (flag)
			{
				if (isOnlyDim)
				{
					return;
				}
				monsterIcon.SortMess = StringMaster.GetString("CharaIcon-01");
				monsterIcon.SetSortMessageColor(ConstValue.DIGIMON_YELLOW);
			}
			else
			{
				monsterIcon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.NOTACTIVE);
				if (isOnlyDim)
				{
					return;
				}
				monsterIcon.SortMess = StringMaster.GetString("CharaIcon-02");
				monsterIcon.SetSortMessageColor(ConstValue.DIGIMON_BLUE);
			}
		}

		private void GetEvolutionDataList(MonsterData md, List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> memS, ref List<EvolutionData.MonsterEvolveData> medList)
		{
			for (int i = 0; i < memS.Count; i++)
			{
				EvolutionData.MonsterEvolveData monsterEvolveData = new EvolutionData.MonsterEvolveData();
				monsterEvolveData.md = md;
				monsterEvolveData.md_next = MonsterDataMng.Instance().CreateMonsterDataByMID(memS[i].nextMonsterId);
				monsterEvolveData.mem = memS[i];
				GameWebAPI.MonsterEvolutionMaterialMaster.Material evolutionMaterial = EvolutionMaterialData.GetEvolutionMaterial(memS[i].monsterEvolutionMaterialId);
				monsterEvolveData.itemList = new List<EvolutionData.MonsterEvolveItem>();
				for (int j = 1; j <= 7; j++)
				{
					EvolutionData.MonsterEvolveItem monsterEvolveItem = new EvolutionData.MonsterEvolveItem();
					string assetCategoryId = evolutionMaterial.GetAssetCategoryId(j);
					string assetValue = evolutionMaterial.GetAssetValue(j);
					string assetNum = evolutionMaterial.GetAssetNum(j);
					if (assetCategoryId == ConstValue.EVOLVE_ITEM_MONS.ToString())
					{
						monsterEvolveItem.catId = ConstValue.EVOLVE_ITEM_MONS;
						monsterEvolveItem.need_num = int.Parse(assetNum);
						List<MonsterData> monsterList = ClassSingleton<MonsterUserDataMng>.Instance.GetMonsterList(assetValue);
						monsterEvolveItem.haveNum = monsterList.Count;
						monsterEvolveData.itemList.Add(monsterEvolveItem);
					}
					else if (assetCategoryId == ConstValue.EVOLVE_ITEM_SOUL.ToString())
					{
						monsterEvolveItem.catId = ConstValue.EVOLVE_ITEM_SOUL;
						monsterEvolveItem.need_num = int.Parse(assetNum);
						monsterEvolveItem.sd_item = this.GetUserSoulDataBySID(assetValue);
						monsterEvolveItem.haveNum = int.Parse(monsterEvolveItem.sd_item.num);
						monsterEvolveData.itemList.Add(monsterEvolveItem);
					}
				}
				medList.Add(monsterEvolveData);
			}
		}

		public List<EvolutionData.MonsterEvolveData> GetVersionUpList(MonsterData monsterData)
		{
			List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> memS = new List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution>();
			this.GetNextVersionUpList(monsterData.userMonster.monsterId, ref memS);
			List<EvolutionData.MonsterEvolveData> result = new List<EvolutionData.MonsterEvolveData>();
			this.GetEvolutionDataList(monsterData, memS, ref result);
			return result;
		}

		public string GetEvolveItemIconPathByID(string id)
		{
			return "EvolveItemThumbnail/" + id + "/thumb";
		}

		private GameWebAPI.UserSoulData GetUserSoulDataBySID(string soulId)
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

		public string GetEggType(string monsterEvolutionRouteId)
		{
			GameWebAPI.RespDataMA_GetMonsterEvolutionRouteM.MonsterEvolutionRouteM[] monsterEvolutionRouteM = MasterDataMng.Instance().RespDataMA_MonsterEvolutionRouteM.monsterEvolutionRouteM;
			string result = "1";
			for (int i = 0; i < monsterEvolutionRouteM.Length; i++)
			{
				if (monsterEvolutionRouteM[i].monsterEvolutionRouteId == monsterEvolutionRouteId)
				{
					result = monsterEvolutionRouteM[i].eggMonsterId;
					break;
				}
			}
			return result;
		}

		public GameWebAPI.RespDataMA_GetSoulM.SoulM GetSoulMaster(string soulId)
		{
			GameWebAPI.RespDataMA_GetSoulM.SoulM result = null;
			GameWebAPI.RespDataMA_GetSoulM.SoulM[] soulM = MasterDataMng.Instance().RespDataMA_SoulM.soulM;
			for (int i = 0; i < soulM.Length; i++)
			{
				if (soulM[i].soulId == soulId)
				{
					result = soulM[i];
					break;
				}
			}
			return result;
		}

		public static int CalcClusterForEvolve(string monsterId)
		{
			GameWebAPI.RespDataMA_GetMonsterMG.MonsterM group = MonsterMaster.GetMonsterMasterByMonsterId(monsterId).Group;
			int growStep = group.growStep.ToInt32();
			int num = 0;
			int num2 = 0;
			if (MonsterGrowStepData.IsRipeScope(growStep))
			{
				num = ConstValue.EVOLVE_COEFFICIENT_FOR_5;
			}
			else if (MonsterGrowStepData.IsPerfectScope(growStep))
			{
				num = ConstValue.EVOLVE_COEFFICIENT_FOR_6;
			}
			else if (MonsterGrowStepData.IsUltimateScope(growStep))
			{
				num = ConstValue.EVOLVE_COEFFICIENT_FOR_7;
			}
			else
			{
				Debug.LogError("growStepの値が不正です");
			}
			GameWebAPI.RespDataMA_GetMonsterMS.MonsterM simple = MonsterMaster.GetMonsterMasterByMonsterId(monsterId).Simple;
			int arousal = simple.GetArousal();
			if (arousal >= 0 && arousal < ConstValue.EVOLVE_COEFFICIENT_RARE.Length)
			{
				num2 = ConstValue.EVOLVE_COEFFICIENT_RARE[arousal];
			}
			return num2 * num;
		}

		public static int CalcClusterForModeChange(string monsterId)
		{
			GameWebAPI.RespDataMA_GetMonsterMG.MonsterM group = MonsterMaster.GetMonsterMasterByMonsterId(monsterId).Group;
			int num = 0;
			int num2 = 0;
			int growStep = group.growStep.ToInt32();
			if (MonsterGrowStepData.IsRipeScope(growStep))
			{
				num = ConstValue.MODE_CHANGE_COEFFICIENT_FOR_5;
			}
			else if (MonsterGrowStepData.IsPerfectScope(growStep))
			{
				num = ConstValue.MODE_CHANGE_COEFFICIENT_FOR_6;
			}
			else if (MonsterGrowStepData.IsUltimateScope(growStep))
			{
				num = ConstValue.MODE_CHANGE_COEFFICIENT_FOR_7;
			}
			else
			{
				Debug.Log("growStepの値が不正です");
			}
			GameWebAPI.RespDataMA_GetMonsterMS.MonsterM simple = MonsterMaster.GetMonsterMasterByMonsterId(monsterId).Simple;
			int arousal = simple.GetArousal();
			if (arousal >= 0 && arousal < ConstValue.MODE_CHANGE_COEFFICIENT_RARE.Length)
			{
				num2 = ConstValue.MODE_CHANGE_COEFFICIENT_RARE[arousal];
			}
			return num2 * num;
		}

		public static int CalcClusterForVersionUp(string baseMonsterId)
		{
			int num = 300;
			int num2 = 450;
			return num * num2;
		}

		public class MonsterEvolveItem
		{
			public int catId;

			public int need_num;

			public int haveNum;

			public GameWebAPI.UserSoulData sd_item;
		}

		public class MonsterEvolveData
		{
			public MonsterData md;

			public MonsterData md_next;

			public GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution mem;

			public List<EvolutionData.MonsterEvolveItem> itemList;
		}
	}
}
