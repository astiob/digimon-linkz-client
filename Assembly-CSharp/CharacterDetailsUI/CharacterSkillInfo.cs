using System;
using UnityEngine;

namespace CharacterDetailsUI
{
	[Serializable]
	public sealed class CharacterSkillInfo
	{
		[SerializeField]
		private MonsterLeaderSkill leaderSkill;

		[SerializeField]
		private MonsterLearnSkill uniqueSkill;

		[SerializeField]
		private MonsterLearnSkill successionSkill;

		[SerializeField]
		private bool existExtraSkill;

		[SerializeField]
		private MonsterLearnSkill successionSkill2;

		public void SetSkill(MonsterData monsterData)
		{
			this.leaderSkill.SetSkill(monsterData);
			this.uniqueSkill.SetSkill(monsterData);
			this.successionSkill.SetSkill(monsterData);
			if (this.existExtraSkill)
			{
				this.successionSkill2.SetSkill(monsterData);
			}
		}

		public void ClearSkill()
		{
			this.leaderSkill.ClearSkill();
			this.uniqueSkill.ClearSkill();
			this.successionSkill.ClearSkill();
			if (this.existExtraSkill)
			{
				this.successionSkill2.ClearSkill();
			}
		}
	}
}
