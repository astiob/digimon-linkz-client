using System;

namespace Monster
{
	public enum MonsterDetailedFilterType
	{
		NONE,
		LEADER_SKILL,
		ACTIVE_SUCCESS,
		PASSIV_SUCCESS = 4,
		MEDAL = 8,
		NO_LEADER_SKILL = 16,
		NO_MEDAL = 32
	}
}
