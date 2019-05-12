using System;
using System.Collections;

namespace System.Net
{
	internal class DigestClient : IAuthenticationModule
	{
		private static readonly Hashtable cache = Hashtable.Synchronized(new Hashtable());

		private static Hashtable Cache
		{
			get
			{
				object syncRoot = DigestClient.cache.SyncRoot;
				lock (syncRoot)
				{
					DigestClient.CheckExpired(DigestClient.cache.Count);
				}
				return DigestClient.cache;
			}
		}

		private static void CheckExpired(int count)
		{
			if (count < 10)
			{
				return;
			}
			DateTime t = DateTime.MaxValue;
			DateTime now = DateTime.Now;
			ArrayList arrayList = null;
			foreach (object obj in DigestClient.cache.Keys)
			{
				int num = (int)obj;
				DigestSession digestSession = (DigestSession)DigestClient.cache[num];
				if (digestSession.LastUse < t && (digestSession.LastUse - now).Ticks > 6000000000L)
				{
					t = digestSession.LastUse;
					if (arrayList == null)
					{
						arrayList = new ArrayList();
					}
					arrayList.Add(num);
				}
			}
			if (arrayList != null)
			{
				foreach (object obj2 in arrayList)
				{
					int num2 = (int)obj2;
					DigestClient.cache.Remove(num2);
				}
			}
		}

		public Authorization Authenticate(string challenge, WebRequest webRequest, ICredentials credentials)
		{
			if (credentials == null || challenge == null)
			{
				return null;
			}
			string text = challenge.Trim();
			if (text.ToLower().IndexOf("digest") == -1)
			{
				return null;
			}
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			if (httpWebRequest == null)
			{
				return null;
			}
			int num = httpWebRequest.Address.GetHashCode() ^ credentials.GetHashCode();
			DigestSession digestSession = (DigestSession)DigestClient.Cache[num];
			bool flag = digestSession == null;
			if (flag)
			{
				digestSession = new DigestSession();
			}
			if (!digestSession.Parse(challenge))
			{
				return null;
			}
			if (flag)
			{
				DigestClient.Cache.Add(num, digestSession);
			}
			return digestSession.Authenticate(webRequest, credentials);
		}

		public Authorization PreAuthenticate(WebRequest webRequest, ICredentials credentials)
		{
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			if (httpWebRequest == null)
			{
				return null;
			}
			if (credentials == null)
			{
				return null;
			}
			int num = httpWebRequest.Address.GetHashCode() ^ credentials.GetHashCode();
			DigestSession digestSession = (DigestSession)DigestClient.Cache[num];
			if (digestSession == null)
			{
				return null;
			}
			return digestSession.Authenticate(webRequest, credentials);
		}

		public string AuthenticationType
		{
			get
			{
				return "Digest";
			}
		}

		public bool CanPreAuthenticate
		{
			get
			{
				return true;
			}
		}
	}
}
