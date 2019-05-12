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
			return new GameWebAPI.RequestDataMA_DungeonTicketMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_DungeonTicketMaster>(base.SetResponse)
			};
		}
	}
}
