using System;
using System.Collections.Generic;
using System.Text;

namespace UnityEngine.Networking
{
	public class NetworkConnection : IDisposable
	{
		private ChannelBuffer[] m_Channels;

		private List<PlayerController> m_PlayerControllers = new List<PlayerController>();

		private NetworkMessage m_NetMsg = new NetworkMessage();

		private HashSet<NetworkIdentity> m_VisList = new HashSet<NetworkIdentity>();

		private NetworkWriter m_Writer;

		private Dictionary<short, NetworkMessageDelegate> m_MessageHandlersDict;

		private NetworkMessageHandlers m_MessageHandlers;

		private HashSet<NetworkInstanceId> m_ClientOwnedObjects;

		private NetworkMessage m_MessageInfo = new NetworkMessage();

		private const int k_MaxMessageLogSize = 150;

		private NetworkError error;

		public int hostId = -1;

		public int connectionId = -1;

		public bool isReady;

		public string address;

		public float lastMessageTime;

		public bool logNetworkMessages = false;

		private Dictionary<short, NetworkConnection.PacketStat> m_PacketStats = new Dictionary<short, NetworkConnection.PacketStat>();

		private bool m_Disposed;

		public NetworkConnection()
		{
			this.m_Writer = new NetworkWriter();
		}

		internal HashSet<NetworkIdentity> visList
		{
			get
			{
				return this.m_VisList;
			}
		}

		public List<PlayerController> playerControllers
		{
			get
			{
				return this.m_PlayerControllers;
			}
		}

		public HashSet<NetworkInstanceId> clientOwnedObjects
		{
			get
			{
				return this.m_ClientOwnedObjects;
			}
		}

		public bool isConnected
		{
			get
			{
				return this.hostId != -1;
			}
		}

		public NetworkError lastError
		{
			get
			{
				return this.error;
			}
			internal set
			{
				this.error = value;
			}
		}

		internal Dictionary<short, NetworkConnection.PacketStat> packetStats
		{
			get
			{
				return this.m_PacketStats;
			}
		}

		public virtual void Initialize(string networkAddress, int networkHostId, int networkConnectionId, HostTopology hostTopology)
		{
			this.m_Writer = new NetworkWriter();
			this.address = networkAddress;
			this.hostId = networkHostId;
			this.connectionId = networkConnectionId;
			int channelCount = hostTopology.DefaultConfig.ChannelCount;
			int packetSize = (int)hostTopology.DefaultConfig.PacketSize;
			if (hostTopology.DefaultConfig.UsePlatformSpecificProtocols && Application.platform != RuntimePlatform.PS4 && Application.platform != RuntimePlatform.PSP2)
			{
				throw new ArgumentOutOfRangeException("Platform specific protocols are not supported on this platform");
			}
			this.m_Channels = new ChannelBuffer[channelCount];
			for (int i = 0; i < channelCount; i++)
			{
				ChannelQOS channelQOS = hostTopology.DefaultConfig.Channels[i];
				int bufferSize = packetSize;
				if (channelQOS.QOS == QosType.ReliableFragmented || channelQOS.QOS == QosType.UnreliableFragmented)
				{
					bufferSize = (int)(hostTopology.DefaultConfig.FragmentSize * 128);
				}
				this.m_Channels[i] = new ChannelBuffer(this, bufferSize, (byte)i, NetworkConnection.IsReliableQoS(channelQOS.QOS), NetworkConnection.IsSequencedQoS(channelQOS.QOS));
			}
		}

		~NetworkConnection()
		{
			this.Dispose(false);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this.m_Disposed && this.m_Channels != null)
			{
				for (int i = 0; i < this.m_Channels.Length; i++)
				{
					this.m_Channels[i].Dispose();
				}
			}
			this.m_Channels = null;
			if (this.m_ClientOwnedObjects != null)
			{
				foreach (NetworkInstanceId netId in this.m_ClientOwnedObjects)
				{
					GameObject gameObject = NetworkServer.FindLocalObject(netId);
					if (gameObject != null)
					{
						gameObject.GetComponent<NetworkIdentity>().ClearClientOwner();
					}
				}
			}
			this.m_ClientOwnedObjects = null;
			this.m_Disposed = true;
		}

		private static bool IsSequencedQoS(QosType qos)
		{
			return qos == QosType.ReliableSequenced || qos == QosType.UnreliableSequenced;
		}

