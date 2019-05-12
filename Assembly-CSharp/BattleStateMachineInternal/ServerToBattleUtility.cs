using Enemy.AI;
using Enemy.DropItem;
using System;
using UnityEngine;

namespace BattleStateMachineInternal
{
	public static class ServerToBattleUtility
	{
		public static float PermillionToPercentage(int value)
		{
			return Mathf.Clamp01((float)value / 10000f);
		}

		public static int PercentageToPermillion(float value)
		{
			return (int)Math.Round((double)(value * 10000f), MidpointRounding.AwayFromZero);
		}

		public static float PermillionToPercentage(int value, bool useClamp)
		{
			return (!useClamp) ? ((float)value / 10000f) : ServerToBattleUtility.PermillionToPercentage(value);
		}

		public static float HundredToPercentage(int value)
		{
			return Mathf.Clamp01((float)value / 100f);
		}

		public static float PermillionToPercentage(string value)
		{
			return ServerToBattleUtility.PermillionToPercentage(int.Parse(value));
		}

		public static AICycle IntToAICycle(int value)
		{
			if (value == 1)
			{
				return AICycle.fixableRotation;
			}
			if (value != 2)
			{
				global::Debug.LogError("AICycleの値が不正です. (" + value + ")");
				return AICycle.fixableRotation;
			}
			return AICycle.targetHpAltenation;
		}

		public static EffectTarget IntToEffectTarget(int value)
		{
			switch (value)
			{
			case 1:
				return EffectTarget.Enemy;
			case 2:
				return EffectTarget.Ally;
			case 3:
				return EffectTarget.Attacker;
			case 4:
				return EffectTarget.EnemyWithoutAttacker;
			case 5:
				return EffectTarget.AllyWithoutAttacker;
			default:
				global::Debug.LogError("EffectTargetの値が不正です. (" + value + ")");
				return EffectTarget.Enemy;
			}
		}

		public static TargetSelectReference IntToTargetSelectReference(int value)
		{
			int num = value - 1;
			if (!Enum.IsDefined(typeof(TargetSelectReference), num))
			{
				global::Debug.LogError("TargetSelectReferenceの値が不正です. (" + value + ")");
			}
			return (TargetSelectReference)num;
		}

		public static SelectingOrder IntToSelectingOrder(int value)
		{
			if (value == 1)
			{
				return SelectingOrder.HighAndHave;
			}
			if (value != 2)
			{
				global::Debug.LogError("SelectingOrderの値が不正です. (" + value + ")");
				return SelectingOrder.HighAndHave;
			}
			return SelectingOrder.LowAndHavent;
		}

		public static DropBoxType IntToDropBoxType(int value)
		{
			if (value == 1)
			{
				return DropBoxType.Normal;
			}
			if (value != 2)
			{
				global::Debug.LogError("DropBoxTypeの値が不正です. (" + value + ")");
				return DropBoxType.Normal;
			}
			return DropBoxType.Rare;
		}

		public static EffectNumbers IntToEffectNumbers(int value)
		{
			if (value == 1)
			{
				return EffectNumbers.Simple;
			}
			if (value != 2)
			{
				global::Debug.LogError("EffectNumbersの値が不正です. (" + value + ")");
				return EffectNumbers.Simple;
			}
			return EffectNumbers.All;
		}

		public static global::Attribute IntToAttribute(int value)
		{
			switch (value)
			{
			case 1:
				return global::Attribute.None;
			case 2:
				return global::Attribute.Red;
			case 3:
				return global::Attribute.Blue;
			case 4:
				return global::Attribute.Yellow;
			case 5:
				return global::Attribute.Green;
			case 6:
				return global::Attribute.White;
			case 7:
				return global::Attribute.Black;
			default:
				return global::Attribute.None;
			}
		}

