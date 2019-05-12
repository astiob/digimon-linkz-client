using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_MonsterMasterOnlyGroupData : MasterBaseData<GameWebAPI.RespDataMA_GetMonsterMG>
	{
		public MA_MonsterMasterOnlyGroupData()
		{
			base.ID = MasterId.MONSTERG;
		}

		public override string GetTableName()
		{
			return "monster_m";
		}

		public override RequestBase CreateRequest()
		{
			GameWebAPI.RequestMA_MonsterMasterOnlyGroupData requestMA_MonsterMasterOnlyGroupData = new GameWebAPI.RequestMA_MonsterMasterOnlyGroupData();
			requestMA_MonsterMasterOnlyGroupData.SetSendData = delegate(GameWebAPI.RequestMA_MonsterM requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			requestMA_MonsterMasterOnlyGroupData.OnReceived = new Action<GameWebAPI.RespDataMA_GetMonsterMG>(base.SetResponse);
			return requestMA_MonsterMasterOnlyGroupData;
		}
	}
}
