using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_ColosseumRankMaster : MasterBaseData<GameWebAPI.RespDataMA_ColosseumRankM>
	{
		public MA_ColosseumRankMaster()
		{
			base.ID = MasterId.COLOSSEUM_RANK;
		}

		public override string GetTableName()
		{
			return "colosseum_rank_m";
		}

		public override RequestBase CreateRequest()
		{
			GameWebAPI.RequestMA_ColosseumRankMaster requestMA_ColosseumRankMaster = new GameWebAPI.RequestMA_ColosseumRankMaster();
			requestMA_ColosseumRankMaster.SetSendData = delegate(GameWebAPI.RequestMA_ColosseumRankM requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			requestMA_ColosseumRankMaster.OnReceived = new Action<GameWebAPI.RespDataMA_ColosseumRankM>(base.SetResponse);
			return requestMA_ColosseumRankMaster;
		}
	}
}
