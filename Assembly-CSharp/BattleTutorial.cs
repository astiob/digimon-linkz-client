using System;
using UnityEngine;

public class BattleTutorial : BattleFunctionBase
{
	private BattleUIComponents ui
	{
		get
		{
			return base.stateManager.battleUiComponents;
		}
	}

	public override void BattleAwakeInitialize()
	{
		if (base.stateManager.onEnableTutorial)
		{
			base.hierarchyData.onEnableBackKey = false;
			base.hierarchyData.onApRevivalMax = true;
			base.hierarchyData.onEnableRandomValue = false;
			base.hierarchyData.onHitRate100Percent = true;
			base.hierarchyData.isPossibleRetire = false;
			base.battleStateData.isPossibleShowDescription = false;
			base.battleStateData.isTutorialInduction = true;
		}
	}

	public void SetPose(bool onPose)
	{
		base.stateManager.time.SetPlaySpeed(base.hierarchyData.on2xSpeedPlay, onPose);
	}

	public void SetAutoPlay(bool onAutoPlay)
	{
		base.hierarchyData.onAutoPlay = onAutoPlay;
		base.stateManager.uiControl.ApplyAutoPlay(base.hierarchyData.onAutoPlay);
	}

	public bool isBattleStarted
	{
		get
		{
			return base.stateManager.battleScreen == BattleScreen.SkillSelects;
		}
	}

	public int currentSelectCharacter
	{
		get
		{
			return base.battleStateData.currentSelectCharacterIndex;
		}
	}

	public GameObject GetAttackUIButton
	{
		get
		{
			return this.ui.skillButton[0].gameObject;
		}
	}

	public GameObject GetDeathblowUIButton
	{
		get
		{
			return this.ui.skillButton[1].gameObject;
		}
	}

	public GameObject GetInheritanceTechniqueUIButton
	{
		get
		{
			return this.ui.skillButton[2].gameObject;
		}
	}

	public GameObject[] GetCharactersUIButtons()
	{
		return new GameObject[]
		{
			this.ui.monsterButton[0].gameObject,
			this.ui.monsterButton[1].gameObject,
			this.ui.monsterButton[2].gameObject
		};
	}

	public GameObject GetCharacterButtonsCenterUIButton
	{
		get
		{
			return this.ui.monsterButton[1].gameObject;
		}
	}

	public GameObject GetCharacterUIButtonRoot
	{
		get
		{
			return this.ui.skillSelectUi.monsterButtonRoot;
		}
	}

	public GameObject GetRevivalDialogEnterUIButton
	{
		get
		{
			return this.ui.characterRevivalDialog.GetRevivalDialogEnterUIButton;
		}
	}

	public GameObject GetRevivalDialogCancelUIButton
	{
		get
		{
			return this.ui.characterRevivalDialog.GetRevivalDialogCancelUIButton;
		}
	}

	public bool isPossibleTargetSelect
	{
		get
		{
			return base.battleStateData.isPossibleTargetSelect;
		}
		set
		{
			base.battleStateData.isPossibleTargetSelect = value;
		}
	}

	public bool isPossibleMenuSelect
	{
		get
		{
			return base.battleStateData.isPossibleMenuSelect;
		}
		set
		{
			base.battleStateData.isPossibleMenuSelect = value;
		}
	}

	public void SetEnableBackKey()
	{
		base.hierarchyData.onEnableBackKey = true;
	}

	public void SetActiveShowSkillDescription()
	{
		base.battleStateData.isPossibleShowDescription = true;
	}

	public void SetDeactiveTutorialInduction()
	{
		base.battleStateData.isTutorialInduction = false;
	}
}
