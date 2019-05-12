using System;
using System.IO;
using System.Linq;
using UnityEngine;

public static class GameCache
{
	public static void ClearCache(string[] ignoreFileNameList = null)
	{
		try
		{
			GameCache.<ClearCache>c__AnonStorey44C <ClearCache>c__AnonStorey44C = new GameCache.<ClearCache>c__AnonStorey44C();
			DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath);
			<ClearCache>c__AnonStorey44C.fileInfoList = directoryInfo.GetFiles();
			int i;
			for (i = 0; i < <ClearCache>c__AnonStorey44C.fileInfoList.Length; i++)
			{
				if (ignoreFileNameList == null || !ignoreFileNameList.Any((string x) => x == <ClearCache>c__AnonStorey44C.fileInfoList[i].Name))
				{
					File.Delete(Application.persistentDataPath + "/" + <ClearCache>c__AnonStorey44C.fileInfoList[i].Name);
				}
			}
		}
		catch
		{
			global::Debug.Log("ゲームキャッシュの削除");
		}
	}
}
