using BattleStateMachineInternal;
using Master;
using System;
using System.Collections;
using UnityEngine;

public class BattleUIControlMulti : BattleUIControlMultiBasic
{
	private MultiBattleDialog _battleDialog;

	private BattleUIMultiX2PlayButton multiX2PlayButton;

	protected BattleInputMulti inputMulti
	{
		get
		{
			return base.stateManager.inputMulti;
		}
	}

	private new BattleUIComponentsMulti ui
	{
		get
		{
			return base.stateManager.battleUiComponents as BattleUIComponentsMulti;
		}
	}

	private MultiBattleDialog battleDialog
	{
		get
		{
			if (this._battleDialog == null)
			{
				this._battleDialog = (this.ui.battleAlwaysUi as MultiBattleAlways).multiBattleDialog;
			}
			return this._battleDialog;
		}
	}

	public override void ApplySetBattleStateRegistration()
	{
		this.battleScreenDetails.Add(BattleScreen.Initialize, new BattleScreenDetail(delegate()
		{
			BattleScreenDetail.DeactiveObjects(new UIWidget[]
			{
				this.ui.initializeUi.widget
			});
		}, delegate()
		{
			BattleScreenDetail.ActiveObjects(new UIWidget[]
			{
				this.ui.initializeUi.widget
			});
			BattleScreenDetail.DeactiveObjects(new UIWidget[]
			{
				this.ui.skillSelectUi.widget
			});
		}, false));
		this.battleScreenDetails.Add(BattleScreen.InsertEnemy, new BattleScreenDetail(delegate()
		{
		}, delegate()
		{
			BattleScreenDetail.DeactiveObjects(new UIWidget[]
			{
				this.ui.skillSelectUi.widget
			});
		}, false));
		this.battleScreenDetails.Add(BattleScreen.ExtraStartAction, new BattleScreenDetail(delegate()
		{
			BattleScreenDetail.DeactiveObjects(new UIWidget[]
			{
				this.ui.extraStartUi
			});
		}, delegate()
		{
			BattleScreenDetail.ActiveObjects(new UIWidget[]
			{
				this.ui.extraStartUi
			});
			BattleScreenDetail.DeactiveObjects(new UIWidget[]
			{
				this.ui.skillSelectUi.widget
			});
		}, false));
		this.battleScreenDetails.Add(BattleScreen.BossStartAction, new BattleScreenDetail(delegate()
		{
			BattleScreenDetail.DeactiveObjects(new UIWidget[]
			{
				this.ui.bossStartUi
			});
		}, delegate()
		{
			BattleScreenDetail.ActiveObjects(new UIWidget[]
			{
				this.ui.bossStartUi
			});
			BattleScreenDetail.DeactiveObjects(new UIWidget[]
			{
				this.ui.skillSelectUi.widget
			});
		}, false));
		this.battleScreenDetails.Add(BattleScreen.StartAction, new BattleScreenDetail(delegate()
		{
		}, delegate()
		{
			BattleScreenDetail.DeactiveObjects(new UIWidget[]
			{
				this.ui.skillSelectUi.widget
			});
		}, false));
		this.battleScreenDetails.Add(BattleScreen.SkillSelects, new BattleScreenDetail(delegate()
		{
		}, delegate()
		{
			if (!this.ui.skillSelectUi.gameObject.activeInHierarchy)
			{
				BattleScreenDetail.ActiveObjects(new UIWidget[]
				{
					this.ui.skillSelectUi.widget
				});
			}
		}, true));
		this.battleScreenDetails.Add(BattleScreen.RoundStart, new BattleScreenDetail(delegate()
		{
		}, delegate()
		{
			if (!this.ui.skillSelectUi.gameObject.activeInHierarchy)
			{
				BattleScreenDetail.ActiveObjects(new UIWidget[]
				{
					this.ui.skillSelectUi.widget
				});
			}
		}, true));
		this.battleScreenDetails.Add(BattleScreen.RoundStartActionsLimit, new BattleScreenDetail(this.ui.roundLimitStart.widget, true));
		this.battleScreenDetails.Add(BattleScreen.RoundStartActionsChallenge, new BattleScreenDetail(this.ui.roundChallengeStart.widget, true));
		this.battleScreenDetails.Add(BattleScreen.TimeOver, new BattleScreenDetail(this.ui.timeOverUi, false));
		this.battleScreenDetails.Add(BattleScreen.RoundActions, new BattleScreenDetail(delegate()
		{
			BattleScreenDetail.DeactiveObjects(new UIWidget[]
			{
				this.ui.turnAction.widget
			});
		}, delegate()
		{
			BattleScreenDetail.ActiveObjects(new UIWidget[]
			{
				this.ui.turnAction.widget
			});
		}, true));
		this.battleScreenDetails.Add(BattleScreen.EnemyTurnAction, new BattleScreenDetail(delegate()
		{
			BattleScreenDetail.DeactiveObjects(new UIWidget[]
			{
				this.ui.enemyTurnUi
			});
		}, delegate()
		{
			BattleScreenDetail.ActiveObjects(new UIWidget[]
			{
				this.ui.enemyTurnUi
			});
			if (!this.ui.skillSelectUi.gameObject.activeInHierarchy)
			{
				BattleScreenDetail.ActiveObjects(new UIWidget[]
				{
					this.ui.skillSelectUi.widget
				});
			}
		}, true));
		this.battleScreenDetails.Add(BattleScreen.IsWarning, new BattleScreenDetail(delegate()
		{
			BattleScreenDetail.DeactiveObjects(new UIWidget[]
			{
				this.ui.isWarning.widget
			});
		}, delegate()
		{
			BattleScreenDetail.ActiveObjects(new UIWidget[]
			{
				this.ui.isWarning.widget
			});
			if (!this.ui.skillSelectUi.gameObject.activeInHierarchy)
			{
				BattleScreenDetail.ActiveObjects(new UIWidget[]
				{
					this.ui.skillSelectUi.widget
				});
			}
		}, true));
		this.battleScreenDetails.Add(BattleScreen.PoisonHit, new BattleScreenDetail(delegate()
		{
		}, delegate()
		{
			if (!this.ui.skillSelectUi.gameObject.activeInHierarchy)
			{
				BattleScreenDetail.ActiveObjects(new UIWidget[]
				{
					this.ui.skillSelectUi.widget
				});
			}
		}, true));
		this.battleScreenDetails.Add(BattleScreen.RevivalCharacter, new BattleScreenDetail(delegate()
		{
		}, delegate()
		{
			BattleScreenDetail.DeactiveObjects(new UIWidget[]
			{
				this.ui.skillSelectUi.widget
			});
		}, true));
		this.battleScreenDetails.Add(BattleScreen.RoundStartActions, new BattleScreenDetail(delegate()
		{
			BattleScreenDetail.DeactiveObjects(new UIWidget[]
			{
				this.ui.roundStart.widget
			});
		}, delegate()
		{
			BattleScreenDetail.ActiveObjects(new UIWidget[]
			{
				this.ui.roundStart.widget
			});
			if (!this.ui.skillSelectUi.gameObject.activeInHierarchy)
			{
				BattleScreenDetail.ActiveObjects(new UIWidget[]
				{
					this.ui.skillSelectUi.widget
				});
			}
		}, true));
		this.battleScreenDetails.Add(BattleScreen.NextBattle, new BattleScreenDetail(delegate()
		{
			BattleScreenDetail.DeactiveObjects(new UIWidget[]
			{
				this.ui.nextWaveUi
			});
		}, delegate()
		{
			BattleScreenDetail.ActiveObjects(new UIWidget[]
			{
				this.ui.nextWaveUi
			});
			if (!this.ui.skillSelectUi.gameObject.activeInHierarchy)
			{
				BattleScreenDetail.ActiveObjects(new UIWidget[]
				{
					this.ui.skillSelectUi.widget
				});
			}
		}, true));
		this.battleScreenDetails.Add(BattleScreen.PlayerWinner, new BattleScreenDetail(delegate()
		{
			BattleScreenDetail.DeactiveObjects(new UIWidget[]
			{
				this.ui.nextWaveUi,
				this.ui.initialInduction.widget
			});
		}, delegate()
		{
			BattleScreenDetail.ActiveObjects(new UIWidget[]
			{
				this.ui.playerWinnerUi.widget,
				this.ui.initialInduction.widget
			});
		}, false));
		this.battleScreenDetails.Add(BattleScreen.PlayerFailed, new BattleScreenDetail(delegate()
		{
			BattleScreenDetail.DeactiveObjects(new UIWidget[]
			{
				this.ui.playerFailUi,
				this.ui.initialInduction.widget
			});
		}, delegate()
		{
			BattleScreenDetail.ActiveObjects(new UIWidget[]
			{
				this.ui.playerFailUi,
				this.ui.initialInduction.widget
			});
			BattleScreenDetail.DeactiveObjects(new UIWidget[]
			{
				this.ui.skillSelectUi.widget
			});
		}, false));
		this.battleScreenDetails.Add(BattleScreen.Continue, new BattleScreenDetail(delegate()
		{
			BattleScreenDetail.DeactiveObjects(new UIWidget[]
			{
				this.ui.continueUi
			});
			if (base.stateManager.multiFunction.IsOwner)
			{
				BattleScreenDetail.DeactiveObjects(new UIWidget[]
				{
					this.ui.continueUi
				});
			}
			else
			{
				this.HideAlertDialog();
			}
		}, delegate()
		{
			BattleScreenDetail.DeactiveObjects(new UIWidget[]
			{
				this.ui.skillSelectUi.widget
			});
			if (base.stateManager.multiFunction.IsOwner)
			{
				this.HideAllDIalog();
				if (!this.battleDialog.IsBlockNewDialog)
				{
					BattleScreenDetail.ActiveObjects(new UIWidget[]
					{
						this.ui.continueUi
					});
					base.ShowContinueDialog();
				}
			}
			else
			{
				this.ShowAlertDialog(StringMaster.GetString("BattleUI-33"), string.Empty, null, false, -1);
			}
		}, true));
		this.ui.fadeoutUi.FadeIn(Color.black, 0f);
	}

