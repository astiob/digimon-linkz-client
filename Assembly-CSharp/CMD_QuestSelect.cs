using Master;
using Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CMD_QuestSelect : CMD
{
	[SerializeField]
	private GUISelectPanelViewPartsUD csSelectPanel;

	public static CMD_QuestSelect instance;

	private List<QuestData.WorldAreaData> worldAreaMList;

	public QuestData.WorldAreaData GetData(int idx)
	{
		return this.worldAreaMList[idx];
	}

	protected override void Awake()
	{
		base.Awake();
		CMD_QuestSelect.instance = this;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		GUICollider.DisableAllCollider("CMD_QuestSelect");
		List<QuestData.WorldAreaData> worldAreaM_Normal = ClassSingleton<QuestData>.Instance.GetWorldAreaM_Normal();
		this.worldAreaMList = new List<QuestData.WorldAreaData>();
		for (int i = 0; i < worldAreaM_Normal.Count; i++)
		{
			if (worldAreaM_Normal[i].isActive)
			{
				List<QuestData.WorldStageData> worldStageData_ByAreaID = ClassSingleton<QuestData>.Instance.GetWorldStageData_ByAreaID(worldAreaM_Normal[i].data.worldAreaId);
				DkLog.W(string.Format("{0} : {1} : {2}", worldAreaM_Normal[i].data.name, worldAreaM_Normal[i].isActive, worldStageData_ByAreaID.Count), false);
				if (worldStageData_ByAreaID.Count > 0)
				{
					this.worldAreaMList.Add(worldAreaM_Normal[i]);
				}
			}
		}
		bool flag = this.IsColosseumOpen() && DataMng.Instance().IsReleaseColosseum;
		if (flag)
		{
			GameWebAPI.RespDataMA_GetWorldAreaM respDataMA_WorldAreaM = MasterDataMng.Instance().RespDataMA_WorldAreaM;
			GameWebAPI.RespDataMA_GetWorldAreaM.WorldAreaM data = respDataMA_WorldAreaM.worldAreaM.Where((GameWebAPI.RespDataMA_GetWorldAreaM.WorldAreaM x) => x.worldAreaId == "5").FirstOrDefault<GameWebAPI.RespDataMA_GetWorldAreaM.WorldAreaM>();
			QuestData.WorldAreaData worldAreaData = new QuestData.WorldAreaData();
			worldAreaData.data = data;
			worldAreaData.isActive = true;
			this.worldAreaMList.Add(worldAreaData);
		}
		base.PartsTitle.SetTitle(StringMaster.GetString("QuestTopTitle"));
		this.InitUI();
		base.Show(f, sizeX, sizeY, aT);
		base.SetTutorialAnyTime("anytime_second_tutorial_quest");
		RestrictionInput.EndLoad();
	}

	private void InitUI()
	{
		this.csSelectPanel.initLocation = true;
		this.csSelectPanel.AllBuild(this.worldAreaMList.Count, true, 1f, 1f, null, null, true);
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
		QuestSecondTutorial.StartQuestSelectTutorial();
	}

	public override void ClosePanel(bool animation = true)
	{
		this.CloseAndFarmCamOn(animation);
	}

	private void CloseAndFarmCamOn(bool animation)
	{
		FarmCameraControlForCMD.On();
		base.ClosePanel(animation);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		CMD_QuestSelect.instance = null;
	}

	private bool IsColosseumOpen()
	{
		GameWebAPI.RespData_ColosseumInfoLogic respData_ColosseumInfo = DataMng.Instance().RespData_ColosseumInfo;
		List<CMD_QuestSelect.Schedule> list = new List<CMD_QuestSelect.Schedule>();
		if (respData_ColosseumInfo == null || respData_ColosseumInfo.colosseumId == 0)
		{
			list.Clear();
			return false;
		}
		if (respData_ColosseumInfo.openAllDay > 0)
		{
			return true;
		}
		if (list.Count<CMD_QuestSelect.Schedule>() == 0)
		{
			GameWebAPI.RespDataMA_ColosseumTimeScheduleM respDataMA_ColosseumTimeScheduleMaster = MasterDataMng.Instance().RespDataMA_ColosseumTimeScheduleMaster;
			if (respDataMA_ColosseumTimeScheduleMaster == null)
			{
				return false;
			}
			string b = respData_ColosseumInfo.colosseumId.ToString();
			foreach (GameWebAPI.RespDataMA_ColosseumTimeScheduleM.ColosseumTimeSchedule colosseumTimeSchedule in respDataMA_ColosseumTimeScheduleMaster.colosseumTimeScheduleM)
			{
				if (colosseumTimeSchedule.colosseumId == b)
				{
					CMD_QuestSelect.Schedule item = new CMD_QuestSelect.Schedule
					{
						start = DateTime.Parse(colosseumTimeSchedule.startHour),
						end = DateTime.Parse(colosseumTimeSchedule.endHour)
					};
					list.Add(item);
				}
			}
		}
		foreach (CMD_QuestSelect.Schedule schedule in list)
		{
			if (schedule.start < ServerDateTime.Now && ServerDateTime.Now < schedule.end)
			{
				return true;
			}
		}
		return false;
	}

	private struct Schedule
	{
		public DateTime start;

		public DateTime end;
	}
}
