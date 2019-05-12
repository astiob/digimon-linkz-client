using BattleStateMachineInternal;
using Master;
using MultiBattle.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleMultiFunction : BattleMultiBasicFunction
{
	private List<string> recoverMembers = new List<string>();

	private bool isWentFarm;

	private string mustOpenMemberFailedDialogMessage = string.Empty;

	private List<string> failedMembers = new List<string>();

	private Dictionary<string, int> liveCheckers;

	private List<RevivalData> revivalDataList = new List<RevivalData>();

	private Dictionary<string, int> pManager = new Dictionary<string, int>();

	private bool isReconnect;

	private bool isSendConnectionRecover;

	public bool isMyTurn { get; private set; }

	protected override string myTCPKey
	{
		get
		{
			return "multiBattle";
		}
	}

	protected override int otherUserCount
	{
		get
		{
			return this.GetOtherUsersId().Count<string>();
		}
	}

	public bool isOwnerOnly
	{
		get
		{
			return this.otherUserCount == 0;
		}
	}

	public string startId
	{
		get
		{
			return base.stateManager.serverControl._startId;
		}
		set
		{
			base.stateManager.serverControl._startId = value;
		}
	}

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

	public void RevivalCancel(string userId)
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

	private void ResetLiveCheckers()
	{
		this.liveCheckers = new Dictionary<string, int>();
		this.GetOtherUsersId().ToList<string>().ForEach(delegate(string userId)
		{
			this.liveCheckers[userId] = 0;
		});
	}

	private void SendPing()
	{
		PingData message = new PingData
		{
			playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
			hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.Ping, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, TCPMessageType.None)
		};
		this.liveCheckers.ToList<KeyValuePair<string, int>>().ForEach(delegate(KeyValuePair<string, int> item)
		{
			this.liveCheckers[item.Key] = 0;
			this.SendMessageForTarget(TCPMessageType.Ping, message, item.Key);
		});
	}

	public int GetPNNum(int index)
	{
		string userId = this.multiUsers[index].userStatus.userId;
		if (this.pManager.ContainsKey(userId))
		{
			return this.pManager[userId];
		}
		return 0;
	}

	private void RefreshDeadDigimonIcon(int index)
	{
		BattleStateManager.current.uiControl.ApplyMonsterButtonEnable(index, false, true);
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
		IEnumerable<int> myIndices = this.GetMyIndices();
		string text = string.Join(",", myIndices.Select((int x) => x.ToString()).ToArray<string>());
		global::Debug.LogFormat("自分のデジモンのインデックス:[{0}]", new object[]
		{
			text
		});
		this.pManager.Clear();
		int num = 0;
		for (int i = 0; i < base.battleStateData.playerCharacters.Length; i++)
		{
			bool isSelect = i == base.CurrentEnemyMyIndex;
			base.stateManager.uiControl.ApplyMonsterButtonEnable(i, isSelect, base.battleStateData.playerCharacters[i].isDied);
			string userId = this.multiUsers[i].userStatus.userId;
			if (!this.pManager.ContainsKey(userId))
			{
				this.pManager[userId] = num;
				num++;
			}
			bool flag = myIndices.Contains(i);
			int playerIndex = this.pManager[userId];
			string playerName = string.Empty;
			if (flag)
			{
				playerName = StringMaster.GetString("BattleUI-25");
			}
			else if (ClassSingleton<MultiBattleData>.Instance.MultiUsers.Length <= i)
			{
				playerName = ClassSingleton<MultiBattleData>.Instance.MultiUsers.FirstOrDefault((MultiUser user) => user.isOwner).userName;
			}
			else
			{
				playerName = ClassSingleton<MultiBattleData>.Instance.MultiUsers[i].userName;
			}
			base.stateManager.uiControlMulti.SetPlayerNumToMonsterButton(i, playerIndex, playerName);
		}
	}

	public BattleStateData GetBattleStateData()
	{
		return base.battleStateData;
	}

	protected override void CheckMasterData()
	{
		if (ConstValue.MULTI_BATTLE_TIMEOUT_TIME == 0)
		{
			base.ShowErrorLog("multiBattleTimeoutTime");
		}
	}

	public override void GetBattleData()
	{
		this.CheckMasterData();
		GameWebAPI.RespData_WorldMultiStartInfo respData_WorldMultiStartInfo = DataMng.Instance().RespData_WorldMultiStartInfo;
		this.startId = respData_WorldMultiStartInfo.startId;
		DataMng.Instance().WD_ReqDngResult.startId = this.startId;
		DataMng.Instance().WD_ReqDngResult.dungeonId = respData_WorldMultiStartInfo.worldDungeonId;
		base.cachedSkillStatuses.Add(base.stateManager.publicAttackSkillId.Trim(), base.stateManager.serverControl.SkillMToSkillStatus(base.stateManager.publicAttackSkillId));
		int num = ClassSingleton<MultiBattleData>.Instance.MultiUsers.Length;
		if (num < 2)
		{
			global::Debug.LogWarning("ユーザが2人未満.");
		}
		int num2 = 3;
		string[] playerUserMonsterIds = ClassSingleton<MultiBattleData>.Instance.PlayerUserMonsterIds;
		int num3 = playerUserMonsterIds.Length;
		if (num3 < num2)
		{
			global::Debug.LogWarningFormat("monsterが{0}匹未満.", new object[]
			{
				num2
			});
		}
		base.userMonsterIds = new string[num2];
		GameWebAPI.Common_MonsterData[] array = new GameWebAPI.Common_MonsterData[num2];
		for (int i = 0; i < respData_WorldMultiStartInfo.party.Length; i++)
		{
			GameWebAPI.RespData_WorldMultiStartInfo.Party party = respData_WorldMultiStartInfo.party[i];
			for (int j = 0; j < party.userMonsters.Length; j++)
			{
				for (int k = 0; k < base.userMonsterIds.Length; k++)
				{
					GameWebAPI.Common_MonsterData common_MonsterData = party.userMonsters[j];
					if (playerUserMonsterIds[k] == common_MonsterData.userMonsterId)
					{
						base.userMonsterIds[k] = common_MonsterData.userMonsterId;
						array[k] = common_MonsterData;
					}
				}
			}
		}
		int num4 = array.Where((GameWebAPI.Common_MonsterData monster) => monster == null).Count<GameWebAPI.Common_MonsterData>();
		if (num4 > 0)
		{
			global::Debug.LogWarning("NULLが入ってる.");
		}
		if (array.Length != num2)
		{
			global::Debug.LogWarning("デジモンが３体ではない.");
		}
		global::Debug.LogFormat("各デジモンのuserMonsterId:", new object[]
		{
			string.Join(",", base.userMonsterIds)
		});
		for (int l = 0; l < base.hierarchyData.usePlayerCharactersId.Length; l++)
		{
			base.hierarchyData.usePlayerCharactersId[l] = l.ToString();
			GameWebAPI.Common_MonsterData commonMonsterData = array[l];
			base.cachedPlayerStatus.Add(l, base.ConvertAPIParamsToPlayerStatus(commonMonsterData));
		}
		GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM worldDungeonM = base.stateManager.serverControl.GetWorldDungeonM(respData_WorldMultiStartInfo.worldDungeonId);
		base.hierarchyData.useStageId = worldDungeonM.background;
		base.hierarchyData.areaName = worldDungeonM.name;
		base.hierarchyData.limitRound = int.Parse(worldDungeonM.limitRound);
		base.hierarchyData.speedClearRound = int.Parse(worldDungeonM.speedClearRound);
		GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM[] worldStageM = MasterDataMng.Instance().RespDataMA_WorldStageM.worldStageM;
		foreach (GameWebAPI.RespDataMA_GetWorldStageM.WorldStageM worldStageM2 in worldStageM)
		{
			if (worldDungeonM.worldStageId == worldStageM2.worldStageId)
			{
				base.hierarchyData.areaId = worldStageM2.worldAreaId.ToInt32();
				break;
			}
		}
		base.hierarchyData.extraEffectsId = base.SetWorldDungeonExtraEffect(worldDungeonM.worldDungeonId);
		base.hierarchyData.isPossibleContinue = (int.Parse(worldDungeonM.canContinue) == 1);
		base.hierarchyData.battleNum = worldDungeonM.battleNum;
		base.hierarchyData.batteWaves = base.stateManager.serverControl.DungeonFloorToBattleWave(respData_WorldMultiStartInfo.dungeonFloor, respData_WorldMultiStartInfo.worldDungeonId);
		base.hierarchyData.digiStoneNumber = DataMng.Instance().GetStone();
		base.battleStateData.beforeConfirmDigiStoneNumber = base.stateManager.hierarchyData.digiStoneNumber;
		base.hierarchyData.playerPursuitPercentage = ServerToBattleUtility.PermillionToPercentage(respData_WorldMultiStartInfo.criticalRate.partyCriticalRate);
		base.hierarchyData.enemyPursuitPercentage = ServerToBattleUtility.PermillionToPercentage(respData_WorldMultiStartInfo.criticalRate.enemyCriticalRate);
		string[] array3 = new string[base.cachedLeaderSkillMs.Keys.Count];
		base.cachedLeaderSkillMs.Keys.CopyTo(array3, 0);
		base.hierarchyData.useInitialIntroduction = false;
	}

	protected override bool CheckStillAlive(string userId)
	{
		return this.multiUsers.Any((MultiBattleData.PvPUserData user) => user.userStatus.userId == userId);
	}

	private MultiBattleData.PvPUserData GetPlayerByUserId(string userId)
	{
		return this.multiUsers.FirstOrDefault((MultiBattleData.PvPUserData user) => user.userStatus.userId == userId);
	}

	private MultiBattleData.PvPUserData GetOwnerPlayer()
	{
		return this.multiUsers.FirstOrDefault((MultiBattleData.PvPUserData user) => user.isOwner);
	}

	protected override IEnumerable<string> GetOtherUsersId()
	{
		return this.multiUsers.Where((MultiBattleData.PvPUserData user) => user.userStatus.userId != ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId).Select((MultiBattleData.PvPUserData user) => user.userStatus.userId).Distinct<string>();
	}

	private List<int> GetMultiUserIdsList()
	{
		return this.multiUsers.Select((MultiBattleData.PvPUserData user) => user.userStatus.userId.ToInt32()).ToList<int>();
	}

	private List<int> GetMultiOtherUserIdsList()
	{
		return this.multiUsers.Where((MultiBattleData.PvPUserData user) => user.userStatus.userId != ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId).Select((MultiBattleData.PvPUserData user) => user.userStatus.userId.ToInt32()).ToList<int>();
	}

	public bool IsMe(int monsterIndex)
	{
		bool res = false;
		IEnumerable<int> myIndices = this.GetMyIndices();
		myIndices.ToList<int>().ForEach(delegate(int myIndex)
		{
			if (monsterIndex == myIndex)
			{
				res = true;
			}
		});
		return res;
	}

	public override void InitializeTCPClient()
	{
		Screen.sleepTimeout = -1;
		this.myTCPUtil = Singleton<TCPUtil>.Instance;
		this.myTCPUtil.MakeTCPClient();
		this.myTCPUtil.battleMode = BattleMode.Multi;
		bool flag = this.myTCPUtil.ConnectTCPServer(ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId);
		if (flag)
		{
			if (this.isReconnect)
			{
				AppCoroutine.Start(this.SendReconnection(), false);
			}
			else
			{
				this.isDisconnected = false;
			}
			this.isReconnect = true;
		}
		else
		{
			global::Debug.LogWarning("TCP接続 failed.");
			this.isDisconnected = true;
			this.ShowDisconnectTCPDialog(null);
		}
		this.myTCPUtil.SetTCPCallBackMethod(new Action<Dictionary<string, object>>(this.TCPCallbackMethod));
		this.myTCPUtil.SetOnExitCallBackMethod(new Action(this.TCPDisconnectedCallbackMethod));
		this.myTCPUtil.SetExceptionMethod(new Action<short, string>(this.ExceptionCallbackMethod));
		global::Debug.Log("Initialized TCP");
	}

	private IEnumerator SendReconnection()
	{
		global::Debug.Log("メンバー復帰処理(820106)を送信.");
		if (!base.onServerConnect || Singleton<TCPUtil>.Instance == null || Singleton<TCPMessageSender>.Instance == null)
		{
			global::Debug.Log("シミュレータでは行わない.");
			yield break;
		}
		IEnumerator wait = Singleton<TCPMessageSender>.Instance.SendMultiBattleResume();
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		yield break;
	}

	protected override void TCPDisconnectedCallbackMethod()
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
			base.StartCoroutine(this.Reconnect(true));
		}
		else
		{
			base.StartCoroutine(base.Exception(delegate(bool retry)
			{
				if (retry)
				{
					global::Debug.Log("無理やり再接続.");
					return this.Reconnect(true);
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

	public IEnumerator Reconnect(bool isDialog = true)
	{
		yield return new WaitForEndOfFrame();
		this.InitializeTCPClient();
		if (isDialog)
		{
			base.StartCoroutine(this.HidingCountDown(this.GetOwnerPlayer().userStatus.userId));
		}
		yield break;
	}

	protected override void TCPCallbackMethod(Dictionary<string, object> arg)
	{
		if (arg.ContainsKey("820102"))
		{
			this.ManageBattleResult(arg["820102"]);
			return;
		}
		if (arg.ContainsKey("800012"))
		{
			this.ManageFailedPlayer(arg["800012"]);
			return;
		}
		if (arg.ContainsKey("820106"))
		{
			this.ManageBattleResume(arg["820106"]);
			return;
		}
		if (!arg.ContainsKey("multiBattle"))
		{
			global::Debug.LogWarningFormat("{0} is not valid key.", new object[]
			{
				arg.Keys.First<string>()
			});
			return;
		}
		Dictionary<object, object> dictionary = arg["multiBattle"] as Dictionary<object, object>;
		string text = dictionary.Keys.First<object>().ToString();
		TCPMessageType tcpMessageType = MultiTools.StringToEnum<TCPMessageType>(text);
		object messageObj = dictionary[text];
		this.RunRecieverPlayerActions(tcpMessageType, messageObj);
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
					this.isDisconnected = true;
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
				base.SendConfirmation(TCPMessageType.Retire, ownerPlayer.userStatus.userId);
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
					Singleton<TCPMessageSender>.Instance.IsWinnerWaitOver = true;
				};
				base.SendConfirmation(TCPMessageType.BattleResult, ownerPlayer.userStatus.userId);
			}
		}
	}

	private int BATTLE_TIMEOUT_TIME
	{
		get
		{
			if (!BattleStateManager.current.onServerConnect)
			{
				return 15;
			}
			return ConstValue.MULTI_BATTLE_TIMEOUT_TIME;
		}
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
			int num = (int)(Time.realtimeSinceStartup - this.pausedTime);
			if (num >= this.BATTLE_TIMEOUT_TIME)
			{
				global::Debug.LogErrorFormat("{0}秒経ったので負け.", new object[]
				{
					num
				});
				this.ShowDisconnectTCPDialog(null);
				return;
			}
			base.StartCoroutine(this.Reconnect(true));
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
		if (string.IsNullOrEmpty(text) || text != this.startId)
		{
			global::Debug.LogWarning("ありえないキー.");
			return;
		}
		if (string.IsNullOrEmpty(text2) || text2 == ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId)
		{
			global::Debug.LogWarning("ありえないキー.");
			return;
		}
		MultiBattleData.PvPUserData playerByUserId = this.GetPlayerByUserId(text2);
		if (playerByUserId == null)
		{
			global::Debug.LogWarning("既にいないユーザ.");
			return;
		}
		this.isDisconnected = true;
		base.StartCoroutine(this.HidingCountDown(playerByUserId.userStatus.userId));
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
				GameWebAPI.RespData_MultiRoomStatusInfoLogic.StatusInfo info = null;
				IEnumerator runRoomStatusInfo = this.RunRoomStatusInfo(delegate(GameWebAPI.RespData_MultiRoomStatusInfoLogic.StatusInfo data)
				{
					info = data;
				});
				while (runRoomStatusInfo.MoveNext())
				{
					object obj = runRoomStatusInfo.Current;
					yield return obj;
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
					object obj2 = function.Current;
					yield return obj2;
				}
			}
			else
			{
				IEnumerator function2 = this.WaitAllPlayersDisconnected(TCPMessageType.ConnectionRecover);
				while (function2.MoveNext())
				{
					object obj3 = function2.Current;
					yield return obj3;
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

	private void ManageBattleResume(object messageObj)
	{
		Singleton<TCPMessageSender>.Instance.IsMultiBattleResume = true;
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
			if (this.isDisconnected)
			{
				return;
			}
			this.RecieveEnemyTurnSync(tcpMessageType, messageObj);
			break;
		case TCPMessageType.RandomSeedSync:
			if (this.isDisconnected)
			{
				return;
			}
			this.RecieveRandomSeedSync(tcpMessageType, messageObj);
			break;
		case TCPMessageType.Emotion:
			if (this.isDisconnected)
			{
				return;
			}
			this.RecieveEmotion(tcpMessageType, messageObj);
			break;
		case TCPMessageType.X2:
			if (this.isDisconnected)
			{
				return;
			}
			this.RecieveX2(tcpMessageType, messageObj);
			break;
		case TCPMessageType.Attack:
			if (this.isDisconnected)
			{
				return;
			}
			this.RecieveAttack(tcpMessageType, messageObj);
			break;
		case TCPMessageType.Revival1:
		case TCPMessageType.Revival2:
		case TCPMessageType.Revival3:
			if (this.isDisconnected)
			{
				return;
			}
			this.RecieveRevival(tcpMessageType, messageObj);
			break;
		case TCPMessageType.Continue:
			if (this.isDisconnected)
			{
				return;
			}
			this.RecieveContinue(tcpMessageType, messageObj);
			break;
		case TCPMessageType.Confirmation:
			this.RecieveConfirmation(tcpMessageType, messageObj);
			break;
		case TCPMessageType.LastConfirmation:
			this.RecieveLastConfirmation(tcpMessageType, messageObj);
			break;
		case TCPMessageType.Ping:
			if (this.isDisconnected)
			{
				return;
			}
			this.RecievePing(tcpMessageType, messageObj);
			break;
		case TCPMessageType.Pong:
			if (this.isDisconnected)
			{
				return;
			}
			this.RecievePong(tcpMessageType, messageObj);
			break;
		case TCPMessageType.Targat:
			if (this.isDisconnected)
			{
				return;
			}
			this.RecieveTarget(tcpMessageType, messageObj);
			break;
		case TCPMessageType.RevivalCancel:
			if (this.isDisconnected)
			{
				return;
			}
			this.RecieveRevivalCancel(tcpMessageType, messageObj);
			break;
		case TCPMessageType.ConnectionRecover:
			this.RecieveConnectionRecover(tcpMessageType, messageObj);
			break;
		case TCPMessageType.LeaderChange:
			base.RecieveLeaderChange(tcpMessageType, messageObj);
			break;
		}
	}

	private void RecieveEnemyTurnSync(TCPMessageType tcpMessageType, object messageObj)
	{
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
		if (this.CheckOverlap(enemyTurnSyncData.playerUserId, enemyTurnSyncData.hashValue))
		{
			return;
		}
		this.lastAction[TCPMessageType.EnemyTurnSync] = delegate()
		{
			this.recieveChecks[TCPMessageType.EnemyTurnSync] = true;
		};
		base.SendConfirmation(tcpMessageType, enemyTurnSyncData.playerUserId);
	}

	private void RecieveRandomSeedSync(TCPMessageType tcpMessageType, object messageObj)
	{
		global::Debug.Log("RandomSeedSync: 受信");
		if (base.stateManager.rootState.currentState.GetType() != typeof(SubStateWaitRandomSeedSync))
		{
			return;
		}
		RandomSeedSyncData randomSeedSyncData = TCPData<RandomSeedSyncData>.Convert(messageObj);
		if (this.CheckOverlap(randomSeedSyncData.playerUserId, randomSeedSyncData.hashValue))
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
		base.SendConfirmation(tcpMessageType, randomSeedSyncData.playerUserId);
	}

	private void RecieveEmotion(TCPMessageType tcpMessageType, object messageObj)
	{
		global::Debug.Log("Emotion: 受信");
		EmotionData emotionData = TCPData<EmotionData>.Convert(messageObj);
		int emotionType = emotionData.emotionType;
		int iconSpritesIndex = emotionData.iconSpritesIndex;
		bool isOther = true;
		base.stateManager.uiControlMulti.ShowEmotion(iconSpritesIndex, emotionType, isOther);
	}

	private void RecieveX2(TCPMessageType tcpMessageType, object messageObj)
	{
		global::Debug.Log("X2: 受信");
		X2Data x2Data = TCPData<X2Data>.Convert(messageObj);
		if (this.CheckOverlap(x2Data.playerUserId, x2Data.hashValue))
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
		base.SendConfirmation(tcpMessageType, x2Data.playerUserId);
	}

	private void RecieveAttack(TCPMessageType tcpMessageType, object messageObj)
	{
		global::Debug.Log("Attack: 受信");
		if (base.CurrentEnemyMyIndex == -1 || base.stateManager.rootState.currentState.GetType() != typeof(SubStateMultiWaitEnemySkillSelect))
		{
			global::Debug.LogWarning("まだ準備していない");
			return;
		}
		AttackData attackData = TCPData<AttackData>.Convert(messageObj);
		if (this.CheckOverlap(attackData.playerUserId, attackData.hashValue))
		{
			return;
		}
		this.lastAction[TCPMessageType.Attack] = delegate()
		{
			CharacterStateControl characterStateControl = this.battleStateData.playerCharacters[this.CurrentEnemyMyIndex];
			int targetIdx = attackData.targetIdx;
			int selectSkillIdx = attackData.selectSkillIdx;
			this.stateManager.uiControlMulti.StartSharedAPAnimation();
			global::Debug.LogFormat("currentCharactersMyIndex: {0}, targetIdx: {1}, selectSkillIdx:{2}", new object[]
			{
				this.CurrentEnemyMyIndex,
				targetIdx,
				selectSkillIdx
			});
			if (attackData.isTargetCharacterEnemy)
			{
				characterStateControl.targetCharacter = this.battleStateData.enemies[targetIdx];
			}
			else
			{
				characterStateControl.targetCharacter = this.battleStateData.playerCharacters[targetIdx];
			}
			characterStateControl.targetCharacter.myIndex = targetIdx;
			characterStateControl.isSelectSkill = selectSkillIdx;
			this.battleStateData.onSkillTrigger = true;
			this.recieveChecks[TCPMessageType.Attack] = true;
		};
		base.SendConfirmation(tcpMessageType, attackData.playerUserId);
	}

	private void RecieveTarget(TCPMessageType tcpMessageType, object messageObj)
	{
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

	private void RecieveContinue(TCPMessageType tcpMessageType, object messageObj)
	{
		global::Debug.Log("Continue: 受信");
		if (base.battleStateData.isContinueFlag || base.stateManager.battleScreen != BattleScreen.Continue)
		{
			return;
		}
		base.battleStateData.isShowRevivalWindow = false;
		ContinueData continueData = TCPData<ContinueData>.Convert(messageObj);
		if (this.CheckOverlap(continueData.playerUserId, continueData.hashValue))
		{
			return;
		}
		this.lastAction[TCPMessageType.Continue] = delegate()
		{
			string playerUserId = continueData.playerUserId;
			int digiStone = continueData.digiStone;
			this.battleStateData.isContinueFlag = true;
			this.recieveChecks[TCPMessageType.Continue] = true;
		};
		base.SendConfirmation(tcpMessageType, continueData.playerUserId);
	}

	private void RecieveRevival(TCPMessageType tcpMessageType, object messageObj)
	{
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
		if (base.CheckHash(revivalData.hashValue))
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
		base.SendConfirmation(myRevivalType, revivalData.playerUserId);
	}

	private void RecieveRevivalCancel(TCPMessageType tcpMessageType, object messageObj)
	{
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
		if (base.CheckHash(revivalData.hashValue))
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
		base.SendConfirmation(TCPMessageType.RevivalCancel, revivalData.playerUserId);
	}

	private void RecieveConfirmation(TCPMessageType tcpMessageType, object messageObj)
	{
		ConfirmationData confirmationData = TCPData<ConfirmationData>.Convert(messageObj);
		if (this.isDisconnected && confirmationData.tcpMessageType != 26)
		{
			return;
		}
		global::Debug.Log("Confirmation: 受信");
		if (this.CheckOverlap(confirmationData.playerUserId, confirmationData.hashValue))
		{
			return;
		}
		this.confirmationChecks[(TCPMessageType)confirmationData.tcpMessageType].Add(confirmationData.playerUserId);
		global::Debug.LogFormat("confirmationChecks : {0}から確認用{1}を受信しました. Count:{2}", new object[]
		{
			confirmationData.playerUserId,
			(TCPMessageType)confirmationData.tcpMessageType,
			this.confirmationChecks[(TCPMessageType)confirmationData.tcpMessageType].Count
		});
	}

	private void RecieveLastConfirmation(TCPMessageType tcpMessageType, object messageObj)
	{
		LastConfirmationData lastConfirmationData = TCPData<LastConfirmationData>.Convert(messageObj);
		if (this.isDisconnected && lastConfirmationData.tcpMessageType != 26)
		{
			return;
		}
		global::Debug.Log("LastConfirmation: 受信");
		if (this.CheckOverlap(lastConfirmationData.playerUserId, lastConfirmationData.hashValue))
		{
			global::Debug.LogFormat("2度防止 tcpMessageType:{0}", new object[]
			{
				(TCPMessageType)lastConfirmationData.tcpMessageType
			});
			return;
		}
		if (lastConfirmationData.failedPlayerUserIds.Length > 0)
		{
			foreach (string item in lastConfirmationData.failedPlayerUserIds)
			{
				if (!this.failedMembers.Contains(item))
				{
					this.failedMembers.Add(item);
				}
			}
		}
		this.confirmationChecks[TCPMessageType.Confirmation].Add(lastConfirmationData.playerUserId);
		base.RunLastAction((TCPMessageType)lastConfirmationData.tcpMessageType);
		global::Debug.LogFormat("tcpMessageType:{0}", new object[]
		{
			(TCPMessageType)lastConfirmationData.tcpMessageType
		});
	}

	private void RecievePing(TCPMessageType tcpMessageType, object messageObj)
	{
		global::Debug.Log("Ping: 受信");
		PingData pingData = TCPData<PingData>.Convert(messageObj);
		string playerUserId = pingData.playerUserId;
		global::Debug.Log(playerUserId);
		PongData message = new PongData
		{
			playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
			hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.Pong, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, TCPMessageType.None)
		};
		this.SendMessageForTarget(TCPMessageType.Pong, message, playerUserId);
	}

	private void RecievePong(TCPMessageType tcpMessageType, object messageObj)
	{
		global::Debug.Log("Pong: 受信");
		PongData pongData = TCPData<PongData>.Convert(messageObj);
		if (this.liveCheckers != null && this.liveCheckers.ContainsKey(pongData.playerUserId))
		{
			Dictionary<string, int> dictionary2;
			Dictionary<string, int> dictionary = dictionary2 = this.liveCheckers;
			string playerUserId;
			string key = playerUserId = pongData.playerUserId;
			int num = dictionary2[playerUserId];
			dictionary[key] = num + 1;
		}
	}

	private void RecieveConnectionRecover(TCPMessageType tcpMessageType, object messageObj)
	{
		global::Debug.Log("ConnectionRecover: 受信");
		ConnectionRecoverData connectionRecoverData = TCPData<ConnectionRecoverData>.Convert(messageObj);
		if (base.CheckHash(connectionRecoverData.hashValue))
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
		base.SendConfirmationDisconnected(TCPMessageType.ConnectionRecover, connectionRecoverData.playerUserId);
	}

	private bool CheckOverlap(string playerUserId, string hashValue)
	{
		bool flag = base.CheckHash(hashValue);
		bool flag2 = this.CheckStillAlive(playerUserId);
		if (flag || !flag2)
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

	private void SendMessageForTarget(TCPMessageType tcpMessageType, object message, string targetId)
	{
		global::Debug.Log("SendMessageForTarget");
		if (this.isDisconnected)
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
			}, "multiBattle");
			global::Debug.LogFormat("[TCPMessage]{0}へ{1}を送信しました.", new object[]
			{
				targetId,
				tcpMessageType
			});
		}
	}

	public override IEnumerator SendMessageInsistently<T>(TCPMessageType tcpMessageType, TCPData<T> message, float waitingTerm = 1f)
	{
		IEnumerator function = this.SendMessageInsistently<T>(tcpMessageType, message, waitingTerm, true, 60f);
		while (function.MoveNext())
		{
			object obj = function.Current;
			yield return obj;
		}
		yield break;
	}

	private IEnumerator SendMessageInsistentlyDisconnected<T>(TCPMessageType tcpMessageType, TCPData<T> message, float waitingTerm = 1f) where T : class
	{
		IEnumerator function = this.SendMessageInsistently<T>(tcpMessageType, message, waitingTerm, false, 15f);
		while (function.MoveNext())
		{
			object obj = function.Current;
			yield return obj;
		}
		yield break;
	}

	private IEnumerator SendMessageInsistently<T>(TCPMessageType tcpMessageType, TCPData<T> message, float waitingTerm, bool enableDisconnected, float maxWaitingCount) where T : class
	{
		global::Debug.Log("SendMessageInsistently");
		int waitingCount = 0;
		for (;;)
		{
			while (enableDisconnected && this.isDisconnected)
			{
				yield return null;
			}
			if (enableDisconnected)
			{
				base.SendMessageForSync(tcpMessageType, message);
			}
			else
			{
				base.SendMessageForSyncDisconnected(tcpMessageType, message);
			}
			global::Debug.LogFormat("残りの人数:{0}/{1}, tcpMessageType{2}, waitingCount:{3}, sent:{4}", new object[]
			{
				this.confirmationChecks[tcpMessageType].Count,
				this.otherUserCount,
				tcpMessageType,
				waitingCount,
				string.Join(",", this.confirmationChecks[tcpMessageType].ToArray())
			});
			IEnumerator wait = Util.WaitForRealTime(waitingTerm);
			while (wait.MoveNext())
			{
				object obj = wait.Current;
				yield return obj;
			}
			waitingCount++;
			bool isTimeOut = (float)waitingCount >= maxWaitingCount;
			bool isConfirm = this.confirmationChecks[tcpMessageType].Count == this.otherUserCount;
			int failedCount = 0;
			foreach (string otherUsersId in this.GetOtherUsersId())
			{
				foreach (string failedMember in this.failedMembers)
				{
					if (otherUsersId == failedMember)
					{
						failedCount++;
					}
				}
			}
			bool isConfirmAndFailedMember = this.otherUserCount - this.confirmationChecks[tcpMessageType].Count == failedCount;
			if (isTimeOut || isConfirmAndFailedMember)
			{
				IEnumerator checkFailedPlayerAndShowDialog = this.CheckFailedPlayerAndShowDialog(tcpMessageType);
				while (checkFailedPlayerAndShowDialog.MoveNext())
				{
					object obj2 = checkFailedPlayerAndShowDialog.Current;
					yield return obj2;
				}
			}
			if (isConfirm || isConfirmAndFailedMember || isTimeOut)
			{
				break;
			}
			yield return null;
		}
		LastConfirmationData lastConfirmationMessage = new LastConfirmationData
		{
			playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
			hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.LastConfirmation, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, tcpMessageType),
			tcpMessageType = tcpMessageType.ToInteger(),
			failedPlayerUserIds = this.failedMembers.Distinct<string>().ToArray<string>()
		};
		if (enableDisconnected)
		{
			base.SendMessageForSync(TCPMessageType.LastConfirmation, lastConfirmationMessage);
		}
		else
		{
			base.SendMessageForSyncDisconnected(TCPMessageType.LastConfirmation, lastConfirmationMessage);
		}
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
		return this.WaitAllPlayers(tcpMessageType, false, 25f);
	}

	private IEnumerator WaitAllPlayers(TCPMessageType tcpMessageType, bool enableDisconnected, float maxWaitingCount)
	{
		global::Debug.LogFormat("{0}の通信待ち.", new object[]
		{
			tcpMessageType
		});
		string userId = string.Empty;
		bool isOwner = false;
		if (tcpMessageType == TCPMessageType.RandomSeedSync || tcpMessageType == TCPMessageType.EnemyTurnSync || tcpMessageType == TCPMessageType.Retire || tcpMessageType == TCPMessageType.Continue || tcpMessageType == TCPMessageType.RevivalCancel || tcpMessageType == TCPMessageType.ConnectionRecover)
		{
			MultiBattleData.PvPUserData activePlayer = this.GetOwnerPlayer();
			userId = activePlayer.userStatus.userId;
			isOwner = activePlayer.isOwner;
		}
		else if (tcpMessageType == TCPMessageType.Attack)
		{
			MultiBattleData.PvPUserData activePlayer2 = this.GetPlayerByUserId(this.multiUsers[base.CurrentEnemyMyIndex].userStatus.userId);
			userId = activePlayer2.userStatus.userId;
			isOwner = activePlayer2.isOwner;
		}
		int waitingCount = 0;
		for (;;)
		{
			while (enableDisconnected && this.isDisconnected)
			{
				yield return null;
			}
			bool isTimeOut = (float)waitingCount >= maxWaitingCount;
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
					object obj2 = check.Current;
					yield return obj2;
				}
			}
			if (this.recieveChecks[tcpMessageType] || isTimeOut || senderFailed)
			{
				break;
			}
			yield return null;
		}
		foreach (string failedMember2 in this.failedMembers)
		{
			if (userId != failedMember2)
			{
				IEnumerator check2 = this.DisconnectAction(failedMember2, false, null);
				while (check2.MoveNext())
				{
					object obj3 = check2.Current;
					yield return obj3;
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

	public override IEnumerator SendAttack()
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
		IEnumerator wait = this.SendMessageInsistently<AttackData>(TCPMessageType.Attack, message, 1f);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		base.battleStateData.isPossibleTargetSelect = true;
		yield break;
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

	public override void OnSkillTrigger()
	{
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
		if (!this.isOwnerOnly)
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

	public IEnumerator Send2xSpeedPlay()
	{
		X2Data message = new X2Data
		{
			playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
			hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.X2, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, TCPMessageType.None),
			on2x = !base.hierarchyData.on2xSpeedPlay,
			onPose = base.battleStateData.isShowMenuWindow
		};
		IEnumerator wait = this.SendMessageInsistently<X2Data>(TCPMessageType.X2, message, 1f);
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

	public void SendRetire(Action callback = null)
	{
		if (base.userMonsterIds != null)
		{
			this.RunRetire(true, callback);
		}
		else
		{
			this.GotoFarm(true, callback);
		}
	}

	public IEnumerator SendTimeOut()
	{
		if (base.IsOwner)
		{
			IEnumerator coroutine = this.RunRetireByOwner(ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, null);
			while (coroutine.MoveNext())
			{
				yield return null;
			}
		}
		else
		{
			IEnumerator coroutine2 = Singleton<TCPMessageSender>.Instance.CountTimeOutMember();
			while (coroutine2.MoveNext())
			{
				while (this.isDisconnected)
				{
					yield return null;
				}
				if (Singleton<TCPMessageSender>.Instance.IsFinalTimeoutForRetire)
				{
					this.ShowDisconnectOwnerDialog(null);
					for (;;)
					{
						yield return null;
					}
				}
				else
				{
					yield return coroutine2.Current;
				}
			}
		}
		yield break;
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
		IEnumerator wait = this.SendMessageInsistently<ContinueData>(TCPMessageType.Continue, message, 1f);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		yield break;
	}

	public bool isSendCharacterRevival { get; private set; }

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
		IEnumerator wait = this.SendMessageInsistently<RevivalData>(revivalType, message, 1f);
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
		IEnumerator wait = this.SendMessageInsistently<RevivalCancelData>(TCPMessageType.RevivalCancel, message, 1f);
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

	public IEnumerator SendClearResult(Action callback = null)
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
				IEnumerator reconnect = this.Reconnect(false);
				while (reconnect.MoveNext())
				{
					object obj = reconnect.Current;
					yield return obj;
				}
			}
			if (!this.myTCPUtil.CheckTCPConnection())
			{
				global::Debug.LogWarning("SendClearResult : CheckTCPConnection");
				this.ShowDisconnectTCPDialog(null);
				yield break;
			}
		}
		if (base.IsOwner)
		{
			IEnumerator coroutine = Singleton<TCPMessageSender>.Instance.SendWinnerOwner(base.battleStateData.playerCharacters, base.userMonsterIds, this.startId.ToInt32(), 1, this.GetMultiOtherUserIdsList());
			while (coroutine.MoveNext())
			{
				while (this.isDisconnected)
				{
					yield return null;
				}
				if (Singleton<TCPMessageSender>.Instance.IsFinalTimeoutForWin || this.confirmationChecks[TCPMessageType.BattleResult].Count >= this.otherUserCount)
				{
					global::Debug.Log("SendMessageForSync TCPMessageType.BattleResult");
					LastConfirmationData lastConfirmationMessage = new LastConfirmationData
					{
						playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
						hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.LastConfirmation, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, TCPMessageType.BattleResult),
						tcpMessageType = TCPMessageType.BattleResult.ToInteger(),
						failedPlayerUserIds = this.failedMembers.Distinct<string>().ToArray<string>()
					};
					base.SendMessageForSync(TCPMessageType.LastConfirmation, lastConfirmationMessage);
					IEnumerator wait = Util.WaitForRealTime(0.5f);
					while (wait.MoveNext())
					{
						object obj2 = wait.Current;
						yield return obj2;
					}
					Singleton<TCPMessageSender>.Instance.IsWinnerWaitOver = true;
				}
				yield return coroutine.Current;
			}
		}
		else
		{
			IEnumerator coroutine2 = Singleton<TCPMessageSender>.Instance.CountWinnerMember();
			while (coroutine2.MoveNext())
			{
				while (this.isDisconnected)
				{
					yield return null;
				}
				if (Singleton<TCPMessageSender>.Instance.IsFinalTimeoutForWin)
				{
					this.ShowDisconnectOwnerDialog(null);
					for (;;)
					{
						yield return null;
					}
				}
				else
				{
					yield return coroutine2.Current;
				}
			}
		}
		if (callback != null)
		{
			callback();
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
		IEnumerator wait = this.SendMessageInsistentlyDisconnected<ConnectionRecoverData>(TCPMessageType.ConnectionRecover, message, 1f);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		UnityEngine.Random.seed = seed;
		this.ConnectionRecover(failedUserIds);
		yield break;
	}

	private void ConnectionRecover(string[] failedUserIds)
	{
		global::Debug.Log("再開.");
		this.isDisconnected = false;
		foreach (string userId in failedUserIds)
		{
			MultiBattleData.PvPUserData playerByUserId = this.GetPlayerByUserId(userId);
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

	public override void RoundStartInitMulti()
	{
		base.CurrentEnemyMyIndex = -1;
	}

	public void Initialize()
	{
		base.battleStateData.isEnableBackKeySystem = false;
		if (this.isWentFarm)
		{
			return;
		}
		this.RoundStartInitMulti();
		this.recieveChecks = new Dictionary<TCPMessageType, bool>();
		this.confirmationChecks = new Dictionary<TCPMessageType, List<string>>();
		foreach (object obj in Enum.GetValues(typeof(TCPMessageType)))
		{
			TCPMessageType key = (TCPMessageType)((int)obj);
			this.recieveChecks[key] = false;
			this.confirmationChecks[key] = new List<string>();
		}
		MultiBattleData instance = ClassSingleton<MultiBattleData>.Instance;
		string myPlayerUserId = instance.MyPlayerUserId;
		base.IsOwner = instance.MultiUsers.Any((MultiUser item) => item.userId == myPlayerUserId && item.isOwner);
		int num = instance.MultiUsers.Count<MultiUser>();
		if (num == 3)
		{
			this.multiUsers = new MultiBattleData.PvPUserData[instance.MultiUsers.Length];
			for (int i = 0; i < instance.MultiUsers.Length; i++)
			{
				MultiBattleData.PvPUserData pvPUserData = new MultiBattleData.PvPUserData();
				pvPUserData.isOwner = instance.MultiUsers[i].isOwner;
				pvPUserData.userStatus = new GameWebAPI.ColosseumUserStatus();
				pvPUserData.userStatus.userId = instance.MultiUsers[i].userId;
				pvPUserData.userStatus.nickname = instance.MultiUsers[i].userName;
				pvPUserData.monsterData = null;
				this.multiUsers[i] = pvPUserData;
			}
		}
		else
		{
			List<MultiUser> list = instance.MultiUsers.ToList<MultiUser>();
			MultiUser item2 = instance.MultiUsers.FirstOrDefault((MultiUser item) => item.isOwner);
			for (int j = 0; j < 3 - num; j++)
			{
				list.Add(item2);
			}
			this.multiUsers = new MultiBattleData.PvPUserData[list.Count<MultiUser>()];
			for (int k = 0; k < list.Count<MultiUser>(); k++)
			{
				MultiBattleData.PvPUserData pvPUserData2 = new MultiBattleData.PvPUserData();
				pvPUserData2.isOwner = list[k].isOwner;
				pvPUserData2.userStatus = new GameWebAPI.ColosseumUserStatus();
				pvPUserData2.userStatus.userId = list[k].userId;
				pvPUserData2.userStatus.nickname = list[k].userName;
				pvPUserData2.monsterData = null;
				this.multiUsers[k] = pvPUserData2;
			}
			if (num < 2)
			{
				global::Debug.LogWarningFormat("ユーザは最低{0}人必要です.", new object[]
				{
					2
				});
			}
		}
		if (!ClassSingleton<MultiBattleData>.Instance.IsSimulator)
		{
			ClassSingleton<MultiBattleData>.Instance.MaxAttackTime = ConstValue.MULTI_MAX_ATTACK_TIME;
			ClassSingleton<MultiBattleData>.Instance.HurryUpAttackTime = ConstValue.MULTI_HURRYUP_ATTACK_TIME;
		}
		base.stateManager.onApplicationPause.Add(new Action<bool>(this.OnApplicationPause));
	}

	public void InitializeMultiAfter()
	{
		base.stateManager.uiControlMulti.InitializeSharedAp();
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
		IEnumerable<int> myIndices = this.GetMyIndices();
		this.isMyTurn = myIndices.Any((int i) => i == base.CurrentEnemyMyIndex);
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
			foreach (int num2 in myIndices.ToArray<int>())
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

	private void ShowDisconnectTCPDialog(Action callback = null)
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

	private void GotoFarm(bool isRetire = false, Action callback = null)
	{
		global::Debug.LogFormat("[GotoFarm]isRetire:{0}", new object[]
		{
			isRetire
		});
		if (this.isWentFarm)
		{
			global::Debug.Log("二度禁止(isWentFarm)");
		}
		this.isWentFarm = true;
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

	private void RunRetire(bool isWaiting, Action callback = null)
	{
		if (isWaiting)
		{
			base.StartCoroutine(this.RunRetireByOwner(ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, callback));
		}
		else
		{
			this.RunRetireAfter(callback);
		}
	}

	private IEnumerator RunRetireByOwner(string retiredPlayerId, Action callback)
	{
		IEnumerator ownerCor = Singleton<TCPMessageSender>.Instance.SendRetireOwner(base.battleStateData.playerCharacters, base.userMonsterIds, this.startId.ToInt32(), 0, this.GetMultiOtherUserIdsList());
		while (ownerCor.MoveNext())
		{
			if (this.confirmationChecks[TCPMessageType.Retire].Count >= this.otherUserCount)
			{
				global::Debug.Log("SendMessageForSync TCPMessageType.Retire");
				while (this.isDisconnected)
				{
					yield return null;
				}
				LastConfirmationData lastConfirmationMessage = new LastConfirmationData
				{
					playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
					hashValue = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.LastConfirmation, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, TCPMessageType.Retire),
					tcpMessageType = TCPMessageType.Retire.ToInteger(),
					failedPlayerUserIds = this.failedMembers.Distinct<string>().ToArray<string>()
				};
				base.SendMessageForSync(TCPMessageType.LastConfirmation, lastConfirmationMessage);
				IEnumerator wait = Util.WaitForRealTime(0.5f);
				while (wait.MoveNext())
				{
					object obj = wait.Current;
					yield return obj;
				}
				this.RunRetireAfter(callback);
				Singleton<TCPMessageSender>.Instance.IsRetireWaitOver = true;
				yield break;
			}
			if (Singleton<TCPMessageSender>.Instance.IsFinalTimeoutForRetire)
			{
				this.RunRetireAfter(callback);
				Singleton<TCPMessageSender>.Instance.IsRetireWaitOver = true;
				yield break;
			}
			yield return ownerCor.Current;
		}
		yield break;
	}

	private void RunRetireAfter(Action callback = null)
	{
		base.battleStateData.isShowRetireWindow = false;
		this.GotoFarm(true, callback);
		base.stateManager.soundManager.VolumeBGM *= 2f;
	}

	private enum DisconnectDialogType
	{
		TCP,
		Owner,
		Member,
		OwnerRetire
	}
}
