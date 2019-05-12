using FarmData;
using Master;
using System;
using UnityEngine;

public class CMD_UpgradeConfirmationDigistone : CMD_UpgradeConfirmation
{
	[SerializeField]
	private UILabel ruleButtonLabel;

	[SerializeField]
	private GameObject ruleButtonObject;

	private void Start()
	{
		base.SetFixedString();
		this.ruleButtonLabel.text = StringMaster.GetString("ShopRule-02");
		if (this.ruleButtonObject != null)
		{
			this.ruleButtonObject.SetActive(false);
		}
	}

	protected override void SetPossessionMoney(FacilityUpgradeM masterUpgrade)
	{
		int point = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point;
		this.possessionDetail.text = point.ToString();
		if (masterUpgrade.upgradeAssetNum1.ToInt32() > point)
		{
			this.costDetail.color = Color.red;
		}
		this.costDetail.text = masterUpgrade.upgradeAssetNum1;
	}

	protected override void OpenModalShortageMessage(string categoryId)
	{
		GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM assetCategory = MasterDataMng.Instance().RespDataMA_AssetCategoryM.GetAssetCategory(categoryId);
		CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(new Action<int>(this.OnCloseConfirmShop), "CMD_Confirm", null) as CMD_Confirm;
		cmd_Confirm.Title = string.Format(StringMaster.GetString("SystemShortage"), assetCategory.assetTitle);
		cmd_Confirm.Info = string.Format(StringMaster.GetString("FacilityUpgradeFailedShortage"), assetCategory.assetTitle);
		cmd_Confirm.BtnTextYes = StringMaster.GetString("SystemButtonGoShop");
		cmd_Confirm.BtnTextNo = StringMaster.GetString("SystemButtonClose");
	}

	private void OnCloseConfirmShop(int selectButtonIndex)
	{
		if (selectButtonIndex == 0)
		{
			base.SetCloseAction(new Action<int>(this.OnCloseDialog));
			this.ClosePanel(true);
		}
	}

	private void OnCloseDialog(int noop)
	{
		int digiStoneNum = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point;
		FarmUI farmUI = Singleton<GUIMain>.Instance.GetComponentInChildren<FarmUI>();
		Action<int> action = delegate(int nop)
		{
			if (digiStoneNum < DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.point)
			{
				farmUI.UpdateFacilityButton(null);
			}
		};
		GUIMain.ShowCommonDialog(action, "CMD_Shop", null);
	}

	private void OnClickedLegalSpecificButton()
	{
		CMDWebWindow cmdwebWindow = GUIMain.ShowCommonDialog(null, "CMDWebWindow", null) as CMDWebWindow;
		cmdwebWindow.TitleText = StringMaster.GetString("ShopRule-02");
		cmdwebWindow.Url = WebAddress.EXT_ADR_TRADE;
	}
}
