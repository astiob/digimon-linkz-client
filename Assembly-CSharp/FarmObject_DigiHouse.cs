using FarmData;
using System;

public sealed class FarmObject_DigiHouse : FarmObject
{
	public override void BuildComplete()
	{
		base.BuildComplete();
		UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(this.userFacilityID);
		if (userFacility == null)
		{
			Debug.LogError("UserFacility Not Found");
			return;
		}
		FacilityHouseM facilityDigiHouseMaster = FarmDataManager.GetFacilityDigiHouseMaster(userFacility.level);
		if (facilityDigiHouseMaster == null)
		{
			Debug.LogError("Digihouse Master Not Found");
			return;
		}
		DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.unitLimitMax = facilityDigiHouseMaster.maxMonsterNum;
	}
}
