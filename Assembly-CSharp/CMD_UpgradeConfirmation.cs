using FarmData;
using Master;
using System;
using UnityEngine;

public class CMD_UpgradeConfirmation : CMD
{
	protected UserFacility userFacility;

	[SerializeField]
	private UILabel facilityNameTitle;

	[SerializeField]
	protected UILabel facilityName;

	[SerializeField]
	protected UITexture thumbnail;

	[SerializeField]
	protected UILabel effectTitle;

	[SerializeField]
	protected UILabel effectDetail;

	[SerializeField]
	private UILabel timeTitle;

	[SerializeField]
	private UILabel timeDetail;

	[SerializeField]
	protected UILabel detail;

	[SerializeField]
	private UILabel possessionTitle;

	[SerializeField]
	protected UILabel possessionDetail;

	[SerializeField]
	private UILabel costTitle;

	[SerializeField]
	protected UILabel costDetail;

	[SerializeField]
	private UILabel confirmLabel;

	[SerializeField]
	private UILabel closeButtonLabel;

	[SerializeField]
	private UILabel yesButtonLabel;

	private void Start()
	{
		this.SetFixedString();
	}

	protected void SetFixedString()
	{
		this.facilityNameTitle.text = StringMaster.GetString("FacilityInfoTitle");
		this.timeTitle.text = StringMaster.GetString("FacilityUpgradeTimeTitle");
		this.possessionTitle.text = StringMaster.GetString("SystemPossession");
		this.costTitle.text = StringMaster.GetString("SystemCost");
		this.confirmLabel.text = StringMaster.GetString("FacilityUpgradeConfirmInfo");
		this.closeButtonLabel.text = StringMaster.GetString("SystemButtonClose");
		this.yesButtonLabel.text = StringMaster.GetString("SystemButtonYes");
	}

	public void SetUserFacility(UserFacility userFacility)
	{
		this.userFacility = userFacility;
		int num = userFacility.level + 1;
		FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(userFacility.facilityId);
		FacilityUpgradeM facilityUpgradeMaster = FarmDataManager.GetFacilityUpgradeMaster(userFacility.facilityId, num);
		this.SetPossessionMoney(facilityUpgradeMaster);
		this.detail.text = FarmDataManager.GetFacilityDescription(userFacility.facilityId, userFacility.level);
		this.timeDetail.text = facilityUpgradeMaster.upgradeTime.ToBuildTime();
		this.facilityName.text = string.Format(StringMaster.GetString("FacilityInfoLv"), facilityMaster.facilityName, num);
		NGUIUtil.ChangeUITextureFromFile(this.thumbnail, facilityMaster.GetIconPath(), false);
		string facilityEffectDetail = FarmDataManager.GetFacilityEffectDetail(userFacility.facilityId, num);
		this.effectTitle.text = facilityMaster.popDescription;
		this.effectDetail.text = string.Format(facilityMaster.popDetails, facilityEffectDetail);
	}

	protected virtual void SetPossessionMoney(FacilityUpgradeM masterUpgrade)
	{
		string gamemoney = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.gamemoney;
		this.possessionDetail.text = StringFormat.Cluster(gamemoney);
		if (masterUpgrade.upgradeAssetNum1.ToInt32() > gamemoney.ToInt32())
		{
			this.costDetail.color = Color.red;
		}
		this.costDetail.text = StringFormat.Cluster(masterUpgrade.upgradeAssetNum1);
	}

	private void OnPushedYesButton()
	{
		int level = this.userFacility.level + 1;
		FacilityUpgradeM facilityUpgradeMaster = FarmDataManager.GetFacilityUpgradeMaster(this.userFacility.facilityId, level);
		if (FarmUtility.IsShortage(facilityUpgradeMaster.upgradeAssetCategoryId1, facilityUpgradeMaster.upgradeAssetNum1))
		{
			this.OpenModalShortageMessage(facilityUpgradeMaster.upgradeAssetCategoryId1);
		}
		else if (2 <= FarmUtility.GetBuildFacilityCount())
		{
			CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
			cmd_ModalMessage.Title = StringMaster.GetString("FacilityUpgradeBuildTitle");
			cmd_ModalMessage.Info = StringMaster.GetString("FacilityUpgradeBuildInfo");
		}
		else
		{
			FarmFacilityData.FacilityID facilityId = (FarmFacilityData.FacilityID)this.userFacility.facilityId;
			FacilityUpgradeConfirm facilityUpgradeConfirm;
			if (facilityId != FarmFacilityData.FacilityID.MEAT_FARM)
			{
				facilityUpgradeConfirm = new FacilityUpgradeConfirm(this, this.userFacility);
			}
			else
			{
				facilityUpgradeConfirm = new FacilityUpgradeConfirm_MeatFarm(this, this.userFacility);
			}
			facilityUpgradeConfirm.Upgrade();
		}
	}

	protected virtual void OpenModalShortageMessage(string categoryId)
	{
		GameWebAPI.RespDataMA_GetAssetCategoryM.AssetCategoryM assetCategory = MasterDataMng.Instance().RespDataMA_AssetCategoryM.GetAssetCategory(categoryId);
		CMD_ModalMessage cmd_ModalMessage = GUIMain.ShowCommonDialog(null, "CMD_ModalMessage") as CMD_ModalMessage;
		cmd_ModalMessage.Title = string.Format(StringMaster.GetString("SystemShortage"), assetCategory.assetTitle);
		cmd_ModalMessage.Info = string.Format(StringMaster.GetString("FacilityUpgradeFailedShortage"), assetCategory.assetTitle);
	}

	protected void OnPushedCloseButton()
	{
		this.ClosePanel(true);
	}
}
