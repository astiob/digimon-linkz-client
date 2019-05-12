using Monster;
using System;
using System.Collections.Generic;

namespace MonsterList.InheritSkill
{
	public sealed class InheritSkillMonsterList
	{
		private List<MonsterData> deckMonster;

		private List<MonsterData> colosseumDeckMonster;

		private InheritSkillIconGrayOut iconGrayOut;

		public void Initialize(List<MonsterData> deckMonsterList, List<MonsterData> colosseumDeckMonsterList, InheritSkillIconGrayOut iconGrayOut)
		{
			this.deckMonster = deckMonsterList;
			this.colosseumDeckMonster = colosseumDeckMonsterList;
			this.iconGrayOut = iconGrayOut;
		}

		public void SetGrayOutDeckMonster(MonsterData ignoreMonster)
		{
			for (int i = 0; i < this.deckMonster.Count; i++)
			{
				if (this.deckMonster[i] != ignoreMonster)
				{
					GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(this.deckMonster[i]);
					if (null != icon)
					{
						this.iconGrayOut.BlockPartyUsed(icon);
					}
				}
			}
			for (int j = 0; j < this.colosseumDeckMonster.Count; j++)
			{
				if (this.colosseumDeckMonster[j] != ignoreMonster)
				{
					GUIMonsterIcon icon2 = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(this.colosseumDeckMonster[j]);
					if (null != icon2)
					{
						this.iconGrayOut.BlockPartyUsed(icon2);
					}
				}
			}
		}

		public void SetGrayOutUserMonsterList(MonsterData ignoreMonster)
		{
			List<MonsterData> monsterDataList = MonsterDataMng.Instance().GetMonsterDataList();
			for (int i = 0; i < monsterDataList.Count; i++)
			{
				if (monsterDataList[i] != ignoreMonster && (monsterDataList[i].userMonster.IsLocked || MonsterStatusData.IsSpecialTrainingType(monsterDataList[i].GetMonsterMaster().Group.monsterType)))
				{
					GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monsterDataList[i]);
					if (null != icon)
					{
						this.iconGrayOut.BlockLockIcon(icon);
					}
				}
			}
		}

		public void ClearIconGrayOutUserMonster(MonsterData ignoreMonster)
		{
			List<MonsterData> monsterDataList = MonsterDataMng.Instance().GetMonsterDataList();
			for (int i = 0; i < monsterDataList.Count; i++)
			{
				if (ignoreMonster != monsterDataList[i])
				{
					GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monsterDataList[i]);
					if (null != icon)
					{
						this.iconGrayOut.ResetIcon(icon);
					}
				}
			}
		}
	}
}
