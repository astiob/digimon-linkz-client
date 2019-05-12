using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_FacilityChipMaster : MasterBaseData<GameWebAPI.RespDataMA_GetFacilityChipM>
	{
		public MA_FacilityChipMaster()
		{
			base.ID = MasterId.FACILITY_CHIP;
		}

		public override string GetTableName()
		{
			return "facility_chip_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_FacilityChipMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_GetFacilityChipM>(base.SetResponse)
			};
		}

		protected override void PrepareData(GameWebAPI.RespDataMA_GetFacilityChipM src)
		{
			FarmDataManager.SetResponseData(src);
			this.data = src;
		}
	}
}
