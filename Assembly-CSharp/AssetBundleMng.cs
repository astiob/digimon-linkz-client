using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

public class AssetBundleMng : MonoBehaviour
{
	private static AssetBundleMng instance;

	private Dictionary<string, AssetBundle> loadedAssetBundleDic = new Dictionary<string, AssetBundle>();

	private bool isWaitDiskSpaceCheck;

	private int countDownloadProcess;

	private long downloadFileSize;

	private string applicationVersion;

	private bool isStopDownload;

	private int waitDownLoadCT;

	private int realABDL_NowCount_LV;

	private string level;

	private Dictionary<string, AB_DownLoad_ALLInfo> DLAllList;

	private AB_DL_RecordC abdlRecordC;

	private string STR_VERSION_ALL_RECORD = "VER_0_1";

	private int openDLStreamNum;

	private List<AB_DownLoadInfo> abdlI_ODLStreamList;

	private void Awake()
	{
		Cache defaultCache = Caching.defaultCache;
		defaultCache.maximumAvailableStorageSpace = 4294967296L;
		defaultCache.expirationDelay = 12960000;
		AssetBundleMng.instance = this;
		this.LoadVersionInfo();
	}

	private void Update()
	{
		this.UpdateDLAll_Progress();
	}

	private void OnDestroy()
	{
		AssetBundleMng.instance = null;
	}

	public static AssetBundleMng Instance()
	{
		return AssetBundleMng.instance;
	}

	private void LoadVersionInfo()
	{
		string version = string.Empty;
		VersionManager.Load(delegate(bool isSuccess, string str)
		{
			if (isSuccess)
			{
				version = str;
			}
			else
			{
				version = "1.0.0";
			}
			this.applicationVersion = version + "/";
		});
	}

	public string GetAppVer()
	{
		return VersionManager.version;
	}

	public string GetAssetBundleRootPath()
	{
		string str = ConstValue.APP_ASSET_DOMAIN + "/asset/AB_DATA/ANDROID/";
		return str + this.applicationVersion;
	}

	public AB_DownLoadInfo DownLoad_OneAssetBundleData(string catName, int ver, string abPath, AssetBundleInfo abInfo, Action<AssetBundle, AB_DownLoadInfo> actEnd = null, bool forceDL = false)
	{
		AB_DownLoadInfo abdlI = new AB_DownLoadInfo
		{
			ver = ver,
			abPath = abPath,
			abInfo = abInfo,
			actEndCallBack = actEnd,
			progress = 0f
		};
		string text = string.Empty;
		string strROOT = string.Empty;
		strROOT = this.GetAssetBundleRootPath();
		text = strROOT + abdlI.abPath + abdlI.abInfo.abName;
		text = text + ".unity3d?" + AssetDataMng.assetVersion;
		uint crc = abdlI.abInfo.crc;
		uint recordCRC = this.GetRecordCRC(abdlI.abPath, abdlI.abInfo.abName);
		int recordVersion = this.GetRecordVersion(abdlI.abPath, abdlI.abInfo.abName);
		if (abdlI.actEndCallBack != null)
		{
			Action action;
			if (forceDL && recordVersion != -1)
			{
				abdlI.ver = recordVersion;
				action = delegate()
				{
					this.StartCoroutine(this.WaitResponse_DownLoad(abdlI, strROOT, forceDL));
				};
			}
			else
			{
				forceDL = false;
				if (this.level != string.Empty && this.level != abdlI.abInfo.level.Trim())
				{
					action = delegate()
					{
						abdlI.actEndCallBack(null, abdlI);
					};
				}
				else if (recordVersion == -1)
				{
					abdlI.ver = 1;
					action = delegate()
					{
						this.StartCoroutine(this.WaitResponse_DownLoad(abdlI, strROOT, false));
					};
				}
				else if (recordCRC != crc)
				{
					abdlI.ver = recordVersion;
					action = delegate()
					{
						this.StartCoroutine(this.WaitResponse_DownLoad(abdlI, strROOT, false));
					};
				}
				else
				{
					Hash128 hash = new Hash128(0u, 0u, 0u, (uint)recordVersion);
					bool flag = Caching.IsVersionCached(text, hash);
					if (flag)
					{
						Caching.MarkAsUsed(text, hash);
						action = delegate()
						{
							abdlI.actEndCallBack(null, abdlI);
						};
					}
					else
					{
						abdlI.ver = recordVersion;
						action = delegate()
						{
							this.StartCoroutine(this.WaitResponse_DownLoad(abdlI, strROOT, false));
						};
					}
				}
			}
			this.waitDownLoadCT = 1;
			base.StartCoroutine(this.DownLoad(action));
			return abdlI;
		}
		Hash128 hash2 = new Hash128(0u, 0u, 0u, 1u);
		bool flag2 = Caching.IsVersionCached(text, hash2);
		if (flag2)
		{
			abdlI.www = WWW.LoadFromCacheOrDownload(text, hash2, 0u);
			while (!abdlI.www.isDone)
			{
				Thread.Sleep(1);
			}
			return abdlI;
		}
		abdlI.ver = recordVersion;
		forceDL = false;
		base.StartCoroutine(this.ReloadFromWWW(abdlI, strROOT));
		string text2 = text.Substring(text.IndexOf("AB_DATA/"));
		string text3 = text2;
		text2 = string.Concat(new string[]
		{
			text3,
			"\n存在するはずのファイルがキャッシュに存在しません!\n CRC = ",
			recordCRC.ToString(),
			",  VER = ",
			recordVersion.ToString()
		});
		NativeMessageDialog.Show(text2);
		return abdlI;
	}

