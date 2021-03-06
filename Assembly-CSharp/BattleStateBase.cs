﻿using BattleStateMachineInternal;
using System;
using System.Collections;

public abstract class BattleStateBase : IMono
{
	private static BattleStateBase _currentState;

	private Action onStart;

	private Action onExit;

	private Action<EventState> onExitGotEvent;

	private IEnumerator runningMainRoutine;

	private bool isAwake;

	private bool _isRunning;

	private Type nextType;

	private static EventState gotEventState;

	private static bool _isGotEvent;

	public BattleStateBase(Action OnStart, Action OnExit)
	{
		this.onStart = OnStart;
		this.onExit = OnExit;
	}

	public BattleStateBase(Action OnStart, Action OnExit, Action<EventState> OnExitGotEvent)
	{
		this.onStart = OnStart;
		this.onExit = OnExit;
		this.onExitGotEvent = OnExitGotEvent;
	}

	private IEnumerator BasedMainRoutine()
	{
		while (BattleStateBase._currentState == this)
		{
			while (this._isRunning)
			{
				IEnumerator r = this.MainRoutine();
				while (!BattleStateBase._isGotEvent && r.MoveNext())
				{
					this.UpdateThisState();
					yield return r.Current;
				}
				this.DisabledThisState();
				if (!BattleStateBase._isGotEvent && this.onExit != null)
				{
					this.onExit();
				}
				this._isRunning = false;
			}
			if (BattleStateBase._isGotEvent)
			{
				BattleStateBase._isGotEvent = false;
				if (this.onExitGotEvent != null)
				{
					this.onExitGotEvent(BattleStateBase.gotEventState);
				}
				yield break;
			}
			if (this.nextType != null && this.nextType == base.GetType())
			{
				this.nextType = null;
				yield break;
			}
			yield return null;
		}
		yield break;
	}

	protected BattleStateManager stateManager
	{
		get
		{
			return BattleStateManager.current;
		}
	}

	protected BattleMode battleMode
	{
		get
		{
			return this.stateManager.battleMode;
		}
	}

	protected bool isMulti
	{
		get
		{
			return this.stateManager.battleMode == BattleMode.Multi;
		}
	}

	protected bool isPvP
	{
		get
		{
			return this.stateManager.battleMode == BattleMode.PvP;
		}
	}

	protected bool onServerConnect
	{
		get
		{
			return this.stateManager.onServerConnect;
		}
	}

	protected BattleStateData battleStateData
	{
		get
		{
			return this.stateManager.battleStateData;
		}
	}

	protected BattleStateHierarchyData hierarchyData
	{
		get
		{
			return this.stateManager.hierarchyData;
		}
	}

	protected virtual void AwakeThisState()
	{
	}

	protected virtual void EnabledThisState()
	{
	}

	protected virtual IEnumerator MainRoutine()
	{
		yield break;
	}

	protected virtual void UpdateThisState()
	{
	}

	protected virtual void DisabledThisState()
	{
	}

	protected virtual void RemovedThisState()
	{
	}

	protected virtual void GetEventThisState(EventState eventState)
	{
	}

	protected virtual void OnStartNextState()
	{
	}

	protected BattleStateBase currentState
	{
		get
		{
			return BattleStateBase._currentState;
		}
	}

	protected bool isGotEvent
	{
		get
		{
			return BattleStateBase._isGotEvent;
		}
	}

	public void OnStartState(Action onStartNextState = null, Type stateType = null)
	{
		this.nextType = stateType;
		BattleStateBase._currentState = this;
		if (!this.isAwake)
		{
			this.AwakeThisState();
			this.isAwake = true;
		}
		this.EnabledThisState();
		if (this.onStart != null)
		{
			this.onStart();
		}
		if (onStartNextState != null)
		{
			onStartNextState();
		}
		this.runningMainRoutine = this.BasedMainRoutine();
		this._isRunning = true;
		base.StartCoroutine(this.runningMainRoutine);
	}

	public Action GetOnStartNextState()
	{
		return new Action(this.OnStartNextState);
	}

	public void OnRemoveState()
	{
		this.RemovedThisState();
		if (this.runningMainRoutine != null)
		{
			base.StopCoroutine(this.runningMainRoutine);
		}
		this.runningMainRoutine = null;
	}

	public void SendEventState(EventState eventState)
	{
		this.GetEventThisState(eventState);
		BattleStateBase._isGotEvent = true;
		BattleStateBase.gotEventState = eventState;
	}

	public bool isRunning
	{
		get
		{
			return this._isRunning;
		}
	}
}
