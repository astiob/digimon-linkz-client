using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PartsQuestDetails : MonoBehaviour
{
	public string worldEventId;

	protected virtual void Awake()
	{
	}

	public virtual void ShowData()
	{
	}

	protected virtual void ShowNumber(List<UISprite> spL, int num, bool dontSHowZero = false)
	{
		TextUtil.ShowNumber(spL, "Common02_FriendshipN_", num, dontSHowZero);
	}

	protected virtual void OnDestroy()
	{
	}

	private void OnClickedDetail()
	{
		CMDWebWindow cmdwebWindow = GUIMain.ShowCommonDialog(null, "CMDWebWindow") as CMDWebWindow;
		cmdwebWindow.TitleText = StringMaster.GetString("PointQuest-1");
		cmdwebWindow.Url = string.Format(WebAddress.EXT_ADR_POINT_QUEST_DETAIL, this.worldEventId);
	}
}
