using System;
using System.Linq;

namespace ExchangeData
{
	public class ExchangeWebAPI : ClassSingleton<ExchangeWebAPI>
	{
		public GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result[] EventExchangeInfoLogicData { get; set; }

		public GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result[] EventExchangeInfoLogicAlwaysData { get; set; }

		public string exchangeErrorCode { get; set; }

		public APIRequestTask AccessEventExchangeInfoLogicAPI()
		{
			APIRequestTask apirequestTask = new APIRequestTask();
			GameWebAPI.EventExchangeInfoLogic eventExchangeInfoLogic = new GameWebAPI.EventExchangeInfoLogic();
			eventExchangeInfoLogic.SetSendData = delegate(GameWebAPI.ReqDataUS_EventExchangeInfoLogic requestParam)
			{
				int countryCode = int.Parse(CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN));
				requestParam.countryCode = countryCode;
			};
			eventExchangeInfoLogic.OnReceived = delegate(GameWebAPI.RespDataMS_EventExchangeInfoLogic response)
			{
				if (response.result != null && response.result.Count<GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result>() > 0)
				{
					this.EventExchangeInfoLogicData = response.result.ToArray<GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result>();
					this.EventExchangeInfoLogicAlwaysData = response.result.Where((GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result x) => int.Parse(x.type) == 1).ToArray<GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result>();
				}
			};
			GameWebAPI.EventExchangeInfoLogic request = eventExchangeInfoLogic;
			apirequestTask.Add(new APIRequestTask(request, true));
			return apirequestTask;
		}

		public APIRequestTask AccessEventExchangeLogicAPI(int detailId, int exchangeNum)
		{
			GameWebAPI.EventExchangeLogic request = new GameWebAPI.EventExchangeLogic
			{
				SetSendData = delegate(GameWebAPI.ReqDataUS_EventExchangeLogic param)
				{
					param.eventExchangeDetailId = detailId;
					param.exchangeNum = exchangeNum;
				},
				OnReceived = delegate(GameWebAPI.RespEventExchangeLogic response)
				{
					this.exchangeErrorCode = string.Empty;
					if (response.resultStatus == 0)
					{
						this.exchangeErrorCode = response.errorCode;
					}
				}
			};
			return new APIRequestTask(request, true);
		}

		public bool IsExistAlwaysExchangeInfo(GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result info)
		{
			foreach (GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result result in this.EventExchangeInfoLogicAlwaysData)
			{
				if (result == info)
				{
					return true;
				}
			}
			return false;
		}

		public void DeleteExchangeInfoLogicResult(string eventExchangeId)
		{
			for (int i = 0; i < this.EventExchangeInfoLogicData.Length; i++)
			{
				if (this.EventExchangeInfoLogicData[i].eventExchangeId == eventExchangeId)
				{
					Array.Clear(this.EventExchangeInfoLogicData, i, 1);
					break;
				}
			}
		}
	}
}
