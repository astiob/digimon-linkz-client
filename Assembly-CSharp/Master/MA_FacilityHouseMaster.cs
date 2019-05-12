using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_FacilityHouseMaster : MasterBaseData<GameWebAPI.RespDataMA_GetFacilityHouseM>
	{
		public MA_FacilityHouseMaster()
		{
			base.ID = MasterId.FACILITY_HOUSE;
		}

		public override string GetTableName()
		{
			return "facility_house_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_FacilityHouseMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_GetFacilityHouseM>(base.SetResponse)
			};
		}

		protected override void PrepareData(GameWebAPI.RespDataMA_GetFacilityHouseM src)
		{
			FarmDataManager.SetResponseData(src);
			this.data = src;
		}
	}
}
