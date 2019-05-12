using Master;
using MultiBattle.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleMultiFunction : BattleMultiBasicFunction
{
	private const int tcpMaxWaitingCount = 60;

	private List<string> recoverMembers = new List<string>();

	private string mustOpenMemberFailedDialogMessage = string.Empty;

	private List<string> failedMembers = new List<string>();

	private bool isMultiBattleResume;

	private bool isSendConnectionRecover;

	private List<RevivalData> revivalDataList = new List<RevivalData>();

	public bool isMyTurn { get; private set; }

	protected override string myTCPKey
	{
		get
		{
			return "multiBattle";
		}
	}

	protected override int MaxTimeOutValue
	{
		get
		{
			return ConstValue.MULTI_BATTLE_TIMEOUT_TIME;
		}
	}

	private MultiBattleData.PvPUserData GetOwnerPlayer()
	{
		return this.multiUsers.FirstOrDefault((MultiBattleData.PvPUserData user) => user.isOwner);
	}

	public bool IsMe(int monsterIndex)
	{
		return this.GetMyIndices().Any((int item) => item == monsterIndex);
	}

	private IEnumerable<int> GetMyIndices()
	{
		return this.multiUsers.Select((MultiBattleData.PvPUserData item, int index) => new
		{
			Index = index,
			Value = item.userStatus.userId
		}).Where(item => item.Value == ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId).Select(item => item.Index);
	}

	public void RefreshMonsterButtons()
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		for (int i = 0; i < base.battleStateData.playerCharacters.Length; i++)
		{
			bool isSelect = i == base.CurrentEnemyMyIndex;
			base.stateManager.uiControl.ApplyMonsterButtonEnable(i, isSelect, base.battleStateData.playerCharacters[i].isDied);
			string userId = this.multiUsers[i].userStatus.userId;
			if (!dictionary.ContainsKey(userId))
			{
				dictionary[userId] = i;
			}
			int playerIndex = dictionary[userId];
			string playerName = string.Empty;
			if (this.IsMe(i))
			{
				playerName = StringMaster.GetString("BattleUI-25");
			}
			else
			{
				playerName = this.multiUsers[i].userStatus.nickname;
			}
			base.stateManager.uiControlMulti.SetPlayerNumToMonsterButton(i, playerIndex, playerName);
		}
	}

	protected override void TCPCallbackMethod(Dictionary<string, object> arg)
	{
		if (arg.ContainsKey("800012"))
		{
			this.ManageFailedPlayer(arg["800012"]);
		}
		else if (arg.ContainsKey("820102"))
		{
			this.ManageBattleResult(arg["820102"]);
		}
		else if (arg.ContainsKey("820106"))
		{
			this.ManageBattleResume(arg["820106"]);
		}
		else
		{
			base.TCPCallbackMethod(arg);
		}
	}

	protected override IEnumerator ResumeTCP()
	{
		IEnumerator hidingCountDown = this.HidingCountDown(this.GetOwnerPlayer().userStatus.userId);
		AppCoroutine.Start(hidingCountDown, false);
		this.isMultiBattleResume = false;
		Dictionary<string, object> data = new Dictionary<string, object>();
		MultiMemberResume message = new MultiMemberResume
		{
			ri = DataMng.Instance().RespData_WorldMultiStartInfo.multiRoomId
		};
		data.Add("820106", message);
		while (!this.isMultiBattleResume)
		{
			Singleton<TCPUtil>.Instance.SendTCPRequest(data, "activityList");
			yield return Util.WaitForRealTime(2f);
		}
		yield break;
	}

	private void ManageBattleResult(object messageObj)
	{
		global::Debug.Log("ManageBattleResult");
		int valueByKey = MultiTools.GetValueByKey<int>(messageObj, "ct");
		if (valueByKey == -1)
		{
			global::Debug.LogWarning("ありえないキー.");
			return;
		}
		MultiBattleData.PvPUserData ownerPlayer = this.GetOwnerPlayer();
		if (ownerPlayer == null)
		{
			global::Debug.LogWarning("オーナーがない（ありえないはず）");
		}
		global::Debug.LogFormat("clearType:{0}", new object[]
		{
			valueByKey
		});
		if (valueByKey == 0)
		{
			if (!base.IsOwner)
			{
				this.lastAction[TCPMessageType.Retire] = delegate()
				{
					this.recieveChecks[TCPMessageType.Retire] = true;
					if (!base.hierarchyData.isPossibleContinue)
					{
						this.RunRetire(false, null);
					}
					else if (base.hierarchyData.limitRound > 0)
					{
						this.RunRetire(false, null);
					}
					else
					{
						this.ShowDisconnectOwnerRetireDialog(delegate
						{
							this.RunRetire(false, null);
						});
					}
				};
				base.SendConfirmation(TCPMessageType.Retire, ownerPlayer.userStatus.userId, string.Empty);
			}
		}
		else
		{
			if (valueByKey != 2 && valueByKey != 1 && valueByKey != 100)
			{
				global::Debug.LogWarningFormat("ありえないClearFlag:{0}.", new object[]
				{
					valueByKey
				});
				return;
			}
			if (!base.IsOwner)
			{
				if (base.stateManager.battleScreen != BattleScreen.PlayerWinner)
				{
					global::Debug.LogWarning("準備まだ.");
					return;
				}
				this.lastAction[TCPMessageType.BattleResult] = delegate()
				{
					this.recieveChecks[TCPMessageType.BattleResult] = true;
				};
				base.SendConfirmation(TCPMessageType.BattleResult, ownerPlayer.userStatus.userId, string.Empty);
			}
		}
	}

	private void ManageFailedPlayer(object messageObj)
	{
		global::Debug.LogWarning("誰か落ちた");
		string text = MultiTools.GetValueByKey<int>(messageObj, "si").ToString();
		string text2 = MultiTools.GetValueByKey<int>(messageObj, "ui").ToString();
		global::Debug.LogFormat("si:{0} ui:{1}", new object[]
		{
			text,
			text2
		});
		if (string.IsNullOrEmpty(text) || text != base.hierarchyData.startId)
		{
			global::Debug.LogWarning("ありえないキー.");
			return;
		}
		if (string.IsNullOrEmpty(text2) || text2 == ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId)
		{
			global::Debug.LogWarning("ありえないキー.");
			return;
		}
		MultiBattleData.PvPUserData playerByUserId = base.GetPlayerByUserId(text2);
		if (playerByUserId == null)
		{
			global::Debug.LogWarning("既にいないユーザ.");
			return;
		}
		this.isDisconnected = true;
		AppCoroutine.Start(this.HidingCountDown(playerByUserId.userStatus.userId), false);
	}

	private void ManageBattleResume(object messageObj)
	{
		this.isMultiBattleResume = true;
		int valueByKey = MultiTools.GetValueByKey<int>(messageObj, "rf");
		global::Debug.LogFormat("rf: {0}.", new object[]
		{
			valueByKey
		});
		if (valueByKey == 1)
		{
			global::Debug.Log("成功.");
		}
		else
		{
			global::Debug.Log("失敗.");
			this.ShowDisconnectTCPDialog(null);
		}
	}

	private IEnumerator HidingCountDown(string failedUserId)
	{
		if (this.recoverMembers.Contains(failedUserId) || this.isSendConnectionRecover || base.stateManager.uiControlMulti.GetNewDialog())
		{
			yield break;
		}
		this.recoverMembers.Add(failedUserId);
		base.stateManager.uiControlMulti.StartFailedTimer(delegate
		{
			base.stateManager.uiControlMulti.HideAlertDialog();
		});
		global::Debug.Log("開始");
		IEnumerator wait = Util.WaitForRealTime((float)ConstValue.MULTI_BATTLE_TIMEOUT_TIME);
		while (wait.MoveNext())
		{
			if (base.IsOwner)
			{
				base.stateManager.battleUiComponentsMulti.continueTimer.Stop();
			}
			base.stateManager.battleUiComponentsMulti.skillSelectUi.attackTime.StopTimer();
			yield return wait.Current;
		}
		global::Debug.Log("時間経過");
		this.recoverMembers.Remove(failedUserId);
		int recoverMemberCount = this.recoverMembers.Count<string>();
		global::Debug.Log("復帰中の人数: " + recoverMemberCount);
		if (recoverMemberCount == 0)
		{
			this.isSendConnectionRecover = true;
			if (base.IsOwner)
			{
				bool isEnd = false;
				GameWebAPI.RespData_MultiRoomStatusInfoLogic.StatusInfo info = null;
				IEnumerator runRoomStatusInfo = this.RunRoomStatusInfo(delegate(GameWebAPI.RespData_MultiRoomStatusInfoLogic.StatusInfo data)
				{
					info = data;
					isEnd = true;
				});
				AppCoroutine.Start(runRoomStatusInfo, false);
				while (!isEnd)
				{
					yield return null;
				}
				List<string> failedUserIdlist = new List<string>();
				foreach (GameWebAPI.RespData_MultiRoomStatusInfoLogic.StatusInfo.Member member in info.member)
				{
					if (member.onlineStatus == 0 && !this.multiUsers.Any((MultiBattleData.PvPUserData x) => x.userStatus.userId != member.userId))
					{
						failedUserIdlist.Add(member.userId);
						global::Debug.Log("切断 " + member.userId);
					}
				}
				IEnumerator function = this.SendConnectionRecover(failedUserIdlist.ToArray());
				while (function.MoveNext())
				{
					object obj = function.Current;
					yield return obj;
				}
			}
			else
			{
				IEnumerator function2 = this.WaitAllPlayersDisconnected(TCPMessageType.ConnectionRecover);
				while (function2.MoveNext())
				{
					object obj2 = function2.Current;
					yield return obj2;
				}
			}
			this.isSendConnectionRecover = false;
			if (base.IsOwner)
			{
				base.stateManager.battleUiComponentsMulti.continueTimer.Restart();
			}
			base.stateManager.battleUiComponentsMulti.skillSelectUi.attackTime.RestartTimer();
			base.stateManager.uiControlMulti.HideAlertDialog();
		}
		yield break;
	}

	private IEnumerator RunRoomStatusInfo(Action<GameWebAPI.RespData_MultiRoomStatusInfoLogic.StatusInfo> callback)
	{
		int multiRoomId = DataMng.Instance().RespData_WorldMultiStartInfo.multiRoomId.ToInt32();
		GameWebAPI.MultiRoomStatusInfoLogic request = new GameWebAPI.MultiRoomStatusInfoLogic
		{
			SetSendData = delegate(GameWebAPI.ReqData_MultiRoomStatusInfoLogic param)
			{
				param.roomId = multiRoomId;
			},
			OnReceived = delegate(GameWebAPI.RespData_MultiRoomStatusInfoLogic resData)
			{
				if (resData.statusInfo.roomId == multiRoomId)
				{
					global::Debug.Log("resData.status:" + resData.statusInfo.status);
					callback(resData.statusInfo);
				}
				else
				{
					global::Debug.LogError("ありえない.");
				}
			}
		};
		return request.Run(new Action(RestrictionInput.EndLoad), delegate(Exception noop)
		{
			RestrictionInput.EndLoad();
		}, null);
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
		case TCPMessageType.X2:
			this.RecieveX2(tcpMessageType, messageObj);
			break;
		case TCPMessageType.Attack:
			this.RecieveAttack(tcpMessageType, messageObj);
			break;
		case TCPMessageType.Revival1:
		case TCPMessageType.Revival2:
		case TCPMessageType.Revival3:
			this.RecieveRevival(tcpMessageType, messageObj);
			break;
		case TCPMessageType.Continue:
			this.RecieveContinue(tcpMessageType, messageObj);
			break;
		case TCPMessageType.Confirmation:
			base.RecieveConfirmation(tcpMessageType, messageObj);
			break;
		case TCPMessageType.LastConfirmation:
			base.RecieveLastConfirmation(tcpMessageType, messageObj);
			break;
		case TCPMessageType.Targat:
			this.RecieveTarget(tcpMessageType, messageObj);
			break;
		case TCPMessageType.RevivalCancel:
			this.RecieveRevivalCancel(tcpMessageType, messageObj);
			break;
		case TCPMessageType.ConnectionRecover:
			this.RecieveConnectionRecover(tcpMessageType, messageObj);
			break;
		case TCPMessageType.LeaderChange:
			base.RecieveLeaderChange(tcpMessageType, messageObj);
			break;
		case TCPMessageType.AdventureScene:
			base.RecieveAdventureSceneData(tcpMessageType, messageObj);
			break;
		}
	}

	public override IEnumerator SendMessageInsistently<T>(TCPMessageType tcpMessageType, TCPData<T> message, float waitingTerm = 2f)
	{
		IEnumerator function = this.SendMessageInsistently<T>(tcpMessageType, message, waitingTerm, true, 60f);
		while (function.MoveNext())
		{
			object obj = function.Current;
			yield return obj;
		}
		yield break;
	}

	private IEnumerator SendMessageInsistentlyDisconnected<T>(TCPMessageType tcpMessageType, TCPData<T> message, float waitingTerm = 2f) where T : class
	{
		IEnumerator function = this.SendMessageInsistently<T>(tcpMessageType, message, waitingTerm, false, 60f);
		while (function.MoveNext())
		{
			object obj = function.Current;
			yield return obj;
		}
		yield break;
	}

	private IEnumerator SendMessageInsistently<T>(TCPMessageType tcpMessageType, TCPData<T> message, float waitingTerm, bool enableDisconnected, float maxWaitingCount) where T : class
	{
		global::Debug.LogFormat("Send tcpMessageType:{0}", new object[]
		{
			tcpMessageType
		});
		float sendWaitTime = waitingTerm;
		float sendTotalWaitTime = 0f;
		for (;;)
		{
			while (enableDisconnected && this.isDisconnected)
			{
				yield return null;
			}
			if (sendWaitTime >= waitingTerm)
			{
				sendWaitTime = 0f;
				if (enableDisconnected)
				{
					base.SendMessageForSync(tcpMessageType, message);
				}
				else
				{
					base.SendMessageForSyncDisconnected(tcpMessageType, message);
				}
			}
			bool isTimeOut = sendTotalWaitTime >= maxWaitingCount;
			bool isConfirm = this.confirmationChecks[tcpMessageType].Count == base.otherUserCount;
			int failedCount = 0;
			foreach (string otherUsersId in base.GetOtherUsersId())
			{
				foreach (string failedMember in this.failedMembers)
				{
					if (otherUsersId == failedMember)
					{
						failedCount++;
					}
				}
			}
			bool isConfirmAndFailedMember = base.otherUserCount - this.confirmationChecks[tcpMessageType].Count == failedCount;
			if (isTimeOut || isConfirmAndFailedMember)
			{
				IEnumerator checkFailedPlayerAndShowDialog = this.CheckFailedPlayerAndShowDialog(tcpMessageType);
				while (checkFailedPlayerAndShowDialog.MoveNext())
				{
					object obj = checkFailedPlayerAndShowDialog.Current;
					yield return obj;
				}
			}
			if (isConfirm || isConfirmAndFailedMember || isTimeOut)
			{
				break;
			}
			sendWaitTime += Time.unscaledDeltaTime;
			sendTotalWaitTime += Time.unscaledDeltaTime;
			yield return null;
		}
		LastConfirmationData lastConfirmationMessage = new LastConfirmationData
		{
			playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
			hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.LastConfirmation, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, tcpMessageType),
			tcpMessageType = tcpMessageType.ToInteger(),
			failedPlayerUserIds = this.failedMembers.Distinct<string>().ToArray<string>()
		};
		base.SendMessageForSyncDisconnected(TCPMessageType.LastConfirmation, lastConfirmationMessage);
		this.confirmationChecks[tcpMessageType].Clear();
		yield break;
		yield break;
	}

	private IEnumerator CheckFailedPlayerAndShowDialog(TCPMessageType tcpMessageType)
	{
		global::Debug.Log("CheckFailedPlayerAndShowDialog");
		IEnumerable<string> sentUserIds = this.confirmationChecks[tcpMessageType].Distinct<string>();
		IEnumerable<MultiBattleData.PvPUserData> otherUsers = this.multiUsers.Where((MultiBattleData.PvPUserData item) => item.userStatus.userId != ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId).Distinct<MultiBattleData.PvPUserData>();
		foreach (MultiBattleData.PvPUserData otherUser in otherUsers)
		{
			if (!sentUserIds.Contains(otherUser.userStatus.userId))
			{
				global::Debug.LogWarningFormat("failedPlayerId:{0}, failedUserName:{1}.", new object[]
				{
					otherUser.userStatus.userId,
					otherUser.userStatus.nickname
				});
				IEnumerator disconnectAction = this.DisconnectAction(otherUser.userStatus.userId, otherUser.isOwner, null);
				while (disconnectAction.MoveNext())
				{
					object obj = disconnectAction.Current;
					yield return obj;
				}
			}
		}
		yield return null;
		yield break;
	}

	private void RefreshFailedPlayer(string retiredPlayerId)
	{
		MultiBattleData.PvPUserData owner = this.GetOwnerPlayer();
		this.multiUsers.ToList<MultiBattleData.PvPUserData>().ForEach(delegate(MultiBattleData.PvPUserData item)
		{
			if (item.userStatus.userId == retiredPlayerId)
			{
				item.userStatus.userId = owner.userStatus.userId;
				item.userStatus.nickname = owner.userStatus.nickname;
			}
		});
	}

	public override IEnumerator WaitAllPlayers(TCPMessageType tcpMessageType)
	{
		return this.WaitAllPlayers(tcpMessageType, true, 70f);
	}

	private IEnumerator WaitAllPlayersDisconnected(TCPMessageType tcpMessageType)
	{
		return this.WaitAllPlayers(tcpMessageType, false, 70f);
	}

	private IEnumerator WaitAllPlayers(TCPMessageType tcpMessageType, bool enableDisconnected, float maxWaitingCount)
	{
		global::Debug.LogFormat("Wait tcpMessageType:{0}", new object[]
		{
			tcpMessageType
		});
		string userId = string.Empty;
		bool isOwner = false;
		if (tcpMessageType == TCPMessageType.RandomSeedSync || tcpMessageType == TCPMessageType.EnemyTurnSync || tcpMessageType == TCPMessageType.Retire || tcpMessageType == TCPMessageType.Continue || tcpMessageType == TCPMessageType.RevivalCancel || tcpMessageType == TCPMessageType.ConnectionRecover || tcpMessageType == TCPMessageType.AdventureScene)
		{
			MultiBattleData.PvPUserData activePlayer = this.GetOwnerPlayer();
			userId = activePlayer.userStatus.userId;
			isOwner = activePlayer.isOwner;
		}
		else if (tcpMessageType == TCPMessageType.Attack)
		{
			MultiBattleData.PvPUserData activePlayer2 = base.GetPlayerByUserId(this.multiUsers[base.CurrentEnemyMyIndex].userStatus.userId);
			userId = activePlayer2.userStatus.userId;
			isOwner = activePlayer2.isOwner;
		}
		float waitingCount = 0f;
		for (;;)
		{
			while (enableDisconnected && this.isDisconnected)
			{
				yield return null;
			}
			bool isTimeOut = waitingCount >= maxWaitingCount;
			bool senderFailed = false;
			foreach (string failedMember in this.failedMembers)
			{
				if (userId == failedMember)
				{
					senderFailed = true;
					break;
				}
			}
			if (isTimeOut || senderFailed)
			{
				if (tcpMessageType == TCPMessageType.Attack && !isOwner)
				{
					CharacterStateControl attacker = base.battleStateData.playerCharacters[base.CurrentEnemyMyIndex];
					int tempAP = attacker.ap;
					attacker.ap = 0;
					base.stateManager.targetSelect.AutoPlayCharacterAndSkillSelectFunction(attacker);
					attacker.ap = tempAP;
					base.battleStateData.onSkillTrigger = true;
				}
				IEnumerator check = this.DisconnectAction(userId, isOwner, null);
				while (check.MoveNext())
				{
					object obj = check.Current;
					yield return obj;
				}
			}
			if (this.recieveChecks[tcpMessageType] || isTimeOut || senderFailed)
			{
				break;
			}
			waitingCount += Time.unscaledDeltaTime;
			yield return null;
		}
		foreach (string failedMember2 in this.failedMembers)
		{
			if (userId != failedMember2)
			{
				IEnumerator check2 = this.DisconnectAction(failedMember2, false, null);
				while (check2.MoveNext())
				{
					object obj2 = check2.Current;
					yield return obj2;
				}
			}
		}
		global::Debug.LogFormat("Finish waiting [{0}].", new object[]
		{
			tcpMessageType
		});
		this.recieveChecks[tcpMessageType] = false;
		yield break;
		yield break;
	}

	private IEnumerator DisconnectAction(string userId, bool isOwner, Action<bool> callback = null)
	{
		global::Debug.LogWarningFormat("DisconnectAction failedPlayerId:{0}", new object[]
		{
			userId
		});
		if (!isOwner)
		{
			global::Debug.Log("failedPlayer is not Owner");
			if (!this.failedMembers.Contains(userId))
			{
				this.failedMembers.Add(userId);
			}
			foreach (MultiBattleData.PvPUserData multiUser in this.multiUsers)
			{
				foreach (string failedMember in this.failedMembers)
				{
					if (multiUser.userStatus.userId == failedMember)
					{
						this.mustOpenMemberFailedDialogMessage = this.mustOpenMemberFailedDialogMessage + string.Format(StringMaster.GetString("BattleUI-29"), multiUser.userStatus.nickname) + "\n";
						break;
					}
				}
			}
			this.failedMembers.ForEach(delegate(string member)
			{
				this.RefreshFailedPlayer(member);
			});
			this.RefreshMonsterButtons();
			if (callback != null)
			{
				callback(true);
			}
			yield break;
		}
		global::Debug.Log("failedPlayer is Owner");
		this.ShowDisconnectOwnerDialog(null);
		for (;;)
		{
			yield return null;
		}
	}

	public IEnumerator SendTimeOut()
	{
		if (base.IsOwner)
		{
			IEnumerator coroutine = this.RunRetireByOwner(ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, null);
			while (coroutine.MoveNext())
			{
				object obj = coroutine.Current;
				yield return obj;
			}
		}
		else
		{
			IEnumerator coroutine2 = this.RunRetireByMember();
			while (coroutine2.MoveNext())
			{
				object obj2 = coroutine2.Current;
				yield return obj2;
			}
		}
		yield break;
	}

	private IEnumerator SendConnectionRecover(string[] failedUserIds)
	{
		int seed = (int)DateTime.Now.Ticks & 65535;
		ConnectionRecoverData message = new ConnectionRecoverData
		{
			playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
			hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.ConnectionRecover, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, TCPMessageType.None),
			failedUserIds = failedUserIds,
			randomSeed = seed
		};
		IEnumerator wait = this.SendMessageInsistentlyDisconnected<ConnectionRecoverData>(TCPMessageType.ConnectionRecover, message, 2f);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		UnityEngine.Random.seed = seed;
		this.ConnectionRecover(failedUserIds);
		yield break;
	}

	private void RecieveConnectionRecover(TCPMessageType tcpMessageType, object messageObj)
	{
		global::Debug.Log("ConnectionRecover: 受信");
		ConnectionRecoverData connectionRecoverData = TCPData<ConnectionRecoverData>.Convert(messageObj);
		if (base.CheckRecieveData(connectionRecoverData.playerUserId, connectionRecoverData.hashValue))
		{
			return;
		}
		this.lastAction[TCPMessageType.ConnectionRecover] = delegate()
		{
			global::Debug.LogFormat("randomSeed:{0}", new object[]
			{
				connectionRecoverData.randomSeed
			});
			UnityEngine.Random.seed = connectionRecoverData.randomSeed;
			this.ConnectionRecover(connectionRecoverData.failedUserIds);
			this.recieveChecks[TCPMessageType.ConnectionRecover] = true;
		};
		base.SendConfirmationDisconnected(TCPMessageType.ConnectionRecover, connectionRecoverData.playerUserId, string.Empty);
	}

	private void ConnectionRecover(string[] failedUserIds)
	{
		global::Debug.Log("再開.");
		this.isDisconnected = false;
		foreach (string userId in failedUserIds)
		{
			MultiBattleData.PvPUserData playerByUserId = base.GetPlayerByUserId(userId);
			if (playerByUserId.isOwner)
			{
				this.isDisconnected = true;
				this.ShowDisconnectOwnerDialog(null);
			}
			else if (!this.failedMembers.Contains(playerByUserId.userStatus.userId))
			{
				global::Debug.LogError("落ちた人: " + playerByUserId.userStatus.userId);
				this.failedMembers.Add(playerByUserId.userStatus.userId);
			}
		}
	}

	private void RecieveEnemyTurnSync(TCPMessageType tcpMessageType, object messageObj)
	{
		if (this.isDisconnected)
		{
			global::Debug.Log("EnemyTurnSync: 切断中");
			return;
		}
		global::Debug.Log("EnemyTurnSync: 受信");
		if (base.stateManager.rootState.currentState.GetType() != typeof(SubStateMultiCharacterRevivalFunction))
		{
			global::Debug.Log("currentState");
			return;
		}
		if (this.isSendCharacterRevival)
		{
			global::Debug.Log("isSendCharacterRevival");
			return;
		}
		EnemyTurnSyncData enemyTurnSyncData = TCPData<EnemyTurnSyncData>.Convert(messageObj);
		if (base.CheckRecieveData(enemyTurnSyncData.playerUserId, enemyTurnSyncData.hashValue))
		{
			return;
		}
		this.lastAction[TCPMessageType.EnemyTurnSync] = delegate()
		{
			this.recieveChecks[TCPMessageType.EnemyTurnSync] = true;
		};
		base.SendConfirmation(tcpMessageType, enemyTurnSyncData.playerUserId, string.Empty);
	}

	public void SendEmotion(int selectedEmotionType)
	{
		int num = 0;
		for (int i = 0; i < this.multiUsers.Length; i++)
		{
			if (this.multiUsers[i].userStatus.userId == ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId)
			{
				num = i;
				break;
			}
		}
		base.stateManager.uiControlMulti.ShowEmotion(num, selectedEmotionType, false);
		SoundPlayer.PlayButtonEnter();
		if (base.otherUserCount != 0)
		{
			EmotionData message = new EmotionData
			{
				playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
				hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.Emotion, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, TCPMessageType.None),
				emotionType = selectedEmotionType,
				iconSpritesIndex = num
			};
			base.SendMessageForSync(TCPMessageType.Emotion, message);
		}
	}

	private void RecieveEmotion(TCPMessageType tcpMessageType, object messageObj)
	{
		if (this.isDisconnected)
		{
			global::Debug.Log("Emotion: 切断中");
			return;
		}
		global::Debug.Log("Emotion: 受信");
		EmotionData emotionData = TCPData<EmotionData>.Convert(messageObj);
		int emotionType = emotionData.emotionType;
		int iconSpritesIndex = emotionData.iconSpritesIndex;
		bool isOther = true;
		base.stateManager.uiControlMulti.ShowEmotion(iconSpritesIndex, emotionType, isOther);
	}

	public IEnumerator Send2xSpeedPlay()
	{
		X2Data message = new X2Data
		{
			playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
			hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.X2, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, TCPMessageType.None),
			on2x = !base.hierarchyData.on2xSpeedPlay,
			onPose = base.battleStateData.isShowMenuWindow
		};
		IEnumerator wait = this.SendMessageInsistently<X2Data>(TCPMessageType.X2, message, 2f);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		base.hierarchyData.on2xSpeedPlay = !base.hierarchyData.on2xSpeedPlay;
		base.stateManager.uiControl.Apply2xPlay(base.hierarchyData.on2xSpeedPlay);
		base.stateManager.time.SetPlaySpeed(base.hierarchyData.on2xSpeedPlay, base.battleStateData.isShowMenuWindow);
		SoundPlayer.PlayButtonSelect();
		yield break;
	}

	private void RecieveX2(TCPMessageType tcpMessageType, object messageObj)
	{
		if (this.isDisconnected)
		{
			global::Debug.Log("X2: 切断中");
			return;
		}
		global::Debug.Log("X2: 受信");
		X2Data x2Data = TCPData<X2Data>.Convert(messageObj);
		if (base.CheckRecieveData(x2Data.playerUserId, x2Data.hashValue))
		{
			return;
		}
		bool on2x = x2Data.on2x;
		bool onPose = x2Data.onPose;
		this.lastAction[TCPMessageType.X2] = delegate()
		{
			this.stateManager.time.SetPlaySpeed(on2x, onPose);
			this.hierarchyData.on2xSpeedPlay = on2x;
			this.battleStateData.isShowMenuWindow = onPose;
			this.stateManager.uiControl.Apply2xPlay(on2x);
		};
		base.SendConfirmation(tcpMessageType, x2Data.playerUserId, string.Empty);
	}

	public void SendTarget()
	{
		CharacterStateControl characterStateControl = base.battleStateData.playerCharacters[base.battleStateData.currentSelectCharacterIndex];
		TargetData targetData = new TargetData
		{
			playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
			attckerIndex = base.battleStateData.currentSelectCharacterIndex,
			selectSkillIdx = characterStateControl.isSelectSkill,
			targetIdx = characterStateControl.targetCharacter.myIndex,
			isTargetCharacterEnemy = characterStateControl.targetCharacter.isEnemy
		};
		global::Debug.LogFormat("[ターゲット]targetIdx:{0}", new object[]
		{
			targetData.targetIdx
		});
		base.SendMessageForSync(TCPMessageType.Targat, targetData);
	}

	private void RecieveTarget(TCPMessageType tcpMessageType, object messageObj)
	{
		if (this.isDisconnected)
		{
			global::Debug.Log("Targat: 切断中");
			return;
		}
		global::Debug.Log("Targat: 受信");
		if (base.CurrentEnemyMyIndex == -1 || base.stateManager.rootState.currentState.GetType() != typeof(SubStateMultiWaitEnemySkillSelect))
		{
			global::Debug.LogWarning("まだ準備していない");
			return;
		}
		TargetData targetData = TCPData<TargetData>.Convert(messageObj);
		CharacterStateControl characterStateControl = base.battleStateData.playerCharacters[targetData.attckerIndex];
		if (characterStateControl == null)
		{
			return;
		}
		if (targetData.isTargetCharacterEnemy)
		{
			characterStateControl.targetCharacter = base.battleStateData.enemies[targetData.targetIdx];
		}
		else
		{
			characterStateControl.targetCharacter = base.battleStateData.playerCharacters[targetData.targetIdx];
		}
		characterStateControl.targetCharacter.myIndex = targetData.targetIdx;
		characterStateControl.isSelectSkill = targetData.selectSkillIdx;
	}

	public IEnumerator SendContinue()
	{
		int beforeConfirmDigiStoneNumber = base.battleStateData.beforeConfirmDigiStoneNumber;
		ContinueData message = new ContinueData
		{
			playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
			hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.Continue, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, TCPMessageType.None),
			digiStone = beforeConfirmDigiStoneNumber
		};
		IEnumerator wait = this.SendMessageInsistently<ContinueData>(TCPMessageType.Continue, message, 2f);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		yield break;
	}

	private void RecieveContinue(TCPMessageType tcpMessageType, object messageObj)
	{
		if (this.isDisconnected)
		{
			global::Debug.Log("Continue: 切断中");
			return;
		}
		global::Debug.Log("Continue: 受信");
		if (base.battleStateData.isContinueFlag || base.stateManager.battleScreen != BattleScreen.Continue)
		{
			return;
		}
		base.battleStateData.isShowRevivalWindow = false;
		ContinueData continueData = TCPData<ContinueData>.Convert(messageObj);
		if (base.CheckRecieveData(continueData.playerUserId, continueData.hashValue))
		{
			return;
		}
		this.lastAction[TCPMessageType.Continue] = delegate()
		{
			base.battleStateData.isContinueFlag = true;
			this.recieveChecks[TCPMessageType.Continue] = true;
		};
		base.SendConfirmation(tcpMessageType, continueData.playerUserId, string.Empty);
	}

	public bool isSendCharacterRevival { get; private set; }

	public int GetReservedCount()
	{
		int num = 0;
		for (int i = 0; i < base.battleStateData.playerCharacters.Length; i++)
		{
			if (base.battleStateData.isRevivalReservedCharacter[i])
			{
				num++;
			}
		}
		return num;
	}

	public string GetRevivalUserId(int revivalCharacterIndex)
	{
		foreach (RevivalData revivalData in this.revivalDataList)
		{
			if (revivalData.revivalCharacterIndex == revivalCharacterIndex)
			{
				return revivalData.playerUserId;
			}
		}
		return null;
	}

	public bool IsRevivalCancel(int revivalCharacterIndex)
	{
		foreach (RevivalData revivalData in this.revivalDataList)
		{
			if (revivalData.revivalCharacterIndex == revivalCharacterIndex)
			{
				return this.failedMembers.Contains(revivalData.playerUserId);
			}
		}
		return false;
	}

	public void RemoveRevivalData(string userId)
	{
		int num = -1;
		for (int i = 0; i < this.revivalDataList.Count<RevivalData>(); i++)
		{
			if (this.revivalDataList[i].playerUserId == userId)
			{
				num = i;
				break;
			}
		}
		if (num != -1)
		{
			this.revivalDataList.RemoveAt(num);
		}
	}

	private void RevivalCancel(string userId)
	{
		for (int i = 0; i < this.revivalDataList.Count<RevivalData>(); i++)
		{
			if (this.revivalDataList[i].playerUserId == userId)
			{
				base.battleStateData.isRevivalReservedCharacter[this.revivalDataList[i].revivalCharacterIndex] = false;
				AlwaysEffectParams alwaysEffectParams = base.battleStateData.revivalReservedEffect[this.revivalDataList[i].revivalCharacterIndex];
				BattleStateManager.current.threeDAction.StopAlwaysEffectAction(new AlwaysEffectParams[]
				{
					alwaysEffectParams
				});
			}
		}
	}

	public IEnumerator SendCharacterRevival()
	{
		this.isSendCharacterRevival = true;
		while (base.battleStateData.currentDigiStoneNumber < 1)
		{
			IEnumerator buyDigistoneFunction = base.stateManager.serverControl.ContinueBuyDigistoneFunction(false);
			while (buyDigistoneFunction.MoveNext())
			{
				yield return null;
			}
		}
		int beforeConfirmDigiStoneNumber = base.battleStateData.beforeConfirmDigiStoneNumber;
		int charIndex = base.battleStateData.currentSelectRevivalCharacter;
		TCPMessageType revivalType = (new TCPMessageType[]
		{
			TCPMessageType.Revival1,
			TCPMessageType.Revival2,
			TCPMessageType.Revival3
		})[charIndex];
		int waveNum = 0;
		RevivalData message = new RevivalData
		{
			playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
			hashValue = Singleton<TCPUtil>.Instance.CreateHash(revivalType, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, TCPMessageType.None),
			digiStone = beforeConfirmDigiStoneNumber,
			revivalCharacterIndex = charIndex,
			waveNum = waveNum
		};
		IEnumerator wait = this.SendMessageInsistently<RevivalData>(revivalType, message, 2f);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		IEnumerator revivalFunction = base.stateManager.deadOrAlive.OnDecisionCharacterRevivalFunction(charIndex);
		while (revivalFunction.MoveNext())
		{
			object obj2 = revivalFunction.Current;
			yield return obj2;
		}
		this.isSendCharacterRevival = false;
		yield break;
	}

	public IEnumerator SendCancelCharacterRevival(string[] userIds)
	{
		RevivalCancelData message = new RevivalCancelData
		{
			playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
			hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.RevivalCancel, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, TCPMessageType.None),
			cancelRevivalUserIds = userIds
		};
		IEnumerator wait = this.SendMessageInsistently<RevivalCancelData>(TCPMessageType.RevivalCancel, message, 2f);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		foreach (string userId in userIds)
		{
			this.RevivalCancel(userId);
			this.RemoveRevivalData(userId);
		}
		yield break;
	}

	private void RecieveRevival(TCPMessageType tcpMessageType, object messageObj)
	{
		if (this.isDisconnected)
		{
			global::Debug.Log("Revival: 切断中");
			return;
		}
		global::Debug.LogWarningFormat("Revival: 受信{0}", new object[]
		{
			base.stateManager.battleScreen
		});
		if (base.stateManager.battleScreen == BattleScreen.RoundStartActions || base.stateManager.battleScreen == BattleScreen.NextBattle || base.stateManager.battleScreen == BattleScreen.Continue || base.stateManager.battleScreen == BattleScreen.RevivalCharacter)
		{
			global::Debug.LogWarningFormat("[無視]Revival, battleState:{0}", new object[]
			{
				base.stateManager.battleScreen
			});
			return;
		}
		if (base.battleStateData.GetCharactersDeath(true))
		{
			global::Debug.LogWarningFormat("[無視]敵は今全員死んでいる.", new object[0]);
			return;
		}
		RevivalData revivalData = TCPData<RevivalData>.Convert(messageObj);
		if (base.CheckRecieveData(revivalData.playerUserId, revivalData.hashValue))
		{
			return;
		}
		int revivalCharacterIndex = revivalData.revivalCharacterIndex;
		TCPMessageType myRevivalType = tcpMessageType;
		global::Debug.LogWarningFormat("復活するキャラのIndex:{0}", new object[]
		{
			revivalCharacterIndex
		});
		this.lastAction[myRevivalType] = delegate()
		{
			if (this.battleStateData.playerCharacters[revivalCharacterIndex].isDied)
			{
				this.stateManager.uiControl.SetHudCollider(false);
				this.battleStateData.isRevivalReservedCharacter[revivalCharacterIndex] = true;
				BattleStateManager.current.threeDAction.PlayAlwaysEffectAction(this.battleStateData.revivalReservedEffect[revivalCharacterIndex], AlwaysEffectState.In);
				this.revivalDataList.Add(revivalData);
				global::Debug.LogWarning("revivalData.playerUserId " + revivalData.playerUserId);
			}
			this.recieveChecks[myRevivalType] = true;
		};
		base.SendConfirmation(myRevivalType, revivalData.playerUserId, string.Empty);
	}

	private void RecieveRevivalCancel(TCPMessageType tcpMessageType, object messageObj)
	{
		if (this.isDisconnected)
		{
			global::Debug.Log("RevivalCancel: 切断中");
			return;
		}
		global::Debug.LogWarningFormat("RevivalCancel: 受信{0}", new object[]
		{
			base.stateManager.battleScreen
		});
		global::Debug.LogWarningFormat("[無視]RevivalCancel, battleState:{0}", new object[]
		{
			base.stateManager.battleScreen
		});
		if (base.stateManager.battleScreen == BattleScreen.Continue)
		{
			global::Debug.LogWarningFormat("[無視]RevivalCancel, battleState:{0}", new object[]
			{
				base.stateManager.battleScreen
			});
			return;
		}
		RevivalCancelData revivalData = TCPData<RevivalCancelData>.Convert(messageObj);
		if (base.CheckRecieveData(revivalData.playerUserId, revivalData.hashValue))
		{
			return;
		}
		this.lastAction[TCPMessageType.RevivalCancel] = delegate()
		{
			foreach (string userId in revivalData.cancelRevivalUserIds)
			{
				this.RevivalCancel(userId);
				this.RemoveRevivalData(userId);
			}
			this.recieveChecks[TCPMessageType.RevivalCancel] = true;
		};
		base.SendConfirmation(TCPMessageType.RevivalCancel, revivalData.playerUserId, string.Empty);
	}

	public void TurnStartInit(CharacterStateControl characterStateControl)
	{
		if (!string.IsNullOrEmpty(this.mustOpenMemberFailedDialogMessage))
		{
			this.ShowDisconnectMemberDialog(delegate
			{
				this.mustOpenMemberFailedDialogMessage = string.Empty;
			});
		}
		base.CurrentEnemyMyIndex = characterStateControl.myIndex;
		this.isMyTurn = this.IsMe(base.CurrentEnemyMyIndex);
		base.battleStateData.isPossibleTargetSelect = this.isMyTurn;
		base.stateManager.uiControlMulti.ShowX2PlayButton();
		base.stateManager.uiControlMulti.ApplySkillSelectUI(this.isMyTurn);
		base.stateManager.uiControlMulti.ShowEmotionButton();
		base.stateManager.targetSelect.TargetManualSelectAndApplyUIFunction(null);
		if (this.isMyTurn)
		{
			if (base.battleStateData.currentWaveNumber == 0 && base.battleStateData.currentTurnNumber == 0)
			{
				base.battleStateData.enableRotateCam = true;
			}
			base.stateManager.uiControlMulti.HideRemainingTurnRightDown();
			SoundPlayer.PlayBattleMyTurnSE();
			base.stateManager.uiControlMulti.ShowRemainingTurnMiddle();
			base.stateManager.uiControlMulti.SetRemainingTurnMiddleLabel(-1, RemainingTurn.MiddleType.You);
		}
		else
		{
			base.battleStateData.enableRotateCam = false;
			base.stateManager.uiControlMulti.ShowRemainingTurnRightDown();
			int num = int.MaxValue;
			bool flag = false;
			foreach (int num2 in this.GetMyIndices())
			{
				int skillOrder = base.battleStateData.playerCharacters[num2].skillOrder;
				if (skillOrder > 0)
				{
					flag = true;
					num = Mathf.Min(num, skillOrder);
				}
			}
			if (flag)
			{
				base.stateManager.uiControlMulti.SetRemainingTurnRightDownLabel(num, RemainingTurn.MiddleType.None);
				base.stateManager.uiControlMulti.ShowRemainingTurnMiddle();
				base.stateManager.uiControlMulti.SetRemainingTurnMiddleLabel(num, RemainingTurn.MiddleType.Others);
			}
			else
			{
				base.stateManager.uiControlMulti.HideRemainingTurnRightDown();
			}
		}
		this.RefreshMonsterButtons();
	}

	protected override void ShowDisconnectTCPDialog(Action callback = null)
	{
		this.ShowDisconnectDialog(BattleMultiFunction.DisconnectDialogType.TCP, callback);
	}

	private void ShowDisconnectOwnerDialog(Action callback = null)
	{
		this.ShowDisconnectDialog(BattleMultiFunction.DisconnectDialogType.Owner, callback);
	}

	private void ShowDisconnectMemberDialog(Action callback = null)
	{
		this.ShowDisconnectDialog(BattleMultiFunction.DisconnectDialogType.Member, callback);
	}

	private void ShowDisconnectOwnerRetireDialog(Action callback = null)
	{
		this.ShowDisconnectDialog(BattleMultiFunction.DisconnectDialogType.OwnerRetire, callback);
	}

	private void ShowDisconnectDialog(BattleMultiFunction.DisconnectDialogType type, Action callback = null)
	{
		string text = string.Empty;
		switch (type)
		{
		case BattleMultiFunction.DisconnectDialogType.TCP:
			text = StringMaster.GetString("BattleUI-27");
			break;
		case BattleMultiFunction.DisconnectDialogType.Owner:
			text = StringMaster.GetString("BattleUI-28");
			break;
		case BattleMultiFunction.DisconnectDialogType.Member:
			text += this.mustOpenMemberFailedDialogMessage;
			text += StringMaster.GetString("BattleUI-30");
			break;
		case BattleMultiFunction.DisconnectDialogType.OwnerRetire:
			text = StringMaster.GetString("BattleUI-42");
			break;
		}
		bool isDisconnectMember = type == BattleMultiFunction.DisconnectDialogType.Member;
		base.stateManager.uiControlMulti.HideAllDIalog();
		base.stateManager.uiControlMulti.ShowAlertDialog(text, StringMaster.GetString("SystemButtonClose"), delegate
		{
			if (callback != null)
			{
				callback();
			}
			this.stateManager.uiControlMulti.HideAlertDialog();
			if (!isDisconnectMember)
			{
				this.GotoFarm(false, null);
			}
		}, !isDisconnectMember, (!isDisconnectMember) ? -1 : 1);
		if (!isDisconnectMember)
		{
			base.stateManager.battleUiComponentsMulti.skillSelectUi.attackTime.StopTimer();
			base.stateManager.uiControlMulti.BlockNewDialog();
		}
	}

	public IEnumerator SendClearResult()
	{
		if (base.IsOwner && !this.isDisconnected)
		{
			global::Debug.LogWarning("SendClearResult : RunRoomStatusInfo");
			GameWebAPI.RespData_MultiRoomStatusInfoLogic.StatusInfo info = null;
			AppCoroutine.Start(this.RunRoomStatusInfo(delegate(GameWebAPI.RespData_MultiRoomStatusInfoLogic.StatusInfo data)
			{
				info = data;
			}), false);
			while (info == null)
			{
				yield return null;
			}
			if (info.status == "11")
			{
				global::Debug.LogWarning("SendClearResult : Reconnect");
				IEnumerator reconnect = base.Reconnect();
				while (reconnect.MoveNext())
				{
					object obj = reconnect.Current;
					yield return obj;
				}
			}
			if (!Singleton<TCPUtil>.Instance.CheckTCPConnection())
			{
				global::Debug.LogWarning("SendClearResult : CheckTCPConnection");
				this.ShowDisconnectTCPDialog(null);
				yield break;
			}
		}
		if (base.IsOwner)
		{
			string[] userMonsterIds = base.hierarchyData.usePlayerCharacters.Select((PlayerStatus item) => item.userMonsterId).ToArray<string>();
			Dictionary<string, object> data2 = this.CreateResultData(base.battleStateData.playerCharacters, userMonsterIds, base.hierarchyData.startId.ToInt32(), 1, base.GetOtherUsersId().Select((string item) => item.ToInt32()).ToList<int>());
			float waitWinTime = 0f;
			do
			{
				while (this.isDisconnected)
				{
					yield return null;
				}
				Singleton<TCPUtil>.Instance.SendTCPRequest(data2, "activityList");
				(data2["820102"] as BattleResult).requestStatus = 1;
				yield return Util.WaitForRealTime(2f);
				waitWinTime += 2f;
			}
			while (waitWinTime < 15f && this.confirmationChecks[TCPMessageType.BattleResult].Count < base.otherUserCount);
			global::Debug.Log("SendMessageForSync TCPMessageType.BattleResult");
			LastConfirmationData lastConfirmationMessage = new LastConfirmationData
			{
				playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
				hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.LastConfirmation, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, TCPMessageType.BattleResult),
				tcpMessageType = TCPMessageType.BattleResult.ToInteger(),
				failedPlayerUserIds = this.failedMembers.Distinct<string>().ToArray<string>()
			};
			base.SendMessageForSyncDisconnected(TCPMessageType.LastConfirmation, lastConfirmationMessage);
			IEnumerator wait = Util.WaitForRealTime(0.5f);
			while (wait.MoveNext())
			{
				object obj2 = wait.Current;
				yield return obj2;
			}
		}
		else
		{
			float waitWinTime2 = 0f;
			for (;;)
			{
				while (this.isDisconnected)
				{
					yield return null;
				}
				waitWinTime2 += Time.unscaledDeltaTime;
				if (waitWinTime2 >= 15f)
				{
					break;
				}
				if (this.recieveChecks[TCPMessageType.BattleResult])
				{
					goto Block_14;
				}
				yield return null;
			}
			this.ShowDisconnectOwnerDialog(null);
			Block_14:;
		}
		base.stateManager.uiControlMulti.BlockNewDialog();
		yield break;
	}

	public void SendRetire(Action callback = null)
	{
		this.RunRetire(true, callback);
	}

	private void RunRetire(bool isWaiting, Action callback = null)
	{
		if (isWaiting)
		{
			AppCoroutine.Start(this.RunRetireByOwner(ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, callback), false);
		}
		else
		{
			this.RunRetireAfter(callback);
		}
	}

	private IEnumerator RunRetireByOwner(string retiredPlayerId, Action callback)
	{
		float waitRetireTime = 0f;
		string[] userMonsterIds = base.hierarchyData.usePlayerCharacters.Select((PlayerStatus item) => item.userMonsterId).ToArray<string>();
		Dictionary<string, object> data = this.CreateResultData(base.battleStateData.playerCharacters, userMonsterIds, base.hierarchyData.startId.ToInt32(), 0, base.GetOtherUsersId().Select((string item) => item.ToInt32()).ToList<int>());
		for (;;)
		{
			while (this.isDisconnected)
			{
				yield return null;
			}
			Singleton<TCPUtil>.Instance.SendTCPRequest(data, "activityList");
			(data["820102"] as BattleResult).requestStatus = 1;
			yield return Util.WaitForRealTime(2f);
			waitRetireTime += 2f;
			if (waitRetireTime >= 15f)
			{
				break;
			}
			if (this.confirmationChecks[TCPMessageType.Retire].Count >= base.otherUserCount)
			{
				goto Block_5;
			}
		}
		this.RunRetireAfter(callback);
		yield break;
		Block_5:
		global::Debug.Log("SendMessageForSync TCPMessageType.Retire");
		LastConfirmationData lastConfirmationMessage = new LastConfirmationData
		{
			playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
			hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.LastConfirmation, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, TCPMessageType.Retire),
			tcpMessageType = TCPMessageType.Retire.ToInteger(),
			failedPlayerUserIds = this.failedMembers.Distinct<string>().ToArray<string>()
		};
		base.SendMessageForSyncDisconnected(TCPMessageType.LastConfirmation, lastConfirmationMessage);
		IEnumerator wait = Util.WaitForRealTime(0.5f);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		this.RunRetireAfter(callback);
		yield break;
		yield break;
	}

	private IEnumerator RunRetireByMember()
	{
		float waitRetireTime = 0f;
		for (;;)
		{
			while (this.isDisconnected)
			{
				yield return null;
			}
			waitRetireTime += Time.unscaledDeltaTime;
			if (waitRetireTime >= 5f)
			{
				break;
			}
			if (this.recieveChecks[TCPMessageType.Retire])
			{
				goto Block_3;
			}
			yield return null;
		}
		this.ShowDisconnectOwnerDialog(null);
		yield break;
		Block_3:
		yield break;
		yield break;
	}

	private void RunRetireAfter(Action callback = null)
	{
		base.battleStateData.isShowRetireWindow = false;
		this.GotoFarm(true, callback);
		base.stateManager.soundManager.VolumeBGM *= 2f;
	}

	private void GotoFarm(bool isRetire = false, Action callback = null)
	{
		global::Debug.LogFormat("[GotoFarm]isRetire:{0}", new object[]
		{
			isRetire
		});
		if (isRetire)
		{
			base.stateManager.events.CallRetireEvent();
		}
		else
		{
			base.stateManager.events.CallConnectionErrorEvent();
		}
		if (base.onServerConnect)
		{
			DataMng.Instance().WD_ReqDngResult.clear = 0;
		}
		if (callback != null)
		{
			callback();
		}
	}

	private Dictionary<string, object> CreateResultData(CharacterStateControl[] playerCharacters, string[] userMonsterIds, int startId, int clearFlag, List<int> ogis)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		List<int> list = new List<int>();
		for (int j = 0; j < playerCharacters.Length; j++)
		{
			if (!playerCharacters[j].isDied)
			{
				list.Add(userMonsterIds[j].ToInt32());
			}
			else
			{
				global::Debug.LogFormat("[死亡したデジモン]hp:{0}, isEnemy:{1}, id:{2}", new object[]
				{
					playerCharacters[j].hp,
					playerCharacters[j].isEnemy,
					userMonsterIds[j]
				});
			}
		}
		string[] value = list.Select((int i) => i.ToString()).ToArray<string>();
		string text = string.Join(",", value);
		global::Debug.LogFormat("生存モンスターID:{0}", new object[]
		{
			text
		});
		string[] value2 = ogis.Select((int i) => i.ToString()).Distinct<string>().ToArray<string>();
		string text2 = string.Join(",", value2);
		global::Debug.LogFormat("生存ユーザー:{0}", new object[]
		{
			text2
		});
		string multiRoomId = DataMng.Instance().RespData_WorldMultiStartInfo.multiRoomId;
		int num = multiRoomId.ToInt32() + "820102".ToInt32();
		global::Debug.LogFormat("clearFlag{0}, uniqueRequestId:{1}", new object[]
		{
			clearFlag,
			num
		});
		BattleResult value3 = new BattleResult
		{
			ri = multiRoomId.ToInt32(),
			si = startId,
			cf = clearFlag,
			amis = list,
			ogis = ogis.Distinct<int>().ToList<int>(),
			clearRound = DataMng.Instance().WD_ReqDngResult.clearRound,
			uniqueRequestId = num
		};
		dictionary.Add("820102", value3);
		return dictionary;
	}

	private enum DisconnectDialogType
	{
		TCP,
		Owner,
		Member,
		OwnerRetire
	}
}