		private static bool IsReliableQoS(QosType qos)
		{
			return qos == QosType.Reliable || qos == QosType.ReliableFragmented || qos == QosType.ReliableSequenced || qos == QosType.ReliableStateUpdate;
		}

		public bool SetChannelOption(int channelId, ChannelOption option, int value)
		{
			return this.m_Channels != null && channelId >= 0 && channelId < this.m_Channels.Length && this.m_Channels[channelId].SetOption(option, value);
		}

		public void Disconnect()
		{
			this.address = "";
			this.isReady = false;
			ClientScene.HandleClientDisconnect(this);
			if (this.hostId != -1)
			{
				byte b;
				NetworkTransport.Disconnect(this.hostId, this.connectionId, out b);
				this.RemoveObservers();
			}
		}

		internal void SetHandlers(NetworkMessageHandlers handlers)
		{
			this.m_MessageHandlers = handlers;
			this.m_MessageHandlersDict = handlers.GetHandlers();
		}

		public bool CheckHandler(short msgType)
		{
			return this.m_MessageHandlersDict.ContainsKey(msgType);
		}

		public bool InvokeHandlerNoData(short msgType)
		{
			return this.InvokeHandler(msgType, null, 0);
		}

		public bool InvokeHandler(short msgType, NetworkReader reader, int channelId)
		{
			bool result;
			if (this.m_MessageHandlersDict.ContainsKey(msgType))
			{
				this.m_MessageInfo.msgType = msgType;
				this.m_MessageInfo.conn = this;
				this.m_MessageInfo.reader = reader;
				this.m_MessageInfo.channelId = channelId;
				NetworkMessageDelegate networkMessageDelegate = this.m_MessageHandlersDict[msgType];
				if (networkMessageDelegate == null)
				{
					if (LogFilter.logError)
					{
						Debug.LogError("NetworkConnection InvokeHandler no handler for " + msgType);
					}
					result = false;
				}
				else
				{
					networkMessageDelegate(this.m_MessageInfo);
					result = true;
				}
			}
			else
			{
				result = false;
			}
			return result;
		}