	private IEnumerator DownLoad(Action action)
	{
		if (this.waitDownLoadCT > 0)
		{
			this.waitDownLoadCT = 0;
			yield return null;
		}
		while (this.IsStopDownload())
		{
			yield return null;
		}
		action();
		action = null;
		yield break;
	}

	private IEnumerator WaitResponse_DownLoad(AB_DownLoadInfo abdlI, string AB_ROOT_PATH, bool forceDL = false)
	{
		string allPath = AB_ROOT_PATH + abdlI.abPath + abdlI.abInfo.abName;
		uint crc = abdlI.abInfo.crc;
		allPath = allPath + ".unity3d?" + AssetDataMng.assetVersion;
		while (this.isWaitDiskSpaceCheck)
		{
			yield return null;
		}
		this.isWaitDiskSpaceCheck = true;
		yield return base.StartCoroutine(AssetBundleDiskSpaceCheck.CheckDiskSpace(abdlI.abInfo, this));
		this.isWaitDiskSpaceCheck = false;
		this.countDownloadProcess++;
		long assetbundleFileSize = long.Parse(abdlI.abInfo.size);
		this.downloadFileSize += AssetBundleDiskSpaceCheck.GetFileSize(assetbundleFileSize);
		while (!Caching.ready)
		{
			yield return null;
		}
		WWWHelper wwwHelper = WWWHelper.LoadFromCacheOrDownload(allPath, abdlI.ver, crc, 60f);
		bool startDownload = true;
		while (startDownload)
		{
			yield return base.StartCoroutine(wwwHelper.StartDownloadAssetBundle(delegate(WWW www)
			{
				if (abdlI.www != null)
				{
					abdlI.www.Dispose();
				}
				abdlI.www = www;
			}, delegate(WWW resultWWW)
			{
				if (resultWWW == null)
				{
					if (!forceDL)
					{
						this.OpenAlert();
					}
				}
				else if (!string.IsNullOrEmpty(resultWWW.error))
				{
					if (!forceDL)
					{
						this.OpenAlert();
					}
					abdlI.www = resultWWW;
				}
				else
				{
					abdlI.www = resultWWW;
					startDownload = false;
				}
			}));
			while (this.IsStopDownload())
			{
				yield return null;
			}
		}
		this.countDownloadProcess--;
		this.downloadFileSize -= AssetBundleDiskSpaceCheck.GetFileSize(assetbundleFileSize);
		if (!forceDL)
		{
			this.Add_OR_SetRecord(abdlI.abPath, abdlI.abInfo.abName, crc, abdlI.ver);
		}
		if (abdlI.actEndCallBack != null)
		{
			abdlI.actEndCallBack(abdlI.www.assetBundle, abdlI);
		}
		yield break;
	}

