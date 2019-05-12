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

		public static float PermillionToPercentage(int value, bool useClamp)
		{
			return (!useClamp) ? ((float)value / 10000f) : ServerToBattleUtility.PermillionToPercentage(value);
		}

		public static float HundredToPercentage(int value)
		{
			return Mathf.Clamp01((float)value / 100f);
		}

		public static int ServerSeqIdToCpuSeqId(int value)
		{
			return value - 1;
		}

		public static int ServerValueToInt(string value)
		{
			return int.Parse(value);
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

		public static DropAssetType IntToDropAssetType(int value)
		{
			switch (value)
			{
			case 1:
				return DropAssetType.Monster;
			case 2:
				return DropAssetType.DigiStone;
			case 3:
				return DropAssetType.LinkPoint;
			case 4:
				return DropAssetType.Chip;
			case 5:
				return DropAssetType.Exp;
			case 6:
				return DropAssetType.Item;
			case 7:
				return DropAssetType.MonsterSlots;
			case 8:
				return DropAssetType.DeckCosts;
			case 9:
				return DropAssetType.FriendMaxNumber;
			case 10:
				return DropAssetType.StaminaMaxNumber;
			case 11:
				return DropAssetType.UnitedFrontTicket;
			case 12:
				return DropAssetType.Meat;
			case 13:
				return DropAssetType.Soul;
			case 14:
				return DropAssetType.Plugin;
			default:
				global::Debug.LogError("DropAssetTypeの値が不正です. (" + value + ")");
				return DropAssetType.Monster;
			}
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

		public static SkillType IntToSkillType(int value)
		{
			switch (value)
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
			default:
				global::Debug.LogError("AffectEffectの値が不正です. (" + value + ")");
				return AffectEffect.Damage;
			}
		}

		public static LeaderSkillType IntToLeaderSkillType(int value)
		{
			switch (value)
			{
			case 1:
				return LeaderSkillType.HpFollowingDamageUp;
			case 2:
				return LeaderSkillType.HpFollowingAttackUp;
			case 3:
				return LeaderSkillType.HpFollowingDefenceUp;
			case 4:
				return LeaderSkillType.HpFollowingSpecialAttackUp;
			case 5:
				return LeaderSkillType.HpFollowingSpecialDefenceUp;
			case 6:
				return LeaderSkillType.HpFollowingSpeedUp;
			case 7:
				return LeaderSkillType.HpFollowingHitRateUp;
			case 8:
				return LeaderSkillType.HpFollowingSatisfactionRateUp;
			case 9:
				return LeaderSkillType.HpMaxDamageUp;
			case 10:
				return LeaderSkillType.HpMaxAttackUp;
			case 11:
				return LeaderSkillType.HpMaxDefenceUp;
			case 12:
				return LeaderSkillType.HpMaxSpecialAttackUp;
			case 13:
				return LeaderSkillType.HpMaxSpecialDefenceUp;
			case 14:
				return LeaderSkillType.HpMaxSpeedUp;
			case 15:
				return LeaderSkillType.HpMaxHitRateUp;
			case 16:
				return LeaderSkillType.HpMaxMachSatisfactionRateUp;
			case 17:
				return LeaderSkillType.SpeciesMachDamageUp;
			case 18:
				return LeaderSkillType.SpeciesMachHpUp;
			case 19:
				return LeaderSkillType.SpeciesMachAttackUp;
			case 20:
				return LeaderSkillType.SpeciesMachDefenceUp;
			case 21:
				return LeaderSkillType.SpeciesMachSpecialAttackUp;
			case 22:
				return LeaderSkillType.SpeciesMachSpecialDefenceUp;
			case 23:
				return LeaderSkillType.SpeciesMachSpeedUp;
			case 24:
				return LeaderSkillType.SpeciesMachHitRateUp;
			case 25:
				return LeaderSkillType.SpeciesMachSatisfactionRateUp;
			case 26:
				return LeaderSkillType.DamageUp;
			case 27:
				return LeaderSkillType.HpUp;
			case 28:
				return LeaderSkillType.AttackUp;
			case 29:
				return LeaderSkillType.DefenceUp;
			case 30:
				return LeaderSkillType.SpecialAttackUp;
			case 31:
				return LeaderSkillType.SpecialDefenceUp;
			case 32:
				return LeaderSkillType.SpeedUp;
			case 33:
				return LeaderSkillType.HitRateUp;
			case 34:
				return LeaderSkillType.SatisfactionRateUp;
			case 35:
				return LeaderSkillType.ToleranceUp;
			default:
				global::Debug.LogError("LeaderSkillTypeの値が不正です. (" + value + ")");
				return LeaderSkillType.HpFollowingDamageUp;
			}
		}

		public static TalentLevel IntToTalentLevel(int value)
		{
			switch (value)
			{
			case 0:
				return TalentLevel.None;
			case 1:
				return TalentLevel.High;
			case 2:
				return TalentLevel.Normal;
			default:
				global::Debug.LogError("TalentLevelの値が不正です. (" + value + ")");
				return TalentLevel.None;
			}
		}

		public static Species IntToSpecies(int value)
		{
			switch (value)
			{
			case 1:
				return Species.PhantomStudents;
			case 2:
				return Species.HeatHaze;
			case 3:
				return Species.Glacier;
			case 4:
				return Species.Electromagnetic;
			case 5:
				return Species.Earth;
			case 6:
				return Species.ShaftOfLight;
			case 7:
				return Species.Abyss;
			default:
				global::Debug.LogError("Speciesの値が不正です. (" + value + ")");
				return Species.Null;
			}
		}

		public static EvolutionStep IntToEvolutionStep(int value)
		{
			switch (value)
			{
			case 2:
				return EvolutionStep.InfancyPhase1;
			case 3:
				return EvolutionStep.InfancyPhase2;
			case 4:
				return EvolutionStep.GrowthPhase;
			case 5:
				return EvolutionStep.MaturationPhase;
			case 6:
				return EvolutionStep.PerfectPhase;
			case 7:
				return EvolutionStep.UltimatePhase;
			case 8:
				return EvolutionStep.AmorPhase1;
			case 9:
				return EvolutionStep.AmorPhase2;
			default:
				global::Debug.LogError("EvolutionStepの値が不正です. (" + value + ")");
				return EvolutionStep.InfancyPhase1;
			}
		}

		public static ToleranceShifter IntToToleranceShifter(int noneValue, int redValue, int blueValue, int yellowValue, int greenValue, int whiteValue, int blackValue, int poisonValue, int confusionValue, int paralysisValue, int sleepValue, int stunValue, int skillLockValue, int instantDeathValue)
		{
			return new ToleranceShifter(noneValue, redValue, blueValue, yellowValue, greenValue, whiteValue, blackValue, poisonValue, confusionValue, paralysisValue, sleepValue, stunValue, skillLockValue, instantDeathValue);
		}

		public static Strength IntToStrength(int value)
		{
			switch (value + 1)
			{
			case 0:
				return Strength.Weak;
			case 1:
				return Strength.None;
			case 2:
				return Strength.Strong;
			default:
				if (value != 99)
				{
					global::Debug.LogError("Strengthの値が不正です. (" + value + ")");
					return Strength.None;
				}
				return Strength.Invalid;
			}
		}

		public static DropAssetType IntToDropAssetType(string value)
		{
			return ServerToBattleUtility.IntToDropAssetType(int.Parse(value));
		}

		public static SkillType IntToSkillType(string value)
		{
			return ServerToBattleUtility.IntToSkillType(int.Parse(value));
		}

		public static TalentLevel IntToTalentLevel(string value)
		{
			return ServerToBattleUtility.IntToTalentLevel(int.Parse(value));
		}

		public static Species IntToSpecies(string value)
		{
			return ServerToBattleUtility.IntToSpecies(int.Parse(value));
		}

		public static Strength IntToStrength(string value)
		{
			return ServerToBattleUtility.IntToStrength(int.Parse(value));
		}

		public static EvolutionStep IntToEvolutionStep(string value)
		{
			return ServerToBattleUtility.IntToEvolutionStep(int.Parse(value));
		}

		public static bool GetIsLeaderSkill(int value)
		{
			switch (value)
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

		public static bool GetUseDrainAffectEffect(int value)
		{
			return value == 36 || value == 37 || value == 49 || value == 50;
		}

		public static PowerType GetPowerType(int value)
		{
			switch (value)
			{
			case 35:
				return PowerType.Percentage;
			default:
				if (value == 14)
				{
					return PowerType.Fixable;
				}
				if (value != 23)
				{
					return PowerType.Percentage;
				}
				return PowerType.Fixable;
			case 38:
				return PowerType.Percentage;
			case 39:
				return PowerType.Fixable;
			case 40:
				return PowerType.Percentage;
			case 41:
				return PowerType.Fixable;
			case 42:
				return PowerType.Fixable;
			case 43:
				return PowerType.Percentage;
			case 44:
				return PowerType.Percentage;
			case 47:
				return PowerType.Fixable;
			case 48:
				return PowerType.Fixable;
			case 49:
				return PowerType.Fixable;
			case 50:
				return PowerType.Fixable;
			case 57:
				return PowerType.Percentage;
			case 58:
				return PowerType.Fixable;
			}
		}

		public static TechniqueType GetTechniqueType(int value)
		{
			switch (value)
			{
			case 34:
				return TechniqueType.Special;
			default:
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
					if (value != 1)
					{
						return TechniqueType.Physics;
					}
					return TechniqueType.Physics;
				}
				break;
			case 36:
				return TechniqueType.Physics;
			case 37:
				return TechniqueType.Special;
			}
		}

		public static bool GetOnHpFollowingLeaderSkill(LeaderSkillType value)
		{
			return value == LeaderSkillType.HpFollowingDamageUp || value == LeaderSkillType.HpFollowingAttackUp || value == LeaderSkillType.HpFollowingDefenceUp || value == LeaderSkillType.HpFollowingSpecialAttackUp || value == LeaderSkillType.HpFollowingSpecialDefenceUp || value == LeaderSkillType.HpFollowingSpeedUp || value == LeaderSkillType.HpFollowingHitRateUp || value == LeaderSkillType.HpFollowingSatisfactionRateUp;
		}

		public static int GetArousal(int value)
		{
			if (value > 0 || value <= 6)
			{
				return value;
			}
			global::Debug.LogError("GetArousalは想定外の値です. (" + value + ")");
			return 1;
		}

		public static bool GetIsLeaderSkill(string value)
		{
			return ServerToBattleUtility.GetIsLeaderSkill(int.Parse(value));
		}
	}
}
