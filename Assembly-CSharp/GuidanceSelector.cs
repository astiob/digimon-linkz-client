using System;
using System.Collections.Generic;
using UnityEngine;

public static class GuidanceSelector
{
	private static bool IsDone(string fileName)
	{
		bool result = false;
		if (!string.IsNullOrEmpty(fileName))
		{
			result = PlayerPrefs.HasKey(fileName);
		}
		return result;
	}

	public static GameWebAPI.RespDataMA_NavigationMessageMaster.NavigationMessageInfo Select(List<GameWebAPI.RespDataMA_NavigationMessageMaster.NavigationMessageInfo> infoList)
	{
		GameWebAPI.RespDataMA_NavigationMessageMaster.NavigationMessageInfo result = null;
		if (infoList != null && 0 < infoList.Count)
		{
			for (int i = 0; i < infoList.Count; i++)
			{
				if (!GuidanceSelector.IsDone(infoList[i].scriptPath))
				{
					result = infoList[i];
					break;
				}
			}
		}
		return result;
	}

	public static List<GameWebAPI.RespDataMA_NavigationMessageMaster.NavigationMessageInfo> GetDoneList(List<GameWebAPI.RespDataMA_NavigationMessageMaster.NavigationMessageInfo> infoList)
	{
		List<GameWebAPI.RespDataMA_NavigationMessageMaster.NavigationMessageInfo> list = new List<GameWebAPI.RespDataMA_NavigationMessageMaster.NavigationMessageInfo>();
		if (infoList != null && 0 < infoList.Count)
		{
			for (int i = 0; i < infoList.Count; i++)
			{
				if (GuidanceSelector.IsDone(infoList[i].scriptPath))
				{
					list.Add(infoList[i]);
				}
			}
		}
		return list;
	}

	public static void DeleteDoneFlag(List<GameWebAPI.RespDataMA_NavigationMessageMaster.NavigationMessageInfo> infoList)
	{
		if (infoList != null && 0 < infoList.Count)
		{
			for (int i = 0; i < infoList.Count; i++)
			{
				if (PlayerPrefs.HasKey(infoList[i].scriptPath))
				{
					PlayerPrefs.DeleteKey(infoList[i].scriptPath);
				}
			}
		}
	}
}
