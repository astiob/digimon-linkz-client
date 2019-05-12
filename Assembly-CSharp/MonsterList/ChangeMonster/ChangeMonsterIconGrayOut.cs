using Master;
using System;

namespace MonsterList.ChangeMonster
{
	public sealed class ChangeMonsterIconGrayOut : MonsterListIconGrayOut
	{
		public void CancelSelect(GUIMonsterIcon icon)
		{
			icon.DimmMess = string.Empty;
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.ACTIVE);
			icon.SetTouchAct_S(this.normalStateAction.onTouch);
			icon.SetTouchAct_L(this.normalStateAction.onPress);
		}

		public void SetSelect(GUIMonsterIcon icon)
		{
			icon.DimmMess = StringMaster.GetString("SystemSelect");
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.DISABLE);
			icon.SetTouchAct_S(this.selectedStateAction.onTouch);
			icon.SetTouchAct_L(this.selectedStateAction.onPress);
		}

		public void BlockPartyUsed(GUIMonsterIcon icon)
		{
			icon.DimmMess = StringMaster.GetString("CharaIcon-04");
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.DISABLE);
			icon.SetTouchAct_S(this.blockStateAction.onTouch);
			icon.SetTouchAct_L(this.blockStateAction.onPress);
		}

		public void SetPartyUsed(GUIMonsterIcon icon)
		{
			icon.DimmMess = StringMaster.GetString("CharaIcon-04");
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.NOTACTIVE);
		}
	}
}
