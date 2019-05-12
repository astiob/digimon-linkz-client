using System;
using System.Collections.Generic;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking
{
	[AddComponentMenu("Network/NetworkMigrationManager")]
	public class NetworkMigrationManager : MonoBehaviour
	{
		[SerializeField]
		private bool m_HostMigration = true;

		[SerializeField]
		private bool m_ShowGUI = true;

		[SerializeField]
		private int m_OffsetX = 10;

		[SerializeField]
		private int m_OffsetY = 300;

		private NetworkClient m_Client;

		private bool m_WaitingToBecomeNewHost;

		private bool m_WaitingReconnectToNewHost;

		private bool m_DisconnectedFromHost;

		private bool m_HostWasShutdown;

		private MatchInfo m_MatchInfo;

		private int m_OldServerConnectionId = -1;

		private string m_NewHostAddress;

		private PeerInfoMessage m_NewHostInfo = new PeerInfoMessage();

		private PeerListMessage m_PeerListMessage = new PeerListMessage();

		private PeerInfoMessage[] m_Peers;

		private Dictionary<int, NetworkMigrationManager.ConnectionPendingPlayers> m_PendingPlayers = new Dictionary<int, NetworkMigrationManager.ConnectionPendingPlayers>();

		private void AddPendingPlayer(GameObject obj, int connectionId, NetworkInstanceId netId, short playerControllerId)
		{
			if (!this.m_PendingPlayers.ContainsKey(connectionId))
			{
				NetworkMigrationManager.ConnectionPendingPlayers value = default(NetworkMigrationManager.ConnectionPendingPlayers);
				value.players = new List<NetworkMigrationManager.PendingPlayerInfo>();
				this.m_PendingPlayers[connectionId] = value;
			}
			NetworkMigrationManager.PendingPlayerInfo item = default(NetworkMigrationManager.PendingPlayerInfo);
			item.netId = netId;
			item.playerControllerId = playerControllerId;
			item.obj = obj;
			this.m_PendingPlayers[connectionId].players.Add(item);
		}

		private GameObject FindPendingPlayer(int connectionId, NetworkInstanceId netId, short playerControllerId)
		{
			if (this.m_PendingPlayers.ContainsKey(connectionId))
			{
				foreach (NetworkMigrationManager.PendingPlayerInfo pendingPlayerInfo in this.m_PendingPlayers[connectionId].players)
				{
					if (pendingPlayerInfo.netId == netId && pendingPlayerInfo.playerControllerId == playerControllerId)
					{
						return pendingPlayerInfo.obj;
					}
				}
			}
			return null;
		}

		private void RemovePendingPlayer(int connectionId)
		{
			this.m_PendingPlayers.Remove(connectionId);
		}

		public bool hostMigration
		{
			get
			{
				return this.m_HostMigration;
			}
			set
			{
				this.m_HostMigration = value;
			}
		}

		public bool showGUI
		{
			get
			{
				return this.m_ShowGUI;
			}
			set
			{
				this.m_ShowGUI = value;
			}
		}

		public int offsetX
		{
			get
			{
				return this.m_OffsetX;
			}
			set
			{
				this.m_OffsetX = value;
			}
		}

		public int offsetY
		{
			get
			{
				return this.m_OffsetY;
			}
			set
			{
				this.m_OffsetY = value;
			}
		}

		public NetworkClient client
		{
			get
			{
				return this.m_Client;
			}
		}

		public bool waitingToBecomeNewHost
		{
			get
			{
				return this.m_WaitingToBecomeNewHost;
			}
			set
			{
				this.m_WaitingToBecomeNewHost = value;
			}
		}

		public bool waitingReconnectToNewHost
		{
			get
			{
				return this.m_WaitingReconnectToNewHost;
			}
			set
			{
				this.m_WaitingReconnectToNewHost = value;
			}
		}

		public bool disconnectedFromHost
		{
			get
			{
				return this.m_DisconnectedFromHost;
			}
		}

		public bool hostWasShutdown
		{
			get
			{
				return this.m_HostWasShutdown;
			}
		}

		public MatchInfo matchInfo
		{
			get
			{
				return this.m_MatchInfo;
			}
		}

		public int oldServerConnectionId
		{
			get
			{
				return this.m_OldServerConnectionId;
			}
		}

		public string newHostAddress
		{
			get
			{
				return this.m_NewHostAddress;
			}
			set
			{
				this.m_NewHostAddress = value;
			}
		}

		public PeerInfoMessage[] peers
		{
			get
			{
				return this.m_Peers;
			}
		}

		public Dictionary<int, NetworkMigrationManager.ConnectionPendingPlayers> pendingPlayers
		{
			get
			{
				return this.m_PendingPlayers;
			}
		}

		private void Start()
		{
			this.Reset(-1);
		}

		public void Reset(int reconnectId)
		{
			this.m_OldServerConnectionId = -1;
			this.m_WaitingToBecomeNewHost = false;
			this.m_WaitingReconnectToNewHost = false;
			this.m_DisconnectedFromHost = false;
			this.m_HostWasShutdown = false;
			ClientScene.SetReconnectId(reconnectId, this.m_Peers);
			if (NetworkManager.singleton != null)
			{
				NetworkManager.singleton.SetupMigrationManager(this);
			}
		}

		internal void AssignAuthorityCallback(NetworkConnection conn, NetworkIdentity uv, bool authorityState)
		{
			PeerAuthorityMessage peerAuthorityMessage = new PeerAuthorityMessage();
			peerAuthorityMessage.connectionId = conn.connectionId;
			peerAuthorityMessage.netId = uv.netId;
			peerAuthorityMessage.authorityState = authorityState;
			if (LogFilter.logDebug)
			{
				Debug.Log("AssignAuthorityCallback send for netId" + uv.netId);
			}
			for (int i = 0; i < NetworkServer.connections.Count; i++)
			{
				NetworkConnection networkConnection = NetworkServer.connections[i];
				if (networkConnection != null)
				{
					networkConnection.Send(17, peerAuthorityMessage);
				}
			}
		}

		public void Initialize(NetworkClient newClient, MatchInfo newMatchInfo)
		{
			if (LogFilter.logDev)
			{
				Debug.Log("NetworkMigrationManager initialize");
			}
			this.m_Client = newClient;
			this.m_MatchInfo = newMatchInfo;
			newClient.RegisterHandlerSafe(11, new NetworkMessageDelegate(this.OnPeerInfo));
			newClient.RegisterHandlerSafe(17, new NetworkMessageDelegate(this.OnPeerClientAuthority));
			NetworkIdentity.clientAuthorityCallback = new NetworkIdentity.ClientAuthorityCallback(this.AssignAuthorityCallback);
		}

		public void DisablePlayerObjects()
		{
			if (LogFilter.logDev)
			{
				Debug.Log("NetworkMigrationManager DisablePlayerObjects");
			}
			if (this.m_Peers == null)
			{
				return;
			}
			foreach (PeerInfoMessage peerInfoMessage in this.m_Peers)
			{
				if (peerInfoMessage.playerIds != null)
				{
					foreach (PeerInfoPlayer peerInfoPlayer in peerInfoMessage.playerIds)
					{
						if (LogFilter.logDev)
						{
							Debug.Log(string.Concat(new object[]
							{
								"DisablePlayerObjects disable player for ",
								peerInfoMessage.address,
								" netId:",
								peerInfoPlayer.netId,
								" control:",
								peerInfoPlayer.playerControllerId
							}));
						}
						GameObject gameObject = ClientScene.FindLocalObject(peerInfoPlayer.netId);
						if (gameObject != null)
						{
							gameObject.SetActive(false);
							this.AddPendingPlayer(gameObject, peerInfoMessage.connectionId, peerInfoPlayer.netId, peerInfoPlayer.playerControllerId);
						}
						else if (LogFilter.logWarn)
						{
							Debug.LogWarning(string.Concat(new object[]
							{
								"DisablePlayerObjects didnt find player Conn:",
								peerInfoMessage.connectionId,
								" NetId:",
								peerInfoPlayer.netId
							}));
						}
					}
				}
			}
		}

		public void SendPeerInfo()
		{
			if (!this.m_HostMigration)
			{
				return;
			}
			PeerListMessage peerListMessage = new PeerListMessage();
			List<PeerInfoMessage> list = new List<PeerInfoMessage>();
			for (int i = 0; i < NetworkServer.connections.Count; i++)
			{
				NetworkConnection networkConnection = NetworkServer.connections[i];
				if (networkConnection != null)
				{
					PeerInfoMessage peerInfoMessage = new PeerInfoMessage();
					string address;
					int port;
					NetworkID networkID;
					NodeID nodeID;
					byte b;
					NetworkTransport.GetConnectionInfo(NetworkServer.serverHostId, networkConnection.connectionId, out address, out port, out networkID, out nodeID, out b);
					peerInfoMessage.connectionId = networkConnection.connectionId;
					peerInfoMessage.port = port;
					if (i == 0)
					{
						peerInfoMessage.port = NetworkServer.listenPort;
						peerInfoMessage.isHost = true;
						peerInfoMessage.address = "<host>";
					}
					else
					{
						peerInfoMessage.address = address;
						peerInfoMessage.isHost = false;
					}
					List<PeerInfoPlayer> list2 = new List<PeerInfoPlayer>();
					foreach (PlayerController playerController in networkConnection.playerControllers)
					{
						if (playerController != null && playerController.unetView != null)
						{
							PeerInfoPlayer item;
							item.netId = playerController.unetView.netId;
							item.playerControllerId = playerController.unetView.playerControllerId;
							list2.Add(item);
						}
					}
					if (networkConnection.clientOwnedObjects != null)
					{
						foreach (NetworkInstanceId netId in networkConnection.clientOwnedObjects)
						{
							GameObject gameObject = NetworkServer.FindLocalObject(netId);
							if (!(gameObject == null))
							{
								NetworkIdentity component = gameObject.GetComponent<NetworkIdentity>();
								if (component.playerControllerId == -1)
								{
									PeerInfoPlayer item2;
									item2.netId = netId;
									item2.playerControllerId = -1;
									list2.Add(item2);
								}
							}
						}
					}
					if (list2.Count > 0)
					{
						peerInfoMessage.playerIds = list2.ToArray();
					}
					list.Add(peerInfoMessage);
				}
			}
			peerListMessage.peers = list.ToArray();
			for (int j = 0; j < NetworkServer.connections.Count; j++)
			{
				NetworkConnection networkConnection2 = NetworkServer.connections[j];
				if (networkConnection2 != null)
				{
					peerListMessage.oldServerConnectionId = networkConnection2.connectionId;
					networkConnection2.Send(11, peerListMessage);
				}
			}
		}

		private void OnPeerClientAuthority(NetworkMessage netMsg)
		{
			PeerAuthorityMessage peerAuthorityMessage = netMsg.ReadMessage<PeerAuthorityMessage>();
			if (LogFilter.logDebug)
			{
				Debug.Log("OnPeerClientAuthority for netId:" + peerAuthorityMessage.netId);
			}
			if (this.m_Peers == null)
			{
				return;
			}
			foreach (PeerInfoMessage peerInfoMessage in this.m_Peers)
			{
				if (peerInfoMessage.connectionId == peerAuthorityMessage.connectionId)
				{
					if (peerInfoMessage.playerIds == null)
					{
						peerInfoMessage.playerIds = new PeerInfoPlayer[0];
					}
					if (peerAuthorityMessage.authorityState)
					{
						foreach (PeerInfoPlayer peerInfoPlayer in peerInfoMessage.playerIds)
						{
							if (peerInfoPlayer.netId == peerAuthorityMessage.netId)
							{
								return;
							}
						}
						PeerInfoPlayer item = default(PeerInfoPlayer);
						item.netId = peerAuthorityMessage.netId;
						item.playerControllerId = -1;
						peerInfoMessage.playerIds = new List<PeerInfoPlayer>(peerInfoMessage.playerIds)
						{
							item
						}.ToArray();
					}
					else
					{
						for (int k = 0; k < peerInfoMessage.playerIds.Length; k++)
						{
							if (peerInfoMessage.playerIds[k].netId == peerAuthorityMessage.netId)
							{
								List<PeerInfoPlayer> list = new List<PeerInfoPlayer>(peerInfoMessage.playerIds);
								list.RemoveAt(k);
								peerInfoMessage.playerIds = list.ToArray();
								break;
							}
						}
					}
				}
			}
			GameObject go = ClientScene.FindLocalObject(peerAuthorityMessage.netId);
			this.OnAuthorityUpdated(go, peerAuthorityMessage.connectionId, peerAuthorityMessage.authorityState);
		}

		private void OnPeerInfo(NetworkMessage netMsg)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("OnPeerInfo");
			}
			netMsg.ReadMessage<PeerListMessage>(this.m_PeerListMessage);
			this.m_Peers = this.m_PeerListMessage.peers;
			this.m_OldServerConnectionId = this.m_PeerListMessage.oldServerConnectionId;
			for (int i = 0; i < this.m_Peers.Length; i++)
			{
				if (LogFilter.logDebug)
				{
					Debug.Log(string.Concat(new object[]
					{
						"peer conn ",
						this.m_Peers[i].connectionId,
						" your conn ",
						this.m_PeerListMessage.oldServerConnectionId
					}));
				}
				if (this.m_Peers[i].connectionId == this.m_PeerListMessage.oldServerConnectionId)
				{
					this.m_Peers[i].isYou = true;
					break;
				}
			}
			this.OnPeersUpdated(this.m_PeerListMessage);
		}

		private void OnServerReconnectPlayerMessage(NetworkMessage netMsg)
		{
			ReconnectMessage reconnectMessage = netMsg.ReadMessage<ReconnectMessage>();
			if (LogFilter.logDev)
			{
				Debug.Log(string.Concat(new object[]
				{
					"OnReconnectMessage: connId=",
					reconnectMessage.oldConnectionId,
					" playerControllerId:",
					reconnectMessage.playerControllerId,
					" netId:",
					reconnectMessage.netId
				}));
			}
			GameObject gameObject = this.FindPendingPlayer(reconnectMessage.oldConnectionId, reconnectMessage.netId, reconnectMessage.playerControllerId);
			if (gameObject == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError(string.Concat(new object[]
					{
						"OnReconnectMessage connId=",
						reconnectMessage.oldConnectionId,
						" player null for netId:",
						reconnectMessage.netId,
						" msg.playerControllerId:",
						reconnectMessage.playerControllerId
					}));
				}
				return;
			}
			if (gameObject.activeSelf)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("OnReconnectMessage connId=" + reconnectMessage.oldConnectionId + " player already active?");
				}
				return;
			}
			if (LogFilter.logDebug)
			{
				Debug.Log("OnReconnectMessage: player=" + gameObject);
			}
			NetworkReader networkReader = null;
			if (reconnectMessage.msgSize != 0)
			{
				networkReader = new NetworkReader(reconnectMessage.msgData);
			}
			if (reconnectMessage.playerControllerId != -1)
			{
				if (networkReader == null)
				{
					this.OnServerReconnectPlayer(netMsg.conn, gameObject, reconnectMessage.oldConnectionId, reconnectMessage.playerControllerId);
				}
				else
				{
					this.OnServerReconnectPlayer(netMsg.conn, gameObject, reconnectMessage.oldConnectionId, reconnectMessage.playerControllerId, networkReader);
				}
			}
			else
			{
				this.OnServerReconnectObject(netMsg.conn, gameObject, reconnectMessage.oldConnectionId);
			}
		}

		public bool ReconnectObjectForConnection(NetworkConnection newConnection, GameObject oldObject, int oldConnectionId)
		{
			if (!NetworkServer.active)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("ReconnectObjectForConnection must have active server");
				}
				return false;
			}
			if (LogFilter.logDebug)
			{
				Debug.Log(string.Concat(new object[]
				{
					"ReconnectObjectForConnection: oldConnId=",
					oldConnectionId,
					" obj=",
					oldObject,
					" conn:",
					newConnection
				}));
			}
			if (!this.m_PendingPlayers.ContainsKey(oldConnectionId))
			{
				if (LogFilter.logError)
				{
					Debug.LogError("ReconnectObjectForConnection oldConnId=" + oldConnectionId + " not found.");
				}
				return false;
			}
			oldObject.SetActive(true);
			oldObject.GetComponent<NetworkIdentity>().SetNetworkInstanceId(new NetworkInstanceId(0u));
			if (!NetworkServer.SpawnWithClientAuthority(oldObject, newConnection))
			{
				if (LogFilter.logError)
				{
					Debug.LogError("ReconnectObjectForConnection oldConnId=" + oldConnectionId + " SpawnWithClientAuthority failed.");
				}
				return false;
			}
			return true;
		}

		public bool ReconnectPlayerForConnection(NetworkConnection newConnection, GameObject oldPlayer, int oldConnectionId, short playerControllerId)
		{
			if (!NetworkServer.active)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("ReconnectPlayerForConnection must have active server");
				}
				return false;
			}
			if (LogFilter.logDebug)
			{
				Debug.Log(string.Concat(new object[]
				{
					"ReconnectPlayerForConnection: oldConnId=",
					oldConnectionId,
					" player=",
					oldPlayer,
					" conn:",
					newConnection
				}));
			}
			if (!this.m_PendingPlayers.ContainsKey(oldConnectionId))
			{
				if (LogFilter.logError)
				{
					Debug.LogError("ReconnectPlayerForConnection oldConnId=" + oldConnectionId + " not found.");
				}
				return false;
			}
			oldPlayer.SetActive(true);
			NetworkServer.Spawn(oldPlayer);
			if (!NetworkServer.AddPlayerForConnection(newConnection, oldPlayer, playerControllerId))
			{
				if (LogFilter.logError)
				{
					Debug.LogError("ReconnectPlayerForConnection oldConnId=" + oldConnectionId + " AddPlayerForConnection failed.");
				}
				return false;
			}
			if (NetworkServer.localClientActive)
			{
				this.SendPeerInfo();
			}
			return true;
		}

		public bool LostHostOnClient(NetworkConnection conn)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkMigrationManager client OnDisconnectedFromHost");
			}
			if (Application.platform == RuntimePlatform.WebGLPlayer)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("LostHostOnClient: Host migration not supported on WebGL");
				}
				return false;
			}
			if (this.m_Client == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkMigrationManager LostHostOnHost client was never initialized.");
				}
				return false;
			}
			if (!this.m_HostMigration)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkMigrationManager LostHostOnHost migration not enabled.");
				}
				return false;
			}
			this.m_DisconnectedFromHost = true;
			this.DisablePlayerObjects();
			byte b;
			NetworkTransport.Disconnect(this.m_Client.hostId, this.m_Client.connection.connectionId, out b);
			if (this.m_OldServerConnectionId != -1)
			{
				NetworkMigrationManager.SceneChangeOption sceneChangeOption;
				this.OnClientDisconnectedFromHost(conn, out sceneChangeOption);
				return sceneChangeOption == NetworkMigrationManager.SceneChangeOption.StayInOnlineScene;
			}
			return false;
		}

		public void LostHostOnHost()
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkMigrationManager LostHostOnHost");
			}
			if (Application.platform == RuntimePlatform.WebGLPlayer)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("LostHostOnHost: Host migration not supported on WebGL");
				}
				return;
			}
			this.OnServerHostShutdown();
			if (this.m_Peers == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkMigrationManager LostHostOnHost no peers");
				}
				return;
			}
			if (this.m_Peers.Length != 1)
			{
				this.m_HostWasShutdown = true;
			}
		}

		public bool BecomeNewHost(int port)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkMigrationManager BecomeNewHost " + this.m_MatchInfo);
			}
			NetworkServer.RegisterHandler(47, new NetworkMessageDelegate(this.OnServerReconnectPlayerMessage));
			NetworkClient networkClient = NetworkServer.BecomeHost(this.m_Client, port, this.m_MatchInfo, this.oldServerConnectionId, this.peers);
			if (networkClient != null)
			{
				if (NetworkManager.singleton != null)
				{
					NetworkManager.singleton.RegisterServerMessages();
					NetworkManager.singleton.UseExternalClient(networkClient);
				}
				else
				{
					Debug.LogWarning("MigrationManager BecomeNewHost - No NetworkManager.");
				}
				networkClient.RegisterHandlerSafe(11, new NetworkMessageDelegate(this.OnPeerInfo));
				this.RemovePendingPlayer(this.m_OldServerConnectionId);
				this.Reset(-1);
				this.SendPeerInfo();
				return true;
			}
			if (LogFilter.logError)
			{
				Debug.LogError("NetworkServer.BecomeHost failed");
			}
			return false;
		}

		protected virtual void OnClientDisconnectedFromHost(NetworkConnection conn, out NetworkMigrationManager.SceneChangeOption sceneChange)
		{
			sceneChange = NetworkMigrationManager.SceneChangeOption.StayInOnlineScene;
		}

		protected virtual void OnServerHostShutdown()
		{
		}

		protected virtual void OnServerReconnectPlayer(NetworkConnection newConnection, GameObject oldPlayer, int oldConnectionId, short playerControllerId)
		{
			this.ReconnectPlayerForConnection(newConnection, oldPlayer, oldConnectionId, playerControllerId);
		}

		protected virtual void OnServerReconnectPlayer(NetworkConnection newConnection, GameObject oldPlayer, int oldConnectionId, short playerControllerId, NetworkReader extraMessageReader)
		{
			this.ReconnectPlayerForConnection(newConnection, oldPlayer, oldConnectionId, playerControllerId);
		}

		protected virtual void OnServerReconnectObject(NetworkConnection newConnection, GameObject oldObject, int oldConnectionId)
		{
			this.ReconnectObjectForConnection(newConnection, oldObject, oldConnectionId);
		}

		protected virtual void OnPeersUpdated(PeerListMessage peers)
		{
			if (LogFilter.logDev)
			{
				Debug.Log("NetworkMigrationManager NumPeers " + peers.peers.Length);
			}
		}

		protected virtual void OnAuthorityUpdated(GameObject go, int connectionId, bool authorityState)
		{
			if (LogFilter.logDev)
			{
				Debug.Log(string.Concat(new object[]
				{
					"NetworkMigrationManager OnAuthorityUpdated for ",
					go,
					" conn:",
					connectionId,
					" state:",
					authorityState
				}));
			}
		}

		public virtual bool FindNewHost(out PeerInfoMessage newHostInfo, out bool youAreNewHost)
		{
			if (this.m_Peers == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkMigrationManager FindLowestHost no peers");
				}
				newHostInfo = null;
				youAreNewHost = false;
				return false;
			}
			if (LogFilter.logDev)
			{
				Debug.Log("NetworkMigrationManager FindLowestHost");
			}
			newHostInfo = new PeerInfoMessage();
			newHostInfo.connectionId = 50000;
			newHostInfo.address = string.Empty;
			newHostInfo.port = 0;
			int num = -1;
			youAreNewHost = false;
			if (this.m_Peers == null)
			{
				return false;
			}
			foreach (PeerInfoMessage peerInfoMessage in this.m_Peers)
			{
				if (peerInfoMessage.connectionId != 0)
				{
					if (!peerInfoMessage.isHost)
					{
						if (peerInfoMessage.isYou)
						{
							num = peerInfoMessage.connectionId;
						}
						if (peerInfoMessage.connectionId < newHostInfo.connectionId)
						{
							newHostInfo = peerInfoMessage;
						}
					}
				}
			}
			if (newHostInfo.connectionId == 50000)
			{
				return false;
			}
			if (newHostInfo.connectionId == num)
			{
				youAreNewHost = true;
			}
			if (LogFilter.logDev)
			{
				Debug.Log("FindNewHost new host is " + newHostInfo.address);
			}
			return true;
		}

		private void OnGUIHost()
		{
			int num = this.m_OffsetY;
			GUI.Label(new Rect((float)this.m_OffsetX, (float)num, 200f, 40f), "Host Was Shutdown ID(" + this.m_OldServerConnectionId + ")");
			num += 25;
			if (Application.platform == RuntimePlatform.WebGLPlayer)
			{
				GUI.Label(new Rect((float)this.m_OffsetX, (float)num, 200f, 40f), "Host Migration not supported for WebGL");
				return;
			}
			if (this.m_WaitingReconnectToNewHost)
			{
				if (GUI.Button(new Rect((float)this.m_OffsetX, (float)num, 200f, 20f), "Reconnect as Client"))
				{
					this.Reset(0);
					if (NetworkManager.singleton != null)
					{
						NetworkManager.singleton.networkAddress = GUI.TextField(new Rect((float)(this.m_OffsetX + 100), (float)num, 95f, 20f), NetworkManager.singleton.networkAddress);
						NetworkManager.singleton.StartClient();
					}
					else
					{
						Debug.LogWarning("MigrationManager Old Host Reconnect - No NetworkManager.");
					}
				}
				num += 25;
			}
			else
			{
				bool flag;
				if (GUI.Button(new Rect((float)this.m_OffsetX, (float)num, 200f, 20f), "Pick New Host") && this.FindNewHost(out this.m_NewHostInfo, out flag))
				{
					this.m_NewHostAddress = this.m_NewHostInfo.address;
					if (flag)
					{
						Debug.LogWarning("MigrationManager FindNewHost - new host is self?");
					}
					else
					{
						this.m_WaitingReconnectToNewHost = true;
					}
				}
				num += 25;
			}
			if (GUI.Button(new Rect((float)this.m_OffsetX, (float)num, 200f, 20f), "Leave Game"))
			{
				if (NetworkManager.singleton != null)
				{
					NetworkManager.singleton.SetupMigrationManager(null);
					NetworkManager.singleton.StopHost();
				}
				else
				{
					Debug.LogWarning("MigrationManager Old Host LeaveGame - No NetworkManager.");
				}
				this.Reset(-1);
			}
			num += 25;
		}

		private void OnGUIClient()
		{
			int num = this.m_OffsetY;
			GUI.Label(new Rect((float)this.m_OffsetX, (float)num, 200f, 40f), "Lost Connection To Host ID(" + this.m_OldServerConnectionId + ")");
			num += 25;
			if (Application.platform == RuntimePlatform.WebGLPlayer)
			{
				GUI.Label(new Rect((float)this.m_OffsetX, (float)num, 200f, 40f), "Host Migration not supported for WebGL");
				return;
			}
			if (this.m_WaitingToBecomeNewHost)
			{
				GUI.Label(new Rect((float)this.m_OffsetX, (float)num, 200f, 40f), "You are the new host");
				num += 25;
				if (GUI.Button(new Rect((float)this.m_OffsetX, (float)num, 200f, 20f), "Start As Host"))
				{
					if (NetworkManager.singleton != null)
					{
						this.BecomeNewHost(NetworkManager.singleton.networkPort);
					}
					else
					{
						Debug.LogWarning("MigrationManager Client BecomeNewHost - No NetworkManager.");
					}
				}
				num += 25;
			}
			else if (this.m_WaitingReconnectToNewHost)
			{
				GUI.Label(new Rect((float)this.m_OffsetX, (float)num, 200f, 40f), "New host is " + this.m_NewHostAddress);
				num += 25;
				if (GUI.Button(new Rect((float)this.m_OffsetX, (float)num, 200f, 20f), "Reconnect To New Host"))
				{
					this.Reset(this.m_OldServerConnectionId);
					if (NetworkManager.singleton != null)
					{
						NetworkManager.singleton.networkAddress = this.m_NewHostAddress;
						NetworkManager.singleton.client.ReconnectToNewHost(this.m_NewHostAddress, NetworkManager.singleton.networkPort);
					}
					else
					{
						Debug.LogWarning("MigrationManager Client reconnect - No NetworkManager.");
					}
				}
				num += 25;
			}
			else
			{
				bool flag;
				if (GUI.Button(new Rect((float)this.m_OffsetX, (float)num, 200f, 20f), "Pick New Host") && this.FindNewHost(out this.m_NewHostInfo, out flag))
				{
					this.m_NewHostAddress = this.m_NewHostInfo.address;
					if (flag)
					{
						this.m_WaitingToBecomeNewHost = true;
					}
					else
					{
						this.m_WaitingReconnectToNewHost = true;
					}
				}
				num += 25;
			}
			if (GUI.Button(new Rect((float)this.m_OffsetX, (float)num, 200f, 20f), "Leave Game"))
			{
				if (NetworkManager.singleton != null)
				{
					NetworkManager.singleton.SetupMigrationManager(null);
					NetworkManager.singleton.StopHost();
				}
				else
				{
					Debug.LogWarning("MigrationManager Client LeaveGame - No NetworkManager.");
				}
				this.Reset(-1);
			}
			num += 25;
		}

		private void OnGUI()
		{
			if (!this.m_ShowGUI)
			{
				return;
			}
			if (this.m_HostWasShutdown)
			{
				this.OnGUIHost();
				return;
			}
			if (!this.m_DisconnectedFromHost)
			{
				return;
			}
			this.OnGUIClient();
		}

		public enum SceneChangeOption
		{
			StayInOnlineScene,
			SwitchToOfflineScene
		}

		public struct PendingPlayerInfo
		{
			public NetworkInstanceId netId;

			public short playerControllerId;

			public GameObject obj;
		}

		public struct ConnectionPendingPlayers
		{
			public List<NetworkMigrationManager.PendingPlayerInfo> players;
		}
	}
}
