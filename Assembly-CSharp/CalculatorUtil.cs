using Ability;
using Monster;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CalculatorUtil
{
	public static int CalcClusterForSuccession(MonsterData baseDigimon, List<MonsterData> partnerDigimons)
	{
		int num = CalculatorUtil.GetArousalValue(baseDigimon.monsterM) + ConstValue.SUCCESSION_BASE_COEFFICIENT;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < partnerDigimons.Count; i++)
		{
			MonsterData monsterData = partnerDigimons[i];
			if (monsterData.commonSkillM != null)
			{
				num2 += monsterData.commonSkillM.inheritancePrice.ToInt32();
				num3 += CalculatorUtil.GetArousalValue(monsterData.monsterM) + ConstValue.SUCCESSION_PARTNER_COEFFICIENT;
			}
		}
		return num * num2 * num3 + ConstValue.SUCCESSION_COEFFICIENT;
	}

	public static int CalcClusterForLaboratory(MonsterData baseDigimon, MonsterData partnerDigimon)
	{
		int num = 0;
		if (baseDigimon != null)
		{
			int num2 = CalculatorUtil.GetArousalValue(baseDigimon.monsterM) + ConstValue.LABORATORY_BASE_PLUS_COEFFICIENT;
			num += num2 * ConstValue.LABORATORY_BASE_COEFFICIENT;
		}
		if (partnerDigimon != null)
		{
			int num3 = CalculatorUtil.GetArousalValue(partnerDigimon.monsterM) + ConstValue.LABORATORY_PARTNER_PLUS_COEFFICIENT;
			num += num3 * ConstValue.LABORATORY_PARTNER_COEFFICIENT;
		}
		return num;
	}

	public static int CalcClusterForAbilityUpgrade(GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList baseUserMonster, GameWebAPI.RespDataUS_GetMonsterList.UserMonsterList materialUserMonster)
	{
		float maxAbility = ClassSingleton<AbilityData>.Instance.GetMaxAbility(baseUserMonster);
		float num = Mathf.Pow(1.15f, maxAbility);
		float num2 = Mathf.Floor(num * 10f) / 10f * 500f;
		int totalAbilityCount = ClassSingleton<AbilityData>.Instance.GetTotalAbilityCount(materialUserMonster);
		float num3 = 1f + 0.1f * (float)(totalAbilityCount - 1);
		return (int)Mathf.Floor(num2 * num3);
	}

	public static int CalcClusterForEvolve(string monsterId)
	{
		GameWebAPI.RespDataMA_GetMonsterMG.MonsterM monsterGroupMasterByMonsterId = MonsterDataMng.Instance().GetMonsterGroupMasterByMonsterId(monsterId);
		int growStep = monsterGroupMasterByMonsterId.growStep.ToInt32();
		int num = 0;
		int num2 = 0;
		if (MonsterGrowStepData.IsRipeScope(growStep))
		{
			num = ConstValue.EVOLVE_COEFFICIENT_FOR_5;
		}
		else if (MonsterGrowStepData.IsPerfectScope(growStep))
		{
			num = ConstValue.EVOLVE_COEFFICIENT_FOR_6;
		}
		else if (MonsterGrowStepData.IsUltimateScope(growStep))
		{
			num = ConstValue.EVOLVE_COEFFICIENT_FOR_7;
		}
		else
		{
			global::Debug.Log("growStepの値が不正です");
		}
		GameWebAPI.RespDataMA_GetMonsterMS.MonsterM monsterMasterByMonsterId = MonsterDataMng.Instance().GetMonsterMasterByMonsterId(monsterId);
		int arousalValue = CalculatorUtil.GetArousalValue(monsterMasterByMonsterId);
		if (arousalValue >= 0 && arousalValue < ConstValue.EVOLVE_COEFFICIENT_RARE.Length)
		{
			num2 = ConstValue.EVOLVE_COEFFICIENT_RARE[arousalValue];
		}
		return num2 * num;
	}

	public static int CalcClusterForModeChange(string monsterId)
	{
		GameWebAPI.RespDataMA_GetMonsterMG.MonsterM monsterGroupMasterByMonsterId = MonsterDataMng.Instance().GetMonsterGroupMasterByMonsterId(monsterId);
		int num = 0;
		int num2 = 0;
		int growStep = monsterGroupMasterByMonsterId.growStep.ToInt32();
		if (MonsterGrowStepData.IsRipeScope(growStep))
		{
			num = ConstValue.MODE_CHANGE_COEFFICIENT_FOR_5;
		}
		else if (MonsterGrowStepData.IsPerfectScope(growStep))
		{
			num = ConstValue.MODE_CHANGE_COEFFICIENT_FOR_6;
		}
		else if (MonsterGrowStepData.IsUltimateScope(growStep))
		{
			num = ConstValue.MODE_CHANGE_COEFFICIENT_FOR_7;
		}
		else
		{
			global::Debug.Log("growStepの値が不正です");
		}
		GameWebAPI.RespDataMA_GetMonsterMS.MonsterM monsterMasterByMonsterId = MonsterDataMng.Instance().GetMonsterMasterByMonsterId(monsterId);
		int arousalValue = CalculatorUtil.GetArousalValue(monsterMasterByMonsterId);
		if (arousalValue >= 0 && arousalValue < ConstValue.MODE_CHANGE_COEFFICIENT_RARE.Length)
		{
			num2 = ConstValue.MODE_CHANGE_COEFFICIENT_RARE[arousalValue];
		}
		return num2 * num;
	}

	public static int CalcClusterForVersionUp(string baseMonsterId)
	{
		int num = 300;
		int num2 = 450;
		return num * num2;
	}

	private static int GetArousalValue(GameWebAPI.RespDataMA_GetMonsterMS.MonsterM monster)
	{
		return monster.rare.ToInt32() - 1;
	}
}
