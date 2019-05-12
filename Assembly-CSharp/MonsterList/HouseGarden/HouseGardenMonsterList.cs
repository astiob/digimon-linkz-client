using Monster;
using System;
using System.Collections.Generic;

namespace MonsterList.HouseGarden
{
	public sealed class HouseGardenMonsterList
	{
		private List<MonsterData> deckMonster;

		private List<MonsterData> colosseumDeckMonster;

		private List<MonsterData> sellMonster;

		private HouseGardenIconGrayOut iconGrayOut;

		private bool ExistDeck(MonsterData monster)
		{
			return this.deckMonster.Contains(monster) || this.colosseumDeckMonster.Contains(monster);
		}

		public void Initialize(List<MonsterData> deckMonsterList, List<MonsterData> colosseumDeckMonsterList, List<MonsterData> sellMonsterList, HouseGardenIconGrayOut iconGrayOut)
		{
			this.deckMonster = deckMonsterList;
			this.colosseumDeckMonster = colosseumDeckMonsterList;
			this.sellMonster = sellMonsterList;
			this.iconGrayOut = iconGrayOut;
		}

		public void SetGrayOutNotSelectedIconHouse()
		{
			List<MonsterData> monsterDataList = MonsterDataMng.Instance().GetMonsterDataList();
			for (int i = 0; i < monsterDataList.Count; i++)
			{
				if (!this.sellMonster.Contains(monsterDataList[i]) && !this.ExistDeck(monsterDataList[i]) && !MonsterStatusData.IsSpecialTrainingType(monsterDataList[i].GetMonsterMaster().Group.monsterType))
				{
					GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monsterDataList[i]);
					this.iconGrayOut.BlockIcon(icon);
				}
			}
		}

		public void SetGrayOutNotSelectedIconGarden()
		{
			List<MonsterData> monsterDataList = MonsterDataMng.Instance().GetMonsterDataList();
			for (int i = 0; i < monsterDataList.Count; i++)
			{
				if (!this.sellMonster.Contains(monsterDataList[i]) && !monsterDataList[i].userMonster.IsGrowing() && !monsterDataList[i].userMonster.IsEgg() && !MonsterStatusData.IsSpecialTrainingType(monsterDataList[i].GetMonsterMaster().Group.monsterType))
				{
					GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monsterDataList[i]);
					this.iconGrayOut.BlockIcon(icon);
				}
			}
		}

		public void ClearGrayOutNotSelectedIconHouse()
		{
			List<MonsterData> monsterDataList = MonsterDataMng.Instance().GetMonsterDataList();
			for (int i = 0; i < monsterDataList.Count; i++)
			{
				if (!this.sellMonster.Contains(monsterDataList[i]) && !this.ExistDeck(monsterDataList[i]) && !MonsterStatusData.IsSpecialTrainingType(monsterDataList[i].GetMonsterMaster().Group.monsterType))
				{
					GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monsterDataList[i]);
					if (monsterDataList[i].userMonster.IsLocked)
					{
						this.iconGrayOut.BlockLockIcon(icon);
					}
					else
					{
						this.iconGrayOut.ResetState(icon);
					}
				}
			}
		}

		public void ClearGrayOutNotSelectedIconGarden()
		{
			List<MonsterData> monsterDataList = MonsterDataMng.Instance().GetMonsterDataList();
			for (int i = 0; i < monsterDataList.Count; i++)
			{
				if (!this.sellMonster.Contains(monsterDataList[i]) && !monsterDataList[i].userMonster.IsGrowing() && !monsterDataList[i].userMonster.IsEgg() && !MonsterStatusData.IsSpecialTrainingType(monsterDataList[i].GetMonsterMaster().Group.monsterType))
				{
					GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monsterDataList[i]);
					if (monsterDataList[i].userMonster.IsLocked)
					{
						this.iconGrayOut.BlockLockIcon(icon);
					}
					else
					{
						this.iconGrayOut.ResetState(icon);
					}
				}
			}
		}

		public void SetGrayOutGrowing(List<MonsterData> monsterList)
		{
			for (int i = 0; i < monsterList.Count; i++)
			{
				if (monsterList[i].userMonster.IsEgg() || monsterList[i].userMonster.IsGrowing())
				{
					GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monsterList[i]);
					this.iconGrayOut.BlockGrowing(icon);
				}
			}
		}

		public void SetGrayOutPartyUsed()
		{
			for (int i = 0; i < this.deckMonster.Count; i++)
			{
				GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(this.deckMonster[i]);
				this.iconGrayOut.BlockPartyUsed(icon);
			}
			for (int j = 0; j < this.colosseumDeckMonster.Count; j++)
			{
				GUIMonsterIcon icon2 = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(this.colosseumDeckMonster[j]);
				this.iconGrayOut.BlockPartyUsed(icon2);
			}
		}

		public void ClearGrayOutPartyUsed()
		{
			for (int i = 0; i < this.deckMonster.Count; i++)
			{
				GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(this.deckMonster[i]);
				this.iconGrayOut.CancelSelect(icon);
			}
			for (int j = 0; j < this.colosseumDeckMonster.Count; j++)
			{
				GUIMonsterIcon icon2 = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(this.colosseumDeckMonster[j]);
				this.iconGrayOut.CancelSelect(icon2);
			}
		}

		public void SetGrayOutBlockMonster()
		{
			List<MonsterData> monsterDataList = MonsterDataMng.Instance().GetMonsterDataList();
			for (int i = 0; i < monsterDataList.Count; i++)
			{
				GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monsterDataList[i]);
				if (icon.Data.userMonster.IsLocked || MonsterStatusData.IsSpecialTrainingType(icon.Data.GetMonsterMaster().Group.monsterType))
				{
					this.iconGrayOut.BlockLockIcon(icon);
				}
				else
				{
					this.iconGrayOut.ResetState(icon);
				}
			}
		}

		public void ClearGrayOutBlockMonster()
		{
			List<MonsterData> monsterDataList = MonsterDataMng.Instance().GetMonsterDataList();
			for (int i = 0; i < monsterDataList.Count; i++)
			{
				GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monsterDataList[i]);
				this.iconGrayOut.ResetState(icon);
			}
		}

		public void SetGrayOutReturnDetailed(GUIMonsterIcon icon, MonsterData monster, bool isMaxSellMonster)
		{
			icon.Lock = monster.userMonster.IsLocked;
			if (this.sellMonster.Contains(monster))
			{
				for (int i = 0; i < this.sellMonster.Count; i++)
				{
					if (this.sellMonster[i] == monster)
					{
						this.iconGrayOut.SetSellMonster(icon, i + 1);
						break;
					}
				}
			}
			else if (monster.userMonster.IsLocked || this.ExistDeck(monster) || isMaxSellMonster || monster.userMonster.IsEgg() || monster.userMonster.IsGrowing() || MonsterStatusData.IsSpecialTrainingType(monster.GetMonsterMaster().Group.monsterType))
			{
				this.iconGrayOut.BlockLockIconReturnDetailed(icon);
			}
			else
			{
				this.iconGrayOut.CancelLockIconReturnDetailed(icon);
			}
		}

		public void SetSellMonsterList()
		{
			for (int i = 0; i < this.sellMonster.Count; i++)
			{
				GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(this.sellMonster[i]);
				this.iconGrayOut.SetSellMonster(icon, i + 1);
			}
		}
	}
}
