using System;
using UnityEngine;

public class BattleStatePvPFadeOut : BattleStateController
{
	public BattleStatePvPFadeOut() : base(null, null)
	{
	}

	protected override void EnabledThisState()
	{
		base.stateManager.uiControl.ApplyFadeOutScreen(base.stateManager.battleScreen);
		base.stateManager.SetBattleScreen(BattleScreen.Fadeout);
		base.stateManager.uiControl.SetTouchEnable(true);
		if (base.onServerConnect)
		{
			base.stateManager.sleep.SetSleepOff(false);
			Input.multiTouchEnabled = true;
			GUIMain.SetBattleCallBack(new Action(this.BattleEndResourcesCleaning));
			GUIMain.FadeBlackReqFromScene("UIPvPResult", 0.5f, 0.5f);
		}
		else
		{
			this.BattleEndResourcesCleaning();
			Application.LoadLevelAsync(BattleStateManager.BattleSceneName);
		}
	}

	private void BattleEndResourcesCleaning()
	{
		base.battleStateData.RemoveAllCachedObjects();
		base.stateManager.cameraControl.DestroyNotContainCameraParams();
		base.hierarchyData.RemoveAllCachedObjects();
		base.stateManager.uiControl.RemoveAllCachedUI();
		base.stateManager.soundManager.ReleaseAudio();
	}

	protected override void DisabledThisState()
	{
		base.DisabledThisState();
		base.stateManager.pvpFunction.FinalizeTCP();
	}
}
