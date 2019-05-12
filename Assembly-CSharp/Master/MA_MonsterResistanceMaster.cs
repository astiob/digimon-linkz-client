using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_MonsterResistanceMaster : MasterBaseData<GameWebAPI.RespDataMA_GetMonsterResistanceM>
	{
		public MA_MonsterResistanceMaster()
		{
			base.ID = MasterId.RESISTANCE;
		}

		public override string GetTableName()
		{
			return "monster_resistance_m";
		}

		public override RequestBase CreateRequest()
		{
			GameWebAPI.RequestMA_MonsterResistanceMaster requestMA_MonsterResistanceMaster = new GameWebAPI.RequestMA_MonsterResistanceMaster();
			requestMA_MonsterResistanceMaster.SetSendData = delegate(GameWebAPI.RequestMA_MonsterResistanceM requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			requestMA_MonsterResistanceMaster.OnReceived = new Action<GameWebAPI.RespDataMA_GetMonsterResistanceM>(base.SetResponse);
			return requestMA_MonsterResistanceMaster;
		}
	}
}
