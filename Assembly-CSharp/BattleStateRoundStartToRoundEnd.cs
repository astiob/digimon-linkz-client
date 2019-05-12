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

	protected BattleStateBase subStateInstantDeathFunction;

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
		}, null);
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
		this.subStateInstantDeathFunction = new SubStateInstantDeathFunction(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStateInstantDeathFunction);
	}

	protected override void EnabledThisState()
	{
		base.stateManager.roundFunction.SetResult(true, false, false);
		base.battleStateData.SetOrderInSortedCharacter(null, -1);
		this.isSkillEnd = false;
		CharacterStateControl[] totalCharacters = this.GetTotalCharacters();
		base.battleStateData.apRevival = new int[totalCharacters.Length];
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
		if (base.battleStateData.currentRoundNumber <= 1)
		{
			foreach (CharacterStateControl characterStateControl in this.GetTotalCharacters())
			{
				characterStateControl.InitChipEffectCountForWave();
				characterStateControl.ResetSkillUseCountForWave();
			}
		}
		base.SetState(this.subStateRoundStartAction.GetType());
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
			IEnumerator waveEndFunction = this.WaveEndFunction();
			while (waveEndFunction.MoveNext())
			{
				yield return null;
			}
			yield break;
		}
		Queue<CharacterStateControl> sortedCharacters = this.GetSortedSpeedCharacerList(null);
		List<CharacterStateControl> sortedCharactersList = sortedCharacters.ToList<CharacterStateControl>();
		base.battleStateData.apRevival = new int[sortedCharacters.Count];
		base.stateManager.roundFunction.ResetPlayers();
		base.stateManager.threeDAction.PlayIdleAnimationUndeadCharactersAction(base.battleStateData.GetTotalCharacters());
		base.battleStateData.enableRotateCam = true;
		while (sortedCharacters.Count > 0)
		{
			base.battleStateData.currentSelectCharacterState = sortedCharacters.Peek();
			if (base.battleStateData.currentSelectCharacterState.isDied)
			{
				base.battleStateData.currentSelectCharacterState.skillOrder = -1;
				sortedCharacters.Dequeue();
			}
			else
			{
				CharacterStateControl sortedCharacter = base.battleStateData.currentSelectCharacterState;
				base.stateManager.threeDAction.PlayIdleAnimationUndeadCharactersAction(this.GetTotalCharacters());
				sortedCharacter.InitChipEffectCountForTurn();
				this.lastCharacter = sortedCharacter;
				base.stateManager.fraudCheck.FraudCheckOverflowMaxHp(sortedCharacter);
				base.SetState(this.subStateWaitForCertainPeriodTimeAction.GetType());
				while (base.isWaitState)
				{
					yield return null;
				}
				sortedCharacter.OnChipTrigger(EffectStatusBase.EffectTriggerType.TurnStarted);
				base.SetState(this.subStatePlayChipEffect.GetType());
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
				base.battleStateData.onSkillTrigger = false;
				base.stateManager.roundFunction.InitRunSufferFlags();
				yield return base.stateManager.roundFunction.RunSufferBeforeCommand(sortedCharactersList, sortedCharacter);
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
				base.stateManager.roundFunction.RunSufferAfterCommand(sortedCharactersList, sortedCharacter);
				if (this.onFreeze || this.lastCharacter.currentSkillStatus.IsNotingAffectEffect())
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
				sortedCharacter.currentSufferState.TurnUpdate();
				base.stateManager.uiControl.HideCharacterHUDFunction();
				base.SetState(this.subStateCharacterDeadCheckFunction.GetType());
				while (base.isWaitState)
				{
					yield return null;
				}
				if (!this.isExit)
				{
					IEnumerator waveEndFunction3 = this.WaveEndFunction();
					while (waveEndFunction3.MoveNext())
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
				}
				else
				{
					base.battleStateData.currentTurnNumber++;
					base.battleStateData.SetOrderInSortedCharacter(sortedCharacters, -1);
				}
				base.SetState(this.subStateWaitForCertainPeriodTimeAction.GetType());
				while (base.isWaitState)
				{
					yield return null;
				}
				sortedCharacter.OnChipTrigger(EffectStatusBase.EffectTriggerType.TurnEnd);
				base.SetState(this.subStatePlayChipEffect.GetType());
				while (base.isWaitState)
				{
					yield return null;
				}
				base.SetState(this.subStateInstantDeathFunction.GetType());
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
					IEnumerator waveEndFunction4 = this.WaveEndFunction();
					while (waveEndFunction4.MoveNext())
					{
						yield return null;
					}
					yield break;
				}
				IEnumerator turnEndFunction = this.TurnEndFunction();
				while (turnEndFunction.MoveNext())
				{
					yield return null;
				}
				if (!sortedCharacter.isRecommand)
				{
					base.battleStateData.currentSelectCharacterState.skillOrder = -1;
					sortedCharacters.Dequeue();
				}
				sortedCharacters = this.GetSortedSpeedCharacerList(sortedCharacters.ToArray());
			}
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
		base.SetState(this.subStateWaitForCertainPeriodTimeAction.GetType());
		while (base.isWaitState)
		{
			yield return null;
		}
		foreach (CharacterStateControl characterStateControl2 in this.GetTotalCharacters())
		{
			characterStateControl2.OnChipTrigger(EffectStatusBase.EffectTriggerType.RoundEnd);
		}
		base.SetState(this.subStatePlayChipEffect.GetType());
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
			IEnumerator waveEndFunction5 = this.WaveEndFunction();
			while (waveEndFunction5.MoveNext())
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
		IEnumerator skillStart = this.PlayAdventureScene(BattleAdventureSceneManager.TriggerType.SkillStart);
		while (skillStart.MoveNext())
		{
			yield return null;
		}
		IEnumerator playSkill = this.PlaySkill();
		while (playSkill.MoveNext())
		{
			yield return null;
		}
		IEnumerator skillEnd = this.PlayAdventureScene(BattleAdventureSceneManager.TriggerType.SkillEnd);
		while (skillEnd.MoveNext())
		{
			yield return null;
		}
		base.stateManager.uiControl.ApplySkillName(false, string.Empty, null);
		yield break;
	}

	private IEnumerator PlaySkill()
	{
		foreach (CharacterStateControl characterStateControl in this.GetTotalCharacters())
		{
			characterStateControl.OnChipTrigger(EffectStatusBase.EffectTriggerType.AttackStarted);
			characterStateControl.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillStartedApMax);
			characterStateControl.OnChipTrigger(EffectStatusBase.EffectTriggerType.AttackCommandedTarget);
			characterStateControl.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillSpecies);
			characterStateControl.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillTargetSpecies);
			characterStateControl.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillDamageStartedSend);
			characterStateControl.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillDamageStartedRecieve);
			characterStateControl.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillAttributeStartedSend);
			characterStateControl.OnChipTrigger(EffectStatusBase.EffectTriggerType.SkillAttributeStartedRecieve);
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
		if (this.lastCharacter.isEnemy)
		{
			BattleWave battleWave = base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber];
			if (!battleWave.enemiesInfinityApFlag[this.lastCharacter.myIndex])
			{
				this.lastCharacter.currentSkillStatus.OnAttackUseAttackerAp(this.lastCharacter);
			}
		}
		else
		{
			this.lastCharacter.currentSkillStatus.OnAttackUseAttackerAp(this.lastCharacter);
		}
		this.lastCharacter.AddSkillUseCount(this.lastCharacter.isSelectSkill, -1);
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
		CharacterStateControl[] playerCharacters = base.stateManager.battleStateData.playerCharacters.Where((CharacterStateControl item) => item.stagingChipIdList.Count > 0).ToArray<CharacterStateControl>();
		CharacterStateControl[] enemies = base.stateManager.battleStateData.enemies.Where((CharacterStateControl item) => item.stagingChipIdList.Count > 0).ToArray<CharacterStateControl>();
		if (playerCharacters.Count<CharacterStateControl>() + enemies.Count<CharacterStateControl>() > 1)
		{
			base.stateManager.cameraControl.PlayTweenCameraMotion(base.battleStateData.commandSelectTweenTargetCamera, null);
			base.stateManager.cameraControl.SetCameraLengthAction(base.battleStateData.commandSelectTweenTargetCamera);
			CharacterStateControl[] characters = base.battleStateData.GetTotalCharacters().Where((CharacterStateControl item) => !item.isDied).ToArray<CharacterStateControl>();
			base.stateManager.threeDAction.ShowAllCharactersAction(characters);
			base.stateManager.threeDAction.PlayIdleAnimationActiveCharacterAction(characters);
		}
		else if (playerCharacters.Count<CharacterStateControl>() == 1)
		{
			string cameraKey = "skillF";
			base.stateManager.cameraControl.PlayCameraMotionActionCharacter(cameraKey, playerCharacters[0]);
			base.stateManager.cameraControl.SetTime(cameraKey, 1f);
			CharacterStateControl[] characters2 = base.battleStateData.playerCharacters.Where((CharacterStateControl item) => !item.isDied).ToArray<CharacterStateControl>();
			base.stateManager.threeDAction.ShowAllCharactersAction(characters2);
			base.stateManager.threeDAction.PlayIdleAnimationActiveCharacterAction(characters2);
		}
		else if (enemies.Count<CharacterStateControl>() == 1)
		{
			string cameraKey2 = "skillF";
			base.stateManager.cameraControl.PlayCameraMotionActionCharacter(cameraKey2, enemies[0]);
			base.stateManager.cameraControl.SetTime(cameraKey2, 1f);
			CharacterStateControl[] characters3 = base.battleStateData.enemies.Where((CharacterStateControl item) => !item.isDied).ToArray<CharacterStateControl>();
			base.stateManager.threeDAction.ShowAllCharactersAction(characters3);
			base.stateManager.threeDAction.PlayIdleAnimationActiveCharacterAction(characters3);
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
		EffectStatusBase.EffectTriggerType[] triggers = new EffectStatusBase.EffectTriggerType[]
		{
			EffectStatusBase.EffectTriggerType.AttackStarted,
			EffectStatusBase.EffectTriggerType.SkillStartedApMax,
			EffectStatusBase.EffectTriggerType.AttackCommandedTarget,
			EffectStatusBase.EffectTriggerType.SkillSpecies,
			EffectStatusBase.EffectTriggerType.SkillTargetSpecies,
			EffectStatusBase.EffectTriggerType.SkillDamageStartedSend,
			EffectStatusBase.EffectTriggerType.SkillDamageStartedRecieve,
			EffectStatusBase.EffectTriggerType.SkillAttributeStartedSend,
			EffectStatusBase.EffectTriggerType.SkillAttributeStartedRecieve
		};
		foreach (CharacterStateControl characterStateControl2 in this.GetTotalCharacters())
		{
			characterStateControl2.RemovePotencyChip(triggers);
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
		foreach (CharacterStateControl characterStateControl in this.GetTotalCharacters())
		{
			characterStateControl.OnChipTrigger(EffectStatusBase.EffectTriggerType.WaveEnd);
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
			foreach (CharacterStateControl characterStateControl in base.battleStateData.GetTotalCharacters())
			{
				characterStateControl.currentSufferState.RemoveSufferState(SufferStateProperty.SufferType.Escape, false);
			}
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

	protected virtual CharacterStateControl[] GetTotalCharacters()
	{
		return base.battleStateData.GetTotalCharacters();
	}

	public Queue<CharacterStateControl> GetSortedSpeedCharacerList(CharacterStateControl[] controlers = null)
	{
		BattleStateData battleStateData = BattleStateManager.current.battleStateData;
		BattleStateManager current = BattleStateManager.current;
		BattleStateHierarchyData hierarchyData = BattleStateManager.current.hierarchyData;
		CharacterStateControl[] collection = (controlers != null) ? controlers : this.GetTotalCharacters();
		Queue<CharacterStateControl> queue = new Queue<CharacterStateControl>(collection);
		if (controlers == null)
		{
			foreach (CharacterStateControl characterStateControl in queue)
			{
				characterStateControl.SpeedRandomize(hierarchyData.onEnableRandomValue);
				current.fraudCheck.FraudCheckOverflowMaxSpeed(characterStateControl);
			}
		}
		CharacterStateControl[] collection2 = CharacterStateControlSorter.SortedSpeedEnemyPriority(queue.ToArray());
		queue = new Queue<CharacterStateControl>(collection2);
		battleStateData.currentTurnNumber = 0;
		battleStateData.SetOrderInSortedCharacter(queue, -1);
		if (!current.onEnableTutorial)
		{
			hierarchyData.onEnableRandomValue = true;
		}
		return queue;
	}

	protected virtual void SaveRecoverData()
	{
	}
}
