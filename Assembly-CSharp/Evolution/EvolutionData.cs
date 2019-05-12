using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Evolution
{
	public sealed class EvolutionData : ClassSingleton<EvolutionData>
	{
		private Dictionary<string, List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution>> monsterEvolutionDic;

		private Dictionary<string, List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution>> baseMonsterEvolutionTable;

		private void GetEvolutionList(string monsterId, List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> evolutionList)
		{
			GameWebAPI.RespDataMA_GetMonsterEvolutionM respDataMA_MonsterEvolutionM = MasterDataMng.Instance().RespDataMA_MonsterEvolutionM;
			for (int i = 0; i < respDataMA_MonsterEvolutionM.monsterEvolutionM.Length; i++)
			{
				if (respDataMA_MonsterEvolutionM.monsterEvolutionM[i].nextMonsterId == monsterId)
				{
					evolutionList.Add(respDataMA_MonsterEvolutionM.monsterEvolutionM[i]);
				}
			}
		}

		private void GetBeforeEvolutionChildList(string monsterId, List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> evolutionList)
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

		private void GetAfterEvolutionChildList(string monsterId, List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> evolutionList)
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
		}

		private Dictionary<string, List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution>> MakeEvolutionDic(GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution[] evoS)
		{
			global::Debug.Log("================================ 進化辞書作成 開始 = " + Time.realtimeSinceStartup);
			Dictionary<string, List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution>> dictionary = new Dictionary<string, List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution>>();
			List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> list = null;
			string b = "-1";
			for (int i = 0; i < evoS.Length; i++)
			{
				if (evoS[i].baseMonsterId != b)
				{
					list = new List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution>();
					dictionary.Add(evoS[i].baseMonsterId, list);
					list.Add(evoS[i]);
					b = evoS[i].baseMonsterId;
				}
				else
				{
					list.Add(evoS[i]);
				}
			}
			global::Debug.Log("================================ 進化辞書作成 終了 = " + Time.realtimeSinceStartup);
			return dictionary;
		}

		public List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> GetEvoList(string baseMonsterId)
		{
			if (!this.monsterEvolutionDic.ContainsKey(baseMonsterId))
			{
				return new List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution>();
			}
			return this.monsterEvolutionDic[baseMonsterId];
		}

		public List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> GetBeforeMonsterEvolutionList(string monsterId, string growStep)
		{
			List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> list = null;
			if (!this.baseMonsterEvolutionTable.TryGetValue(monsterId, out list))
			{
				GrowStep growStep2 = MonsterData.ToGrowStepId(growStep);
				list = new List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution>();
				switch (growStep2)
				{
				case GrowStep.EGG:
					break;
				case GrowStep.CHILD_1:
				case GrowStep.CHILD_2:
				case GrowStep.GROWING:
					this.GetBeforeEvolutionChildList(monsterId, list);
					break;
				default:
					this.GetEvolutionList(monsterId, list);
					break;
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
				GrowStep growStep2 = MonsterData.ToGrowStepId(growStep);
				list = new List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution>();
				switch (growStep2)
				{
				case GrowStep.EGG:
				case GrowStep.CHILD_1:
				case GrowStep.CHILD_2:
					this.GetAfterEvolutionChildList(monsterId, list);
					this.monsterEvolutionDic.Add(monsterId, list);
					break;
				}
			}
			return list;
		}

		public List<EvolutionData.MonsterEvolveData> GetEvolveListByMonsterData(MonsterData md)
		{
			List<EvolutionData.MonsterEvolveData> list = new List<EvolutionData.MonsterEvolveData>();
			List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> evoList = this.GetEvoList(md.monsterM.monsterId);
			for (int i = 0; i < evoList.Count; i++)
			{
				EvolutionData.MonsterEvolveData monsterEvolveData = new EvolutionData.MonsterEvolveData();
				monsterEvolveData.md = md;
				monsterEvolveData.md_next = MonsterDataMng.Instance().CreateMonsterDataByMID(evoList[i].nextMonsterId);
				monsterEvolveData.mem = evoList[i];
				GameWebAPI.MonsterEvolutionMaterialMaster.Material evolutionMaterial = MonsterEvolutionUtil.GetEvolutionMaterial(evoList[i].monsterEvolutionMaterialId);
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
						monsterEvolveItem.md_item = MonsterDataMng.Instance().CreateMonsterDataByMID(assetValue);
						monsterEvolveItem.haveMDList = MonsterDataMng.Instance().GetMonsterDataListByMID(assetValue);
						monsterEvolveData.itemList.Add(monsterEvolveItem);
					}
					else if (assetCategoryId == ConstValue.EVOLVE_ITEM_SOUL.ToString())
					{
						monsterEvolveItem.catId = ConstValue.EVOLVE_ITEM_SOUL;
						monsterEvolveItem.need_num = int.Parse(assetNum);
						monsterEvolveItem.sd_item = MonsterDataMng.Instance().GetUserSoulDataBySID(assetValue);
						monsterEvolveItem.haveNum = int.Parse(monsterEvolveItem.sd_item.num);
						monsterEvolveData.itemList.Add(monsterEvolveItem);
					}
				}
				list.Add(monsterEvolveData);
			}
			return list;
		}

		public List<int> EvolvePostProcess(EvolutionData.MonsterEvolveData med)
		{
			List<int> list = new List<int>();
			List<EvolutionData.MonsterEvolveItem> itemList = med.itemList;
			for (int i = 0; i < itemList.Count; i++)
			{
				if (itemList[i].catId == ConstValue.EVOLVE_ITEM_SOUL)
				{
					int num = int.Parse(itemList[i].sd_item.num) - itemList[i].need_num;
					itemList[i].sd_item.num = num.ToString();
				}
			}
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

		private bool CanEvolve(MonsterData monsterData)
		{
			List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> evoList = this.GetEvoList(monsterData.monsterM.monsterId);
			int i = 0;
			while (i < evoList.Count)
			{
				string monsterId = new EvolutionData.MonsterEvolveData
				{
					md = monsterData,
					md_next = MonsterDataMng.Instance().CreateMonsterDataByMID(evoList[i].nextMonsterId),
					mem = evoList[i]
				}.md_next.userMonster.monsterId;
				string evolutionType = monsterData.GetEvolutionType(monsterId);
				int num = int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney);
				if (evolutionType == "2")
				{
					int num2 = CalculatorUtil.CalcClusterForModeChange(monsterId);
					if (num >= num2)
					{
						goto IL_138;
					}
				}
				else
				{
					if (!(evolutionType == "1") && !(evolutionType == "3") && !(evolutionType == "4"))
					{
						goto IL_138;
					}
					int num2 = CalculatorUtil.CalcClusterForEvolve(monsterId);
					if (num >= num2)
					{
						int num3 = int.Parse(monsterData.userMonster.level);
						int num4 = int.Parse(monsterData.monsterM.maxLevel);
						if (num3 >= num4)
						{
							goto IL_138;
						}
					}
				}
				IL_22F:
				i++;
				continue;
				IL_138:
				bool flag = true;
				GameWebAPI.MonsterEvolutionMaterialMaster.Material evolutionMaterial = MonsterEvolutionUtil.GetEvolutionMaterial(evoList[i].monsterEvolutionMaterialId);
				for (int j = 1; j <= 7; j++)
				{
					string assetCategoryId = evolutionMaterial.GetAssetCategoryId(j);
					string assetValue = evolutionMaterial.GetAssetValue(j);
					string assetNum = evolutionMaterial.GetAssetNum(j);
					if (assetCategoryId == ConstValue.EVOLVE_ITEM_MONS.ToString())
					{
						int num5 = assetNum.ToInt32();
						List<MonsterData> monsterDataListByMID = MonsterDataMng.Instance().GetMonsterDataListByMID(assetValue);
						if (num5 > monsterDataListByMID.Count)
						{
							flag = false;
							break;
						}
					}
					else if (assetCategoryId == ConstValue.EVOLVE_ITEM_SOUL.ToString())
					{
						int num6 = assetNum.ToInt32();
						GameWebAPI.UserSoulData userSoulDataBySID = MonsterDataMng.Instance().GetUserSoulDataBySID(assetValue);
						int num7 = userSoulDataBySID.num.ToInt32();
						if (num6 > num7)
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					return true;
				}
				goto IL_22F;
			}
			return false;
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
				monsterData.dimmLevel = GUIMonsterIcon.DIMM_LEVEL.NOTACTIVE;
				monsterIcon.DimmLevel = GUIMonsterIcon.DIMM_LEVEL.NOTACTIVE;
				if (isOnlyDim)
				{
					return;
				}
				monsterIcon.SortMess = StringMaster.GetString("CharaIcon-02");
				monsterIcon.SetSortMessageColor(ConstValue.DIGIMON_BLUE);
			}
		}

		public List<MonsterData> GetMDL_EvolveDisable()
		{
			List<MonsterData> list = new List<MonsterData>();
			List<MonsterData> monsterDataList = MonsterDataMng.Instance().GetMonsterDataList(false);
			for (int i = 0; i < monsterDataList.Count; i++)
			{
				List<GameWebAPI.RespDataMA_GetMonsterEvolutionM.Evolution> evoList = this.GetEvoList(monsterDataList[i].monsterM.monsterId);
				if (evoList.Count <= 0)
				{
					list.Add(monsterDataList[i]);
				}
			}
			return list;
		}

		public class MonsterEvolveItem
		{
			public int catId;

			public int need_num;

			public MonsterData md_item;

			public List<MonsterData> haveMDList;

			public GameWebAPI.UserSoulData sd_item;

			public int haveNum;
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
