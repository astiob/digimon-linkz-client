using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_MonsterGrowStepMaster : MasterBaseData<GameWebAPI.RespDataMA_GetMonsterGrowStepM>
	{
		public MA_MonsterGrowStepMaster()
		{
			base.ID = MasterId.GROWSTEP;
		}

		public override string GetTableName()
		{
			return "monster_grow_step_m";
		}

		public override RequestBase CreateRequest()
		{
			GameWebAPI.RequestMA_MonsterGrowStepMaster requestMA_MonsterGrowStepMaster = new GameWebAPI.RequestMA_MonsterGrowStepMaster();
			requestMA_MonsterGrowStepMaster.SetSendData = delegate(GameWebAPI.RequestMA_MonsterGrowStepM requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			requestMA_MonsterGrowStepMaster.OnReceived = new Action<GameWebAPI.RespDataMA_GetMonsterGrowStepM>(base.SetResponse);
			return requestMA_MonsterGrowStepMaster;
		}
	}
}
