using Master;
using System;
using UI.Common;
using UnityEngine;
using User;

namespace UI.Gasha
{
	public sealed class GashaStartButtonEvent : MonoBehaviour
	{
		[SerializeField]
		private GashaStartButton buttonOne;

		[SerializeField]
		private GashaStartButton buttonTen;

		private GashaStartButton[] buttonList;

		private GameWebAPI.RespDataGA_GetGachaInfo.Result gashaInfo;

		private bool isTutorial;

		private CMD cofirmDialog;

		private void Awake()
		{
			this.buttonOne.Initialize(new Action<int, int>(this.CheckPayAssetsNumber));
			this.buttonTen.Initialize(new Action<int, int>(this.CheckPayAssetsNumber));
			this.buttonList = new GashaStartButton[]
			{
				this.buttonOne,
				this.buttonTen
			};
		}

		private bool CheckLimitPlayCount(int playCount)
		{
			bool result = false;
			if (this.gashaInfo.ExistLimitPlayCount())
			{
				if (playCount <= this.gashaInfo.GetRemainingPlayCount())
				{
					result = true;
				}
			}
			else
			{
				result = true;
			}
			return result;
		}

		public void SetGashaInfo(GameWebAPI.RespDataGA_GetGachaInfo.Result gashaInfo, bool isTutorial)
		{
			this.gashaInfo = gashaInfo;
			this.isTutorial = isTutorial;
			MasterDataMng.AssetCategory costAssetsCategory = gashaInfo.priceType.GetCostAssetsCategory();
			string costAssetsValue = gashaInfo.priceType.GetCostAssetsValue();
			for (int i = 0; i < gashaInfo.details.Length; i++)
			{
				this.buttonList[i].SetAssets(costAssetsCategory, costAssetsValue, gashaInfo.details[i].GetPrice());
				this.buttonList[i].SetPopText(gashaInfo, gashaInfo.details[i], isTutorial);
			}
		}

		public void SetPlayButton()
		{
			MasterDataMng.AssetCategory costAssetsCategory = this.gashaInfo.priceType.GetCostAssetsCategory();
			string costAssetsValue = this.gashaInfo.priceType.GetCostAssetsValue();
			int number = UserInventory.GetNumber(costAssetsCategory, costAssetsValue);
			int playCount = 0;
			for (int i = 0; i < this.gashaInfo.details.Length; i++)
			{
				if (int.TryParse(this.gashaInfo.details[i].count, out playCount))
				{
					if (this.CheckLimitPlayCount(playCount))
					{
						this.buttonList[i].SetButtonAppearance(costAssetsCategory, costAssetsValue, number, this.gashaInfo.details[i].GetPrice());
					}
					else
					{
						this.buttonList[i].DisableButton();
					}
				}
			}
		}

		private void CheckPayAssetsNumber(int price, int playCount)
		{
			MasterDataMng.AssetCategory prizeAssetsCategory = this.gashaInfo.GetPrizeAssetsCategory();
			if (UserInventory.CheckOverNumber(prizeAssetsCategory, 0))
			{
				FactoryLimitOverNotice.CreateDialog(prizeAssetsCategory, LimitOverNoticeType.GASHA);
			}
			else
			{
				MasterDataMng.AssetCategory costAssetsCategory = this.gashaInfo.priceType.GetCostAssetsCategory();
				string costAssetsValue = this.gashaInfo.priceType.GetCostAssetsValue();
				int num = UserInventory.GetNumber(costAssetsCategory, costAssetsValue);
				if (this.isTutorial && num < price)
				{
					num = price;
				}
				if (num < price)
				{
					if (costAssetsCategory == MasterDataMng.AssetCategory.DIGI_STONE)
					{
						CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.OnClosedShopOpenConfirm), "CMD_Confirm", null) as CMD_Confirm;
						cmd_Confirm.Title = this.gashaInfo.gachaName;
						cmd_Confirm.Info = StringMaster.GetString("GashaShortage");
						cmd_Confirm.BtnTextYes = StringMaster.GetString("SystemButtonGoShop");
						cmd_Confirm.BtnTextNo = StringMaster.GetString("SystemButtonClose");
					}
				}
				else
				{
					GameWebAPI.GA_Req_ExecGacha useDetail = new GameWebAPI.GA_Req_ExecGacha
					{
						gachaId = int.Parse(this.gashaInfo.gachaId),
						playCount = playCount,
						itemCount = num
					};
					this.cofirmDialog = FactoryPayConfirmNotice.CreateDialog(costAssetsCategory, costAssetsValue, this.gashaInfo.gachaName, num, price, new Action(this.OnPushedConfirmYesButton), playCount, useDetail);
				}
			}
		}

		private void OnClosedShopOpenConfirm(int selectButtonIndex)
		{
			if (selectButtonIndex == 0)
			{
				GUIMain.ShowCommonDialog(null, "CMD_Shop", null);
			}
		}

		public void OnPushedConfirmYesButton()
		{
			if (null != this.cofirmDialog)
			{
				IPayConfirmNotice payConfirmNotice = this.cofirmDialog as IPayConfirmNotice;
				if (payConfirmNotice != null)
				{
					GameWebAPI.GA_Req_ExecGacha ga_Req_ExecGacha = payConfirmNotice.GetUseDetail() as GameWebAPI.GA_Req_ExecGacha;
					if (ga_Req_ExecGacha != null)
					{
						ExecGashaBase execGashaBase = FactoryExecGasha.CreateExecGasha(this.gashaInfo, this.isTutorial);
						if (execGashaBase != null)
						{
							AppCoroutine.Start(execGashaBase.Exec(ga_Req_ExecGacha, this.isTutorial), false);
						}
					}
				}
				this.cofirmDialog.ClosePanel(true);
			}
		}

		public void OnPushedOneButton()
		{
			GameWebAPI.RespDataGA_GetGachaInfo.Detail detail = this.gashaInfo.details.GetDetail(1);
			if (detail != null)
			{
				int price = 0;
				if (int.TryParse(detail.GetPrice(), out price))
				{
					this.CheckPayAssetsNumber(price, 1);
				}
			}
		}
	}
}
