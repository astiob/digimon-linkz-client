using Monster;
using System;
using System.Collections.Generic;

namespace MonsterList.StrengthenChara
{
	public sealed class StrengthenCharaMonsterList
	{
		private List<MonsterData> deckMonster;

		private List<MonsterData> colosseumDeckMonster;

		private List<MonsterData> materialMonster;

		private StrengthenCharaIconGrayOut iconGrayOut;

		private bool ExistDeck(MonsterData monster)
		{
			return this.deckMonster.Contains(monster) || this.colosseumDeckMonster.Contains(monster);
		}

		private bool IsNotSelectedMonster(MonsterData ignoreMonster, MonsterData monster)
		{
			bool result = false;
			if (ignoreMonster != monster && !this.materialMonster.Contains(monster) && !this.ExistDeck(monster))
			{
				result = true;
			}
			return result;
		}

		public void Initialize(List<MonsterData> deckMonsterList, List<MonsterData> colosseumDeckMonsterList, List<MonsterData> materialMonsterList, StrengthenCharaIconGrayOut iconGrayOut)
		{
			this.deckMonster = deckMonsterList;
			this.colosseumDeckMonster = colosseumDeckMonsterList;
			this.materialMonster = materialMonsterList;
			this.iconGrayOut = iconGrayOut;
		}

		public void SetGrayOutIconPartyUsed(MonsterData ignoreMonster)
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

		public void ClearGrayOutIconPartyUsed()
		{
			for (int i = 0; i < this.deckMonster.Count; i++)
			{
				GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(this.deckMonster[i]);
				if (null != icon)
				{
					this.iconGrayOut.ResetState(icon);
				}
			}
			for (int j = 0; j < this.colosseumDeckMonster.Count; j++)
			{
				GUIMonsterIcon icon2 = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(this.colosseumDeckMonster[j]);
				if (null != icon2)
				{
					this.iconGrayOut.ResetState(icon2);
				}
			}
		}

		public void SetIconGrayOutUserMonsterList(MonsterData baseMonster)
		{
			List<MonsterData> monsterDataList = MonsterDataMng.Instance().GetMonsterDataList();
			for (int i = 0; i < monsterDataList.Count; i++)
			{
				if (this.IsNotSelectedMonster(baseMonster, monsterDataList[i]))
				{
					GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monsterDataList[i]);
					if (baseMonster == null)
					{
						this.iconGrayOut.ResetState(icon);
					}
					else if (monsterDataList[i].userMonster.IsLocked || MonsterStatusData.IsSpecialTrainingType(monsterDataList[i].GetMonsterMaster().Group.monsterType))
					{
						this.iconGrayOut.BlockLockIcon(icon);
					}
					else
					{
						this.iconGrayOut.CancelLockIcon(icon);
					}
				}
			}
		}

		public void BlockNotSelectIcon(MonsterData baseMonster)
		{
			List<MonsterData> monsterDataList = MonsterDataMng.Instance().GetMonsterDataList();
			for (int i = 0; i < monsterDataList.Count; i++)
			{
				if (this.IsNotSelectedMonster(baseMonster, monsterDataList[i]))
				{
					GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monsterDataList[i]);
					this.iconGrayOut.BlockInvalidIcon(icon);
				}
			}
		}

		public void LockIconReturnDetailed(GUIMonsterIcon icon, MonsterData monster, MonsterData baseMonster)
		{
			if (this.IsNotSelectedMonster(baseMonster, monster))
			{
				if (monster.userMonster.IsLocked || MonsterStatusData.IsSpecialTrainingType(monster.GetMonsterMaster().Group.monsterType))
				{
					this.iconGrayOut.BlockLockIconReturnDetailed(icon);
				}
				else
				{
					this.iconGrayOut.CancelLockIconReturnDetailed(icon);
				}
			}
			icon.Lock = monster.userMonster.IsLocked;
		}
	}
}
