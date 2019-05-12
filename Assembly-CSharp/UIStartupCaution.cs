using Master;
using Neptune.UrlScheme;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIStartupCaution : GUIScreen
{
	[SerializeField]
	private GameObject TXT;

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
		iTween.ValueTo(base.gameObject, iTween.Hash(new object[]
		{
			"from",
			0,
			"to",
			1,
			"time",
			this.fTimer,
			"onupdate",
			"SetAlpha"
		}));
	}

	private void SetAlpha(float value)
	{
		this.TXT.GetComponent<UILabel>().alpha = value;
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
