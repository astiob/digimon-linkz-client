using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SubStatePlayChipEffect : BattleStateController
{
	private Func<bool> IsSkillEnd;

	private List<CharacterStateControl> targetList = new List<CharacterStateControl>();

	public SubStatePlayChipEffect(Action OnExit, Action<EventState> OnExitGotEvent, Func<bool> isSkillEnd = null) : base(null, OnExit, OnExitGotEvent)
	{
		this.IsSkillEnd = isSkillEnd;
	}

	protected override void AwakeThisState()
	{
		base.AddState(new SubStatePlayInvocationEffectAction(null, new Action<EventState>(base.SendEventState)));
		if (base.stateManager.battleMode == BattleMode.PvP)
		{
			base.AddState(new SubStateWaitRandomSeedSync(null, new Action<EventState>(base.SendEventState)));
			base.AddState(new SubStatePvPSkillDetailsFunction(null, new Action<EventState>(base.SendEventState)));
		}
		else if (base.stateManager.battleMode == BattleMode.Multi)
		{
			base.AddState(new SubStateWaitRandomSeedSync(null, new Action<EventState>(base.SendEventState)));
			base.AddState(new SubStateMultiSkillDetailsFunction(null, new Action<EventState>(base.SendEventState)));
		}
		else
		{
			base.AddState(new SubStateSkillDetailsFunction(null, new Action<EventState>(base.SendEventState)));
		}
	}

	protected override void EnabledThisState()
	{
		base.battleStateData.SetChipSkillFlag(true);
	}

	protected override IEnumerator MainRoutine()
	{
		if (base.stateManager.battleMode == BattleMode.PvP)
		{
			if (base.stateManager.isLastBattle && (base.battleStateData.GetCharactersDeath(true) || base.battleStateData.GetCharactersDeath(false)))
			{
				yield break;
			}
		}
		else if (base.stateManager.isLastBattle && base.battleStateData.GetCharactersDeath(true))
		{
			yield break;
		}
		bool flag = !BattleStateManager.current.battleStateData.enemies.Where((CharacterStateControl item) => !item.isDied).Any<CharacterStateControl>();
		bool flag2 = !BattleStateManager.current.battleStateData.playerCharacters.Where((CharacterStateControl item) => !item.isDied).Any<CharacterStateControl>();
		foreach (CharacterStateControl characterStateControl in this.GetTotalCharacters())
		{
			if ((characterStateControl.isEnemy && flag) || (!characterStateControl.isEnemy && flag2))
			{
				characterStateControl.RemoveDeadStagingChips();
			}
		}
		CharacterStateControl[] chipActors = this.GetChipActors();
		while (chipActors.Length > 0)
		{
			List<SubStatePlayChipEffect.ReturnData> resultList = this.CheckPlayStagingChipEffect(chipActors);
			SubStatePlayChipEffect.ChipPlayType chipPlayType = SubStatePlayChipEffect.ChipPlayType.None;
			foreach (SubStatePlayChipEffect.ReturnData returnData in resultList)
			{
				if (returnData.chipPlayType > chipPlayType)
				{
					chipPlayType = returnData.chipPlayType;
				}
			}
			string cameraKey = string.Empty;
			CharacterStateControl target = null;
			if ((chipPlayType == SubStatePlayChipEffect.ChipPlayType.ChipAndSKillPlay || chipPlayType == SubStatePlayChipEffect.ChipPlayType.SKillPlay) && this.IsSkillEnd != null && this.IsSkillEnd())
			{
				List<CharacterStateControl> list = new List<CharacterStateControl>();
				foreach (SubStatePlayChipEffect.ReturnData returnData2 in resultList)
				{
					if (returnData2.chipPlayType == SubStatePlayChipEffect.ChipPlayType.ChipAndSKillPlay)
					{
						returnData2.characterStateControl.CharacterParams.gameObject.SetActive(true);
						list.Add(returnData2.characterStateControl);
					}
				}
				if (list.Count > 0)
				{
					if (list.Count == 1)
					{
						cameraKey = "skillF";
					}
					else
					{
						cameraKey = "skillA";
					}
					list[0].CharacterParams.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
					target = list[0];
				}
			}
			if (chipPlayType != SubStatePlayChipEffect.ChipPlayType.None)
			{
				foreach (SubStatePlayChipEffect.ReturnData returnData3 in resultList)
				{
					if (returnData3.chipPlayType != SubStatePlayChipEffect.ChipPlayType.None)
					{
						returnData3.characterStateControl.CharacterParams.gameObject.SetActive(true);
						returnData3.characterStateControl.CharacterParams.StopAnimation();
						returnData3.characterStateControl.CharacterParams.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
					}
				}
				if (target != null)
				{
					yield return new WaitForEndOfFrame();
					base.stateManager.cameraControl.PlayCameraMotionActionCharacter(cameraKey, target);
					base.stateManager.cameraControl.SetTime(cameraKey, 1f);
				}
				IEnumerator chipWait = this.PlayChipEffectPerformance(resultList);
				while (chipWait.MoveNext())
				{
					object obj = chipWait.Current;
					yield return obj;
				}
				if (!string.IsNullOrEmpty(cameraKey))
				{
					base.stateManager.cameraControl.StopCameraMotionAction(cameraKey);
				}
				IEnumerator chipEffectWait = this.PlayStagingChipEffect(resultList);
				while (chipEffectWait.MoveNext())
				{
					object obj2 = chipEffectWait.Current;
					yield return obj2;
				}
			}
			chipActors = this.GetChipActors();
			if (chipActors.Length == 0)
			{
				foreach (CharacterStateControl characterStateControl2 in base.battleStateData.GetTotalCharacters())
				{
					if (!characterStateControl2.isDied)
					{
						characterStateControl2.CharacterParams.gameObject.SetActive(true);
						base.stateManager.threeDAction.PlayIdleAnimationActiveCharacterAction(new CharacterStateControl[]
						{
							characterStateControl2
						});
					}
					else
					{
						characterStateControl2.CharacterParams.gameObject.SetActive(false);
					}
				}
			}
		}
		yield break;
	}

	protected override void DisabledThisState()
	{
		base.battleStateData.SetChipSkillFlag(false);
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

	private CharacterStateControl[] GetChipActors()
	{
		CharacterStateControl[] totalCharacters = this.GetTotalCharacters();
		return totalCharacters.Where((CharacterStateControl item) => item.stagingChipIdList.Count > 0).ToArray<CharacterStateControl>();
	}

	private List<SubStatePlayChipEffect.ReturnData> CheckPlayStagingChipEffect(CharacterStateControl[] chipActor)
	{
		List<SubStatePlayChipEffect.ReturnData> list = new List<SubStatePlayChipEffect.ReturnData>();
		foreach (CharacterStateControl characterStateControl in chipActor)
		{
			SubStatePlayChipEffect.ReturnData returnData = new SubStatePlayChipEffect.ReturnData();
			returnData.characterStateControl = characterStateControl;
			SubStatePlayChipEffect.ChipPlayType chipPlayType = SubStatePlayChipEffect.ChipPlayType.None;
			foreach (KeyValuePair<int, int> keyValuePair in characterStateControl.stagingChipIdList)
			{
				GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffectDataToId = ChipDataMng.GetChipEffectDataToId(keyValuePair.Key.ToString());
				if (!characterStateControl.isDied || chipEffectDataToId.effectTrigger.ToInt32() == 6)
				{
					if (!returnData.dictionary.ContainsKey(keyValuePair.Value))
					{
						returnData.dictionary[keyValuePair.Value] = new List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
					}
					if (chipEffectDataToId.effectType.ToInt32() == 60)
					{
						SkillStatus skillStatus = base.hierarchyData.GetSkillStatus(chipEffectDataToId.effectValue);
						CharacterStateControl target = this.GetTarget(characterStateControl, chipEffectDataToId);
						if (chipEffectDataToId.effectTrigger.ToInt32() == 37)
						{
							if (skillStatus != null && target != null && this.checkUseSkill(skillStatus, characterStateControl, target))
							{
								returnData.chipPlayType = SubStatePlayChipEffect.ChipPlayType.ChipAndSKillPlay;
								returnData.dictionary[keyValuePair.Value].Add(chipEffectDataToId);
							}
							else
							{
								returnData.characterStateControl.AddChipEffectCount(chipEffectDataToId.chipEffectId.ToInt32(), 1);
							}
						}
						else if (skillStatus != null && target != null)
						{
							returnData.chipPlayType = SubStatePlayChipEffect.ChipPlayType.ChipAndSKillPlay;
							returnData.dictionary[keyValuePair.Value].Add(chipEffectDataToId);
						}
						else
						{
							returnData.characterStateControl.AddChipEffectCount(chipEffectDataToId.chipEffectId.ToInt32(), 1);
						}
					}
					else if (chipEffectDataToId.effectType.ToInt32() == 56)
					{
						SkillStatus skillStatus2 = base.hierarchyData.GetSkillStatus(chipEffectDataToId.effectValue);
						CharacterStateControl target2 = this.GetTarget(characterStateControl, chipEffectDataToId);
						if (skillStatus2 != null && target2 != null)
						{
							chipPlayType = SubStatePlayChipEffect.ChipPlayType.SKillPlay;
							returnData.dictionary[keyValuePair.Value].Add(chipEffectDataToId);
						}
						else
						{
							returnData.characterStateControl.AddChipEffectCount(chipEffectDataToId.chipEffectId.ToInt32(), 1);
						}
					}
					else
					{
						if (returnData.chipPlayType == SubStatePlayChipEffect.ChipPlayType.None)
						{
							returnData.chipPlayType = SubStatePlayChipEffect.ChipPlayType.ChipPlay;
						}
						returnData.dictionary[keyValuePair.Value].Add(chipEffectDataToId);
					}
				}
			}
			if (returnData.chipPlayType == SubStatePlayChipEffect.ChipPlayType.None && chipPlayType != SubStatePlayChipEffect.ChipPlayType.None)
			{
				returnData.chipPlayType = chipPlayType;
			}
			list.Add(returnData);
			characterStateControl.stagingChipIdList.Clear();
		}
		return list;
	}

	private IEnumerator PlayStagingChipEffect(List<SubStatePlayChipEffect.ReturnData> returnDataList)
	{
		foreach (SubStatePlayChipEffect.ReturnData returnData in returnDataList)
		{
			foreach (KeyValuePair<int, List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>> data in returnData.dictionary)
			{
				foreach (GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect in data.Value)
				{
					if (!returnData.characterStateControl.isDied || chipEffect.effectTrigger.ToInt32() == 6)
					{
						SufferStateProperty death = returnData.characterStateControl.currentSufferState.GetSufferStateProperty(SufferStateProperty.SufferType.InstantDeath);
						if (death == null || !death.isActive || death.isMiss || chipEffect.effectTrigger.ToInt32() == 6)
						{
							if (chipEffect.effectType.ToInt32() == 60 || chipEffect.effectType.ToInt32() == 56)
							{
								SkillStatus status = base.hierarchyData.GetSkillStatus(chipEffect.effectValue);
								CharacterStateControl target = this.GetTarget(returnData.characterStateControl, chipEffect);
								if (status == null || target == null)
								{
									returnData.characterStateControl.AddChipEffectCount(chipEffect.chipEffectId.ToInt32(), 1);
								}
								else
								{
									IEnumerator function = this.ChipSkillDetailsFunction(returnData.characterStateControl, data.Key, chipEffect, status, target);
									while (function.MoveNext())
									{
										object obj = function.Current;
										yield return obj;
									}
								}
							}
						}
					}
				}
			}
		}
		yield break;
	}

	private IEnumerator PlayChipEffectPerformance(List<SubStatePlayChipEffect.ReturnData> returnDataList)
	{
		float waitTime = 1f;
		int count = 0;
		int maxChipCount = 0;
		foreach (SubStatePlayChipEffect.ReturnData returnData in returnDataList)
		{
			List<int> list = new List<int>();
			foreach (KeyValuePair<int, List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>> keyValuePair in returnData.dictionary)
			{
				if (keyValuePair.Value.Count > 0)
				{
					if (returnData.chipPlayType != SubStatePlayChipEffect.ChipPlayType.SKillPlay)
					{
						int key = keyValuePair.Key;
						if (list.IndexOf(key) < 0)
						{
							list.Add(key);
						}
					}
				}
			}
			int num = list.Count<int>();
			if (num > maxChipCount)
			{
				maxChipCount = num;
			}
			if (num > 0)
			{
				base.battleStateData.stageGimmickUpEffect[count].SetPosition(returnData.characterStateControl.CharacterParams.transform, null);
				base.stateManager.threeDAction.PlayAlwaysEffectAction(base.battleStateData.stageGimmickUpEffect[count], AlwaysEffectState.In);
				base.stateManager.soundPlayer.TryPlaySE(base.battleStateData.stageGimmickUpEffect[count], AlwaysEffectState.In);
				base.StartCoroutine(base.stateManager.uiControl.ShowChipIcon(waitTime, base.battleStateData.stageGimmickUpEffect[count].targetPosition.position, list.ToArray()));
			}
			count++;
		}
		if (maxChipCount > 0)
		{
			bool isEnd = false;
			base.stateManager.uiControl.ShowBattleChipEffect((float)maxChipCount * waitTime + 1.9f, delegate
			{
				isEnd = true;
			});
			while (!isEnd)
			{
				yield return new WaitForEndOfFrame();
			}
		}
		for (int i = 0; i < base.battleStateData.stageGimmickUpEffect.Length; i++)
		{
			base.stateManager.threeDAction.PlayAlwaysEffectAction(base.battleStateData.stageGimmickUpEffect[i], AlwaysEffectState.Out);
		}
		base.stateManager.threeDAction.StopAlwaysEffectAction(base.battleStateData.stageGimmickUpEffect);
		yield break;
	}

	private IEnumerator ChipSkillDetailsFunction(CharacterStateControl chipCharacter, int chipId, GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect, SkillStatus status, CharacterStateControl target)
	{
		chipCharacter.chipSkillId = chipEffect.effectValue;
		chipCharacter.currentChipId = chipId.ToString();
		base.battleStateData.SetAutoCounterCharacter(chipCharacter);
		chipCharacter.targetCharacter = target;
		base.stateManager.cameraControl.PlayTweenCameraMotion(base.battleStateData.commandSelectTweenTargetCamera, chipCharacter.targetCharacter);
		base.battleStateData.isInvocationEffectPlaying = false;
		base.stateManager.time.SetPlaySpeed(base.hierarchyData.on2xSpeedPlay, base.battleStateData.isShowMenuWindow);
		base.stateManager.uiControl.ApplyTurnActionBarSwipeout(true);
		base.SetState(typeof(SubStatePlayInvocationEffectAction));
		while (base.isWaitState)
		{
			yield return null;
		}
		base.stateManager.time.SetPlaySpeed(base.hierarchyData.on2xSpeedPlay, base.battleStateData.isShowMenuWindow);
		base.stateManager.uiControl.ApplyTurnActionBarSwipeout(false);
		if (base.stateManager.battleMode == BattleMode.PvP)
		{
			base.SetState(typeof(SubStateWaitRandomSeedSync));
			while (base.isWaitState)
			{
				yield return null;
			}
			base.SetState(typeof(SubStatePvPSkillDetailsFunction));
		}
		else if (base.stateManager.battleMode == BattleMode.Multi)
		{
			base.SetState(typeof(SubStateWaitRandomSeedSync));
			while (base.isWaitState)
			{
				yield return null;
			}
			base.SetState(typeof(SubStateMultiSkillDetailsFunction));
		}
		else
		{
			base.SetState(typeof(SubStateSkillDetailsFunction));
		}
		while (base.isWaitState)
		{
			yield return null;
		}
		yield break;
	}

	private CharacterStateControl GetTarget(CharacterStateControl chipCharacter, GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffect)
	{
		SkillStatus skillStatus = base.hierarchyData.GetSkillStatus(chipEffect.effectValue);
		if (chipEffect.effectType.ToInt32() != 56)
		{
			CharacterStateControl[] skillTargetList = base.stateManager.targetSelect.GetSkillTargetList(chipCharacter, skillStatus.target);
			if (chipCharacter.targetCharacter != null && !chipCharacter.targetCharacter.isDied)
			{
				bool flag = false;
				foreach (CharacterStateControl b in skillTargetList)
				{
					if (chipCharacter.targetCharacter == b)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					List<CharacterStateControl> list = new List<CharacterStateControl>();
					list.Add(chipCharacter.targetCharacter);
					foreach (CharacterStateControl characterStateControl in skillTargetList)
					{
						if (chipCharacter.targetCharacter != characterStateControl && !chipCharacter.targetCharacter.isDied)
						{
							list.Add(characterStateControl);
						}
					}
				}
			}
			foreach (AffectEffectProperty affectEffectProperty in skillStatus.affectEffect)
			{
				if (affectEffectProperty.type == AffectEffect.SufferStatusClear)
				{
					bool flag2 = affectEffectProperty.clearPoisonIncidenceRate > 0f;
					bool flag3 = affectEffectProperty.clearConfusionIncidenceRate > 0f;
					bool flag4 = affectEffectProperty.clearParalysisIncidenceRate > 0f;
					bool flag5 = affectEffectProperty.clearSleepIncidenceRate > 0f;
					bool flag6 = affectEffectProperty.clearStunIncidenceRate > 0f;
					bool flag7 = affectEffectProperty.clearSkillLockIncidenceRate > 0f;
					int num = 0;
					CharacterStateControl result = null;
					for (int k = 0; k < skillTargetList.Length; k++)
					{
						if (chipEffect.targetValue2.ToInt32() <= 0 || !(skillTargetList[k] == chipCharacter))
						{
							HaveSufferState currentSufferState = skillTargetList[k].currentSufferState;
							int num2 = 0;
							num2 += ((!flag2 || !currentSufferState.FindSufferState(SufferStateProperty.SufferType.Poison)) ? 0 : 1);
							num2 += ((!flag3 || !currentSufferState.FindSufferState(SufferStateProperty.SufferType.Confusion)) ? 0 : 1);
							num2 += ((!flag4 || !currentSufferState.FindSufferState(SufferStateProperty.SufferType.Paralysis)) ? 0 : 1);
							num2 += ((!flag5 || !currentSufferState.FindSufferState(SufferStateProperty.SufferType.Sleep)) ? 0 : 1);
							num2 += ((!flag6 || !currentSufferState.FindSufferState(SufferStateProperty.SufferType.Stun)) ? 0 : 1);
							num2 += ((!flag7 || !currentSufferState.FindSufferState(SufferStateProperty.SufferType.SkillLock)) ? 0 : 1);
							if (num2 > num)
							{
								num = num2;
								result = skillTargetList[k];
							}
						}
					}
					return result;
				}
			}
			return (skillTargetList.Length <= 0) ? null : skillTargetList[0];
		}
		CharacterStateControl[] skillTargetList2;
		if (chipCharacter.isEnemy)
		{
			skillTargetList2 = base.stateManager.targetSelect.GetSkillTargetList(base.battleStateData.playerCharacters[0], skillStatus.target);
		}
		else
		{
			skillTargetList2 = base.stateManager.targetSelect.GetSkillTargetList(base.battleStateData.enemies[0], skillStatus.target);
		}
		if (skillTargetList2 == null || skillTargetList2.Length == 0)
		{
			return null;
		}
		if (skillStatus.numbers != EffectNumbers.Simple)
		{
			return skillTargetList2[0];
		}
		if (skillStatus.target == EffectTarget.Attacker)
		{
			return chipCharacter;
		}
		if (skillStatus.target == EffectTarget.Enemy)
		{
			if (base.battleStateData.currentSelectCharacterState == null || base.battleStateData.currentSelectCharacterState.isDied)
			{
				return null;
			}
			return base.battleStateData.currentSelectCharacterState;
		}
		else
		{
			if (skillStatus.target == EffectTarget.Ally)
			{
				return chipCharacter;
			}
			if (base.battleStateData.currentSelectCharacterState == null || base.battleStateData.currentSelectCharacterState.isDied)
			{
				return null;
			}
			return base.battleStateData.currentSelectCharacterState;
		}
	}

	private bool checkUseSkill(SkillStatus status, CharacterStateControl currentCharacter, CharacterStateControl tg)
	{
		bool isProtectEnableSkill = false;
		foreach (AffectEffectProperty affectEffectProperty in status.affectEffect)
		{
			if (AffectEffectProperty.IsDamage(affectEffectProperty.type))
			{
				isProtectEnableSkill = true;
				break;
			}
		}
		List<SubStateSkillDetailsFunction.TargetData> list = new List<SubStateSkillDetailsFunction.TargetData>();
		int num = 0;
		for (int i = 0; i < status.affectEffect.Count; i++)
		{
			AffectEffectProperty affectEffectProperty2 = status.affectEffect[i];
			if (AffectEffectProperty.IsDamage(affectEffectProperty2.type))
			{
				num += affectEffectProperty2.hitNumber;
			}
		}
		bool flag = true;
		AffectEffectProperty affectEffectProperty3 = null;
		bool flag2 = false;
		bool result = false;
		bool flag3 = false;
		EffectTarget effectTarget = EffectTarget.Enemy;
		for (int j = 0; j < status.affectEffect.Count; j++)
		{
			DkLog.W("↓↓↓↓↓↓", false);
			AffectEffectProperty affectEffectProperty4 = status.affectEffect[j];
			if (j == 0)
			{
				effectTarget = affectEffectProperty4.target;
			}
			if (affectEffectProperty3 != null && affectEffectProperty3.target != affectEffectProperty4.target)
			{
				CharacterStateControl[] skillTargetList = base.stateManager.targetSelect.GetSkillTargetList(currentCharacter, affectEffectProperty4.target);
				if (skillTargetList != null && skillTargetList.Length > 0)
				{
					currentCharacter.targetCharacter = skillTargetList[0];
				}
				if (effectTarget == affectEffectProperty4.target)
				{
					currentCharacter.targetCharacter = tg;
				}
			}
			list = this.CreateTargetData(list, currentCharacter, affectEffectProperty4, isProtectEnableSkill, ref flag);
			affectEffectProperty3 = affectEffectProperty4;
			if (this.SwitchAffectEffect(affectEffectProperty4, currentCharacter, tg, list, ref flag3, ref flag2))
			{
				DkLog.W(string.Format("targetDataList {0} : currentSuffer.type {1} : tg {2} : skip {3}", new object[]
				{
					list.Count,
					affectEffectProperty4.type,
					string.Empty,
					flag2
				}), false);
				if (!flag2)
				{
					result = true;
					DkLog.W(string.Format("targetDataList.Count {0}", list.Count), false);
				}
			}
		}
		return result;
	}

	private List<SubStateSkillDetailsFunction.TargetData> CreateTargetData(List<SubStateSkillDetailsFunction.TargetData> oldTargetDataList, CharacterStateControl currentCharacter, AffectEffectProperty currentSuffer, bool isProtectEnableSkill, ref bool enableDrawProtectMessage)
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
				if (currentCharacter.targetCharacter == array[0])
				{
					enableDrawProtectMessage = false;
				}
				currentCharacter.targetCharacter = array[0];
			}
			array = new CharacterStateControl[]
			{
				currentCharacter.targetCharacter
			};
		}
		if (!currentSuffer.isMissThrough && oldTargetDataList.Count > 0)
		{
			List<CharacterStateControl> list2 = new List<CharacterStateControl>();
			CharacterStateControl[] array3 = array;
			for (int i = 0; i < array3.Length; i++)
			{
				CharacterStateControl target = array3[i];
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

	private bool SwitchAffectEffect(AffectEffectProperty currentSuffer, CharacterStateControl currentCharacter, CharacterStateControl baseTarget, List<SubStateSkillDetailsFunction.TargetData> targetDataList, ref bool isEnableForPrevious, ref bool skip)
	{
		if (currentSuffer.type != AffectEffect.SkillBranch)
		{
			return true;
		}
		DkLog.W("条件分岐判定処理開始", false);
		if (currentSuffer.type == AffectEffect.SkillBranch && currentSuffer.skillBranchOverlap == 1)
		{
			isEnableForPrevious = false;
			DkLog.W("重複可能だったのでフラグを下ろす", false);
		}
		DkLog.W("isEnableForPrevious : " + isEnableForPrevious, false);
		if (currentSuffer.type == AffectEffect.SkillBranch && (currentSuffer.skillBranchOverlap != 0 || !isEnableForPrevious))
		{
			bool flag = false;
			bool flag2 = currentSuffer.skillBranchTargetType == 4 || currentSuffer.skillBranchTargetType == 6;
			this.targetList.Clear();
			switch (currentSuffer.skillBranchTargetType)
			{
			case 1:
				this.targetList.Add(currentCharacter);
				break;
			case 2:
				this.targetList.Add(baseTarget);
				break;
			case 3:
			case 4:
				foreach (CharacterStateControl item in base.battleStateData.playerCharacters)
				{
					this.targetList.Add(item);
				}
				break;
			case 5:
			case 6:
				foreach (CharacterStateControl item2 in base.battleStateData.enemies)
				{
					this.targetList.Add(item2);
				}
				break;
			}
			foreach (CharacterStateControl characterStateControl in this.targetList)
			{
				if (!(characterStateControl != null) || !characterStateControl.isDied)
				{
					CharacterStateControl characterStateControl2 = characterStateControl;
					SkillBranchType skillBranchType = (SkillBranchType)currentSuffer.skillBranchType;
					switch (skillBranchType)
					{
					case SkillBranchType.None:
						break;
					case SkillBranchType.SufferStatus:
						if (characterStateControl2.currentSufferState.FindSufferState((SufferStateProperty.SufferType)currentSuffer.skillBranchTypeValue))
						{
							flag = true;
						}
						else if (flag2)
						{
							flag = false;
						}
						continue;
					case SkillBranchType.HpRateUp:
					{
						float num = (float)characterStateControl2.hp / ((float)characterStateControl2.extraMaxHp * 1f) * 100f;
						if (num >= (float)currentSuffer.skillBranchTypeValue)
						{
							flag = true;
						}
						else if (flag2)
						{
							flag = false;
						}
						continue;
					}
					case SkillBranchType.HpRateDown:
					{
						float num = (float)characterStateControl2.hp / ((float)characterStateControl2.extraMaxHp * 1f) * 100f;
						if (num * 100f <= (float)currentSuffer.skillBranchTypeValue)
						{
							flag = true;
						}
						else if (flag2)
						{
							flag = false;
						}
						continue;
					}
					case SkillBranchType.MyHpRateUp:
					{
						float num = (float)characterStateControl2.hp / ((float)characterStateControl2.extraMaxHp * 1f) * 100f;
						float num2 = (float)currentCharacter.hp / ((float)currentCharacter.extraMaxHp * 1f) * 100f;
						if (num >= num2)
						{
							flag = true;
						}
						else if (flag2)
						{
							flag = false;
						}
						continue;
					}
					case SkillBranchType.MyHpRateDown:
					{
						float num = (float)characterStateControl2.hp / ((float)characterStateControl2.extraMaxHp * 1f) * 100f;
						float num2 = (float)currentCharacter.hp / ((float)currentCharacter.extraMaxHp * 1f) * 100f;
						if (num <= num2)
						{
							flag = true;
						}
						else if (flag2)
						{
							flag = false;
						}
						continue;
					}
					case SkillBranchType.BehaviorAlready:
						if (currentSuffer.skillBranchTypeValue == 0)
						{
							if (characterStateControl2.skillOrder > currentCharacter.skillOrder)
							{
								flag = true;
							}
							else if (flag2)
							{
								flag = false;
							}
						}
						else if (characterStateControl2.skillOrder < currentCharacter.skillOrder)
						{
							flag = true;
						}
						else if (flag2)
						{
							flag = false;
						}
						continue;
					case SkillBranchType.Attribute:
						continue;
					case SkillBranchType.AttributeMerit:
						continue;
					case SkillBranchType.NotSufferStatus:
						if (!characterStateControl2.currentSufferState.FindSufferState((SufferStateProperty.SufferType)currentSuffer.skillBranchTypeValue))
						{
							flag = true;
						}
						else if (flag2)
						{
							flag = false;
						}
						continue;
					default:
						if (skillBranchType != SkillBranchType.End)
						{
							continue;
						}
						break;
					}
					flag = true;
				}
			}
			DkLog.W(string.Format("isEnable {0}", flag), false);
			if (flag)
			{
				skip = false;
				isEnableForPrevious = true;
			}
			else
			{
				skip = true;
			}
			foreach (SubStateSkillDetailsFunction.TargetData targetData in targetDataList)
			{
				targetData.isAllMiss = false;
			}
			return false;
		}
		skip = true;
		DkLog.W(string.Format("skip {0}", skip), false);
		return false;
	}

	private enum ChipPlayType
	{
		None,
		ChipPlay,
		ChipAndSKillPlay,
		SKillPlay
	}

	private class ReturnData
	{
		public SubStatePlayChipEffect.ChipPlayType chipPlayType;

		public CharacterStateControl characterStateControl;

		public Dictionary<int, List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>> dictionary = new Dictionary<int, List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>>();
	}
}
