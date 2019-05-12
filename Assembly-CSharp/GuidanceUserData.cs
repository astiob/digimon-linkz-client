using System;
using System.Collections;
using System.Collections.Generic;

public static class GuidanceUserData
{
	public static IEnumerator RequestNavigation(Action<int[]> onCompleted)
	{
		int[] idList = null;
		GameWebAPI.RequestNV_NavigationMessage request = new GameWebAPI.RequestNV_NavigationMessage
		{
			OnReceived = delegate(GameWebAPI.RespDataNV_NavigationMessage response)
			{
				idList = response.navigationMessageIdList;
			}
		};
		return request.Run(delegate()
		{
			if (onCompleted != null)
			{
				onCompleted(idList);
			}
		}, null, null);
	}

	public static IEnumerator RequestFinishSave(List<GameWebAPI.RespDataMA_NavigationMessageMaster.NavigationMessageInfo> infoList)
	{
		IEnumerator result = null;
		if (infoList != null && 0 < infoList.Count)
		{
			int[] doneGuidanceIdList = new int[infoList.Count];
			for (int i = 0; i < infoList.Count; i++)
			{
				doneGuidanceIdList[i] = infoList[i].navigationMessageId;
			}
			GameWebAPI.RequestNV_NavigationMessageReadStatusUpdate request = new GameWebAPI.RequestNV_NavigationMessageReadStatusUpdate
			{
				SetSendData = delegate(GameWebAPI.SendDataNV_NavigationMessageReadStatusUpdate param)
				{
					param.navigationMessageId = doneGuidanceIdList;
				}
			};
			result = request.Run(delegate()
			{
				GuidanceSelector.DeleteDoneFlag(infoList);
			}, null, null);
		}
		return result;
	}
}
