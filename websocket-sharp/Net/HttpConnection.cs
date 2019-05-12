using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using WebSocketSharp.Net.Security;

namespace WebSocketSharp.Net
{
	internal sealed class HttpConnection
	{
		private const int _bufferSize = 8192;

		private byte[] _buffer;

		private bool _chunked;

		private HttpListenerContext _context;

		private bool _contextWasBound;

		private StringBuilder _currentLine;

		private EndPointListener _epListener;

		private HttpConnection.InputState _inputState;

		private RequestStream _inputStream;

		private HttpListener _lastListener;

		private HttpConnection.LineState _lineState;

		private ResponseStream _outputStream;

		private int _position;

		private ListenerPrefix _prefix;

		private MemoryStream _requestBuffer;

		private int _reuses;

		private bool _secure;

		private Socket _socket;

		private Stream _stream;

		private int _timeout;

		private Timer _timer;

		public HttpConnection(Socket socket, EndPointListener listener, bool secure, X509Certificate2 cert)
		{
			this._socket = socket;
			this._epListener = listener;
			this._secure = secure;
			NetworkStream networkStream = new NetworkStream(socket, false);
			if (!secure)
			{
				this._stream = networkStream;
			}
			else
			{
				SslStream sslStream = new SslStream(networkStream, false);
				sslStream.AuthenticateAsServer(cert);
				this._stream = sslStream;
			}
			this._timeout = 90000;
			this._timer = new Timer(new TimerCallback(this.onTimeout), null, -1, -1);
			this.init();
		}

		public bool IsClosed
		{
			get
			{
				return this._socket == null;
			}
		}

		public bool IsSecure
		{
			get
			{
				return this._secure;
			}
		}

		public IPEndPoint LocalEndPoint
		{
			get
			{
				return (IPEndPoint)this._socket.LocalEndPoint;
			}
		}

		public ListenerPrefix Prefix
		{
			get
			{
				return this._prefix;
			}
			set
			{
				this._prefix = value;
			}
		}

		public IPEndPoint RemoteEndPoint
		{
			get
			{
				return (IPEndPoint)this._socket.RemoteEndPoint;
			}
		}

		public int Reuses
		{
			get
			{
				return this._reuses;
			}
		}

		public Stream Stream
		{
			get
			{
				return this._stream;
			}
		}

		private void closeSocket()
		{
			if (this._socket == null)
			{
				return;
			}
			try
			{
				this._socket.Close();
			}
			catch
			{
			}
			finally
			{
				this._socket = null;
			}
			this.removeConnection();
		}

		private void init()
		{
			this._chunked = false;
			this._context = new HttpListenerContext(this);
			this._inputState = HttpConnection.InputState.RequestLine;
			this._inputStream = null;
			this._lineState = HttpConnection.LineState.None;
			this._outputStream = null;
			this._position = 0;
			this._prefix = null;
			this._requestBuffer = new MemoryStream();
		}

		private static void onRead(IAsyncResult asyncResult)
		{
			HttpConnection httpConnection = (HttpConnection)asyncResult.AsyncState;
			httpConnection.onReadInternal(asyncResult);
		}

		private void onReadInternal(IAsyncResult asyncResult)
		{
			this._timer.Change(-1, -1);
			int num = -1;
			try
			{
				num = this._stream.EndRead(asyncResult);
				this._requestBuffer.Write(this._buffer, 0, num);
				if (this._requestBuffer.Length > 32768L)
				{
					this.SendError();
					this.Close(true);
					return;
				}
			}
			catch
			{
				if (this._requestBuffer != null && this._requestBuffer.Length > 0L)
				{
					this.SendError();
				}
				if (this._socket != null)
				{
					this.closeSocket();
					this.unbind();
				}
				return;
			}
			if (num <= 0)
			{
				this.closeSocket();
				this.unbind();
				return;
			}
			if (this.processInput(this._requestBuffer.GetBuffer()))
			{
				if (this._context.HaveError)
				{
					this.SendError();
					this.Close(true);
					return;
				}
				this._context.Request.FinishInitialization();
				if (!this._epListener.BindContext(this._context))
				{
					this.SendError("Invalid host", 400);
					this.Close(true);
					return;
				}
				HttpListener listener = this._context.Listener;
				if (this._lastListener != listener)
				{
					this.removeConnection();
					listener.AddConnection(this);
					this._lastListener = listener;
				}
				this._contextWasBound = true;
				listener.RegisterContext(this._context);
				return;
			}
			else
			{
				this._stream.BeginRead(this._buffer, 0, 8192, new AsyncCallback(HttpConnection.onRead), this);
			}
		}

		private void onTimeout(object unused)
		{
			this.closeSocket();
			this.unbind();
		}

