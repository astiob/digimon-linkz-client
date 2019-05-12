using System;
using UnityEngine;

public class CMD_MultiLangSetting_TEST : CMD
{
	public GameObject goTX_TITLE;

	public GameObject goTX_BTN_NO;

	private UILabel ngTX_TITLE;

	private UILabel ngTX_BTN_NO;

	public void SetTitle(string str)
	{
		this.ngTX_TITLE.text = str;
	}

	public void SetBtnText_NO(string str)
	{
		this.ngTX_BTN_NO.text = str;
	}

	protected override void Awake()
	{
		base.Awake();
		this.ngTX_TITLE = this.goTX_TITLE.GetComponent<UILabel>();
		this.ngTX_BTN_NO = this.goTX_BTN_NO.GetComponent<UILabel>();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.Show(f, sizeX, sizeY, aT);
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void ClosePanel(bool animation = true)
	{
		base.ClosePanel(animation);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	private void OnSelectEN()
	{
	}

	private void OnSelectKR()
	{
	}

	private void OnSelectZHT()
	{
	}

	private void updateServer(bool isUpdateRequired)
	{
		if (isUpdateRequired)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			GameWebAPI.RequestUS_RegisterLanguageInfo requestUS_RegisterLanguageInfo = new GameWebAPI.RequestUS_RegisterLanguageInfo();
			requestUS_RegisterLanguageInfo.SetSendData = delegate(GameWebAPI.US_Req_RegisterLanguageInfo param)
			{
				param.countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
			};
			requestUS_RegisterLanguageInfo.OnReceived = delegate(WebAPI.ResponseData response)
			{
				GUIMain.BackToTOP("UIStartupCaution", 0.8f, 0.8f);
				if (GUIFace.instance != null)
				{
					UnityEngine.Object.Destroy(GUIFace.instance.gameObject.transform.parent.gameObject);
				}
			};
			GameWebAPI.RequestUS_RegisterLanguageInfo request = requestUS_RegisterLanguageInfo;
			base.StartCoroutine(request.RunOneTime(new Action(RestrictionInput.EndLoad), delegate(Exception noop)
			{
				RestrictionInput.EndLoad();
			}, null));
		}
		else
		{
			this.ClosePanel(true);
		}
	}
}
