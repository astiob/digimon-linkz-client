using BattleStateMachineInternal;
using MultiBattle.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BattleMultiBasicFunction : BattleFunctionBase
{
	protected const float countDownWaitingTerm = 1f;

	protected const float tcpWaitingTerm = 1f;

	protected const int failedDialogTerm = 10;

	protected const int tcpMaxWaitingCount = 60;

	private const int MAX_EXCEPTION_COUNT = 3;

	protected Dictionary<TCPMessageType, Action> lastAction = new Dictionary<TCPMessageType, Action>();

	protected Dictionary<TCPMessageType, List<string>> confirmationChecks;

	private Queue hashValueQueue = new Queue();

	protected Dictionary<TCPMessageType, bool> recieveChecks;

	protected MultiBattleData.PvPUserData[] multiUsers;

	protected TCPUtil myTCPUtil;

	protected bool isDisconnected;

	protected float pausedTime;

	protected bool isPaused;

	private int exceptionCount;

	protected virtual string myTCPKey
	{
		get
		{
			return string.Empty;
		}
	}

	protected virtual int otherUserCount
	{
		get
		{
			return 1;
		}
	}

	public string[] userMonsterIds
	{
		get
		{
			return base.stateManager.serverControl._userMonsterId;
		}
		set
		{
			base.stateManager.serverControl._userMonsterId = value;
		}
	}

	public Dictionary<int, PlayerStatus> cachedPlayerStatus
	{
		get
		{
			return base.stateManager.serverControl._cachedPlayerStatus;
		}
		set
		{
			base.stateManager.serverControl._cachedPlayerStatus = value;
		}
	}

	public Dictionary<string, GameWebAPI.RespDataMA_GetSkillM.SkillM> cachedLeaderSkillMs
	{
		get
		{
			return base.stateManager.serverControl._cachedLeaderSkillM;
		}
		set
		{
			base.stateManager.serverControl._cachedLeaderSkillM = value;
		}
	}

	public Dictionary<string, Tolerance> cachedTolerances
	{
		get
		{
			return base.stateManager.serverControl._cachedTolerance;
		}
		set
		{
			base.stateManager.serverControl._cachedTolerance = value;
		}
	}

	public Dictionary<string, SkillStatus> cachedSkillStatuses
	{
		get
		{
			return base.stateManager.serverControl._cachedSkillStatus;
		}
		set
		{
			base.stateManager.serverControl._cachedSkillStatus = value;
		}
	}

	public Dictionary<string, LeaderSkillStatus> cachedLeaderSkillStatuses
	{
		get
		{
			return base.stateManager.serverControl._cachedLeaderSkillStatus;
		}
		set
		{
			base.stateManager.serverControl._cachedLeaderSkillStatus = value;
		}
	}

	public Dictionary<string, CharacterDatas> cachedCharacterData
	{
		get
		{
			return base.stateManager.serverControl._cachedCharacterDatas;
		}
		set
		{
			base.stateManager.serverControl._cachedCharacterDatas = value;
		}
	}

	public Dictionary<string, ExtraEffectStatus> cachedExtraEffectStatus
	{
		get
		{
			return base.stateManager.serverControl._cachedExtraEffectStatus;
		}
		set
		{
			base.stateManager.serverControl._cachedExtraEffectStatus = value;
		}
	}

	public int CurrentEnemyMyIndex { protected get; set; }

	public bool IsOwner { get; protected set; }

	protected abstract void RunRecieverPlayerActions(TCPMessageType tcpMessageType, object messageObj);

	public abstract void RoundStartInitMulti();

	public bool CheckHash(string hashString)
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

	public IEnumerator OnlineCheckActionFunction()
	{
		IEnumerator battelStart = Singleton<TCPMessageSender>.Instance.SendPvPOnlineCheck();
		while (battelStart.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	public abstract IEnumerator SendAttack();

	protected PlayerStatus ConvertAPIParamsToPlayerStatus(GameWebAPI.Common_MonsterData commonMonsterData)
	{
		MonsterData monsterData = MultiTools.MakeAndSetMonster(commonMonsterData);
		string monsterGroupId = monsterData.monsterM.monsterGroupId;
		int arousal = ServerToBattleUtility.GetArousal(monsterData.monsterM.rare.ToInt32() - 1);
		int friendshipLevel = ServerToBattleUtility.ServerValueToInt(commonMonsterData.friendship);
		int hp = ServerToBattleUtility.ServerValueToInt(commonMonsterData.hp);
		int attackPower = ServerToBattleUtility.ServerValueToInt(commonMonsterData.attack);
		int defencePower = ServerToBattleUtility.ServerValueToInt(commonMonsterData.defense);
		int specialAttackPower = ServerToBattleUtility.ServerValueToInt(commonMonsterData.spAttack);
		int specialDefencePower = ServerToBattleUtility.ServerValueToInt(commonMonsterData.spDefense);
		int speed = ServerToBattleUtility.ServerValueToInt(commonMonsterData.speed);
		int maxAttackPower = ServerToBattleUtility.ServerValueToInt(monsterData.monsterM.maxAttack);
		int maxDefencePower = ServerToBattleUtility.ServerValueToInt(monsterData.monsterM.maxDefense);
		int maxSpecialAttackPower = ServerToBattleUtility.ServerValueToInt(monsterData.monsterM.maxSpAttack);
		int maxSpecialDefencePower = ServerToBattleUtility.ServerValueToInt(monsterData.monsterM.maxSpDefense);
		int maxSpeed = ServerToBattleUtility.ServerValueToInt(monsterData.monsterM.speed);
		int level = ServerToBattleUtility.ServerValueToInt(commonMonsterData.level);
		string resistanceId = monsterData.monsterM.resistanceId;
		int luck = ServerToBattleUtility.ServerValueToInt(commonMonsterData.luck);
		string uniqueSkillId = commonMonsterData.uniqueSkillId;
		string commonSkillId = commonMonsterData.commonSkillId;
		string text = commonMonsterData.leaderSkillId.Equals("0") ? string.Empty : commonMonsterData.leaderSkillId;
		string iconId = monsterData.monsterM.iconId;
		TalentLevel hp2 = ServerToBattleUtility.IntToTalentLevel(commonMonsterData.hpAbilityFlg);
		TalentLevel attack = ServerToBattleUtility.IntToTalentLevel(commonMonsterData.attackAbilityFlg);
		TalentLevel defence = ServerToBattleUtility.IntToTalentLevel(commonMonsterData.defenseAbilityFlg);
		TalentLevel specialAttack = ServerToBattleUtility.IntToTalentLevel(commonMonsterData.spAttackAbilityFlg);
		TalentLevel specialDefence = ServerToBattleUtility.IntToTalentLevel(commonMonsterData.spDefenseAbilityFlg);
		TalentLevel speed2 = ServerToBattleUtility.IntToTalentLevel(commonMonsterData.speedAbilityFlg);
		Talent talent = new Talent(hp2, attack, defence, specialAttack, specialDefence, speed2);
		if (!this.cachedTolerances.ContainsKey(resistanceId.Trim()))
		{
			this.cachedTolerances.Add(resistanceId.Trim(), base.stateManager.serverControl.ResistanceToTolerance(resistanceId));
		}
		ToleranceShifter arousalTolerance = base.stateManager.serverControl.GetArousalTolerance(monsterData, this.cachedTolerances[resistanceId]);
		FriendshipStatus friendshipStatus = new FriendshipStatus(friendshipLevel, maxAttackPower, maxDefencePower, maxSpecialAttackPower, maxSpecialDefencePower, maxSpeed);
		List<int> list = new List<int>();
		if (commonMonsterData.chipIdList != null && commonMonsterData.chipIdList.Length > 0)
		{
			foreach (int item in commonMonsterData.chipIdList)
			{
				list.Add(item);
			}
		}
		PlayerStatus result = new PlayerStatus(monsterGroupId, hp, attackPower, defencePower, specialAttackPower, specialDefencePower, speed, level, resistanceId, arousalTolerance, luck, uniqueSkillId, commonSkillId, text, iconId, talent, arousal, friendshipStatus, list.ToArray());
		if (!this.cachedSkillStatuses.ContainsKey(uniqueSkillId.Trim()))
		{
			this.cachedSkillStatuses.Add(uniqueSkillId.Trim(), base.stateManager.serverControl.SkillMToSkillStatus(uniqueSkillId));
		}
		if (!this.cachedSkillStatuses.ContainsKey(commonSkillId.Trim()))
		{
			this.cachedSkillStatuses.Add(commonSkillId.Trim(), base.stateManager.serverControl.SkillMToSkillStatus(commonSkillId));
		}
		if (!this.cachedLeaderSkillStatuses.ContainsKey(text.Trim()) && !text.Equals(string.Empty))
		{
			GameWebAPI.RespDataMA_GetSkillM.SkillM value;
			this.cachedLeaderSkillStatuses.Add(text.Trim(), base.stateManager.serverControl.SkillMToLeaderSkillStatus(text, out value));
			this.cachedLeaderSkillMs.Add(text.Trim(), value);
		}
		if (!this.cachedCharacterData.ContainsKey(monsterGroupId.Trim()))
		{
			this.cachedCharacterData.Add(monsterGroupId.Trim(), base.stateManager.serverControl.MonsterMToCharacterData(monsterGroupId));
		}
		return result;
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

	public bool IsDisconnected
	{
		get
		{
			return this.isDisconnected;
		}
	}

	protected abstract void TCPDisconnectedCallbackMethod();

	private void SendMessageForTarget(TCPMessageType tcpMessageType, object message, string targetId, bool enableDisconnected = true)
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
		global::Debug.Log("targetId: " + targetId);
		if (this.myTCPUtil == null)
		{
			global::Debug.LogWarning("this.myTCPUtil is null");
		}
		else
		{
			this.myTCPUtil.SendTCPRequest(data, new List<int>
			{
				targetId.ToInt32()
			}, this.myTCPKey);
			global::Debug.LogFormat("[TCPMessage]{0}へ{1}を送信しました.", new object[]
			{
				targetId,
				tcpMessageType
			});
		}
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
		if (enableDisconnected && this.isDisconnected)
		{
			return;
		}
		Dictionary<string, object> dictionary = ClassSingleton<TCPMessageFactory>.Instance.CreateMessage(tcpMessageType, message);
		if (this.multiUsers == null || this.multiUsers.Length == 0)
		{
			return;
		}
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
		string text = string.Join(",", list.ToArray());
		global::Debug.LogFormat("他{0}名のID: {1}", new object[]
		{
			list.Count,
			text
		});
		if (this.myTCPUtil == null || dictionary == null)
		{
			global::Debug.LogWarning("this.myTCPUtil is null");
		}
		else
		{
			List<int> to = list.Select((string s) => s.ToInt32()).ToList<int>();
			this.myTCPUtil.SendTCPRequest(dictionary, to, this.myTCPKey);
			global::Debug.LogFormat("[TCPMessage]{0}へ{1}を送信しました", new object[]
			{
				text,
				tcpMessageType
			});
		}
	}

	protected abstract bool CheckStillAlive(string userId);

	protected MultiBattleData.PvPUserData GetUserData(bool _isMyData = true)
	{
		foreach (MultiBattleData.PvPUserData pvPUserData in ClassSingleton<MultiBattleData>.Instance.PvPUserDatas)
		{
			bool flag = pvPUserData.userStatus.userId == ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId;
			if (flag == _isMyData)
			{
				return pvPUserData;
			}
		}
		return null;
	}

	public abstract void GetBattleData();

	public abstract void InitializeTCPClient();

	protected abstract void TCPCallbackMethod(Dictionary<string, object> arg);

	public abstract IEnumerator WaitAllPlayers(TCPMessageType tcpMessageType);

	public void FinalizeTCP()
	{
		global::Debug.Log("Finalizeされました.");
		Singleton<TCPUtil>.Instance.TCPDisConnect(true);
		Screen.sleepTimeout = -2;
		if (base.onServerConnect)
		{
			ClassSingleton<FaceChatNotificationAccessor>.Instance.faceChatNotification.StartGetHistoryIdList();
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

	public string GetEnemyName()
	{
		string text = this.multiUsers.Where((MultiBattleData.PvPUserData user) => user.userStatus.userId != ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId).Select((MultiBattleData.PvPUserData user) => user.userStatus.nickname).FirstOrDefault<string>();
		if (string.IsNullOrEmpty(text))
		{
			return "-";
		}
		return text;
	}

	protected abstract IEnumerable<string> GetOtherUsersId();

	public virtual void OnSkillTrigger()
	{
		if (base.battleStateData.isShowMenuWindow)
		{
			return;
		}
		if (base.battleStateData.isShowRevivalWindow)
		{
			return;
		}
		NGUITools.SetActiveSelf(base.stateManager.battleUiComponents.skillSelectUi.gameObject, false);
		base.battleStateData.isPossibleTargetSelect = false;
	}

	protected void SendConfirmation(TCPMessageType tcpMessageType, string targetId)
	{
		this.SendConfirmation(tcpMessageType, targetId, true);
	}

	protected void SendConfirmationDisconnected(TCPMessageType tcpMessageType, string targetId)
	{
		this.SendConfirmation(tcpMessageType, targetId, false);
	}

	private void SendConfirmation(TCPMessageType tcpMessageType, string targetId, bool enableDisconnected)
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
			tcpMessageType = tcpMessageType.ToInteger()
		};
		AppCoroutine.Start(this.SendMessageInsistentlyForTarget<ConfirmationData>(TCPMessageType.Confirmation, message, targetId, enableDisconnected), false);
	}

	protected IEnumerator SendMessageInsistentlyForTarget<T>(TCPMessageType tcpMessageType, TCPData<T> message, string targetId, bool enableDisconnected = true) where T : class
	{
		int waitingCount = 0;
		for (;;)
		{
			global::Debug.LogFormat("[confirmationChecks] tcpMessageType:{0}, confirmationChecks count:{1}, targetId:{2}, waitingCount:{3}", new object[]
			{
				tcpMessageType,
				this.confirmationChecks[tcpMessageType].Count,
				targetId,
				waitingCount
			});
			this.SendMessageForTarget(tcpMessageType, message, targetId, enableDisconnected);
			waitingCount++;
			IEnumerator wait = Util.WaitForRealTime(1f);
			while (wait.MoveNext())
			{
				object obj = wait.Current;
				yield return obj;
			}
			global::Debug.Log("Util.WaitForRealTime 経過");
			int count = Mathf.Min(this.otherUserCount, 1);
			if (this.confirmationChecks[tcpMessageType].Count >= count)
			{
				break;
			}
			yield return null;
		}
		this.confirmationChecks[tcpMessageType] = new List<string>();
		yield break;
		yield break;
	}

	protected string[] SetWorldDungeonExtraEffect(string dungeonId)
	{
		IEnumerable<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM> enumerable = MasterDataMng.Instance().RespDataMA_WorldDungeonExtraEffectM.worldDungeonExtraEffectM.SelectMany((GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM x) => MasterDataMng.Instance().RespDataMA_WorldDungeonExtraEffectManageM.worldDungeonExtraEffectManageM, (GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM x, GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectManageM.WorldDungeonExtraEffectManageM y) => new
		{
			x,
			y
		}).Where(<>__TranspIdent1 => <>__TranspIdent1.x.worldDungeonExtraEffectId == <>__TranspIdent1.y.worldDungeonExtraEffectId && <>__TranspIdent1.y.worldDungeonId == dungeonId).Select(<>__TranspIdent1 => <>__TranspIdent1.x);
		this.cachedExtraEffectStatus.Clear();
		int num = 0;
		string[] array = new string[enumerable.Count<GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM>()];
		foreach (GameWebAPI.RespDataMA_GetWorldDungeonExtraEffectM.WorldDungeonExtraEffectM worldDungeonExtraEffectM in enumerable)
		{
			this.cachedExtraEffectStatus.Add(num.ToString(), new ExtraEffectStatus(worldDungeonExtraEffectM));
			array[num] = num.ToString();
			num++;
		}
		return array;
	}

	public abstract IEnumerator SendMessageInsistently<T>(TCPMessageType tcpMessageType, TCPData<T> message, float waitingTerm = 1f) where T : class;

	protected IEnumerator Exception(Func<bool, IEnumerator> function)
	{
		this.exceptionCount++;
		global::Debug.Log("exceptionCount " + this.exceptionCount);
		IEnumerator wait = function(this.exceptionCount < 3);
		while (wait != null && wait.MoveNext())
		{
			yield return wait.Current;
		}
		if (this.myTCPUtil.CheckTCPConnection())
		{
			this.exceptionCount = 0;
		}
		yield break;
	}

	protected virtual void CheckMasterData()
	{
	}

	protected void ShowErrorLog(string variableName)
	{
		global::Debug.LogError(string.Format("マスタの{0}の値がおかしいです.", variableName));
	}

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
