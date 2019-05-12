using Master;
using Quest;
using System;
using System.Collections;
using System.Linq;

public sealed class GUIScreenResult : GUIScreen
{
	private int timeCt;

	private int seq;

	private GameWebAPI.RespDataWD_DungeonResult result;

	public static GUIScreenResult instance;

	private CMD_BattleResult battleResult;

	private Action actionOpenedFirstClear;

	public string clearDungeonID;

	private GUIScreenResult.ResultTimeOutMode resultTimeOutMode;

	private GameWebAPI.RespData_WorldMultiResultInfoLogic multiResult;

	private bool isMultiFriend;

	private bool isFirstLinkBonus;

	private bool isPointQuest;

	protected override void Awake()
	{
		GUIScreenResult.instance = this;
		base.Awake();
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		ClassSingleton<BattleDataStore>.Instance.DeleteForSystem();
		ServerDateTime.isUpdateServerDateTime = true;
	}

	protected override void Update()
	{
		base.Update();
		GUIScreenResult.UpdateState updateState = (GUIScreenResult.UpdateState)this.seq;
		if (updateState != GUIScreenResult.UpdateState.Initialize)
		{
			if (updateState != GUIScreenResult.UpdateState.OpenResult)
			{
				if (updateState != GUIScreenResult.UpdateState.Wait)
				{
				}
			}
			else if (++this.timeCt >= 20)
			{
				RestrictionInput.EndLoad();
				this.OpenResultUI();
				this.timeCt = 0;
				this.seq = 2;
			}
		}
		else
		{
			this.result = ClassSingleton<QuestData>.Instance.RespDataWD_DungeonResult;
			this.multiResult = ClassSingleton<QuestData>.Instance.RespData_WorldMultiResultInfoLogic;
			if (this.result == null && this.multiResult == null)
			{
				ScreenController.ChangeHomeScreen(CMD_Tips.DISPLAY_PLACE.TitleToFarm);
				this.seq = 2;
			}
			else
			{
				this.seq = 1;
			}
		}
	}

	public override void OnDestroy()
	{
		GUIScreenResult.instance = null;
		base.OnDestroy();
	}

