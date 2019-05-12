using System;
using UnityEngine;

public class AB_DownLoadInfo
{
	public int ver;

	public string abPath;

	public AssetBundleInfo abInfo;

	public Action<AssetBundle, AB_DownLoadInfo> actEndCallBack;

	public float progress;

	public WWW www;
}
