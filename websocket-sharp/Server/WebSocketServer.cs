using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Threading;
using WebSocketSharp.Net;
using WebSocketSharp.Net.WebSockets;

namespace WebSocketSharp.Server
{
	public class WebSocketServer
	{
		private IPAddress _address;

		private WebSocketSharp.Net.AuthenticationSchemes _authSchemes;

		private X509Certificate2 _cert;

		private Func<IIdentity, WebSocketSharp.Net.NetworkCredential> _credentialsFinder;

		private TcpListener _listener;

		private Logger _logger;

		private int _port;

		private string _realm;

		private Thread _receiveRequestThread;

		private bool _secure;

		private WebSocketServiceManager _services;

		private volatile ServerState _state;

		private object _sync;

		private Uri _uri;

		public WebSocketServer() : this(80)
		{
		}

		public WebSocketServer(int port) : this(IPAddress.Any, port)
		{
		}

		public WebSocketServer(string url)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			string message;
			if (!WebSocketServer.tryCreateUri(url, out this._uri, out message))
			{
				throw new ArgumentException(message, "url");
			}
			string dnsSafeHost = this._uri.DnsSafeHost;
			this._address = dnsSafeHost.ToIPAddress();
			if (this._address == null || !this._address.IsLocal())
			{
				throw new ArgumentException("The host part must be the local host name: " + dnsSafeHost, "url");
			}
			this._port = this._uri.Port;
			this._secure = (this._uri.Scheme == "wss");
			this.init();
		}

		public WebSocketServer(int port, bool secure) : this(IPAddress.Any, port, secure)
		{
		}

		public WebSocketServer(IPAddress address, int port) : this(address, port, port == 443)
		{
		}

		public WebSocketServer(IPAddress address, int port, bool secure)
		{
			if (!address.IsLocal())
			{
				throw new ArgumentException("Must be the local IP address: " + address, "address");
			}
			if (!port.IsPortNumber())
			{
				throw new ArgumentOutOfRangeException("port", "Must be between 1 and 65535: " + port);
			}
			if ((port == 80 && secure) || (port == 443 && !secure))
			{
				throw new ArgumentException(string.Format("Invalid pair of 'port' and 'secure': {0}, {1}", port, secure));
			}
			this._address = address;
			this._port = port;
			this._secure = secure;
			this._uri = "/".ToUri();
			this.init();
		}

		public IPAddress Address
		{
			get
			{
				return this._address;
			}
		}

		public WebSocketSharp.Net.AuthenticationSchemes AuthenticationSchemes
		{
			get
			{
				return this._authSchemes;
			}
			set
			{
				if (!this.canSet("AuthenticationSchemes"))
				{
					return;
				}
				this._authSchemes = value;
			}
		}

		public X509Certificate2 Certificate
		{
			get
			{
				return this._cert;
			}
			set
			{
				if (!this.canSet("Certificate"))
				{
					return;
				}
				this._cert = value;
			}
		}

		public bool IsListening
		{
			get
			{
				return this._state == ServerState.Start;
			}
		}

		public bool IsSecure
		{
			get
			{
				return this._secure;
			}
		}

		public bool KeepClean
		{
			get
			{
				return this._services.KeepClean;
			}
			set
			{
				this._services.KeepClean = value;
			}
		}

		public Logger Log
		{
			get
			{
				return this._logger;
			}
		}

		public int Port
		{
			get
			{
				return this._port;
			}
		}

		public string Realm
		{
			get
			{
				string result;
				if ((result = this._realm) == null)
				{
					result = (this._realm = "SECRET AREA");
				}
				return result;
			}
			set
			{
				if (!this.canSet("Realm"))
				{
					return;
				}
				this._realm = value;
			}
		}

		public Func<IIdentity, WebSocketSharp.Net.NetworkCredential> UserCredentialsFinder
		{
			get
			{
				Func<IIdentity, WebSocketSharp.Net.NetworkCredential> result;
				if ((result = this._credentialsFinder) == null)
				{
					result = (this._credentialsFinder = ((IIdentity identity) => null));
				}
				return result;
			}
			set
			{
				if (!this.canSet("UserCredentialsFinder"))
				{
					return;
				}
				this._credentialsFinder = value;
			}
		}

