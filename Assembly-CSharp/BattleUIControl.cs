using BattleStateMachineInternal;
using System;
using System.Collections.Generic;

public abstract class BattleUIControl : BattleFunctionBase
{
	private Dictionary<BattleScreen, BattleScreenDetail> _battleScreenDetails = new Dictionary<BattleScreen, BattleScreenDetail>();

	public virtual Dictionary<BattleScreen, BattleScreenDetail> battleScreenDetails
	{
		get
		{
			return this._battleScreenDetails;
		}
	}

	protected BattleUIComponents ui
	{
		get
		{
			return base.stateManager.battleUiComponents;
		}
	}

	protected BattleStateUIProperty uiProperty
	{
		get
		{
			return base.stateManager.uiProperty;
		}
	}

	protected BattleInputBasic input
	{
		get
		{
			return base.stateManager.input;
		}
	}

	public virtual void ApplySetBattleStateRegistration()
	{
	}

	public bool ApplySetBattleState(BattleScreen previousState, BattleScreen nextState)
	{
		if (this.battleScreenDetails.ContainsKey(previousState))
		{
			this.battleScreenDetails[previousState].PreviousState();
		}
		if (this.battleScreenDetails.ContainsKey(nextState))
		{
			this.battleScreenDetails[nextState].NextState();
		}
		bool flag = this.battleScreenDetails.ContainsKey(nextState) && this.battleScreenDetails[nextState].isAlwaysScreen;
		if (this.ui.battleAlwaysUi != null && flag != this.ui.battleAlwaysUi.gameObject.activeInHierarchy)
		{
			NGUITools.SetActiveSelf(this.ui.battleAlwaysUi.gameObject, flag);
		}
		if (this.ui.rootPanel != null)
		{
			this.ui.rootPanel.Refresh();
		}
		return flag;
	}
}
