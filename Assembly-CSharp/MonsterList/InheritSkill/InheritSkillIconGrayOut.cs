using Master;
using System;

namespace MonsterList.InheritSkill
{
	public sealed class InheritSkillIconGrayOut : MonsterListIconGrayOut
	{
		public void SetSelect(GUIMonsterIcon icon)
		{
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.DISABLE);
			icon.SelectNum = 0;
			icon.SetTouchAct_S(this.selectedStateAction.onTouch);
			icon.SetTouchAct_L(this.selectedStateAction.onPress);
		}

		public void SetSelectPartnerIcon(GUIMonsterIcon icon)
		{
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.DISABLE);
			icon.SelectNum = 1;
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

		public void ResetIcon(GUIMonsterIcon icon)
		{
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.ACTIVE);
			icon.DimmMess = string.Empty;
			icon.SetTouchAct_S(this.normalStateAction.onTouch);
			icon.SetTouchAct_L(this.normalStateAction.onPress);
		}

		public void BlockLockIcon(GUIMonsterIcon icon)
		{
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.DISABLE);
			icon.SetTouchAct_S(this.blockStateAction.onTouch);
			icon.SetTouchAct_L(this.blockStateAction.onPress);
		}

		public void SetLockReturnDetailed(GUIMonsterIcon icon, bool isLock)
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
	}
}
