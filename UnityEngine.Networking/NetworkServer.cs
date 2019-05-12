using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking
{
	public sealed class NetworkServer
	{
		private const int k_RemoveListInterval = 100;

		private static bool s_Active;

		private static volatile NetworkServer s_Instance;

		private static object s_Sync = new Object();

		private static bool m_DontListen;

		private bool m_LocalClientActive;

		private List<NetworkConnection> m_LocalConnectionsFakeList = new List<NetworkConnection>();

		private ULocalConnectionToClient m_LocalConnection;

		private NetworkScene m_NetworkScene;

		private HashSet<int> m_ExternalConnections;

		private NetworkServer.ServerSimpleWrapper m_SimpleServerSimple;

		private float m_MaxDelay = 0.1f;

		private HashSet<NetworkInstanceId> m_RemoveList;

		private int m_RemoveListCount;

		internal static ushort maxPacketSize;

		private static RemovePlayerMessage s_RemovePlayerMessage = new RemovePlayerMessage();

		private NetworkServer()
		{
			NetworkTransport.Init();
			if (LogFilter.logDev)
			{
				Debug.Log("NetworkServer Created version " + Version.Current);
			}
			this.m_RemoveList = new HashSet<NetworkInstanceId>();
			this.m_ExternalConnections = new HashSet<int>();
			this.m_NetworkScene = new NetworkScene();
			this.m_SimpleServerSimple = new NetworkServer.ServerSimpleWrapper(this);
		}

		public static List<NetworkConnection> localConnections
		{
			get
			{
				return NetworkServer.instance.m_LocalConnectionsFakeList;
			}
		}

		public static int listenPort
		{
			get
			{
				return NetworkServer.instance.m_SimpleServerSimple.listenPort;
			}
		}

		public static int serverHostId
		{
			get
			{
				return NetworkServer.instance.m_SimpleServerSimple.serverHostId;
			}
		}

		public static ReadOnlyCollection<NetworkConnection> connections
		{
			get
			{
				return NetworkServer.instance.m_SimpleServerSimple.connections;
			}
		}

		public static Dictionary<short, NetworkMessageDelegate> handlers
		{
			get
			{
				return NetworkServer.instance.m_SimpleServerSimple.handlers;
			}
		}

		public static HostTopology hostTopology
		{
			get
			{
				return NetworkServer.instance.m_SimpleServerSimple.hostTopology;
			}
		}

		public static Dictionary<NetworkInstanceId, NetworkIdentity> objects
		{
			get
			{
				return NetworkServer.instance.m_NetworkScene.localObjects;
			}
		}

		[Obsolete("Moved to NetworkMigrationManager")]
		public static bool sendPeerInfo
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public static bool dontListen
		{
			get
			{
				return NetworkServer.m_DontListen;
			}
			set
			{
				NetworkServer.m_DontListen = value;
			}
		}

		public static bool useWebSockets
		{
			get
			{
				return NetworkServer.instance.m_SimpleServerSimple.useWebSockets;
			}
			set
			{
				NetworkServer.instance.m_SimpleServerSimple.useWebSockets = value;
			}
		}

		internal static NetworkServer instance
		{
			get
			{
				if (NetworkServer.s_Instance == null)
				{
					object obj = NetworkServer.s_Sync;
					lock (obj)
					{
						if (NetworkServer.s_Instance == null)
						{
							NetworkServer.s_Instance = new NetworkServer();
						}
					}
				}
				return NetworkServer.s_Instance;
			}
		}

		public static bool active
		{
			get
			{
				return NetworkServer.s_Active;
			}
		}

		public static bool localClientActive
		{
			get
			{
				return NetworkServer.instance.m_LocalClientActive;
			}
		}

		public static int numChannels
		{
			get
			{
				return NetworkServer.instance.m_SimpleServerSimple.hostTopology.DefaultConfig.ChannelCount;
			}
		}

		public static float maxDelay
		{
			get
			{
				return NetworkServer.instance.m_MaxDelay;
			}
			set
			{
				NetworkServer.instance.InternalSetMaxDelay(value);
			}
		}

		public static Type networkConnectionClass
		{
			get
			{
				return NetworkServer.instance.m_SimpleServerSimple.networkConnectionClass;
			}
		}

		public static void SetNetworkConnectionClass<T>() where T : NetworkConnection
		{
			NetworkServer.instance.m_SimpleServerSimple.SetNetworkConnectionClass<T>();
		}

		public static bool Configure(ConnectionConfig config, int maxConnections)
		{
			return NetworkServer.instance.m_SimpleServerSimple.Configure(config, maxConnections);
		}

		public static bool Configure(HostTopology topology)
		{
			return NetworkServer.instance.m_SimpleServerSimple.Configure(topology);
		}

		public static void Reset()
		{
			NetworkTransport.Shutdown();
			NetworkTransport.Init();
			NetworkServer.s_Instance = null;
			NetworkServer.s_Active = false;
		}

		public static void Shutdown()
		{
			if (NetworkServer.s_Instance != null)
			{
				NetworkServer.s_Instance.InternalDisconnectAll();
				if (!NetworkServer.m_DontListen)
				{
					NetworkServer.s_Instance.m_SimpleServerSimple.Stop();
				}
				NetworkServer.s_Instance = null;
			}
			NetworkServer.m_DontListen = false;
			NetworkServer.s_Active = false;
		}

		public static bool Listen(MatchInfo matchInfo, int listenPort)
		{
			if (!matchInfo.usingRelay)
			{
				return NetworkServer.instance.InternalListen(null, listenPort);
			}
			NetworkServer.instance.InternalListenRelay(matchInfo.address, matchInfo.port, matchInfo.networkId, Utility.GetSourceID(), matchInfo.nodeId);
			return true;
		}

		internal void RegisterMessageHandlers()
		{
			this.m_SimpleServerSimple.RegisterHandlerSafe(35, new NetworkMessageDelegate(NetworkServer.OnClientReadyMessage));
			this.m_SimpleServerSimple.RegisterHandlerSafe(5, new NetworkMessageDelegate(NetworkServer.OnCommandMessage));
			this.m_SimpleServerSimple.RegisterHandlerSafe(6, new NetworkMessageDelegate(NetworkTransform.HandleTransform));
			this.m_SimpleServerSimple.RegisterHandlerSafe(16, new NetworkMessageDelegate(NetworkTransformChild.HandleChildTransform));
			this.m_SimpleServerSimple.RegisterHandlerSafe(38, new NetworkMessageDelegate(NetworkServer.OnRemovePlayerMessage));
			this.m_SimpleServerSimple.RegisterHandlerSafe(40, new NetworkMessageDelegate(NetworkAnimator.OnAnimationServerMessage));
			this.m_SimpleServerSimple.RegisterHandlerSafe(41, new NetworkMessageDelegate(NetworkAnimator.OnAnimationParametersServerMessage));
			this.m_SimpleServerSimple.RegisterHandlerSafe(42, new NetworkMessageDelegate(NetworkAnimator.OnAnimationTriggerServerMessage));
			NetworkServer.maxPacketSize = NetworkServer.hostTopology.DefaultConfig.PacketSize;
		}

		public static void ListenRelay(string relayIp, int relayPort, NetworkID netGuid, SourceID sourceId, NodeID nodeId)
		{
			NetworkServer.instance.InternalListenRelay(relayIp, relayPort, netGuid, sourceId, nodeId);
		}

		private void InternalListenRelay(string relayIp, int relayPort, NetworkID netGuid, SourceID sourceId, NodeID nodeId)
		{
			this.m_SimpleServerSimple.ListenRelay(relayIp, relayPort, netGuid, sourceId, nodeId);
			NetworkServer.s_Active = true;
			this.RegisterMessageHandlers();
		}

		public static bool Listen(int serverPort)
		{
			return NetworkServer.instance.InternalListen(null, serverPort);
		}

		public static bool Listen(string ipAddress, int serverPort)
		{
			return NetworkServer.instance.InternalListen(ipAddress, serverPort);
		}

		internal bool InternalListen(string ipAddress, int serverPort)
		{
			if (NetworkServer.m_DontListen)
			{
				this.m_SimpleServerSimple.Initialize();
			}
			else if (!this.m_SimpleServerSimple.Listen(ipAddress, serverPort))
			{
				return false;
			}
			NetworkServer.maxPacketSize = NetworkServer.hostTopology.DefaultConfig.PacketSize;
			NetworkServer.s_Active = true;
			this.RegisterMessageHandlers();
			return true;
		}

		public static NetworkClient BecomeHost(NetworkClient oldClient, int port, MatchInfo matchInfo, int oldConnectionId, PeerInfoMessage[] peers)
		{
			return NetworkServer.instance.BecomeHostInternal(oldClient, port, matchInfo, oldConnectionId, peers);
		}

		internal NetworkClient BecomeHostInternal(NetworkClient oldClient, int port, MatchInfo matchInfo, int oldConnectionId, PeerInfoMessage[] peers)
		{
			if (NetworkServer.s_Active)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("BecomeHost already a server.");
				}
				return null;
			}
			if (!NetworkClient.active)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("BecomeHost NetworkClient not active.");
				}
				return null;
			}
			NetworkServer.Configure(NetworkServer.hostTopology);
			if (matchInfo == null)
			{
				if (LogFilter.logDev)
				{
					Debug.Log("BecomeHost Listen on " + port);
				}
				if (!NetworkServer.Listen(port))
				{
					if (LogFilter.logError)
					{
						Debug.LogError("BecomeHost bind failed.");
					}
					return null;
				}
			}
			else
			{
				if (LogFilter.logDev)
				{
					Debug.Log("BecomeHost match:" + matchInfo.networkId);
				}
				NetworkServer.ListenRelay(matchInfo.address, matchInfo.port, matchInfo.networkId, Utility.GetSourceID(), matchInfo.nodeId);
			}
			foreach (NetworkIdentity networkIdentity in ClientScene.objects.Values)
			{
				if (!(networkIdentity == null) && !(networkIdentity.gameObject == null))
				{
					NetworkIdentity.AddNetworkId(networkIdentity.netId.Value);
					this.m_NetworkScene.SetLocalObject(networkIdentity.netId, networkIdentity.gameObject, false, false);
					networkIdentity.OnStartServer(true);
				}
			}
			if (LogFilter.logDev)
			{
				Debug.Log("NetworkServer BecomeHost done. oldConnectionId:" + oldConnectionId);
			}
			this.RegisterMessageHandlers();
			if (!NetworkClient.RemoveClient(oldClient) && LogFilter.logError)
			{
				Debug.LogError("BecomeHost failed to remove client");
			}
			if (LogFilter.logDev)
			{
				Debug.Log("BecomeHost localClient ready");
			}
			NetworkClient networkClient = ClientScene.ReconnectLocalServer();
			ClientScene.Ready(networkClient.connection);
			ClientScene.SetReconnectId(oldConnectionId, peers);
			ClientScene.AddPlayer(ClientScene.readyConnection, 0);
			return networkClient;
		}

		private void InternalSetMaxDelay(float seconds)
		{
			for (int i = 0; i < NetworkServer.connections.Count; i++)
			{
				NetworkConnection networkConnection = NetworkServer.connections[i];
				if (networkConnection != null)
				{
					networkConnection.SetMaxDelay(seconds);
				}
			}
			this.m_MaxDelay = seconds;
		}

		internal int AddLocalClient(LocalClient localClient)
		{
			if (this.m_LocalConnectionsFakeList.Count != 0)
			{
				Debug.LogError("Local Connection already exists");
				return -1;
			}
			this.m_LocalConnection = new ULocalConnectionToClient(localClient);
			this.m_LocalConnection.connectionId = 0;
			this.m_SimpleServerSimple.SetConnectionAtIndex(this.m_LocalConnection);
			this.m_LocalConnectionsFakeList.Add(this.m_LocalConnection);
			this.m_LocalConnection.InvokeHandlerNoData(32);
			return 0;
		}

		internal void RemoveLocalClient(NetworkConnection localClientConnection)
		{
			for (int i = 0; i < this.m_LocalConnectionsFakeList.Count; i++)
			{
				if (this.m_LocalConnectionsFakeList[i].connectionId == localClientConnection.connectionId)
				{
					this.m_LocalConnectionsFakeList.RemoveAt(i);
					break;
				}
			}
			if (this.m_LocalConnection != null)
			{
				this.m_LocalConnection.Disconnect();
				this.m_LocalConnection.Dispose();
				this.m_LocalConnection = null;
			}
			this.m_LocalClientActive = false;
			this.m_SimpleServerSimple.RemoveConnectionAtIndex(0);
		}

		internal void SetLocalObjectOnServer(NetworkInstanceId netId, GameObject obj)
		{
			if (LogFilter.logDev)
			{
				Debug.Log(string.Concat(new object[]
				{
					"SetLocalObjectOnServer ",
					netId,
					" ",
					obj
				}));
			}
			this.m_NetworkScene.SetLocalObject(netId, obj, false, true);
		}

		internal void ActivateLocalClientScene()
		{
			if (this.m_LocalClientActive)
			{
				return;
			}
			this.m_LocalClientActive = true;
			foreach (NetworkIdentity networkIdentity in NetworkServer.objects.Values)
			{
				if (!networkIdentity.isClient)
				{
					if (LogFilter.logDev)
					{
						Debug.Log(string.Concat(new object[]
						{
							"ActivateClientScene ",
							networkIdentity.netId,
							" ",
							networkIdentity.gameObject
						}));
					}
					ClientScene.SetLocalObject(networkIdentity.netId, networkIdentity.gameObject);
					networkIdentity.OnStartClient();
				}
			}
		}

		public static bool SendToAll(short msgType, MessageBase msg)
		{
			if (LogFilter.logDev)
			{
				Debug.Log("Server.SendToAll msgType:" + msgType);
			}
			bool flag = true;
			for (int i = 0; i < NetworkServer.connections.Count; i++)
			{
				NetworkConnection networkConnection = NetworkServer.connections[i];
				if (networkConnection != null)
				{
					flag &= networkConnection.Send(msgType, msg);
				}
			}
			return flag;
		}

		private static bool SendToObservers(GameObject contextObj, short msgType, MessageBase msg)
		{
			if (LogFilter.logDev)
			{
				Debug.Log("Server.SendToObservers id:" + msgType);
			}
			bool flag = true;
			NetworkIdentity component = contextObj.GetComponent<NetworkIdentity>();
			if (component == null || component.observers == null)
			{
				return false;
			}
			int count = component.observers.Count;
			for (int i = 0; i < count; i++)
			{
				NetworkConnection networkConnection = component.observers[i];
				flag &= networkConnection.Send(msgType, msg);
			}
			return flag;
		}

		public static bool SendToReady(GameObject contextObj, short msgType, MessageBase msg)
		{
			if (LogFilter.logDev)
			{
				Debug.Log("Server.SendToReady id:" + msgType);
			}
			if (contextObj == null)
			{
				for (int i = 0; i < NetworkServer.connections.Count; i++)
				{
					NetworkConnection networkConnection = NetworkServer.connections[i];
					if (networkConnection != null && networkConnection.isReady)
					{
						networkConnection.Send(msgType, msg);
					}
				}
				return true;
			}
			bool flag = true;
			NetworkIdentity component = contextObj.GetComponent<NetworkIdentity>();
			if (component == null || component.observers == null)
			{
				return false;
			}
			int count = component.observers.Count;
			for (int j = 0; j < count; j++)
			{
				NetworkConnection networkConnection2 = component.observers[j];
				if (networkConnection2.isReady)
				{
					flag &= networkConnection2.Send(msgType, msg);
				}
			}
			return flag;
		}

		public static void SendWriterToReady(GameObject contextObj, NetworkWriter writer, int channelId)
		{
			if (writer.AsArraySegment().Count > 32767)
			{
				throw new UnityException("NetworkWriter used buffer is too big!");
			}
			NetworkServer.SendBytesToReady(contextObj, writer.AsArraySegment().Array, writer.AsArraySegment().Count, channelId);
		}

		public static void SendBytesToReady(GameObject contextObj, byte[] buffer, int numBytes, int channelId)
		{
			if (contextObj == null)
			{
				bool flag = true;
				for (int i = 0; i < NetworkServer.connections.Count; i++)
				{
					NetworkConnection networkConnection = NetworkServer.connections[i];
					if (networkConnection != null && networkConnection.isReady && !networkConnection.SendBytes(buffer, numBytes, channelId))
					{
						flag = false;
					}
				}
				if (!flag && LogFilter.logWarn)
				{
					Debug.LogWarning("SendBytesToReady failed");
				}
				return;
			}
			NetworkIdentity component = contextObj.GetComponent<NetworkIdentity>();
			try
			{
				bool flag2 = true;
				int count = component.observers.Count;
				for (int j = 0; j < count; j++)
				{
					NetworkConnection networkConnection2 = component.observers[j];
					if (networkConnection2.isReady)
					{
						if (!networkConnection2.SendBytes(buffer, numBytes, channelId))
						{
							flag2 = false;
						}
					}
				}
				if (!flag2 && LogFilter.logWarn)
				{
					Debug.LogWarning("SendBytesToReady failed for " + contextObj);
				}
			}
			catch (NullReferenceException)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("SendBytesToReady object " + contextObj + " has not been spawned");
				}
			}
		}

		public static void SendBytesToPlayer(GameObject player, byte[] buffer, int numBytes, int channelId)
		{
			for (int i = 0; i < NetworkServer.connections.Count; i++)
			{
				NetworkConnection networkConnection = NetworkServer.connections[i];
				if (networkConnection != null)
				{
					for (int j = 0; j < networkConnection.playerControllers.Count; j++)
					{
						if (networkConnection.playerControllers[j].IsValid && networkConnection.playerControllers[j].gameObject == player)
						{
							networkConnection.SendBytes(buffer, numBytes, channelId);
							break;
						}
					}
				}
			}
		}

		public static bool SendUnreliableToAll(short msgType, MessageBase msg)
		{
			if (LogFilter.logDev)
			{
				Debug.Log("Server.SendUnreliableToAll msgType:" + msgType);
			}
			bool flag = true;
			for (int i = 0; i < NetworkServer.connections.Count; i++)
			{
				NetworkConnection networkConnection = NetworkServer.connections[i];
				if (networkConnection != null)
				{
					flag &= networkConnection.SendUnreliable(msgType, msg);
				}
			}
			return flag;
		}

		public static bool SendUnreliableToReady(GameObject contextObj, short msgType, MessageBase msg)
		{
			if (LogFilter.logDev)
			{
				Debug.Log("Server.SendUnreliableToReady id:" + msgType);
			}
			if (contextObj == null)
			{
				for (int i = 0; i < NetworkServer.connections.Count; i++)
				{
					NetworkConnection networkConnection = NetworkServer.connections[i];
					if (networkConnection != null && networkConnection.isReady)
					{
						networkConnection.SendUnreliable(msgType, msg);
					}
				}
				return true;
			}
			bool flag = true;
			NetworkIdentity component = contextObj.GetComponent<NetworkIdentity>();
			int count = component.observers.Count;
			for (int j = 0; j < count; j++)
			{
				NetworkConnection networkConnection2 = component.observers[j];
				if (networkConnection2.isReady)
				{
					flag &= networkConnection2.SendUnreliable(msgType, msg);
				}
			}
			return flag;
		}

		public static bool SendByChannelToAll(short msgType, MessageBase msg, int channelId)
		{
			if (LogFilter.logDev)
			{
				Debug.Log("Server.SendByChannelToAll id:" + msgType);
			}
			bool flag = true;
			for (int i = 0; i < NetworkServer.connections.Count; i++)
			{
				NetworkConnection networkConnection = NetworkServer.connections[i];
				if (networkConnection != null)
				{
					flag &= networkConnection.SendByChannel(msgType, msg, channelId);
				}
			}
			return flag;
		}

		public static bool SendByChannelToReady(GameObject contextObj, short msgType, MessageBase msg, int channelId)
		{
			if (LogFilter.logDev)
			{
				Debug.Log("Server.SendByChannelToReady msgType:" + msgType);
			}
			if (contextObj == null)
			{
				for (int i = 0; i < NetworkServer.connections.Count; i++)
				{
					NetworkConnection networkConnection = NetworkServer.connections[i];
					if (networkConnection != null && networkConnection.isReady)
					{
						networkConnection.SendByChannel(msgType, msg, channelId);
					}
				}
				return true;
			}
			bool flag = true;
			NetworkIdentity component = contextObj.GetComponent<NetworkIdentity>();
			int count = component.observers.Count;
			for (int j = 0; j < count; j++)
			{
				NetworkConnection networkConnection2 = component.observers[j];
				if (networkConnection2.isReady)
				{
					flag &= networkConnection2.SendByChannel(msgType, msg, channelId);
				}
			}
			return flag;
		}

		public static void DisconnectAll()
		{
			NetworkServer.instance.InternalDisconnectAll();
		}

		internal void InternalDisconnectAll()
		{
			this.m_SimpleServerSimple.DisconnectAllConnections();
			if (this.m_LocalConnection != null)
			{
				this.m_LocalConnection.Disconnect();
				this.m_LocalConnection.Dispose();
				this.m_LocalConnection = null;
			}
			NetworkServer.s_Active = false;
			this.m_LocalClientActive = false;
		}

		internal static void Update()
		{
			if (NetworkServer.s_Instance != null)
			{
				NetworkServer.s_Instance.InternalUpdate();
			}
		}

		private void UpdateServerObjects()
		{
			foreach (NetworkIdentity networkIdentity in NetworkServer.objects.Values)
			{
				try
				{
					networkIdentity.UNetUpdate();
				}
				catch (NullReferenceException)
				{
				}
				catch (MissingReferenceException)
				{
				}
			}
			if (this.m_RemoveListCount++ % 100 == 0)
			{
				this.CheckForNullObjects();
			}
		}

		private void CheckForNullObjects()
		{
			foreach (NetworkInstanceId networkInstanceId in NetworkServer.objects.Keys)
			{
				NetworkIdentity networkIdentity = NetworkServer.objects[networkInstanceId];
				if (networkIdentity == null || networkIdentity.gameObject == null)
				{
					this.m_RemoveList.Add(networkInstanceId);
				}
			}
			if (this.m_RemoveList.Count > 0)
			{
				foreach (NetworkInstanceId key in this.m_RemoveList)
				{
					NetworkServer.objects.Remove(key);
				}
				this.m_RemoveList.Clear();
			}
		}

		internal void InternalUpdate()
		{
			this.m_SimpleServerSimple.Update();
			if (NetworkServer.m_DontListen)
			{
				this.m_SimpleServerSimple.UpdateConnections();
			}
			this.UpdateServerObjects();
		}

		private void OnConnected(NetworkConnection conn)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("Server accepted client:" + conn.connectionId);
			}
			conn.SetMaxDelay(this.m_MaxDelay);
			conn.InvokeHandlerNoData(32);
			NetworkServer.SendCrc(conn);
		}

		private void OnDisconnected(NetworkConnection conn)
		{
			conn.InvokeHandlerNoData(33);
			for (int i = 0; i < conn.playerControllers.Count; i++)
			{
				if (conn.playerControllers[i].gameObject != null && LogFilter.logWarn)
				{
					Debug.LogWarning("Player not destroyed when connection disconnected.");
				}
			}
			if (LogFilter.logDebug)
			{
				Debug.Log("Server lost client:" + conn.connectionId);
			}
			conn.RemoveObservers();
			conn.Dispose();
		}

		private void OnData(NetworkConnection conn, int receivedSize, int channelId)
		{
			conn.TransportRecieve(this.m_SimpleServerSimple.messageBuffer, receivedSize, channelId);
		}

		private void GenerateConnectError(int error)
		{
			if (LogFilter.logError)
			{
				Debug.LogError("UNet Server Connect Error: " + error);
			}
			this.GenerateError(null, error);
		}

		private void GenerateDataError(NetworkConnection conn, int error)
		{
			if (LogFilter.logError)
			{
				Debug.LogError("UNet Server Data Error: " + (NetworkError)error);
			}
			this.GenerateError(conn, error);
		}

		private void GenerateDisconnectError(NetworkConnection conn, int error)
		{
			if (LogFilter.logError)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"UNet Server Disconnect Error: ",
					(NetworkError)error,
					" conn:[",
					conn,
					"]:",
					conn.connectionId
				}));
			}
			this.GenerateError(conn, error);
		}

		private void GenerateError(NetworkConnection conn, int error)
		{
			if (NetworkServer.handlers.ContainsKey(34))
			{
				ErrorMessage errorMessage = new ErrorMessage();
				errorMessage.errorCode = error;
				NetworkWriter writer = new NetworkWriter();
				errorMessage.Serialize(writer);
				NetworkReader reader = new NetworkReader(writer);
				conn.InvokeHandler(34, reader, 0);
			}
		}

		public static void RegisterHandler(short msgType, NetworkMessageDelegate handler)
		{
			NetworkServer.instance.m_SimpleServerSimple.RegisterHandler(msgType, handler);
		}

		public static void UnregisterHandler(short msgType)
		{
			NetworkServer.instance.m_SimpleServerSimple.UnregisterHandler(msgType);
		}

		public static void ClearHandlers()
		{
			NetworkServer.instance.m_SimpleServerSimple.ClearHandlers();
		}

		public static void ClearSpawners()
		{
			NetworkScene.ClearSpawners();
		}

		public static void GetStatsOut(out int numMsgs, out int numBufferedMsgs, out int numBytes, out int lastBufferedPerSecond)
		{
			numMsgs = 0;
			numBufferedMsgs = 0;
			numBytes = 0;
			lastBufferedPerSecond = 0;
			for (int i = 0; i < NetworkServer.connections.Count; i++)
			{
				NetworkConnection networkConnection = NetworkServer.connections[i];
				if (networkConnection != null)
				{
					int num;
					int num2;
					int num3;
					int num4;
					networkConnection.GetStatsOut(out num, out num2, out num3, out num4);
					numMsgs += num;
					numBufferedMsgs += num2;
					numBytes += num3;
					lastBufferedPerSecond += num4;
				}
			}
		}

		public static void GetStatsIn(out int numMsgs, out int numBytes)
		{
			numMsgs = 0;
			numBytes = 0;
			for (int i = 0; i < NetworkServer.connections.Count; i++)
			{
				NetworkConnection networkConnection = NetworkServer.connections[i];
				if (networkConnection != null)
				{
					int num;
					int num2;
					networkConnection.GetStatsIn(out num, out num2);
					numMsgs += num;
					numBytes += num2;
				}
			}
		}

		public static void SendToClientOfPlayer(GameObject player, short msgType, MessageBase msg)
		{
			for (int i = 0; i < NetworkServer.connections.Count; i++)
			{
				NetworkConnection networkConnection = NetworkServer.connections[i];
				if (networkConnection != null)
				{
					for (int j = 0; j < networkConnection.playerControllers.Count; j++)
					{
						if (networkConnection.playerControllers[j].IsValid && networkConnection.playerControllers[j].gameObject == player)
						{
							networkConnection.Send(msgType, msg);
							return;
						}
					}
				}
			}
			if (LogFilter.logError)
			{
				Debug.LogError("Failed to send message to player object '" + player.name + ", not found in connection list");
			}
		}

		public static void SendToClient(int connectionId, short msgType, MessageBase msg)
		{
			if (connectionId < NetworkServer.connections.Count)
			{
				NetworkConnection networkConnection = NetworkServer.connections[connectionId];
				if (networkConnection != null)
				{
					networkConnection.Send(msgType, msg);
					return;
				}
			}
			if (LogFilter.logError)
			{
				Debug.LogError("Failed to send message to connection ID '" + connectionId + ", not found in connection list");
			}
		}

		public static bool ReplacePlayerForConnection(NetworkConnection conn, GameObject player, short playerControllerId, NetworkHash128 assetId)
		{
			NetworkIdentity networkIdentity;
			if (NetworkServer.GetNetworkIdentity(player, out networkIdentity))
			{
				networkIdentity.SetDynamicAssetId(assetId);
			}
			return NetworkServer.instance.InternalReplacePlayerForConnection(conn, player, playerControllerId);
		}

		public static bool ReplacePlayerForConnection(NetworkConnection conn, GameObject player, short playerControllerId)
		{
			return NetworkServer.instance.InternalReplacePlayerForConnection(conn, player, playerControllerId);
		}

		public static bool AddPlayerForConnection(NetworkConnection conn, GameObject player, short playerControllerId, NetworkHash128 assetId)
		{
			NetworkIdentity networkIdentity;
			if (NetworkServer.GetNetworkIdentity(player, out networkIdentity))
			{
				networkIdentity.SetDynamicAssetId(assetId);
			}
			return NetworkServer.instance.InternalAddPlayerForConnection(conn, player, playerControllerId);
		}

		public static bool AddPlayerForConnection(NetworkConnection conn, GameObject player, short playerControllerId)
		{
			return NetworkServer.instance.InternalAddPlayerForConnection(conn, player, playerControllerId);
		}

		internal bool InternalAddPlayerForConnection(NetworkConnection conn, GameObject playerGameObject, short playerControllerId)
		{
			NetworkIdentity networkIdentity;
			if (!NetworkServer.GetNetworkIdentity(playerGameObject, out networkIdentity))
			{
				if (LogFilter.logError)
				{
					Debug.Log("AddPlayer: playerGameObject has no NetworkIdentity. Please add a NetworkIdentity to " + playerGameObject);
				}
				return false;
			}
			if (!NetworkServer.CheckPlayerControllerIdForConnection(conn, playerControllerId))
			{
				return false;
			}
			PlayerController playerController = null;
			GameObject x = null;
			if (conn.GetPlayerController(playerControllerId, out playerController))
			{
				x = playerController.gameObject;
			}
			if (x != null)
			{
				if (LogFilter.logError)
				{
					Debug.Log("AddPlayer: player object already exists for playerControllerId of " + playerControllerId);
				}
				return false;
			}
			PlayerController playerController2 = new PlayerController(playerGameObject, playerControllerId);
			conn.SetPlayerController(playerController2);
			networkIdentity.SetConnectionToClient(conn, playerController2.playerControllerId);
			NetworkServer.SetClientReady(conn);
			if (this.SetupLocalPlayerForConnection(conn, networkIdentity, playerController2))
			{
				return true;
			}
			if (LogFilter.logDebug)
			{
				Debug.Log(string.Concat(new object[]
				{
					"Adding new playerGameObject object netId: ",
					playerGameObject.GetComponent<NetworkIdentity>().netId,
					" asset ID ",
					playerGameObject.GetComponent<NetworkIdentity>().assetId
				}));
			}
			NetworkServer.FinishPlayerForConnection(conn, networkIdentity, playerGameObject);
			if (networkIdentity.localPlayerAuthority)
			{
				networkIdentity.SetClientOwner(conn);
			}
			return true;
		}

		private static bool CheckPlayerControllerIdForConnection(NetworkConnection conn, short playerControllerId)
		{
			if (playerControllerId < 0)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("AddPlayer: playerControllerId of " + playerControllerId + " is negative");
				}
				return false;
			}
			if (playerControllerId > 32)
			{
				if (LogFilter.logError)
				{
					Debug.Log(string.Concat(new object[]
					{
						"AddPlayer: playerControllerId of ",
						playerControllerId,
						" is too high. max is ",
						32
					}));
				}
				return false;
			}
			if (playerControllerId > 16 && LogFilter.logWarn)
			{
				Debug.LogWarning("AddPlayer: playerControllerId of " + playerControllerId + " is unusually high");
			}
			return true;
		}

		private bool SetupLocalPlayerForConnection(NetworkConnection conn, NetworkIdentity uv, PlayerController newPlayerController)
		{
			if (LogFilter.logDev)
			{
				Debug.Log("NetworkServer SetupLocalPlayerForConnection netID:" + uv.netId);
			}
			ULocalConnectionToClient ulocalConnectionToClient = conn as ULocalConnectionToClient;
			if (ulocalConnectionToClient != null)
			{
				if (LogFilter.logDev)
				{
					Debug.Log("NetworkServer AddPlayer handling ULocalConnectionToClient");
				}
				if (uv.netId.IsEmpty())
				{
					uv.OnStartServer(true);
				}
				uv.RebuildObservers(true);
				this.SendSpawnMessage(uv, null);
				ulocalConnectionToClient.localClient.AddLocalPlayer(newPlayerController);
				uv.SetClientOwner(conn);
				uv.ForceAuthority(true);
				uv.SetLocalPlayer(newPlayerController.playerControllerId);
				return true;
			}
			return false;
		}

		private static void FinishPlayerForConnection(NetworkConnection conn, NetworkIdentity uv, GameObject playerGameObject)
		{
			if (uv.netId.IsEmpty())
			{
				NetworkServer.Spawn(playerGameObject);
			}
			conn.Send(4, new OwnerMessage
			{
				netId = uv.netId,
				playerControllerId = uv.playerControllerId
			});
		}

		internal bool InternalReplacePlayerForConnection(NetworkConnection conn, GameObject playerGameObject, short playerControllerId)
		{
			NetworkIdentity networkIdentity;
			if (!NetworkServer.GetNetworkIdentity(playerGameObject, out networkIdentity))
			{
				if (LogFilter.logError)
				{
					Debug.LogError("ReplacePlayer: playerGameObject has no NetworkIdentity. Please add a NetworkIdentity to " + playerGameObject);
				}
				return false;
			}
			if (!NetworkServer.CheckPlayerControllerIdForConnection(conn, playerControllerId))
			{
				return false;
			}
			if (LogFilter.logDev)
			{
				Debug.Log("NetworkServer ReplacePlayer");
			}
			PlayerController playerController;
			if (conn.GetPlayerController(playerControllerId, out playerController))
			{
				playerController.unetView.SetNotLocalPlayer();
			}
			PlayerController playerController2 = new PlayerController(playerGameObject, playerControllerId);
			conn.SetPlayerController(playerController2);
			networkIdentity.SetConnectionToClient(conn, playerController2.playerControllerId);
			if (LogFilter.logDev)
			{
				Debug.Log("NetworkServer ReplacePlayer setup local");
			}
			if (this.SetupLocalPlayerForConnection(conn, networkIdentity, playerController2))
			{
				return true;
			}
			if (LogFilter.logDebug)
			{
				Debug.Log(string.Concat(new object[]
				{
					"Replacing playerGameObject object netId: ",
					playerGameObject.GetComponent<NetworkIdentity>().netId,
					" asset ID ",
					playerGameObject.GetComponent<NetworkIdentity>().assetId
				}));
			}
			NetworkServer.FinishPlayerForConnection(conn, networkIdentity, playerGameObject);
			if (networkIdentity.localPlayerAuthority)
			{
				networkIdentity.SetClientOwner(conn);
			}
			return true;
		}

		private static bool GetNetworkIdentity(GameObject go, out NetworkIdentity view)
		{
			view = go.GetComponent<NetworkIdentity>();
			if (view == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("UNET failure. GameObject doesn't have NetworkIdentity.");
				}
				return false;
			}
			return true;
		}

		public static void SetClientReady(NetworkConnection conn)
		{
			NetworkServer.instance.SetClientReadyInternal(conn);
		}

		internal void SetClientReadyInternal(NetworkConnection conn)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("SetClientReadyInternal for conn:" + conn.connectionId);
			}
			if (conn.isReady)
			{
				if (LogFilter.logDebug)
				{
					Debug.Log("SetClientReady conn " + conn.connectionId + " already ready");
				}
				return;
			}
			if (conn.playerControllers.Count == 0 && LogFilter.logDebug)
			{
				Debug.LogWarning("Ready with no player object");
			}
			conn.isReady = true;
			ULocalConnectionToClient ulocalConnectionToClient = conn as ULocalConnectionToClient;
			if (ulocalConnectionToClient != null)
			{
				if (LogFilter.logDev)
				{
					Debug.Log("NetworkServer Ready handling ULocalConnectionToClient");
				}
				foreach (NetworkIdentity networkIdentity in NetworkServer.objects.Values)
				{
					if (networkIdentity != null && networkIdentity.gameObject != null)
					{
						bool flag = networkIdentity.OnCheckObserver(conn);
						if (flag)
						{
							networkIdentity.AddObserver(conn);
						}
						if (!networkIdentity.isClient)
						{
							if (LogFilter.logDev)
							{
								Debug.Log("LocalClient.SetSpawnObject calling OnStartClient");
							}
							networkIdentity.OnStartClient();
						}
					}
				}
				return;
			}
			if (LogFilter.logDebug)
			{
				Debug.Log(string.Concat(new object[]
				{
					"Spawning ",
					NetworkServer.objects.Count,
					" objects for conn ",
					conn.connectionId
				}));
			}
			ObjectSpawnFinishedMessage objectSpawnFinishedMessage = new ObjectSpawnFinishedMessage();
			objectSpawnFinishedMessage.state = 0u;
			conn.Send(12, objectSpawnFinishedMessage);
			foreach (NetworkIdentity networkIdentity2 in NetworkServer.objects.Values)
			{
				if (networkIdentity2 == null)
				{
					if (LogFilter.logWarn)
					{
						Debug.LogWarning("Invalid object found in server local object list (null NetworkIdentity).");
					}
				}
				else if (networkIdentity2.gameObject.activeSelf)
				{
					if (LogFilter.logDebug)
					{
						Debug.Log(string.Concat(new object[]
						{
							"Sending spawn message for current server objects name='",
							networkIdentity2.gameObject.name,
							"' netId=",
							networkIdentity2.netId
						}));
					}
					bool flag2 = networkIdentity2.OnCheckObserver(conn);
					if (flag2)
					{
						networkIdentity2.AddObserver(conn);
					}
				}
			}
			objectSpawnFinishedMessage.state = 1u;
			conn.Send(12, objectSpawnFinishedMessage);
		}

		internal static void ShowForConnection(NetworkIdentity uv, NetworkConnection conn)
		{
			if (conn.isReady)
			{
				NetworkServer.instance.SendSpawnMessage(uv, conn);
			}
		}

		internal static void HideForConnection(NetworkIdentity uv, NetworkConnection conn)
		{
			conn.Send(13, new ObjectDestroyMessage
			{
				netId = uv.netId
			});
		}

		public static void SetAllClientsNotReady()
		{
			for (int i = 0; i < NetworkServer.connections.Count; i++)
			{
				NetworkConnection networkConnection = NetworkServer.connections[i];
				if (networkConnection != null)
				{
					NetworkServer.SetClientNotReady(networkConnection);
				}
			}
		}

		public static void SetClientNotReady(NetworkConnection conn)
		{
			NetworkServer.instance.InternalSetClientNotReady(conn);
		}

		internal void InternalSetClientNotReady(NetworkConnection conn)
		{
			if (conn.isReady)
			{
				if (LogFilter.logDebug)
				{
					Debug.Log("PlayerNotReady " + conn);
				}
				conn.isReady = false;
				conn.RemoveObservers();
				NotReadyMessage msg = new NotReadyMessage();
				conn.Send(36, msg);
			}
		}

		private static void OnClientReadyMessage(NetworkMessage netMsg)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("Default handler for ready message from " + netMsg.conn);
			}
			NetworkServer.SetClientReady(netMsg.conn);
		}

		private static void OnRemovePlayerMessage(NetworkMessage netMsg)
		{
			netMsg.ReadMessage<RemovePlayerMessage>(NetworkServer.s_RemovePlayerMessage);
			PlayerController playerController = null;
			netMsg.conn.GetPlayerController(NetworkServer.s_RemovePlayerMessage.playerControllerId, out playerController);
			if (playerController != null)
			{
				netMsg.conn.RemovePlayerController(NetworkServer.s_RemovePlayerMessage.playerControllerId);
				NetworkServer.Destroy(playerController.gameObject);
			}
			else if (LogFilter.logError)
			{
				Debug.LogError("Received remove player message but could not find the player ID: " + NetworkServer.s_RemovePlayerMessage.playerControllerId);
			}
		}

		private static void OnCommandMessage(NetworkMessage netMsg)
		{
			int cmdHash = (int)netMsg.reader.ReadPackedUInt32();
			NetworkInstanceId networkInstanceId = netMsg.reader.ReadNetworkId();
			GameObject gameObject = NetworkServer.FindLocalObject(networkInstanceId);
			if (gameObject == null)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("Instance not found when handling Command message [netId=" + networkInstanceId + "]");
				}
				return;
			}
			NetworkIdentity component = gameObject.GetComponent<NetworkIdentity>();
			if (component == null)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("NetworkIdentity deleted when handling Command message [netId=" + networkInstanceId + "]");
				}
				return;
			}
			bool flag = false;
			foreach (PlayerController playerController in netMsg.conn.playerControllers)
			{
				if (playerController.gameObject != null && playerController.gameObject.GetComponent<NetworkIdentity>().netId == component.netId)
				{
					flag = true;
					break;
				}
			}
			if (!flag && component.clientAuthorityOwner != netMsg.conn)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("Command for object without authority [netId=" + networkInstanceId + "]");
				}
				return;
			}
			if (LogFilter.logDev)
			{
				Debug.Log(string.Concat(new object[]
				{
					"OnCommandMessage for netId=",
					networkInstanceId,
					" conn=",
					netMsg.conn
				}));
			}
			component.HandleCommand(cmdHash, netMsg.reader);
		}

		internal void SpawnObject(GameObject obj)
		{
			if (!NetworkServer.active)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("SpawnObject for " + obj + ", NetworkServer is not active. Cannot spawn objects without an active server.");
				}
				return;
			}
			NetworkIdentity networkIdentity;
			if (!NetworkServer.GetNetworkIdentity(obj, out networkIdentity))
			{
				if (LogFilter.logError)
				{
					Debug.LogError(string.Concat(new object[]
					{
						"SpawnObject ",
						obj,
						" has no NetworkIdentity. Please add a NetworkIdentity to ",
						obj
					}));
				}
				return;
			}
			networkIdentity.OnStartServer(false);
			if (LogFilter.logDebug)
			{
				Debug.Log(string.Concat(new object[]
				{
					"SpawnObject instance ID ",
					networkIdentity.netId,
					" asset ID ",
					networkIdentity.assetId
				}));
			}
			networkIdentity.RebuildObservers(true);
		}

		internal void SendSpawnMessage(NetworkIdentity uv, NetworkConnection conn)
		{
			if (uv.serverOnly)
			{
				return;
			}
			if (uv.sceneId.IsEmpty())
			{
				ObjectSpawnMessage objectSpawnMessage = new ObjectSpawnMessage();
				objectSpawnMessage.netId = uv.netId;
				objectSpawnMessage.assetId = uv.assetId;
				objectSpawnMessage.position = uv.transform.position;
				NetworkWriter networkWriter = new NetworkWriter();
				uv.UNetSerializeAllVars(networkWriter);
				if (networkWriter.Position > 0)
				{
					objectSpawnMessage.payload = networkWriter.ToArray();
				}
				if (conn != null)
				{
					conn.Send(3, objectSpawnMessage);
				}
				else
				{
					NetworkServer.SendToReady(uv.gameObject, 3, objectSpawnMessage);
				}
			}
			else
			{
				ObjectSpawnSceneMessage objectSpawnSceneMessage = new ObjectSpawnSceneMessage();
				objectSpawnSceneMessage.netId = uv.netId;
				objectSpawnSceneMessage.sceneId = uv.sceneId;
				objectSpawnSceneMessage.position = uv.transform.position;
				NetworkWriter networkWriter2 = new NetworkWriter();
				uv.UNetSerializeAllVars(networkWriter2);
				if (networkWriter2.Position > 0)
				{
					objectSpawnSceneMessage.payload = networkWriter2.ToArray();
				}
				if (conn != null)
				{
					conn.Send(10, objectSpawnSceneMessage);
				}
				else
				{
					NetworkServer.SendToReady(uv.gameObject, 3, objectSpawnSceneMessage);
				}
			}
		}

		public static void DestroyPlayersForConnection(NetworkConnection conn)
		{
			if (conn.playerControllers.Count == 0)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("Empty player list given to NetworkServer.Destroy(), nothing to do.");
				}
				return;
			}
			if (conn.clientOwnedObjects != null)
			{
				HashSet<NetworkInstanceId> hashSet = new HashSet<NetworkInstanceId>(conn.clientOwnedObjects);
				foreach (NetworkInstanceId netId in hashSet)
				{
					GameObject gameObject = NetworkServer.FindLocalObject(netId);
					if (gameObject != null)
					{
						NetworkServer.DestroyObject(gameObject);
					}
				}
			}
			foreach (PlayerController playerController in conn.playerControllers)
			{
				if (playerController.IsValid)
				{
					if (!(playerController.unetView == null))
					{
						NetworkServer.DestroyObject(playerController.unetView, true);
					}
					playerController.gameObject = null;
				}
			}
			conn.playerControllers.Clear();
		}

		private static void UnSpawnObject(GameObject obj)
		{
			if (obj == null)
			{
				if (LogFilter.logDev)
				{
					Debug.Log("NetworkServer UnspawnObject is null");
				}
				return;
			}
			NetworkIdentity uv;
			if (!NetworkServer.GetNetworkIdentity(obj, out uv))
			{
				return;
			}
			NetworkServer.UnSpawnObject(uv);
		}

		private static void UnSpawnObject(NetworkIdentity uv)
		{
			NetworkServer.DestroyObject(uv, false);
		}

		private static void DestroyObject(GameObject obj)
		{
			if (obj == null)
			{
				if (LogFilter.logDev)
				{
					Debug.Log("NetworkServer DestroyObject is null");
				}
				return;
			}
			NetworkIdentity uv;
			if (!NetworkServer.GetNetworkIdentity(obj, out uv))
			{
				return;
			}
			NetworkServer.DestroyObject(uv, true);
		}

		private static void DestroyObject(NetworkIdentity uv, bool destroyServerObject)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("DestroyObject instance:" + uv.netId);
			}
			if (NetworkServer.objects.ContainsKey(uv.netId))
			{
				NetworkServer.objects.Remove(uv.netId);
			}
			if (uv.clientAuthorityOwner != null)
			{
				uv.clientAuthorityOwner.RemoveOwnedObject(uv);
			}
			ObjectDestroyMessage objectDestroyMessage = new ObjectDestroyMessage();
			objectDestroyMessage.netId = uv.netId;
			NetworkServer.SendToObservers(uv.gameObject, 1, objectDestroyMessage);
			uv.ClearObservers();
			if (NetworkClient.active && NetworkServer.instance.m_LocalClientActive)
			{
				uv.OnNetworkDestroy();
				ClientScene.SetLocalObject(objectDestroyMessage.netId, null);
			}
			if (destroyServerObject)
			{
				Object.Destroy(uv.gameObject);
			}
			uv.SetNoServer();
		}

		public static void ClearLocalObjects()
		{
			NetworkServer.objects.Clear();
		}

		public static void Spawn(GameObject obj)
		{
			NetworkServer.instance.SpawnObject(obj);
		}

		public static bool SpawnWithClientAuthority(GameObject obj, GameObject player)
		{
			NetworkIdentity component = player.GetComponent<NetworkIdentity>();
			if (component == null)
			{
				Debug.LogError("SpawnWithClientAuthority player object has no NetworkIdentity");
				return false;
			}
			if (component.connectionToClient == null)
			{
				Debug.LogError("SpawnWithClientAuthority player object is not a player.");
				return false;
			}
			return NetworkServer.SpawnWithClientAuthority(obj, component.connectionToClient);
		}

		public static bool SpawnWithClientAuthority(GameObject obj, NetworkConnection conn)
		{
			NetworkServer.Spawn(obj);
			NetworkIdentity component = obj.GetComponent<NetworkIdentity>();
			return !(component == null) && component.isServer && component.AssignClientAuthority(conn);
		}

		public static bool SpawnWithClientAuthority(GameObject obj, NetworkHash128 assetId, NetworkConnection conn)
		{
			NetworkServer.Spawn(obj, assetId);
			NetworkIdentity component = obj.GetComponent<NetworkIdentity>();
			return !(component == null) && component.isServer && component.AssignClientAuthority(conn);
		}

		public static void Spawn(GameObject obj, NetworkHash128 assetId)
		{
			NetworkIdentity networkIdentity;
			if (NetworkServer.GetNetworkIdentity(obj, out networkIdentity))
			{
				networkIdentity.SetDynamicAssetId(assetId);
			}
			NetworkServer.instance.SpawnObject(obj);
		}

		public static void Destroy(GameObject obj)
		{
			NetworkServer.DestroyObject(obj);
		}

		public static void UnSpawn(GameObject obj)
		{
			NetworkServer.UnSpawnObject(obj);
		}

		internal bool InvokeBytes(ULocalConnectionToServer conn, byte[] buffer, int numBytes, int channelId)
		{
			NetworkReader networkReader = new NetworkReader(buffer);
			networkReader.ReadInt16();
			short num = networkReader.ReadInt16();
			if (NetworkServer.handlers.ContainsKey(num) && this.m_LocalConnection != null)
			{
				this.m_LocalConnection.InvokeHandler(num, networkReader, channelId);
				return true;
			}
			return false;
		}

		internal bool InvokeHandlerOnServer(ULocalConnectionToServer conn, short msgType, MessageBase msg, int channelId)
		{
			if (NetworkServer.handlers.ContainsKey(msgType) && this.m_LocalConnection != null)
			{
				NetworkWriter writer = new NetworkWriter();
				msg.Serialize(writer);
				NetworkReader reader = new NetworkReader(writer);
				this.m_LocalConnection.InvokeHandler(msgType, reader, channelId);
				return true;
			}
			if (LogFilter.logError)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Local invoke: Failed to find local connection to invoke handler on [connectionId=",
					conn.connectionId,
					"] for MsgId:",
					msgType
				}));
			}
			return false;
		}

		public static GameObject FindLocalObject(NetworkInstanceId netId)
		{
			return NetworkServer.instance.m_NetworkScene.FindLocalObject(netId);
		}

		public static Dictionary<short, NetworkConnection.PacketStat> GetConnectionStats()
		{
			Dictionary<short, NetworkConnection.PacketStat> dictionary = new Dictionary<short, NetworkConnection.PacketStat>();
			for (int i = 0; i < NetworkServer.connections.Count; i++)
			{
				NetworkConnection networkConnection = NetworkServer.connections[i];
				if (networkConnection != null)
				{
					foreach (short key in networkConnection.packetStats.Keys)
					{
						if (dictionary.ContainsKey(key))
						{
							NetworkConnection.PacketStat packetStat = dictionary[key];
							packetStat.count += networkConnection.packetStats[key].count;
							packetStat.bytes += networkConnection.packetStats[key].bytes;
							dictionary[key] = packetStat;
						}
						else
						{
							dictionary[key] = networkConnection.packetStats[key];
						}
					}
				}
			}
			return dictionary;
		}

		public static void ResetConnectionStats()
		{
			for (int i = 0; i < NetworkServer.connections.Count; i++)
			{
				NetworkConnection networkConnection = NetworkServer.connections[i];
				if (networkConnection != null)
				{
					networkConnection.ResetStats();
				}
			}
		}

		public static bool AddExternalConnection(NetworkConnection conn)
		{
			return NetworkServer.instance.AddExternalConnectionInternal(conn);
		}

		private bool AddExternalConnectionInternal(NetworkConnection conn)
		{
			if (conn.connectionId < 0)
			{
				return false;
			}
			if (conn.connectionId < NetworkServer.connections.Count && NetworkServer.connections[conn.connectionId] != null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("AddExternalConnection failed, already connection for id:" + conn.connectionId);
				}
				return false;
			}
			if (LogFilter.logDebug)
			{
				Debug.Log("AddExternalConnection external connection " + conn.connectionId);
			}
			this.m_SimpleServerSimple.SetConnectionAtIndex(conn);
			this.m_ExternalConnections.Add(conn.connectionId);
			conn.InvokeHandlerNoData(32);
			return true;
		}

		public static void RemoveExternalConnection(int connectionId)
		{
			NetworkServer.instance.RemoveExternalConnectionInternal(connectionId);
		}

		private bool RemoveExternalConnectionInternal(int connectionId)
		{
			if (!this.m_ExternalConnections.Contains(connectionId))
			{
				if (LogFilter.logError)
				{
					Debug.LogError("RemoveExternalConnection failed, no connection for id:" + connectionId);
				}
				return false;
			}
			if (LogFilter.logDebug)
			{
				Debug.Log("RemoveExternalConnection external connection " + connectionId);
			}
			NetworkConnection networkConnection = this.m_SimpleServerSimple.FindConnection(connectionId);
			if (networkConnection != null)
			{
				networkConnection.RemoveObservers();
			}
			this.m_SimpleServerSimple.RemoveConnectionAtIndex(connectionId);
			return true;
		}

		public static bool SpawnObjects()
		{
			if (NetworkServer.active)
			{
				NetworkIdentity[] array = Resources.FindObjectsOfTypeAll<NetworkIdentity>();
				foreach (NetworkIdentity networkIdentity in array)
				{
					if (networkIdentity.gameObject.hideFlags != HideFlags.NotEditable && networkIdentity.gameObject.hideFlags != HideFlags.HideAndDontSave)
					{
						if (!networkIdentity.sceneId.IsEmpty())
						{
							if (LogFilter.logDebug)
							{
								Debug.Log(string.Concat(new object[]
								{
									"SpawnObjects sceneId:",
									networkIdentity.sceneId,
									" name:",
									networkIdentity.gameObject.name
								}));
							}
							networkIdentity.gameObject.SetActive(true);
						}
					}
				}
				foreach (NetworkIdentity networkIdentity2 in array)
				{
					if (networkIdentity2.gameObject.hideFlags != HideFlags.NotEditable && networkIdentity2.gameObject.hideFlags != HideFlags.HideAndDontSave)
					{
						if (!networkIdentity2.sceneId.IsEmpty())
						{
							if (!networkIdentity2.isServer)
							{
								if (!(networkIdentity2.gameObject == null))
								{
									NetworkServer.Spawn(networkIdentity2.gameObject);
									networkIdentity2.ForceAuthority(true);
								}
							}
						}
					}
				}
			}
			return true;
		}

		private static void SendCrc(NetworkConnection targetConnection)
		{
			if (NetworkCRC.singleton == null)
			{
				return;
			}
			if (!NetworkCRC.scriptCRCCheck)
			{
				return;
			}
			CRCMessage crcmessage = new CRCMessage();
			List<CRCMessageEntry> list = new List<CRCMessageEntry>();
			foreach (string text in NetworkCRC.singleton.scripts.Keys)
			{
				list.Add(new CRCMessageEntry
				{
					name = text,
					channel = (byte)NetworkCRC.singleton.scripts[text]
				});
			}
			crcmessage.scripts = list.ToArray();
			targetConnection.Send(14, crcmessage);
		}

		[Obsolete("moved to NetworkMigrationManager")]
		public void SendNetworkInfo(NetworkConnection targetConnection)
		{
		}

		private class ServerSimpleWrapper : NetworkServerSimple
		{
			private NetworkServer m_Server;

			public ServerSimpleWrapper(NetworkServer server)
			{
				this.m_Server = server;
			}

			public override void OnConnectError(int connectionId, byte error)
			{
				this.m_Server.GenerateConnectError((int)error);
			}

			public override void OnDataError(NetworkConnection conn, byte error)
			{
				this.m_Server.GenerateDataError(conn, (int)error);
			}

			public override void OnConnected(NetworkConnection conn)
			{
				this.m_Server.OnConnected(conn);
			}

			public override void OnDisconnected(NetworkConnection conn)
			{
				this.m_Server.OnDisconnected(conn);
			}

			public override void OnData(NetworkConnection conn, int receivedSize, int channelId)
			{
				this.m_Server.OnData(conn, receivedSize, channelId);
			}
		}
	}
}
