using FarmData;
using System;
using UnityEngine;

public sealed class BuildCostLabel : MonoBehaviour
{
	[SerializeField]
	private UISprite costDigistoneIcon;

	[SerializeField]
	private UISprite costChipIcon;

	[SerializeField]
	private UILabel costValueLabel;

	private int costValue;

	public void SetUpgradeCostDetail(int userFacilityID)
	{
		UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(userFacilityID);
		FacilityUpgradeM facilityUpgradeMaster = FarmDataManager.GetFacilityUpgradeMaster(userFacility.facilityId, userFacility.level + 1);
		this.SetCostIcon(facilityUpgradeMaster.upgradeAssetCategoryId1);
		this.costValue = 0;
		if (int.TryParse(facilityUpgradeMaster.upgradeAssetNum1, out this.costValue))
		{
			this.SetCostLabel(facilityUpgradeMaster.upgradeAssetCategoryId1, this.costValue);
		}
	}

	public void SetShortCutCostDetail(int userFacilityID)
	{
		UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(userFacilityID);
		string text = string.Empty;
		string num = string.Empty;
		if (userFacility.level == 0)
		{
			FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(userFacility.facilityId);
			text = facilityMaster.shorteningAssetCategoryId1;
			num = facilityMaster.shorteningAssetNum1;
		}
		else
		{
			FacilityUpgradeM facilityUpgradeMaster = FarmDataManager.GetFacilityUpgradeMaster(userFacility.facilityId, userFacility.level);
			text = facilityUpgradeMaster.shorteningAssetCategoryId1;
			num = facilityUpgradeMaster.shorteningAssetNum1;
		}
		this.SetCostIcon(text);
		this.costValue = FarmUtility.GetShortCutDigiStoneCost(text, num, userFacility.completeTime);
		this.SetCostLabel(text, this.costValue);
	}

	public void SetMertShortCutCostDetail(int cost, string categoryId)
	{
		this.SetCostIcon(categoryId);
		this.SetCostLabel(categoryId, cost);
	}

	private void SetCostIcon(string categoryId)
	{
		int num = 0;
		if (int.TryParse(categoryId, out num))
		{
			if (num == 4)
			{
				this.costDigistoneIcon.gameObject.SetActive(false);
				this.costChipIcon.gameObject.SetActive(true);
			}
			else
			{
				this.costChipIcon.gameObject.SetActive(false);
				this.costDigistoneIcon.gameObject.SetActive(true);
			}
		}
		else
		{
			this.costChipIcon.gameObject.SetActive(false);
			this.costDigistoneIcon.gameObject.SetActive(false);
		}
	}

	private void SetCostLabel(string categoryId, int costValue)
	{
		this.costValueLabel.text = FarmUtility.GetCostString(categoryId, costValue);
		if (FarmUtility.IsShortageToInt(categoryId, costValue))
		{
			this.costValueLabel.color = Color.red;
		}
		else
		{
			this.costValueLabel.color = Color.white;
		}
	}

	public int GetCost()
	{
		return this.costValue;
	}
}
