using BattleStateMachineInternal;
using BattleStateMachineInternal.HelpImage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHelp : BattleFunctionBase
{
	private const string tutorialInfoDirectory = "Tutorial_info/{0}";

	private int currentHelpImage;

	private TutorialUI tutorialUI;

	private List<string> imageNames = new List<string>();

	private bool helpLoaded;

	private BattleUIComponents ui
	{
		get
		{
			return base.stateManager.battleUiComponents;
		}
	}

	private BattleInputBasic input
	{
		get
		{
			return base.stateManager.input;
		}
	}

	private BattleStateUIProperty uiProperty
	{
		get
		{
			return base.stateManager.uiProperty;
		}
	}

	public override void BattleTriggerInitialize()
	{
		BattleHelpImageType type = BattleHelpImageType.Normal;
		BattleMode battleMode = base.battleMode;
		if (battleMode != BattleMode.Multi)
		{
			if (battleMode == BattleMode.PvP)
			{
				type = BattleHelpImageType.PvP;
			}
		}
		else
		{
			type = BattleHelpImageType.Multi;
		}
		BattleHelpImages.Initialize(type);
		foreach (string arg in BattleHelpImages.imageNames)
		{
			this.imageNames.Add(string.Format("Tutorial_info/{0}", arg));
		}
	}

	public IEnumerator HelpInitialize()
	{
		if (!base.stateManager.onServerConnect)
		{
			yield break;
		}
		this.ui.menuDialog.AddHelpButtonEvent(new Action(this.input.OnClickHelpButton));
		this.tutorialUI = this.ui.uiCamera.gameObject.AddComponent<TutorialUI>();
		IEnumerator imageWindow = this.tutorialUI.LoadImageWindow();
		while (imageWindow.MoveNext())
		{
			yield return null;
		}
		this.helpLoaded = true;
		yield break;
	}

	public override void BattleEndBefore()
	{
		if (this.tutorialUI != null && this.tutorialUI.ImageWindow != null)
		{
			base.Destroy(this.tutorialUI.ImageWindow.gameObject);
		}
		if (this.tutorialUI != null)
		{
			base.Destroy(this.tutorialUI);
		}
		BattleHelpImages.UnloadInstance();
	}

	public Texture GetHelpImage(int index)
	{
		return BattleHelpImages.images[index];
	}

	public void ApplyShowHideHelpWindow(bool isShow)
	{
		if (!this.helpLoaded)
		{
			return;
		}
		if (isShow)
		{
			SoundPlayer.PlayButtonEnter();
			base.battleStateData.isShowHelp = true;
			NGUITools.SetActiveSelf(this.ui.helpDialog.gameObject, true);
			base.StartCoroutine(this.tutorialUI.ImageWindow.OpenWindow(this.imageNames, false, new Action(this.CloseWindow), new Action(SoundPlayer.PlayButtonCancel)));
		}
		else
		{
			this.tutorialUI.ImageWindow.CloseWindow();
		}
	}

	private void CloseWindow()
	{
		if (!this.helpLoaded)
		{
			return;
		}
		NGUITools.SetActiveSelf(this.ui.helpDialog.gameObject, false);
		base.battleStateData.isShowHelp = false;
	}
}
