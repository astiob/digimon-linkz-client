using Master;
using Neptune.UrlScheme;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIStartupCaution : GUIScreen
{
	[SerializeField]
	private GameObject TXT;

	[SerializeField]
	private GameObject AnniversaryLogo;

	[SerializeField]
	private bool isDispAnnivLogo;

	private float originTime;

	private float fTimer;

	private float timer;

	private bool tapSkip;

	private bool mainFade;

	protected override void Awake()
	{
		Application.targetFrameRate = 30;
		base.Awake();
		this.originTime = 4.5f;
		this.fTimer = 0.5f;
		this.timer = this.originTime + this.fTimer;
		this.tapSkip = false;
		this.mainFade = false;
		UILabel component = this.TXT.GetComponent<UILabel>();
		component.text = StringMaster.GetString("CautionMessage");
		this.CheckUrlSchemeAction();
	}

	public override void ShowGUI()
	{
		base.ShowGUI();
		if (this.isDispAnnivLogo)
		{
			this.FadeInAnnivLogo();
		}
		else
		{
			this.DisplayNoticeText();
		}
	}

	private void FadeInAnnivLogo()
	{
		this.timer += 3f;
		iTween.ValueTo(base.gameObject, iTween.Hash(new object[]
		{
			"from",
			0,
			"to",
			1,
			"time",
			2f,
			"easeType",
			iTween.EaseType.easeOutQuart,
			"onupdate",
			"SetAnnivLogoAlpha",
			"oncomplete",
			"FadeOutAnnivLogo",
			"oncompletetarget",
			base.gameObject
		}));
	}

	private void FadeOutAnnivLogo()
	{
		iTween.ValueTo(base.gameObject, iTween.Hash(new object[]
		{
			"from",
			1,
			"to",
			0,
			"time",
			1f,
			"easeType",
			iTween.EaseType.easeInCirc,
			"onupdate",
			"SetAnnivLogoAlpha",
			"oncomplete",
			"DisplayNoticeText",
			"oncompletetarget",
			base.gameObject
		}));
	}

	private void DisplayNoticeText()
	{
		iTween.ValueTo(base.gameObject, iTween.Hash(new object[]
		{
			"from",
			0,
			"to",
			1,
			"time",
			this.fTimer,
			"onupdate",
			"SetNoticeTextAlpha"
		}));
	}

	private void SetNoticeTextAlpha(float value)
	{
		this.TXT.GetComponent<UILabel>().alpha = value;
	}

	private void SetAnnivLogoAlpha(float value)
	{
		this.AnniversaryLogo.GetComponent<UITexture>().alpha = value;
	}

	protected override void Update()
	{
		base.Update();
		this.timer -= Time.deltaTime;
		if ((this.timer <= 0f || this.tapSkip) && !this.mainFade)
		{
			this.mainFade = true;
			GUIMain.FadeWhiteReqScreen("UITitle", delegate(int x)
			{
				GUIFadeControll.ActionRestart();
			}, 1.2f, 1.2f, false);
		}
	}

	private void TapSkip()
	{
		if (this.timer > this.originTime)
		{
			return;
		}
		this.tapSkip = true;
	}

	private void CheckUrlSchemeAction()
	{
		if (!NpUrlScheme.IsUrlSchemeAction())
		{
			global::Debug.Log("no UrlScheme");
			return;
		}
		global::Debug.Log("UrlScheme located");
		Dictionary<string, object> queryParam = NpUrlScheme.GetQueryParam();
		if (queryParam != null)
		{
			foreach (KeyValuePair<string, object> keyValuePair in queryParam)
			{
				global::Debug.Log(string.Format("#=#=# Key = {0}, Value = {1}", keyValuePair.Key, keyValuePair.Value));
			}
		}
		else
		{
			global::Debug.Log("queryParamDic is null");
		}
		NpUrlScheme.Clear();
	}
}
