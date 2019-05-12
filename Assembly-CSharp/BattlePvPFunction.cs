using BattleStateMachineInternal;
using Master;
using MultiBattle.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityExtension;

public sealed class BattlePvPFunction : BattleMultiBasicFunction
{
	private const int retireDialogTerm = 10;

	private bool isAlreadyLose;

	private bool isAlreadyFinish;

	private int tryRecoverCount;

	private bool isReconnect;

	private bool isSendRetired;

	private bool isErrorLoseRun;

	public static bool isAlreadyLoseBeforeBattle;

	public bool isRetireDialog { get; set; }

	public bool isSynced { private get; set; }

	public bool isSyncFinished { private get; set; }

	protected override int otherUserCount
	{
		get
		{
			return 1;
		}
	}

	private bool FinishTCPForcely { get; set; }

	public IEnumerator SendReconnection()
	{
		global::Debug.Log("通信復帰（080112）を送信.");
		if (!base.onServerConnect || Singleton<TCPUtil>.Instance == null || Singleton<TCPMessageSender>.Instance == null || this.isAlreadyLose || this.isAlreadyFinish)
		{
			global::Debug.LogFormat("isAlreadyLose:{0}, isAlreadyFinish:{1}.", new object[]
			{
				this.isAlreadyLose,
				this.isAlreadyFinish
			});
			global::Debug.Log(Singleton<TCPMessageSender>.Instance);
			global::Debug.Log(Singleton<TCPUtil>.Instance);
			yield break;
		}
		IEnumerator wait = Singleton<TCPMessageSender>.Instance.SendPvPRecoverCommunicate();
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		yield break;
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

	public void FinishTCP()
	{
		this.FinishTCPForcely = true;
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

	public void Initialize()
	{
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
		base.IsOwner = instance.PvPUserDatas.Where((MultiBattleData.PvPUserData item) => item.userStatus.userId == myPlayerUserId).Select((MultiBattleData.PvPUserData item) => item.isOwner).First<bool>();
		int num = instance.PvPUserDatas.Count<MultiBattleData.PvPUserData>();
		if (num == 2)
		{
			this.multiUsers = instance.PvPUserDatas;
		}
		else
		{
			global::Debug.LogWarningFormat("ユーザは{0}人必要です.", new object[]
			{
				2
			});
		}
		base.stateManager.onApplicationPause.Add(new Action<bool>(this.OnApplicationPause));
	}

	public IEnumerator SendRetire()
	{
		this.isSendRetired = true;
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
		this.FinishTCPForcely = false;
		IEnumerator wait = this.SendMessageInsistently<RetireData>(TCPMessageType.Retire, message, 1f);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		yield break;
	}

	public void StopSomething()
	{
		this.isAlreadyFinish = true;
		this.HideAlertTimerDialog();
		base.stateManager.uiControlPvP.HideSyncWait();
		base.stateManager.uiControlPvP.StopAttackTimer();
		base.stateManager.uiControlPvP.HideEmotionButton();
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
			if (num >= ConstValue.PVP_BATTLE_TIMEOUT_TIME)
			{
				global::Debug.LogErrorFormat("{0}秒経ったので負け.", new object[]
				{
					num
				});
				this.RunLoseAlert();
				return;
			}
			if (this.tryRecoverCount >= 3)
			{
				global::Debug.LogErrorFormat("{0}回復帰しようとしたので負け.", new object[]
				{
					3
				});
				this.RunLoseAlert();
				return;
			}
			this.InitializeTCPClient();
		}
	}

	private void ManageFailedPlayer(object messageObj)
	{
		global::Debug.LogError("誰か落ちた");
		string valueByKey = MultiTools.GetValueByKey<string>(messageObj, "resultCode");
		global::Debug.LogFormat("resultCode :{0}", new object[]
		{
			valueByKey
		});
		if (!this.isSyncFinished)
		{
			return;
		}
		this.ShowEnemyFailedDialog();
	}

	private MultiBattleData.PvPUserData GetPlayerByUserId(string userId)
	{
		return this.multiUsers.Where((MultiBattleData.PvPUserData user) => user.userStatus.userId == userId).FirstOrDefault<MultiBattleData.PvPUserData>();
	}

	private void ShowMyFailedDialog()
	{
		BattleUIControlPvP uiControlPvP = base.stateManager.uiControlPvP;
		uiControlPvP.StartEnemyFailedTimer(delegate
		{
			this.HideAlertTimerDialog();
			global::Debug.LogError("自分が落ちた(時間経過)");
			this.RunLoseAlert();
		}, BattleUIControlPvP.DialogType.MyCount);
	}

	private void ShowEnemyFailedDialog()
	{
		if (this.isSendRetired || this.isAlreadyFinish || this.isAlreadyLose)
		{
			global::Debug.LogFormat("isSendRetired :{0}, isAlreadyFinish:{1}, isAlreadyLose:{2}", new object[]
			{
				this.isSendRetired,
				this.isAlreadyFinish,
				this.isAlreadyLose
			});
			return;
		}
		if (this.isSendRetired)
		{
			base.StartCoroutine(this.JudgeByServer());
			return;
		}
		base.stateManager.uiControlPvP.HideRetireWindow();
		BattleUIControlPvP uiControlPvP = base.stateManager.uiControlPvP;
		uiControlPvP.StartEnemyFailedTimer(delegate
		{
			this.HideAlertTimerDialog();
			global::Debug.LogError("相手が切れた(時間経過)");
		}, BattleUIControlPvP.DialogType.EnemyCount);
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
			base.stateManager.uiControlPvP.HideSkillSelectUI();
			this.FinishTCP();
			this.StopSomething();
			base.stateManager.events.CallWinEvent();
		}
		else if (valueByKey == 1)
		{
			Singleton<TCPMessageSender>.Instance.IsPvPOnlineCheck = true;
			this.HideAlertTimerDialog();
		}
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
			if (this.isAlreadyFinish || this.isAlreadyLose)
			{
				global::Debug.Log("既に勝敗決定済み.");
				return;
			}
			this.pausedTime = Time.realtimeSinceStartup;
			this.isPaused = true;
			this.ShowMyFailedDialog();
		}
		else if (exitCode == 720)
		{
			global::Debug.LogError("タイムアウト 負け.");
			this.RunLoseAlert();
		}
		else if (exitCode == 750 || exitCode == 700 || exitCode == 711 || exitCode == 752 || exitCode == 760)
		{
			global::Debug.Log("無理やり再接続.");
			if (this.isAlreadyLose || this.isAlreadyFinish)
			{
				global::Debug.LogFormat("isAlreadyLose:{0}, isAlreadyFinish:{1}.", new object[]
				{
					this.isAlreadyLose,
					this.isAlreadyFinish
				});
				return;
			}
			this.ShowMyFailedDialog();
			base.StartCoroutine(this.Reconnect());
		}
		else
		{
			base.StartCoroutine(base.Exception(delegate(bool retry)
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
				this.RunLoseAlert();
				return null;
			}));
		}
	}

	public IEnumerator Reconnect()
	{
		yield return new WaitForEndOfFrame();
		if (this.isAlreadyLose || this.isAlreadyFinish)
		{
			global::Debug.LogFormat("isAlreadyLose:{0}, isAlreadyFinish:{1}", new object[]
			{
				this.isAlreadyLose,
				this.isAlreadyFinish
			});
			yield break;
		}
		this.InitializeTCPClient();
		yield break;
	}

	private void RunLoseAlert()
	{
		global::Debug.Log("RunLoseAlert.");
		if (this.isAlreadyLose || this.isAlreadyFinish)
		{
			global::Debug.LogFormat("isAlreadyLose:{0}, isAlreadyFinish:{1}", new object[]
			{
				this.isAlreadyLose,
				this.isAlreadyFinish
			});
			return;
		}
		global::Debug.Log("isAlreadyLose set true.");
		this.isAlreadyLose = true;
		this.StopSomething();
		this.HideAlertTimerDialog();
		base.stateManager.uiControlPvP.HideSkillSelectUI();
		base.stateManager.uiControlPvP.HideRetireWindow();
		this.FinishTCPForcely = false;
		base.stateManager.uiControlPvP.ShowAlertDialog(BattleUIControlPvP.DialogType.Lose, StringMaster.GetString("BattleUI-24"), StringMaster.GetString("SystemButtonClose"), delegate
		{
			base.stateManager.uiControlPvP.HideSyncWait();
			this.HideAlertTimerDialog();
			global::Debug.LogFormat("isSynced:{0}", new object[]
			{
				this.isSynced
			});
			if (this.isSynced)
			{
				base.stateManager.events.CallConnectionErrorEvent();
			}
			else
			{
				this.isErrorLoseRun = true;
			}
		}, true, 10);
		base.stateManager.uiControlPvP.BlockNewDialog();
	}

	private string GetEnemyId()
	{
		return this.multiUsers.Where((MultiBattleData.PvPUserData user) => user.userStatus.userId != ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId).Select((MultiBattleData.PvPUserData user) => user.userStatus.userId).FirstOrDefault<string>();
	}

	private BattleWave[] DungeonFloorToBattleWave(string worldDungeonId)
	{
		GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM worldDungeonM = base.stateManager.serverControl.GetWorldDungeonM(worldDungeonId);
		BattleWave[] array = new BattleWave[1];
		MultiBattleData.PvPUserData userData = base.GetUserData(false);
		if (userData == null)
		{
			global::Debug.LogError("PvPUserData null");
		}
		for (int i = 0; i < array.Length; i++)
		{
			BattleWave battleWave = new BattleWave();
			battleWave.useEnemiesId = new string[userData.monsterData.Length];
			battleWave.enemiesBossFlag = new bool[battleWave.useEnemiesId.Length];
			for (int j = 0; j < userData.monsterData.Length; j++)
			{
				battleWave.useEnemiesId[j] = ((i + 1) * 10 + j).ToString();
				battleWave.enemiesBossFlag[j] = false;
				base.cachedPlayerStatus.Add((i + 1) * 10 + j, base.ConvertAPIParamsToPlayerStatus(userData.monsterData[j]));
			}
			if (BoolExtension.AllMachValue(false, battleWave.enemiesBossFlag))
			{
				battleWave.bgmId = worldDungeonM.bgm;
			}
			else
			{
				battleWave.bgmId = worldDungeonM.bossBgm;
				battleWave.bgmChangeHpPercentage = ServerToBattleUtility.PermillionToPercentage(worldDungeonM.exBossBgmCondition);
				battleWave.changedBgmId = worldDungeonM.exBossBgm;
			}
			array[i] = battleWave;
		}
		return array;
	}

	private void ManageRecoverCommunicate(object messageObj)
	{
		int valueByKey = MultiTools.GetValueByKey<int>(messageObj, "resultCode");
		global::Debug.LogFormat("[通信復帰（080112）]result_code:{0}", new object[]
		{
			valueByKey
		});
		if (valueByKey == 1 || valueByKey == 6)
		{
			global::Debug.LogFormat("[{0}]復帰処理成功.", new object[]
			{
				"080112"
			});
			Singleton<TCPMessageSender>.Instance.IsPvPRecoverCommunicateCheck = true;
		}
		else if (valueByKey == 2)
		{
			global::Debug.LogFormat("[{0}]対戦相手勝利処理済み.", new object[]
			{
				"080112"
			});
			Singleton<TCPMessageSender>.Instance.IsPvPRecoverCommunicateCheck = true;
			this.HideAlertTimerDialog();
			this.RunLoseAlert();
		}
		else if (valueByKey == 3)
		{
			global::Debug.LogFormat("[{0}]対戦相手が敗北処理済み.", new object[]
			{
				"080112"
			});
			Singleton<TCPMessageSender>.Instance.IsPvPRecoverCommunicateCheck = true;
			this.HideAlertTimerDialog();
			this.StopSomething();
			base.stateManager.events.CallWinEvent();
		}
		else if (valueByKey == 4)
		{
			Singleton<TCPMessageSender>.Instance.IsPvPRecoverCommunicateCheck = true;
			this.HideAlertTimerDialog();
			this.BackToTOP();
		}
		else if (valueByKey == 5)
		{
			if (this.isAlreadyLose || this.isAlreadyFinish)
			{
				global::Debug.LogFormat("相手が通信復帰したが無視. isAlreadyLose:{0}, isAlreadyFinish:{1}", new object[]
				{
					this.isAlreadyLose,
					this.isAlreadyFinish
				});
				return;
			}
			Singleton<TCPMessageSender>.Instance.IsPvPRecoverCommunicateCheck = true;
			this.HideAlertTimerDialog();
		}
		else
		{
			global::Debug.LogErrorFormat("ありえないキー:{0}.", new object[]
			{
				valueByKey
			});
		}
	}

	private void BackToTOP()
	{
		string @string = StringMaster.GetString("SaveFailedTitle");
		string string2 = StringMaster.GetString("AlertJsonErrorInfo");
		AlertManager.ButtonActionType actionType = AlertManager.ButtonActionType.Title;
		AlertManager.ShowAlertDialog(delegate(int i)
		{
			GUIMain.BackToTOP("UIStartupCaution", 0.8f, 0.8f);
		}, @string, string2, actionType, false);
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
	}

	private void HideAlertTimerDialog()
	{
		base.stateManager.uiControlPvP.HideAlertDialog();
	}

	private IEnumerator JudgeByServer()
	{
		IEnumerator wait = Singleton<TCPMessageSender>.Instance.SendPvPOnlineCheck();
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		yield break;
	}

	protected override string myTCPKey
	{
		get
		{
			return "pvpBattle";
		}
	}

	public override void RoundStartInitMulti()
	{
		base.CurrentEnemyMyIndex = -1;
	}

	protected override void CheckMasterData()
	{
		if (ConstValue.PVP_BATTLE_TIMEOUT_TIME == 0)
		{
			base.ShowErrorLog("pvpBattleTimeoutTime");
		}
	}

	public override void GetBattleData()
	{
		this.CheckMasterData();
		base.cachedSkillStatuses.Add(base.stateManager.publicAttackSkillId.Trim(), base.stateManager.serverControl.SkillMToSkillStatus(base.stateManager.publicAttackSkillId));
		int num = ClassSingleton<MultiBattleData>.Instance.PvPUserDatas.Length;
		if (num < 2)
		{
			global::Debug.LogWarningFormat("ユーザが{0}人未満.", new object[]
			{
				2
			});
		}
		MultiBattleData.PvPUserData userData = base.GetUserData(true);
		if (userData == null)
		{
			global::Debug.LogError("PvPUserData null");
		}
		int num2 = userData.monsterData.Length;
		if (num2 < 3)
		{
			global::Debug.LogWarningFormat("monsterが{0}匹未満.", new object[]
			{
				3
			});
		}
		base.userMonsterIds = new string[3];
		GameWebAPI.Common_MonsterData[] array = new GameWebAPI.Common_MonsterData[3];
		for (int i = 0; i < num2; i++)
		{
			array[i] = userData.monsterData[i];
		}
		int num3 = array.Where((GameWebAPI.Common_MonsterData monster) => monster == null).Count<GameWebAPI.Common_MonsterData>();
		if (num3 > 0)
		{
			global::Debug.LogWarning("NULLが入ってる.");
		}
		if (array.Length != 3)
		{
			global::Debug.LogWarningFormat("デジモンが{0}体ではない.", new object[]
			{
				3
			});
		}
		global::Debug.LogFormat("各デジモンのuserMonsterId:", new object[]
		{
			string.Join(",", base.userMonsterIds)
		});
		for (int j = 0; j < base.hierarchyData.usePlayerCharactersId.Length; j++)
		{
			global::Debug.LogFormat("usePlayerCharactersId:{0}", new object[]
			{
				j
			});
			base.hierarchyData.usePlayerCharactersId[j] = j.ToString();
			GameWebAPI.Common_MonsterData commonMonsterData = array[j];
			base.cachedPlayerStatus.Add(j, base.ConvertAPIParamsToPlayerStatus(commonMonsterData));
		}
		GameWebAPI.RespDataMA_GetWorldDungeonM.WorldDungeonM worldDungeonM = base.stateManager.serverControl.GetWorldDungeonM(ClassSingleton<MultiBattleData>.Instance.PvPField.worldDungeonId);
		if (worldDungeonM == null)
		{
			global::Debug.LogError("WorldDungeonMのマスターデータにデータがありません.マスターデータを正しく入れてください.");
			return;
		}
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
		base.hierarchyData.isPossibleContinue = (int.Parse(worldDungeonM.canContinue) == 1);
		base.hierarchyData.extraEffectsId = base.SetWorldDungeonExtraEffect(worldDungeonM.worldDungeonId);
		base.hierarchyData.battleNum = worldDungeonM.battleNum;
		base.hierarchyData.batteWaves = this.DungeonFloorToBattleWave(ClassSingleton<MultiBattleData>.Instance.PvPField.worldDungeonId);
		base.hierarchyData.digiStoneNumber = DataMng.Instance().GetStone();
		base.battleStateData.beforeConfirmDigiStoneNumber = base.stateManager.hierarchyData.digiStoneNumber;
		float num4 = 0f;
		base.hierarchyData.playerPursuitPercentage = num4;
		base.hierarchyData.enemyPursuitPercentage = num4;
		string[] array3 = new string[base.cachedLeaderSkillMs.Keys.Count];
		base.cachedLeaderSkillMs.Keys.CopyTo(array3, 0);
		base.hierarchyData.useInitialIntroduction = false;
	}

	public void RunAlreadyLoseEvent()
	{
		if (BattlePvPFunction.isAlreadyLoseBeforeBattle || this.isErrorLoseRun)
		{
			BattlePvPFunction.isAlreadyLoseBeforeBattle = false;
			base.stateManager.events.CallConnectionErrorEvent();
		}
	}

	public override IEnumerator WaitAllPlayers(TCPMessageType tcpMessageType)
	{
		global::Debug.LogFormat("{0}の通信待ち. isErrorLoseRun:{0}", new object[]
		{
			tcpMessageType,
			this.isErrorLoseRun
		});
		int waitingCount = 0;
		for (;;)
		{
			if (waitingCount == 45)
			{
				waitingCount = 0;
				global::Debug.LogError("クライアント判断のタイムアウト.");
				this.ShowEnemyFailedDialog();
			}
			if (base.stateManager.uiControlPvP.IsFailed())
			{
				waitingCount = 0;
				global::Debug.LogError("Time out: サーバーのAPI呼ぶ.");
				IEnumerator judge = this.JudgeByServer();
				while (judge.MoveNext())
				{
					object obj = judge.Current;
					yield return obj;
				}
			}
			if (this.FinishTCPForcely)
			{
				break;
			}
			IEnumerator wait = Util.WaitForRealTime(1f);
			while (wait.MoveNext())
			{
				object obj2 = wait.Current;
				yield return obj2;
			}
			waitingCount++;
			global::Debug.LogFormat("[{0}]waiting ....(waitingCount:{1}) isAlreadyLose:{2}", new object[]
			{
				tcpMessageType,
				waitingCount,
				this.isAlreadyLose
			});
			if (this.recieveChecks[tcpMessageType])
			{
				goto Block_8;
			}
			yield return null;
		}
		global::Debug.LogWarningFormat("[WaitAllPlayers Timeout]tcpMessageType:{0}, waitingCount:{1}. isAlreadyLose:{2}", new object[]
		{
			tcpMessageType,
			waitingCount,
			this.isAlreadyLose
		});
		if (tcpMessageType == TCPMessageType.RandomSeedSync)
		{
			yield break;
		}
		if (tcpMessageType == TCPMessageType.Attack)
		{
			yield break;
		}
		if (tcpMessageType == TCPMessageType.Retire)
		{
			yield break;
		}
		global::Debug.LogError("ありえない.");
		yield break;
		Block_8:
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
		if (this.isAlreadyLose || this.isAlreadyFinish)
		{
			global::Debug.LogFormat("負けてるのでTCP送らない, isAlreadyLose:{0}, isAlreadyFinish:{1}.", new object[]
			{
				this.isAlreadyLose,
				this.isAlreadyFinish
			});
			yield break;
		}
		int waitingCount = 0;
		string createdHash = Singleton<TCPUtil>.Instance.CreateHash(TCPMessageType.LastConfirmation, ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId, tcpMessageType);
		for (;;)
		{
			base.SendMessageForSync(tcpMessageType, message);
			global::Debug.LogFormat("残りの人数:{0}/{1}, tcpMessageType{2}, waitingCount:{3}, sent:{4}", new object[]
			{
				this.confirmationChecks[tcpMessageType].Count,
				this.otherUserCount,
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
				global::Debug.LogError("クライアント判断のタイムアウト.");
				this.ShowEnemyFailedDialog();
			}
			if (base.stateManager.uiControlPvP.IsFailed())
			{
				waitingCount = 0;
				global::Debug.LogError("Time out: サーバーのAPI呼ぶ.");
				IEnumerator judge = this.JudgeByServer();
				while (judge.MoveNext())
				{
					object obj2 = judge.Current;
					yield return obj2;
				}
			}
			if (this.FinishTCPForcely)
			{
				break;
			}
			if (this.confirmationChecks[tcpMessageType].Count == this.otherUserCount)
			{
				goto Block_6;
			}
			yield return null;
		}
		yield break;
		Block_6:
		LastConfirmationData lastConfirmationMessage = new LastConfirmationData
		{
			playerUserId = ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId,
			hashValue = createdHash,
			tcpMessageType = tcpMessageType.ToInteger()
		};
		base.SendMessageForSync(TCPMessageType.LastConfirmation, lastConfirmationMessage);
		this.confirmationChecks[tcpMessageType].Clear();
		yield break;
	}

	public override IEnumerator SendAttack()
	{
		base.stateManager.uiControlPvP.ShowLoading(false);
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
		this.FinishTCPForcely = false;
		IEnumerator wait = this.SendMessageInsistently<AttackData>(TCPMessageType.Attack, message, 1f);
		while (wait.MoveNext())
		{
			object obj = wait.Current;
			yield return obj;
		}
		base.stateManager.uiControlPvP.HideLoading();
		base.battleStateData.isPossibleTargetSelect = true;
		yield break;
	}

	public override void InitializeTCPClient()
	{
		Screen.sleepTimeout = -1;
		this.myTCPUtil = Singleton<TCPUtil>.Instance;
		this.myTCPUtil.MakeTCPClient();
		this.myTCPUtil.battleMode = BattleMode.PvP;
		this.myTCPUtil.SetTCPCallBackMethod(new Action<Dictionary<string, object>>(this.TCPCallbackMethod));
		this.myTCPUtil.SetOnExitCallBackMethod(new Action(this.TCPDisconnectedCallbackMethod));
		this.myTCPUtil.SetExceptionMethod(new Action<short, string>(this.ExceptionCallbackMethod));
		bool flag = this.myTCPUtil.ConnectTCPServer(ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId);
		if (flag)
		{
			this.isDisconnected = false;
			if (this.isReconnect)
			{
				AppCoroutine.Start(this.SendReconnection(), false);
			}
			this.isReconnect = true;
			this.HideAlertTimerDialog();
		}
		else
		{
			global::Debug.LogWarning("TCP接続 failed.");
			this.isDisconnected = true;
		}
		global::Debug.Log("Initialized TCP");
	}

	protected override bool CheckStillAlive(string userId)
	{
		return this.multiUsers.Any((MultiBattleData.PvPUserData user) => user.userStatus.userId == userId);
	}

	protected override void TCPDisconnectedCallbackMethod()
	{
		global::Debug.Log("切断されました.(意図的も含む)");
		this.isDisconnected = true;
	}

	protected override IEnumerable<string> GetOtherUsersId()
	{
		return this.multiUsers.Where((MultiBattleData.PvPUserData user) => user.userStatus.userId != ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId).Select((MultiBattleData.PvPUserData user) => user.userStatus.userId).Distinct<string>();
	}

	protected override void TCPCallbackMethod(Dictionary<string, object> arg)
	{
		if (arg.ContainsKey("800012"))
		{
			this.ManageFailedPlayer(arg["800012"]);
			return;
		}
		if (arg.ContainsKey("080110"))
		{
			this.ManageOnlineCheck(arg["080110"]);
			return;
		}
		if (arg.ContainsKey("080112"))
		{
			this.ManageRecoverCommunicate(arg["080112"]);
			return;
		}
		if (arg.ContainsKey("080114"))
		{
			this.ManageBattleActionLog(arg["080114"]);
			return;
		}
		if (!arg.ContainsKey("pvpBattle"))
		{
			global::Debug.LogWarningFormat("{0} is not valid key.", new object[]
			{
				arg.Keys.First<string>()
			});
			return;
		}
		Dictionary<object, object> dictionary = arg["pvpBattle"] as Dictionary<object, object>;
		string text = dictionary.Keys.First<object>().ToString();
		TCPMessageType tcpMessageType = MultiTools.StringToEnum<TCPMessageType>(text);
		this.RunRecieverPlayerActions(tcpMessageType, dictionary[text]);
	}

	protected override void RunRecieverPlayerActions(TCPMessageType tcpMessageType, object messageObj)
	{
		if (this.isAlreadyLose || this.isAlreadyFinish)
		{
			global::Debug.LogFormat("負けてるので受信しない, isAlreadyLose:{0}, isAlreadyFinish:{1}.", new object[]
			{
				this.isAlreadyLose,
				this.isAlreadyFinish
			});
			return;
		}
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
		{
			global::Debug.Log("RandomSeedSync: 受信");
			if (base.stateManager.rootState.currentState.GetType() != typeof(SubStateWaitRandomSeedSync))
			{
				return;
			}
			RandomSeedSyncData randomSeedSyncData = TCPData<RandomSeedSyncData>.Convert(messageObj);
			bool flag = base.CheckHash(randomSeedSyncData.hashValue);
			bool flag2 = this.CheckStillAlive(randomSeedSyncData.playerUserId);
			bool flag3 = this.GetEnemyId() != randomSeedSyncData.playerUserId;
			if (flag || !flag2 || flag3)
			{
				global::Debug.LogFormat("res:{0}, isStillAlive:{1}", new object[]
				{
					flag,
					flag2
				});
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
			this.HideAlertTimerDialog();
			base.SendConfirmation(tcpMessageType, randomSeedSyncData.playerUserId);
			break;
		}
		case TCPMessageType.Emotion:
			this.RecieveEmotion(tcpMessageType, messageObj);
			break;
		case TCPMessageType.Attack:
		{
			global::Debug.Log("Attack: 受信");
			if (base.CurrentEnemyMyIndex == -1 || base.stateManager.rootState.currentState.GetType() != typeof(SubStateWaitEnemySkillSelect))
			{
				global::Debug.LogWarning("まだ準備していない");
				return;
			}
			AttackData attackData = TCPData<AttackData>.Convert(messageObj);
			bool flag = base.CheckHash(attackData.hashValue);
			bool flag2 = this.CheckStillAlive(attackData.playerUserId);
			string enemyId = this.GetEnemyId();
			bool flag3 = enemyId != attackData.playerUserId;
			if (flag || !flag2 || flag3)
			{
				global::Debug.LogFormat("res:{0}, isStillAlive:{1}", new object[]
				{
					flag,
					flag2
				});
				return;
			}
			global::Debug.LogFormat("攻撃したユーザID: {0},CurrentEnemyMyIndex: {1}, multiUsers.Length: {2}, 待たせているユーザID: {3}", new object[]
			{
				attackData.playerUserId,
				base.CurrentEnemyMyIndex,
				this.multiUsers.Length,
				enemyId
			});
			this.HideAlertTimerDialog();
			this.lastAction[TCPMessageType.Attack] = delegate()
			{
				CharacterStateControl characterStateControl = this.battleStateData.enemies[this.CurrentEnemyMyIndex];
				int targetIdx = attackData.targetIdx;
				int selectSkillIdx = attackData.selectSkillIdx;
				global::Debug.LogFormat("CurrentEnemyMyIndex: {0}, targetIdx: {1}, selectSkillIdx:{2}", new object[]
				{
					this.CurrentEnemyMyIndex,
					targetIdx,
					selectSkillIdx
				});
				if (attackData.isTargetCharacterEnemy)
				{
					characterStateControl.targetCharacter = this.battleStateData.playerCharacters[targetIdx];
				}
				else
				{
					characterStateControl.targetCharacter = this.battleStateData.enemies[targetIdx];
				}
				characterStateControl.targetCharacter.myIndex = targetIdx;
				characterStateControl.isSelectSkill = selectSkillIdx;
				this.battleStateData.onSkillTrigger = true;
				this.recieveChecks[TCPMessageType.Attack] = true;
			};
			base.SendConfirmation(tcpMessageType, attackData.playerUserId);
			break;
		}
		case TCPMessageType.Retire:
		{
			global::Debug.Log("Retire: リタイア");
			RetireData retierData = TCPData<RetireData>.Convert(messageObj);
			bool flag = base.CheckHash(retierData.hashValue);
			bool flag2 = this.CheckStillAlive(retierData.playerUserId);
			if (flag || !flag2)
			{
				global::Debug.LogFormat("res:{0}, isStillAlive:{1}", new object[]
				{
					flag,
					flag2
				});
				return;
			}
			this.lastAction[TCPMessageType.Retire] = delegate()
			{
				if (retierData.retiredPlayerId != ClassSingleton<MultiBattleData>.Instance.MyPlayerUserId)
				{
					this.FinishTCPForcely = false;
					this.isRetireDialog = true;
					ClassSingleton<MultiBattleData>.Instance.BattleResult = 4;
					this.stateManager.uiControlPvP.HideRetireWindow();
					this.stateManager.uiControlPvP.HideSkillSelectUI();
					this.stateManager.uiControlPvP.ShowAlertDialog(BattleUIControlPvP.DialogType.EnemyRetire, StringMaster.GetString("BattleUI-21"), StringMaster.GetString("SystemButtonClose"), delegate
					{
						this.HideAlertTimerDialog();
						this.StopSomething();
						this.isRetireDialog = false;
						this.stateManager.events.CallWinEvent();
					}, true, 10);
					this.stateManager.uiControlPvP.BlockNewDialog();
				}
			};
			base.SendConfirmation(tcpMessageType, retierData.playerUserId);
			break;
		}
		case TCPMessageType.Confirmation:
		{
			global::Debug.Log("Confirmation: 受信");
			ConfirmationData confirmationData = TCPData<ConfirmationData>.Convert(messageObj);
			bool flag3 = this.GetEnemyId() != confirmationData.playerUserId;
			if (flag3)
			{
				global::Debug.Log("isUnknownEnemy");
				return;
			}
			bool flag = base.CheckHash(confirmationData.hashValue);
			bool flag2 = this.CheckStillAlive(confirmationData.playerUserId);
			if (flag || !flag2)
			{
				global::Debug.LogFormat("res:{0}, isStillAlive:{1}", new object[]
				{
					flag,
					flag2
				});
				return;
			}
			this.confirmationChecks[(TCPMessageType)confirmationData.tcpMessageType].Add(confirmationData.playerUserId);
			global::Debug.LogFormat("confirmationChecks : {0}から確認用{1}を受信しました. Count:{2}", new object[]
			{
				confirmationData.playerUserId,
				(TCPMessageType)confirmationData.tcpMessageType,
				this.confirmationChecks[(TCPMessageType)confirmationData.tcpMessageType].Count
			});
			this.HideAlertTimerDialog();
			break;
		}
		case TCPMessageType.LastConfirmation:
		{
			global::Debug.Log("LastConfirmation: 受信");
			LastConfirmationData lastConfirmationData = TCPData<LastConfirmationData>.Convert(messageObj);
			bool flag = base.CheckHash(lastConfirmationData.hashValue);
			bool flag2 = this.CheckStillAlive(lastConfirmationData.playerUserId);
			bool flag3 = this.GetEnemyId() != lastConfirmationData.playerUserId;
			if (flag || !flag2 || flag3)
			{
				global::Debug.LogFormat("2度防止 res:{0}, isStillAlive:{1}, tcpMessageType:{2}", new object[]
				{
					flag,
					flag2,
					(TCPMessageType)lastConfirmationData.tcpMessageType
				});
				return;
			}
			this.confirmationChecks[TCPMessageType.Confirmation].Add(lastConfirmationData.playerUserId);
			base.RunLastAction((TCPMessageType)lastConfirmationData.tcpMessageType);
			global::Debug.LogFormat("tcpMessageType:{0}", new object[]
			{
				(TCPMessageType)lastConfirmationData.tcpMessageType
			});
			break;
		}
		}
	}
}
