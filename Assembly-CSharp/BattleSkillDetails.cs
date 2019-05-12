using System;
using System.Collections.Generic;
using UnityEngine;
using UnityExtension;

public class BattleSkillDetails : BattleFunctionBase
{
	public const int OneTargetLength = 1;

	public const int IsDiedCounterReflectionDamage = -1;

	public bool IsFirstSkillDetails(int detailsIndex)
	{
		return detailsIndex == 0;
	}

	public CharacterStateControl[] GetProtectCharacters(CharacterStateControl[] isTargetsStatus, AffectEffectProperty currentSuffer)
	{
		if (currentSuffer.type == AffectEffect.Damage || currentSuffer.type == AffectEffect.ReferenceTargetHpRate)
		{
			List<CharacterStateControl> list = new List<CharacterStateControl>();
			for (int i = 0; i < isTargetsStatus.Length; i++)
			{
				if (!isTargetsStatus[i].isDied && isTargetsStatus[i].currentSufferState.FindSufferState(SufferStateProperty.SufferType.Protect))
				{
					list.Add(isTargetsStatus[i]);
				}
			}
			if (list.Count > 0)
			{
				CharacterStateControl[] array = CharacterStateControl.SortedSufferStateGenerateStartTiming(list.ToArray(), SufferStateProperty.SufferType.Protect);
				list.Clear();
				for (int j = 0; j < isTargetsStatus.Length; j++)
				{
					list.Add(array[0]);
				}
				return list.ToArray();
			}
		}
		return null;
	}

	public void NotToCounterReflectionMonsterWhoDiedOnTheWay(List<int[]> TotalCounterReflectionDamage)
	{
		for (int i = TotalCounterReflectionDamage.Count - 1; i > -1; i--)
		{
			for (int j = 0; j < TotalCounterReflectionDamage[i].Length; j++)
			{
				if (TotalCounterReflectionDamage[i][j] == -1)
				{
					TotalCounterReflectionDamage.RemoveAt(i);
					break;
				}
			}
		}
	}

