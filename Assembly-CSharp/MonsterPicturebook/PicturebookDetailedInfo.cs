using Monster;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonsterPicturebook
{
	public sealed class PicturebookDetailedInfo
	{
		public PicturebookMonster monster;

		public MonsterSkillClientMaster[] skillGroupSubIdList;

		public int uniqueSkillCount;

		public PicturebookDetailedInfo(PicturebookMonster monsterData)
		{
			this.monster = monsterData;
			Dictionary<string, MonsterSkillClientMaster> skillMasterBySkillGroupId = MonsterSkillData.GetSkillMasterBySkillGroupId(monsterData.monsterMaster.Simple.skillGroupId);
			this.uniqueSkillCount = 1;
			this.skillGroupSubIdList = skillMasterBySkillGroupId.Values.ToArray<MonsterSkillClientMaster>();
			if (this.skillGroupSubIdList.Length == 2 && this.skillGroupSubIdList[0].Simple.name != this.skillGroupSubIdList[1].Simple.name)
			{
				this.uniqueSkillCount = 2;
			}
		}
	}
}
