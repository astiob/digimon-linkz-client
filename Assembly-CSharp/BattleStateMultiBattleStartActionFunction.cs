using System;
using System.Collections;

public class BattleStateMultiBattleStartActionFunction : BattleStateController
{
	public BattleStateMultiBattleStartActionFunction(Action OnRoundStart, Action<bool> OnFail, Action<EventState> OnExitGotEvent) : base(null, OnRoundStart, OnExitGotEvent)
	{
	}

	protected override void AwakeThisState()
	{
		base.AddState(new BattleStateBattleStartAction(null, new Action<EventState>(base.SendEventState)));
	}

	protected override void EnabledThisState()
	{
		base.stateManager.uiControlMulti.HideCharacterHUDFunction();
		base.stateManager.uiControlMulti.ApplySkillSelectUI(false);
		base.stateManager.callAction.HideMonsterDescription();
		base.stateManager.callAction.OnHideEnemyDescriotion();
		base.stateManager.callAction.ShowHideSkillDescription(-1);
		base.stateManager.targetSelect.TargetManualSelectAndApplyUIFunction(null);
	}

	protected override IEnumerator MainRoutine()
	{
		base.SetState(typeof(BattleStateBattleStartAction));
		while (base.isWaitState)
		{
			yield return null;
		}
		yield break;
	}
}