	public int GetCountDownloadProcess()
	{
		return this.countDownloadProcess;
	}

	public long GetDownloadFileSize()
	{
		return this.downloadFileSize;
	}

	private IEnumerator ReloadFromWWW(AB_DownLoadInfo abdlI, string AB_ROOT_PATH)
	{
		yield return base.StartCoroutine(this.WaitResponse_DownLoad(abdlI, AB_ROOT_PATH, true));
		yield break;
	}

	private void OpenAlert()
	{
		if (!this.IsStopDownload())
		{
			this.StopDownload();
			AlertManager.ShowAlertDialog(delegate(int i)
			{
				this.RestartDownload();
			}, "LOCAL_ERROR_ASSET_DATA");
		}
	}

	public IEnumerator WaitCacheReady()
	{
		while (!Caching.ready)
		{
			yield return null;
		}
		yield break;
	}

	public void InitAll()
	{
		this.isWaitDiskSpaceCheck = false;
		this.isStopDownload = false;
		this.waitDownLoadCT = 0;
		this.realABDL_NowCount_LV = 0;
		this.level = string.Empty;
		this.DLAllList = new Dictionary<string, AB_DownLoad_ALLInfo>();
		this.ClearAllRecord();
		this.InitODLStream();
		base.StopAllCoroutines();
	}

	public int RealABDL_NowCount_LV()
	{
		return this.realABDL_NowCount_LV;
	}

	public void AddRealABDL_NowCount_LV(int add)
	{
		this.realABDL_NowCount_LV += add;
	}

	public void SetLevel(string lev)
	{
		this.level = lev;
	}

	public bool InitDownLoad_All(string _level)
	{
		this.level = _level;
		this.realABDL_NowCount_LV = 0;
		bool result = this.ReadAllRecord();
		this.DLAllList = new Dictionary<string, AB_DownLoad_ALLInfo>();
		return result;
	}

	public AB_DownLoad_ALLInfo GetAB_DownLoad_ALLInfo(string name)
	{
		if (!this.DLAllList.ContainsKey(name))
		{
			global::Debug.LogError("===================================== AssetBundleMng:GetAB_DownLoad_ALLInfo = " + name + "登録されてない！");
			return null;
		}
		return this.DLAllList[name];
	}

	public int GetDLAllCount(AssetBundleInfoData abid)
	{
		int num = 0;
		List<AssetBundleInfo> assetBundleInfoList = abid.assetBundleInfoList;
		for (int i = 0; i < assetBundleInfoList.Count; i++)
		{
			AssetBundleInfo assetBundleInfo = assetBundleInfoList[i];
			uint crc = assetBundleInfo.crc;
			uint recordCRC = this.GetRecordCRC(abid.abPath, assetBundleInfo.abName);
			int recordVersion = this.GetRecordVersion(abid.abPath, assetBundleInfo.abName);
			if (!(this.level != string.Empty) || !(this.level != assetBundleInfo.level.Trim()))
			{
				if (recordVersion == -1)
				{
					num++;
				}
				else if (recordCRC != crc)
				{
					num++;
				}
				else
				{
					string url = string.Concat(new object[]
					{
						this.GetAssetBundleRootPath(),
						abid.abPath,
						assetBundleInfo.abName,
						".unity3d?",
						AssetDataMng.assetVersion
					});
					Hash128 hash = new Hash128(0u, 0u, 0u, (uint)recordVersion);
					if (!Caching.IsVersionCached(url, hash))
					{
						num++;
					}
					else
					{
						Caching.MarkAsUsed(url, hash);
					}
				}
			}
		}
		return num;
	}

