using Monster;
using MonsterIcon;
using Quest;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GUIMain : Singleton<GUIMain>
{
	public static bool USE_NGUI = true;

	public static string gUIScreen = string.Empty;

	private static Stack<string> beforeGUIScreen = new Stack<string>();

	private static string nextGUIScreen = string.Empty;

	private static bool backMode_;

	private static Action onFadeBlackLoadScene;

	private static UIRoot parentUIRoot;

	private static UIPanel parentUIPanel;

	[SerializeField]
	private Camera orthoCamera;

	[SerializeField]
	private UICamera uiCamera;

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
			float num = GUIMain.parentUIPanel.GetWindowSize().y - (float)GUIMain.parentUIRoot.manualHeight;
			if (0f < num)
			{
				num *= 0.5f;
			}
			else
			{
				num = 0f;
			}
			return num;
		}
	}

	public static UIRoot GetUIRoot()
	{
		return GUIMain.parentUIRoot;
	}

	public static UIPanel GetUIPanel()
	{
		return GUIMain.parentUIPanel;
	}

	public static Camera GetOrthoCamera()
	{
		return Singleton<GUIMain>.instance.orthoCamera;
	}

	public static UICamera GetUICamera()
	{
		return Singleton<GUIMain>.instance.uiCamera;
	}

	private void Awake()
	{
		GUIMain.parentUIRoot = base.transform.parent.gameObject.GetComponent<UIRoot>();
		GUIMain.parentUIPanel = GUIMain.parentUIRoot.GetComponent<UIPanel>();
		GUIMain.self = this;
	}

	private void Start()
	{
		Loading.Initialize(base.transform);
		GameObject gameObject = new GameObject("MonsterIconPartsPool");
		Transform transform = gameObject.transform;
		transform.parent = base.transform;
		transform.localScale = Vector3.one;
		transform.localPosition = new Vector3(2000f, 2000f, 0f);
		MonsterIconPartsPool partsPool = gameObject.AddComponent<MonsterIconPartsPool>();
		gameObject.SetActive(false);
		MonsterIconFactory.SetPartsPool(partsPool);
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

	public static CommonDialog ShowCommonDialog(Action<int> action, string dialogName, Action<CommonDialog> actionReady = null)
	{
		return GUIManager.ShowCommonDialog(action, dialogName, actionReady);
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
		if (null != topDialog)
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

	public static void AdjustBarrierZ()
	{
		Vector3 localPosition = GUIMain.self.goMainBarrier.transform.localPosition;
		CommonDialog topDialog = GUIManager.GetTopDialog(null, false);
		if (null != topDialog)
		{
			float num = GUIManager.GetDLGPitch() / 8f * 7f;
			localPosition.z = topDialog.gameObject.transform.localPosition.z + num;
		}
		else
		{
			localPosition.z = GUIManager.GetDLGStartZ() + 5f;
		}
		GUIMain.self.goMainBarrier.transform.localPosition = localPosition;
	}

	public static void BarrierOFF()
	{
		Vector3 localPosition = GUIMain.self.goMainBarrier.transform.localPosition;
		if (GUIMain.isBarrierON)
		{
			if (0 < GUIMain.mainBarrierPosZStack.Count)
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

	public static void ReqScreen(string screenName, string prefabName = "")
	{
		if (screenName == GUIMain.gUIScreen)
		{
			return;
		}
		string text = prefabName;
		string b = string.Empty;
		if ("UIColosseum" == screenName)
		{
			b = "UIColosseum";
			text = (screenName = "UIHome");
		}
		if (string.IsNullOrEmpty(text))
		{
			text = screenName;
		}
		GUIMain.ReqScreenRaw(screenName, text);
		string b2 = string.Empty;
		string text2 = screenName;
		switch (text2)
		{
		case "UITitle":
			b2 = "bgm_101";
			goto IL_156;
		case "UIHome":
			GUIMain.nowBgm = string.Empty;
			b2 = "bgm_201";
			goto IL_156;
		case "UIShop":
			goto IL_156;
		case "UIGame":
			if (!string.IsNullOrEmpty(GUIMain.nowBgm))
			{
				SoundMng.Instance().StopBGM(0.5f, null);
				GUIMain.nowBgm = string.Empty;
			}
			goto IL_156;
		case "UIIdle":
			GUIMain.nowBgm = string.Empty;
			goto IL_156;
		}
		b2 = GUIMain.nowBgm;
		IL_156:
		if ("UIColosseum" == b)
		{
			GUIMain.nowBgm = string.Empty;
			b2 = "bgm_203";
		}
		if (GUIMain.nowBgm != b2)
		{
			GUIMain.nowBgm = b2;
			string path = string.Empty;
			if ("UITitle" == screenName)
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
			if ("UIHome" == GUIMain.nextGUIScreen || "UI*****" == GUIMain.nextGUIScreen)
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
		if (GUIMain.nextGUIScreen != null && !string.IsNullOrEmpty(GUIMain.nextGUIScreen) && GUIManager.ReadyGUI(GUIMain.nextGUIScreen))
		{
			if (GUIMain.gUIScreen != null && !string.IsNullOrEmpty(GUIMain.gUIScreen))
			{
				GUIManager.HideGUI(GUIMain.gUIScreen);
				if (!GUIMain.backMode_)
				{
					if (!("UIHome" == GUIMain.nextGUIScreen) && !("UI*****" == GUIMain.nextGUIScreen))
					{
						if (!("UIRestart" == GUIMain.gUIScreen) && !("UI******" == GUIMain.gUIScreen))
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
		ClassSingleton<GUIMonsterIconList>.Instance.PushBackAllMonsterPrefab();
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
		if (null == cd)
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
		ClassSingleton<GUIMonsterIconList>.Instance.PushBackAllMonsterPrefab();
		ClassSingleton<MonsterUserDataMng>.Instance.Initialize();
		ClassSingleton<GUIMonsterIconList>.Instance.AllDelete();
		GUIMain.DestroyAllDialog(null);
		GUIMain.onFadeBlackLoadScene = null;
		FarmSceneryCache.ClearCache();
		TutorialObserver tutorialObserver = UnityEngine.Object.FindObjectOfType<TutorialObserver>();
		if (null != tutorialObserver)
		{
			UnityEngine.Object.Destroy(tutorialObserver.gameObject);
		}
		TutorialUI tutorialUI = UnityEngine.Object.FindObjectOfType<TutorialUI>();
		if (null != tutorialUI)
		{
			if ("GUI" == tutorialUI.gameObject.name)
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
					if (null != gameObject)
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
		if (null != appCoroutine)
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
				GUIFadeControll.SetLoadInfo(delegate(int x)
				{
					GUIFadeControll.ActionRestart();
				}, "Empty", guiName2, string.Empty, actionEnd_, false);
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
			ClassSingleton<GUIMonsterIconList>.Instance.AllDisable();
			ClassSingleton<GUIMonsterIconList>.Instance.AllDelete();
		}
		GUICollider.DisableAllCollider("GUIMain");
		GUIFadeControll.SetLoadInfo(new Action<int>(GUIMain.ShiftGUI), SceneName, "UIIdle", string.Empty, actionEnd, false);
		GUIFadeControll.SetFadeInfo(outSec, 0f, inSec, 1f);
		BattleStateManager.onAutoServerConnect = true;
		ServerDateTime.isUpdateServerDateTime = false;
		ClassSingleton<PlayLimit>.Instance.UseTicketNumCont();
		ClassSingleton<PlayLimit>.Instance.UsePlayLimitNumCont();
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
			if (GUIMain.<>f__switch$map1A == null)
			{
				GUIMain.<>f__switch$map1A = new Dictionary<string, int>(3)
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
			if (GUIMain.<>f__switch$map1A.TryGetValue(screenName2, out num))
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
				if (GUIMain.<>f__switch$map1C == null)
				{
					GUIMain.<>f__switch$map1C = new Dictionary<string, int>(1)
					{
						{
							"UIPvPResult",
							0
						}
					};
				}
				int num2;
				if (GUIMain.<>f__switch$map1C.TryGetValue(screenName3, out num2))
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

	public static void FadeBlackReqFromSceneForMulti(int startId, string screenName, float outSec = 0.5f, float inSec = 0.5f)
	{
		GUIMain.beforeGUIScreen = new Stack<string>();
		GUICollider.DisableAllCollider("GUIMain");
		Action actionReceived = null;
		bool isResult = false;
		string screenName2 = screenName;
		if (screenName2 != null)
		{
			if (GUIMain.<>f__switch$map1D == null)
			{
				GUIMain.<>f__switch$map1D = new Dictionary<string, int>(3)
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
			if (GUIMain.<>f__switch$map1D.TryGetValue(screenName2, out num))
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