		public bool InvokeHandler(NetworkMessage netMsg)
		{
			bool result;
			if (this.m_MessageHandlersDict.ContainsKey(netMsg.msgType))
			{
				NetworkMessageDelegate networkMessageDelegate = this.m_MessageHandlersDict[netMsg.msgType];
				networkMessageDelegate(netMsg);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		internal void HandleFragment(NetworkReader reader, int channelId)
		{
			if (channelId >= 0 && channelId < this.m_Channels.Length)
			{
				ChannelBuffer channelBuffer = this.m_Channels[channelId];
				if (channelBuffer.HandleFragment(reader))
				{
					NetworkReader networkReader = new NetworkReader(channelBuffer.fragmentBuffer.AsArraySegment().Array);
					networkReader.ReadInt16();
					short msgType = networkReader.ReadInt16();
					this.InvokeHandler(msgType, networkReader, channelId);
				}
			}
		}

		public void RegisterHandler(short msgType, NetworkMessageDelegate handler)
		{
			this.m_MessageHandlers.RegisterHandler(msgType, handler);
		}

		public void UnregisterHandler(short msgType)
		{
			this.m_MessageHandlers.UnregisterHandler(msgType);
		}

		internal void SetPlayerController(PlayerController player)
		{
			while ((int)player.playerControllerId >= this.m_PlayerControllers.Count)
			{
				this.m_PlayerControllers.Add(new PlayerController());
			}
			this.m_PlayerControllers[(int)player.playerControllerId] = player;
		}

		internal void RemovePlayerController(short playerControllerId)
		{
			for (int i = this.m_PlayerControllers.Count; i >= 0; i--)
			{
				if ((int)playerControllerId == i && playerControllerId == this.m_PlayerControllers[i].playerControllerId)
				{
					this.m_PlayerControllers[i] = new PlayerController();
					return;
				}
			}
			if (LogFilter.logError)
			{
				Debug.LogError("RemovePlayer player at playerControllerId " + playerControllerId + " not found");
				return;
			}
		}

		internal bool GetPlayerController(short playerControllerId, out PlayerController playerController)
		{
			playerController = null;
			bool result;
			if (this.playerControllers.Count > 0)
			{
				for (int i = 0; i < this.playerControllers.Count; i++)
				{
					if (this.playerControllers[i].IsValid && this.playerControllers[i].playerControllerId == playerControllerId)
					{
						playerController = this.playerControllers[i];
						return true;
					}
				}
				result = false;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public void FlushChannels()
		{
			if (this.m_Channels != null)
			{
				for (int i = 0; i < this.m_Channels.Length; i++)
				{
					this.m_Channels[i].CheckInternalBuffer();
				}
			}
		}

		public void SetMaxDelay(float seconds)
		{
			if (this.m_Channels != null)
			{
				for (int i = 0; i < this.m_Channels.Length; i++)
				{
					this.m_Channels[i].maxDelay = seconds;
				}
			}
		}

		public virtual bool Send(short msgType, MessageBase msg)
		{
			return this.SendByChannel(msgType, msg, 0);
		}

		public virtual bool SendUnreliable(short msgType, MessageBase msg)
		{
			return this.SendByChannel(msgType, msg, 1);
		}

		public virtual bool SendByChannel(short msgType, MessageBase msg, int channelId)
		{
			this.m_Writer.StartMessage(msgType);
			msg.Serialize(this.m_Writer);
			this.m_Writer.FinishMessage();
			return this.SendWriter(this.m_Writer, channelId);
		}

		public virtual bool SendBytes(byte[] bytes, int numBytes, int channelId)
		{
			if (this.logNetworkMessages)
			{
				this.LogSend(bytes);
			}
			return this.CheckChannel(channelId) && this.m_Channels[channelId].SendBytes(bytes, numBytes);
		}

		public virtual bool SendWriter(NetworkWriter writer, int channelId)
		{
			if (this.logNetworkMessages)
			{
				this.LogSend(writer.ToArray());
			}
			return this.CheckChannel(channelId) && this.m_Channels[channelId].SendWriter(writer);
		}

		private void LogSend(byte[] bytes)
		{
			NetworkReader networkReader = new NetworkReader(bytes);
			ushort num = networkReader.ReadUInt16();
			ushort num2 = networkReader.ReadUInt16();
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 4; i < (int)(4 + num); i++)
			{
				stringBuilder.AppendFormat("{0:X2}", bytes[i]);
				if (i > 150)
				{
					break;
				}
			}
			Debug.Log(string.Concat(new object[]
			{
				"ConnectionSend con:",
				this.connectionId,
				" bytes:",
				num,
				" msgId:",
				num2,
				" ",
				stringBuilder
			}));
		}

		private bool CheckChannel(int channelId)
		{
			bool result;
			if (this.m_Channels == null)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("Channels not initialized sending on id '" + channelId);
				}
				result = false;
			}
			else if (channelId < 0 || channelId >= this.m_Channels.Length)
			{
				if (LogFilter.logError)
				{
					Debug.LogError(string.Concat(new object[]
					{
						"Invalid channel when sending buffered data, '",
						channelId,
						"'. Current channel count is ",
						this.m_Channels.Length
					}));
				}
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		public void ResetStats()
		{
		}

		protected void HandleBytes(byte[] buffer, int receivedSize, int channelId)
		{
			NetworkReader reader = new NetworkReader(buffer);
			this.HandleReader(reader, receivedSize, channelId);
		}

		protected void HandleReader(NetworkReader reader, int receivedSize, int channelId)
		{
			while ((ulong)reader.Position < (ulong)((long)receivedSize))
			{
				ushort num = reader.ReadUInt16();
				short num2 = reader.ReadInt16();
				byte[] array = reader.ReadBytes((int)num);
				NetworkReader reader2 = new NetworkReader(array);
				if (this.logNetworkMessages)
				{
					StringBuilder stringBuilder = new StringBuilder();
					for (int i = 0; i < (int)num; i++)
					{
						stringBuilder.AppendFormat("{0:X2}", array[i]);
						if (i > 150)
						{
							break;
						}
					}
					Debug.Log(string.Concat(new object[]
					{
						"ConnectionRecv con:",
						this.connectionId,
						" bytes:",
						num,
						" msgId:",
						num2,
						" ",
						stringBuilder
					}));
				}
				NetworkMessageDelegate networkMessageDelegate = null;
				if (this.m_MessageHandlersDict.ContainsKey(num2))
				{
					networkMessageDelegate = this.m_MessageHandlersDict[num2];
				}
				if (networkMessageDelegate == null)
				{
					if (LogFilter.logError)
					{
						Debug.LogError(string.Concat(new object[]
						{
							"Unknown message ID ",
							num2,
							" connId:",
							this.connectionId
						}));
					}
					break;
				}
				this.m_NetMsg.msgType = num2;
				this.m_NetMsg.reader = reader2;
				this.m_NetMsg.conn = this;
				this.m_NetMsg.channelId = channelId;
				networkMessageDelegate(this.m_NetMsg);
				this.lastMessageTime = Time.time;
			}
		}

		public virtual void GetStatsOut(out int numMsgs, out int numBufferedMsgs, out int numBytes, out int lastBufferedPerSecond)
		{
			numMsgs = 0;
			numBufferedMsgs = 0;
			numBytes = 0;
			lastBufferedPerSecond = 0;
			for (int i = 0; i < this.m_Channels.Length; i++)
			{
				ChannelBuffer channelBuffer = this.m_Channels[i];
				numMsgs += channelBuffer.numMsgsOut;
				numBufferedMsgs += channelBuffer.numBufferedMsgsOut;
				numBytes += channelBuffer.numBytesOut;
				lastBufferedPerSecond += channelBuffer.lastBufferedPerSecond;
			}
		}

		public virtual void GetStatsIn(out int numMsgs, out int numBytes)
		{
			numMsgs = 0;
			numBytes = 0;
			for (int i = 0; i < this.m_Channels.Length; i++)
			{
				ChannelBuffer channelBuffer = this.m_Channels[i];
				numMsgs += channelBuffer.numMsgsIn;
				numBytes += channelBuffer.numBytesIn;
			}
		}

		public override string ToString()
		{
			return string.Format("hostId: {0} connectionId: {1} isReady: {2} channel count: {3}", new object[]
			{
				this.hostId,
				this.connectionId,
				this.isReady,
				(this.m_Channels == null) ? 0 : this.m_Channels.Length
			});
		}

		internal void AddToVisList(NetworkIdentity uv)
		{
			this.m_VisList.Add(uv);
			NetworkServer.ShowForConnection(uv, this);
		}

		internal void RemoveFromVisList(NetworkIdentity uv, bool isDestroyed)
		{
			this.m_VisList.Remove(uv);
			if (!isDestroyed)
			{
				NetworkServer.HideForConnection(uv, this);
			}
		}

		internal void RemoveObservers()
		{
			foreach (NetworkIdentity networkIdentity in this.m_VisList)
			{
				networkIdentity.RemoveObserverInternal(this);
			}
			this.m_VisList.Clear();
		}

		public virtual void TransportReceive(byte[] bytes, int numBytes, int channelId)
		{
			this.HandleBytes(bytes, numBytes, channelId);
		}

		[Obsolete("TransportRecieve has been deprecated. Use TransportReceive instead (UnityUpgradable) -> TransportReceive(*)", false)]
		public virtual void TransportRecieve(byte[] bytes, int numBytes, int channelId)
		{
			this.TransportReceive(bytes, numBytes, channelId);
		}

		public virtual bool TransportSend(byte[] bytes, int numBytes, int channelId, out byte error)
		{
			return NetworkTransport.Send(this.hostId, this.connectionId, channelId, bytes, numBytes, out error);
		}

		internal void AddOwnedObject(NetworkIdentity obj)
		{
			if (this.m_ClientOwnedObjects == null)
			{
				this.m_ClientOwnedObjects = new HashSet<NetworkInstanceId>();
			}
			this.m_ClientOwnedObjects.Add(obj.netId);
		}

		internal void RemoveOwnedObject(NetworkIdentity obj)
		{
			if (this.m_ClientOwnedObjects != null)
			{
				this.m_ClientOwnedObjects.Remove(obj.netId);
			}
		}

		internal static void OnFragment(NetworkMessage netMsg)
		{
			netMsg.conn.HandleFragment(netMsg.reader, netMsg.channelId);
		}

		public class PacketStat
		{
			public short msgType;

			public int count;

			public int bytes;

			public PacketStat()
			{
				this.msgType = 0;
				this.count = 0;
				this.bytes = 0;
			}

			public PacketStat(NetworkConnection.PacketStat s)
			{
				this.msgType = s.msgType;
				this.count = s.count;
				this.bytes = s.bytes;
			}

			public override string ToString()
			{
				return string.Concat(new object[]
				{
					MsgType.MsgTypeToString(this.msgType),
					": count=",
					this.count,
					" bytes=",
					this.bytes
				});
			}
		}
	}
}
