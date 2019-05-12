using Monster;
using System;

public sealed class MonsterData : MonsterUserData
{
	public MonsterData(string monsterId) : base(monsterId)
	{
		base.SetMonsterMaster(monsterId);
	}

	public MonsterData(MonsterData source) : base(new GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList(source.GetMonster()))
	{
	}

	public MonsterData(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster) : base(userMonster)
	{
	}

	public static MonsterData CreateMonsterData(string monsterId)
	{
		GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster = new GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList
		{
			userMonsterId = "-1",
			monsterId = monsterId,
			level = "1",
			defaultSkillGroupSubId = "1",
			commonSkillId = "473",
			extraCommonSkillId = string.Empty,
			leaderSkillId = "0"
		};
		return new MonsterData(userMonster);
	}

	public void InitSkillInfo()
	{
		GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList monster = base.GetMonster();
		base.SetUniqueSkill(monster.monsterId, monster.defaultSkillGroupSubId);
		base.SetCommonSkill(monster.commonSkillId);
		base.SetExtraCommonSkill(monster.extraCommonSkillId);
		base.SetLeaderSkill(monster.leaderSkillId);
	}

	public GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Equip[] GetSlotEquip()
	{
		return base.GetChipEquip().GetEquip().ToArray();
	}

	public GameWebAPI.RespDataMA_GetMonsterMS.MonsterM monsterM
	{
		get
		{
			return base.GetMonsterMaster().Simple;
		}
	}

	public GameWebAPI.RespDataMA_GetMonsterMG.MonsterM monsterMG
	{
		get
		{
			return base.GetMonsterMaster().Group;
		}
	}

	public GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster
	{
		get
		{
			return base.GetMonster();
		}
	}
}
