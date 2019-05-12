using System;
using System.Collections;
using System.Collections.Generic;

namespace MissionData
{
	public class MissionWebAPI : ClassSingleton<MissionWebAPI>
	{
		public GameWebAPI.RespDataMS_MissionInfoLogic MissionInfoLogicData { get; set; }

		public APIRequestTask AccessMissionInfoLogicAPI()
		{
			GameWebAPI.MissionInfoLogic missionInfoLogic = new GameWebAPI.MissionInfoLogic();
			missionInfoLogic.SetSendData = delegate(GameWebAPI.SendDataMS_MissionInfoLogic requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			missionInfoLogic.OnReceived = delegate(GameWebAPI.RespDataMS_MissionInfoLogic response)
			{
				this.MissionInfoLogicData = response;
				IEnumerator enumerator = Enum.GetValues(typeof(Type.DisplayGroup)).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						Type.DisplayGroup displayGroup = (Type.DisplayGroup)obj;
						GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission[] array = (GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission[])typeof(GameWebAPI.RespDataMS_MissionInfoLogic.Result).GetField(displayGroup.ToString()).GetValue(this.MissionInfoLogicData.result);
						foreach (GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission mission in array)
						{
							GameWebAPI.RespDataMS_MissionInfoLogic.Result.Mission mission2 = mission;
							int num = (int)displayGroup;
							mission2.displayGroup = num.ToString();
						}
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
			};
			GameWebAPI.MissionInfoLogic request = missionInfoLogic;
			return new APIRequestTask(request, true);
		}

		public GameWebAPI.RespDataMS_MissionRewardLogic MissionRewardLogicData { get; private set; }

		public APIRequestTask AccessMissionRewardLogicAPI(int missionId)
		{
			GameWebAPI.MissionRewardLogic request = new GameWebAPI.MissionRewardLogic
			{
				SetSendData = delegate(GameWebAPI.ReqDataUS_MissionRewardLogic param)
				{
					param.missionId = new int[]
					{
						missionId
					};
				},
				OnReceived = delegate(GameWebAPI.RespDataMS_MissionRewardLogic response)
				{
					this.MissionRewardLogicData = response;
				}
			};
			return new APIRequestTask(request, true);
		}

		public APIRequestTask AccessMissionRewardLogicAPI(List<MissionItem> missionItem)
		{
			int[] missionIds = new int[missionItem.Count];
			for (int i = 0; i < missionItem.Count; i++)
			{
				missionIds[i] = missionItem[i].missionId;
			}
			GameWebAPI.MissionRewardLogic request = new GameWebAPI.MissionRewardLogic
			{
				SetSendData = delegate(GameWebAPI.ReqDataUS_MissionRewardLogic param)
				{
					param.missionId = missionIds;
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
