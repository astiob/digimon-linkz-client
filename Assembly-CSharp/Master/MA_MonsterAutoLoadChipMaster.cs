using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_MonsterAutoLoadChipMaster : MasterBaseData<GameWebAPI.ResponseMonsterAutoLoadChipMaster>
	{
		public MA_MonsterAutoLoadChipMaster()
		{
			base.ID = MasterId.MONSTER_AUTO_LOAD_CHIP;
		}

		public override string GetTableName()
		{
			return "monster_auto_load_chip_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_MonsterAutoLoadChipMaster
			{
				OnReceived = new Action<GameWebAPI.ResponseMonsterAutoLoadChipMaster>(base.SetResponse)
			};
		}
	}
}
