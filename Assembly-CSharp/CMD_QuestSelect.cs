using Master;
using Quest;
using System;
using System.Collections.Generic;
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
				List<QuestData.WorldStageData> worldStageData_ByAreaID = ClassSingleton<QuestData>.Instance.GetWorldStageData_ByAreaID(worldAreaM_Normal[i].data.worldAreaId, false);
				if (worldStageData_ByAreaID.Count > 0)
				{
					this.worldAreaMList.Add(worldAreaM_Normal[i]);
				}
			}
		}
		base.PartsTitle.SetTitle(StringMaster.GetString("QuestTopTitle"));
		this.InitUI();
		base.Show(f, sizeX, sizeY, aT);
		RestrictionInput.EndLoad();
	}

	private void InitUI()
	{
		this.csSelectPanel.initLocation = true;
		this.csSelectPanel.AllBuild(this.worldAreaMList.Count, true, 1f, 1f, null, null);
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
}