		private bool processInput(byte[] data)
		{
			int num = data.Length;
			int num2 = 0;
			try
			{
				string text;
				while ((text = this.readLine(data, this._position, num - this._position, ref num2)) != null)
				{
					this._position += num2;
					if (text.Length == 0)
					{
						if (this._inputState != HttpConnection.InputState.RequestLine)
						{
							this._currentLine = null;
							return true;
						}
					}
					else
					{
						if (this._inputState == HttpConnection.InputState.RequestLine)
						{
							this._context.Request.SetRequestLine(text);
							this._inputState = HttpConnection.InputState.Headers;
						}
						else
						{
							this._context.Request.AddHeader(text);
						}
						if (this._context.HaveError)
						{
							return true;
						}
					}
				}
			}
			catch (Exception ex)
			{
				this._context.ErrorMessage = ex.Message;
				return true;
			}
			this._position += num2;
			if (num2 == num)
			{
				this._requestBuffer.SetLength(0L);
				this._position = 0;
			}
			return false;
		}

		private string readLine(byte[] buffer, int offset, int length, ref int used)
		{
			if (this._currentLine == null)
			{
				this._currentLine = new StringBuilder();
			}
			int num = offset + length;
			used = 0;
			int num2 = offset;
			while (num2 < num && this._lineState != HttpConnection.LineState.LF)
			{
				used++;
				byte b = buffer[num2];
				if (b == 13)
				{
					this._lineState = HttpConnection.LineState.CR;
				}
				else if (b == 10)
				{
					this._lineState = HttpConnection.LineState.LF;
				}
				else
				{
					this._currentLine.Append((char)b);
				}
				num2++;
			}
			string result = null;
			if (this._lineState == HttpConnection.LineState.LF)
			{
				this._lineState = HttpConnection.LineState.None;
				result = this._currentLine.ToString();
				this._currentLine.Length = 0;
			}
			return result;
		}

		private void removeConnection()
		{
			if (this._lastListener == null)
			{
				this._epListener.RemoveConnection(this);
			}
			else
			{
				this._lastListener.RemoveConnection(this);
			}
		}

		private void unbind()
		{
			if (this._contextWasBound)
			{
				this._epListener.UnbindContext(this._context);
				this._contextWasBound = false;
			}
		}

		internal void Close(bool force)
		{
			if (this._socket == null)
			{
				return;
			}
			if (this._outputStream != null)
			{
				this._outputStream.Close();
				this._outputStream = null;
			}
			HttpListenerRequest request = this._context.Request;
			HttpListenerResponse response = this._context.Response;
			force |= !request.KeepAlive;
			if (!force)
			{
				force = (response.Headers["Connection"] == "close");
			}
			if (!force && request.FlushInput() && (!this._chunked || (this._chunked && !response.ForceCloseChunked)))
			{
				this._reuses++;
				this.unbind();
				this.init();
				this.BeginReadRequest();
				return;
			}
			Socket socket = this._socket;
			this._socket = null;
			try
			{
				socket.Shutdown(SocketShutdown.Both);
			}
			catch
			{
			}
			finally
			{
				if (socket != null)
				{
					socket.Close();
				}
			}
			this.unbind();
			this.removeConnection();
		}

		public void BeginReadRequest()
		{
			if (this._buffer == null)
			{
				this._buffer = new byte[8192];
			}
			try
			{
				if (this._reuses == 1)
				{
					this._timeout = 15000;
				}
				this._timer.Change(this._timeout, -1);
				this._stream.BeginRead(this._buffer, 0, 8192, new AsyncCallback(HttpConnection.onRead), this);
			}
			catch
			{
				this._timer.Change(-1, -1);
				this.closeSocket();
				this.unbind();
			}
		}

		public void Close()
		{
			this.Close(false);
		}

		public RequestStream GetRequestStream(bool chunked, long contentlength)
		{
			if (this._inputStream == null)
			{
				byte[] buffer = this._requestBuffer.GetBuffer();
				int num = buffer.Length;
				this._requestBuffer = null;
				if (chunked)
				{
					this._chunked = true;
					this._context.Response.SendChunked = true;
					this._inputStream = new ChunkedInputStream(this._context, this._stream, buffer, this._position, num - this._position);
				}
				else
				{
					this._inputStream = new RequestStream(this._stream, buffer, this._position, num - this._position, contentlength);
				}
			}
			return this._inputStream;
		}

		public ResponseStream GetResponseStream()
		{
			if (this._outputStream == null)
			{
				HttpListener listener = this._context.Listener;
				bool ignoreErrors = listener == null || listener.IgnoreWriteExceptions;
				this._outputStream = new ResponseStream(this._stream, this._context.Response, ignoreErrors);
			}
			return this._outputStream;
		}

		public void SendError()
		{
			this.SendError(this._context.ErrorMessage, this._context.ErrorStatus);
		}

		public void SendError(string message, int status)
		{
			try
			{
				HttpListenerResponse response = this._context.Response;
				response.StatusCode = status;
				response.ContentType = "text/html";
				string statusDescription = status.GetStatusDescription();
				string s = (message == null || message.Length <= 0) ? string.Format("<h1>{0}</h1>", statusDescription) : string.Format("<h1>{0} ({1})</h1>", statusDescription, message);
				byte[] bytes = response.ContentEncoding.GetBytes(s);
				response.Close(bytes, false);
			}
			catch
			{
			}
		}

		private enum InputState
		{
			RequestLine,
			Headers
		}

		private enum LineState
		{
			None,
			CR,
			LF
		}
	}
}
