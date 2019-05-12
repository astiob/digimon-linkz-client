using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_WorldStageMaster : MasterBaseData<GameWebAPI.RespDataMA_GetWorldDungeonM>
	{
		public MA_WorldStageMaster()
		{
			base.ID = MasterId.WORLD_DNG;
		}

		public override string GetTableName()
		{
			return "world_dungeon_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_WorldDungeonMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_GetWorldDungeonM>(base.SetResponse)
			};
		}
	}
}
