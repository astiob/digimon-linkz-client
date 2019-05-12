using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_CodeMaster : MasterBaseData<GameWebAPI.RespDataMA_CodeM>
	{
		public MA_CodeMaster()
		{
			base.ID = MasterId.CODE_MASTER;
		}

		public override string GetTableName()
		{
			return "code_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_CodeMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_CodeM>(base.SetResponse)
			};
		}
	}
}
