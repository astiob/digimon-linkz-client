using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;

namespace UnityEngine.Networking
{
	[AddComponentMenu("Network/NetworkManager")]
	public class NetworkManager : MonoBehaviour
	{
		[SerializeField]
		private int m_NetworkPort = 7777;

		[SerializeField]
		private bool m_ServerBindToIP;

		[SerializeField]
		private string m_ServerBindAddress = "";

		[SerializeField]
		private string m_NetworkAddress = "localhost";

		[SerializeField]
		private bool m_DontDestroyOnLoad = true;

		[SerializeField]
		private bool m_RunInBackground = true;

		[SerializeField]
		private bool m_ScriptCRCCheck = true;

		[SerializeField]
		private float m_MaxDelay = 0.01f;

		[SerializeField]
		private LogFilter.FilterLevel m_LogLevel = LogFilter.FilterLevel.Info;

		[SerializeField]
		private GameObject m_PlayerPrefab;

		[SerializeField]
		private bool m_AutoCreatePlayer = true;

		[SerializeField]
		private PlayerSpawnMethod m_PlayerSpawnMethod;

		[SerializeField]
		private string m_OfflineScene = "";

		[SerializeField]
		private string m_OnlineScene = "";

		[SerializeField]
		private List<GameObject> m_SpawnPrefabs = new List<GameObject>();

		[SerializeField]
		private bool m_CustomConfig;

		[SerializeField]
		private int m_MaxConnections = 4;

		[SerializeField]
		private ConnectionConfig m_ConnectionConfig;

		[SerializeField]
		private GlobalConfig m_GlobalConfig;

		[SerializeField]
		private List<QosType> m_Channels = new List<QosType>();

		[SerializeField]
		private bool m_UseWebSockets;

		[SerializeField]
		private bool m_UseSimulator;

		[SerializeField]
		private int m_SimulatedLatency = 1;

		[SerializeField]
		private float m_PacketLossPercentage;

		[SerializeField]
		private int m_MaxBufferedPackets = 16;

		[SerializeField]
		private bool m_AllowFragmentation = true;

		[SerializeField]
		private string m_MatchHost = "mm.unet.unity3d.com";

		[SerializeField]
		private int m_MatchPort = 443;

		[SerializeField]
		public string matchName = "default";

		[SerializeField]
		public uint matchSize = 4u;

		private NetworkMigrationManager m_MigrationManager;

		private EndPoint m_EndPoint;

		private bool m_ClientLoadedScene;

		public static string networkSceneName = "";

		public bool isNetworkActive;

		public NetworkClient client;

		private static List<Transform> s_StartPositions = new List<Transform>();

		private static int s_StartPositionIndex;

		public MatchInfo matchInfo;

		public NetworkMatch matchMaker;

		public List<MatchInfoSnapshot> matches;

		public static NetworkManager singleton;

		private static AddPlayerMessage s_AddPlayerMessage = new AddPlayerMessage();

		private static RemovePlayerMessage s_RemovePlayerMessage = new RemovePlayerMessage();

		private static ErrorMessage s_ErrorMessage = new ErrorMessage();

		private static AsyncOperation s_LoadingSceneAsync;

		private static NetworkConnection s_ClientReadyConnection;

		private static string s_Address;

		public int networkPort
		{
			get
			{
				return this.m_NetworkPort;
			}
			set
			{
				this.m_NetworkPort = value;
			}
		}

		public bool serverBindToIP
		{
			get
			{
				return this.m_ServerBindToIP;
			}
			set
			{
				this.m_ServerBindToIP = value;
			}
		}

		public string serverBindAddress
		{
			get
			{
				return this.m_ServerBindAddress;
			}
			set
			{
				this.m_ServerBindAddress = value;
			}
		}

		public string networkAddress
		{
			get
			{
				return this.m_NetworkAddress;
			}
			set
			{
				this.m_NetworkAddress = value;
			}
		}

		public bool dontDestroyOnLoad
		{
			get
			{
				return this.m_DontDestroyOnLoad;
			}
			set
			{
				this.m_DontDestroyOnLoad = value;
			}
		}

		public bool runInBackground
		{
			get
			{
				return this.m_RunInBackground;
			}
			set
			{
				this.m_RunInBackground = value;
			}
		}

		public bool scriptCRCCheck
		{
			get
			{
				return this.m_ScriptCRCCheck;
			}
			set
			{
				this.m_ScriptCRCCheck = value;
			}
		}

		[Obsolete("moved to NetworkMigrationManager")]
		public bool sendPeerInfo
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public float maxDelay
		{
			get
			{
				return this.m_MaxDelay;
			}
			set
			{
				this.m_MaxDelay = value;
			}
		}

		public LogFilter.FilterLevel logLevel
		{
			get
			{
				return this.m_LogLevel;
			}
			set
			{
				this.m_LogLevel = value;
				LogFilter.currentLogLevel = (int)value;
			}
		}

		public GameObject playerPrefab
		{
			get
			{
				return this.m_PlayerPrefab;
			}
			set
			{
				this.m_PlayerPrefab = value;
			}
		}

		public bool autoCreatePlayer
		{
			get
			{
				return this.m_AutoCreatePlayer;
			}
			set
			{
				this.m_AutoCreatePlayer = value;
			}
		}

		public PlayerSpawnMethod playerSpawnMethod
		{
			get
			{
				return this.m_PlayerSpawnMethod;
			}
			set
			{
				this.m_PlayerSpawnMethod = value;
			}
		}

