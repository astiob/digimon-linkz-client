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
			GameWebAPI.RequestMA_FacilityConditionMaster requestMA_FacilityConditionMaster = new GameWebAPI.RequestMA_FacilityConditionMaster();
			requestMA_FacilityConditionMaster.SetSendData = delegate(GameWebAPI.RequestMA_FacilityConditionM requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			requestMA_FacilityConditionMaster.OnReceived = new Action<GameWebAPI.RespDataMA_FacilityConditionM>(base.SetResponse);
			return requestMA_FacilityConditionMaster;
		}

		protected override void PrepareData(GameWebAPI.RespDataMA_FacilityConditionM src)
		{
			FarmDataManager.SetResponseData(src);
			this.data = src;
		}
	}
}
