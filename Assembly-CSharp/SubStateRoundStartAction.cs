using System;
using System.Collections;
using System.Collections.Generic;
using UnityExtension;

public class SubStateRoundStartAction : BattleStateController
{
	public SubStateRoundStartAction(Action OnExit, Action<EventState> OnExitGotEvent) : base(null, OnExit, OnExitGotEvent)
	{
	}

	protected override void AwakeThisState()
	{
		base.AddState(new SubStatePlayChipEffect(null, new Action<EventState>(base.SendEventState), null));
		base.AddState(new SubStatePlayStageEffect(null, new Action<EventState>(base.SendEventState)));
	}

	protected override void EnabledThisState()
	{
	}

	protected override IEnumerator MainRoutine()
	{
		IEnumerator[] functions = new IEnumerator[]
		{
			this.PlayAdventureScene(BattleAdventureSceneManager.TriggerType.WaveStart),
			this.PlayAdventureScene(BattleAdventureSceneManager.TriggerType.RoundStart),
			this.PlayAdventureScene(BattleAdventureSceneManager.TriggerType.TotalRoundStart),
			this.PlayStageEffect(),
			this.PlayChipEffect(),
			this.PlayRoundStartEffect(),
			this.UpFunction(),
			this.DownFunction(),
			this.PlayAdventureScene(BattleAdventureSceneManager.TriggerType.WaveEnd),
			this.PlayAdventureScene(BattleAdventureSceneManager.TriggerType.RoundEnd),
			this.PlayAdventureScene(BattleAdventureSceneManager.TriggerType.TotalRoundEnd)
		};
		foreach (IEnumerator function in functions)
		{
			while (function.MoveNext())
			{
				object obj = function.Current;
				yield return obj;
			}
		}
		yield break;
	}

	private CharacterStateControl[] GetTotalCharacters()
	{
		if (base.stateManager.battleMode != BattleMode.PvP)
		{
			return base.battleStateData.GetTotalCharacters();
		}
		if (base.stateManager.pvpFunction.IsOwner)
		{
			return base.battleStateData.GetTotalCharacters();
		}
		return base.battleStateData.GetTotalCharactersEnemyFirst();
	}

