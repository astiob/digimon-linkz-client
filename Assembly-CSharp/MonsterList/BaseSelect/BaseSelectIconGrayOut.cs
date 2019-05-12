using Master;
using System;

namespace MonsterList.BaseSelect
{
	public sealed class BaseSelectIconGrayOut : MonsterListIconGrayOut
	{
		public void SetSelect(GUIMonsterIcon icon)
		{
			icon.DimmMess = StringMaster.GetString("SystemSelect");
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.DISABLE);
			icon.SetTouchAct_S(this.selectedStateAction.onTouch);
			icon.SetTouchAct_L(this.selectedStateAction.onPress);
		}

		public void SetSelectText(GUIMonsterIcon icon, string iconText)
		{
			icon.DimmMess = iconText;
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.DISABLE);
			icon.SetTouchAct_S(this.selectedStateAction.onTouch);
			icon.SetTouchAct_L(this.selectedStateAction.onPress);
		}

		public void CancelSelect(GUIMonsterIcon icon)
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

		public void BlockLockMonster(GUIMonsterIcon icon)
		{
			icon.DimmMess = string.Empty;
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.DISABLE);
			icon.SetTouchAct_S(this.blockStateAction.onTouch);
			icon.SetTouchAct_L(this.blockStateAction.onPress);
		}

		public void BlockSpecialTypeMonster(GUIMonsterIcon icon)
		{
			icon.DimmMess = string.Empty;
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.DISABLE);
			icon.SetTouchAct_S(this.blockStateAction.onTouch);
			icon.SetTouchAct_L(this.blockStateAction.onPress);
		}

		public void BlockMedalNoPossibility(GUIMonsterIcon icon)
		{
			icon.DimmMess = StringMaster.GetString("MedalInherit_NoPossibility");
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.DISABLE);
			icon.SetTouchAct_S(this.blockStateAction.onTouch);
			icon.SetTouchAct_L(this.blockStateAction.onPress);
		}

		public void BlockLockMonsterReturnDetailed(GUIMonsterIcon icon)
		{
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.DISABLE);
			icon.SetTouchAct_S(this.blockStateAction.onTouch);
			icon.SetTouchAct_L(this.blockStateAction.onPress);
		}

		public void CancelLockMonsterReturnDetailed(GUIMonsterIcon icon)
		{
			icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.ACTIVE);
			icon.SetTouchAct_S(this.normalStateAction.onTouch);
			icon.SetTouchAct_L(this.normalStateAction.onPress);
		}

		public void BlockLevelMax(GUIMonsterIcon icon)
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

		public void SetEvolutionIcon(GUIMonsterIcon icon, bool canEvolve, bool onlyGrayOut)
		{
			icon.SetMessageLevel();
			icon.SetSortMessageColor(ConstValue.DIGIMON_GREEN);
			if (canEvolve)
			{
				if (!onlyGrayOut)
				{
					icon.SortMess = StringMaster.GetString("CharaIcon-01");
					icon.SetSortMessageColor(ConstValue.DIGIMON_YELLOW);
				}
			}
			else
			{
				icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.NOTACTIVE);
				if (!onlyGrayOut)
				{
					icon.SortMess = StringMaster.GetString("CharaIcon-02");
					icon.SetSortMessageColor(ConstValue.DIGIMON_BLUE);
				}
			}
		}

		public void SetVersionUpIcon(GUIMonsterIcon icon, bool canVersionUp, bool onlyGrayOut)
		{
			icon.SetMessageLevel();
			icon.SetSortMessageColor(ConstValue.DIGIMON_GREEN);
			if (canVersionUp)
			{
				if (!onlyGrayOut)
				{
					icon.SortMess = StringMaster.GetString("CharaIcon-05");
					icon.SetSortMessageColor(ConstValue.DIGIMON_YELLOW);
				}
			}
			else
			{
				icon.SetGrayout(GUIMonsterIcon.DIMM_LEVEL.NOTACTIVE);
				if (!onlyGrayOut)
				{
					icon.SortMess = StringMaster.GetString("CharaIcon-06");
					icon.SetSortMessageColor(ConstValue.DIGIMON_BLUE);
				}
			}
		}
	}
}
