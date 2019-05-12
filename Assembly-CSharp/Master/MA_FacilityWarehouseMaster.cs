using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_FacilityWarehouseMaster : MasterBaseData<GameWebAPI.RespDataMA_GetFacilityWarehouseM>
	{
		public MA_FacilityWarehouseMaster()
		{
			base.ID = MasterId.FACILITY_WAREHOUSE;
		}

		public override string GetTableName()
		{
			return "facility_warehouse_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_FacilityWarehouseMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_GetFacilityWarehouseM>(base.SetResponse)
			};
		}

		protected override void PrepareData(GameWebAPI.RespDataMA_GetFacilityWarehouseM src)
		{
			FarmDataManager.SetResponseData(src);
			this.data = src;
		}
	}
}
