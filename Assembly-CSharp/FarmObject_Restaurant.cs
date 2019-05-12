using FarmData;
using System;

public sealed class FarmObject_Restaurant : FarmObject
{
	public override void BuildComplete()
	{
		base.BuildComplete();
		UserFacility userFacility = Singleton<UserDataMng>.Instance.GetUserFacility(this.userFacilityID);
		FacilityRestaurantM facilityRestaurantMaster = FarmDataManager.GetFacilityRestaurantMaster(userFacility.level);
		DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.staminaMax = facilityRestaurantMaster.maxStamina;
		int num = int.Parse(facilityRestaurantMaster.maxStamina);
		if (num > DataMng.Instance().RespDataUS_PlayerInfo.playerInfo.stamina)
		{
			Singleton<UserDataMng>.Instance.RecoveryUserStamina(num);
		}
	}
}
