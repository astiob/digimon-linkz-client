using System;
using UnityEngine;

public class AB_DownLoad_ALLInfo
{
	public AssetBundleMng ABM_Instance;

	public AssetBundleInfoData abid;

	public AB_DownLoadInfo cur_abdlI;

	public Action<int> actEndCallBackAll;

	public int execFCT;

	public int progressFCT;

	public float progressAll;

	public int dlFCT;

	public int dlFCT_COMP;

	public bool bIsAllEnd;

	private bool endFuncOne(AssetBundle ab)
	{
		if (ab)
		{
			ab.Unload(false);
			this.dlFCT_COMP++;
			this.ABM_Instance.AddRealABDL_NowCount_LV(1);
		}
		this.progressFCT++;
		if (this.progressFCT >= this.abid.assetBundleInfoList.Count)
		{
			this.bIsAllEnd = true;
			if (this.actEndCallBackAll != null)
			{
				this.actEndCallBackAll(0);
			}
			try
			{
				AssetBundleMng.Instance().WriteAllRecord();
			}
			catch
			{
				global::Debug.LogWarning("書込み失敗");
			}
			return true;
		}
		return false;
	}

	public void EndCallBackOne(AssetBundle ab, AB_DownLoadInfo abdlI)
	{
		bool flag = this.endFuncOne(ab);
		if (abdlI != null && abdlI.www != null)
		{
			abdlI.www.Dispose();
			abdlI.www = null;
		}
		if (!flag)
		{
			if (this.execFCT < this.abid.assetBundleInfoList.Count)
			{
				this.cur_abdlI = this.ABM_Instance.DownLoad_OneAssetBundleData(this.abid.name, this.abid.ver, this.abid.abPath, this.abid.assetBundleInfoList[this.execFCT++], new Action<AssetBundle, AB_DownLoadInfo>(this.EndCallBackOne), false);
			}
			bool flag2 = this.ABM_Instance.IsODLSExist();
			if (flag2 && this.execFCT < this.abid.assetBundleInfoList.Count)
			{
				AB_DownLoadInfo abdlI2 = this.ABM_Instance.DownLoad_OneAssetBundleData(this.abid.name, this.abid.ver, this.abid.abPath, this.abid.assetBundleInfoList[this.execFCT++], new Action<AssetBundle, AB_DownLoadInfo>(this.EndCallBackOne_ODLS), false);
				this.ABM_Instance.AddODLS(abdlI2);
			}
		}
	}

	public void EndCallBackOne_ODLS(AssetBundle ab, AB_DownLoadInfo abdlI)
	{
		this.ABM_Instance.DelODLS(abdlI);
		bool flag = this.endFuncOne(ab);
		if (abdlI != null && abdlI.www != null)
		{
			abdlI.www.Dispose();
			abdlI.www = null;
		}
		if (!flag)
		{
			bool flag2 = this.ABM_Instance.IsODLSExist();
			if (flag2 && this.execFCT < this.abid.assetBundleInfoList.Count)
			{
				AB_DownLoadInfo abdlI2 = this.ABM_Instance.DownLoad_OneAssetBundleData(this.abid.name, this.abid.ver, this.abid.abPath, this.abid.assetBundleInfoList[this.execFCT++], new Action<AssetBundle, AB_DownLoadInfo>(this.EndCallBackOne_ODLS), false);
				this.ABM_Instance.AddODLS(abdlI2);
			}
		}
	}
}
