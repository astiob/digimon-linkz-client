using MultiBattle.Tools;
using System;
using System.Collections;
using System.Collections.Generic;

public class BattleStatePvPRoundStartToRoundEnd : BattleStateRoundStartToRoundEnd
{
	private bool isCheckEnd;

	private bool isWin;

	private BattleStateBase subStateWaitAllPlayers;

	private BattleStateBase subStateWaitEnemySkillSelect;

	public BattleStatePvPRoundStartToRoundEnd(Action OnExit, Action<bool> OnWin, Action<bool> OnFail, Action<bool> OnTimeOver, Action<EventState> OnExitGotEvent) : base(OnExit, OnWin, OnFail, OnTimeOver, OnExitGotEvent)
	{
	}

	private int limitRound
	{
		get
		{
			int num = ClassSingleton<MultiBattleData>.Instance.MaxRoundNum - base.battleStateData.currentRoundNumber;
			if (num < 0)
			{
				num = 0;
			}
			return num;
		}
	}

	protected override void AwakeThisState()
	{
		this.subStateCharacterDeadCheckFunction = new SubStatePvPCharacterDeadCheckFunction(delegate()
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
		this.subStateSkillDetailsFunction = new SubStatePvPSkillDetailsFunction(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStateSkillDetailsFunction);
		this.subStateEnemyTurnStartAction = new SubStateEnemyTurnStartAction(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStateEnemyTurnStartAction);
		this.subStatePlayInvocationEffectAction = new SubStatePlayInvocationEffectAction(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStatePlayInvocationEffectAction);
		this.subStatePlayerCharacterAndSkillSelectFunction = new SubStatePlayerCharacterAndSkillSelectFunction(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStatePlayerCharacterAndSkillSelectFunction);
		this.subStateOnHitPoisonDamageFunction = new SubStateOnHitPoisonDamageFunction(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStateOnHitPoisonDamageFunction);
		this.subStateRoundStartAction = new SubStatePvPRoundStartAction(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStateRoundStartAction);
		this.subStatePlayChipEffect = new SubStatePlayChipEffect(null, new Action<EventState>(base.SendEventState), () => this.isSkillEnd);
		base.AddState(this.subStatePlayChipEffect);
		this.subStateWaitRandomSeedSync = new SubStateWaitRandomSeedSync(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStateWaitRandomSeedSync);
		this.subStateWaitAllPlayers = new SubStateWaitAllPlayers(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStateWaitAllPlayers);
		this.subStateWaitEnemySkillSelect = new SubStateWaitEnemySkillSelect(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStateWaitEnemySkillSelect);
		this.subStateWaitForCertainPeriodTimeAction = new SubStateWaitForCertainPeriodTimeAction(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStateWaitForCertainPeriodTimeAction);
		base.AddState(new SubStatePlayStageEffect(null, new Action<EventState>(base.SendEventState)));
	}

	protected override void EnabledThisState()
	{
		base.EnabledThisState();
		base.stateManager.uiControlPvP.HideLoading();
	}

	protected override IEnumerator MainRoutine()
	{
		base.stateManager.uiControlPvP.ShowLeftRoundUI(this.limitRound);
		base.stateManager.uiControlPvP.ShowEmotionButton();
		IEnumerator mainRoutine = base.MainRoutine();
		while (mainRoutine.MoveNext())
		{
			object obj = mainRoutine.Current;
			yield return obj;
		}
		yield break;
	}

	protected override IEnumerator PlayerTurnFunction()
	{
		base.stateManager.uiControlPvP.ApplySetAlwaysUIObject(true);
		if (!base.onFreeze)
		{
			base.SetState(this.subStatePlayerCharacterAndSkillSelectFunction.GetType());
			while (base.isWaitState)
			{
				yield return null;
			}
			base.SetState(this.subStateWaitAllPlayers.GetType());
			while (base.isWaitState)
			{
				yield return null;
			}
		}
		base.battleStateData.enableRotateCam = false;
		base.stateManager.uiControlPvP.ApplySetAlwaysUIObject(false);
		if (base.battleStateData.isShowRetireWindow)
		{
			base.battleStateData.isShowRetireWindow = false;
			base.stateManager.uiControl.ApplyShowRetireWindow(false, null);
		}
		yield break;
	}

	protected override IEnumerator EnemyTurnFunction()
	{
		base.SetState(this.subStateEnemyTurnStartAction.GetType());
		while (base.isWaitState)
		{
			yield return null;
		}
		if (!base.onFreeze)
		{
			base.SetState(this.subStateWaitEnemySkillSelect.GetType());
			while (base.isWaitState)
			{
				yield return null;
			}
		}
		yield break;
	}

	protected override IEnumerator TurnEndFunction()
	{
		yield break;
	}

	protected override void DisabledThisState()
	{
		if (base.battleStateData.currentRoundNumber > ClassSingleton<MultiBattleData>.Instance.MaxRoundNum)
		{
			this.onTimeOver(this.DecideWinner(false));
			return;
		}
		base.DisabledThisState();
	}

	protected override List<CharacterStateControl> GetSortedSpeedCharacerList()
	{
		CharacterStateControl[] collection;
		if (base.stateManager.pvpFunction.IsOwner)
		{
			collection = base.battleStateData.GetTotalCharacters();
		}
		else
		{
			collection = base.battleStateData.GetTotalCharactersEnemyFirst();
		}
		List<CharacterStateControl> list = new List<CharacterStateControl>(collection);
		foreach (CharacterStateControl characterStateControl in list)
		{
			characterStateControl.SpeedRandomize(base.hierarchyData.onEnableRandomValue);
			base.stateManager.fraudCheck.FraudCheckOverflowMaxSpeed(characterStateControl);
		}
		CharacterStateControl[] collection2 = CharacterStateControlSorter.SortedSpeedLuck(list.ToArray());
		list = new List<CharacterStateControl>(collection2);
		base.battleStateData.currentTurnNumber = 0;
		base.battleStateData.SetOrderInSortedCharacter(list, -1);
		if (!base.stateManager.onEnableTutorial)
		{
			base.hierarchyData.onEnableRandomValue = true;
		}
		return list;
	}

	protected override void SaveRecoverData()
	{
	}

	private void PvPEnemyAICharacterAndSkillSelectFunction(CharacterStateControl currentCharacter)
	{
		CharacterStateControl[] playerCharacters = base.battleStateData.playerCharacters;
		currentCharacter.isSelectSkill = currentCharacter.SkillIdToIndexOf(base.stateManager.publicAttackSkillId);
		for (int i = 0; i < playerCharacters.Length; i++)
		{
			if (!playerCharacters[i].isDied)
			{
				currentCharacter.targetCharacter = playerCharacters[i];
				break;
			}
		}
	}

	private bool DecideWinner(bool isDiedCheck = false)
	{
		this.isCheckEnd = false;
		foreach (Action action in new List<Action>
		{
			new Action(this.HPSummation),
			new Action(this.HPRatio),
			new Action(this.AliveCount),
			new Action(this.LastPlayMonster)
		})
		{
			action();
			if (this.isCheckEnd)
			{
				break;
			}
		}
		return this.isWin;
	}

	private void HPSummation()
	{
		int num = 0;
		int num2 = 0;
		foreach (CharacterStateControl characterStateControl in base.battleStateData.playerCharacters)
		{
			num += characterStateControl.hp;
		}
		foreach (CharacterStateControl characterStateControl2 in base.battleStateData.enemies)
		{
			num2 += characterStateControl2.hp;
		}
		if (num != num2)
		{
			this.isCheckEnd = true;
			this.isWin = (num > num2);
		}
	}

	private void HPRatio()
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		foreach (CharacterStateControl characterStateControl in base.battleStateData.playerCharacters)
		{
			num += (float)characterStateControl.maxHp;
			num2 += (float)characterStateControl.hp;
		}
		foreach (CharacterStateControl characterStateControl2 in base.battleStateData.enemies)
		{
			num3 += (float)characterStateControl2.maxHp;
			num4 += (float)characterStateControl2.hp;
		}
		if (num2 / num > num4 / num3)
		{
			this.isCheckEnd = true;
			this.isWin = true;
		}
		else if (num2 / num < num4 / num3)
		{
			this.isCheckEnd = true;
			this.isWin = false;
		}
	}

	private void AliveCount()
	{
		int num = 0;
		int num2 = 0;
		foreach (CharacterStateControl characterStateControl in base.battleStateData.playerCharacters)
		{
			if (characterStateControl.hp > 0)
			{
				num++;
			}
		}
		foreach (CharacterStateControl characterStateControl2 in base.battleStateData.enemies)
		{
			if (characterStateControl2.hp > 0)
			{
				num2++;
			}
		}
		if (num != num2)
		{
			this.isCheckEnd = true;
			this.isWin = (num > num2);
		}
	}

	private void LastPlayMonster()
	{
		if (this.lastCharacter == null)
		{
			return;
		}
		this.isCheckEnd = true;
		this.isWin = !this.lastCharacter.isEnemy;
	}
}
