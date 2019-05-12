using Master;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_Shop : CMD
{
	public static CMD_Shop instance;

	[SerializeField]
	private GameObject goListPartsStone;

	[SerializeField]
	private UILabel stoneNum;

	[SerializeField]
	private UILabel labelFund;

	[SerializeField]
	private UILabel labelTrade;

	[SerializeField]
	private GameObject fundObject;

	[SerializeField]
	private GameObject tradeObject;

	[SerializeField]
	private GUISelectPanelStone csSelectPanelStone;

	private List<StoreUtil.StoneStoreData> ssdList;

	private int virtualUsedStoneCount;

	private bool closeWhenConsumed;

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
			this.UpdateDigistone();
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
		if (this.fundObject != null)
		{
			this.fundObject.SetActive(true);
		}
		if (this.tradeObject != null)
		{
			this.tradeObject.SetActive(true);
		}
		base.HideDLG();
		base.StartCoroutine(this.InitShop(f, sizeX, sizeY, aT));
	}

	private IEnumerator InitShop(Action<int> f, float sizeX, float sizeY, float aT)
	{
		this.labelFund.text = StringMaster.GetString("ShopRule-01");
		this.labelTrade.text = StringMaster.GetString("ShopRule-02");
		yield return base.StartCoroutine(StoreInit.Instance().GetProductsOperation());
		yield return base.StartCoroutine(StoreInit.Instance().InitStore());
		yield return base.StartCoroutine(StoreInit.Instance().InitRestoreOperation());
		if (StoreInit.Instance().GetProductsSucceed() && StoreInit.Instance().GetStatus() >= StoreInit.STATUS.DONE_RECONSUME)
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
				this.UpdateDigistone();
				this.SetCommonUI_Stone();
				this.Init_Stone();
				base.Show(f, sizeX, sizeY, aT);
				Singleton<GUIMain>.Instance.SetScreenEdgeCurtain(true);
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

	private void CloseAndFarmCamOn(bool animation)
	{
		FarmCameraControlForCMD.On();
		base.ClosePanel(animation);
	}

	public override void ClosePanel(bool animation = true)
	{
		if (this.hideGUIAction != null)
		{
			this.hideGUIAction();
		}
		this.CloseAndFarmCamOn(animation);
		if (this.csSelectPanelStone != null)
		{
			this.csSelectPanelStone.FadeOutAllListParts(null, false);
			this.csSelectPanelStone.SetHideScrollBarAllWays(true);
		}
		Singleton<GUIMain>.Instance.SetScreenEdgeCurtain(false);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		CMD_Shop.instance = null;
	}

	private void OnClickedB0()
	{
		CommonDialog commonDialog = GUIMain.ShowCommonDialog(null, "CMDWebWindow");
		((CMDWebWindow)commonDialog).TitleText = StringMaster.GetString("ShopRule-02");
		((CMDWebWindow)commonDialog).Url = WebAddress.EXT_ADR_TRADE;
	}

	private void OnClickedB1()
	{
		CommonDialog commonDialog = GUIMain.ShowCommonDialog(null, "CMDWebWindow");
		((CMDWebWindow)commonDialog).TitleText = StringMaster.GetString("ShopRule-01");
		((CMDWebWindow)commonDialog).Url = WebAddress.EXT_ADR_FUND;
	}

	public void UpdateDigistone()
	{
		int num = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point - this.virtualUsedStoneCount;
		this.stoneNum.text = num.ToString();
	}

	private void SetCommonUI_Stone()
	{
		this.csSelectPanelStone.Callback = new Action(this.UpdateDigistone);
		Vector3 localPosition = this.csSelectPanelStone.transform.localPosition;
		GUICollider component = this.csSelectPanelStone.GetComponent<GUICollider>();
		component.SetOriginalPos(localPosition);
		this.csSelectPanelStone.selectParts = this.goListPartsStone;
		Rect listWindowViewRect = default(Rect);
		listWindowViewRect.xMin = -560f;
		listWindowViewRect.xMax = 560f;
		listWindowViewRect.yMin = -256f - GUIMain.VerticalSpaceSize;
		listWindowViewRect.yMax = 256f + GUIMain.VerticalSpaceSize;
		this.csSelectPanelStone.ListWindowViewRect = listWindowViewRect;
	}

	private void Init_Stone()
	{
		this.ssdList = StoreUtil.Instance().GetStoneStoreDataList();
		this.goListPartsStone.SetActive(true);
		this.csSelectPanelStone.initLocation = true;
		this.csSelectPanelStone.AllBuild(this.ssdList);
		this.goListPartsStone.SetActive(false);
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

	public void DeleteListParts(int IDX)
	{
		this.ssdList.RemoveAt(IDX);
		this.goListPartsStone.SetActive(true);
		this.csSelectPanelStone.AllBuild(this.ssdList);
		this.goListPartsStone.SetActive(false);
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
		this.UpdateDigistone();
		if (!isSuccess)
		{
			if (StoreInit.Instance().GetStatus() < StoreInit.STATUS.DONE_RECONSUME)
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