	private IEnumerator PlayAdventureScene(BattleAdventureSceneManager.TriggerType triggerType)
	{
		base.stateManager.battleAdventureSceneManager.OnTrigger(triggerType);
		IEnumerator update = base.stateManager.battleAdventureSceneManager.Update();
		while (update.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator PlayStageEffect()
	{
		if (base.stateManager.onEnableTutorial)
		{
			yield break;
		}
		if (base.battleStateData.currentRoundNumber <= 1)
		{
			IEnumerator gimmickWait = base.stateManager.threeDAction.PlayGimmickAnimation(base.battleStateData.playerCharacters, base.battleStateData.enemies);
			while (gimmickWait.MoveNext())
			{
				object obj = gimmickWait.Current;
				yield return obj;
			}
		}
		if (base.battleStateData.currentRoundNumber <= 1)
		{
			base.battleStateData.reqestStageEffectTriggerList.Add(EffectStatusBase.EffectTriggerType.WaveStarted);
			base.SetState(typeof(SubStatePlayStageEffect));
			while (base.isWaitState)
			{
				yield return null;
			}
		}
		yield break;
	}

	private IEnumerator PlayChipEffect()
	{
		if (base.battleStateData.currentRoundNumber <= 1)
		{
			foreach (CharacterStateControl character in this.GetTotalCharacters())
			{
				character.OnChipTrigger(EffectStatusBase.EffectTriggerType.WaveStarted);
			}
		}
		foreach (CharacterStateControl character2 in this.GetTotalCharacters())
		{
			character2.OnChipTrigger(EffectStatusBase.EffectTriggerType.RoundStarted);
		}
		base.SetState(typeof(SubStatePlayChipEffect));
		while (base.isWaitState)
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator PlayRoundStartEffect()
	{
		this.PlayHpAndApRevivalEffect();
		IEnumerator roundStartScreenFunction = this.RoundStartScreenFunction();
		while (roundStartScreenFunction.MoveNext())
		{
			yield return null;
		}
		IEnumerator roundStartCameraMotionFunction = this.RoundStartCameraMotionFunction();
		while (roundStartCameraMotionFunction.MoveNext())
		{
			yield return null;
		}
		this.StopHpAndApRevivalEffect();
		yield break;
	}

	private void PlayHpAndApRevivalEffect()
	{
		bool flag = false;
		for (int i = 0; i < base.battleStateData.playerCharacters.Length; i++)
		{
			if (base.battleStateData.isRoundStartHpRevival[i] && !base.battleStateData.playerCharacters[i].isDied)
			{
				HitEffectParams hitEffectParams = base.battleStateData.waveChangeHpRevivalEffect[i];
				base.stateManager.threeDAction.PlayHitEffectAction(hitEffectParams, base.battleStateData.playerCharacters[i]);
				if (!flag)
				{
					flag = true;
					base.stateManager.soundPlayer.TryPlaySE(hitEffectParams);
				}
			}
		}
		bool flag2 = false;
		CharacterStateControl[] totalCharacters = base.battleStateData.GetTotalCharacters();
		for (int j = 0; j < base.battleStateData.totalCharacterLength; j++)
		{
			if (!totalCharacters[j].isDied && base.battleStateData.isRoundStartApRevival[j])
			{
				HitEffectParams hitEffectParams2 = base.battleStateData.roundChangeApRevivalEffect[j];
				base.stateManager.threeDAction.PlayHitEffectAction(hitEffectParams2, totalCharacters[j]);
				if (!flag2)
				{
					flag2 = true;
					base.stateManager.soundPlayer.TryPlaySE(hitEffectParams2);
				}
			}
		}
		base.stateManager.threeDAction.ShowAliveCharactersAction(base.battleStateData.GetTotalCharacters());
		base.stateManager.threeDAction.PlayIdleAnimationActiveCharacterAction(base.battleStateData.GetTotalCharacters());
	}

	private void StopHpAndApRevivalEffect()
	{
		base.stateManager.soundPlayer.TryStopSE(base.battleStateData.waveChangeHpRevivalEffect);
		base.stateManager.soundPlayer.TryStopSE(base.battleStateData.roundChangeApRevivalEffect);
		for (int i = 0; i < base.battleStateData.playerCharacters.Length; i++)
		{
			base.stateManager.threeDAction.StopHitEffectAction(new HitEffectParams[]
			{
				base.battleStateData.waveChangeHpRevivalEffect[i]
			});
		}
		for (int j = 0; j < base.battleStateData.totalCharacterLength; j++)
		{
			base.stateManager.threeDAction.StopHitEffectAction(new HitEffectParams[]
			{
				base.battleStateData.roundChangeApRevivalEffect[j]
			});
		}
	}

	protected virtual IEnumerator RoundStartCameraMotionFunction()
	{
		if (base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1)
		{
			base.stateManager.cameraControl.PlayCameraMotionAction("BigBoss/0002_command", base.battleStateData.stageSpawnPoint, true);
		}
		else
		{
			base.stateManager.cameraControl.PlayCameraMotionAction("0002_command", base.battleStateData.stageSpawnPoint, true);
		}
		yield return null;
		base.stateManager.cameraControl.StopCameraMotionAction("0002_command");
		base.stateManager.cameraControl.StopCameraMotionAction("BigBoss/0002_command");
		yield break;
	}

	private IEnumerator RoundStartScreenFunction()
	{
		int speedLimitRoundCount = base.hierarchyData.speedClearRound - base.battleStateData.totalRoundNumber;
		speedLimitRoundCount++;
		if (base.hierarchyData.limitRound > 0 && base.hierarchyData.speedClearRound > 0)
		{
			base.stateManager.uiControl.ApplyRoundLimitStartRevivalText(!BoolExtension.AllMachValue(false, base.battleStateData.isRoundStartApRevival), !BoolExtension.AllMachValue(false, base.battleStateData.isRoundStartHpRevival));
			base.stateManager.uiControl.ApplyRoundChallengeStartRevivalText(false, false);
			base.stateManager.SetBattleScreen(BattleScreen.RoundStartActionsLimit);
			if (speedLimitRoundCount > 0)
			{
				while (base.stateManager.battleUiComponents.roundLimitStart.AnimationTimeCheck())
				{
					yield return null;
				}
				base.stateManager.SetBattleScreen(BattleScreen.RoundStartActionsChallenge);
				while (base.stateManager.battleUiComponents.roundChallengeStart.AnimationIsPlaying())
				{
					yield return null;
				}
			}
			else
			{
				while (base.stateManager.battleUiComponents.roundLimitStart.AnimationIsPlaying())
				{
					yield return null;
				}
			}
		}
		else if (base.hierarchyData.limitRound > 0 && base.hierarchyData.speedClearRound <= 0)
		{
			base.stateManager.uiControl.ApplyRoundLimitStartRevivalText(!BoolExtension.AllMachValue(false, base.battleStateData.isRoundStartApRevival), !BoolExtension.AllMachValue(false, base.battleStateData.isRoundStartHpRevival));
			base.stateManager.SetBattleScreen(BattleScreen.RoundStartActionsLimit);
			while (base.stateManager.battleUiComponents.roundLimitStart.AnimationIsPlaying())
			{
				yield return null;
			}
		}
		else if (base.hierarchyData.limitRound <= 0 && base.hierarchyData.speedClearRound > 0)
		{
			if (speedLimitRoundCount > 0)
			{
				base.stateManager.uiControl.ApplyRoundChallengeStartRevivalText(!BoolExtension.AllMachValue(false, base.battleStateData.isRoundStartApRevival), !BoolExtension.AllMachValue(false, base.battleStateData.isRoundStartHpRevival));
				base.stateManager.SetBattleScreen(BattleScreen.RoundStartActionsChallenge);
				while (base.stateManager.battleUiComponents.roundChallengeStart.AnimationIsPlaying())
				{
					yield return null;
				}
			}
			else
			{
				base.stateManager.uiControl.ApplyRoundStartRevivalText(!BoolExtension.AllMachValue(false, base.battleStateData.isRoundStartApRevival), !BoolExtension.AllMachValue(false, base.battleStateData.isRoundStartHpRevival));
				base.stateManager.SetBattleScreen(BattleScreen.RoundStartActions);
				IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.RoundStartActionWaitSecond, null, null);
				while (wait.MoveNext())
				{
					yield return null;
				}
			}
		}
		else
		{
			base.stateManager.uiControl.ApplyRoundStartRevivalText(!BoolExtension.AllMachValue(false, base.battleStateData.isRoundStartApRevival), !BoolExtension.AllMachValue(false, base.battleStateData.isRoundStartHpRevival));
			base.stateManager.SetBattleScreen(BattleScreen.RoundStartActions);
			IEnumerator wait2 = base.stateManager.time.WaitForCertainPeriodTimeAction(base.stateManager.stateProperty.RoundStartActionWaitSecond, null, null);
			while (wait2.MoveNext())
			{
				yield return null;
			}
		}
		yield break;
	}

	private IEnumerator UpFunction()
	{
		SufferStateProperty.SufferType[] sufferTypes = new SufferStateProperty.SufferType[]
		{
			SufferStateProperty.SufferType.AttackUp,
			SufferStateProperty.SufferType.DefenceUp,
			SufferStateProperty.SufferType.SpAttackUp,
			SufferStateProperty.SufferType.SpDefenceUp,
			SufferStateProperty.SufferType.SpeedUp,
			SufferStateProperty.SufferType.HitRateUp,
			SufferStateProperty.SufferType.SatisfactionRateUp
		};
		IEnumerator function = this.HitEffectFunction(sufferTypes, CharacterAnimationType.revival);
		while (function.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator DownFunction()
	{
		SufferStateProperty.SufferType[] sufferTypes = new SufferStateProperty.SufferType[]
		{
			SufferStateProperty.SufferType.AttackDown,
			SufferStateProperty.SufferType.DefenceDown,
			SufferStateProperty.SufferType.SpAttackDown,
			SufferStateProperty.SufferType.SpDefenceDown,
			SufferStateProperty.SufferType.SpeedDown,
			SufferStateProperty.SufferType.HitRateDown,
			SufferStateProperty.SufferType.SatisfactionRateDown
		};
		IEnumerator function = this.HitEffectFunction(sufferTypes, CharacterAnimationType.hit);
		while (function.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator HitEffectFunction(SufferStateProperty.SufferType[] sufferTypes, CharacterAnimationType characterAnimationType)
	{
		List<CharacterStateControl> characters = new List<CharacterStateControl>();
		foreach (SufferStateProperty.SufferType sufferType in sufferTypes)
		{
			List<CharacterStateControl> temp = this.GetSufferCharacters(sufferType);
			foreach (CharacterStateControl character in temp)
			{
				SufferStateProperty property = character.currentSufferState.GetSufferStateProperty(sufferType);
				if (property.turnRate > 0f && !characters.Contains(character))
				{
					characters.Add(character);
				}
			}
		}
		if (characters.Count == 0)
		{
			yield break;
		}
		base.stateManager.uiControl.HideCharacterHUDFunction();
		base.stateManager.SetBattleScreen(BattleScreen.PoisonHit);
		if (base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1)
		{
			base.stateManager.cameraControl.PlayCameraMotionAction("BigBoss/0002_command", base.battleStateData.stageSpawnPoint, true);
		}
		else
		{
			base.stateManager.cameraControl.PlayCameraMotionAction("0002_command", base.battleStateData.stageSpawnPoint, true);
		}
		HitEffectParams[] hitEffectParams = null;
		foreach (SufferStateProperty.SufferType sufferType2 in sufferTypes)
		{
			string key = sufferType2.ToString();
			HitEffectParams[] temp2 = base.battleStateData.hitEffects.GetObject(key);
			if (temp2 != null)
			{
				hitEffectParams = temp2;
				break;
			}
		}
		base.stateManager.soundPlayer.TryPlaySE(hitEffectParams[0]);
		for (int i = 0; i < characters.Count; i++)
		{
			base.stateManager.threeDAction.PlayAnimationCharacterAction(characterAnimationType, new CharacterStateControl[]
			{
				characters[i]
			});
			base.stateManager.threeDAction.PlayHitEffectAction(hitEffectParams[i], characters[i]);
		}
		float waitSecond = base.stateManager.stateProperty.poisonHitEffectWaitSecond;
		IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(waitSecond, null, null);
		while (wait.MoveNext())
		{
			yield return null;
		}
		for (int j = 0; j < characters.Count; j++)
		{
			base.stateManager.threeDAction.StopHitEffectAction(new HitEffectParams[]
			{
				hitEffectParams[j]
			});
		}
		base.stateManager.soundPlayer.TryStopSE(hitEffectParams[0]);
		base.stateManager.soundPlayer.StopHitEffectSE();
		yield break;
	}

	private List<CharacterStateControl> GetSufferCharacters(SufferStateProperty.SufferType sufferType)
	{
		List<CharacterStateControl> list = new List<CharacterStateControl>();
		foreach (CharacterStateControl characterStateControl in base.battleStateData.GetTotalCharacters())
		{
			base.stateManager.threeDAction.ShowAliveCharactersAction(new CharacterStateControl[]
			{
				characterStateControl
			});
			base.stateManager.threeDAction.PlayIdleAnimationUndeadCharactersAction(new CharacterStateControl[]
			{
				characterStateControl
			});
			if (!characterStateControl.isDied)
			{
				if (characterStateControl.currentSufferState.FindSufferState(sufferType))
				{
					list.Add(characterStateControl);
				}
			}
		}
		return list;
	}
}
