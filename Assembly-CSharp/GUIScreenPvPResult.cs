using System;

public sealed class GUIScreenPvPResult : GUIScreen
{
	public static GUIScreenPvPResult instance;

	private GameWebAPI.RespDataWD_DungeonResult result;

	private int seq;

	private int timeCt;

	protected override void Awake()
	{
		GUIScreenPvPResult.instance = this;
		base.Awake();
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
	}

	protected override void Update()
	{
		base.Update();
		GUIScreenPvPResult.UpdateState updateState = (GUIScreenPvPResult.UpdateState)this.seq;
		if (updateState != GUIScreenPvPResult.UpdateState.Initialize)
		{
			if (updateState != GUIScreenPvPResult.UpdateState.OpenResult)
			{
				if (updateState != GUIScreenPvPResult.UpdateState.Wait)
				{
				}
			}
			else
			{
				this.timeCt++;
				if (this.timeCt >= 20)
				{
					RestrictionInput.EndLoad();
					this.OpenResultUI();
					this.timeCt = 0;
					this.seq = 2;
				}
			}
		}
		else
		{
			this.seq = 1;
		}
	}

	public override void OnDestroy()
	{
		GUIScreenPvPResult.instance = null;
		base.OnDestroy();
	}

	private void OpenResultUI()
	{
		GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseBattleResult), "CMD_PVPBattleResult", null);
	}

	private void OnCloseBattleResult(int i)
	{
		this.GoToPvPTop();
	}

	private void GoToPvPTop()
	{
		ClassSingleton<GUIMonsterIconList>.Instance.RefreshList(MonsterDataMng.Instance().GetMonsterDataList());
		GUIMain.ShowCommonDialog(new Action<int>(this.OnClosePvPTOP), "CMD_PvPTop", null);
	}

	private void OnClosePvPTOP(int idx)
	{
		if (idx == 100)
		{
			this.GoToFarm();
		}
	}

	private void GoToFarm()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
		ScreenController.ChangeHomeScreen(CMD_Tips.DISPLAY_PLACE.BattleToFarm);
	}

	private enum UpdateState
	{
		Initialize,
		OpenResult,
		Wait
	}
}
