using System;

public abstract class BattleStateMainController : BattleStateController
{
	public BattleStateMainController() : base(null, null)
	{
	}

	protected abstract Type startStateType { get; }

	protected override void AwakeThisState()
	{
		this.RegisterStates();
		base.SetState(this.startStateType);
	}

	protected abstract void RegisterStates();

	public new BattleStateBase currentState
	{
		get
		{
			return base.currentState;
		}
	}
}
