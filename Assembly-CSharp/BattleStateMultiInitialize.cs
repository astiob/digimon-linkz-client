using System;
using System.Collections;

public class BattleStateMultiInitialize : BattleStateInitialize
{
	public BattleStateMultiInitialize(Action OnExit) : base(OnExit, null)
	{
	}

	protected override void AwakeThisState()
	{
		base.EnabledThisState();
		base.stateManager.multiFunction.Initialize();
	}

	protected override IEnumerator LoadAfterInitializeUI()
	{
		BattleDebug.Log("ロード完了後UI初期化: 開始");
		base.stateManager.uiControl.ApplyChipNumber(base.battleStateData.currentGettedChip);
		base.stateManager.uiControl.ApplyExpNumber(base.battleStateData.currentGettedExp);
		IEnumerator afterInitializeUI = base.stateManager.uiControlMulti.AfterInitializeUI();
		while (afterInitializeUI.MoveNext())
		{
			yield return null;
		}
		BattleDebug.Log("ロード完了後UI初期化: 完了");
		base.hierarchyData.on2xSpeedPlay = true;
		base.stateManager.uiControl.ApplyHideHitIcon();
		base.stateManager.uiControl.ApplyDroppedItemHide();
		base.stateManager.uiControl.ApplyAutoPlay(base.hierarchyData.onAutoPlay);
		base.stateManager.uiControl.Apply2xPlay(base.hierarchyData.on2xSpeedPlay);
		base.stateManager.uiControl.ApplyAreaName(base.hierarchyData.areaName);
		base.stateManager.multiFunction.InitializeTCPClient(false);
		yield break;
	}

	protected override void DisabledThisState()
	{
		base.battleStateData.isEnableBackKeySystem = base.stateManager.multiFunction.IsOwner;
	}
}
