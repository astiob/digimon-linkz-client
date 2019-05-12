using ExchangeData;
using Master;
using System;

public sealed class CMD_ClearingHouseTop : CMD
{
	private GUIExchangeMenu menu;

	private GUITips tips;

	public static CMD_ClearingHouseTop instance;

	protected override void Awake()
	{
		CMD_ClearingHouseTop.instance = this;
		GUIManager.CloseAllCommonDialog(null);
		base.Awake();
		this.menu = base.gameObject.GetComponent<GUIExchangeMenu>();
		this.tips = base.gameObject.GetComponent<GUITips>();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		CMD_ClearingHouseTop.instance = null;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.PartsTitle.SetTitle(StringMaster.GetString("ExchangeTitle"));
		base.SetTutorialAnyTime("anytime_second_tutorial_exchange");
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.HideDLG();
		APIRequestTask task = ClassSingleton<ExchangeWebAPI>.Instance.AccessEventExchangeInfoLogicAPI();
		base.StartCoroutine(task.Run(delegate
		{
			RestrictionInput.EndLoad();
			this.ShowDLG();
			this.menu.Init();
			GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result[] eventExchangeInfoList = ClassSingleton<ExchangeWebAPI>.Instance.EventExchangeInfoList;
			bool flag = eventExchangeInfoList != null && eventExchangeInfoList.Length > 0 && eventExchangeInfoList[0] != null;
			GUITips.TIPS_DISP_TYPE dispType = (!flag) ? GUITips.TIPS_DISP_TYPE.NoneExchangeNavi : GUITips.TIPS_DISP_TYPE.ExchangeNavi;
			this.tips.Init(dispType);
			this.Show(f, sizeX, sizeY, aT);
		}, delegate(Exception nop)
		{
			RestrictionInput.EndLoad();
			this.ClosePanel(false);
		}, null));
	}

	public override void ClosePanel(bool animation = true)
	{
		FarmCameraControlForCMD.On();
		base.ClosePanel(animation);
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
	}

	public void Rebuild()
	{
		this.menu.Init();
		GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result[] eventExchangeInfoList = ClassSingleton<ExchangeWebAPI>.Instance.EventExchangeInfoList;
		bool flag = eventExchangeInfoList != null && eventExchangeInfoList.Length > 0 && eventExchangeInfoList[0] != null;
		GUITips.TIPS_DISP_TYPE dispType = (!flag) ? GUITips.TIPS_DISP_TYPE.NoneExchangeNavi : GUITips.TIPS_DISP_TYPE.ExchangeNavi;
		this.tips.Init(dispType);
	}
}
