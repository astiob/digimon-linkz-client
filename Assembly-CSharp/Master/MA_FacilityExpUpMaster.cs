using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_FacilityExpUpMaster : MasterBaseData<GameWebAPI.RespDataMA_GetFacilityExpUpM>
	{
		public MA_FacilityExpUpMaster()
		{
			base.ID = MasterId.FACILITY_EXPUP;
		}

		public override string GetTableName()
		{
			return "facility_exp_up_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_FacilityExpUpMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_GetFacilityExpUpM>(base.SetResponse)
			};
		}

		protected override void PrepareData(GameWebAPI.RespDataMA_GetFacilityExpUpM src)
		{
			FarmDataManager.SetResponseData(src);
			this.data = src;
		}
	}
}
