using FarmData;
using System;

public class FarmObject_ChipFarm : FarmObject
{
	public override void BuildComplete()
	{
		base.BuildComplete();
		UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(this.userFacilityID);
		FacilityChipM facilityChipFarmMaster = FarmDataManager.GetFacilityChipFarmMaster(userFacility.level);
		DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.chipLimitMax = int.Parse(facilityChipFarmMaster.maxChipNum);
	}
}
