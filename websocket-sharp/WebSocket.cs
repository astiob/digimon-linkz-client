using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using WebSocketSharp.Net;
using WebSocketSharp.Net.WebSockets;

namespace WebSocketSharp
{
	public class WebSocket : IDisposable
	{
		private const string _guid = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";

		private const string _version = "13";

		internal const int FragmentLength = 1016;

		private AuthenticationChallenge _authChallenge;

		private string _base64Key;

		private RemoteCertificateValidationCallback _certValidationCallback;

		private bool _client;

		private Action _closeContext;

		private CompressionMethod _compression;

		private WebSocketContext _context;

		private CookieCollection _cookies;

		private NetworkCredential _credentials;

		private string _extensions;

		private AutoResetEvent _exitReceiving;

		private object _forConn;

		private object _forSend;

		private Func<WebSocketContext, string> _handshakeRequestChecker;

		private volatile Logger _logger;

		private uint _nonceCount;

		private string _origin;

		private bool _preAuth;

		private string _protocol;

		private string[] _protocols;

		private volatile WebSocketState _readyState;

		private AutoResetEvent _receivePong;

		private bool _secure;

		private WsStream _stream;

		private TcpClient _tcpClient;

		private Uri _uri;

		internal WebSocket(HttpListenerWebSocketContext context, Logger logger)
		{
			this._context = context;
			this._logger = logger;
			this._closeContext = new Action(context.Close);
			this._secure = context.IsSecureConnection;
			this._stream = context.Stream;
			this._uri = context.Path.ToUri();
			this.init();
		}

		internal WebSocket(TcpListenerWebSocketContext context, Logger logger)
		{
			this._context = context;
			this._logger = logger;
			this._closeContext = new Action(context.Close);
			this._secure = context.IsSecureConnection;
			this._stream = context.Stream;
			this._uri = context.Path.ToUri();
			this.init();
		}

		public WebSocket(string url, params string[] protocols)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			string text;
			if (!url.TryCreateWebSocketUri(out this._uri, out text))
			{
				throw new ArgumentException(text, "url");
			}
			if (protocols != null && protocols.Length > 0)
			{
				text = protocols.CheckIfValidProtocols();
				if (text != null)
				{
					throw new ArgumentException(text, "protocols");
				}
				this._protocols = protocols;
			}
			this._base64Key = WebSocket.CreateBase64Key();
			this._client = true;
			this._logger = new Logger();
			this._secure = (this._uri.Scheme == "wss");
			this.init();
		}

