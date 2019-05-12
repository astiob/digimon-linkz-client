using Master;
using System;

namespace MonsterList.TranceResistance
{
	public sealed class TranceResistanceIconGrayOut : MonsterListIconGrayOut
	{
		public void LockIconReturnDetailed(GUIMonsterIcon icon, bool isLock)
		{
			if (isLock)
			{
				icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.DISABLE);
				icon.SetTouchAct_S(this.blockStateAction.onTouch);
				icon.SetTouchAct_L(this.blockStateAction.onPress);
			}
			else
			{
				icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.ACTIVE);
				icon.SetTouchAct_S(this.normalStateAction.onTouch);
				icon.SetTouchAct_L(this.normalStateAction.onPress);
			}
		}

		public void SetSelect(GUIMonsterIcon icon)
		{
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.DISABLE);
			icon.SelectNum = 0;
			icon.SetTouchAct_S(this.selectedStateAction.onTouch);
			icon.SetTouchAct_L(this.selectedStateAction.onPress);
		}

		public void CancelSelect(GUIMonsterIcon icon)
		{
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.ACTIVE);
			icon.SelectNum = -1;
			icon.SetTouchAct_S(this.normalStateAction.onTouch);
			icon.SetTouchAct_L(this.normalStateAction.onPress);
		}

		public void BlockPartyUsed(GUIMonsterIcon icon)
		{
			icon.DimmMess = StringMaster.GetString("CharaIcon-04");
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.DISABLE);
			icon.SetTouchAct_S(this.blockStateAction.onTouch);
			icon.SetTouchAct_L(this.blockStateAction.onPress);
		}

		public void CancelBlockPartyUsed(GUIMonsterIcon icon)
		{
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.ACTIVE);
			icon.DimmMess = string.Empty;
			icon.SetTouchAct_S(this.normalStateAction.onTouch);
			icon.SetTouchAct_L(this.normalStateAction.onPress);
		}

		public void SetIdleIcon(GUIMonsterIcon icon)
		{
			icon.DimmMess = string.Empty;
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.ACTIVE);
			icon.SetTouchAct_S(this.normalStateAction.onTouch);
			icon.SetTouchAct_L(this.normalStateAction.onPress);
		}

		public void SetCanNotDecideIcon(GUIMonsterIcon icon)
		{
			icon.DimmMess = string.Empty;
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.DISABLE);
			icon.SetTouchAct_S(this.blockStateAction.onTouch);
			icon.SetTouchAct_L(this.blockStateAction.onPress);
		}

		public void SetPartnerIcon(GUIMonsterIcon icon)
		{
			icon.SelectNum = 1;
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.DISABLE);
			icon.SetTouchAct_S(this.selectedStateAction.onTouch);
			icon.SetTouchAct_L(this.selectedStateAction.onPress);
		}

		public void BlockLockIcon(GUIMonsterIcon icon)
		{
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.DISABLE);
			icon.SetTouchAct_S(this.blockStateAction.onTouch);
			icon.SetTouchAct_L(this.blockStateAction.onPress);
		}

		public void SelectIcon(GUIMonsterIcon icon)
		{
			icon.SetTouchAct_S(this.selectedStateAction.onTouch);
			icon.SetTouchAct_L(this.selectedStateAction.onPress);
		}
	}
}
