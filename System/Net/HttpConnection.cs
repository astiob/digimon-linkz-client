using Mono.Security.Protocol.Tls;
using System;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace System.Net
{
	internal sealed class HttpConnection
	{
		private const int BufferSize = 8192;

		private System.Net.Sockets.Socket sock;

		private Stream stream;

		private EndPointListener epl;

		private MemoryStream ms;

		private byte[] buffer;

		private HttpListenerContext context;

		private StringBuilder current_line;

		private ListenerPrefix prefix;

		private RequestStream i_stream;

		private ResponseStream o_stream;

		private bool chunked;

		private int chunked_uses;

		private bool context_bound;

		private bool secure;

		private AsymmetricAlgorithm key;

		private HttpConnection.InputState input_state;

		private HttpConnection.LineState line_state;

		private int position;

		public HttpConnection(System.Net.Sockets.Socket sock, EndPointListener epl, bool secure, System.Security.Cryptography.X509Certificates.X509Certificate2 cert, AsymmetricAlgorithm key)
		{
			this.sock = sock;
			this.epl = epl;
			this.secure = secure;
			this.key = key;
			if (!secure)
			{
				this.stream = new System.Net.Sockets.NetworkStream(sock, false);
			}
			else
			{
				SslServerStream sslServerStream = new SslServerStream(new System.Net.Sockets.NetworkStream(sock, false), cert, false, false);
				SslServerStream sslServerStream2 = sslServerStream;
				sslServerStream2.PrivateKeyCertSelectionDelegate = (PrivateKeySelectionCallback)Delegate.Combine(sslServerStream2.PrivateKeyCertSelectionDelegate, new PrivateKeySelectionCallback(this.OnPVKSelection));
				this.stream = sslServerStream;
			}
			this.Init();
		}

		private AsymmetricAlgorithm OnPVKSelection(X509Certificate certificate, string targetHost)
		{
			return this.key;
		}

		private void Init()
		{
			this.context_bound = false;
			this.i_stream = null;
			this.o_stream = null;
			this.prefix = null;
			this.chunked = false;
			this.ms = new MemoryStream();
			this.position = 0;
			this.input_state = HttpConnection.InputState.RequestLine;
			this.line_state = HttpConnection.LineState.None;
			this.context = new HttpListenerContext(this);
		}

		public int ChunkedUses
		{
			get
			{
				return this.chunked_uses;
			}
		}

		public IPEndPoint LocalEndPoint
		{
			get
			{
				return (IPEndPoint)this.sock.LocalEndPoint;
			}
		}

		public IPEndPoint RemoteEndPoint
		{
			get
			{
				return (IPEndPoint)this.sock.RemoteEndPoint;
			}
		}

		public bool IsSecure
		{
			get
			{
				return this.secure;
			}
		}

		public ListenerPrefix Prefix
		{
			get
			{
				return this.prefix;
			}
			set
			{
				this.prefix = value;
			}
		}

		public void BeginReadRequest()
		{
			if (this.buffer == null)
			{
				this.buffer = new byte[8192];
			}
			try
			{
				this.stream.BeginRead(this.buffer, 0, 8192, new AsyncCallback(this.OnRead), this);
			}
			catch
			{
				this.CloseSocket();
			}
		}

		public RequestStream GetRequestStream(bool chunked, long contentlength)
		{
			if (this.i_stream == null)
			{
				byte[] array = this.ms.GetBuffer();
				int num = (int)this.ms.Length;
				this.ms = null;
				if (chunked)
				{
					this.chunked = true;
					this.context.Response.SendChunked = true;
					this.i_stream = new ChunkedInputStream(this.context, this.stream, array, this.position, num - this.position);
				}
				else
				{
					this.i_stream = new RequestStream(this.stream, array, this.position, num - this.position, contentlength);
				}
			}
			return this.i_stream;
		}

		public ResponseStream GetResponseStream()
		{
			if (this.o_stream == null)
			{
				HttpListener listener = this.context.Listener;
				bool ignore_errors = listener == null || listener.IgnoreWriteExceptions;
				this.o_stream = new ResponseStream(this.stream, this.context.Response, ignore_errors);
			}
			return this.o_stream;
		}

		private void OnRead(IAsyncResult ares)
		{
			HttpConnection state = (HttpConnection)ares.AsyncState;
			int num = -1;
			try
			{
				num = this.stream.EndRead(ares);
				this.ms.Write(this.buffer, 0, num);
				if (this.ms.Length > 32768L)
				{
					this.SendError("Bad request", 400);
					this.Close(true);
					return;
				}
			}
			catch
			{
				if (this.ms != null && this.ms.Length > 0L)
				{
					this.SendError();
				}
				if (this.sock != null)
				{
					this.CloseSocket();
				}
				return;
			}
			if (num == 0)
			{
				this.CloseSocket();
				return;
			}
			if (this.ProcessInput(this.ms))
			{
				if (!this.context.HaveError)
				{
					this.context.Request.FinishInitialization();
				}
				if (this.context.HaveError)
				{
					this.SendError();
					this.Close(true);
					return;
				}
				if (!this.epl.BindContext(this.context))
				{
					this.SendError("Invalid host", 400);
					this.Close(true);
				}
				this.context_bound = true;
				return;
			}
			else
			{
				this.stream.BeginRead(this.buffer, 0, 8192, new AsyncCallback(this.OnRead), state);
			}
		}

		private bool ProcessInput(MemoryStream ms)
		{
			byte[] array = ms.GetBuffer();
			int num = (int)ms.Length;
			int num2 = 0;
			string text;
			try
			{
				text = this.ReadLine(array, this.position, num - this.position, ref num2);
				this.position += num2;
			}
			catch (Exception ex)
			{
				this.context.ErrorMessage = "Bad request";
				this.context.ErrorStatus = 400;
				return true;
			}
			while (text != null)
			{
				if (text == string.Empty)
				{
					if (this.input_state != HttpConnection.InputState.RequestLine)
					{
						this.current_line = null;
						ms = null;
						return true;
					}
				}
				else
				{
					if (this.input_state == HttpConnection.InputState.RequestLine)
					{
						this.context.Request.SetRequestLine(text);
						this.input_state = HttpConnection.InputState.Headers;
					}
					else
					{
						try
						{
							this.context.Request.AddHeader(text);
						}
						catch (Exception ex2)
						{
							this.context.ErrorMessage = ex2.Message;
							this.context.ErrorStatus = 400;
							return true;
						}
					}
					if (this.context.HaveError)
					{
						return true;
					}
					if (this.position >= num)
					{
						break;
					}
					try
					{
						text = this.ReadLine(array, this.position, num - this.position, ref num2);
						this.position += num2;
					}
					catch (Exception ex3)
					{
						this.context.ErrorMessage = "Bad request";
						this.context.ErrorStatus = 400;
						return true;
					}
				}
				if (text != null)
				{
					continue;
				}
				IL_194:
				if (num2 == num)
				{
					ms.SetLength(0L);
					this.position = 0;
				}
				return false;
			}
			goto IL_194;
		}

		private string ReadLine(byte[] buffer, int offset, int len, ref int used)
		{
			if (this.current_line == null)
			{
				this.current_line = new StringBuilder();
			}
			int num = offset + len;
			used = 0;
			int num2 = offset;
			while (num2 < num && this.line_state != HttpConnection.LineState.LF)
			{
				used++;
				byte b = buffer[num2];
				if (b == 13)
				{
					this.line_state = HttpConnection.LineState.CR;
				}
				else if (b == 10)
				{
					this.line_state = HttpConnection.LineState.LF;
				}
				else
				{
					this.current_line.Append((char)b);
				}
				num2++;
			}
			string result = null;
			if (this.line_state == HttpConnection.LineState.LF)
			{
				this.line_state = HttpConnection.LineState.None;
				result = this.current_line.ToString();
				this.current_line.Length = 0;
			}
			return result;
		}

		public void SendError(string msg, int status)
		{
			try
			{
				HttpListenerResponse response = this.context.Response;
				response.StatusCode = status;
				response.ContentType = "text/html";
				string statusDescription = HttpListenerResponse.GetStatusDescription(status);
				string s;
				if (msg != null)
				{
					s = string.Format("<h1>{0} ({1})</h1>", statusDescription, msg);
				}
				else
				{
					s = string.Format("<h1>{0}</h1>", statusDescription);
				}
				byte[] bytes = this.context.Response.ContentEncoding.GetBytes(s);
				response.Close(bytes, false);
			}
			catch
			{
			}
		}

		public void SendError()
		{
			this.SendError(this.context.ErrorMessage, this.context.ErrorStatus);
		}

		private void Unbind()
		{
			if (this.context_bound)
			{
				this.epl.UnbindContext(this.context);
				this.context_bound = false;
			}
		}

		public void Close()
		{
			this.Close(false);
		}

		private void CloseSocket()
		{
			if (this.sock == null)
			{
				return;
			}
			try
			{
				this.sock.Close();
			}
			catch
			{
			}
			finally
			{
				this.sock = null;
			}
		}

		internal void Close(bool force_close)
		{
			if (this.sock != null)
			{
				Stream responseStream = this.GetResponseStream();
				responseStream.Close();
				this.o_stream = null;
			}
			if (this.sock == null)
			{
				return;
			}
			force_close |= (this.context.Request.Headers["connection"] == "close");
			if (!force_close)
			{
				int statusCode = this.context.Response.StatusCode;
				bool flag = statusCode == 400 || statusCode == 408 || statusCode == 411 || statusCode == 413 || statusCode == 414 || statusCode == 500 || statusCode == 503;
				force_close |= (this.context.Request.ProtocolVersion <= HttpVersion.Version10);
			}
			if (force_close || !this.context.Request.FlushInput())
			{
				System.Net.Sockets.Socket socket = this.sock;
				this.sock = null;
				try
				{
					if (socket != null)
					{
						socket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
					}
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
				this.Unbind();
				return;
			}
			if (this.chunked && !this.context.Response.ForceCloseChunked)
			{
				this.chunked_uses++;
				this.Unbind();
				this.Init();
				this.BeginReadRequest();
				return;
			}
			this.Unbind();
			this.Init();
			this.BeginReadRequest();
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
