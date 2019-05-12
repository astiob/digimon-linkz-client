using Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIMain : Singleton<GUIMain>
{
	public static bool USE_NGUI = true;

	public static Camera gUICamera;

	public static Camera mainCamera;

	public static string gUIScreen = string.Empty;

	private static Stack<string> beforeGUIScreen = new Stack<string>();

	private static string nextGUIScreen = string.Empty;

	private static bool backMode_;

	private static Action onFadeBlackLoadScene;

	public GameObject goTXT_DBG00;

	public GameStringsFont gsfTXT_DBG00;

	private static float _verticalSpaceSize = 0f;

	private static UIRoot uiRoot;

	private static Camera __camera;

	private static UICamera uiCamera;

	[SerializeField]
	private GameObject goMainBarrier;

	private static Stack<float> mainBarrierPosZStack = new Stack<float>();

	private static bool isBarrierON = false;

	private static string nowBgm = string.Empty;

	private static Action actCallBackBattle;

	public event Action<string> onStartNewScreenEvent;

	protected static GUIMain self
	{
		get
		{
			return Singleton<GUIMain>.instance;
		}
		set
		{
			Singleton<GUIMain>.instance = value;
		}
	}

	public static bool backMode
	{
		get
		{
			return GUIMain.backMode_;
		}
	}

	public static Stack<string> BeforeGUIScreen
	{
		get
		{
			return GUIMain.beforeGUIScreen;
		}
	}

	public static void ForceReWriteScreenStack(List<string> screenList)
	{
		if (screenList.Count == 0)
		{
			return;
		}
		GUIMain.beforeGUIScreen = new Stack<string>();
		for (int i = screenList.Count - 1; i >= 0; i--)
		{
			GUIMain.beforeGUIScreen.Push(screenList[i]);
		}
	}

	public static float VerticalSpaceSize
	{
		get
		{
			return GUIMain._verticalSpaceSize;
		}
		set
		{
			GUIMain._verticalSpaceSize = value;
		}
	}

	public static UIRoot GetUIRoot()
	{
		return GUIMain.uiRoot;
	}

	public static Camera GetOrthoCamera()
	{
		return GUIMain.__camera;
	}

	public static UICamera GetUICamera()
	{
		return GUIMain.uiCamera;
	}

	private void Awake()
	{
		GUIMain.uiRoot = base.gameObject.transform.parent.gameObject.GetComponent<UIRoot>();
		GUIMain.uiCamera = base.gameObject.GetComponent<UICamera>();
		GUIMain.self = this;
		GameObject gameObject = GameObject.Find("GUI/Screen");
		if (gameObject != null)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
		GameObject gameObject2 = GameObject.Find("GUI Camera");
		if (gameObject2 != null)
		{
			gameObject2.SetActive(false);
			GUIMain.gUICamera = gameObject2.GetComponent<Camera>();
		}
		gameObject2 = GameObject.Find("Main Camera");
		if (gameObject2 != null)
		{
			GUIMain.mainCamera = gameObject2.GetComponent<Camera>();
		}
		gameObject2 = GameObject.Find("GUI");
		if (gameObject2 != null)
		{
			Camera component = gameObject2.GetComponent<Camera>();
			GUIMain.__camera = component;
			if (!GUIMain.USE_NGUI)
			{
				GUIMain.VerticalSpaceSize = 0f;
				float num = 1136f;
				float num2 = 640f;
				float num3 = (float)component.pixelWidth / (float)component.pixelHeight;
				float num4 = num / num2;
				float num5 = num3 / num4;
				if (num5 <= 1f)
				{
					component.orthographicSize /= num5;
				}
			}
			else
			{
				this.UpdateVSS();
			}
		}
	}

	private void Start()
	{
		Loading.Initialize(base.transform);
	}

	private void UpdateVSS()
	{
		float num = 1136f;
		float num2 = 640f;
		float num3 = (float)Screen.width;
		float num4 = (float)Screen.height;
		if (num4 > num3)
		{
			num4 = num3;
			num3 = num4;
		}
		float num5 = num3 / num4;
		float num6 = num / num2;
		float num7 = num6 / num5;
		if (num7 >= 1f)
		{
			float num8 = num4;
			num8 /= num3 / num;
			GUIMain.VerticalSpaceSize = (num8 - num2) / 2f;
		}
		else
		{
			GUIMain.VerticalSpaceSize = 0f;
		}
	}

	public static int dialogValue
	{
		get
		{
			return GUIManager.dialogValue;
		}
		set
		{
			GUIManager.dialogValue = value;
		}
	}

	public static CommonDialog ShowCommonDialog(Action<int> action, string dialogName)
	{
		return GUIManager.ShowCommonDialog(action, true, dialogName, null, null, 0.2f, 0f, 0f, -1f, -1f);
	}

	public static void BarrierReset()
	{
		Vector3 localPosition = GUIMain.self.goMainBarrier.transform.localPosition;
		localPosition.z = GUIManager.GetDLGStartZ() + 5f;
		GUIMain.self.goMainBarrier.transform.localPosition = localPosition;
		GUIMain.isBarrierON = false;
		GUIMain.self.goMainBarrier.SetActive(false);
		GUIMain.mainBarrierPosZStack = new Stack<float>();
	}

	public static void BarrierON(Action<int> act = null)
	{
		Vector3 localPosition = GUIMain.self.goMainBarrier.transform.localPosition;
		if (!GUIMain.isBarrierON)
		{
			GUIMain.isBarrierON = true;
			GUIMain.self.goMainBarrier.SetActive(true);
		}
		else
		{
			GUIMain.mainBarrierPosZStack.Push(localPosition.z);
		}
		CommonDialog topDialog = GUIManager.GetTopDialog(null, false);
		if (topDialog != null)
		{
			float num = GUIManager.GetDLGPitch() / 2f;
			localPosition.z = topDialog.gameObject.transform.localPosition.z + num;
		}
		else
		{
			localPosition.z = GUIManager.GetDLGStartZ() + 5f;
		}
		GUIMain.self.goMainBarrier.transform.localPosition = localPosition;
		if (act != null)
		{
			act(GUIMain.mainBarrierPosZStack.Count);
		}
	}

	public static void BarrierOFF()
	{
		Vector3 localPosition = GUIMain.self.goMainBarrier.transform.localPosition;
		if (GUIMain.isBarrierON)
		{
			if (GUIMain.mainBarrierPosZStack.Count > 0)
			{
				localPosition.z = GUIMain.mainBarrierPosZStack.Pop();
			}
			else
			{
				localPosition.z = GUIManager.GetDLGStartZ() + 5f;
				GUIMain.isBarrierON = false;
				GUIMain.self.goMainBarrier.SetActive(false);
			}
			GUIMain.self.goMainBarrier.transform.localPosition = localPosition;
		}
	}

	public static bool IsBarrierON()
	{
		return GUIMain.isBarrierON;
	}

	public static void SelectMainCamera()
	{
		if (GUIMain.mainCamera != null)
		{
			GUIMain.mainCamera.gameObject.SetActive(true);
		}
		if (GUIMain.gUICamera != null)
		{
			GUIMain.gUICamera.gameObject.SetActive(false);
		}
	}

	public static void SelectGUICamera()
	{
		if (GUIMain.mainCamera != null)
		{
			GUIMain.mainCamera.gameObject.SetActive(false);
		}
		if (GUIMain.gUICamera != null)
		{
			GUIMain.gUICamera.gameObject.SetActive(true);
		}
	}

	public static void ReqScreen(string screenName, string prefabName = "")
	{
		if (screenName == GUIMain.gUIScreen)
		{
			return;
		}
		string text = prefabName;
		string a = string.Empty;
		if (screenName == "UIColosseum")
		{
			a = "UIColosseum";
			text = (screenName = "UIHome");
		}
		if (string.IsNullOrEmpty(text))
		{
			text = screenName;
		}
		GUIMain.ReqScreenRaw(screenName, text);
		string b = string.Empty;
		string text2 = screenName;
		switch (text2)
		{
		case "UITitle":
			b = "bgm_101";
			goto IL_15B;
		case "UIHome":
			GUIMain.nowBgm = string.Empty;
			b = "bgm_201";
			goto IL_15B;
		case "UIShop":
			goto IL_15B;
		case "UIGame":
			if (GUIMain.nowBgm != string.Empty)
			{
				SoundMng.Instance().StopBGM(0.5f, null);
				GUIMain.nowBgm = string.Empty;
			}
			goto IL_15B;
		case "UIIdle":
			GUIMain.nowBgm = string.Empty;
			goto IL_15B;
		}
		b = GUIMain.nowBgm;
		IL_15B:
		if (a == "UIColosseum")
		{
			GUIMain.nowBgm = string.Empty;
			b = "bgm_203";
		}
		if (GUIMain.nowBgm != b)
		{
			GUIMain.nowBgm = b;
			string path = string.Empty;
			if (screenName == "UITitle")
			{
				path = "BGMInternal/" + GUIMain.nowBgm + "/sound";
			}
			else
			{
				path = "BGM/" + GUIMain.nowBgm + "/sound";
			}
			SoundMng.Instance().PlayBGM(path, 0.3f, null);
		}
	}

	public static void ReqScreenRaw(string screenName, string prefabName)
	{
		if (!string.IsNullOrEmpty(screenName) && !string.IsNullOrEmpty(screenName))
		{
			GUIMain.nextGUIScreen = screenName;
			GUIManager.LoadGUI(screenName, prefabName);
			if (GUIMain.nextGUIScreen == "UIHome" || GUIMain.nextGUIScreen == "UI*****")
			{
				GUIMain.beforeGUIScreen = new Stack<string>();
			}
		}
	}

	public static void StartupScreen(string screen)
	{
		GUIMain.backMode_ = false;
		if (!string.IsNullOrEmpty(screen))
		{
			GUIMain.nextGUIScreen = screen;
			GUIManager.LoadGUI(GUIMain.nextGUIScreen, GUIMain.nextGUIScreen);
			GUIMain.gUIScreen = string.Empty;
		}
	}

	[SerializeField]
	private void Update()
	{
		this.UpdateVSS();
		if (GUIMain.nextGUIScreen != null && GUIMain.nextGUIScreen != string.Empty && GUIManager.ReadyGUI(GUIMain.nextGUIScreen))
		{
			if (GUIMain.gUIScreen != null && GUIMain.gUIScreen != string.Empty)
			{
				GUIManager.HideGUI(GUIMain.gUIScreen);
				if (!GUIMain.backMode_)
				{
					if (!(GUIMain.nextGUIScreen == "UIHome") && !(GUIMain.nextGUIScreen == "UI*****"))
					{
						if (!(GUIMain.gUIScreen == "UIRestart") && !(GUIMain.gUIScreen == "UI******"))
						{
							GUIMain.beforeGUIScreen.Push(GUIMain.gUIScreen);
						}
					}
				}
				GUIMain.backMode_ = false;
			}
			GUIMain.gUIScreen = GUIMain.nextGUIScreen;
			GUIManager.ShowGUI(GUIMain.nextGUIScreen);
			GUICollider.EnableAllCollider("GUIMain");
			GUIMain.nextGUIScreen = string.Empty;
		}
	}

	public static string GetNowGUIName()
	{
		return GUIMain.gUIScreen;
	}

	public static void FadeBlackReqScreen(string screen, Action<int> act = null, float outSec = 0.25f, float inSec = 0.25f, bool isFadeIn = false, Action<int> endAct = null)
	{
		GUICollider.DisableAllCollider("GUIMain");
		GUIFadeControll.SetLoadInfo(act, string.Empty, screen, string.Empty, endAct, isFadeIn);
		GUIFadeControll.SetFadeInfo(outSec, 0f, inSec, 1f);
		GUIManager.LoadCommonGUI("Effect/FADE_B", GUIMain.self.gameObject);
	}

	public static void FadeWhiteReqScreen(string screen, Action<int> act = null, float outSec = 0.8f, float inSec = 0.8f, bool isFadeIn = false)
	{
		GUICollider.DisableAllCollider("GUIMain");
		GUIFadeControll.SetLoadInfo(act, string.Empty, screen, string.Empty, null, isFadeIn);
		GUIFadeControll.SetFadeInfo(outSec, 0f, inSec, 1f);
		GUIManager.LoadCommonGUI("Effect/FADE_W", GUIMain.self.gameObject);
	}

	public static void DestroyAllDialog(CommonDialog cd = null)
	{
		MonsterDataMng.Instance().PushBackAllMonsterPrefab();
		Dictionary<string, CommonDialog> dialogDic = GUIManager.GetDialogDic();
		List<CommonDialog> list = new List<CommonDialog>();
		foreach (string key in dialogDic.Keys)
		{
			if (dialogDic[key] != cd)
			{
				list.Add(dialogDic[key]);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			GUIManager.DeleteCommonDialog(list[i]);
			UnityEngine.Object.DestroyImmediate(list[i].gameObject);
		}
		if (cd == null)
		{
			GUIManager.HideGUI("CommonDialogBarrier");
		}
		else
		{
			GUIManager.ShowBarrierZset(cd.gameObject.transform.localPosition.z + GUIManager.GetDLG_BARRIER_OFS_Z());
		}
	}

	public static void ResetBGM()
	{
		GUIMain.nowBgm = string.Empty;
	}

	public static void BackToTOP(string guiName = "UIStartupCaution", float outSec = 0.8f, float inSec = 0.8f)
	{
		ServerDateTime.isUpdateServerDateTime = false;
		GUIMain.self.StopAllCoroutines();
		if (null != ClassSingleton<FaceChatNotificationAccessor>.Instance.faceChatNotification)
		{
			ClassSingleton<FaceChatNotificationAccessor>.Instance.faceChatNotification.StopGetHistoryIdList();
		}
		MonsterDataMng.Instance().PushBackAllMonsterPrefab();
		MonsterDataMng.Instance().DestroyAllMonsterData();
		GUIMain.DestroyAllDialog(null);
		GUIMain.onFadeBlackLoadScene = null;
		FarmSceneryCache.ClearCache();
		TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
		if (tutorialObserver != null)
		{
			UnityEngine.Object.Destroy(tutorialObserver.gameObject);
		}
		TutorialUI tutorialUI = UnityEngine.Object.FindObjectOfType<TutorialUI>();
		if (tutorialUI != null)
		{
			if (tutorialUI.gameObject.name == "GUI")
			{
				UnityEngine.Object.Destroy(tutorialUI);
				string[] array = new string[]
				{
					"ROOT/Panel",
					"ROOT/HitIcon",
					"ROOT/HUD"
				};
				foreach (string name in array)
				{
					GameObject gameObject = GameObject.Find(name);
					if (gameObject != null)
					{
						UnityEngine.Object.Destroy(gameObject);
					}
				}
			}
			else
			{
				UnityEngine.Object.Destroy(tutorialUI.gameObject);
			}
		}
		AppCoroutine appCoroutine = UnityEngine.Object.FindObjectOfType<AppCoroutine>();
		if (appCoroutine != null)
		{
			UnityEngine.Object.Destroy(appCoroutine.gameObject);
		}
		GUIMain.BarrierReset();
		Singleton<GUIManager>.Instance.UseOutsideTouchControl = false;
		FarmRoot instance = FarmRoot.Instance;
		if (null != instance)
		{
			InputControll input = instance.Input;
			if (null != input)
			{
				input.enabled = false;
			}
		}
		ClassSingleton<QuestData>.Instance.ClearWorldAreaMList();
		FarmCameraControlForCMD.ClearRefCT();
		GUICollider.DisableAllCollider("GUIMain");
		SoundMng.Instance().StopBGM(0.5f, delegate(int n)
		{
			if (GUIMain.gUIScreen != guiName)
			{
				string guiName2 = guiName;
				Action<int> actionEnd_ = new Action<int>(GUIMain.actCallBackBackToTOP);
				GUIFadeControll.SetLoadInfo(null, "Empty", guiName2, string.Empty, actionEnd_, false);
				GUIFadeControll.SetFadeInfo(outSec, 0f, inSec, 1f);
				GUIManager.LoadCommonGUI("Effect/FADE_W", GUIMain.self.gameObject);
			}
		});
		GUIMain.nowBgm = string.Empty;
		Resources.UnloadUnusedAssets();
	}

	private static void actCallBackBackToTOP(int i)
	{
	}

	public static void OpenURLAndQuitAPP(string url)
	{
		Application.OpenURL(url);
		Application.Quit();
	}

	public static void FadeBlackLoadScene(string SceneName, float outSec = 0.5f, float inSec = 0.5f, bool destroyMonsterIcon = true, Action<int> actionEnd = null)
	{
		GUIMain.actCallBackBattle = null;
		if (destroyMonsterIcon)
		{
			MonsterDataMng.Instance().DestroyAllMonsterDataAndPrefab();
		}
		GUICollider.DisableAllCollider("GUIMain");
		GUIFadeControll.SetLoadInfo(new Action<int>(GUIMain.ShiftGUI), SceneName, "UIIdle", string.Empty, actionEnd, false);
		GUIFadeControll.SetFadeInfo(outSec, 0f, inSec, 1f);
		BattleStateManager.onAutoServerConnect = true;
		GUIManager.LoadCommonGUI("Effect/FADE_B", GUIMain.self.gameObject);
		Resources.UnloadUnusedAssets();
	}

	private static void ShiftGUI(int i)
	{
		if (GUIMain.onFadeBlackLoadScene != null)
		{
			GUIMain.onFadeBlackLoadScene();
			GUIMain.onFadeBlackLoadScene = null;
		}
		GUIFadeControll.ActionRestart();
	}

	public static void SetAction_FadeBlackLoadScene(Action action)
	{
		GUIMain.onFadeBlackLoadScene = (Action)Delegate.Combine(GUIMain.onFadeBlackLoadScene, action);
	}

	public static void SetBattleCallBack(Action actCallBack)
	{
		GUIMain.actCallBackBattle = actCallBack;
	}

	public static void FadeBlackReqFromScene(string screenName, float outSec = 0.5f, float inSec = 0.5f)
	{
		GUIMain.beforeGUIScreen = new Stack<string>();
		GUICollider.DisableAllCollider("GUIMain");
		Action actionReceived = null;
		string screenName2 = screenName;
		if (screenName2 != null)
		{
			if (GUIMain.<>f__switch$map1C == null)
			{
				GUIMain.<>f__switch$map1C = new Dictionary<string, int>(3)
				{
					{
						"UIResult",
						0
					},
					{
						"UIPvPResult",
						0
					},
					{
						"UIHome",
						1
					}
				};
			}
			int num;
			if (GUIMain.<>f__switch$map1C.TryGetValue(screenName2, out num))
			{
				if (num == 0)
				{
					actionReceived = delegate()
					{
						GUIFadeControll.ActionRestart();
						GUIMain.OnNewScreenStart(screenName);
					};
					goto IL_E0;
				}
				if (num != 1)
				{
				}
			}
		}
		inSec = 0f;
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		actionReceived = delegate()
		{
			Loading.DisableMask();
			TipsLoading.Instance.StartTipsLoad(CMD_Tips.DISPLAY_PLACE.TitleToFarm, true);
			GUIFadeControll.LoadGUIAll();
		};
		IL_E0:
		Time.timeScale = 1f;
		GUIFadeControll.SetLoadInfo(delegate(int x)
		{
			if (GUIMain.actCallBackBattle != null)
			{
				GUIMain.actCallBackBattle();
			}
			string screenName3 = screenName;
			if (screenName3 != null)
			{
				if (GUIMain.<>f__switch$map1E == null)
				{
					GUIMain.<>f__switch$map1E = new Dictionary<string, int>(1)
					{
						{
							"UIPvPResult",
							0
						}
					};
				}
				int num2;
				if (GUIMain.<>f__switch$map1E.TryGetValue(screenName3, out num2))
				{
					if (num2 == 0)
					{
						actionReceived();
						return;
					}
				}
			}
			Singleton<GUIMain>.instance.StartCoroutine(APIUtil.Instance().SendBattleResult(actionReceived));
		}, "Empty", screenName, string.Empty, null, false);
		GUIFadeControll.SetFadeInfo(outSec, 0f, inSec, 1f);
		GUIManager.LoadCommonGUI("Effect/FADE_B", GUIMain.self.gameObject);
		GUIMain.backMode_ = true;
	}

	private static IEnumerator SendBattleResult(Action remainingSceneChangeAction)
	{
		if (!Loading.IsShow())
		{
			Loading.Display(Loading.LoadingType.LARGE, false);
		}
		GameWebAPI.WorldResultLogic worldResultLogic = new GameWebAPI.WorldResultLogic();
		worldResultLogic.SetSendData = delegate(GameWebAPI.WD_Req_DngResult param)
		{
			param.startId = DataMng.Instance().WD_ReqDngResult.startId;
			param.dungeonId = DataMng.Instance().WD_ReqDngResult.dungeonId;
			param.clear = DataMng.Instance().WD_ReqDngResult.clear;
			int[] aliveInfo = DataMng.Instance().WD_ReqDngResult.aliveInfo;
			if (aliveInfo != null)
			{
				param.aliveInfo = new int[aliveInfo.Length];
				for (int i = 0; i < aliveInfo.Length; i++)
				{
					param.aliveInfo[i] = aliveInfo[i];
				}
			}
		};
		worldResultLogic.OnReceived = delegate(GameWebAPI.RespDataWD_DungeonResult response)
		{
			ClassSingleton<QuestData>.Instance.RespDataWD_DungeonResult = response;
		};
		GameWebAPI.WorldResultLogic request = worldResultLogic;
		return request.Run(delegate()
		{
			ClassSingleton<BattleDataStore>.Instance.DeleteForSystem();
			ClassSingleton<QuestData>.Instance.ClearDNGDataCache();
			if (remainingSceneChangeAction != null)
			{
				remainingSceneChangeAction();
			}
		}, delegate(Exception nop)
		{
			ClassSingleton<QuestData>.Instance.RespDataWD_DungeonResult = null;
			ClassSingleton<QuestData>.Instance.ClearDNGDataCache();
			if (remainingSceneChangeAction != null)
			{
				remainingSceneChangeAction();
			}
		}, null);
	}

	public static void FadeBlackReqFromSceneForMulti(int startId, string screenName, float outSec = 0.5f, float inSec = 0.5f)
	{
		GUIMain.beforeGUIScreen = new Stack<string>();
		GUICollider.DisableAllCollider("GUIMain");
		Action actionReceived = null;
		bool isResult = false;
		string screenName2 = screenName;
		if (screenName2 != null)
		{
			if (GUIMain.<>f__switch$map1F == null)
			{
				GUIMain.<>f__switch$map1F = new Dictionary<string, int>(3)
				{
					{
						"UIResult",
						0
					},
					{
						"UIPvPResult",
						0
					},
					{
						"UIHome",
						1
					}
				};
			}
			int num;
			if (GUIMain.<>f__switch$map1F.TryGetValue(screenName2, out num))
			{
				if (num == 0)
				{
					isResult = true;
					actionReceived = delegate()
					{
						GUIFadeControll.ActionRestart();
						GUIMain.OnNewScreenStart(screenName);
					};
					goto IL_F5;
				}
				if (num != 1)
				{
				}
			}
		}
		inSec = 0f;
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		actionReceived = delegate()
		{
			Loading.DisableMask();
			TipsLoading.Instance.StartTipsLoad(CMD_Tips.DISPLAY_PLACE.TitleToFarm, true);
			GUIFadeControll.LoadGUIAll();
		};
		IL_F5:
		Time.timeScale = 1f;
		GUIFadeControll.SetLoadInfo(delegate(int x)
		{
			if (GUIMain.actCallBackBattle != null)
			{
				GUIMain.actCallBackBattle();
			}
			if (isResult)
			{
				Singleton<GUIMain>.instance.StartCoroutine(APIUtil.Instance().SendBattleResultForMulti(actionReceived, startId));
			}
			else
			{
				if (actionReceived != null)
				{
					actionReceived();
				}
				ClassSingleton<QuestData>.Instance.ClearDNGDataCache();
			}
		}, "Empty", screenName, string.Empty, null, false);
		GUIFadeControll.SetFadeInfo(outSec, 0f, inSec, 1f);
		GUIManager.LoadCommonGUI("Effect/FADE_B", GUIMain.self.gameObject);
		GUIMain.backMode_ = true;
	}

	private static void OnNewScreenStart(string newScreenName)
	{
		if (GUIMain.self.onStartNewScreenEvent != null)
		{
			GUIMain.self.onStartNewScreenEvent(newScreenName);
		}
	}
}
