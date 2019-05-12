using FarmData;
using Master;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class ListPartsFacilityStock : GUIListPartBS
{
	[SerializeField]
	private UITexture facilityIcon;

	[SerializeField]
	private UILabel facilityName;

	[SerializeField]
	private UILabel stockCount;

	[SerializeField]
	private UILabel buildTime;

	[SerializeField]
	private UILabel tagStockCount;

	[SerializeField]
	private UILabel tagBuildTime;

	[SerializeField]
	private UILabel labelBtnSell;

	[SerializeField]
	private UILabel labelBtnPlace;

	private Action<int> actCallBackPlace;

	public ListPartsFacilityStock.FacilityStockListData StockFacility { get; set; }

	public void SetDetail(Action<int> _actCallBackPlace)
	{
		this.tagStockCount.text = StringMaster.GetString("FarmEditStockCount");
		this.tagBuildTime.text = StringMaster.GetString("FarmEditStockTime");
		this.labelBtnSell.text = StringMaster.GetString("SystemButtonSell");
		this.labelBtnPlace.text = StringMaster.GetString("SystemButtonPlace");
		this.actCallBackPlace = _actCallBackPlace;
		FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(this.StockFacility.facilityId);
		NGUIUtil.ChangeUITextureFromFile(this.facilityIcon, facilityMaster.GetIconPath(), false);
		this.facilityName.text = facilityMaster.facilityName;
		this.stockCount.text = this.StockFacility.userFacilityIdList.Count.ToString();
		this.buildTime.text = facilityMaster.buildingTime.ToBuildTime();
	}

	private void OnClickedPlace()
	{
		if (this.actCallBackPlace != null)
		{
			this.actCallBackPlace(this.StockFacility.facilityId);
		}
	}

	private void OnClickedInfo()
	{
		CMD_FacilityInfoNoneEffect cmd_FacilityInfoNoneEffect = GUIMain.ShowCommonDialog(null, "CMD_FacilityInfo_only") as CMD_FacilityInfoNoneEffect;
		cmd_FacilityInfoNoneEffect.SetFacilityInfo(this.StockFacility.facilityId);
	}

	private void OnClickedSell()
	{
		FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(this.StockFacility.facilityId);
		if (facilityMaster.sellFlg == "1")
		{
			CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.OnPushedSellYesButton), "CMD_Confirm") as CMD_Confirm;
			cmd_Confirm.Title = StringMaster.GetString("SystemConfirm");
			cmd_Confirm.Info = string.Format(StringMaster.GetString("FacilitySaleComfirmInfo"), facilityMaster.facilityName, facilityMaster.sellPrice);
		}
	}

	private void OnPushedSellYesButton(int selectButtonIndex)
	{
		if (selectButtonIndex == 0)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
			base.StartCoroutine(this.RequestSell().Run(delegate
			{
				RestrictionInput.EndLoad();
				FarmRoot instance = FarmRoot.Instance;
				if (null != instance)
				{
					Singleton<UserDataMng>.Instance.DeleteUserStockFacility(this.StockFacility.userFacilityIdList[0]);
					this.StockFacility.userFacilityIdList.RemoveAt(0);
					this.stockCount.text = this.StockFacility.userFacilityIdList.Count.ToString();
					if (this.StockFacility.userFacilityIdList.Count <= 0)
					{
						CMD_FacilityStock.instance.RebuildFacilityStockList();
					}
				}
			}, null, null));
		}
	}

	private APIRequestTask RequestSell()
	{
		GameWebAPI.RequestFA_FacilitySell requestFA_FacilitySell = new GameWebAPI.RequestFA_FacilitySell();
		requestFA_FacilitySell.SetSendData = delegate(GameWebAPI.FA_Req_FacilitySell param)
		{
			param.userFacilityId = this.StockFacility.userFacilityIdList[0];
		};
		requestFA_FacilitySell.OnReceived = delegate(GameWebAPI.RespDataFA_FacilitySell response)
		{
			int num = int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney);
			num += response.sellPrice;
			DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney = num.ToString();
			GUIPlayerStatus.RefreshParams_S(false);
		};
		GameWebAPI.RequestFA_FacilitySell request = requestFA_FacilitySell;
		return new APIRequestTask(request, true);
	}

	public sealed class FacilityStockListData
	{
		public List<int> userFacilityIdList;

		public int facilityId;
	}
}