	public void DownLoad_AllAssetBundleData(string name, AssetBundleInfoData abid, Action<int> actAllEnd)
	{
		if (this.DLAllList.ContainsKey(name))
		{
			global::Debug.LogError("===================================== AssetBundleMng:DownLoad_AllAssetBundleData = " + name + "既にあります！");
		}
		else
		{
			AB_DownLoad_ALLInfo ab_DownLoad_ALLInfo = new AB_DownLoad_ALLInfo();
			ab_DownLoad_ALLInfo.ABM_Instance = this;
			ab_DownLoad_ALLInfo.abid = abid;
			ab_DownLoad_ALLInfo.actEndCallBackAll = actAllEnd;
			ab_DownLoad_ALLInfo.execFCT = 0;
			ab_DownLoad_ALLInfo.progressFCT = 0;
			ab_DownLoad_ALLInfo.progressAll = 0f;
			ab_DownLoad_ALLInfo.dlFCT = this.GetDLAllCount(abid);
			ab_DownLoad_ALLInfo.dlFCT_COMP = 0;
			ab_DownLoad_ALLInfo.bIsAllEnd = false;
			ab_DownLoad_ALLInfo.cur_abdlI = this.DownLoad_OneAssetBundleData(ab_DownLoad_ALLInfo.abid.name, ab_DownLoad_ALLInfo.abid.ver, ab_DownLoad_ALLInfo.abid.abPath, ab_DownLoad_ALLInfo.abid.assetBundleInfoList[ab_DownLoad_ALLInfo.execFCT++], new Action<AssetBundle, AB_DownLoadInfo>(ab_DownLoad_ALLInfo.EndCallBackOne), false);
			this.DLAllList.Add(name, ab_DownLoad_ALLInfo);
		}
	}

	public bool IsStopDownload()
	{
		return this.isStopDownload;
	}

	public void StopDownload()
	{
		this.isStopDownload = true;
	}

	public void RestartDownload()
	{
		this.isStopDownload = false;
	}

	private void UpdateDLAll_Progress()
	{
		if (this.DLAllList != null)
		{
			foreach (string key in this.DLAllList.Keys)
			{
				AB_DownLoad_ALLInfo ab_DownLoad_ALLInfo = this.DLAllList[key];
				if (!ab_DownLoad_ALLInfo.bIsAllEnd)
				{
					float num = 0f;
					if (ab_DownLoad_ALLInfo.cur_abdlI.www != null)
					{
						num = ab_DownLoad_ALLInfo.cur_abdlI.www.progress;
					}
					ab_DownLoad_ALLInfo.cur_abdlI.progress = num;
					float num2 = (float)ab_DownLoad_ALLInfo.dlFCT;
					float num3 = (float)ab_DownLoad_ALLInfo.dlFCT_COMP;
					if (num2 < 1f)
					{
						num2 = 1f;
					}
					float num4 = 1f / num2;
					float progressAll = num3 / num2 + num4 * num;
					ab_DownLoad_ALLInfo.progressAll = progressAll;
				}
				else
				{
					ab_DownLoad_ALLInfo.progressAll = 1f;
				}
			}
		}
	}

