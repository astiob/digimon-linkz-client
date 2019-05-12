using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using WebAPIRequest;

public class CMD_MultiLangSetting : CMD
{
	private const string BUTTON_ACTIVE_SPRITE = "Common02_Btn_BaseON1";

	private const string BUTTON_INACTIVE_SPRITE = "Common02_Btn_BaseOFF";

	public GameObject goTX_BTN_NO;

	[SerializeField]
	private UISprite enBtnImg;

	[SerializeField]
	private UISprite cnBtnImg;

	[SerializeField]
	private UISprite krBtnImg;

	[CompilerGenerated]
	private static Action <>f__mg$cache0;

	protected override void Awake()
	{
		base.Awake();
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
		bool isUpdateRequired = CountrySetting.IsReloadRequired(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
		CountrySetting.SetCountryCode(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN), CountrySetting.CountryCode.EN);
		this.updateServer(isUpdateRequired);
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
			CountrySetting.ReloadMaster();
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
			GameWebAPI.RequestUS_RegisterLanguageInfo requestUS_RegisterLanguageInfo2 = requestUS_RegisterLanguageInfo;
			RequestBase request = requestUS_RegisterLanguageInfo2;
			if (CMD_MultiLangSetting.<>f__mg$cache0 == null)
			{
				CMD_MultiLangSetting.<>f__mg$cache0 = new Action(RestrictionInput.EndLoad);
			}
			base.StartCoroutine(request.RunOneTime(CMD_MultiLangSetting.<>f__mg$cache0, delegate(Exception noop)
			{
				RestrictionInput.EndLoad();
			}, null));
		}
		else
		{
			base.ClosePanel(true);
		}
	}
}
