using System;
using System.Linq;

namespace ExchangeData
{
	public class ExchangeWebAPI : ClassSingleton<ExchangeWebAPI>
	{
		public GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result[] EventExchangeInfoList { get; set; }

		public string exchangeErrorCode { get; set; }

		public APIRequestTask AccessEventExchangeInfoLogicAPI()
		{
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
					this.EventExchangeInfoList = response.result;
				}
				else
				{
					this.EventExchangeInfoList = new GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result[0];
				}
			};
			GameWebAPI.EventExchangeInfoLogic request = eventExchangeInfoLogic;
			return new APIRequestTask(request, true);
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
	}
}