		public static AffectEffect IntToAffectEffect(int value)
		{
			switch (value)
			{
			case 1:
				return AffectEffect.Damage;
			case 2:
				return AffectEffect.AttackUp;
			case 3:
				return AffectEffect.AttackDown;
			case 4:
				return AffectEffect.DefenceUp;
			case 5:
				return AffectEffect.DefenceDown;
			case 6:
				return AffectEffect.SpAttackUp;
			case 7:
				return AffectEffect.SpAttackDown;
			case 8:
				return AffectEffect.SpDefenceUp;
			case 9:
				return AffectEffect.SpDefenceDown;
			case 10:
				return AffectEffect.SpeedUp;
			case 11:
				return AffectEffect.SpeedDown;
			case 12:
				return AffectEffect.CorrectionUpReset;
			case 13:
				return AffectEffect.CorrectionDownReset;
			case 14:
				return AffectEffect.HpRevival;
			case 15:
				return AffectEffect.Counter;
			case 16:
				return AffectEffect.Reflection;
			case 17:
				return AffectEffect.Protect;
			case 18:
				return AffectEffect.HateUp;
			case 19:
				return AffectEffect.HateDown;
			case 20:
				return AffectEffect.PowerCharge;
			case 21:
				return AffectEffect.Destruct;
			case 22:
				return AffectEffect.Paralysis;
			case 23:
				return AffectEffect.Poison;
			case 24:
				return AffectEffect.Sleep;
			case 25:
				return AffectEffect.SkillLock;
			case 26:
				return AffectEffect.HitRateUp;
			case 27:
				return AffectEffect.HitRateDown;
			case 28:
				return AffectEffect.InstantDeath;
			case 29:
				return AffectEffect.Confusion;
			case 30:
				return AffectEffect.Stun;
			case 31:
				return AffectEffect.SufferStatusClear;
			case 32:
				return AffectEffect.SatisfactionRateUp;
			case 33:
				return AffectEffect.SatisfactionRateDown;
			case 34:
				return AffectEffect.Damage;
			case 35:
				return AffectEffect.HpRevival;
			case 36:
				return AffectEffect.Damage;
			case 37:
				return AffectEffect.Damage;
			case 38:
				return AffectEffect.Poison;
			case 39:
				return AffectEffect.ApRevival;
			case 40:
				return AffectEffect.ApRevival;
			case 41:
				return AffectEffect.ApDown;
			case 42:
				return AffectEffect.ApUp;
			case 43:
				return AffectEffect.ApDown;
			case 44:
				return AffectEffect.ApUp;
			case 45:
				return AffectEffect.ApConsumptionDown;
			case 46:
				return AffectEffect.ApConsumptionUp;
			case 47:
				return AffectEffect.Damage;
			case 48:
				return AffectEffect.Damage;
			case 49:
				return AffectEffect.Damage;
			case 50:
				return AffectEffect.Damage;
			case 51:
				return AffectEffect.CountGuard;
			case 52:
				return AffectEffect.TurnBarrier;
			case 53:
				return AffectEffect.CountBarrier;
			case 54:
				return AffectEffect.Recommand;
			case 55:
				return AffectEffect.DamageRateUp;
			case 56:
				return AffectEffect.DamageRateDown;
			case 57:
				return AffectEffect.Regenerate;
			case 58:
				return AffectEffect.Regenerate;
			case 59:
				return AffectEffect.TurnEvasion;
			case 60:
				return AffectEffect.CountEvasion;
			case 61:
				return AffectEffect.ReferenceTargetHpRate;
			case 62:
				return AffectEffect.ApDrain;
			case 63:
				return AffectEffect.HpBorderlineDamage;
			case 64:
				return AffectEffect.HpBorderlineSpDamage;
			case 65:
				return AffectEffect.DefenseThroughDamage;
			case 66:
				return AffectEffect.DefenseThroughDamage;
			case 67:
				return AffectEffect.HpSettingFixable;
			case 68:
				return AffectEffect.HpSettingPercentage;
			case 69:
				return AffectEffect.Escape;
			case 70:
				return AffectEffect.Nothing;
			case 71:
				return AffectEffect.RefHpRateNonAttribute;
			case 72:
				return AffectEffect.SkillBranch;
			case 73:
				return AffectEffect.ChangeToleranceUp;
			case 74:
				return AffectEffect.ChangeToleranceDown;
			case 75:
				return AffectEffect.ClearTolerance;
			default:
				global::Debug.LogError("AffectEffectの値が不正です. (" + value + ")");
				return AffectEffect.Damage;
			}
		}

		public static bool GetUseDrainAffectEffect(int value)
		{
			return value == 36 || value == 37 || value == 49 || value == 50;
		}

