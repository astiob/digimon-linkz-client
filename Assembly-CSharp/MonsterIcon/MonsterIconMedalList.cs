using System;
using UnityEngine;

namespace MonsterIcon
{
	[Serializable]
	public sealed class MonsterIconMedalList
	{
		[SerializeField]
		private MonsterIconMedal[] medalList;

		private int goldMedalCount;

		private int silverMedalCount;

		private void CountMedal(string abilityFlag, ref int goldMedalCount, ref int silverMedalCount)
		{
			ConstValue.Medal medal = (ConstValue.Medal)int.Parse(abilityFlag);
			if (medal == ConstValue.Medal.Gold)
			{
				goldMedalCount++;
			}
			else if (medal == ConstValue.Medal.Silver)
			{
				silverMedalCount++;
			}
		}

		public void SetMedal(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster)
		{
			this.goldMedalCount = 0;
			this.silverMedalCount = 0;
			this.CountMedal(userMonster.hpAbilityFlg, ref this.goldMedalCount, ref this.silverMedalCount);
			this.CountMedal(userMonster.attackAbilityFlg, ref this.goldMedalCount, ref this.silverMedalCount);
			this.CountMedal(userMonster.defenseAbilityFlg, ref this.goldMedalCount, ref this.silverMedalCount);
			this.CountMedal(userMonster.spAttackAbilityFlg, ref this.goldMedalCount, ref this.silverMedalCount);
			this.CountMedal(userMonster.spDefenseAbilityFlg, ref this.goldMedalCount, ref this.silverMedalCount);
			this.CountMedal(userMonster.speedAbilityFlg, ref this.goldMedalCount, ref this.silverMedalCount);
		}

		public void Show()
		{
			int num = 0;
			if (0 < this.goldMedalCount)
			{
				num = 1;
				this.medalList[0].SetGoldMedal();
				this.medalList[0].SetMedalNum(this.goldMedalCount);
				this.medalList[0].Show();
			}
			if (0 < this.silverMedalCount)
			{
				this.medalList[num].SetSilverMedal();
				this.medalList[num].SetMedalNum(this.silverMedalCount);
				this.medalList[num].Show();
			}
			else
			{
				this.medalList[num].Clear();
			}
			for (int i = num + 1; i < this.medalList.Length; i++)
			{
				this.medalList[i].Clear();
			}
		}

		public void Clear()
		{
			for (int i = 0; i < this.medalList.Length; i++)
			{
				this.medalList[i].Clear();
			}
		}
	}
}
