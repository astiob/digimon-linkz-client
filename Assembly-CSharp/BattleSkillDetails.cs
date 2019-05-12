using System;
using System.Collections.Generic;
using UnityEngine;
using UnityExtension;

public class BattleSkillDetails : BattleFunctionBase
{
	private void CorrectionUpReset(CharacterStateControl target)
	{
		SufferStateProperty.SufferType[] array = new SufferStateProperty.SufferType[]
		{
			SufferStateProperty.SufferType.AttackUp,
			SufferStateProperty.SufferType.DefenceUp,
			SufferStateProperty.SufferType.SpAttackUp,
			SufferStateProperty.SufferType.SpDefenceUp,
			SufferStateProperty.SufferType.SpeedUp,
			SufferStateProperty.SufferType.HitRateUp
		};
		foreach (SufferStateProperty.SufferType type in array)
		{
			if (target.currentSufferState.FindSufferState(type))
			{
				target.currentSufferState.RemoveSufferState(type);
			}
		}
	}

	private void CorrectionDownReset(CharacterStateControl target)
	{
		SufferStateProperty.SufferType[] array = new SufferStateProperty.SufferType[]
		{
			SufferStateProperty.SufferType.AttackDown,
			SufferStateProperty.SufferType.DefenceDown,
			SufferStateProperty.SufferType.SpAttackDown,
			SufferStateProperty.SufferType.SpDefenceDown,
			SufferStateProperty.SufferType.SpeedDown,
			SufferStateProperty.SufferType.HitRateDown
		};
		foreach (SufferStateProperty.SufferType type in array)
		{
			if (target.currentSufferState.FindSufferState(type))
			{
				target.currentSufferState.RemoveSufferState(type);
			}
		}
	}

	private void SufferStatusClear(CharacterStateControl target, AffectEffectProperty affectEffectProperty)
	{
		Dictionary<SufferStateProperty.SufferType, float> dictionary = new Dictionary<SufferStateProperty.SufferType, float>
		{
			{
				SufferStateProperty.SufferType.Poison,
				affectEffectProperty.clearPoisonIncidenceRate
			},
			{
				SufferStateProperty.SufferType.Confusion,
				affectEffectProperty.clearConfusionIncidenceRate
			},
			{
				SufferStateProperty.SufferType.Paralysis,
				affectEffectProperty.clearParalysisIncidenceRate
			},
			{
				SufferStateProperty.SufferType.Sleep,
				affectEffectProperty.clearSleepIncidenceRate
			},
			{
				SufferStateProperty.SufferType.Stun,
				affectEffectProperty.clearStunIncidenceRate
			},
			{
				SufferStateProperty.SufferType.SkillLock,
				affectEffectProperty.clearSkillLockIncidenceRate
			}
		};
		foreach (KeyValuePair<SufferStateProperty.SufferType, float> keyValuePair in dictionary)
		{
			if (RandomExtension.Switch(keyValuePair.Value) && target.currentSufferState.FindSufferState(keyValuePair.Key))
			{
				target.currentSufferState.RemoveSufferState(keyValuePair.Key);
			}
		}
	}

	private void ApUp(CharacterStateControl target, AffectEffectProperty affectEffectProperty)
	{
		int num;
		if (affectEffectProperty.powerType == PowerType.Fixable)
		{
			num = affectEffectProperty.upPower;
		}
		else
		{
			num = Mathf.FloorToInt((float)target.maxAp * affectEffectProperty.upPercent);
		}
		target.ap += num;
	}

	private void ApDown(CharacterStateControl target, AffectEffectProperty affectEffectProperty)
	{
		int num;
		if (affectEffectProperty.powerType == PowerType.Fixable)
		{
			num = affectEffectProperty.downPower;
		}
		else
		{
			num = Mathf.FloorToInt((float)target.maxAp * affectEffectProperty.downPercent);
		}
		target.ap -= num;
	}

	private int HpRevival(CharacterStateControl target, AffectEffectProperty affectEffectProperty)
	{
		int num;
		if (affectEffectProperty.powerType == PowerType.Fixable)
		{
			num = affectEffectProperty.revivalPower;
		}
		else
		{
			num = Mathf.FloorToInt((float)target.extraMaxHp * affectEffectProperty.revivalPercent);
		}
		target.hp += num;
		return num;
	}

	private void HateUp(CharacterStateControl target, AffectEffectProperty affectEffectProperty)
	{
		target.hate += affectEffectProperty.upPower;
	}

