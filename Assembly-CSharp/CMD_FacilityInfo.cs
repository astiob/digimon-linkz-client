using FarmData;
using Master;
using System;
using UnityEngine;

public class CMD_FacilityInfo : CMD
{
	private UserFacility userFacility;

	[SerializeField]
	private UILabel facilityNameTitle;

	[SerializeField]
	private UILabel facilityName;

	[SerializeField]
	private UILabel effectInfo;

	[SerializeField]
	private UILabel effectDetail;

	[SerializeField]
	private UILabel detailTitle;

	[SerializeField]
	private UILabel detail;

	[SerializeField]
	private UITexture thumbnail;

	[SerializeField]
	private GameObject upgradeButton;

	[SerializeField]
	private UILabel upgradeButtonLabel;

	[SerializeField]
	private GameObject closeButton;

	[SerializeField]
	private UILabel closeButtonLabel;

	[SerializeField]
	private GameObject centerCloseButton;

	[SerializeField]
	private UILabel centerCloseButtonLabel;

	private void Start()
	{
		this.facilityNameTitle.text = StringMaster.GetString("FacilityInfoTitle");
		this.detailTitle.text = StringMaster.GetString("FacilityInfoDescription");
		this.upgradeButtonLabel.text = StringMaster.GetString("FacilityInfoUpgradeButton");
		this.closeButtonLabel.text = StringMaster.GetString("SystemButtonClose");
		this.centerCloseButtonLabel.text = StringMaster.GetString("SystemButtonClose");
	}

	public void SetFacilityInfo(UserFacility userFacility)
	{
		this.userFacility = userFacility;
		FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(userFacility.facilityId);
		this.detail.text = facilityMaster.description;
		if (int.Parse(facilityMaster.maxLevel) > userFacility.level)
		{
			this.facilityName.text = string.Format(StringMaster.GetString("FacilityInfoLv"), facilityMaster.facilityName, userFacility.level);
		}
		else
		{
			this.facilityName.text = string.Format(StringMaster.GetString("FacilityInfoLvMax"), facilityMaster.facilityName, userFacility.level);
		}
		string facilityEffectDetail = FarmDataManager.GetFacilityEffectDetail(userFacility.facilityId, userFacility.level);
		this.effectInfo.text = facilityMaster.popDescription;
		this.effectDetail.text = string.Format(facilityMaster.popDetails, facilityEffectDetail);
		NGUIUtil.ChangeUITextureFromFile(this.thumbnail, facilityMaster.GetIconPath(), false);
		if (!string.IsNullOrEmpty(this.userFacility.completeTime) || int.Parse(facilityMaster.maxLevel) <= this.userFacility.level)
		{
			this.upgradeButton.gameObject.SetActive(false);
			this.closeButton.gameObject.SetActive(false);
			this.centerCloseButton.gameObject.SetActive(true);
		}
	}

	private void OnPushedUpgradeButton()
	{
		FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(this.userFacility.facilityId);
		if (!string.IsNullOrEmpty(this.userFacility.completeTime) || int.Parse(facilityMaster.maxLevel) <= this.userFacility.level)
		{
			return;
		}
		int level = this.userFacility.level + 1;
		FacilityUpgradeM facilityUpgradeMaster = FarmDataManager.GetFacilityUpgradeMaster(this.userFacility.facilityId, level);
		int num = facilityUpgradeMaster.upgradeAssetCategoryId1.ToInt32();
		if (num == 4)
		{
			CMD_UpgradeConfirmation cmd_UpgradeConfirmation = GUIMain.ShowCommonDialog(null, "CMD_UpgradeConfirmation") as CMD_UpgradeConfirmation;
			cmd_UpgradeConfirmation.SetUserFacility(this.userFacility);
		}
		else
		{
			CMD_UpgradeConfirmationDigistone cmd_UpgradeConfirmationDigistone = GUIMain.ShowCommonDialog(null, "CMD_UpgradeConfirm_STONE") as CMD_UpgradeConfirmationDigistone;
			cmd_UpgradeConfirmationDigistone.SetUserFacility(this.userFacility);
		}
		this.ClosePanel(true);
	}

	private void OnPushedCloseButton()
	{
		this.ClosePanel(true);
	}
}
