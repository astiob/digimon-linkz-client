using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_TipsMaster : MasterBaseData<GameWebAPI.RespDataMA_GetTipsM>
	{
		public MA_TipsMaster()
		{
			base.ID = MasterId.TIPS;
		}

		public override string GetTableName()
		{
			return "tips_m";
		}

		public override RequestBase CreateRequest()
		{
			GameWebAPI.RequestMA_TipsMaster requestMA_TipsMaster = new GameWebAPI.RequestMA_TipsMaster();
			requestMA_TipsMaster.SetSendData = delegate(GameWebAPI.RequestMA_TipsM requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			requestMA_TipsMaster.OnReceived = new Action<GameWebAPI.RespDataMA_GetTipsM>(base.SetResponse);
			return requestMA_TipsMaster;
		}

		protected override void PrepareData(GameWebAPI.RespDataMA_GetTipsM src)
		{
			src.Initialize();
			this.data = src;
		}
	}
}
