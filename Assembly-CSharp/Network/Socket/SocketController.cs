using System;

namespace Network.Socket
{
	public sealed class SocketController
	{
		private SocketPongInfo pingpong;

		private SocketSynchronize synchronize;

		private SocketSynchronizeListener synchronizeListener;

		private SocketResponseReceive responseReceive;

		private SocketNpCloudListener npCloudListener;

		private SocketConnectionAsync connection;

		public SocketController(SocketPongInfo pingpong, SocketSynchronize synchronize)
		{
			this.pingpong = pingpong;
			this.synchronize = synchronize;
			SocketSynchronizeMonitor.Create(this.synchronize);
			this.responseReceive = new SocketResponseReceive(this.synchronize);
			this.npCloudListener = new SocketNpCloudListener(this.responseReceive);
			SocketNpCloudListenerLostPacketSimulation.Create(this.npCloudListener);
			this.synchronizeListener = new SocketSynchronizeListener(this);
			this.npCloudListener.AddListener(this.synchronizeListener);
		}

		public SocketController(SocketPongInfo pingpong)
		{
			this.pingpong = pingpong;
			this.synchronize = null;
			this.synchronizeListener = null;
			this.responseReceive = new SocketResponseReceive(this.synchronize);
			this.npCloudListener = new SocketNpCloudListener(this.responseReceive);
			SocketNpCloudListenerLostPacketSimulation.Create(this.npCloudListener);
		}

		public SocketConnectionAsync Connection(string userId)
		{
			this.connection = new SocketConnectionAsync(userId, this);
			return this.connection;
		}

		public SocketPongInfo GetPongInfo()
		{
			return this.pingpong;
		}

		public SocketSynchronize GetSocketSynchronize()
		{
			return this.synchronize;
		}

		public SocketResponseReceive GetResponseReceive()
		{
			return this.responseReceive;
		}

		public SocketNpCloudListener GetEventListener()
		{
			return this.npCloudListener;
		}

		public SocketConnectionAsync GetConnection()
		{
			return this.connection;
		}

		public void DeleteConnection()
		{
			this.connection.Delete();
			this.connection = null;
		}

		public void Clear()
		{
			this.npCloudListener.ClearListener();
			this.responseReceive.ClearCallback();
			if (this.synchronize != null)
			{
				this.synchronize.Clear();
				this.npCloudListener.AddListener(this.synchronizeListener);
			}
		}

		public void Delete()
		{
			if (this.connection != null)
			{
				this.DeleteConnection();
			}
			this.npCloudListener.ClearListener();
			this.npCloudListener = null;
			this.synchronizeListener = null;
			this.responseReceive.ClearCallback();
			this.responseReceive = null;
			this.synchronize.Clear();
			this.synchronize = null;
			this.pingpong = null;
		}
	}
}
