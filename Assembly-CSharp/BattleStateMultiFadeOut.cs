using System;
using UnityEngine;

public class BattleStateMultiFadeOut : BattleStateController
{
	public BattleStateMultiFadeOut() : base(null, null)
	{
	}

	protected override void EnabledThisState()
	{
		base.stateManager.uiControl.ApplyFadeOutScreen(base.stateManager.battleScreen);
		base.stateManager.SetBattleScreen(BattleScreen.Fadeout);
		base.stateManager.uiControlMulti.HideAllDIalog();
		base.stateManager.uiControl.SetTouchEnable(true);
		global::Debug.Log("BattleStateMultiFadeOut onServerConnect " + base.onServerConnect);
		if (base.onServerConnect)
		{
			base.stateManager.sleep.SetSleepOff(false);
			Input.multiTouchEnabled = true;
			GUIMain.SetBattleCallBack(new Action(this.BattleEndResourcesCleaning));
			if (DataMng.Instance().WD_ReqDngResult.clear == 1)
			{
				GUIMain.FadeBlackReqFromSceneForMulti(base.stateManager.multiFunction.startId.ToInt32(), "UIResult", 0.5f, 0.5f);
			}
			else
			{
				GUIMain.FadeBlackReqFromSceneForMulti(base.stateManager.multiFunction.startId.ToInt32(), "UIHome", 0.5f, 0.5f);
			}
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
		base.stateManager.multiFunction.FinalizeTCP();
	}
}
