using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_FacilityMaster : MasterBaseData<GameWebAPI.RespDataMA_GetFacilityM>
	{
		public MA_FacilityMaster()
		{
			base.ID = MasterId.FACILITY;
		}

		public override string GetTableName()
		{
			return "facility_m";
		}

		public override RequestBase CreateRequest()
		{
			GameWebAPI.RequestMA_FacilityMaster requestMA_FacilityMaster = new GameWebAPI.RequestMA_FacilityMaster();
			requestMA_FacilityMaster.SetSendData = delegate(GameWebAPI.RequestMA_FacilityM requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			requestMA_FacilityMaster.OnReceived = new Action<GameWebAPI.RespDataMA_GetFacilityM>(base.SetResponse);
			return requestMA_FacilityMaster;
		}

		protected override void PrepareData(GameWebAPI.RespDataMA_GetFacilityM src)
		{
			FarmDataManager.SetResponseData(src);
			this.data = src;
		}
	}
}
