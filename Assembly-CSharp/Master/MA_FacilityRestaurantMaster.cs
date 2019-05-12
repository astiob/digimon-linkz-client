using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_FacilityRestaurantMaster : MasterBaseData<GameWebAPI.RespDataMA_GetFacilityRestaurantM>
	{
		public MA_FacilityRestaurantMaster()
		{
			base.ID = MasterId.FACILITY_RESTAURANT;
		}

		public override string GetTableName()
		{
			return "facility_restaurant_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_FacilityRestaurantMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_GetFacilityRestaurantM>(base.SetResponse)
			};
		}

		protected override void PrepareData(GameWebAPI.RespDataMA_GetFacilityRestaurantM src)
		{
			FarmDataManager.SetResponseData(src);
			this.data = src;
		}
	}
}
