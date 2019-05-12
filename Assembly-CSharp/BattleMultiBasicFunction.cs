using MultiBattle.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BattleMultiBasicFunction : BattleFunctionBase
{
	protected const float tcpSendWaitingTerm = 2f;

	protected const float sendTcpWaitTime = 15f;

	private const int MAX_EXCEPTION_COUNT = 3;

	protected Dictionary<TCPMessageType, Action> lastAction = new Dictionary<TCPMessageType, Action>();

	protected Dictionary<TCPMessageType, List<string>> confirmationChecks;

	private Queue hashValueQueue = new Queue();

	protected Dictionary<TCPMessageType, bool> recieveChecks;

	protected MultiBattleData.PvPUserData[] multiUsers;

	protected bool isDisconnected;

	protected float pausedTime;

	protected bool isPaused;

	private int tryRecoverCount;

	private int exceptionCount;

	private List<string> adventureSceneEndList = new List<string>();

	protected virtual string myTCPKey
	{
		get
		{
			return string.Empty;
		}
	}

	protected virtual int MaxTimeOutValue
	{
		get
		{
			return 0;
		}
	}

	protected virtual int MaxRecoverCount
	{
		get
		{
			return 0;
		}
	}

	protected int otherUserCount
	{
		get
		{
			return this.GetOtherUsersId().Count<string>();
		}
	}

	public int CurrentEnemyMyIndex { protected get; set; }

	public bool IsOwner { get; protected set; }

	public bool IsDisconnected
	{
		get
		{
			return this.isDisconnected;
		}
	}

	protected MultiBattleData.PvPUserData GetPlayerByUserId(string userId)
	{
		return this.multiUsers.Where((MultiBattleData.PvPUserData user) => user.userStatus.userId == userId).FirstOrDefault<MultiBattleData.PvPUserData>();
	}

	protected bool CheckRecieveData(string playerUserId, string hashValue)
	{
		bool flag = this.CheckHash(hashValue);
		bool flag2 = this.CheckStillAlive(playerUserId);
		bool flag3 = this.GetOtherUsersId().Where((string item) => item == playerUserId).Count<string>() == 0;
		if (flag || !flag2 || flag3)
		{
			global::Debug.LogFormat("isHash:{0}, isStillAlive:{1}", new object[]
			{
				flag,
				flag2
			});
			return true;
		}
		return false;
	}

	private bool CheckHash(string hashString)
	{
		bool flag = this.hashValueQueue.Contains(hashString);
		if (flag)
		{
			global::Debug.LogFormat("すでにあるhashString:{0}", new object[]
			{
				hashString
			});
			return true;
		}
		this.hashValueQueue.Enqueue(hashString);
		int num = 30;
		if (this.hashValueQueue.Count > num)
		{
			this.hashValueQueue.Dequeue();
		}
		return false;
	}

	private bool CheckStillAlive(string userId)
	{
		return this.multiUsers.Any((MultiBattleData.PvPUserData user) => user.userStatus.userId == userId);
	}

	protected void RunLastAction(TCPMessageType tcpMessageType)
	{
		if (this.lastAction.ContainsKey(tcpMessageType))
		{
			this.lastAction[tcpMessageType]();
		}
		else
		{
			global::Debug.LogWarningFormat("{0}はthis.lastAction内にない", new object[]
			{
				tcpMessageType
			});
		}
	}

	public void RunAttackAutomatically()
	{
		CharacterStateControl characterStateControl = base.battleStateData.playerCharacters[base.battleStateData.currentSelectCharacterIndex];
		characterStateControl.isSelectSkill = 0;
		BattleStateManager.current.uiControl.ApplySkillButtonRotation(characterStateControl.isSelectSkill, 0);
		base.battleStateData.onSkillTrigger = true;
		base.stateManager.targetSelect.SetTarget(characterStateControl, null);
	}

	public string GetPlayerName()
	{
		string text = this.multiUsers.Where((MultiBattleData.PvPUserData user) => user.userStatus.userId == ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId).Select((MultiBattleData.PvPUserData user) => user.userStatus.nickname).FirstOrDefault<string>();
		if (string.IsNullOrEmpty(text))
		{
			return "-";
		}
		return text;
	}

	public string GetPlayerTitleId()
	{
		return this.multiUsers.Where((MultiBattleData.PvPUserData user) => user.userStatus.userId == ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId).Select((MultiBattleData.PvPUserData user) => user.userStatus.titleId).FirstOrDefault<string>();
	}

	public string GetEnemyName()
	{
		string text = this.multiUsers.Where((MultiBattleData.PvPUserData user) => user.userStatus.userId != ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId).Select((MultiBattleData.PvPUserData user) => user.userStatus.nickname).FirstOrDefault<string>();
		if (string.IsNullOrEmpty(text))
		{
			return "-";
		}
		return text;
	}

	public string GetEnemyTitleId()
	{
		return this.multiUsers.Where((MultiBattleData.PvPUserData user) => user.userStatus.userId != ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId).Select((MultiBattleData.PvPUserData user) => user.userStatus.titleId).FirstOrDefault<string>();
	}

	protected IEnumerable<string> GetOtherUsersId()
	{
		return this.multiUsers.Where((MultiBattleData.PvPUserData user) => user.userStatus.userId != ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId).Select((MultiBattleData.PvPUserData user) => user.userStatus.userId).Distinct<string>();
	}

	public void Initialize()
	{
		this.CurrentEnemyMyIndex = -1;
		this.recieveChecks = new Dictionary<TCPMessageType, bool>();
		this.confirmationChecks = new Dictionary<TCPMessageType, List<string>>();
		foreach (object obj in Enum.GetValues(typeof(TCPMessageType)))
		{
			TCPMessageType key = (TCPMessageType)((int)obj);
			this.recieveChecks[key] = false;
			this.confirmationChecks[key] = new List<string>();
		}
		this.multiUsers = ClassSingleton<MultiBattleData>.Instance.PvPUserDatas;
		this.IsOwner = this.multiUsers.Any((MultiBattleData.PvPUserData item) => item.userStatus.userId == ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId && item.isOwner);
		base.stateManager.onApplicationPause.Add(new Action<bool>(this.OnApplicationPause));
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (!this.isPaused)
		{
			return;
		}
		if (!pauseStatus)
		{
			this.isPaused = false;
			this.tryRecoverCount++;
			int num = (int)(Time.realtimeSinceStartup - this.pausedTime);
			if (this.MaxTimeOutValue > 0 && num >= this.MaxTimeOutValue)
			{
				global::Debug.LogErrorFormat("{0}秒経ったので負け.", new object[]
				{
					num
				});
				this.ShowDisconnectTCPDialog(null);
				return;
			}
			if (this.MaxRecoverCount > 0 && this.tryRecoverCount >= this.MaxRecoverCount)
			{
				global::Debug.LogErrorFormat("{0}回復帰しようとしたので負け.", new object[]
				{
					this.tryRecoverCount
				});
				this.ShowDisconnectTCPDialog(null);
				return;
			}
			base.StartCoroutine(this.Reconnect());
		}
	}

	public void InitializeTCPClient(bool isReconnect = false)
	{
		bool flag = this.ConnectTCPServer(base.stateManager.battleMode);
		if (flag)
		{
			if (isReconnect)
			{
				AppCoroutine.Start(this.ResumeTCP(), false);
			}
			else
			{
				AppCoroutine.Start(this.SendConnectionNotice(), false);
				this.isDisconnected = false;
			}
		}
		else
		{
			global::Debug.LogWarning("TCP接続 failed.");
			this.isDisconnected = true;
			this.ShowDisconnectTCPDialog(null);
		}
		global::Debug.Log("Initialized TCP");
	}

	protected virtual IEnumerator SendConnectionNotice()
	{
		yield break;
	}

	protected virtual IEnumerator ResumeTCP()
	{
		yield break;
	}

	private bool ConnectTCPServer(BattleMode battleMode)
	{
		Screen.sleepTimeout = -1;
		Singleton<TCPUtil>.Instance.MakeTCPClient();
		Singleton<TCPUtil>.Instance.battleMode = battleMode;
		Singleton<TCPUtil>.Instance.SetTCPCallBackMethod(new Action<Dictionary<string, object>>(this.TCPCallbackMethod));
		Singleton<TCPUtil>.Instance.SetOnExitCallBackMethod(new Action(this.TCPDisconnectedCallbackMethod));
		Singleton<TCPUtil>.Instance.SetExceptionMethod(new Action<short, string>(this.ExceptionCallbackMethod));
		return Singleton<TCPUtil>.Instance.ConnectTCPServer(ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId);
	}

	protected virtual void TCPCallbackMethod(Dictionary<string, object> arg)
	{
		if (!arg.ContainsKey(this.myTCPKey))
		{
			global::Debug.LogWarningFormat("{0} is not valid key.", new object[]
			{
				arg.Keys.First<string>()
			});
			return;
		}
		Dictionary<object, object> dictionary = arg[this.myTCPKey] as Dictionary<object, object>;
		string text = dictionary.Keys.First<object>().ToString();
		TCPMessageType tcpMessageType = MultiTools.StringToEnum<TCPMessageType>(text);
		object messageObj = dictionary[text];
		this.RunRecieverPlayerActions(tcpMessageType, messageObj);
	}

	protected abstract void RunRecieverPlayerActions(TCPMessageType tcpMessageType, object messageObj);

	private void TCPDisconnectedCallbackMethod()
	{
		global::Debug.Log("切断されました.(意図的も含む)");
		this.isDisconnected = true;
	}

	private void ExceptionCallbackMethod(short exitCode, string message)
	{
		global::Debug.LogWarningFormat("Exceptionが発生しました。exitCode:{0}, message:{1}", new object[]
		{
			exitCode,
			message
		});
		this.isDisconnected = true;
		if (exitCode == 741)
		{
			global::Debug.LogError("意図的に切断(Pause)、あとで勝手に再接続.");
			this.pausedTime = Time.realtimeSinceStartup;
			this.isPaused = true;
		}
		else if (exitCode == 720)
		{
			global::Debug.LogError("タイムアウト 負け.");
			this.ShowDisconnectTCPDialog(null);
		}
		else if (exitCode == 750 || exitCode == 700 || exitCode == 711 || exitCode == 752 || exitCode == 760)
		{
			global::Debug.Log("無理やり再接続.");
			base.StartCoroutine(this.Reconnect());
		}
		else
		{
			base.StartCoroutine(this.Exception(delegate(bool retry)
			{
				if (retry)
				{
					global::Debug.Log("無理やり再接続.");
					return this.Reconnect();
				}
				global::Debug.LogErrorFormat("その他何らかのエラー. exitCode:{0}.", new object[]
				{
					exitCode
				});
				this.ShowDisconnectTCPDialog(null);
				return null;
			}));
		}
	}

	protected IEnumerator Reconnect()
	{
		yield return new WaitForEndOfFrame();
		this.InitializeTCPClient(true);
		yield break;
	}

	private IEnumerator Exception(Func<bool, IEnumerator> function)
	{
		this.exceptionCount++;
		global::Debug.Log("exceptionCount " + this.exceptionCount);
		IEnumerator wait = function(this.exceptionCount < 3);
		while (wait != null && wait.MoveNext())
		{
			yield return wait.Current;
		}
		if (Singleton<TCPUtil>.Instance.CheckTCPConnection())
		{
			this.exceptionCount = 0;
		}
		yield break;
	}

	public void FinalizeTCP()
	{
		global::Debug.Log("Finalizeされました.");
		Singleton<TCPUtil>.Instance.TCPDisConnect(true);
		Screen.sleepTimeout = -2;
	}

	protected virtual void ShowDisconnectTCPDialog(Action callback = null)
	{
	}

	public IEnumerator SendRandomSeedSync()
	{
		int randomSeed = (int)DateTime.Now.Ticks & 65535;
		RandomSeedSyncData message = new RandomSeedSyncData
		{
			playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
			hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.RandomSeedSync, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, TCPMessageType.None),
			randomSeed = randomSeed
		};
		global::Debug.LogFormat("[RandomSeedSync送信] randomSeed:{0}", new object[]
		{
			randomSeed
		});
		IEnumerator wait = base.stateManager.multiBasicFunction.SendMessageInsistently<RandomSeedSyncData>(TCPMessageType.RandomSeedSync, message, 2f);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		UnityEngine.Random.seed = randomSeed;
		yield break;
	}

	protected virtual void RecieveRandomSeedSync(TCPMessageType tcpMessageType, object messageObj)
	{
		if (this.isDisconnected)
		{
			global::Debug.Log("RandomSeedSync: 切断中");
			return;
		}
		global::Debug.Log("RandomSeedSync: 受信");
		if (base.stateManager.rootState.currentState.GetType() != typeof(SubStateWaitRandomSeedSync))
		{
			return;
		}
		RandomSeedSyncData randomSeedSyncData = TCPData<RandomSeedSyncData>.Convert(messageObj);
		if (this.CheckRecieveData(randomSeedSyncData.playerUserId, randomSeedSyncData.hashValue))
		{
			return;
		}
		this.lastAction[TCPMessageType.RandomSeedSync] = delegate()
		{
			global::Debug.LogFormat("randomSeed:{0}", new object[]
			{
				randomSeedSyncData.randomSeed
			});
			UnityEngine.Random.seed = randomSeedSyncData.randomSeed;
			this.recieveChecks[TCPMessageType.RandomSeedSync] = true;
		};
		this.SendConfirmation(tcpMessageType, randomSeedSyncData.playerUserId, string.Empty);
	}

	public virtual IEnumerator SendAttack()
	{
		base.battleStateData.isPossibleTargetSelect = false;
		CharacterStateControl attackerCharacter = base.battleStateData.playerCharacters[base.battleStateData.currentSelectCharacterIndex];
		AttackData message = new AttackData
		{
			playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
			hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.Attack, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, TCPMessageType.None),
			selectSkillIdx = attackerCharacter.isSelectSkill,
			targetIdx = attackerCharacter.targetCharacter.myIndex,
			isTargetCharacterEnemy = attackerCharacter.targetCharacter.isEnemy
		};
		global::Debug.LogFormat("[攻撃]targetIdx:{0} selectSkillIdx:{1}", new object[]
		{
			message.targetIdx,
			message.selectSkillIdx
		});
		IEnumerator wait = this.SendMessageInsistently<AttackData>(TCPMessageType.Attack, message, 2f);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		base.battleStateData.isPossibleTargetSelect = true;
		yield break;
	}

	protected virtual void RecieveAttack(TCPMessageType tcpMessageType, object messageObj)
	{
		if (this.isDisconnected)
		{
			global::Debug.Log("Attack: 切断中");
			return;
		}
		global::Debug.Log("Attack: 受信");
		if (base.stateManager.rootState.currentState.GetType() != typeof(SubStateMultiWaitEnemySkillSelect) && base.stateManager.rootState.currentState.GetType() != typeof(SubStateWaitEnemySkillSelect))
		{
			global::Debug.LogWarning("まだ準備していない");
			return;
		}
		AttackData attackData = TCPData<AttackData>.Convert(messageObj);
		if (this.CheckRecieveData(attackData.playerUserId, attackData.hashValue))
		{
			return;
		}
		this.lastAction[TCPMessageType.Attack] = delegate()
		{
			CharacterStateControl currentSelectCharacterState = this.battleStateData.currentSelectCharacterState;
			int targetIdx = attackData.targetIdx;
			int selectSkillIdx = attackData.selectSkillIdx;
			if (this.battleStateData.currentSelectCharacterState.isEnemy)
			{
				if (attackData.isTargetCharacterEnemy)
				{
					currentSelectCharacterState.targetCharacter = this.battleStateData.playerCharacters[targetIdx];
				}
				else
				{
					currentSelectCharacterState.targetCharacter = this.battleStateData.enemies[targetIdx];
				}
			}
			else if (attackData.isTargetCharacterEnemy)
			{
				currentSelectCharacterState.targetCharacter = this.battleStateData.enemies[targetIdx];
			}
			else
			{
				currentSelectCharacterState.targetCharacter = this.battleStateData.playerCharacters[targetIdx];
			}
			currentSelectCharacterState.targetCharacter.myIndex = targetIdx;
			currentSelectCharacterState.isSelectSkill = selectSkillIdx;
			this.battleStateData.onSkillTrigger = true;
			this.recieveChecks[TCPMessageType.Attack] = true;
		};
		this.SendConfirmation(tcpMessageType, attackData.playerUserId, string.Empty);
	}

	public IEnumerator SendLeaderChange(int leaderindex)
	{
		LeaderChangeData message = new LeaderChangeData
		{
			playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
			hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.LeaderChange, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, TCPMessageType.None),
			leaderIndex = leaderindex
		};
		IEnumerator wait = this.SendMessageInsistently<LeaderChangeData>(TCPMessageType.LeaderChange, message, 2f);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		base.battleStateData.ChangePlayerLeader(message.leaderIndex);
		base.battleStateData.ChangeEnemyLeader(message.leaderIndex);
		yield break;
	}

	protected void RecieveLeaderChange(TCPMessageType tcpMessageType, object messageObj)
	{
		if (this.isDisconnected)
		{
			global::Debug.Log("LeaderChange: 切断中");
			return;
		}
		global::Debug.Log("LeaderChange: 受信");
		LeaderChangeData data = TCPData<LeaderChangeData>.Convert(messageObj);
		if (this.CheckHash(data.hashValue))
		{
			return;
		}
		this.lastAction[tcpMessageType] = delegate()
		{
			this.battleStateData.ChangePlayerLeader(data.leaderIndex);
			this.battleStateData.ChangeEnemyLeader(data.leaderIndex);
			this.recieveChecks[tcpMessageType] = true;
		};
		this.SendConfirmation(tcpMessageType, data.playerUserId, string.Empty);
	}

	public bool isAdventureSceneAllEnd { get; private set; }

	public IEnumerator SendAdventureSceneData()
	{
		this.isAdventureSceneAllEnd = false;
		string[] otherUsersIds = this.GetOtherUsersId().ToArray<string>();
		if (otherUsersIds.Length > 0)
		{
			int count = 0;
			foreach (string otherUsersId in otherUsersIds)
			{
				foreach (string adventureSceneEnd in this.adventureSceneEndList)
				{
					if (otherUsersId == adventureSceneEnd)
					{
						count++;
						break;
					}
				}
			}
			if (!base.stateManager.battleAdventureSceneManager.isUpdate && count >= otherUsersIds.Length)
			{
				this.isAdventureSceneAllEnd = true;
			}
		}
		else
		{
			this.isAdventureSceneAllEnd = true;
		}
		if (this.isAdventureSceneAllEnd)
		{
			this.adventureSceneEndList.Clear();
		}
		AdventureSceneData message = new AdventureSceneData
		{
			playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
			hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.AdventureScene, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, TCPMessageType.None),
			isEnd = ((!this.isAdventureSceneAllEnd) ? 0 : 1)
		};
		IEnumerator wait = this.SendMessageInsistently<AdventureSceneData>(TCPMessageType.AdventureScene, message, 2f);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		yield break;
	}

	protected void RecieveAdventureSceneData(TCPMessageType tcpMessageType, object messageObj)
	{
		if (this.isDisconnected)
		{
			global::Debug.Log("AdventureSceneData: 切断中");
			return;
		}
		global::Debug.Log("AdventureSceneData: 受信");
		AdventureSceneData data = TCPData<AdventureSceneData>.Convert(messageObj);
		if (this.CheckHash(data.hashValue))
		{
			return;
		}
		this.lastAction[tcpMessageType] = delegate()
		{
			this.isAdventureSceneAllEnd = (data.isEnd == 1);
			if (this.isAdventureSceneAllEnd)
			{
				this.adventureSceneEndList.Clear();
			}
			this.recieveChecks[tcpMessageType] = true;
		};
		int num = (!base.stateManager.battleAdventureSceneManager.isUpdate) ? 1 : 0;
		this.SendConfirmation(tcpMessageType, data.playerUserId, num.ToString());
	}

	protected void SendConfirmation(TCPMessageType tcpMessageType, string targetId, string value1 = "")
	{
		this.SendConfirmation(tcpMessageType, targetId, value1, true);
	}

	protected void SendConfirmationDisconnected(TCPMessageType tcpMessageType, string targetId, string value1 = "")
	{
		this.SendConfirmation(tcpMessageType, targetId, value1, false);
	}

	private void SendConfirmation(TCPMessageType tcpMessageType, string targetId, string value1, bool enableDisconnected)
	{
		global::Debug.LogFormat("[SendConfirmation] tcpMessageType:{0}, MyPlayerUserId:{1}, targetId:{2}", new object[]
		{
			tcpMessageType,
			ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
			targetId
		});
		ConfirmationData message = new ConfirmationData
		{
			playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
			hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.Confirmation, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, TCPMessageType.None),
			tcpMessageType = tcpMessageType.ToInteger(),
			value1 = value1
		};
		AppCoroutine.Start(this.SendConfirm<ConfirmationData>(TCPMessageType.Confirmation, message, targetId, enableDisconnected), false);
	}

	protected void RecieveConfirmation(TCPMessageType tcpMessageType, object messageObj)
	{
		global::Debug.Log("Confirmation: 受信");
		ConfirmationData confirmationData = TCPData<ConfirmationData>.Convert(messageObj);
		if (this.CheckRecieveData(confirmationData.playerUserId, confirmationData.hashValue))
		{
			return;
		}
		TCPMessageType tcpMessageType2 = (TCPMessageType)confirmationData.tcpMessageType;
		this.confirmationChecks[tcpMessageType2].Add(confirmationData.playerUserId);
		global::Debug.LogFormat("confirmationChecks : {0}から確認用{1}を受信しました. Count:{2}", new object[]
		{
			confirmationData.playerUserId,
			tcpMessageType2,
			this.confirmationChecks[tcpMessageType2].Count
		});
		if (tcpMessageType2 == TCPMessageType.AdventureScene && confirmationData.value1 == "1" && !this.adventureSceneEndList.Contains(confirmationData.playerUserId))
		{
			this.adventureSceneEndList.Add(confirmationData.playerUserId);
		}
	}

	protected void RecieveLastConfirmation(TCPMessageType tcpMessageType, object messageObj)
	{
		global::Debug.Log("LastConfirmation: 受信");
		LastConfirmationData lastConfirmationData = TCPData<LastConfirmationData>.Convert(messageObj);
		if (this.CheckRecieveData(lastConfirmationData.playerUserId, lastConfirmationData.hashValue))
		{
			return;
		}
		this.confirmationChecks[TCPMessageType.Confirmation].Add(lastConfirmationData.playerUserId);
		this.RunLastAction((TCPMessageType)lastConfirmationData.tcpMessageType);
		global::Debug.LogFormat("tcpMessageType:{0}", new object[]
		{
			(TCPMessageType)lastConfirmationData.tcpMessageType
		});
	}

	protected void SendMessageForSync(TCPMessageType tcpMessageType, object message)
	{
		this.SendMessageForSync(tcpMessageType, message, true);
	}

	protected void SendMessageForSyncDisconnected(TCPMessageType tcpMessageType, object message)
	{
		this.SendMessageForSync(tcpMessageType, message, false);
	}

	private void SendMessageForSync(TCPMessageType tcpMessageType, object message, bool enableDisconnected)
	{
		List<string> list = this.GetOtherUsersId().ToList<string>();
		if (list.Count <= 0)
		{
			global::Debug.LogFormat("{0} 今は自分だけ", new object[]
			{
				tcpMessageType
			});
			this.confirmationChecks[tcpMessageType].Clear();
			return;
		}
		this.SendMessageForTarget(tcpMessageType, message, list.ToArray(), enableDisconnected);
	}

	private IEnumerator SendConfirm<T>(TCPMessageType tcpMessageType, TCPData<T> message, string targetId, bool enableDisconnected = true) where T : class
	{
		global::Debug.LogFormat("Send tcpMessageType:{0}", new object[]
		{
			tcpMessageType
		});
		float sendWaitTime = 2f;
		for (;;)
		{
			if (sendWaitTime >= 2f)
			{
				sendWaitTime = 0f;
				this.SendMessageForTarget(tcpMessageType, message, new string[]
				{
					targetId
				}, enableDisconnected);
			}
			sendWaitTime += Time.unscaledDeltaTime;
			if (this.otherUserCount == 0 || this.confirmationChecks[tcpMessageType].Where((string item) => item == targetId).Any<string>())
			{
				break;
			}
			yield return null;
		}
		this.confirmationChecks[tcpMessageType].Clear();
		yield break;
		yield break;
	}

	private void SendMessageForTarget(TCPMessageType tcpMessageType, object message, string[] targetIds, bool enableDisconnected = true)
	{
		if (enableDisconnected && this.isDisconnected)
		{
			return;
		}
		Dictionary<string, object> data = ClassSingleton<TCPMessageFactory>.Instance.CreateMessage(tcpMessageType, message);
		if (this.multiUsers == null || this.multiUsers.Length == 0)
		{
			return;
		}
		global::Debug.Log("targetIds: " + targetIds);
		if (Singleton<TCPUtil>.Instance == null)
		{
			global::Debug.LogWarning("this.myTCPUtil is null");
		}
		else
		{
			Singleton<TCPUtil>.Instance.SendTCPRequest(data, targetIds.Select((string item) => item.ToInt32()).ToList<int>(), this.myTCPKey);
			global::Debug.LogFormat("[TCPMessage]{0}を送信しました.", new object[]
			{
				tcpMessageType
			});
		}
	}

	public abstract IEnumerator SendMessageInsistently<T>(TCPMessageType tcpMessageType, TCPData<T> message, float waitingTerm = 2f) where T : class;

	public abstract IEnumerator WaitAllPlayers(TCPMessageType tcpMessageType);

	public enum ClearFlag
	{
		Failed,
		Clear
	}

	public enum ClearType
	{
		Failed,
		FirstClear,
		Clear,
		Expired = 100
	}
}
