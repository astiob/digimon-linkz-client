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
		UnityEngine.Object @object;
		if (actEnd == null && AssetDataCacheMng.Instance() != null)
		{
			@object = this.GetFromCache(path);
			if (@object != null)
			{
				return @object;
			}
		}
		if (this.USE_ASSET_BUNDLE == ABPrefabType.None)
		{
			@object = this.GetFromInternalResource(path);
		}
		else
		{
			if (this.USE_RESOURCE_DATA_FOR_AB)
			{
				@object = this.GetFromInternalResource(path);
			}
			else if (this.IsAssetBundleData(path))
			{
				@object = null;
			}
			else
			{
				@object = this.GetFromInternalResource(path);
			}
			if (@object == null)
			{
				AssetDataMng.FindABInfoResult findABInfoResult = this.FindAssetBundleInfo(path);
				if (findABInfoResult != null)
				{
					if (actEnd != null)
					{
						bool flag = AssetBundleMng.Instance().LoadObjectASync(findABInfoResult.abInfoData, findABInfoResult.objName, actEnd);
						if (flag)
						{
							return null;
						}
					}
					else
					{
						@object = AssetBundleMng.Instance().LoadObject(findABInfoResult.abInfoData, findABInfoResult.objName);
					}
				}
				if (@object == null)
				{
					global::Debug.LogError("====================================== AssetDataMng:LoadObject = " + path + "がない！");
				}
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

	private AssetDataMng.FindABInfoResult FindAssetBundleInfo(string path)
	{
		string localizedPath = AssetDataMng.GetLocalizedPath(path);
		AssetDataMng.FindABInfoResult findABInfoResult = this._FindAssetBundleInfo(localizedPath);
		if (!localizedPath.Equals(path) && findABInfoResult == null)
		{
			findABInfoResult = this._FindAssetBundleInfo(path);
		}
		return findABInfoResult;
	}

	private AssetDataMng.FindABInfoResult _FindAssetBundleInfo(string path)
	{
		string text = string.Empty;
		for (int i = 0; i < this.abidList.Count; i++)
		{
			if (path.Length > this.abidList[i].abPath.Length && path.StartsWith(this.abidList[i].abPath))
			{
				text = path.Substring(this.abidList[i].abPath.Length);
				List<AssetBundleInfo> assetBundleInfoList = this.abidList[i].assetBundleInfoList;
				for (int j = 0; j < assetBundleInfoList.Count; j++)
				{
					int k;
					for (k = 0; k < assetBundleInfoList[j].objNameList.Count; k++)
					{
						if (text == assetBundleInfoList[j].objNameList[k])
						{
							break;
						}
					}
					if (k < assetBundleInfoList[j].objNameList.Count && assetBundleInfoList[j].ContainsPath(text))
					{
						return new AssetDataMng.FindABInfoResult
						{
							abInfoData = this.abidList[i],
							objName = text
						};
					}
				}
			}
		}
		return null;
	}

	private UnityEngine.Object GetFromCache(string path)
	{
		string localizedPath = AssetDataMng.GetLocalizedPath(path);
		UnityEngine.Object @object = this._GetFromCache(localizedPath);
		if (!localizedPath.Equals(path) && @object == null)
		{
			@object = this._GetFromCache(path);
		}
		return @object;
	}

	private UnityEngine.Object _GetFromCache(string path)
	{
		bool flag = AssetDataCacheMng.Instance().IsCacheExist(path);
		if (flag)
		{
			return AssetDataCacheMng.Instance().GetCache(path);
		}
		return null;
	}

	private UnityEngine.Object GetFromInternalResource(string path)
	{
		string localizedPath = AssetDataMng.GetLocalizedPath(path);
		UnityEngine.Object @object = Resources.Load(localizedPath);
		if (!localizedPath.Equals(path) && @object == null)
		{
			@object = Resources.Load(path);
		}
		return @object;
	}

	public static string GetLocalizedPath(string path)
	{
		string countryPrefix = CountrySetting.GetCountryPrefix(CountrySetting.CountryCode.EN);
		return (!string.IsNullOrEmpty(countryPrefix)) ? string.Format("{0}/{1}", countryPrefix, path) : path;
	}

	public bool IsIncludedInAssetBundle(string path)
	{
		return this._FindAssetBundleInfo(path) != null;
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
		string version = DataMng.Instance().RespDataCM_ABVersion.assetBundleVersionList[0].version;
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
			global::Debug.LogWarningFormat("ERROR : =========== AB_Init => {0}AB_info.txt = {1}", new object[]
			{
				AssetBundleMng.Instance().GetAB_ROOT_PATH(),
				errorText
			});
			this.OpenAlert(new Action(this.LoadABInfo_FromWWW));
		}
	}

	private IEnumerator LoadABData_FromWWW()
	{
		string strROOT = AssetBundleMng.Instance().GetAB_ROOT_PATH();
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
		return ConstValue.APP_ASSET_DOMAIN + "/asset/img/" + CountrySetting.GetCountryPrefix(CountrySetting.CountryCode.EN);
	}

	private class FindABInfoResult
	{
		public AssetBundleInfoData abInfoData;

		public string objName;
	}
}
