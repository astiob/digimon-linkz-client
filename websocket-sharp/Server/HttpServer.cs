using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Threading;
using WebSocketSharp.Net;
using WebSocketSharp.Net.WebSockets;

namespace WebSocketSharp.Server
{
	public class HttpServer
	{
		private HttpListener _listener;

		private Logger _logger;

		private int _port;

		private Thread _receiveRequestThread;

		private string _rootPath;

		private bool _secure;

		private WebSocketServiceManager _services;

		private volatile ServerState _state;

		private object _sync;

		private bool _windows;

		public HttpServer() : this(80)
		{
		}

		public HttpServer(int port) : this(port, port == 443)
		{
		}

		public HttpServer(int port, bool secure)
		{
			if (!port.IsPortNumber())
			{
				throw new ArgumentOutOfRangeException("port", "Must be between 1 and 65535: " + port);
			}
			if ((port == 80 && secure) || (port == 443 && !secure))
			{
				throw new ArgumentException(string.Format("Invalid pair of 'port' and 'secure': {0}, {1}", port, secure));
			}
			this._port = port;
			this._secure = secure;
			this._listener = new HttpListener();
			this._logger = new Logger();
			this._services = new WebSocketServiceManager(this._logger);
			this._state = ServerState.Ready;
			this._sync = new object();
			OperatingSystem osversion = Environment.OSVersion;
			if (osversion.Platform != PlatformID.Unix && osversion.Platform != PlatformID.MacOSX)
			{
				this._windows = true;
			}
			string uriPrefix = string.Format("http{0}://*:{1}/", (!this._secure) ? "" : "s", this._port);
			this._listener.Prefixes.Add(uriPrefix);
		}

