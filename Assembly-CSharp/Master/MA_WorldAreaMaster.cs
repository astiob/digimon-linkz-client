using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_WorldAreaMaster : MasterBaseData<GameWebAPI.RespDataMA_GetWorldStageM>
	{
		public MA_WorldAreaMaster()
		{
			base.ID = MasterId.WORLD_STAGE;
		}

		public override string GetTableName()
		{
			return "world_stage_m";
		}

		public override RequestBase CreateRequest()
		{
			GameWebAPI.RequestMA_WorldStageMaster requestMA_WorldStageMaster = new GameWebAPI.RequestMA_WorldStageMaster();
			requestMA_WorldStageMaster.SetSendData = delegate(GameWebAPI.RequestMA_WorldStageM requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			requestMA_WorldStageMaster.OnReceived = new Action<GameWebAPI.RespDataMA_GetWorldStageM>(base.SetResponse);
			return requestMA_WorldStageMaster;
		}
	}
}
