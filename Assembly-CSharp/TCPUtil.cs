using Master;
using Neptune.Cloud;
using Neptune.Cloud.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCPUtil : Singleton<TCPUtil>, INpCloud
{
	private string projectId = "gsdigimon";

	private string SocketCtrl = "socket/ActiveController";

	private const string MESSAGE_KEY_ERROR = "errorMsg";

	private const string MESSAGE_KEY_DEFAULT = "activityList";

	public const string TCP_SHARDING_CHAT = "chat";

	public const string TCP_SHARDING_PVP = "pvp";

	public const string TCP_SHARDING_MULTI = "multi";

	private string myUserId;

	private string hostAddress;

	private static bool isTCPSending;

	private int timeOut = 10;

	private NpCloud manager;

	public Dictionary<TCPMessageType, List<string>> confirmationChecks;

	private Action<string> afterGetTcpShardingStringAction;

	private Action<bool> afterConnectTCPAction;

	private Action<Dictionary<string, object>> receiveDataCallBackAction;

	private Action onExitCallBackAction;

	private Action<short, string> onExceptionAction;

	private Action<string, string> onRequestExceptionAction;

	private Dictionary<string, object> parameter;

	private Dictionary<string, object> header;

	private int requestCount;

	public BattleMode battleMode { get; set; }

	public static bool IsTCPSending
	{
		get
		{
			return TCPUtil.isTCPSending;
		}
	}

	public static bool USE_DEBUG_OUT { get; set; }

	public void PrepareTCPServer(Action<string> action, string shardingType)
	{
		this.afterGetTcpShardingStringAction = action;
		GameWebAPI.RespData_GetTcpShardingString data = null;
		GameWebAPI.GetTcpShardingString request = new GameWebAPI.GetTcpShardingString
		{
			SetSendData = delegate(GameWebAPI.ReqData_GetTcpShardingString param)
			{
				param.type = shardingType;
			},
			OnReceived = delegate(GameWebAPI.RespData_GetTcpShardingString response)
			{
				data = response;
			}
		};
		base.StartCoroutine(request.Run(delegate()
		{
			RestrictionInput.EndLoad();
			this.hostAddress = data.server;
			global::Debug.Log("接続先TCPサーバは「" + this.hostAddress + "」です");
			if (this.afterGetTcpShardingStringAction != null)
			{
				this.afterGetTcpShardingStringAction(data.server);
			}
		}, null, null));
		this.parameter = new Dictionary<string, object>();
		this.parameter.Add("X-AppVer", WebAPIPlatformValue.GetAppVersion());
		this.header = new Dictionary<string, object>();
		this.header.Add("headers", this.parameter);
	}

	public void SetTcpShardingString(GameWebAPI.RespData_GetTcpShardingString data)
	{
		this.hostAddress = data.server;
		global::Debug.Log("接続先TCPサーバは「" + this.hostAddress + "」です");
	}

	public void GetTCPSystemReceponseData(Dictionary<string, object> data)
	{
		int num = -1;
		foreach (KeyValuePair<string, object> keyValuePair in data)
		{
			Dictionary<object, object> dictionary = (Dictionary<object, object>)keyValuePair.Value;
			foreach (KeyValuePair<object, object> keyValuePair2 in dictionary)
			{
				if (keyValuePair2.Key.ToString() == "cgi")
				{
					num = Convert.ToInt32(keyValuePair2.Value);
				}
			}
			if (ClassSingleton<ChatData>.Instance.CurrentChatInfo.groupId == num)
			{
				Singleton<TCPUtil>.Instance.TCPDisConnect(false);
			}
		}
	}

	public bool CheckPrepareTCPServer()
	{
		return this.hostAddress != null;
	}

	public bool CheckTCPConnection()
	{
		bool result = false;
		if (this.manager != null)
		{
			result = this.manager.IsConnected;
		}
		return result;
	}

	public void MakeTCPClient()
	{
		if (this.manager == null)
		{
			this.manager = new NpCloud(this, string.Empty);
		}
		this.ResetAllCallBackMethod();
	}

	public void ResetAllCallBackMethod()
	{
		this.afterGetTcpShardingStringAction = null;
		this.afterConnectTCPAction = null;
		this.receiveDataCallBackAction = null;
		this.onExitCallBackAction = null;
		this.onExceptionAction = null;
		this.onRequestExceptionAction = null;
	}

	public bool ConnectTCPServer(string userId)
	{
		bool flag = this.CheckPrepareTCPServer();
		bool flag2 = this.CheckTCPConnection();
		if (flag2)
		{
			return true;
		}
		if (this.manager == null || !flag)
		{
			return false;
		}
		bool flag3 = false;
		this.myUserId = userId;
		try
		{
			global::Debug.Log(string.Concat(new string[]
			{
				"TCP接続開始：tcp://",
				this.hostAddress,
				"/?projectid=",
				this.projectId,
				"&me=",
				this.myUserId
			}));
			flag3 = this.manager.Connect(NpCloudSocketType.TCP, string.Concat(new string[]
			{
				"tcp://",
				this.hostAddress,
				"/?projectid=",
				this.projectId,
				"&me=",
				this.myUserId
			}), this.timeOut, this.header);
		}
		catch (Exception ex)
		{
			global::Debug.LogError(string.Format("Source:{0}", ex.Source));
			global::Debug.LogError(string.Format("StackTrace:{0}", ex.StackTrace));
			global::Debug.LogError(string.Format("TargetSite:{0}", ex.TargetSite));
			global::Debug.LogError(string.Format("Error:{0}", ex.Message));
		}
		if (flag3)
		{
			global::Debug.Log("TCP Connected Success");
			this.SetTCPParam();
			UnityEngine.Object.DontDestroyOnLoad(NpCloudHandlerSystem.gameObject);
			return true;
		}
		global::Debug.Log("TCP Connected Failed");
		return false;
	}

	public void ConnectTCPServerAsync(string userId)
	{
		bool flag = this.CheckPrepareTCPServer();
		if (!this.CheckTCPConnection() && this.manager != null && flag)
		{
			this.myUserId = userId;
			try
			{
				global::Debug.Log(string.Concat(new string[]
				{
					"TCP接続開始：tcp://",
					this.hostAddress,
					"/?projectid=",
					this.projectId,
					"&me=",
					this.myUserId
				}));
				this.manager.ConnectAsync(NpCloudSocketType.TCP, string.Concat(new string[]
				{
					"tcp://",
					this.hostAddress,
					"/?projectid=",
					this.projectId,
					"&me=",
					this.myUserId
				}), this.timeOut, new Action<bool>(this.AfterConnectTCPExec), this.header);
			}
			catch (Exception ex)
			{
				global::Debug.LogError(string.Format("Error:{0}", ex.Message));
				global::Debug.Log("TCP Connected Failed");
				this.AfterConnectTCPExec(false);
			}
		}
	}

	private void SetTCPParam()
	{
		if (BattleStateManager.current != null && !BattleStateManager.current.onServerConnect)
		{
			this.manager.PongEndCount = 5;
			this.manager.PongIntervalTime = 3;
		}
		else
		{
			this.manager.PongEndCount = ConstValue.TCP_PONG_END_COUNT;
			this.manager.PongIntervalTime = ConstValue.TCP_PONG_INTERVAL_TIME;
		}
	}

	public void SendTCPRequest(Dictionary<string, object> data, string tcpKey = "activityList")
	{
		if (this.manager == null)
		{
			global::Debug.LogWarning("NpCloud Manager Null");
			return;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add(tcpKey, data);
		this.ShowSendData(tcpKey, data);
		this.manager.Request(dictionary, this.SocketCtrl, false, 0u);
	}

	public void SendTCPRequest(Dictionary<string, object> data, List<int> to, string tcpKey = "activityList")
	{
		if (this.manager == null)
		{
			global::Debug.LogWarning("NpCloud Manager Null");
			return;
		}
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		dictionary.Add(tcpKey, data);
		this.ShowSendData(tcpKey, data);
		this.manager.Request(to, dictionary);
	}

	public void TCPDisConnect(bool isClearManager = false)
	{
		if (this.manager == null)
		{
			return;
		}
		if (isClearManager)
		{
			this.manager.Delete();
			this.manager = null;
		}
		else
		{
			this.manager.Exit();
		}
	}

	public void OnExit()
	{
		if (this.onExitCallBackAction != null)
		{
			this.onExitCallBackAction();
		}
	}

	private void AfterConnectTCPExec(bool result)
	{
		if (result)
		{
			this.SetTCPParam();
			UnityEngine.Object.DontDestroyOnLoad(NpCloudHandlerSystem.gameObject);
		}
		this.afterConnectTCPAction(result);
	}

	public void SetAfterConnectTCPMethod(Action<bool> action)
	{
		this.afterConnectTCPAction = action;
	}

	public void SetTCPCallBackMethod(Action<Dictionary<string, object>> action)
	{
		this.receiveDataCallBackAction = action;
	}

	public void SetOnExitCallBackMethod(Action action)
	{
		this.onExitCallBackAction = action;
	}

	public void SetExceptionMethod(Action<short, string> action)
	{
		this.onExceptionAction = action;
	}

	public void SetRequestExceptionMethod(Action<string, string> action)
	{
		this.onRequestExceptionAction = action;
	}

	public void OnResponse(int sender, Dictionary<string, object> parameter)
	{
		bool flag = false;
		foreach (KeyValuePair<string, object> keyValuePair in parameter)
		{
			if (keyValuePair.Key == "errorMsg")
			{
				flag = true;
				AlertManager.ShowAlertDialog(null, "TCP Server Error", keyValuePair.Value.ToString(), AlertManager.ButtonActionType.Close, false);
			}
		}
		if (!flag && this.receiveDataCallBackAction != null)
		{
			this.receiveDataCallBackAction(parameter);
		}
	}

	public void OnCtrlResponse(string command, Dictionary<string, object> parameter)
	{
		bool flag = false;
		foreach (KeyValuePair<string, object> keyValuePair in parameter)
		{
			if (keyValuePair.Key == "errorMsg")
			{
				flag = true;
				AlertManager.ShowAlertDialog(null, "TCP Server Error", keyValuePair.Value.ToString(), AlertManager.ButtonActionType.Close, false);
			}
		}
		if (!flag && this.receiveDataCallBackAction != null)
		{
			this.receiveDataCallBackAction(parameter);
		}
	}

	public void OnCloudException(short exitCode, string message)
	{
		if (exitCode == 712)
		{
			this.hostAddress = message;
			this.TCPReConnection(0);
			return;
		}
		if (this.onExceptionAction == null)
		{
			global::Debug.Log("コールバックはありません");
		}
		else
		{
			this.onExceptionAction(exitCode, message);
		}
	}

	public void TCPReConnection(int idx)
	{
		this.ConnectTCPServer(this.myUserId);
	}

	public void OnRequestException(NpCloudErrorData error)
	{
		global::Debug.Log("resultCode: " + error.resultCode);
		global::Debug.Log("command : " + error.command);
		global::Debug.Log("resultMsg: " + error.resultMsg);
		global::Debug.Log("detail : " + error.detail);
		if (this.onRequestExceptionAction != null)
		{
			this.onRequestExceptionAction(error.resultCode, error.resultMsg);
		}
	}

	public string CreateHash(TCPMessageType tcpMessageType, string userId, TCPMessageType tcpMessageTypeForLastConfirmation = TCPMessageType.None)
	{
		string inputString = string.Format("{0}_{1}_{2}_{3}", new object[]
		{
			userId,
			tcpMessageType,
			tcpMessageTypeForLastConfirmation,
			DateTime.Now.ToString("yyyyMMddhhmmssffff")
		});
		return Util.GetHashString(inputString);
	}

	public bool CheckHash(string hashString, Queue hashValueQueue)
	{
		bool flag = hashValueQueue.Contains(hashString);
		if (flag)
		{
			return true;
		}
		hashValueQueue.Enqueue(hashString);
		if (hashValueQueue.Count > 10)
		{
			hashValueQueue.Dequeue();
		}
		return false;
	}

	public void InitConfirmationChecks()
	{
		TCPUtil.isTCPSending = false;
		this.confirmationChecks = new Dictionary<TCPMessageType, List<string>>();
		IEnumerator enumerator = Enum.GetValues(typeof(TCPMessageType)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				TCPMessageType key = (TCPMessageType)obj;
				this.confirmationChecks[key] = new List<string>();
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	public void SendConfirmation(TCPMessageType tcpMessageType, string targetId, string myUserId, string tcpKey)
	{
		global::Debug.LogFormat("[SendConfirmation] tcpMessageType:{0}, MyPlayerUserId:{1}, targetId:{2}", new object[]
		{
			tcpMessageType,
			myUserId,
			targetId
		});
		ConfirmationData message = new ConfirmationData
		{
			playerUserId = myUserId,
			hashValue = this.CreateHash(TCPMessageType.Confirmation, myUserId, TCPMessageType.None),
			tcpMessageType = tcpMessageType.ToInteger()
		};
		this.SendMessageForTarget(TCPMessageType.Confirmation, message, new List<int>
		{
			targetId.ToInt32()
		}, tcpKey);
	}

	public IEnumerator SendMessageInsistently<T>(TCPMessageType tcpMessageType, TCPData<T> message, List<int> TCPSendUserIdList, string TCPKey = "activityList", Action<List<int>> SendFaildAction = null) where T : class
	{
		TCPUtil.isTCPSending = true;
		int waitingCount = 0;
		List<int> InsistentlySendUserIdList = new List<int>();
		foreach (int item in TCPSendUserIdList)
		{
			InsistentlySendUserIdList.Add(item);
		}
		for (;;)
		{
			global::Debug.LogFormat("[SendMessage] tcpMessageType:{0}, MyPlayerUserId:{1} , waitingCount:{2}", new object[]
			{
				tcpMessageType,
				this.myUserId,
				waitingCount
			});
			for (int i = 0; i < this.confirmationChecks[tcpMessageType].Count; i++)
			{
				InsistentlySendUserIdList.Remove(int.Parse(this.confirmationChecks[tcpMessageType][i]));
			}
			bool sendResult = this.SendMessageForTarget(tcpMessageType, message, InsistentlySendUserIdList, TCPKey);
			yield return AppCoroutine.Start(Util.WaitForRealTime(1f), false);
			waitingCount++;
			bool finishForcely = false;
			if ((float)waitingCount >= 10f || !sendResult)
			{
				finishForcely = true;
				if ((float)waitingCount >= 10f && SendFaildAction != null)
				{
					SendFaildAction(InsistentlySendUserIdList);
				}
			}
			if (this.confirmationChecks[tcpMessageType].Count == TCPSendUserIdList.Count || finishForcely)
			{
				break;
			}
			yield return null;
		}
		this.confirmationChecks[tcpMessageType].Clear();
		TCPUtil.isTCPSending = false;
		global::Debug.LogFormat("[PacketEnd] tcpMessageType:{0}", new object[]
		{
			tcpMessageType
		});
		yield break;
		yield break;
	}

	public bool SendMessageForTarget(TCPMessageType tcpMessageType, object message, List<int> TCPSendUserIdList, string TCPKey = "activityList")
	{
		Dictionary<string, object> data = ClassSingleton<TCPMessageFactory>.Instance.CreateMessage(tcpMessageType, message);
		if (TCPSendUserIdList == null || TCPSendUserIdList.Count == 0)
		{
			return false;
		}
		if (this.CheckTCPConnection())
		{
			this.SendTCPRequest(data, TCPSendUserIdList, TCPKey);
			return true;
		}
		global::Debug.LogError("TCP not connect");
		return false;
	}

	public IEnumerator SendChatMessage(int chatGroupId, string message, int type, Action<int> action = null)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("810002", new TCPUtil.SocketMessage
		{
			cgi = chatGroupId,
			msg = message,
			tp = type
		});
		Singleton<TCPUtil>.Instance.SendTCPRequest(data, "activityList");
		yield return null;
		if (action != null)
		{
			action(0);
		}
		yield break;
	}

	public IEnumerator SendChatExpulsion(int chatGroupId, string target, Action<int> action = null)
	{
		Dictionary<string, object> data = new Dictionary<string, object>();
		data.Add("810003", new TCPUtil.SocketExpulsion
		{
			chatGroupId = chatGroupId,
			target = target
		});
		Singleton<TCPUtil>.Instance.SendTCPRequest(data, "activityList");
		yield return null;
		if (action != null)
		{
			action(0);
		}
		yield break;
	}

	public IEnumerator SendSystemMessege(int cgi, string uid, string uname)
	{
		string msg = string.Format(StringMaster.GetString("ChatLog-01"), uname);
		while (!Singleton<TCPUtil>.Instance.CheckPrepareTCPServer())
		{
			yield return null;
		}
		if (Singleton<TCPUtil>.Instance.ConnectTCPServer(uid))
		{
			base.StartCoroutine(this.SendChatMessage(cgi, msg, 3, null));
		}
		yield break;
	}

	public void SendSystemMessegeAlreadyConnected(int cgi, string mes, string uname, Action<int> action = null)
	{
		string message = string.Format(StringMaster.GetString(mes), uname);
		base.StartCoroutine(this.SendChatMessage(cgi, message, 3, action));
	}

	private void ShowSendData(string key, Dictionary<string, object> sendData)
	{
	}

	public void OnJoinRoom(NpRoomParameter roomData)
	{
	}

	public void OnLeaveRoom(NpLeaveParameter leaveData)
	{
	}

	public void GetJoinRoomList(List<NpRoomParameter> roomData)
	{
	}

	public void OnGetRoomList(List<NpRoomParameter> roomData)
	{
	}

	public void OnMessage(NpMessageParameter msgData)
	{
	}

	public void OnRoomMsgLog(List<NpRoomMsgLog> roomMsgLogList)
	{
	}

	public void FindUser(List<int> findList)
	{
	}

	public void OnFindUser(List<int> on, List<int> off)
	{
	}

	public void InitializeUniqueRequestId()
	{
		TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks);
		this.requestCount = (int)timeSpan.TotalSeconds;
	}

	public int GetUniqueRequestId()
	{
		this.requestCount++;
		global::Debug.Log("requestCount : " + this.requestCount);
		return this.requestCount;
	}

	private class SocketMessage
	{
		public int cgi;

		public string msg;

		public int tp;
	}

	private class SocketExpulsion
	{
		public int chatGroupId;

		public string target;
	}

	public class SystemMsg
	{
		public string key;

		public string uname;
	}
}
