using System;
using UnityEngine;

public class GUISelectRewardListPanel : GUISelectPanelBSPartsUD
{
	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Update()
	{
		base.Update();
	}

	public void AllBuild(GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission data)
	{
		base.InitBuild();
		this.partsCount = data.reward.Length;
		GUISelectPanelBSPartsUD.PanelBuildData panelBuildData = base.CalcBuildData(1, this.partsCount, 1f, 1f);
		float startX = panelBuildData.startX;
		float num = panelBuildData.startY;
		if (data != null)
		{
			foreach (GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission.Reward data2 in data.reward)
			{
				GameObject gameObject = base.AddBuildPart();
				GUIListPartsMissionReward component = gameObject.GetComponent<GUIListPartsMissionReward>();
				if (component != null)
				{
					component.SetOriginalPos(new Vector3(startX, num, -5f));
					component.Data = data2;
				}
				num -= panelBuildData.pitchH;
			}
		}
		base.height = panelBuildData.lenH;
		base.InitMinMaxLocation(-1, 0f);
	}
}