		public string offlineScene
		{
			get
			{
				return this.m_OfflineScene;
			}
			set
			{
				this.m_OfflineScene = value;
			}
		}

		public string onlineScene
		{
			get
			{
				return this.m_OnlineScene;
			}
			set
			{
				this.m_OnlineScene = value;
			}
		}

		public List<GameObject> spawnPrefabs
		{
			get
			{
				return this.m_SpawnPrefabs;
			}
		}

		public List<Transform> startPositions
		{
			get
			{
				return NetworkManager.s_StartPositions;
			}
		}

		public bool customConfig
		{
			get
			{
				return this.m_CustomConfig;
			}
			set
			{
				this.m_CustomConfig = value;
			}
		}

		public ConnectionConfig connectionConfig
		{
			get
			{
				if (this.m_ConnectionConfig == null)
				{
					this.m_ConnectionConfig = new ConnectionConfig();
				}
				return this.m_ConnectionConfig;
			}
		}

		public GlobalConfig globalConfig
		{
			get
			{
				if (this.m_GlobalConfig == null)
				{
					this.m_GlobalConfig = new GlobalConfig();
				}
				return this.m_GlobalConfig;
			}
		}

		public int maxConnections
		{
			get
			{
				return this.m_MaxConnections;
			}
			set
			{
				this.m_MaxConnections = value;
			}
		}

		public List<QosType> channels
		{
			get
			{
				return this.m_Channels;
			}
		}

		public EndPoint secureTunnelEndpoint
		{
			get
			{
				return this.m_EndPoint;
			}
			set
			{
				this.m_EndPoint = value;
			}
		}

		public bool useWebSockets
		{
			get
			{
				return this.m_UseWebSockets;
			}
			set
			{
				this.m_UseWebSockets = value;
			}
		}

		public bool useSimulator
		{
			get
			{
				return this.m_UseSimulator;
			}
			set
			{
				this.m_UseSimulator = value;
			}
		}

		public int simulatedLatency
		{
			get
			{
				return this.m_SimulatedLatency;
			}
			set
			{
				this.m_SimulatedLatency = value;
			}
		}

		public float packetLossPercentage
		{
			get
			{
				return this.m_PacketLossPercentage;
			}
			set
			{
				this.m_PacketLossPercentage = value;
			}
		}

		public string matchHost
		{
			get
			{
				return this.m_MatchHost;
			}
			set
			{
				this.m_MatchHost = value;
			}
		}

		public int matchPort
		{
			get
			{
				return this.m_MatchPort;
			}
			set
			{
				this.m_MatchPort = value;
			}
		}

		public bool clientLoadedScene
		{
			get
			{
				return this.m_ClientLoadedScene;
			}
			set
			{
				this.m_ClientLoadedScene = value;
			}
		}

		public NetworkMigrationManager migrationManager
		{
			get
			{
				return this.m_MigrationManager;
			}
		}

		public int numPlayers
		{
			get
			{
				int num = 0;
				for (int i = 0; i < NetworkServer.connections.Count; i++)
				{
					NetworkConnection networkConnection = NetworkServer.connections[i];
					if (networkConnection != null)
					{
						for (int j = 0; j < networkConnection.playerControllers.Count; j++)
						{
							if (networkConnection.playerControllers[j].IsValid)
							{
								num++;
							}
						}
					}
				}
				return num;
			}
		}

		private void Awake()
		{
			this.InitializeSingleton();
		}

		private void InitializeSingleton()
		{
			if (!(NetworkManager.singleton != null) || !(NetworkManager.singleton == this))
			{
				int logLevel = (int)this.m_LogLevel;
				if (logLevel != -1)
				{
					LogFilter.currentLogLevel = logLevel;
				}
				if (this.m_DontDestroyOnLoad)
				{
					if (NetworkManager.singleton != null)
					{
						if (LogFilter.logDev)
						{
							Debug.Log("Multiple NetworkManagers detected in the scene. Only one NetworkManager can exist at a time. The duplicate NetworkManager will not be used.");
						}
						Object.Destroy(base.gameObject);
						return;
					}
					if (LogFilter.logDev)
					{
						Debug.Log("NetworkManager created singleton (DontDestroyOnLoad)");
					}
					NetworkManager.singleton = this;
					if (Application.isPlaying)
					{
						Object.DontDestroyOnLoad(base.gameObject);
					}
				}
				else
				{
					if (LogFilter.logDev)
					{
						Debug.Log("NetworkManager created singleton (ForScene)");
					}
					NetworkManager.singleton = this;
				}
				if (this.m_NetworkAddress != "")
				{
					NetworkManager.s_Address = this.m_NetworkAddress;
				}
				else if (NetworkManager.s_Address != "")
				{
					this.m_NetworkAddress = NetworkManager.s_Address;
				}
			}
		}