	private void HateDown(CharacterStateControl target, AffectEffectProperty affectEffectProperty)
	{
		target.hate += affectEffectProperty.downPower;
	}

	private void AddSufferStateOthers(CharacterStateControl target, AffectEffectProperty affectEffectProperty)
	{
		SufferStateProperty.Data data = new SufferStateProperty.Data(affectEffectProperty, base.battleStateData.currentLastGenerateStartTimingSufferState);
		target.currentSufferState.SetSufferState(data, null);
		base.battleStateData.currentLastGenerateStartTimingSufferState++;
	}

	public SkillResults GetOtherSkillResult(AffectEffectProperty affectEffectProperty, CharacterStateControl attackerCharacter, CharacterStateControl targetCharacter)
	{
		SkillResults skillResults = new SkillResults();
		skillResults.useAffectEffectProperty = affectEffectProperty;
		skillResults.hitIconAffectEffect = affectEffectProperty.type;
		skillResults.attackCharacter = attackerCharacter;
		skillResults.targetCharacter = targetCharacter;
		if (!affectEffectProperty.OnHit(attackerCharacter, targetCharacter))
		{
			skillResults.onMissHit = true;
			return skillResults;
		}
		AffectEffect type = affectEffectProperty.type;
		switch (type)
		{
		case AffectEffect.CorrectionUpReset:
			this.CorrectionUpReset(targetCharacter);
			break;
		case AffectEffect.CorrectionDownReset:
			this.CorrectionDownReset(targetCharacter);
			break;
		case AffectEffect.HpRevival:
		{
			int num = this.HpRevival(targetCharacter, affectEffectProperty);
			skillResults.attackPower = num;
			skillResults.originalAttackPower = num;
			break;
		}
		default:
			switch (type)
			{
			case AffectEffect.SufferStatusClear:
				this.SufferStatusClear(targetCharacter, affectEffectProperty);
				break;
			default:
				if (type != AffectEffect.HpSettingFixable)
				{
					if (type != AffectEffect.HpSettingPercentage)
					{
						if (type != AffectEffect.Recommand)
						{
							this.AddSufferStateOthers(targetCharacter, affectEffectProperty);
						}
						else
						{
							targetCharacter.isRecommand = true;
						}
					}
					else
					{
						int num2 = this.HpSettingPercentage(targetCharacter, affectEffectProperty);
						if (num2 > 0)
						{
							skillResults.hitIconAffectEffect = AffectEffect.HpRevival;
						}
						else
						{
							skillResults.hitIconAffectEffect = AffectEffect.Damage;
						}
						num2 = Mathf.Abs(num2);
						skillResults.attackPower = num2;
						skillResults.originalAttackPower = num2;
					}
				}
				else
				{
					int num3 = this.HpSettingFixable(targetCharacter, affectEffectProperty);
					if (num3 > 0)
					{
						skillResults.hitIconAffectEffect = AffectEffect.HpRevival;
					}
					else
					{
						skillResults.hitIconAffectEffect = AffectEffect.Damage;
					}
					num3 = Mathf.Abs(num3);
					skillResults.attackPower = num3;
					skillResults.originalAttackPower = num3;
				}
				break;
			case AffectEffect.ApUp:
				this.ApUp(targetCharacter, affectEffectProperty);
				break;
			case AffectEffect.ApDown:
				this.ApDown(targetCharacter, affectEffectProperty);
				break;
			}
			break;
		case AffectEffect.HateUp:
			this.HateUp(targetCharacter, affectEffectProperty);
			break;
		case AffectEffect.HateDown:
			this.HateDown(targetCharacter, affectEffectProperty);
			break;
		}
		skillResults.onMissHit = false;
		return skillResults;
	}

	private int HpSettingFixable(CharacterStateControl target, AffectEffectProperty affectEffectProperty)
	{
		int num = affectEffectProperty.revivalPower;
		num = Mathf.Min(num, target.extraMaxHp);
		int result = num - target.hp;
		target.hp = num;
		return result;
	}

	private int HpSettingPercentage(CharacterStateControl target, AffectEffectProperty affectEffectProperty)
	{
		int num = Mathf.FloorToInt((float)target.extraMaxHp * affectEffectProperty.revivalPercent);
		int result = num - target.hp;
		target.hp = num;
		return result;
	}