	public override IEnumerator AfterInitializeUI()
	{
		base.stateManager.uiControl.AfterInitializeUIBefore();
		this.SetupAutoPlay();
		this.SetupX2Play();
		this.SetupMonster();
		this.SetupSkill();
		this.SetupMenu();
		this.SetupDialog();
		this.SetupAttackTime();
		this.SetupEmotion();
		this.SetupSharedAP();
		this.SetupRemaining();
		this.SetWinner();
		base.stateManager.uiControlMulti.ShowSharedAP();
		IEnumerator helpInitialize = base.stateManager.help.HelpInitialize();
		while (helpInitialize.MoveNext())
		{
			yield return null;
		}
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
		base.DroppingItemInitialize();
		base.InstantiateGimmickStatusEffect();
		this.ui.rootPanel.RebuildAllDrawCalls();
		yield break;
	}

	private void SetupMenu()
	{
		(this.ui.battleAlwaysUi as MultiBattleAlways).SetupMenuButton(base.stateManager.multiBasicFunction.IsOwner);
		BattleInputUtility.AddEvent(this.ui.menuButton.onClick, new Action(this.inputMulti.OnMultiShowMenu));
		this.ui.menuDialog.AddRetireButtonEvent(new Action(base.input.OnClickMenuRetireButton));
		this.ui.menuDialog.AddCloseButtonEvent(new Action(base.input.OnClickMenuCloseButton));
	}

