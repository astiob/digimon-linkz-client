using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_FacilityConditionMaster : MasterBaseData<GameWebAPI.RespDataMA_FacilityConditionM>
	{
		public MA_FacilityConditionMaster()
		{
			base.ID = MasterId.FACILITY_CONDITION;
		}

		public override string GetTableName()
		{
			return "facility_condition_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_FacilityConditionM
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_FacilityConditionM>(base.SetResponse)
			};
		}

		protected override void PrepareData(GameWebAPI.RespDataMA_FacilityConditionM src)
		{
			FarmDataManager.SetResponseData(src);
			this.data = src;
		}
	}
}