		public event EventHandler<CloseEventArgs> OnClose
		{
			add
			{
				EventHandler<CloseEventArgs> eventHandler = this.OnClose;
				EventHandler<CloseEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<CloseEventArgs>>(ref this.OnClose, (EventHandler<CloseEventArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<CloseEventArgs> eventHandler = this.OnClose;
				EventHandler<CloseEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<CloseEventArgs>>(ref this.OnClose, (EventHandler<CloseEventArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		public event EventHandler<ErrorEventArgs> OnError
		{
			add
			{
				EventHandler<ErrorEventArgs> eventHandler = this.OnError;
				EventHandler<ErrorEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ErrorEventArgs>>(ref this.OnError, (EventHandler<ErrorEventArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<ErrorEventArgs> eventHandler = this.OnError;
				EventHandler<ErrorEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ErrorEventArgs>>(ref this.OnError, (EventHandler<ErrorEventArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		public event EventHandler<MessageEventArgs> OnMessage
		{
			add
			{
				EventHandler<MessageEventArgs> eventHandler = this.OnMessage;
				EventHandler<MessageEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<MessageEventArgs>>(ref this.OnMessage, (EventHandler<MessageEventArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<MessageEventArgs> eventHandler = this.OnMessage;
				EventHandler<MessageEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<MessageEventArgs>>(ref this.OnMessage, (EventHandler<MessageEventArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		public event EventHandler OnOpen
		{
			add
			{
				EventHandler eventHandler = this.OnOpen;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.OnOpen, (EventHandler)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler eventHandler = this.OnOpen;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.OnOpen, (EventHandler)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		void IDisposable.Dispose()
		{
			this.Close(CloseStatusCode.Away, null);
		}

		internal CookieCollection CookieCollection
		{
			get
			{
				return this._cookies;
			}
		}

		internal Func<WebSocketContext, string> CustomHandshakeRequestChecker
		{
			get
			{
				Func<WebSocketContext, string> result;
				if ((result = this._handshakeRequestChecker) == null)
				{
					result = ((WebSocketContext context) => null);
				}
				return result;
			}
			set
			{
				this._handshakeRequestChecker = value;
			}
		}

		internal bool IsConnected
		{
			get
			{
				return this._readyState == WebSocketState.Open || this._readyState == WebSocketState.Closing;
			}
		}

		public CompressionMethod Compression
		{
			get
			{
				return this._compression;
			}
			set
			{
				object forConn = this._forConn;
				lock (forConn)
				{
					string text = this.checkIfAvailable("Set operation of Compression", false, false);
					if (text != null)
					{
						this._logger.Error(text);
						this.error(text);
					}
					else
					{
						this._compression = value;
					}
				}
			}
		}

		public IEnumerable<Cookie> Cookies
		{
			get
			{
				object syncRoot = this._cookies.SyncRoot;
				lock (syncRoot)
				{
					IEnumerator enumerator = this._cookies.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							Cookie cookie = (Cookie)obj;
							yield return cookie;
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
				}
				yield break;
			}
		}

		public NetworkCredential Credentials
		{
			get
			{
				return this._credentials;
			}
		}

		public string Extensions
		{
			get
			{
				return this._extensions ?? string.Empty;
			}
		}

		public bool IsAlive
		{
			get
			{
				return this.Ping();
			}
		}

		public bool IsSecure
		{
			get
			{
				return this._secure;
			}
		}

		public Logger Log
		{
			get
			{
				return this._logger;
			}
			internal set
			{
				this._logger = value;
			}
		}

		public string Origin
		{
			get
			{
				return this._origin;
			}
			set
			{
				object forConn = this._forConn;
				lock (forConn)
				{
					string text = this.checkIfAvailable("Set operation of Origin", false, false);
					if (text == null)
					{
						if (value.IsNullOrEmpty())
						{
							this._origin = value;
							return;
						}
						Uri uri;
						if (!Uri.TryCreate(value, UriKind.Absolute, out uri) || uri.Segments.Length > 1)
						{
							text = "The syntax of Origin must be '<scheme>://<host>[:<port>]'.";
						}
					}
					if (text != null)
					{
						this._logger.Error(text);
						this.error(text);
					}
					else
					{
						this._origin = value.TrimEnd(new char[]
						{
							'/'
						});
					}
				}
			}
		}

		public string Protocol
		{
			get
			{
				return this._protocol ?? string.Empty;
			}
			internal set
			{
				this._protocol = value;
			}
		}

		public WebSocketState ReadyState
		{
			get
			{
				return this._readyState;
			}
		}

		public RemoteCertificateValidationCallback ServerCertificateValidationCallback
		{
			get
			{
				return this._certValidationCallback;
			}
			set
			{
				object forConn = this._forConn;
				lock (forConn)
				{
					string text = this.checkIfAvailable("Set operation of ServerCertificateValidationCallback", false, false);
					if (text != null)
					{
						this._logger.Error(text);
						this.error(text);
					}
					else
					{
						this._certValidationCallback = value;
					}
				}
			}
		}

		public Uri Url
		{
			get
			{
				return this._uri;
			}
			internal set
			{
				this._uri = value;
			}
		}

		private bool acceptCloseFrame(WsFrame frame)
		{
			PayloadData payloadData = frame.PayloadData;
			this.close(payloadData, !payloadData.ContainsReservedCloseStatusCode, false);
			return false;
		}

		private bool acceptDataFrame(WsFrame frame)
		{
			MessageEventArgs e = (!frame.IsCompressed) ? new MessageEventArgs(frame.Opcode, frame.PayloadData) : new MessageEventArgs(frame.Opcode, frame.PayloadData.ApplicationData.Decompress(this._compression));
			if (this._readyState == WebSocketState.Open)
			{
				this.OnMessage.Emit(this, e);
			}
			return true;
		}

		private void acceptException(Exception exception, string reason)
		{
			CloseStatusCode closeStatusCode = CloseStatusCode.Abnormal;
			string text = reason;
			if (exception is WebSocketException)
			{
				WebSocketException ex = (WebSocketException)exception;
				closeStatusCode = ex.Code;
				reason = ex.Message;
			}
			if (closeStatusCode == CloseStatusCode.Abnormal || closeStatusCode == CloseStatusCode.TlsHandshakeFailure)
			{
				this._logger.Fatal(exception.ToString());
				reason = text;
			}
			else
			{
				this._logger.Error(reason);
				text = null;
			}
			this.error(text ?? closeStatusCode.GetMessage());
			if (this._readyState == WebSocketState.Connecting && !this._client)
			{
				this.Close(HttpStatusCode.BadRequest);
			}
			else
			{
				this.close(closeStatusCode, reason ?? closeStatusCode.GetMessage(), false);
			}
		}

		private bool acceptFragmentedFrame(WsFrame frame)
		{
			return frame.IsContinuation || this.acceptFragments(frame);
		}

		private bool acceptFragments(WsFrame first)
		{
			bool result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				memoryStream.WriteBytes(first.PayloadData.ApplicationData);
				if (!this.concatenateFragmentsInto(memoryStream))
				{
					result = false;
				}
				else
				{
					byte[] data;
					if (this._compression != CompressionMethod.None)
					{
						data = memoryStream.DecompressToArray(this._compression);
					}
					else
					{
						memoryStream.Close();
						data = memoryStream.ToArray();
					}
					if (this._readyState == WebSocketState.Open)
					{
						this.OnMessage.Emit(this, new MessageEventArgs(first.Opcode, data));
					}
					result = true;
				}
			}
			return result;
		}

		private bool acceptFrame(WsFrame frame)
		{
			return (!frame.IsCompressed || this._compression != CompressionMethod.None) ? ((!frame.IsFragmented) ? ((!frame.IsData) ? ((!frame.IsPing) ? ((!frame.IsPong) ? ((!frame.IsClose) ? this.acceptUnsupportedFrame(frame, CloseStatusCode.PolicyViolation, null) : this.acceptCloseFrame(frame)) : this.acceptPongFrame(frame)) : this.acceptPingFrame(frame)) : this.acceptDataFrame(frame)) : this.acceptFragmentedFrame(frame)) : this.acceptUnsupportedFrame(frame, CloseStatusCode.IncorrectData, "A compressed data has been received without available decompression method.");
		}

		private bool acceptHandshake()
		{
			this._logger.Debug(string.Format("A WebSocket connection request from {0}:\n{1}", this._context.UserEndPoint, this._context));
			string text = this.checkIfValidHandshakeRequest(this._context);
			if (text != null)
			{
				this._logger.Error(text);
				this.error("An error has occurred while connecting.");
				this.Close(HttpStatusCode.BadRequest);
				return false;
			}
			if (this._protocol != null && !this._context.SecWebSocketProtocols.Contains((string protocol) => protocol == this._protocol))
			{
				this._protocol = null;
			}
			string text2 = this._context.Headers["Sec-WebSocket-Extensions"];
			if (text2 != null && text2.Length > 0)
			{
				this.acceptSecWebSocketExtensionsHeader(text2);
			}
			return this.send(this.createHandshakeResponse());
		}

		private bool acceptPingFrame(WsFrame frame)
		{
			Mask mask = (!this._client) ? Mask.Unmask : Mask.Mask;
			if (this.send(WsFrame.CreatePongFrame(mask, frame.PayloadData)))
			{
				this._logger.Trace("Returned a Pong.");
			}
			return true;
		}

		private bool acceptPongFrame(WsFrame frame)
		{
			this._receivePong.Set();
			this._logger.Trace("Received a Pong.");
			return true;
		}

		private void acceptSecWebSocketExtensionsHeader(string value)
		{
			StringBuilder stringBuilder = new StringBuilder(32);
			bool flag = false;
			foreach (string text in value.SplitHeaderValue(new char[]
			{
				','
			}))
			{
				string text2 = text.Trim();
				string value2 = text2.RemovePrefix(new string[]
				{
					"x-webkit-"
				});
				if (!flag && value2.IsCompressionExtension())
				{
					CompressionMethod compressionMethod = value2.ToCompressionMethod();
					if (compressionMethod != CompressionMethod.None)
					{
						this._compression = compressionMethod;
						flag = true;
						stringBuilder.Append(text2 + ", ");
					}
				}
			}
			int length = stringBuilder.Length;
			if (length > 0)
			{
				stringBuilder.Length = length - 2;
				this._extensions = stringBuilder.ToString();
			}
		}

		private bool acceptUnsupportedFrame(WsFrame frame, CloseStatusCode code, string reason)
		{
			this._logger.Debug("Unsupported frame:\n" + frame.PrintToString(false));
			this.acceptException(new WebSocketException(code, reason), null);
			return false;
		}

		private string checkIfAvailable(string operation, bool availableAsServer, bool availableAsConnected)
		{
			return (this._client || availableAsServer) ? (availableAsConnected ? null : this._readyState.CheckIfConnectable()) : (operation + " isn't available as a server.");
		}

		private string checkIfCanConnect()
		{
			return (this._client || this._readyState != WebSocketState.Closed) ? this._readyState.CheckIfConnectable() : "Connect isn't available to reconnect as a server.";
		}

		private string checkIfValidHandshakeRequest(WebSocketContext context)
		{
			NameValueCollection headers = context.Headers;
			return context.IsWebSocketRequest ? (this.validateHostHeader(headers["Host"]) ? (this.validateSecWebSocketKeyHeader(headers["Sec-WebSocket-Key"]) ? (this.validateSecWebSocketVersionClientHeader(headers["Sec-WebSocket-Version"]) ? this.CustomHandshakeRequestChecker(context) : "Invalid Sec-WebSocket-Version header.") : "Invalid Sec-WebSocket-Key header.") : "Invalid Host header.") : "Not WebSocket connection request.";
		}

		private string checkIfValidHandshakeResponse(HandshakeResponse response)
		{
			NameValueCollection headers = response.Headers;
			return (!response.IsUnauthorized) ? (response.IsWebSocketResponse ? (this.validateSecWebSocketAcceptHeader(headers["Sec-WebSocket-Accept"]) ? (this.validateSecWebSocketProtocolHeader(headers["Sec-WebSocket-Protocol"]) ? (this.validateSecWebSocketExtensionsHeader(headers["Sec-WebSocket-Extensions"]) ? (this.validateSecWebSocketVersionServerHeader(headers["Sec-WebSocket-Version"]) ? null : "Invalid Sec-WebSocket-Version header.") : "Invalid Sec-WebSocket-Extensions header.") : "Invalid Sec-WebSocket-Protocol header.") : "Invalid Sec-WebSocket-Accept header.") : "Not WebSocket connection response.") : string.Format("HTTP {0} authorization is required.", response.AuthChallenge.Scheme);
		}

		private void close(CloseStatusCode code, string reason, bool wait)
		{
			this.close(new PayloadData(((ushort)code).Append(reason)), !code.IsReserved(), wait);
		}

		private void close(PayloadData payload, bool send, bool wait)
		{
			object forConn = this._forConn;
			lock (forConn)
			{
				if (this._readyState == WebSocketState.Closing || this._readyState == WebSocketState.Closed)
				{
					this._logger.Info("Closing the WebSocket connection has already been done.");
					return;
				}
				this._readyState = WebSocketState.Closing;
			}
			this._logger.Trace("Start closing handshake.");
			CloseEventArgs closeEventArgs = new CloseEventArgs(payload);
			closeEventArgs.WasClean = ((!this._client) ? this.closeHandshake((!send) ? null : WsFrame.CreateCloseFrame(Mask.Unmask, payload).ToByteArray(), (!wait) ? 0 : 1000, new Action(this.closeServerResources)) : this.closeHandshake((!send) ? null : WsFrame.CreateCloseFrame(Mask.Mask, payload).ToByteArray(), (!wait) ? 0 : 5000, new Action(this.closeClientResources)));
			this._logger.Trace("End closing handshake.");
			this._readyState = WebSocketState.Closed;
			try
			{
				this.OnClose.Emit(this, closeEventArgs);
			}
			catch (Exception ex)
			{
				this._logger.Fatal(ex.ToString());
				this.error("An exception has occurred while OnClose.");
			}
		}

		private void closeAsync(PayloadData payload, bool send, bool wait)
		{
			Action<PayloadData, bool, bool> closer = new Action<PayloadData, bool, bool>(this.close);
			closer.BeginInvoke(payload, send, wait, delegate(IAsyncResult ar)
			{
				closer.EndInvoke(ar);
			}, null);
		}

		private void closeClientResources()
		{
			if (this._stream != null)
			{
				this._stream.Dispose();
				this._stream = null;
			}
			if (this._tcpClient != null)
			{
				this._tcpClient.Close();
				this._tcpClient = null;
			}
		}

		private bool closeHandshake(byte[] frame, int timeout, Action release)
		{
			bool flag = frame != null && this._stream.Write(frame);
			bool flag2 = timeout == 0 || (flag && this._exitReceiving != null && this._exitReceiving.WaitOne(timeout));
			release();
			if (this._receivePong != null)
			{
				this._receivePong.Close();
				this._receivePong = null;
			}
			if (this._exitReceiving != null)
			{
				this._exitReceiving.Close();
				this._exitReceiving = null;
			}
			bool flag3 = flag && flag2;
			this._logger.Debug(string.Format("Was clean?: {0}\nsent: {1} received: {2}", flag3, flag, flag2));
			return flag3;
		}

		private void closeServerResources()
		{
			if (this._closeContext == null)
			{
				return;
			}
			this._closeContext();
			this._closeContext = null;
			this._stream = null;
			this._context = null;
		}

		private bool concatenateFragmentsInto(Stream dest)
		{
			WsFrame wsFrame;
			for (;;)
			{
				wsFrame = this._stream.ReadFrame();
				if (wsFrame.IsFinal)
				{
					if (wsFrame.IsContinuation)
					{
						break;
					}
					if (wsFrame.IsPing)
					{
						this.acceptPingFrame(wsFrame);
					}
					else
					{
						if (!wsFrame.IsPong)
						{
							goto IL_68;
						}
						this.acceptPongFrame(wsFrame);
					}
				}
				else
				{
					if (!wsFrame.IsContinuation)
					{
						goto IL_A1;
					}
					dest.WriteBytes(wsFrame.PayloadData.ApplicationData);
				}
			}
			dest.WriteBytes(wsFrame.PayloadData.ApplicationData);
			return true;
			IL_68:
			if (wsFrame.IsClose)
			{
				return this.acceptCloseFrame(wsFrame);
			}
			IL_A1:
			return this.acceptUnsupportedFrame(wsFrame, CloseStatusCode.IncorrectData, "An incorrect data has been received while receiving fragmented data.");
		}

		private bool connect()
		{
			object forConn = this._forConn;
			bool result;
			lock (forConn)
			{
				string text = this._readyState.CheckIfConnectable();
				if (text != null)
				{
					this._logger.Error(text);
					this.error(text);
					result = false;
				}
				else
				{
					try
					{
						if ((!this._client) ? this.acceptHandshake() : this.doHandshake())
						{
							this._readyState = WebSocketState.Open;
							return true;
						}
					}
					catch (Exception exception)
					{
						this.acceptException(exception, "An exception has occurred while connecting.");
					}
					result = false;
				}
			}
			return result;
		}

		private string createExtensionsRequest()
		{
			StringBuilder stringBuilder = new StringBuilder(32);
			if (this._compression != CompressionMethod.None)
			{
				stringBuilder.Append(this._compression.ToExtensionString());
			}
			return (stringBuilder.Length <= 0) ? null : stringBuilder.ToString();
		}

		private HandshakeRequest createHandshakeRequest()
		{
			string pathAndQuery = this._uri.PathAndQuery;
			string value = (this._uri.Port != 80) ? this._uri.Authority : this._uri.DnsSafeHost;
			HandshakeRequest handshakeRequest = new HandshakeRequest(pathAndQuery);
			NameValueCollection headers = handshakeRequest.Headers;
			headers["Host"] = value;
			if (!this._origin.IsNullOrEmpty())
			{
				headers["Origin"] = this._origin;
			}
			headers["Sec-WebSocket-Key"] = this._base64Key;
			if (this._protocols != null)
			{
				headers["Sec-WebSocket-Protocol"] = this._protocols.ToString(", ");
			}
			string text = this.createExtensionsRequest();
			if (text != null)
			{
				headers["Sec-WebSocket-Extensions"] = text;
			}
			headers["Sec-WebSocket-Version"] = "13";
			AuthenticationResponse authenticationResponse = null;
			if (this._authChallenge != null && this._credentials != null)
			{
				authenticationResponse = new AuthenticationResponse(this._authChallenge, this._credentials, this._nonceCount);
				this._nonceCount = authenticationResponse.NonceCount;
			}
			else if (this._preAuth)
			{
				authenticationResponse = new AuthenticationResponse(this._credentials);
			}
			if (authenticationResponse != null)
			{
				headers["Authorization"] = authenticationResponse.ToString();
			}
			if (this._cookies.Count > 0)
			{
				handshakeRequest.SetCookies(this._cookies);
			}
			return handshakeRequest;
		}

		private HandshakeResponse createHandshakeResponse()
		{
			HandshakeResponse handshakeResponse = new HandshakeResponse(HttpStatusCode.SwitchingProtocols);
			NameValueCollection headers = handshakeResponse.Headers;
			headers["Sec-WebSocket-Accept"] = WebSocket.CreateResponseKey(this._base64Key);
			if (this._protocol != null)
			{
				headers["Sec-WebSocket-Protocol"] = this._protocol;
			}
			if (this._extensions != null)
			{
				headers["Sec-WebSocket-Extensions"] = this._extensions;
			}
			if (this._cookies.Count > 0)
			{
				handshakeResponse.SetCookies(this._cookies);
			}
			return handshakeResponse;
		}

		private HandshakeResponse createHandshakeResponse(HttpStatusCode code)
		{
			HandshakeResponse handshakeResponse = HandshakeResponse.CreateCloseResponse(code);
			handshakeResponse.Headers["Sec-WebSocket-Version"] = "13";
			return handshakeResponse;
		}

		private bool doHandshake()
		{
			this.setClientStream();
			HandshakeResponse handshakeResponse = this.sendHandshakeRequest();
			string text = this.checkIfValidHandshakeResponse(handshakeResponse);
			if (text != null)
			{
				this._logger.Error(text);
				text = "An error has occurred while connecting.";
				this.error(text);
				this.close(CloseStatusCode.Abnormal, text, false);
				return false;
			}
			CookieCollection cookies = handshakeResponse.Cookies;
			if (cookies.Count > 0)
			{
				this._cookies.SetOrRemove(cookies);
			}
			return true;
		}

		private void error(string message)
		{
			this.OnError.Emit(this, new ErrorEventArgs(message));
		}

		private void init()
		{
			this._compression = CompressionMethod.None;
			this._cookies = new CookieCollection();
			this._forConn = new object();
			this._forSend = new object();
			this._readyState = WebSocketState.Connecting;
		}

		private void open()
		{
			try
			{
				this.OnOpen.Emit(this, EventArgs.Empty);
				if (this._readyState == WebSocketState.Open)
				{
					this.startReceiving();
				}
			}
			catch (Exception exception)
			{
				this.acceptException(exception, "An exception has occurred while opening.");
			}
		}

		private HandshakeResponse receiveHandshakeResponse()
		{
			HandshakeResponse handshakeResponse = this._stream.ReadHandshakeResponse();
			this._logger.Debug("A response to this WebSocket connection request:\n" + handshakeResponse.ToString());
			return handshakeResponse;
		}

		private bool send(byte[] frame)
		{
			object forConn = this._forConn;
			bool result;
			lock (forConn)
			{
				if (this._readyState != WebSocketState.Open)
				{
					this._logger.Warn("Sending has been interrupted.");
					result = false;
				}
				else
				{
					result = this._stream.Write(frame);
				}
			}
			return result;
		}

		private void send(HandshakeRequest request)
		{
			this._logger.Debug(string.Format("A WebSocket connection request to {0}:\n{1}", this._uri, request));
			this._stream.WriteHandshake(request);
		}

		private bool send(HandshakeResponse response)
		{
			this._logger.Debug("A response to the WebSocket connection request:\n" + response.ToString());
			return this._stream.WriteHandshake(response);
		}

		private bool send(WsFrame frame)
		{
			object forConn = this._forConn;
			bool result;
			lock (forConn)
			{
				if (this._readyState != WebSocketState.Open)
				{
					this._logger.Warn("Sending has been interrupted.");
					result = false;
				}
				else
				{
					result = this._stream.Write(frame.ToByteArray());
				}
			}
			return result;
		}

		private bool send(Opcode opcode, byte[] data)
		{
			object forSend = this._forSend;
			bool result;
			lock (forSend)
			{
				bool flag = false;
				try
				{
					bool compressed = false;
					if (this._compression != CompressionMethod.None)
					{
						data = data.Compress(this._compression);
						compressed = true;
					}
					Mask mask = (!this._client) ? Mask.Unmask : Mask.Mask;
					flag = this.send(WsFrame.CreateFrame(Fin.Final, opcode, mask, data, compressed));
				}
				catch (Exception ex)
				{
					this._logger.Fatal(ex.ToString());
					this.error("An exception has occurred while sending a data.");
				}
				result = flag;
			}
			return result;
		}

		private bool send(Opcode opcode, Stream stream)
		{
			object forSend = this._forSend;
			bool result;
			lock (forSend)
			{
				bool flag = false;
				Stream stream2 = stream;
				bool flag2 = false;
				try
				{
					if (this._compression != CompressionMethod.None)
					{
						stream = stream.Compress(this._compression);
						flag2 = true;
					}
					Mask mask = (!this._client) ? Mask.Unmask : Mask.Mask;
					flag = this.sendFragmented(opcode, stream, mask, flag2);
				}
				catch (Exception ex)
				{
					this._logger.Fatal(ex.ToString());
					this.error("An exception has occurred while sending a data.");
				}
				finally
				{
					if (flag2)
					{
						stream.Dispose();
					}
					stream2.Dispose();
				}
				result = flag;
			}
			return result;
		}

		private void sendAsync(Opcode opcode, byte[] data, Action<bool> completed)
		{
			Func<Opcode, byte[], bool> sender = new Func<Opcode, byte[], bool>(this.send);
			sender.BeginInvoke(opcode, data, delegate(IAsyncResult ar)
			{
				try
				{
					bool obj = sender.EndInvoke(ar);
					if (completed != null)
					{
						completed(obj);
					}
				}
				catch (Exception ex)
				{
					this._logger.Fatal(ex.ToString());
					this.error("An exception has occurred while callback.");
				}
			}, null);
		}

		private void sendAsync(Opcode opcode, Stream stream, Action<bool> completed)
		{
			Func<Opcode, Stream, bool> sender = new Func<Opcode, Stream, bool>(this.send);
			sender.BeginInvoke(opcode, stream, delegate(IAsyncResult ar)
			{
				try
				{
					bool obj = sender.EndInvoke(ar);
					if (completed != null)
					{
						completed(obj);
					}
				}
				catch (Exception ex)
				{
					this._logger.Fatal(ex.ToString());
					this.error("An exception has occurred while callback.");
				}
			}, null);
		}

		private bool sendFragmented(Opcode opcode, Stream stream, Mask mask, bool compressed)
		{
			long length = stream.Length;
			long num = length / 1016L;
			int num2 = (int)(length % 1016L);
			long num3 = (num2 != 0) ? (num - 1L) : (num - 2L);
			byte[] array;
			if (num == 0L)
			{
				array = new byte[num2];
				return stream.Read(array, 0, num2) == num2 && this.send(WsFrame.CreateFrame(Fin.Final, opcode, mask, array, compressed));
			}
			array = new byte[1016];
			if (stream.Read(array, 0, 1016) != 1016 || !this.send(WsFrame.CreateFrame(Fin.More, opcode, mask, array, compressed)))
			{
				return false;
			}
			for (long num4 = 0L; num4 < num3; num4 += 1L)
			{
				if (stream.Read(array, 0, 1016) != 1016 || !this.send(WsFrame.CreateFrame(Fin.More, Opcode.Cont, mask, array, compressed)))
				{
					return false;
				}
			}
			int num5 = 1016;
			if (num2 != 0)
			{
				array = new byte[num5 = num2];
			}
			return stream.Read(array, 0, num5) == num5 && this.send(WsFrame.CreateFrame(Fin.Final, Opcode.Cont, mask, array, compressed));
		}

		private HandshakeResponse sendHandshakeRequest()
		{
			HandshakeRequest handshakeRequest = this.createHandshakeRequest();
			HandshakeResponse handshakeResponse = this.sendHandshakeRequest(handshakeRequest);
			if (handshakeResponse.IsUnauthorized)
			{
				this._authChallenge = handshakeResponse.AuthChallenge;
				if (this._credentials != null && (!this._preAuth || this._authChallenge.Scheme == "digest"))
				{
					if (handshakeResponse.Headers.Contains("Connection", "close"))
					{
						this.closeClientResources();
						this.setClientStream();
					}
					AuthenticationResponse authenticationResponse = new AuthenticationResponse(this._authChallenge, this._credentials, this._nonceCount);
					this._nonceCount = authenticationResponse.NonceCount;
					handshakeRequest.Headers["Authorization"] = authenticationResponse.ToString();
					handshakeResponse = this.sendHandshakeRequest(handshakeRequest);
				}
			}
			return handshakeResponse;
		}

		private HandshakeResponse sendHandshakeRequest(HandshakeRequest request)
		{
			this.send(request);
			return this.receiveHandshakeResponse();
		}

		private void setClientStream()
		{
			string dnsSafeHost = this._uri.DnsSafeHost;
			int port = this._uri.Port;
			this._tcpClient = new TcpClient(dnsSafeHost, port);
			this._stream = WsStream.CreateClientStream(this._tcpClient, this._secure, dnsSafeHost, this._certValidationCallback);
		}

		private void startReceiving()
		{
			this._exitReceiving = new AutoResetEvent(false);
			this._receivePong = new AutoResetEvent(false);
			Action receive = null;
			receive = delegate()
			{
				this._stream.ReadFrameAsync(delegate(WsFrame frame)
				{
					if (this.acceptFrame(frame) && this._readyState != WebSocketState.Closed)
					{
						receive();
					}
					else if (this._exitReceiving != null)
					{
						this._exitReceiving.Set();
					}
				}, delegate(Exception ex)
				{
					this.acceptException(ex, "An exception has occurred while receiving a message.");
				});
			};
			receive();
		}

		private bool validateHostHeader(string value)
		{
			if (value == null || value.Length == 0)
			{
				return false;
			}
			if (!this._uri.IsAbsoluteUri)
			{
				return true;
			}
			int num = value.IndexOf(':');
			string text = (num <= 0) ? value : value.Substring(0, num);
			string dnsSafeHost = this._uri.DnsSafeHost;
			return Uri.CheckHostName(text) != UriHostNameType.Dns || Uri.CheckHostName(dnsSafeHost) != UriHostNameType.Dns || text == dnsSafeHost;
		}

		private bool validateSecWebSocketAcceptHeader(string value)
		{
			return value != null && value == WebSocket.CreateResponseKey(this._base64Key);
		}

		private bool validateSecWebSocketExtensionsHeader(string value)
		{
			bool flag = this._compression != CompressionMethod.None;
			if (value == null || value.Length == 0)
			{
				if (flag)
				{
					this._compression = CompressionMethod.None;
				}
				return true;
			}
			if (!flag)
			{
				return false;
			}
			IEnumerable<string> source = value.SplitHeaderValue(new char[]
			{
				','
			});
			if (source.Contains((string extension) => extension.Trim() != this._compression.ToExtensionString()))
			{
				return false;
			}
			this._extensions = value;
			return true;
		}

		private bool validateSecWebSocketKeyHeader(string value)
		{
			if (value == null || value.Length == 0)
			{
				return false;
			}
			this._base64Key = value;
			return true;
		}

		private bool validateSecWebSocketProtocolHeader(string value)
		{
			if (value == null)
			{
				return this._protocols == null;
			}
			if (this._protocols == null || !this._protocols.Contains((string protocol) => protocol == value))
			{
				return false;
			}
			this._protocol = value;
			return true;
		}

		private bool validateSecWebSocketVersionClientHeader(string value)
		{
			return value != null && value == "13";
		}

		private bool validateSecWebSocketVersionServerHeader(string value)
		{
			return value == null || value == "13";
		}

		internal void Close(HandshakeResponse response)
		{
			this._readyState = WebSocketState.Closing;
			this.send(response);
			this.closeServerResources();
			this._readyState = WebSocketState.Closed;
		}

		internal void Close(HttpStatusCode code)
		{
			this.Close(this.createHandshakeResponse(code));
		}

		internal void Close(CloseEventArgs args, byte[] frame, int timeout)
		{
			object forConn = this._forConn;
			lock (forConn)
			{
				if (this._readyState == WebSocketState.Closing || this._readyState == WebSocketState.Closed)
				{
					this._logger.Info("Closing the WebSocket connection has already been done.");
					return;
				}
				this._readyState = WebSocketState.Closing;
			}
			args.WasClean = this.closeHandshake(frame, timeout, new Action(this.closeServerResources));
			this._readyState = WebSocketState.Closed;
			try
			{
				this.OnClose.Emit(this, args);
			}
			catch (Exception ex)
			{
				this._logger.Fatal(ex.ToString());
			}
		}

		internal void ConnectAsServer()
		{
			try
			{
				if (this.acceptHandshake())
				{
					this._readyState = WebSocketState.Open;
					this.open();
				}
			}
			catch (Exception exception)
			{
				this.acceptException(exception, "An exception has occurred while connecting.");
			}
		}

		internal static string CreateBase64Key()
		{
			byte[] array = new byte[16];
			Random random = new Random();
			random.NextBytes(array);
			return Convert.ToBase64String(array);
		}

		internal static string CreateResponseKey(string base64Key)
		{
			StringBuilder stringBuilder = new StringBuilder(base64Key, 64);
			stringBuilder.Append("258EAFA5-E914-47DA-95CA-C5AB0DC85B11");
			SHA1 sha = new SHA1CryptoServiceProvider();
			byte[] inArray = sha.ComputeHash(Encoding.UTF8.GetBytes(stringBuilder.ToString()));
			return Convert.ToBase64String(inArray);
		}

		internal bool Ping(byte[] frame, int timeout)
		{
			return this.send(frame) && this._receivePong.WaitOne(timeout);
		}

		internal void Send(Opcode opcode, byte[] data, Dictionary<CompressionMethod, byte[]> cache)
		{
			object forSend = this._forSend;
			lock (forSend)
			{
				object forConn = this._forConn;
				lock (forConn)
				{
					if (this._readyState == WebSocketState.Open)
					{
						try
						{
							byte[] array;
							if (!cache.TryGetValue(this._compression, out array))
							{
								array = WsFrame.CreateFrame(Fin.Final, opcode, Mask.Unmask, data.Compress(this._compression), this._compression != CompressionMethod.None).ToByteArray();
								cache.Add(this._compression, array);
							}
							this._stream.Write(array);
						}
						catch (Exception ex)
						{
							this._logger.Fatal(ex.ToString());
							this.error("An exception has occurred while sending a data.");
						}
					}
				}
			}
		}

		internal void Send(Opcode opcode, Stream stream, Dictionary<CompressionMethod, Stream> cache)
		{
			object forSend = this._forSend;
			lock (forSend)
			{
				try
				{
					Stream stream2;
					if (!cache.TryGetValue(this._compression, out stream2))
					{
						stream2 = stream.Compress(this._compression);
						cache.Add(this._compression, stream2);
					}
					else
					{
						stream2.Position = 0L;
					}
					if (this._readyState == WebSocketState.Open)
					{
						this.sendFragmented(opcode, stream2, Mask.Unmask, this._compression != CompressionMethod.None);
					}
				}
				catch (Exception ex)
				{
					this._logger.Fatal(ex.ToString());
					this.error("An exception has occurred while sending a data.");
				}
			}
		}

		public void Close()
		{
			string text = this._readyState.CheckIfClosable();
			if (text != null)
			{
				this._logger.Error(text);
				this.error(text);
				return;
			}
			bool flag = this._readyState == WebSocketState.Open;
			this.close(new PayloadData(), flag, flag);
		}

		public void Close(ushort code)
		{
			this.Close(code, null);
		}

		public void Close(CloseStatusCode code)
		{
			this.Close(code, null);
		}

		public void Close(ushort code, string reason)
		{
			byte[] appData = null;
			string text;
			if ((text = this._readyState.CheckIfClosable()) == null && (text = code.CheckIfValidCloseStatusCode()) == null)
			{
				text = (appData = code.Append(reason)).CheckIfValidControlData("reason");
			}
			string text2 = text;
			if (text2 != null)
			{
				this._logger.Error(string.Format("{0}\ncode: {1} reason: {2}", text2, code, reason));
				this.error(text2);
				return;
			}
			bool flag = this._readyState == WebSocketState.Open && !code.IsReserved();
			this.close(new PayloadData(appData), flag, flag);
		}

		public void Close(CloseStatusCode code, string reason)
		{
			byte[] appData = null;
			string text;
			if ((text = this._readyState.CheckIfClosable()) == null)
			{
				text = (appData = ((ushort)code).Append(reason)).CheckIfValidControlData("reason");
			}
			string text2 = text;
			if (text2 != null)
			{
				this._logger.Error(string.Format("{0}\ncode: {1} reason: {2}", text2, code, reason));
				this.error(text2);
				return;
			}
			bool flag = this._readyState == WebSocketState.Open && !code.IsReserved();
			this.close(new PayloadData(appData), flag, flag);
		}

		public void CloseAsync()
		{
			string text = this._readyState.CheckIfClosable();
			if (text != null)
			{
				this._logger.Error(text);
				this.error(text);
				return;
			}
			bool flag = this._readyState == WebSocketState.Open;
			this.closeAsync(new PayloadData(), flag, flag);
		}

		public void CloseAsync(ushort code)
		{
			this.CloseAsync(code, null);
		}

		public void CloseAsync(CloseStatusCode code)
		{
			this.CloseAsync(code, null);
		}

		public void CloseAsync(ushort code, string reason)
		{
			byte[] appData = null;
			string text;
			if ((text = this._readyState.CheckIfClosable()) == null && (text = code.CheckIfValidCloseStatusCode()) == null)
			{
				text = (appData = code.Append(reason)).CheckIfValidControlData("reason");
			}
			string text2 = text;
			if (text2 != null)
			{
				this._logger.Error(string.Format("{0}\ncode: {1} reason: {2}", text2, code, reason));
				this.error(text2);
				return;
			}
			bool flag = this._readyState == WebSocketState.Open && !code.IsReserved();
			this.closeAsync(new PayloadData(appData), flag, flag);
		}

		public void CloseAsync(CloseStatusCode code, string reason)
		{
			byte[] appData = null;
			string text;
			if ((text = this._readyState.CheckIfClosable()) == null)
			{
				text = (appData = ((ushort)code).Append(reason)).CheckIfValidControlData("reason");
			}
			string text2 = text;
			if (text2 != null)
			{
				this._logger.Error(string.Format("{0}\ncode: {1} reason: {2}", text2, code, reason));
				this.error(text2);
				return;
			}
			bool flag = this._readyState == WebSocketState.Open && !code.IsReserved();
			this.closeAsync(new PayloadData(appData), flag, flag);
		}

		public void Connect()
		{
			string text = this.checkIfCanConnect();
			if (text != null)
			{
				this._logger.Error(text);
				this.error(text);
				return;
			}
			if (this.connect())
			{
				this.open();
			}
		}

		public void ConnectAsync()
		{
			string text = this.checkIfCanConnect();
			if (text != null)
			{
				this._logger.Error(text);
				this.error(text);
				return;
			}
			Func<bool> connector = new Func<bool>(this.connect);
			connector.BeginInvoke(delegate(IAsyncResult ar)
			{
				if (connector.EndInvoke(ar))
				{
					this.open();
				}
			}, null);
		}

		public bool Ping()
		{
			return (!this._client) ? this.Ping(WsFrame.EmptyUnmaskPingData, 1000) : this.Ping(WsFrame.CreatePingFrame(Mask.Mask).ToByteArray(), 5000);
		}

		public bool Ping(string message)
		{
			if (message == null || message.Length == 0)
			{
				return this.Ping();
			}
			byte[] bytes = Encoding.UTF8.GetBytes(message);
			string text = bytes.CheckIfValidControlData("message");
			if (text != null)
			{
				this._logger.Error(text);
				this.error(text);
				return false;
			}
			return (!this._client) ? this.Ping(WsFrame.CreatePingFrame(Mask.Unmask, bytes).ToByteArray(), 1000) : this.Ping(WsFrame.CreatePingFrame(Mask.Mask, bytes).ToByteArray(), 5000);
		}

		public void Send(byte[] data)
		{
			string text = this._readyState.CheckIfOpen() ?? data.CheckIfValidSendData();
			if (text != null)
			{
				this._logger.Error(text);
				this.error(text);
				return;
			}
			long longLength = data.LongLength;
			if (longLength <= 1016L)
			{
				this.send(Opcode.Binary, (longLength <= 0L || !this._client || this._compression != CompressionMethod.None) ? data : data.Copy(longLength));
			}
			else
			{
				this.send(Opcode.Binary, new MemoryStream(data));
			}
		}

		public void Send(FileInfo file)
		{
			string text = this._readyState.CheckIfOpen() ?? file.CheckIfValidSendData();
			if (text != null)
			{
				this._logger.Error(text);
				this.error(text);
				return;
			}
			this.send(Opcode.Binary, file.OpenRead());
		}

		public void Send(string data)
		{
			string text = this._readyState.CheckIfOpen() ?? data.CheckIfValidSendData();
			if (text != null)
			{
				this._logger.Error(text);
				this.error(text);
				return;
			}
			byte[] bytes = Encoding.UTF8.GetBytes(data);
			if (bytes.LongLength <= 1016L)
			{
				this.send(Opcode.Text, bytes);
			}
			else
			{
				this.send(Opcode.Text, new MemoryStream(bytes));
			}
		}

		public void SendAsync(byte[] data, Action<bool> completed)
		{
			string text = this._readyState.CheckIfOpen() ?? data.CheckIfValidSendData();
			if (text != null)
			{
				this._logger.Error(text);
				this.error(text);
				return;
			}
			long longLength = data.LongLength;
			if (longLength <= 1016L)
			{
				this.sendAsync(Opcode.Binary, (longLength <= 0L || !this._client || this._compression != CompressionMethod.None) ? data : data.Copy(longLength), completed);
			}
			else
			{
				this.sendAsync(Opcode.Binary, new MemoryStream(data), completed);
			}
		}

		public void SendAsync(FileInfo file, Action<bool> completed)
		{
			string text = this._readyState.CheckIfOpen() ?? file.CheckIfValidSendData();
			if (text != null)
			{
				this._logger.Error(text);
				this.error(text);
				return;
			}
			this.sendAsync(Opcode.Binary, file.OpenRead(), completed);
		}

		public void SendAsync(string data, Action<bool> completed)
		{
			string text = this._readyState.CheckIfOpen() ?? data.CheckIfValidSendData();
			if (text != null)
			{
				this._logger.Error(text);
				this.error(text);
				return;
			}
			byte[] bytes = Encoding.UTF8.GetBytes(data);
			if (bytes.LongLength <= 1016L)
			{
				this.sendAsync(Opcode.Text, bytes, completed);
			}
			else
			{
				this.sendAsync(Opcode.Text, new MemoryStream(bytes), completed);
			}
		}

		public void SendAsync(Stream stream, int length, Action<bool> completed)
		{
			WebSocket.<SendAsync>c__AnonStorey11 <SendAsync>c__AnonStorey = new WebSocket.<SendAsync>c__AnonStorey11();
			<SendAsync>c__AnonStorey.length = length;
			<SendAsync>c__AnonStorey.completed = completed;
			<SendAsync>c__AnonStorey.<>f__this = this;
			WebSocket.<SendAsync>c__AnonStorey11 <SendAsync>c__AnonStorey2 = <SendAsync>c__AnonStorey;
			string msg;
			if ((msg = this._readyState.CheckIfOpen()) == null)
			{
				msg = (stream.CheckIfCanRead() ?? ((<SendAsync>c__AnonStorey.length >= 1) ? null : "'length' must be greater than 0."));
			}
			<SendAsync>c__AnonStorey2.msg = msg;
			if (<SendAsync>c__AnonStorey.msg != null)
			{
				this._logger.Error(<SendAsync>c__AnonStorey.msg);
				this.error(<SendAsync>c__AnonStorey.msg);
				return;
			}
			stream.ReadBytesAsync(<SendAsync>c__AnonStorey.length, delegate(byte[] data)
			{
				int num = data.Length;
				if (num == 0)
				{
					<SendAsync>c__AnonStorey.msg = "A data cannot be read from 'stream'.";
					<SendAsync>c__AnonStorey.<>f__this._logger.Error(<SendAsync>c__AnonStorey.msg);
					<SendAsync>c__AnonStorey.<>f__this.error(<SendAsync>c__AnonStorey.msg);
					return;
				}
				if (num < <SendAsync>c__AnonStorey.length)
				{
					<SendAsync>c__AnonStorey.<>f__this._logger.Warn(string.Format("A data with 'length' cannot be read from 'stream'.\nexpected: {0} actual: {1}", <SendAsync>c__AnonStorey.length, num));
				}
				bool obj = (num > 1016) ? <SendAsync>c__AnonStorey.<>f__this.send(Opcode.Binary, new MemoryStream(data)) : <SendAsync>c__AnonStorey.<>f__this.send(Opcode.Binary, data);
				if (<SendAsync>c__AnonStorey.completed != null)
				{
					<SendAsync>c__AnonStorey.completed(obj);
				}
			}, delegate(Exception ex)
			{
				<SendAsync>c__AnonStorey.<>f__this._logger.Fatal(ex.ToString());
				<SendAsync>c__AnonStorey.<>f__this.error("An exception has occurred while sending a data.");
			});
		}

		public void SetCookie(Cookie cookie)
		{
			object forConn = this._forConn;
			lock (forConn)
			{
				string text = this.checkIfAvailable("SetCookie", false, false) ?? ((cookie != null) ? null : "'cookie' must not be null.");
				if (text != null)
				{
					this._logger.Error(text);
					this.error(text);
				}
				else
				{
					object syncRoot = this._cookies.SyncRoot;
					lock (syncRoot)
					{
						this._cookies.SetOrRemove(cookie);
					}
				}
			}
		}

		public void SetCredentials(string username, string password, bool preAuth)
		{
			object forConn = this._forConn;
			lock (forConn)
			{
				string text = this.checkIfAvailable("SetCredentials", false, false);
				if (text == null)
				{
					if (username.IsNullOrEmpty())
					{
						this._credentials = null;
						this._preAuth = false;
						this._logger.Warn("Credentials was set back to the default.");
						return;
					}
					text = ((!username.Contains(new char[]
					{
						':'
					}) && username.IsText()) ? ((password.IsNullOrEmpty() || password.IsText()) ? null : "'password' contains an invalid character.") : "'username' contains an invalid character.");
				}
				if (text != null)
				{
					this._logger.Error(text);
					this.error(text);
				}
				else
				{
					this._credentials = new NetworkCredential(username, password, this._uri.PathAndQuery, new string[0]);
					this._preAuth = preAuth;
				}
			}
		}
	}
}
