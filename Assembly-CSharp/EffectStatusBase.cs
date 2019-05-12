using System;

public class EffectStatusBase
{
	protected static ConstValue.ResistanceType GetSkillResistanceType(AffectEffectProperty skillPropety)
	{
		ConstValue.ResistanceType result = ConstValue.ResistanceType.NONE;
		if (!AffectEffectProperty.IsDamage(skillPropety.type))
		{
			switch (skillPropety.type)
			{
			case AffectEffect.Paralysis:
				result = ConstValue.ResistanceType.PARALYSIS;
				break;
			case AffectEffect.Poison:
				result = ConstValue.ResistanceType.POISON;
				break;
			case AffectEffect.Sleep:
				result = ConstValue.ResistanceType.SLEEP;
				break;
			case AffectEffect.SkillLock:
				result = ConstValue.ResistanceType.SKILL_LOCK;
				break;
			case AffectEffect.InstantDeath:
				result = ConstValue.ResistanceType.DEATH;
				break;
			case AffectEffect.Confusion:
				result = ConstValue.ResistanceType.CONFUSION;
				break;
			case AffectEffect.Stun:
				result = ConstValue.ResistanceType.STUN;
				break;
			}
		}
		else
		{
			switch (skillPropety.attribute)
			{
			case global::Attribute.None:
				result = ConstValue.ResistanceType.NOTHINGNESS;
				break;
			case global::Attribute.Red:
				result = ConstValue.ResistanceType.FIRE;
				break;
			case global::Attribute.Blue:
				result = ConstValue.ResistanceType.WATER;
				break;
			case global::Attribute.Yellow:
				result = ConstValue.ResistanceType.THUNDER;
				break;
			case global::Attribute.Green:
				result = ConstValue.ResistanceType.NATURE;
				break;
			case global::Attribute.White:
				result = ConstValue.ResistanceType.LIGHT;
				break;
			case global::Attribute.Black:
				result = ConstValue.ResistanceType.DARK;
				break;
			}
		}
		return result;
	}

	protected enum ExtraTargetSubType
	{
		Non,
		MonsterResistance,
		MonsterTribe,
		MonsterGroup,
		GrowStep,
		Quest,
		SkillId,
		Suffer,
		MonsterIntegrationGroup,
		SkillAttribute
	}

	public enum ExtraEffectType
	{
		Non,
		SkillDamage = 10,
		SkillPower,
		SkillHit,
		Atk = 21,
		Def,
		Hp,
		Speed,
		Satk,
		Sdef,
		AllStatus,
		Cluster = 31,
		Critical = 40,
		Hit,
		Poison = 50,
		Confusion,
		Sleep,
		Paralysis,
		Stun,
		SkillLock,
		Counter,
		Guts,
		HittingTheTarget,
		CriticalTheTarget,
		Skill,
		SkillAdd,
		DropRateUp = 70,
		ExtaraStageRateUp,
		DropCountUp,
		FloorLotUp,
		SpeedClearDropCountUp,
		StageEffextInvalid = 80,
		LeaderChange = 90
	}

	public enum ExtraEffectSubType
	{
		Non,
		Overwrite,
		Ratio,
		Fixed,
		SingleBootFixed = 13
	}

	public enum EffectTriggerType
	{
		Usually,
		TurnStarted,
		TurnEnd,
		WaveStarted,
		WaveEnd,
		HpPercentage,
		Dead,
		HpFixed,
		RoundStarted,
		RoundEnd,
		Kill,
		Area,
		AttackStarted,
		SufferHit,
		SkillMiss,
		SkillStartedApMax,
		AttackCommandedTarget,
		SkillSpecies,
		SkillTargetSpecies,
		SkillAttributeStartedSend,
		SkillAttributeStartedRecieve,
		LastDead,
		MonsterGroupId,
		MonsterIntegrationGroupId,
		SkillDamageStartedSend,
		SkillDamageStartedRecieve,
		SkillDamageHitSend,
		SkillDamageHitRecieve,
		SkillDamageEndSend,
		SkillDamageEndRecieve,
		DamagePossibility,
		SkillDamageStartedSendEvery,
		SkillDamageStartedRecieveEvery,
		SkillDamageHitSendEvery,
		SkillDamageHitRecieveEvery,
		SkillDamageEndSendEvery,
		SkillDamageEndRecieveEvery,
		SufferHitAndUseSkill
	}
}
