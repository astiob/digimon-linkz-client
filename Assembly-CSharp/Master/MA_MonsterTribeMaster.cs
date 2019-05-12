using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_MonsterTribeMaster : MasterBaseData<GameWebAPI.RespDataMA_GetMonsterTribeM>
	{
		public MA_MonsterTribeMaster()
		{
			base.ID = MasterId.TRIBE;
		}

		public override string GetTableName()
		{
			return "monster_tribe_m";
		}

		public override RequestBase CreateRequest()
		{
			GameWebAPI.RequestMA_MonsterTribeMaster requestMA_MonsterTribeMaster = new GameWebAPI.RequestMA_MonsterTribeMaster();
			requestMA_MonsterTribeMaster.SetSendData = delegate(GameWebAPI.RequestMA_MonsterTribeM requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			requestMA_MonsterTribeMaster.OnReceived = new Action<GameWebAPI.RespDataMA_GetMonsterTribeM>(base.SetResponse);
			return requestMA_MonsterTribeMaster;
		}
	}
}
