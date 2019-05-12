using LitJson;
using Neptune.Movie;
using Neptune.OAuth;
using Neptune.WebView;
using Quest;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CMDWebWindow : CMD, INpWebViewListener
{
	[SerializeField]
	private UILabel titleLabel;

	[SerializeField]
	private Rect webViewRect;

	private float defaultBgmVolume;

	private bool isMute;

	private string prevBGMPath;

	private float webWindowOpenWait;

	protected static NpWebView webViewObject;

	protected string _Url = string.Empty;

	public static CMDWebWindow instance;

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

	public string TitleText
	{
		get
		{
			return this.titleLabel.text;
		}
		set
		{
			this.SetTitle(value);
		}
	}

	public Action callbackAction { get; set; }

	protected override void Awake()
	{
		base.Awake();
		this.defaultBgmVolume = SoundMng.Instance().VolumeBGM;
		AlertManager.alertOpenedAction = new Action(this.alertOpenedAction);
		CMDWebWindow.instance = this;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.Show(f, sizeX, sizeY, aT);
		this.SetWebView();
	}

	protected override void Update()
	{
		base.Update();
		this.webWindowOpenWait += Time.deltaTime;
	}

	protected override void WindowOpened()
	{
		if (!string.IsNullOrEmpty(this._Url))
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
		if (this.isMute)
		{
			SoundMng.Instance().VolumeBGM = this.defaultBgmVolume;
			this.isMute = false;
		}
		if (!string.IsNullOrEmpty(this._Url))
		{
			CMDWebWindow.DeleteWebView();
		}
		CMDWebWindow.instance = null;
	}

	public override void ClosePanelNotEndShow(bool animation = true)
	{
		base.ClosePanelNotEndShow(animation);
		if (!string.IsNullOrEmpty(this._Url))
		{
			CMDWebWindow.DeleteWebView();
		}
	}

	public void SetTitle(string title)
	{
		this.titleLabel.text = title;
	}

	public void SetTitleTrim(string title)
	{
		this.titleLabel.text = Regex.Replace(title, "\\t|\\n|\\r", string.Empty);
	}

	private void StartWebView(string url = null)
	{
		if (url != null)
		{
			this._Url = url;
		}
		this.WebViewOpen(this._Url);
	}

	private List<int> GetWebViewMargin(float left, float top, float right, float bottom)
	{
		float num = (float)Screen.width;
		float num2 = (float)Screen.height;
		UIRoot uiroot = GUIMain.GetUIRoot();
		if (null != uiroot)
		{
			Transform transform = uiroot.transform;
			float x = transform.localScale.x;
			float y = transform.localScale.y;
			left *= x;
			bottom *= y;
			right *= x;
			top *= y;
		}
		Camera orthoCamera = GUIMain.GetOrthoCamera();
		Vector3 zero = Vector3.zero;
		zero.x = left;
		zero.y = bottom;
		Vector3 vector = orthoCamera.WorldToScreenPoint(zero);
		zero.x = right;
		zero.y = top;
		Vector3 vector2 = orthoCamera.WorldToScreenPoint(zero);
		int item = (int)vector.x;
		int item2 = (int)(num2 - vector2.y);
		int item3 = (int)(num - vector2.x);
		int item4 = (int)vector.y;
		return new List<int>
		{
			item,
			item2,
			item3,
			item4
		};
	}

	protected void SetWebView()
	{
		CMDWebWindow.webViewObject = new NpWebView();
		CMDWebWindow.webViewObject.SetHardwareAccelerated(true);
		CMDWebWindow.webViewObject.SetNpWebViewListener(base.gameObject, this);
		Transform transform = base.transform;
		float num = transform.localPosition.x - 8000f;
		int num2 = Mathf.CeilToInt(this.webViewRect.y + transform.localPosition.y);
		int num3 = Mathf.CeilToInt(this.webViewRect.y - this.webViewRect.height + transform.localPosition.y);
		int num4 = Mathf.CeilToInt(this.webViewRect.x + num);
		int num5 = Mathf.CeilToInt(this.webViewRect.x + this.webViewRect.width + num);
		List<int> webViewMargin = this.GetWebViewMargin((float)num4, (float)num2, (float)num5, (float)num3);
		Camera orthoCamera = GUIMain.GetOrthoCamera();
		int num6 = orthoCamera.pixelWidth - (webViewMargin[0] + webViewMargin[2]);
		int num7 = orthoCamera.pixelHeight - (webViewMargin[1] + webViewMargin[3]);
		CMDWebWindow.webViewObject.ReSize((float)webViewMargin[0], (float)webViewMargin[1], (float)num6, (float)num7);
	}

	protected void WebViewOpen(string url)
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
		global::Debug.Log("Tag :" + param.Tag);
		global::Debug.Log("URL :" + param.Url);
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
					GUIMain.ShowCommonDialog(null, "CMD_GashaTOP", null);
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
					GUIMain.ShowCommonDialog(null, "CMD_ClearingHouseTOP", null);
				});
			}
			this.ClosePanel(true);
			break;
		case "ToggleBGM":
			this.ToggleBGM(false);
			break;
		case "Link":
			if (base.GetActionStatus() != CommonDialog.ACT_STATUS.CLOSING && base.GetActionStatus() != CommonDialog.ACT_STATUS.CLOSED)
			{
				string text = param.Url;
				text = this.DecodeUrl(text);
				if (CMDWebWindow.webViewObject != null)
				{
					CMDWebWindow.webViewObject.Close();
				}
				this.StartWebView(ConstValue.APP_WEB_DOMAIN + text);
			}
			break;
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

	private void ToggleBGM(bool isForceMute = false)
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

	protected void actCBQuest(bool isSuccess)
	{
		if (isSuccess)
		{
			base.SetCloseAction(delegate(int i)
			{
				GUIMain.BarrierOFF();
				GUIManager.CloseAllCommonDialog(delegate
				{
					GUIMain.ShowCommonDialog(null, "CMD_QuestSelect", null);
				});
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

	private void alertOpenedAction()
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