		public WebSocketServiceManager WebSocketServices
		{
			get
			{
				return this._services;
			}
		}

		private void abort()
		{
			object sync = this._sync;
			lock (sync)
			{
				if (!this.IsListening)
				{
					return;
				}
				this._state = ServerState.ShuttingDown;
			}
			this._listener.Stop();
			this._services.Stop(1011.ToByteArrayInternally(ByteOrder.Big), true);
			this._state = ServerState.Stop;
		}

		private void acceptRequestAsync(TcpClient client)
		{
			ThreadPool.QueueUserWorkItem(delegate(object state)
			{
				try
				{
					TcpListenerWebSocketContext webSocketContext = client.GetWebSocketContext(this._cert, this._secure, this._logger);
					if (this._authSchemes == WebSocketSharp.Net.AuthenticationSchemes.Anonymous || this.authenticateRequest(this._authSchemes, webSocketContext))
					{
						this.acceptWebSocket(webSocketContext);
					}
				}
				catch (Exception ex)
				{
					this._logger.Fatal(ex.ToString());
					client.Close();
				}
			});
		}

		private void acceptWebSocket(TcpListenerWebSocketContext context)
		{
			string path = context.Path;
			WebSocketServiceHost webSocketServiceHost;
			if (path == null || !this._services.TryGetServiceHostInternally(path, out webSocketServiceHost))
			{
				context.Close(WebSocketSharp.Net.HttpStatusCode.NotImplemented);
				return;
			}
			if (this._uri.IsAbsoluteUri)
			{
				context.WebSocket.Url = new Uri(this._uri, path);
			}
			webSocketServiceHost.StartSession(context);
		}

		private bool authenticateRequest(WebSocketSharp.Net.AuthenticationSchemes scheme, TcpListenerWebSocketContext context)
		{
			string challenge = (scheme != WebSocketSharp.Net.AuthenticationSchemes.Basic) ? ((scheme != WebSocketSharp.Net.AuthenticationSchemes.Digest) ? null : HttpUtility.CreateDigestAuthChallenge(this.Realm)) : HttpUtility.CreateBasicAuthChallenge(this.Realm);
			if (challenge == null)
			{
				context.Close(WebSocketSharp.Net.HttpStatusCode.Forbidden);
				return false;
			}
			int retry = -1;
			string expected = scheme.ToString();
			string realm = this.Realm;
			Func<IIdentity, WebSocketSharp.Net.NetworkCredential> credentialsFinder = this.UserCredentialsFinder;
			Func<bool> auth = null;
			auth = delegate()
			{
				retry++;
				if (retry > 99)
				{
					context.Close(WebSocketSharp.Net.HttpStatusCode.Forbidden);
					return false;
				}
				string text = context.Headers["Authorization"];
				if (text == null || !text.StartsWith(expected, StringComparison.OrdinalIgnoreCase))
				{
					context.SendAuthChallenge(challenge);
					return auth();
				}
				context.SetUser(scheme, realm, credentialsFinder);
				if (context.IsAuthenticated)
				{
					return true;
				}
				context.SendAuthChallenge(challenge);
				return auth();
			};
			return auth();
		}

		private bool canSet(string property)
		{
			if (this._state == ServerState.Start || this._state == ServerState.ShuttingDown)
			{
				this._logger.Error(string.Format("Set operation of {0} isn't available because the server has already started.", property));
				return false;
			}
			return true;
		}

		private string checkIfCertExists()
		{
			return (!this._secure || this._cert != null) ? null : "The secure connection requires a server certificate.";
		}

		private void init()
		{
			this._authSchemes = WebSocketSharp.Net.AuthenticationSchemes.Anonymous;
			this._listener = new TcpListener(this._address, this._port);
			this._logger = new Logger();
			this._services = new WebSocketServiceManager(this._logger);
			this._state = ServerState.Ready;
			this._sync = new object();
		}

		private void receiveRequest()
		{
			for (;;)
			{
				try
				{
					this.acceptRequestAsync(this._listener.AcceptTcpClient());
				}
				catch (SocketException ex)
				{
					this._logger.Warn("Receiving has been stopped.\nreason: " + ex.Message);
					break;
				}
				catch (Exception ex2)
				{
					this._logger.Fatal(ex2.ToString());
					break;
				}
			}
			if (this.IsListening)
			{
				this.abort();
			}
		}

