using Mono.Security.Protocol.Tls;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace System.Net
{
	internal class WebConnection
	{
		private ServicePoint sPoint;

		private Stream nstream;

		private System.Net.Sockets.Socket socket;

		private object socketLock = new object();

		private WebExceptionStatus status;

		private WaitCallback initConn;

		private bool keepAlive;

		private byte[] buffer;

		private static AsyncCallback readDoneDelegate = new AsyncCallback(WebConnection.ReadDone);

		private EventHandler abortHandler;

		private WebConnection.AbortHelper abortHelper;

		private ReadState readState;

		internal WebConnectionData Data;

		private bool chunkedRead;

		private ChunkStream chunkStream;

		private Queue queue;

		private bool reused;

		private int position;

		private bool busy;

		private HttpWebRequest priority_request;

		private NetworkCredential ntlm_credentials;

		private bool ntlm_authenticated;

		private bool unsafe_sharing;

		private bool ssl;

		private bool certsAvailable;

		private Exception connect_exception;

		private static object classLock = new object();

		private static Type sslStream;

		private static PropertyInfo piClient;

		private static PropertyInfo piServer;

		private static PropertyInfo piTrustFailure;

		private static MethodInfo method_GetSecurityPolicyFromNonMainThread;

		public WebConnection(WebConnectionGroup group, ServicePoint sPoint)
		{
			this.sPoint = sPoint;
			this.buffer = new byte[4096];
			this.readState = ReadState.None;
			this.Data = new WebConnectionData();
			this.initConn = new WaitCallback(this.InitConnection);
			this.queue = group.Queue;
			this.abortHelper = new WebConnection.AbortHelper();
			this.abortHelper.Connection = this;
			this.abortHandler = new EventHandler(this.abortHelper.Abort);
		}

		private bool CanReuse()
		{
			return !this.socket.Poll(0, System.Net.Sockets.SelectMode.SelectRead);
		}

		private void LoggedThrow(Exception e)
		{
			Console.WriteLine("Throwing this exception: " + e);
			throw e;
		}

		internal static Stream DownloadPolicy(string url, string proxy)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			if (proxy != null)
			{
				httpWebRequest.Proxy = new WebProxy(proxy);
			}
			return httpWebRequest.GetResponse().GetResponseStream();
		}

		private void CheckUnityWebSecurity(HttpWebRequest request)
		{
			if (!Environment.SocketSecurityEnabled)
			{
				return;
			}
			Console.WriteLine("CheckingSecurityForUrl: " + request.RequestUri.AbsoluteUri);
			System.Uri requestUri = request.RequestUri;
			string text = string.Empty;
			if (!requestUri.IsDefaultPort)
			{
				text = ":" + requestUri.Port;
			}
			if (requestUri.ToString() == string.Concat(new string[]
			{
				requestUri.Scheme,
				"://",
				requestUri.Host,
				text,
				"/crossdomain.xml"
			}))
			{
				return;
			}
			try
			{
				if (WebConnection.method_GetSecurityPolicyFromNonMainThread == null)
				{
					Type type = Type.GetType("UnityEngine.UnityCrossDomainHelper, CrossDomainPolicyParser, Version=1.0.0.0, Culture=neutral");
					if (type == null)
					{
						this.LoggedThrow(new SecurityException("Cant find type UnityCrossDomainHelper"));
					}
					WebConnection.method_GetSecurityPolicyFromNonMainThread = type.GetMethod("GetSecurityPolicyForDotNetWebRequest");
					if (WebConnection.method_GetSecurityPolicyFromNonMainThread == null)
					{
						this.LoggedThrow(new SecurityException("Cant find GetSecurityPolicyFromNonMainThread"));
					}
				}
				MethodInfo method = typeof(WebConnection).GetMethod("DownloadPolicy", BindingFlags.Static | BindingFlags.NonPublic);
				if (method == null)
				{
					this.LoggedThrow(new SecurityException("Cannot find method DownloadPolicy"));
				}
				if (!(bool)WebConnection.method_GetSecurityPolicyFromNonMainThread.Invoke(null, new object[]
				{
					request.RequestUri.ToString(),
					method
				}))
				{
					this.LoggedThrow(new SecurityException("Webrequest was denied"));
				}
			}
			catch (Exception arg)
			{
				this.LoggedThrow(new SecurityException("Unexpected error while trying to call method_GetSecurityPolicyBlocking : " + arg));
			}
		}

		private void Connect(HttpWebRequest request)
		{
			object obj = this.socketLock;
			lock (obj)
			{
				if (this.socket != null && this.socket.Connected && this.status == WebExceptionStatus.Success && this.CanReuse() && this.CompleteChunkedRead())
				{
					this.reused = true;
				}
				else
				{
					this.reused = false;
					if (this.socket != null)
					{
						this.socket.Close();
						this.socket = null;
					}
					this.chunkStream = null;
					IPHostEntry hostEntry = this.sPoint.HostEntry;
					if (hostEntry == null)
					{
						this.status = ((!this.sPoint.UsesProxy) ? WebExceptionStatus.NameResolutionFailure : WebExceptionStatus.ProxyNameResolutionFailure);
					}
					else
					{
						WebConnectionData data = this.Data;
						foreach (IPAddress ipaddress in hostEntry.AddressList)
						{
							this.socket = new System.Net.Sockets.Socket(ipaddress.AddressFamily, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
							IPEndPoint ipendPoint = new IPEndPoint(ipaddress, this.sPoint.Address.Port);
							this.socket.SetSocketOption(System.Net.Sockets.SocketOptionLevel.Tcp, System.Net.Sockets.SocketOptionName.Debug, (!this.sPoint.UseNagleAlgorithm) ? 1 : 0);
							this.socket.NoDelay = !this.sPoint.UseNagleAlgorithm;
							if (!this.sPoint.CallEndPointDelegate(this.socket, ipendPoint))
							{
								this.socket.Close();
								this.socket = null;
								this.status = WebExceptionStatus.ConnectFailure;
							}
							else
							{
								try
								{
									if (request.Aborted)
									{
										break;
									}
									this.CheckUnityWebSecurity(request);
									this.socket.Connect(ipendPoint, false);
									this.status = WebExceptionStatus.Success;
									break;
								}
								catch (ThreadAbortException)
								{
									System.Net.Sockets.Socket socket = this.socket;
									this.socket = null;
									if (socket != null)
									{
										socket.Close();
									}
									break;
								}
								catch (ObjectDisposedException ex)
								{
									break;
								}
								catch (Exception ex2)
								{
									System.Net.Sockets.Socket socket2 = this.socket;
									this.socket = null;
									if (socket2 != null)
									{
										socket2.Close();
									}
									if (!request.Aborted)
									{
										this.status = WebExceptionStatus.ConnectFailure;
									}
									this.connect_exception = ex2;
								}
							}
						}
					}
				}
			}
		}

		private static void EnsureSSLStreamAvailable()
		{
			object obj = WebConnection.classLock;
			lock (obj)
			{
				if (WebConnection.sslStream == null)
				{
					WebConnection.sslStream = typeof(HttpsClientStream);
					WebConnection.piClient = WebConnection.sslStream.GetProperty("SelectedClientCertificate");
					WebConnection.piServer = WebConnection.sslStream.GetProperty("ServerCertificate");
					WebConnection.piTrustFailure = WebConnection.sslStream.GetProperty("TrustFailure");
				}
			}
		}

		private bool CreateTunnel(HttpWebRequest request, Stream stream, out byte[] buffer)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("CONNECT ");
			stringBuilder.Append(request.Address.Host);
			stringBuilder.Append(':');
			stringBuilder.Append(request.Address.Port);
			stringBuilder.Append(" HTTP/");
			if (request.ServicePoint.ProtocolVersion == HttpVersion.Version11)
			{
				stringBuilder.Append("1.1");
			}
			else
			{
				stringBuilder.Append("1.0");
			}
			stringBuilder.Append("\r\nHost: ");
			stringBuilder.Append(request.Address.Authority);
			string challenge = this.Data.Challenge;
			this.Data.Challenge = null;
			bool flag = request.Headers["Proxy-Authorization"] != null;
			if (flag)
			{
				stringBuilder.Append("\r\nProxy-Authorization: ");
				stringBuilder.Append(request.Headers["Proxy-Authorization"]);
			}
			else if (challenge != null && this.Data.StatusCode == 407)
			{
				flag = true;
				ICredentials credentials = request.Proxy.Credentials;
				Authorization authorization = AuthenticationManager.Authenticate(challenge, request, credentials);
				if (authorization != null)
				{
					stringBuilder.Append("\r\nProxy-Authorization: ");
					stringBuilder.Append(authorization.Message);
				}
			}
			stringBuilder.Append("\r\n\r\n");
			this.Data.StatusCode = 0;
			byte[] bytes = Encoding.Default.GetBytes(stringBuilder.ToString());
			stream.Write(bytes, 0, bytes.Length);
			int num;
			WebHeaderCollection webHeaderCollection = this.ReadHeaders(request, stream, out buffer, out num);
			if (!flag && webHeaderCollection != null && num == 407)
			{
				this.Data.StatusCode = num;
				this.Data.Challenge = webHeaderCollection["Proxy-Authenticate"];
				return false;
			}
			if (num != 200)
			{
				string where = string.Format("The remote server returned a {0} status code.", num);
				this.HandleError(WebExceptionStatus.SecureChannelFailure, null, where);
				return false;
			}
			return webHeaderCollection != null;
		}

		private WebHeaderCollection ReadHeaders(HttpWebRequest request, Stream stream, out byte[] retBuffer, out int status)
		{
			retBuffer = null;
			status = 200;
			byte[] array = new byte[1024];
			MemoryStream memoryStream = new MemoryStream();
			bool flag = false;
			int num2;
			WebHeaderCollection webHeaderCollection;
			for (;;)
			{
				int num = stream.Read(array, 0, 1024);
				if (num == 0)
				{
					break;
				}
				memoryStream.Write(array, 0, num);
				num2 = 0;
				string text = null;
				webHeaderCollection = new WebHeaderCollection();
				while (WebConnection.ReadLine(memoryStream.GetBuffer(), ref num2, (int)memoryStream.Length, ref text))
				{
					if (text == null)
					{
						goto Block_2;
					}
					if (flag)
					{
						webHeaderCollection.Add(text);
					}
					else
					{
						int num3 = text.IndexOf(' ');
						if (num3 == -1)
						{
							goto Block_5;
						}
						status = (int)uint.Parse(text.Substring(num3 + 1, 3));
						flag = true;
					}
				}
			}
			this.HandleError(WebExceptionStatus.ServerProtocolViolation, null, "ReadHeaders");
			return null;
			Block_2:
			if (memoryStream.Length - (long)num2 > 0L)
			{
				retBuffer = new byte[memoryStream.Length - (long)num2];
				Buffer.BlockCopy(memoryStream.GetBuffer(), num2, retBuffer, 0, retBuffer.Length);
			}
			return webHeaderCollection;
			Block_5:
			this.HandleError(WebExceptionStatus.ServerProtocolViolation, null, "ReadHeaders2");
			return null;
		}

		private bool CreateStream(HttpWebRequest request)
		{
			try
			{
				System.Net.Sockets.NetworkStream networkStream = new System.Net.Sockets.NetworkStream(this.socket, false);
				if (request.Address.Scheme == System.Uri.UriSchemeHttps)
				{
					this.ssl = true;
					WebConnection.EnsureSSLStreamAvailable();
					if (!this.reused || this.nstream == null || this.nstream.GetType() != WebConnection.sslStream)
					{
						byte[] array = null;
						if (this.sPoint.UseConnect && !this.CreateTunnel(request, networkStream, out array))
						{
							return false;
						}
						object[] args = new object[]
						{
							networkStream,
							request.ClientCertificates,
							request,
							array
						};
						this.nstream = (Stream)Activator.CreateInstance(WebConnection.sslStream, args);
						SslClientStream sslClientStream = (SslClientStream)this.nstream;
						ServicePointManager.ChainValidationHelper @object = new ServicePointManager.ChainValidationHelper(request);
						sslClientStream.ServerCertValidation2 += @object.ValidateChain;
						this.certsAvailable = false;
					}
				}
				else
				{
					this.ssl = false;
					this.nstream = networkStream;
				}
			}
			catch (Exception)
			{
				if (!request.Aborted)
				{
					this.status = WebExceptionStatus.ConnectFailure;
				}
				return false;
			}
			return true;
		}

		private void HandleError(WebExceptionStatus st, Exception e, string where)
		{
			this.status = st;
			lock (this)
			{
				if (st == WebExceptionStatus.RequestCanceled)
				{
					this.Data = new WebConnectionData();
				}
			}
			if (e == null)
			{
				try
				{
					throw new Exception(new StackTrace().ToString());
				}
				catch (Exception ex)
				{
					e = ex;
				}
			}
			HttpWebRequest httpWebRequest = null;
			if (this.Data != null && this.Data.request != null)
			{
				httpWebRequest = this.Data.request;
			}
			this.Close(true);
			if (httpWebRequest != null)
			{
				httpWebRequest.FinishedReading = true;
				httpWebRequest.SetResponseError(st, e, where);
			}
		}

		private static void ReadDone(IAsyncResult result)
		{
			WebConnection webConnection = (WebConnection)result.AsyncState;
			WebConnectionData data = webConnection.Data;
			Stream stream = webConnection.nstream;
			if (stream == null)
			{
				webConnection.Close(true);
				return;
			}
			int num = -1;
			try
			{
				num = stream.EndRead(result);
			}
			catch (Exception e)
			{
				webConnection.HandleError(WebExceptionStatus.ReceiveFailure, e, "ReadDone1");
				return;
			}
			if (num == 0)
			{
				webConnection.HandleError(WebExceptionStatus.ReceiveFailure, null, "ReadDone2");
				return;
			}
			if (num < 0)
			{
				webConnection.HandleError(WebExceptionStatus.ServerProtocolViolation, null, "ReadDone3");
				return;
			}
			int num2 = -1;
			num += webConnection.position;
			if (webConnection.readState == ReadState.None)
			{
				Exception ex = null;
				try
				{
					num2 = webConnection.GetResponse(webConnection.buffer, num);
				}
				catch (Exception ex2)
				{
					ex = ex2;
				}
				if (ex != null)
				{
					webConnection.HandleError(WebExceptionStatus.ServerProtocolViolation, ex, "ReadDone4");
					return;
				}
			}
			if (webConnection.readState != ReadState.Content)
			{
				int num3 = num * 2;
				int num4 = (num3 >= webConnection.buffer.Length) ? num3 : webConnection.buffer.Length;
				byte[] dst = new byte[num4];
				Buffer.BlockCopy(webConnection.buffer, 0, dst, 0, num);
				webConnection.buffer = dst;
				webConnection.position = num;
				webConnection.readState = ReadState.None;
				WebConnection.InitRead(webConnection);
				return;
			}
			webConnection.position = 0;
			WebConnectionStream webConnectionStream = new WebConnectionStream(webConnection);
			string text = data.Headers["Transfer-Encoding"];
			webConnection.chunkedRead = (text != null && text.ToLower().IndexOf("chunked") != -1);
			if (!webConnection.chunkedRead)
			{
				webConnectionStream.ReadBuffer = webConnection.buffer;
				webConnectionStream.ReadBufferOffset = num2;
				webConnectionStream.ReadBufferSize = num;
				webConnectionStream.CheckResponseInBuffer();
			}
			else if (webConnection.chunkStream == null)
			{
				try
				{
					webConnection.chunkStream = new ChunkStream(webConnection.buffer, num2, num, data.Headers);
				}
				catch (Exception e2)
				{
					webConnection.HandleError(WebExceptionStatus.ServerProtocolViolation, e2, "ReadDone5");
					return;
				}
			}
			else
			{
				webConnection.chunkStream.ResetBuffer();
				try
				{
					webConnection.chunkStream.Write(webConnection.buffer, num2, num);
				}
				catch (Exception e3)
				{
					webConnection.HandleError(WebExceptionStatus.ServerProtocolViolation, e3, "ReadDone6");
					return;
				}
			}
			data.stream = webConnectionStream;
			if (!WebConnection.ExpectContent(data.StatusCode) || data.request.Method == "HEAD")
			{
				webConnectionStream.ForceCompletion();
			}
			data.request.SetResponseData(data);
		}

		private static bool ExpectContent(int statusCode)
		{
			return statusCode >= 200 && statusCode != 204 && statusCode != 304;
		}

		internal void GetCertificates()
		{
			X509Certificate client = (X509Certificate)WebConnection.piClient.GetValue(this.nstream, null);
			X509Certificate x509Certificate = (X509Certificate)WebConnection.piServer.GetValue(this.nstream, null);
			this.sPoint.SetCertificates(client, x509Certificate);
			this.certsAvailable = (x509Certificate != null);
		}

		internal static void InitRead(object state)
		{
			WebConnection webConnection = (WebConnection)state;
			Stream stream = webConnection.nstream;
			try
			{
				int count = webConnection.buffer.Length - webConnection.position;
				stream.BeginRead(webConnection.buffer, webConnection.position, count, WebConnection.readDoneDelegate, webConnection);
			}
			catch (Exception e)
			{
				webConnection.HandleError(WebExceptionStatus.ReceiveFailure, e, "InitRead");
			}
		}

		private int GetResponse(byte[] buffer, int max)
		{
			int num = 0;
			string text = null;
			bool flag = false;
			bool flag2 = false;
			for (;;)
			{
				if (this.readState != ReadState.None)
				{
					goto IL_114;
				}
				if (!WebConnection.ReadLine(buffer, ref num, max, ref text))
				{
					break;
				}
				if (text == null)
				{
					flag2 = true;
				}
				else
				{
					flag2 = false;
					this.readState = ReadState.Status;
					string[] array = text.Split(new char[]
					{
						' '
					});
					if (array.Length < 2)
					{
						return -1;
					}
					if (string.Compare(array[0], "HTTP/1.1", true) == 0)
					{
						this.Data.Version = HttpVersion.Version11;
						this.sPoint.SetVersion(HttpVersion.Version11);
					}
					else
					{
						this.Data.Version = HttpVersion.Version10;
						this.sPoint.SetVersion(HttpVersion.Version10);
					}
					this.Data.StatusCode = (int)uint.Parse(array[1]);
					if (array.Length >= 3)
					{
						this.Data.StatusDescription = string.Join(" ", array, 2, array.Length - 2);
					}
					else
					{
						this.Data.StatusDescription = string.Empty;
					}
					if (num >= max)
					{
						return num;
					}
					goto IL_114;
				}
				IL_2CA:
				if (!flag2 && !flag)
				{
					return -1;
				}
				continue;
				IL_114:
				flag2 = false;
				if (this.readState != ReadState.Status)
				{
					goto IL_2CA;
				}
				this.readState = ReadState.Headers;
				this.Data.Headers = new WebHeaderCollection();
				ArrayList arrayList = new ArrayList();
				bool flag3 = false;
				while (!flag3)
				{
					if (!WebConnection.ReadLine(buffer, ref num, max, ref text))
					{
						break;
					}
					if (text == null)
					{
						flag3 = true;
					}
					else if (text.Length > 0 && (text[0] == ' ' || text[0] == '\t'))
					{
						int num2 = arrayList.Count - 1;
						if (num2 < 0)
						{
							break;
						}
						string value = (string)arrayList[num2] + text;
						arrayList[num2] = value;
					}
					else
					{
						arrayList.Add(text);
					}
				}
				if (!flag3)
				{
					return -1;
				}
				foreach (object obj in arrayList)
				{
					string @internal = (string)obj;
					this.Data.Headers.SetInternal(@internal);
				}
				if (this.Data.StatusCode != 100)
				{
					goto IL_2C1;
				}
				this.sPoint.SendContinue = true;
				if (num >= max)
				{
					return num;
				}
				if (this.Data.request.ExpectContinue)
				{
					this.Data.request.DoContinueDelegate(this.Data.StatusCode, this.Data.Headers);
					this.Data.request.ExpectContinue = false;
				}
				this.readState = ReadState.None;
				flag = true;
				goto IL_2CA;
			}
			return -1;
			IL_2C1:
			this.readState = ReadState.Content;
			return num;
		}

		private void InitConnection(object state)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)state;
			httpWebRequest.WebConnection = this;
			if (httpWebRequest.Aborted)
			{
				return;
			}
			this.keepAlive = httpWebRequest.KeepAlive;
			this.Data = new WebConnectionData();
			this.Data.request = httpWebRequest;
			WebExceptionStatus webExceptionStatus;
			for (;;)
			{
				this.Connect(httpWebRequest);
				if (httpWebRequest.Aborted)
				{
					break;
				}
				if (this.status != WebExceptionStatus.Success)
				{
					goto Block_3;
				}
				if (this.CreateStream(httpWebRequest))
				{
					goto IL_D2;
				}
				if (httpWebRequest.Aborted)
				{
					return;
				}
				webExceptionStatus = this.status;
				if (this.Data.Challenge == null)
				{
					goto IL_B4;
				}
			}
			return;
			Block_3:
			if (!httpWebRequest.Aborted)
			{
				httpWebRequest.SetWriteStreamError(this.status, this.connect_exception);
				this.Close(true);
			}
			return;
			IL_B4:
			Exception exc = this.connect_exception;
			this.connect_exception = null;
			httpWebRequest.SetWriteStreamError(webExceptionStatus, exc);
			this.Close(true);
			return;
			IL_D2:
			this.readState = ReadState.None;
			httpWebRequest.SetWriteStream(new WebConnectionStream(this, httpWebRequest));
		}

		internal EventHandler SendRequest(HttpWebRequest request)
		{
			if (request.Aborted)
			{
				return null;
			}
			lock (this)
			{
				if (!this.busy)
				{
					this.busy = true;
					this.status = WebExceptionStatus.Success;
					ThreadPool.QueueUserWorkItem(this.initConn, request);
				}
				else
				{
					Queue obj = this.queue;
					lock (obj)
					{
						this.queue.Enqueue(request);
					}
				}
			}
			return this.abortHandler;
		}

		private void SendNext()
		{
			Queue obj = this.queue;
			lock (obj)
			{
				if (this.queue.Count > 0)
				{
					this.SendRequest((HttpWebRequest)this.queue.Dequeue());
				}
			}
		}

		internal void NextRead()
		{
			lock (this)
			{
				this.Data.request.FinishedReading = true;
				string name = (!this.sPoint.UsesProxy) ? "Connection" : "Proxy-Connection";
				string text = (this.Data.Headers == null) ? null : this.Data.Headers[name];
				bool flag = this.Data.Version == HttpVersion.Version11 && this.keepAlive;
				if (text != null)
				{
					text = text.ToLower();
					flag = (this.keepAlive && text.IndexOf("keep-alive") != -1);
				}
				if ((this.socket != null && !this.socket.Connected) || !flag || (text != null && text.IndexOf("close") != -1))
				{
					this.Close(false);
				}
				this.busy = false;
				if (this.priority_request != null)
				{
					this.SendRequest(this.priority_request);
					this.priority_request = null;
				}
				else
				{
					this.SendNext();
				}
			}
		}

		private static bool ReadLine(byte[] buffer, ref int start, int max, ref string output)
		{
			bool flag = false;
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			while (start < max)
			{
				num = (int)buffer[start++];
				if (num == 10)
				{
					if (stringBuilder.Length > 0 && stringBuilder[stringBuilder.Length - 1] == '\r')
					{
						stringBuilder.Length--;
					}
					flag = false;
					break;
				}
				if (flag)
				{
					stringBuilder.Length--;
					break;
				}
				if (num == 13)
				{
					flag = true;
				}
				stringBuilder.Append((char)num);
			}
			if (num != 10 && num != 13)
			{
				return false;
			}
			if (stringBuilder.Length == 0)
			{
				output = null;
				return num == 10 || num == 13;
			}
			if (flag)
			{
				stringBuilder.Length--;
			}
			output = stringBuilder.ToString();
			return true;
		}

		internal IAsyncResult BeginRead(HttpWebRequest request, byte[] buffer, int offset, int size, AsyncCallback cb, object state)
		{
			lock (this)
			{
				if (this.Data.request != request)
				{
					throw new ObjectDisposedException(typeof(System.Net.Sockets.NetworkStream).FullName);
				}
				if (this.nstream == null)
				{
					return null;
				}
			}
			IAsyncResult asyncResult = null;
			if (this.chunkedRead)
			{
				if (!this.chunkStream.WantMore)
				{
					goto IL_9A;
				}
			}
			try
			{
				asyncResult = this.nstream.BeginRead(buffer, offset, size, cb, state);
				cb = null;
			}
			catch (Exception)
			{
				this.HandleError(WebExceptionStatus.ReceiveFailure, null, "chunked BeginRead");
				throw;
			}
			IL_9A:
			if (this.chunkedRead)
			{
				WebAsyncResult webAsyncResult = new WebAsyncResult(cb, state, buffer, offset, size);
				webAsyncResult.InnerAsyncResult = asyncResult;
				if (asyncResult == null)
				{
					webAsyncResult.SetCompleted(true, null);
					webAsyncResult.DoCallback();
				}
				return webAsyncResult;
			}
			return asyncResult;
		}

		internal int EndRead(HttpWebRequest request, IAsyncResult result)
		{
			lock (this)
			{
				if (this.Data.request != request)
				{
					throw new ObjectDisposedException(typeof(System.Net.Sockets.NetworkStream).FullName);
				}
				if (this.nstream == null)
				{
					throw new ObjectDisposedException(typeof(System.Net.Sockets.NetworkStream).FullName);
				}
			}
			int num = 0;
			WebAsyncResult webAsyncResult = null;
			IAsyncResult innerAsyncResult = ((WebAsyncResult)result).InnerAsyncResult;
			if (this.chunkedRead && innerAsyncResult is WebAsyncResult)
			{
				webAsyncResult = (WebAsyncResult)innerAsyncResult;
				IAsyncResult innerAsyncResult2 = webAsyncResult.InnerAsyncResult;
				if (innerAsyncResult2 != null && !(innerAsyncResult2 is WebAsyncResult))
				{
					num = this.nstream.EndRead(innerAsyncResult2);
				}
			}
			else if (!(innerAsyncResult is WebAsyncResult))
			{
				num = this.nstream.EndRead(innerAsyncResult);
				webAsyncResult = (WebAsyncResult)result;
			}
			if (this.chunkedRead)
			{
				bool flag = num == 0;
				try
				{
					this.chunkStream.WriteAndReadBack(webAsyncResult.Buffer, webAsyncResult.Offset, webAsyncResult.Size, ref num);
					if (!flag && num == 0 && this.chunkStream.WantMore)
					{
						num = this.EnsureRead(webAsyncResult.Buffer, webAsyncResult.Offset, webAsyncResult.Size);
					}
				}
				catch (Exception ex)
				{
					if (ex is WebException)
					{
						throw ex;
					}
					throw new WebException("Invalid chunked data.", ex, WebExceptionStatus.ServerProtocolViolation, null);
				}
				if ((flag || num == 0) && this.chunkStream.ChunkLeft != 0)
				{
					this.HandleError(WebExceptionStatus.ReceiveFailure, null, "chunked EndRead");
					throw new WebException("Read error", null, WebExceptionStatus.ReceiveFailure, null);
				}
			}
			return (num == 0) ? -1 : num;
		}

		private int EnsureRead(byte[] buffer, int offset, int size)
		{
			byte[] array = null;
			int num = 0;
			while (num == 0 && this.chunkStream.WantMore)
			{
				int num2 = this.chunkStream.ChunkLeft;
				if (num2 <= 0)
				{
					num2 = 1024;
				}
				else if (num2 > 16384)
				{
					num2 = 16384;
				}
				if (array == null || array.Length < num2)
				{
					array = new byte[num2];
				}
				int num3 = this.nstream.Read(array, 0, num2);
				if (num3 <= 0)
				{
					return 0;
				}
				this.chunkStream.Write(array, 0, num3);
				num += this.chunkStream.Read(buffer, offset + num, size - num);
			}
			return num;
		}

		private bool CompleteChunkedRead()
		{
			if (!this.chunkedRead || this.chunkStream == null)
			{
				return true;
			}
			while (this.chunkStream.WantMore)
			{
				int num = this.nstream.Read(this.buffer, 0, this.buffer.Length);
				if (num <= 0)
				{
					return false;
				}
				this.chunkStream.Write(this.buffer, 0, num);
			}
			return true;
		}

		internal IAsyncResult BeginWrite(HttpWebRequest request, byte[] buffer, int offset, int size, AsyncCallback cb, object state)
		{
			lock (this)
			{
				if (this.Data.request != request)
				{
					throw new ObjectDisposedException(typeof(System.Net.Sockets.NetworkStream).FullName);
				}
				if (this.nstream == null)
				{
					return null;
				}
			}
			IAsyncResult result = null;
			try
			{
				result = this.nstream.BeginWrite(buffer, offset, size, cb, state);
			}
			catch (Exception)
			{
				this.status = WebExceptionStatus.SendFailure;
				throw;
			}
			return result;
		}

		internal void EndWrite2(HttpWebRequest request, IAsyncResult result)
		{
			if (request.FinishedReading)
			{
				return;
			}
			lock (this)
			{
				if (this.Data.request != request)
				{
					throw new ObjectDisposedException(typeof(System.Net.Sockets.NetworkStream).FullName);
				}
				if (this.nstream == null)
				{
					throw new ObjectDisposedException(typeof(System.Net.Sockets.NetworkStream).FullName);
				}
			}
			try
			{
				this.nstream.EndWrite(result);
			}
			catch (Exception ex)
			{
				this.status = WebExceptionStatus.SendFailure;
				if (ex.InnerException != null)
				{
					throw ex.InnerException;
				}
				throw;
			}
		}

		internal bool EndWrite(HttpWebRequest request, IAsyncResult result)
		{
			if (request.FinishedReading)
			{
				return true;
			}
			lock (this)
			{
				if (this.Data.request != request)
				{
					throw new ObjectDisposedException(typeof(System.Net.Sockets.NetworkStream).FullName);
				}
				if (this.nstream == null)
				{
					throw new ObjectDisposedException(typeof(System.Net.Sockets.NetworkStream).FullName);
				}
			}
			bool result2;
			try
			{
				this.nstream.EndWrite(result);
				result2 = true;
			}
			catch
			{
				this.status = WebExceptionStatus.SendFailure;
				result2 = false;
			}
			return result2;
		}

		internal int Read(HttpWebRequest request, byte[] buffer, int offset, int size)
		{
			lock (this)
			{
				if (this.Data.request != request)
				{
					throw new ObjectDisposedException(typeof(System.Net.Sockets.NetworkStream).FullName);
				}
				if (this.nstream == null)
				{
					return 0;
				}
			}
			int num = 0;
			try
			{
				bool flag = false;
				if (!this.chunkedRead)
				{
					num = this.nstream.Read(buffer, offset, size);
					flag = (num == 0);
				}
				if (this.chunkedRead)
				{
					try
					{
						this.chunkStream.WriteAndReadBack(buffer, offset, size, ref num);
						if (!flag && num == 0 && this.chunkStream.WantMore)
						{
							num = this.EnsureRead(buffer, offset, size);
						}
					}
					catch (Exception e)
					{
						this.HandleError(WebExceptionStatus.ReceiveFailure, e, "chunked Read1");
						throw;
					}
					if ((flag || num == 0) && this.chunkStream.WantMore)
					{
						this.HandleError(WebExceptionStatus.ReceiveFailure, null, "chunked Read2");
						throw new WebException("Read error", null, WebExceptionStatus.ReceiveFailure, null);
					}
				}
			}
			catch (Exception e2)
			{
				this.HandleError(WebExceptionStatus.ReceiveFailure, e2, "Read");
			}
			return num;
		}

		internal bool Write(HttpWebRequest request, byte[] buffer, int offset, int size, ref string err_msg)
		{
			err_msg = null;
			lock (this)
			{
				if (this.Data.request != request)
				{
					throw new ObjectDisposedException(typeof(System.Net.Sockets.NetworkStream).FullName);
				}
				if (this.nstream == null)
				{
					return false;
				}
			}
			try
			{
				this.nstream.Write(buffer, offset, size);
				if (this.ssl && !this.certsAvailable)
				{
					this.GetCertificates();
				}
			}
			catch (Exception ex)
			{
				err_msg = ex.Message;
				WebExceptionStatus st = WebExceptionStatus.SendFailure;
				string where = "Write: " + err_msg;
				if (ex is WebException)
				{
					this.HandleError(st, ex, where);
					return false;
				}
				if (this.ssl && (bool)WebConnection.piTrustFailure.GetValue(this.nstream, null))
				{
					st = WebExceptionStatus.TrustFailure;
					where = "Trust failure";
				}
				this.HandleError(st, ex, where);
				return false;
			}
			return true;
		}

		internal void Close(bool sendNext)
		{
			lock (this)
			{
				if (this.nstream != null)
				{
					try
					{
						this.nstream.Close();
					}
					catch
					{
					}
					this.nstream = null;
				}
				if (this.socket != null)
				{
					try
					{
						this.socket.Close();
					}
					catch
					{
					}
					this.socket = null;
				}
				this.busy = false;
				this.Data = new WebConnectionData();
				if (sendNext)
				{
					this.SendNext();
				}
			}
		}

		private void Abort(object sender, EventArgs args)
		{
			lock (this)
			{
				Queue obj = this.queue;
				lock (obj)
				{
					HttpWebRequest httpWebRequest = (HttpWebRequest)sender;
					if (this.Data.request == httpWebRequest)
					{
						if (!httpWebRequest.FinishedReading)
						{
							this.status = WebExceptionStatus.RequestCanceled;
							this.Close(false);
							if (this.queue.Count > 0)
							{
								this.Data.request = (HttpWebRequest)this.queue.Dequeue();
								this.SendRequest(this.Data.request);
							}
						}
					}
					else
					{
						httpWebRequest.FinishedReading = true;
						httpWebRequest.SetResponseError(WebExceptionStatus.RequestCanceled, null, "User aborted");
						if (this.queue.Count > 0 && this.queue.Peek() == sender)
						{
							this.queue.Dequeue();
						}
						else if (this.queue.Count > 0)
						{
							object[] array = this.queue.ToArray();
							this.queue.Clear();
							for (int i = array.Length - 1; i >= 0; i--)
							{
								if (array[i] != sender)
								{
									this.queue.Enqueue(array[i]);
								}
							}
						}
					}
				}
			}
		}

		internal void ResetNtlm()
		{
			this.ntlm_authenticated = false;
			this.ntlm_credentials = null;
			this.unsafe_sharing = false;
		}

		internal bool Busy
		{
			get
			{
				bool result;
				lock (this)
				{
					result = this.busy;
				}
				return result;
			}
		}

		internal bool Connected
		{
			get
			{
				bool result;
				lock (this)
				{
					result = (this.socket != null && this.socket.Connected);
				}
				return result;
			}
		}

		internal HttpWebRequest PriorityRequest
		{
			set
			{
				this.priority_request = value;
			}
		}

		internal bool NtlmAuthenticated
		{
			get
			{
				return this.ntlm_authenticated;
			}
			set
			{
				this.ntlm_authenticated = value;
			}
		}

		internal NetworkCredential NtlmCredential
		{
			get
			{
				return this.ntlm_credentials;
			}
			set
			{
				this.ntlm_credentials = value;
			}
		}

		internal bool UnsafeAuthenticatedConnectionSharing
		{
			get
			{
				return this.unsafe_sharing;
			}
			set
			{
				this.unsafe_sharing = value;
			}
		}

		private class AbortHelper
		{
			public WebConnection Connection;

			public void Abort(object sender, EventArgs args)
			{
				WebConnection webConnection = ((HttpWebRequest)sender).WebConnection;
				if (webConnection == null)
				{
					webConnection = this.Connection;
				}
				webConnection.Abort(sender, args);
			}
		}
	}
}