	private void SetupAutoPlay()
	{
		(this.ui.battleAlwaysUi as MultiBattleAlways).HideAutoPlay();
	}

	private void SetupMonster()
	{
		for (int i = 0; i < this.ui.skillSelectUi.monsterButton.Length; i++)
		{
			BattleInputUtility.AddEvent(this.ui.skillSelectUi.monsterButton[i].playerMonsterDescriptionSwitch.onDisengagePress, new Action<int>(this.inputMulti.OnClickMultiMonsterButton), i);
			BattleInputUtility.AddEvent(this.ui.skillSelectUi.monsterButton[i].playerMonsterDescriptionSwitch.onHoldWaitPress, new Action<int>(base.input.OnPressMonsterButton), i);
			BattleInputUtility.AddEvent(this.ui.skillSelectUi.monsterButton[i].playerMonsterDescriptionSwitch.onDisengagePress, new Action(base.input.OffPressMonsterButton));
		}
	}

	private void SetupSkill()
	{
		for (int i = 0; i < this.ui.skillSelectUi.skillButton.Length; i++)
		{
			if (i > 0)
			{
				this.ui.skillSelectUi.skillButton[i].SetClickCallback(new Action<int>(base.input.OnClickSkillButton), i);
				this.ui.skillSelectUi.skillButton[i].SetHoldWaitPressCallback(new Action<int>(base.input.OnPressSkillButton), i);
				this.ui.skillSelectUi.skillButton[i].SetDisengagePressCallback(new Action<int>(base.input.OffPressSkillButton), i);
			}
			else
			{
				this.ui.skillSelectUi.skillButton[i].SetButtonCallback(new Action<int>(base.input.OnClickSkillButton), i);
			}
			this.ui.skillSelectUi.skillButton[i].SetRotationEffectCallback(new Action<int>(base.input.OnSkillButtonRotateAfter), i);
		}
	}

