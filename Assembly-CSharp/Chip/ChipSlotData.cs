using System;
using System.Collections.Generic;

namespace Chip
{
	public sealed class ChipSlotData
	{
		private Dictionary<int, ChipClientSlotInfo> monsterSlotInfoList;

		public void Initialize()
		{
			if (this.monsterSlotInfoList == null)
			{
				this.monsterSlotInfoList = new Dictionary<int, ChipClientSlotInfo>();
			}
			else
			{
				this.monsterSlotInfoList.Clear();
			}
		}

		public void SetMonsterSlotList(GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.SlotInfo[] slotInfoList)
		{
			if (slotInfoList == null)
			{
				return;
			}
			this.monsterSlotInfoList.Clear();
			for (int i = 0; i < slotInfoList.Length; i++)
			{
				ChipClientSlotInfo value = new ChipClientSlotInfo(slotInfoList[i]);
				this.monsterSlotInfoList.Add(slotInfoList[i].userMonsterId, value);
			}
		}

		public void AddMonsterSlotList(GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.SlotInfo[] slotInfoList)
		{
			if (slotInfoList == null)
			{
				return;
			}
			for (int i = 0; i < slotInfoList.Length; i++)
			{
				ChipClientSlotInfo value = new ChipClientSlotInfo(slotInfoList[i]);
				this.monsterSlotInfoList.Add(slotInfoList[i].userMonsterId, value);
			}
		}

		public void UpdateMonsterSlotList(GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.SlotInfo[] slotInfoList)
		{
			if (slotInfoList == null)
			{
				return;
			}
			for (int i = 0; i < slotInfoList.Length; i++)
			{
				this.monsterSlotInfoList.Remove(slotInfoList[i].userMonsterId);
				ChipClientSlotInfo value = new ChipClientSlotInfo(slotInfoList[i]);
				this.monsterSlotInfoList.Add(slotInfoList[i].userMonsterId, value);
			}
		}

		public void DeleteMonsterSlotList(string[] userMonsterIdList)
		{
			if (userMonsterIdList == null)
			{
				return;
			}
			for (int i = 0; i < userMonsterIdList.Length; i++)
			{
				this.DeleteMonsterSlot(userMonsterIdList[i]);
			}
		}

		public void DeleteMonsterSlot(string userMonsterId)
		{
			if (string.IsNullOrEmpty(userMonsterId))
			{
				return;
			}
			int key = int.Parse(userMonsterId);
			this.monsterSlotInfoList.Remove(key);
		}

		public ChipClientSlotInfo GetSlotInfo(string userMonsterId)
		{
			ChipClientSlotInfo result = null;
			int key = int.Parse(userMonsterId);
			this.monsterSlotInfoList.TryGetValue(key, out result);
			return result;
		}
	}
}
