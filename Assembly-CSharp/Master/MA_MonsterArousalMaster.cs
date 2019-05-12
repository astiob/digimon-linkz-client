using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_MonsterArousalMaster : MasterBaseData<GameWebAPI.RespDataMA_MonsterArousalMaster>
	{
		public MA_MonsterArousalMaster()
		{
			base.ID = MasterId.MONSTER_AROUSAL;
		}

		public override string GetTableName()
		{
			return "monster_rare_m";
		}

		public override RequestBase CreateRequest()
		{
			GameWebAPI.RequestMonsterArousalMaster requestMonsterArousalMaster = new GameWebAPI.RequestMonsterArousalMaster();
			requestMonsterArousalMaster.SetSendData = delegate(GameWebAPI.RequestMonsterArousalM requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			requestMonsterArousalMaster.OnReceived = new Action<GameWebAPI.RespDataMA_MonsterArousalMaster>(base.SetResponse);
			return requestMonsterArousalMaster;
		}
	}
}
