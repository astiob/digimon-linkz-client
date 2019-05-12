using System;
using System.Collections.Generic;
using UnityEngine;

public class StandardSkillConverter
{
	public static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM Convert(List<GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM> subSkillDetails)
	{
		GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM result = null;
		StandardSkillConverter.AffectEffect affectEffect = (StandardSkillConverter.AffectEffect)subSkillDetails[0].effectType.ToInt32();
		switch (affectEffect)
		{
		case StandardSkillConverter.AffectEffect.PhysicalDamage:
		case StandardSkillConverter.AffectEffect.SpecialDamage:
		case StandardSkillConverter.AffectEffect.PhysicalDamegeFixable:
		case StandardSkillConverter.AffectEffect.SpecialDamageFixable:
			result = StandardSkillConverter.ConvertToDamage(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.AttackUp:
		case StandardSkillConverter.AffectEffect.AttackDown:
		case StandardSkillConverter.AffectEffect.DefenceUp:
		case StandardSkillConverter.AffectEffect.DefenceDown:
		case StandardSkillConverter.AffectEffect.SpAttackUp:
		case StandardSkillConverter.AffectEffect.SpAttackDown:
		case StandardSkillConverter.AffectEffect.SpDefenceUp:
		case StandardSkillConverter.AffectEffect.SpDefenceDown:
		case StandardSkillConverter.AffectEffect.SpeedUp:
		case StandardSkillConverter.AffectEffect.SpeedDown:
		case StandardSkillConverter.AffectEffect.HitRateUp:
		case StandardSkillConverter.AffectEffect.HitRateDown:
		case StandardSkillConverter.AffectEffect.SatisfactionLateUp:
		case StandardSkillConverter.AffectEffect.SatisfactionLateDown:
			result = StandardSkillConverter.ConvertToCorrectionUpDown(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.CorrectionUpReset:
		case StandardSkillConverter.AffectEffect.CorrectionDownReset:
			result = StandardSkillConverter.ConvertToCorrectionReset(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.HpRevivalFixable:
			result = StandardSkillConverter.ConvertToHpRevivalFixable(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.Counter:
		case StandardSkillConverter.AffectEffect.Reflection:
			result = StandardSkillConverter.ConvertToCounterReflection(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.Protect:
			result = StandardSkillConverter.ConvertToProtect(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.HateUp:
		case StandardSkillConverter.AffectEffect.HateDown:
			result = StandardSkillConverter.ConvertToHate(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.PowerCharge:
			result = StandardSkillConverter.ConvertToPowerCharge(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.Destruction:
			result = StandardSkillConverter.ConvertToDestruction(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.Paralysis:
			result = StandardSkillConverter.ConvertToParalysis(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.PoisonFixable:
			result = StandardSkillConverter.ConvertToPoisonFixable(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.Sleep:
			result = StandardSkillConverter.ConvertToSleep(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.SlillLock:
			result = StandardSkillConverter.ConvertToSlillLock(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.InstantDeath:
			result = StandardSkillConverter.ConvertToInstantDeath(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.Confusion:
			result = StandardSkillConverter.ConvertToConfusion(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.Stun:
			result = StandardSkillConverter.ConvertToStun(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.SufferStatusClear:
			result = StandardSkillConverter.ConvertToSufferStatusClear(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.HpRevivalPercentage:
			result = StandardSkillConverter.ConvertToHpRevivalPercentage(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.PhysicalDamegeDrain:
		case StandardSkillConverter.AffectEffect.SpecialDamageDrain:
		case StandardSkillConverter.AffectEffect.PhysicalDamegeDrainFixable:
		case StandardSkillConverter.AffectEffect.SpecialDamageDrainFixable:
			result = StandardSkillConverter.ConvertToDamegeDrain(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.PoisonPercentage:
			result = StandardSkillConverter.ConvertToPoisonPercentage(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.ApBoosterFixable:
			result = StandardSkillConverter.ConvertToApBoosterFixable(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.ApBoosterPercentage:
			result = StandardSkillConverter.ConvertToApBoosterPercentage(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.ApDownFixable:
		case StandardSkillConverter.AffectEffect.ApUpFixable:
			result = StandardSkillConverter.ConvertToApUpDownFixable(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.ApDownPercentage:
		case StandardSkillConverter.AffectEffect.ApUpPercentage:
			result = StandardSkillConverter.ConvertToApUpDownPercentage(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.ApConsumptionDown:
		case StandardSkillConverter.AffectEffect.ApConsumptionUp:
			result = StandardSkillConverter.ConvertToApConsumptionUpDown(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.CountGuard:
			result = StandardSkillConverter.ConvertToCountGuard(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.TurnBarrier:
			result = StandardSkillConverter.ConvertToTurnBarrier(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.CountBarrier:
			result = StandardSkillConverter.ConvertToCountBarrier(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.Recommand:
			result = StandardSkillConverter.ConvertToRecommand(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.DamageRateUp:
		case StandardSkillConverter.AffectEffect.DamageRateDown:
			result = StandardSkillConverter.ConvertToDamageRateUpDown(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.RegeneratePercentage:
			result = StandardSkillConverter.ConvertToRegeneratePercentage(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.RegenerateFixable:
			result = StandardSkillConverter.ConvertToRegenerateFixable(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.TurnEvasion:
			result = StandardSkillConverter.ConvertToTurnEvasion(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.CountEvasion:
			result = StandardSkillConverter.ConvertToCountEvasion(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.ReferenceTargetHpRate:
			result = StandardSkillConverter.ConvertToReferenceTargetHpRate(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.ApDrain:
			result = StandardSkillConverter.ConvertToApDrain(subSkillDetails.ToArray());
			break;
		case StandardSkillConverter.AffectEffect.HpBorderlineDamage:
			result = StandardSkillConverter.ConvertToHpBorderlineDamage(subSkillDetails.ToArray());
			break;
		default:
			UnityEngine.Debug.LogError("Not AffectEffect " + affectEffect);
			break;
		}
		return result;
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToDamage(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect1 = skillDetails[0].motionCount.ToInt32(),
			effect2 = skillDetails[0].effect.ToInt32(),
			effect4 = skillDetails[0].subRate.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToDamegeDrain(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
	{
		GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM skillDetailM = new GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM();
		skillDetailM.skillId = skillDetails[0].skillId;
		skillDetailM.subId = skillDetails[0].subId;
		skillDetailM.effectType = skillDetails[0].effectType.ToInt32();
		skillDetailM.hitRate = skillDetails[0].hitRate.ToInt32();
		skillDetailM.target = skillDetails[0].target.ToInt32();
		skillDetailM.targetType = skillDetails[0].targetType.ToInt32();
		skillDetailM.attribute = skillDetails[0].attribute.ToInt32();
		skillDetailM.isMissTrough = skillDetails[0].isMissTrough.ToInt32();
		skillDetailM.effect1 = skillDetails[0].motionCount.ToInt32();
		skillDetailM.effect2 = skillDetails[0].effect.ToInt32();
		if (skillDetails.Length >= 2 && skillDetails[1] != null)
		{
			skillDetailM.effect3 = skillDetails[1].effect.ToInt32();
		}
		skillDetailM.effect4 = skillDetails[0].subRate.ToInt32();
		return skillDetailM;
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToCorrectionUpDown(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect1 = skillDetails[0].continuousRound.ToInt32(),
			effect3 = skillDetails[0].effect.ToInt32(),
			effect4 = skillDetails[0].effect2.ToInt32(),
			effect5 = skillDetails[0].effect3.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToCorrectionReset(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			isMissTrough = skillDetails[0].isMissTrough.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToHpRevivalFixable(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect2 = skillDetails[0].effect.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToHpRevivalPercentage(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect3 = skillDetails[0].effect.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToCounterReflection(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect1 = skillDetails[0].continuousRound.ToInt32(),
			effect3 = skillDetails[0].effect.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToProtect(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect1 = skillDetails[0].continuousRound.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToHate(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect1 = skillDetails[0].continuousRound.ToInt32(),
			effect2 = skillDetails[0].effect.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToPowerCharge(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect1 = skillDetails[0].continuousRound.ToInt32(),
			effect3 = skillDetails[0].effect.ToInt32(),
			effect4 = skillDetails[1].effect.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToDestruction(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			isMissTrough = skillDetails[0].isMissTrough.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToParalysis(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect1 = skillDetails[0].continuousRound.ToInt32(),
			effect3 = skillDetails[0].effect.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToPoisonFixable(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect1 = skillDetails[0].continuousRound.ToInt32(),
			effect2 = skillDetails[0].effect.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToPoisonPercentage(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect1 = skillDetails[0].continuousRound.ToInt32(),
			effect3 = skillDetails[0].effect.ToInt32(),
			effect4 = skillDetails[0].effect2.ToInt32(),
			effect5 = skillDetails[0].effect3.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToSleep(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect1 = skillDetails[0].continuousRound.ToInt32(),
			effect3 = skillDetails[0].effect.ToInt32(),
			effect4 = skillDetails[0].subRate.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToSlillLock(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect1 = skillDetails[0].continuousRound.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToInstantDeath(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			isMissTrough = skillDetails[0].isMissTrough.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToConfusion(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect1 = skillDetails[0].continuousRound.ToInt32(),
			effect3 = skillDetails[0].effect.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToStun(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect1 = skillDetails[0].continuousRound.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToSufferStatusClear(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect3 = skillDetails[1].effect.ToInt32(),
			effect4 = skillDetails[2].effect.ToInt32(),
			effect5 = skillDetails[3].effect.ToInt32(),
			effect6 = skillDetails[4].effect.ToInt32(),
			effect7 = skillDetails[5].effect.ToInt32(),
			effect8 = skillDetails[6].effect.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToApBoosterFixable(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect2 = skillDetails[0].effect.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToApBoosterPercentage(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect3 = skillDetails[0].effect.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToApUpDownFixable(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect2 = skillDetails[0].effect.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToApUpDownPercentage(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect3 = skillDetails[0].effect.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToApConsumptionUpDown(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect1 = skillDetails[0].continuousRound.ToInt32(),
			effect2 = skillDetails[0].effect.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToCountGuard(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect1 = skillDetails[0].motionCount.ToInt32(),
			effect3 = skillDetails[0].effect.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToTurnBarrier(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect1 = skillDetails[0].continuousRound.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToCountBarrier(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect1 = skillDetails[0].motionCount.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToTurnEvasion(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect1 = skillDetails[0].continuousRound.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToCountEvasion(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect1 = skillDetails[0].motionCount.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToRecommand(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			isMissTrough = skillDetails[0].isMissTrough.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToDamageRateUpDown(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect1 = skillDetails[0].continuousRound.ToInt32(),
			effect3 = skillDetails[1].effect.ToInt32(),
			effect4 = skillDetails[2].effect.ToInt32(),
			effect5 = skillDetails[3].effect.ToInt32(),
			effect6 = skillDetails[4].effect.ToInt32(),
			effect7 = skillDetails[5].effect.ToInt32(),
			effect8 = skillDetails[6].effect.ToInt32(),
			effect9 = skillDetails[7].effect.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToRegeneratePercentage(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect1 = skillDetails[0].continuousRound.ToInt32(),
			effect3 = skillDetails[0].effect.ToInt32(),
			effect4 = skillDetails[0].effect2.ToInt32(),
			effect5 = skillDetails[0].effect3.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToRegenerateFixable(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect1 = skillDetails[0].continuousRound.ToInt32(),
			effect2 = skillDetails[0].effect.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToReferenceTargetHpRate(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect1 = skillDetails[0].motionCount.ToInt32(),
			effect3 = skillDetails[0].effect.ToInt32(),
			effect4 = skillDetails[0].subRate.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToApDrain(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect2 = skillDetails[0].effect.ToInt32()
		};
	}

	private static GameWebAPI.RespDataMA_GetSkillDetailM.SkillDetailM ConvertToHpBorderlineDamage(GameWebAPI.RespDataMA_GetSkillDetailM.ReceiveSkillDetailM[] skillDetails)
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
			effect1 = skillDetails[0].motionCount.ToInt32(),
			effect4 = skillDetails[0].subRate.ToInt32(),
			effect5 = skillDetails[0].effect.ToInt32(),
			effect6 = skillDetails[0].effect2.ToInt32(),
			effect7 = skillDetails[0].effect3.ToInt32(),
			effect8 = skillDetails[0].effect4.ToInt32(),
			effect9 = skillDetails[0].effect5.ToInt32()
		};
	}

	public enum AffectEffect
	{
		PhysicalDamage = 1,
		AttackUp,
		AttackDown,
		DefenceUp,
		DefenceDown,
		SpAttackUp,
		SpAttackDown,
		SpDefenceUp,
		SpDefenceDown,
		SpeedUp,
		SpeedDown,
		CorrectionUpReset,
		CorrectionDownReset,
		HpRevivalFixable,
		Counter,
		Reflection,
		Protect,
		HateUp,
		HateDown,
		PowerCharge,
		Destruction,
		Paralysis,
		PoisonFixable,
		Sleep,
		SlillLock,
		HitRateUp,
		HitRateDown,
		InstantDeath,
		Confusion,
		Stun,
		SufferStatusClear,
		SatisfactionLateUp,
		SatisfactionLateDown,
		SpecialDamage,
		HpRevivalPercentage,
		PhysicalDamegeDrain,
		SpecialDamageDrain,
		PoisonPercentage,
		ApBoosterFixable,
		ApBoosterPercentage,
		ApDownFixable,
		ApUpFixable,
		ApDownPercentage,
		ApUpPercentage,
		ApConsumptionDown,
		ApConsumptionUp,
		PhysicalDamegeFixable,
		SpecialDamageFixable,
		PhysicalDamegeDrainFixable,
		SpecialDamageDrainFixable,
		CountGuard,
		TurnBarrier,
		CountBarrier,
		Recommand,
		DamageRateUp,
		DamageRateDown,
		RegeneratePercentage,
		RegenerateFixable,
		TurnEvasion,
		CountEvasion,
		ReferenceTargetHpRate,
		ApDrain,
		HpBorderlineDamage
	}
}
