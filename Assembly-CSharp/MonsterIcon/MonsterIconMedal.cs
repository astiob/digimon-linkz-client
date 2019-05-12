using Master;
using System;
using UnityEngine;

namespace MonsterIcon
{
	public sealed class MonsterIconMedal : MonoBehaviour
	{
		[SerializeField]
		private MonsterIconMedal.MedalUI medalLeft;

		[SerializeField]
		private MonsterIconMedal.MedalUI medalRight;

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

		private void SetMedalSpriteName(MonsterIconMedal.MedalUI ui, string spriteName)
		{
			ui.medal.enabled = true;
			ui.medal.spriteName = spriteName;
		}

		private void SetSilverMedal(MonsterIconMedal.MedalUI ui, int medalCount)
		{
			this.SetMedalSpriteName(ui, "Common02_Talent_Silver");
			ui.num.enabled = true;
			ui.num.text = string.Format(StringMaster.GetString("SystemItemCount2"), medalCount);
		}

		public void SetMedal(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster)
		{
			int num = 0;
			int num2 = 0;
			this.CountMedal(userMonster.hpAbilityFlg, ref num, ref num2);
			this.CountMedal(userMonster.attackAbilityFlg, ref num, ref num2);
			this.CountMedal(userMonster.defenseAbilityFlg, ref num, ref num2);
			this.CountMedal(userMonster.spAttackAbilityFlg, ref num, ref num2);
			this.CountMedal(userMonster.spDefenseAbilityFlg, ref num, ref num2);
			this.CountMedal(userMonster.speedAbilityFlg, ref num, ref num2);
			if (0 < num)
			{
				this.SetMedalSpriteName(this.medalLeft, "Common02_Talent_Gold");
				if (0 < num2)
				{
					this.SetSilverMedal(this.medalRight, num2);
				}
			}
			else if (0 < num2)
			{
				this.SetSilverMedal(this.medalLeft, num2);
			}
		}

		public void ClearMedal()
		{
			this.medalLeft.medal.enabled = false;
			this.medalLeft.num.enabled = false;
			this.medalRight.medal.enabled = false;
			this.medalRight.num.enabled = false;
		}

		[Serializable]
		private sealed class MedalUI
		{
			[SerializeField]
			public UISprite medal;

			[SerializeField]
			public UILabel num;
		}
	}
}