	private void SetupX2Play()
	{
		this.multiX2PlayButton = (BattleUIMultiX2PlayButton)this.ui.x2PlayButton;
		this.HideX2PlayButton();
		this.HideX2PlayErrorDialog();
		this.multiX2PlayButton.AddEvent(new Action(this.inputMulti.OnClickMultiX2PlayButton));
	}

	private void SetupAttackTime()
	{
		this.ui.skillSelectUi.attackTime.callBackAction = new Action(base.stateManager.multiFunction.RunAttackAutomatically);
	}

	private void SetupEmotion()
	{
		this.ui.emotionSenderMulti = this.ui.skillSelectUi.emotionSenderMulti;
		(this.ui.battleAlwaysUi as MultiBattleAlways).SetupEmotion(this.ui.emotionSenderMulti, new Action<int>(base.stateManager.multiFunction.SendEmotion));
		this.HideEmotionButton();
		this.HideEmotion();
	}

	private void SetupSharedAP()
	{
		int sharedAp = 9;
		SharedStatus sharedStatus = new SharedStatus(sharedAp);
		sharedStatus.SetStatic();
		CharacterStateControl[] playerCharacters = base.battleStateData.playerCharacters;
		SharedStatus.SetAllStaticStatus(playerCharacters);
		this.ui.sharedAP = (this.ui.battleAlwaysUi as MultiBattleAlways).sharedAP;
	}

	private void SetupRemaining()
	{
		this.ui.remainingTurnRightDown = (this.ui.battleAlwaysUi as MultiBattleAlways).remainingTurn;
		this.ui.remainingTurnRightDown.gameObject.SetActive(true);
		this.HideRemainingTurnRightDown();
		this.ui.remainingTurnMiddle = this.ui.skillSelectUi.remainingTurnMiddle;
		this.ui.remainingTurnMiddle.gameObject.SetActive(true);
		this.HideRemainingTurnMiddle();
	}

	private void SetWinner()
	{
		this.ui.initialInduction.AddEvent(new Action(base.input.OnClickCloseInitialInductionButton));
		this.ui.playerWinnerUi.AddEvent(new Action(base.input.OnClickSkipWinnerAction));
	}

