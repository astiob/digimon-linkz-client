using System;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;

namespace UnityEngine.Networking
{
	[DisallowMultipleComponent]
	[AddComponentMenu("Network/NetworkLobbyPlayer")]
	public class NetworkLobbyPlayer : NetworkBehaviour
	{
		[Tooltip("Enable to show the default lobby GUI for this player.")]
		[SerializeField]
		public bool ShowLobbyGUI = true;

		private byte m_Slot;

		private bool m_ReadyToBegin;

		public byte slot
		{
			get
			{
				return this.m_Slot;
			}
			set
			{
				this.m_Slot = value;
			}
		}

		public bool readyToBegin
		{
			get
			{
				return this.m_ReadyToBegin;
			}
			set
			{
				this.m_ReadyToBegin = value;
			}
		}

		private void Start()
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}

		private void OnEnable()
		{
			SceneManager.sceneLoaded += this.OnSceneLoaded;
		}

		private void OnDisable()
		{
			SceneManager.sceneLoaded -= this.OnSceneLoaded;
		}

		public override void OnStartClient()
		{
			NetworkLobbyManager networkLobbyManager = NetworkManager.singleton as NetworkLobbyManager;
			if (networkLobbyManager)
			{
				networkLobbyManager.lobbySlots[(int)this.m_Slot] = this;
				this.m_ReadyToBegin = false;
				this.OnClientEnterLobby();
			}
			else
			{
				Debug.LogError("LobbyPlayer could not find a NetworkLobbyManager. The LobbyPlayer requires a NetworkLobbyManager object to function. Make sure that there is one in the scene.");
			}
		}

		public void SendReadyToBeginMessage()
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkLobbyPlayer SendReadyToBeginMessage");
			}
			NetworkLobbyManager networkLobbyManager = NetworkManager.singleton as NetworkLobbyManager;
			if (networkLobbyManager)
			{
				LobbyReadyToBeginMessage lobbyReadyToBeginMessage = new LobbyReadyToBeginMessage();
				lobbyReadyToBeginMessage.slotId = (byte)base.playerControllerId;
				lobbyReadyToBeginMessage.readyState = true;
				networkLobbyManager.client.Send(43, lobbyReadyToBeginMessage);
			}
		}

		public void SendNotReadyToBeginMessage()
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkLobbyPlayer SendReadyToBeginMessage");
			}
			NetworkLobbyManager networkLobbyManager = NetworkManager.singleton as NetworkLobbyManager;
			if (networkLobbyManager)
			{
				LobbyReadyToBeginMessage lobbyReadyToBeginMessage = new LobbyReadyToBeginMessage();
				lobbyReadyToBeginMessage.slotId = (byte)base.playerControllerId;
				lobbyReadyToBeginMessage.readyState = false;
				networkLobbyManager.client.Send(43, lobbyReadyToBeginMessage);
			}
		}

		public void SendSceneLoadedMessage()
		{
			if (LogFilter.logDebug)
			{
				Debug.Log("NetworkLobbyPlayer SendSceneLoadedMessage");
			}
			NetworkLobbyManager networkLobbyManager = NetworkManager.singleton as NetworkLobbyManager;
			if (networkLobbyManager)
			{
				IntegerMessage msg = new IntegerMessage((int)base.playerControllerId);
				networkLobbyManager.client.Send(44, msg);
			}
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			NetworkLobbyManager networkLobbyManager = NetworkManager.singleton as NetworkLobbyManager;
			if (networkLobbyManager)
			{
				string name = scene.name;
				if (name == networkLobbyManager.lobbyScene)
				{
					return;
				}
			}
			if (base.isLocalPlayer)
			{
				this.SendSceneLoadedMessage();
			}
		}

		public void RemovePlayer()
		{
			if (base.isLocalPlayer && !this.m_ReadyToBegin)
			{
				if (LogFilter.logDebug)
				{
					Debug.Log("NetworkLobbyPlayer RemovePlayer");
				}
				ClientScene.RemovePlayer(base.GetComponent<NetworkIdentity>().playerControllerId);
			}
		}

		public virtual void OnClientEnterLobby()
		{
		}

		public virtual void OnClientExitLobby()
		{
		}

		public virtual void OnClientReady(bool readyState)
		{
		}

		public override bool OnSerialize(NetworkWriter writer, bool initialState)
		{
			writer.WritePackedUInt32(1u);
			writer.Write(this.m_Slot);
			writer.Write(this.m_ReadyToBegin);
			return true;
		}

		public override void OnDeserialize(NetworkReader reader, bool initialState)
		{
			if (reader.ReadPackedUInt32() != 0u)
			{
				this.m_Slot = reader.ReadByte();
				this.m_ReadyToBegin = reader.ReadBoolean();
			}
		}

		private void OnGUI()
		{
			if (this.ShowLobbyGUI)
			{
				NetworkLobbyManager networkLobbyManager = NetworkManager.singleton as NetworkLobbyManager;
				if (networkLobbyManager)
				{
					if (!networkLobbyManager.showLobbyGUI)
					{
						return;
					}
					string name = SceneManager.GetSceneAt(0).name;
					if (name != networkLobbyManager.lobbyScene)
					{
						return;
					}
				}
				Rect position = new Rect((float)(100 + this.m_Slot * 100), 200f, 90f, 20f);
				if (base.isLocalPlayer)
				{
					string text;
					if (this.m_ReadyToBegin)
					{
						text = "(Ready)";
					}
					else
					{
						text = "(Not Ready)";
					}
					GUI.Label(position, text);
					if (this.m_ReadyToBegin)
					{
						position.y += 25f;
						if (GUI.Button(position, "STOP"))
						{
							this.SendNotReadyToBeginMessage();
						}
					}
					else
					{
						position.y += 25f;
						if (GUI.Button(position, "START"))
						{
							this.SendReadyToBeginMessage();
						}
						position.y += 25f;
						if (GUI.Button(position, "Remove"))
						{
							ClientScene.RemovePlayer(base.GetComponent<NetworkIdentity>().playerControllerId);
						}
					}
				}
				else
				{
					GUI.Label(position, "Player [" + base.netId + "]");
					position.y += 25f;
					GUI.Label(position, "Ready [" + this.m_ReadyToBegin + "]");
				}
			}
		}
	}
}