	public void GetSleepWakeUp(CharacterStateControl[] targetsStatus, bool[] onMissHit)
	{
		List<CharacterStateControl> list = new List<CharacterStateControl>(targetsStatus);
		list.RemoveAll((CharacterStateControl c) => targetsStatus[0] == c);
		for (int i = 0; i < list.Count; i++)
		{
			if (!list[i].isDied && list[i].currentSufferState.FindSufferState(SufferStateProperty.SufferType.Sleep) && !onMissHit[i] && list[i].currentSufferState.onSleep.GetSleepGetupOccurrence(true))
			{
				list[i].currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.Sleep);
			}
		}
	}

	public void CorrectionUpReset(CharacterStateControl isTargetsStatus)
	{
		if (isTargetsStatus.currentSufferState.FindSufferState(SufferStateProperty.SufferType.AttackUp))
		{
			isTargetsStatus.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.AttackUp);
		}
		if (isTargetsStatus.currentSufferState.FindSufferState(SufferStateProperty.SufferType.DefenceUp))
		{
			isTargetsStatus.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.DefenceUp);
		}
		if (isTargetsStatus.currentSufferState.FindSufferState(SufferStateProperty.SufferType.SpAttackUp))
		{
			isTargetsStatus.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.SpAttackUp);
		}
		if (isTargetsStatus.currentSufferState.FindSufferState(SufferStateProperty.SufferType.SpDefenceUp))
		{
			isTargetsStatus.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.SpDefenceUp);
		}
		if (isTargetsStatus.currentSufferState.FindSufferState(SufferStateProperty.SufferType.SpeedUp))
		{
			isTargetsStatus.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.SpeedUp);
		}
		if (isTargetsStatus.currentSufferState.FindSufferState(SufferStateProperty.SufferType.HitRateUp))
		{
			isTargetsStatus.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.HitRateUp);
		}
	}

	public void CorrectionDownReset(CharacterStateControl isTargetsStatus)
	{
		if (isTargetsStatus.currentSufferState.FindSufferState(SufferStateProperty.SufferType.AttackDown))
		{
			isTargetsStatus.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.AttackDown);
		}
		if (isTargetsStatus.currentSufferState.FindSufferState(SufferStateProperty.SufferType.DefenceDown))
		{
			isTargetsStatus.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.DefenceDown);
		}
		if (isTargetsStatus.currentSufferState.FindSufferState(SufferStateProperty.SufferType.SpAttackDown))
		{
			isTargetsStatus.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.SpAttackDown);
		}
		if (isTargetsStatus.currentSufferState.FindSufferState(SufferStateProperty.SufferType.SpDefenceDown))
		{
			isTargetsStatus.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.SpDefenceDown);
		}
		if (isTargetsStatus.currentSufferState.FindSufferState(SufferStateProperty.SufferType.SpeedDown))
		{
			isTargetsStatus.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.SpeedDown);
		}
		if (isTargetsStatus.currentSufferState.FindSufferState(SufferStateProperty.SufferType.HitRateDown))
		{
			isTargetsStatus.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.HitRateDown);
		}
	}

	public void SufferStatusClear(CharacterStateControl isTargetsStatus, AffectEffectProperty currentSuffer)
	{
		if (RandomExtension.Switch(currentSuffer.clearPoisonIncidenceRate, null) && isTargetsStatus.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Poison))
		{
			isTargetsStatus.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.Poison);
		}
		if (RandomExtension.Switch(currentSuffer.clearConfusionIncidenceRate, null) && isTargetsStatus.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Confusion))
		{
			isTargetsStatus.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.Confusion);
		}
		if (RandomExtension.Switch(currentSuffer.clearParalysisIncidenceRate, null) && isTargetsStatus.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Paralysis))
		{
			isTargetsStatus.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.Paralysis);
		}
		if (RandomExtension.Switch(currentSuffer.clearSleepIncidenceRate, null) && isTargetsStatus.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Sleep))
		{
			isTargetsStatus.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.Sleep);
		}
		if (RandomExtension.Switch(currentSuffer.clearStunIncidenceRate, null) && isTargetsStatus.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Stun))
		{
			isTargetsStatus.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.Stun);
		}
		if (RandomExtension.Switch(currentSuffer.clearSkillLockIncidenceRate, null) && isTargetsStatus.currentSufferState.FindSufferState(SufferStateProperty.SufferType.SkillLock))
		{
			isTargetsStatus.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.SkillLock);
		}
	}

	public void ApUp(CharacterStateControl isTargetsStatus, AffectEffectProperty currentSuffer)
	{
		int num;
		if (currentSuffer.powerType == PowerType.Fixable)
		{
			num = currentSuffer.upPower;
		}
		else
		{
			num = Mathf.FloorToInt((float)isTargetsStatus.maxAp * currentSuffer.upPercent);
		}
		isTargetsStatus.ap += num;
	}

	public void ApDown(CharacterStateControl isTargetsStatus, AffectEffectProperty currentSuffer)
	{
		int num;
		if (currentSuffer.powerType == PowerType.Fixable)
		{
			num = currentSuffer.downPower;
		}
		else
		{
			num = Mathf.FloorToInt((float)isTargetsStatus.maxAp * currentSuffer.downPercent);
		}
		isTargetsStatus.ap -= num;
	}

	public int HpRevival(CharacterStateControl isTargetsStatus, AffectEffectProperty currentSuffer)
	{
		int num;
		if (currentSuffer.powerType == PowerType.Fixable)
		{
			num = currentSuffer.revivalPower;
		}
		else
		{
			num = Mathf.FloorToInt((float)isTargetsStatus.maxHp * currentSuffer.revivalPercent);
		}
		isTargetsStatus.hp += num;
		return num;
	}

	public void HateUp(ref int hate, AffectEffectProperty currentSuffer)
	{
		hate += currentSuffer.upPower;
	}

	public void HateDown(ref int hate, AffectEffectProperty currentSuffer)
	{
		hate -= currentSuffer.downPower;
	}

	public void AddSufferStateOthers(CharacterStateControl isTargetsStatus, AffectEffectProperty currentSuffer)
	{
		SufferStateProperty suffer = new SufferStateProperty(currentSuffer);
		isTargetsStatus.currentSufferState.SetSufferState(suffer, null);
	}
}
