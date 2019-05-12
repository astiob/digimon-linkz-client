using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SubStateSkillDetailsFunction : BattleStateController
{
	private List<CharacterStateControl> countBarrierList;

	private List<CharacterStateControl> countEvasionList;

	private bool isBigBoss;

	public SubStateSkillDetailsFunction(Action OnExit, Action<EventState> OnExitGotEvent) : base(null, OnExit, OnExitGotEvent)
	{
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
		base.battleStateData.UpdateHitIconCharacter = null;
		this.countBarrierList = new List<CharacterStateControl>();
		this.countEvasionList = new List<CharacterStateControl>();
	}

	protected override IEnumerator MainRoutine()
	{
		CharacterStateControl currentCharacter = null;
		SkillStatus status = null;
		if (base.battleStateData.IsChipSkill())
		{
			currentCharacter = base.battleStateData.GetAutoCounterCharacter();
			status = base.battleStateData.skillStatus.GetObject(currentCharacter.chipSkillId);
		}
		else
		{
			currentCharacter = base.battleStateData.currentSelectCharacterState;
			status = currentCharacter.currentSkillStatus;
		}
		if (!currentCharacter.isMultiAreaRandomDamageSkill && currentCharacter.isSelectSkill == 0)
		{
			currentCharacter.OnChipTrigger(ChipEffectStatus.EffectTriggerType.AttackHit, false);
		}
		base.stateManager.uiControl.ApplyHideHitIcon();
		if (base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1)
		{
			this.isBigBoss = true;
		}
		List<SubStateSkillDetailsFunction.TargetData> targetDataList = new List<SubStateSkillDetailsFunction.TargetData>();
		AffectEffectProperty lastSuffer = null;
		for (int i = 0; i < status.affectEffect.Count; i++)
		{
			base.battleStateData.UpdateHitIconCharacter = null;
			AffectEffectProperty currentSuffer = status.affectEffect[i];
			this.SetRandomSeed(currentSuffer);
			if (lastSuffer != null && lastSuffer.target != currentSuffer.target)
			{
				CharacterStateControl[] target = base.stateManager.targetSelect.GetSkillTargetList(currentCharacter, currentSuffer.target);
				if (target != null && target.Length > 0)
				{
					currentCharacter.targetCharacter = target[0];
				}
			}
			lastSuffer = currentSuffer;
			targetDataList = this.CreateTargetData(targetDataList, currentCharacter, currentSuffer);
			if (targetDataList.Count == 0)
			{
				break;
			}
			base.stateManager.threeDAction.MotionResetAliveCharacterActionVoid(targetDataList.Select((SubStateSkillDetailsFunction.TargetData item) => item.target).ToArray<CharacterStateControl>());
			yield return null;
			CameraParams currentTargetCamera = null;
			string cameraKey = "skillF";
			if (currentSuffer.effectNumbers == EffectNumbers.All)
			{
				if (targetDataList[0].target.isEnemy && this.isBigBoss)
				{
					cameraKey = "BigBoss/skillA";
				}
				else
				{
					cameraKey = "skillA";
				}
			}
			else if (targetDataList[0].target.isEnemy && this.isBigBoss)
			{
				cameraKey = "BigBoss/skillF";
			}
			else
			{
				cameraKey = "skillF";
			}
			base.stateManager.cameraControl.PlayCameraMotionActionCharacter(cameraKey, targetDataList[0].target);
			CharacterStateControl[] showList = null;
			CharacterStateControl[] hideList = null;
			if (targetDataList[0].target.isEnemy)
			{
				showList = base.battleStateData.enemies.Where((CharacterStateControl x) => !x.isDied).ToArray<CharacterStateControl>();
				hideList = base.battleStateData.playerCharacters.Where((CharacterStateControl x) => !x.isDied).ToArray<CharacterStateControl>();
			}
			else
			{
				showList = base.battleStateData.playerCharacters.Where((CharacterStateControl x) => !x.isDied).ToArray<CharacterStateControl>();
				hideList = base.battleStateData.enemies.Where((CharacterStateControl x) => !x.isDied).ToArray<CharacterStateControl>();
			}
			base.stateManager.threeDAction.HideAllCharactersAction(hideList);
			base.stateManager.threeDAction.ShowAllCharactersAction(showList);
			base.stateManager.threeDAction.PlayIdleAnimationActiveCharacterAction(showList);
			if (!targetDataList[0].target.isEnemy)
			{
				this.ShowRevivalEffect();
			}
			else
			{
				this.HideRevivalEffect();
			}
			foreach (SubStateSkillDetailsFunction.TargetData targetData in targetDataList)
			{
				if (targetData.target == currentCharacter)
				{
					currentCharacter.CharacterParams.gameObject.SetActive(true);
					base.stateManager.threeDAction.PlayIdleAnimationCharactersAction(new CharacterStateControl[]
					{
						currentCharacter
					});
					break;
				}
			}
			base.stateManager.uiControl.HideCharacterHUDFunction();
			if (base.stateManager.skillDetails.IsFirstSkillDetails(i))
			{
				base.battleStateData.SetPlayPassiveEffectFunctionValues(currentCharacter, targetDataList.Select((SubStateSkillDetailsFunction.TargetData item) => item.target).ToArray<CharacterStateControl>(), status, currentSuffer);
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
			foreach (SubStateSkillDetailsFunction.TargetData targetData2 in targetDataList)
			{
				if (targetData2.onProtect)
				{
					base.stateManager.uiControl.ApplyTurnActionBarSwipeout(true);
					base.stateManager.uiControl.ApplySkillName(true, StringMaster.GetString("BattleUI-47"), targetData2.target);
					break;
				}
			}
			if (currentSuffer.type == AffectEffect.Damage || currentSuffer.type == AffectEffect.ReferenceTargetHpRate)
			{
				IEnumerator function = this.AffectEffectDamage(currentCharacter, targetDataList, currentSuffer, status, currentTargetCamera, i);
				while (function.MoveNext())
				{
					object obj = function.Current;
					yield return obj;
				}
			}
			else if (currentSuffer.type == AffectEffect.Destruct)
			{
				IEnumerator function2 = this.AffectEffectDestruct(currentCharacter, targetDataList, currentSuffer, currentTargetCamera);
				while (function2.MoveNext())
				{
					object obj2 = function2.Current;
					yield return obj2;
				}
			}
			else if (currentSuffer.type == AffectEffect.ApDrain)
			{
				IEnumerator function3 = this.AffectEffectApDrain(currentCharacter, targetDataList, currentSuffer, currentTargetCamera);
				while (function3.MoveNext())
				{
					object obj3 = function3.Current;
					yield return obj3;
				}
			}
			else if (Tolerance.OnInfluenceToleranceAffectEffect(currentSuffer.type))
			{
				IEnumerator function4 = this.ToleranceOnInfluenceToleranceAffectEffect(currentCharacter, targetDataList, currentSuffer, currentTargetCamera);
				while (function4.MoveNext())
				{
					object obj4 = function4.Current;
					yield return obj4;
				}
			}
			else
			{
				IEnumerator function5 = this.Other(currentCharacter, targetDataList, currentSuffer, currentTargetCamera);
				while (function5.MoveNext())
				{
					object obj5 = function5.Current;
					yield return obj5;
				}
			}
			foreach (SubStateSkillDetailsFunction.TargetData targetData3 in targetDataList)
			{
				if (targetData3.onProtect)
				{
					base.stateManager.uiControl.ApplyTurnActionBarSwipeout(false);
					break;
				}
			}
			IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.skillAfterWaitSecond, null, null);
			while (wait.MoveNext())
			{
				object obj6 = wait.Current;
				yield return obj6;
			}
			base.battleStateData.SEStopFunctionCall();
			while (base.battleStateData.StopHitAnimationCall())
			{
				yield return null;
			}
			base.stateManager.threeDAction.HideDeadCharactersAction(base.battleStateData.GetTotalCharacters());
			base.battleStateData.currentLastGenerateStartTimingSufferState++;
			base.stateManager.cameraControl.StopCameraMotionAction(cameraKey);
			base.stateManager.uiControl.HideCharacterHUDFunction();
			base.stateManager.uiControl.ApplyHideHitIcon();
			if (base.stateManager.IsLastBattleAndAllDeath())
			{
				break;
			}
		}
		foreach (CharacterStateControl character in base.battleStateData.GetTotalCharacters())
		{
			if (character.hitSufferList.Count > 0)
			{
				character.OnChipTrigger(ChipEffectStatus.EffectTriggerType.Suffer, false);
			}
		}
		foreach (CharacterStateControl character2 in base.battleStateData.GetTotalCharacters())
		{
			character2.hitSufferList = new List<SufferStateProperty>();
		}
		if (!currentCharacter.isMultiAreaRandomDamageSkill && currentCharacter.isSelectSkill == 0)
		{
			currentCharacter.currentSkillStatus.ClearAffectEffect();
		}
		this.UpdateCountBarrier();
		this.UpdateCountEvasion();
		yield break;
	}

	protected virtual void SetRandomSeed(AffectEffectProperty currentSuffer)
	{
	}

	private List<SubStateSkillDetailsFunction.TargetData> CreateTargetData(List<SubStateSkillDetailsFunction.TargetData> oldTargetDataList, CharacterStateControl currentCharacter, AffectEffectProperty currentSuffer)
	{
		List<SubStateSkillDetailsFunction.TargetData> list = new List<SubStateSkillDetailsFunction.TargetData>();
		CharacterStateControl[] array = base.stateManager.targetSelect.GetSkillTargetList(currentCharacter, currentSuffer.target);
		if (array == null || array.Length == 0)
		{
			return list;
		}
		bool onProtect = false;
		CharacterStateControl[] protectCharacters = base.stateManager.skillDetails.GetProtectCharacters(array, currentSuffer);
		if (protectCharacters != null)
		{
			onProtect = true;
			if (currentCharacter.targetCharacter.isDied)
			{
				return list;
			}
			array = new CharacterStateControl[]
			{
				protectCharacters[0]
			};
			currentCharacter.targetCharacter = array[0];
		}
		else if (currentSuffer.effectNumbers == EffectNumbers.Simple)
		{
			array = new CharacterStateControl[]
			{
				currentCharacter.targetCharacter
			};
		}
		bool flag = true;
		foreach (CharacterStateControl characterStateControl in array)
		{
			if (!characterStateControl.isDied)
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			return list;
		}
		if (!currentSuffer.isMissThrough)
		{
			List<CharacterStateControl> list2 = new List<CharacterStateControl>();
			foreach (CharacterStateControl characterStateControl2 in array)
			{
				bool flag2 = true;
				foreach (SubStateSkillDetailsFunction.TargetData targetData in oldTargetDataList)
				{
					if (targetData.isMiss && characterStateControl2.myIndex == targetData.target.myIndex)
					{
						flag2 = false;
						break;
					}
				}
				if (flag2)
				{
					list2.Add(characterStateControl2);
				}
			}
			if (list2.Count <= 0)
			{
				return list;
			}
			array = list2.ToArray();
		}
		foreach (CharacterStateControl target in array)
		{
			list.Add(new SubStateSkillDetailsFunction.TargetData
			{
				target = target,
				isMiss = false,
				onProtect = onProtect
			});
		}
		return list;
	}

	private IEnumerator AffectEffectDamage(CharacterStateControl currentCharacter, List<SubStateSkillDetailsFunction.TargetData> targetDataList, AffectEffectProperty currentSuffer, SkillStatus status, CameraParams cameraMotion, int currentSkillDetails)
	{
		int hate = 0;
		int totalDamage = 0;
		bool isApRevival = false;
		List<int[]> totalCounterReflectionDamage = new List<int[]>();
		AffectEffect counterReflectionAffectType = AffectEffect.Counter;
		List<CharacterStateControl> currentDeathCharacters = new List<CharacterStateControl>();
		List<BattleLogData.AttackLog> attackLog = new List<BattleLogData.AttackLog>();
		List<BattleLogData.BuffLog> buffLog = new List<BattleLogData.BuffLog>();
		for (int i = 0; i < targetDataList.Count; i++)
		{
			CharacterStateControl target = targetDataList[i].target;
			bool add = false;
			TechniqueType techniqueType = currentSuffer.techniqueType;
			if (techniqueType != TechniqueType.Physics)
			{
				if (techniqueType == TechniqueType.Special)
				{
					if (target.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Reflection))
					{
						add = true;
					}
				}
			}
			else if (target.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Counter))
			{
				add = true;
			}
			if (add)
			{
				totalCounterReflectionDamage.Add(new int[currentSuffer.hitNumber]);
			}
		}
		bool isAllTargetDeathBreak = false;
		ExtraEffectType sendExtraEffectType = ExtraEffectType.Non;
		for (int hitNumber = 0; hitNumber < currentSuffer.hitNumber; hitNumber++)
		{
			List<SkillResults> skillResults = new List<SkillResults>();
			List<ExtraEffectType> extraEffectTypes = new List<ExtraEffectType>();
			int counterReflectionCount = 0;
			List<HitIcon> hitIconlist = new List<HitIcon>();
			for (int j = 0; j < targetDataList.Count; j++)
			{
				CharacterStateControl target2 = targetDataList[j].target;
				if (!target2.isDied)
				{
					SkillResults atp = status.OnAttack(currentSkillDetails, currentCharacter, target2, base.hierarchyData.onEnableRandomValue);
					skillResults.Add(atp);
					if (hitNumber == 0)
					{
						targetDataList[j].isMiss = true;
					}
					if (!atp.onMissHit)
					{
						targetDataList[j].isMiss = false;
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
					if (!target2.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Sleep))
					{
						TechniqueType techniqueType = currentSuffer.techniqueType;
						if (techniqueType != TechniqueType.Physics)
						{
							if (techniqueType == TechniqueType.Special)
							{
								if (target2.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Reflection))
								{
									totalCounterReflectionDamage[counterReflectionCount][hitNumber] = ((!target2.isDied) ? target2.currentSufferState.onReflection.GetReflectDamage(atp.attackPowerNormal) : -1);
									counterReflectionAffectType = AffectEffect.Reflection;
									counterReflectionCount++;
								}
							}
						}
						else if (target2.currentSufferState.FindSufferState(SufferStateProperty.SufferType.Counter))
						{
							totalCounterReflectionDamage[counterReflectionCount][hitNumber] = ((!target2.isDied) ? target2.currentSufferState.onCounter.GetReflectDamage(atp.attackPowerNormal) : -1);
							counterReflectionAffectType = AffectEffect.Counter;
							counterReflectionCount++;
						}
					}
					bool isTurnBarrier = target2.currentSufferState.onTurnBarrier.isActive;
					bool isCountBarrier = target2.currentSufferState.onCountBarrier.isActive;
					bool isTurnEvasion = target2.currentSufferState.onTurnEvasion.isActive;
					bool isCountEvasion = target2.currentSufferState.onCountEvasion.isActive;
					if (!atp.onMissHit && !isTurnBarrier && !isCountBarrier && !isTurnEvasion && !isCountEvasion)
					{
						this.CheckFraud(atp.attackPower, currentCharacter, target2);
					}
					if (atp.onWeakHit == Strength.Weak && target2.isEnemy)
					{
						base.battleStateData.currentHalfEffectDamageNumber++;
					}
					if (target2.isDied && !currentDeathCharacters.Contains(target2))
					{
						currentDeathCharacters.Add(target2);
					}
					ExtraEffectType extraEffectType = ExtraEffectType.Non;
					if (atp.attackPower < atp.originalAttackPower)
					{
						extraEffectType = ExtraEffectType.Down;
					}
					else if (atp.attackPower > atp.originalAttackPower)
					{
						extraEffectType = ExtraEffectType.Up;
					}
					if (sendExtraEffectType != ExtraEffectType.Up)
					{
						sendExtraEffectType = extraEffectType;
					}
					extraEffectTypes.Add(extraEffectType);
					Vector3 hitIconPosition = base.stateManager.uiControl.GetFixableCharacterCenterPosition2DFunction(atp.targetCharacter);
					AffectEffect hitIconAffectEffect = AffectEffect.Damage;
					int damage = atp.attackPower;
					bool isCritical = atp.onCriticalHit;
					Strength isWeak = atp.onWeakHit;
					bool isMiss = atp.onMissHit;
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
						this.AddCountBarrierList(target2);
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
						this.AddCountEvasionList(target2);
					}
					HitIcon hitIcon = base.stateManager.uiControl.ApplyShowHitIcon(j, hitIconPosition, hitIconAffectEffect, damage, isWeak, isMiss, isCritical, false, false, extraEffectType);
					hitIconlist.Add(hitIcon);
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
			Action<Vector3[]> updateMethd = delegate(Vector3[] positions)
			{
				for (int m = 0; m < hitIconlist.Count; m++)
				{
					hitIconlist[m].HitIconReposition(positions[m]);
				}
			};
			CharacterStateControl[] skillResultTargets = skillResults.Select((SkillResults item) => item.targetCharacter).ToArray<CharacterStateControl>();
			bool[] skillResultMissHits = skillResults.Select((SkillResults item) => item.onMissHit).ToArray<bool>();
			bool isAllTargetDeath = true;
			foreach (CharacterStateControl skillResultTarget in skillResultTargets)
			{
				if (!skillResultTarget.isDied)
				{
					isAllTargetDeath = false;
					break;
				}
			}
			base.battleStateData.UpdateHitIconCharacter = base.stateManager.threeDAction.GetUpdateHitIconCharacters(updateMethd, skillResultTargets, hitIconlist.Count);
			this.ApplyMonsterIconEnabled(currentCharacter);
			if (cameraMotion != null && !base.stateManager.IsLastBattleAndAllDeath())
			{
				cameraMotion.PlayCameraShake();
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
			base.battleStateData.SetPlayAnimationActionValues(currentCharacter, skillResultTargets, currentSuffer.type, interval, skillResultMissHits, currentSuffer, true, sendExtraEffectType);
			base.SetState(typeof(SubStatePlayHitAnimationAction));
			while (base.isWaitState)
			{
				yield return null;
			}
			if (base.stateManager.battleMode != BattleMode.SkipAction && currentSuffer.hitNumber > 1)
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
			base.battleStateData.UpdateHitIconCharacter = null;
		}
		if (currentDeathCharacters.Count > 0)
		{
			currentCharacter.OnChipTrigger(ChipEffectStatus.EffectTriggerType.Kill, false);
		}
		if (cameraMotion != null && !base.stateManager.IsLastBattleAndAllDeath())
		{
			cameraMotion.StopCameraShake();
		}
		base.stateManager.skillDetails.NotToCounterReflectionMonsterWhoDiedOnTheWay(totalCounterReflectionDamage);
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
			IEnumerator function2 = this.DrainRecovery(currentCharacter, currentSuffer, cameraMotion, totalDamage);
			while (function2.MoveNext())
			{
				object obj2 = function2.Current;
				yield return obj2;
			}
		}
		CharacterStateControl[] targets = targetDataList.Select((SubStateSkillDetailsFunction.TargetData item) => item.target).ToArray<CharacterStateControl>();
		bool[] isMisses = targetDataList.Select((SubStateSkillDetailsFunction.TargetData item) => item.isMiss).ToArray<bool>();
		base.stateManager.skillDetails.GetSleepWakeUp(targets, isMisses);
		if (totalCounterReflectionDamage.Count > 0 && !currentCharacter.isDied)
		{
			BattleLogData.BuffLog reflectionLog = new BattleLogData.BuffLog();
			reflectionLog.effectType = AffectEffect.Reflection;
			for (int k = 0; k < totalCounterReflectionDamage.Count; k++)
			{
				for (int i2 = 0; i2 < totalCounterReflectionDamage[k].Length; i2++)
				{
					reflectionLog.value += totalCounterReflectionDamage[k][i2];
				}
			}
			buffLog.Add(reflectionLog);
			IEnumerator function3 = this.TotalCounterReflectionDamage(currentCharacter, currentSuffer, cameraMotion, counterReflectionAffectType, totalCounterReflectionDamage);
			while (function3.MoveNext())
			{
				object obj3 = function3.Current;
				yield return obj3;
			}
		}
		currentCharacter.hate += hate;
		base.battleStateData.SetApRevival(currentCharacter, isApRevival);
		this.SendBattleLogs(currentCharacter, attackLog, buffLog);
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

	protected virtual void SendBattleLogs(CharacterStateControl currentCharacter, List<BattleLogData.AttackLog> attackLog, List<BattleLogData.BuffLog> buffLog)
	{
	}

	private IEnumerator DrainRecovery(CharacterStateControl currentCharacter, AffectEffectProperty currentSuffer, CameraParams cameraMotion, int totalDamage)
	{
		IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.skillAfterWaitSecond, null, null);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		base.stateManager.threeDAction.HideDeadCharactersAction(base.battleStateData.GetTotalCharacters());
		string cameraKey = "skillF";
		if (currentCharacter.isEnemy && this.isBigBoss)
		{
			cameraKey = "BigBoss/skillF";
		}
		else
		{
			cameraKey = "skillF";
		}
		base.stateManager.cameraControl.PlayCameraMotionActionCharacter(cameraKey, currentCharacter);
		if (!currentCharacter.isEnemy)
		{
			this.ShowRevivalEffect();
		}
		else
		{
			this.HideRevivalEffect();
		}
		int recoveryDamage = Mathf.FloorToInt((float)totalDamage * currentSuffer.revivalPercent);
		currentCharacter.hp += recoveryDamage;
		HitIcon hitIcon = base.stateManager.uiControl.ApplyShowHitIcon(0, base.stateManager.uiControl.GetFixableCharacterCenterPosition2DFunction(currentCharacter), currentSuffer.type, recoveryDamage, Strength.None, false, false, true, true, ExtraEffectType.Non);
		Action<Vector3> updateMethd = delegate(Vector3 position)
		{
			hitIcon.HitIconReposition(position);
		};
		base.battleStateData.UpdateHitIconCharacter = base.stateManager.threeDAction.GetUpdateHitIconCharacters(updateMethd, currentCharacter);
		base.battleStateData.SetPlayAnimationActionValues(currentCharacter, new CharacterStateControl[]
		{
			currentCharacter
		}, AffectEffect.HpRevival, base.stateManager.stateProperty.attackerCharacterDrainActionWaitSecond, new bool[1], currentSuffer, false, ExtraEffectType.Non);
		base.SetState(typeof(SubStatePlayHitAnimationAction));
		while (base.isWaitState)
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator TotalCounterReflectionDamage(CharacterStateControl currentCharacter, AffectEffectProperty currentSuffer, CameraParams cameraMotion, AffectEffect counterReflectionAffectType, List<int[]> totalCounterReflectionDamage)
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
		string cameraKey = "skillF";
		if (currentCharacter.isEnemy && this.isBigBoss)
		{
			cameraKey = "BigBoss/skillF";
		}
		else
		{
			cameraKey = "skillF";
		}
		base.stateManager.cameraControl.PlayCameraMotionActionCharacter(cameraKey, currentCharacter);
		if (!currentCharacter.isEnemy)
		{
			this.ShowRevivalEffect();
		}
		else
		{
			this.HideRevivalEffect();
		}
		int[] isDamage = new int[currentSuffer.hitNumber];
		for (int i = 0; i < totalCounterReflectionDamage.Count; i++)
		{
			for (int i2 = 0; i2 < totalCounterReflectionDamage[i].Length; i2++)
			{
				isDamage[i2] += totalCounterReflectionDamage[i][i2];
			}
		}
		AffectEffect affectEffect = counterReflectionAffectType;
		if (currentCharacter.currentSufferState.onTurnBarrier.isActive)
		{
			affectEffect = AffectEffect.TurnBarrier;
		}
		else if (currentCharacter.currentSufferState.onCountBarrier.isActive)
		{
			affectEffect = AffectEffect.CountBarrier;
			this.AddCountBarrierList(currentCharacter);
		}
		else if (currentCharacter.currentSufferState.onTurnEvasion.isActive)
		{
			affectEffect = AffectEffect.TurnEvasion;
		}
		else if (currentCharacter.currentSufferState.onCountEvasion.isActive)
		{
			affectEffect = AffectEffect.CountEvasion;
			this.AddCountEvasionList(currentCharacter);
		}
		for (int j = 0; j < isDamage.Length; j++)
		{
			if (affectEffect == counterReflectionAffectType)
			{
				currentCharacter.hp -= isDamage[j];
			}
			HitIcon hitIcon = base.stateManager.uiControl.ApplyShowHitIcon(j, base.stateManager.uiControl.GetFixableCharacterCenterPosition2DFunction(currentCharacter), affectEffect, (isDamage == null || j >= isDamage.Length) ? -1 : isDamage[j], Strength.None, false, false, false, true, ExtraEffectType.Non);
			Action<Vector3> updateMethd = delegate(Vector3 position)
			{
				hitIcon.HitIconReposition(position);
			};
			base.battleStateData.UpdateHitIconCharacter = base.stateManager.threeDAction.GetUpdateHitIconCharacters(updateMethd, currentCharacter);
			base.battleStateData.SetPlayAnimationActionValues(currentCharacter, new CharacterStateControl[]
			{
				currentCharacter
			}, AffectEffect.Damage, base.stateManager.stateProperty.targetCounterReflectionActionWaitSecond / (float)isDamage.Length, new bool[1], currentSuffer, true, ExtraEffectType.Non);
			base.SetState(typeof(SubStatePlayHitAnimationAction));
			while (base.isWaitState)
			{
				yield return null;
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

	private IEnumerator AffectEffectDestruct(CharacterStateControl currentCharacter, List<SubStateSkillDetailsFunction.TargetData> targetDataList, AffectEffectProperty currentSuffer, CameraParams cameraMotion)
	{
		string cameraKey = "skillF";
		if (currentCharacter.isEnemy && this.isBigBoss)
		{
			cameraKey = "BigBoss/skillF";
		}
		else
		{
			cameraKey = "skillF";
		}
		base.stateManager.cameraControl.PlayCameraMotionActionCharacter(cameraKey, currentCharacter);
		if (!currentCharacter.isEnemy)
		{
			this.ShowRevivalEffect();
		}
		else
		{
			this.HideRevivalEffect();
		}
		base.stateManager.uiControl.HideCharacterHUDFunction();
		currentCharacter.Kill();
		this.AfterEnemyDeadFunction(new CharacterStateControl[]
		{
			currentCharacter
		});
		base.battleStateData.SetPlayAnimationActionValues(currentCharacter, new CharacterStateControl[]
		{
			currentCharacter
		}, currentSuffer.type, base.stateManager.stateProperty.destructCharacterDeathActionWaitSecond, new bool[1], null, false, ExtraEffectType.Non);
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
		if (currentSuffer.effectNumbers == EffectNumbers.All)
		{
			if (targetDataList[0].target.isEnemy && this.isBigBoss)
			{
				cameraKey = "BigBoss/skillA";
			}
			else
			{
				cameraKey = "skillA";
			}
		}
		else if (targetDataList[0].target.isEnemy && this.isBigBoss)
		{
			cameraKey = "BigBoss/skillF";
		}
		else
		{
			cameraKey = "skillF";
		}
		base.stateManager.cameraControl.PlayCameraMotionActionCharacter(cameraKey, currentCharacter.targetCharacter);
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
				skillResultTargets.Add(targetDataList[targets].target);
				skillResultMisses.Add(targetDataList[targets].isMiss);
				if (targetDataList[targets].target.isDied)
				{
					currentDeathCharacters.Add(targetDataList[targets].target);
				}
			}
		}
		this.AfterEnemyDeadFunction(currentDeathCharacters.ToArray());
		Action<Vector3[]> updateMethd = delegate(Vector3[] positions)
		{
			for (int i = 0; i < hitIconList.Count; i++)
			{
				hitIconList[i].HitIconReposition(positions[i]);
			}
		};
		base.battleStateData.UpdateHitIconCharacter = base.stateManager.threeDAction.GetUpdateHitIconCharacters(updateMethd, skillResultTargets.ToArray(), hitIconList.Count);
		base.stateManager.cameraControl.PlayCameraShake();
		base.stateManager.soundPlayer.StopHitEffectSE();
		base.battleStateData.SetPlayAnimationActionValues(currentCharacter, skillResultTargets.ToArray(), currentSuffer.type, base.stateManager.stateProperty.multiHitIntervalWaitSecond, skillResultMisses.ToArray(), null, false, ExtraEffectType.Non);
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

	protected virtual IEnumerator AffectEffectApDrain(CharacterStateControl currentCharacter, List<SubStateSkillDetailsFunction.TargetData> targetDataList, AffectEffectProperty currentSuffer, CameraParams cameraMotion)
	{
		List<BattleLogData.BuffLog> buffLog = new List<BattleLogData.BuffLog>();
		List<CharacterStateControl> skillResultTargets = new List<CharacterStateControl>();
		List<bool> skillResultMisses = new List<bool>();
		List<HitIcon> hitIconList = new List<HitIcon>();
		int totalApDrain = 0;
		for (int i = 0; i < targetDataList.Count; i++)
		{
			targetDataList[i].isMiss = true;
			if (!targetDataList[i].target.isDied)
			{
				if (!targetDataList[i].target.currentSufferState.onTurnBarrier.isActive)
				{
					if (targetDataList[i].target.currentSufferState.onCountBarrier.isActive)
					{
						this.AddCountBarrierList(targetDataList[i].target);
					}
					else if (!targetDataList[i].target.currentSufferState.onTurnEvasion.isActive)
					{
						if (targetDataList[i].target.currentSufferState.onCountEvasion.isActive)
						{
							this.AddCountEvasionList(targetDataList[i].target);
						}
						else if (currentSuffer.OnHit(currentCharacter, targetDataList[i].target))
						{
							targetDataList[i].isMiss = false;
							int lastAp = targetDataList[i].target.ap;
							targetDataList[i].target.ap -= currentSuffer.apDrainPower;
							totalApDrain += lastAp - targetDataList[i].target.ap;
						}
					}
				}
				Vector3 hitIconPosition = base.stateManager.uiControl.GetFixableCharacterCenterPosition2DFunction(targetDataList[i].target);
				HitIcon hitIcon = base.stateManager.uiControl.ApplyShowHitIcon(i, hitIconPosition, AffectEffect.ApDown, 0, Strength.None, targetDataList[i].isMiss, false, false, false, ExtraEffectType.Non);
				hitIconList.Add(hitIcon);
				skillResultTargets.Add(targetDataList[i].target);
				skillResultMisses.Add(targetDataList[i].isMiss);
				BattleLogData.BuffLog log = new BattleLogData.BuffLog
				{
					index = targetDataList[i].target.myIndex,
					miss = targetDataList[i].isMiss,
					effectType = currentSuffer.type
				};
				buffLog.Add(log);
			}
		}
		this.SendBattleLogs(currentCharacter, null, buffLog);
		Action<Vector3[]> updateMethd = delegate(Vector3[] positions)
		{
			for (int j = 0; j < hitIconList.Count; j++)
			{
				hitIconList[j].HitIconReposition(positions[j]);
			}
		};
		base.battleStateData.UpdateHitIconCharacter = base.stateManager.threeDAction.GetUpdateHitIconCharacters(updateMethd, skillResultTargets.ToArray(), hitIconList.Count);
		base.battleStateData.SetPlayAnimationActionValues(currentCharacter, skillResultTargets.ToArray(), AffectEffect.ApDown, base.stateManager.stateProperty.multiHitIntervalWaitSecond, skillResultMisses.ToArray(), null, false, ExtraEffectType.Non);
		base.SetState(typeof(SubStatePlayHitAnimationAction));
		while (base.isWaitState)
		{
			yield return null;
		}
		if (targetDataList.Where((SubStateSkillDetailsFunction.TargetData item) => !item.isMiss).Count<SubStateSkillDetailsFunction.TargetData>() > 0)
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
			string cameraKey = "skillF";
			if (currentCharacter.isEnemy && this.isBigBoss)
			{
				cameraKey = "BigBoss/skillF";
			}
			else
			{
				cameraKey = "skillF";
			}
			base.stateManager.cameraControl.PlayCameraMotionActionCharacter(cameraKey, currentCharacter);
			if (!currentCharacter.isEnemy)
			{
				this.ShowRevivalEffect();
			}
			else
			{
				this.HideRevivalEffect();
			}
			currentCharacter.ap += totalApDrain;
			HitIcon hitIcon2 = base.stateManager.uiControl.ApplyShowHitIcon(0, base.stateManager.uiControl.GetFixableCharacterCenterPosition2DFunction(currentCharacter), currentSuffer.type, 0, Strength.None, false, false, true, true, ExtraEffectType.Non);
			Action<Vector3> updateMethd2 = delegate(Vector3 position)
			{
				hitIcon2.HitIconReposition(position);
			};
			base.battleStateData.UpdateHitIconCharacter = base.stateManager.threeDAction.GetUpdateHitIconCharacters(updateMethd2, currentCharacter);
			base.battleStateData.SetPlayAnimationActionValues(currentCharacter, new CharacterStateControl[]
			{
				currentCharacter
			}, AffectEffect.ApDrain, base.stateManager.stateProperty.attackerCharacterDrainActionWaitSecond, new bool[1], currentSuffer, false, ExtraEffectType.Non);
			base.SetState(typeof(SubStatePlayHitAnimationAction));
			while (base.isWaitState)
			{
				yield return null;
			}
		}
		yield break;
	}

	private IEnumerator ToleranceOnInfluenceToleranceAffectEffect(CharacterStateControl currentCharacter, List<SubStateSkillDetailsFunction.TargetData> targetDataList, AffectEffectProperty currentSuffer, CameraParams cameraMotion)
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
						if (targetDataList[i].target.currentSufferState.onCountBarrier.isActive)
						{
							this.AddCountBarrierList(targetDataList[i].target);
						}
						else if (!targetDataList[i].target.currentSufferState.onTurnEvasion.isActive)
						{
							if (targetDataList[i].target.currentSufferState.onCountEvasion.isActive)
							{
								this.AddCountEvasionList(targetDataList[i].target);
							}
							else if (currentSuffer.OnHit(currentCharacter, targetDataList[i].target))
							{
								targetDataList[i].isMiss = false;
								if (currentSuffer.type == AffectEffect.InstantDeath)
								{
									instantDeathCharacters.Add(targetDataList[i].target);
								}
								else
								{
									SufferStateProperty suffer = new SufferStateProperty(currentSuffer);
									targetDataList[i].target.currentSufferState.SetSufferState(suffer, currentCharacter);
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
		this.SendBattleLogs(currentCharacter, null, buffLog);
		List<CharacterStateControl> skillResultTargets = new List<CharacterStateControl>();
		List<bool> skillResultMisses = new List<bool>();
		List<HitIcon> hitIconList = new List<HitIcon>();
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
				skillResultTargets.Add(targetDataList[j].target);
				skillResultMisses.Add(isMiss);
			}
		}
		foreach (SubStateSkillDetailsFunction.TargetData targetData in targetDataList)
		{
			if (!targetData.isMiss && !base.battleStateData.IsChipSkill())
			{
				SufferStateProperty suffer2 = new SufferStateProperty(currentSuffer);
				targetData.target.hitSufferList.Add(suffer2);
			}
		}
		if (instantDeathCharacters.Count > 0)
		{
			currentCharacter.OnChipTrigger(ChipEffectStatus.EffectTriggerType.Kill, false);
		}
		this.AfterEnemyDeadFunction(instantDeathCharacters.ToArray());
		Action<Vector3[]> updateMethd = delegate(Vector3[] positions)
		{
			for (int k = 0; k < hitIconList.Count; k++)
			{
				hitIconList[k].HitIconReposition(positions[k]);
			}
		};
		base.battleStateData.UpdateHitIconCharacter = base.stateManager.threeDAction.GetUpdateHitIconCharacters(updateMethd, skillResultTargets.ToArray(), hitIconList.Count);
		if (currentSuffer.type == AffectEffect.InstantDeath && cameraMotion != null)
		{
			cameraMotion.PlayCameraShake();
		}
		base.battleStateData.SetPlayAnimationActionValues(currentCharacter, skillResultTargets.ToArray(), currentSuffer.type, base.stateManager.stateProperty.multiHitIntervalWaitSecond, skillResultMisses.ToArray(), null, false, ExtraEffectType.Non);
		base.SetState(typeof(SubStatePlayHitAnimationAction));
		while (base.isWaitState)
		{
			yield return null;
		}
		if (currentSuffer.type == AffectEffect.InstantDeath && cameraMotion != null)
		{
			cameraMotion.StopCameraShake();
		}
		IEnumerator function = this.DropItem(instantDeathCharacters.ToArray());
		while (function.MoveNext())
		{
			object obj = function.Current;
			yield return obj;
		}
		yield break;
	}

	private IEnumerator Other(CharacterStateControl currentCharacter, List<SubStateSkillDetailsFunction.TargetData> targetDataList, AffectEffectProperty currentSuffer, CameraParams cameraMotion)
	{
		List<CharacterStateControl> deadCharacters = new List<CharacterStateControl>();
		int hate = 0;
		bool isPlayCameraShake = false;
		List<BattleLogData.BuffLog> buffLog = new List<BattleLogData.BuffLog>();
		List<CharacterStateControl> skillResultTargets = new List<CharacterStateControl>();
		List<bool> skillResultMisses = new List<bool>();
		List<HitIcon> hitIconList = new List<HitIcon>();
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
							SufferStateProperty suffer = new SufferStateProperty(currentSuffer);
							currentCharacter.currentSufferState.SetSufferState(suffer, null);
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
		this.SendBattleLogs(currentCharacter, null, buffLog);
		foreach (SubStateSkillDetailsFunction.TargetData targetData in targetDataList)
		{
			if (!targetData.isMiss && !base.battleStateData.IsChipSkill())
			{
				SufferStateProperty suffer2 = new SufferStateProperty(currentSuffer);
				targetData.target.hitSufferList.Add(suffer2);
			}
		}
		if (isPlayCameraShake && cameraMotion != null)
		{
			cameraMotion.PlayCameraShake();
		}
		Action<Vector3[]> updateMethd = delegate(Vector3[] positions)
		{
			for (int j = 0; j < hitIconList.Count; j++)
			{
				hitIconList[j].HitIconReposition(positions[j]);
			}
		};
		base.battleStateData.UpdateHitIconCharacter = base.stateManager.threeDAction.GetUpdateHitIconCharacters(updateMethd, skillResultTargets.ToArray(), hitIconList.Count);
		base.battleStateData.SetPlayAnimationActionValues(currentCharacter, skillResultTargets.ToArray(), currentSuffer.type, base.stateManager.stateProperty.multiHitIntervalWaitSecond, skillResultMisses.ToArray(), null, false, ExtraEffectType.Non);
		base.SetState(typeof(SubStatePlayHitAnimationAction));
		while (base.isWaitState)
		{
			yield return null;
		}
		if (isPlayCameraShake && cameraMotion != null)
		{
			cameraMotion.StopCameraShake();
		}
		IEnumerator function = this.DropItem(deadCharacters.ToArray());
		while (function.MoveNext())
		{
			object obj = function.Current;
			yield return obj;
		}
		currentCharacter.hate += hate;
		yield break;
	}

	protected override void DisabledThisState()
	{
		base.battleStateData.UpdateHitIconCharacter = null;
		this.ShowRevivalEffect();
	}

	protected override void GetEventThisState(EventState eventState)
	{
	}

	private void AddCountBarrierList(CharacterStateControl value)
	{
		if (!this.countBarrierList.Contains(value))
		{
			this.countBarrierList.Add(value);
		}
	}

	private void UpdateCountBarrier()
	{
		foreach (CharacterStateControl characterStateControl in this.countBarrierList)
		{
			characterStateControl.currentSufferState.onCountBarrier.currentKeepRound--;
			if (characterStateControl.currentSufferState.onCountBarrier.currentKeepRound <= 0)
			{
				characterStateControl.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.CountBarrier);
			}
		}
	}

	private void AddCountEvasionList(CharacterStateControl value)
	{
		if (!this.countEvasionList.Contains(value))
		{
			this.countEvasionList.Add(value);
		}
	}

	private void UpdateCountEvasion()
	{
		foreach (CharacterStateControl characterStateControl in this.countEvasionList)
		{
			characterStateControl.currentSufferState.onCountEvasion.currentKeepRound--;
			if (characterStateControl.currentSufferState.onCountEvasion.currentKeepRound <= 0)
			{
				characterStateControl.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.CountEvasion);
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
	}
}
