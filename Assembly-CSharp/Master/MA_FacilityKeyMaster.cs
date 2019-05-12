using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_FacilityKeyMaster : MasterBaseData<GameWebAPI.RespDataMA_GetFacilityKeyM>
	{
		public MA_FacilityKeyMaster()
		{
			base.ID = MasterId.FACILITY_KEY;
		}

		public override string GetTableName()
		{
			return "facility_key_m";
		}

		public override RequestBase CreateRequest()
		{
			GameWebAPI.RequestMA_FacilityKeyMaster requestMA_FacilityKeyMaster = new GameWebAPI.RequestMA_FacilityKeyMaster();
			requestMA_FacilityKeyMaster.SetSendData = delegate(GameWebAPI.RequestMA_FacilityKeyM requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			requestMA_FacilityKeyMaster.OnReceived = new Action<GameWebAPI.RespDataMA_GetFacilityKeyM>(base.SetResponse);
			return requestMA_FacilityKeyMaster;
		}

		protected override void PrepareData(GameWebAPI.RespDataMA_GetFacilityKeyM src)
		{
			FarmDataManager.SetResponseData(src);
			this.data = src;
		}
	}
}
