using Evolution;
using Master;
using Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using Title;
using UnityEngine;

public sealed class GUIScreenTitle : GUIScreen
{
	[SerializeField]
	private UILabel userID;

	[SerializeField]
	private UILabel userCode;

	[SerializeField]
	private UILabel appVersion;

	[SerializeField]
	private UILabel buildNumLabel;

	[SerializeField]
	private UILabel NpVersion;

	[SerializeField]
	private GameObject tapToStart;

	[SerializeField]
	private UISprite cacheClearButtonSprite;

	[SerializeField]
	private UILabel cacheClearButtonLabel;

	[SerializeField]
	private BoxCollider cacheClearButtonCollider;

	[SerializeField]
	private UILabel takeoverButtonLabel;

	[SerializeField]
	private UILabel optionButtonLabel;

	[SerializeField]
	private UILabel inquiryButtonLabel;

	[SerializeField]
	private GooglePlayGamesObjects googlePlay;

	private CloudBackup backup = new CloudBackup();

	protected override void Start()
	{
		this.userID.text = string.Empty;
		this.userCode.text = string.Empty;
		this.appVersion.text = string.Empty;
		this.buildNumLabel.text = string.Empty;
		this.NpVersion.text = string.Empty;
		this.cacheClearButtonLabel.text = StringMaster.GetString("TitleCacheButton");
		this.takeoverButtonLabel.text = StringMaster.GetString("TakeOverTitle");
		this.optionButtonLabel.text = StringMaster.GetString("TitleOptionButton");
		this.inquiryButtonLabel.text = StringMaster.GetString("InquiryTitle");
		base.Start();
		base.gameObject.SetActive(true);
		if (!global::Debug.isDebugBuild || this.buildNumLabel != null)
		{
		}
		base.StartCoroutine(this.StartEvent());
	}

