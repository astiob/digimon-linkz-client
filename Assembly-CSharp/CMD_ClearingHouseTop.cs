using ExchangeData;
using Master;
using System;

public sealed class CMD_ClearingHouseTop : CMD
{
	private GUIExchangeMenu menu;

	private GUITips tips;

	protected override void Awake()
	{
		GUIManager.CloseAllCommonDialog(null);
		base.Awake();
		this.menu = base.gameObject.GetComponent<GUIExchangeMenu>();
		this.tips = base.gameObject.GetComponent<GUITips>();
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		base.PartsTitle.SetTitle(StringMaster.GetString("ExchangeTitle"));
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.HideDLG();
		APIRequestTask task = ClassSingleton<ExchangeWebAPI>.Instance.AccessEventExchangeInfoLogicAPI();
		base.StartCoroutine(task.Run(delegate
		{
			RestrictionInput.EndLoad();
			this.ShowDLG();
			this.menu.Init();
			GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result[] eventExchangeInfoLogicData = ClassSingleton<ExchangeWebAPI>.Instance.EventExchangeInfoLogicData;
			bool flag = eventExchangeInfoLogicData != null && eventExchangeInfoLogicData.Length > 0 && eventExchangeInfoLogicData[0] != null;
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
		this.CloseAndFarmCamOn(animation);
	}

	private void CloseAndFarmCamOn(bool animation)
	{
		FarmCameraControlForCMD.On();
		base.ClosePanel(animation);
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
	}
}
