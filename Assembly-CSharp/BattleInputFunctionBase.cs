using BattleStateMachineInternal;
using System;
using UnityEngine;

public abstract class BattleInputFunctionBase : MonoBehaviour, IBattleFunctionInput
{
	protected BattleStateManager stateManager
	{
		get
		{
			return BattleStateManager.current;
		}
	}

	protected BattleMode battleMode
	{
		get
		{
			return this.stateManager.battleMode;
		}
	}

	protected BattleScreen battleScreen
	{
		get
		{
			return this.stateManager.battleScreen;
		}
	}

	protected bool isSkipAction
	{
		get
		{
			return this.stateManager.battleMode == BattleMode.SkipAction;
		}
	}

	protected bool onServerConnect
	{
		get
		{
			return this.stateManager.onServerConnect;
		}
	}

	protected BattleStateData battleStateData
	{
		get
		{
			return this.stateManager.battleStateData;
		}
	}

	protected BattleStateHierarchyData hierarchyData
	{
		get
		{
			return this.stateManager.hierarchyData;
		}
	}

	protected BattleStateProperty stateProperty
	{
		get
		{
			return this.stateManager.stateProperty;
		}
	}

	protected BattleUIComponents ui
	{
		get
		{
			return this.stateManager.battleUiComponents;
		}
	}

	protected BattleStateUIProperty uiProperty
	{
		get
		{
			return this.stateManager.uiProperty;
		}
	}

	protected BattleCallAction callAction
	{
		get
		{
			return BattleStateManager.current.callAction;
		}
	}

	public virtual void BattleAwakeInitialize()
	{
	}

	public virtual void BattleTriggerInitialize()
	{
	}

	public virtual void BattleEndBefore()
	{
	}
}
