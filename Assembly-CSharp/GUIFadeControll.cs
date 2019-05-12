using System;
using UnityEngine;

public class GUIFadeControll : MonoBehaviour
{
	private static string APLoadName = string.Empty;

	private static string LoadGUIName = string.Empty;

	private static string HideGUIName = string.Empty;

	private static Action<int> fadeAction;

	private static float foutTime = 0.4f;

	private static float waitTime = 0.05f;

	private static float finTime = 0.4f;

	private static float alphaMax = 1f;

	private static bool fadeEnd = true;

	private static float alphaScale = 1f;

	private static Action<int> fadeEndAction;

	private static bool isExecuteFadeIn;

	private bool isLoaded;

	private static bool actionStop;

	private GameObject goScreen;

	private UISprite sprScreenNG;

	private bool fadeEfcStart_;

	private EffectBase fadeEfcBase_ = new EffectBase();

	private Color EfcColorW = new Color(1f, 1f, 1f, 1f);

	private bool isBarrier;

	public static bool FadeEnd
	{
		get
		{
			return GUIFadeControll.fadeEnd;
		}
	}

	public static void SetLoadInfo(Action<int> action_ = null, string APLoadName_ = "", string LoadGUIName_ = "", string HideGUIName_ = "", Action<int> actionEnd_ = null, bool executeFadeIn = false)
	{
		GUIFadeControll.APLoadName = APLoadName_;
		GUIFadeControll.LoadGUIName = LoadGUIName_;
		GUIFadeControll.HideGUIName = HideGUIName_;
		GUIFadeControll.fadeAction = action_;
		GUIFadeControll.fadeEndAction = actionEnd_;
		GUIFadeControll.isExecuteFadeIn = executeFadeIn;
	}

	public static void SetFadeInfo(float foutTime_, float waitTime_, float finTime_, float alphaMax_)
	{
		GUIFadeControll.foutTime = foutTime_;
		GUIFadeControll.waitTime = waitTime_;
		GUIFadeControll.finTime = finTime_;
		GUIFadeControll.alphaMax = alphaMax_;
		GUIFadeControll.alphaScale = 1f;
	}

	public static void ActionRestart()
	{
		GUIFadeControll.actionStop = false;
		GUIFadeControll.LoadGUIAll();
	}

	public static void StartFadeIn(float scale = 1f)
	{
		GUIFadeControll.actionStop = false;
		GUIFadeControll.alphaScale = scale;
	}

	private void Awake()
	{
		GUIFadeControll.fadeEnd = false;
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.name == "SCREEN")
			{
				this.goScreen = transform.gameObject;
				if (this.goScreen != null)
				{
					this.sprScreenNG = this.goScreen.GetComponent<UISprite>();
				}
			}
		}
		EFFECT_BASE_KEY_FRAME[] fadeKeys = EffectKeyFrame.GetFadeKeys(GUIFadeControll.foutTime, GUIFadeControll.waitTime, GUIFadeControll.finTime, GUIFadeControll.alphaMax);
		this.fadeEfcBase_.efSetKeyFrameTbl(fadeKeys);
		this.fadeEfcBase_.efSetLoopCt(1);
		this.fadeEfcBase_.efInit();
		this.fadeEfcBase_.efStop();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (GUIManager.BarrierReqToFade)
		{
			this.isBarrier = true;
			GUIManager.ShowBarrier();
		}
		else
		{
			this.isBarrier = false;
		}
		GUIFadeControll.actionStop = false;
	}

	private void Update()
	{
		if (!this.fadeEfcStart_)
		{
			this.fadeEfcBase_.efStart();
			this.fadeEfcStart_ = true;
		}
		if (!this.fadeEfcBase_.efIsEnd())
		{
			this.fadeEfcBase_.efTransform(this.goScreen.transform);
			EFFECT_BASE_KEY_FRAME curFrameValue = this.fadeEfcBase_.getCurFrameValue();
			this.EfcColorW.a = curFrameValue.col_a * GUIFadeControll.alphaScale;
			if (this.sprScreenNG != null)
			{
				this.sprScreenNG.color = this.EfcColorW;
			}
			if (!GUIFadeControll.actionStop)
			{
				this.fadeEfcBase_.efUpdate();
			}
			if (this.fadeEfcBase_.efGetCurKeyFrameIdx() == 1 && !this.isLoaded)
			{
				if (this.isBarrier)
				{
					GUIManager.HideBarrier();
				}
				if (!GUIFadeControll.isExecuteFadeIn)
				{
					if (GUIFadeControll.fadeAction != null)
					{
						GUIFadeControll.actionStop = true;
						GUIFadeControll.fadeAction(0);
					}
					else
					{
						GUIFadeControll.LoadGUIAll();
					}
				}
				else
				{
					GUIFadeControll.actionStop = true;
					if (GUIFadeControll.fadeAction != null)
					{
						GUIFadeControll.fadeAction(0);
					}
					GUIFadeControll.LoadGUIAll();
				}
				this.isLoaded = true;
			}
		}
		else
		{
			if (GUIFadeControll.fadeEndAction != null)
			{
				GUIFadeControll.fadeEndAction(0);
			}
			UnityEngine.Object.Destroy(base.gameObject);
			GUIFadeControll.fadeEnd = true;
		}
	}

	public static void LoadGUIAll()
	{
		if (GUIFadeControll.HideGUIName != string.Empty)
		{
			GUIManager.HideGUI(GUIFadeControll.HideGUIName);
		}
		if (GUIFadeControll.APLoadName != string.Empty)
		{
			Application.LoadLevel(GUIFadeControll.APLoadName);
		}
		if (GUIFadeControll.LoadGUIName != string.Empty)
		{
			GUIMain.ReqScreen(GUIFadeControll.LoadGUIName, string.Empty);
		}
	}

	private void DummyMethod()
	{
	}
}