		public static PowerType GetPowerType(int value)
		{
			switch (value)
			{
			case 47:
				return PowerType.Fixable;
			case 48:
				return PowerType.Fixable;
			case 49:
				return PowerType.Fixable;
			case 50:
				return PowerType.Fixable;
			default:
				switch (value)
				{
				case 39:
					return PowerType.Fixable;
				default:
					if (value == 14)
					{
						return PowerType.Fixable;
					}
					if (value == 23)
					{
						return PowerType.Fixable;
					}
					if (value == 58)
					{
						return PowerType.Fixable;
					}
					if (value != 67)
					{
						return PowerType.Percentage;
					}
					return PowerType.Fixable;
				case 41:
					return PowerType.Fixable;
				case 42:
					return PowerType.Fixable;
				}
				break;
			}
		}

		public static TechniqueType GetTechniqueType(int value)
		{
			switch (value)
			{
			case 47:
				return TechniqueType.Physics;
			case 48:
				return TechniqueType.Special;
			case 49:
				return TechniqueType.Physics;
			case 50:
				return TechniqueType.Special;
			default:
				switch (value)
				{
				case 34:
					return TechniqueType.Special;
				default:
					switch (value)
					{
					case 64:
						return TechniqueType.Special;
					default:
						if (value != 1)
						{
							return TechniqueType.Physics;
						}
						return TechniqueType.Physics;
					case 66:
						return TechniqueType.Special;
					}
					break;
				case 36:
					return TechniqueType.Physics;
				case 37:
					return TechniqueType.Special;
				}
				break;
			}
		}

		public static Strength IntToStrength(string value)
		{
			if (value != null)
			{
				if (value == "-1")
				{
					return Strength.Weak;
				}
				if (value == "0")
				{
					return Strength.None;
				}
				if (value == "1")
				{
					return Strength.Strong;
				}
				if (value == "2")
				{
					return Strength.Drain;
				}
				if (value == "99")
				{
					return Strength.Invalid;
				}
			}
			global::Debug.LogError("Strengthの値が不正です. (" + value + ")");
			return Strength.None;
		}

		public static LeaderSkillType IntToLeaderSkillType(int value)
		{
			if (Enum.IsDefined(typeof(LeaderSkillType), value))
			{
				return (LeaderSkillType)value;
			}
			global::Debug.LogError("LeaderSkillTypeの値が不正です. (" + value + ")");
			return LeaderSkillType.HpFollowingDamageUp;
		}

		public static bool GetOnHpFollowingLeaderSkill(LeaderSkillType value)
		{
			return value == LeaderSkillType.HpFollowingDamageUp || value == LeaderSkillType.HpFollowingAttackUp || value == LeaderSkillType.HpFollowingDefenceUp || value == LeaderSkillType.HpFollowingSpecialAttackUp || value == LeaderSkillType.HpFollowingSpecialDefenceUp || value == LeaderSkillType.HpFollowingSpeedUp || value == LeaderSkillType.HpFollowingHitRateUp || value == LeaderSkillType.HpFollowingSatisfactionRateUp;
		}

		public static bool GetIsLeaderSkill(string value)
		{
			switch (int.Parse(value))
			{
			case 1:
				return true;
			case 2:
				return false;
			case 3:
				return false;
			case 4:
				return false;
			default:
				global::Debug.LogError("GetIsLeaderSkillは想定外の値です. (" + value + ")");
				return false;
			}
		}

		public static SkillType IntToSkillType(string value)
		{
			switch (int.Parse(value))
			{
			case 2:
				return SkillType.Deathblow;
			case 3:
				return SkillType.InheritanceTechnique;
			case 4:
				return SkillType.Attack;
			default:
				global::Debug.LogError("SkillTypeの値が不正です. (" + value + ")");
				return SkillType.Attack;
			}
		}

		public static Tolerance ResistanceToTolerance(GameWebAPI.RespDataMA_GetMonsterResistanceM.MonsterResistanceM data)
		{
			return new Tolerance(ServerToBattleUtility.IntToStrength(data.none), ServerToBattleUtility.IntToStrength(data.fire), ServerToBattleUtility.IntToStrength(data.water), ServerToBattleUtility.IntToStrength(data.thunder), ServerToBattleUtility.IntToStrength(data.nature), ServerToBattleUtility.IntToStrength(data.light), ServerToBattleUtility.IntToStrength(data.dark), ServerToBattleUtility.IntToStrength(data.poison), ServerToBattleUtility.IntToStrength(data.confusion), ServerToBattleUtility.IntToStrength(data.paralysis), ServerToBattleUtility.IntToStrength(data.sleep), ServerToBattleUtility.IntToStrength(data.stun), ServerToBattleUtility.IntToStrength(data.skillLock), ServerToBattleUtility.IntToStrength(data.death));
		}
	}
}