		public event EventHandler<HttpRequestEventArgs> OnConnect
		{
			add
			{
				EventHandler<HttpRequestEventArgs> eventHandler = this.OnConnect;
				EventHandler<HttpRequestEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnConnect, (EventHandler<HttpRequestEventArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<HttpRequestEventArgs> eventHandler = this.OnConnect;
				EventHandler<HttpRequestEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnConnect, (EventHandler<HttpRequestEventArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		public event EventHandler<HttpRequestEventArgs> OnDelete
		{
			add
			{
				EventHandler<HttpRequestEventArgs> eventHandler = this.OnDelete;
				EventHandler<HttpRequestEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnDelete, (EventHandler<HttpRequestEventArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<HttpRequestEventArgs> eventHandler = this.OnDelete;
				EventHandler<HttpRequestEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnDelete, (EventHandler<HttpRequestEventArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		public event EventHandler<HttpRequestEventArgs> OnGet
		{
			add
			{
				EventHandler<HttpRequestEventArgs> eventHandler = this.OnGet;
				EventHandler<HttpRequestEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnGet, (EventHandler<HttpRequestEventArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<HttpRequestEventArgs> eventHandler = this.OnGet;
				EventHandler<HttpRequestEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnGet, (EventHandler<HttpRequestEventArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		public event EventHandler<HttpRequestEventArgs> OnHead
		{
			add
			{
				EventHandler<HttpRequestEventArgs> eventHandler = this.OnHead;
				EventHandler<HttpRequestEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnHead, (EventHandler<HttpRequestEventArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<HttpRequestEventArgs> eventHandler = this.OnHead;
				EventHandler<HttpRequestEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnHead, (EventHandler<HttpRequestEventArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		public event EventHandler<HttpRequestEventArgs> OnOptions
		{
			add
			{
				EventHandler<HttpRequestEventArgs> eventHandler = this.OnOptions;
				EventHandler<HttpRequestEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnOptions, (EventHandler<HttpRequestEventArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<HttpRequestEventArgs> eventHandler = this.OnOptions;
				EventHandler<HttpRequestEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnOptions, (EventHandler<HttpRequestEventArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		public event EventHandler<HttpRequestEventArgs> OnPatch
		{
			add
			{
				EventHandler<HttpRequestEventArgs> eventHandler = this.OnPatch;
				EventHandler<HttpRequestEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnPatch, (EventHandler<HttpRequestEventArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<HttpRequestEventArgs> eventHandler = this.OnPatch;
				EventHandler<HttpRequestEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnPatch, (EventHandler<HttpRequestEventArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		public event EventHandler<HttpRequestEventArgs> OnPost
		{
			add
			{
				EventHandler<HttpRequestEventArgs> eventHandler = this.OnPost;
				EventHandler<HttpRequestEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnPost, (EventHandler<HttpRequestEventArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<HttpRequestEventArgs> eventHandler = this.OnPost;
				EventHandler<HttpRequestEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnPost, (EventHandler<HttpRequestEventArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		public event EventHandler<HttpRequestEventArgs> OnPut
		{
			add
			{
				EventHandler<HttpRequestEventArgs> eventHandler = this.OnPut;
				EventHandler<HttpRequestEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnPut, (EventHandler<HttpRequestEventArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<HttpRequestEventArgs> eventHandler = this.OnPut;
				EventHandler<HttpRequestEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnPut, (EventHandler<HttpRequestEventArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		public event EventHandler<HttpRequestEventArgs> OnTrace
		{
			add
			{
				EventHandler<HttpRequestEventArgs> eventHandler = this.OnTrace;
				EventHandler<HttpRequestEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnTrace, (EventHandler<HttpRequestEventArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<HttpRequestEventArgs> eventHandler = this.OnTrace;
				EventHandler<HttpRequestEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<HttpRequestEventArgs>>(ref this.OnTrace, (EventHandler<HttpRequestEventArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		public AuthenticationSchemes AuthenticationSchemes
		{
			get
			{
				return this._listener.AuthenticationSchemes;
			}
			set
			{
				if (!this.canSet("AuthenticationSchemes"))
				{
					return;
				}
				this._listener.AuthenticationSchemes = value;
			}
		}

		public X509Certificate2 Certificate
		{
			get
			{
				return this._listener.DefaultCertificate;
			}
			set
			{
				if (!this.canSet("Certificate"))
				{
					return;
				}
				if (EndPointListener.CertificateExists(this._port, this._listener.CertificateFolderPath))
				{
					this._logger.Warn("The server certificate associated with the port number already exists.");
				}
				this._listener.DefaultCertificate = value;
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
				return this._listener.Realm;
			}
			set
			{
				if (!this.canSet("Realm"))
				{
					return;
				}
				this._listener.Realm = value;
			}
		}

		public string RootPath
		{
			get
			{
				return (!this._rootPath.IsNullOrEmpty()) ? this._rootPath : (this._rootPath = "./Public");
			}
			set
			{
				if (!this.canSet("RootPath"))
				{
					return;
				}
				this._rootPath = value;
			}
		}

		public Func<IIdentity, NetworkCredential> UserCredentialsFinder
		{
			get
			{
				return this._listener.UserCredentialsFinder;
			}
			set
			{
				if (!this.canSet("UserCredentialsFinder"))
				{
					return;
				}
				this._listener.UserCredentialsFinder = value;
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
			this._services.Stop(1011.ToByteArrayInternally(ByteOrder.Big), true);
			this._listener.Abort();
			this._state = ServerState.Stop;
		}

		private void acceptHttpRequest(HttpListenerContext context)
		{
			HttpRequestEventArgs e = new HttpRequestEventArgs(context);
			string httpMethod = context.Request.HttpMethod;
			if (httpMethod == "GET")
			{
				if (this.OnGet != null)
				{
					this.OnGet(this, e);
					return;
				}
			}
			else if (httpMethod == "HEAD")
			{
				if (this.OnHead != null)
				{
					this.OnHead(this, e);
					return;
				}
			}
			else if (httpMethod == "POST")
			{
				if (this.OnPost != null)
				{
					this.OnPost(this, e);
					return;
				}
			}
			else if (httpMethod == "PUT")
			{
				if (this.OnPut != null)
				{
					this.OnPut(this, e);
					return;
				}
			}
			else if (httpMethod == "DELETE")
			{
				if (this.OnDelete != null)
				{
					this.OnDelete(this, e);
					return;
				}
			}
			else if (httpMethod == "OPTIONS")
			{
				if (this.OnOptions != null)
				{
					this.OnOptions(this, e);
					return;
				}
			}
			else if (httpMethod == "TRACE")
			{
				if (this.OnTrace != null)
				{
					this.OnTrace(this, e);
					return;
				}
			}
			else if (httpMethod == "CONNECT")
			{
				if (this.OnConnect != null)
				{
					this.OnConnect(this, e);
					return;
				}
			}
			else if (httpMethod == "PATCH" && this.OnPatch != null)
			{
				this.OnPatch(this, e);
				return;
			}
			context.Response.StatusCode = 501;
		}

		private void acceptRequestAsync(HttpListenerContext context)
		{
			ThreadPool.QueueUserWorkItem(delegate(object state)
			{
				try
				{
					AuthenticationSchemes authenticationSchemes = this._listener.SelectAuthenticationScheme(context);
					if (authenticationSchemes == AuthenticationSchemes.Anonymous || this.authenticateRequest(authenticationSchemes, context))
					{
						if (context.Request.IsUpgradeTo("websocket"))
						{
							this.acceptWebSocketRequest(context.AcceptWebSocket(this._logger));
						}
						else
						{
							this.acceptHttpRequest(context);
							context.Response.Close();
						}
					}
				}
				catch (Exception ex)
				{
					this._logger.Fatal(ex.ToString());
					context.Connection.Close(true);
				}
			});
		}

		private void acceptWebSocketRequest(HttpListenerWebSocketContext context)
		{
			string path = context.Path;
			WebSocketServiceHost webSocketServiceHost;
			if (path == null || !this._services.TryGetServiceHostInternally(path, out webSocketServiceHost))
			{
				context.Close(HttpStatusCode.NotImplemented);
				return;
			}
			webSocketServiceHost.StartSession(context);
		}

		private bool authenticateRequest(AuthenticationSchemes scheme, HttpListenerContext context)
		{
			if (context.Request.IsAuthenticated)
			{
				return true;
			}
			if (scheme == AuthenticationSchemes.Basic)
			{
				context.Response.CloseWithAuthChallenge(HttpUtility.CreateBasicAuthChallenge(this._listener.Realm));
			}
			else if (scheme == AuthenticationSchemes.Digest)
			{
				context.Response.CloseWithAuthChallenge(HttpUtility.CreateDigestAuthChallenge(this._listener.Realm));
			}
			else
			{
				context.Response.Close(HttpStatusCode.Forbidden);
			}
			return false;
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
			return (!this._secure || EndPointListener.CertificateExists(this._port, this._listener.CertificateFolderPath) || this.Certificate != null) ? null : "The secure connection requires a server certificate.";
		}

		private void receiveRequest()
		{
			for (;;)
			{
				try
				{
					this.acceptRequestAsync(this._listener.GetContext());
				}
				catch (HttpListenerException ex)
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
			this._listener.Close();
			this._receiveRequestThread.Join(millisecondsTimeout);
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

		public byte[] GetFile(string path)
		{
			string text = this.RootPath + path;
			if (this._windows)
			{
				text = text.Replace("/", "\\");
			}
			return (!File.Exists(text)) ? null : File.ReadAllBytes(text);
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
			this._services.Stop(new byte[0], true);
			this.stopListener(5000);
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
			this._services.Stop(data, !code.IsReserved());
			this.stopListener(5000);
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
			this._services.Stop(data, !code.IsReserved());
			this.stopListener(5000);
			this._state = ServerState.Stop;
		}
	}
}
