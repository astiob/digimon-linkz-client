using System;
using System.Collections;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace System.Net
{
	/// <summary>Provides connection management for HTTP connections.</summary>
	public class ServicePoint
	{
		private System.Uri uri;

		private int connectionLimit;

		private int maxIdleTime;

		private int currentConnections;

		private DateTime idleSince;

		private Version protocolVersion;

		private X509Certificate certificate;

		private X509Certificate clientCertificate;

		private IPHostEntry host;

		private bool usesProxy;

		private Hashtable groups;

		private bool sendContinue = true;

		private bool useConnect;

		private object locker = new object();

		private object hostE = new object();

		private bool useNagle;

		private BindIPEndPoint endPointCallback;

		internal ServicePoint(System.Uri uri, int connectionLimit, int maxIdleTime)
		{
			this.uri = uri;
			this.connectionLimit = connectionLimit;
			this.maxIdleTime = maxIdleTime;
			this.currentConnections = 0;
			this.idleSince = DateTime.Now;
		}

		/// <summary>Gets the Uniform Resource Identifier (URI) of the server that this <see cref="T:System.Net.ServicePoint" /> object connects to.</summary>
		/// <returns>An instance of the <see cref="T:System.Uri" /> class that contains the URI of the Internet server that this <see cref="T:System.Net.ServicePoint" /> object connects to.</returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Net.ServicePoint" /> is in host mode.</exception>
		public System.Uri Address
		{
			get
			{
				return this.uri;
			}
		}

		private static Exception GetMustImplement()
		{
			return new NotImplementedException();
		}

		/// <summary>Specifies the delegate to associate a local <see cref="T:System.Net.IPEndPoint" /> with a <see cref="T:System.Net.ServicePoint" />.</summary>
		/// <returns>A delegate that forces a <see cref="T:System.Net.ServicePoint" /> to use a particular local Internet Protocol (IP) address and port number. The default value is null.</returns>
		public BindIPEndPoint BindIPEndPointDelegate
		{
			get
			{
				return this.endPointCallback;
			}
			set
			{
				this.endPointCallback = value;
			}
		}

		/// <summary>Gets the certificate received for this <see cref="T:System.Net.ServicePoint" /> object.</summary>
		/// <returns>An instance of the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> class that contains the security certificate received for this <see cref="T:System.Net.ServicePoint" /> object.</returns>
		public X509Certificate Certificate
		{
			get
			{
				return this.certificate;
			}
		}

		/// <summary>Gets the last client certificate sent to the server.</summary>
		/// <returns>An <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> object that contains the public values of the last client certificate sent to the server.</returns>
		public X509Certificate ClientCertificate
		{
			get
			{
				return this.clientCertificate;
			}
		}

		/// <summary>Gets or sets the number of milliseconds after which an active <see cref="T:System.Net.ServicePoint" /> connection is closed.</summary>
		/// <returns>A <see cref="T:System.Int32" /> that specifies the number of milliseconds that an active <see cref="T:System.Net.ServicePoint" /> connection remains open. The default is -1, which allows an active <see cref="T:System.Net.ServicePoint" /> connection to stay connected indefinitely. Set this property to 0 to force <see cref="T:System.Net.ServicePoint" /> connections to close after servicing a request.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified for a set operation is a negative number less than -1.</exception>
		[MonoTODO]
		public int ConnectionLeaseTimeout
		{
			get
			{
				throw ServicePoint.GetMustImplement();
			}
			set
			{
				throw ServicePoint.GetMustImplement();
			}
		}

		/// <summary>Gets or sets the maximum number of connections allowed on this <see cref="T:System.Net.ServicePoint" /> object.</summary>
		/// <returns>The maximum number of connections allowed on this <see cref="T:System.Net.ServicePoint" /> object.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The connection limit is equal to or less than 0. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		///   <IPermission class="System.Net.DnsPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public int ConnectionLimit
		{
			get
			{
				return this.connectionLimit;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException();
				}
				this.connectionLimit = value;
			}
		}

		/// <summary>Gets the connection name. </summary>
		/// <returns>A <see cref="T:System.String" /> that represents the connection name. </returns>
		public string ConnectionName
		{
			get
			{
				return this.uri.Scheme;
			}
		}

		/// <summary>Gets the number of open connections associated with this <see cref="T:System.Net.ServicePoint" /> object.</summary>
		/// <returns>The number of open connections associated with this <see cref="T:System.Net.ServicePoint" /> object.</returns>
		public int CurrentConnections
		{
			get
			{
				return this.currentConnections;
			}
		}

		/// <summary>Gets the date and time that the <see cref="T:System.Net.ServicePoint" /> object was last connected to a host.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> object that contains the date and time at which the <see cref="T:System.Net.ServicePoint" /> object was last connected.</returns>
		public DateTime IdleSince
		{
			get
			{
				return this.idleSince;
			}
			internal set
			{
				object obj = this.locker;
				lock (obj)
				{
					this.idleSince = value;
				}
			}
		}

		/// <summary>Gets or sets the amount of time a connection associated with the <see cref="T:System.Net.ServicePoint" /> object can remain idle before the connection is closed.</summary>
		/// <returns>The length of time, in milliseconds, that a connection associated with the <see cref="T:System.Net.ServicePoint" /> object can remain idle before it is closed and reused for another connection.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <see cref="P:System.Net.ServicePoint.MaxIdleTime" /> is set to less than <see cref="F:System.Threading.Timeout.Infinite" /> or greater than <see cref="F:System.Int32.MaxValue" />. </exception>
		public int MaxIdleTime
		{
			get
			{
				return this.maxIdleTime;
			}
			set
			{
				if (value < -1 || value > 2147483647)
				{
					throw new ArgumentOutOfRangeException();
				}
				this.maxIdleTime = value;
			}
		}

		/// <summary>Gets the version of the HTTP protocol that the <see cref="T:System.Net.ServicePoint" /> object uses.</summary>
		/// <returns>A <see cref="T:System.Version" /> object that contains the HTTP protocol version that the <see cref="T:System.Net.ServicePoint" /> object uses.</returns>
		public virtual Version ProtocolVersion
		{
			get
			{
				return this.protocolVersion;
			}
		}

		/// <summary>Gets or sets the size of the receiving buffer for the socket used by this <see cref="T:System.Net.ServicePoint" />.</summary>
		/// <returns>A <see cref="T:System.Int32" /> that contains the size, in bytes, of the receive buffer. The default is 8192.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified for a set operation is greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		[MonoTODO]
		public int ReceiveBufferSize
		{
			get
			{
				throw ServicePoint.GetMustImplement();
			}
			set
			{
				throw ServicePoint.GetMustImplement();
			}
		}

		/// <summary>Indicates whether the <see cref="T:System.Net.ServicePoint" /> object supports pipelined connections.</summary>
		/// <returns>true if the <see cref="T:System.Net.ServicePoint" /> object supports pipelined connections; otherwise, false.</returns>
		public bool SupportsPipelining
		{
			get
			{
				return HttpVersion.Version11.Equals(this.protocolVersion);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that determines whether 100-Continue behavior is used.</summary>
		/// <returns>true to expect 100-Continue responses for POST requests; otherwise, false. The default value is true.</returns>
		public bool Expect100Continue
		{
			get
			{
				return this.SendContinue;
			}
			set
			{
				this.SendContinue = value;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that determines whether the Nagle algorithm is used on connections managed by this <see cref="T:System.Net.ServicePoint" /> object.</summary>
		/// <returns>true to use the Nagle algorithm; otherwise, false. The default value is true.</returns>
		public bool UseNagleAlgorithm
		{
			get
			{
				return this.useNagle;
			}
			set
			{
				this.useNagle = value;
			}
		}

		internal bool SendContinue
		{
			get
			{
				return this.sendContinue && (this.protocolVersion == null || this.protocolVersion == HttpVersion.Version11);
			}
			set
			{
				this.sendContinue = value;
			}
		}

		internal bool UsesProxy
		{
			get
			{
				return this.usesProxy;
			}
			set
			{
				this.usesProxy = value;
			}
		}

		internal bool UseConnect
		{
			get
			{
				return this.useConnect;
			}
			set
			{
				this.useConnect = value;
			}
		}

		internal bool AvailableForRecycling
		{
			get
			{
				return this.CurrentConnections == 0 && this.maxIdleTime != -1 && DateTime.Now >= this.IdleSince.AddMilliseconds((double)this.maxIdleTime);
			}
		}

		internal Hashtable Groups
		{
			get
			{
				if (this.groups == null)
				{
					this.groups = new Hashtable();
				}
				return this.groups;
			}
		}

		internal IPHostEntry HostEntry
		{
			get
			{
				object obj = this.hostE;
				lock (obj)
				{
					if (this.host != null)
					{
						return this.host;
					}
					string text = this.uri.Host;
					if (this.uri.HostNameType == System.UriHostNameType.IPv6 || this.uri.HostNameType == System.UriHostNameType.IPv4)
					{
						if (this.uri.HostNameType == System.UriHostNameType.IPv6)
						{
							text = text.Substring(1, text.Length - 2);
						}
						this.host = new IPHostEntry();
						this.host.AddressList = new IPAddress[]
						{
							IPAddress.Parse(text)
						};
						return this.host;
					}
					try
					{
						this.host = Dns.GetHostByName(text);
					}
					catch
					{
						return null;
					}
				}
				return this.host;
			}
		}

		internal void SetVersion(Version version)
		{
			this.protocolVersion = version;
		}

		private WebConnectionGroup GetConnectionGroup(string name)
		{
			if (name == null)
			{
				name = string.Empty;
			}
			WebConnectionGroup webConnectionGroup = this.Groups[name] as WebConnectionGroup;
			if (webConnectionGroup != null)
			{
				return webConnectionGroup;
			}
			webConnectionGroup = new WebConnectionGroup(this, name);
			this.Groups[name] = webConnectionGroup;
			return webConnectionGroup;
		}

		internal EventHandler SendRequest(HttpWebRequest request, string groupName)
		{
			object obj = this.locker;
			WebConnection connection;
			lock (obj)
			{
				WebConnectionGroup connectionGroup = this.GetConnectionGroup(groupName);
				connection = connectionGroup.GetConnection(request);
			}
			return connection.SendRequest(request);
		}

		/// <summary>Removes the specified connection group from this <see cref="T:System.Net.ServicePoint" /> object.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> value that indicates whether the connection group was closed.</returns>
		/// <param name="connectionGroupName">The name of the connection group that contains the connections to close and remove from this service point. </param>
		public bool CloseConnectionGroup(string connectionGroupName)
		{
			object obj = this.locker;
			lock (obj)
			{
				WebConnectionGroup connectionGroup = this.GetConnectionGroup(connectionGroupName);
				if (connectionGroup != null)
				{
					connectionGroup.Close();
					return true;
				}
			}
			return false;
		}

		internal void IncrementConnection()
		{
			object obj = this.locker;
			lock (obj)
			{
				this.currentConnections++;
				this.idleSince = DateTime.Now.AddMilliseconds(1000000.0);
			}
		}

		internal void DecrementConnection()
		{
			object obj = this.locker;
			lock (obj)
			{
				this.currentConnections--;
				if (this.currentConnections == 0)
				{
					this.idleSince = DateTime.Now;
				}
			}
		}

		internal void SetCertificates(X509Certificate client, X509Certificate server)
		{
			this.certificate = server;
			this.clientCertificate = client;
		}

		internal bool CallEndPointDelegate(System.Net.Sockets.Socket sock, IPEndPoint remote)
		{
			if (this.endPointCallback == null)
			{
				return true;
			}
			int num = 0;
			checked
			{
				for (;;)
				{
					IPEndPoint ipendPoint = null;
					try
					{
						ipendPoint = this.endPointCallback(this, remote, num);
					}
					catch
					{
						return false;
					}
					if (ipendPoint == null)
					{
						break;
					}
					try
					{
						sock.Bind(ipendPoint);
					}
					catch (System.Net.Sockets.SocketException)
					{
						num++;
						continue;
					}
					return true;
				}
				return true;
			}
		}
	}
}
