using System;
using System.Collections;

public class SubStateWaitEnemySkillSelect : SubStatePlayerCharacterAndSkillSelectFunction
{
	public SubStateWaitEnemySkillSelect(Action OnExit, Action<EventState> OnExitGotEvent) : base(OnExit, OnExitGotEvent)
	{
	}

	protected override void EnabledThisState()
	{
		base.EnabledThisState();
		base.stateManager.uiControl.SetHudCollider(false);
		base.stateManager.uiControlPvP.ShowLoading(true);
		if (base.hierarchyData.onAutoPlay == 0)
		{
			if (!this.currentCharacter.isEnemy)
			{
				base.stateManager.uiControlPvP.ShowSkillSelectUI();
			}
			else
			{
				base.stateManager.uiControlPvP.HideSkillSelectUI();
			}
		}
	}

	protected override IEnumerator MainRoutine()
	{
		base.stateManager.pvpFunction.CurrentEnemyMyIndex = this.currentCharacter.myIndex;
		IEnumerator action = base.stateManager.pvpFunction.WaitAllPlayers(TCPMessageType.Attack);
		while (action.MoveNext())
		{
			object obj = action.Current;
			yield return obj;
		}
		yield break;
	}

	protected override void DisabledThisState()
	{
		base.DisabledThisState();
		base.stateManager.uiControlPvP.HideLoading();
	}

	protected override void GetEventThisState(EventState eventState)
	{
	}
}
