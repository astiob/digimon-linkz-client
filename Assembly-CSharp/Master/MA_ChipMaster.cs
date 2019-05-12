using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_ChipMaster : MasterBaseData<GameWebAPI.RespDataMA_ChipM>
	{
		public MA_ChipMaster()
		{
			base.ID = MasterId.CHIP;
		}

		public override string GetTableName()
		{
			return "chip_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_ChipMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_ChipM>(base.SetResponse)
			};
		}
	}
}
