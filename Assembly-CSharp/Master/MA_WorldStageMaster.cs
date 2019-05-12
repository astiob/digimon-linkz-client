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
			GameWebAPI.RequestMA_WorldDungeonMaster requestMA_WorldDungeonMaster = new GameWebAPI.RequestMA_WorldDungeonMaster();
			requestMA_WorldDungeonMaster.SetSendData = delegate(GameWebAPI.RequestMA_WorldDungeonM requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			requestMA_WorldDungeonMaster.OnReceived = new Action<GameWebAPI.RespDataMA_GetWorldDungeonM>(base.SetResponse);
			return requestMA_WorldDungeonMaster;
		}
	}
}
