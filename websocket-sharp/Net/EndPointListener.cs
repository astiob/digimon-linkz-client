using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace WebSocketSharp.Net
{
	internal sealed class EndPointListener
	{
		private List<ListenerPrefix> _all;

		private X509Certificate2 _cert;

		private IPEndPoint _endpoint;

		private Dictionary<ListenerPrefix, HttpListener> _prefixes;

		private bool _secure;

		private Socket _socket;

		private List<ListenerPrefix> _unhandled;

		private Dictionary<HttpConnection, HttpConnection> _unregistered;

		public EndPointListener(IPAddress address, int port, bool secure, string certFolderPath, X509Certificate2 defaultCert)
		{
			if (secure)
			{
				this._secure = secure;
				this._cert = EndPointListener.getCertificate(port, certFolderPath, defaultCert);
				if (this._cert == null)
				{
					throw new ArgumentException("Server certificate not found.");
				}
			}
			this._endpoint = new IPEndPoint(address, port);
			this._socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			this._socket.Bind(this._endpoint);
			this._socket.Listen(500);
			SocketAsyncEventArgs socketAsyncEventArgs = new SocketAsyncEventArgs();
			socketAsyncEventArgs.UserToken = this;
			socketAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(EndPointListener.onAccept);
			this._socket.AcceptAsync(socketAsyncEventArgs);
			this._prefixes = new Dictionary<ListenerPrefix, HttpListener>();
			this._unregistered = new Dictionary<HttpConnection, HttpConnection>();
		}

		private static void addSpecial(List<ListenerPrefix> prefixes, ListenerPrefix prefix)
		{
			if (prefixes == null)
			{
				return;
			}
			foreach (ListenerPrefix listenerPrefix in prefixes)
			{
				if (listenerPrefix.Path == prefix.Path)
				{
					throw new HttpListenerException(400, "Prefix already in use.");
				}
			}
			prefixes.Add(prefix);
		}

		private void checkIfRemove()
		{
			if (this._prefixes.Count > 0)
			{
				return;
			}
			if (this._unhandled != null && this._unhandled.Count > 0)
			{
				return;
			}
			if (this._all != null && this._all.Count > 0)
			{
				return;
			}
			EndPointManager.RemoveEndPoint(this, this._endpoint);
		}

		private static RSACryptoServiceProvider createRSAFromFile(string filename)
		{
			RSACryptoServiceProvider rsacryptoServiceProvider = new RSACryptoServiceProvider();
			byte[] array = null;
			using (FileStream fileStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				array = new byte[fileStream.Length];
				fileStream.Read(array, 0, array.Length);
			}
			rsacryptoServiceProvider.ImportCspBlob(array);
			return rsacryptoServiceProvider;
		}

		private static X509Certificate2 getCertificate(int port, string certFolderPath, X509Certificate2 defaultCert)
		{
			try
			{
				string text = Path.Combine(certFolderPath, string.Format("{0}.cer", port));
				string text2 = Path.Combine(certFolderPath, string.Format("{0}.key", port));
				if (File.Exists(text) && File.Exists(text2))
				{
					return new X509Certificate2(text)
					{
						PrivateKey = EndPointListener.createRSAFromFile(text2)
					};
				}
			}
			catch
			{
			}
			return defaultCert;
		}

		private static HttpListener matchFromList(string host, string path, List<ListenerPrefix> list, out ListenerPrefix prefix)
		{
			prefix = null;
			if (list == null)
			{
				return null;
			}
			HttpListener result = null;
			int num = -1;
			foreach (ListenerPrefix listenerPrefix in list)
			{
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

		private static void onAccept(object sender, EventArgs e)
		{
			SocketAsyncEventArgs socketAsyncEventArgs = (SocketAsyncEventArgs)e;
			EndPointListener endPointListener = (EndPointListener)socketAsyncEventArgs.UserToken;
			Socket socket = null;
			if (socketAsyncEventArgs.SocketError == SocketError.Success)
			{
				socket = socketAsyncEventArgs.AcceptSocket;
				socketAsyncEventArgs.AcceptSocket = null;
			}
			try
			{
				endPointListener._socket.AcceptAsync(socketAsyncEventArgs);
			}
			catch
			{
				if (socket != null)
				{
					socket.Close();
				}
				return;
			}
			if (socket == null)
			{
				return;
			}
			HttpConnection httpConnection = null;
			try
			{
				httpConnection = new HttpConnection(socket, endPointListener, endPointListener._secure, endPointListener._cert);
				object syncRoot = ((ICollection)endPointListener._unregistered).SyncRoot;
				lock (syncRoot)
				{
					endPointListener._unregistered[httpConnection] = httpConnection;
				}
				httpConnection.BeginReadRequest();
			}
			catch
			{
				if (httpConnection != null)
				{
					httpConnection.Close(true);
				}
				else
				{
					socket.Close();
				}
			}
		}

		private static bool removeSpecial(List<ListenerPrefix> prefixes, ListenerPrefix prefix)
		{
			if (prefixes == null)
			{
				return false;
			}
			int count = prefixes.Count;
			for (int i = 0; i < count; i++)
			{
				if (prefixes[i].Path == prefix.Path)
				{
					prefixes.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		private HttpListener searchListener(Uri uri, out ListenerPrefix prefix)
		{
			prefix = null;
			if (uri == null)
			{
				return null;
			}
			string host = uri.Host;
			int port = uri.Port;
			string text = HttpUtility.UrlDecode(uri.AbsolutePath);
			string text2 = (text[text.Length - 1] != '/') ? (text + "/") : text;
			HttpListener httpListener = null;
			int num = -1;
			if (host != null && host.Length > 0)
			{
				foreach (ListenerPrefix listenerPrefix in this._prefixes.Keys)
				{
					string path = listenerPrefix.Path;
					if (path.Length >= num)
					{
						if (!(listenerPrefix.Host != host) && listenerPrefix.Port == port)
						{
							if (text.StartsWith(path) || text2.StartsWith(path))
							{
								num = path.Length;
								httpListener = this._prefixes[listenerPrefix];
								prefix = listenerPrefix;
							}
						}
					}
				}
				if (num != -1)
				{
					return httpListener;
				}
			}
			List<ListenerPrefix> list = this._unhandled;
			httpListener = EndPointListener.matchFromList(host, text, list, out prefix);
			if (text != text2 && httpListener == null)
			{
				httpListener = EndPointListener.matchFromList(host, text2, list, out prefix);
			}
			if (httpListener != null)
			{
				return httpListener;
			}
			list = this._all;
			httpListener = EndPointListener.matchFromList(host, text, list, out prefix);
			if (text != text2 && httpListener == null)
			{
				httpListener = EndPointListener.matchFromList(host, text2, list, out prefix);
			}
			if (httpListener != null)
			{
				return httpListener;
			}
			return null;
		}

		internal static bool CertificateExists(int port, string certFolderPath)
		{
			string path = Path.Combine(certFolderPath, string.Format("{0}.cer", port));
			string path2 = Path.Combine(certFolderPath, string.Format("{0}.key", port));
			return File.Exists(path) && File.Exists(path2);
		}

		internal void RemoveConnection(HttpConnection connection)
		{
			object syncRoot = ((ICollection)this._unregistered).SyncRoot;
			lock (syncRoot)
			{
				this._unregistered.Remove(connection);
			}
		}

		public void AddPrefix(ListenerPrefix prefix, HttpListener listener)
		{
			if (prefix.Host == "*")
			{
				List<ListenerPrefix> list;
				List<ListenerPrefix> list2;
				do
				{
					list = this._unhandled;
					list2 = ((list == null) ? new List<ListenerPrefix>() : new List<ListenerPrefix>(list));
					prefix.Listener = listener;
					EndPointListener.addSpecial(list2, prefix);
				}
				while (Interlocked.CompareExchange<List<ListenerPrefix>>(ref this._unhandled, list2, list) != list);
				return;
			}
			if (prefix.Host == "+")
			{
				List<ListenerPrefix> list;
				List<ListenerPrefix> list2;
				do
				{
					list = this._all;
					list2 = ((list == null) ? new List<ListenerPrefix>() : new List<ListenerPrefix>(list));
					prefix.Listener = listener;
					EndPointListener.addSpecial(list2, prefix);
				}
				while (Interlocked.CompareExchange<List<ListenerPrefix>>(ref this._all, list2, list) != list);
				return;
			}
			Dictionary<ListenerPrefix, HttpListener> prefixes;
			for (;;)
			{
				prefixes = this._prefixes;
				if (prefixes.ContainsKey(prefix))
				{
					break;
				}
				Dictionary<ListenerPrefix, HttpListener> dictionary = new Dictionary<ListenerPrefix, HttpListener>(prefixes);
				dictionary[prefix] = listener;
				if (Interlocked.CompareExchange<Dictionary<ListenerPrefix, HttpListener>>(ref this._prefixes, dictionary, prefixes) == prefixes)
				{
					return;
				}
			}
			HttpListener httpListener = prefixes[prefix];
			if (httpListener != listener)
			{
				throw new HttpListenerException(400, "There's another listener for " + prefix);
			}
			return;
		}

		public bool BindContext(HttpListenerContext context)
		{
			HttpListenerRequest request = context.Request;
			ListenerPrefix prefix;
			HttpListener httpListener = this.searchListener(request.Url, out prefix);
			if (httpListener == null)
			{
				return false;
			}
			context.Listener = httpListener;
			context.Connection.Prefix = prefix;
			return true;
		}

		public void Close()
		{
			this._socket.Close();
			object syncRoot = ((ICollection)this._unregistered).SyncRoot;
			lock (syncRoot)
			{
				Dictionary<HttpConnection, HttpConnection> dictionary = new Dictionary<HttpConnection, HttpConnection>(this._unregistered);
				foreach (HttpConnection httpConnection in dictionary.Keys)
				{
					httpConnection.Close(true);
				}
				dictionary.Clear();
				this._unregistered.Clear();
			}
		}

		public void RemovePrefix(ListenerPrefix prefix, HttpListener listener)
		{
			if (prefix.Host == "*")
			{
				List<ListenerPrefix> list;
				List<ListenerPrefix> list2;
				do
				{
					list = this._unhandled;
					list2 = ((list == null) ? new List<ListenerPrefix>() : new List<ListenerPrefix>(list));
					if (!EndPointListener.removeSpecial(list2, prefix))
					{
						break;
					}
				}
				while (Interlocked.CompareExchange<List<ListenerPrefix>>(ref this._unhandled, list2, list) != list);
				this.checkIfRemove();
				return;
			}
			if (prefix.Host == "+")
			{
				List<ListenerPrefix> list;
				List<ListenerPrefix> list2;
				do
				{
					list = this._all;
					list2 = ((list == null) ? new List<ListenerPrefix>() : new List<ListenerPrefix>(list));
					if (!EndPointListener.removeSpecial(list2, prefix))
					{
						break;
					}
				}
				while (Interlocked.CompareExchange<List<ListenerPrefix>>(ref this._all, list2, list) != list);
				this.checkIfRemove();
				return;
			}
			Dictionary<ListenerPrefix, HttpListener> prefixes;
			Dictionary<ListenerPrefix, HttpListener> dictionary;
			do
			{
				prefixes = this._prefixes;
				if (!prefixes.ContainsKey(prefix))
				{
					break;
				}
				dictionary = new Dictionary<ListenerPrefix, HttpListener>(prefixes);
				dictionary.Remove(prefix);
			}
			while (Interlocked.CompareExchange<Dictionary<ListenerPrefix, HttpListener>>(ref this._prefixes, dictionary, prefixes) != prefixes);
			this.checkIfRemove();
		}

		public void UnbindContext(HttpListenerContext context)
		{
			if (context == null || context.Listener == null)
			{
				return;
			}
			context.Listener.UnregisterContext(context);
		}
	}
}
