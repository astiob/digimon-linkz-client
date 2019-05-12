using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_WorldDungeonAdventureSceneMaster : MasterBaseData<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster>
	{
		public MA_WorldDungeonAdventureSceneMaster()
		{
			base.ID = MasterId.WORLD_DUNGEON_ADVENTURE_SCENE;
		}

		public override string GetTableName()
		{
			return "world_dungeon_adventure_scene_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestDataMA_WorldDungeonAdventureSceneMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster>(base.SetResponse)
			};
		}
	}
}
