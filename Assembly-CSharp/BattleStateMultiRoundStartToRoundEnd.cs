using System;
using System.Collections;

public class BattleStateMultiRoundStartToRoundEnd : BattleStateRoundStartToRoundEnd
{
	private BattleStateBase subStateWaitAllPlayers;

	private BattleStateBase subStateMultiWaitEnemySkillSelect;

	private BattleStateBase subStateMultiAreaRandomDamageHitFunction;

	public BattleStateMultiRoundStartToRoundEnd(Action OnExit, Action<bool> OnWin, Action<bool> OnFail, Action<bool> OnTimeOver, Action<EventState> OnExitGotEvent) : base(OnExit, OnWin, OnFail, OnTimeOver, OnExitGotEvent)
	{
	}

	protected override void AwakeThisState()
	{
		this.subStateCharacterDeadCheckFunction = new SubStateMultiCharacterDeadCheckFunction(delegate()
		{
			base.stateManager.roundFunction.SetResult(true, false, false);
		}, delegate(bool isNextWave)
		{
			base.stateManager.roundFunction.SetResult(false, true, isNextWave);
		}, delegate(bool isNextWave)
		{
			base.stateManager.roundFunction.SetResult(false, false, isNextWave);
		}, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStateCharacterDeadCheckFunction);
		this.subStateCharacterRevivalFunction = new SubStateMultiCharacterRevivalFunction(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStateCharacterRevivalFunction);
		this.subStateSkillDetailsFunction = new SubStateMultiSkillDetailsFunction(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStateSkillDetailsFunction);
		this.subStateEnemyTurnStartAction = new SubStateEnemyTurnStartAction(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStateEnemyTurnStartAction);
		this.subStatePlayInvocationEffectAction = new SubStatePlayInvocationEffectAction(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStatePlayInvocationEffectAction);
		this.subStatePlayerCharacterAndSkillSelectFunction = new SubStateMultiPlayerCharacterAndSkillSelectFunction(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStatePlayerCharacterAndSkillSelectFunction);
		this.subStateOnHitPoisonDamageFunction = new SubStateOnHitPoisonDamageFunction(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStateOnHitPoisonDamageFunction);
		this.subStateRoundStartAction = new SubStateMultiRoundStartAction(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStateRoundStartAction);
		this.subStatePlayChipEffect = new SubStatePlayChipEffect(null, new Action<EventState>(base.SendEventState), () => this.isSkillEnd);
		base.AddState(this.subStatePlayChipEffect);
		this.subStateWaitRandomSeedSync = new SubStateWaitRandomSeedSync(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStateWaitRandomSeedSync);
		this.subStateWaitAllPlayers = new SubStateWaitAllPlayers(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStateWaitAllPlayers);
		this.subStateMultiWaitEnemySkillSelect = new SubStateMultiWaitEnemySkillSelect(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStateMultiWaitEnemySkillSelect);
		this.subStateMultiAreaRandomDamageHitFunction = new SubStateMultiAreaRandomDamageHitFunction(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStateMultiAreaRandomDamageHitFunction);
		this.subStateWaitForCertainPeriodTimeAction = new SubStateWaitForCertainPeriodTimeAction(null, new Action<EventState>(base.SendEventState));
		base.AddState(this.subStateWaitForCertainPeriodTimeAction);
	}

	protected override IEnumerator MainRoutine()
	{
		base.stateManager.multiFunction.RefreshMonsterButtons();
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
		if (!base.onFreeze)
		{
			base.stateManager.multiFunction.TurnStartInit(this.lastCharacter);
			if (base.stateManager.multiFunction.isMyTurn)
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
			else
			{
				base.SetState(this.subStateMultiWaitEnemySkillSelect.GetType());
				while (base.isWaitState)
				{
					yield return null;
				}
			}
		}
		base.battleStateData.enableRotateCam = false;
		yield break;
	}

	protected override IEnumerator SkillFunction()
	{
		IEnumerator skillFunction = base.SkillFunction();
		while (skillFunction.MoveNext())
		{
			object obj = skillFunction.Current;
			yield return obj;
		}
		base.SetState(this.subStateMultiAreaRandomDamageHitFunction.GetType());
		while (base.isWaitState)
		{
			yield return null;
		}
		yield break;
	}

	protected override IEnumerator TurnEndFunction()
	{
		base.stateManager.uiControlMulti.StopSharedAPAnimation();
		yield break;
	}

	protected override void SaveRecoverData()
	{
	}
}
