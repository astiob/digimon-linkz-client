using Enemy.AI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityExtension;

public static class CharacterStateControlSorter
{
	private static int CompareHpBase(CharacterStateControl x, CharacterStateControl y)
	{
		if (x == y)
		{
			return 0;
		}
		if (x.hp > y.hp)
		{
			return -1;
		}
		if (x.hp < y.hp)
		{
			return 1;
		}
		return 0;
	}

	private static int CompareHpRangeBase(CharacterStateControl x, CharacterStateControl y, float minRange, float maxRange)
	{
		if (x == y)
		{
			return 0;
		}
		if (!x.GetHpRemainingAmoutRange(minRange, maxRange))
		{
			return 1;
		}
		if (!y.GetHpRemainingAmoutRange(minRange, maxRange))
		{
			return -1;
		}
		return CharacterStateControlSorter.CompareHpBase(x, y);
	}

	private static int CompareAttackBase(CharacterStateControl x, CharacterStateControl y)
	{
		if (x == y)
		{
			return 0;
		}
		float num = (float)x.extraAttackPower;
		float num2 = (float)y.extraAttackPower;
		num += (float)x.extraAttackPower * x.leaderSkillResult.attackUpPercent;
		num2 += (float)y.extraAttackPower * y.leaderSkillResult.attackUpPercent;
		if (num > num2)
		{
			return -1;
		}
		if (num < num2)
		{
			return 1;
		}
		return 0;
	}

	private static int CompareDefenceBase(CharacterStateControl x, CharacterStateControl y)
	{
		if (x == y)
		{
			return 0;
		}
		float num = (float)x.extraDefencePower;
		float num2 = (float)y.extraDefencePower;
		num += (float)x.extraDefencePower * x.leaderSkillResult.defenceUpPercent;
		num2 += (float)y.extraDefencePower * y.leaderSkillResult.defenceUpPercent;
		if (num > num2)
		{
			return -1;
		}
		if (num < num2)
		{
			return 1;
		}
		return 0;
	}

	private static int CompareSpecialAttackBase(CharacterStateControl x, CharacterStateControl y)
	{
		if (x == y)
		{
			return 0;
		}
		float num = (float)x.extraSpecialAttackPower;
		float num2 = (float)y.extraSpecialAttackPower;
		num += (float)x.extraSpecialAttackPower * x.leaderSkillResult.specialAttackUpPercent;
		num2 += (float)y.extraSpecialAttackPower * y.leaderSkillResult.specialAttackUpPercent;
		if (num > num2)
		{
			return -1;
		}
		if (num < num2)
		{
			return 1;
		}
		return 0;
	}

	private static int CompareSpecialDefenceBase(CharacterStateControl x, CharacterStateControl y)
	{
		if (x == y)
		{
			return 0;
		}
		float num = (float)x.extraSpecialDefencePower;
		float num2 = (float)y.extraSpecialDefencePower;
		num += (float)x.extraSpecialDefencePower * x.leaderSkillResult.specialDefenceUpPercent;
		num2 += (float)y.extraSpecialDefencePower * y.leaderSkillResult.specialDefenceUpPercent;
		if (num > num2)
		{
			return -1;
		}
		if (num < num2)
		{
			return 1;
		}
		return 0;
	}

