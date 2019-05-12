using FarmData;
using Master;
using System;

public sealed class FacilityUpgradeConfirm_MeatFarm : FacilityUpgradeConfirm
{
	private UserFacility storehouse;

	private FacilityWarehouseM masterStorehouse;

	public FacilityUpgradeConfirm_MeatFarm(CMD_UpgradeConfirmation upgradeConfirmationUI, UserFacility userFacility) : base(upgradeConfirmationUI, userFacility)
	{
		this.storehouse = Singleton<UserDataMng>.Instance.GetUserStorehouse();
		if (this.storehouse != null && 0 < this.storehouse.level)
		{
			this.masterStorehouse = FarmDataManager.GetFacilityStorehouseMaster(this.storehouse.level);
		}
	}

	public override void Upgrade()
	{
		int num = 50;
		if (this.masterStorehouse != null)
		{
			num = int.Parse(this.masterStorehouse.limitMeatNum);
		}
		FacilityMeatFieldM facilityMeatFarmMaster = FarmDataManager.GetFacilityMeatFarmMaster(this.userFacility.level + 1);
		if (facilityMeatFarmMaster != null)
		{
			if (num < int.Parse(facilityMeatFarmMaster.maxMeatNum))
			{
				CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(delegate(int selectButton)
				{
					if (selectButton == 0)
					{
						this.UpgradeMeatFarm();
					}
				}, "CMD_Confirm", null) as CMD_Confirm;
				cmd_Confirm.Title = StringMaster.GetString("SystemConfirm");
				cmd_Confirm.Info = StringMaster.GetString("FacilityUpgradeMeatOver");
			}
			else
			{
				this.UpgradeMeatFarm();
			}
		}
	}

	private void UpgradeMeatFarm()
	{
		if (!this.IsMeatNumOver())
		{
			this.MeatHarvestAndMeatFarmUpGrade();
		}
		else
		{
			CMD_Confirm cmd_Confirm = GUIMain.ShowCommonDialog(delegate(int selectButton)
			{
				if (selectButton == 0)
				{
					this.<Upgrade>__BaseCallProxy0();
				}
			}, "CMD_Confirm", null) as CMD_Confirm;
			cmd_Confirm.Title = StringMaster.GetString("SystemConfirm");
			cmd_Confirm.Info = StringMaster.GetString("FacilityUpgradeMeatLost");
		}
	}

	private bool IsMeatNumOver()
	{
		bool result = false;
		FarmObject_MeatFarm farmObject_MeatFarm = this.targetFarmObject as FarmObject_MeatFarm;
		if (null != farmObject_MeatFarm)
		{
			int cropCount = farmObject_MeatFarm.GetCropCount(farmObject_MeatFarm.GetPassSeconds(), this.userFacility.level);
			int num = 50;
			if (this.masterStorehouse != null)
			{
				num = int.Parse(this.masterStorehouse.limitMeatNum);
			}
			int num2 = int.Parse(DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.meatNum);
			result = (num2 + cropCount > num);
		}
		return result;
	}

	private void MeatHarvestAndMeatFarmUpGrade()
	{
		RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
		APIRequestTask apirequestTask = this.RequestHearvest();
		apirequestTask.Add(base.RequestUpgrade());
		this.parentUI.StartCoroutine(apirequestTask.Run(delegate
		{
			this.parentUI.ClosePanel(true);
			RestrictionInput.EndLoad();
		}, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null));
	}

	private APIRequestTask RequestHearvest()
	{
		RequestFA_FacilityHarvest requestFA_FacilityHarvest = new RequestFA_FacilityHarvest();
		requestFA_FacilityHarvest.SetSendData = delegate(FacilityHarvest param)
		{
			param.userFacilityId1 = this.userFacility.userFacilityId;
			param.userFacilityId2 = ((this.storehouse == null) ? 0 : this.storehouse.userFacilityId);
		};
		requestFA_FacilityHarvest.OnReceived = delegate(FacilityHarvestResult response)
		{
			GameWebAPI.RespDataUS_GetPlayerInfo.PlayerInfo playerInfo = DataMng.Instance().RespDataUS_PlayerInfo.playerInfo;
			playerInfo.meatNum = (int.Parse(playerInfo.meatNum) + response.harvestNum).ToString();
			GUIPlayerStatus.RefreshParams_S(false);
		};
		RequestFA_FacilityHarvest request = requestFA_FacilityHarvest;
		return new APIRequestTask(request, false);
	}
}