	private void SetupDialog()
	{
		this.battleDialog.gameObject.SetActive(true);
		this.HideAlertDialog();
		this.ui.characterRevivalDialog.AddSpecificTradeButtonEvent(new Action(base.input.OnClickSpecificTradeButton));
		this.ui.characterRevivalDialog.AddRevivalButtonEvent(new Action(this.inputMulti.OnClickMultiCharacterRevivalButton));
		this.ui.characterRevivalDialog.AddCloseButtonEvent(new Action(base.input.OnClickCharacterRevivalCloseButton));
		this.ui.dialogRetire.AddEvent(new Action(this.inputMulti.OnClickMultiRetireDialogOkButton), new Action(base.input.OnClickRetireDialogCancelButton));
		if (base.stateManager.multiFunction.IsOwner)
		{
			this.ui.continueTimer = this.ui.dialogContinue.GetComponent<UITimer>();
			this.ui.continueTimer.Set(10, delegate
			{
				base.stateManager.multiFunction.SendRetire(null);
			});
		}
		this.ui.dialogContinue.AddEvent(new Action(base.input.OnClickSpecificTradeButton), new Action(base.input.OnClickContinueDialogRevivalButton), new Action(base.input.OnClickContinueDialogRetireButton), base.stateManager.multiFunction.IsOwner);
	}

	public void ApplyHUDRecoverMulti(int index, bool isRevivalAp = false, bool isRevivalHp = false, int apNum = 0)
	{
		UIComponentSkinner apUpRootComponentSkinner = base.stateManager.battleUiComponents.hudObjectInstanced[index].apUpRootComponentSkinner;
		UIComponentSkinner hpUpRootComponentSkinner = base.stateManager.battleUiComponents.hudObjectInstanced[index].hpUpRootComponentSkinner;
		UILabel multiAPNumber = base.stateManager.battleUiComponents.hudObjectInstanced[index].multiAPNumber;
		GameObject hudApObj = base.stateManager.battleUiComponents.hudObjectInstanced[index].apUpObject;
		GameObject hudHpObj = base.stateManager.battleUiComponents.hudObjectInstanced[index].hpUpObject;
		AnimatorFinishEventTrigger component = hudApObj.GetComponent<AnimatorFinishEventTrigger>();
		AnimatorFinishEventTrigger component2 = hudHpObj.GetComponent<AnimatorFinishEventTrigger>();
		component.OnFinishAnimation = delegate(string str)
		{
			NGUITools.SetActiveSelf(hudApObj, false);
		};
		component2.OnFinishAnimation = delegate(string str)
		{
			NGUITools.SetActiveSelf(hudHpObj, false);
		};
		apUpRootComponentSkinner.SetSkins(0);
		hpUpRootComponentSkinner.SetSkins(0);
		if (isRevivalAp)
		{
			apUpRootComponentSkinner.SetSkins(1);
			if (apNum > 0)
			{
				multiAPNumber.text = string.Format("AP+{0}", apNum);
			}
		}
		if (isRevivalHp)
		{
			hpUpRootComponentSkinner.SetSkins(1);
		}
	}

	public void HideHUDRecoverMulti(int index)
	{
		GameObject apUpObject = base.stateManager.battleUiComponents.hudObjectInstanced[index].apUpObject;
		GameObject hpUpObject = base.stateManager.battleUiComponents.hudObjectInstanced[index].hpUpObject;
		NGUITools.SetActiveSelf(apUpObject, false);
		NGUITools.SetActiveSelf(hpUpObject, false);
	}

	public void HideMenu()
	{
		(this.ui.battleAlwaysUi as MultiBattleAlways).HideMenu();
	}

	public void ShowX2PlayButton()
	{
		NGUITools.SetActiveSelf(this.multiX2PlayButton.gameObject, true);
	}

	public void HideX2PlayButton()
	{
		NGUITools.SetActiveSelf(this.multiX2PlayButton.gameObject, false);
	}

