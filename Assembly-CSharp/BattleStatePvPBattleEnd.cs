using MultiBattle.Tools;
using System;
using System.Collections;
using System.Collections.Generic;

public class BattleStatePvPBattleEnd : BattleStateBase
{
	private GameWebAPI.RespData_ColosseumBattleEndLogic colosseumEnd;

	public BattleStatePvPBattleEnd(Action OnExit) : base(null, OnExit)
	{
	}

	protected override IEnumerator MainRoutine()
	{
		base.stateManager.uiControl.SetTouchEnable(true);
		base.stateManager.uiControl.HideCharacterHUDFunction();
		base.stateManager.pvpFunction.StopSomething();
		if (base.onServerConnect)
		{
			IEnumerator startMsg = this.EndColosseum();
			while (startMsg.MoveNext())
			{
				object obj = startMsg.Current;
				yield return obj;
			}
			MultiBattleData.BattleEndResponseData responseData = new MultiBattleData.BattleEndResponseData();
			if (this.colosseumEnd != null)
			{
				responseData.resultCode = this.colosseumEnd.resultCode;
				List<MultiBattleData.BattleEndResponseData.Reward> rwardList = new List<MultiBattleData.BattleEndResponseData.Reward>();
				if (this.colosseumEnd.reward != null)
				{
					for (int i = 0; i < this.colosseumEnd.reward.Length; i++)
					{
						rwardList.Add(new MultiBattleData.BattleEndResponseData.Reward
						{
							assetCategoryId = this.colosseumEnd.reward[i].assetCategoryId,
							assetNum = this.colosseumEnd.reward[i].assetNum,
							assetValue = this.colosseumEnd.reward[i].assetValue
						});
					}
				}
				List<MultiBattleData.BattleEndResponseData.Reward> firstRankupRwardList = new List<MultiBattleData.BattleEndResponseData.Reward>();
				if (this.colosseumEnd.firstRankUpReward != null)
				{
					for (int j = 0; j < this.colosseumEnd.firstRankUpReward.Length; j++)
					{
						firstRankupRwardList.Add(new MultiBattleData.BattleEndResponseData.Reward
						{
							assetCategoryId = this.colosseumEnd.firstRankUpReward[j].assetCategoryId,
							assetNum = this.colosseumEnd.firstRankUpReward[j].assetNum,
							assetValue = this.colosseumEnd.firstRankUpReward[j].assetValue
						});
					}
				}
				responseData.reward = rwardList.ToArray();
				responseData.firstRankUpReward = firstRankupRwardList.ToArray();
				responseData.score = this.colosseumEnd.score;
				responseData.colosseumRankId = this.colosseumEnd.colosseumRankId;
				responseData.isFirstRankUp = this.colosseumEnd.isFirstRankUp;
			}
			else
			{
				responseData.reward = new MultiBattleData.BattleEndResponseData.Reward[0];
			}
			ClassSingleton<MultiBattleData>.Instance.BattleEndResponse = responseData;
			yield break;
		}
		yield break;
	}

	protected override void DisabledThisState()
	{
	}

	private IEnumerator EndColosseum()
	{
		GameWebAPI.ColosseumBattleEndLogic request = new GameWebAPI.ColosseumBattleEndLogic
		{
			SetSendData = delegate(GameWebAPI.ReqData_ColosseumBattleEndLogic param)
			{
				param.battleResult = ClassSingleton<MultiBattleData>.Instance.BattleResult;
				param.roundCount = base.battleStateData.currentRoundNumber;
				param.isMockBattle = ((!(ClassSingleton<MultiBattleData>.Instance.MockBattleUserCode == "0")) ? 1 : 0);
				param.skillUseDeckPosition = "0";
			},
			OnReceived = delegate(GameWebAPI.RespData_ColosseumBattleEndLogic resData)
			{
				this.colosseumEnd = resData;
			}
		};
		return request.Run(new Action(RestrictionInput.EndLoad), delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null);
	}
}
