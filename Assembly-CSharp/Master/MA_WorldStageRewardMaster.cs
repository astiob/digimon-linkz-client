using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_WorldStageRewardMaster : MasterBaseData<GameWebAPI.RespDataMA_GetWorldDungeonRewardM>
	{
		public MA_WorldStageRewardMaster()
		{
			base.ID = MasterId.WORLD_DUNGEON_REWARD;
		}

		public override string GetTableName()
		{
			return "world_dungeon_reward_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_WorldDungeonRewardMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_GetWorldDungeonRewardM>(base.SetResponse)
			};
		}
	}
}