	public void ShowX2PlayErrorDialog()
	{
		this.multiX2PlayButton.SetActiveSpeedAlert(true);
	}

	public void HideX2PlayErrorDialog()
	{
		this.multiX2PlayButton.SetActiveSpeedAlert(false);
	}

	public void ShowAlertDialog(string message, string buttonText, Action action = null, bool isWithButton = false, int maxTime = -1)
	{
		if (this.battleDialog.IsBlockNewDialog)
		{
			global::Debug.Log("既にダイアログが存在する");
			return;
		}
		this.battleDialog.SetMessage(message, buttonText);
		this.battleDialog.callBackAction = action;
		this.battleDialog.maxTime = maxTime;
		int skin = (!isWithButton) ? 2 : 1;
		this.battleDialog.SetSkin(skin);
	}

	public void StartFailedTimer(Action action)
	{
		if (this.battleDialog.IsAlreadyOpen())
		{
			return;
		}
		if (this.battleDialog.gameObject.activeInHierarchy)
		{
			string waitingConnectionFormat = "メンバーの通信が切断されたので\n復帰を待ちます\nあと{0}秒";
			this.battleDialog.StartFailedTimer(waitingConnectionFormat, action, false);
		}
	}

	public void BlockNewDialog()
	{
		this.battleDialog.IsBlockNewDialog = true;
	}

	public bool GetNewDialog()
	{
		return this.battleDialog.IsBlockNewDialog;
	}

	public void ShowAlertLeftTimeMessage(int leftTime)
	{
		if (this.battleDialog.IsBlockNewDialog)
		{
			return;
		}
		int num = Mathf.Clamp(leftTime, 0, ConstValue.MULTI_BATTLE_TIMEOUT_TIME);
		string message = string.Format(StringMaster.GetString("BattleUI-22"), num);
		this.battleDialog.SetMessage(message, string.Empty);
		this.battleDialog.SetSkin(2);
	}

	public void HideAlertDialog()
	{
		this.battleDialog.SetSkin(0);
	}

	public void ShowRemainingTurnRightDown()
	{
		this.ui.remainingTurnRightDown.SetEnable(true);
	}

	public void HideRemainingTurnRightDown()
	{
		this.ui.remainingTurnRightDown.SetEnable(false);
	}

	public void SetRemainingTurnRightDownLabel(int num, RemainingTurn.MiddleType middleType)
	{
		this.ui.remainingTurnRightDown.SetLabel(num, middleType);
	}

	public void ShowRemainingTurnMiddle()
	{
		this.ui.remainingTurnMiddle.SetEnable(true);
	}

	public void HideRemainingTurnMiddle()
	{
		this.ui.remainingTurnMiddle.SetEnable(false);
	}

	public void SetRemainingTurnMiddleLabel(int num, RemainingTurn.MiddleType middleType)
	{
		this.ui.remainingTurnMiddle.SetLabel(num, middleType);
	}

	public void ShowEmotionButton()
	{
		(this.ui.battleAlwaysUi as MultiBattleAlways).SetEmotionButton(true);
	}

	public void HideEmotionButton()
	{
		(this.ui.battleAlwaysUi as MultiBattleAlways).SetEmotionButton(false);
	}

	public void ShowEmotion(int index, int emotionType, bool isOther = false)
	{
		this.ui.emotionSenderMulti.SetEmotion(index, emotionType, isOther);
	}

	public void HideEmotion()
	{
		this.ui.emotionSenderMulti.HideAll();
	}

	public void ShowSharedAP()
	{
		this.ui.sharedAP.gameObject.SetActive(true);
	}

	public void HideSharedAP()
	{
		this.ui.sharedAP.gameObject.SetActive(false);
	}

	public void StartSharedAPAnimation()
	{
		CharacterStateControl currentSelectCharacterState = base.battleStateData.currentSelectCharacterState;
		if (currentSelectCharacterState.IsSelectedSkill)
		{
			this.ui.sharedAP.PlaySelectAnimation(currentSelectCharacterState.currentSkillStatus.needAp);
		}
	}

