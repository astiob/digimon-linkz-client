using Master;
using System;
using UnityEngine;

namespace UI.Gasha
{
	public sealed class GashaMainInfomation : MonoBehaviour
	{
		[SerializeField]
		private GUISelectPanelGashaEdit gashaDetailBanner;

		[SerializeField]
		private GameObject shopButton;

		[SerializeField]
		private GameObject detailButton;

		[SerializeField]
		private GameObject cautionButton;

		[SerializeField]
		private UILabel cautionButtonLabel;

		private GameWebAPI.RespDataGA_GetGachaInfo.Result gashaInfo;

		private PartsTitleBase titleParts;

		private void OnPushedShopButton()
		{
			GUIMain.ShowCommonDialog(null, "CMD_Shop", null);
		}

		private void OnPushedCautionButton()
		{
			if (this.gashaInfo != null)
			{
				CMDWebWindow cmdwebWindow = GUIMain.ShowCommonDialog(null, "CMDWebWindow", null) as CMDWebWindow;
				if (null != cmdwebWindow)
				{
					string empty = string.Empty;
					string empty2 = string.Empty;
					this.gashaInfo.GetUrlPickUpPage(out empty, out empty2);
					cmdwebWindow.SetTitleTrim(empty);
					cmdwebWindow.Url = empty2;
				}
			}
		}

		private void OnPushedInfoButton()
		{
			if (this.gashaInfo != null)
			{
				CMDWebWindow cmdwebWindow = GUIMain.ShowCommonDialog(null, "CMDWebWindow", null) as CMDWebWindow;
				if (null != cmdwebWindow)
				{
					cmdwebWindow.SetTitle(this.gashaInfo.gachaName);
					cmdwebWindow.Url = string.Format("{0}{1}{2}", ConstValue.APP_WEB_DOMAIN, ConstValue.WEB_INFO_ADR, this.gashaInfo.gachaId);
				}
			}
		}

		private void SetCautionButtonText(MasterDataMng.AssetCategory category)
		{
			if (category != MasterDataMng.AssetCategory.MONSTER)
			{
				if (category != MasterDataMng.AssetCategory.CHIP)
				{
					if (category == MasterDataMng.AssetCategory.DUNGEON_TICKET)
					{
						this.cautionButtonLabel.text = StringMaster.GetString("TicketGashaLineupCaution");
					}
				}
				else
				{
					this.cautionButtonLabel.text = StringMaster.GetString("ChipGashaLineupCaution");
				}
			}
			else
			{
				this.cautionButtonLabel.text = StringMaster.GetString("GashaLineupCaution");
			}
		}

		public void SetTitleParts(PartsTitleBase title)
		{
			this.titleParts = title;
		}

		public void SetGashaInfo(GameWebAPI.RespDataGA_GetGachaInfo.Result gashaInfo)
		{
			this.gashaInfo = gashaInfo;
			this.gashaDetailBanner.Create();
			this.gashaDetailBanner.AllBuild(gashaInfo.subImagePath);
			this.gashaDetailBanner.selectParts.SetActive(false);
			switch (gashaInfo.priceType.GetCostAssetsCategory())
			{
			case MasterDataMng.AssetCategory.DIGI_STONE:
				this.titleParts.SetTitle(StringMaster.GetString("GashaTitleRare"));
				this.shopButton.SetActive(true);
				this.detailButton.SetActive(true);
				this.cautionButton.SetActive(true);
				this.SetCautionButtonText(gashaInfo.GetPrizeAssetsCategory());
				return;
			case MasterDataMng.AssetCategory.LINK_POINT:
				this.titleParts.SetTitle(StringMaster.GetString("GashaTitleLink"));
				this.shopButton.SetActive(false);
				this.detailButton.SetActive(false);
				this.cautionButton.SetActive(false);
				return;
			}
			this.titleParts.SetTitle(StringMaster.GetString("CaptureTitle"));
			this.shopButton.SetActive(false);
			this.detailButton.SetActive(true);
			this.cautionButton.SetActive(true);
			this.SetCautionButtonText(gashaInfo.GetPrizeAssetsCategory());
		}
	}
}
