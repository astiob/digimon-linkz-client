using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleStateFadeOut : BattleStateController
{
	public BattleStateFadeOut() : base(null, null)
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
			SceneManager.LoadSceneAsync(BattleStateManager.BattleSceneName);
		}
	}

	private void BattleEndResourcesCleaning()
	{
		base.battleStateData.RemoveAllCachedObjects();
		base.stateManager.cameraControl.DestroyNotContainCameraParams();
		base.hierarchyData.RemoveAllCachedObjects();
		base.stateManager.uiControl.RemoveAllCachedUI();
		base.stateManager.soundManager.ReleaseAudio();
		BattleEffectManager.Instance.Destroy();
	}
}
