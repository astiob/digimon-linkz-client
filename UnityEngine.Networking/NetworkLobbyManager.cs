using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;

namespace UnityEngine.Networking
{
	[AddComponentMenu("Network/NetworkLobbyManager")]
	public class NetworkLobbyManager : NetworkManager
	{
		[SerializeField]
		private bool m_ShowLobbyGUI = true;

		[SerializeField]
		private int m_MaxPlayers = 4;

		[SerializeField]
		private int m_MaxPlayersPerConnection = 1;

		[SerializeField]
		private int m_MinPlayers;

		[SerializeField]
		private NetworkLobbyPlayer m_LobbyPlayerPrefab;

		[SerializeField]
		private GameObject m_GamePlayerPrefab;

		[SerializeField]
		private string m_LobbyScene = "";

		[SerializeField]
		private string m_PlayScene = "";

		private List<NetworkLobbyManager.PendingPlayer> m_PendingPlayers = new List<NetworkLobbyManager.PendingPlayer>();

		public NetworkLobbyPlayer[] lobbySlots;

		private static LobbyReadyToBeginMessage s_ReadyToBeginMessage = new LobbyReadyToBeginMessage();

		private static IntegerMessage s_SceneLoadedMessage = new IntegerMessage();

		private static LobbyReadyToBeginMessage s_LobbyReadyToBeginMessage = new LobbyReadyToBeginMessage();

		public bool showLobbyGUI
		{
			get
			{
				return this.m_ShowLobbyGUI;
			}
			set
			{
				this.m_ShowLobbyGUI = value;
			}
		}

		public int maxPlayers
		{
			get
			{
				return this.m_MaxPlayers;
			}
			set
			{
				this.m_MaxPlayers = value;
			}
		}

		public int maxPlayersPerConnection
		{
			get
			{
				return this.m_MaxPlayersPerConnection;
			}
			set
			{
				this.m_MaxPlayersPerConnection = value;
			}
		}

		public int minPlayers
		{
			get
			{
				return this.m_MinPlayers;
			}
			set
			{
				this.m_MinPlayers = value;
			}
		}

		public NetworkLobbyPlayer lobbyPlayerPrefab
		{
			get
			{
				return this.m_LobbyPlayerPrefab;
			}
			set
			{
				this.m_LobbyPlayerPrefab = value;
			}
		}

		public GameObject gamePlayerPrefab
		{
			get
			{
				return this.m_GamePlayerPrefab;
			}
			set
			{
				this.m_GamePlayerPrefab = value;
			}
		}

		public string lobbyScene
		{
			get
			{
				return this.m_LobbyScene;
			}
			set
			{
				this.m_LobbyScene = value;
				base.offlineScene = value;
			}
		}

		public string playScene
		{
			get
			{
				return this.m_PlayScene;
			}
			set
			{
				this.m_PlayScene = value;
			}
		}

		private void OnValidate()
		{
			if (this.m_MaxPlayers <= 0)
			{
				this.m_MaxPlayers = 1;
			}
			if (this.m_MaxPlayersPerConnection <= 0)
			{
				this.m_MaxPlayersPerConnection = 1;
			}
			if (this.m_MaxPlayersPerConnection > this.maxPlayers)
			{
				this.m_MaxPlayersPerConnection = this.maxPlayers;
			}
			if (this.m_MinPlayers < 0)
			{
				this.m_MinPlayers = 0;
			}
			if (this.m_MinPlayers > this.m_MaxPlayers)
			{
				this.m_MinPlayers = this.m_MaxPlayers;
			}
			if (this.m_LobbyPlayerPrefab != null)
			{
				NetworkIdentity component = this.m_LobbyPlayerPrefab.GetComponent<NetworkIdentity>();
				if (component == null)
				{
					this.m_LobbyPlayerPrefab = null;
					Debug.LogWarning("LobbyPlayer prefab must have a NetworkIdentity component.");
				}
			}
			if (this.m_GamePlayerPrefab != null)
			{
				NetworkIdentity component2 = this.m_GamePlayerPrefab.GetComponent<NetworkIdentity>();
				if (component2 == null)
				{
					this.m_GamePlayerPrefab = null;
					Debug.LogWarning("GamePlayer prefab must have a NetworkIdentity component.");
				}
			}
		}

