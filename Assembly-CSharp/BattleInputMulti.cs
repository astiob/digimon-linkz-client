using System;
using System.Collections;
using UnityEngine;

public class BattleInputMulti : BattleInputBasic
{
	public override void OnClickSkillButton(int index)
	{
		base.OnClickSkillButton(index);
		base.stateManager.uiControlMulti.RefreshSharedAP(true);
		base.stateManager.uiControlMulti.StartSharedAPAnimation();
	}

	public void OnMultiShowMenu()
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
		base.battleStateData.isShowMenuWindow = true;
		base.stateManager.uiControl.ApplyShowMenuWindow(true, true, null);
		base.stateManager.soundPlayer.SetPauseVolume(true);
		SoundPlayer.PlayMenuOpen();
		base.stateManager.uiControl.SetHudCollider(false);
	}

	public void OnClickMultiX2PlayButton()
	{
		if (!base.stateManager.multiFunction.IsOwner)
		{
			base.stateManager.uiControlMulti.ShowX2PlayErrorDialog();
		}
		else if (!base.battleStateData.isShowRevivalWindow && !base.battleStateData.isShowMenuWindow && !base.battleStateData.isShowContinueWindow && !base.battleStateData.isShowHelp && !base.battleStateData.isShowSpecificTrade)
		{
			base.StartCoroutine(this.X2SpeedPlay());
		}
	}

	private IEnumerator X2SpeedPlay()
	{
		base.stateManager.uiControl.SetTouchEnable(false);
		IEnumerator wait = base.stateManager.multiFunction.Send2xSpeedPlay();
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		base.stateManager.uiControl.SetTouchEnable(true);
		yield break;
	}

	public void OnClickMultiRetireDialogOkButton()
	{
		if (base.stateManager.battleScreen == BattleScreen.RevivalCharacter || base.battleStateData.isRunnedRevivalFunction)
		{
			return;
		}
		if (!base.battleStateData.isShowContinueWindow)
		{
			base.battleStateData.isBattleRetired = true;
		}
		base.battleStateData.isShowRetireWindow = false;
		base.stateManager.time.SetPlaySpeed(false, false);
		base.stateManager.uiControl.ApplyCurrentSelectArrow(false, default(Vector3));
		base.stateManager.soundPlayer.SetPauseVolume(false);
		SoundPlayer.PlayButtonEnter();
		base.StartCoroutine(this.Retire());
	}

	private IEnumerator Retire()
	{
		bool isEnd = false;
		base.stateManager.uiControl.SetTouchEnable(false);
		base.stateManager.multiFunction.SendRetire(delegate
		{
			isEnd = true;
		});
		while (!isEnd)
		{
			yield return null;
		}
		base.stateManager.uiControl.SetTouchEnable(true);
		Action onFinished = delegate()
		{
			base.battleStateData.isShowRetireWindow = false;
		};
		IEnumerator wait = base.stateManager.uiControlMulti.WaitOpenCloseDialog(false, base.ui.dialogRetire.gameObject, base.ui.dialogRetire.openCloseDialog, onFinished);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		yield break;
	}

	public void OnClickMultiMonsterButton(int index)
	{
		if (!base.stateManager.multiFunction.IsMe(index))
		{
			return;
		}
		if (base.battleStateData.isShowRevivalWindow)
		{
			return;
		}
		if (base.battleStateData.isShowMenuWindow)
		{
			return;
		}
		if (base.battleStateData.isShowCharacterDescription)
		{
			return;
		}
		if (base.battleStateData.isShowSpecificTrade)
		{
			return;
		}
		if (!base.battleStateData.playerCharacters[index].isDied)
		{
			return;
		}
		if (base.battleStateData.isRevivalReservedCharacter[index])
		{
			return;
		}
		if (!base.hierarchyData.isPossibleContinue)
		{
			return;
		}
		SoundPlayer.PlayButtonEnter();
		base.battleStateData.isShowRevivalWindow = true;
		base.battleStateData.currentSelectRevivalCharacter = index;
		base.stateManager.uiControl.ApplyDigiStoneNumber(base.battleStateData.currentDigiStoneNumber);
		base.stateManager.uiControl.ApplyEnableCharacterRevivalWindow(true, base.battleStateData.currentDigiStoneNumber >= 1, null);
	}

	public void OnClickMultiCharacterRevivalButton()
	{
		if (base.battleStateData.isShowMenuWindow)
		{
			return;
		}
		if (!base.battleStateData.isShowRevivalWindow)
		{
			return;
		}
		if (base.battleStateData.isShowSpecificTrade)
		{
			return;
		}
		if (base.battleStateData.currentDigiStoneNumber < 1)
		{
			return;
		}
		base.stateManager.uiControl.SetHudCollider(false);
		base.StartCoroutine(this.CharacterRevival());
	}

	private IEnumerator CharacterRevival()
	{
		base.stateManager.uiControl.SetTouchEnable(false);
		IEnumerator wait = base.stateManager.multiFunction.SendCharacterRevival();
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		base.stateManager.uiControl.SetTouchEnable(true);
		yield break;
	}

	public void OnClickMultiContinueDialogRevivalButton()
	{
		base.stateManager.battleUiComponentsMulti.continueTimer.Stop();
		base.callAction.OnDeathEndRevival();
	}
}
