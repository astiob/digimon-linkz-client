using System;
using System.Collections.Generic;
using UnityEngine;

public class GUISelectPanelChipList : GUISelectPanelViewPartsUD
{
	public static List<GUIListChipParts.Data> partsDataList;

	public void AllBuild(int widthLength, Vector2 windowSize, List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList> dataList)
	{
		this.AllBuild(widthLength, windowSize, dataList.ToArray(), false);
	}

	public void AllBuild(int widthLength, Vector2 windowSize, GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] dataList, bool shouldDim = false)
	{
		Rect listWindowViewRect = default(Rect);
		float num = windowSize.x * 0.5f;
		float num2 = windowSize.y * 0.5f;
		listWindowViewRect.xMin = -num;
		listWindowViewRect.xMax = num;
		listWindowViewRect.yMin = -num2;
		listWindowViewRect.yMax = num2;
		base.ListWindowViewRect = listWindowViewRect;
		this.fRecycleViewMaxY = num2 * 1.5f;
		this.fRecycleViewMinY = -num2 * 1.5f;
		this.PARTS_CT_MN = widthLength;
		this.RecycleViewSectorSize = 4;
		List<string> myDigimonChipGroupIds = null;
		if (shouldDim)
		{
			myDigimonChipGroupIds = CMD_ChipSphere.DataChg.GetChipEquip().GetChipGroupList();
		}
		GUISelectPanelChipList.partsDataList = new List<GUIListChipParts.Data>();
		for (int i = 0; i < dataList.Length; i++)
		{
			GUIListChipParts.Data data = new GUIListChipParts.Data();
			data.index = i;
			data.userChip = dataList[i];
			data.masterChip = ChipDataMng.GetChipMainData(dataList[i].chipId.ToString());
			data.shouldDim = shouldDim;
			data.myDigimonChipGroupIds = myDigimonChipGroupIds;
			GUISelectPanelChipList.partsDataList.Add(data);
		}
		base.initLocation = true;
		base.AllBuild(dataList.Length, true, 1f, 1f, null, null);
	}

	public void RefreshList(int partsCount)
	{
		base.RefreshList(partsCount, this.PARTS_CT_MN, null, true);
	}

	public void AddWidgetDepth(int depth)
	{
		base.GetComponent<DepthController>().AddWidgetDepth(base.transform, depth);
	}

	public void SetShortTouchCallback(Action<GUIListChipParts.Data> callback)
	{
		foreach (GUIListChipParts.Data data in GUISelectPanelChipList.partsDataList)
		{
			data.actTouchShort = callback;
		}
	}

	public void SetLongTouchCallback(Action<GUIListChipParts.Data> callback)
	{
		foreach (GUIListChipParts.Data data in GUISelectPanelChipList.partsDataList)
		{
			data.actTouchLong = callback;
		}
	}

	public void SetNowSelectMessage(int userChiId, bool isSelect)
	{
		foreach (GUIListChipParts.Data data in GUISelectPanelChipList.partsDataList)
		{
			if (data.userChip.userChipId == userChiId)
			{
				GameObject onePart = base.GetOnePart(data.index);
				if (onePart != null)
				{
					GUIListChipParts component = onePart.GetComponent<GUIListChipParts>();
					component.SetNowSelectMessage(isSelect);
				}
				break;
			}
		}
	}

	public void SetSelectColor(int userChiId, bool isSelect)
	{
		foreach (GUIListChipParts.Data data in GUISelectPanelChipList.partsDataList)
		{
			if (data.userChip.userChipId == userChiId)
			{
				data.isSelect = isSelect;
				GameObject onePart = base.GetOnePart(data.index);
				if (onePart != null)
				{
					GUIListChipParts component = onePart.GetComponent<GUIListChipParts>();
					component.SetSelectColor(isSelect);
				}
				break;
			}
		}
	}

	public void SetAllSelectColor(bool isSelect)
	{
		foreach (GUIListChipParts.Data data in GUISelectPanelChipList.partsDataList)
		{
			data.isSelect = isSelect;
			GameObject onePart = base.GetOnePart(data.index);
			if (onePart != null)
			{
				GUIListChipParts component = onePart.GetComponent<GUIListChipParts>();
				component.SetSelectColor(isSelect);
			}
		}
	}

	public void SetSelectMessage(int userChiId, string value)
	{
		foreach (GUIListChipParts.Data data in GUISelectPanelChipList.partsDataList)
		{
			if (data.userChip.userChipId == userChiId)
			{
				data.selectMessage = value;
				GameObject onePart = base.GetOnePart(data.index);
				if (onePart != null)
				{
					GUIListChipParts component = onePart.GetComponent<GUIListChipParts>();
					component.SetSelectMessage(value);
				}
				break;
			}
		}
	}

	public void SetAllSelectMessage(string value)
	{
		foreach (GUIListChipParts.Data data in GUISelectPanelChipList.partsDataList)
		{
			data.selectMessage = value;
			GameObject onePart = base.GetOnePart(data.index);
			if (onePart != null)
			{
				GUIListChipParts component = onePart.GetComponent<GUIListChipParts>();
				component.SetSelectMessage(value);
			}
		}
	}
}