	private void OpenResultUI()
	{
		int num = GUIScreenResult.ClearType.Expire.ToInteger();
		if (this.multiResult != null)
		{
			num = this.multiResult.clearType;
		}
		else
		{
			num = this.result.clearType;
		}
		if (num >= GUIScreenResult.ClearType.Expire.ToInteger())
		{
			this.CreatePopup(StringMaster.GetString("BattleResult-03"), StringMaster.GetString("BattleResult-04"));
		}
		else
		{
			this.battleResult = (GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseBattleResult), "CMD_BattleResult", null) as CMD_BattleResult);
		}
	}

	private void CreatePopup(string title, string message)
	{
		CMD_Alert cmd_Alert = GUIMain.ShowCommonDialog(delegate(int i)
		{
			if (this.resultTimeOutMode == GUIScreenResult.ResultTimeOutMode.HOME)
			{
				RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
				ScreenController.ChangeHomeScreen(CMD_Tips.DISPLAY_PLACE.TitleToFarm);
			}
			else
			{
				GUIMain.BackToTOP("UIStartupCaution", 0.8f, 0.8f);
			}
		}, "CMD_Alert", null) as CMD_Alert;
		cmd_Alert.Title = title;
		cmd_Alert.Info = message;
		cmd_Alert.SetDisplayButton(CMD_Alert.DisplayButton.CLOSE);
	}

	public void SetResultTimeOutMode(GUIScreenResult.ResultTimeOutMode mode)
	{
		this.resultTimeOutMode = mode;
	}

	private void OnCloseBattleResult(int i)
	{
		this.clearDungeonID = this.battleResult.clearDungeonID;
		int num = GUIScreenResult.ClearType.Expire.ToInteger();
		bool flag = false;
		if (this.multiResult != null)
		{
			num = this.multiResult.clearType;
			flag = (num == GUIScreenResult.ClearType.FirstClear.ToInteger());
			if (this.multiResult.dungeonReward.linkBonus != null)
			{
				foreach (GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.LinkBonus linkBonus2 in this.multiResult.dungeonReward.linkBonus)
				{
					if (linkBonus2.reward != null)
					{
						this.isFirstLinkBonus = true;
						break;
					}
				}
			}
			this.isMultiFriend = true;
		}
		else if (this.result != null)
		{
			num = this.result.clearType;
			flag = (num == GUIScreenResult.ClearType.FirstClear.ToInteger());
		}
		string worldDungeonId = (this.multiResult != null) ? DataMng.Instance().RespData_WorldMultiStartInfo.worldDungeonId : ClassSingleton<QuestData>.Instance.RespDataWD_DungeonStart.worldDungeonId;
		this.isPointQuest = this.IsPointQuest(worldDungeonId);
		if (flag)
		{
			if (this.result != null && this.result.dungeonReward != null && 0 < this.result.dungeonReward.Length)
			{
				if (this.actionOpenedFirstClear != null)
				{
					this.actionOpenedFirstClear();
					this.actionOpenedFirstClear = null;
				}
				GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseFirstClear), "CMD_FirstClear", null);
			}
			else if (this.multiResult != null && this.multiResult.dungeonReward != null && this.multiResult.dungeonReward.clearReward != null)
			{
				if (this.actionOpenedFirstClear != null)
				{
					this.actionOpenedFirstClear();
					this.actionOpenedFirstClear = null;
				}
				GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseFirstClear), "CMD_FirstClear", null);
			}
			else
			{
				this.OnCloseFirstClear(0);
			}
		}
		else if (num == GUIScreenResult.ClearType.Clear.ToInteger())
		{
			this.OnCloseFirstClear(0);
		}
	}

	private bool IsPointQuest(string worldDungeonId)
	{
		GameWebAPI.RespDataMA_GetWorldDungeonM respDataMA_WorldDungeonM = MasterDataMng.Instance().RespDataMA_WorldDungeonM;
		GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM dungeonInfo = respDataMA_WorldDungeonM.worldDungeonM.SingleOrDefault((GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM x) => x.worldDungeonId == worldDungeonId);
		if (dungeonInfo == null)
		{
			return false;
		}
		GameWebAPI.RespDataMA_GetWorldStageM respDataMA_WorldStageM = MasterDataMng.Instance().RespDataMA_WorldStageM;
		GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM stageInfo = respDataMA_WorldStageM.worldStageM.SingleOrDefault((GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM x) => x.worldStageId == dungeonInfo.worldStageId);
		if (stageInfo == null)
		{
			return false;
		}
		GameWebAPI.RespDataMA_GetWorldAreaM respDataMA_WorldAreaM = MasterDataMng.Instance().RespDataMA_WorldAreaM;
		GameWebAPI.RespDataMA_GetWorldAreaM.WorldAreaM worldAreaM = respDataMA_WorldAreaM.worldAreaM.SingleOrDefault((GameWebAPI.RespDataMA_GetWorldAreaM.WorldAreaM x) => x.worldAreaId == stageInfo.worldAreaId);
		return worldAreaM != null && (worldAreaM.type == "3" || worldAreaM.type == "4");
	}

	private void OnCloseFirstClear(int i)
	{
		if (this.isPointQuest)
		{
			this.isPointQuest = false;
			GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseFirstClear), "CMD_PointResult", null);
		}
		else if (this.isFirstLinkBonus)
		{
			this.isFirstLinkBonus = false;
			GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseFirstClear), "CMD_LinkBonus", null);
		}
		else if (this.isMultiFriend)
		{
			this.isMultiFriend = false;
			GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseFirstClear), "CMD_MultiResult", null);
		}
		else
		{
			base.StartCoroutine(this.OpenBattleNextChoice());
		}
	}

	private IEnumerator OpenBattleNextChoice()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
		APIRequestTask task = Singleton<UserDataMng>.Instance.RequestPlayerInfo(true);
		return task.Run(delegate
		{
			RestrictionInput.EndLoad();
			CMD_BattleNextChoice cmd_BattleNextChoice = GUIMain.ShowCommonDialog(null, "CMD_BattleNextChoice", null) as CMD_BattleNextChoice;
			cmd_BattleNextChoice.screenResult = this;
		}, delegate(Exception e)
		{
			RestrictionInput.EndLoad();
		}, null);
	}

	public void SetActionOfOpenedFirstClear(Action action)
	{
		this.actionOpenedFirstClear = action;
	}

	private enum ClearType
	{
		Retire,
		FirstClear,
		Clear,
		Expire = 100
	}

	private enum UpdateState
	{
		Initialize,
		OpenResult,
		Wait
	}

	public enum ResultTimeOutMode
	{
		HOME,
		TITLE
	}
}
