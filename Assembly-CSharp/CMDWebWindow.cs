using LitJson;
using Neptune.Movie;
using Neptune.OAuth;
using Neptune.WebView;
using Quest;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CMDWebWindow : CMD, INpWebViewListener
{
	public GameObject goTXT_TITLE;

	private GameStringsFont gsfTXT_TITLE;

	private float defaultBgmVolume;

	private bool isMute;

	private string prevBGMPath;

	private float webWindowOpenWait;

	private string titleText;

	public static CMDWebWindow instance;

	private bool isFullscreen;

	private static NpWebView webViewObject;

	private string _Url = string.Empty;

	public string TitleText
	{
		get
		{
			return this.titleText;
		}
		set
		{
			this.titleText = value;
		}
	}

	public Action callbackAction { get; set; }

	public bool IsFullscreen
	{
		get
		{
			return this.isFullscreen;
		}
		set
		{
			this.isFullscreen = value;
			PartsMenu.instance.Active(false);
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this.defaultBgmVolume = SoundMng.Instance().VolumeBGM;
		this.gsfTXT_TITLE = this.goTXT_TITLE.GetComponent<GameStringsFont>();
		AlertManager.alertOpenedAction = new Action(this.alertOpenedAction);
		CMDWebWindow.instance = this;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.Show(f, sizeX, sizeY, aT);
	}

	protected override void Update()
	{
		base.Update();
		this.gsfTXT_TITLE.text = this.titleText;
		this.webWindowOpenWait += Time.deltaTime;
	}

	protected override void WindowOpened()
	{
		if (this._Url != string.Empty)
		{
			this.StartWebView(null);
		}
	}

	protected override void OnDestroy()
	{
		AlertManager.alertOpenedAction = null;
		base.OnDestroy();
	}

	public override void ClosePanel(bool animation = true)
	{
		base.ClosePanel(animation);
		if (PartsMenu.instance != null)
		{
			PartsMenu.instance.Active(true);
		}
		if (this._Url != string.Empty)
		{
			this.CloseWebView();
		}
		if (this.isMute)
		{
			SoundMng.Instance().VolumeBGM = this.defaultBgmVolume;
			this.isMute = false;
		}
	}

	public override void ClosePanelNotEndShow(bool animation = true)
	{
		base.ClosePanelNotEndShow(animation);
		if (this._Url != string.Empty)
		{
			this.CloseWebView();
		}
	}

	public string Url
	{
		get
		{
			return this._Url;
		}
		set
		{
			this._Url = value;
		}
	}

	private void OnCloseWebAndStartHTTP(int i)
	{
		Application.OpenURL(this.Url);
	}

	private void StartWebView(string url = null)
	{
		List<int> webViewMargin = GUIManager.GetWebViewMargin(230f, -290f, -442f, 442f);
		CMDWebWindow.webViewObject = new NpWebView();
		CMDWebWindow.webViewObject.SetHardwareAccelerated(true);
		CMDWebWindow.webViewObject.SetNpWebViewListener(base.gameObject, this);
		GameObject gameObject = Singleton<GUIMain>.Instance.gameObject;
		if (gameObject != null)
		{
			Camera component = gameObject.GetComponent<Camera>();
			int num = component.pixelWidth - (webViewMargin[0] + webViewMargin[2]);
			int num2 = component.pixelHeight - (webViewMargin[1] + webViewMargin[3]);
			if (this.isFullscreen)
			{
				int width = Screen.width;
				int num3 = Screen.height - 64;
				CMDWebWindow.webViewObject.ReSize(0f, 74f, (float)width, (float)num3);
			}
			else
			{
				CMDWebWindow.webViewObject.ReSize((float)webViewMargin[0], (float)webViewMargin[1], (float)num, (float)num2);
			}
		}
		if (url != null)
		{
			this._Url = url;
		}
		this.WebViewOpen(this._Url);
	}

	private void WebViewOpen(string url)
	{
		string requestMethod = "GET";
		int num = url.IndexOf('?');
		if (num == -1)
		{
			url += "?disabledVC=0";
		}
		else
		{
			url += "&disabledVC=0";
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary = NpOAuth.Instance.RequestHeaderDic(requestMethod, url, null);
		dictionary["Content-Type"] = "application/x-www-form-urlencoded";
		dictionary["X-AppVer"] = WebAPIPlatformValue.GetAppVersion();
		CMDWebWindow.webViewObject.Open(url, dictionary, string.Empty);
	}

	public virtual void OnShouldOverrideUrlLoading(string url)
	{
	}

	public virtual void OnPageStarted(string url)
	{
	}

	public virtual void OnPageFinished(string url)
	{
		global::Debug.Log("ページのロードが完了しました");
	}

	public virtual void OnReceivedError(string url)
	{
		global::Debug.Log("正常に開けないため閉じます");
		this.ClosePanel(true);
	}

	public virtual void OnActionEvent(string jsonString)
	{
		if (this.webWindowOpenWait > 1f)
		{
			global::Debug.Log("#=#=# OnActionEvent:jsonString = " + jsonString);
			CMDWebWindow.WebParam param = JsonMapper.ToObject<CMDWebWindow.WebParam>(jsonString);
			if (PartsMenu.instance != null)
			{
				PartsMenu.instance.ForceHide(true);
			}
			this.OpenPageByParam(param);
			this.webWindowOpenWait = 0f;
		}
	}

	private void OpenPageByParam(CMDWebWindow.WebParam param)
	{
		string tag = param.Tag;
		switch (tag)
		{
		case "MoveGacha":
			if (GUIManager.CheckTopDialog("CMD_GashaTOP", null) == null)
			{
				this.ClosePanelNotEndShow(true);
				base.SetCloseAction(delegate(int i)
				{
					if (this.callbackAction != null)
					{
						this.callbackAction();
					}
					GUIMain.BarrierOFF();
					GUIMain.ShowCommonDialog(null, "CMD_GashaTOP");
				});
			}
			this.ClosePanel(true);
			break;
		case "MoveQuest":
			if (CMD_QuestSelect.instance == null)
			{
				this.ClosePanelNotEndShow(true);
				RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
				List<string> list = new List<string>();
				list.Add("1");
				list.Add("3");
				list.Add("8");
				ClassSingleton<QuestData>.Instance.GetWorldDungeonInfo(list, new Action<bool>(this.actCBQuest));
			}
			else
			{
				this.ClosePanel(true);
			}
			break;
		case "MoveExchange":
			if (GUIManager.CheckTopDialog("CMD_ClearingHouseTOP", null) == null)
			{
				this.ClosePanelNotEndShow(true);
				base.SetCloseAction(delegate(int i)
				{
					if (this.callbackAction != null)
					{
						this.callbackAction();
					}
					GUIMain.BarrierOFF();
					GUIMain.ShowCommonDialog(null, "CMD_ClearingHouseTOP");
				});
			}
			this.ClosePanel(true);
			break;
		case "ToggleBGM":
			this.ToggleBGM(false);
			break;
		case "Link":
		{
			string text = param.Url;
			text = this.DecodeUrl(text);
			this.CloseWebView();
			this.StartWebView(ConstValue.APP_WEB_DOMAIN + text);
			if (base.GetActionStatus() == CommonDialog.ACT_STATUS.CLOSING || base.GetActionStatus() == CommonDialog.ACT_STATUS.CLOSED)
			{
				this.CloseWebView();
			}
			break;
		}
		case "ExLink":
			Application.OpenURL(param.Url);
			break;
		case "PlayMovie":
		{
			this.isMute = false;
			this.ToggleBGM(true);
			PlayMovie component = base.GetComponent<PlayMovie>();
			component.actionFinishedMovie = new Action<bool>(this.ToggleBGM);
			NpMovie.TouchFinish = true;
			NpMovie.SoundEnable = true;
			NpMovie.ControllerEnabled = false;
			string path = ConstValue.APP_ASSET_DOMAIN + this.DecodeUrl(param.Url);
			NpMovie.PlayStreaming(path, base.gameObject, component);
			break;
		}
		}
	}

	private string DecodeUrl(string url)
	{
		url = url.Replace("%2F", "/");
		url = url.Replace("%3F", "?");
		url = url.Replace("%3D", "=");
		url = url.Replace("%26", "&");
		return url;
	}

	public void ToggleBGM(bool isForceMute = false)
	{
		if (!this.isMute || isForceMute)
		{
			this.isMute = true;
			string nowBGMPath = SoundMng.Instance().GetNowBGMPath();
			if (!string.IsNullOrEmpty(nowBGMPath))
			{
				this.prevBGMPath = nowBGMPath;
			}
			SoundMng.Instance().StopBGM(0f, null);
		}
		else
		{
			this.isMute = false;
			SoundMng.Instance().PlayBGM(this.prevBGMPath, 0f, null);
		}
	}

	private void CloseWebView()
	{
		if (CMDWebWindow.webViewObject != null)
		{
			CMDWebWindow.webViewObject.Close();
			CMDWebWindow.webViewObject.Destroy();
			CMDWebWindow.webViewObject = null;
		}
	}

	private void actCBQuest(bool isSuccess)
	{
		if (isSuccess)
		{
			base.SetCloseAction(delegate(int i)
			{
				GUIMain.BarrierOFF();
				GUIManager.CloseAllCommonDialog(new Action<int>(this.ShowQuestSelect));
				if (this.callbackAction != null)
				{
					this.callbackAction();
				}
			});
			this.ClosePanel(false);
		}
		else
		{
			RestrictionInput.EndLoad();
		}
	}

	private void ShowQuestSelect(int i)
	{
		GUIMain.ShowCommonDialog(null, "CMD_QuestSelect");
	}

	public void alertOpenedAction()
	{
		if (base.GetActionStatus() < CommonDialog.ACT_STATUS.CLOSING)
		{
			this.ClosePanel(true);
		}
	}

	public static void DeleteWebView()
	{
		if (CMDWebWindow.webViewObject != null)
		{
			CMDWebWindow.webViewObject.Close();
			CMDWebWindow.webViewObject.Destroy();
			CMDWebWindow.webViewObject = null;
		}
	}

	private class WebParam
	{
		public string Tag;

		public string Url;
	}
}
