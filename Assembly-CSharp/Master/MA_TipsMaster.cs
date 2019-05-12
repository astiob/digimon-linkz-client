using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_TipsMaster : MasterBaseData<GameWebAPI.RespDataMA_GetTipsM>
	{
		public MA_TipsMaster()
		{
			base.ID = MasterId.TIPS;
		}

		public override string GetTableName()
		{
			return "tips_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_TipsMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_GetTipsM>(base.SetResponse)
			};
		}

		protected override void PrepareData(GameWebAPI.RespDataMA_GetTipsM src)
		{
			src.Initialize();
			this.data = src;
		}
	}
}
