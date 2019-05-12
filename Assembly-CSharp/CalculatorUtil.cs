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

	public static int CalcClusterForReinforcement(List<MonsterData> partnerDigimons)
	{
		GameWebAPI.RespDataCP_Campaign respDataCP_Campaign = DataMng.Instance().RespDataCP_Campaign;
		GameWebAPI.RespDataCP_Campaign.CampaignInfo campaign = respDataCP_Campaign.GetCampaign(GameWebAPI.RespDataCP_Campaign.CampaignType.TrainCostDown, false);
		float num = 1f;
		if (campaign != null)
		{
			num = campaign.rate.ToFloat();
		}
		float reinforcementCost = MonsterDataMng.Instance().GetReinforcementCost(partnerDigimons);
		return Mathf.FloorToInt(reinforcementCost * num);
	}

	public static int CalcClusterForEvolve(string monsterId)
	{
		GameWebAPI.RespDataMA_GetMonsterMG.MonsterM monsterGroupMasterByMonsterId = MonsterDataMng.Instance().GetMonsterGroupMasterByMonsterId(monsterId);
		int num = 0;
		int num2 = 0;
		string growStep = monsterGroupMasterByMonsterId.growStep;
		switch (growStep)
		{
		case "5":
		case "8":
			num = ConstValue.EVOLVE_COEFFICIENT_FOR_5;
			goto IL_D4;
		case "6":
			num = ConstValue.EVOLVE_COEFFICIENT_FOR_6;
			goto IL_D4;
		case "7":
		case "9":
			num = ConstValue.EVOLVE_COEFFICIENT_FOR_7;
			goto IL_D4;
		}
		global::Debug.Log("growStepの値が不正です");
		IL_D4:
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
		switch (monsterGroupMasterByMonsterId.growStep.ToInt32())
		{
		case 5:
		case 8:
			num = ConstValue.MODE_CHANGE_COEFFICIENT_FOR_5;
			break;
		case 6:
			num = ConstValue.MODE_CHANGE_COEFFICIENT_FOR_6;
			break;
		case 7:
		case 9:
			num = ConstValue.MODE_CHANGE_COEFFICIENT_FOR_7;
			break;
		default:
			global::Debug.Log("growStepの値が不正です");
			break;
		}
		GameWebAPI.RespDataMA_GetMonsterMS.MonsterM monsterMasterByMonsterId = MonsterDataMng.Instance().GetMonsterMasterByMonsterId(monsterId);
		int arousalValue = CalculatorUtil.GetArousalValue(monsterMasterByMonsterId);
		if (arousalValue >= 0 && arousalValue < ConstValue.MODE_CHANGE_COEFFICIENT_RARE.Length)
		{
			num2 = ConstValue.MODE_CHANGE_COEFFICIENT_RARE[arousalValue];
		}
		return num2 * num;
	}

	private static int GetArousalValue(GameWebAPI.RespDataMA_GetMonsterMS.MonsterM monster)
	{
		return monster.rare.ToInt32() - 1;
	}
}
