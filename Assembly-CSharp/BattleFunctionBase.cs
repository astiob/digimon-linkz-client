using BattleStateMachineInternal;
using System;

public abstract class BattleFunctionBase : IMono, IBattleFunctionInput
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
