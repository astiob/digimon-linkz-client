using System;

public class BattleStateMultiWaveController : BattleStateWaveController
{
	public BattleStateMultiWaveController(Action OnExit, Action<EventState> OnExitGotEvent) : base(OnExit, OnExitGotEvent)
	{
	}

	protected override bool isPose
	{
		get
		{
			return false;
		}
	}
}
