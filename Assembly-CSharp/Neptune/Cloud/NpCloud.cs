using Neptune.Cloud.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Neptune.Cloud
{
	public class NpCloud : INpCloudHandlerSystem, INpCloudManager
	{
		private INpCloud mListener;

		private INpCloudChat mChatListener;

		private short mErrorCode;

		private string mErrorMsg = string.Empty;

		private NpCloud.Type mCloudType;

		private string mHashKey = string.Empty;

		private NpCloudHandlerSystem mHandlerInstance;

		private bool mIsConnectAsync;

		private Action<bool> mConnectAsyncAction;

		private bool mIsConnectAsyncResult;

		public NpCloud(INpCloud listener, string hashKey = "")
		{
			try
			{
				this.mListener = listener;
				this.mHashKey = hashKey;
				if (this.ManagerInstance != null)
				{
					NpCloudManager.DeleteInstance();
				}
				NpCloudManager.CreateInstance(this);
				this.mHandlerInstance = NpCloudHandlerSystem.CreateInstance(this);
			}
			catch (NpCloudException errorData)
			{
				this.SetErrorData(errorData);
				this.Exit(this.mErrorCode, this.mErrorMsg);
			}
			catch (Exception errorData2)
			{
				this.SetErrorData(errorData2);
				this.Exit(this.mErrorCode, this.mErrorMsg);
			}
		}

		private NpCloudManager ManagerInstance
		{
			get
			{
				return NpCloudManager.GetInstance();
			}
		}

		private uint UserID
		{
			get
			{
				return this.ManagerInstance.UserID;
			}
		}

		public int PongEndCount
		{
			get
			{
				return this.ManagerInstance.PongEndCount;
			}
			set
			{
				this.ManagerInstance.PongEndCount = value;
			}
		}

		public int PongIntervalTime
		{
			get
			{
				return this.ManagerInstance.PongIntervalTime;
			}
			set
			{
				this.ManagerInstance.PongIntervalTime = value * 1000;
			}
		}

		public bool IsConnected
		{
			get
			{
				return this.mCloudType == NpCloud.Type.Idle && this.ManagerInstance.IsConnected;
			}
		}

		public void Delete()
		{
			try
			{
				this.Exit(0, string.Empty);
				this.mHandlerInstance.Destroy();
				this.mHandlerInstance = null;
				NpCloudManager.DeleteInstance();
			}
			catch (NpCloudException errorData)
			{
				this.SetErrorData(errorData);
				this.Exit(this.mErrorCode, this.mErrorMsg);
			}
			catch (Exception errorData2)
			{
				this.SetErrorData(errorData2);
				this.Exit(this.mErrorCode, this.mErrorMsg);
			}
		}

		public void SetChatCallBackListener(INpCloudChat chatListener)
		{
			this.mChatListener = chatListener;
		}

		public bool Connect(NpCloudSocketType type, string url, int timeOut, object option = null)
		{
			if (this.mCloudType != NpCloud.Type.None)
			{
				return true;
			}
			Action connect = delegate()
			{
				NpCloudSocketSystem instance = NpCloudSocketSystem.GetInstance();
				bool flag = false;
				global::Debug.Log("npCloudSocketSystem " + instance);
				if (instance != null)
				{
					flag = instance.Connect(timeOut, option);
				}
				if (flag)
				{
					this.mCloudType = NpCloud.Type.Idle;
				}
				else
				{
					this.mCloudType = NpCloud.Type.Error;
				}
			};
			return this.ConnectCore(type, url, connect);
		}

		public void ConnectAsync(NpCloudSocketType type, string url, int timeOut, Action<bool> result, object option = null)
		{
			if (this.mCloudType != NpCloud.Type.None)
			{
				result(true);
				return;
			}
			this.mIsConnectAsync = false;
			this.mConnectAsyncAction = result;
			Action connect = delegate()
			{
				Action<bool, Exception> result2 = delegate(bool flg, Exception e)
				{
					this.ConnectResultAction(flg, e, result);
				};
				NpCloudSocketSystem.GetInstance().ConnectAsync(timeOut, result2, option);
				this.mCloudType = NpCloud.Type.Connecting;
			};
			if (!this.ConnectCore(type, url, connect))
			{
				this.mIsConnectAsync = true;
				this.mIsConnectAsyncResult = false;
			}
		}

		private bool ConnectCore(NpCloudSocketType type, string url, Action connect)
		{
			bool result;
			try
			{
				this.mHandlerInstance.Active(true);
				this.mErrorCode = 0;
				this.mErrorMsg = string.Empty;
				this.mCloudType = NpCloud.Type.None;
				NpCloudSocketSystem.CreateInstance(url, type, this.mHashKey);
				connect();
				if (this.mCloudType == NpCloud.Type.Error)
				{
					result = false;
				}
				else
				{
					result = true;
				}
			}
			catch (NpCloudException errorData)
			{
				this.SetErrorData(errorData);
				this.Exit(this.mErrorCode, this.mErrorMsg);
				result = false;
			}
			catch (Exception errorData2)
			{
				this.SetErrorData(errorData2);
				this.Exit(this.mErrorCode, this.mErrorMsg);
				result = false;
			}
			return result;
		}

		private void ConnectResultAction(bool flg, Exception e, Action<bool> result)
		{
			if (!flg)
			{
				if (e is NpCloudException)
				{
					NpCloudException ex = (NpCloudException)e;
					this.mErrorCode = ex.ExitCode;
					this.mErrorMsg = ex.Message;
				}
				else
				{
					string text = string.Format("Message = {0}\nStackTrace = {1}", e.Message, e.StackTrace);
					this.mErrorCode = 700;
					this.mErrorMsg = text;
				}
			}
			this.mIsConnectAsync = true;
			this.mIsConnectAsyncResult = flg;
		}

		public void Request(List<int> to, object parameter)
		{
			try
			{
				NpCloudContentParameter npCloudContentParameter = new NpCloudContentParameter();
				npCloudContentParameter.to = to;
				npCloudContentParameter.msg = new List<object>
				{
					parameter
				};
				NpCloudServerData npCloudServerData = new NpCloudServerData();
				npCloudServerData.content = npCloudContentParameter;
				npCloudServerData.type = NpCloudType.direct;
				npCloudServerData.value = NpCloudValueType.send;
				this.ManagerInstance.Request(npCloudServerData);
			}
			catch (NpCloudException errorData)
			{
				this.SetErrorData(errorData);
				this.Exit(this.mErrorCode, this.mErrorMsg);
			}
			catch (Exception errorData2)
			{
				this.SetErrorData(errorData2);
				this.Exit(this.mErrorCode, this.mErrorMsg);
			}
		}

		public uint Request(object parameter, string ctrl, bool isHash = false, uint uid = 0u)
		{
			uint result;
			try
			{
				if (!isHash)
				{
					NpCloudServerData npCloudServerData = new NpCloudServerData();
					npCloudServerData.content = parameter;
					npCloudServerData.ctrl = ctrl;
					npCloudServerData.type = NpCloudType.common;
					npCloudServerData.value = NpCloudValueType.send;
					this.ManagerInstance.Request(npCloudServerData);
					result = 0u;
				}
				else
				{
					result = this.ManagerInstance.SequenceRequest<object>(parameter, ctrl, uid);
				}
			}
			catch (NpCloudException errorData)
			{
				this.SetErrorData(errorData);
				this.Exit(this.mErrorCode, this.mErrorMsg);
				result = 0u;
			}
			catch (Exception errorData2)
			{
				this.SetErrorData(errorData2);
				this.Exit(this.mErrorCode, this.mErrorMsg);
				result = 0u;
			}
			return result;
		}

		public void FindUser(List<int> findList)
		{
			this.ManagerInstance.FindUser(findList);
		}

		public void GetRoomListForHttp(NpCloudRoomListData data)
		{
			this.mHandlerInstance.GetRoomList(data);
		}

		public uint Create(string roomName)
		{
			return this.Create(roomName, RoomType.Area, null);
		}

		public uint Create(string roomName, RoomType roomType)
		{
			return this.Create(roomName, roomType, null);
		}

		public uint Create(string roomName, List<RoomCondition> conditionsList)
		{
			return this.Create(roomName, RoomType.Area, conditionsList);
		}

		public uint Create(string roomName, RoomType roomType, List<RoomCondition> conditionsList)
		{
			uint result;
			try
			{
				NpRoomRequestParameter npRoomRequestParameter = new NpRoomRequestParameter();
				npRoomRequestParameter.param.room_name = roomName;
				npRoomRequestParameter.param.room_type = (int)roomType;
				npRoomRequestParameter.param.user_id = this.UserID;
				if (conditionsList != null && conditionsList.Count > 0)
				{
					npRoomRequestParameter.param.room_condition = conditionsList;
				}
				result = this.ManagerInstance.JoinRequest<NpRoomRequestParameter>("9100", npRoomRequestParameter, 0u);
			}
			catch (NpCloudException errorData)
			{
				this.SetErrorData(errorData);
				this.Exit(this.mErrorCode, this.mErrorMsg);
				result = 0u;
			}
			catch (Exception errorData2)
			{
				this.SetErrorData(errorData2);
				this.Exit(this.mErrorCode, this.mErrorMsg);
				result = 0u;
			}
			return result;
		}

		public uint Join(string roomId)
		{
			uint result;
			try
			{
				NpJoinLeaveRoomReqParam npJoinLeaveRoomReqParam = new NpJoinLeaveRoomReqParam();
				npJoinLeaveRoomReqParam.param.room_id = roomId;
				npJoinLeaveRoomReqParam.param.user_id = this.UserID;
				result = this.ManagerInstance.JoinRequest<NpJoinLeaveRoomReqParam>("9102", npJoinLeaveRoomReqParam, 0u);
			}
			catch (NpCloudException errorData)
			{
				this.SetErrorData(errorData);
				this.Exit(this.mErrorCode, this.mErrorMsg);
				result = 0u;
			}
			catch (Exception errorData2)
			{
				this.SetErrorData(errorData2);
				this.Exit(this.mErrorCode, this.mErrorMsg);
				result = 0u;
			}
			return result;
		}

		public uint Leave(string roomId)
		{
			uint result;
			try
			{
				NpJoinLeaveRoomReqParam npJoinLeaveRoomReqParam = new NpJoinLeaveRoomReqParam();
				npJoinLeaveRoomReqParam.param.room_id = roomId;
				npJoinLeaveRoomReqParam.param.user_id = this.UserID;
				result = this.ManagerInstance.LeaveRequest<NpJoinLeaveRoomReqParam>(npJoinLeaveRoomReqParam, roomId);
			}
			catch (NpCloudException errorData)
			{
				this.SetErrorData(errorData);
				this.Exit(this.mErrorCode, this.mErrorMsg);
				result = 0u;
			}
			catch (Exception errorData2)
			{
				this.SetErrorData(errorData2);
				this.Exit(this.mErrorCode, this.mErrorMsg);
				result = 0u;
			}
			return result;
		}

		public uint RemoveRoom(string roomId)
		{
			uint result;
			try
			{
				result = this.ManagerInstance.RemoveRoom(roomId);
			}
			catch (NpCloudException errorData)
			{
				this.SetErrorData(errorData);
				this.Exit(this.mErrorCode, this.mErrorMsg);
				result = 0u;
			}
			catch (Exception errorData2)
			{
				this.SetErrorData(errorData2);
				this.Exit(this.mErrorCode, this.mErrorMsg);
				result = 0u;
			}
			return result;
		}

		public uint ForceLeaveRoom(string roomId, int targetUserId)
		{
			uint result;
			try
			{
				result = this.ManagerInstance.ForceLeaveRoom(roomId, targetUserId);
			}
			catch (NpCloudException errorData)
			{
				this.SetErrorData(errorData);
				this.Exit(this.mErrorCode, this.mErrorMsg);
				result = 0u;
			}
			catch (Exception errorData2)
			{
				this.SetErrorData(errorData2);
				this.Exit(this.mErrorCode, this.mErrorMsg);
				result = 0u;
			}
			return result;
		}

		public void SendRoomMessage(string roomId, string message)
		{
			try
			{
				this.ManagerInstance.SendRoomMessage(roomId, message);
			}
			catch (NpCloudException errorData)
			{
				this.SetErrorData(errorData);
				this.Exit(this.mErrorCode, this.mErrorMsg);
			}
			catch (Exception errorData2)
			{
				this.SetErrorData(errorData2);
				this.Exit(this.mErrorCode, this.mErrorMsg);
			}
		}

		public void GetRoomMsgLog(string roomId, int limit)
		{
			try
			{
				this.ManagerInstance.GetRoomMsgLog(roomId, limit);
			}
			catch (NpCloudException errorData)
			{
				this.SetErrorData(errorData);
				this.Exit(this.mErrorCode, this.mErrorMsg);
			}
			catch (Exception errorData2)
			{
				this.SetErrorData(errorData2);
				this.Exit(this.mErrorCode, this.mErrorMsg);
			}
		}

		public void Exit()
		{
			try
			{
				this.Exit(0, string.Empty);
			}
			catch (NpCloudException errorData)
			{
				this.SetErrorData(errorData);
				this.Exit(this.mErrorCode, this.mErrorMsg);
			}
			catch (Exception errorData2)
			{
				this.SetErrorData(errorData2);
				this.Exit(this.mErrorCode, this.mErrorMsg);
			}
		}

		private void Exit(short exitCode, string message)
		{
			this.mCloudType = NpCloud.Type.None;
			this.mHandlerInstance.Active(false);
			Action exitAction = delegate()
			{
				this.ExitAction(exitCode, message);
			};
			NpCloudSocketSystem.DeleteInstance(exitAction);
		}

		private void ExitAction(short exitCode, string message)
		{
			this.mErrorCode = 0;
			this.mErrorMsg = string.Empty;
			if (exitCode == 0)
			{
				this.mListener.OnExit();
			}
			else
			{
				this.mListener.OnCloudException(exitCode, message);
			}
		}

		public void OnResponse(int sender, Dictionary<string, object> receponse)
		{
			this.mListener.OnResponse(sender, receponse);
		}

		public void OnFindUser(List<int> on, List<int> off)
		{
			if (this.mChatListener != null)
			{
				this.mChatListener.OnFindUser(on, off);
			}
		}

		public void OnMessage(NpMessageParameter msgData)
		{
			if (this.mChatListener != null)
			{
				this.mChatListener.OnMessage(msgData);
			}
		}

		public void OnRoomMsgLog(List<NpRoomMsgLog> roomMsgLogList)
		{
			if (this.mChatListener != null)
			{
				this.mChatListener.OnRoomMsgLog(roomMsgLogList);
			}
		}

		public void OnCtrlResponse(string command, Dictionary<string, object> parameter)
		{
			this.mListener.OnCtrlResponse(command, parameter);
		}

		public void OnJoinRoom(NpRoomParameter roomData)
		{
			if (this.mChatListener != null)
			{
				this.mChatListener.OnJoinRoom(roomData);
			}
		}

		public void OnLeaveRoom(NpLeaveParameter leaveData)
		{
			if (this.mChatListener != null)
			{
				this.mChatListener.OnLeaveRoom(leaveData);
			}
		}

		public void OnRequestException(NpCloudErrorData error)
		{
			this.mListener.OnRequestException(error);
		}

		public void OnGetRoomList(List<NpRoomParameter> roomData)
		{
			if (this.mChatListener != null)
			{
				this.mChatListener.OnGetRoomList(roomData);
			}
		}

		public void OnHttpRequestException(uint errorCode, string command, string errorMsg, string detail)
		{
			NpCloudErrorData npCloudErrorData = new NpCloudErrorData();
			npCloudErrorData.resultCode = errorCode.ToString();
			npCloudErrorData.resultMsg = errorMsg;
			npCloudErrorData.command = errorMsg;
			npCloudErrorData.detail = detail;
			this.mListener.OnRequestException(npCloudErrorData);
		}

		public void Update()
		{
			try
			{
				if (this.mErrorCode != 0)
				{
					this.Exit(this.mErrorCode, this.mErrorMsg);
				}
				else
				{
					switch (this.mCloudType)
					{
					case NpCloud.Type.Connecting:
						if (this.mConnectAsyncAction != null && this.mIsConnectAsync)
						{
							this.mCloudType = NpCloud.Type.Idle;
							this.mConnectAsyncAction(this.mIsConnectAsyncResult);
						}
						break;
					case NpCloud.Type.Idle:
						this.ManagerInstance.Update();
						break;
					}
				}
			}
			catch (NpCloudException ex)
			{
				UnityEngine.Debug.LogError(ex.Message + "\n" + ex.StackTrace);
				this.SetErrorData(ex);
				this.Exit(this.mErrorCode, this.mErrorMsg);
			}
			catch (Exception errorData)
			{
				this.SetErrorData(errorData);
				this.Exit(this.mErrorCode, this.mErrorMsg);
			}
		}

		public void OnApplicationPause(bool pauseStatus)
		{
			if (pauseStatus)
			{
				this.Exit(741, "OnApplicationPause");
			}
		}

		public void OnApplicationQuit()
		{
			this.Exit(740, "OnApplicationQuit");
		}

		private void SetErrorData(Exception e)
		{
			string text = string.Format("Message = {0}\nStackTrace = {1}", e.Message, e.StackTrace);
			this.mErrorCode = 700;
			this.mErrorMsg = text;
		}

		private void SetErrorData(NpCloudException e)
		{
			this.mErrorCode = e.ExitCode;
			this.mErrorMsg = e.Message;
		}

		private enum Type
		{
			None,
			Connecting,
			Idle,
			Error
		}
	}
}