	private static int CompareSpeedBase(CharacterStateControl x, CharacterStateControl y, bool checkRandom = false, bool checkBath = true)
	{
		if (x == y)
		{
			return 0;
		}
		float num = (float)x.extraSpeed;
		float num2 = (float)y.extraSpeed;
		if (checkRandom)
		{
			num = x.randomedSpeed;
			num2 = y.randomedSpeed;
		}
		else
		{
			num += (float)x.extraSpeed * x.leaderSkillResult.speedUpPercent;
			num2 += (float)y.extraSpeed * y.leaderSkillResult.speedUpPercent;
		}
		if (checkBath)
		{
			SufferStateProperty sufferStateProperty = x.currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.SpeedUp);
			if (sufferStateProperty.isActive)
			{
				num += (float)x.extraSpeed * sufferStateProperty.upPercent;
			}
			SufferStateProperty sufferStateProperty2 = x.currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.SpeedDown);
			if (sufferStateProperty2.isActive)
			{
				num -= (float)x.extraSpeed * sufferStateProperty2.downPercent;
			}
			SufferStateProperty sufferStateProperty3 = y.currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.SpeedUp);
			if (sufferStateProperty3.isActive)
			{
				num2 += (float)y.extraSpeed * sufferStateProperty3.upPercent;
			}
			SufferStateProperty sufferStateProperty4 = y.currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.SpeedDown);
			if (sufferStateProperty4.isActive)
			{
				num2 -= (float)y.extraSpeed * sufferStateProperty4.downPercent;
			}
		}
		if (num > num2)
		{
			return -1;
		}
		if (num < num2)
		{
			return 1;
		}
		return 0;
	}

	private static int CompareHateBase(CharacterStateControl x, CharacterStateControl y)
	{
		if (x == y)
		{
			return 0;
		}
		int hate = x.hate;
		int hate2 = y.hate;
		if (hate > hate2)
		{
			return -1;
		}
		if (hate < hate2)
		{
			return 1;
		}
		return 0;
	}

	private static int CompareLuckBase(CharacterStateControl x, CharacterStateControl y)
	{
		if (x == y)
		{
			return 0;
		}
		if (x.luck > y.luck)
		{
			return -1;
		}
		if (x.luck < y.luck)
		{
			return 1;
		}
		return 0;
	}

	private static int CompareToleranceBase(Strength x, Strength y)
	{
		if (x == y)
		{
			return 0;
		}
		if (x != y)
		{
			switch (x)
			{
			case Strength.None:
				if (y == Strength.Strong || y == Strength.Invalid)
				{
					return 1;
				}
				break;
			case Strength.Strong:
				if (y == Strength.Invalid)
				{
					return 1;
				}
				break;
			case Strength.Weak:
				return 1;
			}
			return -1;
		}
		return 0;
	}

	private static int CompareToleranceAttributeBase(CharacterStateControl x, CharacterStateControl y, global::Attribute attribute)
	{
		return CharacterStateControlSorter.CompareToleranceBase(x.tolerance.GetAttributeStrength(attribute), y.tolerance.GetAttributeStrength(attribute));
	}

	private static int CompareToleranceAffectEffectBase(CharacterStateControl x, CharacterStateControl y, AffectEffect affectEffect)
	{
		return CharacterStateControlSorter.CompareToleranceBase(x.tolerance.GetAffectEffectStrength(affectEffect), y.tolerance.GetAffectEffectStrength(affectEffect));
	}

	private static int CompareAttributeBase(CharacterStateControl x, CharacterStateControl y, global::Attribute attribute)
	{
		if (x == y)
		{
			return 0;
		}
		int num = 0;
		int num2 = 0;
		foreach (SkillStatus skillStatus2 in x.skillStatus)
		{
			num += skillStatus2.AttributeMachLevel(attribute);
		}
		foreach (SkillStatus skillStatus4 in y.skillStatus)
		{
			num2 += skillStatus4.AttributeMachLevel(attribute);
		}
		if (num > num2)
		{
			return -1;
		}
		if (num < num2)
		{
			return 1;
		}
		return 0;
	}

	private static int CompareApRangeBase(CharacterStateControl x, CharacterStateControl y, float minValue, float maxValue)
	{
		if (x == y)
		{
			return 0;
		}
		int num = 0;
		int num2 = 0;
		if (minValue <= (float)x.ap && (float)x.ap <= maxValue)
		{
			num = x.ap;
		}
		if (minValue <= (float)y.ap && (float)y.ap <= maxValue)
		{
			num2 = y.ap;
		}
		return Mathf.Clamp(num2 - num, -1, 1);
	}

	private static bool GetApRemingAmoutRange(CharacterStateControl state, float minValue, float maxValue)
	{
		return minValue <= (float)state.ap && (float)state.ap <= maxValue;
	}

	private static int CompareAffectEffectRangeBase(CharacterStateControl x, CharacterStateControl y, int minValue, int maxValue)
	{
		if (x == y)
		{
			return 0;
		}
		int num = 0;
		int num2 = 0;
		foreach (SkillStatus skillStatus2 in x.skillStatus)
		{
			List<int> serverAffectEffectList = CharacterStateControlSorter.GetServerAffectEffectList(skillStatus2);
			foreach (int num3 in serverAffectEffectList)
			{
				if (minValue <= num3 && num3 <= maxValue)
				{
					num++;
					break;
				}
			}
		}
		foreach (SkillStatus skillStatus4 in y.skillStatus)
		{
			List<int> serverAffectEffectList2 = CharacterStateControlSorter.GetServerAffectEffectList(skillStatus4);
			foreach (int num4 in serverAffectEffectList2)
			{
				if (minValue <= num4 && num4 <= maxValue)
				{
					num2++;
					break;
				}
			}
		}
		return Mathf.Clamp(num2 - num, -1, 1);
	}

	private static List<int> GetServerAffectEffectList(SkillStatus skillStatus)
	{
		List<int> list = new List<int>();
		foreach (AffectEffectProperty affectEffectProperty in skillStatus.affectEffect)
		{
			if (affectEffectProperty.type == AffectEffect.HpRevival)
			{
				if (affectEffectProperty.powerType == PowerType.Fixable)
				{
					list.Add(14);
				}
				else
				{
					list.Add(35);
				}
			}
			else if (affectEffectProperty.type == AffectEffect.Damage)
			{
				if (affectEffectProperty.techniqueType == TechniqueType.Physics)
				{
					if (affectEffectProperty.useDrain)
					{
						if (affectEffectProperty.powerType == PowerType.Fixable)
						{
							list.Add(49);
						}
						else
						{
							list.Add(36);
						}
					}
					else if (affectEffectProperty.powerType == PowerType.Fixable)
					{
						list.Add(47);
					}
					else
					{
						list.Add(1);
					}
				}
				else if (affectEffectProperty.useDrain)
				{
					if (affectEffectProperty.powerType == PowerType.Fixable)
					{
						list.Add(50);
					}
					else
					{
						list.Add(37);
					}
				}
				else if (affectEffectProperty.powerType == PowerType.Fixable)
				{
					list.Add(48);
				}
				else
				{
					list.Add(34);
				}
			}
			else if (affectEffectProperty.type == AffectEffect.Poison)
			{
				if (affectEffectProperty.powerType == PowerType.Fixable)
				{
					list.Add(23);
				}
				else
				{
					list.Add(38);
				}
			}
			else if (affectEffectProperty.type == AffectEffect.ApRevival)
			{
				if (affectEffectProperty.powerType == PowerType.Fixable)
				{
					list.Add(39);
				}
				else
				{
					list.Add(40);
				}
			}
			else if (affectEffectProperty.type == AffectEffect.ApUp)
			{
				if (affectEffectProperty.powerType == PowerType.Fixable)
				{
					list.Add(42);
				}
				else
				{
					list.Add(44);
				}
			}
			else if (affectEffectProperty.type == AffectEffect.ApDown)
			{
				if (affectEffectProperty.powerType == PowerType.Fixable)
				{
					list.Add(41);
				}
				else
				{
					list.Add(43);
				}
			}
			else
			{
				list.Add((int)(affectEffectProperty.type + 1));
			}
		}
		return list;
	}

	private static int CompareSufferTypeRangeBase(CharacterStateControl x, CharacterStateControl y, int minValue, int maxValue)
	{
		if (x == y)
		{
			return 0;
		}
		int num = 0;
		int num2 = 0;
		foreach (SufferStateProperty.SufferType sufferType in x.currentSufferState.GetSufferOrderList())
		{
			int num3 = (int)sufferType;
			if (minValue <= num3 && num3 <= maxValue)
			{
				num++;
			}
		}
		foreach (SufferStateProperty.SufferType sufferType2 in y.currentSufferState.GetSufferOrderList())
		{
			int num4 = (int)sufferType2;
			if (minValue <= num4 && num4 <= maxValue)
			{
				num2++;
			}
		}
		return Mathf.Clamp(num2 - num, -1, 1);
	}

	private static int CompareHate(CharacterStateControl x, CharacterStateControl y)
	{
		if (x == y)
		{
			return 0;
		}
		int num = CharacterStateControlSorter.CompareHateBase(x, y);
		if (Mathf.Abs(num) == 1)
		{
			return num;
		}
		return RandomExtension.IntPlusMinus();
	}

	private static bool CheckTargetSelect(CharacterStateControl x, AIActionClip aiActionClip)
	{
		if (aiActionClip == null)
		{
			return false;
		}
		TargetSelectReference targetSelectRerefence = aiActionClip.targetSelectRerefence;
		if (targetSelectRerefence != TargetSelectReference.Hp)
		{
			return targetSelectRerefence != TargetSelectReference.Ap || CharacterStateControlSorter.GetApRemingAmoutRange(x, aiActionClip.minValue, aiActionClip.maxValue);
		}
		return x.GetHpRemainingAmoutRange(aiActionClip.minValue, aiActionClip.maxValue);
	}

	private static int CompareTargetSelect(CharacterStateControl x, CharacterStateControl y, AIActionClip aiActionClip)
	{
		if (x == y)
		{
			return 0;
		}
		int num = 0;
		if (aiActionClip != null)
		{
			switch (aiActionClip.targetSelectRerefence)
			{
			case TargetSelectReference.Hp:
				num = CharacterStateControlSorter.CompareHpRangeBase(x, y, aiActionClip.minValue, aiActionClip.maxValue);
				break;
			case TargetSelectReference.Hate:
				num = CharacterStateControlSorter.CompareHateBase(x, y);
				break;
			case TargetSelectReference.Attack:
				num = CharacterStateControlSorter.CompareAttackBase(x, y);
				break;
			case TargetSelectReference.Defence:
				num = CharacterStateControlSorter.CompareDefenceBase(x, y);
				break;
			case TargetSelectReference.SpecialAttack:
				num = CharacterStateControlSorter.CompareSpecialAttackBase(x, y);
				break;
			case TargetSelectReference.SpecialDefence:
				num = CharacterStateControlSorter.CompareSpecialDefenceBase(x, y);
				break;
			case TargetSelectReference.Speed:
				num = CharacterStateControlSorter.CompareSpeedBase(x, y, false, false);
				break;
			case TargetSelectReference.Luck:
				num = CharacterStateControlSorter.CompareLuckBase(x, y);
				break;
			case TargetSelectReference.ToleranceNone:
				num = CharacterStateControlSorter.CompareToleranceAttributeBase(x, y, global::Attribute.None);
				break;
			case TargetSelectReference.ToleranceRed:
				num = CharacterStateControlSorter.CompareToleranceAttributeBase(x, y, global::Attribute.Red);
				break;
			case TargetSelectReference.ToleranceBlue:
				num = CharacterStateControlSorter.CompareToleranceAttributeBase(x, y, global::Attribute.Blue);
				break;
			case TargetSelectReference.ToleranceYellow:
				num = CharacterStateControlSorter.CompareToleranceAttributeBase(x, y, global::Attribute.Yellow);
				break;
			case TargetSelectReference.ToleranceGreen:
				num = CharacterStateControlSorter.CompareToleranceAttributeBase(x, y, global::Attribute.Green);
				break;
			case TargetSelectReference.ToleranceWhite:
				num = CharacterStateControlSorter.CompareToleranceAttributeBase(x, y, global::Attribute.White);
				break;
			case TargetSelectReference.ToleranceBlack:
				num = CharacterStateControlSorter.CompareToleranceAttributeBase(x, y, global::Attribute.Black);
				break;
			case TargetSelectReference.TolerancePoison:
				num = CharacterStateControlSorter.CompareToleranceAffectEffectBase(x, y, AffectEffect.Poison);
				break;
			case TargetSelectReference.ToleranceConfusion:
				num = CharacterStateControlSorter.CompareToleranceAffectEffectBase(x, y, AffectEffect.Confusion);
				break;
			case TargetSelectReference.ToleranceParalysis:
				num = CharacterStateControlSorter.CompareToleranceAffectEffectBase(x, y, AffectEffect.Paralysis);
				break;
			case TargetSelectReference.ToleranceSleep:
				num = CharacterStateControlSorter.CompareToleranceAffectEffectBase(x, y, AffectEffect.Sleep);
				break;
			case TargetSelectReference.ToleranceStun:
				num = CharacterStateControlSorter.CompareToleranceAffectEffectBase(x, y, AffectEffect.Stun);
				break;
			case TargetSelectReference.ToleranceSkillLock:
				num = CharacterStateControlSorter.CompareToleranceAffectEffectBase(x, y, AffectEffect.SkillLock);
				break;
			case TargetSelectReference.ToleranceInstantDeath:
				num = CharacterStateControlSorter.CompareToleranceAffectEffectBase(x, y, AffectEffect.InstantDeath);
				break;
			case TargetSelectReference.AttributeNone:
				num = CharacterStateControlSorter.CompareAttributeBase(x, y, global::Attribute.None);
				break;
			case TargetSelectReference.AttributeRed:
				num = CharacterStateControlSorter.CompareAttributeBase(x, y, global::Attribute.Red);
				break;
			case TargetSelectReference.AttributeBlue:
				num = CharacterStateControlSorter.CompareAttributeBase(x, y, global::Attribute.Blue);
				break;
			case TargetSelectReference.AttributeYellow:
				num = CharacterStateControlSorter.CompareAttributeBase(x, y, global::Attribute.Yellow);
				break;
			case TargetSelectReference.AttributeGreen:
				num = CharacterStateControlSorter.CompareAttributeBase(x, y, global::Attribute.Green);
				break;
			case TargetSelectReference.AttributeWhite:
				num = CharacterStateControlSorter.CompareAttributeBase(x, y, global::Attribute.White);
				break;
			case TargetSelectReference.AttributeBlack:
				num = CharacterStateControlSorter.CompareAttributeBase(x, y, global::Attribute.Black);
				break;
			case TargetSelectReference.Ap:
				num = CharacterStateControlSorter.CompareApRangeBase(x, y, aiActionClip.minValue, aiActionClip.maxValue);
				break;
			case TargetSelectReference.AffectEffect:
				num = CharacterStateControlSorter.CompareAffectEffectRangeBase(x, y, (int)aiActionClip.minValue, (int)aiActionClip.maxValue);
				break;
			case TargetSelectReference.SufferType:
				num = CharacterStateControlSorter.CompareSufferTypeRangeBase(x, y, (int)aiActionClip.minValue, (int)aiActionClip.maxValue);
				break;
			}
			if (aiActionClip.selectingOrder == SelectingOrder.LowAndHavent)
			{
				num *= -1;
			}
		}
		return num;
	}

	private static int CompareBaseTargetSelect(CharacterStateControl x, CharacterStateControl y, SkillStatus skillStatus)
	{
		if (x == y)
		{
			return 0;
		}
		int num = CharacterStateControlSorter.CompareToleranceBase(skillStatus.GetSkillStrength(x.tolerance), skillStatus.GetSkillStrength(y.tolerance));
		if (Mathf.Abs(num) == 1)
		{
			return num;
		}
		num = -CharacterStateControlSorter.CompareHpBase(x, y);
		if (Mathf.Abs(num) == 1)
		{
			return num;
		}
		return CharacterStateControlSorter.CompareHate(x, y);
	}

	public static CharacterStateControl[] SortedTargetSelect(CharacterStateControl[] characterStatus, SkillStatus skillState, AIActionClip actionClip = null)
	{
		List<CharacterStateControl> list = new List<CharacterStateControl>();
		foreach (CharacterStateControl characterStateControl in characterStatus)
		{
			if (CharacterStateControlSorter.CheckTargetSelect(characterStateControl, actionClip))
			{
				list.Add(characterStateControl);
			}
		}
		if (list.Count > 0)
		{
			Comparison<CharacterStateControl> comparison = (CharacterStateControl x, CharacterStateControl y) => CharacterStateControlSorter.CompareTargetSelect(x, y, actionClip);
			list.Sort(comparison);
		}
		else
		{
			list = new List<CharacterStateControl>(characterStatus);
			Comparison<CharacterStateControl> comparison2 = (CharacterStateControl x, CharacterStateControl y) => CharacterStateControlSorter.CompareBaseTargetSelect(x, y, skillState);
			list.Sort(comparison2);
		}
		return list.ToArray();
	}

	public static CharacterStateControl[] SortedSpeedEnemyPriority(CharacterStateControl[] characterStatus)
	{
		CharacterStateControl[] array = characterStatus.Clone() as CharacterStateControl[];
		Array.Sort<CharacterStateControl>(array, new Comparison<CharacterStateControl>(CharacterStateControlSorter.CompareSpeedEnemyPriority));
		return array;
	}

	private static int CompareSpeedEnemyPriority(CharacterStateControl x, CharacterStateControl y)
	{
		if (x == y)
		{
			return 0;
		}
		int num = CharacterStateControlSorter.CompareSpeedBase(x, y, true, true);
		if (Mathf.Abs(num) == 1)
		{
			return num;
		}
		if (x.isEnemy == y.isEnemy)
		{
			return RandomExtension.IntPlusMinus();
		}
		if (x.isEnemy)
		{
			return -1;
		}
		return 1;
	}

	public static CharacterStateControl[] SortedSpeedLuck(CharacterStateControl[] characterStatus)
	{
		CharacterStateControl[] array = characterStatus.Clone() as CharacterStateControl[];
		Array.Sort<CharacterStateControl>(array, new Comparison<CharacterStateControl>(CharacterStateControlSorter.CompareSpeedLuck));
		return array;
	}

	private static int CompareSpeedLuck(CharacterStateControl x, CharacterStateControl y)
	{
		if (x == y)
		{
			return 0;
		}
		int num = CharacterStateControlSorter.CompareSpeedBase(x, y, true, true);
		if (Mathf.Abs(num) == 1)
		{
			return num;
		}
		num = CharacterStateControlSorter.CompareLuckBase(x, y);
		if (Mathf.Abs(num) == 1)
		{
			return num;
		}
		return RandomExtension.IntPlusMinus();
	}

	public static CharacterStateControl[] SortedSufferStateGenerateStartTiming(CharacterStateControl[] characterStatus, SufferStateProperty.SufferType sufferType)
	{
		CharacterStateControl[] array = characterStatus.Clone() as CharacterStateControl[];
		Comparison<CharacterStateControl> comparison = (CharacterStateControl x, CharacterStateControl y) => CharacterStateControlSorter.CompareGenreationStartTimingBase(x, y, sufferType);
		Array.Sort<CharacterStateControl>(array, comparison);
		return array;
	}

	private static int CompareGenreationStartTimingBase(CharacterStateControl x, CharacterStateControl y, SufferStateProperty.SufferType sufferType)
	{
		if (x == y)
		{
			return 0;
		}
		int num = 0;
		SufferStateProperty sufferStateProperty = x.currentSufferState.GetSufferStateProperty(sufferType);
		SufferStateProperty sufferStateProperty2 = y.currentSufferState.GetSufferStateProperty(sufferType);
		if (sufferStateProperty.generationStartTiming < sufferStateProperty2.generationStartTiming)
		{
			num = -1;
		}
		else if (sufferStateProperty.generationStartTiming > sufferStateProperty2.generationStartTiming)
		{
			num = 1;
		}
		if (Mathf.Abs(num) == 1)
		{
			return num;
		}
		return RandomExtension.IntPlusMinus();
	}
}
