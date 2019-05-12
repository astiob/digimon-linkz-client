using Master;
using Monster;
using Quest;
using System;
using System.Collections;
using Title;
using TS;
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

	[SerializeField]
	private UITexture titleLogo;

	[SerializeField]
	private UITexture rightSignage;

	private CloudBackup backup = new CloudBackup();

	protected override void Start()
	{
		this.userID.text = string.Empty;
		this.userCode.text = string.Empty;
		this.appVersion.text = string.Empty;
		this.buildNumLabel.text = string.Empty;
		this.NpVersion.text = string.Empty;
		this.userID.gameObject.SetActive(false);
		this.userCode.gameObject.SetActive(false);
		this.appVersion.gameObject.SetActive(false);
		this.buildNumLabel.gameObject.SetActive(false);
		this.NpVersion.gameObject.SetActive(false);
		this.cacheClearButtonLabel.text = StringMaster.GetString("TitleCacheButton");
		this.takeoverButtonLabel.text = StringMaster.GetString("TakeOverTitle");
		this.optionButtonLabel.text = StringMaster.GetString("TitleOptionButton");
		this.inquiryButtonLabel.text = StringMaster.GetString("InquiryTitle");
		this.titleLogo.mainTexture = (Resources.Load(string.Format("UITexture/InternationalTitleLogo/digimon_linkz_logo_{0}", CountrySetting.GetCountryPrefix(CountrySetting.CountryCode.EN))) as Texture2D);
		this.rightSignage.mainTexture = (Resources.Load("UITexture/InternationalTitleLogo/RightSignage_en") as Texture2D);
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
		ClassSingleton<MonsterUserDataMng>.Instance.Initialize();
		GUIMonsterIcon.InitMonsterGO(Singleton<GUIMain>.Instance.transform);
		ClassSingleton<GUIMonsterIconList>.Instance.Initialize();
		yield return base.StartCoroutine(AssetBundleMng.Instance().WaitCacheReady());
		yield return base.StartCoroutine(StoreInit.Instance().InitStore());
		this.googlePlay.Bootup();
		yield return base.StartCoroutine(this.AuthLogin());
		yield return base.StartCoroutine(APIUtil.Instance().StartGameLogin());
		string responseContactCode = PlayerPrefs.GetString("InquiryCode", string.Empty);
		if (string.IsNullOrEmpty(responseContactCode))
		{
			GameWebAPI.RequestCM_InquiryCodeRequest requestCM_InquiryCodeRequest = new GameWebAPI.RequestCM_InquiryCodeRequest();
			requestCM_InquiryCodeRequest.OnReceived = delegate(GameWebAPI.InquiryCodeRequest response)
			{
				PlayerPrefs.SetString("InquiryCode", response.inquiryCode);
			};
			GameWebAPI.RequestCM_InquiryCodeRequest request = requestCM_InquiryCodeRequest;
			yield return base.StartCoroutine(request.Run(null, null, null));
		}
		yield return base.StartCoroutine(this.InitAssetBundleDownloadInfo());
		yield return base.StartCoroutine(this.DownloadFontAsset());
		UILabel.defaultFont = (AssetDataMng.Instance().LoadObject("Font/DigimonTitle", null, true) as Font);
		this.userID.gameObject.SetActive(true);
		this.userCode.gameObject.SetActive(true);
		this.appVersion.gameObject.SetActive(true);
		this.buildNumLabel.gameObject.SetActive(true);
		this.NpVersion.gameObject.SetActive(true);
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
		if (string.IsNullOrEmpty(PlayerPrefs.GetString("PlayerCountryCode")))
		{
			GUIMain.ShowCommonDialog(null, "CMD_MultiLangSetting", null);
		}
		if ("0" != tutorialStatus.endFlg)
		{
			CMD_BaseSelect.LoadSetting();
			CMD_ChipSortModal.LoadSetting();
		}
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
		GUIMain.ShowCommonDialog(null, "CMD_TakeoverInput", null);
	}

	public void OnClickedCacheClear()
	{
		this.googlePlay.EnableMenu(false);
		CMD_CacheClear cmd_CacheClear = GUIMain.ShowCommonDialog(null, "CMD_Cache", null) as CMD_CacheClear;
		cmd_CacheClear.onSuccessCacheClear = new Action(this.OnClickedScreen);
	}

	public void OnClickedOption()
	{
		this.googlePlay.EnableMenu(false);
		GUIMain.ShowCommonDialog(null, "CMD_Option", null);
	}

	public void OnClickedContact()
	{
		this.googlePlay.EnableMenu(false);
		GUIMain.ShowCommonDialog(null, "CMD_inquiry", null);
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
			string @string = StringMaster.GetString("PenaltyTitle");
			string message = DataMng.Instance().RespDataCM_Login.penaltyUserInfo.penalty.message;
			AlertManager.ShowAlertDialog(delegate(int x)
			{
				this.CancelGameStart();
			}, @string, message, AlertManager.ButtonActionType.Close, false);
			yield break;
		}
		APIUtil.Instance().alertOnlyCloseButton = true;
		yield return base.StartCoroutine(StoreInit.Instance().InitRestoreOperation());
		if (!Loading.IsShow())
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		}
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
		RestrictionInput.EndLoad();
		ConfirmGDPR gdpr = new ConfirmGDPR();
		yield return base.StartCoroutine(gdpr.Ready());
		if (gdpr.IsUpdateRule())
		{
			bool isCancel = false;
			yield return base.StartCoroutine(gdpr.ShowConfirm(delegate(bool result)
			{
				isCancel = result;
			}));
			if (isCancel)
			{
				this.CancelGameStart();
				yield break;
			}
		}
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		UpdateMasterData updateMasterData = new UpdateMasterData();
		yield return base.StartCoroutine(updateMasterData.UpdateData());
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

	private IEnumerator DownloadFontAsset()
	{
		bool result = AssetDataMng.Instance().AB_StartDownLoad("FONT", 4);
		if (result)
		{
			while (AssetDataMng.Instance().IsAssetBundleDownloading())
			{
				yield return null;
			}
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
			ScriptUtil.ShowCommonDialog(new Action<int>(this.AppQuit), "BackKeyConfirmTitle", "BackKeyConfirmExit", "SEInternal/Common/se_106");
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