		private byte FindSlot()
		{
			byte b = 0;
			while ((int)b < this.maxPlayers)
			{
				if (this.lobbySlots[(int)b] == null)
				{
					return b;
				}
				b += 1;
			}
			return byte.MaxValue;
		}

		private void SceneLoadedForPlayer(NetworkConnection conn, GameObject lobbyPlayerGameObject)
		{
			NetworkLobbyPlayer component = lobbyPlayerGameObject.GetComponent<NetworkLobbyPlayer>();
			if (!(component == null))
			{
				string name = SceneManager.GetSceneAt(0).name;
				if (LogFilter.logDebug)
				{
					Debug.Log(string.Concat(new object[]
					{
						"NetworkLobby SceneLoadedForPlayer scene:",
						name,
						" ",
						conn
					}));
				}
				if (name == this.m_LobbyScene)
				{
					NetworkLobbyManager.PendingPlayer item;
					item.conn = conn;
					item.lobbyPlayer = lobbyPlayerGameObject;
					this.m_PendingPlayers.Add(item);
				}
				else
				{
					short playerControllerId = lobbyPlayerGameObject.GetComponent<NetworkIdentity>().playerControllerId;
					GameObject gameObject = this.OnLobbyServerCreateGamePlayer(conn, playerControllerId);
					if (gameObject == null)
					{
						Transform startPosition = base.GetStartPosition();
						if (startPosition != null)
						{
							gameObject = Object.Instantiate<GameObject>(this.gamePlayerPrefab, startPosition.position, startPosition.rotation);
						}
						else
						{
							gameObject = Object.Instantiate<GameObject>(this.gamePlayerPrefab, Vector3.zero, Quaternion.identity);
						}
					}
					if (this.OnLobbyServerSceneLoadedForPlayer(lobbyPlayerGameObject, gameObject))
					{
						NetworkServer.ReplacePlayerForConnection(conn, gameObject, playerControllerId);
					}
				}
			}
		}

		private static int CheckConnectionIsReadyToBegin(NetworkConnection conn)
		{
			int num = 0;
			for (int i = 0; i < conn.playerControllers.Count; i++)
			{
				PlayerController playerController = conn.playerControllers[i];
				if (playerController.IsValid)
				{
					NetworkLobbyPlayer component = playerController.gameObject.GetComponent<NetworkLobbyPlayer>();
					if (component.readyToBegin)
					{
						num++;
					}
				}
			}
			return num;
		}

		public void CheckReadyToBegin()
		{
			string name = SceneManager.GetSceneAt(0).name;
			if (!(name != this.m_LobbyScene))
			{
				int num = 0;
				int num2 = 0;
				for (int i = 0; i < NetworkServer.connections.Count; i++)
				{
					NetworkConnection networkConnection = NetworkServer.connections[i];
					if (networkConnection != null)
					{
						num2++;
						num += NetworkLobbyManager.CheckConnectionIsReadyToBegin(networkConnection);
					}
				}
				if (this.m_MinPlayers <= 0 || num >= this.m_MinPlayers)
				{
					if (num >= num2)
					{
						this.m_PendingPlayers.Clear();
						this.OnLobbyServerPlayersReady();
					}
				}
			}
		}

		public void ServerReturnToLobby()
		{
			if (!NetworkServer.active)
			{
				Debug.Log("ServerReturnToLobby called on client");
			}
			else
			{
				this.ServerChangeScene(this.m_LobbyScene);
			}
		}

		private void CallOnClientEnterLobby()
		{
			this.OnLobbyClientEnter();
			for (int i = 0; i < this.lobbySlots.Length; i++)
			{
				NetworkLobbyPlayer networkLobbyPlayer = this.lobbySlots[i];
				if (!(networkLobbyPlayer == null))
				{
					networkLobbyPlayer.readyToBegin = false;
					networkLobbyPlayer.OnClientEnterLobby();
				}
			}
		}

