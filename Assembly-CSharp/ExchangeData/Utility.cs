using System;

namespace ExchangeData
{
	public static class Utility
	{
		public static GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result GetNowResult(GameWebAPI.RespDataMS_EventExchangeInfoLogic info, DateTime now)
		{
			foreach (GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result result2 in info.result)
			{
				DateTime t = DateTime.Parse(result2.startTime);
				DateTime t2 = DateTime.Parse(result2.endTime);
				if (DateTime.Compare(t, now) < 0 && DateTime.Compare(t2, now) > 0)
				{
					return result2;
				}
			}
			return null;
		}

		public static GameWebAPI.RespDataMA_GetItemM.ItemM GetUseExchangeItem(GameWebAPI.RespDataMS_EventExchangeInfoLogic.Result.Detail.Item consumeItem, GameWebAPI.RespDataMA_GetItemM.ItemM[] itemList)
		{
			foreach (GameWebAPI.RespDataMA_GetItemM.ItemM itemM in itemList)
			{
				if (itemM.itemId == consumeItem.assetValue)
				{
					return itemM;
				}
			}
			return null;
		}
	}
}
