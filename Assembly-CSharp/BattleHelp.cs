using BattleStateMachineInternal.HelpImage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleHelp : BattleFunctionBase
{
	private const string tutorialInfoDirectory = "Tutorial_info/{0}";

	private TutorialUI tutorialUI;

	private List<string> imageNames = new List<string>();

	private bool helpLoaded;

	[CompilerGenerated]
	private static Action <>f__mg$cache0;

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
			TutorialImageWindow imageWindow = this.tutorialUI.ImageWindow;
			List<string> pageResourcesNames = this.imageNames;
			bool isDisplayThumbnail = false;
			Action closedAction = new Action(this.CloseWindow);
			if (BattleHelp.<>f__mg$cache0 == null)
			{
				BattleHelp.<>f__mg$cache0 = new Action(SoundPlayer.PlayButtonCancel);
			}
			base.StartCoroutine(imageWindow.OpenWindow(pageResourcesNames, isDisplayThumbnail, closedAction, BattleHelp.<>f__mg$cache0));
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
