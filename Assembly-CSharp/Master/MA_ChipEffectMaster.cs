using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_ChipEffectMaster : MasterBaseData<GameWebAPI.RespDataMA_ChipEffectM>
	{
		public MA_ChipEffectMaster()
		{
			base.ID = MasterId.CHIP_EFFECT;
		}

		public override string GetTableName()
		{
			return "chip_effect_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_ChipEffectMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_ChipEffectM>(base.SetResponse)
			};
		}
	}
}
