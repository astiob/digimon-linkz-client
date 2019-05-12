using BattleStateMachineInternal;
using MultiBattle.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleUIControlBasic : BattleUIControl
{
	private List<GameWebAPI.RespDataMA_GetWorldDungeonOptionRewardM.WorldDungeonOptionReward> speedRewardList = new List<GameWebAPI.RespDataMA_GetWorldDungeonOptionRewardM.WorldDungeonOptionReward>();

	private UIPanel parentPanel;

	private List<BattleUIControlBasic.SortDepthData> oldSortDepthDataList = new List<BattleUIControlBasic.SortDepthData>();

	private List<BattleUIControlBasic.SortDepthData> newSortDepthDataList = new List<BattleUIControlBasic.SortDepthData>();

	private GameObject currentSelectMonster { get; set; }

	public IEnumerator BeforeInitializeUI()
	{
		base.ui.rootPanel = base.ui.uiRoot.GetComponent<UIPanel>();
		base.ui.rootCamera = base.ui.uiRoot.GetComponentInChildren<UICamera>();
		UIAnchorOverride.SetUIRoot(base.ui.uiRoot);
		base.ui.rootPanel.alpha = 0f;
		this.CreateSelectMonster();
		yield return new WaitForEndOfFrame();
		UIAnchorRemover remover = base.ui.rootPanel.GetComponentInChildren<UIAnchorRemover>();
		remover.RemoveAnchors(true, false, true);
		NGUITools.SetActiveSelf(base.ui.initializeUi.loadingGaugeRootObject, false);
		NGUITools.SetActiveSelf(base.ui.initializeUi.gameObject, true);
		this.SetInitActive();
		base.ui.rootPanel.alpha = 1f;
		NGUITools.SetActiveSelf(base.ui.initializeUi.loadingGaugeRootObject, true);
		this.ApplyCurrentSelectArrow(false, default(Vector3), 0);
		yield break;
	}

	protected void AfterInitializeUIBefore()
	{
		base.ui.GetUIRoot();
		this.parentPanel = base.ui.initializeUi.panel;
		foreach (UITouchChecker uitouchChecker in base.ui.skillSelectUi.touchChecker)
		{
			uitouchChecker.uiCamera = base.ui.rootCamera;
		}
	}

	private void SetPanelSort(UIPanel p, int n)
	{
		p.depth = this.parentPanel.depth - 10 * n;
		p.sortingOrder = this.parentPanel.sortingOrder - n;
	}

	public IEnumerator HitIconInitialize()
	{
		Transform hitParent = new GameObject("HitIcon").transform;
		hitParent.gameObject.layer = LayerMask.NameToLayer("UI");
		base.ui.otherCeratedUITransforms.Add(hitParent);
		hitParent.SetParent(base.ui.uiRoot.transform);
		hitParent.position = Vector3.zero;
		hitParent.localScale = Vector3.one;
		UIPanel panel = hitParent.gameObject.AddComponent<UIPanel>();
		panel.clipping = UIDrawCall.Clipping.ConstrainButDontClip;
		panel.SetAnchor(this.parentPanel.gameObject);
		panel.updateAnchors = UIRect.AnchorUpdate.OnStart;
		yield return new WaitForEndOfFrame();
		this.SetPanelSort(panel, 1);
		panel.transform.localPosition = new Vector3(0f, 0f, 40f);
		int maxCharacter = 6;
		base.ui.hitIconObjectInstanced = new List<List<HitIcon>>();
		for (int i = 0; i < maxCharacter; i++)
		{
			List<HitIcon> list = new List<HitIcon>();
			for (int j = 0; j < base.uiProperty.hitIconLengthTime; j++)
			{
				GameObject gameObject = base.Instantiate<GameObject>(base.ui.hitIconObject);
				gameObject.name = string.Concat(new object[]
				{
					base.ui.hitIconObject.name,
					i,
					"_",
					j
				});
				UITweenController uitweenController = gameObject.AddComponent<UITweenController>();
				uitweenController.GetObject();
				uitweenController.afterObjectDisable = true;
				gameObject.transform.SetParent(hitParent.transform);
				gameObject.transform.position = Vector3.zero;
				gameObject.transform.localScale = base.uiProperty.hitIconLocalScale;
				list.Add(gameObject.GetComponent<HitIcon>());
			}
			base.ui.hitIconObjectInstanced.Add(list);
		}
		yield break;
	}

	public IEnumerator HUDInitialize()
	{
		Transform hudParent = new GameObject("HUD").transform;
		hudParent.gameObject.layer = LayerMask.NameToLayer("UI");
		base.ui.otherCeratedUITransforms.Add(hudParent);
		hudParent.SetParent(base.ui.uiRoot.transform);
		hudParent.position = Vector3.zero;
		hudParent.localScale = Vector3.one;
		this.currentSelectMonster.transform.SetParent(hudParent);
		UIPanel panel = hudParent.gameObject.AddComponent<UIPanel>();
		panel.clipping = UIDrawCall.Clipping.ConstrainButDontClip;
		panel.SetAnchor(this.parentPanel.gameObject);
		panel.updateAnchors = UIRect.AnchorUpdate.OnStart;
		yield return new WaitForEndOfFrame();
		this.SetPanelSort(panel, 2);
		panel.transform.localPosition = new Vector3(0f, 0f, 20f);
		for (int i = 0; i < base.battleStateData.totalPreloadCharacterLength; i++)
		{
			Transform transform = base.Instantiate<GameObject>(base.ui.hudObject).transform;
			transform.name = "HUD" + i;
			transform.SetParent(hudParent.transform);
			transform.position = Vector3.zero;
			transform.localScale = base.uiProperty.hudObjectLocalScale;
			base.ui.hudObjectInstanced.Add(transform.GetComponent<BattleUIHUD>());
			base.ui.hudObjectInstanced[i].InitNum();
			base.ui.hudColliderInstanced.Add(transform.GetComponent<Collider>());
			base.ui.hudObjectDepthController.Add(transform.gameObject.GetComponent<DepthController>());
			if (i >= base.battleStateData.playerCharacters.Length)
			{
				int onHoldWaitPressValue = i - base.battleStateData.playerCharacters.Length;
				base.ui.hudObjectInstanced[i].AddEvent(new Action<int>(base.input.OnPressHud), new Action(base.input.OffPressHud), onHoldWaitPressValue);
			}
		}
		foreach (BattleWave battleWave in base.hierarchyData.batteWaves)
		{
			if (battleWave.cameraType == 1)
			{
				Transform transform2 = base.Instantiate<GameObject>(base.ui.bigBossHudObject).transform;
				transform2.name = "BIG_BOSS_HUD";
				transform2.SetParent(hudParent.transform);
				transform2.localPosition = new Vector3(-20f, 230f, 0f);
				transform2.localScale = Vector3.one;
				transform2.gameObject.SetActive(false);
				base.ui.bigBossHudObjectInstanced = transform2.GetComponent<BattleUIHUD>();
				base.ui.bigBossHudObjectInstanced.InitNum();
				base.ui.hudColliderInstanced.Add(transform2.GetComponent<Collider>());
				base.ui.hudObjectDepthController.Add(transform2.gameObject.GetComponent<DepthController>());
				base.ui.bigBossHudObjectInstanced.AddEvent(new Action<int>(base.input.OnPressHud), new Action(base.input.OffPressHud), 0);
				break;
			}
		}
		yield break;
	}

	private void CreateSelectMonster()
	{
		GameObject uibattlePrefab = ResourcesPath.GetUIBattlePrefab("SelectMonster");
		GameObject gameObject = base.Instantiate<GameObject>(uibattlePrefab);
		Transform transform = gameObject.transform;
		transform.localPosition = Vector3.zero;
		transform.localScale = Vector3.one;
		this.currentSelectMonster = gameObject;
	}

	public IEnumerator ManualSelectTargetInitialize()
	{
		Transform manualSelectTargetParent = new GameObject("ManualSelectTarget").transform;
		manualSelectTargetParent.gameObject.layer = LayerMask.NameToLayer("UI");
		manualSelectTargetParent.SetParent(base.ui.skillSelectUi.transform);
		manualSelectTargetParent.localPosition = Vector3.zero;
		manualSelectTargetParent.localScale = Vector3.one;
		UIPanel panel = manualSelectTargetParent.gameObject.AddComponent<UIPanel>();
		panel.updateAnchors = UIRect.AnchorUpdate.OnStart;
		yield return new WaitForEndOfFrame();
		this.SetPanelSort(panel, 3);
		panel.transform.localPosition = new Vector3(0f, 0f, 40f);
		for (int i = 0; i < base.battleStateData.maxPreloadCharacterLength; i++)
		{
			Transform transform = base.Instantiate<GameObject>(base.ui.manualSelectTargetObject).transform;
			transform.SetParent(manualSelectTargetParent);
			transform.position = Vector3.zero;
			transform.localScale = base.uiProperty.manualSelectTargetObjectLocalScale;
			base.ui.manualSelectTargetObjectInstanced.Add(transform.GetComponent<ManualSelectTarget>());
			transform = base.Instantiate<GameObject>(base.ui.toleranceIconObject).transform;
			transform.SetParent(manualSelectTargetParent);
			transform.position = Vector3.zero;
			transform.localScale = base.uiProperty.manualSelectTargetObjectLocalScale;
			base.ui.toleranceIconObjectInstanced.Add(transform.GetComponent<ToleranceIcon>());
		}
		yield break;
	}

	public void DroppingItemInitialize()
	{
		int maxDropNum = base.battleStateData.maxDropNum;
		for (int i = 0; i < maxDropNum; i++)
		{
			Transform transform = base.Instantiate<GameObject>(base.ui.droppingItemObject).transform;
			transform.SetParent(base.ui.battleAlwaysUi.transform);
			transform.position = Vector3.zero;
			transform.localScale = Vector3.one;
			transform.name = "DropItem" + i;
			base.ui.droppingItemObjectInstanced.Add(transform.GetComponent<DroppingItem>());
		}
	}

	public void InstantiateGimmickStatusEffect()
	{
		if (base.ui.battleGimmickStatusObject == null)
		{
			return;
		}
		int num = 6;
		for (int i = 0; i < num; i++)
		{
			Transform transform = base.Instantiate<GameObject>(base.ui.battleGimmickStatusObject).transform;
			transform.SetParent(base.ui.battleAlwaysUi.transform);
			transform.position = Vector3.zero;
			transform.localScale = Vector3.one;
			transform.name = "GimmickStatus" + i;
			base.ui.battleGimmickStatusInstanced.Add(transform.gameObject);
			transform.gameObject.SetActive(false);
		}
	}

	public virtual IEnumerator AfterInitializeUI()
	{
		this.AfterInitializeUIBefore();
		for (int i = 0; i < base.ui.skillSelectUi.monsterButton.Length; i++)
		{
			BattleInputUtility.AddEvent(base.ui.skillSelectUi.monsterButton[i].playerMonsterDescriptionSwitch.onDisengagePress, new Action<int>(base.input.OnClickMonsterButton), i);
			BattleInputUtility.AddEvent(base.ui.skillSelectUi.monsterButton[i].playerMonsterDescriptionSwitch.onHoldWaitPress, new Action<int>(base.input.OnPressMonsterButton), i);
			BattleInputUtility.AddEvent(base.ui.skillSelectUi.monsterButton[i].playerMonsterDescriptionSwitch.onDisengagePress, new Action(base.input.OffPressMonsterButton));
		}
		for (int j = 0; j < base.ui.skillSelectUi.skillButton.Length; j++)
		{
			if (j > 0)
			{
				base.ui.skillSelectUi.skillButton[j].SetClickCallback(new Action<int>(base.input.OnClickSkillButton), j);
				base.ui.skillSelectUi.skillButton[j].SetHoldWaitPressCallback(new Action<int>(base.input.OnPressSkillButton), j);
				base.ui.skillSelectUi.skillButton[j].SetDisengagePressCallback(new Action<int>(base.input.OffPressSkillButton), j);
			}
			else
			{
				base.ui.skillSelectUi.skillButton[j].SetButtonCallback(new Action<int>(base.input.OnClickSkillButton), j);
			}
			base.ui.skillSelectUi.skillButton[j].SetRotationEffectCallback(new Action<int>(base.input.OnSkillButtonRotateAfter), j);
		}
		BattleInputUtility.AddEvent(base.ui.menuButton.onClick, new Action(base.input.OnClickMenuButton));
		base.ui.characterRevivalDialog.AddRevivalButtonEvent(new Action(base.input.OnClickCharacterRevivalButton));
		base.ui.characterRevivalDialog.AddCloseButtonEvent(new Action(base.input.OnClickCharacterRevivalCloseButton));
		base.ui.characterRevivalDialog.AddSpecificTradeButtonEvent(new Action(base.input.OnClickSpecificTradeButton));
		base.ui.autoPlayButton.AddEvent(new Action(base.input.OnClickAutoPlayButton));
		base.ui.x2PlayButton.AddEvent(new Action(base.input.OnClickX2PlayButton));
		BattleInputUtility.AddEvent(base.ui.menuButton.onClick, new Action(base.input.OnClickMenuButton));
		base.ui.menuDialog.AddRetireButtonEvent(new Action(base.input.OnClickMenuRetireButton));
		base.ui.menuDialog.AddCloseButtonEvent(new Action(base.input.OnClickMenuCloseButton));
		base.ui.dialogRetire.AddEvent(new Action(base.input.OnClickRetireDialogOkButton), new Action(base.input.OnClickRetireDialogCancelButton));
		base.ui.dialogContinue.AddEvent(new Action(base.input.OnClickSpecificTradeButton), new Action(base.input.OnClickContinueDialogRevivalButton), new Action(base.input.OnClickContinueDialogRetireButton), true);
		IEnumerator helpInitialize = base.stateManager.help.HelpInitialize();
		while (helpInitialize.MoveNext())
		{
			yield return null;
		}
		base.ui.initialInduction.AddEvent(new Action(base.input.OnClickCloseInitialInductionButton));
		base.ui.playerWinnerUi.AddEvent(new Action(base.input.OnClickSkipWinnerAction));
		IEnumerator hitIconInitialize = this.HitIconInitialize();
		while (hitIconInitialize.MoveNext())
		{
			yield return null;
		}
		IEnumerator hudInitialize = this.HUDInitialize();
		while (hudInitialize.MoveNext())
		{
			yield return null;
		}
		IEnumerator manualSelectTargetInitialize = this.ManualSelectTargetInitialize();
		while (manualSelectTargetInitialize.MoveNext())
		{
			yield return null;
		}
		this.DroppingItemInitialize();
		this.InstantiateGimmickStatusEffect();
		base.ui.rootPanel.RebuildAllDrawCalls();
		this.InitSpeedClearRound();
		yield break;
	}

	protected void InitSpeedClearRound()
	{
		if (!base.stateManager.onServerConnect)
		{
			return;
		}
		GameWebAPI.RespData_WorldMultiStartInfo respData_WorldMultiStartInfo = DataMng.Instance().RespData_WorldMultiStartInfo;
		bool flag = null != respData_WorldMultiStartInfo;
		int num;
		if (flag)
		{
			if (CMD_MultiRecruitPartyWait.UserType == CMD_MultiRecruitPartyWait.USER_TYPE.OWNER)
			{
				num = 2;
			}
			else
			{
				num = 3;
			}
		}
		else
		{
			num = 1;
		}
		foreach (GameWebAPI.RespDataMA_GetWorldDungeonOptionRewardM.WorldDungeonOptionReward worldDungeonOptionReward in MasterDataMng.Instance().RespDataMA_WorldDungeonOptionRewardM.worldDungeonOptionRewardM)
		{
			if (worldDungeonOptionReward.worldDungeonId.Equals(DataMng.Instance().WD_ReqDngResult.dungeonId) && int.Parse(worldDungeonOptionReward.joinType) == num && int.Parse(worldDungeonOptionReward.type) == 1)
			{
				this.speedRewardList.Add(worldDungeonOptionReward);
			}
		}
		if (this.speedRewardList.Count > 0)
		{
			base.hierarchyData.speedClearRound = int.Parse(this.speedRewardList[this.speedRewardList.Count - 1].clearValue);
		}
	}

	public void RemoveAllCachedUI()
	{
		foreach (BattleUIHUD battleUIHUD in base.ui.hudObjectInstanced)
		{
			if (battleUIHUD != null)
			{
				base.Destroy(battleUIHUD.gameObject);
			}
		}
		if (base.ui.bigBossHudObjectInstanced != null)
		{
			base.Destroy(base.ui.bigBossHudObjectInstanced.gameObject);
		}
		foreach (List<HitIcon> list in base.ui.hitIconObjectInstanced)
		{
			foreach (HitIcon hitIcon in list)
			{
				if (hitIcon != null)
				{
					base.Destroy(hitIcon.gameObject);
				}
			}
		}
		foreach (DroppingItem droppingItem in base.ui.droppingItemObjectInstanced)
		{
			if (droppingItem != null)
			{
				base.Destroy(droppingItem.gameObject);
			}
		}
		foreach (ManualSelectTarget manualSelectTarget in base.ui.manualSelectTargetObjectInstanced)
		{
			if (manualSelectTarget != null)
			{
				base.Destroy(manualSelectTarget.gameObject);
			}
		}
		foreach (Transform transform in base.ui.otherCeratedUITransforms)
		{
			if (transform != null)
			{
				base.Destroy(transform.gameObject);
			}
		}
		base.ui.Destroy();
	}

	protected void ShowContinueDialog()
	{
		base.StartCoroutine(this.WaitOpenCloseDialog(true, base.ui.dialogContinue.gameObject, base.ui.dialogContinue.openCloseDialog, null));
	}

	public IEnumerator HideContinueDialog()
	{
		IEnumerator wait = this.WaitOpenCloseDialog(false, base.ui.dialogContinue.gameObject, base.ui.dialogContinue.openCloseDialog, null);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		yield break;
	}

	public void ApplyLoadingGauge(int current, int max)
	{
		base.ui.initializeUi.ApplyLoadingGauge(current, max);
	}

	public override void ApplySetBattleStateRegistration()
	{
		this.battleScreenDetails.Add(BattleScreen.Initialize, new BattleScreenDetail(base.ui.initializeUi.widget, false));
		this.battleScreenDetails.Add(BattleScreen.BossStartAction, new BattleScreenDetail(base.ui.bossStartUi, false));
		if (base.ui.extraStartUi != null)
		{
			this.battleScreenDetails.Add(BattleScreen.ExtraStartAction, new BattleScreenDetail(base.ui.extraStartUi, false));
		}
		this.battleScreenDetails.Add(BattleScreen.StartAction, new BattleScreenDetail(false));
		this.battleScreenDetails.Add(BattleScreen.SkillSelects, new BattleScreenDetail(delegate()
		{
			BattleScreenDetail.DeactiveObjects(new UIWidget[]
			{
				base.ui.skillSelectUi.widget
			});
		}, delegate()
		{
			BattleScreenDetail.ActiveObjects(new UIWidget[]
			{
				base.ui.skillSelectUi.widget
			});
		}, true));
		this.battleScreenDetails.Add(BattleScreen.RoundStart, new BattleScreenDetail(true));
		this.battleScreenDetails.Add(BattleScreen.RoundActions, new BattleScreenDetail(base.ui.turnAction.widget, true));
		this.battleScreenDetails.Add(BattleScreen.EnemyTurnAction, new BattleScreenDetail(base.ui.enemyTurnUi, true));
		this.battleScreenDetails.Add(BattleScreen.IsWarning, new BattleScreenDetail(base.ui.isWarning.widget, true));
		this.battleScreenDetails.Add(BattleScreen.PoisonHit, new BattleScreenDetail(true));
		this.battleScreenDetails.Add(BattleScreen.RevivalCharacter, new BattleScreenDetail(true));
		this.battleScreenDetails.Add(BattleScreen.RoundStartActions, new BattleScreenDetail(base.ui.roundStart.widget, true));
		this.battleScreenDetails.Add(BattleScreen.RoundStartActionsLimit, new BattleScreenDetail(base.ui.roundLimitStart.widget, true));
		this.battleScreenDetails.Add(BattleScreen.RoundStartActionsChallenge, new BattleScreenDetail(base.ui.roundChallengeStart.widget, true));
		this.battleScreenDetails.Add(BattleScreen.NextBattle, new BattleScreenDetail(base.ui.nextWaveUi, false));
		this.battleScreenDetails.Add(BattleScreen.TimeOver, new BattleScreenDetail(base.ui.timeOverUi, false));
		this.battleScreenDetails.Add(BattleScreen.PlayerWinner, new BattleScreenDetail(new UIWidget[]
		{
			base.ui.playerWinnerUi.widget,
			base.ui.initialInduction.widget
		}, false));
		this.battleScreenDetails.Add(BattleScreen.PlayerFailed, new BattleScreenDetail(new UIWidget[]
		{
			base.ui.playerFailUi,
			base.ui.initialInduction.widget
		}, false));
		this.battleScreenDetails.Add(BattleScreen.Continue, new BattleScreenDetail(delegate()
		{
			BattleScreenDetail.DeactiveObjects(new UIWidget[]
			{
				base.ui.continueUi
			});
		}, delegate()
		{
			BattleScreenDetail.ActiveObjects(new UIWidget[]
			{
				base.ui.continueUi
			});
			this.ShowContinueDialog();
		}, true));
		base.ui.fadeoutUi.FadeIn(Color.black, 0f);
	}

	protected virtual void SetInitActive()
	{
		NGUITools.SetActiveSelf(base.ui.initializeUi.gameObject, true);
		if (base.ui.battleAlwaysUi != null)
		{
			NGUITools.SetActiveSelf(base.ui.battleAlwaysUi.gameObject, false);
		}
		if (base.ui.extraStartUi != null)
		{
			NGUITools.SetActiveSelf(base.ui.extraStartUi.gameObject, false);
		}
		NGUITools.SetActiveSelf(base.ui.bossStartUi.gameObject, false);
		NGUITools.SetActiveSelf(base.ui.skillSelectUi.gameObject, false);
		NGUITools.SetActiveSelf(base.ui.roundStart.gameObject, false);
		NGUITools.SetActiveSelf(base.ui.turnAction.gameObject, false);
		NGUITools.SetActiveSelf(base.ui.enemyTurnUi.gameObject, false);
		NGUITools.SetActiveSelf(base.ui.isWarning.gameObject, false);
		NGUITools.SetActiveSelf(base.ui.timeOverUi.gameObject, false);
		if (base.ui.playerWinnerUi != null)
		{
			NGUITools.SetActiveSelf(base.ui.playerWinnerUi.gameObject, false);
		}
		NGUITools.SetActiveSelf(base.ui.nextWaveUi.gameObject, false);
		NGUITools.SetActiveSelf(base.ui.playerFailUi.gameObject, false);
		if (base.ui.continueUi != null)
		{
			NGUITools.SetActiveSelf(base.ui.continueUi.gameObject, false);
		}
		NGUITools.SetActiveSelf(base.ui.initialInduction.gameObject, false);
		if (base.ui.characterRevivalDialog != null)
		{
			NGUITools.SetActiveSelf(base.ui.characterRevivalDialog.gameObject, false);
		}
		if (base.ui.menuDialog != null)
		{
			NGUITools.SetActiveSelf(base.ui.menuDialog.gameObject, false);
		}
		NGUITools.SetActiveSelf(base.ui.dialogRetire.gameObject, false);
		if (base.ui.helpDialog != null)
		{
			NGUITools.SetActiveSelf(base.ui.helpDialog.gameObject, false);
		}
		if (base.ui.characterStatusDescription != null)
		{
			NGUITools.SetActiveSelf(base.ui.characterStatusDescription.gameObject, false);
		}
		NGUITools.SetActiveSelf(base.ui.enemyStatusDescriptionGO, false);
		base.ui.initialInduction.HideRoot();
	}

	public void ApplyRoundStartRevivalText(bool onRevivalAp, bool onRevivalHp)
	{
		base.ui.roundStart.ApplyRoundStartRevivalText(onRevivalAp, onRevivalHp);
	}

	public void ApplyRoundLimitStartRevivalText(bool onRevivalAp, bool onRevivalHp)
	{
		base.ui.roundLimitStart.ApplyRoundStartRevivalText(onRevivalAp, onRevivalHp);
	}

	public void ApplyRoundChallengeStartRevivalText(bool onRevivalAp, bool onRevivalHp)
	{
		base.ui.roundChallengeStart.ApplyRoundStartRevivalText(onRevivalAp, onRevivalHp);
	}

	public void ApplyBattleStartAction(bool value)
	{
		base.ui.battleStartAction.SetActive(value);
	}

	public void ApplyBattleStartActionTitle(bool value)
	{
		base.ui.battleStartAction.ApplyBattleStartActionTitle(value);
	}

	public void ApplyPlayerLeaderSkill(bool isHavingLeaderSkill, string leaderSkillName, bool isChange = false)
	{
		base.ui.battleStartAction.ApplyPlayerLeaderSkill(isHavingLeaderSkill, leaderSkillName, isChange);
	}

	public void ApplyEnemyLeaderSkill(bool isHavingLeaderSkill, string leaderSkillName, bool isChange = false)
	{
		base.ui.battleStartAction.ApplyEnemyLeaderSkill(isHavingLeaderSkill, leaderSkillName, isChange);
	}

	public void ApplyVSUI(bool value)
	{
		base.ui.battleStartAction.ApplyVSUI(value);
	}

	public void ApplySkillButtonData(int index, SkillStatus skills, bool onEnable, bool onSkillLock, CharacterStateControl character, int useSkillCount)
	{
		base.ui.skillSelectUi.skillButton[index].ApplySkillButtonData(skills, onEnable, onSkillLock, character, useSkillCount);
	}

	public void ApplySkillButtonColliderActive(int index, bool value)
	{
		base.ui.skillSelectUi.skillButton[index].SetColliderActive(value);
	}

	public void ApplyTwoSkillButtonPosition()
	{
		base.ui.skillSelectUi.ApplyTwoSkillButtonPosition();
	}

	public void ApplyThreeSkillButtonPosition()
	{
		base.ui.skillSelectUi.ApplyThreeSkillButtonPosition();
	}

	public void ApplySkillButtonReflesh()
	{
		base.ui.skillSelectUi.RefleshSkillButton();
	}

	public void ApplySkillButtonRotation(int oldIndex = -1, int newIndex = -1)
	{
		base.ui.skillSelectUi.ApplySkillButtonRotation(oldIndex, newIndex);
	}

	public void ApplyAllMonsterButtonEnable(bool value)
	{
		foreach (BattleMonsterButton battleMonsterButton in base.ui.skillSelectUi.monsterButton)
		{
			battleMonsterButton.gameObject.SetActive(value);
		}
	}

	public void ApplyMonsterButtonEnable(int index, bool isSelect, bool isDead)
	{
		if (isDead)
		{
			base.ui.skillSelectUi.monsterButton[index].SetType(BattleMonsterButton.Type.Dead);
			return;
		}
		if (isSelect)
		{
			base.ui.skillSelectUi.monsterButton[index].SetType(BattleMonsterButton.Type.Select);
		}
		else
		{
			base.ui.skillSelectUi.monsterButton[index].SetType(BattleMonsterButton.Type.None);
		}
	}

	public void ApplyMonsterButtonIcon(int index, CharacterStateControl characterStatus, bool isLeade, string resourcePath, string assetBundlePath)
	{
		base.ui.skillSelectUi.monsterButton[index].ApplyMonsterButtonIcon(characterStatus, isLeade, resourcePath, assetBundlePath);
		base.ui.skillSelectUi.monsterButton[index].SetPlayerNameActive(base.battleMode == BattleMode.Multi);
		base.ui.skillSelectUi.monsterButton[index].SetPlayerNumber(0);
	}

	public void ApplyLeaderIcon(int index, bool isLeader)
	{
		base.ui.skillSelectUi.monsterButton[index].ApplyLeaderIcon(isLeader);
	}

	public void ApplySkillName(bool isShow, string skillName = "", CharacterStateControl characterStateControl = null)
	{
		base.ui.turnAction.ApplySkillName(isShow, skillName, characterStateControl);
	}

	public void ApplySkillDescriptionEnable(int index, bool onEnable)
	{
		base.ui.skillSelectUi.skillButton[index].ApplySkillDescriptionEnable(onEnable);
	}

	public int GetSkillButtonLength()
	{
		return base.ui.skillSelectUi.skillButton.Length;
	}

	public bool GetIsClickedUI()
	{
		foreach (UITouchChecker uitouchChecker in base.ui.skillSelectUi.touchChecker)
		{
			if (uitouchChecker.gameObject.activeInHierarchy && uitouchChecker.isClicked)
			{
				return true;
			}
		}
		return false;
	}

	public void ApplyMonsterDescription(bool isShow, CharacterStateControl characterStatus = null, int currentSelectCharacter = 0)
	{
		NGUITools.SetActiveSelf(base.ui.characterStatusDescription.gameObject, isShow);
		if (!isShow)
		{
			return;
		}
		base.ui.characterStatusDescription.ApplyMonsterDescription(isShow, characterStatus);
		base.ui.uiRoot.GetComponent<UIPanel>().Refresh();
	}

	public void ApplyEnemyDescription(bool isShow, CharacterStateControl characterStatus = null)
	{
		NGUITools.SetActiveSelf(base.ui.enemyStatusDescriptionGO, isShow);
		if (!isShow)
		{
			return;
		}
		if (base.ui.pvpEnemyStatusDescription != null && BattleStateManager.current.battleMode == BattleMode.PvP)
		{
			base.ui.pvpEnemyStatusDescription.ApplyMonsterDescription(isShow, characterStatus);
		}
		else
		{
			base.ui.enemyStatusDescription.ApplyEnemyDescription(isShow, characterStatus);
		}
	}

	public void ApplyCurrentSelectArrow(bool isEnable, Vector3 position = default(Vector3), int index = 0)
	{
		bool state = position.z > 0f && position.x > 0f && position.x < 1f && position.y > 0f && position.y < 1f && isEnable;
		NGUITools.SetActiveSelf(this.currentSelectMonster, state);
		if (base.ui.arrowDepthController == null)
		{
			base.ui.arrowDepthController = this.currentSelectMonster.GetComponent<DepthController>();
		}
		Vector3 position2 = base.ui.uiCamera.ViewportToWorldPoint(position);
		float z = position2.z;
		position2.Set(position2.x, position2.y, this.currentSelectMonster.transform.position.z);
		this.currentSelectMonster.transform.position = position2;
		int widgetManualDepth = 0;
		if (base.ui.hudObjectDepthController.Count > index)
		{
			widgetManualDepth = base.ui.hudObjectDepthController[index].GetDepth();
		}
		base.ui.arrowDepthController.SetWidgetManualDepth(widgetManualDepth);
	}

	public HitIcon ApplyShowHitIcon(int index, Vector3 position, AffectEffect affect, int onDamage, Strength onWeak, bool onMiss, bool onCrithical, bool isDrain, bool isCounter, bool isReflection, ExtraEffectType extraEffectType = ExtraEffectType.Non, bool isHitIcon = true)
	{
		int index2 = 0;
		for (int i = 0; i < base.ui.hitIconObjectInstanced[index].Count; i++)
		{
			index2 = i;
			if (!base.ui.hitIconObjectInstanced[index][i].gameObject.activeSelf)
			{
				break;
			}
		}
		HitIcon hitIcon = base.ui.hitIconObjectInstanced[index][index2];
		if (isHitIcon)
		{
			hitIcon.transform.localScale = base.uiProperty.hitIconLocalScale;
			NGUITools.SetActiveSelf(hitIcon.gameObject, true);
			Vector3 vector = base.ui.uiCamera.ViewportToWorldPoint(position);
			hitIcon.transform.position = new Vector3(vector.x, vector.y, 0f);
			DepthController component = hitIcon.GetComponent<DepthController>();
			component.SetWidgetDepth(index);
			NGUITools.SetActiveSelf(hitIcon.gameObject, true);
			hitIcon.ApplyShowHitIcon(affect, onDamage, onWeak, onMiss, onCrithical, isDrain, isCounter, isReflection, extraEffectType);
		}
		return hitIcon;
	}

	public void ApplyHideHitIcon()
	{
		foreach (List<HitIcon> list in base.ui.hitIconObjectInstanced)
		{
			foreach (HitIcon hitIcon in list)
			{
				NGUITools.SetActiveSelf(hitIcon.gameObject, false);
			}
		}
	}

	public virtual void ApplyCharacterHudContent(int index, CharacterStateControl characterStatus = null)
	{
		base.ui.hudObjectInstanced[index].ApplyCharacterHudContent(characterStatus);
	}

	public virtual void ApplyBigBossCharacterHudContent(CharacterStateControl characterStatus = null)
	{
		if (base.ui.bigBossHudObjectInstanced != null && characterStatus != null && characterStatus.isEnemy && base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1)
		{
			base.ui.bigBossHudObjectInstanced.ApplyCharacterHudContent(characterStatus);
		}
	}

	public void ApplyEnableCharacterRevivalWindow(bool isShow, bool isPossibleRevival = false, Action onFinishedAction = null)
	{
		base.StartCoroutine(base.ui.characterRevivalDialog.ApplyEnableCharacterRevivalWindow(isShow, isPossibleRevival, onFinishedAction));
	}

	public void PlayBattleGimmickStatusAnimator(int index, Vector3 pos, bool isUp)
	{
		if (base.ui.battleGimmickStatusInstanced.Count > index)
		{
			GameObject gameObject = base.ui.battleGimmickStatusInstanced[index];
			gameObject.SetActive(true);
			Vector3 position = base.ui.uiCamera.ViewportToWorldPoint(pos);
			gameObject.transform.position = position;
			Animator component = gameObject.GetComponent<Animator>();
			if (component != null)
			{
				component.SetBool("IsStatusUp", isUp);
				component.SetTrigger("Play");
			}
		}
	}

	public IEnumerator ApplyDroppedItemMove(Vector3 startPosition, bool isRare, int index = 0)
	{
		if (base.ui.droppingItemObjectInstanced.Count < index)
		{
			yield break;
		}
		bool onNormalFlash = false;
		bool onRareFlash = false;
		base.ui.droppingItemObjectInstanced[index].SetActive(true);
		base.ui.droppingItemObjectInstanced[index].SetRare(isRare);
		base.ui.droppingItemObjectInstanced[index].ResetToBeginning();
		Vector3 pos = base.ui.uiCamera.ViewportToWorldPoint(startPosition);
		pos.Set(pos.x, pos.y, 0f);
		base.ui.droppingItemObjectInstanced[index].transform.position = pos;
		Hashtable movedHash = new Hashtable();
		movedHash.Add("position", base.ui.itemInfoField.GetBoxImagePosition(isRare));
		movedHash.Add("time", TimeExtension.GetTimeScaleDivided(base.uiProperty.droppingItemMoveDuration));
		movedHash.Add("easetype", base.uiProperty.droppingItemMoveEaseType);
		iTween.MoveTo(base.ui.droppingItemObjectInstanced[index].gameObject, movedHash);
		if (isRare)
		{
			onRareFlash = true;
		}
		else
		{
			onNormalFlash = true;
		}
		IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(TimeExtension.GetTimeScaleDivided(base.uiProperty.droppingItemMoveDuration), null, null);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		base.ui.droppingItemObjectInstanced[index].PlayForward();
		wait = base.stateManager.time.WaitForCertainPeriodTimeAction(TimeExtension.GetTimeScaleDivided(base.uiProperty.droppingItemUIActionDuration - base.uiProperty.droppingItemMoveDuration), null, null);
		while (wait.MoveNext())
		{
			object obj2 = wait.Current;
			yield return obj2;
		}
		if (onNormalFlash)
		{
			base.ui.itemInfoField.TwinkleNormalBox();
		}
		if (onRareFlash)
		{
			base.ui.itemInfoField.TwinkleRareBox();
		}
		yield break;
	}

	public void ApplyDroppedItemHide()
	{
		foreach (DroppingItem droppingItem in base.ui.droppingItemObjectInstanced)
		{
			droppingItem.SetActive(false);
		}
	}

	public void ApplyTurnActionBarSwipeout(bool isReset)
	{
		base.ui.turnAction.ApplyTurnActionBarSwipeout(isReset);
	}

	public void ApplyShowMenuWindow(bool isShow, bool isEnableRetire = true, Action onFinishedAction = null)
	{
		base.StartCoroutine(base.ui.menuDialog.ApplyShowMenuWindow(isShow, isEnableRetire, onFinishedAction));
	}

	public void ApplyShowRetireWindow(bool isShow, Action onFinishedAction = null)
	{
		base.StartCoroutine(base.ui.menuDialog.ApplyShowRetireWindow(isShow, onFinishedAction));
	}

	public void ApplyWarning(SufferStateProperty.SufferType sufferType, CharacterStateControl characterStateControl = null)
	{
		base.ui.isWarning.ApplyWarning(sufferType, characterStateControl);
	}

	public void ApplyWarning(string value, bool isEnemy)
	{
		base.ui.isWarning.ApplyWarning(value, isEnemy);
	}

	public void ApplyDroppedItemNumber(int normalItems, int rareItems)
	{
		if (base.ui.menuDialog == null || BattleStateManager.current.battleMode == BattleMode.PvP)
		{
			return;
		}
		base.ui.itemInfoField.ApplyDroppedItemNumber(normalItems, rareItems);
		base.ui.menuDialog.dropNormalItemCount.text = normalItems.ToString();
		base.ui.menuDialog.dropRareItemCount.text = rareItems.ToString();
	}

	public void ApplyChipNumber(int gettedChipNumber)
	{
		base.ui.menuDialog.coinCount.text = gettedChipNumber.ToString();
	}

	public void ApplyExpNumber(int gettedExpNumber)
	{
		base.ui.menuDialog.expCount.text = gettedExpNumber.ToString();
	}

	public void ApplyContinueNeedDigiStone(int needDigiStone, int currentDigistone, bool isCheckDigiStoneZero = false)
	{
		base.ui.dialogContinue.ApplyContinueNeedDigiStone(base.battleStateData, needDigiStone, currentDigistone, isCheckDigiStoneZero);
	}

	public void ApplyAreaName(string areaName)
	{
		base.ui.menuDialog.areaName.text = areaName;
	}

	public void ApplyDigiStoneNumber(int digiStoneNumber)
	{
		base.ui.characterRevivalDialog.ApplyDigiStoneNumber(digiStoneNumber);
		base.ui.dialogContinue.ApplyDigiStoneNumber(digiStoneNumber);
	}

	public void ApplyManualSelectTarget(int index, bool isShow, Strength[] iconTypes = null)
	{
		ManualSelectTarget manualSelectTarget = base.ui.manualSelectTargetObjectInstanced[index];
		ToleranceIcon toleranceIcon = base.ui.toleranceIconObjectInstanced[index];
		manualSelectTarget.SetActive(isShow);
		toleranceIcon.SetActive(isShow);
		if (!isShow)
		{
			return;
		}
		manualSelectTarget.SetToleranceTarget(iconTypes);
		toleranceIcon.SetToleranceIcon(iconTypes);
	}

	public void ApplyCharacterHudBoss(int index, bool isBoss)
	{
		base.ui.hudObjectInstanced[index].ApplyCharacterHudBoss(isBoss);
	}

	public void ApplyBigBossCharacterHudBoss(bool isBoss)
	{
		if (base.ui.bigBossHudObjectInstanced != null && base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1)
		{
			base.ui.bigBossHudObjectInstanced.ApplyCharacterHudBoss(isBoss);
		}
	}

	private void ApplyManualSelectTargetReposition(int index, Vector3 position)
	{
		ManualSelectTarget manualSelectTarget = base.ui.manualSelectTargetObjectInstanced[index];
		if (manualSelectTarget.gameObject.activeInHierarchy)
		{
			Vector3 vector = base.ui.uiCamera.ViewportToWorldPoint(position);
			manualSelectTarget.transform.position = new Vector3(vector.x, vector.y, manualSelectTarget.transform.parent.transform.position.z);
		}
		ToleranceIcon toleranceIcon = base.ui.toleranceIconObjectInstanced[index];
		if (toleranceIcon.gameObject.activeInHierarchy)
		{
			Vector3 vector2 = base.ui.uiCamera.ViewportToWorldPoint(position);
			toleranceIcon.transform.position = new Vector3(vector2.x, vector2.y, toleranceIcon.transform.parent.transform.position.z);
		}
	}

	private void SetActiveCharacterHud(int index, bool isShow, Vector3 position = default(Vector3))
	{
		bool state = position.z > 0f && position.x > 0f && position.x < 1f && position.y > 0f && position.y < 1f && isShow;
		NGUITools.SetActiveSelf(base.ui.hudObjectInstanced[index].gameObject, state);
	}

	private void ApplyCharacterHudPosition(int index, Vector3 position)
	{
		this.SetActiveCharacterHud(index, true, position);
		Vector3 position2 = base.ui.uiCamera.ViewportToWorldPoint(position);
		float z = position2.z;
		position2.Set(position2.x, position2.y, base.ui.hudObjectInstanced[index].transform.parent.transform.position.z);
		base.ui.hudObjectInstanced[index].transform.position = position2;
		int widgetManualDepth = (int)(z * 1000f) * -1;
		base.ui.hudObjectDepthController[index].SetWidgetManualDepth(widgetManualDepth);
	}

	private void ApplyCharacterHudPosition(int index, Vector3 position, int depth)
	{
		this.SetActiveCharacterHud(index, true, position);
		Vector3 position2 = base.ui.uiCamera.ViewportToWorldPoint(position);
		float z = position2.z;
		position2.Set(position2.x, position2.y, base.ui.hudObjectInstanced[index].transform.parent.transform.position.z);
		base.ui.hudObjectInstanced[index].transform.position = position2;
		base.ui.hudObjectDepthController[index].SetWidgetManualDepth(depth);
	}

	private void ApplyBigBossCharacterHud(bool isShow)
	{
		if (base.ui.bigBossHudObjectInstanced != null && base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1)
		{
			NGUITools.SetActiveSelf(base.ui.bigBossHudObjectInstanced.gameObject, isShow);
		}
	}

	public void ApplyCharacterHudReset(int index)
	{
		base.ui.hudObjectInstanced[index].ApplyCharacterHudReset();
	}

	public void ApplyBigBossCharacterHudReset()
	{
		if (base.ui.bigBossHudObjectInstanced != null && base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1)
		{
			base.ui.bigBossHudObjectInstanced.ApplyCharacterHudReset();
		}
	}

	private void ApplyCharacterHudCollider(int index, bool isEnable)
	{
		base.ui.hudColliderInstanced[index].enabled = isEnable;
	}

	public virtual void ApplySetAlwaysUIColliders(bool isEnable)
	{
		base.ui.battleAlwaysUi.SetColliderActive(isEnable);
	}

	public void ApplySetContinueUIColliders(bool isEnable)
	{
		if (base.ui.dialogContinue == null)
		{
			return;
		}
		base.ui.dialogContinue.SetColliderActive(isEnable);
	}

	public void ApplyWaveAndRound(int wave, int round)
	{
		base.ui.itemInfoField.SetRemainingRoundText(-1);
		if (base.hierarchyData.batteWaves.Length == 0 || base.hierarchyData.batteWaves.Length <= wave)
		{
			base.ui.itemInfoField.ApplyWaveAndRound(wave, round, base.hierarchyData.batteWaves.Length);
			if (base.ui.menuDialog != null && BattleStateManager.current.battleMode != BattleMode.PvP)
			{
				base.ui.menuDialog.SetValuetextReplacer(wave, round, base.hierarchyData.batteWaves.Length);
			}
		}
		else
		{
			int maxWave = base.hierarchyData.battleNum.ToInt32();
			BattleWave battleWave = base.hierarchyData.batteWaves[wave];
			base.ui.itemInfoField.ApplyWaveAndRound(battleWave, round, maxWave);
			if (base.ui.menuDialog != null && BattleStateManager.current.battleMode != BattleMode.PvP)
			{
				base.ui.menuDialog.SetValuetextReplacer(battleWave, round, maxWave);
				if (base.hierarchyData.limitRound > 0)
				{
					base.ui.itemInfoField.SetRemainingRoundText(base.hierarchyData.limitRound - base.battleStateData.totalRoundNumber + 1);
				}
				else
				{
					base.ui.itemInfoField.SetRemainingRoundText(-1);
				}
			}
			else
			{
				base.ui.itemInfoField.SetRemainingRoundText(ClassSingleton<MultiBattleData>.Instance.MaxRoundNum - base.battleStateData.totalRoundNumber);
			}
		}
		if (base.hierarchyData.limitRound > 0 || base.hierarchyData.speedClearRound > 0)
		{
			int num = base.hierarchyData.limitRound - base.battleStateData.totalRoundNumber;
			num++;
			base.ui.roundLimitStart.ApplyWaveAndRound(round, num);
			base.ui.roundChallengeStart.ApplyWaveAndRoundSpeed(base.battleStateData.totalRoundNumber, this.speedRewardList);
		}
		base.ui.roundStart.ApplyWaveAndRound(round);
	}

	public IEnumerator ApplyShowInitialInduction(int type)
	{
		base.ui.playerWinnerUi.SetColliderEnabled(false);
		base.ui.initialInduction.ShowFace(base.hierarchyData, type);
		IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(base.uiProperty.showInitialIntroductionDialogWaitSecond, null, null);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		while (base.battleStateData.isShowInitialIntroduction)
		{
			yield return null;
		}
		base.ui.initialInduction.PlayTween();
		SoundPlayer.PlayButtonEnter();
		wait = base.stateManager.time.WaitForCertainPeriodTimeAction(base.uiProperty.afterHideInitialIntroductionDialogWaitSecond, null, null);
		while (wait.MoveNext())
		{
			object obj2 = wait.Current;
			yield return obj2;
		}
		yield break;
	}

	public void ApplySpecificTrade(bool isShow = false)
	{
		base.ui.dialogContinue.ApplySpecificTrade(isShow);
	}

	public void ApplyAutoPlay(int isEnable)
	{
		base.ui.autoPlayButton.ApplyAutoPlay(isEnable);
	}

	public void Apply2xPlay(bool isEnable)
	{
		base.ui.x2PlayButton.Apply2xPlay(isEnable);
	}

	public void ApplyFadeOutScreen(BattleScreen previousScreen)
	{
		if (previousScreen == BattleScreen.PlayerFailed)
		{
			base.ui.fadeoutUi.Fade(Color.black, 0f, 0.5f);
		}
		else
		{
			base.ui.fadeoutUi.Fade(Color.black, 0f, 0f);
		}
	}

	public void Fade(Color color, float fadeTime = 0f, float alpha = 0f)
	{
		base.ui.fadeoutUi.Fade(color, fadeTime, alpha);
	}

	public void ShowHidePlayerWinnerButton(bool isShow = false)
	{
		if (base.ui.playerWinnerUi == null)
		{
			NGUITools.SetActiveSelf(base.ui.winGameObject, isShow);
		}
		else
		{
			NGUITools.SetActiveSelf(base.ui.playerWinnerUi.nextButtonGO, isShow);
		}
	}

	public IEnumerator IsStageStartAnimation()
	{
		if (base.ui.extraStartUi != null)
		{
			Animation effectAnimation = base.ui.extraStartUi.GetComponentInChildren<Animation>();
			if (effectAnimation != null)
			{
				while (effectAnimation.isPlaying)
				{
					yield return new WaitForEndOfFrame();
				}
			}
		}
		yield break;
	}

	public void ShowBattleExtraEffect(BattleExtraEffectUI.AnimationType animationType)
	{
		base.ui.battleExtraEffectUI.gameObject.SetActive(true);
		base.ui.battleExtraEffectUI.Play(animationType);
	}

	public void HideBattleExtraEffect()
	{
		base.ui.battleExtraEffectUI.gameObject.SetActive(false);
	}

	public bool IsBattleExtraEffect()
	{
		return base.ui.battleExtraEffectUI.isPlaying;
	}

	public void ShowBattleChipEffect(float waitTime, Action endAction)
	{
		if (base.ui.chipBarLnvocation != null)
		{
			base.ui.chipBarLnvocation.PlayAnimation(waitTime, endAction);
		}
	}

	public IEnumerator ShowChipIcon(float waitTime, Vector3 pos, int[] chips)
	{
		foreach (int chipId in chips)
		{
			Transform chipTransform = base.Instantiate<GameObject>(base.ui.chipThumbnailAdvent).transform;
			if (!(chipTransform == null))
			{
				chipTransform.gameObject.SetActive(false);
				chipTransform.SetParent(base.ui.battleAlwaysUi.transform);
				chipTransform.position = Vector3.zero;
				chipTransform.localScale = Vector3.one;
				yield return new WaitForEndOfFrame();
				Vector3 viewportPoint = base.hierarchyData.cameraObject.camera3D.WorldToViewportPoint(pos);
				Vector3 positon = base.ui.uiCamera.ViewportToWorldPoint(viewportPoint);
				chipTransform.position = positon;
				chipTransform.gameObject.SetActive(true);
				ChipThumbnailAdvent chipAdvent = chipTransform.GetComponent<ChipThumbnailAdvent>();
				if (!(chipAdvent == null))
				{
					GameWebAPI.RespDataMA_ChipM.Chip chipData = ChipDataMng.GetChipMainData(chipId.ToString());
					if (chipData != null)
					{
						chipAdvent.SetData(chipData);
						yield return new WaitForSeconds(waitTime);
					}
				}
			}
		}
		yield break;
	}

	public void SetManualSelectTarget(int index, bool isShow = false, CharacterStateControl character = null, SkillStatus skill = null)
	{
		if (!isShow)
		{
			this.ApplyManualSelectTarget(index, isShow, null);
			return;
		}
		Strength[] skillStrengthList = skill.GetSkillStrengthList(character.tolerance);
		this.ApplyManualSelectTarget(index, isShow, skillStrengthList);
	}

	public void SetManualSelectTargetReposition(int index, CharacterStateControl character = null)
	{
		this.ApplyManualSelectTargetReposition(index, this.GetCharacterCenterPosition2DFunction(character));
	}

	public void SetHudCollider(bool enable)
	{
		CharacterStateControl[] totalCharacters = base.battleStateData.GetTotalCharacters();
		for (int i = 0; i < totalCharacters.Length; i++)
		{
			this.ApplyCharacterHudCollider(i, totalCharacters[i].isEnemy && enable);
		}
	}

	public Vector3 GetCharacterCenterPosition2DFunction(CharacterStateControl character)
	{
		return base.hierarchyData.cameraObject.camera3D.WorldToViewportPoint(character.CharacterParams.characterCenterTarget.position);
	}

	public Vector3 GetFixableCharacterCenterPosition2DFunction(CharacterStateControl character)
	{
		return base.hierarchyData.cameraObject.camera3D.WorldToViewportPoint(character.CharacterParams.GetFixableCenterPosition());
	}

	public Vector3 GetHUDCenterPosition2DFunction(CharacterStateControl character)
	{
		return base.hierarchyData.cameraObject.camera3D.WorldToViewportPoint(character.CharacterParams.HudPosition());
	}

	public void ShowCharacterHUDFunction(params CharacterStateControl[] characters)
	{
		foreach (CharacterStateControl characterStateControl in characters)
		{
			if (!characterStateControl.isDied || !characterStateControl.isDiedJustBefore)
			{
				int index = (!characterStateControl.isEnemy) ? characterStateControl.myIndex : (characterStateControl.myIndex + base.battleStateData.playerCharacters.Length);
				base.stateManager.uiControl.ApplyCharacterHudContent(index, characterStateControl);
			}
		}
		if (base.ui.bigBossHudObjectInstanced != null && base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1)
		{
			CharacterStateControl[] enemies = base.battleStateData.enemies;
			int num = 0;
			if (num < enemies.Length)
			{
				CharacterStateControl characterStatus = enemies[num];
				base.stateManager.uiControl.ApplyBigBossCharacterHudContent(characterStatus);
			}
		}
		this.RepositionCharacterHUDPosition(characters);
	}

	public void HideCharacterHUDFunction()
	{
		for (int i = 0; i < base.battleStateData.totalPreloadCharacterLength; i++)
		{
			this.SetActiveCharacterHud(i, false, default(Vector3));
		}
		this.ApplyBigBossCharacterHud(false);
	}

	public void RepositionCharacterHUDPosition(params CharacterStateControl[] characters)
	{
		List<CharacterStateControl> list = new List<CharacterStateControl>(characters);
		Comparison<CharacterStateControl> comparison = delegate(CharacterStateControl x, CharacterStateControl y)
		{
			if (x == y)
			{
				return 0;
			}
			float z = this.GetCharacterCenterPosition2DFunction(x).z;
			float z2 = this.GetCharacterCenterPosition2DFunction(y).z;
			if (z < z2)
			{
				return -1;
			}
			if (z > z2)
			{
				return 1;
			}
			return 0;
		};
		list.Sort(comparison);
		this.newSortDepthDataList = new List<BattleUIControlBasic.SortDepthData>();
		foreach (var <>__AnonType in list.Select((CharacterStateControl item, int index) => new
		{
			item,
			index
		}))
		{
			int index3 = (!<>__AnonType.item.isEnemy) ? <>__AnonType.item.myIndex : (<>__AnonType.item.myIndex + base.battleStateData.playerCharacters.Length);
			int depth = <>__AnonType.index * -1000;
			BattleUIControlBasic.SortDepthData sortDepthData = new BattleUIControlBasic.SortDepthData();
			for (int i = 0; i < characters.Length; i++)
			{
				if (<>__AnonType.item == characters[i])
				{
					sortDepthData.index = i;
				}
			}
			sortDepthData.depth = depth;
			this.newSortDepthDataList.Add(sortDepthData);
			if (<>__AnonType.item.isDied && <>__AnonType.item.isDiedJustBefore)
			{
				this.SetActiveCharacterHud(index3, false, default(Vector3));
			}
			else
			{
				this.ApplyCharacterHudPosition(index3, this.GetHUDCenterPosition2DFunction(<>__AnonType.item), depth);
			}
		}
		this.SortCharacterHudUI();
		if (base.ui.bigBossHudObjectInstanced != null && base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1)
		{
			foreach (CharacterStateControl characterStateControl in list)
			{
				int index2 = (!characterStateControl.isEnemy) ? characterStateControl.myIndex : (characterStateControl.myIndex + base.battleStateData.playerCharacters.Length);
				if (characterStateControl.isEnemy)
				{
					this.SetActiveCharacterHud(index2, false, default(Vector3));
					if (characterStateControl.isDied && characterStateControl.isDiedJustBefore)
					{
						this.ApplyBigBossCharacterHud(false);
					}
					else
					{
						this.ApplyBigBossCharacterHud(true);
					}
				}
			}
		}
	}

	public void CharacterHudResetAndUpdate(bool isSetDieJustBefore = false)
	{
		foreach (CharacterStateControl characterStateControl in base.battleStateData.GetTotalCharacters())
		{
			int totalCharacterIndex = base.battleStateData.GetTotalCharacterIndex(characterStateControl);
			if (isSetDieJustBefore && characterStateControl.isDied)
			{
				characterStateControl.isDiedJustBefore = true;
			}
			this.SetActiveCharacterHud(totalCharacterIndex, !characterStateControl.isDied, default(Vector3));
			this.ApplyCharacterHudContent(totalCharacterIndex, characterStateControl);
			this.ApplyCharacterHudReset(totalCharacterIndex);
		}
		if (base.ui.bigBossHudObjectInstanced != null && base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1)
		{
			foreach (CharacterStateControl characterStateControl2 in base.battleStateData.GetTotalCharacters())
			{
				int index = (!characterStateControl2.isEnemy) ? characterStateControl2.myIndex : (characterStateControl2.myIndex + base.battleStateData.playerCharacters.Length);
				if (characterStateControl2.isEnemy)
				{
					this.SetActiveCharacterHud(index, false, default(Vector3));
					this.ApplyBigBossCharacterHud(!characterStateControl2.isDied);
					this.ApplyBigBossCharacterHudContent(characterStateControl2);
					this.ApplyBigBossCharacterHudReset();
					break;
				}
			}
		}
	}

	public void SetMenuAuto2xButtonEnabled(bool isEnable)
	{
		this.ApplySetAlwaysUIColliders(isEnable);
		base.battleStateData.isImpossibleShowMenu = !isEnable;
	}

	public IEnumerator WaitOpenCloseDialog(bool isOpen, GameObject dialogObject, UIOpenCloseDialog openCloseDialog, Action onFinishedClose = null)
	{
		base.stateManager.dialogOpenCloseWaitFlag = true;
		this.SetTouchEnable(false);
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
		this.SetTouchEnable(true);
		base.stateManager.dialogOpenCloseWaitFlag = false;
		yield break;
	}

	public void SetTouchEnable(bool enable)
	{
		base.ui.rootCamera.useMouse = enable;
		base.ui.rootCamera.useTouch = enable;
		base.ui.rootCamera.useKeyboard = enable;
		base.ui.rootCamera.useController = enable;
	}

	private void SortCharacterHudUI()
	{
		bool flag = false;
		this.newSortDepthDataList.Sort((BattleUIControlBasic.SortDepthData a, BattleUIControlBasic.SortDepthData b) => a.depth - b.depth);
		if (this.oldSortDepthDataList.Count > 0)
		{
			for (int i = 0; i < this.oldSortDepthDataList.Count; i++)
			{
				if (this.newSortDepthDataList.Count > i && this.oldSortDepthDataList[i].index != this.newSortDepthDataList[i].index)
				{
					UIPanel panel = base.ui.hudObjectDepthController[this.oldSortDepthDataList[i].index].GetPanel();
					if (panel != null)
					{
						panel.SortWidgetsByManualDepth();
						panel.RebuildAllDrawCalls();
						flag = true;
					}
					break;
				}
			}
		}
		else
		{
			foreach (BattleUIControlBasic.SortDepthData sortDepthData in this.newSortDepthDataList)
			{
				UIPanel panel2 = base.ui.hudObjectDepthController[sortDepthData.index].GetPanel();
				if (panel2 != null)
				{
					panel2.SortWidgetsByManualDepth();
					panel2.RebuildAllDrawCalls();
					flag = true;
				}
			}
		}
		if (flag)
		{
			this.oldSortDepthDataList = this.newSortDepthDataList;
			flag = false;
		}
	}

	private class SortDepthData
	{
		public int index;

		public int depth;
	}
}