	public void StopSharedAPAnimation()
	{
		this.ui.sharedAP.StopSelectAnimation();
	}

	public void PlayApUpAnimations(Action callback = null)
	{
		int sharedAp = SharedStatus.currentSharedStatus.sharedAp;
		this.ui.sharedAP.Play(sharedAp, callback);
	}

	public void ResetHUD(params CharacterStateControl[] characters)
	{
		foreach (CharacterStateControl characterStateControl in characters)
		{
			characterStateControl.skillOrder = -1;
			BattleUIHUD battleUIHUD = this.ui.hudObjectInstanced[characterStateControl.myIndex];
			NGUITools.SetActiveSelf(battleUIHUD.gameObject, true);
			battleUIHUD.transform.localPosition = Vector3.zero;
		}
	}

	public void ApplySkillSelectUI(bool isActive)
	{
		NGUITools.SetActiveSelf(this.ui.skillSelectUi.skillButtonRoot, isActive);
		NGUITools.SetActiveSelf(this.ui.skillSelectUi.attackTime.gameObject, isActive);
	}

	public void SetPlayerNumToMonsterButton(int iconIndex, int playerIndex, string playerName)
	{
		BattleMonsterButton battleMonsterButton = this.ui.skillSelectUi.monsterButton[iconIndex];
		battleMonsterButton.SetPlayerNumber(playerIndex + 1);
		string text = playerName;
		if (text.Length > 5)
		{
			text = text.Remove(5, playerName.Length - 5);
			text += "...";
		}
		battleMonsterButton.SetPlayerName(text);
		if (playerIndex == 0)
		{
			battleMonsterButton.SetPlayerNameColor(new Color(0.9372549f, 0.03137255f, 0.03137255f));
		}
		else if (playerIndex == 1)
		{
			battleMonsterButton.SetPlayerNameColor(new Color(0.160784319f, 1f, 0f));
		}
		else
		{
			battleMonsterButton.SetPlayerNameColor(new Color(0.905882359f, 0.03137255f, 0.905882359f));
		}
		this.ui.hudObjectInstanced[iconIndex].SetPnNo(playerIndex);
	}

	public void HideAllDIalog()
	{
		if (base.battleStateData.isShowRevivalWindow)
		{
			base.battleStateData.isShowRevivalWindow = false;
			NGUITools.SetActiveSelf(this.ui.characterRevivalDialog.gameObject, false);
		}
		if (base.battleStateData.isShowMenuWindow)
		{
			base.battleStateData.isShowMenuWindow = false;
			NGUITools.SetActiveSelf(this.ui.menuDialog.gameObject, false);
		}
		if (base.battleStateData.isShowContinueWindow)
		{
			base.battleStateData.isShowContinueWindow = false;
			NGUITools.SetActiveSelf(this.ui.dialogContinue.gameObject, false);
		}
		if (base.battleStateData.isShowHelp)
		{
			base.battleStateData.isShowHelp = false;
			base.stateManager.help.ApplyShowHideHelpWindow(false);
		}
		if (base.battleStateData.isShowSpecificTrade)
		{
			base.battleStateData.isShowSpecificTrade = false;
			base.ApplySpecificTrade(false);
		}
		if (base.battleStateData.isShowRetireWindow)
		{
			base.battleStateData.isShowRetireWindow = false;
			NGUITools.SetActiveSelf(this.ui.dialogRetire.gameObject, false);
		}
		if (base.stateManager.onServerConnect)
		{
			GUIManager.CloseAllCommonDialog(null);
		}
		base.stateManager.uiControl.SetTouchEnable(true);
	}

	public void ShowWinnerUI()
	{
		this.ui.battleAlwaysUi.gameObject.SetActive(true);
		(this.ui.battleAlwaysUi as MultiBattleAlways).ShowWinnerUI();
	}
}
