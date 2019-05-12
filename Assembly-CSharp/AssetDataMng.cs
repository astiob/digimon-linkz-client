using LitJson;
using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AssetDataMng : MonoBehaviour
{
	public const int MAX_DL_STREAM = 4;

	public bool ShowExtraIcon;

	private bool isInitialized;

	private static AssetDataMng instance;

	public static int assetVersion;

	private string strAssetPassBG = "Data/AssetBundle/BG/";

	private string strAssetPassObj = "Data/AssetBundle/Obj/";

	private string strAssetPassBGM = "Data/AssetBundle/BGM/";

	private string strAssetPassChara = "Data/AssetBundle/Chara/";

	private string strAssetPassTScript = "Data/AssetBundle/TScript/";

	private string strAssetPassVoice = "Data/AssetBundle/Voice/";

	[SerializeField]
	public ABPrefabType USE_ASSET_BUNDLE;

	[SerializeField]
	public bool USE_RESOURCE_DATA_FOR_AB = true;

	private static Action<string> actCB_LevelRecord;

	private List<string> abFolderList;

	private List<AssetBundleInfoData> abidList;

	private Action<int> actCallBack_AB_Init;

	private string www_err_bk = string.Empty;

	private Dictionary<string, AssetBundleInfoData> dlList;

	private int curInfoIDX = -1;

	public bool isAssetBundleDownloading;

	private int currentDownloadStream = 4;

	private string level = string.Empty;

	private int realABDL_TotalCount_LV;

	private AssetDataMng()
	{
	}

	protected virtual void Awake()
	{
		if (this.isInitialized)
		{
			return;
		}
		AssetDataMng.instance = this;
		this.isInitialized = true;
	}

	protected virtual void Start()
	{
	}

	protected virtual void Update()
	{
		this.UpdateDownLoad();
	}

	protected virtual void OnDestroy()
	{
		AssetDataMng.instance = null;
	}

	public static AssetDataMng Instance()
	{
		return AssetDataMng.instance;
	}

	public void Initialize()
	{
		this.Awake();
	}

	public string GetDataPath(ASSETDATAMNG_DATA_TYPE type)
	{
		string empty = string.Empty;
		switch (type)
		{
		case ASSETDATAMNG_DATA_TYPE.BG:
			empty = this.strAssetPassBG;
			break;
		case ASSETDATAMNG_DATA_TYPE.OBJ:
			empty = this.strAssetPassObj;
			break;
		case ASSETDATAMNG_DATA_TYPE.BGM:
			empty = this.strAssetPassBGM;
			break;
		case ASSETDATAMNG_DATA_TYPE.Chara:
			empty = this.strAssetPassChara;
			break;
		case ASSETDATAMNG_DATA_TYPE.TScript:
			empty = this.strAssetPassTScript;
			break;
		case ASSETDATAMNG_DATA_TYPE.Voice:
			empty = this.strAssetPassVoice;
			break;
		}
		return empty;
	}

	public bool IsAssetBundleData(string path)
	{
		for (int i = 0; i < this.abidList.Count; i++)
		{
			if (path.StartsWith(this.abidList[i].abPath))
			{
				return true;
			}
		}
		return false;
	}

	public static void SetLevelRecord_CB(Action<string> act)
	{
		AssetDataMng.actCB_LevelRecord = act;
	}

	public void LoadObjectASync(string path, Action<UnityEngine.Object> actEnd)
	{
		this.LoadObject(path, actEnd, true);
	}

	public UnityEngine.Object LoadObject(string path, Action<UnityEngine.Object> actEnd = null, bool showAlert = true)
	{
		if (actEnd == null && AssetDataCacheMng.Instance() != null)
		{
			bool flag = AssetDataCacheMng.Instance().IsCacheExist(path);
			if (flag)
			{
				return AssetDataCacheMng.Instance().GetCache(path);
			}
		}
		UnityEngine.Object @object;
		if (this.USE_ASSET_BUNDLE == ABPrefabType.None)
		{
			@object = Resources.Load(path);
		}
		else
		{
			if (this.USE_RESOURCE_DATA_FOR_AB)
			{
				@object = Resources.Load(path);
			}
			else if (this.IsAssetBundleData(path))
			{
				@object = null;
			}
			else
			{
				@object = Resources.Load(path);
			}
			if (@object == null)
			{
				string resourceName = string.Empty;
				for (int i = 0; i < this.abidList.Count; i++)
				{
					if (path.Length > this.abidList[i].abPath.Length && path.StartsWith(this.abidList[i].abPath))
					{
						resourceName = path.Substring(this.abidList[i].abPath.Length);
						if (actEnd != null)
						{
							if (AssetBundleMng.Instance().LoadObjectASync(this.abidList[i], resourceName, actEnd))
							{
								return null;
							}
						}
						else
						{
							@object = AssetBundleMng.Instance().LoadObject(this.abidList[i], resourceName);
							if (!(@object == null))
							{
								break;
							}
						}
					}
				}
				global::Debug.Assert(null != @object, "AssetDataMng:LoadObject = " + path + "がない");
			}
		}
		if (@object == null && showAlert)
		{
			string messageString = string.Empty;
			if (File.Exists(path))
			{
				messageString = StringMaster.GetString("AlertFileErrorMemory");
			}
			else
			{
				messageString = StringMaster.GetString("AlertFileErrorNotFound");
			}
			NativeMessageDialog.Show(messageString);
		}
		if (actEnd != null)
		{
			actEnd(@object);
		}
		return @object;
	}

	public bool AB_Init(Action<int> actCallBack)
	{
		if (this.USE_ASSET_BUNDLE == ABPrefabType.None)
		{
			if (actCallBack != null)
			{
				actCallBack(0);
			}
			return false;
		}
		this.actCallBack_AB_Init = actCallBack;
		this.abFolderList = new List<string>();
		this.abidList = new List<AssetBundleInfoData>();
		this.LoadABInfo_FromWWW();
		return true;
	}

	private void LoadABInfo_FromWWW()
	{
		string accessPath = DataMng.Instance().RespDataCM_ABVersion.assetBundleVersionList[0].accessPath;
		WWWHelper wwwhelper = new WWWHelper(accessPath, null, null, 40f);
		base.StartCoroutine(wwwhelper.StartRequest(new Action<string, string, WWWHelper.TimeOut>(this.OnReceivedAssetBundleInfo)));
	}

	private void OnReceivedAssetBundleInfo(string responseText, string errorText, WWWHelper.TimeOut isTimeOut)
	{
		bool flag = false;
		if (!string.IsNullOrEmpty(errorText) || isTimeOut == WWWHelper.TimeOut.YES)
		{
			flag = true;
		}
		else
		{
			try
			{
				AssetBundleFileInfoList assetBundleFileInfoList = JsonMapper.ToObject<AssetBundleFileInfoList>(responseText);
				AssetDataMng.assetVersion = assetBundleFileInfoList.version;
				for (int i = 0; i < assetBundleFileInfoList.assetBundleFileInfo.Count; i++)
				{
					string name = assetBundleFileInfoList.assetBundleFileInfo[i].name;
					this.abFolderList.Add(name);
				}
				base.StartCoroutine(this.LoadABData_FromWWW());
			}
			catch
			{
				flag = true;
			}
		}
		if (flag)
		{
			global::Debug.LogWarningFormat("ERROR : =========== AB_Init => {0} = {1}", new object[]
			{
				DataMng.Instance().RespDataCM_ABVersion.assetBundleVersionList[0].accessPath,
				errorText
			});
			this.OpenAlert(new Action(this.LoadABInfo_FromWWW));
		}
	}

	private IEnumerator LoadABData_FromWWW()
	{
		string strROOT = AssetBundleMng.Instance().GetAssetBundleRootPath();
		string fullpath = string.Empty;
		for (int i = 0; i < this.abFolderList.Count; i++)
		{
			fullpath = string.Concat(new object[]
			{
				strROOT,
				this.abFolderList[i],
				"?",
				AssetDataMng.assetVersion
			});
			bool downloadStart = true;
			while (downloadStart)
			{
				this.www_err_bk = string.Empty;
				WWWHelper www = new WWWHelper(fullpath, null, null, 40f);
				yield return base.StartCoroutine(www.StartRequest(new Action<string, string, WWWHelper.TimeOut>(this.OnReceivedAssetBundleData)));
				if (string.IsNullOrEmpty(this.www_err_bk))
				{
					downloadStart = false;
				}
				else
				{
					bool bClosed = false;
					this.OpenAlert(delegate
					{
						bClosed = true;
					});
					while (!bClosed)
					{
						yield return null;
					}
				}
			}
		}
		int assetBundleCategoryCount = (this.abidList != null) ? this.abidList.Count : 0;
		if (this.actCallBack_AB_Init != null)
		{
			this.actCallBack_AB_Init(assetBundleCategoryCount);
		}
		yield break;
	}

	private void OnReceivedAssetBundleData(string responseText, string errorText, WWWHelper.TimeOut isTimeOut)
	{
		if (!string.IsNullOrEmpty(errorText) || isTimeOut == WWWHelper.TimeOut.YES)
		{
			this.www_err_bk = errorText;
		}
		else
		{
			try
			{
				AssetBundleInfoData item = JsonMapper.ToObject<AssetBundleInfoData>(responseText);
				this.abidList.Add(item);
			}
			catch (JsonException ex)
			{
				this.www_err_bk = ex.ToString();
			}
		}
	}

	private void OpenAlert(Action onClose)
	{
		bool isLoadingShow = Loading.IsShow();
		if (isLoadingShow)
		{
			Loading.Invisible();
		}
		AlertManager.ShowAlertDialog(delegate(int i)
		{
			if (onClose != null)
			{
				onClose();
			}
			if (isLoadingShow)
			{
				Loading.ResumeDisplay();
			}
		}, "LOCAL_ERROR_ASSET_DATA");
	}

	public bool IsInitializedAssetBundle()
	{
		return this.abidList != null && 0 < this.abidList.Count;
	}

	private string GetNameFromFolderPath(string fPath)
	{
		string text = fPath.Substring(0, fPath.Length - 1);
		string[] array = text.Split(new char[]
		{
			'/'
		});
		return array[array.Length - 1];
	}

	public int RealABDL_TotalCount_LV()
	{
		int num = this.realABDL_TotalCount_LV;
		if (num == 0)
		{
			num = 1;
		}
		return num;
	}

	public int RealABDL_NowCount_LV()
	{
		int num = AssetBundleMng.Instance().RealABDL_NowCount_LV();
		if (num > this.realABDL_TotalCount_LV)
		{
			num = this.realABDL_TotalCount_LV;
		}
		if (this.realABDL_TotalCount_LV == 0)
		{
			return 1;
		}
		return num;
	}

	public int GetDownloadAssetBundleCount(string level)
	{
		int num = 0;
		if (this.USE_ASSET_BUNDLE != ABPrefabType.None)
		{
			if (!this.IsAssetBundleDownloading())
			{
				this.level = level;
				AssetBundleMng.Instance().InitDownLoad_All(level);
			}
			for (int i = 0; i < this.abidList.Count; i++)
			{
				num += AssetBundleMng.Instance().GetDLAllCount(this.abidList[i]);
			}
		}
		return num;
	}

	public void StartDownloadAssetBundle(int count, int downloadStream = 4)
	{
		if (this.USE_ASSET_BUNDLE != ABPrefabType.None)
		{
			this.realABDL_TotalCount_LV = count;
			this.dlList = new Dictionary<string, AssetBundleInfoData>();
			this.curInfoIDX = 0;
			this.isAssetBundleDownloading = true;
			this.currentDownloadStream = ((downloadStream <= 0) ? 1 : Math.Min(downloadStream, 4));
			AssetBundleMng.Instance().InitODLStream();
		}
	}

	public bool IsAssetBundleDownloading()
	{
		return this.USE_ASSET_BUNDLE != ABPrefabType.None && this.isAssetBundleDownloading;
	}

	public bool AB_StartDownLoad(string _level = "", int downloadStream = 4)
	{
		this.level = _level;
		bool flag = AssetBundleMng.Instance().InitDownLoad_All(this.level);
		if (flag)
		{
			this.dlList = new Dictionary<string, AssetBundleInfoData>();
			this.curInfoIDX = 0;
			this.isAssetBundleDownloading = true;
			this.currentDownloadStream = ((downloadStream <= 0) ? 1 : Math.Min(downloadStream, 4));
			AssetBundleMng.Instance().InitODLStream();
			this.realABDL_TotalCount_LV = 0;
			for (int i = 0; i < this.abidList.Count; i++)
			{
				int dlallCount = AssetBundleMng.Instance().GetDLAllCount(this.abidList[i]);
				this.realABDL_TotalCount_LV += dlallCount;
			}
		}
		return flag;
	}

	public List<DLProgressInfo> GetAll_AB_DownloadStreamProgress()
	{
		List<DLProgressInfo> list = new List<DLProgressInfo>();
		foreach (string name in this.dlList.Keys)
		{
			AB_DownLoad_ALLInfo ab_DownLoad_ALLInfo = AssetBundleMng.Instance().GetAB_DownLoad_ALLInfo(name);
			if (ab_DownLoad_ALLInfo != null && !ab_DownLoad_ALLInfo.bIsAllEnd)
			{
				list.Add(new DLProgressInfo
				{
					name = ab_DownLoad_ALLInfo.abid.name,
					progress = ab_DownLoad_ALLInfo.progressAll,
					fct = ab_DownLoad_ALLInfo.dlFCT,
					fct_comp = ab_DownLoad_ALLInfo.dlFCT_COMP
				});
			}
		}
		return list;
	}

	private void UpdateDownLoad()
	{
		if (AssetBundleMng.Instance() == null)
		{
			return;
		}
		if (AssetBundleMng.Instance().IsStopDownload())
		{
			return;
		}
		if (this.curInfoIDX > -1)
		{
			if (this.curInfoIDX < this.abidList.Count)
			{
				if (this.dlList.Count < this.currentDownloadStream)
				{
					int dlallCount = AssetBundleMng.Instance().GetDLAllCount(this.abidList[this.curInfoIDX]);
					if (dlallCount > 0)
					{
						AssetBundleMng.Instance().DownLoad_AllAssetBundleData(this.abidList[this.curInfoIDX].name, this.abidList[this.curInfoIDX], null);
						this.dlList.Add(this.abidList[this.curInfoIDX].name, this.abidList[this.curInfoIDX]);
					}
					this.curInfoIDX++;
				}
			}
			else if (this.curInfoIDX >= this.abidList.Count && this.dlList.Count < this.currentDownloadStream)
			{
				int odlstreamNum = this.currentDownloadStream - this.dlList.Count;
				AssetBundleMng.Instance().SetODLStreamNum(odlstreamNum);
			}
			foreach (string text in this.dlList.Keys)
			{
				AB_DownLoad_ALLInfo ab_DownLoad_ALLInfo = AssetBundleMng.Instance().GetAB_DownLoad_ALLInfo(text);
				if (ab_DownLoad_ALLInfo != null && ab_DownLoad_ALLInfo.bIsAllEnd)
				{
					this.dlList.Remove(text);
					if (ab_DownLoad_ALLInfo.cur_abdlI != null && ab_DownLoad_ALLInfo.cur_abdlI.www != null)
					{
						ab_DownLoad_ALLInfo.cur_abdlI.www.Dispose();
					}
					GC.Collect();
					break;
				}
			}
			if (this.curInfoIDX >= this.abidList.Count && this.dlList.Count == 0)
			{
				this.isAssetBundleDownloading = false;
				this.curInfoIDX = -1;
			}
		}
	}

	public static string GetWebAssetImagePath()
	{
		return ConstValue.APP_ASSET_DOMAIN + "/asset/img";
	}
}
