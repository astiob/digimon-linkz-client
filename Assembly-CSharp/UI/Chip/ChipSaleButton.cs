using Master;
using System;
using System.Collections.Generic;
using System.Linq;
using UI.SaleCheck;
using UnityEngine;

namespace UI.Chip
{
	public sealed class ChipSaleButton : GUICollider
	{
		[SerializeField]
		private CMD_ChipAdministration parentWindow;

		private readonly string ASSET_CHIP_ID = 17.ToString();

		private void OnPushed()
		{
			CMD_SaleCheck cmd_SaleCheck = GUIMain.ShowCommonDialog(new Action<int>(this.OnClosedSaleCheckDialog), "CMD_SaleCheck", null) as CMD_SaleCheck;
			List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList> saleChipList = this.parentWindow.GetSaleChipList();
			int salePrice = this.parentWindow.GetSalePrice();
			cmd_SaleCheck.SetParams(saleChipList, StringFormat.Cluster(salePrice));
		}

		private bool ExistSalesBonus()
		{
			ChipSaleButton.<ExistSalesBonus>c__AnonStorey0 <ExistSalesBonus>c__AnonStorey = new ChipSaleButton.<ExistSalesBonus>c__AnonStorey0();
			<ExistSalesBonus>c__AnonStorey.$this = this;
			List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList> saleChipList = this.parentWindow.GetSaleChipList();
			List<string> list = new List<string>();
			for (int j = 0; j < saleChipList.Count; j++)
			{
				list.Add(saleChipList[j].chipId.ToString());
			}
			<ExistSalesBonus>c__AnonStorey.salesBonuses = MasterDataMng.Instance().AssetSalesBonusMaster.assetSalesBonusM;
			bool flag = false;
			int i;
			for (i = 0; i < <ExistSalesBonus>c__AnonStorey.salesBonuses.Length; i++)
			{
				flag = list.Any((string x) => <ExistSalesBonus>c__AnonStorey.$this.ASSET_CHIP_ID == <ExistSalesBonus>c__AnonStorey.salesBonuses[i].baseAssetCategoryId && x == <ExistSalesBonus>c__AnonStorey.salesBonuses[i].baseAssetValue);
				if (flag)
				{
					break;
				}
			}
			return flag;
		}

		private void OnClosedSaleCheckDialog(int selectButtonIndex)
		{
			if (selectButtonIndex == 0)
			{
				if (this.ExistSalesBonus())
				{
					GUIMain.ShowCommonDialog(null, "CMD_SaleCheck_Bonus", new Action<CommonDialog>(this.OnReadyBonusCheckPopup));
				}
				else
				{
					this.StartSaleChip();
				}
			}
		}

		private void OnReadyBonusCheckPopup(CommonDialog popup)
		{
			CMD_SaleBonusCheck cmd_SaleBonusCheck = popup as CMD_SaleBonusCheck;
			if (null != cmd_SaleBonusCheck)
			{
				SaleAsset saleAssetStorage = cmd_SaleBonusCheck.GetSaleAssetStorage();
				List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList> saleChipList = this.parentWindow.GetSaleChipList();
				for (int i = 0; i < saleChipList.Count; i++)
				{
					GameWebAPI.RespDataMA_ChipM.Chip chipMaster = ChipDataMng.GetChipMaster(saleChipList[i].chipId);
					if (chipMaster != null)
					{
						saleAssetStorage.AddSaleAsset(this.ASSET_CHIP_ID, chipMaster.chipId);
					}
				}
				int salePrice = this.parentWindow.GetSalePrice();
				cmd_SaleBonusCheck.SetText(StringMaster.GetString("ChipSaleCheck-01"), StringMaster.GetString("AssetSalesBonusConfirmInfo"), salePrice);
				cmd_SaleBonusCheck.CreateBonusList();
				cmd_SaleBonusCheck.SetActionYesButton(new Action<CommonDialog>(this.OnPushYesButtonBonusCheckPopup));
			}
		}

		private void OnPushYesButtonBonusCheckPopup(CommonDialog popup)
		{
			CMD_SaleBonusCheck cmd_SaleBonusCheck = popup as CMD_SaleBonusCheck;
			if (null != cmd_SaleBonusCheck)
			{
				cmd_SaleBonusCheck.SetWindowClosedAction(new Action(this.StartSaleChip));
				cmd_SaleBonusCheck.ClosePanel(true);
			}
		}

		private void StartSaleChip()
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
			List<GameWebAPI.RespDataCS_ChipListLogic.UserChipList> saleChipList = this.parentWindow.GetSaleChipList();
			List<int> list = new List<int>();
			for (int i = 0; i < saleChipList.Count; i++)
			{
				list.Add(saleChipList[i].userChipId);
			}
			this.RequestSaleChip(list);
		}

		private void RequestSaleChip(List<int> userChipIdList)
		{
			int resultCode = 0;
			bool isBonus = false;
			GameWebAPI.ChipSellLogic request = new GameWebAPI.ChipSellLogic
			{
				SetSendData = delegate(GameWebAPI.ReqDataCS_ChipSellLogic param)
				{
					param.materialChip = userChipIdList.ToArray();
				},
				OnReceived = delegate(GameWebAPI.RespDataCS_ChipSellLogic resData)
				{
					resultCode = resData.resultCode;
					if (resultCode == 1)
					{
						isBonus = resData.IsBonus();
						ChipDataMng.DeleteUserChipData(userChipIdList);
						int num = int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney);
						int num2 = num + this.parentWindow.GetSalePrice();
						DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney = num2.ToString();
					}
				}
			};
			AppCoroutine.Start(request.Run(delegate()
			{
				RestrictionInput.EndLoad();
				if (resultCode == 1)
				{
					this.parentWindow.OnCompletedSaleChip();
					if (isBonus)
					{
						CMD_ModalMessageNoBtn cmd_ModalMessageNoBtn = GUIMain.ShowCommonDialog(null, "CMD_ModalMessageNoBtn", null) as CMD_ModalMessageNoBtn;
						cmd_ModalMessageNoBtn.SetParam(StringMaster.GetString("ChipSellRecoverItem"));
						cmd_ModalMessageNoBtn.SetFontSize(24);
						cmd_ModalMessageNoBtn.AdjustSize();
					}
				}
				else
				{
					AlertManager.ShowModalMessage(null, StringMaster.GetString("SystemDataMismatchTitle"), string.Format(StringMaster.GetString("ChipDataMismatchMesage"), resultCode), AlertManager.ButtonActionType.Close, false);
				}
			}, null, null), false);
		}
	}
}
