using Chip;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Monster
{
	public sealed class MonsterChipEquipData
	{
		private ChipClientSlotInfo monsterSlotInfo;

		public void SetChipEquip(string userMonsterId)
		{
			this.monsterSlotInfo = ChipDataMng.GetUserChipSlotData().GetSlotInfo(userMonsterId);
		}

		public void SetEmptyChipEquip(string userMonsterId)
		{
			GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.SlotInfo slotInfo = new GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.SlotInfo
			{
				userMonsterId = int.Parse(userMonsterId)
			};
			this.monsterSlotInfo = new ChipClientSlotInfo(slotInfo);
		}

		public void UpdateEquipList(GameWebAPI.ReqDataCS_ChipEquipLogic equip)
		{
			if (equip == null)
			{
				return;
			}
			if (equip.act == 1)
			{
				GameWebAPI.RespDataCS_ChipListLogic.UserChipList userChip = ChipDataMng.GetUserChip(equip.userChipId);
				ChipClientEquip chipClientEquip = new ChipClientEquip(equip, userChip.chipId);
				chipClientEquip.chipId = userChip.chipId;
				this.monsterSlotInfo.AddChipEquip(chipClientEquip);
			}
			else
			{
				this.monsterSlotInfo.DeleteChipEquip(equip.userChipId);
			}
		}

		public bool IsAttachedChip()
		{
			return 0 < this.monsterSlotInfo.GetChipEquip().Count;
		}

		public void SetChipIdList(int[] idList)
		{
			if (idList == null)
			{
				return;
			}
			if (this.monsterSlotInfo == null)
			{
				this.monsterSlotInfo = new ChipClientSlotInfo(new GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.SlotInfo());
			}
			else
			{
				this.monsterSlotInfo.DeleteAllChipEquip();
			}
			for (int i = 0; i < idList.Length; i++)
			{
				if (0 < idList[i])
				{
					this.monsterSlotInfo.AddChipEquip(new ChipClientEquip(idList[i]));
				}
			}
		}

		public int[] GetChipIdList()
		{
			return this.monsterSlotInfo.GetChipEquip().Select((ChipClientEquip x) => x.chipId).ToArray<int>();
		}

		public List<string> GetChipGroupList()
		{
			List<string> list = null;
			List<ChipClientEquip> chipEquip = this.monsterSlotInfo.GetChipEquip();
			if (0 < chipEquip.Count)
			{
				list = new List<string>();
				for (int i = 0; i < chipEquip.Count; i++)
				{
					GameWebAPI.RespDataMA_ChipM.Chip chipMainData = ChipDataMng.GetChipMainData(chipEquip[i].chipId.ToString());
					if (chipMainData != null)
					{
						list.Add(chipMainData.chipGroupId);
					}
				}
			}
			return list;
		}

		public List<GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Equip> GetEquip()
		{
			List<GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Equip> list = new List<GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Equip>();
			List<ChipClientEquip> chipEquip = this.monsterSlotInfo.GetChipEquip();
			for (int i = 0; i < chipEquip.Count; i++)
			{
				list.Add(chipEquip[i]);
			}
			return list;
		}

		public GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Manage GetSlotStatus()
		{
			return this.monsterSlotInfo.GetSlotNum();
		}
	}
}
