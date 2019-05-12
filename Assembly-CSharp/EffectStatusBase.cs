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
		Skill,
		Suffer,
		MonsterIntegrationGroup
	}

	public enum ExtraEffectType
	{
		Non,
		Damage = 10,
		SkillDamage,
		SkillHit,
		DefaultAttackDamage = 16,
		DefaultAttackHit,
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
		LeaderChange = 70,
		StageEffextInvalid = 80
	}

	public enum ExtraEffectSubType
	{
		Non,
		Overwrite,
		Ratio,
		Fixed
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
		AttackHit,
		Suffer,
		SkillMiss,
		SkillStartedApMax,
		AttackCommandedTarget,
		SkillSpecies,
		SkillTargetSpecies,
		SkillAttribute,
		SkillRecieveAttribute
	}
}
