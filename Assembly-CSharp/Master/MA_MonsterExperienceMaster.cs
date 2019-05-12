using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_MonsterExperienceMaster : MasterBaseData<GameWebAPI.RespDataMA_GetMonsterExperienceM>
	{
		public MA_MonsterExperienceMaster()
		{
			base.ID = MasterId.MONSTER_EXPERIENCE;
		}

		public override string GetTableName()
		{
			return "monster_experience_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_MonsterExperienceMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_GetMonsterExperienceM>(base.SetResponse)
			};
		}
	}
}
