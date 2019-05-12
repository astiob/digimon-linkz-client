using FarmData;
using Monster;
using Neptune.Movie;
using Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TutorialRequestHeader;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebAPIRequest;

public sealed class TutorialControlToGame : MonoBehaviour
{
	private TutorialEmphasizeUI emphasizeUI;

	private int tutorialBattleDungeonId;

	private Action actionStartBattleResultScreen;

	private TutorialControlToGame.SoundVolumeFadeInfo soundInfo;

	private GameObject digimon;

	private bool isBackgroundDownload;

	private bool isStandardDownload;

	private List<GameObject> effectObjects = new List<GameObject>();

	public bool IsBackgroundDownload
	{
		get
		{
			return this.isBackgroundDownload;
		}
	}

	public bool IsStandardDownload
	{
		get
		{
			return this.isStandardDownload;
		}
	}

	public void SetTutorialDungeonId()
	{
		this.tutorialBattleDungeonId = 9001;
	}

	private IEnumerator LoadPrefab<T>(string fileName, bool active, Transform parent, Action<T> loadCompleted)
	{
		GameObject resource = AssetDataMng.Instance().LoadObject(fileName, null, true) as GameObject;
		yield return null;
		GameObject go = UnityEngine.Object.Instantiate<GameObject>(resource);
		go.transform.parent = parent;
		go.transform.localScale = Vector3.one;
		go.transform.localPosition = Vector3.zero;
		go.SetActive(active);
		yield return null;
		if (loadCompleted != null)
		{
			loadCompleted(go.GetComponent<T>());
		}
		resource = null;
		Resources.UnloadUnusedAssets();
		yield break;
	}

	public void ChangeScene(int sceneType, Action completed, bool isSkipTutorial, Action skipBattle)
	{
		GUIManager.CloseAllCommonDialog(delegate(int x)
		{
			this.OpenNewScene(sceneType, completed, isSkipTutorial, skipBattle);
		});
	}

	private void OpenNewScene(int sceneType, Action completed, bool isSkipTutorial, Action skipBattle)
	{
		TutorialScene.BackGroundType backGroundType = TutorialScene.BackGroundType.NONE;
		switch (sceneType)
		{
		case 0:
			backGroundType = TutorialScene.BackGroundType.BLACK;
			break;
		case 1:
			backGroundType = TutorialScene.BackGroundType.BLUE;
			break;
		case 2:
			backGroundType = TutorialScene.BackGroundType.WARP;
			break;
		case 3:
			backGroundType = TutorialScene.BackGroundType.DIGITAL_WORLD;
			break;
		case 4:
			backGroundType = TutorialScene.BackGroundType.DIGITAL_WORLD_COLLAPSE;
			break;
		case 5:
			base.StartCoroutine(this.ServerRequestStartBattle(completed, skipBattle));
			break;
		case 6:
			this.ChangeHomeScene(completed, isSkipTutorial);
			break;
		case 7:
			backGroundType = TutorialScene.BackGroundType.WHITE;
			break;
		}
		if (backGroundType != TutorialScene.BackGroundType.NONE)
		{
			TutorialScene tutorialScene = this.GetTutorialScene();
			tutorialScene.SetBackGround(backGroundType, completed);
		}
	}

	private void ChangeHomeScene(Action completed, bool isSkipTutorial)
	{
		Singleton<GUIMain>.Instance.gameObject.transform.localPosition = Vector3.zero;
		GUIScreenHomeTutorial guiscreenHomeTutorial = ScreenController.ChangeTutorialHomeScreen() as GUIScreenHomeTutorial;
		guiscreenHomeTutorial.actionFinishedLoad = completed;
		guiscreenHomeTutorial.isSkipTutorial = isSkipTutorial;
	}

	private TutorialScene GetTutorialScene()
	{
		TutorialScene tutorialScene = UnityEngine.Object.FindObjectOfType<TutorialScene>();
		if (null == tutorialScene)
		{
			GUIMain.ReqScreen("TutorialScene", string.Empty);
			tutorialScene = UnityEngine.Object.FindObjectOfType<TutorialScene>();
		}
		return tutorialScene;
	}

	public int GetBlackSceneID()
	{
		return 0;
	}

	public int GetBlueSceneID()
	{
		return 1;
	}

	public void ShakeBackGround(float intensity, float decay, Action completed)
	{
		TutorialScene tutorialScene = UnityEngine.Object.FindObjectOfType<TutorialScene>();
		if (null != tutorialScene)
		{
			tutorialScene.ShakeBackGround(intensity, decay, completed);
		}
	}

	public void ShakeStopBackGround(float decay, Action completed)
	{
		TutorialScene tutorialScene = UnityEngine.Object.FindObjectOfType<TutorialScene>();
		if (null != tutorialScene)
		{
			tutorialScene.ShakeStopBackGround(decay, completed);
		}
	}

	public void SuspendShakeBackGround()
	{
		TutorialScene tutorialScene = UnityEngine.Object.FindObjectOfType<TutorialScene>();
		if (null != tutorialScene)
		{
			tutorialScene.SuspendShakeBackGround();
		}
	}

	public IEnumerator OpenUI(string uiType, TutorialUI tutorialUI, Action completed)
	{
		if ("NAME" == uiType)
		{
			if (string.IsNullOrEmpty(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.nickname))
			{
				IEnumerator enumerator = this.LoadPrefab<TutorialUserName>("Tutorial/TutorialUserName", true, tutorialUI.transform, delegate(TutorialUserName uiName)
				{
					uiName.FinishAction = completed;
				});
				yield return base.StartCoroutine(enumerator);
			}
			else if (completed != null)
			{
				completed();
			}
		}
		yield break;
	}

