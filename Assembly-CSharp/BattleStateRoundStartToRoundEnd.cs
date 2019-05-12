using BattleStateMachineInternal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BattleStateRoundStartToRoundEnd : BattleStateController
{
	private Action<bool> onPlayerWinner;

	private Action<bool> onPlayerFail;

	protected Action<bool> onTimeOver;

	private Action onNext;

	protected bool isSkillEnd;

	protected CharacterStateControl lastCharacter;

	protected BattleStateBase subStateCharacterDeadCheckFunction;

	protected BattleStateBase subStateCharacterRevivalFunction;

	protected BattleStateBase subStateSkillDetailsFunction;

	protected BattleStateBase subStateEnemyTurnStartAction;

	protected BattleStateBase subStatePlayInvocationEffectAction;

	protected BattleStateBase subStatePlayerCharacterAndSkillSelectFunction;

	protected BattleStateBase subStateOnHitPoisonDamageFunction;

	protected BattleStateBase subStateRoundStartAction;

	protected BattleStateBase subStatePlayChipEffect;

	protected BattleStateBase subStateWaitRandomSeedSync;

	protected BattleStateBase subStateWaitForCertainPeriodTimeAction;

	public BattleStateRoundStartToRoundEnd(Action OnExit, Action<bool> OnWin, Action<bool> OnFail, Action<bool> OnTimeOver, Action<EventState> OnExitGotEvent) : base(null, null, OnExitGotEvent)
	{
		this.onPlayerWinner = OnWin;
		this.onPlayerFail = OnFail;
		this.onTimeOver = OnTimeOver;
		this.onNext = OnExit;
	}

	private bool[] result
	{
		get
		{
			return base.stateManager.roundFunction.result;
		}
	}

	private bool isExit
	{
		get
		{
			return base.stateManager.roundFunction.isExit;
		}
	}

	protected bool onFreeze
	{
		get
		{
			return base.stateManager.roundFunction.onFreeze;
		}
	}

	protected override void AwakeThisState()
	{
		this.subStateCharacterDeadCheckFunction = new SubStateCharacterDeadCheckFunction(delegate()
		{
			base.stateManager.roundFunction.SetResult(true, false, false);
		}, delegate(bool isNextWave)
		{
			base.stateManager.roundFunction.SetResult(false, true, isNextWave);
		}, delegate(bool isNextWave)
		{
			base.stateManager.roundFunction.SetResult(false, false, isNextWave);
		});
		base.AddState(this.subStateCharacterDeadCheckFunction);
		this.subStateCharacterRevivalFunction = new SubStateCharacterRevivalFunction(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStateCharacterRevivalFunction);
		this.subStateSkillDetailsFunction = new SubStateSkillDetailsFunction(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStateSkillDetailsFunction);
		this.subStateEnemyTurnStartAction = new SubStateEnemyTurnStartAction(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStateEnemyTurnStartAction);
		this.subStatePlayInvocationEffectAction = new SubStatePlayInvocationEffectAction(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStatePlayInvocationEffectAction);
		this.subStatePlayerCharacterAndSkillSelectFunction = new SubStatePlayerCharacterAndSkillSelectFunction(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStatePlayerCharacterAndSkillSelectFunction);
		this.subStateOnHitPoisonDamageFunction = new SubStateOnHitPoisonDamageFunction(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStateOnHitPoisonDamageFunction);
		this.subStateRoundStartAction = new SubStateRoundStartAction(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStateRoundStartAction);
		this.subStatePlayChipEffect = new SubStatePlayChipEffect(null, new Action<EventState>(base.SendEventState), () => this.isSkillEnd);
		base.AddState(this.subStatePlayChipEffect);
		this.subStateWaitRandomSeedSync = new SubStateWaitRandomSeedSync(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStateWaitRandomSeedSync);
		this.subStateWaitForCertainPeriodTimeAction = new SubStateWaitForCertainPeriodTimeAction(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStateWaitForCertainPeriodTimeAction);
		base.AddState(new SubStatePlayStageEffect(null, new Action<EventState>(base.SendEventState)));
	}

	protected override void EnabledThisState()
	{
		base.stateManager.roundFunction.SetResult(true, false, false);
		base.battleStateData.SetOrderInSortedCharacter(null, -1);
		this.isSkillEnd = false;
	}

	protected override IEnumerator MainRoutine()
	{
		base.stateManager.SetBattleScreen(BattleScreen.RoundStart);
		IEnumerator startMsg = base.stateManager.roundFunction.RunRoundStartMessage();
		while (startMsg.MoveNext())
		{
			object obj = startMsg.Current;
			yield return obj;
		}
		base.SetState(this.subStateWaitRandomSeedSync.GetType());
		while (base.isWaitState)
		{
			yield return null;
		}
		foreach (CharacterStateControl character in base.battleStateData.playerCharacters)
		{
			character.InitializeSpecialCorrectionStatus();
		}
		foreach (CharacterStateControl character2 in base.battleStateData.enemies)
		{
			character2.InitializeSpecialCorrectionStatus();
		}
		if (base.battleStateData.currentRoundNumber <= 1)
		{
			foreach (CharacterStateControl character3 in base.battleStateData.playerCharacters)
			{
				character3.InitChipEffectCountForWave();
			}
			foreach (CharacterStateControl character4 in base.battleStateData.enemies)
			{
				character4.InitChipEffectCountForWave();
			}
		}
		base.SetState(this.subStateRoundStartAction.GetType());
		while (base.isWaitState)
		{
			yield return null;
		}
		List<CharacterStateControl> sortedCharacters = this.GetSortedSpeedCharacerList();
		base.stateManager.roundFunction.ResetPlayers();
		base.stateManager.threeDAction.PlayIdleAnimationUndeadCharactersAction(base.battleStateData.GetTotalCharacters());
		base.battleStateData.enableRotateCam = true;
		base.battleStateData.apRevival = new int[sortedCharacters.Count];
		for (int sortedIndex = 0; sortedIndex < sortedCharacters.Count; sortedIndex++)
		{
			if (!sortedCharacters[sortedIndex].isDied)
			{
				sortedCharacters[sortedIndex].InitChipEffectCountForTurn();
				this.lastCharacter = sortedCharacters[sortedIndex];
				base.stateManager.fraudCheck.FraudCheckOverflowMaxHp(sortedCharacters[sortedIndex]);
				base.stateManager.threeDAction.PlayIdleAnimationUndeadCharactersAction(sortedCharacters.ToArray());
				CharacterStateControl sortedCharacter = sortedCharacters[sortedIndex];
				base.battleStateData.currentActiveCharacter = sortedIndex;
				base.SetState(this.subStateWaitForCertainPeriodTimeAction.GetType());
				while (base.isWaitState)
				{
					yield return null;
				}
				sortedCharacter.OnChipTrigger(EffectStatusBase.EffectTriggerType.TurnStarted, false);
				base.SetState(this.subStatePlayChipEffect.GetType());
				while (base.isWaitState)
				{
					yield return null;
				}
				base.battleStateData.onSkillTrigger = false;
				base.stateManager.roundFunction.InitRunSufferFlags();
				base.stateManager.roundFunction.RunSufferBeforeCommand(sortedCharacters, sortedCharacter);
				if (!sortedCharacter.isEnemy)
				{
					IEnumerator playerTurnFunction = this.PlayerTurnFunction();
					while (playerTurnFunction.MoveNext())
					{
						yield return null;
					}
				}
				else
				{
					IEnumerator enemyTurnFunction = this.EnemyTurnFunction();
					while (enemyTurnFunction.MoveNext())
					{
						yield return null;
					}
				}
				base.stateManager.uiControl.HideCharacterHUDFunction();
				base.stateManager.roundFunction.RunSufferAfterCommand(sortedCharacters, sortedCharacter);
				if (this.onFreeze)
				{
					IEnumerator action = base.stateManager.roundFunction.RunOnFreezAction(sortedCharacter);
					while (action.MoveNext())
					{
						object obj2 = action.Current;
						yield return obj2;
					}
				}
				else
				{
					IEnumerator skillFunction = this.SkillFunction();
					while (skillFunction.MoveNext())
					{
						yield return null;
					}
				}
				base.stateManager.uiControl.HideCharacterHUDFunction();
				base.SetState(this.subStateCharacterDeadCheckFunction.GetType());
				while (base.isWaitState)
				{
					yield return null;
				}
				if (!this.isExit)
				{
					IEnumerator waveEndFunction = this.WaveEndFunction();
					while (waveEndFunction.MoveNext())
					{
						yield return null;
					}
					yield break;
				}
				if (sortedCharacter.isRecommand)
				{
					sortedCharacter.isRecommand = false;
					sortedCharacter.targetCharacter = null;
					sortedCharacter.isSelectSkill = -1;
					sortedIndex--;
				}
				else
				{
					base.battleStateData.currentTurnNumber++;
					base.battleStateData.SetOrderInSortedCharacter(sortedCharacters, sortedIndex);
				}
				base.SetState(this.subStateWaitForCertainPeriodTimeAction.GetType());
				while (base.isWaitState)
				{
					yield return null;
				}
				sortedCharacter.OnChipTrigger(EffectStatusBase.EffectTriggerType.TurnEnd, false);
				base.SetState(this.subStatePlayChipEffect.GetType());
				while (base.isWaitState)
				{
					yield return null;
				}
				IEnumerator turnEndFunction = this.TurnEndFunction();
				while (turnEndFunction.MoveNext())
				{
					yield return null;
				}
			}
		}
		foreach (CharacterStateControl character5 in base.battleStateData.playerCharacters)
		{
			character5.OnChipTrigger(EffectStatusBase.EffectTriggerType.RoundEnd, false);
		}
		foreach (CharacterStateControl character6 in base.battleStateData.enemies)
		{
			character6.OnChipTrigger(EffectStatusBase.EffectTriggerType.RoundEnd, false);
		}
		base.SetState(this.subStatePlayChipEffect.GetType());
		while (base.isWaitState)
		{
			yield return null;
		}
		base.SetState(this.subStateOnHitPoisonDamageFunction.GetType());
		while (base.isWaitState)
		{
			yield return null;
		}
		base.battleStateData.reqestStageEffectTriggerList.Add(EffectStatusBase.EffectTriggerType.RoundEnd);
		base.SetState(typeof(SubStatePlayStageEffect));
		while (base.isWaitState)
		{
			yield return null;
		}
		base.SetState(this.subStateCharacterDeadCheckFunction.GetType());
		while (base.isWaitState)
		{
			yield return null;
		}
		if (!this.isExit)
		{
			IEnumerator waveEndFunction2 = this.WaveEndFunction();
			while (waveEndFunction2.MoveNext())
			{
				yield return null;
			}
			yield break;
		}
		base.stateManager.roundFunction.OnHitDamageBgmChangeFunction();
		base.SetState(this.subStateCharacterRevivalFunction.GetType());
		while (base.isWaitState)
		{
			yield return null;
		}
		yield break;
	}

	protected virtual IEnumerator PlayerTurnFunction()
	{
		if (!this.onFreeze)
		{
			base.SetState(this.subStatePlayerCharacterAndSkillSelectFunction.GetType());
			while (base.isWaitState)
			{
				yield return null;
			}
		}
		base.battleStateData.enableRotateCam = false;
		yield break;
	}

	protected virtual IEnumerator EnemyTurnFunction()
	{
		base.SetState(this.subStateEnemyTurnStartAction.GetType());
		while (base.isWaitState)
		{
			yield return null;
		}
		if (!this.onFreeze)
		{
			base.stateManager.targetSelect.EnemyAICharacterAndSkillSelectFunction(this.lastCharacter);
		}
		yield break;
	}

	protected virtual IEnumerator SkillFunction()
	{
		base.stateManager.SetBattleScreen(BattleScreen.RoundActions);
		base.stateManager.uiControl.ApplyTurnActionBarSwipeout(false);
		foreach (CharacterStateControl character in base.battleStateData.playerCharacters)
		{
			character.OnChipTrigger(EffectStatusBase.EffectTriggerType.AttackHit, false);
			character.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillStartedApMax, false);
			character.OnChipTrigger(EffectStatusBase.EffectTriggerType.AttackCommandedTarget, false);
			character.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillSpecies, false);
			character.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillTargetSpecies, false);
			character.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillAttribute, false);
			character.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillRecieveAttribute, false);
		}
		foreach (CharacterStateControl character2 in base.battleStateData.enemies)
		{
			character2.OnChipTrigger(EffectStatusBase.EffectTriggerType.AttackHit, false);
			character2.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillStartedApMax, false);
			character2.OnChipTrigger(EffectStatusBase.EffectTriggerType.AttackCommandedTarget, false);
			character2.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillSpecies, false);
			character2.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillTargetSpecies, false);
			character2.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillAttribute, false);
			character2.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillRecieveAttribute, false);
		}
		CharacterStateControl[] totalCharacters = base.stateManager.battleStateData.GetTotalCharacters();
		if (totalCharacters.Where((CharacterStateControl item) => item.stagingChipIdList.Count > 0).Any<CharacterStateControl>())
		{
			base.stateManager.cameraControl.PlayTweenCameraMotion(base.battleStateData.commandSelectTweenTargetCamera, null);
			base.stateManager.cameraControl.SetCameraLengthAction(base.battleStateData.commandSelectTweenTargetCamera);
		}
		base.SetState(this.subStateWaitForCertainPeriodTimeAction.GetType());
		while (base.isWaitState)
		{
			yield return null;
		}
		base.SetState(this.subStatePlayChipEffect.GetType());
		while (base.isWaitState)
		{
			yield return null;
		}
		IEnumerator skillStart = this.PlayAdventureScene(BattleAdventureSceneManager.TriggerType.SkillStart);
		while (skillStart.MoveNext())
		{
			yield return null;
		}
		if (this.lastCharacter.isEnemy)
		{
			BattleWave batteWave = base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber];
			if (!batteWave.enemiesInfinityApFlag[this.lastCharacter.myIndex])
			{
				this.lastCharacter.currentSkillStatus.OnAttackUseAttackerAp(this.lastCharacter);
			}
		}
		else
		{
			this.lastCharacter.currentSkillStatus.OnAttackUseAttackerAp(this.lastCharacter);
		}
		if (this.lastCharacter.currentSkillStatus.skillType == SkillType.Attack)
		{
			base.battleStateData.isInvocationEffectPlaying = true;
		}
		base.stateManager.time.SetPlaySpeed(base.hierarchyData.on2xSpeedPlay, base.battleStateData.isShowMenuWindow);
		base.stateManager.uiControl.ApplyTurnActionBarSwipeout(true);
		base.SetState(this.subStatePlayInvocationEffectAction.GetType());
		while (base.isWaitState)
		{
			yield return null;
		}
		if (this.lastCharacter.currentSkillStatus.skillType == SkillType.Attack)
		{
			base.battleStateData.isInvocationEffectPlaying = false;
		}
		base.stateManager.time.SetPlaySpeed(base.hierarchyData.on2xSpeedPlay, base.battleStateData.isShowMenuWindow);
		if (!this.lastCharacter.isEnemy)
		{
			base.battleStateData.lastAttackPlayerCharacterIndex = this.lastCharacter.myIndex;
		}
		base.stateManager.uiControl.ApplyTurnActionBarSwipeout(false);
		base.stateManager.uiControl.CharacterHudResetAndUpdate(false);
		base.battleStateData.SetChipSkillFlag(false);
		base.battleStateData.isConfusionAttack = base.stateManager.roundFunction.onConfusion;
		base.SetState(this.subStateSkillDetailsFunction.GetType());
		while (base.isWaitState)
		{
			yield return null;
		}
		base.battleStateData.isConfusionAttack = false;
		this.isSkillEnd = true;
		CharacterStateControl[] playerCharacters = base.stateManager.battleStateData.playerCharacters.Where((CharacterStateControl item) => !item.isDied && item.stagingChipIdList.Count > 0).ToArray<CharacterStateControl>();
		CharacterStateControl[] enemies = base.stateManager.battleStateData.enemies.Where((CharacterStateControl item) => !item.isDied && item.stagingChipIdList.Count > 0).ToArray<CharacterStateControl>();
		if (playerCharacters.Count<CharacterStateControl>() + enemies.Count<CharacterStateControl>() > 1)
		{
			base.stateManager.cameraControl.PlayTweenCameraMotion(base.battleStateData.commandSelectTweenTargetCamera, null);
			base.stateManager.cameraControl.SetCameraLengthAction(base.battleStateData.commandSelectTweenTargetCamera);
		}
		else if (playerCharacters.Count<CharacterStateControl>() == 1)
		{
			string cameraKey = "skillF";
			base.stateManager.cameraControl.PlayCameraMotionActionCharacter(cameraKey, playerCharacters[0]);
			base.stateManager.cameraControl.SetTime(cameraKey, 1f);
		}
		else if (enemies.Count<CharacterStateControl>() == 1)
		{
			string cameraKey2 = "skillF";
			base.stateManager.cameraControl.PlayCameraMotionActionCharacter(cameraKey2, enemies[0]);
			base.stateManager.cameraControl.SetTime(cameraKey2, 1f);
		}
		base.SetState(this.subStateWaitForCertainPeriodTimeAction.GetType());
		while (base.isWaitState)
		{
			yield return null;
		}
		base.SetState(this.subStatePlayChipEffect.GetType());
		while (base.isWaitState)
		{
			yield return null;
		}
		base.stateManager.uiControl.ApplySkillName(false, string.Empty, null);
		IEnumerator skillEnd = this.PlayAdventureScene(BattleAdventureSceneManager.TriggerType.SkillEnd);
		while (skillEnd.MoveNext())
		{
			yield return null;
		}
		yield break;
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

	protected virtual IEnumerator TurnEndFunction()
	{
		yield break;
	}

	private IEnumerator WaveEndFunction()
	{
		foreach (CharacterStateControl character in base.battleStateData.playerCharacters)
		{
			character.OnChipTrigger(EffectStatusBase.EffectTriggerType.WaveEnd, false);
		}
		foreach (CharacterStateControl character2 in base.battleStateData.enemies)
		{
			character2.OnChipTrigger(EffectStatusBase.EffectTriggerType.WaveEnd, false);
		}
		base.SetState(this.subStatePlayChipEffect.GetType());
		while (base.isWaitState)
		{
			yield return null;
		}
		yield break;
	}

	protected override void DisabledThisState()
	{
		this.isSkillEnd = false;
		if (base.isGotEvent)
		{
			return;
		}
		if (BattleStateManager.current.onServerConnect)
		{
			DataMng.Instance().WD_ReqDngResult.clearRound = base.battleStateData.totalRoundNumber;
		}
		if (base.battleStateData.totalRoundNumber > base.hierarchyData.limitRound && base.hierarchyData.limitRound > 0)
		{
			this.onTimeOver(false);
			return;
		}
		if (this.isExit)
		{
			this.SaveRecoverData();
			if (this.onNext != null)
			{
				this.onNext();
			}
		}
		else if (this.result[0])
		{
			this.SaveRecoverData();
			if (this.onPlayerWinner != null)
			{
				this.onPlayerWinner(this.result[1]);
			}
		}
		else if (this.onPlayerFail != null)
		{
			this.onPlayerFail(this.result[1]);
		}
	}

	protected virtual List<CharacterStateControl> GetSortedSpeedCharacerList()
	{
		List<CharacterStateControl> list = new List<CharacterStateControl>(base.battleStateData.GetTotalCharacters());
		foreach (CharacterStateControl characterStateControl in list)
		{
			characterStateControl.SpeedRandomize(base.hierarchyData.onEnableRandomValue);
			base.stateManager.fraudCheck.FraudCheckOverflowMaxSpeed(characterStateControl);
		}
		CharacterStateControl[] collection = CharacterStateControlSorter.SortedSpeedEnemyPriority(list.ToArray());
		list = new List<CharacterStateControl>(collection);
		base.battleStateData.currentTurnNumber = 0;
		base.battleStateData.SetOrderInSortedCharacter(list, -1);
		if (!base.stateManager.onEnableTutorial)
		{
			base.hierarchyData.onEnableRandomValue = true;
		}
		return list;
	}

	protected virtual void SaveRecoverData()
	{
		base.stateManager.recover.Save();
	}
}
