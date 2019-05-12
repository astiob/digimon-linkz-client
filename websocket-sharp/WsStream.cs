using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using WebSocketSharp.Net;
using WebSocketSharp.Net.Security;

namespace WebSocketSharp
{
	internal class WsStream : IDisposable
	{
		private const int _handshakeHeadersLimitLen = 8192;

		private object _forWrite;

		private Stream _innerStream;

		private bool _secure;

		private WsStream(Stream innerStream, bool secure)
		{
			this._innerStream = innerStream;
			this._secure = secure;
			this._forWrite = new object();
		}

		internal WsStream(NetworkStream innerStream) : this(innerStream, false)
		{
		}

		internal WsStream(WebSocketSharp.Net.Security.SslStream innerStream) : this(innerStream, true)
		{
		}

		public bool DataAvailable
		{
			get
			{
				return (!this._secure) ? ((NetworkStream)this._innerStream).DataAvailable : ((WebSocketSharp.Net.Security.SslStream)this._innerStream).DataAvailable;
			}
		}

		public bool IsSecure
		{
			get
			{
				return this._secure;
			}
		}

		private static byte[] readHandshakeEntityBody(Stream stream, string length)
		{
			long num = long.Parse(length);
			return (num <= 1024L) ? stream.ReadBytes((int)num) : stream.ReadBytes(num, 1024);
		}

		private static string[] readHandshakeHeaders(Stream stream)
		{
			List<byte> buffer = new List<byte>();
			int count = 0;
			Action<int> action = delegate(int i)
			{
				buffer.Add((byte)i);
				count++;
			};
			bool flag = false;
			while (count < 8192)
			{
				if (stream.ReadByte().EqualsWith('\r', action) && stream.ReadByte().EqualsWith('\n', action) && stream.ReadByte().EqualsWith('\r', action) && stream.ReadByte().EqualsWith('\n', action))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				throw new WebSocketException("The header part of a handshake is greater than the limit length.");
			}
			string text = "\r\n";
			return Encoding.UTF8.GetString(buffer.ToArray()).Replace(text + " ", " ").Replace(text + "\t", " ").Split(new string[]
			{
				text
			}, StringSplitOptions.RemoveEmptyEntries);
		}

		internal static WsStream CreateClientStream(TcpClient client, bool secure, string host, RemoteCertificateValidationCallback validationCallback)
		{
			NetworkStream stream = client.GetStream();
			if (secure)
			{
				if (validationCallback == null)
				{
					validationCallback = ((object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true);
				}
				WebSocketSharp.Net.Security.SslStream sslStream = new WebSocketSharp.Net.Security.SslStream(stream, false, validationCallback);
				sslStream.AuthenticateAsClient(host);
				return new WsStream(sslStream);
			}
			return new WsStream(stream);
		}

		internal static WsStream CreateServerStream(TcpClient client, X509Certificate cert, bool secure)
		{
			NetworkStream stream = client.GetStream();
			if (secure)
			{
				WebSocketSharp.Net.Security.SslStream sslStream = new WebSocketSharp.Net.Security.SslStream(stream, false);
				sslStream.AuthenticateAsServer(cert);
				return new WsStream(sslStream);
			}
			return new WsStream(stream);
		}

		internal static WsStream CreateServerStream(HttpListenerContext context)
		{
			HttpConnection connection = context.Connection;
			return new WsStream(connection.Stream, connection.IsSecure);
		}

		internal T ReadHandshake<T>(Func<string[], T> parser, int millisecondsTimeout) where T : HandshakeBase
		{
			bool timeout = false;
			Timer timer = new Timer(delegate(object state)
			{
				timeout = true;
				this._innerStream.Close();
			}, null, millisecondsTimeout, -1);
			T result = (T)((object)null);
			Exception ex = null;
			try
			{
				result = parser(WsStream.readHandshakeHeaders(this._innerStream));
				string text = result.Headers["Content-Length"];
				if (text != null && text.Length > 0)
				{
					result.EntityBodyData = WsStream.readHandshakeEntityBody(this._innerStream, text);
				}
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			finally
			{
				timer.Change(-1, -1);
				timer.Dispose();
			}
			string text2 = (!timeout) ? ((ex == null) ? null : "An exception has occurred while receiving a handshake.") : "A timeout has occurred while receiving a handshake.";
			if (text2 != null)
			{
				throw new WebSocketException(text2, ex);
			}
			return result;
		}

		internal bool Write(byte[] data)
		{
			object forWrite = this._forWrite;
			bool result;
			lock (forWrite)
			{
				try
				{
					this._innerStream.Write(data, 0, data.Length);
					result = true;
				}
				catch
				{
					result = false;
				}
			}
			return result;
		}

		public void Close()
		{
			this._innerStream.Close();
		}

		public void Dispose()
		{
			this._innerStream.Dispose();
		}

		public WsFrame ReadFrame()
		{
			return WsFrame.Parse(this._innerStream, true);
		}

		public void ReadFrameAsync(Action<WsFrame> completed, Action<Exception> error)
		{
			WsFrame.ParseAsync(this._innerStream, true, completed, error);
		}

		public HandshakeRequest ReadHandshakeRequest()
		{
			return this.ReadHandshake<HandshakeRequest>(new Func<string[], HandshakeRequest>(HandshakeRequest.Parse), 90000);
		}

		public HandshakeResponse ReadHandshakeResponse()
		{
			return this.ReadHandshake<HandshakeResponse>(new Func<string[], HandshakeResponse>(HandshakeResponse.Parse), 90000);
		}

		public bool WriteFrame(WsFrame frame)
		{
			return this.Write(frame.ToByteArray());
		}

		public bool WriteHandshake(HandshakeBase handshake)
		{
			return this.Write(handshake.ToByteArray());
		}
	}
}
