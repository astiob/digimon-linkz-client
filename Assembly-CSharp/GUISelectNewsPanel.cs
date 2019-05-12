using System;
using System.Collections.Generic;
using UnityEngine;

public class GUISelectNewsPanel : GUISelectPanelBSPartsUD
{
	private List<GUIListNewsParts> guiListNewsPartsList;

	private Action callbackCloseParts;

	public int AllBuild(GameWebAPI.RespDataIN_InfoList dts, int tabNumber, Action callbackCloseParts = null)
	{
		this.callbackCloseParts = callbackCloseParts;
		this.guiListNewsPartsList = new List<GUIListNewsParts>();
		base.InitBuild();
		this.partsCount = 0;
		if (base.selectCollider != null)
		{
			foreach (GameWebAPI.RespDataIN_InfoList.InfoList infoList2 in dts.infoList)
			{
				if (tabNumber == 1)
				{
					this.partsCount++;
				}
				else if (tabNumber == 2)
				{
					if (infoList2.groupType == "3")
					{
						this.partsCount++;
					}
				}
				else if (tabNumber == 3 && infoList2.groupType != "3")
				{
					this.partsCount++;
				}
			}
			GUISelectPanelBSPartsUD.PanelBuildData panelBuildData = base.CalcBuildData(1, this.partsCount, 1f, 1f);
			float num = panelBuildData.startY;
			foreach (GameWebAPI.RespDataIN_InfoList.InfoList infoList4 in dts.infoList)
			{
				if (tabNumber == 1)
				{
					this.CreateInfoItem(infoList4, new Vector3(0f, num, -5f));
					num -= panelBuildData.pitchH;
				}
				else if (tabNumber == 2)
				{
					if (infoList4.groupType == "3")
					{
						this.CreateInfoItem(infoList4, new Vector3(0f, num, -5f));
						num -= panelBuildData.pitchH;
					}
				}
				else if (tabNumber == 3 && infoList4.groupType != "3")
				{
					this.CreateInfoItem(infoList4, new Vector3(0f, num, -5f));
					num -= panelBuildData.pitchH;
				}
			}
			base.height = panelBuildData.lenH;
			base.InitMinMaxLocation(-1, 0f);
		}
		return this.partsCount;
	}

	private void CreateInfoItem(GameWebAPI.RespDataIN_InfoList.InfoList dt, Vector3 setPos)
	{
		GameObject gameObject = base.AddBuildPart();
		GUIListNewsParts component = gameObject.GetComponent<GUIListNewsParts>();
		if (component != null)
		{
			component.SetOriginalPos(setPos);
			component.Data = dt;
			component.callbackClose = new Action(this.CallbackCloseParts);
			this.guiListNewsPartsList.Add(component);
		}
	}

	private void CallbackCloseParts()
	{
		if (this.callbackCloseParts != null)
		{
			this.callbackCloseParts();
		}
	}

	public void Refresh(GameWebAPI.RespDataIN_InfoList dts)
	{
		foreach (GUIListNewsParts guilistNewsParts in this.guiListNewsPartsList)
		{
			foreach (GameWebAPI.RespDataIN_InfoList.InfoList infoList2 in dts.infoList)
			{
				if (guilistNewsParts.Data.userInfoId == infoList2.userInfoId)
				{
					guilistNewsParts.Data = infoList2;
					break;
				}
			}
		}
	}
}
