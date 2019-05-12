using Master;
using MissionData;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class CMD_Mission : CMD
{
	[Header("ミッションリストのタッチ領域")]
	[SerializeField]
	private GUISelectPanelMission missionList;

	[SerializeField]
	[Header("ミッション選択のタッチ領域")]
	private GUISelectPanelMissionSelect csSelectPanelMissionSelect;

	[Header("ミッションの段階終了時カットイン演出コントローラ")]
	[SerializeField]
	private PartsUpperCutinController cutinController;

	[SerializeField]
	private GameObject missionListOriginalItem;

	private bool isRebuildRecycle;

	private List<CMD_Mission.MissionType> missionTypeList = new List<CMD_Mission.MissionType>
	{
		CMD_Mission.MissionType.Beginner,
		CMD_Mission.MissionType.Midrange,
		CMD_Mission.MissionType.Daily,
		CMD_Mission.MissionType.Total
	};

	public static CMD_Mission.MissionType nowFocusType = CMD_Mission.MissionType.Beginner;

	private List<GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission[]> missionAllList;

	private List<CMD_Mission.MissionStateData> missionStateDataList;

	private CMD_Mission.MissionStateData missionStateData_BAK;

	public GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission GetMissionData(int IDX)
	{
		for (int i = 0; i < this.missionAllList.Count; i++)
		{
			int num = int.Parse(this.missionAllList[i][0].displayGroup);
			if (num == (int)CMD_Mission.nowFocusType)
			{
				return this.missionAllList[i][IDX];
			}
		}
		return null;
	}

	public GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission[] GetMissionData(CMD_Mission.MissionType type)
	{
		for (int i = 0; i < this.missionAllList.Count; i++)
		{
			int num = int.Parse(this.missionAllList[i][0].displayGroup);
			if (type == (CMD_Mission.MissionType)num)
			{
				return this.missionAllList[i];
			}
		}
		return null;
	}

	private void AddAllList()
	{
		this.missionAllList = new List<GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission[]>();
		for (int i = 0; i < this.missionTypeList.Count; i++)
		{
			GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission[] array = null;
			switch (this.missionTypeList[i])
			{
			case CMD_Mission.MissionType.Daily:
				array = ClassSingleton<MissionWebAPI>.Instance.MissionInfoLogicData.result.daily;
				break;
			case CMD_Mission.MissionType.Total:
				array = ClassSingleton<MissionWebAPI>.Instance.MissionInfoLogicData.result.total;
				break;
			case CMD_Mission.MissionType.Beginner:
				array = ClassSingleton<MissionWebAPI>.Instance.MissionInfoLogicData.result.beginner;
				break;
			case CMD_Mission.MissionType.Midrange:
				array = ClassSingleton<MissionWebAPI>.Instance.MissionInfoLogicData.result.midrange;
				break;
			}
			if (array != null && array.Length > 0)
			{
				int[] compNum = new int[]
				{
					0,
					-1,
					1
				};
				array = array.OrderBy((GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission v) => compNum[v.status]).ToArray<GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission>();
				this.missionAllList.Add(array);
			}
		}
	}

	public GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission[] GetMisssionDataByIDX(int IDX)
	{
		return this.missionAllList[IDX];
	}

	private int GetIndexByNowFocusType(CMD_Mission.MissionType type)
	{
		for (int i = 0; i < this.missionAllList.Count; i++)
		{
			int num = int.Parse(this.missionAllList[i][0].displayGroup);
			if (num == (int)type)
			{
				return i;
			}
		}
		if (this.missionAllList.Count > 0)
		{
			CMD_Mission.nowFocusType = (CMD_Mission.MissionType)int.Parse(this.missionAllList[0][0].displayGroup);
			return 0;
		}
		return -1;
	}

	private CMD_Mission.MissionType SetValidFocusType(CMD_Mission.MissionType type, ref bool isChanged)
	{
		for (int i = 0; i < this.missionAllList.Count; i++)
		{
			int num = int.Parse(this.missionAllList[i][0].displayGroup);
			if (num == (int)type)
			{
				isChanged = false;
				return type;
			}
		}
		if (this.missionAllList.Count > 0)
		{
			CMD_Mission.MissionType result = (CMD_Mission.MissionType)int.Parse(this.missionAllList[0][0].displayGroup);
			isChanged = true;
			return result;
		}
		isChanged = true;
		return CMD_Mission.MissionType.INVALID;
	}

	private bool IsValidFocusType(CMD_Mission.MissionType type)
	{
		for (int i = 0; i < this.missionAllList.Count; i++)
		{
			int num = int.Parse(this.missionAllList[i][0].displayGroup);
			if (num == (int)type)
			{
				return true;
			}
		}
		return false;
	}

	private void InitMissionStateDataList()
	{
		this.missionStateDataList = new List<CMD_Mission.MissionStateData>();
		for (int i = 0; i < this.missionTypeList.Count; i++)
		{
			CMD_Mission.MissionStateData missionStateData = new CMD_Mission.MissionStateData();
			missionStateData.type = this.missionTypeList[i];
			missionStateData.isFirst = true;
			missionStateData.nowSelectLoc = 0f;
			this.missionStateDataList.Add(missionStateData);
		}
	}

	private CMD_Mission.MissionStateData GetMissionStateDataByType(CMD_Mission.MissionType type)
	{
		for (int i = 0; i < this.missionStateDataList.Count; i++)
		{
			if (this.missionStateDataList[i].type == type)
			{
				return this.missionStateDataList[i];
			}
		}
		return null;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.HideDLG();
		APIRequestTask task = ClassSingleton<MissionWebAPI>.Instance.AccessMissionInfoLogicAPI();
		base.StartCoroutine(task.Run(delegate
		{
			RestrictionInput.EndLoad();
			this.ShowDLG();
			this.SetTutorialAnyTime("anytime_second_tutorial_mission_beginner");
			this.AddAllList();
			this.InitMissionStateDataList();
			this.InitMission(f, sizeX, sizeY, aT);
		}, delegate(Exception nop)
		{
			RestrictionInput.EndLoad();
			this.ClosePanel(true);
		}, null));
	}

	private void InitMission(Action<int> f, float sizeX, float sizeY, float aT)
	{
		if (this.missionAllList.Count <= 0)
		{
			return;
		}
		bool flag = false;
		CMD_Mission.nowFocusType = this.SetValidFocusType(CMD_Mission.nowFocusType, ref flag);
		if (ClassSingleton<MissionWebAPI>.Instance.MissionInfoLogicData.result.total != null)
		{
			foreach (GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission mission in ClassSingleton<MissionWebAPI>.Instance.MissionInfoLogicData.result.total)
			{
				if (mission.status == 1)
				{
					CMD_Mission.nowFocusType = CMD_Mission.MissionType.Total;
					break;
				}
			}
		}
		this.SetTitle(CMD_Mission.nowFocusType);
		this.CreateSelectPanel(CMD_Mission.nowFocusType);
		this.csSelectPanelMissionSelect.AllBuild(this.missionAllList.Count, true, 1f, 1f, null, this);
		int indexByNowFocusType = this.GetIndexByNowFocusType(CMD_Mission.nowFocusType);
		this.csSelectPanelMissionSelect.SetCellAnimReserve(indexByNowFocusType, false);
		this.csSelectPanelMissionSelect.RefreshBadge();
		ClassSingleton<FaceMissionAccessor>.Instance.faceMission.MissionTapSave();
		base.Show(f, sizeX, sizeY, aT);
	}

	private void CreateSelectPanel(CMD_Mission.MissionType newFocus)
	{
		if (this.missionStateData_BAK != null)
		{
			this.missionStateData_BAK.nowSelectLoc = this.missionList.SelectLoc;
		}
		CMD_Mission.nowFocusType = newFocus;
		CMD_Mission.MissionStateData missionStateDataByType = this.GetMissionStateDataByType(CMD_Mission.nowFocusType);
		this.missionList.SelectLoc = missionStateDataByType.nowSelectLoc;
		this.missionStateData_BAK = missionStateDataByType;
		GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission[] missionData = this.GetMissionData(CMD_Mission.nowFocusType);
		this.missionList.gameObject.SetActive(true);
		this.missionListOriginalItem.gameObject.SetActive(true);
		this.missionList.StartFadeEfcCT = 0;
		bool isFirst = missionStateDataByType.isFirst;
		if (!this.isRebuildRecycle)
		{
			this.missionList.AllBuild(missionData.Length, isFirst, 1f, 1f, null, this);
			this.isRebuildRecycle = true;
		}
		else
		{
			GUISelectPanelViewPartsUD guiselectPanelViewPartsUD = this.missionList;
			bool initLoc = isFirst;
			guiselectPanelViewPartsUD.RefreshList(missionData.Length, 1, null, initLoc);
			this.missionListOriginalItem.gameObject.SetActive(false);
			this.missionList.FadeOutAllListParts(null, true);
			this.missionList.FadeInAllListParts(null);
		}
		missionStateDataByType.isFirst = false;
	}

	public bool AnyDataNotReceived(CMD_Mission.MissionType type)
	{
		GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission[] missionData = this.GetMissionData(type);
		if (missionData != null)
		{
			if (missionData.Any((GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission _dt) => _dt.status == 1))
			{
				return true;
			}
		}
		return false;
	}

	private void SetTitle(CMD_Mission.MissionType missionType)
	{
		string title = this.GetTitle(missionType);
		base.PartsTitle.SetTitle(title);
	}

	public string GetTitle(CMD_Mission.MissionType missionType)
	{
		string result = string.Empty;
		if (missionType == CMD_Mission.MissionType.Beginner)
		{
			result = StringMaster.GetString("MissionTitleBeginner");
		}
		else if (missionType == CMD_Mission.MissionType.Midrange)
		{
			result = StringMaster.GetString("MissionTitleMidrange");
		}
		else if (missionType == CMD_Mission.MissionType.Daily)
		{
			result = StringMaster.GetString("MissionTitleDaily");
		}
		else if (missionType == CMD_Mission.MissionType.Total)
		{
			result = StringMaster.GetString("MissionTitleAccumulation");
		}
		return result;
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
		this.TutorialBegginerClear();
	}

	private void TutorialBegginerClear()
	{
		if (!this.IsValidFocusType(CMD_Mission.MissionType.Beginner))
		{
			TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
			if (tutorialObserver != null)
			{
				GUIMain.BarrierON(null);
				tutorialObserver.StartSecondTutorial("second_tutorial_mission_beginner", new Action(GUIMain.BarrierOFF), delegate
				{
					GUICollider.EnableAllCollider("CMD_Mission");
				});
			}
			else
			{
				GUICollider.EnableAllCollider("CMD_Mission");
			}
		}
	}

	public override void ClosePanel(bool animation = true)
	{
		ClassSingleton<FaceMissionAccessor>.Instance.faceMission.SetBadge(true);
		ClassSingleton<FacePresentAccessor>.Instance.facePresent.SetBadgeOnly();
		GUIFace.SetFacilityShopButtonBadge();
		PartsMenu.SetMenuButtonAlertBadge();
		this.CloseAndFarmCamOn(animation);
		this.missionList.FadeOutAllListParts(null, false);
		this.missionList.SetHideScrollBarAllWays(true);
		this.csSelectPanelMissionSelect.FadeOutAllListParts(null, false);
		this.csSelectPanelMissionSelect.SetHideScrollBarAllWays(true);
	}

	private void CloseAndFarmCamOn(bool animation)
	{
		FarmCameraControlForCMD.On();
		base.ClosePanel(animation);
	}

	public void OnTouchedMission(CMD_Mission.MissionType type)
	{
		CMD_Mission.nowFocusType = type;
		this.SetTitle(CMD_Mission.nowFocusType);
		this.CreateSelectPanel(CMD_Mission.nowFocusType);
	}

	public void OnPushedButton(MissionItem missionItem)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		APIRequestTask task = ClassSingleton<MissionWebAPI>.Instance.AccessMissionRewardLogicAPI(missionItem.missionId);
		base.StartCoroutine(task.Run(delegate
		{
			RestrictionInput.EndLoad();
			this.OpenRewardModalMessage(missionItem);
		}, null, null));
	}

	private void OpenRewardModalMessage(MissionItem missionItem)
	{
		GameWebAPI.RespDataMS_MissionRewardLogic.Result[] result = ClassSingleton<MissionWebAPI>.Instance.MissionRewardLogicData.result;
		int num = 0;
		List<string> list = new List<string>
		{
			StringMaster.GetString("Mission-10")
		};
		foreach (GameWebAPI.RespDataMS_MissionRewardLogic.Result result2 in result)
		{
			if (result2.viewFlg == "1")
			{
				int.TryParse(result2.assetCategoryId, out num);
				string assetTitle = DataMng.Instance().GetAssetTitle(result2.assetCategoryId, result2.assetValue);
				string assetNum = result2.assetNum;
				list.Add(string.Format(StringMaster.GetString("SystemItemCount"), assetTitle, assetNum));
			}
		}
		string info = string.Join("\n", list.ToArray());
		Action<int> action = delegate(int a)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			APIRequestTask apirequestTask = ClassSingleton<MissionWebAPI>.Instance.AccessMissionInfoLogicAPI();
			apirequestTask.Add(DataMng.Instance().RequestMyPageData(false));
			this.StartCoroutine(apirequestTask.Run(delegate
			{
				if (int.Parse(missionItem.missionCategoryId) == 191 || int.Parse(missionItem.missionCategoryId) == 192)
				{
					PartsUpperCutinController.AnimeType playType = (!(missionItem.lastStepFlg == "1")) ? PartsUpperCutinController.AnimeType.StageClear : PartsUpperCutinController.AnimeType.MissionClear;
					Loading.Invisible();
					this.cutinController.PlayAnimator(playType, delegate
					{
						this.RunReMissionInfoLogicAPIHelper();
					});
				}
				else
				{
					this.RunReMissionInfoLogicAPIHelper();
				}
			}, delegate(Exception nop)
			{
				RestrictionInput.EndLoad();
				this.ClosePanel(true);
			}, null));
		};
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(action, "CMD_ModalMessage", null) as CMD_ModalMessage;
		cmd_ModalMessage.Title = StringMaster.GetString("Mission-07");
		cmd_ModalMessage.Info = info;
		cmd_ModalMessage.AdjustSize();
	}

	private void RunReMissionInfoLogicAPIHelper()
	{
		RestrictionInput.EndLoad();
		this.AddAllList();
		this.csSelectPanelMissionSelect.RefreshBadge();
		bool flag = false;
		CMD_Mission.MissionType missionType = this.SetValidFocusType(CMD_Mission.nowFocusType, ref flag);
		if (CMD_Mission.nowFocusType != missionType)
		{
			if (missionType != CMD_Mission.MissionType.INVALID)
			{
				CMD_Mission.nowFocusType = missionType;
				this.csSelectPanelMissionSelect.ResetColorCurIDX();
				this.csSelectPanelMissionSelect.RefreshList(this.missionAllList.Count, 1, null, true);
				int indexByNowFocusType = this.GetIndexByNowFocusType(CMD_Mission.nowFocusType);
				this.csSelectPanelMissionSelect.FadeOutAllListParts(null, true);
				this.csSelectPanelMissionSelect.FadeInAllListParts(null);
				this.csSelectPanelMissionSelect.ResetAnimIDX();
				this.csSelectPanelMissionSelect.SetCellAnimReserve(indexByNowFocusType, false);
				this.SetTitle(CMD_Mission.nowFocusType);
				this.CreateSelectPanel(CMD_Mission.nowFocusType);
			}
			else
			{
				this.csSelectPanelMissionSelect.FadeOutAllListParts(null, true);
				this.missionList.FadeOutAllListParts(null, true);
			}
			if (!this.IsValidFocusType(CMD_Mission.MissionType.Beginner))
			{
				this.TutorialBegginerClear();
			}
		}
		else
		{
			this.CreateSelectPanel(CMD_Mission.nowFocusType);
		}
	}

	public enum MissionType
	{
		INVALID = -1,
		Daily = 1,
		Total,
		Beginner,
		Midrange,
		COUNT
	}

	public class MissionStateData
	{
		public CMD_Mission.MissionType type;

		public bool isFirst = true;

		public float nowSelectLoc;
	}
}
