using BattleStateMachineInternal;
using JsonFx.Json;
using MultiBattle.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public sealed class TCPMessageSender : Singleton<TCPMessageSender>
{
	private const float sendingWaitingTerm = 2f;

	private const float memberWaitingTerm = 1f;

	private const float waitingWinTimeEnoughPassed = 5f;

	private const float waitingWinFinalTimeout = 15f;

	private const float waitingRetireTimeout = 5f;

	private float waitWinTime;

	private float waitBattleStartTime;

	private float waitBattleEndTime;

	private float waitOnlineCheckTime;

	private float waitRoomResumeTime;

	private float waitRetireTime;

	public bool IsWinnerWaitOver { get; set; }

	public bool IsRetireWaitOver { private get; set; }

	public bool IsPvPBattleStart { private get; set; }

	public bool IsPvPBattleEnd { private get; set; }

	public bool IsPvPOnlineCheck { private get; set; }

	public bool IsPvPRecoverCommunicateCheck { private get; set; }

	public bool IsMultiBattleResume { private get; set; }

	public bool IsEnoughTimePassedWin
	{
		get
		{
			return this.waitWinTime >= 5f;
		}
	}

	public bool IsFinalTimeoutForWin
	{
		get
		{
			return this.waitWinTime >= 15f;
		}
	}

	public bool IsFinalTimeoutForRetire
	{
		get
		{
			return this.waitRetireTime >= 5f;
		}
	}

	public IEnumerator SendWinnerOwner(CharacterStateControl[] playerCharacters, string[] userMonsterIds, int startId, int clearFlag, List<int> ogis)
	{
		this.IsWinnerWaitOver = false;
		this.waitWinTime = 0f;
		Dictionary<string, object> data = this.CreateResultData(playerCharacters, userMonsterIds, startId, clearFlag, ogis);
		while (!this.IsWinnerWaitOver && !(Singleton<TCPUtil>.Instance == null))
		{
			Singleton<TCPUtil>.Instance.SendTCPRequest(data, "activityList");
			(data["820102"] as BattleResult).requestStatus = 1;
			IEnumerator wait = Util.WaitForRealTime(2f);
			while (wait.MoveNext())
			{
				object obj = wait.Current;
				yield return obj;
			}
			this.waitWinTime += 2f;
		}
		yield break;
		yield break;
	}

	public IEnumerator CountWinnerMember()
	{
		this.IsWinnerWaitOver = false;
		this.waitWinTime = 0f;
		while (!this.IsWinnerWaitOver)
		{
			IEnumerator wait = Util.WaitForRealTime(1f);
			while (wait.MoveNext())
			{
				object obj = wait.Current;
				yield return obj;
			}
			this.waitWinTime += 1f;
			Debug.LogFormat("waitWinTime:{0}", new object[]
			{
				this.waitWinTime
			});
		}
		yield break;
		yield break;
	}

	public IEnumerator CountTimeOutMember()
	{
		this.IsRetireWaitOver = false;
		this.waitRetireTime = 0f;
		while (!this.IsRetireWaitOver)
		{
			IEnumerator wait = Util.WaitForRealTime(1f);
			while (wait.MoveNext())
			{
				object obj = wait.Current;
				yield return obj;
			}
			this.waitRetireTime += 1f;
			Debug.LogFormat("waitRetireTime:{0}", new object[]
			{
				this.waitRetireTime
			});
		}
		yield break;
		yield break;
	}

	public IEnumerator SendRetireOwner(CharacterStateControl[] playerCharacters, string[] userMonsterIds, int startId, int clearFlag, List<int> ogis)
	{
		this.IsRetireWaitOver = false;
		this.waitRetireTime = 0f;
		Dictionary<string, object> data = this.CreateResultData(playerCharacters, userMonsterIds, startId, clearFlag, ogis);
		while (!this.IsRetireWaitOver && !(Singleton<TCPUtil>.Instance == null))
		{
			Singleton<TCPUtil>.Instance.SendTCPRequest(data, "activityList");
			(data["820102"] as BattleResult).requestStatus = 1;
			IEnumerator wait = Util.WaitForRealTime(2f);
			while (wait.MoveNext())
			{
				object obj = wait.Current;
				yield return obj;
			}
			this.waitRetireTime += 2f;
		}
		yield break;
		yield break;
	}

	public IEnumerator SendPvPBattleStart()
	{
		this.IsPvPBattleStart = false;
		this.waitBattleStartTime = 0f;
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("080108", string.Empty);
		while (!this.IsPvPBattleStart && !(Singleton<TCPUtil>.Instance == null))
		{
			Singleton<TCPUtil>.Instance.SendTCPRequest(data, "activityList");
			IEnumerator wait = Util.WaitForRealTime(2f);
			while (wait.MoveNext())
			{
				object obj = wait.Current;
				yield return obj;
			}
			this.waitBattleStartTime += 2f;
		}
		yield break;
		yield break;
	}

	public IEnumerator SendPvPBattleEnd(int battle_result, int wave_count, List<int> use_inheritance_skill)
	{
		this.IsPvPBattleEnd = false;
		this.waitBattleEndTime = 0f;
		Dictionary<string, object> data = new Dictionary<string, object>();
		PvPBattleEnd message = new PvPBattleEnd
		{
			battle_result = battle_result,
			wave_count = wave_count,
			use_inheritance_skill = use_inheritance_skill
		};
		data.Add("080109", message);
		while (!this.IsPvPBattleEnd && !(Singleton<TCPUtil>.Instance == null))
		{
			Singleton<TCPUtil>.Instance.SendTCPRequest(data, "activityList");
			IEnumerator wait = Util.WaitForRealTime(2f);
			while (wait.MoveNext())
			{
				object obj = wait.Current;
				yield return obj;
			}
			this.waitBattleEndTime += 2f;
		}
		yield break;
		yield break;
	}

	public IEnumerator SendPvPOnlineCheck()
	{
		this.IsPvPOnlineCheck = false;
		this.waitOnlineCheckTime = 0f;
		Dictionary<string, object> data = new Dictionary<string, object>();
		PvPOnlineCheck message = new PvPOnlineCheck
		{
			uniqueRequestId = Singleton<TCPUtil>.Instance.GetUniqueRequestId()
		};
		data.Add("080110", message);
		while (!this.IsPvPOnlineCheck && !(Singleton<TCPUtil>.Instance == null))
		{
			Singleton<TCPUtil>.Instance.SendTCPRequest(data, "activityList");
			IEnumerator wait = Util.WaitForRealTime(2f);
			while (wait.MoveNext())
			{
				object obj = wait.Current;
				yield return obj;
			}
			this.waitOnlineCheckTime += 2f;
		}
		yield break;
		yield break;
	}

	public IEnumerator SendPvPRecoverCommunicate()
	{
		this.IsPvPRecoverCommunicateCheck = false;
		this.waitOnlineCheckTime = 0f;
		Dictionary<string, object> data = new Dictionary<string, object>();
		PvPBattleRecover message = new PvPBattleRecover
		{
			isMockBattle = ((!(ClassSingleton<MultiBattleData>.Instance.MockBattleUserCode == "0")) ? 1 : 0),
			uniqueRequestId = Singleton<TCPUtil>.Instance.GetUniqueRequestId()
		};
		data.Add("080112", message);
		while (!this.IsPvPRecoverCommunicateCheck && !(Singleton<TCPUtil>.Instance == null))
		{
			Singleton<TCPUtil>.Instance.SendTCPRequest(data, "activityList");
			IEnumerator wait = Util.WaitForRealTime(2f);
			while (wait.MoveNext())
			{
				object obj = wait.Current;
				yield return obj;
			}
			this.waitOnlineCheckTime += 2f;
		}
		yield break;
		yield break;
	}

	public IEnumerator SendMultiBattleResume()
	{
		this.IsMultiBattleResume = false;
		this.waitRoomResumeTime = 0f;
		Dictionary<string, object> data = new Dictionary<string, object>();
		string multiRoomId = DataMng.Instance().RespData_WorldMultiStartInfo.multiRoomId;
		MultiMemberResume message = new MultiMemberResume
		{
			ri = multiRoomId
		};
		data.Add("820106", message);
		while (!this.IsMultiBattleResume && !(Singleton<TCPUtil>.Instance == null))
		{
			Singleton<TCPUtil>.Instance.SendTCPRequest(data, "activityList");
			IEnumerator wait = Util.WaitForRealTime(2f);
			while (wait.MoveNext())
			{
				object obj = wait.Current;
				yield return obj;
			}
			this.waitRoomResumeTime += 2f;
		}
		yield break;
		yield break;
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
				Debug.LogFormat("[死亡したデジモン]hp:{0}, isEnemy:{1}, id:{2}", new object[]
				{
					playerCharacters[j].hp,
					playerCharacters[j].isEnemy,
					userMonsterIds[j]
				});
			}
		}
		string[] value = list.Select((int i) => i.ToString()).ToArray<string>();
		string text = string.Join(",", value);
		Debug.LogFormat("生存モンスターID:{0}", new object[]
		{
			text
		});
		string[] value2 = ogis.Select((int i) => i.ToString()).Distinct<string>().ToArray<string>();
		string text2 = string.Join(",", value2);
		Debug.LogFormat("生存ユーザー:{0}", new object[]
		{
			text2
		});
		string multiRoomId = DataMng.Instance().RespData_WorldMultiStartInfo.multiRoomId;
		int num = multiRoomId.ToInt32() + "820102".ToInt32();
		Debug.LogFormat("clearFlag{0}, uniqueRequestId:{1}", new object[]
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
}
