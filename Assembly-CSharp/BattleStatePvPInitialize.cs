using System;
using System.Collections;

public class BattleStatePvPInitialize : BattleStateInitialize
{
	public BattleStatePvPInitialize(Action OnExit, Action<EventState> OnExitGotEvent) : base(OnExit, OnExitGotEvent)
	{
	}

	protected override string enemySpawnPoint1
	{
		get
		{
			return "0001_enemiesOne_small";
		}
	}

	protected override string enemySpawnPoint2
	{
		get
		{
			return "0002_enemiesTwo_small";
		}
	}

	protected override string enemySpawnPoint3
	{
		get
		{
			return "0003_enemiesThree_small_pvp";
		}
	}

	protected override string insertPlayerPath
	{
		get
		{
			return "insertPVPCharacterEffect";
		}
	}

	protected override string insertEnemyPath
	{
		get
		{
			return "insertPVPEnemyEffect";
		}
	}

	protected override void AwakeThisState()
	{
		base.EnabledThisState();
		base.stateManager.pvpFunction.Initialize();
	}

	protected override IEnumerator CheckRecover()
	{
		yield break;
	}

	protected override IEnumerator LoadAfterInitializeUI()
	{
		BattleDebug.Log("ロード完了後UI初期化: 開始");
		IEnumerator afterInitializeUI = base.stateManager.uiControlPvP.AfterInitializeUI();
		while (afterInitializeUI.MoveNext())
		{
			yield return null;
		}
		BattleDebug.Log("ロード完了後UI初期化: 完了");
		base.hierarchyData.on2xSpeedPlay = true;
		base.stateManager.uiControl.ApplyHideHitIcon();
		base.stateManager.uiControl.ApplyDroppedItemHide();
		base.stateManager.uiControlPvP.ApplyDroppedItemIconHide();
		base.stateManager.uiControlPvP.ApplySetAlwaysUIObject(false);
		base.stateManager.uiControlPvP.ApplyAttackTimer(true);
		base.stateManager.uiControlPvP.RegisterAttackRed();
		base.stateManager.pvpFunction.InitializeTCPClient(false);
		string playerName = base.stateManager.pvpFunction.GetPlayerName();
		string enemyName = base.stateManager.pvpFunction.GetEnemyName();
		base.stateManager.uiControlPvP.SetPlayerName(playerName);
		base.stateManager.uiControlPvP.SetEnemyName(enemyName);
		string playerTitleId = base.stateManager.pvpFunction.GetPlayerTitleId();
		string enemyTitleId = base.stateManager.pvpFunction.GetEnemyTitleId();
		base.stateManager.uiControlPvP.SetPlayerTitle(playerTitleId);
		base.stateManager.uiControlPvP.SetEnemyTitle(enemyTitleId);
		yield break;
	}

	protected override void DisabledThisState()
	{
		base.battleStateData.isEnableBackKeySystem = false;
	}
}
