using Master;
using System;
using System.Collections.Generic;
using UnityEngine;
using WebAPIRequest;

public class CMD_History : CMD
{
	[SerializeField]
	private GameObject digiStoneWindow;

	[SerializeField]
	private GameObject presentWindow;

	[SerializeField]
	private UILabel totalValue;

	[SerializeField]
	private UILabel tollValue;

	[SerializeField]
	private UILabel freeValue;

	[SerializeField]
	private GUISelectPresentBoxPanel selectPresentHistoryPanel;

	[SerializeField]
	private GameObject partPresetHistory;

	[SerializeField]
	private GameObject prezentZeroTXT;

	[SerializeField]
	private UILabel degistoneTitleText;

	[SerializeField]
	private UILabel totalText;

	[SerializeField]
	private UILabel tollText;

	[SerializeField]
	private UILabel freeText;

	[SerializeField]
	private UILabel presentTitleText;

	[SerializeField]
	private UILabel historyMessageText;

	private GameWebAPI.RespDataPR_PrizeReceiveHistory presentData;

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
		this.presentData = null;
		this.CloseAndFarmCamOn(animation);
	}

	public override void Show(Action<int> f, float sizeX, float sizeY, float aT)
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
		base.HideDLG();
		RequestList requestList = new RequestList();
		RequestBase addRequest = new GameWebAPI.RequestOP_ChargeItemList
		{
			OnReceived = delegate(GameWebAPI.RespDataOP_ChargeItemList response)
			{
				this.tollValue.text = response.paymentStarPieceNum.ToString();
				this.freeValue.text = response.freeStarPieceNum.ToString();
				this.totalValue.text = (response.paymentStarPieceNum + response.freeStarPieceNum).ToString();
			}
		};
		requestList.AddRequest(addRequest);
		GameWebAPI.RequestPR_PrizeReceiveHistory requestPR_PrizeReceiveHistory = new GameWebAPI.RequestPR_PrizeReceiveHistory();
		requestPR_PrizeReceiveHistory.OnReceived = delegate(GameWebAPI.RespDataPR_PrizeReceiveHistory response)
		{
			DataMng.Instance().RespDataPR_PrizeReceiveHistory = response;
		};
		addRequest = requestPR_PrizeReceiveHistory;
		requestList.AddRequest(addRequest);
		base.StartCoroutine(requestList.RunOneTime(delegate()
		{
			RestrictionInput.EndLoad();
			this.presentData = DataMng.Instance().RespDataPR_PrizeReceiveHistory;
			this.ShowDLG();
			this.PartsTitle.SetTitle(StringMaster.GetString("OtherHistory"));
			this.SettingTab();
			this.<Show>__BaseCallProxy0(f, sizeX, sizeY, aT);
			this.presentWindow.transform.gameObject.SetActive(true);
			this.selectPresentHistoryPanel.initLocation = true;
			this.selectPresentHistoryPanel.AllBuildHistory(this.presentData);
			this.presentWindow.transform.gameObject.SetActive(false);
			this.partPresetHistory.SetActive(false);
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
			this.<ClosePanel>__BaseCallProxy1(false);
		}, null));
		this.degistoneTitleText.text = StringMaster.GetString("OtherHistory-01");
		this.totalText.text = StringMaster.GetString("DegistoneHistory-01");
		this.tollText.text = StringMaster.GetString("DegistoneHistory-02");
		this.freeText.text = StringMaster.GetString("DegistoneHistory-03");
		this.presentTitleText.text = StringMaster.GetString("OtherHistory-02");
		this.historyMessageText.text = StringMaster.GetString("PresentHistory-01");
	}

	private void SettingTab()
	{
		base.MultiTab.InitMultiTab(new List<Action<int>>
		{
			new Action<int>(this.ChangeTab),
			new Action<int>(this.ChangeTab)
		}, new List<string>
		{
			StringMaster.GetString("OtherHistory-01"),
			StringMaster.GetString("OtherHistory-02")
		});
		base.MultiTab.SetOnOffColor(ConstValue.TAB_YELLOW, Color.white);
		base.MultiTab.SetFocus(1);
		this.ChangeTab(1);
	}

	private void ChangeTab(int Tab)
	{
		if (Tab != 1)
		{
			if (Tab == 2)
			{
				this.digiStoneWindow.transform.gameObject.SetActive(false);
				this.presentWindow.transform.gameObject.SetActive(true);
				if (this.presentData.prizeReceiveHistory.Length == 0)
				{
					this.prezentZeroTXT.SetActive(true);
				}
				else
				{
					this.prezentZeroTXT.SetActive(false);
				}
			}
		}
		else
		{
			this.digiStoneWindow.transform.gameObject.SetActive(true);
			this.presentWindow.transform.gameObject.SetActive(false);
		}
	}

	private void FakeMethod()
	{
	}

	public enum TAB
	{
		DigiStone = 1,
		Present
	}
}
