using System;
using System.Collections.Generic;

namespace MonsterList.BaseSelect
{
	public sealed class BaseSelectMonsterList
	{
		private List<MonsterData> deckMonster;

		private List<MonsterData> colosseumDeckMonster;

		private BaseSelectIconGrayOut iconGrayOut;

		public void Initialize(List<MonsterData> deckMonsterList, List<MonsterData> colosseumDeckMonsterList, BaseSelectIconGrayOut iconGrayOut)
		{
			this.deckMonster = deckMonsterList;
			this.colosseumDeckMonster = colosseumDeckMonsterList;
			this.iconGrayOut = iconGrayOut;
		}

		public void GrayOutPartyUsed()
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
	}
}
