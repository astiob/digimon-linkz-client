using System;
using System.Collections.Generic;

[Serializable]
public class AssetBundleInfoData
{
	public int ver;

	public DateTime time;

	public string name;

	public string abPath;

	public List<AssetBundleInfo> assetBundleInfoList;
}
