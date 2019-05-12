using Monster;
using System;

namespace Chip
{
	public static class ChipAPIRequest
	{
		public static GameWebAPI.MonsterSlotInfoListLogic RequestAPIMonsterSlotInfo(int[] userMonsterIdList)
		{
			return new GameWebAPI.MonsterSlotInfoListLogic
			{
				SetSendData = delegate(GameWebAPI.ReqDataCS_MonsterSlotInfoListLogic param)
				{
					param.userMonsterId = userMonsterIdList;
				},
				OnReceived = delegate(GameWebAPI.RespDataCS_MonsterSlotInfoListLogic response)
				{
					if (response != null)
					{
						ChipDataMng.GetUserChipSlotData().UpdateMonsterSlotList(response.slotInfo);
						if (userMonsterIdList == null || userMonsterIdList.Length == 0)
						{
							ClassSingleton<MonsterUserDataMng>.Instance.RefreshMonsterSlot();
						}
						else
						{
							ClassSingleton<MonsterUserDataMng>.Instance.RefreshMonsterSlot(userMonsterIdList);
						}
					}
				}
			};
		}

		public static APIRequestTask RequestAPIMonsterSlotInfoList(int[] userMonsterIdList, bool requestRetry = true)
		{
			GameWebAPI.MonsterSlotInfoListLogic request = ChipAPIRequest.RequestAPIMonsterSlotInfo(userMonsterIdList);
			return new APIRequestTask(request, requestRetry);
		}
	}
}