	private IEnumerator StartEvent()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
		DataMng.Instance().RespDataUS_MonsterList = null;
		yield return base.StartCoroutine(AssetBundleMng.Instance().WaitCacheReady());
		yield return base.StartCoroutine(StoreInit.Instance().InitStore());
		this.googlePlay.Bootup();
		yield return base.StartCoroutine(this.AuthLogin());
		yield return base.StartCoroutine(APIUtil.Instance().StartGameLogin());
		this.userCode.text = string.Format(StringMaster.GetString("TitleUserCode"), DataMng.Instance().RespDataCM_Login.playerInfo.userCode);
		this.appVersion.text = string.Format(StringMaster.GetString("TitleAppVersion"), VersionManager.version);
		GameWebAPI.RespDataCM_Login.TutorialStatus tutorialStatus = DataMng.Instance().RespDataCM_Login.tutorialStatus;
		if ("0" == tutorialStatus.endFlg && "0" == tutorialStatus.statusId)
		{
			this.cacheClearButtonCollider.enabled = false;
			this.cacheClearButtonLabel.color = Color.gray;
			this.cacheClearButtonLabel.effectColor = Color.gray;
			this.cacheClearButtonSprite.spriteName = "Common02_Btn_BaseG";
		}
		yield return this.backup.CheckClearMissionForGoogle();
		if (GUICollider.IsAllColliderDisable())
		{
			GUICollider.EnableAllCollider(string.Empty);
		}
		RestrictionInput.EndLoad();
		yield break;
	}

	private IEnumerator AuthLogin()
	{
		bool completed = false;
		while (!completed)
		{
			string authResult = null;
			Action<string> onCompleted = delegate(string result)
			{
				authResult = result;
			};
			base.StartCoroutine(APIUtil.Instance().OAuthLogin(onCompleted));
			while (string.IsNullOrEmpty(authResult))
			{
				yield return null;
			}
			if ("Success" == authResult)
			{
				completed = true;
			}
		}
		yield break;
	}

	public void OnClickedScreen()
	{
		if (null == DataMng.Instance() || DataMng.Instance().RespDataCM_Login == null)
		{
			return;
		}
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		if (this.tapToStart != null)
		{
			this.tapToStart.SetActive(false);
		}
		if (this.googlePlay != null)
		{
			this.googlePlay.EnableMenu(false);
		}
		base.StartCoroutine(this.GameStart());
	}

	public void OnClickedTakeover()
	{
		this.googlePlay.EnableMenu(false);
		CMD_Takeover.currentMode = CMD_Takeover.MODE.INPUT;
		GUIMain.ShowCommonDialog(null, "CMD_TakeoverInput");
	}

	public void OnClickedCacheClear()
	{
		this.googlePlay.EnableMenu(false);
		CMD_CacheClear cmd_CacheClear = GUIMain.ShowCommonDialog(null, "CMD_Cache") as CMD_CacheClear;
		cmd_CacheClear.onSuccessCacheClear = new Action(this.OnClickedScreen);
	}

	public void OnClickedOption()
	{
		this.googlePlay.EnableMenu(false);
		GUIMain.ShowCommonDialog(null, "CMD_Option");
	}

	public void OnClickedContact()
	{
		this.googlePlay.EnableMenu(false);
		GUIMain.ShowCommonDialog(null, "CMD_inquiry");
	}

	private void CancelGameStart()
	{
		this.tapToStart.SetActive(true);
		RestrictionInput.EndLoad();
	}

	private IEnumerator GameStart()
	{
		ClassSingleton<QuestData>.Instance.ClearDNGDataCache();
		if (DataMng.Instance().RespDataCM_Login.penaltyUserInfo != null && DataMng.Instance().RespDataCM_Login.penaltyUserInfo.penaltyLevel == "2")
		{
			RestrictionInput.SuspensionLoad();
			string title = StringMaster.GetString("PenaltyTitle");
			string message = DataMng.Instance().RespDataCM_Login.penaltyUserInfo.penalty.message;
			AlertManager.ShowAlertDialog(delegate(int x)
			{
				this.CancelGameStart();
			}, title, message, AlertManager.ButtonActionType.Close, false);
			yield break;
		}
		APIUtil.Instance().alertOnlyCloseButton = true;
		yield return base.StartCoroutine(StoreInit.Instance().InitRestoreOperation());
		APIUtil.Instance().alertOnlyCloseButton = false;
		AgreementConsent agreementConsent = new AgreementConsent();
		bool isAgreement = false;
		yield return base.StartCoroutine(agreementConsent.CheckAgreement(delegate(bool result)
		{
			isAgreement = result;
		}));
		if (!isAgreement)
		{
			this.CancelGameStart();
			yield break;
		}
		yield return base.StartCoroutine(this.UpdateMasterData());
		yield return base.StartCoroutine(this.InitAssetBundleDownloadInfo());
		bool tutorialStart = this.CheckFirstTutorial();
		if (tutorialStart)
		{
			yield break;
		}
		bool isUpdate = this.UpdateAssetBundle();
		if (isUpdate)
		{
			yield break;
		}
		ScreenController.ChangeHomeScreen(CMD_Tips.DISPLAY_PLACE.TitleToFarm);
		yield break;
	}

	private IEnumerator UpdateMasterData()
	{
		List<MasterId> updateInfoList = new List<MasterId>();
		GameObject go = UnityEngine.Object.Instantiate<GameObject>(AssetDataMng.Instance().LoadObject("UIPrefab/MasterDataLoadingGauge", null, true) as GameObject);
		MasterDataLoadingGauge topDownload = go.GetComponent<MasterDataLoadingGauge>();
		MasterDataMng.Instance().ClearCache();
		MasterDataMng.Instance().InitialFileIO();
		base.StartCoroutine(MasterDataMng.Instance().ReadMasterData(updateInfoList, new Action<int, int>(topDownload.SetLoadProgress)));
		yield return base.StartCoroutine(topDownload.WaitMasterDataLoad());
		topDownload = null;
		UnityEngine.Object.Destroy(go);
		go = null;
		yield return base.StartCoroutine(MasterDataMng.Instance().GetMasterDataUpdateInfo(updateInfoList));
		if (0 < updateInfoList.Count)
		{
			Loading.Invisible();
			base.StartCoroutine(MasterDataMng.Instance().UpdateLocalMasterData(updateInfoList));
			CMD_ShortDownload shortDownload = GUIMain.ShowCommonDialog(null, "CMD_ShortDownload") as CMD_ShortDownload;
			yield return base.StartCoroutine(shortDownload.WaitMasterDataDownload());
			shortDownload.ClosePanel(true);
			Loading.ResumeDisplay();
		}
		MasterDataMng.Instance().ReleaseFileIO();
		GameWebAPI.Instance().SetMasterDataVersion();
		ClassSingleton<EvolutionData>.Instance.Initialize();
		yield break;
	}

	private IEnumerator InitAssetBundleDownloadInfo()
	{
		APIRequestTask task = DataMng.Instance().RequestABVersion(true);
		yield return base.StartCoroutine(task.Run(null, null, null));
		bool isFinished = false;
		AssetDataMng.Instance().AB_Init(delegate(int categoryCount)
		{
			isFinished = true;
		});
		while (!isFinished)
		{
			yield return null;
		}
		yield break;
	}

	private bool CheckFirstTutorial()
	{
		TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
		if (null != tutorialObserver)
		{
			UnityEngine.Object.Destroy(tutorialObserver);
		}
		GameObject original = AssetDataMng.Instance().LoadObject("Tutorial/TutorialObserver", null, true) as GameObject;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(original);
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		tutorialObserver = gameObject.GetComponent<TutorialObserver>();
		Resources.UnloadUnusedAssets();
		bool flag = "0" == DataMng.Instance().RespDataCM_Login.tutorialStatus.endFlg;
		if (flag)
		{
			tutorialObserver.StartFirstTutorial(DataMng.Instance().RespDataCM_Login.tutorialStatus);
		}
		return flag;
	}

	private bool UpdateAssetBundle()
	{
		if (AssetDataMng.Instance().IsInitializedAssetBundle() && 0 < AssetDataMng.Instance().GetDownloadAssetBundleCount(string.Empty))
		{
			GUIMain.FadeWhiteReqScreen("UIAssetBundleDownLoad", null, 0.8f, 0.8f, true);
			return true;
		}
		return false;
	}

	protected override void Update()
	{
		this.UpdateBackKeyAndroid();
	}

	private void UpdateBackKeyAndroid()
	{
		if (GUIManager.IsEnableBackKeyAndroid() && Input.GetKeyDown(KeyCode.Escape))
		{
			CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.AppQuit), "CMD_Confirm") as CMD_Confirm;
			cmd_Confirm.Title = StringMaster.GetString("BackKeyConfirmTitle");
			cmd_Confirm.Info = StringMaster.GetString("BackKeyConfirmExit");
			SoundMng.Instance().PlaySE("SEInternal/Common/se_106", 0f, false, true, null, -1, 1f);
		}
	}

	private void AppQuit(int idx)
	{
		if (idx == 0)
		{
			Application.Quit();
		}
	}
}
