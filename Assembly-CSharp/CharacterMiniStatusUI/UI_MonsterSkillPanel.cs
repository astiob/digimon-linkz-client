using System;
using UnityEngine;

namespace CharacterMiniStatusUI
{
	public sealed class UI_MonsterSkillPanel : MonoBehaviour
	{
		[SerializeField]
		private MonsterResistanceList resistanceList;

		[SerializeField]
		private MonsterLeaderSkill leaderSkill;

		[SerializeField]
		private MonsterLearnSkill uniqueSkill;

		[SerializeField]
		private MonsterLearnSkill commonSkill;

		[SerializeField]
		private MonsterLearnSkill extraCommonSkill;

		[SerializeField]
		private UI_ExtraCommonSkillParts extraCommonSkillUI;

		public void SetMonsterData(MonsterData monsterData)
		{
			this.resistanceList.SetValues(monsterData);
			this.leaderSkill.SetSkill(monsterData);
			this.uniqueSkill.SetSkill(monsterData);
			this.commonSkill.SetSkill(monsterData);
			this.extraCommonSkill.SetSkill(monsterData);
			this.extraCommonSkillUI.SetMonsterData(monsterData);
		}

		public void ClearData()
		{
			this.resistanceList.ClearValues();
			this.leaderSkill.ClearSkill();
			this.uniqueSkill.ClearSkill();
			this.commonSkill.ClearSkill();
			this.extraCommonSkill.ClearSkill();
			this.extraCommonSkillUI.ClearData();
		}
	}
}
