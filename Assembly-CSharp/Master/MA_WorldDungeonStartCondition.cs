using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_WorldDungeonStartCondition : MasterBaseData<GameWebAPI.RespDataMA_WorldDungeonStartCondition>
	{
		public MA_WorldDungeonStartCondition()
		{
			base.ID = MasterId.WORLD_DUNGEON_START_CONDITION;
		}

		public override string GetTableName()
		{
			return "world_dungeon_start_condition_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_WorldDungeonStartCondition
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_WorldDungeonStartCondition>(base.SetResponse)
			};
		}
	}
}
