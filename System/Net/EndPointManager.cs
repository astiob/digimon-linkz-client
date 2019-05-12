using System;
using System.Collections;

namespace System.Net
{
	internal sealed class EndPointManager
	{
		private static Hashtable ip_to_endpoints = new Hashtable();

		private EndPointManager()
		{
		}

		public static void AddListener(HttpListener listener)
		{
			ArrayList arrayList = new ArrayList();
			try
			{
				Hashtable obj = EndPointManager.ip_to_endpoints;
				lock (obj)
				{
					foreach (string text in listener.Prefixes)
					{
						EndPointManager.AddPrefixInternal(text, listener);
						arrayList.Add(text);
					}
				}
			}
			catch
			{
				foreach (object obj2 in arrayList)
				{
					string prefix = (string)obj2;
					EndPointManager.RemovePrefix(prefix, listener);
				}
				throw;
			}
		}

		public static void AddPrefix(string prefix, HttpListener listener)
		{
			Hashtable obj = EndPointManager.ip_to_endpoints;
			lock (obj)
			{
				EndPointManager.AddPrefixInternal(prefix, listener);
			}
		}

		private static void AddPrefixInternal(string p, HttpListener listener)
		{
			ListenerPrefix listenerPrefix = new ListenerPrefix(p);
			if (listenerPrefix.Path.IndexOf('%') != -1)
			{
				throw new HttpListenerException(400, "Invalid path.");
			}
			if (listenerPrefix.Path.IndexOf("//") != -1)
			{
				throw new HttpListenerException(400, "Invalid path.");
			}
			EndPointListener eplistener = EndPointManager.GetEPListener(IPAddress.Any, listenerPrefix.Port, listener, listenerPrefix.Secure);
			eplistener.AddPrefix(listenerPrefix, listener);
		}

		private static EndPointListener GetEPListener(IPAddress addr, int port, HttpListener listener, bool secure)
		{
			Hashtable hashtable;
			if (EndPointManager.ip_to_endpoints.ContainsKey(addr))
			{
				hashtable = (Hashtable)EndPointManager.ip_to_endpoints[addr];
			}
			else
			{
				hashtable = new Hashtable();
				EndPointManager.ip_to_endpoints[addr] = hashtable;
			}
			EndPointListener endPointListener;
			if (hashtable.ContainsKey(port))
			{
				endPointListener = (EndPointListener)hashtable[port];
			}
			else
			{
				endPointListener = new EndPointListener(addr, port, secure);
				hashtable[port] = endPointListener;
			}
			return endPointListener;
		}

		public static void RemoveEndPoint(EndPointListener epl, IPEndPoint ep)
		{
			Hashtable obj = EndPointManager.ip_to_endpoints;
			lock (obj)
			{
				Hashtable hashtable = (Hashtable)EndPointManager.ip_to_endpoints[ep.Address];
				hashtable.Remove(ep.Port);
				if (hashtable.Count == 0)
				{
					EndPointManager.ip_to_endpoints.Remove(ep.Address);
				}
				epl.Close();
			}
		}

		public static void RemoveListener(HttpListener listener)
		{
			Hashtable obj = EndPointManager.ip_to_endpoints;
			lock (obj)
			{
				foreach (string prefix in listener.Prefixes)
				{
					EndPointManager.RemovePrefixInternal(prefix, listener);
				}
			}
		}

		public static void RemovePrefix(string prefix, HttpListener listener)
		{
			Hashtable obj = EndPointManager.ip_to_endpoints;
			lock (obj)
			{
				EndPointManager.RemovePrefixInternal(prefix, listener);
			}
		}

		private static void RemovePrefixInternal(string prefix, HttpListener listener)
		{
			ListenerPrefix listenerPrefix = new ListenerPrefix(prefix);
			if (listenerPrefix.Path.IndexOf('%') != -1)
			{
				return;
			}
			if (listenerPrefix.Path.IndexOf("//") != -1)
			{
				return;
			}
			EndPointListener eplistener = EndPointManager.GetEPListener(IPAddress.Any, listenerPrefix.Port, listener, listenerPrefix.Secure);
			eplistener.RemovePrefix(listenerPrefix, listener);
		}
	}
}
