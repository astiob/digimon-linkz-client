using System;
using UnityEngine;

public class BattleStateFadeOut : BattleStateController
{
	public BattleStateFadeOut() : base(null, null)
	{
	}

	protected override void EnabledThisState()
	{
		if (base.isSkipAction)
		{
			if (!base.stateManager.log.battleStopFlag)
			{
				base.Destroy(base.battleStateData.commandSelectTweenTargetCamera.gameObject);
				base.Destroy(base.battleStateData.characterRoot.gameObject);
				base.Destroy(base.battleStateData.skillEffectRoot.gameObject);
				base.Destroy(base.battleStateData.stageRoot.gameObject);
				base.Destroy(base.battleStateData.spawnPointRoot.gameObject);
				base.Destroy(base.battleStateData.cameraMotionRoot.gameObject);
				base.Destroy(base.battleStateData.hitEffectRoot.gameObject);
				base.Destroy(base.battleStateData.alwaysEffectRoot.gameObject);
				base.stateManager.BattleTrigger();
			}
			return;
		}
		base.stateManager.uiControl.ApplyFadeOutScreen(base.stateManager.battleScreen);
		base.stateManager.SetBattleScreen(BattleScreen.Fadeout);
		base.stateManager.uiControl.SetTouchEnable(true);
		if (base.onServerConnect)
		{
			base.stateManager.sleep.SetSleepOff(false);
			Input.multiTouchEnabled = true;
			GUIMain.SetBattleCallBack(new Action(this.BattleEndResourcesCleaning));
			if (DataMng.Instance().WD_ReqDngResult.clear == 1)
			{
				GUIMain.FadeBlackReqFromScene("UIResult", 0.5f, 0.5f);
			}
			else if (DataMng.Instance().WD_ReqDngResult.clear == 2)
			{
				GUIMain.FadeBlackReqFromScene("UIResult", 0.5f, 0.5f);
			}
			else
			{
				GUIMain.FadeBlackReqFromScene("UIHome", 0.5f, 0.5f);
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
}
