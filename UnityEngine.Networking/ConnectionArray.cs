using System;
using System.Collections.Generic;

namespace UnityEngine.Networking
{
	internal class ConnectionArray
	{
		private List<NetworkConnection> m_LocalConnections;

		private List<NetworkConnection> m_Connections;

		public ConnectionArray()
		{
			this.m_Connections = new List<NetworkConnection>();
			this.m_LocalConnections = new List<NetworkConnection>();
		}

		internal List<NetworkConnection> localConnections
		{
			get
			{
				return this.m_LocalConnections;
			}
		}

		internal List<NetworkConnection> connections
		{
			get
			{
				return this.m_Connections;
			}
		}

		public int Count
		{
			get
			{
				return this.m_Connections.Count;
			}
		}

		public int LocalIndex
		{
			get
			{
				return -this.m_LocalConnections.Count;
			}
		}

		public int Add(int connId, NetworkConnection conn)
		{
			int result;
			if (connId < 0)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("ConnectionArray Add bad id " + connId);
				}
				result = -1;
			}
			else if (connId < this.m_Connections.Count && this.m_Connections[connId] != null)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("ConnectionArray Add dupe at " + connId);
				}
				result = -1;
			}
			else
			{
				while (connId > this.m_Connections.Count - 1)
				{
					this.m_Connections.Add(null);
				}
				this.m_Connections[connId] = conn;
				result = connId;
			}
			return result;
		}

		public NetworkConnection Get(int connId)
		{
			NetworkConnection result;
			if (connId < 0)
			{
				result = this.m_LocalConnections[Mathf.Abs(connId) - 1];
			}
			else if (connId < 0 || connId > this.m_Connections.Count)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("ConnectionArray Get invalid index " + connId);
				}
				result = null;
			}
			else
			{
				result = this.m_Connections[connId];
			}
			return result;
		}

		public NetworkConnection GetUnsafe(int connId)
		{
			NetworkConnection result;
			if (connId < 0 || connId > this.m_Connections.Count)
			{
				result = null;
			}
			else
			{
				result = this.m_Connections[connId];
			}
			return result;
		}

		public void Remove(int connId)
		{
			if (connId < 0)
			{
				this.m_LocalConnections[Mathf.Abs(connId) - 1] = null;
			}
			else if (connId < 0 || connId > this.m_Connections.Count)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("ConnectionArray Remove invalid index " + connId);
				}
			}
			else
			{
				this.m_Connections[connId] = null;
			}
		}

		public int AddLocal(NetworkConnection conn)
		{
			this.m_LocalConnections.Add(conn);
			int num = -this.m_LocalConnections.Count;
			conn.connectionId = num;
			return num;
		}

		public bool ContainsPlayer(GameObject player, out NetworkConnection conn)
		{
			conn = null;
			bool result;
			if (player == null)
			{
				result = false;
			}
			else
			{
				for (int i = this.LocalIndex; i < this.m_Connections.Count; i++)
				{
					conn = this.Get(i);
					if (conn != null)
					{
						for (int j = 0; j < conn.playerControllers.Count; j++)
						{
							if (conn.playerControllers[j].IsValid && conn.playerControllers[j].gameObject == player)
							{
								return true;
							}
						}
					}
				}
				result = false;
			}
			return result;
		}
	}
}
