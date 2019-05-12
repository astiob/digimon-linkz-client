using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_MonsterSpecificTypeMaster : MasterBaseData<GameWebAPI.RespDataMA_MonsterSpecificTypeMaster>
	{
		public MA_MonsterSpecificTypeMaster()
		{
			base.ID = MasterId.MONSTER_SPECIFIC_TYPE;
		}

		public override string GetTableName()
		{
			return "monster_status_m";
		}

		public override RequestBase CreateRequest()
		{
			GameWebAPI.RequestMonsterSpecificTypeMaster requestMonsterSpecificTypeMaster = new GameWebAPI.RequestMonsterSpecificTypeMaster();
			requestMonsterSpecificTypeMaster.SetSendData = delegate(GameWebAPI.RequestMonsterSpecificTypeM requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			requestMonsterSpecificTypeMaster.OnReceived = new Action<GameWebAPI.RespDataMA_MonsterSpecificTypeMaster>(base.SetResponse);
			return requestMonsterSpecificTypeMaster;
		}
	}
}