		private void startReceiving()
		{
			this._receiveRequestThread = new Thread(new ThreadStart(this.receiveRequest));
			this._receiveRequestThread.IsBackground = true;
			this._receiveRequestThread.Start();
		}

		private void stopListener(int millisecondsTimeout)
		{
			this._listener.Stop();
			this._receiveRequestThread.Join(millisecondsTimeout);
		}

		private static bool tryCreateUri(string uriString, out Uri result, out string message)
		{
			if (!uriString.TryCreateWebSocketUri(out result, out message))
			{
				return false;
			}
			if (result.PathAndQuery != "/")
			{
				result = null;
				message = "Must not contain the path or query component: " + uriString;
				return false;
			}
			return true;
		}

		public void AddWebSocketService<TWithNew>(string path) where TWithNew : WebSocketService, new()
		{
			this.AddWebSocketService<TWithNew>(path, () => Activator.CreateInstance<TWithNew>());
		}

		public void AddWebSocketService<T>(string path, Func<T> constructor) where T : WebSocketService
		{
			string text = path.CheckIfValidServicePath() ?? ((constructor != null) ? null : "'constructor' must not be null.");
			if (text != null)
			{
				this._logger.Error(string.Format("{0}\nservice path: {1}", text, path));
				return;
			}
			WebSocketServiceHost<T> webSocketServiceHost = new WebSocketServiceHost<T>(path, constructor, this._logger);
			if (!this.KeepClean)
			{
				webSocketServiceHost.KeepClean = false;
			}
			this._services.Add(webSocketServiceHost.Path, webSocketServiceHost);
		}

		public bool RemoveWebSocketService(string path)
		{
			string text = path.CheckIfValidServicePath();
			if (text != null)
			{
				this._logger.Error(string.Format("{0}\nservice path: {1}", text, path));
				return false;
			}
			return this._services.Remove(path);
		}

		public void Start()
		{
			object sync = this._sync;
			lock (sync)
			{
				string text = this._state.CheckIfStartable() ?? this.checkIfCertExists();
				if (text != null)
				{
					this._logger.Error(string.Format("{0}\nstate: {1}\nsecure: {2}", text, this._state, this._secure));
				}
				else
				{
					this._services.Start();
					this._listener.Start();
					this.startReceiving();
					this._state = ServerState.Start;
				}
			}
		}

		public void Stop()
		{
			object sync = this._sync;
			lock (sync)
			{
				string text = this._state.CheckIfStart();
				if (text != null)
				{
					this._logger.Error(string.Format("{0}\nstate: {1}", text, this._state));
					return;
				}
				this._state = ServerState.ShuttingDown;
			}
			this.stopListener(5000);
			this._services.Stop(new byte[0], true);
			this._state = ServerState.Stop;
		}

		public void Stop(ushort code, string reason)
		{
			byte[] data = null;
			object sync = this._sync;
			lock (sync)
			{
				string text;
				if ((text = this._state.CheckIfStart()) == null && (text = code.CheckIfValidCloseStatusCode()) == null)
				{
					text = (data = code.Append(reason)).CheckIfValidControlData("reason");
				}
				string text2 = text;
				if (text2 != null)
				{
					this._logger.Error(string.Format("{0}\nstate: {1}\ncode: {2}\nreason: {3}", new object[]
					{
						text2,
						this._state,
						code,
						reason
					}));
					return;
				}
				this._state = ServerState.ShuttingDown;
			}
			this.stopListener(5000);
			this._services.Stop(data, !code.IsReserved());
			this._state = ServerState.Stop;
		}

		public void Stop(CloseStatusCode code, string reason)
		{
			byte[] data = null;
			object sync = this._sync;
			lock (sync)
			{
				string text;
				if ((text = this._state.CheckIfStart()) == null)
				{
					text = (data = ((ushort)code).Append(reason)).CheckIfValidControlData("reason");
				}
				string text2 = text;
				if (text2 != null)
				{
					this._logger.Error(string.Format("{0}\nstate: {1}\nreason: {2}", text2, this._state, reason));
					return;
				}
				this._state = ServerState.ShuttingDown;
			}
			this.stopListener(5000);
			this._services.Stop(data, !code.IsReserved());
			this._state = ServerState.Stop;
		}
	}
}
