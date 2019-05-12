using System;

[Serializable]
public class Talent
{
	public string hpAbilityFlg;

	public string hpAbility;

	public string attackAbilityFlg;

	public string attackAbility;

	public string defenseAbilityFlg;

	public string defenseAbility;

	public string spAttackAbilityFlg;

	public string spAttackAbility;

	public string spDefenseAbilityFlg;

	public string spDefenseAbility;

	public string speedAbilityFlg;

	public string speedAbility;

	public Talent()
	{
		this.hpAbilityFlg = "0";
		this.hpAbility = "0";
		this.attackAbilityFlg = "0";
		this.attackAbility = "0";
		this.defenseAbilityFlg = "0";
		this.defenseAbility = "0";
		this.spAttackAbilityFlg = "0";
		this.spAttackAbility = "0";
		this.spDefenseAbilityFlg = "0";
		this.spDefenseAbility = "0";
		this.speedAbilityFlg = "0";
		this.speedAbility = "0";
	}

	public Talent(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList userMonster)
	{
		this.hpAbilityFlg = userMonster.hpAbilityFlg;
		this.hpAbility = userMonster.hpAbility;
		this.attackAbilityFlg = userMonster.attackAbilityFlg;
		this.attackAbility = userMonster.attackAbility;
		this.defenseAbilityFlg = userMonster.defenseAbilityFlg;
		this.defenseAbility = userMonster.defenseAbility;
		this.spAttackAbilityFlg = userMonster.spAttackAbilityFlg;
		this.spAttackAbility = userMonster.spAttackAbility;
		this.spDefenseAbilityFlg = userMonster.spDefenseAbilityFlg;
		this.spDefenseAbility = userMonster.spDefenseAbility;
		this.speedAbilityFlg = userMonster.speedAbilityFlg;
		this.speedAbility = userMonster.speedAbility;
	}

	public Talent(GameWebAPI.Common_MonsterData commonMonsterData)
	{
		this.hpAbilityFlg = commonMonsterData.hpAbilityFlg;
		this.hpAbility = commonMonsterData.hpAbility;
		this.attackAbilityFlg = commonMonsterData.attackAbilityFlg;
		this.attackAbility = commonMonsterData.attackAbility;
		this.defenseAbilityFlg = commonMonsterData.defenseAbilityFlg;
		this.defenseAbility = commonMonsterData.defenseAbility;
		this.spAttackAbilityFlg = commonMonsterData.spAttackAbilityFlg;
		this.spAttackAbility = commonMonsterData.spAttackAbility;
		this.spDefenseAbilityFlg = commonMonsterData.spDefenseAbilityFlg;
		this.spDefenseAbility = commonMonsterData.spDefenseAbility;
		this.speedAbilityFlg = commonMonsterData.speedAbilityFlg;
		this.speedAbility = commonMonsterData.speedAbility;
	}
}
