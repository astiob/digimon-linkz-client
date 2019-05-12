using System;
using WebAPIRequest;

namespace Master
{
	public sealed class MA_EventPointAchieveRewardMaster : MasterBaseData<GameWebAPI.RespDataMA_EventPointAchieveRewardM>
	{
		public MA_EventPointAchieveRewardMaster()
		{
			base.ID = MasterId.EVENT_POINT_REWARD;
		}

		public override string GetTableName()
		{
			return "event_point_achieve_reward_m";
		}

		public override RequestBase CreateRequest()
		{
			return new GameWebAPI.RequestDataMA_EventPointAchieveRewardM
			{
				OnReceived = new Action<GameWebAPI.RespDataMA_EventPointAchieveRewardM>(base.SetResponse)
			};
		}
	}
}
