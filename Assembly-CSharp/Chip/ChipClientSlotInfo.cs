using System;
using System.Collections.Generic;

namespace Chip
{
	public sealed class ChipClientSlotInfo
	{
		private GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.SlotInfo monsterSlotInfo;

		private List<ChipClientEquip> equipList;

		public ChipClientSlotInfo(GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.SlotInfo slotInfo)
		{
			this.monsterSlotInfo = slotInfo;
			this.equipList = new List<ChipClientEquip>();
			if (slotInfo.equip != null)
			{
				for (int i = 0; i < slotInfo.equip.Length; i++)
				{
					GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChip = ChipDataMng.GetUserChip(slotInfo.equip[i].userChipId);
					this.equipList.Add(new ChipClientEquip(slotInfo.equip[i], userChip.chipId));
				}
			}
		}

		public void AddChipEquip(ChipClientEquip chipEquip)
		{
			if (chipEquip == null)
			{
				return;
			}
			this.equipList.Add(chipEquip);
		}

		public void DeleteChipEquip(int userChipId)
		{
			for (int i = 0; i < this.equipList.Count; i++)
			{
				if (this.equipList[i].userChipId == userChipId)
				{
					this.equipList.Remove(this.equipList[i]);
					break;
				}
			}
		}

		public void DeleteAllChipEquip()
		{
			this.equipList.Clear();
		}

		public int UserMonsterId()
		{
			return this.monsterSlotInfo.userMonsterId;
		}

		public GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Manage GetSlotNum()
		{
			return this.monsterSlotInfo.manage;
		}

		public List<ChipClientEquip> GetChipEquip()
		{
			return this.equipList;
		}

		public ChipClientEquip FindChipEquip(int userChipId)
		{
			ChipClientEquip result = null;
			for (int i = 0; i < this.equipList.Count; i++)
			{
				if (this.equipList[i].userChipId == userChipId)
				{
					result = this.equipList[i];
					break;
				}
			}
			return result;
		}
	}
}
