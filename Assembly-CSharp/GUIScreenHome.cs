using Master;
using Monster;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIScreenHome : GUIScreen
{
	private PartsMenu partsMenu;

	protected GameObject goFARM_ROOT;

	private bool isStartGuidance;

	public static bool isManualScreenFadeIn;

	public bool isFinishedStartLoading;

	public static Action homeOpenCallback;

	protected GameObject faceUI;

	private List<CampaignFacilityIcon> campaignFacilityIconList;

	public static bool enableBackKeyAndroid = true;

	private static bool isCacheBattled;

	public override void ShowGUI()
	{
		Time.timeScale = 1f;
		GameObject gameObject = GUIManager.LoadCommonGUI("Parts/PartsMenu", base.gameObject);
		gameObject.transform.localPosition = new Vector3(0f, 0f, -300f);
		this.partsMenu = gameObject.GetComponent<PartsMenu>();
		this.CreateHomeUI();
		this.ServerRequest();
	}

	protected virtual void ServerRequest()
	{
		NormalTask normalTask = new NormalTask(APIUtil.Instance().RequestHomeData());
		normalTask.Add(new NormalTask(new Func<IEnumerator>(this.StartEvent)));
		base.StartCoroutine(normalTask.Run(null, null, null));
	}

	protected virtual IEnumerator StartEvent()
	{
		yield return base.StartCoroutine(this.checkAndSyncCountryCode());
		yield return base.StartCoroutine(this.CreateHomeData());
		RestrictionInput.DeleteDisplayObject();
		TipsLoading.Instance.StopTipsLoad(true);
		Loading.Invisible();
		if (!GUIScreenHome.isManualScreenFadeIn)
		{
			yield return base.StartCoroutine(this.StartScreenFadeIn(null));
		}
		this.isFinishedStartLoading = true;
		bool isPenaltyLevelTwo = false;
		yield return base.StartCoroutine(this.PenaltyCheck(delegate
		{
			GUIMain.BackToTOP("UIStartupCaution", 0.8f, 0.8f);
			isPenaltyLevelTwo = true;
		}));
		if (isPenaltyLevelTwo)
		{
			yield break;
		}
		GUIManager.ResetTouchingCount();
		yield return base.StartCoroutine(this.ShowLoginBonusCampaign());
		yield return base.StartCoroutine(this.ShowLoginBonusNormal());
		GameWebAPI.RespDataUS_GetPlayerInfo playerInfo = DataMng.Instance().RespDataUS_PlayerInfo;
		GameWebAPI.RespDataCM_LoginBonus loginBonus = DataMng.Instance().RespDataCM_LoginBonus;
		if (loginBonus != null && loginBonus.loginBonus != null && loginBonus.loginBonus.normal != null && loginBonus.loginBonus.normal.Length > 0 && playerInfo != null && playerInfo.playerInfo != null && playerInfo.playerInfo.loginCount == 3)
		{
			bool isReviewDialogClose = false;
			Action onFinishedAction = delegate()
			{
				isReviewDialogClose = true;
			};
			LeadReview.ShowReviewConfirm(LeadReview.MessageType.TOTAL_LOGIN_COUNT_3DAYS, onFinishedAction, false);
			while (!isReviewDialogClose)
			{
				yield return null;
			}
		}
		yield return base.StartCoroutine(this.CheckRecoverBattle());
		Loading.Display(Loading.LoadingType.LARGE, false);
		while (!AssetDataCacheMng.Instance().IsCacheAllReadyType(AssetDataCacheMng.CACHE_TYPE.CHARA_PARTY))
		{
			yield return null;
		}
		Loading.Invisible();
		TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
		yield return base.StartCoroutine(tutorialObserver.StartGuidance(new Action<bool>(this.StartedGuidance)));
		GUIFace.SetFacilityAlertIcon();
		ClassSingleton<FaceMissionAccessor>.Instance.faceMission.SetParticleMissionIcon();
		yield break;
	}

	private void StartedGuidance(bool isActionGuidance)
	{
		this.isStartGuidance = isActionGuidance;
		this.StartFarm();
	}

	public IEnumerator StartScreenFadeIn(Action finish = null)
	{
		GUIFadeControll.StartFadeIn(0f);
		GameObject fadeObj = GUIManager.LoadCommonGUI("Render2D/SquaresROOT", Singleton<GUIMain>.Instance.gameObject);
		SquaresEffect squaresEffect = fadeObj.GetComponent<SquaresEffect>();
		squaresEffect.Initialize();
		while (fadeObj != null)
		{
			yield return null;
		}
		if (finish != null)
		{
			finish();
		}
		GUIScreenHome.isManualScreenFadeIn = false;
		yield break;
	}

	protected virtual IEnumerator CreateHomeData()
	{
		GUIPlayerStatus.RefreshParams_S(false);
		ClassSingleton<GUIMonsterIconList>.Instance.RefreshList(MonsterDataMng.Instance().GetMonsterDataList());
		ClassSingleton<FaceMissionAccessor>.Instance.faceMission.MissionTapCheck();
		ClassSingleton<FaceMissionAccessor>.Instance.faceMission.SetBadge(false);
		ClassSingleton<FacePresentAccessor>.Instance.facePresent.SetBadgeOnly();
		ClassSingleton<FaceNewsAccessor>.Instance.faceNews.SetBadgeOnly();
		ClassSingleton<PartsMenuFriendIconAccessor>.Instance.partsMenuFriendIcon.FrinedListCheck();
		if (ConstValue.IS_CHAT_OPEN != 1)
		{
			ClassSingleton<FaceChatNotificationAccessor>.Instance.faceChatNotification.gameObject.SetActive(false);
		}
		GUIFace.SetFacilityShopButtonBadge();
		PartsMenu.SetMenuButtonAlertBadge();
		this.DownloadMenuBanner();
		while (!TextureManager.instance.isLoadSaveData)
		{
			yield return null;
		}
		GameWebAPI.RespDataCM_LoginBonus respDataCM_LoginBonus = DataMng.Instance().RespDataCM_LoginBonus;
		if (respDataCM_LoginBonus.loginBonus != null && respDataCM_LoginBonus.loginBonus.campaign != null)
		{
			GameWebAPI.RespDataCM_LoginBonus.LoginBonus[] campaign2 = respDataCM_LoginBonus.loginBonus.campaign;
			for (int i = 0; i < campaign2.Length; i++)
			{
				GameWebAPI.RespDataCM_LoginBonus.LoginBonus campaign = campaign2[i];
				bool isLoadEnd = false;
				string path = CMD_LoginCampaign.GetBgPathForFTP(campaign.backgroundImg);
				TextureManager.instance.Load(path, delegate(Texture2D texture)
				{
					isLoadEnd = true;
				}, 30f, true);
				while (!isLoadEnd)
				{
					yield return null;
				}
			}
		}
		yield return base.StartCoroutine(this.CreateFarm());
		this.StartCacheBattle();
		this.StartCacheParty();
		LeadCapture.Instance.CheckCaptureUpdate();
		this.ShowCampaignFacilityIcon();
		yield break;
	}

	public void ShowCampaignFacilityIcon()
	{
		this.campaignFacilityIconList = new List<CampaignFacilityIcon>();
		CampaignFacilityIcon campaignFacilityIcon = CampaignFacilityIcon.Create(GameWebAPI.RespDataCP_Campaign.CampaignType.MedalTakeOverUp, base.gameObject);
		if (campaignFacilityIcon != null)
		{
			this.campaignFacilityIconList.Add(campaignFacilityIcon);
		}
	}

	public void CloseAllCampaignFacilityIcon()
	{
		if (this.campaignFacilityIconList != null)
		{
			foreach (CampaignFacilityIcon campaignFacilityIcon in this.campaignFacilityIconList)
			{
				campaignFacilityIcon.Close();
			}
			this.campaignFacilityIconList.Clear();
		}
	}

	private void StartFarm()
	{
		ServerDateTime.isUpdateServerDateTime = true;
		FarmRoot instance = FarmRoot.Instance;
		instance.DigimonManager.AppaearanceDigimon(null);
		this.EnableFarmInput();
		List<string> deckMonsterPathList = ClassSingleton<MonsterUserDataMng>.Instance.GetDeckMonsterPathList(false);
		AssetDataCacheMng.Instance().RegisterCacheType(deckMonsterPathList, AssetDataCacheMng.CACHE_TYPE.CHARA_PARTY, false);
		if (ConstValue.IS_CHAT_OPEN == 1)
		{
			ClassSingleton<FaceChatNotificationAccessor>.Instance.faceChatNotification.StartGetHistoryIdList();
		}
		GUIMain.BarrierOFF();
		this.ShowWebWindow();
		if (GUIScreenHome.homeOpenCallback != null)
		{
			GUIScreenHome.homeOpenCallback();
			GUIScreenHome.homeOpenCallback = null;
		}
	}

	protected IEnumerator CreateFarm()
	{
		GameObject go = AssetDataMng.Instance().LoadObject("Farm/Fields/farm_01/FARM_ROOT", null, true) as GameObject;
		yield return null;
		this.goFARM_ROOT = UnityEngine.Object.Instantiate<GameObject>(go);
		go = null;
		Resources.UnloadUnusedAssets();
		yield return null;
		FarmRoot farmRoot = FarmRoot.Instance;
		yield return base.StartCoroutine(farmRoot.Initialize(base.GetComponent<FarmUI>()));
		yield break;
	}

	protected void EnableFarmInput()
	{
		FarmRoot instance = FarmRoot.Instance;
		InputControll input = instance.Input;
		if (null != input)
		{
			input.enabled = true;
		}
	}

	private IEnumerator PenaltyCheck(Action OnPenaltyLevelTwo)
	{
		GameWebAPI.RespDataMP_MyPage mypageData = DataMng.Instance().RespDataMP_MyPage;
		if (mypageData.penaltyUserInfo != null && (mypageData.penaltyUserInfo.penaltyLevel == "1" || mypageData.penaltyUserInfo.penaltyLevel == "2"))
		{
			bool isClose = false;
			Action<int> onClosedAction = delegate(int x)
			{
				if (mypageData.penaltyUserInfo.penaltyLevel == "2")
				{
					OnPenaltyLevelTwo();
				}
				isClose = true;
			};
			string title = StringMaster.GetString("PenaltyTitle");
			string message = mypageData.penaltyUserInfo.penalty.message;
			AlertManager.ShowAlertDialog(onClosedAction, title, message, AlertManager.ButtonActionType.Close, false);
			while (!isClose)
			{
				yield return null;
			}
		}
		yield break;
	}

	private IEnumerator ShowLoginBonusCampaign()
	{
		DataMng.Instance().ShowLoginBonusNumC = 0;
		GameWebAPI.RespDataCM_LoginBonus respDataCM_LoginBonus = DataMng.Instance().RespDataCM_LoginBonus;
		if (respDataCM_LoginBonus.loginBonus == null || respDataCM_LoginBonus.loginBonus.campaign == null)
		{
			yield return null;
		}
		else
		{
			int showNum = respDataCM_LoginBonus.loginBonus.campaign.Length;
			while (showNum > DataMng.Instance().ShowLoginBonusNumC)
			{
				bool isClose = false;
				Action<int> onClosedAction = delegate(int x)
				{
					isClose = true;
					DataMng.Instance().ShowLoginBonusNumC++;
				};
				GameWebAPI.RespDataCM_LoginBonus.LoginBonus loginBonus = respDataCM_LoginBonus.loginBonus.campaign[DataMng.Instance().ShowLoginBonusNumC];
				if (loginBonus.loginBonusId != "2")
				{
					GUIMain.ShowCommonDialog(onClosedAction, "CMD_LoginAnimator", null);
				}
				else
				{
					GUIMain.ShowCommonDialog(onClosedAction, "CMD_CampaignLogin", null);
				}
				while (!isClose)
				{
					yield return null;
				}
			}
		}
		yield break;
	}

	private IEnumerator ShowLoginBonusNormal()
	{
		DataMng.Instance().ShowLoginBonusNumN = 0;
		GameWebAPI.RespDataCM_LoginBonus respDataCM_LoginBonus = DataMng.Instance().RespDataCM_LoginBonus;
		if (respDataCM_LoginBonus.loginBonus == null || respDataCM_LoginBonus.loginBonus.normal == null)
		{
			yield return null;
		}
		else
		{
			int showNum = respDataCM_LoginBonus.loginBonus.normal.Length;
			while (showNum > DataMng.Instance().ShowLoginBonusNumN)
			{
				bool isClose = false;
				Action<int> action = delegate(int x)
				{
					isClose = true;
					DataMng.Instance().ShowLoginBonusNumN++;
				};
				GUIMain.ShowCommonDialog(action, "CMD_NormalLogin", null);
				while (!isClose)
				{
					yield return null;
				}
			}
		}
		yield break;
	}

	private IEnumerator CheckRecoverBattle()
	{
		BattleNextBattleOption.ClearBattleMenuSettings();
		if (ClassSingleton<BattleDataStore>.Instance.IsBattleRecoverable)
		{
			bool isCancel = false;
			Action onBattleRecover = delegate()
			{
				GUIMain.BarrierOFF();
			};
			Action onCancelAction = delegate()
			{
				isCancel = true;
				PlayerPrefs.SetString("userDungeonTicketId", string.Empty);
			};
			ClassSingleton<BattleDataStore>.Instance.OpenBattleRecoverConfirm(onBattleRecover, onCancelAction);
			while (!isCancel)
			{
				yield return null;
			}
		}
		yield break;
	}

	private void ShowWebWindow()
	{
		DataMng dataMng = DataMng.Instance();
		if (!this.isStartGuidance)
		{
			if (null != dataMng && dataMng.IsPopUpInformaiton)
			{
				Action<int> action = delegate(int x)
				{
					base.StartCoroutine(this.ShowExtraPopupInformations());
				};
				CMDWebWindow cmdwebWindow = GUIMain.ShowCommonDialog(action, "CMDWebWindow", null) as CMDWebWindow;
				cmdwebWindow.TitleText = StringMaster.GetString("InfomationTitle");
				cmdwebWindow.Url = WebAddress.EXT_ADR_INFO;
				CMDWebWindow cmdwebWindow2 = cmdwebWindow;
				cmdwebWindow2.Url = cmdwebWindow2.Url + "?countryCode=" + CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN);
				if (null != dataMng)
				{
					dataMng.IsPopUpInformaiton = false;
				}
			}
			else
			{
				base.StartCoroutine(this.ShowExtraPopupInformations());
			}
		}
	}

	private IEnumerator ShowExtraPopupInformations()
	{
		DataMng.Instance().ShowPopupInfoNum = 0;
		if (DataMng.Instance().ShowPopupInfoIds == null)
		{
			DataMng.Instance().ShowPopupInfoIds = new Queue();
		}
		GameWebAPI.RespDataIN_InfoList dts = DataMng.Instance().RespDataIN_InfoList;
		GameWebAPI.RespDataIN_InfoList.InfoList[] infoList = dts.infoList;
		while (DataMng.Instance().ShowPopupInfoNum < infoList.Length)
		{
			GameWebAPI.RespDataIN_InfoList.InfoList dt = infoList[DataMng.Instance().ShowPopupInfoNum];
			if (dt.popupFlg == 1 && !DataMng.Instance().ShowPopupInfoIds.Contains(int.Parse(dt.userInfoId)))
			{
				bool isClose = false;
				Action<int> action = delegate(int x)
				{
					isClose = true;
					DataMng.Instance().ShowPopupInfoNum++;
				};
				CMDWebWindowPopup cd = GUIMain.ShowCommonDialog(action, "CMDWebWindowPopup", null) as CMDWebWindowPopup;
				cd.setLinkCategoryType(int.Parse(dt.linkCategoryType));
				cd.userInfoId = int.Parse(dt.userInfoId);
				cd.TitleText = dt.title;
				cd.Url = ConstValue.APP_WEB_DOMAIN + ConstValue.WEB_INFO_ADR + dt.userInfoId;
				CMDWebWindowPopup cmdwebWindowPopup = cd;
				cmdwebWindowPopup.Url = cmdwebWindowPopup.Url + "&countryCode=" + CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN);
				while (!isClose)
				{
					yield return null;
				}
			}
			else
			{
				DataMng.Instance().ShowPopupInfoNum++;
			}
		}
		yield break;
	}

	private void DownloadMenuBanner()
	{
		GUIBannerPanel.Data = DataMng.Instance().RespData_BannerMaster;
		if (GUIBannerPanel.Data != null)
		{
			this.partsMenu.SetMenuBanner();
		}
	}

	protected override void Update()
	{
		this.UpdateBackKeyAndroid();
	}

	private void UpdateBackKeyAndroid()
	{
		if (GUIScreenHome.enableBackKeyAndroid && GUIManager.IsEnableBackKeyAndroid() && Input.GetKeyDown(KeyCode.Escape))
		{
			CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.BackToTitle), "CMD_Confirm", null) as CMD_Confirm;
			cmd_Confirm.Title = StringMaster.GetString("SystemConfirm");
			cmd_Confirm.Info = StringMaster.GetString("BackKeyConfirmGoTitle");
			this.partsMenu.ForceHide(false);
			SoundMng.Instance().PlaySE("SEInternal/Common/se_106", 0f, false, true, null, -1, 1f);
		}
	}

	private void BackToTitle(int idx)
	{
		if (idx == 0)
		{
			GUIMain.BackToTOP("UITitle", 0.8f, 0.8f);
		}
	}

	protected void StartCacheBattle()
	{
		if (!GUIScreenHome.isCacheBattled)
		{
			AssetDataCacheMng.Instance().DeleteCacheType(AssetDataCacheMng.CACHE_TYPE.BATTLE_COMMON);
			List<string> battleCommon = AssetDataCacheData.GetBattleCommon();
			AssetDataCacheMng.Instance().RegisterCacheType(battleCommon, AssetDataCacheMng.CACHE_TYPE.BATTLE_COMMON, false);
			GUIScreenHome.isCacheBattled = true;
		}
	}

	protected void StartCacheParty()
	{
		AssetDataCacheMng.Instance().DeleteCacheType(AssetDataCacheMng.CACHE_TYPE.CHARA_PARTY);
		List<string> deckMonsterPathList = ClassSingleton<MonsterUserDataMng>.Instance.GetDeckMonsterPathList(true);
		AssetDataCacheMng.Instance().RegisterCacheType(deckMonsterPathList, AssetDataCacheMng.CACHE_TYPE.CHARA_PARTY, false);
	}

	private IEnumerator checkAndSyncCountryCode()
	{
		if (!DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.countryCode.Equals(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN)))
		{
			GameWebAPI.RequestUS_RegisterLanguageInfo requestUS_RegisterLanguageInfo = new GameWebAPI.RequestUS_RegisterLanguageInfo();
			requestUS_RegisterLanguageInfo.SetSendData = delegate(GameWebAPI.US_Req_RegisterLanguageInfo param)
			{
				param.countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
			};
			GameWebAPI.RequestUS_RegisterLanguageInfo request = requestUS_RegisterLanguageInfo;
			yield return base.StartCoroutine(request.Run(null, null, null));
		}
		yield break;
	}

	public void CreateHomeUI()
	{
		if (null == this.faceUI)
		{
			GameObject gameObject = AssetDataMng.Instance().LoadObject("UIPrefab/GUI/Face", null, true) as GameObject;
			this.faceUI = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			if (null != this.faceUI)
			{
				Transform transform = this.faceUI.transform;
				transform.parent = Singleton<GUIMain>.Instance.transform;
				transform.localScale = Vector3.one;
				transform.localPosition = Vector3.zero;
				transform.localRotation = Quaternion.identity;
				transform.name = gameObject.name;
				if (null != GUIFaceIndicator.instance && null != GUIFace.instance)
				{
					GUIFaceIndicator.instance.gameObject.SetActive(true);
					GUIFace.instance.gameObject.SetActive(true);
				}
			}
		}
	}

	public override void HideGUI()
	{
		if (null != this.faceUI)
		{
			UnityEngine.Object.Destroy(this.faceUI);
			this.faceUI = null;
		}
	}
}