	public bool LoadObjectASync(AssetBundleInfoData abid, string resourceName, Action<UnityEngine.Object> actEnd)
	{
		AssetBundleMng.<LoadObjectASync>c__AnonStorey7 <LoadObjectASync>c__AnonStorey = new AssetBundleMng.<LoadObjectASync>c__AnonStorey7();
		<LoadObjectASync>c__AnonStorey.resourceName = resourceName;
		<LoadObjectASync>c__AnonStorey.actEnd = actEnd;
		<LoadObjectASync>c__AnonStorey.$this = this;
		<LoadObjectASync>c__AnonStorey.abiList = abid.assetBundleInfoList;
		bool flag = false;
		int m;
		for (m = 0; m < <LoadObjectASync>c__AnonStorey.abiList.Count; m++)
		{
			for (int i = 0; i < <LoadObjectASync>c__AnonStorey.abiList[m].objNameList.Count; i++)
			{
				if (<LoadObjectASync>c__AnonStorey.resourceName == <LoadObjectASync>c__AnonStorey.abiList[m].objNameList[i])
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				this.DownLoad_OneAssetBundleData(abid.name, abid.ver, abid.abPath, <LoadObjectASync>c__AnonStorey.abiList[m], delegate(AssetBundle ab, AB_DownLoadInfo abdlI)
				{
					UnityEngine.Object obj = <LoadObjectASync>c__AnonStorey.$this.FindObjectAndUnloadAB(abdlI, <LoadObjectASync>c__AnonStorey.abiList[m], <LoadObjectASync>c__AnonStorey.resourceName);
					if (<LoadObjectASync>c__AnonStorey.actEnd != null)
					{
						<LoadObjectASync>c__AnonStorey.actEnd(obj);
					}
				}, true);
				break;
			}
		}
		return flag;
	}

	public UnityEngine.Object LoadObject(AssetBundleInfoData abid, string resourceName)
	{
		UnityEngine.Object result = null;
		List<AssetBundleInfo> assetBundleInfoList = abid.assetBundleInfoList;
		for (int i = 0; i < assetBundleInfoList.Count; i++)
		{
			bool flag = false;
			for (int j = 0; j < assetBundleInfoList[i].objNameList.Count; j++)
			{
				if (resourceName == assetBundleInfoList[i].objNameList[j])
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				AB_DownLoadInfo abdlI = this.DownLoad_OneAssetBundleData(abid.name, abid.ver, abid.abPath, assetBundleInfoList[i], null, false);
				result = this.FindObjectAndUnloadAB(abdlI, assetBundleInfoList[i], resourceName);
				break;
			}
		}
		return result;
	}