	private IEnumerator ServerRequestStartBattle(Action completed, Action skipBattle)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
		yield return base.StartCoroutine(this.RequestBattleData());
		if (!ClassSingleton<QuestData>.Instance.IsTutorialDungeonClear())
		{
			yield return base.StartCoroutine(this.StartBattle());
			BattleStateManager.onAutoChangeTutorialMode = true;
			GUICollider.DisableAllCollider(base.gameObject.name);
			GUIMain.ReqScreen("UIIdle", string.Empty);
			GUIFace.ForceHideDigiviceBtn_S();
			BattleStateManager.onAutoServerConnect = true;
			SceneManager.LoadScene(BattleStateManager.BattleSceneName);
			if (completed != null)
			{
				completed();
			}
		}
		else
		{
			TutorialScene scene = this.GetTutorialScene();
			scene.SetBackGround(TutorialScene.BackGroundType.BLUE, skipBattle);
		}
		yield break;
	}

	private IEnumerator RequestBattleData()
	{
		RequestList requestList = new RequestList();
		ClassSingleton<MonsterUserDataMng>.Instance.Initialize();
		RequestBase addRequest = new TutorialMonsterEvolution();
		requestList.AddRequest(addRequest);
		GameWebAPI.MonsterSlotInfoListLogic monsterSlotInfoListLogic = new GameWebAPI.MonsterSlotInfoListLogic();
		monsterSlotInfoListLogic.OnReceived = delegate(GameWebAPI.RespDataCS_MonsterSlotInfoListLogic response)
		{
			ChipDataMng.GetUserChipSlotData().SetMonsterSlotList(response.slotInfo);
		};
		addRequest = monsterSlotInfoListLogic;
		requestList.AddRequest(addRequest);
		GameWebAPI.RequestMonsterList requestMonsterList = new GameWebAPI.RequestMonsterList();
		requestMonsterList.OnReceived = delegate(GameWebAPI.RespDataUS_GetMonsterList response)
		{
			ClassSingleton<MonsterUserDataMng>.Instance.SetUserMonsterData(response.userMonsterList);
		};
		addRequest = requestMonsterList;
		requestList.AddRequest(addRequest);
		GameWebAPI.RequestMN_DeckList requestMN_DeckList = new GameWebAPI.RequestMN_DeckList();
		requestMN_DeckList.OnReceived = delegate(GameWebAPI.RespDataMN_GetDeckList response)
		{
			DataMng.Instance().RespDataMN_DeckList = response;
		};
		addRequest = requestMN_DeckList;
		requestList.AddRequest(addRequest);
		GameWebAPI.RequestWD_WorldDungeonInfo requestWD_WorldDungeonInfo = new GameWebAPI.RequestWD_WorldDungeonInfo();
		requestWD_WorldDungeonInfo.SetSendData = delegate(GameWebAPI.WD_Req_DngInfo param)
		{
			param.worldId = "4";
		};
		requestWD_WorldDungeonInfo.OnReceived = delegate(GameWebAPI.RespDataWD_GetDungeonInfo response)
		{
			ClassSingleton<QuestData>.Instance.AddWD_DngInfoDataList("4", response);
		};
		addRequest = requestWD_WorldDungeonInfo;
		requestList.AddRequest(addRequest);
		return requestList.Run(null, null, null);
	}

	private IEnumerator StartBattle()
	{
		TipsLoading.Instance.StartTipsLoad(CMD_Tips.DISPLAY_PLACE.TutorialToBattle, true);
		string dungeonId = this.tutorialBattleDungeonId.ToString();
		GameWebAPI.RequestWD_WorldStart requestWD_WorldStart = new GameWebAPI.RequestWD_WorldStart();
		requestWD_WorldStart.SetSendData = delegate(GameWebAPI.WD_Req_DngStart param)
		{
			param.dungeonId = dungeonId;
			param.deckNum = "1";
		};
		requestWD_WorldStart.OnReceived = delegate(GameWebAPI.RespDataWD_DungeonStart response)
		{
			ClassSingleton<QuestData>.Instance.RespDataWD_DungeonStart = response;
		};
		GameWebAPI.RequestWD_WorldStart request = requestWD_WorldStart;
		return request.Run(delegate()
		{
			if (null != DataMng.Instance() && DataMng.Instance().WD_ReqDngResult != null)
			{
				DataMng.Instance().WD_ReqDngResult.dungeonId = dungeonId;
			}
		}, null, null);
	}

	public IEnumerator WaitBattleStart(Action startEffectEndAction)
	{
		BattleTutorial managerTutorial;
		for (managerTutorial = UnityEngine.Object.FindObjectOfType<BattleStateManager>().tutorial; managerTutorial == null; managerTutorial = UnityEngine.Object.FindObjectOfType<BattleStateManager>().tutorial)
		{
			yield return null;
		}
		managerTutorial.isPossibleTargetSelect = false;
		bool wait = false == managerTutorial.isBattleStarted;
		while (wait)
		{
			yield return null;
			wait = (false == managerTutorial.isBattleStarted);
		}
		if (startEffectEndAction != null)
		{
			startEffectEndAction();
		}
		yield break;
	}

	public void BattlePause(bool enable)
	{
		BattleTutorial tutorial = UnityEngine.Object.FindObjectOfType<BattleStateManager>().tutorial;
		if (tutorial != null)
		{
			tutorial.SetPose(enable);
		}
	}

	public IEnumerator WaitBattleActionSelect(Action completed)
	{
		BattleTutorial managerTutorial = UnityEngine.Object.FindObjectOfType<BattleStateManager>().tutorial;
		int nowIndex = managerTutorial.currentSelectCharacter;
		int prevIndex = nowIndex;
		while (prevIndex == nowIndex)
		{
			yield return null;
			nowIndex = managerTutorial.currentSelectCharacter;
		}
		if (completed != null)
		{
			completed();
		}
		yield break;
	}

	public void StartManualBattle()
	{
		BattleStateManager battleStateManager = UnityEngine.Object.FindObjectOfType<BattleStateManager>();
		if (null != battleStateManager)
		{
			BattleTutorial tutorial = battleStateManager.tutorial;
			if (tutorial != null)
			{
				tutorial.SetDeactiveTutorialInduction();
				tutorial.isPossibleTargetSelect = true;
				tutorial.SetEnableBackKey();
			}
		}
	}

	public void WaitBattleResultScreen(Action completed)
	{
		this.actionStartBattleResultScreen = completed;
		Singleton<GUIMain>.Instance.onStartNewScreenEvent += this.OnStartBattleResultScreen;
	}

	private void OnStartBattleResultScreen(string screenName)
	{
		if ("UIResult" == screenName && this.actionStartBattleResultScreen != null)
		{
			Singleton<GUIMain>.Instance.onStartNewScreenEvent -= this.OnStartBattleResultScreen;
			this.actionStartBattleResultScreen();
			this.actionStartBattleResultScreen = null;
			GUIScreenResult guiscreenResult = UnityEngine.Object.FindObjectOfType<GUIScreenResult>();
			guiscreenResult.SetResultTimeOutMode(GUIScreenResult.ResultTimeOutMode.TITLE);
		}
	}

	public IEnumerator WaitBattleResultEffectEnd(Action<CMD_BattleResult> completed)
	{
		GUIScreenResult screenResult = GUIScreenResult.instance;
		if (null != screenResult)
		{
			CMD_BattleResult battleResult = UnityEngine.Object.FindObjectOfType<CMD_BattleResult>();
			while (null == battleResult)
			{
				yield return null;
				battleResult = UnityEngine.Object.FindObjectOfType<CMD_BattleResult>();
			}
			battleResult.SetActionEffectFinished(completed);
		}
		else if (completed != null)
		{
			completed(null);
		}
		yield break;
	}

	public void WaitFirstClearWindowOpen(Action completed)
	{
		if (null != GUIScreenResult.instance)
		{
			GUIScreenResult.instance.SetActionOfOpenedFirstClear(completed);
		}
	}

	public void DeleteBattleScreen(Action completed)
	{
		SoundMng.Instance().StopBGM(0f, null);
		SceneManager.LoadScene("Empty");
		Time.timeScale = 1f;
		Singleton<GUIMain>.Instance.gameObject.transform.localPosition = Vector3.zero;
		if (completed != null)
		{
			completed();
		}
	}

	public void SelectFacility(int facilityId)
	{
		FarmRoot instance = FarmRoot.Instance;
		if (null != instance)
		{
			FarmObject[] array = instance.Scenery.farmObjects.Where((FarmObject x) => x.facilityID == facilityId).ToArray<FarmObject>();
			if (0 < array.Length)
			{
				instance.SelectObject.SetSelectObject(array[0].gameObject);
			}
		}
	}

	public void ReleaseFacility()
	{
		FarmRoot instance = FarmRoot.Instance;
		if (null != instance)
		{
			instance.ClearSettingFarmObject();
		}
	}

	public void WaitBuildCompleteTap(Action onTouch, Action completed, int facilityId)
	{
		FarmRoot farmRoot = FarmRoot.Instance;
		if (null != farmRoot)
		{
			FarmObject facility = farmRoot.Scenery.farmObjects.SingleOrDefault((FarmObject x) => x.facilityID == facilityId);
			if (null != facility)
			{
				FarmObject_MeatFarm farmObject_MeatFarm = facility as FarmObject_MeatFarm;
				if (null != farmObject_MeatFarm)
				{
					farmObject_MeatFarm.IsDummyFacility = true;
				}
				GUICollider.DisableAllCollider(base.gameObject.name);
				InputControll inputController = farmRoot.Input;
				inputController.disabledCameraOperation = true;
				inputController.RemoveAllTouchEndEvent();
				inputController.EnableControl = true;
				ConstructionDetail constructionDetail = farmRoot.farmUI.GetConstructionDetail(facility.userFacilityID);
				constructionDetail.DisableCollider();
				farmRoot.Input.AddTouchEndEvent(delegate(InputControll ic, bool isDrag)
				{
					if (!isDrag && null != ic.rayHitColliderObject && facility == ic.rayHitColliderObject.GetComponent<FarmObject>())
					{
						inputController.RemoveAllTouchEndEvent();
						inputController.EnableControl = false;
						if (onTouch != null)
						{
							onTouch();
						}
						UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(facility.userFacilityID);
						userFacility.level = 1;
						userFacility.completeTime = null;
						facility.ClearConstruction();
						this.FacilityBuildEffect(farmRoot, facility, inputController, completed);
					}
				});
			}
		}
	}

	private void FacilityBuildEffect(FarmRoot farmRoot, FarmObject facility, InputControll ic, Action completed)
	{
		SoundMng.Instance().TryPlaySE("SEInternal/Farm/se_202", 0f, false, true, null, -1);
		EffectAnimatorObserver buildCompleteEffect = farmRoot.GetBuildCompleteEffect(facility.transform);
		if (null != buildCompleteEffect)
		{
			EffectAnimatorEventTime component = buildCompleteEffect.GetComponent<EffectAnimatorEventTime>();
			component.SetEvent(0, delegate
			{
				facility.BuildEffect();
				ic.disabledCameraOperation = false;
				GUICollider.EnableAllCollider(this.gameObject.name);
				if (completed != null)
				{
					completed();
				}
			});
			buildCompleteEffect.Play();
		}
	}

	public void FacilityBuildComplete(int facilityId)
	{
		FarmRoot instance = FarmRoot.Instance;
		if (null != instance)
		{
			FarmObject farmObject = instance.Scenery.farmObjects.SingleOrDefault((FarmObject x) => x.facilityID == facilityId);
			if (null != farmObject)
			{
				UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(farmObject.userFacilityID);
				FarmObject_MeatFarm farmObject_MeatFarm = farmObject as FarmObject_MeatFarm;
				if (null != farmObject_MeatFarm)
				{
					farmObject_MeatFarm.IsDummyFacility = true;
				}
				if (!string.IsNullOrEmpty(userFacility.completeTime))
				{
					userFacility.level = 1;
					userFacility.completeTime = null;
					farmObject.ClearConstruction();
					farmObject.BuildEffect();
				}
			}
		}
	}

	public void WaitMeatHarvest(Action completed, int meatNum)
	{
		FarmRoot instance = FarmRoot.Instance;
		if (null != instance)
		{
			FarmObject[] array = instance.Scenery.farmObjects.Where((FarmObject x) => x.facilityID == 1).ToArray<FarmObject>();
			if (0 < array.Length)
			{
				InputControll inputController = instance.Input;
				inputController.disabledCameraOperation = true;
				inputController.RemoveAllTouchEndEvent();
				inputController.EnableControl = true;
				GUICollider.DisableAllCollider(base.gameObject.name);
				FarmObject_MeatFarm component = array[0].GetComponent<FarmObject_MeatFarm>();
				component.SetDummyMeat(meatNum, delegate
				{
					if (completed != null)
					{
						completed();
					}
					inputController.EnableControl = false;
					inputController.disabledCameraOperation = false;
					GUICollider.EnableAllCollider(this.gameObject.name);
				});
			}
		}
	}

	public IEnumerator WaitOpenTrainingMenuUI(Action completed)
	{
		CMD_Training_Menu window = UnityEngine.Object.FindObjectOfType<CMD_Training_Menu>();
		while (null == window)
		{
			yield return null;
			window = UnityEngine.Object.FindObjectOfType<CMD_Training_Menu>();
		}
		if (completed != null)
		{
			window.onOpened += completed;
		}
		yield break;
	}

	public IEnumerator WaitOpenMealSelectDigimonUI(Action completed)
	{
		CMD_BaseSelect window = UnityEngine.Object.FindObjectOfType<CMD_BaseSelect>();
		while (null == window)
		{
			yield return null;
			window = UnityEngine.Object.FindObjectOfType<CMD_BaseSelect>();
		}
		if (completed != null)
		{
			window.onOpened += completed;
		}
		yield break;
	}

	public IEnumerator WaitOpenMealUI(Action completed)
	{
		CMD_MealExecution window = UnityEngine.Object.FindObjectOfType<CMD_MealExecution>();
		while (null == window)
		{
			yield return null;
			window = UnityEngine.Object.FindObjectOfType<CMD_MealExecution>();
		}
		if (completed != null)
		{
			completed();
		}
		yield break;
	}

	public IEnumerator WaitOpenGashaUI(Action onOpened)
	{
		CMD_GashaTOP window = UnityEngine.Object.FindObjectOfType<CMD_GashaTOP>();
		while (null == window)
		{
			yield return null;
			window = UnityEngine.Object.FindObjectOfType<CMD_GashaTOP>();
		}
		if (onOpened != null)
		{
			window.onOpened += onOpened;
		}
		yield break;
	}

	public IEnumerator WaitOpenDigiGarden(Action onOpened)
	{
		while (null == CMD_DigiGarden.instance)
		{
			yield return null;
		}
		CMD_DigiGarden.instance.IsOfflineModeFlag = true;
		CMD_DigiGarden.instance.OfflineTimeUntilEvolution = "23:59:00";
		CMD_DigiGarden.instance.OfflineGrowNeedStone = 24;
		if (onOpened != null)
		{
			CMD_DigiGarden.instance.onOpened += onOpened;
		}
		yield break;
	}

	public IEnumerator WaitChangeDigiGardenList(Action onOpened)
	{
		CMD_FarewellListRun ui = UnityEngine.Object.FindObjectOfType<CMD_FarewellListRun>();
		while (null == ui)
		{
			yield return null;
			ui = UnityEngine.Object.FindObjectOfType<CMD_FarewellListRun>();
		}
		if (onOpened != null)
		{
			ui.onOpened += onOpened;
		}
		yield break;
	}

	public void WaitChangeDigiGardenSetList(Action onOpened)
	{
		CMD_FarewellListRun cmd_FarewellListRun = UnityEngine.Object.FindObjectOfType<CMD_FarewellListRun>();
		if (null != cmd_FarewellListRun)
		{
			if (onOpened != null)
			{
				cmd_FarewellListRun.SetCloseAction(delegate(int i)
				{
					onOpened();
				});
			}
		}
		else if (onOpened != null)
		{
			onOpened();
		}
	}

	public void WaitOpenedForGashaCharacterDetailWindow(Action completed)
	{
		CMD_GashaTOP cmd_GashaTOP = UnityEngine.Object.FindObjectOfType<CMD_GashaTOP>();
		if (null != cmd_GashaTOP)
		{
			cmd_GashaTOP.SetFinishedActionCutScene(completed);
		}
		else if (completed != null)
		{
			completed();
		}
	}

	public void WaitOpenedForDigiGardenCharacterDetailWindow(Action completed)
	{
		CMD_DigiGarden cmd_DigiGarden = UnityEngine.Object.FindObjectOfType<CMD_DigiGarden>();
		if (null != cmd_DigiGarden)
		{
			cmd_DigiGarden.SetFinishedActionCutScene(completed);
		}
		else if (completed != null)
		{
			completed();
		}
	}

	public void WaitLevelUpForMeal(Action completed)
	{
		CMD_MealExecution cmd_MealExecution = UnityEngine.Object.FindObjectOfType<CMD_MealExecution>();
		cmd_MealExecution.SetActionLevelUp(completed);
	}

	private TutorialEmphasizeUI FindEmphasizeUI(string uiType)
	{
		TutorialEmphasizeUI tutorialEmphasizeUI = null;
		string uiType2 = uiType;
		switch (uiType2)
		{
		case "MEAL_DIGI":
		{
			List<MonsterData> monsterDataList = MonsterDataMng.Instance().GetMonsterDataList();
			MonsterSort.SortMonsterUserDataList(monsterDataList, MonsterSortType.DATE, MonsterSortOrder.DESC);
			GUIMonsterIcon icon = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monsterDataList.Last<MonsterData>());
			tutorialEmphasizeUI = icon.gameObject.AddComponent<TutorialEmphasizeUI>();
			tutorialEmphasizeUI.UiName = TutorialEmphasizeUI.UiNameType.MEAL_DIGI;
			return tutorialEmphasizeUI;
		}
		case "GARDEN_DIGI":
		{
			List<MonsterData> monsterDataList2 = MonsterDataMng.Instance().GetMonsterDataList();
			MonsterSort.SortMonsterUserDataList(monsterDataList2, MonsterSortType.DATE, MonsterSortOrder.DESC);
			GUIMonsterIcon icon2 = ClassSingleton<GUIMonsterIconList>.Instance.GetIcon(monsterDataList2.Last<MonsterData>());
			tutorialEmphasizeUI = icon2.gameObject.AddComponent<TutorialEmphasizeUI>();
			tutorialEmphasizeUI.UiName = TutorialEmphasizeUI.UiNameType.GARDEN_DIGI;
			return tutorialEmphasizeUI;
		}
		case "PARTY":
		{
			BattleTutorial tutorial = UnityEngine.Object.FindObjectOfType<BattleStateManager>().tutorial;
			GameObject getCharacterUIButtonRoot = tutorial.GetCharacterUIButtonRoot;
			tutorialEmphasizeUI = getCharacterUIButtonRoot.AddComponent<TutorialEmphasizeUI>();
			tutorialEmphasizeUI.UiName = TutorialEmphasizeUI.UiNameType.PARTY;
			BoxCollider[] componentsInChildren = getCharacterUIButtonRoot.GetComponentsInChildren<BoxCollider>();
			if (componentsInChildren != null)
			{
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = false;
				}
			}
			tutorialEmphasizeUI.IsParentChange = true;
			return tutorialEmphasizeUI;
		}
		case "ATTACK":
		{
			BattleTutorial tutorial2 = UnityEngine.Object.FindObjectOfType<BattleStateManager>().tutorial;
			GameObject getAttackUIButton = tutorial2.GetAttackUIButton;
			tutorialEmphasizeUI = getAttackUIButton.AddComponent<TutorialEmphasizeUI>();
			tutorialEmphasizeUI.UiName = TutorialEmphasizeUI.UiNameType.ATTACK;
			tutorialEmphasizeUI.IsParentChange = true;
			return tutorialEmphasizeUI;
		}
		case "OPERATION":
		{
			BattleTutorial tutorial3 = UnityEngine.Object.FindObjectOfType<BattleStateManager>().tutorial;
			GameObject getInheritanceTechniqueUIButton = tutorial3.GetInheritanceTechniqueUIButton;
			tutorialEmphasizeUI = getInheritanceTechniqueUIButton.AddComponent<TutorialEmphasizeUI>();
			tutorialEmphasizeUI.UiName = TutorialEmphasizeUI.UiNameType.OPERATION;
			tutorialEmphasizeUI.IsParentChange = true;
			return tutorialEmphasizeUI;
		}
		case "SKILL":
		{
			BattleTutorial tutorial4 = UnityEngine.Object.FindObjectOfType<BattleStateManager>().tutorial;
			GameObject getDeathblowUIButton = tutorial4.GetDeathblowUIButton;
			tutorialEmphasizeUI = getDeathblowUIButton.AddComponent<TutorialEmphasizeUI>();
			tutorialEmphasizeUI.UiName = TutorialEmphasizeUI.UiNameType.SKILL;
			tutorialEmphasizeUI.IsParentChange = true;
			return tutorialEmphasizeUI;
		}
		case "SKILL_HOLD":
		{
			BattleTutorial tutorial5 = UnityEngine.Object.FindObjectOfType<BattleStateManager>().tutorial;
			GameObject getDeathblowUIButton2 = tutorial5.GetDeathblowUIButton;
			tutorialEmphasizeUI = getDeathblowUIButton2.AddComponent<TutorialEmphasizeUI>();
			tutorialEmphasizeUI.UiName = TutorialEmphasizeUI.UiNameType.SKILL_HOLD;
			tutorialEmphasizeUI.IsParentChange = true;
			tutorialEmphasizeUI.IsHoldPressOnly = true;
			return tutorialEmphasizeUI;
		}
		case "RECOVERY":
		{
			BattleTutorial tutorial6 = UnityEngine.Object.FindObjectOfType<BattleStateManager>().tutorial;
			GameObject getCharacterButtonsCenterUIButton = tutorial6.GetCharacterButtonsCenterUIButton;
			tutorialEmphasizeUI = getCharacterButtonsCenterUIButton.AddComponent<TutorialEmphasizeUI>();
			tutorialEmphasizeUI.UiName = TutorialEmphasizeUI.UiNameType.RECOVERY;
			tutorialEmphasizeUI.IsParentChange = true;
			return tutorialEmphasizeUI;
		}
		case "RECOVERY_YES":
		{
			BattleTutorial tutorial7 = UnityEngine.Object.FindObjectOfType<BattleStateManager>().tutorial;
			GameObject getRevivalDialogEnterUIButton = tutorial7.GetRevivalDialogEnterUIButton;
			tutorialEmphasizeUI = getRevivalDialogEnterUIButton.AddComponent<TutorialEmphasizeUI>();
			tutorialEmphasizeUI.UiName = TutorialEmphasizeUI.UiNameType.RECOVERY_YES;
			tutorialEmphasizeUI.IsParentChange = true;
			return tutorialEmphasizeUI;
		}
		case "RECOVERY_NO":
		{
			BattleTutorial tutorial8 = UnityEngine.Object.FindObjectOfType<BattleStateManager>().tutorial;
			GameObject getRevivalDialogCancelUIButton = tutorial8.GetRevivalDialogCancelUIButton;
			tutorialEmphasizeUI = getRevivalDialogCancelUIButton.AddComponent<TutorialEmphasizeUI>();
			tutorialEmphasizeUI.UiName = TutorialEmphasizeUI.UiNameType.RECOVERY_NO;
			tutorialEmphasizeUI.IsParentChange = true;
			return tutorialEmphasizeUI;
		}
		}
		TutorialEmphasizeUI[] source = UnityEngine.Object.FindObjectsOfType<TutorialEmphasizeUI>();
		TutorialEmphasizeUI[] array = source.Where((TutorialEmphasizeUI x) => x.UiName.ToString() == uiType).ToArray<TutorialEmphasizeUI>();
		if (0 < array.Length)
		{
			tutorialEmphasizeUI = array[0];
			if (tutorialEmphasizeUI.UiName == TutorialEmphasizeUI.UiNameType.DIGISTONE)
			{
				BoxCollider componentInChildren = tutorialEmphasizeUI.GetComponentInChildren<BoxCollider>();
				componentInChildren.enabled = false;
			}
		}
		return tutorialEmphasizeUI;
	}

	public void SetInduceUI(string uiType, bool enabledFrame, int arrowPosition, TutorialUI tutorialUI, Action onPushedEvent)
	{
		this.emphasizeUI = this.FindEmphasizeUI(uiType);
		Action addPushEvent = onPushedEvent;
		if (null != this.emphasizeUI)
		{
			GameObject gameObject = this.emphasizeUI.gameObject;
			TutorialEmphasizeUI.UiNameType uiName = this.emphasizeUI.UiName;
			switch (uiName)
			{
			case TutorialEmphasizeUI.UiNameType.MEAL_DIGI:
			{
				GUIMonsterIcon component = this.emphasizeUI.GetComponent<GUIMonsterIcon>();
				component.LongTouch = false;
				break;
			}
			default:
				if (uiName != TutorialEmphasizeUI.UiNameType.SKILL)
				{
					if (uiName == TutorialEmphasizeUI.UiNameType.SKILL_HOLD)
					{
						BattleTutorial tutorial = UnityEngine.Object.FindObjectOfType<BattleStateManager>().tutorial;
						if (tutorial != null)
						{
							tutorial.SetActiveShowSkillDescription();
						}
						HoldPressButton component2 = this.emphasizeUI.GetComponent<HoldPressButton>();
						this.emphasizeUI.onHoldWaitPress = component2.onHoldWaitPress;
						this.emphasizeUI.onDisengagePress = component2.onDisengagePress;
						component2.enabled = false;
					}
				}
				else
				{
					HoldPressButton component3 = this.emphasizeUI.GetComponent<HoldPressButton>();
					component3.enabled = false;
				}
				break;
			case TutorialEmphasizeUI.UiNameType.MEAL_PLUS:
			{
				GUICollider component4 = this.emphasizeUI.GetComponent<GUICollider>();
				component4.callBackState = GUICollider.CallBackState.OnTouchEnded;
				break;
			}
			case TutorialEmphasizeUI.UiNameType.RECOVERY:
			{
				HoldPressButton component5 = this.emphasizeUI.GetComponent<HoldPressButton>();
				component5.enabled = false;
				List<EventDelegate> holdPressButtonEvents = component5.onDisengagePress;
				addPushEvent = delegate()
				{
					onPushedEvent();
					EventDelegate.Execute(holdPressButtonEvents);
				};
				gameObject = this.emphasizeUI.transform.FindChild("MonsterButtonGraphicRootScaler/MonsterButtonGraphicRoot").gameObject;
				break;
			}
			}
			this.emphasizeUI.SetEmphasize(tutorialUI.transform);
			this.emphasizeUI.SetActiveCollider(false);
			tutorialUI.SetTargetUI(enabledFrame, (TutorialTarget.ArrowPositon)arrowPosition, gameObject, delegate
			{
				this.emphasizeUI.SetActiveCollider(true);
				this.emphasizeUI.SetPushedEvent(addPushEvent);
			});
		}
	}

	public void SetPushUI(string uiType, TutorialUI tutorialUI, Action onPushedAction)
	{
		this.emphasizeUI = this.FindEmphasizeUI(uiType);
		if (null != this.emphasizeUI)
		{
			this.emphasizeUI.SetEmphasizeAfterWindowOpened(tutorialUI.transform, delegate
			{
				tutorialUI.SetTargetUI(true, TutorialTarget.ArrowPositon.TOP, this.emphasizeUI.gameObject, delegate
				{
					this.emphasizeUI.SetPushedEvent(onPushedAction);
				});
			});
		}
	}

	public void SetPopUI(string uiType, int arrowPosition, TutorialUI tutorialUI, Action completed)
	{
		this.emphasizeUI = this.FindEmphasizeUI(uiType);
		if (null != this.emphasizeUI)
		{
			tutorialUI.SetTargetUI(false, (TutorialTarget.ArrowPositon)arrowPosition, this.emphasizeUI.gameObject, delegate
			{
				this.emphasizeUI.SetEmphasize(tutorialUI.transform);
				if (completed != null)
				{
					completed();
				}
			});
		}
	}

	public void ResetEmphasizeUI(TutorialUI tutorialUI, Action completed)
	{
		tutorialUI.ClearTargetUI(completed);
		if (null != this.emphasizeUI)
		{
			this.emphasizeUI.ResetEmphasize();
			TutorialEmphasizeUI.UiNameType uiName = this.emphasizeUI.UiName;
			switch (uiName)
			{
			case TutorialEmphasizeUI.UiNameType.MEAL_PLUS:
			{
				GUICollider component = this.emphasizeUI.GetComponent<GUICollider>();
				component.activeCollider = false;
				goto IL_160;
			}
			case TutorialEmphasizeUI.UiNameType.PARTY:
			{
				BoxCollider[] componentsInChildren = this.emphasizeUI.GetComponentsInChildren<BoxCollider>();
				if (componentsInChildren != null)
				{
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						componentsInChildren[i].enabled = true;
					}
				}
				UnityEngine.Object.Destroy(this.emphasizeUI);
				goto IL_160;
			}
			case TutorialEmphasizeUI.UiNameType.RECOVERY:
			{
				HoldPressButton component2 = this.emphasizeUI.GetComponent<HoldPressButton>();
				component2.enabled = true;
				UnityEngine.Object.Destroy(this.emphasizeUI);
				goto IL_160;
			}
			case TutorialEmphasizeUI.UiNameType.RECOVERY_NO:
			case TutorialEmphasizeUI.UiNameType.ATTACK:
			case TutorialEmphasizeUI.UiNameType.RECOVERY_YES:
				break;
			default:
				if (uiName == TutorialEmphasizeUI.UiNameType.SKILL)
				{
					HoldPressButton component3 = this.emphasizeUI.GetComponent<HoldPressButton>();
					component3.enabled = true;
					UnityEngine.Object.Destroy(this.emphasizeUI);
					goto IL_160;
				}
				if (uiName != TutorialEmphasizeUI.UiNameType.OPERATION)
				{
					goto IL_160;
				}
				break;
			case TutorialEmphasizeUI.UiNameType.DIGISTONE:
			{
				BoxCollider componentInChildren = this.emphasizeUI.GetComponentInChildren<BoxCollider>();
				componentInChildren.enabled = true;
				goto IL_160;
			}
			case TutorialEmphasizeUI.UiNameType.SKILL_HOLD:
				UnityEngine.Object.Destroy(this.emphasizeUI);
				goto IL_160;
			}
			UnityEngine.Object.Destroy(this.emphasizeUI);
			IL_160:
			this.emphasizeUI = null;
		}
	}

	public void CloseAllUI(Action onCompleted)
	{
		GUIManager.CloseAllCommonDialog(delegate(int x)
		{
			if (onCompleted != null)
			{
				onCompleted();
			}
		});
	}

	public IEnumerator MoveFarmCamera(int posGridX, int posGridY, float time, Action completed)
	{
		FarmRoot farmRoot = FarmRoot.Instance;
		if (null != farmRoot)
		{
			FarmGrid farmGrid = farmRoot.Field.Grid;
			int gridIndex = farmGrid.GetGridIndex(posGridX, posGridY);
			Vector3 position = farmGrid.GetPositionGridCenter(gridIndex, false);
			position.y -= farmRoot.gameObject.transform.localPosition.y;
			GUICameraControll farmCamera = farmRoot.Camera.GetComponent<GUICameraControll>();
			if (null != farmCamera)
			{
				yield return base.StartCoroutine(farmCamera.MoveCameraToLookAtPoint(position, time));
			}
		}
		if (completed != null)
		{
			completed();
		}
		yield break;
	}

	public IEnumerator BuildFacility(int facilityId, int posX, int posY, bool serverRequest, Action<int> completed)
	{
		FarmRoot farmRoot = FarmRoot.Instance;
		if (null != farmRoot)
		{
			FarmField farmField = farmRoot.Field;
			FarmGrid farmGrid = farmField.Grid;
			FarmScenery farmScenery = farmRoot.Scenery;
			FarmObjectSetting farmObjectSetting = farmRoot.SettingObject;
			if (farmScenery.BuildFarmObject(facilityId))
			{
				FarmObject farmObject = farmObjectSetting.farmObject;
				FarmGrid.GridPosition gridPosition = new FarmGrid.GridPosition
				{
					x = posX,
					y = posY
				};
				int gridIndex = farmGrid.GetGridIndex(gridPosition);
				Vector3 gridPosition3D = farmGrid.GetPositionGridCenter(gridIndex, false);
				farmObject.SetPosition(farmField.gridHorizontal, farmField.gridVertical, gridPosition3D);
				farmObject.DisplayedInFront(true);
				if (serverRequest)
				{
					RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
					APIRequestTask task = farmScenery.SaveFarmObjectPosition(completed);
					yield return base.StartCoroutine(task.Run(new Action(RestrictionInput.EndLoad), null, null));
				}
				else
				{
					yield return null;
					farmScenery.SaveResponseToFacilityBuild(0);
					FarmObject_MeatFarm meatFarm = farmObject.GetComponent<FarmObject_MeatFarm>();
					meatFarm.IsDummyFacility = true;
					FacilityM masterFacility = FarmDataManager.GetFacilityMaster(facilityId);
					if ("0" != masterFacility.buildingAssetNum1)
					{
						FarmUtility.PayCost(masterFacility.buildingAssetCategoryId1, "-" + masterFacility.buildingAssetNum1);
					}
					if (completed != null)
					{
						completed(0);
					}
				}
			}
		}
		yield break;
	}

	public void SetFacilityBuildDummyTime(int userFacilityID, int buildingTime)
	{
		FarmRoot instance = FarmRoot.Instance;
		if (null != instance)
		{
			UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(userFacilityID);
			ConstructionDetail constructionDetail = instance.farmUI.GetConstructionDetail(userFacilityID);
			if (null == constructionDetail)
			{
				FarmScenery scenery = instance.Scenery;
				scenery.StartConstruction(userFacilityID);
				userFacility.level = 0;
				constructionDetail = instance.farmUI.GetConstructionDetail(userFacilityID);
			}
			if (0 < buildingTime)
			{
				constructionDetail.DisableTimeCountDown(buildingTime);
			}
			userFacility.completeTime = FarmUtility.GetDateString(ServerDateTime.Now.AddSeconds((double)buildingTime));
		}
	}

	public IEnumerator TargetFacility(int id, bool popEnable, Action completed, float adjustY)
	{
		FarmRoot farmRoot = FarmRoot.Instance;
		if (null != farmRoot)
		{
			FarmScenery farmScenery = farmRoot.Scenery;
			FarmObject farmObject = farmScenery.farmObjects.SingleOrDefault((FarmObject x) => x.facilityID == id);
			if (null != farmObject)
			{
				if (popEnable)
				{
					IEnumerator createPopIe = farmObject.CreatePop(FarmObjectPop.PopType.ARROW, farmRoot.Camera.transform, adjustY);
					yield return base.StartCoroutine(createPopIe);
					if (completed != null)
					{
						completed();
					}
				}
				else
				{
					farmObject.DeletePop();
					completed();
				}
			}
		}
		yield break;
	}

	public void StartSoundVolumeFade(bool fadein, float time)
	{
		if (!this.soundInfo.running)
		{
			this.soundInfo.duration = time;
			this.soundInfo.elapsedTime = 0f;
			if (!fadein)
			{
				this.soundInfo.originalVolumeSE = SoundMng.Instance().VolumeSE;
				this.soundInfo.originalVolumeBGM = SoundMng.Instance().VolumeBGM;
			}
			this.soundInfo.running = true;
			base.StartCoroutine(this.SoundVolumeFade(fadein));
		}
	}

	private IEnumerator SoundVolumeFade(bool fadein)
	{
		float fromVolumeSE = this.soundInfo.originalVolumeSE;
		float fromVolumeBGM = this.soundInfo.originalVolumeBGM;
		float toVolumeSE = 0f;
		float toVolumeBGM = 0f;
		if (fadein)
		{
			fromVolumeSE = 0f;
			fromVolumeBGM = 0f;
			toVolumeSE = this.soundInfo.originalVolumeSE;
			toVolumeBGM = this.soundInfo.originalVolumeBGM;
		}
		while (this.soundInfo.elapsedTime < this.soundInfo.duration)
		{
			this.soundInfo.elapsedTime = this.soundInfo.elapsedTime + Time.deltaTime;
			float rate = this.soundInfo.elapsedTime / this.soundInfo.duration;
			SoundMng.Instance().VolumeSE = (float)Mathf.FloorToInt(Mathf.Lerp(fromVolumeSE, toVolumeSE, rate));
			SoundMng.Instance().VolumeBGM = (float)Mathf.FloorToInt(Mathf.Lerp(fromVolumeBGM, toVolumeBGM, rate));
			yield return null;
		}
		if (fadein)
		{
			SoundMng.Instance().VolumeSE = this.soundInfo.originalVolumeSE;
			SoundMng.Instance().VolumeBGM = this.soundInfo.originalVolumeBGM;
		}
		else
		{
			SoundMng.Instance().VolumeSE = 0f;
			SoundMng.Instance().VolumeBGM = 0f;
		}
		this.soundInfo.running = false;
		yield break;
	}

	public void SetBgm(string fileName, bool play, float fadeTime)
	{
		if (play)
		{
			SoundMng.Instance().PlayBGM(fileName, fadeTime, null);
		}
		else
		{
			SoundMng.Instance().StopBGM(fadeTime, null);
		}
	}

	public void SetSe(string fileName, bool play, float fadeTime, bool loop, float pitch)
	{
		if (play)
		{
			SoundMng.Instance().PlaySE(fileName, fadeTime, loop, true, null, -1, pitch);
		}
		else
		{
			SoundMng.Instance().StopSE(fileName, fadeTime, null);
		}
	}

	public void PlayMovie(Action completed)
	{
		TutorialMovie component = base.GetComponent<TutorialMovie>();
		component.actionFinishedMovie = completed;
		NpMovie.TouchFinish = false;
		NpMovie.SoundEnable = true;
		NpMovie.ControllerEnabled = false;
		string path = ConstValue.APP_ASSET_DOMAIN + "/asset/Movie/OPPV.mp4";
		NpMovie.PlayStreaming(path, base.gameObject, component);
	}

	public IEnumerator DrawDigimon(int monsterGroupID, float scale, Vector2 adjustPosition, Action completed)
	{
		MonsterClientMaster monsterMaster = MonsterMaster.GetMonsterMasterByMonsterGroupId(monsterGroupID);
		string path = MonsterObject.GetFilePath(monsterMaster.Group.modelId);
		GameObject resource = AssetDataMng.Instance().LoadObject(path, null, true) as GameObject;
		yield return null;
		Camera uiCamera = Singleton<GUIMain>.Instance.GetComponent<Camera>();
		Vector3 cemeraPosition = uiCamera.transform.position;
		this.digimon = UnityEngine.Object.Instantiate<GameObject>(resource);
		this.digimon.transform.localScale = new Vector3(scale, scale, scale);
		this.digimon.transform.localPosition = cemeraPosition + new Vector3(adjustPosition.x, adjustPosition.y, -2f);
		this.digimon.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
		CharacterParams characterParams = this.digimon.GetComponent<CharacterParams>();
		characterParams.PlayIdleAnimation();
		Light light = this.digimon.AddComponent<Light>();
		light.type = LightType.Directional;
		yield return null;
		resource = null;
		Resources.UnloadUnusedAssets();
		if (completed != null)
		{
			completed();
		}
		yield break;
	}

	public void DeleteDigimon()
	{
		if (null != this.digimon)
		{
			UnityEngine.Object.Destroy(this.digimon);
			this.digimon = null;
		}
	}

	public void StartEffect(int type, Action completed)
	{
		switch (type)
		{
		case 0:
			base.StartCoroutine(this.LoadPrefab<TutorialSiren>("Tutorial/TutorialSiren", true, Singleton<GUIMain>.Instance.transform, delegate(TutorialSiren siren)
			{
				siren.name = TutorialControlToGame.ScreenEffectType.CIRCLE_FADE.ToString();
				this.effectObjects.Add(siren.gameObject);
				if (completed != null)
				{
					completed();
				}
			}));
			break;
		case 1:
			base.StartCoroutine(this.LoadPrefab<Transform>("Tutorial/TutorialConnecting", true, Singleton<GUIMain>.Instance.transform, delegate(Transform connect)
			{
				connect.name = TutorialControlToGame.ScreenEffectType.CONNECTING.ToString();
				this.effectObjects.Add(connect.gameObject);
				if (completed != null)
				{
					completed();
				}
			}));
			break;
		case 2:
			base.StartCoroutine(this.LoadPrefab<TutorialShutdown>("Tutorial/TutorialShutdown", false, Singleton<GUIMain>.Instance.transform, delegate(TutorialShutdown shutdown)
			{
				shutdown.name = TutorialControlToGame.ScreenEffectType.SHUT_DOWN.ToString();
				this.effectObjects.Add(shutdown.gameObject);
				shutdown.OnFinishedAnimation = completed;
				shutdown.gameObject.SetActive(true);
			}));
			break;
		}
	}

	public void StopEffect(int type)
	{
		for (int i = 0; i < this.effectObjects.Count; i++)
		{
			GameObject gameObject = this.effectObjects[i];
			if (null != gameObject)
			{
				string text = ((TutorialControlToGame.ScreenEffectType)type).ToString();
				if (!string.IsNullOrEmpty(text) && text == gameObject.name)
				{
					this.effectObjects.Remove(gameObject);
					UnityEngine.Object.Destroy(gameObject);
				}
			}
		}
	}

	public void SetAutoBattleFlag()
	{
		BattleTutorial tutorial = UnityEngine.Object.FindObjectOfType<BattleStateManager>().tutorial;
		tutorial.SetAutoPlay(2);
	}

	public void SetPlayerMeatCount(int meatNum)
	{
		DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.meatNum = meatNum.ToString();
		GUIPlayerStatus.RefreshParams_S(false);
		CMD_MealExecution cmd_MealExecution = UnityEngine.Object.FindObjectOfType<CMD_MealExecution>();
		if (null != cmd_MealExecution)
		{
			cmd_MealExecution.SetMeatNum(meatNum);
		}
	}

	public void SetPlayerDigiStoneCount(int digiStoneNum)
	{
		DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point = digiStoneNum;
		GUIPlayerStatus.RefreshParams_S(false);
	}

	public void SetPlayerLinkPointCount(int linkPointNum)
	{
		DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.friendPoint = linkPointNum.ToString();
	}

	public void SetDigimonExp(int index, int setLevel, int setExp)
	{
		string level = setLevel.ToString();
		GameWebAPI.RespDataMA_GetMonsterExperienceM.MonsterExperienceM[] monsterExperienceM = MasterDataMng.Instance().RespDataMA_MonsterExperienceM.monsterExperienceM;
		GameWebAPI.RespDataMA_GetMonsterExperienceM.MonsterExperienceM monsterExperienceM2 = monsterExperienceM.SingleOrDefault((GameWebAPI.RespDataMA_GetMonsterExperienceM.MonsterExperienceM x) => x.level == level);
		int num = int.Parse(monsterExperienceM2.experienceNum) + setExp;
		string nextLevel = (setLevel + 1).ToString();
		monsterExperienceM2 = monsterExperienceM.SingleOrDefault((GameWebAPI.RespDataMA_GetMonsterExperienceM.MonsterExperienceM x) => x.level == nextLevel);
		string experienceNum = monsterExperienceM2.experienceNum;
		List<MonsterData> monsterDataList = MonsterDataMng.Instance().GetMonsterDataList();
		MonsterSort.SortMonsterUserDataList(monsterDataList, MonsterSortType.DATE, MonsterSortOrder.DESC);
		int num2 = monsterDataList.Count - index - 1;
		if (monsterDataList != null && num2 < monsterDataList.Count && monsterDataList[num2] != null)
		{
			MonsterData monsterData = monsterDataList[num2];
			monsterData.userMonster.level = level;
			monsterData.userMonster.ex = num.ToString();
			monsterData.userMonster.levelEx = setExp.ToString();
			monsterData.userMonster.nextLevelEx = experienceNum;
			StatusValue statusValue = MonsterStatusData.GetStatusValue(monsterData.userMonster.monsterId, monsterData.userMonster.level);
			monsterData.userMonster.hp = statusValue.hp.ToString();
			monsterData.userMonster.attack = statusValue.attack.ToString();
			monsterData.userMonster.defense = statusValue.defense.ToString();
			monsterData.userMonster.spAttack = statusValue.magicAttack.ToString();
			monsterData.userMonster.spDefense = statusValue.magicDefense.ToString();
		}
	}

	public void SetMealUI_NotServerRequest()
	{
		CMD_MealExecution cmd_MealExecution = UnityEngine.Object.FindObjectOfType<CMD_MealExecution>();
		cmd_MealExecution.DontExec = true;
	}

	public void OpenCommonDialog(string openDialogName, Action actionOpened)
	{
		if ("CMD_BaseSelect" == openDialogName)
		{
			CMD_BaseSelect.BaseType = CMD_BaseSelect.BASE_TYPE.MEAL;
		}
		if ("CMD_MealExecution" == openDialogName)
		{
			List<MonsterData> monsterDataList = MonsterDataMng.Instance().GetMonsterDataList();
			MonsterSort.SortMonsterUserDataList(monsterDataList, MonsterSortType.DATE, MonsterSortOrder.DESC);
			CMD_MealExecution.DataChg = monsterDataList.Last<MonsterData>();
		}
		if ("CMD_CharacterDetailed" == openDialogName)
		{
			List<MonsterData> monsterDataList2 = MonsterDataMng.Instance().GetMonsterDataList();
			MonsterSort.SortMonsterUserDataList(monsterDataList2, MonsterSortType.DATE, MonsterSortOrder.DESC);
			CMD_CharacterDetailed.DataChg = monsterDataList2.First<MonsterData>();
		}
		CommonDialog commonDialog = GUIMain.ShowCommonDialog(null, openDialogName);
		commonDialog.onOpened += actionOpened;
	}

	public void OpenConfirmDialog(string openDialogName, Action actionOpened)
	{
		if ("GashaConfirmDialog" == openDialogName)
		{
			CMD_GashaTOP cmd_GashaTOP = UnityEngine.Object.FindObjectOfType<CMD_GashaTOP>();
			if (null != cmd_GashaTOP)
			{
				cmd_GashaTOP.OnClickedSingle();
			}
		}
	}

	public void StartDownload(string level, Action completed)
	{
		if (!string.IsNullOrEmpty(level))
		{
			CMD_ShortDownload progressUI = GUIMain.ShowCommonDialog(null, "CMD_ShortDownload") as CMD_ShortDownload;
			progressUI.SetOnOpened(delegate(int x)
			{
				this.StartCoroutine(this.WaitDownloaded(level, progressUI, completed));
			});
		}
		else if (completed != null)
		{
			completed();
		}
	}

	public void StartBackgroundDownload(string level, Action completed, int downloadStream = 4)
	{
		Action action = delegate()
		{
			this.isBackgroundDownload = false;
			if (completed != null)
			{
				completed();
			}
		};
		if (!string.IsNullOrEmpty(level))
		{
			this.isBackgroundDownload = true;
			base.StartCoroutine(this.IE_StartBackgroundDownload(level, action, downloadStream));
		}
		else
		{
			action();
		}
	}

	private IEnumerator IE_StartBackgroundDownload(string level, Action completed, int downloadStream = 4)
	{
		bool result = AssetDataMng.Instance().AB_StartDownLoad(level, downloadStream);
		if (result)
		{
			while (this.isBackgroundDownload && AssetDataMng.Instance().IsAssetBundleDownloading())
			{
				yield return null;
			}
		}
		if (this.isBackgroundDownload && completed != null)
		{
			completed();
		}
		yield break;
	}

	public void WaitBackgroundDownload(Action completed)
	{
		base.StartCoroutine(this.IE_WaitBackgroundDownload(completed));
	}

	private IEnumerator IE_WaitBackgroundDownload(Action completed)
	{
		this.isBackgroundDownload = false;
		CMD_ShortDownload progressUI = GUIMain.ShowCommonDialog(null, "CMD_ShortDownload") as CMD_ShortDownload;
		yield return base.StartCoroutine(progressUI.WaitAssetBundleDownload());
		progressUI.SetCloseAction(delegate(int x)
		{
			if (completed != null)
			{
				completed();
			}
		});
		progressUI.ClosePanel(true);
		yield break;
	}

	public void StartStandardDownload()
	{
		base.StartCoroutine(this.IE_StartStandardDownload());
	}

	private IEnumerator IE_StartStandardDownload()
	{
		AssetDataMng assetDataMng = AssetDataMng.Instance();
		int count = assetDataMng.GetDownloadAssetBundleCount(string.Empty);
		assetDataMng.StartDownloadAssetBundle(count, 4);
		this.isStandardDownload = true;
		while (assetDataMng.IsAssetBundleDownloading())
		{
			yield return null;
		}
		this.isStandardDownload = false;
		yield break;
	}

	private IEnumerator WaitDownloaded(string level, CMD_ShortDownload progressUI, Action completed)
	{
		bool result = AssetDataMng.Instance().AB_StartDownLoad(level, 4);
		if (result)
		{
			yield return base.StartCoroutine(progressUI.WaitAssetBundleDownload());
			progressUI.SetCloseAction(delegate(int x)
			{
				if (completed != null)
				{
					completed();
				}
			});
			progressUI.ClosePanel(true);
		}
		else
		{
			AlertManager.ShowAlertDialog(delegate(int nop)
			{
				GUIMain.BackToTOP("UIStartupCaution", 0.8f, 0.8f);
			}, "LOCAL_ERROR_JSONPARSE");
		}
		yield break;
	}

	public void SaveFinishFirstTutorial(Action completed)
	{
		GUIMain.ReqScreen("UIAssetBundleDownLoad", string.Empty);
		if (completed != null)
		{
			completed();
		}
	}

	private void OnDestroy()
	{
		for (int i = 0; i < Enum.GetNames(typeof(TutorialControlToGame.ScreenEffectType)).Length; i++)
		{
			this.StopEffect(i);
		}
	}

	public IEnumerator WaitOpenFacilityShop(Action onOpened)
	{
		CMD_FacilityShop window = UnityEngine.Object.FindObjectOfType<CMD_FacilityShop>();
		while (null == window)
		{
			yield return null;
			window = UnityEngine.Object.FindObjectOfType<CMD_FacilityShop>();
		}
		if (onOpened != null)
		{
			window.onOpened += onOpened;
		}
		yield break;
	}

	public IEnumerator WaitOpenMission(Action onOpened)
	{
		CMD_Mission window = UnityEngine.Object.FindObjectOfType<CMD_Mission>();
		while (null == window)
		{
			yield return null;
			window = UnityEngine.Object.FindObjectOfType<CMD_Mission>();
		}
		if (onOpened != null)
		{
			window.onOpened += onOpened;
		}
		yield break;
	}

	public void LocalDigimonEvolution()
	{
		MonsterData oldestMonster = ClassSingleton<MonsterUserDataMng>.Instance.GetOldestMonster();
		string monsterEvolutionRouteId = oldestMonster.userMonster.monsterEvolutionRouteId;
		GameWebAPI.RespDataMA_GetMonsterEvolutionRouteM respDataMA_MonsterEvolutionRouteM = MasterDataMng.Instance().RespDataMA_MonsterEvolutionRouteM;
		for (int i = 0; i < respDataMA_MonsterEvolutionRouteM.monsterEvolutionRouteM.Length; i++)
		{
			if (monsterEvolutionRouteId == respDataMA_MonsterEvolutionRouteM.monsterEvolutionRouteM[i].monsterEvolutionRouteId)
			{
				string growthMonsterId = respDataMA_MonsterEvolutionRouteM.monsterEvolutionRouteM[i].growthMonsterId;
				if (oldestMonster.userMonster.monsterId != growthMonsterId)
				{
					MonsterData monsterData = MonsterDataMng.Instance().CreateMonsterDataByMID(growthMonsterId);
					monsterData.userMonster.userId = oldestMonster.userMonster.userId;
					monsterData.userMonster.userMonsterId = oldestMonster.userMonster.userMonsterId;
					monsterData.userMonster.leaderSkillId = oldestMonster.userMonster.leaderSkillId;
					monsterData.userMonster.commonSkillId = oldestMonster.userMonster.commonSkillId;
					monsterData.userMonster.defaultSkillGroupSubId = oldestMonster.userMonster.defaultSkillGroupSubId;
					ClassSingleton<MonsterUserDataMng>.Instance.UpdateUserMonsterData(monsterData.userMonster);
					ClassSingleton<GUIMonsterIconList>.Instance.RefreshList(MonsterDataMng.Instance().GetMonsterDataList());
					oldestMonster.userMonster.ex = "0";
					oldestMonster.userMonster.hpAbilityFlg = "0";
					oldestMonster.userMonster.attackAbilityFlg = "0";
					oldestMonster.userMonster.defenseAbilityFlg = "0";
					oldestMonster.userMonster.spAttackAbilityFlg = "0";
					oldestMonster.userMonster.spDefenseAbilityFlg = "0";
					oldestMonster.userMonster.speedAbilityFlg = "0";
					oldestMonster.userMonster.friendship = "0";
					StatusValue statusValue = MonsterStatusData.GetStatusValue(oldestMonster.userMonster.monsterId, oldestMonster.userMonster.level);
					statusValue.luck = 1;
					oldestMonster.SetStatus(statusValue);
				}
				break;
			}
		}
	}

	private enum SceneType
	{
		BLACK,
		TUTORIAL,
		WARP,
		WORLD,
		WORLD_COLLAPSE,
		BATTLE,
		FARM_TUTORIAL,
		WHITE
	}

	private struct SoundVolumeFadeInfo
	{
		public float originalVolumeSE;

		public int originalVolumeVC;

		public float originalVolumeBGM;

		public float duration;

		public float elapsedTime;

		public bool running;
	}

	public enum ScreenEffectType
	{
		CIRCLE_FADE,
		CONNECTING,
		SHUT_DOWN
	}

	public enum WaitOpenDetailWindowType
	{
		GASHA_DETAIL_UI,
		EVOLUTION_DETAIL_UI
	}
}
