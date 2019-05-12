using Master;
using Monster;
using System;

namespace UI.MonsterInfoParts
{
	public static class MonsterBasicInfoExtensions
	{
		public static void SetData(MonsterBasicInfo basicInfo, MonsterUserData monster)
		{
			if (monster.GetMonster().IsEgg())
			{
				basicInfo.SetEggData(StringMaster.GetString("CharaStatus-04"), monster.GetMonsterMaster().Simple.rare);
			}
			else
			{
				basicInfo.SetMonsterData(monster as MonsterData);
			}
		}
	}
}
