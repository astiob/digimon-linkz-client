using System;

namespace System.Net.Sockets
{
	/// <summary>Provides client connections for TCP network services.</summary>
	public class TcpClient : IDisposable
	{
		private NetworkStream stream;

		private bool active;

		private Socket client;

		private bool disposed;

		private TcpClient.Properties values;

		private int recv_timeout;

		private int send_timeout;

		private int recv_buffer_size;

		private int send_buffer_size;

		private LingerOption linger_state;

		private bool no_delay;

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Sockets.TcpClient" /> class.</summary>
		public TcpClient()
		{
			this.Init(AddressFamily.InterNetwork);
			this.client.Bind(new IPEndPoint(IPAddress.Any, 0));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Sockets.TcpClient" /> class with the specified family.</summary>
		/// <param name="family">The <see cref="P:System.Net.IPAddress.AddressFamily" /> of the IP protocol. </param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="family" /> parameter is not equal to AddressFamily.InterNetwork -or- The <paramref name="family" /> parameter is not equal to AddressFamily.InterNetworkV6 </exception>
		public TcpClient(AddressFamily family)
		{
			if (family != AddressFamily.InterNetwork && family != AddressFamily.InterNetworkV6)
			{
				throw new ArgumentException("Family must be InterNetwork or InterNetworkV6", "family");
			}
			this.Init(family);
			IPAddress address = IPAddress.Any;
			if (family == AddressFamily.InterNetworkV6)
			{
				address = IPAddress.IPv6Any;
			}
			this.client.Bind(new IPEndPoint(address, 0));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Sockets.TcpClient" /> class and binds it to the specified local endpoint.</summary>
		/// <param name="localEP">The <see cref="T:System.Net.IPEndPoint" /> to which you bind the TCP <see cref="T:System.Net.Sockets.Socket" />. </param>
		/// <exception cref="T:System.ArgumentNullException">The  <paramref name="localEP" /> parameter is null. </exception>
		public TcpClient(IPEndPoint local_end_point)
		{
			this.Init(local_end_point.AddressFamily);
			this.client.Bind(local_end_point);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Sockets.TcpClient" /> class and connects to the specified port on the specified host.</summary>
		/// <param name="hostname">The DNS name of the remote host to which you intend to connect. </param>
		/// <param name="port">The port number of the remote host to which you intend to connect. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="hostname" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="port" /> parameter is not between <see cref="F:System.Net.IPEndPoint.MinPort" /> and <see cref="F:System.Net.IPEndPoint.MaxPort" />. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when accessing the socket. See the Remarks section for more information. </exception>
		public TcpClient(string hostname, int port)
		{
			this.Connect(hostname, port);
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Net.Sockets.TcpClient" />. </summary>
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Init(AddressFamily family)
		{
			this.active = false;
			if (this.client != null)
			{
				this.client.Close();
				this.client = null;
			}
			this.client = new Socket(family, SocketType.Stream, ProtocolType.Tcp);
		}

		/// <summary>Gets or set a value that indicates whether a connection has been made.</summary>
		/// <returns>true if the connection has been made; otherwise, false.</returns>
		protected bool Active
		{
			get
			{
				return this.active;
			}
			set
			{
				this.active = value;
			}
		}

		/// <summary>Gets or sets the underlying <see cref="T:System.Net.Sockets.Socket" />.</summary>
		/// <returns>The underlying network <see cref="T:System.Net.Sockets.Socket" />.</returns>
		public Socket Client
		{
			get
			{
				return this.client;
			}
			set
			{
				this.client = value;
				this.stream = null;
			}
		}

		/// <summary>Gets the amount of data that has been received from the network and is available to be read.</summary>
		/// <returns>The number of bytes of data received from the network and available to be read.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public int Available
		{
			get
			{
				return this.client.Available;
			}
		}

		/// <summary>Gets a value indicating whether the underlying <see cref="T:System.Net.Sockets.Socket" /> for a <see cref="T:System.Net.Sockets.TcpClient" /> is connected to a remote host.</summary>
		/// <returns>true if the <see cref="P:System.Net.Sockets.TcpClient.Client" /> socket was connected to a remote resource as of the most recent operation; otherwise, false.</returns>
		public bool Connected
		{
			get
			{
				return this.client.Connected;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that specifies whether the <see cref="T:System.Net.Sockets.TcpClient" /> allows only one client to use a port.</summary>
		/// <returns>true if the <see cref="T:System.Net.Sockets.TcpClient" /> allows only one client to use a specific port; otherwise, false. The default is true for Windows Server 2003 and Windows XP Service Pack 2 and later, and false for all other versions.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the underlying socket.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The underlying <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public bool ExclusiveAddressUse
		{
			get
			{
				return this.client.ExclusiveAddressUse;
			}
			set
			{
				this.client.ExclusiveAddressUse = value;
			}
		}

		internal void SetTcpClient(Socket s)
		{
			this.Client = s;
		}

		/// <summary>Gets or sets information on the linger state of the associated socket.</summary>
		/// <returns>A <see cref="T:System.Net.Sockets.LingerOption" />. By default, lingering is disabled.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public LingerOption LingerState
		{
			get
			{
				if ((this.values & TcpClient.Properties.LingerState) != (TcpClient.Properties)0u)
				{
					return this.linger_state;
				}
				return (LingerOption)this.client.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger);
			}
			set
			{
				if (!this.client.Connected)
				{
					this.linger_state = value;
					this.values |= TcpClient.Properties.LingerState;
					return;
				}
				this.client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, value);
			}
		}

		/// <summary>Gets or sets a value that disables a delay when send or receive buffers are not full.</summary>
		/// <returns>true if the delay is disabled, otherwise false. The default value is false.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public bool NoDelay
		{
			get
			{
				if ((this.values & TcpClient.Properties.NoDelay) != (TcpClient.Properties)0u)
				{
					return this.no_delay;
				}
				return (bool)this.client.GetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug);
			}
			set
			{
				if (!this.client.Connected)
				{
					this.no_delay = value;
					this.values |= TcpClient.Properties.NoDelay;
					return;
				}
				this.client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug, (!value) ? 0 : 1);
			}
		}

		/// <summary>Gets or sets the size of the receive buffer.</summary>
		/// <returns>The size of the receive buffer, in bytes. The default value is 8192 bytes.</returns>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when setting the buffer size.-or-In .NET Compact Framework applications, you cannot set this property. For a workaround, see the Platform Note in Remarks.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int ReceiveBufferSize
		{
			get
			{
				if ((this.values & TcpClient.Properties.ReceiveBufferSize) != (TcpClient.Properties)0u)
				{
					return this.recv_buffer_size;
				}
				return (int)this.client.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer);
			}
			set
			{
				if (!this.client.Connected)
				{
					this.recv_buffer_size = value;
					this.values |= TcpClient.Properties.ReceiveBufferSize;
					return;
				}
				this.client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, value);
			}
		}

