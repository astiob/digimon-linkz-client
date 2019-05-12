using System;

public class BattleInputBasic : BattleInputFunctionBase
{
	public void OnClickMonsterButton(int index)
	{
		if (base.battleStateData.isShowRevivalWindow)
		{
			return;
		}
		if (base.battleStateData.isShowMenuWindow)
		{
			return;
		}
		if (base.battleStateData.isShowCharacterDescription)
		{
			return;
		}
		if (base.battleStateData.isShowSpecificTrade)
		{
			return;
		}
		base.callAction.OnCharacterRevival(index);
	}

	public virtual void OnClickSkillButton(int index)
	{
		if (base.battleStateData.isShowMenuWindow)
		{
			return;
		}
		if (base.battleStateData.isShowRevivalWindow)
		{
			return;
		}
		if (base.battleStateData.isShowSpecificTrade)
		{
			return;
		}
		if (index > 0)
		{
			base.callAction.SetSkill(index - 1);
		}
		else
		{
			base.callAction.SetAttack();
		}
	}

	public void OnPressSkillButton(int index)
	{
		if (base.battleStateData.isShowMenuWindow)
		{
			return;
		}
		if (base.battleStateData.isShowRevivalWindow)
		{
			return;
		}
		if (base.battleStateData.isShowSpecificTrade)
		{
			return;
		}
		base.callAction.ShowHideSkillDescription(index);
	}

	public void OffPressSkillButton(int index)
	{
		if (base.battleStateData.isShowMenuWindow)
		{
			return;
		}
		if (base.battleStateData.isShowRevivalWindow)
		{
			return;
		}
		if (base.battleStateData.isShowSpecificTrade)
		{
			return;
		}
		base.callAction.ShowHideSkillDescription(-1);
		base.ui.skillSelectUi.skillButton[index].SetColliderActive(true);
	}

	public void OnPressMonsterButton(int index)
	{
		if (base.battleStateData.isShowMenuWindow)
		{
			return;
		}
		if (base.battleStateData.isShowRevivalWindow)
		{
			return;
		}
		if (base.battleStateData.isShowSpecificTrade)
		{
			return;
		}
		base.callAction.ShowMonsterDescription(index);
	}

	public void OffPressMonsterButton()
	{
		base.callAction.HideMonsterDescription();
	}

	public void OnClickCharacterRevivalButton()
	{
		if (base.battleStateData.isShowMenuWindow)
		{
			return;
		}
		if (!base.battleStateData.isShowRevivalWindow)
		{
			return;
		}
		if (base.battleStateData.isShowSpecificTrade)
		{
			return;
		}
		if (base.battleStateData.isShowShop)
		{
			return;
		}
		base.callAction.OnDecisionCharacterRevival();
	}

	public void OnClickCharacterRevivalCloseButton()
	{
		if (base.battleStateData.isShowMenuWindow)
		{
			return;
		}
		if (!base.battleStateData.isShowRevivalWindow)
		{
			return;
		}
		if (base.battleStateData.isShowSpecificTrade)
		{
			return;
		}
		if (base.battleStateData.isShowShop)
		{
			return;
		}
		base.callAction.OnCancelCharacterRevival();
	}

	public void OnClickAutoPlayButton()
	{
		if (base.battleStateData.isShowMenuWindow)
		{
			return;
		}
		if (base.battleStateData.isShowRevivalWindow)
		{
			return;
		}
		if (base.battleStateData.isShowContinueWindow)
		{
			return;
		}
		if (base.battleStateData.isShowSpecificTrade)
		{
			return;
		}
		base.callAction.OnAutoPlay();
	}

	public void OnClickX2PlayButton()
	{
		if (base.battleStateData.isShowRevivalWindow)
		{
			return;
		}
		if (base.battleStateData.isShowMenuWindow)
		{
			return;
		}
		if (base.battleStateData.isShowContinueWindow)
		{
			return;
		}
		if (base.battleStateData.isShowHelp)
		{
			return;
		}
		if (base.battleStateData.isShowSpecificTrade)
		{
			return;
		}
		base.callAction.On2xSpeedPlay();
	}

