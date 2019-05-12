using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_WorldStageOptionRewardMaster : MasterBaseData<GameWebAPI.RespDataMA_GetWorldDungeonOptionRewardM>
	{
		public MA_WorldStageOptionRewardMaster()
		{
			base.ID = MasterId.WORLD_DUNGEON_OPTION_REWARD;
		}

		public override string GetTableName()
		{
			return "world_dungeon_option_reward_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_WorldDungeonOptionRewardMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_GetWorldDungeonOptionRewardM>(base.SetResponse)
			};
		}
	}
}
