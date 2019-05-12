using Monster;
using System;
using System.Collections.Generic;

namespace MonsterList.TranceResistance
{
	public sealed class TranceResistanceMonsterList
	{
		private List<MonsterData> deckMonster;

		private List<MonsterData> colosseumDeckMonster;

		private TranceResistanceIconGrayOut iconGrayOut;

		public void Initialize(List<MonsterData> deckMonsterList, List<MonsterData> colosseumDeckMonsterList, TranceResistanceIconGrayOut iconGrayOut)
		{
			this.deckMonster = deckMonsterList;
			this.colosseumDeckMonster = colosseumDeckMonsterList;
			this.iconGrayOut = iconGrayOut;
		}

		public void ClearGrayOutIconPartyUsedMonster()
		{
			for (int i = 0; i < this.deckMonster.Count; i++)
			{
				GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(this.deckMonster[i]);
				this.iconGrayOut.CancelBlockPartyUsed(icon);
			}
			for (int j = 0; j < this.colosseumDeckMonster.Count; j++)
			{
				GUIMonsterIcon icon2 = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(this.colosseumDeckMonster[j]);
				this.iconGrayOut.CancelBlockPartyUsed(icon2);
			}
		}

		public void SetGrayOutIconPartyUsedMonster(MonsterData ignoreMonster)
		{
			for (int i = 0; i < this.deckMonster.Count; i++)
			{
				if (this.deckMonster[i] != ignoreMonster)
				{
					GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(this.deckMonster[i]);
					this.iconGrayOut.BlockPartyUsed(icon);
				}
			}
			for (int j = 0; j < this.colosseumDeckMonster.Count; j++)
			{
				if (this.colosseumDeckMonster[j] != ignoreMonster)
				{
					GUIMonsterIcon icon2 = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(this.colosseumDeckMonster[j]);
					this.iconGrayOut.BlockPartyUsed(icon2);
				}
			}
		}

		public void ClearIconGrayOutPartnerMonster(MonsterData ignoreMonster, string monsterGroupId)
		{
			List<MonsterData> monsterDataList = MonsterDataMng.Instance().GetMonsterDataList();
			for (int i = 0; i < monsterDataList.Count; i++)
			{
				if (monsterDataList[i] != ignoreMonster && monsterDataList[i].monsterMG.monsterGroupId == monsterGroupId && !MonsterGrowStepData.IsUltimateScope(monsterDataList[i].GetMonsterMaster().Group.growStep))
				{
					GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monsterDataList[i]);
					if (null != icon)
					{
						this.iconGrayOut.SetIdleIcon(icon);
					}
				}
			}
		}

		public void ClearIconGrayOutPartnerMonsterCheckLock(MonsterData ignoreMonster, string monsterGroupId)
		{
			List<MonsterData> monsterDataList = MonsterDataMng.Instance().GetMonsterDataList();
			for (int i = 0; i < monsterDataList.Count; i++)
			{
				if (monsterDataList[i] != ignoreMonster && !monsterDataList[i].userMonster.IsLocked && monsterDataList[i].monsterMG.monsterGroupId == monsterGroupId && !MonsterGrowStepData.IsUltimateScope(monsterDataList[i].GetMonsterMaster().Group.growStep))
				{
					GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monsterDataList[i]);
					if (null != icon)
					{
						this.iconGrayOut.SetIdleIcon(icon);
					}
				}
			}
		}

		public void ClearIconGrayOutPartnerMonster(MonsterData baseDigimon, List<MonsterData> partnerMonsterList)
		{
			List<MonsterData> monsterDataList = MonsterDataMng.Instance().GetMonsterDataList();
			for (int i = 0; i < monsterDataList.Count; i++)
			{
				if (monsterDataList[i] != baseDigimon && !partnerMonsterList.Contains(monsterDataList[i]))
				{
					GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monsterDataList[i]);
					this.iconGrayOut.SetIdleIcon(icon);
				}
			}
		}

		public void SetIconGrayOutPartnerMonster(MonsterData baseDigimon, List<MonsterData> partnerMonsterList)
		{
			List<MonsterData> monsterDataList = MonsterDataMng.Instance().GetMonsterDataList();
			for (int i = 0; i < monsterDataList.Count; i++)
			{
				if (monsterDataList[i] != baseDigimon)
				{
					string b = string.Empty;
					if (baseDigimon != null)
					{
						b = baseDigimon.monsterMG.monsterGroupId;
					}
					else if (partnerMonsterList != null && 0 < partnerMonsterList.Count)
					{
						b = partnerMonsterList[0].monsterMG.monsterGroupId;
					}
					if (monsterDataList[i].monsterMG.monsterGroupId != b || MonsterGrowStepData.IsUltimateScope(monsterDataList[i].GetMonsterMaster().Group.growStep))
					{
						GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monsterDataList[i]);
						this.iconGrayOut.SetCanNotDecideIcon(icon);
					}
					else if (0 < partnerMonsterList.Count && monsterDataList[i] == partnerMonsterList[0])
					{
						GUIMonsterIcon icon2 = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monsterDataList[i]);
						this.iconGrayOut.SetPartnerIcon(icon2);
					}
					else if (baseDigimon != null && (monsterDataList[i].userMonster.IsLocked || MonsterStatusData.IsSpecialTrainingType(monsterDataList[i].GetMonsterMaster().Group.monsterType) || 0 < partnerMonsterList.Count))
					{
						GUIMonsterIcon icon3 = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monsterDataList[i]);
						this.iconGrayOut.BlockLockIcon(icon3);
					}
				}
			}
		}
	}
}