	public SkillResults GetToleranceSkillResult(AffectEffectProperty affectEffectProperty, CharacterStateControl attackerCharacter, CharacterStateControl targetCharacter)
	{
		SkillResults skillResults = new SkillResults();
		skillResults.useAffectEffectProperty = affectEffectProperty;
		skillResults.hitIconAffectEffect = affectEffectProperty.type;
		skillResults.attackCharacter = attackerCharacter;
		skillResults.targetCharacter = targetCharacter;
		if (!affectEffectProperty.OnHit(attackerCharacter, targetCharacter))
		{
			skillResults.onMissHit = true;
			return skillResults;
		}
		Strength affectEffectStrength = targetCharacter.tolerance.GetAffectEffectStrength(affectEffectProperty.type);
		if (affectEffectStrength == Strength.Invalid)
		{
			skillResults.hitIconAffectEffect = AffectEffect.Invalid;
			skillResults.onWeakHit = Strength.Invalid;
			skillResults.onMissHit = false;
		}
		else if (targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.TurnBarrier))
		{
			skillResults.hitIconAffectEffect = AffectEffect.TurnBarrier;
			skillResults.onWeakHit = Strength.None;
			skillResults.onMissHit = false;
		}
		else if (targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.CountBarrier))
		{
			skillResults.hitIconAffectEffect = AffectEffect.CountBarrier;
			skillResults.onWeakHit = Strength.None;
			skillResults.onMissHit = false;
		}
		else if (targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.TurnEvasion))
		{
			skillResults.hitIconAffectEffect = AffectEffect.TurnEvasion;
			skillResults.onWeakHit = Strength.None;
			skillResults.onMissHit = false;
		}
		else if (targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.CountEvasion))
		{
			skillResults.hitIconAffectEffect = AffectEffect.CountEvasion;
			skillResults.onWeakHit = Strength.None;
			skillResults.onMissHit = false;
		}
		else
		{
			skillResults.hitIconAffectEffect = affectEffectProperty.type;
			skillResults.onWeakHit = Strength.None;
			skillResults.onMissHit = false;
			if (affectEffectProperty.type == AffectEffect.InstantDeath)
			{
				targetCharacter.Kill();
			}
			else
			{
				this.AddSufferStateOthers(targetCharacter, affectEffectProperty);
			}
		}
		return skillResults;
	}

	public SkillResults GetApDrainSkillResult(AffectEffectProperty affectEffectProperty, CharacterStateControl attackerCharacter, CharacterStateControl targetCharacter)
	{
		SkillResults skillResults = new SkillResults();
		skillResults.useAffectEffectProperty = affectEffectProperty;
		skillResults.hitIconAffectEffect = affectEffectProperty.type;
		skillResults.attackCharacter = attackerCharacter;
		skillResults.targetCharacter = targetCharacter;
		if (!affectEffectProperty.OnHit(attackerCharacter, targetCharacter))
		{
			skillResults.onMissHit = true;
			return skillResults;
		}
		if (targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.TurnBarrier))
		{
			skillResults.hitIconAffectEffect = AffectEffect.TurnBarrier;
			skillResults.onMissHit = false;
		}
		else if (targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.CountBarrier))
		{
			skillResults.hitIconAffectEffect = AffectEffect.CountBarrier;
			skillResults.onMissHit = false;
		}
		else if (targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.TurnEvasion))
		{
			skillResults.hitIconAffectEffect = AffectEffect.TurnEvasion;
			skillResults.onMissHit = false;
		}
		else if (targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.CountEvasion))
		{
			skillResults.hitIconAffectEffect = AffectEffect.CountEvasion;
			skillResults.onMissHit = false;
		}
		else
		{
			skillResults.hitIconAffectEffect = AffectEffect.ApDown;
			skillResults.onMissHit = false;
			int ap = targetCharacter.ap;
			targetCharacter.ap -= affectEffectProperty.apDrainPower;
			skillResults.attackPower = ap - targetCharacter.ap;
		}
		return skillResults;
	}

	public SkillResults GetDestructTargetSkillResult(AffectEffectProperty affectEffectProperty, CharacterStateControl attackerCharacter, CharacterStateControl targetCharacter)
	{
		SkillResults skillResults = new SkillResults();
		skillResults.useAffectEffectProperty = affectEffectProperty;
		skillResults.hitIconAffectEffect = affectEffectProperty.type;
		skillResults.attackCharacter = attackerCharacter;
		skillResults.targetCharacter = targetCharacter;
		if (!affectEffectProperty.OnHit(attackerCharacter, targetCharacter))
		{
			skillResults.onMissHit = true;
			return skillResults;
		}
		targetCharacter.OnHitDestruct();
		skillResults.onMissHit = false;
		return skillResults;
	}
}
