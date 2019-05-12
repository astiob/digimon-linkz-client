using Neptune.Common;
using System;
using System.Collections.Generic;

namespace Neptune.Cloud.Core
{
	public class NpCloudManager
	{
		private const string kVrmpErrConnectServerIncorrect = "406";

		private List<INpCloudManagerSystem> mListenerlist;

		private INpCloudManager mListener;

		private NpCloudChatSystem mChatSystem;

		private NpCloudRoomSystem mRoomSystem;

		private static NpCloudManager instance;

		private NpCloudManager(INpCloudManager listener)
		{
			this.mListenerlist = new List<INpCloudManagerSystem>();
			this.mListener = listener;
		}

		private NpCloudSocketSystem SocketInstance
		{
			get
			{
				return NpCloudSocketSystem.GetInstance();
			}
		}

		public bool IsConnected
		{
			get
			{
				return this.SocketInstance != null && this.SocketInstance.IsConnected;
			}
		}

		public uint UserID
		{
			get
			{
				if (this.SocketInstance == null)
				{
					return 0u;
				}
				return this.SocketInstance.UserID;
			}
		}

		public int PongEndCount
		{
			get
			{
				if (this.SocketInstance == null)
				{
					return 0;
				}
				return this.SocketInstance.PongEndCount;
			}
			set
			{
				this.SocketInstance.PongEndCount = value;
			}
		}

		public int PongIntervalTime
		{
			get
			{
				if (this.SocketInstance == null)
				{
					return 0;
				}
				return this.SocketInstance.PongIntervalTime;
			}
			set
			{
				this.SocketInstance.PongIntervalTime = value;
			}
		}

		public static void CreateInstance(INpCloudManager listener)
		{
			if (NpCloudManager.instance != null)
			{
				return;
			}
			NpCloudManager.instance = new NpCloudManager(listener);
			NpCloudManager.instance.mRoomSystem = new NpCloudRoomSystem();
			NpCloudManager.instance.mRoomSystem.JoinRoom = new Action<NpRoomParameter>(NpCloudManager.instance.mListener.OnJoinRoom);
			NpCloudManager.instance.mRoomSystem.LeaveRoom = new Action<NpLeaveParameter>(NpCloudManager.instance.mListener.OnLeaveRoom);
			NpCloudManager.instance.mRoomSystem.RequestException = new Action<NpCloudErrorData>(NpCloudManager.instance.mListener.OnRequestException);
			NpCloudManager.instance.AddListener(NpCloudManager.instance.mRoomSystem);
			NpCloudManager.instance.mChatSystem = new NpCloudChatSystem();
			NpCloudManager.instance.mChatSystem.Message = new Action<NpMessageParameter>(NpCloudManager.instance.mListener.OnMessage);
			NpCloudManager.instance.mChatSystem.RoomMsgLog = new Action<List<NpRoomMsgLog>>(NpCloudManager.instance.mListener.OnRoomMsgLog);
			NpCloudManager.instance.mChatSystem.RequestException = new Action<NpCloudErrorData>(NpCloudManager.instance.mListener.OnRequestException);
			NpCloudManager.instance.AddListener(NpCloudManager.instance.mChatSystem);
		}

		public static NpCloudManager GetInstance()
		{
			return NpCloudManager.instance;
		}

		public static void DeleteInstance()
		{
			if (NpCloudManager.instance != null)
			{
				NpCloudManager.instance.mListenerlist.Clear();
				NpCloudManager.instance.mListenerlist = null;
				NpCloudManager.instance.mChatSystem.ClearData();
				NpCloudManager.instance.mRoomSystem.ClearData();
				NpCloudManager.instance.mChatSystem = null;
				NpCloudManager.instance.mRoomSystem = null;
			}
			NpCloudManager.instance = null;
		}

		public void FindUser(List<int> findList)
		{
			this.Request(new NpCloudServerData
			{
				content = findList,
				type = NpCloudType.direct,
				value = NpCloudValueType.finduser
			});
		}

		public IList<int> GetUserList(string roomId)
		{
			return this.mRoomSystem.GetUserList(roomId);
		}

		public uint JoinRequest<T>(string ctrl, T parameter, uint uid = 0u) where T : class
		{
			return this.SequenceRequest<T>(parameter, ctrl, uid);
		}

		public uint LeaveRequest<T>(T parameter, string roomId) where T : class
		{
			if (!this.mRoomSystem.IsRoom(roomId))
			{
				throw new NpCloudException(770, "ルームIDが異常です");
			}
			return this.SequenceRequest<T>(parameter, "9105", 0u);
		}

		public uint RemoveRoom(string roomId)
		{
			return this.SequenceRequest<NpRemoveRoomParameter>(new NpRemoveRoomParameter
			{
				param = 
				{
					room_id = roomId
				}
			}, "9101", 0u);
		}

		public uint ForceLeaveRoom(string roomId, int targetUserId)
		{
			return this.SequenceRequest<NpForceLeaveRoomReqParam>(new NpForceLeaveRoomReqParam
			{
				param = 
				{
					room_id = roomId,
					user_id = (uint)targetUserId
				}
			}, "9106", 0u);
		}

		public void SendRoomMessage(string roomId, string message)
		{
			if (!this.mRoomSystem.IsRoom(roomId))
			{
				throw new NpCloudException(770, "ルームIDが異常です");
			}
			this.mChatSystem.SendRoomMessage(roomId, message);
		}

