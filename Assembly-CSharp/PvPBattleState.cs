using MultiBattle.Tools;
using System;

public class PvPBattleState : BattleStateMainController
{
	protected override Type startStateType
	{
		get
		{
			return typeof(BattleStatePvPInitialize);
		}
	}

	protected override void RegisterStates()
	{
		base.AddState(new BattleStatePvPInitialize(delegate()
		{
			base.SetState(typeof(BattleStateWaveController));
		}));
		base.AddState(new BattleStateWaveController(delegate()
		{
			base.SetState(typeof(BattleStatePvPBattleStartActionFunction));
		}, null));
		base.AddState(new BattleStatePvPBattleStartActionFunction(delegate()
		{
			base.SetState(typeof(BattleStatePvPRoundStartToRoundEnd));
		}));
		base.AddState(new BattleStatePvPRoundStartToRoundEnd(delegate()
		{
			base.SetState(typeof(BattleStatePvPRoundStartToRoundEnd));
		}, delegate(bool isNextWave)
		{
			int battleResult;
			if (!isNextWave)
			{
				battleResult = 1;
			}
			else
			{
				battleResult = 2;
			}
			ClassSingleton<MultiBattleData>.Instance.BattleResult = battleResult;
			base.SetState(typeof(BattleStatePvPBattleEnd));
		}, delegate(bool isContinue)
		{
			if (isContinue)
			{
				base.SetState(typeof(BattleStatePvPRoundStartToRoundEnd));
			}
			else
			{
				ClassSingleton<MultiBattleData>.Instance.BattleResult = 2;
				base.SetState(typeof(BattleStatePvPBattleEnd));
			}
		}, delegate(bool isNextWave)
		{
			int battleResult;
			if (isNextWave)
			{
				battleResult = 1;
			}
			else
			{
				battleResult = 2;
			}
			ClassSingleton<MultiBattleData>.Instance.BattleResult = battleResult;
			base.SetState(typeof(BattleStatePvPTimeOver));
		}, delegate(EventState eventState)
		{
			base.stateManager.pvpFunction.FinishTCP();
			if (eventState == EventState.Retire)
			{
				ClassSingleton<MultiBattleData>.Instance.BattleResult = 4;
				base.SetState(typeof(BattleStatePvPRetire));
			}
			else if (eventState == EventState.ConnectionError)
			{
				ClassSingleton<MultiBattleData>.Instance.BattleResult = 3;
				base.SetState(typeof(BattleStatePvPBattleEnd));
			}
			else if (eventState == EventState.Win)
			{
				ClassSingleton<MultiBattleData>.Instance.BattleResult = 1;
				base.SetState(typeof(BattleStatePvPBattleEnd));
			}
		}));
		base.AddState(new BattleStatePvPTimeOver(delegate()
		{
			base.SetState(typeof(BattleStatePvPBattleEnd));
		}));
		base.AddState(new BattleStatePvPRetire(delegate()
		{
			base.SetState(typeof(BattleStatePvPBattleEnd));
		}, delegate(EventState eventState)
		{
			base.stateManager.pvpFunction.FinishTCP();
			if (eventState == EventState.ConnectionError)
			{
				ClassSingleton<MultiBattleData>.Instance.BattleResult = 4;
			}
			base.SetState(typeof(BattleStatePvPBattleEnd));
		}));
		base.AddState(new BattleStatePvPBattleEnd(delegate()
		{
			if (ClassSingleton<MultiBattleData>.Instance.BattleResult == 1)
			{
				base.SetState(typeof(BattleStatePlayerWinner));
			}
			else
			{
				base.SetState(typeof(BattleStatePvPPlayerFailed));
			}
		}));
		base.AddState(new BattleStatePlayerWinner(delegate()
		{
			base.SetState(typeof(BattleStatePvPFadeOut));
		}));
		base.AddState(new BattleStatePvPPlayerFailed(delegate()
		{
			base.SetState(typeof(BattleStatePvPFadeOut));
		}));
		base.AddState(new BattleStatePvPFadeOut());
	}
}
