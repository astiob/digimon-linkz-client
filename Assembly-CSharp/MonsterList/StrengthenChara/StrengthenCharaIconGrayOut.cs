using Master;
using System;

namespace MonsterList.StrengthenChara
{
	public sealed class StrengthenCharaIconGrayOut : MonsterListIconGrayOut
	{
		public void ResetState(GUIMonsterIcon icon)
		{
			icon.DimmMess = string.Empty;
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.ACTIVE);
			icon.SetTouchAct_S(this.normalStateAction.onTouch);
			icon.SetTouchAct_L(this.normalStateAction.onPress);
		}

		public void SetSelect(GUIMonsterIcon icon)
		{
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.DISABLE);
			icon.SelectNum = 0;
			icon.SetTouchAct_S(this.selectedStateAction.onTouch);
			icon.SetTouchAct_L(this.selectedStateAction.onPress);
		}

		public void SetSelectPartner(GUIMonsterIcon icon, int iconNumber)
		{
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.DISABLE);
			icon.SelectNum = iconNumber;
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

		public void BlockInvalidIcon(GUIMonsterIcon icon)
		{
			icon.DimmMess = string.Empty;
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.DISABLE);
			icon.SetTouchAct_S(this.blockStateAction.onTouch);
			icon.SetTouchAct_L(this.blockStateAction.onPress);
		}

		public void BlockLockIcon(GUIMonsterIcon icon)
		{
			icon.DimmMess = string.Empty;
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.DISABLE);
			icon.SetTouchAct_S(this.blockStateAction.onTouch);
			icon.SetTouchAct_L(this.blockStateAction.onPress);
		}

		public void CancelLockIcon(GUIMonsterIcon icon)
		{
			icon.DimmMess = string.Empty;
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.ACTIVE);
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

		public void BlockLockIconReturnDetailed(GUIMonsterIcon icon)
		{
			icon.DimmMess = string.Empty;
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.DISABLE);
			icon.SetTouchAct_S(this.blockStateAction.onTouch);
			icon.SetTouchAct_L(this.blockStateAction.onPress);
		}

		public void CancelLockIconReturnDetailed(GUIMonsterIcon icon)
		{
			icon.DimmMess = string.Empty;
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.ACTIVE);
			icon.SetTouchAct_S(this.normalStateAction.onTouch);
			icon.SetTouchAct_L(this.normalStateAction.onPress);
		}

		public void SelectIcon(GUIMonsterIcon icon)
		{
			icon.SetTouchAct_S(this.selectedStateAction.onTouch);
			icon.SetTouchAct_L(this.selectedStateAction.onPress);
		}
	}
}
