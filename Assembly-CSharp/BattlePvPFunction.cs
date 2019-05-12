using BattleStateMachineInternal;
using JsonFx.Json;
using Master;
using MultiBattle.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using WebAPIRequest;

public sealed class BattlePvPFunction : BattleMultiBasicFunction
{
	private const int failedDialogTerm = 10;

	private const int retireDialogTerm = 10;

	private bool isSendEvent;

	public static bool isAlreadyLoseBeforeBattle;

	public static int battleActionLogResult = -1;

	private int returnPvPOnlineCheck = -1;

	private int returnPvPJudgmentCheck = -1;

	private bool isPvPRecoverCommunicateCheck;

	private bool isPvPConnectionNoticeCheck;

	private bool isRegist;

	private bool isResume;

	private bool isEnemyFailedAction;

	private IEnumerator enemyFailedAction;

	[CompilerGenerated]
	private static Action <>f__mg$cache0;

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

	public IEnumerator BattleEndActionFunction()
	{
		base.stateManager.uiControlPvP.ShowLoading(false);
		if (this.isSendEvent)
		{
			yield return this.BattleEndActionTCP();
			yield return this.BattleEndActionJudgmentCheck();
			yield return this.BattleEndActionHttp();
		}
		else
		{
			this.StopSomething();
			yield return this.BattleEndActionTCP();
			yield return this.BattleEndActionJudgmentCheck();
			yield return this.BattleEndActionSync();
			yield return this.BattleEndActionHttp();
		}
		base.stateManager.uiControlPvP.HideLoading();
		yield break;
	}