		public void GetRoomMsgLog(string roomId, int limit)
		{
			if (!this.mRoomSystem.IsRoom(roomId))
			{
				throw new NpCloudException(770, "ルームIDが異常です");
			}
			this.mChatSystem.GetRoomMsgLog(roomId, limit);
		}

		public void Update()
		{
			byte[] array = null;
			while (this.SocketInstance != null)
			{
				NPCloudReceiveParameter<NPCloudReceiveHashParaemter> requestParameter = this.SocketInstance.GetRequestParameter<NPCloudReceiveParameter<NPCloudReceiveHashParaemter>>(out array);
				if (requestParameter == null || array == null)
				{
					break;
				}
				string[] array2 = requestParameter.command.Split(new char[]
				{
					':'
				});
				NPCloudReceiveParameter<NpCloudVenusErrorParameter> npcloudReceiveParameter = NpMessagePack.Unpack<NPCloudReceiveParameter<NpCloudVenusErrorParameter>>(array);
				NpCloudErrorData npCloudErrorData = new NpCloudErrorData();
				npCloudErrorData.command = array2[1];
				npCloudErrorData.resultCode = requestParameter.resultCode;
				npCloudErrorData.resultMsg = requestParameter.resultMsg;
				npCloudErrorData.venus = npcloudReceiveParameter.body;
				if (requestParameter.resultCode == null || !requestParameter.resultCode.Equals("0"))
				{
					if (requestParameter.resultCode == "406")
					{
						throw new NpCloudException(712, requestParameter.body.shardingServ);
					}
					for (int i = 0; i < this.mListenerlist.Count; i++)
					{
						if (this.mListenerlist[i].ReceiveException(npCloudErrorData))
						{
						}
					}
					this.mListener.OnRequestException(npCloudErrorData);
				}
				object receiveData = null;
				string text = array2[1];
				if (text == null)
				{
					goto IL_2DC;
				}
				if (NpCloudManager.<>f__switch$map4 == null)
				{
					NpCloudManager.<>f__switch$map4 = new Dictionary<string, int>(8)
					{
						{
							string.Empty,
							0
						},
						{
							"9100",
							1
						},
						{
							"9101",
							1
						},
						{
							"9102",
							1
						},
						{
							"9105",
							1
						},
						{
							"9106",
							1
						},
						{
							"9201",
							2
						},
						{
							"9202",
							3
						}
					};
				}
				int num;
				if (!NpCloudManager.<>f__switch$map4.TryGetValue(text, out num))
				{
					goto IL_2DC;
				}
				switch (num)
				{
				case 0:
					if (array2[0].Equals(NpCloudValueType.send.ToString()))
					{
						NPCloudReceiveParameter<Dictionary<string, object>> npcloudReceiveParameter2 = NpMessagePack.Unpack<NPCloudReceiveParameter<Dictionary<string, object>>>(array);
						this.mListener.OnResponse((int)npcloudReceiveParameter2.sender, npcloudReceiveParameter2.body);
					}
					else if (array2[0].Equals(NpCloudValueType.finduser.ToString()))
					{
						NPCloudReceiveParameter<NpCloudReceiveFindUserParameter> npcloudReceiveParameter3 = NpMessagePack.Unpack<NPCloudReceiveParameter<NpCloudReceiveFindUserParameter>>(array);
						List<int> on = npcloudReceiveParameter3.body.on.ConvertAll<int>((string x) => int.Parse(x));
						List<int> off = npcloudReceiveParameter3.body.off.ConvertAll<int>((string x) => int.Parse(x));
						this.mListener.OnFindUser(on, off);
					}
					continue;
				case 1:
					receiveData = this.SocketInstance.Deserialize<NpCloudReceiveRoomParameter>(requestParameter.body.data);
					break;
				case 2:
					receiveData = NpMessagePack.Unpack<NPCloudReceiveParameter<string>>(array);
					break;
				case 3:
					receiveData = NpMessagePack.Unpack<NPCloudReceiveParameter<List<RoomMsgLog>>>(array);
					break;
				default:
					goto IL_2DC;
				}
				IL_2FE:
				for (int j = 0; j < this.mListenerlist.Count; j++)
				{
					if (this.mListenerlist[j].Receive(array2[1], receiveData, requestParameter.resTime))
					{
					}
				}
				continue;
				IL_2DC:
				NPCloudReceiveParameter<Dictionary<string, object>> npcloudReceiveParameter4 = NpMessagePack.Unpack<NPCloudReceiveParameter<Dictionary<string, object>>>(array);
				this.mListener.OnCtrlResponse(array2[1], npcloudReceiveParameter4.body);
				goto IL_2FE;
			}
		}

		public void AddListener(INpCloudManagerSystem listener)
		{
			if (this.mListenerlist.Contains(listener))
			{
				return;
			}
			this.mListenerlist.Add(listener);
		}

		public uint SequenceRequest<T>(T paramter, string ctrl, uint uid) where T : class
		{
			if (!this.IsConnected)
			{
				throw new NpCloudException(760, "ソケットに未接続");
			}
			NpCloudServerData npCloudServerData = new NpCloudServerData();
			npCloudServerData.ctrl = ctrl;
			npCloudServerData.content = paramter;
			npCloudServerData.value = NpCloudValueType.send;
			npCloudServerData.type = NpCloudType.common;
			return this.SocketInstance.SequenceRequest(uid, npCloudServerData);
		}

		public void Request(NpCloudServerData parameter)
		{
			if (!this.IsConnected)
			{
				throw new NpCloudException(760, "ソケットに未接続");
			}
			this.SocketInstance.Request(parameter);
		}
	}
}
