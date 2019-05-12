using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_SkillDetailMaster : MasterBaseData<GameWebAPI.RespDataMA_GetSkillDetailM>
	{
		public MA_SkillDetailMaster()
		{
			base.ID = MasterId.SKILL_DETAIL;
		}

		public override string GetTableName()
		{
			return "skill_detail_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestMA_SkillDetailMaster
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_GetSkillDetailM>(base.SetResponse)
			};
		}
	}
}
