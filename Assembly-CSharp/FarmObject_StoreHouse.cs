using FarmData;
using System;

public sealed class FarmObject_StoreHouse : FarmObject
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
		FacilityWarehouseM facilityStorehouseMaster = FarmDataManager.GetFacilityStorehouseMaster(userFacility.level);
		if (facilityStorehouseMaster == null)
		{
			Debug.LogError("masterStorehouse Master Not Found");
			return;
		}
		DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.meatLimitMax = facilityStorehouseMaster.limitMeatNum;
		GUIPlayerStatus.RefreshParams_S(false);
	}
}
