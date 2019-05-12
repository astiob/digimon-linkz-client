using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SubStateSkillDetailsFunction : BattleStateController
{
	private Dictionary<SufferStateProperty.SufferType, List<CharacterStateControl>> countDictionary;

	private bool isBigBoss;

	private string cameraKey = string.Empty;

	private List<SubStateSkillDetailsFunction.TargetData> cacheTargetDataList;

	public SubStateSkillDetailsFunction(Action OnExit, Action<EventState> OnExitGotEvent) : base(null, OnExit, OnExitGotEvent)
	{
	}

	private void SetTargetLog(CharacterStateControl target, bool isMiss, bool isDamage)
	{
		SubStateSkillDetailsFunction.TargetData targetData = new SubStateSkillDetailsFunction.TargetData();
		targetData.target = target;
		targetData.isMiss = isMiss;
		targetData.isDamage = isDamage;
		this.cacheTargetDataList.Add(targetData);
	}

	protected override void AwakeThisState()
	{
		base.AddState(new SubStatePlayHitAnimationAction(null, new Action<EventState>(base.SendEventState)));
		base.AddState(new SubStateEnemiesItemDroppingFunction(null, new Action<EventState>(base.SendEventState)));
		base.AddState(new SubStatePlayPassiveEffectFunction(null, new Action<EventState>(base.SendEventState)));
		base.AddState(new SubStatePlayChipEffect(null, new Action<EventState>(base.SendEventState), null));
		base.AddState(new SubStateWaitForCertainPeriodTimeAction(null, new Action<EventState>(base.SendEventState)));
	}

	protected override void EnabledThisState()
	{
		this.countDictionary = new Dictionary<SufferStateProperty.SufferType, List<CharacterStateControl>>();
		this.cacheTargetDataList = new List<SubStateSkillDetailsFunction.TargetData>();
	}

	protected override IEnumerator MainRoutine()
	{
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
		bool enableDrawProtectMessage = true;
		if (currentCharacter.targetCharacter.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Protect))
		{
			enableDrawProtectMessage = false;
		}
		List<SubStateSkillDetailsFunction.TargetData> targetDataList = new List<SubStateSkillDetailsFunction.TargetData>();
		AffectEffectProperty lastSuffer = null;
		for (int i = 0; i < status.affectEffect.Count; i++)
		{
			AffectEffectProperty currentSuffer = status.affectEffect[i];
			if (lastSuffer != null && lastSuffer.target != currentSuffer.target)
			{
				CharacterStateControl[] target = base.stateManager.targetSelect.GetSkillTargetList(currentCharacter, currentSuffer.target);
				if (target != null && target.Length > 0)
				{
					currentCharacter.targetCharacter = target[0];
				}
			}
			lastSuffer = currentSuffer;
			targetDataList = this.CreateTargetData(targetDataList, currentCharacter, currentSuffer, isProtectEnableSkill);
			if (targetDataList.Count == 0)
			{
				break;
			}
			if (i != 0)
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
			this.PlayCamera(targetDataList.Select((SubStateSkillDetailsFunction.TargetData item) => item.target).ToArray<CharacterStateControl>());
			this.ShowDigimon(targetDataList[0].target);
			yield return null;
			base.stateManager.uiControl.HideCharacterHUDFunction();
			if (i == 0)
			{
				base.battleStateData.SetPlayPassiveEffectFunctionValues(targetDataList.Select((SubStateSkillDetailsFunction.TargetData item) => item.target).ToArray<CharacterStateControl>(), status, currentSuffer);
				base.SetState(typeof(SubStatePlayPassiveEffectFunction));
				while (base.isWaitState)
				{
					yield return null;
				}
			}
			if (currentCharacter.isSelectSkill > 0)
			{
				currentCharacter.hate += currentSuffer.GetHate() * targetDataList.Count;
			}
			if (enableDrawProtectMessage)
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
			if (AffectEffectProperty.IsDamage(currentSuffer.type))
			{
				IEnumerator function = this.AffectEffectDamage(currentCharacter, targetDataList, currentSuffer, status, i);
				while (function.MoveNext())
				{
					object obj3 = function.Current;
					yield return obj3;
				}
			}
			else if (currentSuffer.type == AffectEffect.Destruct)
			{
				IEnumerator function2 = this.AffectEffectDestruct(currentCharacter, targetDataList, currentSuffer);
				while (function2.MoveNext())
				{
					object obj4 = function2.Current;
					yield return obj4;
				}
			}
			else if (currentSuffer.type == AffectEffect.ApDrain)
			{
				IEnumerator function3 = this.AffectEffectApDrain(currentCharacter, targetDataList, currentSuffer);
				while (function3.MoveNext())
				{
					object obj5 = function3.Current;
					yield return obj5;
				}
			}
			else if (Tolerance.OnInfluenceToleranceAffectEffect(currentSuffer.type))
			{
				IEnumerator function4 = this.ToleranceOnInfluenceToleranceAffectEffect(currentCharacter, targetDataList, currentSuffer);
				while (function4.MoveNext())
				{
					object obj6 = function4.Current;
					yield return obj6;
				}
			}
			else
			{
				IEnumerator function5 = this.Other(currentCharacter, targetDataList, currentSuffer);
				while (function5.MoveNext())
				{
					object obj7 = function5.Current;
					yield return obj7;
				}
			}
			if (enableDrawProtectMessage)
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
			IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.skillAfterWaitSecond, null, null);
			while (wait.MoveNext())
			{
				object obj8 = wait.Current;
				yield return obj8;
			}
			base.battleStateData.SEStopFunctionCall();
			while (base.battleStateData.StopHitAnimationCall())
			{
				yield return null;
			}
			base.stateManager.threeDAction.HideDeadCharactersAction(base.battleStateData.GetTotalCharacters());
			this.StopCamera();
			base.stateManager.uiControl.HideCharacterHUDFunction();
			base.stateManager.uiControl.ApplyHideHitIcon();
			if (base.stateManager.IsLastBattleAndAllDeath())
			{
				break;
			}
		}
		this.OnChipTriggerForSufferHit();
		if (!base.battleStateData.IsChipSkill())
		{
			foreach (SubStateSkillDetailsFunction.TargetData targetData3 in this.cacheTargetDataList)
			{
				if (!targetData3.isMiss && !currentCharacter.isDied && targetData3.isDamage)
				{
					currentCharacter.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillDamageEndSend);
					break;
				}
			}
			foreach (SubStateSkillDetailsFunction.TargetData targetData4 in this.cacheTargetDataList)
			{
				if (!targetData4.isMiss && !targetData4.target.isDied && targetData4.isDamage)
				{
					targetData4.target.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillDamageEndRecieve);
				}
			}
			if (this.cacheTargetDataList.Where((SubStateSkillDetailsFunction.TargetData item) => item.isMiss).Any<SubStateSkillDetailsFunction.TargetData>() && !currentCharacter.isDied)
			{
				currentCharacter.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillMiss);
			}
		}
		if (currentCharacter.currentSkillStatus != null)
		{
			currentCharacter.currentSkillStatus.ClearAffectEffect();
		}
		foreach (CharacterStateControl character in base.battleStateData.GetTotalCharacters())
		{
			character.ClearGutsData();
		}
		this.UpdateCount(SufferStateProperty.SufferType.CountGuard);
		this.UpdateCount(SufferStateProperty.SufferType.CountBarrier);
		this.UpdateCount(SufferStateProperty.SufferType.CountEvasion);
		CharacterStateControl[] showList = base.battleStateData.GetTotalCharacters().Where((CharacterStateControl item) => !item.isDied).ToArray<CharacterStateControl>();
		base.stateManager.threeDAction.ShowAllCharactersAction(showList);
		base.stateManager.threeDAction.PlayIdleAnimationActiveCharacterAction(showList);
		yield break;
	}

	private void OnChipTriggerForSufferHit()
	{
		CharacterStateControl[] array;
		if (base.stateManager.battleMode == BattleMode.PvP)
		{
			if (base.stateManager.pvpFunction.IsOwner)
			{
				array = base.battleStateData.GetTotalCharacters();
			}
			else
			{
				array = base.battleStateData.GetTotalCharactersEnemyFirst();
			}
		}
		else
		{
			array = base.battleStateData.GetTotalCharacters();
		}
		foreach (CharacterStateControl characterStateControl in array)
		{
			if (characterStateControl.hitSufferList.Count > 0)
			{
				characterStateControl.OnChipTrigger(EffectStatusBase.EffectTriggerType.SufferHit);
			}
		}
		foreach (CharacterStateControl characterStateControl2 in array)
		{
			characterStateControl2.hitSufferList.Clear();
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
					flag2 = !targetData.isMiss;
				}
				else
				{
					flag2 = oldTargetDataList.Where((SubStateSkillDetailsFunction.TargetData item) => !item.isMiss).Any<SubStateSkillDetailsFunction.TargetData>();
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
				isMiss = false,
				onProtect = flag,
				isDamage = isProtectEnableSkill
			});
		}
		return list;
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

	private IEnumerator AffectEffectDamage(CharacterStateControl currentCharacter, List<SubStateSkillDetailsFunction.TargetData> targetDataList, AffectEffectProperty currentSuffer, SkillStatus status, int currentSkillDetails)
	{
		int hate = 0;
		int totalDamage = 0;
		bool isApRevival = false;
		List<int[]> totalCounterReflectionDamage = new List<int[]>();
		AffectEffect counterReflectionAffectType = AffectEffect.Counter;
		List<CharacterStateControl> currentDeathCharacters = new List<CharacterStateControl>();
		List<BattleLogData.AttackLog> attackLog = new List<BattleLogData.AttackLog>();
		List<BattleLogData.BuffLog> buffLog = new List<BattleLogData.BuffLog>();
		bool isAllTargetDeathBreak = false;
		for (int hitNumber = 0; hitNumber < currentSuffer.hitNumber; hitNumber++)
		{
			List<SkillResults> skillResults = new List<SkillResults>();
			List<HitIcon> hitIconlist = new List<HitIcon>();
			totalCounterReflectionDamage.Add(new int[targetDataList.Count]);
			for (int i = 0; i < targetDataList.Count; i++)
			{
				CharacterStateControl target = targetDataList[i].target;
				if (!target.isDied)
				{
					SkillResults atp = status.OnAttack(currentSkillDetails, currentCharacter, target, base.hierarchyData.onEnableRandomValue);
					skillResults.Add(atp);
					if (hitNumber == 0)
					{
						targetDataList[i].isMiss = true;
					}
					if (!atp.onMissHit)
					{
						targetDataList[i].isMiss = false;
					}
					if (atp.onCriticalHit)
					{
						isApRevival = true;
						if (currentCharacter.isSelectSkill > 0)
						{
							hate += currentSuffer.GetHate();
						}
					}
					totalDamage += atp.attackPower;
					if (!target.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Sleep))
					{
						if (currentSuffer.techniqueType == TechniqueType.Physics)
						{
							if (target.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Counter))
							{
								int reflectDamage = target.currentSufferState.onCounter.GetReflectDamage(atp.attackPower);
								totalCounterReflectionDamage[hitNumber][i] = reflectDamage;
								counterReflectionAffectType = AffectEffect.Counter;
							}
						}
						else if (currentSuffer.techniqueType == TechniqueType.Special && target.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Reflection))
						{
							int reflectDamage2 = target.currentSufferState.onReflection.GetReflectDamage(atp.attackPower);
							totalCounterReflectionDamage[hitNumber][i] = reflectDamage2;
							counterReflectionAffectType = AffectEffect.Reflection;
						}
					}
					if (!atp.onMissHit && !target.currentSufferState.onTurnBarrier.isActive && !target.currentSufferState.onCountBarrier.isActive && !target.currentSufferState.onTurnEvasion.isActive && !target.currentSufferState.onCountEvasion.isActive)
					{
						this.CheckFraud(atp.attackPower, currentCharacter, target);
					}
					if (target.isDied && !currentDeathCharacters.Contains(target))
					{
						currentDeathCharacters.Add(target);
					}
					Vector3 hitIconPosition = base.stateManager.uiControl.GetFixableCharacterCenterPosition2DFunction(atp.targetCharacter);
					AffectEffect hitIconAffectEffect = AffectEffect.Damage;
					int damage = atp.attackPower;
					bool isCritical = atp.onCriticalHit;
					Strength isWeak = atp.onWeakHit;
					bool isMiss = atp.onMissHit;
					ExtraEffectType extraEffectType = atp.extraEffectType;
					if (atp.onWeakHit == Strength.Invalid)
					{
						hitIconAffectEffect = AffectEffect.Invalid;
						isMiss = false;
						isWeak = Strength.None;
					}
					else if (atp.targetCharacter.currentSufferState.onTurnBarrier.isActive)
					{
						hitIconAffectEffect = AffectEffect.TurnBarrier;
						isMiss = false;
						isWeak = Strength.None;
					}
					else if (atp.targetCharacter.currentSufferState.onCountBarrier.isActive)
					{
						hitIconAffectEffect = AffectEffect.CountBarrier;
						isMiss = false;
						isWeak = Strength.None;
					}
					else if (atp.targetCharacter.currentSufferState.onTurnEvasion.isActive)
					{
						hitIconAffectEffect = AffectEffect.TurnEvasion;
						isMiss = false;
						isWeak = Strength.None;
					}
					else if (atp.targetCharacter.currentSufferState.onCountEvasion.isActive)
					{
						hitIconAffectEffect = AffectEffect.CountEvasion;
						isMiss = false;
						isWeak = Strength.None;
					}
					HitIcon hitIcon = base.stateManager.uiControl.ApplyShowHitIcon(i, hitIconPosition, hitIconAffectEffect, damage, isWeak, isMiss, isCritical, false, false, extraEffectType);
					hitIconlist.Add(hitIcon);
					this.SetTargetLog(atp.targetCharacter, isMiss, true);
					BattleLogData.AttackLog log = attackLog.FirstOrDefault((BattleLogData.AttackLog x) => x.index == atp.targetCharacter.myIndex);
					if (log == null)
					{
						List<int> _startUpChip = new List<int>();
						foreach (int chipEffectId in atp.targetCharacter.stagingChipIdList.Keys)
						{
							if (_startUpChip.IndexOf(chipEffectId) < 0)
							{
								_startUpChip.Add(chipEffectId);
							}
						}
						BattleLogData.AttackLog newLog = new BattleLogData.AttackLog
						{
							index = atp.targetCharacter.myIndex,
							damage = new List<int>
							{
								atp.attackPower
							},
							miss = new List<bool>
							{
								atp.onMissHit
							},
							critical = new List<bool>
							{
								atp.onCriticalHit
							},
							isDead = atp.targetCharacter.isDied,
							startUpChip = _startUpChip
						};
						attackLog.Add(newLog);
					}
					else
					{
						log.damage.Add(atp.attackPower);
						log.miss.Add(atp.onMissHit);
						log.critical.Add(atp.onCriticalHit);
						log.isDead = atp.targetCharacter.isDied;
						List<int> _startUpChip2 = new List<int>();
						foreach (int chipEffectId2 in atp.targetCharacter.stagingChipIdList.Keys)
						{
							if (_startUpChip2.IndexOf(chipEffectId2) < 0)
							{
								_startUpChip2.Add(chipEffectId2);
							}
						}
						log.startUpChip = _startUpChip2;
					}
				}
			}
			this.AfterEnemyDeadFunction(currentDeathCharacters.ToArray());
			CharacterStateControl[] skillResultTargets = skillResults.Select((SkillResults item) => item.targetCharacter).ToArray<CharacterStateControl>();
			bool[] skillResultMissHits = skillResults.Select((SkillResults item) => item.onMissHit).ToArray<bool>();
			ExtraEffectType[] skillResultExtraEffectTypes = skillResults.Select((SkillResults item) => item.extraEffectType).ToArray<ExtraEffectType>();
			bool isAllTargetDeath = true;
			foreach (CharacterStateControl skillResultTarget in skillResultTargets)
			{
				if (!skillResultTarget.isDied)
				{
					isAllTargetDeath = false;
					break;
				}
			}
			this.ApplyMonsterIconEnabled(currentCharacter);
			if (!base.stateManager.IsLastBattleAndAllDeath())
			{
				base.stateManager.cameraControl.PlayCameraShake();
			}
			int hitLength = currentSuffer.hitNumber;
			foreach (SubStateSkillDetailsFunction.TargetData targetData in targetDataList)
			{
				if (targetData.onProtect)
				{
					hitLength = Mathf.Max(targetDataList.Count, currentSuffer.hitNumber);
					break;
				}
			}
			float interval = 0f;
			if (isAllTargetDeath)
			{
				interval = base.stateManager.stateProperty.multiHitIntervalWaitSecond;
			}
			else
			{
				interval = base.stateManager.stateProperty.multiHitIntervalWaitSecond / (float)hitLength;
			}
			base.battleStateData.SetPlayAnimationActionValues(currentCharacter, skillResultTargets, currentSuffer.type, interval, skillResultMissHits, hitIconlist.ToArray(), skillResultExtraEffectTypes, currentSuffer, true);
			base.SetState(typeof(SubStatePlayHitAnimationAction));
			while (base.isWaitState)
			{
				yield return null;
			}
			foreach (SkillResults skillResult in skillResults)
			{
				if (skillResult.targetCharacter.currentSufferState.onCountBarrier.isActive)
				{
					this.AddCountDictionary(SufferStateProperty.SufferType.CountBarrier, skillResult.targetCharacter);
				}
				else if (skillResult.targetCharacter.currentSufferState.onCountEvasion.isActive)
				{
					this.AddCountDictionary(SufferStateProperty.SufferType.CountEvasion, skillResult.targetCharacter);
				}
				else if (skillResult.targetCharacter.currentSufferState.onCountGuard.isActive)
				{
					this.AddCountDictionary(SufferStateProperty.SufferType.CountGuard, skillResult.targetCharacter);
				}
			}
			if (currentSuffer.hitNumber > 1)
			{
				IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(0.25f, null, null);
				while (wait.MoveNext())
				{
					yield return null;
				}
			}
			if (isAllTargetDeath)
			{
				isAllTargetDeathBreak = true;
				break;
			}
		}
		if (currentDeathCharacters.Count > 0)
		{
			currentCharacter.OnChipTrigger(EffectStatusBase.EffectTriggerType.Kill);
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
		IEnumerator function = this.DropItem(currentDeathCharacters.ToArray());
		while (function.MoveNext())
		{
			object obj = function.Current;
			yield return obj;
		}
		if (currentSuffer.useDrain && !currentCharacter.isDied)
		{
			BattleLogData.BuffLog drainLog = new BattleLogData.BuffLog
			{
				effectType = AffectEffect.HpRevival,
				value = Mathf.FloorToInt((float)totalDamage * currentSuffer.revivalPercent)
			};
			buffLog.Add(drainLog);
			IEnumerator function2 = this.DrainRecovery(currentCharacter, currentSuffer, totalDamage);
			while (function2.MoveNext())
			{
				object obj2 = function2.Current;
				yield return obj2;
			}
		}
		CharacterStateControl[] targets = targetDataList.Select((SubStateSkillDetailsFunction.TargetData item) => item.target).ToArray<CharacterStateControl>();
		bool[] isMisses = targetDataList.Select((SubStateSkillDetailsFunction.TargetData item) => item.isMiss).ToArray<bool>();
		base.stateManager.skillDetails.GetSleepWakeUp(targets, isMisses);
		List<int> drawDamageList = new List<int>();
		for (int j = 0; j < totalCounterReflectionDamage.Count; j++)
		{
			int total = 0;
			for (int k = 0; k < targetDataList.Count; k++)
			{
				if (!targetDataList[k].target.isDied)
				{
					total += totalCounterReflectionDamage[j][k];
				}
			}
			if (total > 0)
			{
				drawDamageList.Add(total);
			}
		}
		if (drawDamageList.Count > 0 && !currentCharacter.isDied)
		{
			BattleLogData.BuffLog reflectionLog = new BattleLogData.BuffLog();
			reflectionLog.effectType = AffectEffect.Reflection;
			foreach (int drawDamage in drawDamageList)
			{
				reflectionLog.value += drawDamage;
			}
			buffLog.Add(reflectionLog);
			IEnumerator function3 = this.TotalCounterReflectionDamage(currentCharacter, currentSuffer, counterReflectionAffectType, drawDamageList);
			while (function3.MoveNext())
			{
				object obj3 = function3.Current;
				yield return obj3;
			}
		}
		currentCharacter.hate += hate;
		base.battleStateData.SetApRevival(currentCharacter, isApRevival);
		IEnumerator logs = this.SendBattleLogs(currentCharacter, attackLog, buffLog);
		while (logs.MoveNext())
		{
			object obj4 = logs.Current;
			yield return obj4;
		}
		yield break;
	}

	protected virtual void AfterEnemyDeadFunction(params CharacterStateControl[] currentDeathCharacters)
	{
		base.stateManager.deadOrAlive.AfterEnemyDeadFunction(currentDeathCharacters);
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
		base.battleStateData.currentDeadCharacters = currentDeadCharacters;
		base.SetState(typeof(SubStateEnemiesItemDroppingFunction));
		while (base.isWaitState)
		{
			yield return null;
		}
		yield break;
	}

	protected virtual IEnumerator SendBattleLogs(CharacterStateControl currentCharacter, List<BattleLogData.AttackLog> attackLog, List<BattleLogData.BuffLog> buffLog)
	{
		yield break;
	}

	private IEnumerator DrainRecovery(CharacterStateControl currentCharacter, AffectEffectProperty currentSuffer, int totalDamage)
	{
		IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.skillAfterWaitSecond, null, null);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		base.stateManager.threeDAction.HideDeadCharactersAction(base.battleStateData.GetTotalCharacters());
		this.PlayCamera(new CharacterStateControl[]
		{
			currentCharacter
		});
		int recoveryDamage = Mathf.FloorToInt((float)totalDamage * currentSuffer.revivalPercent);
		currentCharacter.hp += recoveryDamage;
		HitIcon hitIcon = base.stateManager.uiControl.ApplyShowHitIcon(0, base.stateManager.uiControl.GetFixableCharacterCenterPosition2DFunction(currentCharacter), currentSuffer.type, recoveryDamage, Strength.None, false, false, true, true, ExtraEffectType.Non);
		base.battleStateData.SetPlayAnimationActionValues(currentCharacter, new CharacterStateControl[]
		{
			currentCharacter
		}, AffectEffect.HpRevival, base.stateManager.stateProperty.attackerCharacterDrainActionWaitSecond, new bool[1], new HitIcon[]
		{
			hitIcon
		}, new ExtraEffectType[1], currentSuffer, false);
		base.SetState(typeof(SubStatePlayHitAnimationAction));
		while (base.isWaitState)
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator TotalCounterReflectionDamage(CharacterStateControl currentCharacter, AffectEffectProperty currentSuffer, AffectEffect counterReflectionAffectType, List<int> totalCounterReflectionDamage)
	{
		IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.skillAfterWaitSecond, null, null);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		wait = base.stateManager.threeDAction.MotionResetAliveCharacterAction(new CharacterStateControl[]
		{
			currentCharacter
		});
		while (wait.MoveNext())
		{
			object obj2 = wait.Current;
			yield return obj2;
		}
		this.PlayCamera(new CharacterStateControl[]
		{
			currentCharacter
		});
		for (int i = 0; i < totalCounterReflectionDamage.Count; i++)
		{
			if (currentCharacter.isDied)
			{
				break;
			}
			AffectEffect affectEffect = counterReflectionAffectType;
			if (currentCharacter.currentSufferState.onTurnBarrier.isActive)
			{
				affectEffect = AffectEffect.TurnBarrier;
			}
			else if (currentCharacter.currentSufferState.onCountBarrier.isActive)
			{
				affectEffect = AffectEffect.CountBarrier;
			}
			else if (currentCharacter.currentSufferState.onTurnEvasion.isActive)
			{
				affectEffect = AffectEffect.TurnEvasion;
			}
			else if (currentCharacter.currentSufferState.onCountEvasion.isActive)
			{
				affectEffect = AffectEffect.CountEvasion;
			}
			if (affectEffect == counterReflectionAffectType)
			{
				currentCharacter.hp -= totalCounterReflectionDamage[i];
			}
			HitIcon hitIcon = base.stateManager.uiControl.ApplyShowHitIcon(i, base.stateManager.uiControl.GetFixableCharacterCenterPosition2DFunction(currentCharacter), affectEffect, totalCounterReflectionDamage[i], Strength.None, false, false, false, true, ExtraEffectType.Non);
			this.SetTargetLog(currentCharacter, false, false);
			base.battleStateData.SetPlayAnimationActionValues(currentCharacter, new CharacterStateControl[]
			{
				currentCharacter
			}, AffectEffect.Damage, base.stateManager.stateProperty.targetCounterReflectionActionWaitSecond / (float)totalCounterReflectionDamage.Count, new bool[1], new HitIcon[]
			{
				hitIcon
			}, new ExtraEffectType[1], currentSuffer, true);
			base.SetState(typeof(SubStatePlayHitAnimationAction));
			while (base.isWaitState)
			{
				yield return null;
			}
			if (currentCharacter.currentSufferState.onCountBarrier.isActive)
			{
				this.AddCountDictionary(SufferStateProperty.SufferType.CountBarrier, currentCharacter);
			}
			else if (currentCharacter.currentSufferState.onCountEvasion.isActive)
			{
				this.AddCountDictionary(SufferStateProperty.SufferType.CountEvasion, currentCharacter);
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
				object obj3 = dropItem.Current;
				yield return obj3;
			}
		}
		yield break;
	}

	private IEnumerator AffectEffectDestruct(CharacterStateControl currentCharacter, List<SubStateSkillDetailsFunction.TargetData> targetDataList, AffectEffectProperty currentSuffer)
	{
		this.PlayCamera(new CharacterStateControl[]
		{
			currentCharacter
		});
		base.stateManager.uiControl.HideCharacterHUDFunction();
		currentCharacter.Kill();
		this.AfterEnemyDeadFunction(new CharacterStateControl[]
		{
			currentCharacter
		});
		HitIcon selfHitIcon = base.stateManager.uiControl.ApplyShowHitIcon(0, base.stateManager.uiControl.GetFixableCharacterCenterPosition2DFunction(currentCharacter), currentSuffer.type, -1, Strength.None, false, false, false, false, ExtraEffectType.Non);
		base.battleStateData.SetPlayAnimationActionValues(currentCharacter, new CharacterStateControl[]
		{
			currentCharacter
		}, currentSuffer.type, base.stateManager.stateProperty.destructCharacterDeathActionWaitSecond, new bool[1], new HitIcon[]
		{
			selfHitIcon
		}, new ExtraEffectType[1], null, false);
		base.SetState(typeof(SubStatePlayHitAnimationAction));
		while (base.isWaitState)
		{
			yield return null;
		}
		IEnumerator function = this.DropItem(new CharacterStateControl[]
		{
			currentCharacter
		});
		while (function.MoveNext())
		{
			object obj = function.Current;
			yield return obj;
		}
		base.battleStateData.SEStopFunctionCall();
		this.PlayCamera(targetDataList.Select((SubStateSkillDetailsFunction.TargetData item) => item.target).ToArray<CharacterStateControl>());
		if (!targetDataList[0].target.isEnemy)
		{
			this.ShowRevivalEffect();
		}
		else
		{
			this.HideRevivalEffect();
		}
		List<CharacterStateControl> skillResultTargets = new List<CharacterStateControl>();
		List<bool> skillResultMisses = new List<bool>();
		List<HitIcon> hitIconList = new List<HitIcon>();
		List<CharacterStateControl> currentDeathCharacters = new List<CharacterStateControl>();
		List<ExtraEffectType> extraEffectTypeList = new List<ExtraEffectType>();
		for (int targets = 0; targets < targetDataList.Count; targets++)
		{
			targetDataList[targets].isMiss = true;
			if (!targetDataList[targets].target.isDied)
			{
				if (currentSuffer.OnHit(currentCharacter, targetDataList[targets].target))
				{
					targetDataList[targets].target.OnHitDestruct();
					targetDataList[targets].isMiss = false;
				}
				Vector3 hitIconPosition = base.stateManager.uiControl.GetFixableCharacterCenterPosition2DFunction(targetDataList[targets].target);
				bool isMiss = targetDataList[targets].isMiss;
				HitIcon hitIcon = base.stateManager.uiControl.ApplyShowHitIcon(targets, hitIconPosition, currentSuffer.type, -1, Strength.None, isMiss, false, false, false, ExtraEffectType.Non);
				hitIconList.Add(hitIcon);
				this.SetTargetLog(targetDataList[targets].target, targetDataList[targets].isMiss, false);
				skillResultTargets.Add(targetDataList[targets].target);
				skillResultMisses.Add(targetDataList[targets].isMiss);
				extraEffectTypeList.Add(ExtraEffectType.Non);
				if (targetDataList[targets].target.isDied)
				{
					currentDeathCharacters.Add(targetDataList[targets].target);
				}
			}
		}
		this.AfterEnemyDeadFunction(currentDeathCharacters.ToArray());
		base.stateManager.cameraControl.PlayCameraShake();
		base.stateManager.soundPlayer.StopHitEffectSE();
		base.battleStateData.SetPlayAnimationActionValues(currentCharacter, skillResultTargets.ToArray(), currentSuffer.type, base.stateManager.stateProperty.multiHitIntervalWaitSecond, skillResultMisses.ToArray(), hitIconList.ToArray(), extraEffectTypeList.ToArray(), null, false);
		base.SetState(typeof(SubStatePlayHitAnimationAction));
		while (base.isWaitState)
		{
			yield return null;
		}
		base.stateManager.cameraControl.StopCameraShake();
		if (this.isBigBoss)
		{
			IEnumerator transitionCount = base.stateManager.threeDAction.BigBossExitAction(currentCharacter);
			while (transitionCount.MoveNext())
			{
				yield return null;
			}
		}
		IEnumerator function2 = this.DropItem(currentDeathCharacters.ToArray());
		while (function2.MoveNext())
		{
			object obj2 = function2.Current;
			yield return obj2;
		}
		yield break;
	}

	protected virtual IEnumerator AffectEffectApDrain(CharacterStateControl currentCharacter, List<SubStateSkillDetailsFunction.TargetData> targetDataList, AffectEffectProperty currentSuffer)
	{
		List<BattleLogData.BuffLog> buffLog = new List<BattleLogData.BuffLog>();
		List<CharacterStateControl> skillResultTargets = new List<CharacterStateControl>();
		List<bool> skillResultMisses = new List<bool>();
		List<HitIcon> hitIconList = new List<HitIcon>();
		List<ExtraEffectType> extraEffectTypeList = new List<ExtraEffectType>();
		int totalApDrain = 0;
		for (int i = 0; i < targetDataList.Count; i++)
		{
			targetDataList[i].isMiss = true;
			if (!targetDataList[i].target.isDied)
			{
				if (!targetDataList[i].target.currentSufferState.onTurnBarrier.isActive)
				{
					if (!targetDataList[i].target.currentSufferState.onCountBarrier.isActive)
					{
						if (!targetDataList[i].target.currentSufferState.onTurnEvasion.isActive)
						{
							if (!targetDataList[i].target.currentSufferState.onCountEvasion.isActive)
							{
								if (currentSuffer.OnHit(currentCharacter, targetDataList[i].target))
								{
									targetDataList[i].isMiss = false;
									int lastAp = targetDataList[i].target.ap;
									targetDataList[i].target.ap -= currentSuffer.apDrainPower;
									totalApDrain += lastAp - targetDataList[i].target.ap;
								}
							}
						}
					}
				}
				Vector3 hitIconPosition = base.stateManager.uiControl.GetFixableCharacterCenterPosition2DFunction(targetDataList[i].target);
				HitIcon hitIcon = base.stateManager.uiControl.ApplyShowHitIcon(i, hitIconPosition, AffectEffect.ApDown, 0, Strength.None, targetDataList[i].isMiss, false, false, false, ExtraEffectType.Non);
				hitIconList.Add(hitIcon);
				this.SetTargetLog(targetDataList[i].target, targetDataList[i].isMiss, false);
				skillResultTargets.Add(targetDataList[i].target);
				skillResultMisses.Add(targetDataList[i].isMiss);
				extraEffectTypeList.Add(ExtraEffectType.Non);
				BattleLogData.BuffLog log = new BattleLogData.BuffLog
				{
					index = targetDataList[i].target.myIndex,
					miss = targetDataList[i].isMiss,
					effectType = currentSuffer.type
				};
				buffLog.Add(log);
			}
		}
		IEnumerator logs = this.SendBattleLogs(currentCharacter, null, buffLog);
		while (logs.MoveNext())
		{
			object obj = logs.Current;
			yield return obj;
		}
		base.battleStateData.SetPlayAnimationActionValues(currentCharacter, skillResultTargets.ToArray(), AffectEffect.ApDown, base.stateManager.stateProperty.multiHitIntervalWaitSecond, skillResultMisses.ToArray(), hitIconList.ToArray(), extraEffectTypeList.ToArray(), null, false);
		base.SetState(typeof(SubStatePlayHitAnimationAction));
		while (base.isWaitState)
		{
			yield return null;
		}
		for (int j = 0; j < targetDataList.Count; j++)
		{
			if (targetDataList[j].target.currentSufferState.onCountBarrier.isActive)
			{
				this.AddCountDictionary(SufferStateProperty.SufferType.CountBarrier, targetDataList[j].target);
			}
			else if (targetDataList[j].target.currentSufferState.onCountEvasion.isActive)
			{
				this.AddCountDictionary(SufferStateProperty.SufferType.CountEvasion, targetDataList[j].target);
			}
		}
		if (targetDataList.Where((SubStateSkillDetailsFunction.TargetData item) => !item.isMiss).Count<SubStateSkillDetailsFunction.TargetData>() > 0)
		{
			IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.skillAfterWaitSecond, null, null);
			while (wait.MoveNext())
			{
				object obj2 = wait.Current;
				yield return obj2;
			}
			wait = base.stateManager.threeDAction.MotionResetAliveCharacterAction(new CharacterStateControl[]
			{
				currentCharacter
			});
			while (wait.MoveNext())
			{
				object obj3 = wait.Current;
				yield return obj3;
			}
			this.PlayCamera(new CharacterStateControl[]
			{
				currentCharacter
			});
			currentCharacter.ap += totalApDrain;
			HitIcon hitIcon2 = base.stateManager.uiControl.ApplyShowHitIcon(0, base.stateManager.uiControl.GetFixableCharacterCenterPosition2DFunction(currentCharacter), currentSuffer.type, 0, Strength.None, false, false, true, true, ExtraEffectType.Non);
			base.battleStateData.SetPlayAnimationActionValues(currentCharacter, new CharacterStateControl[]
			{
				currentCharacter
			}, AffectEffect.ApDrain, base.stateManager.stateProperty.attackerCharacterDrainActionWaitSecond, new bool[1], new HitIcon[]
			{
				hitIcon2
			}, new ExtraEffectType[1], currentSuffer, false);
			base.SetState(typeof(SubStatePlayHitAnimationAction));
			while (base.isWaitState)
			{
				yield return null;
			}
		}
		yield break;
	}

	private IEnumerator ToleranceOnInfluenceToleranceAffectEffect(CharacterStateControl currentCharacter, List<SubStateSkillDetailsFunction.TargetData> targetDataList, AffectEffectProperty currentSuffer)
	{
		List<CharacterStateControl> instantDeathCharacters = new List<CharacterStateControl>();
		List<BattleLogData.BuffLog> buffLog = new List<BattleLogData.BuffLog>();
		for (int i = 0; i < targetDataList.Count; i++)
		{
			targetDataList[i].isMiss = true;
			if (!targetDataList[i].target.isDied)
			{
				Strength isWeak = Strength.None;
				isWeak = targetDataList[i].target.tolerance.GetAffectEffectStrength(currentSuffer.type);
				if (isWeak != Strength.Invalid)
				{
					if (!targetDataList[i].target.currentSufferState.onTurnBarrier.isActive)
					{
						if (!targetDataList[i].target.currentSufferState.onCountBarrier.isActive)
						{
							if (!targetDataList[i].target.currentSufferState.onTurnEvasion.isActive)
							{
								if (!targetDataList[i].target.currentSufferState.onCountEvasion.isActive)
								{
									if (currentSuffer.OnHit(currentCharacter, targetDataList[i].target))
									{
										targetDataList[i].isMiss = false;
										if (currentSuffer.type == AffectEffect.InstantDeath)
										{
											instantDeathCharacters.Add(targetDataList[i].target);
										}
										else
										{
											SufferStateProperty suffer = new SufferStateProperty(currentSuffer, base.battleStateData.currentLastGenerateStartTimingSufferState);
											targetDataList[i].target.currentSufferState.SetSufferState(suffer, currentCharacter);
											base.battleStateData.currentLastGenerateStartTimingSufferState++;
										}
									}
								}
							}
						}
					}
				}
				BattleLogData.BuffLog log = new BattleLogData.BuffLog
				{
					index = targetDataList[i].target.myIndex,
					miss = targetDataList[i].isMiss,
					effectType = currentSuffer.type
				};
				buffLog.Add(log);
			}
		}
		IEnumerator logs = this.SendBattleLogs(currentCharacter, null, buffLog);
		while (logs.MoveNext())
		{
			object obj = logs.Current;
			yield return obj;
		}
		List<CharacterStateControl> skillResultTargets = new List<CharacterStateControl>();
		List<bool> skillResultMisses = new List<bool>();
		List<HitIcon> hitIconList = new List<HitIcon>();
		List<ExtraEffectType> extraEffectTypeList = new List<ExtraEffectType>();
		for (int j = 0; j < targetDataList.Count; j++)
		{
			if (!targetDataList[j].target.isDied)
			{
				foreach (CharacterStateControl instantDeathCharacter in instantDeathCharacters)
				{
					if (instantDeathCharacter == targetDataList[j].target)
					{
						targetDataList[j].target.Kill();
						break;
					}
				}
				Vector3 hitIconPosition = base.stateManager.uiControl.GetFixableCharacterCenterPosition2DFunction(targetDataList[j].target);
				bool isMiss = targetDataList[j].isMiss;
				Strength isWeak2 = targetDataList[j].target.tolerance.GetAffectEffectStrength(currentSuffer.type);
				AffectEffect hitIconAffectEffect = currentSuffer.type;
				if (isWeak2 == Strength.Invalid)
				{
					hitIconAffectEffect = AffectEffect.Invalid;
					isMiss = false;
				}
				else if (targetDataList[j].target.currentSufferState.onTurnBarrier.isActive)
				{
					hitIconAffectEffect = AffectEffect.TurnBarrier;
					isMiss = false;
				}
				else if (targetDataList[j].target.currentSufferState.onCountBarrier.isActive)
				{
					hitIconAffectEffect = AffectEffect.CountBarrier;
					isMiss = false;
				}
				else if (targetDataList[j].target.currentSufferState.onTurnEvasion.isActive)
				{
					hitIconAffectEffect = AffectEffect.TurnEvasion;
					isMiss = false;
				}
				else if (targetDataList[j].target.currentSufferState.onCountEvasion.isActive)
				{
					hitIconAffectEffect = AffectEffect.CountEvasion;
					isMiss = false;
				}
				HitIcon hitIcon = base.stateManager.uiControl.ApplyShowHitIcon(j, hitIconPosition, hitIconAffectEffect, -1, Strength.None, isMiss, false, false, false, ExtraEffectType.Non);
				hitIconList.Add(hitIcon);
				this.SetTargetLog(targetDataList[j].target, isMiss, false);
				skillResultTargets.Add(targetDataList[j].target);
				skillResultMisses.Add(isMiss);
				extraEffectTypeList.Add(ExtraEffectType.Non);
			}
		}
		foreach (SubStateSkillDetailsFunction.TargetData targetData in targetDataList)
		{
			if (!targetData.isMiss && !base.battleStateData.IsChipSkill())
			{
				SufferStateProperty suffer2 = new SufferStateProperty(currentSuffer, 0);
				targetData.target.hitSufferList.Add(suffer2);
			}
		}
		if (instantDeathCharacters.Count > 0)
		{
			currentCharacter.OnChipTrigger(EffectStatusBase.EffectTriggerType.Kill);
		}
		this.AfterEnemyDeadFunction(instantDeathCharacters.ToArray());
		if (currentSuffer.type == AffectEffect.InstantDeath)
		{
			base.stateManager.cameraControl.PlayCameraShake();
		}
		base.battleStateData.SetPlayAnimationActionValues(currentCharacter, skillResultTargets.ToArray(), currentSuffer.type, base.stateManager.stateProperty.multiHitIntervalWaitSecond, skillResultMisses.ToArray(), hitIconList.ToArray(), extraEffectTypeList.ToArray(), null, false);
		base.SetState(typeof(SubStatePlayHitAnimationAction));
		while (base.isWaitState)
		{
			yield return null;
		}
		for (int k = 0; k < targetDataList.Count; k++)
		{
			if (targetDataList[k].target.currentSufferState.onCountBarrier.isActive)
			{
				this.AddCountDictionary(SufferStateProperty.SufferType.CountBarrier, targetDataList[k].target);
			}
			else if (targetDataList[k].target.currentSufferState.onCountEvasion.isActive)
			{
				this.AddCountDictionary(SufferStateProperty.SufferType.CountEvasion, targetDataList[k].target);
			}
		}
		if (currentSuffer.type == AffectEffect.InstantDeath)
		{
			base.stateManager.cameraControl.StopCameraShake();
		}
		IEnumerator function = this.DropItem(instantDeathCharacters.ToArray());
		while (function.MoveNext())
		{
			object obj2 = function.Current;
			yield return obj2;
		}
		yield break;
	}

	protected virtual IEnumerator Other(CharacterStateControl currentCharacter, List<SubStateSkillDetailsFunction.TargetData> targetDataList, AffectEffectProperty currentSuffer)
	{
		List<CharacterStateControl> deadCharacters = new List<CharacterStateControl>();
		int hate = 0;
		List<BattleLogData.BuffLog> buffLog = new List<BattleLogData.BuffLog>();
		List<CharacterStateControl> skillResultTargets = new List<CharacterStateControl>();
		List<bool> skillResultMisses = new List<bool>();
		List<HitIcon> hitIconList = new List<HitIcon>();
		List<ExtraEffectType> extraEffectTypeList = new List<ExtraEffectType>();
		for (int i = 0; i < targetDataList.Count; i++)
		{
			BattleLogData.BuffLog log = new BattleLogData.BuffLog();
			int hitIconDigit = -1;
			targetDataList[i].isMiss = true;
			if (!targetDataList[i].target.isDied)
			{
				if (currentSuffer.OnHit(currentCharacter, targetDataList[i].target))
				{
					AffectEffect type = currentSuffer.type;
					switch (type)
					{
					case AffectEffect.ApUp:
						base.stateManager.skillDetails.ApUp(targetDataList[i].target, currentSuffer);
						break;
					case AffectEffect.ApDown:
						base.stateManager.skillDetails.ApDown(targetDataList[i].target, currentSuffer);
						break;
					default:
						switch (type)
						{
						case AffectEffect.CorrectionUpReset:
							base.stateManager.skillDetails.CorrectionUpReset(targetDataList[i].target);
							break;
						case AffectEffect.CorrectionDownReset:
							base.stateManager.skillDetails.CorrectionDownReset(targetDataList[i].target);
							break;
						case AffectEffect.HpRevival:
							hitIconDigit = base.stateManager.skillDetails.HpRevival(targetDataList[i].target, currentSuffer);
							log.value = hitIconDigit;
							break;
						default:
							if (type != AffectEffect.SufferStatusClear)
							{
								base.stateManager.skillDetails.AddSufferStateOthers(targetDataList[i].target, currentSuffer);
							}
							else
							{
								base.stateManager.skillDetails.SufferStatusClear(targetDataList[i].target, currentSuffer);
							}
							break;
						case AffectEffect.HateUp:
							base.stateManager.skillDetails.HateUp(ref hate, currentSuffer);
							break;
						case AffectEffect.HateDown:
							base.stateManager.skillDetails.HateDown(ref hate, currentSuffer);
							break;
						case AffectEffect.PowerCharge:
						{
							SufferStateProperty suffer = new SufferStateProperty(currentSuffer, base.battleStateData.currentLastGenerateStartTimingSufferState);
							currentCharacter.currentSufferState.SetSufferState(suffer, null);
							base.battleStateData.currentLastGenerateStartTimingSufferState++;
							break;
						}
						}
						break;
					case AffectEffect.TurnBarrier:
						if (targetDataList[i].target.currentSufferState.onCountBarrier.isActive)
						{
							targetDataList[i].target.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.CountBarrier);
						}
						base.stateManager.skillDetails.AddSufferStateOthers(targetDataList[i].target, currentSuffer);
						break;
					case AffectEffect.CountBarrier:
						if (targetDataList[i].target.currentSufferState.onTurnBarrier.isActive)
						{
							targetDataList[i].target.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.TurnBarrier);
						}
						base.stateManager.skillDetails.AddSufferStateOthers(targetDataList[i].target, currentSuffer);
						break;
					case AffectEffect.Recommand:
						targetDataList[i].target.isRecommand = true;
						break;
					case AffectEffect.TurnEvasion:
						if (targetDataList[i].target.currentSufferState.onCountEvasion.isActive)
						{
							targetDataList[i].target.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.CountEvasion);
						}
						base.stateManager.skillDetails.AddSufferStateOthers(targetDataList[i].target, currentSuffer);
						break;
					case AffectEffect.CountEvasion:
						if (targetDataList[i].target.currentSufferState.onTurnEvasion.isActive)
						{
							targetDataList[i].target.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.TurnEvasion);
						}
						base.stateManager.skillDetails.AddSufferStateOthers(targetDataList[i].target, currentSuffer);
						break;
					case AffectEffect.ApDrain:
						base.stateManager.skillDetails.ApDown(targetDataList[i].target, currentSuffer);
						base.stateManager.skillDetails.ApUp(currentCharacter, currentSuffer);
						break;
					}
					targetDataList[i].isMiss = false;
				}
				Vector3 hitIconPosition = base.stateManager.uiControl.GetFixableCharacterCenterPosition2DFunction(targetDataList[i].target);
				HitIcon hitIcon = base.stateManager.uiControl.ApplyShowHitIcon(i, hitIconPosition, currentSuffer.type, hitIconDigit, Strength.None, targetDataList[i].isMiss, false, false, false, ExtraEffectType.Non);
				hitIconList.Add(hitIcon);
				this.SetTargetLog(targetDataList[i].target, targetDataList[i].isMiss, false);
				skillResultTargets.Add(targetDataList[i].target);
				skillResultMisses.Add(targetDataList[i].isMiss);
				log.index = targetDataList[i].target.myIndex;
				log.effectType = currentSuffer.type;
				log.miss = targetDataList[i].isMiss;
				if (currentCharacter.stagingChipIdList.Count<KeyValuePair<int, int>>() > 0)
				{
					log.chipUserIndex = currentCharacter.myIndex;
					List<int> _startUpChip = new List<int>();
					foreach (int chipEffectId in currentCharacter.stagingChipIdList.Keys)
					{
						if (_startUpChip.IndexOf(chipEffectId) < 0)
						{
							_startUpChip.Add(chipEffectId);
						}
					}
					log.startUpChip = _startUpChip;
				}
				buffLog.Add(log);
			}
		}
		IEnumerator logs = this.SendBattleLogs(currentCharacter, null, buffLog);
		while (logs.MoveNext())
		{
			object obj = logs.Current;
			yield return obj;
		}
		foreach (SubStateSkillDetailsFunction.TargetData targetData in targetDataList)
		{
			if (!targetData.isMiss && !base.battleStateData.IsChipSkill())
			{
				SufferStateProperty suffer2 = new SufferStateProperty(currentSuffer, 0);
				targetData.target.hitSufferList.Add(suffer2);
			}
		}
		base.battleStateData.SetPlayAnimationActionValues(currentCharacter, skillResultTargets.ToArray(), currentSuffer.type, base.stateManager.stateProperty.multiHitIntervalWaitSecond, skillResultMisses.ToArray(), hitIconList.ToArray(), extraEffectTypeList.ToArray(), null, false);
		base.SetState(typeof(SubStatePlayHitAnimationAction));
		while (base.isWaitState)
		{
			yield return null;
		}
		IEnumerator function = this.DropItem(deadCharacters.ToArray());
		while (function.MoveNext())
		{
			object obj2 = function.Current;
			yield return obj2;
		}
		currentCharacter.hate += hate;
		yield break;
	}

	protected override void DisabledThisState()
	{
		this.ShowRevivalEffect();
	}

	protected override void GetEventThisState(EventState eventState)
	{
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

	private void AddCountDictionary(SufferStateProperty.SufferType key, CharacterStateControl value)
	{
		if (!this.countDictionary.ContainsKey(key))
		{
			this.countDictionary.Add(key, new List<CharacterStateControl>());
		}
		HaveSufferState currentSufferState = value.currentSufferState;
		if (currentSufferState.FindSufferState(key))
		{
			SufferStateProperty sufferStateProperty = currentSufferState.GetSufferStateProperty(key);
			if (sufferStateProperty.isMultiHitThrough)
			{
				sufferStateProperty.currentKeepRound--;
				if (sufferStateProperty.currentKeepRound <= 0)
				{
					currentSufferState.RemoveSufferState(key);
				}
			}
			else if (!this.countDictionary[key].Contains(value))
			{
				this.countDictionary[key].Add(value);
			}
		}
	}

	private void UpdateCount(SufferStateProperty.SufferType key)
	{
		List<CharacterStateControl> list = null;
		this.countDictionary.TryGetValue(key, out list);
		if (list == null || list.Count == 0)
		{
			return;
		}
		foreach (CharacterStateControl characterStateControl in list)
		{
			HaveSufferState currentSufferState = characterStateControl.currentSufferState;
			if (currentSufferState.FindSufferState(key))
			{
				SufferStateProperty sufferStateProperty = currentSufferState.GetSufferStateProperty(key);
				if (!sufferStateProperty.isMultiHitThrough)
				{
					sufferStateProperty.currentKeepRound--;
					if (sufferStateProperty.currentKeepRound <= 0)
					{
						currentSufferState.RemoveSufferState(key);
					}
				}
			}
		}
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

	protected class TargetData
	{
		public CharacterStateControl target;

		public bool isMiss;

		public bool onProtect;

		public bool isDamage;
	}
}
