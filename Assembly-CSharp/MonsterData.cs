using Monster;
using System;
using System.Collections.Generic;

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

	public bool IsAttachedChip()
	{
		return base.GetChipEquip().IsAttachedChip();
	}

	public void SetChipIdList(int[] chipIdList)
	{
		base.GetChipEquip().SetChipIdList(chipIdList);
	}

	public int[] GetChipIdList()
	{
		return base.GetChipEquip().GetChipIdList();
	}

	public List<string> GetChipGroupList()
	{
		return base.GetChipEquip().GetChipGroupList();
	}

	public GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Equip[] GetSlotEquip()
	{
		return base.GetChipEquip().GetEquip().ToArray();
	}

	public GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Manage GetSlotStatus()
	{
		return base.GetChipEquip().GetSlotStatus();
	}

	public bool CheckHaveMedal()
	{
		return base.ExistMedal();
	}

	public GameWebAPI.RespDataMA_GetSkillM.SkillM actionSkillM
	{
		get
		{
			return base.GetUniqueSkill();
		}
	}

	public GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM actionSkillDetailM
	{
		get
		{
			return base.GetUniqueSkillDetail();
		}
	}

	public GameWebAPI.RespDataMA_GetSkillM.SkillM commonSkillM
	{
		get
		{
			return base.GetCommonSkill();
		}
	}

	public GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM commonSkillDetailM
	{
		get
		{
			return base.GetCommonSkillDetail();
		}
	}

	public GameWebAPI.RespDataMA_GetSkillM.SkillM commonSkillM2
	{
		get
		{
			return base.GetExtraCommonSkill();
		}
	}

	public GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM commonSkillDetailM2
	{
		get
		{
			return base.GetExtraCommonSkillDetail();
		}
	}

	public GameWebAPI.RespDataMA_GetSkillM.SkillM leaderSkillM
	{
		get
		{
			return base.GetLeaderSkill();
		}
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

	public static GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM SerchResistanceById(string resistanceId)
	{
		return MonsterResistanceData.GetResistanceMaster(resistanceId);
	}

	public int GetPriceValue()
	{
		return base.GetPrice();
	}
}
