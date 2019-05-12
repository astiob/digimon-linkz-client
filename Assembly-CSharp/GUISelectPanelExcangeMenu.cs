using System;
using System.Collections.Generic;
using UnityEngine;

public class GUISelectPanelExcangeMenu : GUISelectPanelBSPartsUD
{
	public List<ExchangeMenuItem> AllBuild(List<GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result> dts)
	{
		base.InitBuild();
		List<GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result> availableInfo = this.GetAvailableInfo(dts);
		List<ExchangeMenuItem> list = new List<ExchangeMenuItem>();
		this.partsCount = availableInfo.Count;
		if (base.selectCollider != null)
		{
			GUISelectPanelBSPartsUD.PanelBuildData panelBuildData = base.CalcBuildData(1, this.partsCount, 1f, 1f);
			float num = panelBuildData.startY;
			for (int i = 0; i < this.partsCount; i++)
			{
				GameObject gameObject = base.AddBuildPart();
				ExchangeMenuItem component = gameObject.GetComponent<ExchangeMenuItem>();
				if (component != null)
				{
					component.Init(availableInfo[i]);
					component.SetOriginalPos(new Vector3(0f, num, -5f));
					list.Add(component);
				}
				num -= panelBuildData.pitchH;
			}
			base.height = panelBuildData.lenH;
			base.InitMinMaxLocation(-1, 0f);
		}
		return list;
	}

	private List<GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result> GetAvailableInfo(List<GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result> infoList)
	{
		List<GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result> list = new List<GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result>();
		foreach (GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result result in infoList)
		{
			DateTime t = DateTime.Parse(result.startTime);
			DateTime t2 = DateTime.Parse(result.endTime);
			if (DateTime.Compare(t, ServerDateTime.Now) < 0 || DateTime.Compare(t2, ServerDateTime.Now) > 0)
			{
				list.Add(result);
			}
		}
		return infoList;
	}
}
