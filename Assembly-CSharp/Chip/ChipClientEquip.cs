using System;

namespace Chip
{
	public sealed class ChipClientEquip : GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Equip
	{
		public int chipId;

		public ChipClientEquip(int chipId)
		{
			this.chipId = chipId;
		}

		public ChipClientEquip(GameWebAPI.RespDataCS_MonsterSlotInfoListLogic.Equip equip, int chipId)
		{
			this.dispNum = equip.dispNum;
			this.type = equip.type;
			this.userChipId = equip.userChipId;
			this.chipId = chipId;
		}

		public ChipClientEquip(GameWebAPI.ReqDataCS_ChipEquipLogic equipResult, int chipId)
		{
			this.dispNum = equipResult.dispNum;
			this.type = equipResult.type;
			this.userChipId = equipResult.userChipId;
			this.chipId = chipId;
		}
	}
}
