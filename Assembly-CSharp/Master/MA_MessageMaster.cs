using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_MessageMaster : MasterBaseData<GameWebAPI.RespDataMA_MessageM>
	{
		public MA_MessageMaster()
		{
			base.ID = MasterId.MESSAGE_MASTER;
		}

		public override string GetTableName()
		{
			return "message_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_MessageMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_MessageM>(base.SetResponse)
			};
		}
	}
}
