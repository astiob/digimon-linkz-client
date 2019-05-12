using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_FacilityMeatFieldMaster : MasterBaseData<GameWebAPI.RespDataMA_GetFacilityMeatFieldM>
	{
		public MA_FacilityMeatFieldMaster()
		{
			base.ID = MasterId.FACILITY_MEAT;
		}

		public override string GetTableName()
		{
			return "facility_meat_field_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_FacilityMeatFieldMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_GetFacilityMeatFieldM>(base.SetResponse)
			};
		}

		protected override void PrepareData(GameWebAPI.RespDataMA_GetFacilityMeatFieldM src)
		{
			FarmDataManager.SetResponseData(src);
			this.data = src;
		}
	}
}
