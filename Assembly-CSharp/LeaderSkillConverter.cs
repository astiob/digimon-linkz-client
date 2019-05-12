using System;
using System.Collections.Generic;
using UnityEngine;

public class LeaderSkillConverter
{
	public static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM Convert(List<GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM> subSkillDetails)
	{
		GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM result = null;
		LeaderSkillType leaderSkillType = (LeaderSkillType)subSkillDetails[0].effectType.ToInt32();
		switch (leaderSkillType)
		{
		case LeaderSkillType.HpFollowingDamageUp:
		case LeaderSkillType.HpFollowingAttackUp:
		case LeaderSkillType.HpFollowingDefenceUp:
		case LeaderSkillType.HpFollowingSpecialAttackUp:
		case LeaderSkillType.HpFollowingSpecialDefenceUp:
		case LeaderSkillType.HpFollowingSpeedUp:
		case LeaderSkillType.HpFollowingHitRateUp:
		case LeaderSkillType.HpFollowingSatisfactionRateUp:
			result = LeaderSkillConverter.ConvertToHpFollowing(subSkillDetails.ToArray());
			break;
		case LeaderSkillType.HpMaxDamageUp:
		case LeaderSkillType.HpMaxAttackUp:
		case LeaderSkillType.HpMaxDefenceUp:
		case LeaderSkillType.HpMaxSpecialAttackUp:
		case LeaderSkillType.HpMaxSpecialDefenceUp:
		case LeaderSkillType.HpMaxSpeedUp:
		case LeaderSkillType.HpMaxHitRateUp:
		case LeaderSkillType.HpMaxMachSatisfactionRateUp:
			result = LeaderSkillConverter.ConvertToHpMax(subSkillDetails.ToArray());
			break;
		case LeaderSkillType.SpeciesMachDamageUp:
		case LeaderSkillType.SpeciesMachHpUp:
		case LeaderSkillType.SpeciesMachAttackUp:
		case LeaderSkillType.SpeciesMachDefenceUp:
		case LeaderSkillType.SpeciesMachSpecialAttackUp:
		case LeaderSkillType.SpeciesMachSpecialDefenceUp:
		case LeaderSkillType.SpeciesMachSpeedUp:
		case LeaderSkillType.SpeciesMachHitRateUp:
		case LeaderSkillType.SpeciesMachSatisfactionRateUp:
			result = LeaderSkillConverter.ConvertToSpeciesMach(subSkillDetails.ToArray());
			break;
		case LeaderSkillType.DamageUp:
		case LeaderSkillType.HpUp:
		case LeaderSkillType.AttackUp:
		case LeaderSkillType.DefenceUp:
		case LeaderSkillType.SpecialAttackUp:
		case LeaderSkillType.SpecialDefenceUp:
		case LeaderSkillType.SpeedUp:
		case LeaderSkillType.HitRateUp:
		case LeaderSkillType.SatisfactionRateUp:
			result = LeaderSkillConverter.ConvertToUp(subSkillDetails.ToArray());
			break;
		case LeaderSkillType.ToleranceUp:
			result = LeaderSkillConverter.ConvertToToleranceUp(subSkillDetails.ToArray());
			break;
		default:
			UnityEngine.Debug.LogError("Not LeaderSkillType " + leaderSkillType);
			break;
		}
		return result;
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToHpFollowing(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
	{
		return new GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM
		{
			skillId = skillDetails[0].skillId,
			subId = skillDetails[0].subId,
			effectType = skillDetails[0].effectType.ToInt32(),
			hitRate = skillDetails[0].hitRate.ToInt32(),
			target = skillDetails[0].target.ToInt32(),
			targetType = skillDetails[0].targetType.ToInt32(),
			attribute = skillDetails[0].attribute.ToInt32(),
			isMissTrough = skillDetails[0].isMissTrough.ToInt32(),
			effect1 = skillDetails[0].effect.ToInt32(),
			effect2 = skillDetails[0].subRate.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToHpMax(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
	{
		return new GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM
		{
			skillId = skillDetails[0].skillId,
			subId = skillDetails[0].subId,
			effectType = skillDetails[0].effectType.ToInt32(),
			hitRate = skillDetails[0].hitRate.ToInt32(),
			target = skillDetails[0].target.ToInt32(),
			targetType = skillDetails[0].targetType.ToInt32(),
			attribute = skillDetails[0].attribute.ToInt32(),
			isMissTrough = skillDetails[0].isMissTrough.ToInt32(),
			effect1 = skillDetails[0].effect.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToSpeciesMach(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
	{
		return new GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM
		{
			skillId = skillDetails[0].skillId,
			subId = skillDetails[0].subId,
			effectType = skillDetails[0].effectType.ToInt32(),
			hitRate = skillDetails[0].hitRate.ToInt32(),
			target = skillDetails[0].target.ToInt32(),
			targetType = skillDetails[0].targetType.ToInt32(),
			attribute = skillDetails[0].attribute.ToInt32(),
			isMissTrough = skillDetails[0].isMissTrough.ToInt32(),
			effect1 = skillDetails[0].effect.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToUp(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
	{
		return new GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM
		{
			skillId = skillDetails[0].skillId,
			subId = skillDetails[0].subId,
			effectType = skillDetails[0].effectType.ToInt32(),
			hitRate = skillDetails[0].hitRate.ToInt32(),
			target = skillDetails[0].target.ToInt32(),
			targetType = skillDetails[0].targetType.ToInt32(),
			attribute = skillDetails[0].attribute.ToInt32(),
			isMissTrough = skillDetails[0].isMissTrough.ToInt32(),
			effect1 = skillDetails[0].effect.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToToleranceUp(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
	{
		return new GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM
		{
			skillId = skillDetails[0].skillId,
			subId = skillDetails[0].subId,
			effectType = skillDetails[0].effectType.ToInt32(),
			hitRate = skillDetails[0].hitRate.ToInt32(),
			target = skillDetails[0].target.ToInt32(),
			targetType = skillDetails[0].targetType.ToInt32(),
			attribute = skillDetails[0].attribute.ToInt32(),
			isMissTrough = skillDetails[0].isMissTrough.ToInt32(),
			effect3 = skillDetails[0].effect.ToInt32(),
			effect4 = skillDetails[1].effect.ToInt32(),
			effect5 = skillDetails[2].effect.ToInt32(),
			effect6 = skillDetails[3].effect.ToInt32(),
			effect7 = skillDetails[4].effect.ToInt32(),
			effect8 = skillDetails[5].effect.ToInt32(),
			effect9 = skillDetails[6].effect.ToInt32(),
			effect10 = skillDetails[7].effect.ToInt32(),
			effect11 = skillDetails[8].effect.ToInt32(),
			effect12 = skillDetails[9].effect.ToInt32(),
			effect13 = skillDetails[10].effect.ToInt32(),
			effect14 = skillDetails[11].effect.ToInt32(),
			effect15 = skillDetails[12].effect.ToInt32(),
			effect16 = skillDetails[13].effect.ToInt32()
		};
	}
}
