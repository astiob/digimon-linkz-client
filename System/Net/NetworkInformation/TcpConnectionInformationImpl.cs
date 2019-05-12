using System;

namespace System.Net.NetworkInformation
{
	internal class TcpConnectionInformationImpl : TcpConnectionInformation
	{
		private IPEndPoint local;

		private IPEndPoint remote;

		private TcpState state;

		public TcpConnectionInformationImpl(IPEndPoint local, IPEndPoint remote, TcpState state)
		{
			this.local = local;
			this.remote = remote;
			this.state = state;
		}

		public override IPEndPoint LocalEndPoint
		{
			get
			{
				return this.local;
			}
		}

		public override IPEndPoint RemoteEndPoint
		{
			get
			{
				return this.remote;
			}
		}

		public override TcpState State
		{
			get
			{
				return this.state;
			}
		}
	}
}