		/// <summary>Gets or sets the amount of time a <see cref="T:System.Net.Sockets.TcpClient" /> will wait to receive data once a read operation is initiated.</summary>
		/// <returns>The time-out value of the connection in milliseconds. The default value is 0.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int ReceiveTimeout
		{
			get
			{
				if ((this.values & TcpClient.Properties.ReceiveTimeout) != (TcpClient.Properties)0u)
				{
					return this.recv_timeout;
				}
				return (int)this.client.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout);
			}
			set
			{
				if (!this.client.Connected)
				{
					this.recv_timeout = value;
					this.values |= TcpClient.Properties.ReceiveTimeout;
					return;
				}
				this.client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, value);
			}
		}

		/// <summary>Gets or sets the size of the send buffer.</summary>
		/// <returns>The size of the send buffer, in bytes. The default value is 8192 bytes.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int SendBufferSize
		{
			get
			{
				if ((this.values & TcpClient.Properties.SendBufferSize) != (TcpClient.Properties)0u)
				{
					return this.send_buffer_size;
				}
				return (int)this.client.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer);
			}
			set
			{
				if (!this.client.Connected)
				{
					this.send_buffer_size = value;
					this.values |= TcpClient.Properties.SendBufferSize;
					return;
				}
				this.client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, value);
			}
		}

		/// <summary>Gets or sets the amount of time a <see cref="T:System.Net.Sockets.TcpClient" /> will wait for a send operation to complete successfully.</summary>
		/// <returns>The send time-out value, in milliseconds. The default is 0.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int SendTimeout
		{
			get
			{
				if ((this.values & TcpClient.Properties.SendTimeout) != (TcpClient.Properties)0u)
				{
					return this.send_timeout;
				}
				return (int)this.client.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout);
			}
			set
			{
				if (!this.client.Connected)
				{
					this.send_timeout = value;
					this.values |= TcpClient.Properties.SendTimeout;
					return;
				}
				this.client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, value);
			}
		}

		/// <summary>Disposes this <see cref="T:System.Net.Sockets.TcpClient" /> instance and requests that the underlying TCP connection be closed.</summary>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public void Close()
		{
			((IDisposable)this).Dispose();
		}

		/// <summary>Connects the client to a remote TCP host using the specified remote network endpoint.</summary>
		/// <param name="remoteEP">The <see cref="T:System.Net.IPEndPoint" /> to which you intend to connect. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="remoteEp" /> parameter is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when accessing the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.TcpClient" /> is closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public void Connect(IPEndPoint remote_end_point)
		{
			try
			{
				this.client.Connect(remote_end_point);
				this.active = true;
			}
			finally
			{
				this.CheckDisposed();
			}
		}

		/// <summary>Connects the client to a remote TCP host using the specified IP address and port number.</summary>
		/// <param name="address">The <see cref="T:System.Net.IPAddress" /> of the host to which you intend to connect. </param>
		/// <param name="port">The port number to which you intend to connect. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="port" /> is not between <see cref="F:System.Net.IPEndPoint.MinPort" /> and <see cref="F:System.Net.IPEndPoint.MaxPort" />. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when accessing the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		///   <see cref="T:System.Net.Sockets.TcpClient" /> is closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public void Connect(IPAddress address, int port)
		{
			this.Connect(new IPEndPoint(address, port));
		}

		private void SetOptions()
		{
			TcpClient.Properties properties = this.values;
			this.values = (TcpClient.Properties)0u;
			if ((properties & TcpClient.Properties.LingerState) != (TcpClient.Properties)0u)
			{
				this.LingerState = this.linger_state;
			}
			if ((properties & TcpClient.Properties.NoDelay) != (TcpClient.Properties)0u)
			{
				this.NoDelay = this.no_delay;
			}
			if ((properties & TcpClient.Properties.ReceiveBufferSize) != (TcpClient.Properties)0u)
			{
				this.ReceiveBufferSize = this.recv_buffer_size;
			}
			if ((properties & TcpClient.Properties.ReceiveTimeout) != (TcpClient.Properties)0u)
			{
				this.ReceiveTimeout = this.recv_timeout;
			}
			if ((properties & TcpClient.Properties.SendBufferSize) != (TcpClient.Properties)0u)
			{
				this.SendBufferSize = this.send_buffer_size;
			}
			if ((properties & TcpClient.Properties.SendTimeout) != (TcpClient.Properties)0u)
			{
				this.SendTimeout = this.send_timeout;
			}
		}

		/// <summary>Connects the client to the specified port on the specified host.</summary>
		/// <param name="hostname">The DNS name of the remote host to which you intend to connect. </param>
		/// <param name="port">The port number of the remote host to which you intend to connect. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="hostname" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="port" /> parameter is not between <see cref="F:System.Net.IPEndPoint.MinPort" /> and <see cref="F:System.Net.IPEndPoint.MaxPort" />. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when accessing the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		///   <see cref="T:System.Net.Sockets.TcpClient" /> is closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public void Connect(string hostname, int port)
		{
			IPAddress[] hostAddresses = Dns.GetHostAddresses(hostname);
			this.Connect(hostAddresses, port);
		}

		/// <summary>Connects the client to a remote TCP host using the specified IP addresses and port number.</summary>
		/// <param name="ipAddresses">The <see cref="T:System.Net.IPAddress" /> array of the host to which you intend to connect.</param>
		/// <param name="port">The port number to which you intend to connect.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="ipAddresses" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The port number is not valid.</exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.Security.SecurityException">A caller higher in the call stack does not have permission for the requested operation. </exception>
		/// <exception cref="T:System.NotSupportedException">This method is valid for sockets that use the <see cref="F:System.Net.Sockets.AddressFamily.InterNetwork" /> flag or the <see cref="F:System.Net.Sockets.AddressFamily.InterNetworkV6" /> flag.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public void Connect(IPAddress[] ipAddresses, int port)
		{
			this.CheckDisposed();
			if (ipAddresses == null)
			{
				throw new ArgumentNullException("ipAddresses");
			}
			for (int i = 0; i < ipAddresses.Length; i++)
			{
				try
				{
					IPAddress ipaddress = ipAddresses[i];
					if (ipaddress.Equals(IPAddress.Any) || ipaddress.Equals(IPAddress.IPv6Any))
					{
						throw new SocketException(10049);
					}
					this.Init(ipaddress.AddressFamily);
					if (ipaddress.AddressFamily == AddressFamily.InterNetwork)
					{
						this.client.Bind(new IPEndPoint(IPAddress.Any, 0));
					}
					else
					{
						if (ipaddress.AddressFamily != AddressFamily.InterNetworkV6)
						{
							throw new NotSupportedException("This method is only valid for sockets in the InterNetwork and InterNetworkV6 families");
						}
						this.client.Bind(new IPEndPoint(IPAddress.IPv6Any, 0));
					}
					this.Connect(new IPEndPoint(ipaddress, port));
					if (this.values != (TcpClient.Properties)0u)
					{
						this.SetOptions();
					}
					break;
				}
				catch (Exception ex)
				{
					this.Init(AddressFamily.InterNetwork);
					if (i == ipAddresses.Length - 1)
					{
						throw ex;
					}
				}
			}
		}

		/// <summary>Asynchronously accepts an incoming connection attempt.</summary>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> object returned by a call to <see cref="Overload:System.Net.Sockets.TcpClient.BeginConnect" />.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="asyncResult" /> parameter is null. </exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="asyncResult" /> parameter was not returned by a call to a <see cref="Overload:System.Net.Sockets.TcpClient.BeginConnect" /> method. </exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="M:System.Net.Sockets.TcpClient.EndConnect(System.IAsyncResult)" /> method was previously called for the asynchronous connection. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the <see cref="T:System.Net.Sockets.Socket" />. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The underlying <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public void EndConnect(IAsyncResult asyncResult)
		{
			this.client.EndConnect(asyncResult);
		}

		/// <summary>Begins an asynchronous request for a remote host connection. The remote host is specified by an <see cref="T:System.Net.IPAddress" /> and a port number (<see cref="T:System.Int32" />).</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> object that references the asynchronous connection.</returns>
		/// <param name="address">The <see cref="T:System.Net.IPAddress" /> of the remote host.</param>
		/// <param name="port">The port number of the remote host.</param>
		/// <param name="requestCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the operation is complete. </param>
		/// <param name="state">A user-defined object that contains information about the connect operation. This object is passed to the <paramref name="requestCallback" /> delegate when the operation is complete.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.Security.SecurityException">A caller higher in the call stack does not have permission for the requested operation. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The port number is not valid.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public IAsyncResult BeginConnect(IPAddress address, int port, AsyncCallback callback, object state)
		{
			return this.client.BeginConnect(address, port, callback, state);
		}

		/// <summary>Begins an asynchronous request for a remote host connection. The remote host is specified by an <see cref="T:System.Net.IPAddress" /> array and a port number (<see cref="T:System.Int32" />).</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> object that references the asynchronous connection.</returns>
		/// <param name="addresses">At least one <see cref="T:System.Net.IPAddress" /> that designates the remote hosts.</param>
		/// <param name="port">The port number of the remote hosts.</param>
		/// <param name="requestCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the operation is complete.</param>
		/// <param name="state">A user-defined object that contains information about the connect operation. This object is passed to the <paramref name="requestCallback" /> delegate when the operation is complete.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="addresses" /> parameter is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.Security.SecurityException">A caller higher in the call stack does not have permission for the requested operation. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The port number is not valid.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public IAsyncResult BeginConnect(IPAddress[] addresses, int port, AsyncCallback callback, object state)
		{
			return this.client.BeginConnect(addresses, port, callback, state);
		}

		/// <summary>Begins an asynchronous request for a remote host connection. The remote host is specified by a host name (<see cref="T:System.String" />) and a port number (<see cref="T:System.Int32" />).</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> object that references the asynchronous connection.</returns>
		/// <param name="host">The name of the remote host.</param>
		/// <param name="port">The port number of the remote host.</param>
		/// <param name="requestCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the operation is complete.</param>
		/// <param name="state">A user-defined object that contains information about the connect operation. This object is passed to the <paramref name="requestCallback" /> delegate when the operation is complete.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="host" /> parameter is null. </exception>
		/// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when attempting to access the socket. See the Remarks section for more information. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.Socket" /> has been closed. </exception>
		/// <exception cref="T:System.Security.SecurityException">A caller higher in the call stack does not have permission for the requested operation. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The port number is not valid.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Net.SocketPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public IAsyncResult BeginConnect(string host, int port, AsyncCallback callback, object state)
		{
			return this.client.BeginConnect(host, port, callback, state);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Net.Sockets.TcpClient" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">Set to true to release both managed and unmanaged resources; false to release only unmanaged resources. </param>
		protected virtual void Dispose(bool disposing)
		{
			if (this.disposed)
			{
				return;
			}
			this.disposed = true;
			if (disposing)
			{
				NetworkStream networkStream = this.stream;
				this.stream = null;
				if (networkStream != null)
				{
					networkStream.Close();
					this.active = false;
				}
				else if (this.client != null)
				{
					this.client.Close();
					this.client = null;
				}
			}
		}

		/// <summary>Frees resources used by the <see cref="T:System.Net.Sockets.TcpClient" /> class.</summary>
		~TcpClient()
		{
			this.Dispose(false);
		}

		/// <summary>Returns the <see cref="T:System.Net.Sockets.NetworkStream" /> used to send and receive data.</summary>
		/// <returns>The underlying <see cref="T:System.Net.Sockets.NetworkStream" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Net.Sockets.TcpClient" /> is not connected to a remote host. </exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.Net.Sockets.TcpClient" /> has been closed. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public NetworkStream GetStream()
		{
			NetworkStream result;
			try
			{
				if (this.stream == null)
				{
					this.stream = new NetworkStream(this.client, true);
				}
				result = this.stream;
			}
			finally
			{
				this.CheckDisposed();
			}
			return result;
		}

		private void CheckDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
		}

		private enum Properties : uint
		{
			LingerState = 1u,
			NoDelay,
			ReceiveBufferSize = 4u,
			ReceiveTimeout = 8u,
			SendBufferSize = 16u,
			SendTimeout = 32u
		}
	}
}
