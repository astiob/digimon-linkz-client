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
					foreach (object obj in Enum.GetValues(typeof(Type.DisplayGroup)))
					{
						Type.DisplayGroup displayGroup = (Type.DisplayGroup)((int)obj);
						GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission[] array = (GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission[])typeof(GameWebAPI.RespDataMS_MissionInfoLogic.Result).GetField(displayGroup.ToString()).GetValue(this.MissionInfoLogicData.result);
						foreach (GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission mission in array)
						{
							GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission mission2 = mission;
							int num = (int)displayGroup;
							mission2.displayGroup = num.ToString();
						}
					}
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
