using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SubStateSkillDetailsFunction : BattleStateController
{
	private SufferStatePropertyCounter sufferStatePropertyCounter;

	private bool isPublicAttack;

	private bool isBigBoss;

	private string cameraKey = string.Empty;

	private List<SkillResults> cacheTargetDataList;

	private SubStateEnemiesItemDroppingFunction subStateEnemiesItemDroppingFunction;

	private SubStatePlayHitAnimationAction subStatePlayHitAnimationAction;

	private List<IEnumerator> hitAnimationList = new List<IEnumerator>();

	private SubStateSkillDetailsFunction.EverySkillLog everySkillLog;

	public SubStateSkillDetailsFunction(Action OnExit, Action<EventState> OnExitGotEvent) : base(null, OnExit, OnExitGotEvent)
	{
	}

	protected override void AwakeThisState()
	{
		this.subStateEnemiesItemDroppingFunction = new SubStateEnemiesItemDroppingFunction(null, new Action<EventState>(base.SendEventState));
		this.subStatePlayHitAnimationAction = new SubStatePlayHitAnimationAction(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStateEnemiesItemDroppingFunction);
		base.AddState(this.subStatePlayHitAnimationAction);
		base.AddState(new SubStatePlayPassiveEffectFunction(null, new Action<EventState>(base.SendEventState)));
	}

	protected override void EnabledThisState()
	{
		this.sufferStatePropertyCounter = new SufferStatePropertyCounter();
		this.cacheTargetDataList = new List<SkillResults>();
		this.hitAnimationList = new List<IEnumerator>();
		this.everySkillLog = new SubStateSkillDetailsFunction.EverySkillLog();
	}

	protected override IEnumerator MainRoutine()
	{
		foreach (CharacterStateControl character in this.GetTotalCharacters())
		{
			character.OnChipTrigger(EffectStatusBase.EffectTriggerType.DamagePossibility);
		}
		CharacterStateControl currentCharacter = null;
		SkillStatus status = null;
		if (base.battleStateData.IsChipSkill())
		{
			currentCharacter = base.battleStateData.GetAutoCounterCharacter();
			status = base.hierarchyData.GetSkillStatus(currentCharacter.chipSkillId);
		}
		else
		{
			currentCharacter = base.battleStateData.currentSelectCharacterState;
			status = currentCharacter.currentSkillStatus;
		}
		base.stateManager.uiControl.ApplyHideHitIcon();
		this.isPublicAttack = (status.skillType == SkillType.Attack);
		if (base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1)
		{
			this.isBigBoss = true;
		}
		bool isProtectEnableSkill = false;
		foreach (AffectEffectProperty affectEffect in status.affectEffect)
		{
			if (AffectEffectProperty.IsDamage(affectEffect.type))
			{
				isProtectEnableSkill = true;
				break;
			}
		}
		List<SubStateSkillDetailsFunction.TargetData> targetDataList = new List<SubStateSkillDetailsFunction.TargetData>();
		int attackCount = 0;
		for (int i = 0; i < status.affectEffect.Count; i++)
		{
			AffectEffectProperty currentSuffer = status.affectEffect[i];
			if (AffectEffectProperty.IsDamage(currentSuffer.type))
			{
				attackCount += currentSuffer.hitNumber;
			}
		}
		AffectEffectProperty lastSuffer = null;
		for (int j = 0; j < status.affectEffect.Count; j++)
		{
			AffectEffectProperty currentSuffer2 = status.affectEffect[j];
			if (lastSuffer != null && lastSuffer.target != currentSuffer2.target)
			{
				CharacterStateControl[] target = base.stateManager.targetSelect.GetSkillTargetList(currentCharacter, currentSuffer2.target);
				if (target != null && target.Length > 0)
				{
					currentCharacter.targetCharacter = target[0];
				}
			}
			lastSuffer = currentSuffer2;
			targetDataList = this.CreateTargetData(targetDataList, currentCharacter, currentSuffer2, isProtectEnableSkill);
			if (targetDataList.Count == 0)
			{
				break;
			}
			if (j != 0)
			{
				IEnumerator motionReset = base.stateManager.threeDAction.MotionResetAliveCharacterAction(new CharacterStateControl[]
				{
					currentCharacter
				});
				while (motionReset.MoveNext())
				{
					object obj = motionReset.Current;
					yield return obj;
				}
				IEnumerator targetMotionReset = base.stateManager.threeDAction.MotionResetAliveCharacterAction(targetDataList.Select((SubStateSkillDetailsFunction.TargetData item) => item.target).ToArray<CharacterStateControl>());
				while (targetMotionReset.MoveNext())
				{
					object obj2 = targetMotionReset.Current;
					yield return obj2;
				}
			}
			yield return this.ShowDigimonAndCamera(targetDataList.Select((SubStateSkillDetailsFunction.TargetData item) => item.target).ToArray<CharacterStateControl>());
			if (j == 0)
			{
				base.battleStateData.SetPlayPassiveEffectFunctionValues(targetDataList.Select((SubStateSkillDetailsFunction.TargetData item) => item.target).ToArray<CharacterStateControl>(), status, currentSuffer2);
				base.SetState(typeof(SubStatePlayPassiveEffectFunction));
				while (base.isWaitState)
				{
					yield return null;
				}
			}
			if (!this.isPublicAttack)
			{
				currentCharacter.hate += currentSuffer2.GetHate() * targetDataList.Count;
			}
			if (targetDataList.Where((SubStateSkillDetailsFunction.TargetData item) => item.onProtect).Any<SubStateSkillDetailsFunction.TargetData>())
			{
				foreach (SubStateSkillDetailsFunction.TargetData targetData in targetDataList)
				{
					if (targetData.onProtect)
					{
						base.stateManager.uiControl.ApplyTurnActionBarSwipeout(true);
						base.stateManager.uiControl.ApplySkillName(true, StringMaster.GetString("BattleUI-47"), targetData.target);
						break;
					}
				}
			}
			yield return this.SkillStartFunction(currentCharacter, targetDataList, currentSuffer2, attackCount);
			yield return this.SkillEndFunction();
			if (targetDataList.Where((SubStateSkillDetailsFunction.TargetData item) => item.onProtect).Any<SubStateSkillDetailsFunction.TargetData>())
			{
				foreach (SubStateSkillDetailsFunction.TargetData targetData2 in targetDataList)
				{
					if (targetData2.onProtect)
					{
						base.stateManager.uiControl.ApplyTurnActionBarSwipeout(false);
						break;
					}
				}
			}
			if (currentCharacter.isDied || base.stateManager.IsLastBattleAndAllDeath())
			{
				break;
			}
		}
		yield return this.PLayEverySkillAnimation();
		this.OnChipTriggerForSufferHit();
		this.OnChipTriggerForSkillDamage(currentCharacter);
		if (currentCharacter.currentSkillStatus != null)
		{
			currentCharacter.currentSkillStatus.ClearAffectEffect();
		}
		foreach (CharacterStateControl character2 in this.GetTotalCharacters())
		{
			character2.ClearGutsData();
		}
		this.sufferStatePropertyCounter.UpdateCount(SufferStateProperty.SufferType.CountGuard);
		this.sufferStatePropertyCounter.UpdateCount(SufferStateProperty.SufferType.CountBarrier);
		this.sufferStatePropertyCounter.UpdateCount(SufferStateProperty.SufferType.CountEvasion);
		yield return this.SendLog(currentCharacter);
		yield break;
	}

	private CharacterStateControl[] GetTotalCharacters()
	{
		CharacterStateControl[] result;
		if (base.stateManager.battleMode == BattleMode.PvP)
		{
			if (base.stateManager.pvpFunction.IsOwner)
			{
				result = base.battleStateData.GetTotalCharacters();
			}
			else
			{
				result = base.battleStateData.GetTotalCharactersEnemyFirst();
			}
		}
		else
		{
			result = base.battleStateData.GetTotalCharacters();
		}
		return result;
	}

	private void OnChipTriggerForSufferHit()
	{
		CharacterStateControl[] totalCharacters = this.GetTotalCharacters();
		foreach (CharacterStateControl characterStateControl in totalCharacters)
		{
			if (characterStateControl.hitSufferList.Count > 0)
			{
				characterStateControl.OnChipTrigger(EffectStatusBase.EffectTriggerType.SufferHit);
			}
		}
		foreach (CharacterStateControl characterStateControl2 in totalCharacters)
		{
			characterStateControl2.hitSufferList.Clear();
		}
	}

	private void OnChipTriggerForSkillDamage(CharacterStateControl currentCharacter)
	{
		if (base.battleStateData.IsChipSkill())
		{
			return;
		}
		SkillResults[] source = this.cacheTargetDataList.Where((SkillResults item) => item.attackCharacter == currentCharacter).ToArray<SkillResults>();
		if (!currentCharacter.isDied)
		{
			if (source.Where((SkillResults item) => !item.onMissHit && AffectEffectProperty.IsDamage(item.hitIconAffectEffect) && AffectEffectProperty.IsDamage(item.useAffectEffectProperty.type)).Any<SkillResults>())
			{
				currentCharacter.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillDamageHitSend);
			}
			if (source.Where((SkillResults item) => AffectEffectProperty.IsDamage(item.hitIconAffectEffect) && AffectEffectProperty.IsDamage(item.useAffectEffectProperty.type)).Any<SkillResults>())
			{
				currentCharacter.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillDamageEndSend);
			}
			if (source.Where((SkillResults item) => item.onMissHit).Any<SkillResults>())
			{
				currentCharacter.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillMiss);
			}
		}
		CharacterStateControl[] array = source.Select((SkillResults item) => item.targetCharacter).Distinct<CharacterStateControl>().ToArray<CharacterStateControl>();
		foreach (CharacterStateControl target in array)
		{
			if (!target.isDied)
			{
				SkillResults[] source2 = this.cacheTargetDataList.Where((SkillResults item) => item.targetCharacter == target).ToArray<SkillResults>();
				if (source2.Where((SkillResults item) => !item.onMissHit && AffectEffectProperty.IsDamage(item.hitIconAffectEffect) && AffectEffectProperty.IsDamage(item.useAffectEffectProperty.type)).Any<SkillResults>())
				{
					target.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillDamageHitRecieve);
				}
				if (source2.Where((SkillResults item) => AffectEffectProperty.IsDamage(item.hitIconAffectEffect) && AffectEffectProperty.IsDamage(item.useAffectEffectProperty.type)).Any<SkillResults>())
				{
					target.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillDamageEndRecieve);
				}
			}
		}
	}

	private List<SubStateSkillDetailsFunction.TargetData> CreateTargetData(List<SubStateSkillDetailsFunction.TargetData> oldTargetDataList, CharacterStateControl currentCharacter, AffectEffectProperty currentSuffer, bool isProtectEnableSkill)
	{
		List<SubStateSkillDetailsFunction.TargetData> list = new List<SubStateSkillDetailsFunction.TargetData>();
		CharacterStateControl[] array;
		if (base.battleStateData.isConfusionAttack)
		{
			if (currentCharacter.isEnemy)
			{
				array = base.stateManager.targetSelect.GetSkillTargetList(base.battleStateData.playerCharacters[0], currentSuffer.target);
			}
			else
			{
				array = base.stateManager.targetSelect.GetSkillTargetList(base.battleStateData.enemies[0], currentSuffer.target);
			}
		}
		else
		{
			array = base.stateManager.targetSelect.GetSkillTargetList(currentCharacter, currentSuffer.target);
		}
		if (array == null || array.Length == 0)
		{
			return list;
		}
		bool flag = false;
		if (currentSuffer.effectNumbers == EffectNumbers.Simple)
		{
			CharacterStateControl[] array2 = array.Where((CharacterStateControl item) => item.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Protect)).ToArray<CharacterStateControl>();
			if (base.battleStateData.isConfusionAttack)
			{
				flag = (isProtectEnableSkill && array2 != null && array2.Length > 0);
			}
			else
			{
				flag = (isProtectEnableSkill && array2 != null && array2.Length > 0 && currentCharacter.isEnemy != array[0].isEnemy);
			}
			if (flag)
			{
				array = CharacterStateControlSorter.SortedSufferStateGenerateStartTiming(array2, SufferStateProperty.SufferType.Protect);
				currentCharacter.targetCharacter = array[0];
			}
			if (currentCharacter.targetCharacter.isDied)
			{
				return list;
			}
			array = new CharacterStateControl[]
			{
				currentCharacter.targetCharacter
			};
		}
		if (!currentSuffer.isMissThrough && oldTargetDataList.Count > 0)
		{
			List<CharacterStateControl> list2 = new List<CharacterStateControl>();
			foreach (CharacterStateControl target in array)
			{
				SubStateSkillDetailsFunction.TargetData targetData = oldTargetDataList.Where((SubStateSkillDetailsFunction.TargetData item) => item.target == target).SingleOrDefault<SubStateSkillDetailsFunction.TargetData>();
				bool flag2;
				if (targetData != null)
				{
					flag2 = !targetData.isAllMiss;
				}
				else
				{
					flag2 = oldTargetDataList.Where((SubStateSkillDetailsFunction.TargetData item) => !item.isAllMiss).Any<SubStateSkillDetailsFunction.TargetData>();
				}
				if (flag2)
				{
					list2.Add(target);
				}
			}
			if (list2.Count <= 0)
			{
				return list;
			}
			array = list2.ToArray();
		}
		foreach (CharacterStateControl target2 in array)
		{
			list.Add(new SubStateSkillDetailsFunction.TargetData
			{
				target = target2,
				isAllMiss = true,
				onProtect = flag,
				isDamage = isProtectEnableSkill
			});
		}
		return list;
	}

	private IEnumerator SkillStartFunction(CharacterStateControl currentCharacter, List<SubStateSkillDetailsFunction.TargetData> targetDataList, AffectEffectProperty currentSuffer, int attackCount)
	{
		if (AffectEffectProperty.IsDamage(currentSuffer.type))
		{
			return this.AffectEffectDamage(currentCharacter, targetDataList, currentSuffer, attackCount);
		}
		if (currentSuffer.type == AffectEffect.Destruct)
		{
			return this.AffectEffectDestruct(currentCharacter, targetDataList, currentSuffer);
		}
		if (currentSuffer.type == AffectEffect.ApDrain)
		{
			return this.AffectEffectApDrain(currentCharacter, targetDataList, currentSuffer);
		}
		if (Tolerance.OnInfluenceToleranceAffectEffect(currentSuffer.type))
		{
			return this.ToleranceOnInfluenceToleranceAffectEffect(currentCharacter, targetDataList, currentSuffer);
		}
		return this.Other(currentCharacter, targetDataList, currentSuffer);
	}

	private IEnumerator SkillEndFunction()
	{
		IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.skillAfterWaitSecond, null, null);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		base.stateManager.threeDAction.HideDeadCharactersAction(base.battleStateData.GetTotalCharacters());
		this.StopCamera();
		base.stateManager.uiControl.HideCharacterHUDFunction();
		base.stateManager.uiControl.ApplyHideHitIcon();
		yield break;
	}

	private IEnumerator AffectEffectDamage(CharacterStateControl currentCharacter, List<SubStateSkillDetailsFunction.TargetData> targetDataList, AffectEffectProperty currentSuffer, int attackCount)
	{
		int totalDamage = 0;
		bool isApRevival = false;
		List<int> totalCounterReflectionDamage = new List<int>();
		AffectEffect counterReflectionAffectType = AffectEffect.Counter;
		bool isAllTargetDeathBreak = false;
		for (int hitNumber = 0; hitNumber < currentSuffer.hitNumber; hitNumber++)
		{
			SubStatePlayHitAnimationAction.Data data = new SubStatePlayHitAnimationAction.Data();
			int counterReflectionDamage = 0;
			if (!base.battleStateData.IsChipSkill() && !this.isPublicAttack)
			{
				currentCharacter.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillDamageStartedSendEvery);
				this.ExecuteEverySkill(EffectStatusBase.EffectTriggerType.SkillDamageStartedSendEvery, currentCharacter, targetDataList.Select((SubStateSkillDetailsFunction.TargetData item) => item.target).ToArray<CharacterStateControl>());
				foreach (SubStateSkillDetailsFunction.TargetData targetData in targetDataList)
				{
					targetData.target.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillDamageStartedRecieveEvery);
					this.ExecuteEverySkill(EffectStatusBase.EffectTriggerType.SkillDamageStartedRecieveEvery, targetData.target, new CharacterStateControl[]
					{
						currentCharacter
					});
				}
				if (currentCharacter.isDied)
				{
					break;
				}
			}
			for (int i = 0; i < targetDataList.Count; i++)
			{
				CharacterStateControl target = targetDataList[i].target;
				if (!target.isDied)
				{
					SkillResults skillResult = SkillStatus.GetSkillResults(currentSuffer, currentCharacter, target, base.hierarchyData.onEnableRandomValue, attackCount);
					if (targetDataList[i].isAllMiss)
					{
						targetDataList[i].isAllMiss = skillResult.onMissHit;
					}
					if (!skillResult.onMissHit)
					{
						if (skillResult.targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.CountBarrier))
						{
							this.sufferStatePropertyCounter.AddCountDictionary(SufferStateProperty.SufferType.CountBarrier, skillResult.targetCharacter);
						}
						else if (skillResult.targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.CountEvasion))
						{
							this.sufferStatePropertyCounter.AddCountDictionary(SufferStateProperty.SufferType.CountEvasion, skillResult.targetCharacter);
						}
						else if (skillResult.targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.CountGuard))
						{
							this.sufferStatePropertyCounter.AddCountDictionary(SufferStateProperty.SufferType.CountGuard, skillResult.targetCharacter);
						}
						else
						{
							this.CheckFraud(skillResult.attackPower, currentCharacter, target);
							if (skillResult.onCriticalHit)
							{
								isApRevival = true;
								if (currentCharacter.isSelectSkill > 0)
								{
									currentCharacter.hate += currentSuffer.GetHate();
								}
							}
							totalDamage += skillResult.attackPower;
							if (!target.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Sleep))
							{
								if (currentSuffer.techniqueType == TechniqueType.Physics)
								{
									SufferStateProperty counterSuffer = target.currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.Counter);
									if (counterSuffer.isActive)
									{
										int reflectDamage = counterSuffer.GetReflectDamage(skillResult.attackPower);
										counterReflectionDamage += reflectDamage;
										counterReflectionAffectType = AffectEffect.Counter;
									}
								}
								else if (currentSuffer.techniqueType == TechniqueType.Special)
								{
									SufferStateProperty reflectionSuffer = target.currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.Reflection);
									if (reflectionSuffer.isActive)
									{
										int reflectDamage2 = reflectionSuffer.GetReflectDamage(skillResult.attackPower);
										counterReflectionDamage += reflectDamage2;
										counterReflectionAffectType = AffectEffect.Reflection;
									}
								}
							}
						}
					}
					data.AddHitIcon(targetDataList[i].target, skillResult.hitIconAffectEffect, skillResult.attackPower, skillResult.onWeakHit, skillResult.onMissHit, skillResult.onCriticalHit, false, false, false, skillResult.extraEffectType);
					this.cacheTargetDataList.Add(skillResult);
				}
			}
			isAllTargetDeathBreak = !targetDataList.Where((SubStateSkillDetailsFunction.TargetData item) => !item.target.isDied).Any<SubStateSkillDetailsFunction.TargetData>();
			if (counterReflectionDamage > 0)
			{
				totalCounterReflectionDamage.Add(counterReflectionDamage);
			}
			this.ApplyMonsterIconEnabled(currentCharacter);
			if (!base.stateManager.IsLastBattleAndAllDeath())
			{
				base.stateManager.cameraControl.PlayCameraShake();
			}
			float interval = 0f;
			if (isAllTargetDeathBreak)
			{
				interval = base.stateManager.stateProperty.multiHitIntervalWaitSecond;
			}
			else
			{
				interval = base.stateManager.stateProperty.multiHitIntervalWaitSecond / (float)currentSuffer.hitNumber;
			}
			data.time = interval;
			data.affectEffectProperty = currentSuffer;
			this.subStatePlayHitAnimationAction.Init(data);
			base.SetState(this.subStatePlayHitAnimationAction.GetType());
			while (base.isWaitState)
			{
				yield return null;
			}
			if (!isAllTargetDeathBreak && currentSuffer.hitNumber > 1)
			{
				IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(0.25f, null, null);
				while (wait.MoveNext())
				{
					yield return null;
				}
			}
			if (!base.battleStateData.IsChipSkill() && !this.isPublicAttack)
			{
				if (data.hitIconList.Where((SubStatePlayHitAnimationAction.Data.HitIcon item) => !item.isMiss && AffectEffectProperty.IsDamage(item.affectEffect)).Any<SubStatePlayHitAnimationAction.Data.HitIcon>())
				{
					currentCharacter.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillDamageHitSendEvery);
					this.ExecuteEverySkill(EffectStatusBase.EffectTriggerType.SkillDamageHitSendEvery, currentCharacter, targetDataList.Select((SubStateSkillDetailsFunction.TargetData item) => item.target).ToArray<CharacterStateControl>());
				}
				currentCharacter.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillDamageEndSendEvery);
				this.ExecuteEverySkill(EffectStatusBase.EffectTriggerType.SkillDamageEndSendEvery, currentCharacter, targetDataList.Select((SubStateSkillDetailsFunction.TargetData item) => item.target).ToArray<CharacterStateControl>());
				foreach (SubStatePlayHitAnimationAction.Data.HitIcon hitIcon in data.hitIconList)
				{
					if (!hitIcon.isMiss && AffectEffectProperty.IsDamage(hitIcon.affectEffect))
					{
						hitIcon.target.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillDamageHitRecieveEvery);
						this.ExecuteEverySkill(EffectStatusBase.EffectTriggerType.SkillDamageHitRecieveEvery, hitIcon.target, new CharacterStateControl[]
						{
							currentCharacter
						});
					}
					hitIcon.target.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillDamageEndRecieveEvery);
					this.ExecuteEverySkill(EffectStatusBase.EffectTriggerType.SkillDamageEndRecieveEvery, hitIcon.target, new CharacterStateControl[]
					{
						currentCharacter
					});
				}
				if (currentCharacter.isDied)
				{
					break;
				}
			}
			if (isAllTargetDeathBreak)
			{
				break;
			}
		}
		if (!base.stateManager.IsLastBattleAndAllDeath())
		{
			base.stateManager.cameraControl.StopCameraShake();
		}
		if (this.isBigBoss && isAllTargetDeathBreak)
		{
			foreach (SubStateSkillDetailsFunction.TargetData targetData2 in targetDataList)
			{
				IEnumerator transitionCount = base.stateManager.threeDAction.BigBossExitAction(targetData2.target);
				while (transitionCount.MoveNext())
				{
					yield return null;
				}
			}
		}
		SubStateSkillDetailsFunction.TargetData[] deads = targetDataList.Where((SubStateSkillDetailsFunction.TargetData item) => item.target.isDied).ToArray<SubStateSkillDetailsFunction.TargetData>();
		if (deads.Count<SubStateSkillDetailsFunction.TargetData>() > 0)
		{
			currentCharacter.OnChipTrigger(EffectStatusBase.EffectTriggerType.Kill);
			IEnumerator dropItem = this.DropItem(deads.Select((SubStateSkillDetailsFunction.TargetData item) => item.target).ToArray<CharacterStateControl>());
			while (dropItem.MoveNext())
			{
				object obj = dropItem.Current;
				yield return obj;
			}
		}
		foreach (SubStateSkillDetailsFunction.TargetData targetData3 in targetDataList)
		{
			if (!targetData3.isAllMiss)
			{
				SufferStateProperty sufferStateProperty = targetData3.target.currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.Sleep);
				if (sufferStateProperty.isActive && sufferStateProperty.GetSleepGetupOccurrenceDamage())
				{
					targetData3.target.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.Sleep);
				}
			}
		}
		if (!base.stateManager.IsLastBattleAndAllDeath() && currentSuffer.useDrain && !currentCharacter.isDied)
		{
			IEnumerator function = this.DrainRecovery(currentCharacter, currentSuffer, totalDamage);
			while (function.MoveNext())
			{
				object obj2 = function.Current;
				yield return obj2;
			}
		}
		if (!base.stateManager.IsLastBattleAndAllDeath() && totalCounterReflectionDamage.Count > 0 && !currentCharacter.isDied)
		{
			IEnumerator function2 = this.TotalCounterReflectionDamage(currentCharacter, currentSuffer, counterReflectionAffectType, totalCounterReflectionDamage);
			while (function2.MoveNext())
			{
				object obj3 = function2.Current;
				yield return obj3;
			}
		}
		base.battleStateData.SetApRevival(currentCharacter, isApRevival);
		yield break;
	}

	protected virtual void CheckFraud(int damage, CharacterStateControl currentCharacter, CharacterStateControl targetCharacter)
	{
		base.stateManager.fraudCheck.FraudCheckMaximumAttackerDamage(damage, currentCharacter);
		base.stateManager.fraudCheck.FraudCheckMinimumTargetDamage(damage, targetCharacter);
	}

	protected virtual void ApplyMonsterIconEnabled(CharacterStateControl currentCharacter)
	{
	}

	protected virtual IEnumerator DropItem(CharacterStateControl[] currentDeadCharacters)
	{
		this.subStateEnemiesItemDroppingFunction.Init(currentDeadCharacters);
		base.SetState(this.subStateEnemiesItemDroppingFunction.GetType());
		while (base.isWaitState)
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator DrainRecovery(CharacterStateControl currentCharacter, AffectEffectProperty currentSuffer, int totalDamage)
	{
		SubStatePlayHitAnimationAction.Data data = new SubStatePlayHitAnimationAction.Data();
		yield return this.SkillEndFunction();
		yield return this.ShowDigimonAndCamera(new CharacterStateControl[]
		{
			currentCharacter
		});
		int recoveryDamage = Mathf.FloorToInt((float)totalDamage * currentSuffer.revivalPercent);
		currentCharacter.hp += recoveryDamage;
		SkillResults skillResult = new SkillResults();
		skillResult.useAffectEffectProperty = currentSuffer;
		skillResult.hitIconAffectEffect = AffectEffect.HpRevival;
		skillResult.attackCharacter = currentCharacter;
		skillResult.targetCharacter = currentCharacter;
		skillResult.onMissHit = false;
		skillResult.attackPower = recoveryDamage;
		skillResult.originalAttackPower = recoveryDamage;
		data.AddHitIcon(skillResult.targetCharacter, skillResult.hitIconAffectEffect, skillResult.attackPower, skillResult.onWeakHit, skillResult.onMissHit, skillResult.onCriticalHit, true, false, false, skillResult.extraEffectType);
		this.cacheTargetDataList.Add(skillResult);
		data.time = base.stateManager.stateProperty.attackerCharacterDrainActionWaitSecond;
		data.affectEffectProperty = currentSuffer;
		this.subStatePlayHitAnimationAction.Init(data);
		base.SetState(this.subStatePlayHitAnimationAction.GetType());
		while (base.isWaitState)
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator TotalCounterReflectionDamage(CharacterStateControl currentCharacter, AffectEffectProperty currentSuffer, AffectEffect counterReflectionAffectType, List<int> totalCounterReflectionDamage)
	{
		yield return this.SkillEndFunction();
		yield return this.ShowDigimonAndCamera(new CharacterStateControl[]
		{
			currentCharacter
		});
		for (int i = 0; i < totalCounterReflectionDamage.Count; i++)
		{
			if (currentCharacter.isDied)
			{
				break;
			}
			AffectEffect affectEffect = AffectEffect.Damage;
			if (currentCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.TurnBarrier))
			{
				affectEffect = AffectEffect.TurnBarrier;
			}
			else if (currentCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.CountBarrier))
			{
				affectEffect = AffectEffect.CountBarrier;
				this.sufferStatePropertyCounter.AddCountDictionary(SufferStateProperty.SufferType.CountBarrier, currentCharacter);
			}
			else if (currentCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.TurnEvasion))
			{
				affectEffect = AffectEffect.TurnEvasion;
			}
			else if (currentCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.CountEvasion))
			{
				affectEffect = AffectEffect.CountEvasion;
				this.sufferStatePropertyCounter.AddCountDictionary(SufferStateProperty.SufferType.CountEvasion, currentCharacter);
			}
			else
			{
				currentCharacter.hp -= totalCounterReflectionDamage[i];
			}
			SkillResults skillResult = new SkillResults();
			skillResult.useAffectEffectProperty = currentSuffer;
			skillResult.hitIconAffectEffect = affectEffect;
			skillResult.attackCharacter = currentCharacter.targetCharacter;
			skillResult.targetCharacter = currentCharacter;
			skillResult.attackPower = totalCounterReflectionDamage[i];
			skillResult.originalAttackPower = totalCounterReflectionDamage[i];
			skillResult.onMissHit = false;
			SubStatePlayHitAnimationAction.Data data = new SubStatePlayHitAnimationAction.Data();
			data.AddHitIcon(skillResult.targetCharacter, skillResult.hitIconAffectEffect, skillResult.attackPower, skillResult.onWeakHit, skillResult.onMissHit, skillResult.onCriticalHit, false, counterReflectionAffectType == AffectEffect.Counter, counterReflectionAffectType == AffectEffect.Reflection, skillResult.extraEffectType);
			this.cacheTargetDataList.Add(skillResult);
			data.time = base.stateManager.stateProperty.targetCounterReflectionActionWaitSecond / (float)totalCounterReflectionDamage.Count;
			data.affectEffectProperty = currentSuffer;
			this.subStatePlayHitAnimationAction.Init(data);
			base.SetState(this.subStatePlayHitAnimationAction.GetType());
			while (base.isWaitState)
			{
				yield return null;
			}
			if (totalCounterReflectionDamage.Count > 1)
			{
				IEnumerator wait2 = base.stateManager.time.WaitForCertainPeriodTimeAction(0.25f, null, null);
				while (wait2.MoveNext())
				{
					yield return null;
				}
			}
		}
		if (currentCharacter.isDied)
		{
			if (this.isBigBoss)
			{
				IEnumerator transitionCount = base.stateManager.threeDAction.BigBossExitAction(currentCharacter);
				while (transitionCount.MoveNext())
				{
					yield return null;
				}
			}
			IEnumerator dropItem = this.DropItem(new CharacterStateControl[]
			{
				currentCharacter
			});
			while (dropItem.MoveNext())
			{
				object obj = dropItem.Current;
				yield return obj;
			}
		}
		yield break;
	}

	private IEnumerator AffectEffectDestruct(CharacterStateControl currentCharacter, List<SubStateSkillDetailsFunction.TargetData> targetDataList, AffectEffectProperty currentSuffer)
	{
		yield return this.AffectEffectDestructActor(currentCharacter, targetDataList, currentSuffer);
		yield return this.SkillEndFunction();
		yield return this.AffectEffectDestructTarget(currentCharacter, targetDataList, currentSuffer);
		yield break;
	}

	private IEnumerator AffectEffectDestructActor(CharacterStateControl currentCharacter, List<SubStateSkillDetailsFunction.TargetData> targetDataList, AffectEffectProperty currentSuffer)
	{
		SubStatePlayHitAnimationAction.Data data = new SubStatePlayHitAnimationAction.Data();
		yield return this.ShowDigimonAndCamera(new CharacterStateControl[]
		{
			currentCharacter
		});
		base.stateManager.uiControl.HideCharacterHUDFunction();
		currentCharacter.Kill();
		data.AddHitIcon(currentCharacter, currentSuffer.type, 0, Strength.None, false, false, false, false, false, ExtraEffectType.Non);
		data.time = base.stateManager.stateProperty.destructCharacterDeathActionWaitSecond;
		data.affectEffectProperty = currentSuffer;
		this.subStatePlayHitAnimationAction.Init(data);
		base.SetState(this.subStatePlayHitAnimationAction.GetType());
		while (base.isWaitState)
		{
			yield return null;
		}
		if (this.isBigBoss)
		{
			IEnumerator transitionCount = base.stateManager.threeDAction.BigBossExitAction(currentCharacter);
			while (transitionCount.MoveNext())
			{
				yield return null;
			}
		}
		IEnumerator dropItem = this.DropItem(new CharacterStateControl[]
		{
			currentCharacter
		});
		while (dropItem.MoveNext())
		{
			object obj = dropItem.Current;
			yield return obj;
		}
		yield break;
	}

	private IEnumerator AffectEffectDestructTarget(CharacterStateControl currentCharacter, List<SubStateSkillDetailsFunction.TargetData> targetDataList, AffectEffectProperty currentSuffer)
	{
		SubStatePlayHitAnimationAction.Data data = new SubStatePlayHitAnimationAction.Data();
		yield return this.ShowDigimonAndCamera(targetDataList.Select((SubStateSkillDetailsFunction.TargetData item) => item.target).ToArray<CharacterStateControl>());
		for (int i = 0; i < targetDataList.Count; i++)
		{
			if (!targetDataList[i].target.isDied)
			{
				SkillResults skillResult = base.stateManager.skillDetails.GetDestructTargetSkillResult(currentSuffer, currentCharacter, targetDataList[i].target);
				targetDataList[i].isAllMiss = skillResult.onMissHit;
				data.AddHitIcon(targetDataList[i].target, skillResult.hitIconAffectEffect, 0, skillResult.onWeakHit, skillResult.onMissHit, skillResult.onCriticalHit, false, false, false, skillResult.extraEffectType);
				this.cacheTargetDataList.Add(skillResult);
			}
		}
		base.stateManager.cameraControl.PlayCameraShake();
		base.stateManager.soundPlayer.StopHitEffectSE();
		data.time = base.stateManager.stateProperty.multiHitIntervalWaitSecond;
		data.affectEffectProperty = currentSuffer;
		this.subStatePlayHitAnimationAction.Init(data);
		base.SetState(this.subStatePlayHitAnimationAction.GetType());
		while (base.isWaitState)
		{
			yield return null;
		}
		base.stateManager.cameraControl.StopCameraShake();
		yield break;
	}

	protected virtual IEnumerator AffectEffectApDrain(CharacterStateControl currentCharacter, List<SubStateSkillDetailsFunction.TargetData> targetDataList, AffectEffectProperty currentSuffer)
	{
		SubStatePlayHitAnimationAction.Data data = new SubStatePlayHitAnimationAction.Data();
		int totalApDrain = 0;
		for (int i = 0; i < targetDataList.Count; i++)
		{
			if (!targetDataList[i].target.isDied)
			{
				SkillResults skillResult = base.stateManager.skillDetails.GetApDrainSkillResult(currentSuffer, currentCharacter, targetDataList[i].target);
				targetDataList[i].isAllMiss = skillResult.onMissHit;
				if (!skillResult.onMissHit)
				{
					if (skillResult.targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.CountBarrier))
					{
						this.sufferStatePropertyCounter.AddCountDictionary(SufferStateProperty.SufferType.CountBarrier, skillResult.targetCharacter);
					}
					else if (skillResult.targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.CountEvasion))
					{
						this.sufferStatePropertyCounter.AddCountDictionary(SufferStateProperty.SufferType.CountEvasion, skillResult.targetCharacter);
					}
					else
					{
						totalApDrain += skillResult.attackPower;
					}
				}
				data.AddHitIcon(targetDataList[i].target, skillResult.hitIconAffectEffect, 0, skillResult.onWeakHit, skillResult.onMissHit, skillResult.onCriticalHit, false, false, false, skillResult.extraEffectType);
				this.cacheTargetDataList.Add(skillResult);
			}
		}
		data.time = base.stateManager.stateProperty.multiHitIntervalWaitSecond;
		data.affectEffectProperty = currentSuffer;
		this.subStatePlayHitAnimationAction.Init(data);
		base.SetState(this.subStatePlayHitAnimationAction.GetType());
		while (base.isWaitState)
		{
			yield return null;
		}
		yield return this.ApDrain(currentCharacter, targetDataList, currentSuffer, totalApDrain);
		yield break;
	}

	private IEnumerator ApDrain(CharacterStateControl currentCharacter, List<SubStateSkillDetailsFunction.TargetData> targetDataList, AffectEffectProperty currentSuffer, int totalApDrain)
	{
		SubStatePlayHitAnimationAction.Data data = new SubStatePlayHitAnimationAction.Data();
		yield return this.SkillEndFunction();
		yield return this.ShowDigimonAndCamera(new CharacterStateControl[]
		{
			currentCharacter
		});
		currentCharacter.ap += totalApDrain;
		data.AddHitIcon(currentCharacter, AffectEffect.ApDrain, 0, Strength.None, false, false, false, false, false, ExtraEffectType.Non);
		data.time = base.stateManager.stateProperty.attackerCharacterDrainActionWaitSecond;
		data.affectEffectProperty = currentSuffer;
		this.subStatePlayHitAnimationAction.Init(data);
		base.SetState(this.subStatePlayHitAnimationAction.GetType());
		while (base.isWaitState)
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator ToleranceOnInfluenceToleranceAffectEffect(CharacterStateControl currentCharacter, List<SubStateSkillDetailsFunction.TargetData> targetDataList, AffectEffectProperty currentSuffer)
	{
		SubStatePlayHitAnimationAction.Data data = new SubStatePlayHitAnimationAction.Data();
		for (int i = 0; i < targetDataList.Count; i++)
		{
			if (!targetDataList[i].target.isDied)
			{
				SkillResults skillResult = base.stateManager.skillDetails.GetToleranceSkillResult(currentSuffer, currentCharacter, targetDataList[i].target);
				targetDataList[i].isAllMiss = skillResult.onMissHit;
				if (!skillResult.onMissHit)
				{
					if (skillResult.targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.CountBarrier))
					{
						this.sufferStatePropertyCounter.AddCountDictionary(SufferStateProperty.SufferType.CountBarrier, skillResult.targetCharacter);
					}
					else if (skillResult.targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.CountEvasion))
					{
						this.sufferStatePropertyCounter.AddCountDictionary(SufferStateProperty.SufferType.CountEvasion, skillResult.targetCharacter);
					}
				}
				data.AddHitIcon(targetDataList[i].target, skillResult.hitIconAffectEffect, skillResult.attackPower, skillResult.onWeakHit, skillResult.onMissHit, skillResult.onCriticalHit, false, false, false, skillResult.extraEffectType);
				this.cacheTargetDataList.Add(skillResult);
			}
		}
		foreach (SubStateSkillDetailsFunction.TargetData targetData in targetDataList)
		{
			if (!targetData.isAllMiss && !base.battleStateData.IsChipSkill())
			{
				SufferStateProperty suffer = new SufferStateProperty(currentSuffer, 0);
				targetData.target.hitSufferList.Add(suffer);
			}
		}
		if (currentSuffer.type == AffectEffect.InstantDeath)
		{
			base.stateManager.cameraControl.PlayCameraShake();
		}
		data.time = base.stateManager.stateProperty.multiHitIntervalWaitSecond;
		data.affectEffectProperty = currentSuffer;
		this.subStatePlayHitAnimationAction.Init(data);
		base.SetState(this.subStatePlayHitAnimationAction.GetType());
		while (base.isWaitState)
		{
			yield return null;
		}
		if (currentSuffer.type == AffectEffect.InstantDeath)
		{
			base.stateManager.cameraControl.StopCameraShake();
		}
		SubStateSkillDetailsFunction.TargetData[] deads = targetDataList.Where((SubStateSkillDetailsFunction.TargetData item) => item.target.isDied).ToArray<SubStateSkillDetailsFunction.TargetData>();
		if (deads.Length > 0)
		{
			currentCharacter.OnChipTrigger(EffectStatusBase.EffectTriggerType.Kill);
			IEnumerator dropItem = this.DropItem(deads.Select((SubStateSkillDetailsFunction.TargetData item) => item.target).ToArray<CharacterStateControl>());
			while (dropItem.MoveNext())
			{
				object obj = dropItem.Current;
				yield return obj;
			}
		}
		yield break;
	}

	protected virtual IEnumerator Other(CharacterStateControl currentCharacter, List<SubStateSkillDetailsFunction.TargetData> targetDataList, AffectEffectProperty currentSuffer)
	{
		SubStatePlayHitAnimationAction.Data data = new SubStatePlayHitAnimationAction.Data();
		for (int i = 0; i < targetDataList.Count; i++)
		{
			if (!targetDataList[i].target.isDied)
			{
				SkillResults skillResult = base.stateManager.skillDetails.GetOtherSkillResult(currentSuffer, currentCharacter, targetDataList[i].target);
				targetDataList[i].isAllMiss = skillResult.onMissHit;
				data.AddHitIcon(targetDataList[i].target, skillResult.hitIconAffectEffect, skillResult.attackPower, skillResult.onWeakHit, skillResult.onMissHit, skillResult.onCriticalHit, false, false, false, skillResult.extraEffectType);
				this.cacheTargetDataList.Add(skillResult);
			}
		}
		foreach (SubStateSkillDetailsFunction.TargetData targetData in targetDataList)
		{
			if (!targetData.isAllMiss && !base.battleStateData.IsChipSkill())
			{
				SufferStateProperty suffer = new SufferStateProperty(currentSuffer, 0);
				targetData.target.hitSufferList.Add(suffer);
			}
		}
		data.time = base.stateManager.stateProperty.multiHitIntervalWaitSecond;
		data.affectEffectProperty = currentSuffer;
		this.subStatePlayHitAnimationAction.Init(data);
		base.SetState(this.subStatePlayHitAnimationAction.GetType());
		while (base.isWaitState)
		{
			yield return null;
		}
		yield break;
	}

	protected override void DisabledThisState()
	{
		this.ShowRevivalEffect();
	}

	protected override void GetEventThisState(EventState eventState)
	{
	}

	private IEnumerator ShowDigimonAndCamera(CharacterStateControl[] targets)
	{
		this.PlayCamera(targets);
		this.ShowDigimon(targets[0]);
		yield return null;
		this.PlayCamera(targets);
		yield return null;
		yield break;
	}

	private void ShowDigimon(CharacterStateControl target)
	{
		CharacterStateControl[] characters;
		CharacterStateControl[] characters2;
		if (!target.isEnemy)
		{
			characters = base.battleStateData.playerCharacters.Where((CharacterStateControl x) => !x.isDied).ToArray<CharacterStateControl>();
			characters2 = base.battleStateData.enemies.Where((CharacterStateControl x) => !x.isDied).ToArray<CharacterStateControl>();
		}
		else
		{
			characters = base.battleStateData.enemies.Where((CharacterStateControl x) => !x.isDied).ToArray<CharacterStateControl>();
			characters2 = base.battleStateData.playerCharacters.Where((CharacterStateControl x) => !x.isDied).ToArray<CharacterStateControl>();
		}
		base.stateManager.threeDAction.ShowAllCharactersAction(characters);
		base.stateManager.threeDAction.PlayIdleAnimationActiveCharacterAction(characters);
		base.stateManager.threeDAction.HideAllCharactersAction(characters2);
	}

	private void PlayCamera(CharacterStateControl[] targets)
	{
		if (targets == null || targets.Length == 0)
		{
			return;
		}
		if (targets.Length > 1)
		{
			if (targets[0].isEnemy && this.isBigBoss)
			{
				this.cameraKey = "BigBoss/skillA";
			}
			else
			{
				this.cameraKey = "skillA";
			}
		}
		else if (targets[0].isEnemy && this.isBigBoss)
		{
			this.cameraKey = "BigBoss/skillF";
		}
		else
		{
			this.cameraKey = "skillF";
		}
		base.stateManager.cameraControl.PlayCameraMotionActionCharacter(this.cameraKey, targets[0]);
		if (!targets[0].isEnemy)
		{
			this.ShowRevivalEffect();
		}
		else
		{
			this.HideRevivalEffect();
		}
	}

	private void StopCamera()
	{
		base.stateManager.cameraControl.StopCameraMotionAction(this.cameraKey);
	}

	private void ShowRevivalEffect()
	{
		AlwaysEffectParams[] isActiveRevivalReservedEffect = base.battleStateData.GetIsActiveRevivalReservedEffect();
		foreach (AlwaysEffectParams alwaysEffect in isActiveRevivalReservedEffect)
		{
			base.stateManager.threeDAction.PlayAlwaysEffectAction(alwaysEffect, AlwaysEffectState.Always);
		}
	}

	private void HideRevivalEffect()
	{
		AlwaysEffectParams[] isActiveRevivalReservedEffect = base.battleStateData.GetIsActiveRevivalReservedEffect();
		foreach (AlwaysEffectParams alwaysEffectParams in isActiveRevivalReservedEffect)
		{
			base.stateManager.threeDAction.StopAlwaysEffectAction(new AlwaysEffectParams[]
			{
				alwaysEffectParams
			});
		}
	}

	private void ExecuteEverySkill(EffectStatusBase.EffectTriggerType effectTriggerType, CharacterStateControl actor, CharacterStateControl[] targets)
	{
		if (!actor.everySkillList.ContainsKey(effectTriggerType))
		{
			return;
		}
		foreach (AffectEffectProperty affectEffectProperty in actor.everySkillList[effectTriggerType])
		{
			CharacterStateControl[] array = BattleStateManager.current.targetSelect.GetSkillTargetList(actor, affectEffectProperty.target);
			if (array.Length != 0)
			{
				if (affectEffectProperty.effectNumbers == EffectNumbers.Simple)
				{
					switch (affectEffectProperty.target)
					{
					case EffectTarget.Enemy:
						array = new CharacterStateControl[]
						{
							targets[0]
						};
						break;
					case EffectTarget.Ally:
						array = new CharacterStateControl[]
						{
							array[0]
						};
						break;
					case EffectTarget.Attacker:
						array = new CharacterStateControl[]
						{
							actor
						};
						break;
					case EffectTarget.EnemyWithoutAttacker:
						array = new CharacterStateControl[]
						{
							array[0]
						};
						break;
					case EffectTarget.AllyWithoutAttacker:
						array = new CharacterStateControl[]
						{
							array[0]
						};
						break;
					}
				}
				if (AffectEffectProperty.IsDamage(affectEffectProperty.type))
				{
					for (int i = 0; i < affectEffectProperty.hitNumber; i++)
					{
						SubStatePlayHitAnimationAction.Data data = new SubStatePlayHitAnimationAction.Data();
						foreach (CharacterStateControl characterStateControl in array)
						{
							if (!characterStateControl.isDied)
							{
								SkillResults skillResults = SkillStatus.GetSkillResults(affectEffectProperty, actor, characterStateControl, base.stateManager.hierarchyData.onEnableRandomValue, 0);
								if (!skillResults.onMissHit)
								{
									if (skillResults.targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.CountBarrier))
									{
										this.sufferStatePropertyCounter.AddCountDictionary(SufferStateProperty.SufferType.CountBarrier, skillResults.targetCharacter);
									}
									else if (skillResults.targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.CountEvasion))
									{
										this.sufferStatePropertyCounter.AddCountDictionary(SufferStateProperty.SufferType.CountEvasion, skillResults.targetCharacter);
									}
									else if (skillResults.targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.CountGuard))
									{
										this.sufferStatePropertyCounter.AddCountDictionary(SufferStateProperty.SufferType.CountGuard, skillResults.targetCharacter);
									}
								}
								data.AddHitIcon(characterStateControl, skillResults.hitIconAffectEffect, skillResults.attackPower, skillResults.onWeakHit, skillResults.onMissHit, skillResults.onCriticalHit, false, false, false, skillResults.extraEffectType);
							}
						}
						IEnumerable<SubStatePlayHitAnimationAction.Data.HitIcon> enumerable = data.hitIconList.Where((SubStatePlayHitAnimationAction.Data.HitIcon item) => item.target.isDied);
						if (enumerable != null)
						{
							foreach (SubStatePlayHitAnimationAction.Data.HitIcon hitIcon in enumerable)
							{
								if (!this.everySkillLog.deadList.Contains(hitIcon.target))
								{
									this.everySkillLog.deadList.Add(hitIcon.target);
								}
							}
						}
						data.affectEffectProperty = affectEffectProperty;
						data.time = base.stateManager.stateProperty.multiHitIntervalWaitSecond;
						this.everySkillLog.skillResultList.Add(data);
					}
				}
				else
				{
					SubStatePlayHitAnimationAction.Data data2 = new SubStatePlayHitAnimationAction.Data();
					foreach (CharacterStateControl characterStateControl2 in array)
					{
						if (!characterStateControl2.isDied)
						{
							SkillResults otherSkillResult = base.stateManager.skillDetails.GetOtherSkillResult(affectEffectProperty, actor, characterStateControl2);
							data2.AddHitIcon(characterStateControl2, otherSkillResult.hitIconAffectEffect, otherSkillResult.attackPower, otherSkillResult.onWeakHit, otherSkillResult.onMissHit, otherSkillResult.onCriticalHit, false, false, false, otherSkillResult.extraEffectType);
						}
					}
					IEnumerable<SubStatePlayHitAnimationAction.Data.HitIcon> enumerable2 = data2.hitIconList.Where((SubStatePlayHitAnimationAction.Data.HitIcon item) => item.target.isDied);
					if (enumerable2 != null)
					{
						foreach (SubStatePlayHitAnimationAction.Data.HitIcon hitIcon2 in enumerable2)
						{
							if (!this.everySkillLog.deadList.Contains(hitIcon2.target))
							{
								this.everySkillLog.deadList.Add(hitIcon2.target);
							}
						}
					}
					data2.affectEffectProperty = affectEffectProperty;
					data2.time = base.stateManager.stateProperty.multiHitIntervalWaitSecond;
					this.everySkillLog.skillResultList.Add(data2);
				}
			}
		}
		actor.everySkillList[effectTriggerType].Clear();
	}

	private IEnumerator PLayEverySkillAnimation()
	{
		this.MargeEverySkill();
		foreach (SubStatePlayHitAnimationAction.Data data in this.everySkillLog.skillResultList)
		{
			CharacterStateControl[] targets = data.hitIconList.Select((SubStatePlayHitAnimationAction.Data.HitIcon item) => item.target).ToArray<CharacterStateControl>();
			yield return base.stateManager.threeDAction.MotionResetAliveCharacterAction(targets);
			yield return this.ShowDigimonAndCamera(targets);
			this.subStatePlayHitAnimationAction.Init(data);
			base.SetState(this.subStatePlayHitAnimationAction.GetType());
			while (base.isWaitState)
			{
				yield return null;
			}
			CharacterStateControl[] deads = this.everySkillLog.deadList.FindAll(new Predicate<CharacterStateControl>(targets.Contains<CharacterStateControl>)).ToArray();
			if (deads.Length > 0)
			{
				yield return this.DropItem(deads);
			}
			yield return this.SkillEndFunction();
		}
		yield break;
	}

	private IEnumerator SendLog(CharacterStateControl currentCharacter)
	{
		List<BattleLogData.AttackLog> attackLogList = new List<BattleLogData.AttackLog>();
		List<BattleLogData.BuffLog> buffLogList = new List<BattleLogData.BuffLog>();
		SkillResults[] attackDatas = this.cacheTargetDataList.Where((SkillResults item) => item.attackCharacter == currentCharacter).ToArray<SkillResults>();
		foreach (SkillResults data in attackDatas)
		{
			if (data.hitIconAffectEffect == AffectEffect.Damage)
			{
				BattleLogData.AttackLog attackLog = attackLogList.Where((BattleLogData.AttackLog item) => item.index == data.targetCharacter.myIndex).FirstOrDefault<BattleLogData.AttackLog>();
				if (attackLog != null)
				{
					attackLog.index = data.targetCharacter.myIndex;
					attackLog.damage.Add(data.attackPower);
					attackLog.miss.Add(data.onMissHit);
					attackLog.critical.Add(data.onCriticalHit);
					attackLog.isDead = data.targetCharacter.isDied;
				}
				else
				{
					attackLog = new BattleLogData.AttackLog();
					attackLog.index = data.targetCharacter.myIndex;
					attackLog.damage = new List<int>
					{
						data.attackPower
					};
					attackLog.miss = new List<bool>
					{
						data.onMissHit
					};
					attackLog.critical = new List<bool>
					{
						data.onCriticalHit
					};
					attackLog.isDead = data.targetCharacter.isDied;
					if (base.battleStateData.IsChipSkill())
					{
						attackLog.startUpChip = new List<int>();
						foreach (KeyValuePair<int, int> stagingChip in currentCharacter.stagingChipIdList)
						{
							if (!attackLog.startUpChip.Contains(stagingChip.Key))
							{
								attackLog.startUpChip.Add(stagingChip.Key);
							}
						}
					}
					attackLogList.Add(attackLog);
				}
			}
			else
			{
				BattleLogData.BuffLog buffLog = new BattleLogData.BuffLog();
				buffLog.index = data.targetCharacter.myIndex;
				buffLog.miss = data.onMissHit;
				buffLog.effectType = data.hitIconAffectEffect;
				buffLog.value = data.attackPower;
				if (base.battleStateData.IsChipSkill())
				{
					buffLog.chipUserIndex = currentCharacter.myIndex;
					buffLog.startUpChip = new List<int>();
					foreach (KeyValuePair<int, int> stagingChip2 in currentCharacter.stagingChipIdList)
					{
						if (!buffLog.startUpChip.Contains(stagingChip2.Key))
						{
							buffLog.startUpChip.Add(stagingChip2.Key);
						}
					}
				}
				buffLogList.Add(buffLog);
			}
		}
		IEnumerator logs = this.SendBattleLogs(currentCharacter, attackLogList, buffLogList);
		while (logs.MoveNext())
		{
			object obj = logs.Current;
			yield return obj;
		}
		yield break;
	}

	protected virtual IEnumerator SendBattleLogs(CharacterStateControl currentCharacter, List<BattleLogData.AttackLog> attackLog, List<BattleLogData.BuffLog> buffLog)
	{
		yield break;
	}

	private void MargeEverySkill()
	{
		List<SubStateSkillDetailsFunction.MargeData> list = new List<SubStateSkillDetailsFunction.MargeData>();
		List<SubStatePlayHitAnimationAction.Data> list2 = new List<SubStatePlayHitAnimationAction.Data>();
		SubStatePlayHitAnimationAction.Data skillResult;
		foreach (SubStatePlayHitAnimationAction.Data skillResult2 in this.everySkillLog.skillResultList)
		{
			skillResult = skillResult2;
			SubStatePlayHitAnimationAction.Data.HitIcon hitIcon;
			foreach (SubStatePlayHitAnimationAction.Data.HitIcon hitIcon3 in skillResult.hitIconList)
			{
				hitIcon = hitIcon3;
				SubStateSkillDetailsFunction.MargeData margeData = list.Where((SubStateSkillDetailsFunction.MargeData item) => item.target == hitIcon.target && item.affectEffectProperty != null && item.affectEffectProperty.type == skillResult.affectEffectProperty.type).SingleOrDefault<SubStateSkillDetailsFunction.MargeData>();
				if (margeData == null)
				{
					list.Add(new SubStateSkillDetailsFunction.MargeData
					{
						target = hitIcon.target,
						affectEffectProperty = skillResult.affectEffectProperty,
						hitIconList = 
						{
							hitIcon
						}
					});
				}
				else
				{
					margeData.hitIconList.Add(hitIcon);
				}
			}
		}
		IEnumerable<IGrouping<CharacterStateControl, SubStateSkillDetailsFunction.MargeData>> enumerable = list.GroupBy((SubStateSkillDetailsFunction.MargeData item) => item.target);
		foreach (IGrouping<CharacterStateControl, SubStateSkillDetailsFunction.MargeData> grouping in enumerable)
		{
			foreach (SubStateSkillDetailsFunction.MargeData margeData2 in grouping)
			{
				List<SubStatePlayHitAnimationAction.Data.HitIcon> list3 = new List<SubStatePlayHitAnimationAction.Data.HitIcon>();
				List<SubStatePlayHitAnimationAction.Data.HitIcon> list4 = new List<SubStatePlayHitAnimationAction.Data.HitIcon>();
				List<SubStatePlayHitAnimationAction.Data.HitIcon> list5 = new List<SubStatePlayHitAnimationAction.Data.HitIcon>();
				foreach (SubStatePlayHitAnimationAction.Data.HitIcon hitIcon2 in margeData2.hitIconList)
				{
					if (AffectEffectProperty.IsDamage(hitIcon2.affectEffect))
					{
						if (!margeData2.target.isDied || this.everySkillLog.deadList.Contains(margeData2.target))
						{
							list3.Add(hitIcon2);
						}
					}
					else if (hitIcon2.affectEffect == AffectEffect.HpRevival)
					{
						if (!hitIcon2.target.isDied)
						{
							list4.Add(hitIcon2);
						}
					}
					else if (!hitIcon2.target.isDied)
					{
						list5.Add(hitIcon2);
					}
				}
				SubStatePlayHitAnimationAction.Data data = new SubStatePlayHitAnimationAction.Data();
				if (list3.Count > 0 || list4.Count > 0)
				{
					int num = list3.Sum((SubStatePlayHitAnimationAction.Data.HitIcon item) => item.damage);
					int num2 = list4.Sum((SubStatePlayHitAnimationAction.Data.HitIcon item) => item.damage);
					int num3 = num - num2;
					AffectEffect affectEffect;
					if (num3 > 0)
					{
						affectEffect = AffectEffect.Damage;
					}
					else
					{
						affectEffect = AffectEffect.HpRevival;
					}
					data.AddHitIcon(margeData2.target, affectEffect, Mathf.Abs(num3), Strength.None, false, false, false, false, false, ExtraEffectType.Non);
				}
				SubStatePlayHitAnimationAction.Data.HitIcon other;
				foreach (SubStatePlayHitAnimationAction.Data.HitIcon other2 in list5)
				{
					other = other2;
					if (!data.hitIconList.Where((SubStatePlayHitAnimationAction.Data.HitIcon item) => item.affectEffect == other.affectEffect).Any<SubStatePlayHitAnimationAction.Data.HitIcon>())
					{
						data.AddHitIcon(other.target, other.affectEffect, other.damage, other.strength, other.isMiss, other.isCritical, other.isDrain, other.isCounter, other.isReflection, other.extraEffectType);
					}
				}
				data.affectEffectProperty = margeData2.affectEffectProperty;
				data.time = base.stateManager.stateProperty.multiHitIntervalWaitSecond;
				list2.Add(data);
			}
		}
		this.everySkillLog.skillResultList = list2;
	}

	protected class TargetData
	{
		public CharacterStateControl target;

		public bool isAllMiss = true;

		public bool onProtect;

		public bool isDamage;
	}

	private class EverySkillLog
	{
		public List<CharacterStateControl> deadList = new List<CharacterStateControl>();

		public List<SubStatePlayHitAnimationAction.Data> skillResultList = new List<SubStatePlayHitAnimationAction.Data>();
	}

	private class MargeData
	{
		public CharacterStateControl target;

		public AffectEffectProperty affectEffectProperty;

		public List<SubStatePlayHitAnimationAction.Data.HitIcon> hitIconList = new List<SubStatePlayHitAnimationAction.Data.HitIcon>();
	}
}
