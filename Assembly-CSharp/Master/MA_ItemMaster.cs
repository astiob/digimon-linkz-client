using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_ItemMaster : MasterBaseData<GameWebAPI.RespDataMA_GetItemM>
	{
		public MA_ItemMaster()
		{
			base.ID = MasterId.ITEM;
		}

		public override string GetTableName()
		{
			return "item_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_ItemMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_GetItemM>(base.SetResponse)
			};
		}
	}
}
