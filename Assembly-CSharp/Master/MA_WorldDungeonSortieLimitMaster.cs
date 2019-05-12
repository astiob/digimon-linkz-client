using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_WorldDungeonSortieLimitMaster : MasterBaseData<GameWebAPI.RespDataMA_WorldDungeonSortieLimit>
	{
		public MA_WorldDungeonSortieLimitMaster()
		{
			base.ID = MasterId.WORLD_DUNGEON_SORTIE_LIMIT;
		}

		public override string GetTableName()
		{
			return "world_dungeon_sortie_limit_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_WorldDungeonSortieLimit
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_WorldDungeonSortieLimit>(base.SetResponse)
			};
		}
	}
}
