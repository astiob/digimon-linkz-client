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
			GameWebAPI.RequestDataMA_WorldDungeonAdventureSceneMaster requestDataMA_WorldDungeonAdventureSceneMaster = new GameWebAPI.RequestDataMA_WorldDungeonAdventureSceneMaster();
			requestDataMA_WorldDungeonAdventureSceneMaster.SetSendData = delegate(GameWebAPI.RequestDataMA_WorldDungeonAdventureSceneM requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			requestDataMA_WorldDungeonAdventureSceneMaster.OnReceived = new Action<GameWebAPI.RespDataMA_WorldDungeonAdventureSceneMaster>(base.SetResponse);
			return requestDataMA_WorldDungeonAdventureSceneMaster;
		}
	}
}