		private void OnValidate()
		{
			if (this.m_SimulatedLatency < 1)
			{
				this.m_SimulatedLatency = 1;
			}
			if (this.m_SimulatedLatency > 500)
			{
				this.m_SimulatedLatency = 500;
			}
			if (this.m_PacketLossPercentage < 0f)
			{
				this.m_PacketLossPercentage = 0f;
			}
			if (this.m_PacketLossPercentage > 99f)
			{
				this.m_PacketLossPercentage = 99f;
			}
			if (this.m_MaxConnections <= 0)
			{
				this.m_MaxConnections = 1;
			}
			if (this.m_MaxConnections > 32000)
			{
				this.m_MaxConnections = 32000;
			}
			if (this.m_MaxBufferedPackets <= 0)
			{
				this.m_MaxBufferedPackets = 0;
			}
			if (this.m_MaxBufferedPackets > 512)
			{
				this.m_MaxBufferedPackets = 512;
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkManager - MaxBufferedPackets cannot be more than " + 512);
				}
			}
			if (this.m_PlayerPrefab != null && this.m_PlayerPrefab.GetComponent<NetworkIdentity>() == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkManager - playerPrefab must have a NetworkIdentity.");
				}
				this.m_PlayerPrefab = null;
			}
			if (this.m_ConnectionConfig != null && this.m_ConnectionConfig.MinUpdateTimeout <= 0u)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("NetworkManager MinUpdateTimeout cannot be zero or less. The value will be reset to 1 millisecond");
				}
				this.m_ConnectionConfig.MinUpdateTimeout = 1u;
			}
			if (this.m_GlobalConfig != null)
			{
				if (this.m_GlobalConfig.ThreadAwakeTimeout <= 0u)
				{
					if (LogFilter.logError)
					{
						Debug.LogError("NetworkManager ThreadAwakeTimeout cannot be zero or less. The value will be reset to 1 millisecond");
					}
					this.m_GlobalConfig.ThreadAwakeTimeout = 1u;
				}
			}
		}

		internal void RegisterServerMessages()
		{
			NetworkServer.RegisterHandler(32, new NetworkMessageDelegate(this.OnServerConnectInternal));
			NetworkServer.RegisterHandler(33, new NetworkMessageDelegate(this.OnServerDisconnectInternal));
			NetworkServer.RegisterHandler(35, new NetworkMessageDelegate(this.OnServerReadyMessageInternal));
			NetworkServer.RegisterHandler(37, new NetworkMessageDelegate(this.OnServerAddPlayerMessageInternal));
			NetworkServer.RegisterHandler(38, new NetworkMessageDelegate(this.OnServerRemovePlayerMessageInternal));
			NetworkServer.RegisterHandler(34, new NetworkMessageDelegate(this.OnServerErrorInternal));
		}

		public void SetupMigrationManager(NetworkMigrationManager man)
		{
			this.m_MigrationManager = man;
		}

		public bool StartServer(ConnectionConfig config, int maxConnections)
		{
			return this.StartServer(null, config, maxConnections);
		}

		public bool StartServer()
		{
			return this.StartServer(null);
		}

		public bool StartServer(MatchInfo info)
		{
			return this.StartServer(info, null, -1);
		}

		private bool StartServer(MatchInfo info, ConnectionConfig config, int maxConnections)
		{
			this.InitializeSingleton();
			this.OnStartServer();
			if (this.m_RunInBackground)
			{
				Application.runInBackground = true;
			}
			NetworkCRC.scriptCRCCheck = this.scriptCRCCheck;
			NetworkServer.useWebSockets = this.m_UseWebSockets;
			if (this.m_GlobalConfig != null)
			{
				NetworkTransport.Init(this.m_GlobalConfig);
			}
			if (this.m_CustomConfig && this.m_ConnectionConfig != null && config == null)
			{
				this.m_ConnectionConfig.Channels.Clear();
				for (int i = 0; i < this.m_Channels.Count; i++)
				{
					this.m_ConnectionConfig.AddChannel(this.m_Channels[i]);
				}
				NetworkServer.Configure(this.m_ConnectionConfig, this.m_MaxConnections);
			}
			if (config != null)
			{
				NetworkServer.Configure(config, maxConnections);
			}
			if (info != null)
			{
				if (!NetworkServer.Listen(info, this.m_NetworkPort))
				{
					if (LogFilter.logError)
					{
						Debug.LogError("StartServer listen failed.");
					}
					return false;
				}
			}
			else if (this.m_ServerBindToIP && !string.IsNullOrEmpty(this.m_ServerBindAddress))
			{
				if (!NetworkServer.Listen(this.m_ServerBindAddress, this.m_NetworkPort))
				{
					if (LogFilter.logError)
					{
						Debug.LogError("StartServer listen on " + this.m_ServerBindAddress + " failed.");
					}
					return false;
				}
			}
			else if (!NetworkServer.Listen(this.m_NetworkPort))
			{
				if (LogFilter.logError)
				{
					Debug.LogError("StartServer listen failed.");
				}
				return false;
			}
			this.RegisterServerMessages();
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkManager StartServer port:" + this.m_NetworkPort);
			}
			this.isNetworkActive = true;
			string name = SceneManager.GetSceneAt(0).name;
			if (!string.IsNullOrEmpty(this.m_OnlineScene) && this.m_OnlineScene != name && this.m_OnlineScene != this.m_OfflineScene)
			{
				this.ServerChangeScene(this.m_OnlineScene);
			}
			else
			{
				NetworkServer.SpawnObjects();
			}
			return true;
		}

		internal void RegisterClientMessages(NetworkClient client)
		{
			client.RegisterHandler(32, new NetworkMessageDelegate(this.OnClientConnectInternal));
			client.RegisterHandler(33, new NetworkMessageDelegate(this.OnClientDisconnectInternal));
			client.RegisterHandler(36, new NetworkMessageDelegate(this.OnClientNotReadyMessageInternal));
			client.RegisterHandler(34, new NetworkMessageDelegate(this.OnClientErrorInternal));
			client.RegisterHandler(39, new NetworkMessageDelegate(this.OnClientSceneInternal));
			if (this.m_PlayerPrefab != null)
			{
				ClientScene.RegisterPrefab(this.m_PlayerPrefab);
			}
			for (int i = 0; i < this.m_SpawnPrefabs.Count; i++)
			{
				GameObject gameObject = this.m_SpawnPrefabs[i];
				if (gameObject != null)
				{
					ClientScene.RegisterPrefab(gameObject);
				}
			}
		}

		public void UseExternalClient(NetworkClient externalClient)
		{
			if (this.m_RunInBackground)
			{
				Application.runInBackground = true;
			}
			if (externalClient != null)
			{
				this.client = externalClient;
				this.isNetworkActive = true;
				this.RegisterClientMessages(this.client);
				this.OnStartClient(this.client);
			}
			else
			{
				this.OnStopClient();
				ClientScene.DestroyAllClientObjects();
				ClientScene.HandleClientDisconnect(this.client.connection);
				this.client = null;
				if (!string.IsNullOrEmpty(this.m_OfflineScene))
				{
					this.ClientChangeScene(this.m_OfflineScene, false);
				}
			}
			NetworkManager.s_Address = this.m_NetworkAddress;
		}

		public NetworkClient StartClient(MatchInfo info, ConnectionConfig config, int hostPort)
		{
			this.InitializeSingleton();
			this.matchInfo = info;
			if (this.m_RunInBackground)
			{
				Application.runInBackground = true;
			}
			this.isNetworkActive = true;
			if (this.m_GlobalConfig != null)
			{
				NetworkTransport.Init(this.m_GlobalConfig);
			}
			this.client = new NetworkClient();
			this.client.hostPort = hostPort;
			if (config != null)
			{
				if (config.UsePlatformSpecificProtocols && Application.platform != RuntimePlatform.PS4 && Application.platform != RuntimePlatform.PSP2)
				{
					throw new ArgumentOutOfRangeException("Platform specific protocols are not supported on this platform");
				}
				this.client.Configure(config, 1);
			}
			else if (this.m_CustomConfig && this.m_ConnectionConfig != null)
			{
				this.m_ConnectionConfig.Channels.Clear();
				for (int i = 0; i < this.m_Channels.Count; i++)
				{
					this.m_ConnectionConfig.AddChannel(this.m_Channels[i]);
				}
				if (this.m_ConnectionConfig.UsePlatformSpecificProtocols && Application.platform != RuntimePlatform.PS4 && Application.platform != RuntimePlatform.PSP2)
				{
					throw new ArgumentOutOfRangeException("Platform specific protocols are not supported on this platform");
				}
				this.client.Configure(this.m_ConnectionConfig, this.m_MaxConnections);
			}
			this.RegisterClientMessages(this.client);
			if (this.matchInfo != null)
			{
				if (LogFilter.logDebug)
				{
					Debug.Log("NetworkManager StartClient match: " + this.matchInfo);
				}
				this.client.Connect(this.matchInfo);
			}
			else if (this.m_EndPoint != null)
			{
				if (LogFilter.logDebug)
				{
					Debug.Log("NetworkManager StartClient using provided SecureTunnel");
				}
				this.client.Connect(this.m_EndPoint);
			}
			else
			{
				if (string.IsNullOrEmpty(this.m_NetworkAddress))
				{
					if (LogFilter.logError)
					{
						Debug.LogError("Must set the Network Address field in the manager");
					}
					return null;
				}
				if (LogFilter.logDebug)
				{
					Debug.Log(string.Concat(new object[]
					{
						"NetworkManager StartClient address:",
						this.m_NetworkAddress,
						" port:",
						this.m_NetworkPort
					}));
				}
				if (this.m_UseSimulator)
				{
					this.client.ConnectWithSimulator(this.m_NetworkAddress, this.m_NetworkPort, this.m_SimulatedLatency, this.m_PacketLossPercentage);
				}
				else
				{
					this.client.Connect(this.m_NetworkAddress, this.m_NetworkPort);
				}
			}
			if (this.m_MigrationManager != null)
			{
				this.m_MigrationManager.Initialize(this.client, this.matchInfo);
			}
			this.OnStartClient(this.client);
			NetworkManager.s_Address = this.m_NetworkAddress;
			return this.client;
		}

		public NetworkClient StartClient(MatchInfo matchInfo)
		{
			return this.StartClient(matchInfo, null);
		}

		public NetworkClient StartClient()
		{
			return this.StartClient(null, null);
		}

		public NetworkClient StartClient(MatchInfo info, ConnectionConfig config)
		{
			return this.StartClient(info, config, 0);
		}

		public virtual NetworkClient StartHost(ConnectionConfig config, int maxConnections)
		{
			this.OnStartHost();
			NetworkClient result;
			if (this.StartServer(config, maxConnections))
			{
				NetworkClient networkClient = this.ConnectLocalClient();
				this.OnServerConnect(networkClient.connection);
				this.OnStartClient(networkClient);
				result = networkClient;
			}
			else
			{
				result = null;
			}
			return result;
		}

		public virtual NetworkClient StartHost(MatchInfo info)
		{
			this.OnStartHost();
			this.matchInfo = info;
			NetworkClient result;
			if (this.StartServer(info))
			{
				NetworkClient networkClient = this.ConnectLocalClient();
				this.OnStartClient(networkClient);
				result = networkClient;
			}
			else
			{
				result = null;
			}
			return result;
		}

		public virtual NetworkClient StartHost()
		{
			this.OnStartHost();
			NetworkClient result;
			if (this.StartServer())
			{
				NetworkClient networkClient = this.ConnectLocalClient();
				this.OnStartClient(networkClient);
				result = networkClient;
			}
			else
			{
				result = null;
			}
			return result;
		}

		private NetworkClient ConnectLocalClient()
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkManager StartHost port:" + this.m_NetworkPort);
			}
			this.m_NetworkAddress = "localhost";
			this.client = ClientScene.ConnectLocalServer();
			this.RegisterClientMessages(this.client);
			if (this.m_MigrationManager != null)
			{
				this.m_MigrationManager.Initialize(this.client, this.matchInfo);
			}
			return this.client;
		}

		public void StopHost()
		{
			bool active = NetworkServer.active;
			this.OnStopHost();
			this.StopServer();
			this.StopClient();
			if (this.m_MigrationManager != null)
			{
				if (active)
				{
					this.m_MigrationManager.LostHostOnHost();
				}
			}
		}

		public void StopServer()
		{
			if (NetworkServer.active)
			{
				this.OnStopServer();
				if (LogFilter.logDebug)
				{
					Debug.Log("NetworkManager StopServer");
				}
				this.isNetworkActive = false;
				NetworkServer.Shutdown();
				this.StopMatchMaker();
				if (!string.IsNullOrEmpty(this.m_OfflineScene))
				{
					this.ServerChangeScene(this.m_OfflineScene);
				}
				this.CleanupNetworkIdentities();
			}
		}

		public void StopClient()
		{
			this.OnStopClient();
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkManager StopClient");
			}
			this.isNetworkActive = false;
			if (this.client != null)
			{
				this.client.Disconnect();
				this.client.Shutdown();
				this.client = null;
			}
			this.StopMatchMaker();
			ClientScene.DestroyAllClientObjects();
			if (!string.IsNullOrEmpty(this.m_OfflineScene))
			{
				this.ClientChangeScene(this.m_OfflineScene, false);
			}
			this.CleanupNetworkIdentities();
		}

		public virtual void ServerChangeScene(string newSceneName)
		{
			if (string.IsNullOrEmpty(newSceneName))
			{
				if (LogFilter.logError)
				{
					Debug.LogError("ServerChangeScene empty scene name");
				}
			}
			else
			{
				if (LogFilter.logDebug)
				{
					Debug.Log("ServerChangeScene " + newSceneName);
				}
				NetworkServer.SetAllClientsNotReady();
				NetworkManager.networkSceneName = newSceneName;
				NetworkManager.s_LoadingSceneAsync = SceneManager.LoadSceneAsync(newSceneName);
				StringMessage msg = new StringMessage(NetworkManager.networkSceneName);
				NetworkServer.SendToAll(39, msg);
				NetworkManager.s_StartPositionIndex = 0;
				NetworkManager.s_StartPositions.Clear();
			}
		}

		private void CleanupNetworkIdentities()
		{
			foreach (NetworkIdentity networkIdentity in Resources.FindObjectsOfTypeAll<NetworkIdentity>())
			{
				networkIdentity.MarkForReset();
			}
		}

		internal void ClientChangeScene(string newSceneName, bool forceReload)
		{
			if (string.IsNullOrEmpty(newSceneName))
			{
				if (LogFilter.logError)
				{
					Debug.LogError("ClientChangeScene empty scene name");
				}
			}
			else
			{
				if (LogFilter.logDebug)
				{
					Debug.Log("ClientChangeScene newSceneName:" + newSceneName + " networkSceneName:" + NetworkManager.networkSceneName);
				}
				if (newSceneName == NetworkManager.networkSceneName)
				{
					if (this.m_MigrationManager != null)
					{
						this.FinishLoadScene();
						return;
					}
					if (!forceReload)
					{
						this.FinishLoadScene();
						return;
					}
				}
				NetworkManager.s_LoadingSceneAsync = SceneManager.LoadSceneAsync(newSceneName);
				NetworkManager.networkSceneName = newSceneName;
			}
		}

		private void FinishLoadScene()
		{
			if (this.client != null)
			{
				if (NetworkManager.s_ClientReadyConnection != null)
				{
					this.m_ClientLoadedScene = true;
					this.OnClientConnect(NetworkManager.s_ClientReadyConnection);
					NetworkManager.s_ClientReadyConnection = null;
				}
			}
			else if (LogFilter.logDev)
			{
				Debug.Log("FinishLoadScene client is null");
			}
			if (NetworkServer.active)
			{
				NetworkServer.SpawnObjects();
				this.OnServerSceneChanged(NetworkManager.networkSceneName);
			}
			if (this.IsClientConnected() && this.client != null)
			{
				this.RegisterClientMessages(this.client);
				this.OnClientSceneChanged(this.client.connection);
			}
		}

		internal static void UpdateScene()
		{
			if (!(NetworkManager.singleton == null))
			{
				if (NetworkManager.s_LoadingSceneAsync != null)
				{
					if (NetworkManager.s_LoadingSceneAsync.isDone)
					{
						if (LogFilter.logDebug)
						{
							Debug.Log("ClientChangeScene done readyCon:" + NetworkManager.s_ClientReadyConnection);
						}
						NetworkManager.singleton.FinishLoadScene();
						NetworkManager.s_LoadingSceneAsync.allowSceneActivation = true;
						NetworkManager.s_LoadingSceneAsync = null;
					}
				}
			}
		}

		private void OnDestroy()
		{
			if (LogFilter.logDev)
			{
				Debug.Log("NetworkManager destroyed");
			}
		}

		public static void RegisterStartPosition(Transform start)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log(string.Concat(new object[]
				{
					"RegisterStartPosition: (",
					start.gameObject.name,
					") ",
					start.position
				}));
			}
			NetworkManager.s_StartPositions.Add(start);
		}

		public static void UnRegisterStartPosition(Transform start)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log(string.Concat(new object[]
				{
					"UnRegisterStartPosition: (",
					start.gameObject.name,
					") ",
					start.position
				}));
			}
			NetworkManager.s_StartPositions.Remove(start);
		}

		public bool IsClientConnected()
		{
			return this.client != null && this.client.isConnected;
		}

		public static void Shutdown()
		{
			if (!(NetworkManager.singleton == null))
			{
				NetworkManager.s_StartPositions.Clear();
				NetworkManager.s_StartPositionIndex = 0;
				NetworkManager.s_ClientReadyConnection = null;
				NetworkManager.singleton.StopHost();
				NetworkManager.singleton = null;
			}
		}

		internal void OnServerConnectInternal(NetworkMessage netMsg)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkManager:OnServerConnectInternal");
			}
			netMsg.conn.SetMaxDelay(this.m_MaxDelay);
			if (this.m_MaxBufferedPackets != 512)
			{
				for (int i = 0; i < NetworkServer.numChannels; i++)
				{
					netMsg.conn.SetChannelOption(i, ChannelOption.MaxPendingBuffers, this.m_MaxBufferedPackets);
				}
			}
			if (!this.m_AllowFragmentation)
			{
				for (int j = 0; j < NetworkServer.numChannels; j++)
				{
					netMsg.conn.SetChannelOption(j, ChannelOption.AllowFragmentation, 0);
				}
			}
			if (NetworkManager.networkSceneName != "" && NetworkManager.networkSceneName != this.m_OfflineScene)
			{
				StringMessage msg = new StringMessage(NetworkManager.networkSceneName);
				netMsg.conn.Send(39, msg);
			}
			if (this.m_MigrationManager != null)
			{
				this.m_MigrationManager.SendPeerInfo();
			}
			this.OnServerConnect(netMsg.conn);
		}

		internal void OnServerDisconnectInternal(NetworkMessage netMsg)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkManager:OnServerDisconnectInternal");
			}
			if (this.m_MigrationManager != null)
			{
				this.m_MigrationManager.SendPeerInfo();
			}
			this.OnServerDisconnect(netMsg.conn);
		}

		internal void OnServerReadyMessageInternal(NetworkMessage netMsg)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkManager:OnServerReadyMessageInternal");
			}
			this.OnServerReady(netMsg.conn);
		}

		internal void OnServerAddPlayerMessageInternal(NetworkMessage netMsg)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkManager:OnServerAddPlayerMessageInternal");
			}
			netMsg.ReadMessage<AddPlayerMessage>(NetworkManager.s_AddPlayerMessage);
			if (NetworkManager.s_AddPlayerMessage.msgSize != 0)
			{
				NetworkReader extraMessageReader = new NetworkReader(NetworkManager.s_AddPlayerMessage.msgData);
				this.OnServerAddPlayer(netMsg.conn, NetworkManager.s_AddPlayerMessage.playerControllerId, extraMessageReader);
			}
			else
			{
				this.OnServerAddPlayer(netMsg.conn, NetworkManager.s_AddPlayerMessage.playerControllerId);
			}
			if (this.m_MigrationManager != null)
			{
				this.m_MigrationManager.SendPeerInfo();
			}
		}

		internal void OnServerRemovePlayerMessageInternal(NetworkMessage netMsg)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkManager:OnServerRemovePlayerMessageInternal");
			}
			netMsg.ReadMessage<RemovePlayerMessage>(NetworkManager.s_RemovePlayerMessage);
			PlayerController player;
			netMsg.conn.GetPlayerController(NetworkManager.s_RemovePlayerMessage.playerControllerId, out player);
			this.OnServerRemovePlayer(netMsg.conn, player);
			netMsg.conn.RemovePlayerController(NetworkManager.s_RemovePlayerMessage.playerControllerId);
			if (this.m_MigrationManager != null)
			{
				this.m_MigrationManager.SendPeerInfo();
			}
		}

		internal void OnServerErrorInternal(NetworkMessage netMsg)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkManager:OnServerErrorInternal");
			}
			netMsg.ReadMessage<ErrorMessage>(NetworkManager.s_ErrorMessage);
			this.OnServerError(netMsg.conn, NetworkManager.s_ErrorMessage.errorCode);
		}

		internal void OnClientConnectInternal(NetworkMessage netMsg)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkManager:OnClientConnectInternal");
			}
			netMsg.conn.SetMaxDelay(this.m_MaxDelay);
			string name = SceneManager.GetSceneAt(0).name;
			if (string.IsNullOrEmpty(this.m_OnlineScene) || this.m_OnlineScene == this.m_OfflineScene || name == this.m_OnlineScene)
			{
				this.m_ClientLoadedScene = false;
				this.OnClientConnect(netMsg.conn);
			}
			else
			{
				NetworkManager.s_ClientReadyConnection = netMsg.conn;
			}
		}

		internal void OnClientDisconnectInternal(NetworkMessage netMsg)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkManager:OnClientDisconnectInternal");
			}
			if (this.m_MigrationManager != null)
			{
				if (this.m_MigrationManager.LostHostOnClient(netMsg.conn))
				{
					return;
				}
			}
			if (!string.IsNullOrEmpty(this.m_OfflineScene))
			{
				this.ClientChangeScene(this.m_OfflineScene, false);
			}
			if (this.matchMaker != null && this.matchInfo != null && this.matchInfo.networkId != NetworkID.Invalid && this.matchInfo.nodeId != NodeID.Invalid)
			{
				this.matchMaker.DropConnection(this.matchInfo.networkId, this.matchInfo.nodeId, this.matchInfo.domain, new NetworkMatch.BasicResponseDelegate(this.OnDropConnection));
			}
			this.OnClientDisconnect(netMsg.conn);
		}

		internal void OnClientNotReadyMessageInternal(NetworkMessage netMsg)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkManager:OnClientNotReadyMessageInternal");
			}
			ClientScene.SetNotReady();
			this.OnClientNotReady(netMsg.conn);
		}

		internal void OnClientErrorInternal(NetworkMessage netMsg)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkManager:OnClientErrorInternal");
			}
			netMsg.ReadMessage<ErrorMessage>(NetworkManager.s_ErrorMessage);
			this.OnClientError(netMsg.conn, NetworkManager.s_ErrorMessage.errorCode);
		}

		internal void OnClientSceneInternal(NetworkMessage netMsg)
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkManager:OnClientSceneInternal");
			}
			string newSceneName = netMsg.reader.ReadString();
			if (this.IsClientConnected() && !NetworkServer.active)
			{
				this.ClientChangeScene(newSceneName, true);
			}
		}

		public virtual void OnServerConnect(NetworkConnection conn)
		{
		}

		public virtual void OnServerDisconnect(NetworkConnection conn)
		{
			NetworkServer.DestroyPlayersForConnection(conn);
			if (conn.lastError != NetworkError.Ok)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("ServerDisconnected due to error: " + conn.lastError);
				}
			}
		}

		public virtual void OnServerReady(NetworkConnection conn)
		{
			if (conn.playerControllers.Count == 0)
			{
				if (LogFilter.logDebug)
				{
					Debug.Log("Ready with no player object");
				}
			}
			NetworkServer.SetClientReady(conn);
		}

		public virtual void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
		{
			this.OnServerAddPlayerInternal(conn, playerControllerId);
		}

		public virtual void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
		{
			this.OnServerAddPlayerInternal(conn, playerControllerId);
		}

		private void OnServerAddPlayerInternal(NetworkConnection conn, short playerControllerId)
		{
			if (this.m_PlayerPrefab == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("The PlayerPrefab is empty on the NetworkManager. Please setup a PlayerPrefab object.");
				}
			}
			else if (this.m_PlayerPrefab.GetComponent<NetworkIdentity>() == null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("The PlayerPrefab does not have a NetworkIdentity. Please add a NetworkIdentity to the player prefab.");
				}
			}
			else if ((int)playerControllerId < conn.playerControllers.Count && conn.playerControllers[(int)playerControllerId].IsValid && conn.playerControllers[(int)playerControllerId].gameObject != null)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("There is already a player at that playerControllerId for this connections.");
				}
			}
			else
			{
				Transform startPosition = this.GetStartPosition();
				GameObject player;
				if (startPosition != null)
				{
					player = Object.Instantiate<GameObject>(this.m_PlayerPrefab, startPosition.position, startPosition.rotation);
				}
				else
				{
					player = Object.Instantiate<GameObject>(this.m_PlayerPrefab, Vector3.zero, Quaternion.identity);
				}
				NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
			}
		}

		public Transform GetStartPosition()
		{
			if (NetworkManager.s_StartPositions.Count > 0)
			{
				for (int i = NetworkManager.s_StartPositions.Count - 1; i >= 0; i--)
				{
					if (NetworkManager.s_StartPositions[i] == null)
					{
						NetworkManager.s_StartPositions.RemoveAt(i);
					}
				}
			}
			Transform result;
			if (this.m_PlayerSpawnMethod == PlayerSpawnMethod.Random && NetworkManager.s_StartPositions.Count > 0)
			{
				int index = Random.Range(0, NetworkManager.s_StartPositions.Count);
				result = NetworkManager.s_StartPositions[index];
			}
			else if (this.m_PlayerSpawnMethod == PlayerSpawnMethod.RoundRobin && NetworkManager.s_StartPositions.Count > 0)
			{
				if (NetworkManager.s_StartPositionIndex >= NetworkManager.s_StartPositions.Count)
				{
					NetworkManager.s_StartPositionIndex = 0;
				}
				Transform transform = NetworkManager.s_StartPositions[NetworkManager.s_StartPositionIndex];
				NetworkManager.s_StartPositionIndex++;
				result = transform;
			}
			else
			{
				result = null;
			}
			return result;
		}

		public virtual void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
		{
			if (player.gameObject != null)
			{
				NetworkServer.Destroy(player.gameObject);
			}
		}

		public virtual void OnServerError(NetworkConnection conn, int errorCode)
		{
		}

		public virtual void OnServerSceneChanged(string sceneName)
		{
		}

		public virtual void OnClientConnect(NetworkConnection conn)
		{
			if (!this.clientLoadedScene)
			{
				ClientScene.Ready(conn);
				if (this.m_AutoCreatePlayer)
				{
					ClientScene.AddPlayer(0);
				}
			}
		}

		public virtual void OnClientDisconnect(NetworkConnection conn)
		{
			this.StopClient();
			if (conn.lastError != NetworkError.Ok)
			{
				if (LogFilter.logError)
				{
					Debug.LogError("ClientDisconnected due to error: " + conn.lastError);
				}
			}
		}

		public virtual void OnClientError(NetworkConnection conn, int errorCode)
		{
		}

		public virtual void OnClientNotReady(NetworkConnection conn)
		{
		}

		public virtual void OnClientSceneChanged(NetworkConnection conn)
		{
			ClientScene.Ready(conn);
			if (this.m_AutoCreatePlayer)
			{
				bool flag = ClientScene.localPlayers.Count == 0;
				bool flag2 = false;
				for (int i = 0; i < ClientScene.localPlayers.Count; i++)
				{
					if (ClientScene.localPlayers[i].gameObject != null)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					flag = true;
				}
				if (flag)
				{
					ClientScene.AddPlayer(0);
				}
			}
		}

		public void StartMatchMaker()
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkManager StartMatchMaker");
			}
			this.SetMatchHost(this.m_MatchHost, this.m_MatchPort, this.m_MatchPort == 443);
		}

		public void StopMatchMaker()
		{
			if (this.matchMaker != null && this.matchInfo != null && this.matchInfo.networkId != NetworkID.Invalid && this.matchInfo.nodeId != NodeID.Invalid)
			{
				this.matchMaker.DropConnection(this.matchInfo.networkId, this.matchInfo.nodeId, this.matchInfo.domain, new NetworkMatch.BasicResponseDelegate(this.OnDropConnection));
			}
			if (this.matchMaker != null)
			{
				Object.Destroy(this.matchMaker);
				this.matchMaker = null;
			}
			this.matchInfo = null;
			this.matches = null;
		}

		public void SetMatchHost(string newHost, int port, bool https)
		{
			if (this.matchMaker == null)
			{
				this.matchMaker = base.gameObject.AddComponent<NetworkMatch>();
			}
			if (newHost == "127.0.0.1")
			{
				newHost = "localhost";
			}
			string text = "http://";
			if (https)
			{
				text = "https://";
			}
			if (newHost.StartsWith("http://"))
			{
				newHost = newHost.Replace("http://", "");
			}
			if (newHost.StartsWith("https://"))
			{
				newHost = newHost.Replace("https://", "");
			}
			this.m_MatchHost = newHost;
			this.m_MatchPort = port;
			string text2 = string.Concat(new object[]
			{
				text,
				this.m_MatchHost,
				":",
				this.m_MatchPort
			});
			if (LogFilter.logDebug)
			{
				Debug.Log("SetMatchHost:" + text2);
			}
			this.matchMaker.baseUri = new Uri(text2);
		}

		public virtual void OnStartHost()
		{
		}

		public virtual void OnStartServer()
		{
		}

		public virtual void OnStartClient(NetworkClient client)
		{
		}

		public virtual void OnStopServer()
		{
		}

		public virtual void OnStopClient()
		{
		}

		public virtual void OnStopHost()
		{
		}

		public virtual void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
		{
			if (LogFilter.logDebug)
			{
				Debug.LogFormat("NetworkManager OnMatchCreate Success:{0}, ExtendedInfo:{1}, matchInfo:{2}", new object[]
				{
					success,
					extendedInfo,
					matchInfo
				});
			}
			if (success)
			{
				this.StartHost(matchInfo);
			}
		}

		public virtual void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
		{
			if (LogFilter.logDebug)
			{
				Debug.LogFormat("NetworkManager OnMatchList Success:{0}, ExtendedInfo:{1}, matchList.Count:{2}", new object[]
				{
					success,
					extendedInfo,
					matchList.Count
				});
			}
			this.matches = matchList;
		}

		public virtual void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
		{
			if (LogFilter.logDebug)
			{
				Debug.LogFormat("NetworkManager OnMatchJoined Success:{0}, ExtendedInfo:{1}, matchInfo:{2}", new object[]
				{
					success,
					extendedInfo,
					matchInfo
				});
			}
			if (success)
			{
				this.StartClient(matchInfo);
			}
		}

		public virtual void OnDestroyMatch(bool success, string extendedInfo)
		{
			if (LogFilter.logDebug)
			{
				Debug.LogFormat("NetworkManager OnDestroyMatch Success:{0}, ExtendedInfo:{1}", new object[]
				{
					success,
					extendedInfo
				});
			}
		}

		public virtual void OnDropConnection(bool success, string extendedInfo)
		{
			if (LogFilter.logDebug)
			{
				Debug.LogFormat("NetworkManager OnDropConnection Success:{0}, ExtendedInfo:{1}", new object[]
				{
					success,
					extendedInfo
				});
			}
		}

		public virtual void OnSetMatchAttributes(bool success, string extendedInfo)
		{
			if (LogFilter.logDebug)
			{
				Debug.LogFormat("NetworkManager OnSetMatchAttributes Success:{0}, ExtendedInfo:{1}", new object[]
				{
					success,
					extendedInfo
				});
			}
		}
	}
}
