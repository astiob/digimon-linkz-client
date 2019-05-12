using System;
using System.Collections.Generic;
using UnityEngine;

public class ChipList
{
	private GameObject gameObject;

	private GUISelectPanelChipList guiSelectPanelChipList;

	private int widthLength;

	private Vector2 windowSize = Vector2.zero;

	public ChipList(GameObject parent, int widthLength, Vector2 windowSize, GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] dataList, bool shouldDim = false, bool isInitLocation = true)
	{
		this.AllBuild(parent, widthLength, windowSize, dataList, shouldDim, isInitLocation);
	}

	public ChipList(GameObject parent, int widthLength, Vector2 windowSize, List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList> dataList)
	{
		this.AllBuild(parent, widthLength, windowSize, dataList.ToArray(), false, false);
	}

	private void AllBuild(GameObject parent, int widthLength, Vector2 windowSize, GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] dataList, bool shouldDim = false, bool isInitLocation = false)
	{
		this.gameObject = new GameObject();
		this.gameObject.name = "ChipListBase";
		this.gameObject.transform.SetParent(parent.transform);
		this.gameObject.transform.localPosition = Vector3.zero;
		this.gameObject.transform.localScale = Vector3.one;
		GameObject gameObject = GUIManager.LoadCommonGUI("SelectListPanel/SelectListPanelChip", this.gameObject);
		GameObject gameObject2 = GUIManager.LoadCommonGUI("ListParts/ListPartsChip", this.gameObject);
		this.guiSelectPanelChipList = gameObject.GetComponent<GUISelectPanelChipList>();
		this.guiSelectPanelChipList.selectParts = gameObject2;
		gameObject2.SetActive(false);
		this.widthLength = widthLength;
		this.windowSize = windowSize;
		this.guiSelectPanelChipList.AllBuild(widthLength, windowSize, dataList, shouldDim, isInitLocation);
	}

	public void ReAllBuild(GameWebAPI.RespDataCS_ChipListLogic.UserChipList[] dataList, bool shouldDim = false, bool isInitLocation = false)
	{
		this.guiSelectPanelChipList.AllBuild(this.widthLength, this.windowSize, dataList, shouldDim, isInitLocation);
		this.guiSelectPanelChipList.RefreshList(dataList.Length, isInitLocation);
	}

	public void ReAllBuild(List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList> dataList, bool isInitLocation = false)
	{
		this.guiSelectPanelChipList.AllBuild(this.widthLength, this.windowSize, dataList, isInitLocation);
	}

	public void AddWidgetDepth(int depth)
	{
		this.guiSelectPanelChipList.AddWidgetDepth(depth);
	}

	public void SetPosition(Vector3 position)
	{
		this.gameObject.transform.localPosition = position;
	}

	public void SetScrollBarPosX(float x)
	{
		this.guiSelectPanelChipList.ScrollBarPosX = x;
		this.guiSelectPanelChipList.ScrollBarBGPosX = x;
	}

	public void SetShortTouchCallback(Action<GUIListChipParts.Data> callback)
	{
		this.guiSelectPanelChipList.SetShortTouchCallback(callback);
	}

	public void SetLongTouchCallback(Action<GUIListChipParts.Data> callback)
	{
		this.guiSelectPanelChipList.SetLongTouchCallback(callback);
	}

	public void SetSelectColor(int userChipId, bool isSelect)
	{
		this.guiSelectPanelChipList.SetSelectColor(userChipId, isSelect);
	}

	public void SetNowSelectMessage(int userChipId, bool isSelect)
	{
		this.guiSelectPanelChipList.SetNowSelectMessage(userChipId, isSelect);
	}

	public void SetAllSelectColor(bool isSelect)
	{
		this.guiSelectPanelChipList.SetAllSelectColor(isSelect);
	}

	public void SetSelectMessage(int userChipId, string value)
	{
		this.guiSelectPanelChipList.SetSelectMessage(userChipId, value);
	}

	public void SetAllSelectMessage(string value)
	{
		this.guiSelectPanelChipList.SetAllSelectMessage(value);
	}

	public void SetTouchAreaWidth(float w)
	{
		BoxCollider component = this.guiSelectPanelChipList.GetComponent<BoxCollider>();
		Vector3 size = component.size;
		size.x = w;
		component.size = size;
	}
}
