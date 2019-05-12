using Master;
using System;
using System.Collections;
using UnityEngine;

public class BattleUIControlPvP : BattleUIControlMultiBasic
{
	private BattleRoundEndPvP battleRoundEnd;

	private MultiBattleDialog _battleDialog;

	private BattleUIControlPvP.DialogType myType;

	private bool isReOpenCountDown;

	private Action reopenAction;

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
			return this.ui.attackTime;
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

	public override void ApplySetBattleStateRegistration()
	{
		base.ApplySetBattleStateRegistration();
	}

	protected override void SetInitActive()
	{
		base.SetInitActive();
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
		base.stateManager.uiControl.AfterInitializeUIBefore();
		this.HideAlertDialog();
		for (int i = 0; i < this.ui.monsterButton.Length; i++)
		{
			BattleInputUtility.AddEvent(this.ui.monsterButton[i].playerMonsterDescriptionSwitch.onHoldWaitPress, new Action<int>(base.input.OnPressMonsterButton), i);
			BattleInputUtility.AddEvent(this.ui.monsterButton[i].playerMonsterDescriptionSwitch.onDisengagePress, new Action(base.input.OffPressMonsterButton));
		}
		for (int j = 0; j < this.ui.skillButton.Length; j++)
		{
			if (j > 0)
			{
				BattleInputUtility.AddEvent(this.ui.skillButton[j].skillDescriptionSwitch.onClick, new Action<int>(base.input.OnClickSkillButton), j);
			}
			else
			{
				BattleInputUtility.AddEvent(this.ui.skillButton[j].button.onClick, new Action<int>(base.input.OnClickSkillButton), j);
			}
			UITweenEventSystem tevsystem = this.ui.skillButton[j].rotationEffect1.gameObject.GetComponent<UITweenEventSystem>();
			BattleInputUtility.AddEvent(tevsystem.onFinished, new Action<int>(base.input.OnSkillButtonRotateAfter), j);
			tevsystem = this.ui.skillButton[j].rotationEffect2.gameObject.GetComponent<UITweenEventSystem>();
			BattleInputUtility.AddEvent(tevsystem.onFinished, new Action<int>(base.input.OnSkillButtonRotateAfter), j);
			if (j != 0)
			{
				BattleInputUtility.AddEvent(this.ui.skillButton[j].skillDescriptionSwitch.onHoldWaitPress, new Action<int>(base.input.OnPressSkillButton), j);
				BattleInputUtility.AddEvent(this.ui.skillButton[j].skillDescriptionSwitch.onDisengagePress, new Action<int>(base.input.OffPressSkillButton), j);
			}
		}
		BattleInputUtility.AddEvent(this.ui.menuButton.onClick, new Action(this.inputPvP.OnPvPShowMenu));
		this.ui.dialogRetire.AddEvent(new Action(this.inputPvP.OnClickPvPRetireDialogOkButton), new Action(base.input.OnClickRetireDialogCancelButton));
		this.ui.initialInduction.AddEvent(new Action(base.input.OnClickCloseInitialInductionButton));
		BattleInputUtility.AddEvent(this.ui.winNextButton.onClick, new Action(base.input.OnClickSkipWinnerAction));
		BattleInputUtility.AddEvent(this.ui.loseNextButton.onClick, new Action(base.input.OnClickSkipWinnerAction));
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
		this.SetupEmotion();
		this.SetupTimer();
		yield break;
	}

	private void SetupEmotion()
	{
		this.ui.emotionSenderMulti = this.ui.skillSelectUi.emotionSenderMulti;
		this.alwaysUi.Initialize(this.ui, new Action<UIButton>(base.stateManager.pvpFunction.SendEmotion));
		this.HideEmotionButton();
		this.HideEmotion();
	}

	private void SetupTimer()
	{
		this.attackTime.checkEnemyRecoverDialog = new Func<bool>(this.CheckEnemyRecoverDialog);
	}

	private bool CheckEnemyRecoverDialog()
	{
		return this.myType == BattleUIControlPvP.DialogType.EnemyCount;
	}

