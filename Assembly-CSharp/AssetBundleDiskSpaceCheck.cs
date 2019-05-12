using System;
using System.Collections;
using UnityEngine;

public static class AssetBundleDiskSpaceCheck
{
	public static long GetFileSize(long size)
	{
		return size * 2L;
	}

	public static IEnumerator CheckDiskSpace(AssetBundleInfo info, AssetBundleMng manager)
	{
		long size = AssetBundleDiskSpaceCheck.GetFileSize(long.Parse(info.size));
		while (!AssetBundleDiskSpaceCheck.IsFreeSpace(size + manager.GetDownloadFileSize()))
		{
			while (0 < manager.GetCountDownloadProcess())
			{
				yield return null;
			}
			bool isOpen = true;
			AlertManager.ShowAlertDialog(delegate(int nop)
			{
				isOpen = false;
			}, "LOCAL_ERROR_SAVE_DATA_IO");
			while (isOpen)
			{
				yield return null;
			}
		}
		yield break;
	}

	private static bool IsFreeSpace(long writeSize)
	{
		long freeSpace = AssetBundleDiskSpaceCheck.GetFreeSpace(Application.temporaryCachePath);
		return freeSpace > writeSize;
	}

	private static long GetFreeSpace(string pass)
	{
		return UserSystemSettings.GetFreeSpace(pass);
	}
}
