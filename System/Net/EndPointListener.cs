using Mono.Security.Authenticode;
using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace System.Net
{
	internal sealed class EndPointListener
	{
		private IPEndPoint endpoint;

		private System.Net.Sockets.Socket sock;

		private Hashtable prefixes;

		private ArrayList unhandled;

		private ArrayList all;

		private System.Security.Cryptography.X509Certificates.X509Certificate2 cert;

		private AsymmetricAlgorithm key;

		private bool secure;

		public EndPointListener(IPAddress addr, int port, bool secure)
		{
			if (secure)
			{
				this.secure = secure;
				this.LoadCertificateAndKey(addr, port);
			}
			this.endpoint = new IPEndPoint(addr, port);
			this.sock = new System.Net.Sockets.Socket(addr.AddressFamily, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
			this.sock.Bind(this.endpoint);
			this.sock.Listen(500);
			this.sock.BeginAccept(new AsyncCallback(EndPointListener.OnAccept), this);
			this.prefixes = new Hashtable();
		}

		private void LoadCertificateAndKey(IPAddress addr, int port)
		{
			try
			{
				string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
				string path = Path.Combine(folderPath, ".mono");
				path = Path.Combine(path, "httplistener");
				string fileName = Path.Combine(path, string.Format("{0}.cer", port));
				string filename = Path.Combine(path, string.Format("{0}.pvk", port));
				this.cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(fileName);
				this.key = PrivateKey.CreateFromFile(filename).RSA;
			}
			catch
			{
			}
		}

		private static void OnAccept(IAsyncResult ares)
		{
			EndPointListener endPointListener = (EndPointListener)ares.AsyncState;
			System.Net.Sockets.Socket socket = null;
			try
			{
				socket = endPointListener.sock.EndAccept(ares);
			}
			catch
			{
			}
			finally
			{
				try
				{
					endPointListener.sock.BeginAccept(new AsyncCallback(EndPointListener.OnAccept), endPointListener);
				}
				catch
				{
					if (socket != null)
					{
						try
						{
							socket.Close();
						}
						catch
						{
						}
						socket = null;
					}
				}
			}
			if (socket == null)
			{
				return;
			}
			if (endPointListener.secure && (endPointListener.cert == null || endPointListener.key == null))
			{
				socket.Close();
				return;
			}
			HttpConnection httpConnection = new HttpConnection(socket, endPointListener, endPointListener.secure, endPointListener.cert, endPointListener.key);
			httpConnection.BeginReadRequest();
		}

		public bool BindContext(HttpListenerContext context)
		{
			HttpListenerRequest request = context.Request;
			ListenerPrefix prefix;
			HttpListener httpListener = this.SearchListener(request.UserHostName, request.Url, out prefix);
			if (httpListener == null)
			{
				return false;
			}
			context.Listener = httpListener;
			context.Connection.Prefix = prefix;
			httpListener.RegisterContext(context);
			return true;
		}

		public void UnbindContext(HttpListenerContext context)
		{
			if (context == null || context.Request == null)
			{
				return;
			}
			HttpListenerRequest request = context.Request;
			ListenerPrefix listenerPrefix;
			HttpListener httpListener = this.SearchListener(request.UserHostName, request.Url, out listenerPrefix);
			if (httpListener != null)
			{
				httpListener.UnregisterContext(context);
			}
		}

		private HttpListener SearchListener(string host, System.Uri uri, out ListenerPrefix prefix)
		{
			prefix = null;
			if (uri == null)
			{
				return null;
			}
			if (host != null)
			{
				int num = host.IndexOf(':');
				if (num >= 0)
				{
					host = host.Substring(0, num);
				}
			}
			string text = HttpUtility.UrlDecode(uri.AbsolutePath);
			string text2 = (text[text.Length - 1] != '/') ? (text + "/") : text;
			HttpListener httpListener = null;
			int num2 = -1;
			Hashtable obj = this.prefixes;
			lock (obj)
			{
				if (host != null && host != string.Empty)
				{
					foreach (object obj2 in this.prefixes.Keys)
					{
						ListenerPrefix listenerPrefix = (ListenerPrefix)obj2;
						string path = listenerPrefix.Path;
						if (path.Length >= num2)
						{
							if (listenerPrefix.Host == host && (text.StartsWith(path) || text2.StartsWith(path)))
							{
								num2 = path.Length;
								httpListener = (HttpListener)this.prefixes[listenerPrefix];
								prefix = listenerPrefix;
							}
						}
					}
					if (num2 != -1)
					{
						return httpListener;
					}
				}
				httpListener = this.MatchFromList(host, text, this.unhandled, out prefix);
				if (httpListener != null)
				{
					return httpListener;
				}
				httpListener = this.MatchFromList(host, text, this.all, out prefix);
				if (httpListener != null)
				{
					return httpListener;
				}
			}
			return null;
		}

		private HttpListener MatchFromList(string host, string path, ArrayList list, out ListenerPrefix prefix)
		{
			prefix = null;
			if (list == null)
			{
				return null;
			}
			HttpListener result = null;
			int num = -1;
			foreach (object obj in list)
			{
				ListenerPrefix listenerPrefix = (ListenerPrefix)obj;
				string path2 = listenerPrefix.Path;
				if (path2.Length >= num)
				{
					if (path.StartsWith(path2))
					{
						num = path2.Length;
						result = listenerPrefix.Listener;
						prefix = listenerPrefix;
					}
				}
			}
			return result;
		}

		private void AddSpecial(ArrayList coll, ListenerPrefix prefix)
		{
			if (coll == null)
			{
				return;
			}
			foreach (object obj in coll)
			{
				ListenerPrefix listenerPrefix = (ListenerPrefix)obj;
				if (listenerPrefix.Path == prefix.Path)
				{
					throw new HttpListenerException(400, "Prefix already in use.");
				}
			}
			coll.Add(prefix);
		}

		private void RemoveSpecial(ArrayList coll, ListenerPrefix prefix)
		{
			if (coll == null)
			{
				return;
			}
			int count = coll.Count;
			for (int i = 0; i < count; i++)
			{
				ListenerPrefix listenerPrefix = (ListenerPrefix)coll[i];
				if (listenerPrefix.Path == prefix.Path)
				{
					coll.RemoveAt(i);
					this.CheckIfRemove();
					return;
				}
			}
		}

		private void CheckIfRemove()
		{
			if (this.prefixes.Count > 0)
			{
				return;
			}
			if (this.unhandled != null && this.unhandled.Count > 0)
			{
				return;
			}
			if (this.all != null && this.all.Count > 0)
			{
				return;
			}
			EndPointManager.RemoveEndPoint(this, this.endpoint);
		}

		public void Close()
		{
			this.sock.Close();
		}

		public void AddPrefix(ListenerPrefix prefix, HttpListener listener)
		{
			Hashtable obj = this.prefixes;
			lock (obj)
			{
				if (prefix.Host == "*")
				{
					if (this.unhandled == null)
					{
						this.unhandled = new ArrayList();
					}
					prefix.Listener = listener;
					this.AddSpecial(this.unhandled, prefix);
				}
				else if (prefix.Host == "+")
				{
					if (this.all == null)
					{
						this.all = new ArrayList();
					}
					prefix.Listener = listener;
					this.AddSpecial(this.all, prefix);
				}
				else if (this.prefixes.ContainsKey(prefix))
				{
					HttpListener httpListener = (HttpListener)this.prefixes[prefix];
					if (httpListener != listener)
					{
						throw new HttpListenerException(400, "There's another listener for " + prefix);
					}
				}
				else
				{
					this.prefixes[prefix] = listener;
				}
			}
		}

		public void RemovePrefix(ListenerPrefix prefix, HttpListener listener)
		{
			Hashtable obj = this.prefixes;
			lock (obj)
			{
				if (prefix.Host == "*")
				{
					this.RemoveSpecial(this.unhandled, prefix);
				}
				else if (prefix.Host == "+")
				{
					this.RemoveSpecial(this.all, prefix);
				}
				else if (this.prefixes.ContainsKey(prefix))
				{
					this.prefixes.Remove(prefix);
					this.CheckIfRemove();
				}
			}
		}
	}
}
