using System;
using System.Collections.Generic;

public abstract class BattleStateController : BattleStateBase
{
	private Dictionary<Type, BattleStateBase> stateList = new Dictionary<Type, BattleStateBase>();

	private new BattleStateBase currentState;

	public BattleStateController(Action OnStart, Action OnExit) : base(OnStart, OnExit)
	{
	}

	public BattleStateController(Action OnStart, Action OnExit, Action<EventState> OnExitGotEvent) : base(OnStart, OnExit, OnExitGotEvent)
	{
	}

	protected bool isWaitState
	{
		get
		{
			return this.currentState != null && this.currentState.isRunning;
		}
	}

	protected void SetState(Type stateType)
	{
		BattleStateBase battleStateBase = this.currentState;
		this.currentState = this.stateList[stateType];
		this.currentState.OnStartState((!IMono.op_True(battleStateBase)) ? null : battleStateBase.GetOnStartNextState(), stateType);
	}

	protected void AddState(BattleStateBase state)
	{
		this.stateList.Add(state.GetType(), state);
	}

	protected void RemoveState(Type stateType)
	{
		this.stateList[stateType].OnRemoveState();
		this.stateList.Remove(stateType);
	}

	protected void RemoveState(BattleStateBase state)
	{
		this.RemoveState(state.GetType());
	}

	protected BattleStateBase GetState(Type stateType)
	{
		return this.stateList[stateType];
	}

	protected void ClearState()
	{
		foreach (KeyValuePair<Type, BattleStateBase> keyValuePair in this.stateList)
		{
			keyValuePair.Value.OnRemoveState();
		}
		this.stateList.Clear();
	}
}
