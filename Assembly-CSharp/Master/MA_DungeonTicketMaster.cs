using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_DungeonTicketMaster : MasterBaseData<GameWebAPI.RespDataMA_DungeonTicketMaster>
	{
		public MA_DungeonTicketMaster()
		{
			base.ID = MasterId.DUNGEON_TICKET;
		}

		public override string GetTableName()
		{
			return "dungeon_ticket_m";
		}

		public override RequestBase CreateRequest()
		{
			GameWebAPI.RequestDataMA_DungeonTicketMaster requestDataMA_DungeonTicketMaster = new GameWebAPI.RequestDataMA_DungeonTicketMaster();
			requestDataMA_DungeonTicketMaster.SetSendData = delegate(GameWebAPI.RequestDataMA_DungeonTicketM requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			requestDataMA_DungeonTicketMaster.OnReceived = new Action<GameWebAPI.RespDataMA_DungeonTicketMaster>(base.SetResponse);
			return requestDataMA_DungeonTicketMaster;
		}
	}
}
