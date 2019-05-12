using Master;
using System;
using System.Collections;
using UnityEngine;

public class BattleUIControlPvP : BattleUIControlMultiBasic
{
	private BattleRoundEndPvP battleRoundEnd;

	private MultiBattleDialog _battleDialog;

	protected BattleInputPvP inputPvP
	{
		get
		{
			return base.stateManager.inputPvP;
		}
	}

	private new BattleUIComponentsPvP ui
	{
		get
		{
			return base.stateManager.battleUiComponents as BattleUIComponentsPvP;
		}
	}

	private AttackTime attackTime
	{
		get
		{
			return this.ui.skillSelectUi.attackTime;
		}
	}

	private MultiBattleDialog battleDialog
	{
		get
		{
			this._battleDialog = (this._battleDialog ?? (this.ui.battleAlwaysUi as AlwaysPvP).dialog);
			return this._battleDialog;
		}
	}

	private AlwaysPvP alwaysUi
	{
		get
		{
			return this.ui.battleAlwaysUi as AlwaysPvP;
		}
	}

	public override MultiConnetionMessage connetionMessage
	{
		get
		{
			return this.alwaysUi.connetionMessage;
		}
	}

	private void CreateBattleRoundEnd()
	{
		GameObject pvPPrefab = ResourcesPath.GetPvPPrefab("PvPBattle_RoundEnd");
		GameObject gameObject = base.Instantiate<GameObject>(pvPPrefab);
		Transform transform = gameObject.transform;
		transform.SetParent(this.ui.skillSelectUi.transform);
		transform.localPosition = new Vector3(0f, 310f, 0f);
		transform.localScale = Vector3.one;
		this.battleRoundEnd = gameObject.GetComponent<BattleRoundEndPvP>();
	}

	public override IEnumerator AfterInitializeUI()
	{
		this.CreateBattleRoundEnd();
		NGUITools.SetActiveSelf(this.battleDialog.gameObject, true);
		base.AfterInitializeUIBefore();
		this.HideAlertDialog();
		for (int i = 0; i < this.ui.skillSelectUi.monsterButton.Length; i++)
		{
			BattleInputUtility.AddEvent(this.ui.skillSelectUi.monsterButton[i].playerMonsterDescriptionSwitch.onHoldWaitPress, new Action<int>(base.input.OnPressMonsterButton), i);
			BattleInputUtility.AddEvent(this.ui.skillSelectUi.monsterButton[i].playerMonsterDescriptionSwitch.onDisengagePress, new Action(base.input.OffPressMonsterButton));
		}
		for (int j = 0; j < this.ui.skillSelectUi.skillButton.Length; j++)
		{
			if (j > 0)
			{
				this.ui.skillSelectUi.skillButton[j].SetClickCallback(new Action<int>(base.input.OnClickSkillButton), j);
				this.ui.skillSelectUi.skillButton[j].SetHoldWaitPressCallback(new Action<int>(base.input.OnPressSkillButton), j);
				this.ui.skillSelectUi.skillButton[j].SetDisengagePressCallback(new Action<int>(base.input.OffPressSkillButton), j);
			}
			else
			{
				this.ui.skillSelectUi.skillButton[j].SetButtonCallback(new Action<int>(base.input.OnClickSkillButton), j);
			}
			this.ui.skillSelectUi.skillButton[j].SetRotationEffectCallback(new Action<int>(base.input.OnSkillButtonRotateAfter), j);
		}
		BattleInputUtility.AddEvent(this.ui.menuButton.onClick, new Action(this.inputPvP.OnPvPShowMenu));
		this.ui.dialogRetire.AddEvent(new Action(this.inputPvP.OnClickPvPRetireDialogOkButton), new Action(base.input.OnClickRetireDialogCancelButton));
		this.ui.initialInduction.AddEvent(new Action(base.input.OnClickCloseInitialInductionButton));
		BattleInputUtility.AddEvent(this.ui.winNextButton.onClick, new Action(base.input.OnClickSkipWinnerAction));
		IEnumerator hitIconInitialize = base.stateManager.uiControl.HitIconInitialize();
		while (hitIconInitialize.MoveNext())
		{
			yield return null;
		}
		IEnumerator hudInitialize = base.stateManager.uiControl.HUDInitialize();
		while (hudInitialize.MoveNext())
		{
			yield return null;
		}
		IEnumerator manualSelectTargetInitialize = base.stateManager.uiControl.ManualSelectTargetInitialize();
		while (manualSelectTargetInitialize.MoveNext())
		{
			yield return null;
		}
		this.ui.rootPanel.RebuildAllDrawCalls();
		base.InstantiateGimmickStatusEffect();
		base.stateManager.uiControl.ApplyHideHitIcon();
		base.stateManager.uiControl.ApplyDroppedItemHide();
		base.stateManager.uiControlPvP.ApplySetAlwaysUIObject(false);
		this.ui.itemInfoField.ApplyDroppedItemIconHide();
		this.SetupEmotion();
		this.SetupTimer();
		string playerName = base.stateManager.pvpFunction.GetPlayerName();
		string enemyName = base.stateManager.pvpFunction.GetEnemyName();
		this.ui.playerNameLabel.text = playerName;
		this.ui.enemyNameLabel.text = enemyName;
		string playerTitleId = base.stateManager.pvpFunction.GetPlayerTitleId();
		string enemyTitleId = base.stateManager.pvpFunction.GetEnemyTitleId();
		TitleDataMng.SetTitleIcon(playerTitleId, this.ui.playerTitleIcon);
		TitleDataMng.SetTitleIcon(enemyTitleId, this.ui.enemyTitleIcon);
		yield break;
	}

