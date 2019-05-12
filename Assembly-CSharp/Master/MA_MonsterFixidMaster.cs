using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_MonsterFixidMaster : MasterBaseData<GameWebAPI.RespDataMA_MonsterFixidM>
	{
		public MA_MonsterFixidMaster()
		{
			base.ID = MasterId.MONSTER_FIXID;
		}

		public override string GetTableName()
		{
			return "monster_fixed_value_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMonsterFixidMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_MonsterFixidM>(base.SetResponse)
			};
		}

		protected override void PrepareData(GameWebAPI.RespDataMA_MonsterFixidM src)
		{
			FarmDataManager.SetResponseData(src);
			this.data = src;
		}
	}
}