	public void HideEmotion()
	{
		this.ui.emotionSenderMulti.HideAll();
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

	public void StopAttackTimer()
	{
		this.attackTime.StopTimer();
	}

	public void ApplySetAlwaysUIObject(bool isEnable)
	{
		this.alwaysUi.ApplySetAlwaysUIObject(isEnable);
	}

	public void SetPlayerName(string playerName)
	{
		this.ui.playerNameLabel.text = playerName;
	}

	public void SetPlayerTitle(string playerTitleId)
	{
		TitleDataMng.SetTitleIcon(playerTitleId, this.ui.playerTitleIcon);
	}

	public void SetEnemyName(string enemyName)
	{
		this.ui.enemyNameLabel.text = enemyName;
	}

	public void SetEnemyTitle(string enemyTitleId)
	{
		TitleDataMng.SetTitleIcon(enemyTitleId, this.ui.enemyTitleIcon);
	}

	public void ShowAlwaysUI()
	{
		NGUITools.SetActiveSelf(this.alwaysUi.gameObject, true);
	}

	public void HideAlwaysUI()
	{
		NGUITools.SetActiveSelf(this.alwaysUi.gameObject, false);
	}

	public void RegisterAttackRed()
	{
		this.attackTime.callBackAction = new Action(base.stateManager.pvpFunction.RunAttackAutomatically);
	}

	public void ShowAlertDialog(BattleUIControlPvP.DialogType type, string message, string buttonText, Action action = null, bool isWithButton = false, int maxTime = -1)
	{
		if (this.battleDialog.IsBlockNewDialog)
		{
			return;
		}
		if (this.myType == BattleUIControlPvP.DialogType.EnemyCount && type == BattleUIControlPvP.DialogType.MustRecover)
		{
			this.isReOpenCountDown = true;
		}
		this.myType = type;
		this.battleDialog.SetMessage(message, buttonText);
		this.battleDialog.callBackAction = action;
		this.battleDialog.maxTime = maxTime;
		int skin = (!isWithButton) ? 2 : 1;
		this.battleDialog.SetSkin(skin);
	}

	public void BlockNewDialog()
	{
		this.battleDialog.IsBlockNewDialog = true;
	}

	public bool IsFailed()
	{
		return this.battleDialog.isFailed;
	}

	public void StartEnemyFailedTimer(Action action, BattleUIControlPvP.DialogType dialogType)
	{
		if (this.battleDialog.IsAlreadyOpen())
		{
			return;
		}
		this.reopenAction = action;
		if (this.myType == BattleUIControlPvP.DialogType.MyCount && dialogType == BattleUIControlPvP.DialogType.EnemyCount)
		{
			return;
		}
		this.myType = dialogType;
		if (this.battleDialog.gameObject.activeSelf)
		{
			base.stateManager.uiControlPvP.StopAttackTimer();
			string waitingConnectionFormat = string.Empty;
			if (this.myType == BattleUIControlPvP.DialogType.EnemyCount)
			{
				waitingConnectionFormat = StringMaster.GetString("BattleUI-22");
				this.battleDialog.StartFailedTimer(waitingConnectionFormat, action, true);
			}
			else if (this.myType == BattleUIControlPvP.DialogType.MyCount)
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
		if (this.isReOpenCountDown)
		{
			this.StartEnemyFailedTimer(this.reopenAction, BattleUIControlPvP.DialogType.EnemyCount);
		}
		else
		{
			this.myType = BattleUIControlPvP.DialogType.None;
		}
		this.isReOpenCountDown = false;
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

	public void HideLeftRoundUI()
	{
		this.battleRoundEnd.HideLeftRoundUI();
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

	public void ApplyDroppedItemIconHide()
	{
		this.ui.itemInfoField.ApplyDroppedItemIconHide();
	}

	public enum DialogType
	{
		None,
		Lose,
		MustRecover,
		EnemyCount,
		EnemyRetire,
		MyCount
	}
}
