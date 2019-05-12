using Master;
using MultiBattle.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BattlePvPFunction : BattleMultiBasicFunction
{
	private const int failedDialogTerm = 10;

	private const int retireDialogTerm = 10;

	private bool isSyncFinished;

	private bool isSendEvent;

	public static bool isAlreadyLoseBeforeBattle;

	public static int battleActionLogResult = -1;

	public void FinishedSync()
	{
		this.isSyncFinished = true;
	}

	protected override string myTCPKey
	{
		get
		{
			return "pvpBattle";
		}
	}

	protected override int MaxTimeOutValue
	{
		get
		{
			return ConstValue.PVP_BATTLE_TIMEOUT_TIME;
		}
	}

	protected override int MaxRecoverCount
	{
		get
		{
			return 3;
		}
	}

	public IEnumerator BattleStartActionFunction()
	{
		IEnumerator battelStart = Singleton<TCPMessageSender>.Instance.SendPvPBattleStart();
		while (battelStart.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	public IEnumerator BattleEndActionFunction()
	{
		List<int> skill = new List<int>();
		skill.Add(0);
		IEnumerator battelEnd = Singleton<TCPMessageSender>.Instance.SendPvPBattleEnd(ClassSingleton<MultiBattleData>.Instance.BattleResult, base.battleStateData.currentRoundNumber, skill);
		while (battelEnd.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	public void StopSomething()
	{
		this.isDisconnected = true;
		base.stateManager.uiControlPvP.HideAlertDialog();
		base.stateManager.uiControlPvP.HideSyncWait();
		base.stateManager.uiControlPvP.StopAttackTimer();
		base.stateManager.uiControlPvP.HideEmotionButton();
		base.stateManager.uiControlPvP.HideSkillSelectUI();
		base.stateManager.uiControlPvP.HideRetireWindow();
	}

	protected override IEnumerator Reconnect(bool isDialog = true)
	{
		yield return new WaitForEndOfFrame();
		if (base.stateManager.uiControlPvP.IsAlreadyOpen())
		{
			global::Debug.Log("IsAlreadyOpen");
			yield break;
		}
		base.InitializeTCPClient(true);
		if (isDialog)
		{
			base.stateManager.uiControlPvP.StartEnemyFailedTimer(delegate
			{
				base.stateManager.uiControlPvP.HideAlertDialog();
				global::Debug.LogError("自分が落ちた(時間経過)");
				this.ShowDisconnectTCPDialog(null);
			}, BattleUIControlPvP.DialogType.MyCount);
		}
		yield break;
	}

	protected override void ShowDisconnectTCPDialog(Action callback = null)
	{
		if (this.isSendEvent)
		{
			return;
		}
		this.StopSomething();
		base.stateManager.uiControlPvP.ShowAlertDialog(BattleUIControlPvP.DialogType.Lose, StringMaster.GetString("BattleUI-24"), StringMaster.GetString("SystemButtonClose"), delegate
		{
			this.stateManager.uiControlPvP.HideAlertDialog();
			this.stateManager.events.CallConnectionErrorEvent();
			this.isSendEvent = true;
			if (callback != null)
			{
				callback();
			}
		}, true, 10);
		base.stateManager.uiControlPvP.BlockNewDialog();
	}

	private void ShowWinDialog()
	{
		if (this.isSendEvent)
		{
			return;
		}
		this.StopSomething();
		base.stateManager.uiControlPvP.ShowAlertDialog(BattleUIControlPvP.DialogType.EnemyRetire, StringMaster.GetString("BattleUI-21"), StringMaster.GetString("SystemButtonClose"), delegate
		{
			base.stateManager.uiControlPvP.HideAlertDialog();
			base.stateManager.events.CallWinEvent();
			this.isSendEvent = true;
		}, true, 10);
		base.stateManager.uiControlPvP.BlockNewDialog();
	}

	private void ShowRetireDialog()
	{
		if (this.isSendEvent)
		{
			return;
		}
		this.StopSomething();
		base.stateManager.events.CallRetireEvent();
		this.isSendEvent = true;
	}

	private void ShowConnectionErrorDialog()
	{
		if (this.isSendEvent)
		{
			return;
		}
		this.StopSomething();
		base.stateManager.events.CallConnectionErrorEvent();
		this.isSendEvent = true;
	}

	private void ShowBackToTitleDialog()
	{
		if (this.isSendEvent)
		{
			return;
		}
		this.StopSomething();
		string @string = StringMaster.GetString("SaveFailedTitle");
		string string2 = StringMaster.GetString("AlertJsonErrorInfo");
		AlertManager.ButtonActionType actionType = AlertManager.ButtonActionType.Title;
		AlertManager.ShowAlertDialog(delegate(int i)
		{
			GUIMain.BackToTOP("UIStartupCaution", 0.8f, 0.8f);
		}, @string, string2, actionType, false);
		this.isSendEvent = true;
	}

	public IEnumerator CheckAlreadyLoseBeforeBattle()
	{
		if (BattlePvPFunction.isAlreadyLoseBeforeBattle)
		{
			BattlePvPFunction.isAlreadyLoseBeforeBattle = false;
			this.ShowConnectionErrorDialog();
		}
		else
		{
			IEnumerator sendReconnection = Singleton<TCPMessageSender>.Instance.SendPvPRecoverCommunicate();
			while (sendReconnection.MoveNext())
			{
				yield return null;
			}
		}
		yield break;
	}

	public override IEnumerator WaitAllPlayers(TCPMessageType tcpMessageType)
	{
		global::Debug.LogFormat("{0}の通信待ち.", new object[]
		{
			tcpMessageType
		});
		int waitingCount = 0;
		for (;;)
		{
			while (this.isDisconnected)
			{
				yield return null;
			}
			IEnumerator wait = Util.WaitForRealTime(1f);
			while (wait.MoveNext())
			{
				object obj = wait.Current;
				yield return obj;
			}
			waitingCount++;
			global::Debug.LogFormat("[{0}]waiting ....(waitingCount:{1})", new object[]
			{
				tcpMessageType,
				waitingCount
			});
			if (waitingCount == 45)
			{
				waitingCount = 0;
				IEnumerator enemyFailedAction = this.EnemyFailedAction();
				while (enemyFailedAction.MoveNext())
				{
					yield return null;
				}
			}
			if (this.recieveChecks[tcpMessageType])
			{
				break;
			}
			yield return null;
		}
		global::Debug.LogFormat("Finish waiting [{0}].", new object[]
		{
			tcpMessageType
		});
		this.recieveChecks[tcpMessageType] = false;
		yield break;
		yield break;
	}

	public override IEnumerator SendMessageInsistently<T>(TCPMessageType tcpMessageType, TCPData<T> message, float waitingTerm = 1f)
	{
		int waitingCount = 0;
		for (;;)
		{
			while (this.isDisconnected)
			{
				yield return null;
			}
			base.SendMessageForSync(tcpMessageType, message);
			global::Debug.LogFormat("残りの人数:{0}/{1}, tcpMessageType{2}, waitingCount:{3}, sent:{4}", new object[]
			{
				this.confirmationChecks[tcpMessageType].Count,
				base.otherUserCount,
				tcpMessageType,
				waitingCount,
				string.Join(",", this.confirmationChecks[tcpMessageType].ToArray())
			});
			global::Debug.LogFormat("tcpMessageType:{0}, waitingCount:{1}", new object[]
			{
				tcpMessageType,
				waitingCount
			});
			IEnumerator wait = Util.WaitForRealTime(waitingTerm);
			while (wait.MoveNext())
			{
				object obj = wait.Current;
				yield return obj;
			}
			waitingCount++;
			if (waitingCount == 45)
			{
				waitingCount = 0;
				IEnumerator enemyFailedAction = this.EnemyFailedAction();
				while (enemyFailedAction.MoveNext())
				{
					yield return null;
				}
			}
			if (this.confirmationChecks[tcpMessageType].Count == base.otherUserCount)
			{
				break;
			}
			yield return null;
		}
		LastConfirmationData lastConfirmationMessage = new LastConfirmationData
		{
			playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
			hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.LastConfirmation, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, tcpMessageType),
			tcpMessageType = tcpMessageType.ToInteger()
		};
		base.SendMessageForSync(TCPMessageType.LastConfirmation, lastConfirmationMessage);
		this.confirmationChecks[tcpMessageType].Clear();
		yield break;
		yield break;
	}

	protected override void TCPCallbackMethod(Dictionary<string, object> arg)
	{
		if (arg.ContainsKey("800012"))
		{
			this.ManageFailedPlayer(arg["800012"]);
		}
		else if (arg.ContainsKey("080110"))
		{
			this.ManageOnlineCheck(arg["080110"]);
		}
		else if (arg.ContainsKey("080112"))
		{
			this.ManageRecoverCommunicate(arg["080112"]);
		}
		else if (arg.ContainsKey("080114"))
		{
			this.ManageBattleActionLog(arg["080114"]);
		}
		else
		{
			base.TCPCallbackMethod(arg);
		}
	}

	private void ManageFailedPlayer(object messageObj)
	{
		if (!this.isSyncFinished)
		{
			return;
		}
		global::Debug.LogError("誰か落ちた");
		string valueByKey = MultiTools.GetValueByKey<string>(messageObj, "resultCode");
		global::Debug.LogFormat("resultCode :{0}", new object[]
		{
			valueByKey
		});
		AppCoroutine.Start(this.EnemyFailedAction(), false);
	}

	private IEnumerator EnemyFailedAction()
	{
		base.stateManager.uiControlPvP.HideRetireWindow();
		bool isEnd = false;
		base.stateManager.uiControlPvP.StartEnemyFailedTimer(delegate
		{
			isEnd = true;
			base.stateManager.uiControlPvP.HideAlertDialog();
			global::Debug.LogError("相手が落ちた(時間経過)");
		}, BattleUIControlPvP.DialogType.EnemyCount);
		while (!isEnd)
		{
			yield return null;
		}
		global::Debug.LogError("Time out: サーバーのAPI呼ぶ.");
		IEnumerator judge = Singleton<TCPMessageSender>.Instance.SendPvPOnlineCheck();
		while (judge.MoveNext())
		{
			object obj = judge.Current;
			yield return obj;
		}
		yield break;
	}

	private void ManageOnlineCheck(object messageObj)
	{
		int valueByKey = MultiTools.GetValueByKey<int>(messageObj, "resultCode");
		global::Debug.LogFormat("[対戦相手のオンラインステータスチェック（080110）]result_code:{0}", new object[]
		{
			valueByKey
		});
		if (valueByKey == 0)
		{
			Singleton<TCPMessageSender>.Instance.IsPvPOnlineCheck = true;
			this.ShowWinDialog();
		}
		else if (valueByKey == 1)
		{
			Singleton<TCPMessageSender>.Instance.IsPvPOnlineCheck = true;
			base.stateManager.uiControlPvP.HideAlertDialog();
		}
	}

	private void ManageRecoverCommunicate(object messageObj)
	{
		int valueByKey = MultiTools.GetValueByKey<int>(messageObj, "resultCode");
		global::Debug.LogFormat("[通信復帰（080112）]result_code:{0}", new object[]
		{
			valueByKey
		});
		if (valueByKey == 1 || valueByKey == 6 || valueByKey == 5)
		{
			Singleton<TCPMessageSender>.Instance.IsPvPRecoverCommunicateCheck = true;
			base.stateManager.uiControlPvP.HideAlertDialog();
			this.isDisconnected = false;
		}
		else if (valueByKey == 2)
		{
			Singleton<TCPMessageSender>.Instance.IsPvPRecoverCommunicateCheck = true;
			this.ShowDisconnectTCPDialog(null);
		}
		else if (valueByKey == 3)
		{
			Singleton<TCPMessageSender>.Instance.IsPvPRecoverCommunicateCheck = true;
			this.ShowWinDialog();
		}
		else if (valueByKey == 4)
		{
			Singleton<TCPMessageSender>.Instance.IsPvPRecoverCommunicateCheck = true;
			this.ShowBackToTitleDialog();
		}
		else
		{
			global::Debug.LogErrorFormat("ありえないキー:{0}.", new object[]
			{
				valueByKey
			});
		}
	}

	private void ManageBattleActionLog(object messageObj)
	{
		int valueByKey = MultiTools.GetValueByKey<int>(messageObj, "resultCode");
		if (valueByKey == 1)
		{
			global::Debug.LogFormat("[0]成功.", new object[]
			{
				"080114"
			});
		}
		else if (valueByKey == 0)
		{
			global::Debug.LogFormat("[0]失敗.", new object[]
			{
				"080114"
			});
		}
		if (MasterDataMng.Instance().RespDataMA_CodeM.codeM.PVP_BATTLE_ACTION_LOG == 1)
		{
			BattlePvPFunction.battleActionLogResult = valueByKey;
		}
	}

	protected override void RunRecieverPlayerActions(TCPMessageType tcpMessageType, object messageObj)
	{
		switch (tcpMessageType)
		{
		case TCPMessageType.None:
			return;
		default:
			if (tcpMessageType == TCPMessageType.LeaderChange)
			{
				base.RecieveLeaderChange(tcpMessageType, messageObj);
			}
			break;
		case TCPMessageType.RandomSeedSync:
			this.RecieveRandomSeedSync(tcpMessageType, messageObj);
			break;
		case TCPMessageType.Emotion:
			this.RecieveEmotion(tcpMessageType, messageObj);
			break;
		case TCPMessageType.Attack:
			this.RecieveAttack(tcpMessageType, messageObj);
			break;
		case TCPMessageType.Retire:
			this.RecieveRetire(tcpMessageType, messageObj);
			break;
		case TCPMessageType.Confirmation:
			base.RecieveConfirmation(tcpMessageType, messageObj);
			break;
		case TCPMessageType.LastConfirmation:
			base.RecieveLastConfirmation(tcpMessageType, messageObj);
			break;
		}
	}

	public void SendEmotion(UIButton button)
	{
		int index = 0;
		int iconSpritesIndex = 1;
		UISprite componentInChildren = button.transform.GetComponentInChildren<UISprite>();
		if (componentInChildren == null)
		{
			return;
		}
		string spriteName = componentInChildren.spriteName;
		base.stateManager.uiControlPvP.ShowEmotion(index, spriteName, false);
		SoundPlayer.PlayButtonEnter();
		EmotionData message = new EmotionData
		{
			playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
			hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.Emotion, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, TCPMessageType.None),
			spriteName = spriteName,
			iconSpritesIndex = iconSpritesIndex
		};
		base.SendMessageForSync(TCPMessageType.Emotion, message);
	}

	private void RecieveEmotion(TCPMessageType tcpMessageType, object messageObj)
	{
		if (this.isDisconnected)
		{
			return;
		}
		global::Debug.Log("Emotion: 受信");
		EmotionData emotionData = TCPData<EmotionData>.Convert(messageObj);
		string spriteName = emotionData.spriteName;
		int iconSpritesIndex = emotionData.iconSpritesIndex;
		bool isOther = true;
		base.stateManager.uiControlPvP.ShowEmotion(iconSpritesIndex, spriteName, isOther);
	}

	public IEnumerator SendRetire()
	{
		RetireData message = new RetireData
		{
			playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
			hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.Retire, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, TCPMessageType.None),
			retiredPlayerId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId
		};
		global::Debug.LogFormat("[リタイア]playerUserId:{0} hashValue:{1}", new object[]
		{
			message.playerUserId,
			message.hashValue
		});
		IEnumerator wait = this.SendMessageInsistently<RetireData>(TCPMessageType.Retire, message, 1f);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		this.ShowRetireDialog();
		yield break;
	}

	private void RecieveRetire(TCPMessageType tcpMessageType, object messageObj)
	{
		if (this.isDisconnected)
		{
			return;
		}
		global::Debug.Log("Retire: リタイア");
		RetireData retierData = TCPData<RetireData>.Convert(messageObj);
		if (base.CheckRecieveData(retierData.playerUserId, retierData.hashValue))
		{
			return;
		}
		this.lastAction[TCPMessageType.Retire] = delegate()
		{
			if (retierData.retiredPlayerId != ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId)
			{
				this.ShowWinDialog();
			}
		};
		base.SendConfirmation(tcpMessageType, retierData.playerUserId, string.Empty);
	}
}