	private IEnumerator BattleEndActionJudgmentCheck()
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		PvPJudgmentCheck message = new PvPJudgmentCheck
		{
			uniqueRequestId = Singleton<TCPUtil>.Instance.GetUniqueRequestId()
		};
		data.Add("080120", message);
		int count = 0;
		int ReSendCount = ConstValue.PVP_BATTLEEND_JUDGMENTCHECK;
		while (count < ReSendCount)
		{
			this.returnPvPJudgmentCheck = -1;
			Singleton<TCPUtil>.Instance.SendTCPRequest(data, "activityList");
			while (this.returnPvPJudgmentCheck == -1)
			{
				yield return null;
			}
			if (this.returnPvPJudgmentCheck != 1)
			{
				break;
			}
			yield return Util.WaitForRealTime(3f);
			count++;
			if (count >= ReSendCount)
			{
				global::Debug.Log(ReSendCount + "回試合判定を行いました");
				break;
			}
		}
		yield break;
	}

	private IEnumerator BattleEndActionTCP()
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		PvPRegist message = new PvPRegist
		{
			uniqueRequestId = Singleton<TCPUtil>.Instance.GetUniqueRequestId(),
			battleResult = ClassSingleton<MultiBattleData>.Instance.BattleResult
		};
		data.Add("080119", message);
		this.isRegist = false;
		float waitingCount = 0f;
		while (!this.isRegist)
		{
			Singleton<TCPUtil>.Instance.SendTCPRequest(data, "activityList");
			yield return Util.WaitForRealTime(2f);
			waitingCount += 2f;
			if (waitingCount >= 15f)
			{
				yield break;
			}
		}
		yield break;
	}

	private IEnumerator BattleEndActionSync()
	{
		if (base.IsOwner)
		{
			EnemyTurnSyncData message = new EnemyTurnSyncData
			{
				playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
				hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.EnemyTurnSync, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, TCPMessageType.None)
			};
			float sendWaitTime = 2f;
			float waitingCount = 0f;
			for (;;)
			{
				if (sendWaitTime >= 2f)
				{
					sendWaitTime = 0f;
					base.SendMessageForSyncDisconnected(TCPMessageType.EnemyTurnSync, message);
				}
				if (waitingCount >= 45f || this.confirmationChecks[TCPMessageType.EnemyTurnSync].Count == base.otherUserCount)
				{
					break;
				}
				sendWaitTime += Time.unscaledDeltaTime;
				waitingCount += Time.unscaledDeltaTime;
				yield return null;
			}
			LastConfirmationData message2 = new LastConfirmationData
			{
				playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
				hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.LastConfirmation, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, TCPMessageType.EnemyTurnSync),
				tcpMessageType = TCPMessageType.EnemyTurnSync.ToInteger()
			};
			base.SendMessageForSync(TCPMessageType.LastConfirmation, message2);
			this.confirmationChecks[TCPMessageType.EnemyTurnSync].Clear();
		}
		else
		{
			float waitingCount2 = 0f;
			while (waitingCount2 < 45f && !this.recieveChecks[TCPMessageType.EnemyTurnSync])
			{
				waitingCount2 += Time.unscaledDeltaTime;
				yield return null;
			}
			this.recieveChecks[TCPMessageType.EnemyTurnSync] = false;
		}
		yield break;
	}

	private IEnumerator BattleEndActionHttp()
	{
		GameWebAPI.RespData_ColosseumBattleEndLogic colosseumEnd = null;
		GameWebAPI.ColosseumBattleEndLogic request = new GameWebAPI.ColosseumBattleEndLogic
		{
			SetSendData = delegate(GameWebAPI.ReqData_ColosseumBattleEndLogic param)
			{
				param.battleResult = ClassSingleton<MultiBattleData>.Instance.BattleResult;
				param.roundCount = this.battleStateData.currentRoundNumber;
				param.isMockBattle = ((!(ClassSingleton<MultiBattleData>.Instance.MockBattleUserCode == "0")) ? 1 : 0);
				param.skillUseDeckPosition = "0";
			},
			OnReceived = delegate(GameWebAPI.RespData_ColosseumBattleEndLogic resData)
			{
				colosseumEnd = resData;
			}
		};
		RequestBase request2 = request;
		if (BattlePvPFunction.<>f__mg$cache0 == null)
		{
			BattlePvPFunction.<>f__mg$cache0 = new Action(RestrictionInput.EndLoad);
		}
		yield return request2.Run(BattlePvPFunction.<>f__mg$cache0, delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null);
		MultiBattleData.BattleEndResponseData responseData = new MultiBattleData.BattleEndResponseData();
		if (colosseumEnd != null)
		{
			responseData.resultCode = colosseumEnd.resultCode;
			List<MultiBattleData.BattleEndResponseData.Reward> list = new List<MultiBattleData.BattleEndResponseData.Reward>();
			if (colosseumEnd.reward != null)
			{
				for (int i = 0; i < colosseumEnd.reward.Length; i++)
				{
					list.Add(new MultiBattleData.BattleEndResponseData.Reward
					{
						assetCategoryId = colosseumEnd.reward[i].assetCategoryId,
						assetNum = colosseumEnd.reward[i].assetNum,
						assetValue = colosseumEnd.reward[i].assetValue
					});
				}
			}
			List<MultiBattleData.BattleEndResponseData.Reward> list2 = new List<MultiBattleData.BattleEndResponseData.Reward>();
			if (colosseumEnd.firstRankUpReward != null)
			{
				for (int j = 0; j < colosseumEnd.firstRankUpReward.Length; j++)
				{
					list2.Add(new MultiBattleData.BattleEndResponseData.Reward
					{
						assetCategoryId = colosseumEnd.firstRankUpReward[j].assetCategoryId,
						assetNum = colosseumEnd.firstRankUpReward[j].assetNum,
						assetValue = colosseumEnd.firstRankUpReward[j].assetValue
					});
				}
			}
			responseData.reward = list.ToArray();
			responseData.firstRankUpReward = list2.ToArray();
			responseData.score = colosseumEnd.score;
			responseData.colosseumRankId = colosseumEnd.colosseumRankId;
			responseData.isFirstRankUp = colosseumEnd.isFirstRankUp;
			if (colosseumEnd.battleRecord != null)
			{
				responseData.battleRecord = new MultiBattleData.BattleEndResponseData.ColosseumBattleRecord();
				responseData.battleRecord.count = colosseumEnd.battleRecord.count;
				responseData.battleRecord.winPercent = colosseumEnd.battleRecord.winPercent;
			}
		}
		else
		{
			responseData.reward = new MultiBattleData.BattleEndResponseData.Reward[0];
		}
		ClassSingleton<MultiBattleData>.Instance.BattleEndResponse = responseData;
		yield break;
	}

	protected override IEnumerator SendConnectionNotice()
	{
		this.isPvPConnectionNoticeCheck = false;
		Dictionary<string, object> data = new Dictionary<string, object>();
		PvPConnectionNoticeCheck message = new PvPConnectionNoticeCheck
		{
			uniqueRequestId = Singleton<TCPUtil>.Instance.GetUniqueRequestId()
		};
		data.Add("800013", message);
		while (!this.isPvPConnectionNoticeCheck)
		{
			Singleton<TCPUtil>.Instance.SendTCPRequest(data, "activityList");
			yield return Util.WaitForRealTime(2f);
		}
		yield break;
	}

	protected override IEnumerator ResumeTCP()
	{
		if (this.isResume)
		{
			yield break;
		}
		this.isResume = true;
		if (!this.isSendEvent)
		{
			base.stateManager.uiControlPvP.HideRetireWindow();
			base.stateManager.uiControlPvP.StartEnemyFailedTimer(delegate
			{
				base.stateManager.uiControlPvP.HideAlertDialog();
				this.ShowDisconnectTCPDialog(null);
			}, BattleUIControlPvP.DialogType.MyCount);
		}
		this.isPvPRecoverCommunicateCheck = false;
		Dictionary<string, object> data = new Dictionary<string, object>();
		PvPBattleRecover message = new PvPBattleRecover
		{
			isMockBattle = ((!(ClassSingleton<MultiBattleData>.Instance.MockBattleUserCode == "0")) ? 1 : 0),
			uniqueRequestId = Singleton<TCPUtil>.Instance.GetUniqueRequestId()
		};
		data.Add("080112", message);
		while (!this.isPvPRecoverCommunicateCheck)
		{
			Singleton<TCPUtil>.Instance.SendTCPRequest(data, "activityList");
			yield return Util.WaitForRealTime(2f);
		}
		yield return this.SendConnectionNotice();
		if (!this.isSendEvent)
		{
			base.stateManager.uiControlPvP.HideAlertDialog();
		}
		this.isDisconnected = false;
		this.isResume = false;
		yield break;
	}

	private void StopSomething()
	{
		this.isSendEvent = true;
		base.stateManager.uiControlPvP.HideAlertDialog();
		base.stateManager.uiControlPvP.HideSyncWait();
		base.stateManager.uiControlPvP.StopAttackTimer();
		base.stateManager.uiControlPvP.HideEmotionButton();
		base.stateManager.uiControlPvP.HideSkillSelectUI();
		base.stateManager.uiControlPvP.HideRetireWindow();
		if (this.enemyFailedAction != null)
		{
			AppCoroutine.Stop(this.enemyFailedAction, false);
		}
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
			if (callback != null)
			{
				callback();
			}
		}, true, 10);
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
		}, true, 10);
	}

	private void ShowConnectionErrorDialog()
	{
		if (this.isSendEvent)
		{
			return;
		}
		this.StopSomething();
		base.stateManager.events.CallConnectionErrorEvent();
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
	}

	public IEnumerator CheckAlreadyLoseBeforeBattle()
	{
		if (BattlePvPFunction.isAlreadyLoseBeforeBattle)
		{
			BattlePvPFunction.isAlreadyLoseBeforeBattle = false;
			this.ShowConnectionErrorDialog();
		}
		yield return null;
		yield break;
	}

	public override IEnumerator WaitAllPlayers(TCPMessageType tcpMessageType)
	{
		global::Debug.LogFormat("Wait tcpMessageType:{0}", new object[]
		{
			tcpMessageType
		});
		float waitingCount = 0f;
		for (;;)
		{
			while (this.isSendEvent || this.isDisconnected)
			{
				yield return null;
			}
			if (waitingCount >= 45f)
			{
				waitingCount = 0f;
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
			waitingCount += Time.unscaledDeltaTime;
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

	public override IEnumerator SendMessageInsistently<T>(TCPMessageType tcpMessageType, TCPData<T> message, float waitingTerm = 2f)
	{
		global::Debug.LogFormat("Send tcpMessageType:{0}", new object[]
		{
			tcpMessageType
		});
		float sendWaitTime = waitingTerm;
		float sendTotalWaitTime = 0f;
		for (;;)
		{
			while (this.isSendEvent || this.isDisconnected || this.isEnemyFailedAction)
			{
				yield return null;
			}
			if (sendWaitTime >= waitingTerm)
			{
				sendWaitTime = 0f;
				base.SendMessageForSync(tcpMessageType, message);
			}
			if (sendTotalWaitTime >= 45f)
			{
				sendTotalWaitTime = 0f;
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
			sendWaitTime += Time.unscaledDeltaTime;
			sendTotalWaitTime += Time.unscaledDeltaTime;
			yield return null;
		}
		LastConfirmationData message2 = new LastConfirmationData
		{
			playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
			hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.LastConfirmation, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, tcpMessageType),
			tcpMessageType = tcpMessageType.ToInteger()
		};
		base.SendMessageForSync(TCPMessageType.LastConfirmation, message2);
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
		else if (arg.ContainsKey("080120"))
		{
			this.ManageJudgmentCheck(arg["080120"]);
		}
		else if (arg.ContainsKey("080112"))
		{
			this.ManageRecoverCommunicate(arg["080112"]);
		}
		else if (arg.ContainsKey("800013"))
		{
			this.ManageConnectionNotice(arg["800013"]);
		}
		else if (arg.ContainsKey("080114"))
		{
			this.ManageBattleActionLog(arg["080114"]);
		}
		else if (arg.ContainsKey("080119"))
		{
			this.ManageRegist(arg["080119"]);
		}
		else
		{
			base.TCPCallbackMethod(arg);
		}
	}

	private void ShowErrorCodeAlert(Dictionary<string, object> arg)
	{
		string text = string.Empty;
		foreach (KeyValuePair<string, object> keyValuePair in arg)
		{
			if (keyValuePair.Key != null && keyValuePair.Value != null)
			{
				Dictionary<object, object> dictionary = (Dictionary<object, object>)keyValuePair.Value;
				foreach (KeyValuePair<object, object> keyValuePair2 in dictionary)
				{
					if (keyValuePair2.Key.ToString() == "errorCode" && keyValuePair2.Value != null)
					{
						text = keyValuePair2.Value.ToString();
						break;
					}
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				AlertManager.ShowAlertDialog(null, text);
				global::Debug.LogError("Key " + keyValuePair.Key + " errorCode " + text);
				break;
			}
		}
	}

	private void ManageFailedPlayer(object messageObj)
	{
		string valueByKey = MultiTools.GetValueByKey<string>(messageObj, "resultCode");
		global::Debug.LogFormat("切断時処理(800012) resultCode :{0}", new object[]
		{
			valueByKey
		});
		this.enemyFailedAction = this.EnemyFailedAction();
		AppCoroutine.Start(this.enemyFailedAction, false);
	}

	private IEnumerator EnemyFailedAction()
	{
		if (this.isSendEvent || this.isEnemyFailedAction)
		{
			yield break;
		}
		this.isEnemyFailedAction = true;
		base.stateManager.uiControlPvP.HideRetireWindow();
		base.stateManager.uiControlPvP.StartEnemyFailedTimer(delegate
		{
			base.stateManager.uiControlPvP.HideAlertDialog();
		}, BattleUIControlPvP.DialogType.EnemyCount);
		Dictionary<string, object> data = new Dictionary<string, object>();
		PvPOnlineCheck message = new PvPOnlineCheck
		{
			uniqueRequestId = Singleton<TCPUtil>.Instance.GetUniqueRequestId()
		};
		data.Add("080110", message);
		this.returnPvPOnlineCheck = -1;
		float sendWaitTime = 3f;
		float sendTotalWaitTime = 0f;
		while (this.returnPvPOnlineCheck != 1)
		{
			if (sendWaitTime >= 3f)
			{
				sendWaitTime = 0f;
				Singleton<TCPUtil>.Instance.SendTCPRequest(data, "activityList");
			}
			if (sendTotalWaitTime >= (float)ConstValue.PVP_BATTLE_ENEMY_RECOVER_TIME)
			{
				break;
			}
			sendWaitTime += Time.unscaledDeltaTime;
			sendTotalWaitTime += Time.unscaledDeltaTime;
			yield return null;
		}
		base.stateManager.uiControlPvP.HideAlertDialog();
		if (this.returnPvPOnlineCheck == 1)
		{
		}
		Dictionary<string, object> data2 = new Dictionary<string, object>();
		PvPJudgmentCheck message2 = new PvPJudgmentCheck
		{
			uniqueRequestId = Singleton<TCPUtil>.Instance.GetUniqueRequestId()
		};
		data2.Add("080120", message2);
		this.returnPvPJudgmentCheck = -1;
		while (this.returnPvPJudgmentCheck == -1)
		{
			Singleton<TCPUtil>.Instance.SendTCPRequest(data2, "activityList");
			yield return Util.WaitForRealTime(3f);
		}
		this.isEnemyFailedAction = false;
		yield break;
	}

	private void ManageOnlineCheck(object messageObj)
	{
		int valueByKey = MultiTools.GetValueByKey<int>(messageObj, "resultCode");
		global::Debug.LogFormat("[対戦相手のオンラインステータスチェック（080110）]result_code:{0}", new object[]
		{
			valueByKey
		});
		this.returnPvPOnlineCheck = valueByKey;
	}

	private void ManageJudgmentCheck(object messageObj)
	{
		string valueByKey = MultiTools.GetValueByKey<string>(messageObj, "resultCode");
		global::Debug.LogFormat("[試合判定（080120）]result_code:{0}", new object[]
		{
			valueByKey
		});
		this.returnPvPJudgmentCheck = valueByKey.ToInt32();
		if (this.returnPvPJudgmentCheck != 1)
		{
			if (this.returnPvPJudgmentCheck == 2)
			{
				this.ShowWinDialog();
			}
			else if (this.returnPvPJudgmentCheck == 3)
			{
				this.ShowDisconnectTCPDialog(null);
			}
		}
	}

	private void ManageRecoverCommunicate(object messageObj)
	{
		int valueByKey = MultiTools.GetValueByKey<int>(messageObj, "resultCode");
		global::Debug.LogFormat("[通信復帰（080112）]result_code:{0}", new object[]
		{
			valueByKey
		});
		if (valueByKey == 1)
		{
			this.isPvPRecoverCommunicateCheck = true;
		}
		else if (valueByKey != 5)
		{
			if (valueByKey == 2 || valueByKey == 6)
			{
				this.isPvPRecoverCommunicateCheck = true;
				this.ShowDisconnectTCPDialog(null);
			}
			else if (valueByKey == 3)
			{
				this.isPvPRecoverCommunicateCheck = true;
				this.ShowWinDialog();
			}
			else if (valueByKey == 4)
			{
				this.isPvPRecoverCommunicateCheck = true;
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
	}

	private void ManageConnectionNotice(object messageObj)
	{
		global::Debug.Log("[TCPサーバーへ接続通知（800013）]");
		this.isPvPConnectionNoticeCheck = true;
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

	private void ManageRegist(object messageObj)
	{
		global::Debug.Log("[バトル結果登録（080119）]");
		this.isRegist = true;
	}

	protected override void RunRecieverPlayerActions(TCPMessageType tcpMessageType, object messageObj)
	{
		switch (tcpMessageType)
		{
		case TCPMessageType.None:
			return;
		case TCPMessageType.EnemyTurnSync:
			this.RecieveEnemyTurnSync(tcpMessageType, messageObj);
			break;
		case TCPMessageType.RandomSeedSync:
			this.RecieveRandomSeedSync(tcpMessageType, messageObj);
			break;
		case TCPMessageType.Emotion:
			this.RecieveEmotion(tcpMessageType, messageObj);
			break;
		default:
			if (tcpMessageType != TCPMessageType.LeaderChange)
			{
				if (tcpMessageType == TCPMessageType.AdventureScene)
				{
					base.RecieveAdventureSceneData(tcpMessageType, messageObj);
				}
			}
			else
			{
				base.RecieveLeaderChange(tcpMessageType, messageObj);
			}
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

	private void RecieveEnemyTurnSync(TCPMessageType tcpMessageType, object messageObj)
	{
		global::Debug.Log("EnemyTurnSync: 受信");
		EnemyTurnSyncData enemyTurnSyncData = TCPData<EnemyTurnSyncData>.Convert(messageObj);
		if (base.CheckRecieveData(enemyTurnSyncData.playerUserId, enemyTurnSyncData.hashValue))
		{
			return;
		}
		this.lastAction[TCPMessageType.EnemyTurnSync] = delegate()
		{
			this.recieveChecks[TCPMessageType.EnemyTurnSync] = true;
		};
		base.SendConfirmationDisconnected(tcpMessageType, enemyTurnSyncData.playerUserId, string.Empty);
	}

	public void SendEmotion(UIButton button)
	{
		int index = 0;
		int iconSpritesIndex = 1;
		UITexture componentInChildren = button.transform.GetComponentInChildren<UITexture>();
		if (componentInChildren == null)
		{
			return;
		}
		string name = componentInChildren.mainTexture.name;
		base.stateManager.uiControlPvP.ShowEmotion(index, name, false);
		SoundPlayer.PlayButtonEnter();
		EmotionData message = new EmotionData
		{
			playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
			hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.Emotion, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, TCPMessageType.None),
			spriteName = name,
			iconSpritesIndex = iconSpritesIndex
		};
		base.SendMessageForSync(TCPMessageType.Emotion, message);
	}

	private void RecieveEmotion(TCPMessageType tcpMessageType, object messageObj)
	{
		global::Debug.Log("Emotion: 受信");
		EmotionData emotionData = TCPData<EmotionData>.Convert(messageObj);
		string spriteName = emotionData.spriteName;
		int iconSpritesIndex = emotionData.iconSpritesIndex;
		bool isOther = true;
		base.stateManager.uiControlPvP.ShowEmotion(iconSpritesIndex, spriteName, isOther);
	}

	public IEnumerator SendRetire()
	{
		if (this.isSendEvent)
		{
			yield break;
		}
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
		IEnumerator wait = this.SendMessageInsistently<RetireData>(TCPMessageType.Retire, message, 2f);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		this.StopSomething();
		base.stateManager.events.CallRetireEvent();
		yield break;
	}

	private void RecieveRetire(TCPMessageType tcpMessageType, object messageObj)
	{
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

	public void SendPvPBattleActionLog(AttackData attackData, int attackerIndex, bool isMyAction, BattleStateData battleStateData, List<BattleLogData.AttackLog> attackLog, List<BattleLogData.BuffLog> buffLog)
	{
		BattleLogData value = new BattleLogData
		{
			playerUserId = attackData.playerUserId,
			attackerIndex = attackerIndex,
			selectSkillIdx = attackData.selectSkillIdx,
			targetIdx = attackData.targetIdx,
			isTargetCharacterEnemy = attackData.isTargetCharacterEnemy,
			isMyAction = isMyAction,
			round = battleStateData.currentRoundNumber,
			turn = battleStateData.currentTurnNumber + 1,
			attackLog = attackLog,
			buffLog = buffLog
		};
		string action = JsonWriter.Serialize(value);
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		PvPBattleActionLog value2 = new PvPBattleActionLog
		{
			action = action,
			isMockBattle = ((!(ClassSingleton<MultiBattleData>.Instance.MockBattleUserCode == "0")) ? 1 : 0)
		};
		dictionary.Add("080114", value2);
		Singleton<TCPUtil>.Instance.SendTCPRequest(dictionary, "activityList");
	}
}
