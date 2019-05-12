using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CMD_Shop : CMD
{
	public static CMD_Shop instance;

	[SerializeField]
	private UILabel stoneNum;

	[SerializeField]
	private GameObject fundObject;

	[SerializeField]
	private GameObject tradeObject;

	[SerializeField]
	private GUISelectPanelStone productScrollView;

	[SerializeField]
	private GameObject productScrollViewItem;

	private List<StoreUtil.StoneStoreData> storeProductList;

	private bool closeWhenConsumed;

	private int virtualUsedStoneCount;

	private Action hideGUIAction;

	public int VirtualUsedStoneNum
	{
		get
		{
			return this.virtualUsedStoneCount;
		}
		set
		{
			this.virtualUsedStoneCount = value;
			this.SetDigistoneNumber();
		}
	}

	public bool CloseWhenConsumed
	{
		get
		{
			return this.closeWhenConsumed;
		}
		set
		{
			this.closeWhenConsumed = value;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		CMD_Shop.instance = this;
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		if (null != this.fundObject)
		{
			this.fundObject.SetActive(false);
		}
		if (null != this.tradeObject)
		{
			this.tradeObject.SetActive(false);
		}
		base.HideDLG();
		base.StartCoroutine(this.InitShop(f, sizeX, sizeY, aT));
	}

	private IEnumerator InitShop(Action<int> f, float sizeX, float sizeY, float aT)
	{
		yield return base.StartCoroutine(StoreInit.Instance().InitStore());
		yield return base.StartCoroutine(StoreInit.Instance().InitRestoreOperation());
		yield return base.StartCoroutine(StoreInit.Instance().GetProductsOperation());
		if (StoreInit.Instance().IsSuccessReceiveProducts() && StoreInit.STATUS.DONE_RECONSUME <= StoreInit.Instance().GetStatus())
		{
			if (DataMng.Instance().RespDataSH_Info.isShopMaintenance == 1)
			{
				base.SetCloseAction(f);
				StoreInit.Instance().SetStatusToDoneInit();
				AlertManager.ShowAlertDialog(delegate(int i)
				{
					base.ClosePanel(false);
				}, "C-SH05");
			}
			else if (DataMng.Instance().RespDataSH_Info.isOverDigiStone == 1)
			{
				base.SetCloseAction(f);
				if (AlertManager.CheckDialogMessage("C-SH06"))
				{
					AlertManager.ShowAlertDialog(delegate(int i)
					{
						base.ClosePanel(false);
					}, "C-SH06");
				}
			}
			else
			{
				base.ShowDLG();
				base.PartsTitle.SetTitle(StringMaster.GetString("ShopTitle"));
				this.SetDigistoneNumber();
				this.SetProductScrollView();
				base.Show(f, sizeX, sizeY, aT);
			}
		}
		else
		{
			base.SetCloseAction(f);
			base.ClosePanel(false);
		}
		RestrictionInput.EndLoad();
		yield break;
	}

	protected override void WindowOpened()
	{
		base.WindowOpened();
		FarmCameraControlForCMD.Off();
	}

	public override void ClosePanel(bool animation = true)
	{
		if (this.hideGUIAction != null)
		{
			this.hideGUIAction();
			this.hideGUIAction = null;
		}
		FarmCameraControlForCMD.On();
		base.ClosePanel(animation);
		if (null != this.productScrollView)
		{
			this.productScrollView.FadeOutAllListParts(null, false);
			this.productScrollView.SetHideScrollBarAllWays(true);
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		CMD_Shop.instance = null;
	}

	private void OnClickedB0()
	{
		CMDWebWindow cmdwebWindow = GUIMain.ShowCommonDialog(null, "CMDWebWindow", null) as CMDWebWindow;
		if (null != cmdwebWindow)
		{
			cmdwebWindow.TitleText = StringMaster.GetString("ShopRule-02");
			cmdwebWindow.Url = WebAddress.EXT_ADR_TRADE;
		}
	}

	private void OnClickedB1()
	{
		CMDWebWindow cmdwebWindow = GUIMain.ShowCommonDialog(null, "CMDWebWindow", null) as CMDWebWindow;
		if (null != cmdwebWindow)
		{
			cmdwebWindow.TitleText = StringMaster.GetString("ShopRule-01");
			cmdwebWindow.Url = WebAddress.EXT_ADR_FUND;
		}
	}

	private void SetDigistoneNumber()
	{
		int num = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point - this.virtualUsedStoneCount;
		this.stoneNum.text = num.ToString();
	}

	private void SetProductScrollView()
	{
		this.productScrollView.Callback = new Action(this.SetDigistoneNumber);
		Vector3 localPosition = this.productScrollView.transform.localPosition;
		GUICollider component = this.productScrollView.GetComponent<GUICollider>();
		component.SetOriginalPos(localPosition);
		this.productScrollView.selectParts = this.productScrollViewItem;
		Rect listWindowViewRect = default(Rect);
		listWindowViewRect.xMin = -560f;
		listWindowViewRect.xMax = 560f;
		listWindowViewRect.yMin = -256f - GUIMain.VerticalSpaceSize;
		listWindowViewRect.yMax = 256f + GUIMain.VerticalSpaceSize;
		this.productScrollView.ListWindowViewRect = listWindowViewRect;
		this.storeProductList = StoreUtil.Instance().GetStoneStoreDataList();
		this.productScrollView.initLocation = true;
		this.productScrollView.AllBuild(this.storeProductList);
	}

	public void DeleteListParts(int index)
	{
		this.storeProductList.RemoveAt(index);
		this.productScrollView.AllBuild(this.storeProductList);
	}

	public void OnEndConsume(bool isSuccess)
	{
		if (DataMng.Instance().RespDataSH_ReqVerify != null && DataMng.Instance().RespDataSH_ReqVerify.status == 2)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
			GameWebAPI.RequestUS_UserStatus requestUS_UserStatus = new GameWebAPI.RequestUS_UserStatus();
			requestUS_UserStatus.SetSendData = delegate(GameWebAPI.PlayerInfoSendData param)
			{
				param.keys = "point";
			};
			requestUS_UserStatus.OnReceived = delegate(GameWebAPI.RespDataUS_GetPlayerInfo response)
			{
				DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point = response.playerInfo.point;
			};
			GameWebAPI.RequestUS_UserStatus request = requestUS_UserStatus;
			base.StartCoroutine(request.Run(delegate()
			{
				RestrictionInput.EndLoad();
				this.OnUpdatedDigistone(isSuccess);
			}, null, null));
		}
		else
		{
			this.OnUpdatedDigistone(isSuccess);
		}
	}

	private void OnUpdatedDigistone(bool isSuccess)
	{
		GUIPlayerStatus.RefreshParams_S(false);
		this.SetDigistoneNumber();
		if (!isSuccess)
		{
			if (StoreInit.STATUS.DONE_RECONSUME > StoreInit.Instance().GetStatus())
			{
				this.ClosePanel(true);
			}
		}
		else if (this.closeWhenConsumed)
		{
			this.ClosePanel(true);
		}
	}

	public void SetHideGUIAction(Action action)
	{
		this.hideGUIAction = action;
	}
}
