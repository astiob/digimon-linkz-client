using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace WebSocketSharp.Net
{
	internal sealed class EndPointManager
	{
		private static Dictionary<IPAddress, Dictionary<int, EndPointListener>> _ipToEndpoints = new Dictionary<IPAddress, Dictionary<int, EndPointListener>>();

		private EndPointManager()
		{
		}

		private static void addPrefix(string uriPrefix, HttpListener httpListener)
		{
			ListenerPrefix listenerPrefix = new ListenerPrefix(uriPrefix);
			if (listenerPrefix.Path.IndexOf('%') != -1)
			{
				throw new HttpListenerException(400, "Invalid path.");
			}
			if (listenerPrefix.Path.IndexOf("//", StringComparison.Ordinal) != -1)
			{
				throw new HttpListenerException(400, "Invalid path.");
			}
			EndPointListener endPointListener = EndPointManager.getEndPointListener(IPAddress.Any, listenerPrefix.Port, httpListener, listenerPrefix.Secure);
			endPointListener.AddPrefix(listenerPrefix, httpListener);
		}

		private static EndPointListener getEndPointListener(IPAddress address, int port, HttpListener httpListener, bool secure)
		{
			Dictionary<int, EndPointListener> dictionary;
			if (EndPointManager._ipToEndpoints.ContainsKey(address))
			{
				dictionary = EndPointManager._ipToEndpoints[address];
			}
			else
			{
				dictionary = new Dictionary<int, EndPointListener>();
				EndPointManager._ipToEndpoints[address] = dictionary;
			}
			EndPointListener endPointListener;
			if (dictionary.ContainsKey(port))
			{
				endPointListener = dictionary[port];
			}
			else
			{
				endPointListener = new EndPointListener(address, port, secure, httpListener.CertificateFolderPath, httpListener.DefaultCertificate);
				dictionary[port] = endPointListener;
			}
			return endPointListener;
		}

		private static void removePrefix(string uriPrefix, HttpListener httpListener)
		{
			ListenerPrefix listenerPrefix = new ListenerPrefix(uriPrefix);
			if (listenerPrefix.Path.IndexOf('%') != -1)
			{
				return;
			}
			if (listenerPrefix.Path.IndexOf("//", StringComparison.Ordinal) != -1)
			{
				return;
			}
			EndPointListener endPointListener = EndPointManager.getEndPointListener(IPAddress.Any, listenerPrefix.Port, httpListener, listenerPrefix.Secure);
			endPointListener.RemovePrefix(listenerPrefix, httpListener);
		}

		public static void AddListener(HttpListener httpListener)
		{
			List<string> list = new List<string>();
			object syncRoot = ((ICollection)EndPointManager._ipToEndpoints).SyncRoot;
			lock (syncRoot)
			{
				try
				{
					foreach (string text in httpListener.Prefixes)
					{
						EndPointManager.addPrefix(text, httpListener);
						list.Add(text);
					}
				}
				catch
				{
					foreach (string uriPrefix in list)
					{
						EndPointManager.removePrefix(uriPrefix, httpListener);
					}
					throw;
				}
			}
		}

		public static void AddPrefix(string uriPrefix, HttpListener httpListener)
		{
			object syncRoot = ((ICollection)EndPointManager._ipToEndpoints).SyncRoot;
			lock (syncRoot)
			{
				EndPointManager.addPrefix(uriPrefix, httpListener);
			}
		}

		public static void RemoveEndPoint(EndPointListener epListener, IPEndPoint endpoint)
		{
			object syncRoot = ((ICollection)EndPointManager._ipToEndpoints).SyncRoot;
			lock (syncRoot)
			{
				Dictionary<int, EndPointListener> dictionary = EndPointManager._ipToEndpoints[endpoint.Address];
				dictionary.Remove(endpoint.Port);
				if (dictionary.Count == 0)
				{
					EndPointManager._ipToEndpoints.Remove(endpoint.Address);
				}
				epListener.Close();
			}
		}

		public static void RemoveListener(HttpListener httpListener)
		{
			object syncRoot = ((ICollection)EndPointManager._ipToEndpoints).SyncRoot;
			lock (syncRoot)
			{
				foreach (string uriPrefix in httpListener.Prefixes)
				{
					EndPointManager.removePrefix(uriPrefix, httpListener);
				}
			}
		}

		public static void RemovePrefix(string uriPrefix, HttpListener httpListener)
		{
			object syncRoot = ((ICollection)EndPointManager._ipToEndpoints).SyncRoot;
			lock (syncRoot)
			{
				EndPointManager.removePrefix(uriPrefix, httpListener);
			}
		}
	}
}
