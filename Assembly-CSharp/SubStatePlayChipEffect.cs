using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SubStatePlayChipEffect : BattleStateController
{
	private List<CharacterStateControl> characters = new List<CharacterStateControl>();

	private Func<bool> IsSkillEnd;

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
		this.characters.Clear();
	}

	protected override IEnumerator MainRoutine()
	{
		if (base.stateManager.isLastBattle && (base.battleStateData.GetCharactersDeath(true) || base.battleStateData.GetCharactersDeath(false)))
		{
			yield break;
		}
		bool isLoop = false;
		int maxChipCount = this.GetMaxChipCount();
		while (this.characters.Count > 0)
		{
			isLoop = true;
			List<SubStatePlayChipEffect.ReturnData> resultList = this.CheckPlayStagingChipEffect();
			SubStatePlayChipEffect.ChipPlayType chipPlayType = SubStatePlayChipEffect.ChipPlayType.None;
			foreach (SubStatePlayChipEffect.ReturnData temp in resultList)
			{
				if (temp.chipPlayType > chipPlayType)
				{
					chipPlayType = temp.chipPlayType;
				}
			}
			string cameraKey = string.Empty;
			if (chipPlayType == SubStatePlayChipEffect.ChipPlayType.ChipAndSKillPlay && this.IsSkillEnd != null && this.IsSkillEnd())
			{
				List<CharacterStateControl> list = new List<CharacterStateControl>();
				foreach (SubStatePlayChipEffect.ReturnData result in resultList)
				{
					if (result.chipPlayType == SubStatePlayChipEffect.ChipPlayType.ChipAndSKillPlay)
					{
						result.characterStateControl.CharacterParams.gameObject.SetActive(true);
						list.Add(result.characterStateControl);
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
					base.stateManager.threeDAction.PlayIdleAnimationActiveCharacterAction(new CharacterStateControl[]
					{
						list[0]
					});
					base.stateManager.cameraControl.PlayCameraMotionActionCharacter(cameraKey, list[0]);
					base.stateManager.cameraControl.SetTime(cameraKey, 1f);
				}
			}
			if (chipPlayType != SubStatePlayChipEffect.ChipPlayType.None)
			{
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
			this.characters.Clear();
			maxChipCount = this.GetMaxChipCount();
			if (maxChipCount == 0)
			{
				break;
			}
		}
		if (isLoop)
		{
			foreach (CharacterStateControl character in base.battleStateData.playerCharacters)
			{
				if (!character.isDied)
				{
					character.CharacterParams.gameObject.SetActive(true);
					character.CharacterParams.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
				}
				else
				{
					character.CharacterParams.gameObject.SetActive(false);
				}
			}
			foreach (CharacterStateControl character2 in base.battleStateData.enemies)
			{
				if (!character2.isDied)
				{
					character2.CharacterParams.gameObject.SetActive(true);
					character2.CharacterParams.PlayAnimation(CharacterAnimationType.idle, SkillType.Attack, 0, null, null);
				}
				else
				{
					character2.CharacterParams.gameObject.SetActive(false);
				}
			}
		}
		yield break;
	}

	protected override void DisabledThisState()
	{
		base.battleStateData.SetChipSkillFlag(false);
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
						if (chipEffect.effectType.ToInt32() == 60 || chipEffect.effectType.ToInt32() == 56)
						{
							SkillStatus status = base.battleStateData.skillStatus.GetObject(chipEffect.effectValue);
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
		yield break;
	}

	private List<SubStatePlayChipEffect.ReturnData> CheckPlayStagingChipEffect()
	{
		List<SubStatePlayChipEffect.ReturnData> list = new List<SubStatePlayChipEffect.ReturnData>();
		foreach (CharacterStateControl characterStateControl in this.characters)
		{
			SubStatePlayChipEffect.ReturnData returnData = new SubStatePlayChipEffect.ReturnData();
			returnData.characterStateControl = characterStateControl;
			foreach (KeyValuePair<int, int> keyValuePair in characterStateControl.stagingChipIdList)
			{
				GameWebAPI.RespDataMA_ChipEffectM.ChipEffect chipEffectDataToId = ChipDataMng.GetChipEffectDataToId(keyValuePair.Key.ToString());
				if (!characterStateControl.isDied || chipEffectDataToId.effectTrigger.ToInt32() == 6)
				{
					if (!returnData.dictionary.ContainsKey(keyValuePair.Value))
					{
						returnData.dictionary[keyValuePair.Value] = new List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>();
					}
					if (chipEffectDataToId.effectType.ToInt32() == 60 || chipEffectDataToId.effectType.ToInt32() == 56)
					{
						SkillStatus @object = base.battleStateData.skillStatus.GetObject(chipEffectDataToId.effectValue);
						CharacterStateControl target = this.GetTarget(characterStateControl, chipEffectDataToId);
						if (@object != null && target != null)
						{
							returnData.chipPlayType = SubStatePlayChipEffect.ChipPlayType.ChipAndSKillPlay;
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
			list.Add(returnData);
			characterStateControl.stagingChipIdList.Clear();
		}
		return list;
	}

	private int GetMaxChipCount()
	{
		int num = 0;
		if (base.stateManager.battleMode == BattleMode.PvP)
		{
			if (base.stateManager.pvpFunction.IsOwner)
			{
				num += this.GetMaxChipCount(base.battleStateData.GetTotalCharacters());
			}
			else
			{
				num += this.GetMaxChipCount(base.battleStateData.GetTotalCharactersEnemyFirst());
			}
		}
		else
		{
			num += this.GetMaxChipCount(base.battleStateData.playerCharacters);
			num += this.GetMaxChipCount(base.battleStateData.enemies);
		}
		return num;
	}

	private int GetMaxChipCount(CharacterStateControl[] characterStateControls)
	{
		int num = 0;
		foreach (CharacterStateControl characterStateControl in characterStateControls)
		{
			if (characterStateControl.stagingChipIdList.Count > 0)
			{
				if (num < characterStateControl.stagingChipIdList.Count)
				{
					num = characterStateControl.stagingChipIdList.Count;
				}
				this.characters.Add(characterStateControl);
			}
		}
		return num;
	}

	private IEnumerator PlayChipEffectPerformance(List<SubStatePlayChipEffect.ReturnData> returnDataList)
	{
		float waitTime = 1f;
		int count = 0;
		int maxChipCount = 0;
		foreach (SubStatePlayChipEffect.ReturnData returnData in returnDataList)
		{
			List<int> stagingChipIdList = new List<int>();
			foreach (KeyValuePair<int, List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>> data in returnData.dictionary)
			{
				if (data.Value.Count > 0)
				{
					int chipId = data.Key;
					if (stagingChipIdList.IndexOf(chipId) < 0)
					{
						stagingChipIdList.Add(chipId);
					}
				}
			}
			int listCount = stagingChipIdList.Count<int>();
			if (listCount > maxChipCount)
			{
				maxChipCount = listCount;
			}
			if (listCount > 0)
			{
				base.battleStateData.stageGimmickUpEffect[count].SetPosition(returnData.characterStateControl.CharacterParams.transform, null);
				base.stateManager.threeDAction.PlayAlwaysEffectAction(base.battleStateData.stageGimmickUpEffect[count], AlwaysEffectState.In);
				base.stateManager.soundPlayer.TryPlaySE(base.battleStateData.stageGimmickUpEffect[count], AlwaysEffectState.In);
				base.StartCoroutine(base.stateManager.uiControl.ShowChipIcon(waitTime, base.battleStateData.stageGimmickUpEffect[count].targetPosition.position, stagingChipIdList.ToArray()));
			}
			count++;
		}
		bool isEnd = false;
		base.stateManager.uiControl.ShowBattleChipEffect((float)maxChipCount * waitTime + 1.9f, delegate
		{
			isEnd = true;
		});
		while (!isEnd)
		{
			yield return new WaitForEndOfFrame();
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
		SkillStatus @object = base.battleStateData.skillStatus.GetObject(chipEffect.effectValue);
		if (chipEffect.effectType.ToInt32() != 56)
		{
			CharacterStateControl[] skillTargetList = base.stateManager.targetSelect.GetSkillTargetList(chipCharacter, @object.target);
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
			foreach (AffectEffectProperty affectEffectProperty in @object.affectEffect)
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
							num2 += ((!flag2 || !currentSufferState.onPoison.isActive) ? 0 : 1);
							num2 += ((!flag3 || !currentSufferState.onConfusion.isActive) ? 0 : 1);
							num2 += ((!flag4 || !currentSufferState.onParalysis.isActive) ? 0 : 1);
							num2 += ((!flag5 || !currentSufferState.onSleep.isActive) ? 0 : 1);
							num2 += ((!flag6 || !currentSufferState.onStun.isActive) ? 0 : 1);
							num2 += ((!flag7 || !currentSufferState.onSkillLock.isActive) ? 0 : 1);
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
		if (base.battleStateData.currentSelectCharacterState != null && !base.battleStateData.currentSelectCharacterState.isDied)
		{
			return base.battleStateData.currentSelectCharacterState;
		}
		return null;
	}

	private enum ChipPlayType
	{
		None,
		ChipPlay,
		ChipAndSKillPlay
	}

	private class ReturnData
	{
		public SubStatePlayChipEffect.ChipPlayType chipPlayType;

		public CharacterStateControl characterStateControl;

		public Dictionary<int, List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>> dictionary = new Dictionary<int, List<GameWebAPI.RespDataMA_ChipEffectM.ChipEffect>>();
	}
}
