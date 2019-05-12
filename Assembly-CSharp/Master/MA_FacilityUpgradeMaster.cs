using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_FacilityUpgradeMaster : MasterBaseData<GameWebAPI.RespDataMA_GetFacilityUpgradeM>
	{
		public MA_FacilityUpgradeMaster()
		{
			base.ID = MasterId.FACILITY_UPGRADE;
		}

		public override string GetTableName()
		{
			return "facility_upgrade_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_FacilityUpgradeMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_GetFacilityUpgradeM>(base.SetResponse)
			};
		}

		protected override void PrepareData(GameWebAPI.RespDataMA_GetFacilityUpgradeM src)
		{
			FarmDataManager.SetResponseData(src);
			this.data = src;
		}
	}
}