	private void SetupEmotion()
	{
		this.ui.emotionSenderMulti = this.ui.skillSelectUi.emotionSenderPvP;
		this.alwaysUi.Initialize(this.ui, new Action<UIButton>(base.stateManager.pvpFunction.SendEmotion));
		this.HideEmotionButton();
		this.ui.emotionSenderMulti.HideAll();
	}

	private void SetupTimer()
	{
		this.attackTime.gameObject.SetActive(true);
		this.attackTime.callBackAction = new Action(base.stateManager.pvpFunction.RunAttackAutomatically);
		this.attackTime.checkEnemyRecoverDialog = (() => this.battleDialog.IsAlreadyOpen());
	}

	public void StopAttackTimer()
	{
		this.attackTime.StopTimer();
	}

	public void ShowEmotionButton()
	{
		this.alwaysUi.ShowEmotionButton();
	}

	public void HideEmotionButton()
	{
		this.alwaysUi.HideEmotionButton();
	}

	public void ShowEmotion(int index, string spriteName, bool isOther = false)
	{
		this.ui.emotionSenderMulti.SetEmotion(index, spriteName, isOther);
	}

	public void ApplySetAlwaysUIObject(bool isEnable)
	{
		this.alwaysUi.ApplySetAlwaysUIObject(isEnable);
	}

	public void ShowAlertDialog(BattleUIControlPvP.DialogType type, string message, string buttonText, Action action = null, bool isWithButton = false, int maxTime = -1)
	{
		if (this.battleDialog.IsBlockNewDialog)
		{
			return;
		}
		this.battleDialog.SetMessage(message, buttonText);
		this.battleDialog.callBackAction = action;
		this.battleDialog.maxTime = maxTime;
		int skin = (!isWithButton) ? 2 : 1;
		this.battleDialog.SetSkin(skin);
	}

	public void StartEnemyFailedTimer(Action action, BattleUIControlPvP.DialogType dialogType)
	{
		if (this.battleDialog.IsAlreadyOpen())
		{
			return;
		}
		if (this.battleDialog.gameObject.activeSelf)
		{
			base.stateManager.uiControlPvP.StopAttackTimer();
			string waitingConnectionFormat = string.Empty;
			if (dialogType == BattleUIControlPvP.DialogType.EnemyCount)
			{
				waitingConnectionFormat = StringMaster.GetString("BattleUI-22");
				this.battleDialog.StartFailedTimer(waitingConnectionFormat, action, true);
			}
			else if (dialogType == BattleUIControlPvP.DialogType.MyCount)
			{
				waitingConnectionFormat = StringMaster.GetString("BattleUI-44");
				this.battleDialog.StartFailedTimer(waitingConnectionFormat, action, false);
			}
		}
	}

	public void HideAlertDialog()
	{
		this.battleDialog.SetSkin(0);
		this.attackTime.RestartTimer();
	}

	public void HideRetireWindow()
	{
		if (!base.battleStateData.isShowRetireWindow)
		{
			return;
		}
		base.stateManager.uiControl.ApplyShowRetireWindow(false, null);
	}

	public void ShowSkillSelectUI()
	{
		NGUITools.SetActiveSelf(this.ui.skillSelectUi.gameObject, true);
	}

	public void HideSkillSelectUI()
	{
		NGUITools.SetActiveSelf(this.ui.skillSelectUi.gameObject, false);
	}

	public void ShowLeftRoundUI(int leftRound)
	{
		this.battleRoundEnd.ShowLeftRoundUI(leftRound);
	}

	public void HideSyncWait()
	{
		GameObject gameObject = (base.stateManager.battleUiComponents as BattleUIComponentsPvP).pvpBattleSyncWaitUi.gameObject;
		NGUITools.SetActiveSelf(gameObject, false);
	}

	public override void ApplySetAlwaysUIColliders(bool isEnable)
	{
		this.alwaysUi.ApplySetAlwaysUIColliders(isEnable);
	}

	public enum DialogType
	{
		None,
		Lose,
		EnemyCount,
		EnemyRetire,
		MyCount
	}
}
