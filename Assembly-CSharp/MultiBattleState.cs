using System;

public class MultiBattleState : BattleStateMainController
{
	protected override Type startStateType
	{
		get
		{
			return typeof(BattleStateMultiInitialize);
		}
	}

	protected override void RegisterStates()
	{
		base.AddState(new BattleStateMultiInitialize(delegate()
		{
			base.SetState(typeof(BattleStateMultiWaveController));
			base.RemoveState(typeof(BattleStateMultiInitialize));
		}));
		base.AddState(new BattleStateMultiWaveController(delegate()
		{
			base.SetState(typeof(BattleStateMultiBattleStartActionFunction));
		}, new Action<EventState>(this.ExitGotEvent)));
		base.AddState(new BattleStateMultiBattleStartActionFunction(delegate()
		{
			base.SetState(typeof(BattleStateMultiRoundStartToRoundEnd));
		}, null, new Action<EventState>(this.ExitGotEvent)));
		base.AddState(new BattleStateMultiRoundStartToRoundEnd(delegate()
		{
			base.SetState(typeof(BattleStateMultiRoundStartToRoundEnd));
		}, delegate(bool isNextWave)
		{
			if (!isNextWave)
			{
				base.SetState(typeof(BattleStateMultiPlayerWinner));
			}
			else
			{
				base.SetState(typeof(BattleStateMultiWaveController));
			}
		}, delegate(bool isContinue)
		{
			if (isContinue)
			{
				base.SetState(typeof(BattleStateMultiRoundStartToRoundEnd));
			}
			else
			{
				base.SetState(typeof(BattleStateMultiPlayerFailed));
			}
		}, delegate(bool isNextWave)
		{
			base.SetState(typeof(BattleStateMultiTimeOver));
		}, new Action<EventState>(this.ExitGotEvent)));
		base.AddState(new BattleStateMultiPlayerWinner(delegate()
		{
			base.SetState(typeof(BattleStateMultiFadeOut));
		}, delegate(EventState eventState)
		{
			base.SetState(typeof(BattleStateMultiFadeOut));
		}));
		base.AddState(new BattleStateMultiPlayerFailed(delegate()
		{
			base.SetState(typeof(BattleStateMultiFadeOut));
		}));
		base.AddState(new BattleStateMultiTimeOver(delegate()
		{
			base.SetState(typeof(BattleStateMultiFadeOut));
		}, delegate(EventState eventState)
		{
			base.SetState(typeof(BattleStateMultiFadeOut));
		}));
		base.AddState(new BattleStateMultiFadeOut());
	}

	private void ExitGotEvent(EventState eventState)
	{
		if (eventState == EventState.Retire)
		{
			base.SetState(typeof(BattleStateMultiPlayerFailed));
		}
		else if (eventState == EventState.ConnectionError)
		{
			base.SetState(typeof(BattleStateMultiPlayerFailed));
		}
	}
}