	public void OnClickMenuButton()
	{
		if (base.battleStateData.isShowRevivalWindow)
		{
			return;
		}
		if (base.battleStateData.isShowMenuWindow)
		{
			return;
		}
		if (base.battleStateData.isShowContinueWindow)
		{
			return;
		}
		if (base.battleStateData.isShowHelp)
		{
			return;
		}
		if (base.battleStateData.isShowSpecificTrade)
		{
			return;
		}
		base.callAction.OnShowMenu();
	}

	public void OnClickMenuRetireButton()
	{
		if (base.battleStateData.isShowRetireWindow)
		{
			return;
		}
		if (base.battleStateData.isShowHelp)
		{
			return;
		}
		if (!base.hierarchyData.isPossibleRetire)
		{
			return;
		}
		if (base.battleStateData.isShowSpecificTrade)
		{
			return;
		}
		base.callAction.OnRetireCheck();
	}

	public void OnClickMenuCloseButton()
	{
		if (base.battleStateData.isShowRevivalWindow)
		{
			return;
		}
		if (!base.battleStateData.isShowMenuWindow)
		{
			return;
		}
		if (base.battleStateData.isShowHelp)
		{
			return;
		}
		if (base.battleStateData.isShowSpecificTrade)
		{
			return;
		}
		base.callAction.OnHideMenu();
	}

	public void OnClickRetireDialogOkButton()
	{
		base.stateManager.uiControl.ApplySetContinueUIColliders(true);
		base.callAction.OnOkRetire();
	}

	public void OnClickRetireDialogCancelButton()
	{
		base.stateManager.uiControl.ApplySetContinueUIColliders(true);
		base.callAction.OnCancelRetire();
	}

	public void OnClickContinueDialogRetireButton()
	{
		if (base.battleStateData.isShowRetireWindow)
		{
			return;
		}
		if (base.battleStateData.isShowHelp)
		{
			return;
		}
		if (!base.hierarchyData.isPossibleRetire)
		{
			return;
		}
		if (base.battleStateData.isShowSpecificTrade)
		{
			return;
		}
		if (base.battleStateData.isContinueFlag)
		{
			return;
		}
		if (base.battleStateData.isShowShop)
		{
			return;
		}
		base.stateManager.uiControl.ApplySetContinueUIColliders(false);
		base.callAction.OnRetireCheck();
	}

	public void OnClickContinueDialogRevivalButton()
	{
		base.callAction.OnDeathEndRevival();
	}

	public void OnClickHelpButton()
	{
		base.callAction.OnShowHelp();
	}

	public void OnClickHelpCloseButton()
	{
		base.callAction.OnHideHelp();
	}

	public void OnPressHud(int index)
	{
		if (base.battleStateData.isTutorialInduction)
		{
			return;
		}
		if (base.battleStateData.isShowRevivalWindow)
		{
			return;
		}
		if (base.battleStateData.isShowMenuWindow)
		{
			return;
		}
		if (base.battleStateData.isShowContinueWindow)
		{
			return;
		}
		if (base.battleStateData.isShowHelp)
		{
			return;
		}
		if (base.battleStateData.isShowSpecificTrade)
		{
			return;
		}
		base.callAction.OnShowEnemyDescription(index);
	}

	public void OffPressHud()
	{
		base.callAction.OnHideEnemyDescriotion();
	}

	public void OnClickSkipWinnerAction()
	{
		base.callAction.OnSkipWinnerAction();
	}

	public void OnClickSpecificTradeButton()
	{
		base.callAction.OnShowSpecificTrade();
	}

	public void OnClickCloseInitialInductionButton()
	{
		base.callAction.OnCloseInitialInduction();
	}

	public void OnShowEnemyDescription3D(int index)
	{
		if (base.battleStateData.isTutorialInduction)
		{
			return;
		}
		if (base.battleStateData.isShowRevivalWindow)
		{
			return;
		}
		if (base.battleStateData.isShowMenuWindow)
		{
			return;
		}
		if (base.battleStateData.isShowContinueWindow)
		{
			return;
		}
		if (base.battleStateData.isShowHelp)
		{
			return;
		}
		if (base.battleStateData.isShowSpecificTrade)
		{
			return;
		}
		base.callAction.OnShowEnemyDescription(index);
	}

	public void OnHideEnemyDescriotion3D()
	{
		base.callAction.OnHideEnemyDescriotion();
	}

	public void OnSkillButtonRotateAfter(int index)
	{
		base.ui.skillSelectUi.skillButton[index].SetColliderActive(true);
	}
}
