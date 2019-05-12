using System;
using System.Collections;
using UnityEngine;

public class BattleInputPvP : BattleInputBasic
{
	public void OnPvPShowMenu()
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
		if (base.battleStateData.isShowRetireWindow)
		{
			return;
		}
		if (!base.hierarchyData.isPossibleRetire)
		{
			return;
		}
		base.battleStateData.isShowRetireWindow = true;
		base.stateManager.uiControl.ApplyShowRetireWindow(true, null);
		base.stateManager.soundPlayer.SetPauseVolume(true);
		SoundPlayer.PlayMenuOpen();
		base.stateManager.uiControl.SetHudCollider(false);
	}

	public void OnClickPvPRetireDialogOkButton()
	{
		if (!base.battleStateData.isShowContinueWindow)
		{
			base.battleStateData.isBattleRetired = true;
		}
		base.battleStateData.isShowRetireWindow = false;
		base.stateManager.time.SetPlaySpeed(false, false);
		base.stateManager.uiControl.ApplyCurrentSelectArrow(false, default(Vector3));
		base.stateManager.soundPlayer.SetPauseVolume(false);
		SoundPlayer.PlayButtonEnter();
		base.stateManager.uiControl.SetTouchEnable(false);
		Action onFinishedClose = delegate()
		{
			base.battleStateData.isShowRetireWindow = false;
			base.stateManager.events.CallRetireEvent();
			base.stateManager.uiControlPvP.HideSkillSelectUI();
		};
		base.StartCoroutine(this.WaitOpenPvPCloseDialog(false, base.ui.dialogRetire.gameObject, base.ui.dialogRetire.openCloseDialog, onFinishedClose));
	}

	private IEnumerator WaitOpenPvPCloseDialog(bool isOpen, GameObject dialogObject, UIOpenCloseDialog openCloseDialog, Action onFinishedClose = null)
	{
		base.stateManager.dialogOpenCloseWaitFlag = true;
		base.stateManager.uiControl.SetTouchEnable(false);
		if (isOpen)
		{
			NGUITools.SetActiveSelf(dialogObject, true);
			yield return base.StartCoroutine(openCloseDialog.InitializeRoutine());
			openCloseDialog.PlayOpenAnimation(null);
		}
		else
		{
			openCloseDialog.PlayCloseAnimation(onFinishedClose);
		}
		while (openCloseDialog.isPlaying)
		{
			yield return null;
		}
		if (!isOpen)
		{
			NGUITools.SetActiveSelf(dialogObject, false);
		}
		base.stateManager.dialogOpenCloseWaitFlag = false;
		yield break;
	}
}