	private UnityEngine.Object FindObjectAndUnloadAB(AB_DownLoadInfo abdlI, AssetBundleInfo abi, string objName)
	{
		Type typeFromHandle = typeof(Texture2D);
		switch (abdlI.abInfo.type)
		{
		case 0:
			typeFromHandle = typeof(GameObject);
			break;
		case 1:
			typeFromHandle = typeof(Texture2D);
			break;
		case 2:
			typeFromHandle = typeof(TextAsset);
			break;
		case 3:
			typeFromHandle = typeof(AudioClip);
			break;
		case 4:
			typeFromHandle = typeof(ScriptableObject);
			break;
		case 5:
			typeFromHandle = typeof(RuntimeAnimatorController);
			break;
		case 6:
			typeFromHandle = typeof(RuntimeAnimatorController);
			break;
		case 7:
			typeFromHandle = typeof(TextAsset);
			break;
		case 8:
			typeFromHandle = typeof(Font);
			break;
		}
		if (this.loadedAssetBundleDic.ContainsKey(abi.abName))
		{
			this.loadedAssetBundleDic[abi.abName].Unload(false);
		}
		UnityEngine.Object @object = abdlI.www.assetBundle.LoadAsset(objName, typeFromHandle);
		if (@object == null && objName.IndexOf("/") != -1)
		{
			string[] array = objName.Split(new char[]
			{
				'/'
			});
			string text = array[array.Length - 1];
			if (abi.realCT == 1)
			{
				@object = abdlI.www.assetBundle.LoadAsset(text, typeFromHandle);
			}
			else
			{
				List<string> objNameList = abi.objNameList;
				int num = 0;
				int i;
				for (i = 0; i < objNameList.Count; i++)
				{
					if (objNameList[i].EndsWith(text))
					{
						if (objNameList[i] == objName)
						{
							break;
						}
						num++;
					}
				}
				UnityEngine.Object[] array2 = abdlI.www.assetBundle.LoadAllAssets(typeFromHandle);
				for (i = 0; i < array2.Length; i++)
				{
					if (array2[i].name == text)
					{
						if (num == 0)
						{
							break;
						}
						num--;
					}
				}
				if (i == array2.Length)
				{
					for (i = 0; i < array2.Length; i++)
					{
						if (array2[i].name == objName)
						{
							break;
						}
					}
					if (array2.Length <= i)
					{
						global::Debug.LogError("===================================== AssetBundleMng:LoadObject NOT FOUND_2!!");
						global::Debug.LogError("===================================== AssetBundleMng:LoadObject objName = " + objName);
					}
				}
				@object = array2[i];
			}
		}
		if (typeFromHandle != typeof(AudioClip))
		{
			abdlI.www.assetBundle.Unload(false);
		}
		else
		{
			this.loadedAssetBundleDic.AddOrReplace(abi.abName, abdlI.www.assetBundle);
		}
		if (abdlI.www.error != null)
		{
			CMD_Alert cmd_Alert = GUIMain.ShowCommonDialog(null, "CMD_Alert", null) as CMD_Alert;
			cmd_Alert.Title = "File:" + objName + " AB:" + abi.abName;
			cmd_Alert.Info = TextUtil.GetWinTextSkipColorCode(abdlI.www.error, 18);
			cmd_Alert.SetDisplayButton(CMD_Alert.DisplayButton.CLOSE);
		}
		else if (@object == null)
		{
			CMD_Alert cmd_Alert2 = GUIMain.ShowCommonDialog(null, "CMD_Alert", null) as CMD_Alert;
			cmd_Alert2.Title = "File:" + objName + " AB:" + abi.abName;
			cmd_Alert2.Info = "UE:OBJが NULL!!";
			cmd_Alert2.SetDisplayButton(CMD_Alert.DisplayButton.CLOSE);
		}
		abdlI.www.Dispose();
		abdlI.www = null;
		return @object;
	}

	public void ClearAllRecord()
	{
		this.abdlRecordC = new AB_DL_RecordC();
		this.abdlRecordC.dateTime = DateTime.Now;
		this.abdlRecordC.DLRecordList = new Dictionary<string, AB_DL_RecordVal>();
	}

	public bool ReadAllRecord()
	{
		string path = Application.persistentDataPath + "/ab_all_record_" + this.STR_VERSION_ALL_RECORD + ".txt";
		bool flag = true;
		bool flag2 = true;
		string json = string.Empty;
		try
		{
			json = File.ReadAllText(path, Encoding.UTF8);
		}
		catch
		{
			flag = false;
		}
		if (flag)
		{
			AB_DL_RecordC ab_DL_RecordC = null;
			try
			{
				ab_DL_RecordC = JsonMapper.ToObject<AB_DL_RecordC>(json);
			}
			catch (JsonException ex)
			{
				flag2 = false;
				global::Debug.Log(ex.Message);
			}
			if (flag2)
			{
				this.abdlRecordC = ab_DL_RecordC;
				global::Debug.Log("============================== READ ALL AB RECORD!!");
			}
		}
		else
		{
			this.abdlRecordC = new AB_DL_RecordC();
			this.abdlRecordC.dateTime = DateTime.Now;
			this.abdlRecordC.DLRecordList = new Dictionary<string, AB_DL_RecordVal>();
			try
			{
				this.WriteAllRecord();
			}
			catch
			{
				flag2 = false;
			}
		}
		return flag2;
	}

