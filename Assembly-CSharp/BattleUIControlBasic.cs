using BattleStateMachineInternal;
using Master;
using MultiBattle.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUIControlBasic : BattleUIControl
{
	private UIPanel parentPanel;

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
		this.ApplyCurrentSelectArrow(false, default(Vector3));
		yield break;
	}

	public void AfterInitializeUIBefore()
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
			List<HitIcon> currentHitIconObjectInstanced = new List<HitIcon>();
			for (int i2 = 0; i2 < base.uiProperty.hitIconLengthTime; i2++)
			{
				GameObject hit = base.Instantiate<GameObject>(base.ui.hitIconObject);
				hit.name = string.Concat(new object[]
				{
					base.ui.hitIconObject.name,
					i,
					"_",
					i2
				});
				UITweenController twC = hit.AddComponent<UITweenController>();
				twC.GetObject();
				twC.afterObjectDisable = true;
				hit.transform.SetParent(hitParent.transform);
				hit.transform.position = Vector3.zero;
				hit.transform.localScale = base.uiProperty.hitIconLocalScale;
				currentHitIconObjectInstanced.Add(hit.GetComponent<HitIcon>());
			}
			base.ui.hitIconObjectInstanced.Add(currentHitIconObjectInstanced);
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
			Transform hud = base.Instantiate<GameObject>(base.ui.hudObject).transform;
			hud.name = "HUD" + i;
			hud.SetParent(hudParent.transform);
			hud.position = Vector3.zero;
			hud.localScale = base.uiProperty.hudObjectLocalScale;
			base.ui.hudObjectInstanced.Add(hud.GetComponent<BattleUIHUD>());
			base.ui.hudObjectInstanced[i].InitNum();
			base.ui.hudColliderInstanced.Add(hud.GetComponent<Collider>());
			base.ui.hudObjectDepthController.Add(hud.gameObject.GetComponent<DepthController>());
			if (i >= base.battleStateData.playerCharacters.Length)
			{
				int onHoldWaitPressValue = i - base.battleStateData.playerCharacters.Length;
				base.ui.hudObjectInstanced[i].AddEvent(new Action<int>(base.input.OnPressHud), new Action(base.input.OffPressHud), onHoldWaitPressValue);
			}
		}
		foreach (BattleWave batteWave in base.hierarchyData.batteWaves)
		{
			if (batteWave.cameraType == 1)
			{
				Transform hud2 = base.Instantiate<GameObject>(base.ui.bigBossHudObject).transform;
				hud2.name = "BIG_BOSS_HUD";
				hud2.SetParent(hudParent.transform);
				hud2.localPosition = new Vector3(-20f, 230f, 0f);
				hud2.localScale = Vector3.one;
				hud2.gameObject.SetActive(false);
				base.ui.bigBossHudObjectInstanced = hud2.GetComponent<BattleUIHUD>();
				base.ui.bigBossHudObjectInstanced.InitNum();
				base.ui.hudColliderInstanced.Add(hud2.GetComponent<Collider>());
				base.ui.hudObjectDepthController.Add(hud2.gameObject.GetComponent<DepthController>());
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
			Transform manual = base.Instantiate<GameObject>(base.ui.manualSelectTargetObject).transform;
			manual.SetParent(manualSelectTargetParent);
			manual.position = Vector3.zero;
			manual.localScale = base.uiProperty.manualSelectTargetObjectLocalScale;
			base.ui.manualSelectTargetObjectInstanced.Add(manual.GetComponent<ManualSelectTarget>());
			manual = base.Instantiate<GameObject>(base.ui.toleranceIconObject).transform;
			manual.SetParent(manualSelectTargetParent);
			manual.position = Vector3.zero;
			manual.localScale = base.uiProperty.manualSelectTargetObjectLocalScale;
			base.ui.toleranceIconObjectInstanced.Add(manual.GetComponent<ToleranceIcon>());
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
		for (int i = 0; i < base.ui.monsterButton.Length; i++)
		{
			BattleInputUtility.AddEvent(base.ui.monsterButton[i].playerMonsterDescriptionSwitch.onDisengagePress, new Action<int>(base.input.OnClickMonsterButton), i);
			BattleInputUtility.AddEvent(base.ui.monsterButton[i].playerMonsterDescriptionSwitch.onHoldWaitPress, new Action<int>(base.input.OnPressMonsterButton), i);
			BattleInputUtility.AddEvent(base.ui.monsterButton[i].playerMonsterDescriptionSwitch.onDisengagePress, new Action(base.input.OffPressMonsterButton));
		}
		for (int j = 0; j < base.ui.skillButton.Length; j++)
		{
			if (j > 0)
			{
				BattleInputUtility.AddEvent(base.ui.skillButton[j].skillDescriptionSwitch.onClick, new Action<int>(base.input.OnClickSkillButton), j);
			}
			else
			{
				BattleInputUtility.AddEvent(base.ui.skillButton[j].button.onClick, new Action<int>(base.input.OnClickSkillButton), j);
			}
			UITweenEventSystem tevsystem = base.ui.skillButton[j].rotationEffect1.gameObject.GetComponent<UITweenEventSystem>();
			BattleInputUtility.AddEvent(tevsystem.onFinished, new Action<int>(base.input.OnSkillButtonRotateAfter), j);
			tevsystem = base.ui.skillButton[j].rotationEffect2.gameObject.GetComponent<UITweenEventSystem>();
			BattleInputUtility.AddEvent(tevsystem.onFinished, new Action<int>(base.input.OnSkillButtonRotateAfter), j);
			if (j != 0)
			{
				BattleInputUtility.AddEvent(base.ui.skillButton[j].skillDescriptionSwitch.onHoldWaitPress, new Action<int>(base.input.OnPressSkillButton), j);
				BattleInputUtility.AddEvent(base.ui.skillButton[j].skillDescriptionSwitch.onDisengagePress, new Action<int>(base.input.OffPressSkillButton), j);
			}
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
		yield break;
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
		if (base.ui.rootPanel != null)
		{
			base.Destroy(base.ui.rootPanel.gameObject);
		}
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
			base.ui.skillSelectUi.SetColliderActive(true);
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

	public void ApplySkillButtonData(int index, SkillStatus skills, bool onEnable, bool onSkillLock, CharacterStateControl character)
	{
		if (index < 0)
		{
			return;
		}
		int num = index + 1;
		if (base.ui.skillButton != null && base.ui.skillButton.Length > num)
		{
			base.ui.skillButton[num].ApplySkillButtonData(skills, onEnable, onSkillLock, character);
		}
	}

	public void ApplySkillButtonReflesh()
	{
		foreach (BattleSkillBtn battleSkillBtn in base.ui.skillButton)
		{
			battleSkillBtn.rotationEffect2.enabled = false;
			battleSkillBtn.rotationEffect2.ResetToBeginning();
			battleSkillBtn.rotationEffect2.tweenFactor = 0f;
			battleSkillBtn.rotationEffect1.enabled = false;
			battleSkillBtn.rotationEffect1.ResetToBeginning();
			battleSkillBtn.rotationEffect1.tweenFactor = 0f;
			battleSkillBtn.rotationEffect1.transform.localScale = Vector3.one;
			battleSkillBtn.rotationEffect2.transform.localScale = Vector3.one;
			battleSkillBtn.skillButtonMode.SetSkins(0);
		}
	}

	public void ApplySkillButtonRotation(int oldIndex = -1, int newIndex = -1)
	{
		if (oldIndex > -1)
		{
			base.ui.skillButton[oldIndex].SetColliderActive(false);
			base.ui.skillButton[oldIndex].rotationEffect2.enabled = true;
			base.ui.skillButton[oldIndex].rotationEffect2.PlayForward();
		}
		if (newIndex > -1)
		{
			base.ui.skillButton[newIndex].SetColliderActive(false);
			base.ui.skillButton[newIndex].rotationEffect1.enabled = true;
			base.ui.skillButton[newIndex].rotationEffect1.PlayForward();
		}
		for (int i = 0; i < base.ui.skillButton.Length; i++)
		{
			if (i != oldIndex && i != newIndex)
			{
				base.ui.skillButton[i].SetColliderActive(true);
			}
		}
	}

	public void ApplyAllMonsterButtonEnable(bool value)
	{
		foreach (BattleMonsterButton battleMonsterButton in base.ui.monsterButton)
		{
			battleMonsterButton.gameObject.SetActive(value);
		}
	}

	public void ApplyMonsterButtonEnable(int index, bool isSelect, bool isDead)
	{
		if (isDead)
		{
			base.ui.monsterButton[index].buttonMode.SetSkins(2);
			base.ui.monsterButton[index].currentSelection.SetScreenPosition(0);
			return;
		}
		if (isSelect)
		{
			base.ui.monsterButton[index].buttonMode.SetSkins(1);
			base.ui.monsterButton[index].currentSelection.SetScreenPosition(1);
		}
		else
		{
			base.ui.monsterButton[index].buttonMode.SetSkins(0);
			base.ui.monsterButton[index].currentSelection.SetScreenPosition(0);
		}
	}

	public void ApplyMonsterButtonIcon(int index, Sprite image, CharacterStateControl characterStatus, bool isLeader)
	{
		base.ui.monsterButton[index].ApplyMonsterButtonIcon(image, characterStatus, isLeader);
	}

	public void ApplyLeaderIcon(int index, bool isLeader)
	{
		base.ui.monsterButton[index].ApplyLeaderIcon(isLeader);
	}

	public void ApplySkillName(bool isShow, string skillName = "", CharacterStateControl characterStateControl = null)
	{
		base.ui.turnAction.ApplySkillName(isShow, skillName, characterStateControl);
	}

	public void ApplySkillDescriptionEnable(int index, bool onEnable)
	{
		if (onEnable)
		{
			base.ui.skillButton[index + 1].skillDescriptionEnabled.SetSkins(0);
		}
		else
		{
			base.ui.skillButton[index + 1].skillDescriptionEnabled.SetSkins(1);
		}
	}

	public void ApplyMonsterDescription(bool isShow, CharacterStateControl characterStatus = null, int currentSelectCharacter = 0)
	{
		if (characterStatus != null && !characterStatus.isInitSpecialCorrectionStatus)
		{
			return;
		}
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

	public void ApplyCurrentSelectArrow(bool isEnable, Vector3 position)
	{
		bool flag = position.z > 0f && position.x > 0f && position.x < 1f && position.y > 0f && position.y < 1f && isEnable;
		NGUITools.SetActiveSelf(this.currentSelectMonster, flag);
		if (!flag)
		{
			return;
		}
		if (base.ui.arrowDepthController == null)
		{
			base.ui.arrowDepthController = this.currentSelectMonster.GetComponent<DepthController>();
		}
		Vector3 position2 = base.ui.uiCamera.ViewportToWorldPoint(position);
		float z = position2.z;
		position2.Set(position2.x, position2.y, this.currentSelectMonster.transform.position.z);
		this.currentSelectMonster.transform.position = position2;
		BattleUIControlBasic.ApplyDepthController(base.ui.arrowDepthController, z);
	}

	public HitIcon ApplyShowHitIcon(int index, Vector3 position, AffectEffect affect, int onDamage, Strength onWeak, bool onMiss, bool onCrithical, bool isDrain, bool isRecoil, ExtraEffectType extraEffectType = ExtraEffectType.Non)
	{
		HitIcon unActiveHitIcon = this.GetUnActiveHitIcon(index, position);
		unActiveHitIcon.ApplyShowHitIcon(affect, onDamage, onWeak, onMiss, onCrithical, isDrain, isRecoil, extraEffectType);
		return unActiveHitIcon;
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

	public IEnumerator ApplyDroppedItemMove(Vector3[] startPosition, bool[] isRare)
	{
		bool onNormalFlash = false;
		bool onRareFlash = false;
		for (int i = 0; i < startPosition.Length; i++)
		{
			base.ui.droppingItemObjectInstanced[i].SetActive(true);
			base.ui.droppingItemObjectInstanced[i].SetRare(isRare[i]);
			base.ui.droppingItemObjectInstanced[i].ResetToBeginning();
			Vector3 pos = base.ui.uiCamera.ViewportToWorldPoint(startPosition[i]);
			pos.Set(pos.x, pos.y, 0f);
			base.ui.droppingItemObjectInstanced[i].transform.position = pos;
			Hashtable movedHash = new Hashtable();
			movedHash.Add("position", base.ui.itemInfoField.GetBoxImagePosition(isRare[i]));
			movedHash.Add("time", TimeExtension.GetTimeScaleDivided(base.uiProperty.droppingItemMoveDuration));
			movedHash.Add("easetype", base.uiProperty.droppingItemMoveEaseType);
			iTween.MoveTo(base.ui.droppingItemObjectInstanced[i].gameObject, movedHash);
			if (isRare[i])
			{
				onRareFlash = true;
			}
			else
			{
				onNormalFlash = true;
			}
		}
		IEnumerator wait = base.stateManager.time.WaitForCertainPeriodTimeAction(TimeExtension.GetTimeScaleDivided(base.uiProperty.droppingItemMoveDuration), null, null);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		for (int j = 0; j < startPosition.Length; j++)
		{
			base.ui.droppingItemObjectInstanced[j].PlayForward();
		}
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

	public void ApplyManualSelectTarget(int index, bool isShow, Strength iconType = Strength.None)
	{
		ManualSelectTarget manualSelectTarget = base.ui.manualSelectTargetObjectInstanced[index];
		manualSelectTarget.SetActive(isShow);
		if (!isShow)
		{
			return;
		}
		manualSelectTarget.SetToleranceTarget(iconType);
	}

	public void ApplyTargetToleranceIcon(int index, bool isShow, Strength iconType = Strength.None)
	{
		ToleranceIcon toleranceIcon = base.ui.toleranceIconObjectInstanced[index];
		toleranceIcon.SetActive(isShow);
		if (!isShow)
		{
			return;
		}
		toleranceIcon.SetToleranceIcon(iconType);
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
		if (!manualSelectTarget.gameObject.activeInHierarchy)
		{
			return;
		}
		Vector3 vector = base.ui.uiCamera.ViewportToWorldPoint(position);
		manualSelectTarget.transform.position = new Vector3(vector.x, vector.y, manualSelectTarget.transform.parent.transform.position.z);
	}

	private void ApplyTargetToleranceIconReposition(int index, Vector3 position)
	{
		ToleranceIcon toleranceIcon = base.ui.toleranceIconObjectInstanced[index];
		if (!toleranceIcon.gameObject.activeInHierarchy)
		{
			return;
		}
		Vector3 vector = base.ui.uiCamera.ViewportToWorldPoint(position);
		toleranceIcon.transform.position = new Vector3(vector.x, vector.y, toleranceIcon.transform.parent.transform.position.z);
	}

	private void ApplyCharacterHudPosition(int index, bool isShow, Vector3 position)
	{
		bool flag = position.z > 0f && position.x > 0f && position.x < 1f && position.y > 0f && position.y < 1f && isShow;
		NGUITools.SetActiveSelf(base.ui.hudObjectInstanced[index].gameObject, flag);
		if (!flag)
		{
			return;
		}
		Vector3 position2 = base.ui.uiCamera.ViewportToWorldPoint(position);
		float z = position2.z;
		position2.Set(position2.x, position2.y, base.ui.hudObjectInstanced[index].transform.parent.transform.position.z);
		base.ui.hudObjectInstanced[index].transform.position = position2;
		BattleUIControlBasic.ApplyDepthController(base.ui.hudObjectDepthController[index], z);
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

	public bool GetIsClickedUI()
	{
		foreach (UITouchChecker uitouchChecker in base.ui.skillSelectUi.touchChecker)
		{
			if (uitouchChecker.isClicked)
			{
				return true;
			}
		}
		return false;
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
			int num2 = base.hierarchyData.speedClearRound - base.battleStateData.totalRoundNumber;
			num++;
			num2++;
			base.ui.roundLimitStart.ApplyWaveAndRound(round, num);
			base.ui.roundChallengeStart.ApplyWaveAndRoundSpeed(num2);
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

	public void ApplyAutoPlay(bool isEnable)
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

	public void ShowBattleStageEffect()
	{
		if (base.ui.battleStageEffectObject != null)
		{
			base.ui.battleStageEffectObject.SetActive(true);
		}
	}

	public void HideBattleStageEffect()
	{
		if (base.ui.battleStageEffectObject != null)
		{
			base.ui.battleStageEffectObject.SetActive(false);
		}
	}

	public bool IsBattleStageEffect()
	{
		bool result = false;
		if (base.ui.battleStageEffectObject != null)
		{
			Animation componentInChildren = base.ui.battleStageEffectObject.GetComponentInChildren<Animation>();
			if (componentInChildren != null)
			{
				result = componentInChildren.isPlaying;
			}
		}
		return result;
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
			this.ApplyManualSelectTarget(index, isShow, Strength.None);
			return;
		}
		Strength skillStrength = skill.GetSkillStrength(character.tolerance);
		this.ApplyManualSelectTarget(index, isShow, skillStrength);
	}

	public void SetManualSelectTargetReposition(int index, CharacterStateControl character = null)
	{
		this.ApplyManualSelectTargetReposition(index, this.GetCharacterCenterPosition2DFunction(character));
	}

	public void SetTargetToleranceIcon(int index, bool isShow = false, CharacterStateControl character = null, SkillStatus skill = null)
	{
		if (!isShow)
		{
			this.ApplyTargetToleranceIcon(index, isShow, Strength.None);
			return;
		}
		Strength skillStrength = skill.GetSkillStrength(character.tolerance);
		this.ApplyTargetToleranceIcon(index, isShow, skillStrength);
	}

	public void SetTargetToleranceIconReposition(int index, CharacterStateControl character = null)
	{
		this.ApplyTargetToleranceIconReposition(index, this.GetCharacterCenterPosition2DFunction(character));
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
		if (base.isSkipAction)
		{
			return Vector3.zero;
		}
		return base.hierarchyData.cameraObject.camera3D.WorldToViewportPoint(character.CharacterParams.characterCenterTarget.position);
	}

	public Vector3 GetFixableCharacterCenterPosition2DFunction(CharacterStateControl character)
	{
		if (base.isSkipAction)
		{
			return Vector3.zero;
		}
		return base.hierarchyData.cameraObject.camera3D.WorldToViewportPoint(character.CharacterParams.GetFixableCenterPosition());
	}

	public Vector3 GetHUDCenterPosition2DFunction(CharacterStateControl character)
	{
		if (base.isSkipAction)
		{
			return Vector3.zero;
		}
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
			this.ApplyCharacterHudPosition(i, false, default(Vector3));
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
		foreach (CharacterStateControl characterStateControl in list)
		{
			int index = (!characterStateControl.isEnemy) ? characterStateControl.myIndex : (characterStateControl.myIndex + base.battleStateData.playerCharacters.Length);
			if (characterStateControl.isDied && characterStateControl.isDiedJustBefore)
			{
				this.ApplyCharacterHudPosition(index, false, default(Vector3));
			}
			else
			{
				this.ApplyCharacterHudPosition(index, true, this.GetHUDCenterPosition2DFunction(characterStateControl));
			}
		}
		if (base.ui.bigBossHudObjectInstanced != null && base.hierarchyData.batteWaves[base.battleStateData.currentWaveNumber].cameraType == 1)
		{
			foreach (CharacterStateControl characterStateControl2 in list)
			{
				int index2 = (!characterStateControl2.isEnemy) ? characterStateControl2.myIndex : (characterStateControl2.myIndex + base.battleStateData.playerCharacters.Length);
				if (characterStateControl2.isEnemy)
				{
					this.ApplyCharacterHudPosition(index2, false, default(Vector3));
					if (characterStateControl2.isDied && characterStateControl2.isDiedJustBefore)
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
			this.ApplyCharacterHudPosition(totalCharacterIndex, !characterStateControl.isDied, default(Vector3));
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
					this.ApplyCharacterHudPosition(index, false, default(Vector3));
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

	public HitIcon GetUnActiveHitIcon(int index, Vector3 position)
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
		hitIcon.transform.localScale = base.uiProperty.hitIconLocalScale;
		NGUITools.SetActiveSelf(hitIcon.gameObject, true);
		Vector3 vector = base.ui.uiCamera.ViewportToWorldPoint(position);
		hitIcon.transform.position = new Vector3(vector.x, vector.y, 0f);
		DepthController component = hitIcon.GetComponent<DepthController>();
		component.SetWidgetDepth(index);
		NGUITools.SetActiveSelf(hitIcon.gameObject, true);
		return hitIcon;
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

	private static void ApplyDepthController(DepthController d, float z)
	{
		int widgetDepth = int.MaxValue - Mathf.RoundToInt(z * 1E+07f);
		d.SetWidgetDepth(widgetDepth);
	}

	private static void SetActiveSelfs(bool state, params Component[] gos)
	{
		foreach (Component component in gos)
		{
			NGUITools.SetActiveSelf(component.gameObject, state);
		}
	}

	public static int GetEvolutionStepSetSkinner(EvolutionStep evolutionStep)
	{
		if (GrowStep.GROWING.ConverBattleInt() == (int)evolutionStep)
		{
			return 0;
		}
		if (GrowStep.RIPE.ConverBattleInt() == (int)evolutionStep || GrowStep.ARMOR_1.ConverBattleInt() == (int)evolutionStep)
		{
			return 1;
		}
		if (GrowStep.PERFECT.ConverBattleInt() == (int)evolutionStep)
		{
			return 2;
		}
		if (GrowStep.ULTIMATE.ConverBattleInt() == (int)evolutionStep || GrowStep.ARMOR_2.ConverBattleInt() == (int)evolutionStep)
		{
			return 3;
		}
		return 0;
	}

	public static int GetEvolutionStepSetTextReplacer(EvolutionStep evolutionStep)
	{
		if (evolutionStep != EvolutionStep.AmorPhase2)
		{
			return (int)evolutionStep;
		}
		return 6;
	}

	public static int GetSpeciesSetTextReplacer(Species species)
	{
		return (int)species;
	}
}
