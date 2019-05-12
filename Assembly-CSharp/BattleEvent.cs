using System;

public class BattleEvent : BattleFunctionBase
{
	public void CallRetireEvent()
	{
		base.stateManager.rootState.currentState.SendEventState(EventState.Retire);
	}

	public void CallWinEvent()
	{
		base.stateManager.rootState.currentState.SendEventState(EventState.Win);
	}

	public void CallConnectionErrorEvent()
	{
		base.stateManager.rootState.currentState.SendEventState(EventState.ConnectionError);
	}
}