	public void WriteAllRecord()
	{
		string text = Application.persistentDataPath + "/ab_all_record_" + this.STR_VERSION_ALL_RECORD + ".txt";
		global::Debug.Log("============================== APP DATA DATA_PATH = " + text);
		string contents = JsonMapper.ToJson(this.abdlRecordC);
		string text2 = text;
		string text3 = text2 + "_suffix";
		try
		{
			File.WriteAllText(text3, contents, Encoding.UTF8);
			string json = File.ReadAllText(text3);
			AB_DL_RecordC ab_DL_RecordC = JsonMapper.ToObject<AB_DL_RecordC>(json);
			if (ab_DL_RecordC != null)
			{
				if (File.Exists(text2))
				{
					File.Delete(text2);
				}
				File.Move(text3, text2);
			}
			else if (File.Exists(text3))
			{
				File.Delete(text3);
			}
		}
		catch (JsonException ex)
		{
			if (File.Exists(text3))
			{
				File.Delete(text3);
			}
			if (ex != null)
			{
				global::Debug.LogWarning(ex.Message);
			}
		}
		catch
		{
			global::Debug.LogWarning("ファイルの保存失敗");
		}
		global::Debug.Log("============================== ALL AB RECORD SAVED!!");
	}

	private uint GetRecordCRC(string path, string name)
	{
		Dictionary<string, AB_DL_RecordVal> dlrecordList = this.abdlRecordC.DLRecordList;
		if (!dlrecordList.ContainsKey(path + "_" + name))
		{
			return 0u;
		}
		AB_DL_RecordVal ab_DL_RecordVal = dlrecordList[path + "_" + name];
		return ab_DL_RecordVal.crc;
	}

	private int GetRecordVersion(string path, string name)
	{
		Dictionary<string, AB_DL_RecordVal> dlrecordList = this.abdlRecordC.DLRecordList;
		if (!dlrecordList.ContainsKey(path + "_" + name))
		{
			return -1;
		}
		AB_DL_RecordVal ab_DL_RecordVal = dlrecordList[path + "_" + name];
		return ab_DL_RecordVal.ver;
	}

	private void Add_OR_SetRecord(string path, string name, uint crc, int ver)
	{
		Dictionary<string, AB_DL_RecordVal> dlrecordList = this.abdlRecordC.DLRecordList;
		if (!dlrecordList.ContainsKey(path + "_" + name))
		{
			string key = path + "_" + name;
			dlrecordList.Add(key, new AB_DL_RecordVal
			{
				dateTime = DateTime.Now,
				crc = crc,
				ver = ver
			});
		}
		else
		{
			AB_DL_RecordVal ab_DL_RecordVal = new AB_DL_RecordVal();
			ab_DL_RecordVal.dateTime = DateTime.Now;
			ab_DL_RecordVal.crc = crc;
			ab_DL_RecordVal.ver = ver;
			dlrecordList[path + "_" + name] = ab_DL_RecordVal;
		}
	}

	public void InitODLStream()
	{
		this.openDLStreamNum = 0;
		this.abdlI_ODLStreamList = new List<AB_DownLoadInfo>();
	}

	public void SetODLStreamNum(int num)
	{
		this.openDLStreamNum = num;
	}

	public bool IsODLSExist()
	{
		return this.openDLStreamNum > 0 && this.openDLStreamNum > this.abdlI_ODLStreamList.Count;
	}

	public void AddODLS(AB_DownLoadInfo abdlI)
	{
		string abName = abdlI.abInfo.abName;
		for (int i = 0; i < this.abdlI_ODLStreamList.Count; i++)
		{
			if (this.abdlI_ODLStreamList[i].abInfo.abName == abName)
			{
				global::Debug.LogError("==================================================############ AB PARA ERROR = " + abName);
			}
		}
		this.abdlI_ODLStreamList.Add(abdlI);
	}

	public void DelODLS(AB_DownLoadInfo abdlI)
	{
		for (int i = 0; i < this.abdlI_ODLStreamList.Count; i++)
		{
			if (this.abdlI_ODLStreamList[i] == abdlI)
			{
				this.abdlI_ODLStreamList.RemoveAt(i);
			}
		}
	}

	public List<AB_DownLoadInfo> GetODLStreamList()
	{
		return this.abdlI_ODLStreamList;
	}
}