		private void CallOnClientExitLobby()
		{
			this.OnLobbyClientExit();
			for (int i = 0; i < this.lobbySlots.Length; i++)
			{
				NetworkLobbyPlayer networkLobbyPlayer = this.lobbySlots[i];
				if (!(networkLobbyPlayer == null))
				{
					networkLobbyPlayer.OnClientExitLobby();
				}
			}
		}

		public bool SendReturnToLobby()
		{
			bool result;
			if (this.client == null || !this.client.isConnected)
			{
				result = false;
			}
			else
			{
				EmptyMessage msg = new EmptyMessage();
				this.client.Send(46, msg);
				result = true;
			}
			return result;
		}

		public override void OnServerConnect(NetworkConnection conn)
		{
			if (base.numPlayers > this.maxPlayers)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("NetworkLobbyManager can't accept new connection [" + conn + "], too many players connected.");
				}
				conn.Disconnect();
			}
			else
			{
				string name = SceneManager.GetSceneAt(0).name;
				if (name != this.m_LobbyScene)
				{
					if (LogFilter.logWarn)
					{
						Debug.LogWarning("NetworkLobbyManager can't accept new connection [" + conn + "], not in lobby and game already in progress.");
					}
					conn.Disconnect();
				}
				else
				{
					base.OnServerConnect(conn);
					for (int i = 0; i < this.lobbySlots.Length; i++)
					{
						if (this.lobbySlots[i])
						{
							this.lobbySlots[i].SetDirtyBit(1u);
						}
					}
					this.OnLobbyServerConnect(conn);
				}
			}
		}

		public override void OnServerDisconnect(NetworkConnection conn)
		{
			base.OnServerDisconnect(conn);
			for (int i = 0; i < this.lobbySlots.Length; i++)
			{
				NetworkLobbyPlayer networkLobbyPlayer = this.lobbySlots[i];
				if (!(networkLobbyPlayer == null))
				{
					if (networkLobbyPlayer.connectionToClient == conn)
					{
						this.lobbySlots[i] = null;
						NetworkServer.Destroy(networkLobbyPlayer.gameObject);
					}
				}
			}
			this.OnLobbyServerDisconnect(conn);
		}

		public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
		{
			string name = SceneManager.GetSceneAt(0).name;
			if (!(name != this.m_LobbyScene))
			{
				int num = 0;
				for (int i = 0; i < conn.playerControllers.Count; i++)
				{
					if (conn.playerControllers[i].IsValid)
					{
						num++;
					}
				}
				if (num >= this.maxPlayersPerConnection)
				{
					if (LogFilter.logWarn)
					{
						Debug.LogWarning("NetworkLobbyManager no more players for this connection.");
					}
					EmptyMessage msg = new EmptyMessage();
					conn.Send(45, msg);
				}
				else
				{
					byte b = this.FindSlot();
					if (b == 255)
					{
						if (LogFilter.logWarn)
						{
							Debug.LogWarning("NetworkLobbyManager no space for more players");
						}
						EmptyMessage msg2 = new EmptyMessage();
						conn.Send(45, msg2);
					}
					else
					{
						GameObject gameObject = this.OnLobbyServerCreateLobbyPlayer(conn, playerControllerId);
						if (gameObject == null)
						{
							gameObject = Object.Instantiate<GameObject>(this.lobbyPlayerPrefab.gameObject, Vector3.zero, Quaternion.identity);
						}
						NetworkLobbyPlayer component = gameObject.GetComponent<NetworkLobbyPlayer>();
						component.slot = b;
						this.lobbySlots[(int)b] = component;
						NetworkServer.AddPlayerForConnection(conn, gameObject, playerControllerId);
					}
				}
			}
		}

		public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
		{
			short playerControllerId = player.playerControllerId;
			byte slot = player.gameObject.GetComponent<NetworkLobbyPlayer>().slot;
			this.lobbySlots[(int)slot] = null;
			base.OnServerRemovePlayer(conn, player);
			for (int i = 0; i < this.lobbySlots.Length; i++)
			{
				NetworkLobbyPlayer networkLobbyPlayer = this.lobbySlots[i];
				if (networkLobbyPlayer != null)
				{
					networkLobbyPlayer.GetComponent<NetworkLobbyPlayer>().readyToBegin = false;
					NetworkLobbyManager.s_LobbyReadyToBeginMessage.slotId = networkLobbyPlayer.slot;
					NetworkLobbyManager.s_LobbyReadyToBeginMessage.readyState = false;
					NetworkServer.SendToReady(null, 43, NetworkLobbyManager.s_LobbyReadyToBeginMessage);
				}
			}
			this.OnLobbyServerPlayerRemoved(conn, playerControllerId);
		}

		public override void ServerChangeScene(string sceneName)
		{
			if (sceneName == this.m_LobbyScene)
			{
				for (int i = 0; i < this.lobbySlots.Length; i++)
				{
					NetworkLobbyPlayer networkLobbyPlayer = this.lobbySlots[i];
					if (!(networkLobbyPlayer == null))
					{
						NetworkIdentity component = networkLobbyPlayer.GetComponent<NetworkIdentity>();
						PlayerController playerController;
						if (component.connectionToClient.GetPlayerController(component.playerControllerId, out playerController))
						{
							NetworkServer.Destroy(playerController.gameObject);
						}
						if (NetworkServer.active)
						{
							networkLobbyPlayer.GetComponent<NetworkLobbyPlayer>().readyToBegin = false;
							NetworkServer.ReplacePlayerForConnection(component.connectionToClient, networkLobbyPlayer.gameObject, component.playerControllerId);
						}
					}
				}
			}
			base.ServerChangeScene(sceneName);
		}

		public override void OnServerSceneChanged(string sceneName)
		{
			if (sceneName != this.m_LobbyScene)
			{
				for (int i = 0; i < this.m_PendingPlayers.Count; i++)
				{
					NetworkLobbyManager.PendingPlayer pendingPlayer = this.m_PendingPlayers[i];
					this.SceneLoadedForPlayer(pendingPlayer.conn, pendingPlayer.lobbyPlayer);
				}
				this.m_PendingPlayers.Clear();
			}
			this.OnLobbyServerSceneChanged(sceneName);
		}

		private void OnServerReadyToBeginMessage(NetworkMessage netMsg)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkLobbyManager OnServerReadyToBeginMessage");
			}
			netMsg.ReadMessage<LobbyReadyToBeginMessage>(NetworkLobbyManager.s_ReadyToBeginMessage);
			PlayerController playerController;
			if (!netMsg.conn.GetPlayerController((short)NetworkLobbyManager.s_ReadyToBeginMessage.slotId, out playerController))
			{
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkLobbyManager OnServerReadyToBeginMessage invalid playerControllerId " + NetworkLobbyManager.s_ReadyToBeginMessage.slotId);
				}
			}
			else
			{
				NetworkLobbyPlayer component = playerController.gameObject.GetComponent<NetworkLobbyPlayer>();
				component.readyToBegin = NetworkLobbyManager.s_ReadyToBeginMessage.readyState;
				NetworkServer.SendToReady(null, 43, new LobbyReadyToBeginMessage
				{
					slotId = component.slot,
					readyState = NetworkLobbyManager.s_ReadyToBeginMessage.readyState
				});
				this.CheckReadyToBegin();
			}
		}

		private void OnServerSceneLoadedMessage(NetworkMessage netMsg)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkLobbyManager OnSceneLoadedMessage");
			}
			netMsg.ReadMessage<IntegerMessage>(NetworkLobbyManager.s_SceneLoadedMessage);
			PlayerController playerController;
			if (!netMsg.conn.GetPlayerController((short)NetworkLobbyManager.s_SceneLoadedMessage.value, out playerController))
			{
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkLobbyManager OnServerSceneLoadedMessage invalid playerControllerId " + NetworkLobbyManager.s_SceneLoadedMessage.value);
				}
			}
			else
			{
				this.SceneLoadedForPlayer(netMsg.conn, playerController.gameObject);
			}
		}

		private void OnServerReturnToLobbyMessage(NetworkMessage netMsg)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkLobbyManager OnServerReturnToLobbyMessage");
			}
			this.ServerReturnToLobby();
		}

		public override void OnStartServer()
		{
			if (string.IsNullOrEmpty(this.m_LobbyScene))
			{
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkLobbyManager LobbyScene is empty. Set the LobbyScene in the inspector for the NetworkLobbyMangaer");
				}
			}
			else if (string.IsNullOrEmpty(this.m_PlayScene))
			{
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkLobbyManager PlayScene is empty. Set the PlayScene in the inspector for the NetworkLobbyMangaer");
				}
			}
			else
			{
				if (this.lobbySlots.Length == 0)
				{
					this.lobbySlots = new NetworkLobbyPlayer[this.maxPlayers];
				}
				NetworkServer.RegisterHandler(43, new NetworkMessageDelegate(this.OnServerReadyToBeginMessage));
				NetworkServer.RegisterHandler(44, new NetworkMessageDelegate(this.OnServerSceneLoadedMessage));
				NetworkServer.RegisterHandler(46, new NetworkMessageDelegate(this.OnServerReturnToLobbyMessage));
				this.OnLobbyStartServer();
			}
		}

		public override void OnStartHost()
		{
			this.OnLobbyStartHost();
		}

		public override void OnStopHost()
		{
			this.OnLobbyStopHost();
		}

		public override void OnStartClient(NetworkClient lobbyClient)
		{
			if (this.lobbySlots.Length == 0)
			{
				this.lobbySlots = new NetworkLobbyPlayer[this.maxPlayers];
			}
			if (this.m_LobbyPlayerPrefab == null || this.m_LobbyPlayerPrefab.gameObject == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkLobbyManager no LobbyPlayer prefab is registered. Please add a LobbyPlayer prefab.");
				}
			}
			else
			{
				ClientScene.RegisterPrefab(this.m_LobbyPlayerPrefab.gameObject);
			}
			if (this.m_GamePlayerPrefab == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkLobbyManager no GamePlayer prefab is registered. Please add a GamePlayer prefab.");
				}
			}
			else
			{
				ClientScene.RegisterPrefab(this.m_GamePlayerPrefab);
			}
			lobbyClient.RegisterHandler(43, new NetworkMessageDelegate(this.OnClientReadyToBegin));
			lobbyClient.RegisterHandler(45, new NetworkMessageDelegate(this.OnClientAddPlayerFailedMessage));
			this.OnLobbyStartClient(lobbyClient);
		}

		public override void OnClientConnect(NetworkConnection conn)
		{
			this.OnLobbyClientConnect(conn);
			this.CallOnClientEnterLobby();
			base.OnClientConnect(conn);
		}

		public override void OnClientDisconnect(NetworkConnection conn)
		{
			this.OnLobbyClientDisconnect(conn);
			base.OnClientDisconnect(conn);
		}

		public override void OnStopClient()
		{
			this.OnLobbyStopClient();
			this.CallOnClientExitLobby();
		}

		public override void OnClientSceneChanged(NetworkConnection conn)
		{
			string name = SceneManager.GetSceneAt(0).name;
			if (name == this.m_LobbyScene)
			{
				if (this.client.isConnected)
				{
					this.CallOnClientEnterLobby();
				}
			}
			else
			{
				this.CallOnClientExitLobby();
			}
			base.OnClientSceneChanged(conn);
			this.OnLobbyClientSceneChanged(conn);
		}

		private void OnClientReadyToBegin(NetworkMessage netMsg)
		{
			netMsg.ReadMessage<LobbyReadyToBeginMessage>(NetworkLobbyManager.s_LobbyReadyToBeginMessage);
			if ((int)NetworkLobbyManager.s_LobbyReadyToBeginMessage.slotId >= this.lobbySlots.Count<NetworkLobbyPlayer>())
			{
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkLobbyManager OnClientReadyToBegin invalid lobby slot " + NetworkLobbyManager.s_LobbyReadyToBeginMessage.slotId);
				}
			}
			else
			{
				NetworkLobbyPlayer networkLobbyPlayer = this.lobbySlots[(int)NetworkLobbyManager.s_LobbyReadyToBeginMessage.slotId];
				if (networkLobbyPlayer == null || networkLobbyPlayer.gameObject == null)
				{
					if (LogFilter.logError)
					{
						Debug.LogError("NetworkLobbyManager OnClientReadyToBegin no player at lobby slot " + NetworkLobbyManager.s_LobbyReadyToBeginMessage.slotId);
					}
				}
				else
				{
					networkLobbyPlayer.readyToBegin = NetworkLobbyManager.s_LobbyReadyToBeginMessage.readyState;
					networkLobbyPlayer.OnClientReady(NetworkLobbyManager.s_LobbyReadyToBeginMessage.readyState);
				}
			}
		}

		private void OnClientAddPlayerFailedMessage(NetworkMessage netMsg)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkLobbyManager Add Player failed.");
			}
			this.OnLobbyClientAddPlayerFailed();
		}

		public virtual void OnLobbyStartHost()
		{
		}

		public virtual void OnLobbyStopHost()
		{
		}

		public virtual void OnLobbyStartServer()
		{
		}

		public virtual void OnLobbyServerConnect(NetworkConnection conn)
		{
		}

		public virtual void OnLobbyServerDisconnect(NetworkConnection conn)
		{
		}

		public virtual void OnLobbyServerSceneChanged(string sceneName)
		{
		}

		public virtual GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
		{
			return null;
		}

		public virtual GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
		{
			return null;
		}

		public virtual void OnLobbyServerPlayerRemoved(NetworkConnection conn, short playerControllerId)
		{
		}

		public virtual bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
		{
			return true;
		}

		public virtual void OnLobbyServerPlayersReady()
		{
			this.ServerChangeScene(this.m_PlayScene);
		}

		public virtual void OnLobbyClientEnter()
		{
		}

		public virtual void OnLobbyClientExit()
		{
		}

		public virtual void OnLobbyClientConnect(NetworkConnection conn)
		{
		}

		public virtual void OnLobbyClientDisconnect(NetworkConnection conn)
		{
		}

		public virtual void OnLobbyStartClient(NetworkClient lobbyClient)
		{
		}

		public virtual void OnLobbyStopClient()
		{
		}

		public virtual void OnLobbyClientSceneChanged(NetworkConnection conn)
		{
		}

		public virtual void OnLobbyClientAddPlayerFailed()
		{
		}

		private void OnGUI()
		{
			if (this.showLobbyGUI)
			{
				string name = SceneManager.GetSceneAt(0).name;
				if (!(name != this.m_LobbyScene))
				{
					Rect position = new Rect(90f, 180f, 500f, 150f);
					GUI.Box(position, "Players:");
					if (NetworkClient.active)
					{
						Rect position2 = new Rect(100f, 300f, 120f, 20f);
						if (GUI.Button(position2, "Add Player"))
						{
							this.TryToAddPlayer();
						}
					}
				}
			}
		}

		public void TryToAddPlayer()
		{
			if (NetworkClient.active)
			{
				short num = -1;
				List<PlayerController> playerControllers = NetworkClient.allClients[0].connection.playerControllers;
				if (playerControllers.Count < this.maxPlayers)
				{
					num = (short)playerControllers.Count;
				}
				else
				{
					short num2 = 0;
					while ((int)num2 < this.maxPlayers)
					{
						if (!playerControllers[(int)num2].IsValid)
						{
							num = num2;
							break;
						}
						num2 += 1;
					}
				}
				if (LogFilter.logDebug)
				{
					Debug.Log(string.Concat(new object[]
					{
						"NetworkLobbyManager TryToAddPlayer controllerId ",
						num,
						" ready:",
						ClientScene.ready
					}));
				}
				if (num == -1)
				{
					if (LogFilter.logDebug)
					{
						Debug.Log("NetworkLobbyManager No Space!");
					}
				}
				else if (ClientScene.ready)
				{
					ClientScene.AddPlayer(num);
				}
				else
				{
					ClientScene.AddPlayer(NetworkClient.allClients[0].connection, num);
				}
			}
			else if (LogFilter.logDebug)
			{
				Debug.Log("NetworkLobbyManager NetworkClient not active!");
			}
		}

		private struct PendingPlayer
		{
			public NetworkConnection conn;

			public GameObject lobbyPlayer;
		}
	}
}
