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
		switch (this.seq)
		{
		case 0:
			this.seq = 1;
			break;
		case 1:
			if (++this.timeCt >= 20)
			{
				RestrictionInput.EndLoad();
				this.OpenResultUI();
				this.timeCt = 0;
				this.seq = 2;
			}
			break;
		}
	}

	public override void OnDestroy()
	{
		GUIScreenPvPResult.instance = null;
		base.OnDestroy();
	}

	private void OpenResultUI()
	{
		GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseBattleResult), "CMD_PVPBattleResult");
	}

	private void OnCloseBattleResult(int i)
	{
		this.GoToPvPTop();
	}

	private void GoToPvPTop()
	{
		ClassSingleton<GUIMonsterIconList>.Instance.RefreshList(MonsterDataMng.Instance().GetMonsterDataList());
		MonsterDataMng.Instance().InitMonsterGO();
		GUIMain.ShowCommonDialog(new Action<int>(this.OnClosePvPTOP), "CMD_PvPTop");
	}

	private void OnClosePvPTOP(int idx)
	{
		if (idx == 0)
		{
			return;
		}
		this.GoToFarm();
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
