using System;

public class SingleBattleState : BattleStateMainController
{
	protected override Type startStateType
	{
		get
		{
			return typeof(BattleStateInitialize);
		}
	}

	protected override void RegisterStates()
	{
		base.AddState(new BattleStateInitialize(delegate()
		{
			base.SetState(typeof(BattleStateWaveController));
			base.RemoveState(typeof(BattleStateInitialize));
		}));
		base.AddState(new BattleStateWaveController(delegate()
		{
			base.SetState(typeof(BattleStateBattleStartActionFunction));
		}, null));
		base.AddState(new BattleStateBattleStartActionFunction(delegate()
		{
			base.SetState(typeof(BattleStateRoundStartToRoundEnd));
		}, delegate()
		{
			base.SetState(typeof(BattleStatePlayerWinner));
		}));
		base.AddState(new BattleStateRoundStartToRoundEnd(delegate()
		{
			base.SetState(typeof(BattleStateRoundStartToRoundEnd));
		}, delegate(bool isNextWave)
		{
			if (!isNextWave)
			{
				base.SetState(typeof(BattleStatePlayerWinner));
			}
			else
			{
				base.SetState(typeof(BattleStateWaveController));
			}
		}, delegate(bool isContinue)
		{
			if (isContinue)
			{
				base.SetState(typeof(BattleStateRoundStartToRoundEnd));
			}
			else
			{
				base.SetState(typeof(BattleStatePlayerFailed));
			}
		}, delegate(bool isNextWave)
		{
			base.SetState(typeof(BattleStateTimeOver));
		}, delegate(EventState eventState)
		{
			if (eventState == EventState.Retire)
			{
				base.SetState(typeof(BattleStatePlayerFailed));
			}
		}));
		base.AddState(new BattleStatePlayerWinner(delegate()
		{
			base.SetState(typeof(BattleStateFadeOut));
		}));
		base.AddState(new BattleStatePlayerFailed(delegate()
		{
			base.SetState(typeof(BattleStateFadeOut));
		}));
		base.AddState(new BattleStateTimeOver(delegate()
		{
			base.SetState(typeof(BattleStateFadeOut));
		}));
		base.AddState(new BattleStateFadeOut());
	}
}
