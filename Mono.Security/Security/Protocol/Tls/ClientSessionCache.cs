using System;
using System.Collections;

namespace Mono.Security.Protocol.Tls
{
	internal class ClientSessionCache
	{
		private static Hashtable cache = new Hashtable();

		private static object locker = new object();

		public static void Add(string host, byte[] id)
		{
			object obj = ClientSessionCache.locker;
			lock (obj)
			{
				string key = BitConverter.ToString(id);
				ClientSessionInfo clientSessionInfo = (ClientSessionInfo)ClientSessionCache.cache[key];
				if (clientSessionInfo == null)
				{
					ClientSessionCache.cache.Add(key, new ClientSessionInfo(host, id));
				}
				else if (clientSessionInfo.HostName == host)
				{
					clientSessionInfo.KeepAlive();
				}
				else
				{
					clientSessionInfo.Dispose();
					ClientSessionCache.cache.Remove(key);
					ClientSessionCache.cache.Add(key, new ClientSessionInfo(host, id));
				}
			}
		}

		public static byte[] FromHost(string host)
		{
			object obj = ClientSessionCache.locker;
			byte[] result;
			lock (obj)
			{
				foreach (object obj2 in ClientSessionCache.cache.Values)
				{
					ClientSessionInfo clientSessionInfo = (ClientSessionInfo)obj2;
					if (clientSessionInfo.HostName == host && clientSessionInfo.Valid)
					{
						clientSessionInfo.KeepAlive();
						return clientSessionInfo.Id;
					}
				}
				result = null;
			}
			return result;
		}

		private static ClientSessionInfo FromContext(Context context, bool checkValidity)
		{
			if (context == null)
			{
				return null;
			}
			byte[] sessionId = context.SessionId;
			if (sessionId == null || sessionId.Length == 0)
			{
				return null;
			}
			string key = BitConverter.ToString(sessionId);
			ClientSessionInfo clientSessionInfo = (ClientSessionInfo)ClientSessionCache.cache[key];
			if (clientSessionInfo == null)
			{
				return null;
			}
			if (context.ClientSettings.TargetHost != clientSessionInfo.HostName)
			{
				return null;
			}
			if (checkValidity && !clientSessionInfo.Valid)
			{
				clientSessionInfo.Dispose();
				ClientSessionCache.cache.Remove(key);
				return null;
			}
			return clientSessionInfo;
		}

		public static bool SetContextInCache(Context context)
		{
			object obj = ClientSessionCache.locker;
			bool result;
			lock (obj)
			{
				ClientSessionInfo clientSessionInfo = ClientSessionCache.FromContext(context, false);
				if (clientSessionInfo == null)
				{
					result = false;
				}
				else
				{
					clientSessionInfo.GetContext(context);
					clientSessionInfo.KeepAlive();
					result = true;
				}
			}
			return result;
		}

		public static bool SetContextFromCache(Context context)
		{
			object obj = ClientSessionCache.locker;
			bool result;
			lock (obj)
			{
				ClientSessionInfo clientSessionInfo = ClientSessionCache.FromContext(context, true);
				if (clientSessionInfo == null)
				{
					result = false;
				}
				else
				{
					clientSessionInfo.SetContext(context);
					clientSessionInfo.KeepAlive();
					result = true;
				}
			}
			return result;
		}
	}
}
