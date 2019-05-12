using System;
using System.Collections;
using UnityEngine;

public class DropItemList
{
	private GameObject gameObject;

	private GUISelectPanelDropItemList guiSelectPanelChipList;

	public DropItemList(GameObject parent, int widthLength, Vector2 windowSize, GameWebAPI.RespDataWD_DungeonStart.Drop[] standardDrops, GameWebAPI.RespDataWD_DungeonStart.LuckDrop luckDrop, GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.DropReward[] ownerMultiDrops, GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.DropReward[] multiDrops, GameWebAPI.RespData_WorldMultiResultInfoLogic.DungeonReward.LuckDrop[] multiLuckDrops, GameWebAPI.RespDataWD_DungeonResult.OptionDrop[] optionDrops, GameWebAPI.RespDataWD_DungeonResult.EventChipReward[] eventChipRewards)
	{
		this.gameObject = new GameObject();
		this.gameObject.name = "DropListBase";
		this.gameObject.transform.SetParent(parent.transform);
		this.gameObject.transform.localPosition = Vector3.zero;
		this.gameObject.transform.localScale = Vector3.one;
		GameObject gameObject = GUIManager.LoadCommonGUI("SelectListPanel/SelectListPanelDropItem", this.gameObject);
		GameObject gameObject2 = GUIManager.LoadCommonGUI("ListParts/ListParts_drop", this.gameObject);
		this.guiSelectPanelChipList = gameObject.GetComponent<GUISelectPanelDropItemList>();
		this.guiSelectPanelChipList.selectParts = gameObject2;
		gameObject2.SetActive(false);
		this.guiSelectPanelChipList.AllBuild(widthLength, windowSize, standardDrops, luckDrop, ownerMultiDrops, multiDrops, multiLuckDrops, optionDrops, eventChipRewards);
	}

	public IEnumerator SetDrops(bool isSkip = false, Action callBack = null)
	{
		return this.guiSelectPanelChipList.SetDrops(isSkip, callBack);
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
}
