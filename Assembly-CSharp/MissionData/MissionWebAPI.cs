using System;

namespace MissionData
{
	public class MissionWebAPI : ClassSingleton<MissionWebAPI>
	{
		public GameWebAPI.RespDataMS_MissionInfoLogic MissionInfoLogicData { get; set; }

		public APIRequestTask AccessMissionInfoLogicAPI()
		{
			GameWebAPI.MissionInfoLogic request = new GameWebAPI.MissionInfoLogic
			{
				OnReceived = delegate(GameWebAPI.RespDataMS_MissionInfoLogic response)
				{
					this.MissionInfoLogicData = response;
				}
			};
			return new APIRequestTask(request, true);
		}

		public GameWebAPI.RespDataMS_MissionRewardLogic MissionRewardLogicData { get; private set; }

		public APIRequestTask AccessMissionRewardLogicAPI(int missionId)
		{
			GameWebAPI.MissionRewardLogic request = new GameWebAPI.MissionRewardLogic
			{
				SetSendData = delegate(GameWebAPI.ReqDataUS_MissionRewardLogic param)
				{
					param.missionId = missionId;
				},
				OnReceived = delegate(GameWebAPI.RespDataMS_MissionRewardLogic response)
				{
					this.MissionRewardLogicData = response;
				}
			};
			return new APIRequestTask(request, true);
		}
	}
}
