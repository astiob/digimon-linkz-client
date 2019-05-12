using Monster;
using System;
using System.Collections.Generic;

namespace MonsterList.ChangeMonster
{
	public sealed class ChangeMonsterMonsterList
	{
		private List<MonsterUserData> colosseumDeckMonsterList;

		private ChangeMonsterIconGrayOut iconGrayOut;

		public void Initialize(ChangeMonsterIconGrayOut iconGrayOut)
		{
			this.colosseumDeckMonsterList = new List<MonsterUserData>();
			this.iconGrayOut = iconGrayOut;
		}

		public void SetIconColosseumDeck(MonsterUserData targetMonster, MonsterUserData[] colosseumDeckMonster)
		{
			this.colosseumDeckMonsterList.Clear();
			this.colosseumDeckMonsterList.AddRange(colosseumDeckMonster);
			for (int i = 0; i < colosseumDeckMonster.Length; i++)
			{
				if (colosseumDeckMonster[i] != null)
				{
					GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(colosseumDeckMonster[i] as MonsterData);
					if (targetMonster == colosseumDeckMonster[i])
					{
						this.iconGrayOut.BlockPartyUsed(icon);
					}
					else
					{
						this.iconGrayOut.SetPartyUsed(icon);
					}
				}
			}
		}

		public void CancelSelectedIcon(MonsterUserData monster)
		{
			GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monster as MonsterData);
			this.iconGrayOut.CancelSelect(icon);
			if (this.colosseumDeckMonsterList.Contains(monster))
			{
				this.iconGrayOut.SetPartyUsed(icon);
			}
		}
	}
}
