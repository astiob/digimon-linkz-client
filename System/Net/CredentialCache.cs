using System;
using System.Collections;

namespace System.Net
{
	/// <summary>Provides storage for multiple credentials.</summary>
	public class CredentialCache : IEnumerable, ICredentials, ICredentialsByHost
	{
		private static NetworkCredential empty = new NetworkCredential(string.Empty, string.Empty, string.Empty);

		private Hashtable cache;

		private Hashtable cacheForHost;

		/// <summary>Creates a new instance of the <see cref="T:System.Net.CredentialCache" /> class.</summary>
		public CredentialCache()
		{
			this.cache = new Hashtable();
			this.cacheForHost = new Hashtable();
		}

		/// <summary>Gets the system credentials of the application.</summary>
		/// <returns>An <see cref="T:System.Net.ICredentials" /> that represents the system credentials of the application.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="USERNAME" />
		/// </PermissionSet>
		[MonoTODO("Need EnvironmentPermission implementation first")]
		public static ICredentials DefaultCredentials
		{
			get
			{
				return CredentialCache.empty;
			}
		}

		/// <summary>Gets the network credentials of the current security context.</summary>
		/// <returns>An <see cref="T:System.Net.NetworkCredential" /> that represents the network credentials of the current user or application.</returns>
		public static NetworkCredential DefaultNetworkCredentials
		{
			get
			{
				return CredentialCache.empty;
			}
		}

		/// <summary>Returns the <see cref="T:System.Net.NetworkCredential" /> instance associated with the specified Uniform Resource Identifier (URI) and authentication type.</summary>
		/// <returns>A <see cref="T:System.Net.NetworkCredential" /> or, if there is no matching credential in the cache, null.</returns>
		/// <param name="uriPrefix">A <see cref="T:System.Uri" /> that specifies the URI prefix of the resources that the credential grants access to. </param>
		/// <param name="authType">The authentication scheme used by the resource named in <paramref name="uriPrefix" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="uriPrefix" /> or <paramref name="authType" /> is null. </exception>
		public NetworkCredential GetCredential(System.Uri uriPrefix, string authType)
		{
			int num = -1;
			NetworkCredential result = null;
			if (uriPrefix == null || authType == null)
			{
				return null;
			}
			string text = uriPrefix.AbsolutePath;
			text = text.Substring(0, text.LastIndexOf('/'));
			IDictionaryEnumerator enumerator = this.cache.GetEnumerator();
			while (enumerator.MoveNext())
			{
				CredentialCache.CredentialCacheKey credentialCacheKey = enumerator.Key as CredentialCache.CredentialCacheKey;
				if (credentialCacheKey.Length > num)
				{
					if (string.Compare(credentialCacheKey.AuthType, authType, true) == 0)
					{
						System.Uri uriPrefix2 = credentialCacheKey.UriPrefix;
						if (!(uriPrefix2.Scheme != uriPrefix.Scheme))
						{
							if (uriPrefix2.Port == uriPrefix.Port)
							{
								if (!(uriPrefix2.Host != uriPrefix.Host))
								{
									if (text.StartsWith(credentialCacheKey.AbsPath))
									{
										num = credentialCacheKey.Length;
										result = (NetworkCredential)enumerator.Value;
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		/// <summary>Returns an enumerator that can iterate through the <see cref="T:System.Net.CredentialCache" /> instance.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> for the <see cref="T:System.Net.CredentialCache" />.</returns>
		public IEnumerator GetEnumerator()
		{
			return this.cache.Values.GetEnumerator();
		}

		/// <summary>Adds a <see cref="T:System.Net.NetworkCredential" /> instance to the credential cache for use with protocols other than SMTP and associates it with a Uniform Resource Identifier (URI) prefix and authentication protocol. </summary>
		/// <param name="uriPrefix">A <see cref="T:System.Uri" /> that specifies the URI prefix of the resources that the credential grants access to. </param>
		/// <param name="authType">The authentication scheme used by the resource named in <paramref name="uriPrefix" />. </param>
		/// <param name="cred">The <see cref="T:System.Net.NetworkCredential" /> to add to the credential cache. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="uriPrefix" /> is null. -or- <paramref name="authType" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">The same credentials are added more than once. </exception>
		public void Add(System.Uri uriPrefix, string authType, NetworkCredential cred)
		{
			if (uriPrefix == null)
			{
				throw new ArgumentNullException("uriPrefix");
			}
			if (authType == null)
			{
				throw new ArgumentNullException("authType");
			}
			this.cache.Add(new CredentialCache.CredentialCacheKey(uriPrefix, authType), cred);
		}

		/// <summary>Deletes a <see cref="T:System.Net.NetworkCredential" /> instance from the cache if it is associated with the specified Uniform Resource Identifier (URI) prefix and authentication protocol.</summary>
		/// <param name="uriPrefix">A <see cref="T:System.Uri" /> that specifies the URI prefix of the resources that the credential is used for. </param>
		/// <param name="authType">The authentication scheme used by the host named in <paramref name="uriPrefix" />. </param>
		public void Remove(System.Uri uriPrefix, string authType)
		{
			if (uriPrefix == null)
			{
				throw new ArgumentNullException("uriPrefix");
			}
			if (authType == null)
			{
				throw new ArgumentNullException("authType");
			}
			this.cache.Remove(new CredentialCache.CredentialCacheKey(uriPrefix, authType));
		}

		/// <summary>Returns the <see cref="T:System.Net.NetworkCredential" /> instance associated with the specified host, port, and authentication protocol.</summary>
		/// <returns>A <see cref="T:System.Net.NetworkCredential" /> or, if there is no matching credential in the cache, null.</returns>
		/// <param name="host">A <see cref="T:System.String" /> that identifies the host computer.</param>
		/// <param name="port">A <see cref="T:System.Int32" /> that specifies the port to connect to on <paramref name="host" />.</param>
		/// <param name="authenticationType">A <see cref="T:System.String" /> that identifies the authentication scheme used when connecting to <paramref name="host" />. See Remarks.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="host" /> is null. -or- <paramref name="authType" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="authType" /> not an accepted value. See Remarks. -or-<paramref name="host" /> is equal to the empty string ("").</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="port" /> is less than zero.</exception>
		public NetworkCredential GetCredential(string host, int port, string authenticationType)
		{
			NetworkCredential result = null;
			if (host == null || port < 0 || authenticationType == null)
			{
				return null;
			}
			IDictionaryEnumerator enumerator = this.cacheForHost.GetEnumerator();
			while (enumerator.MoveNext())
			{
				CredentialCache.CredentialCacheForHostKey credentialCacheForHostKey = enumerator.Key as CredentialCache.CredentialCacheForHostKey;
				if (string.Compare(credentialCacheForHostKey.AuthType, authenticationType, true) == 0)
				{
					if (!(credentialCacheForHostKey.Host != host))
					{
						if (credentialCacheForHostKey.Port == port)
						{
							result = (NetworkCredential)enumerator.Value;
						}
					}
				}
			}
			return result;
		}

		/// <summary>Adds a <see cref="T:System.Net.NetworkCredential" /> instance for use with SMTP to the credential cache and associates it with a host computer, port, and authentication protocol. Credentials added using this method are valid for SMTP only. This method does not work for HTTP or FTP requests.</summary>
		/// <param name="host">A <see cref="T:System.String" /> that identifies the host computer.</param>
		/// <param name="port">A <see cref="T:System.Int32" /> that specifies the port to connect to on <paramref name="host" />.</param>
		/// <param name="authenticationType">A <see cref="T:System.String" /> that identifies the authentication scheme used when connecting to <paramref name="host" /> using <paramref name="cred" />. See Remarks.</param>
		/// <param name="credential">The <see cref="T:System.Net.NetworkCredential" /> to add to the credential cache. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="host" /> is null. -or-<paramref name="authType" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="authType" /> not an accepted value. See Remarks. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="port" /> is less than zero.</exception>
		public void Add(string host, int port, string authenticationType, NetworkCredential credential)
		{
			if (host == null)
			{
				throw new ArgumentNullException("host");
			}
			if (port < 0)
			{
				throw new ArgumentOutOfRangeException("port");
			}
			if (authenticationType == null)
			{
				throw new ArgumentOutOfRangeException("authenticationType");
			}
			this.cacheForHost.Add(new CredentialCache.CredentialCacheForHostKey(host, port, authenticationType), credential);
		}

		/// <summary>Deletes a <see cref="T:System.Net.NetworkCredential" /> instance from the cache if it is associated with the specified host, port, and authentication protocol.</summary>
		/// <param name="host">A <see cref="T:System.String" /> that identifies the host computer.</param>
		/// <param name="port">A <see cref="T:System.Int32" /> that specifies the port to connect to on <paramref name="host" />.</param>
		/// <param name="authenticationType">A <see cref="T:System.String" /> that identifies the authentication scheme used when connecting to <paramref name="host" />. See Remarks.</param>
		public void Remove(string host, int port, string authenticationType)
		{
			if (host == null)
			{
				return;
			}
			if (authenticationType == null)
			{
				return;
			}
			this.cacheForHost.Remove(new CredentialCache.CredentialCacheForHostKey(host, port, authenticationType));
		}

		private class CredentialCacheKey
		{
			private System.Uri uriPrefix;

			private string authType;

			private string absPath;

			private int len;

			private int hash;

			internal CredentialCacheKey(System.Uri uriPrefix, string authType)
			{
				this.uriPrefix = uriPrefix;
				this.authType = authType;
				this.absPath = uriPrefix.AbsolutePath;
				this.absPath = this.absPath.Substring(0, this.absPath.LastIndexOf('/'));
				this.len = uriPrefix.AbsoluteUri.Length;
				this.hash = uriPrefix.GetHashCode() + authType.GetHashCode();
			}

			public int Length
			{
				get
				{
					return this.len;
				}
			}

			public string AbsPath
			{
				get
				{
					return this.absPath;
				}
			}

			public System.Uri UriPrefix
			{
				get
				{
					return this.uriPrefix;
				}
			}

			public string AuthType
			{
				get
				{
					return this.authType;
				}
			}

			public override int GetHashCode()
			{
				return this.hash;
			}

			public override bool Equals(object obj)
			{
				CredentialCache.CredentialCacheKey credentialCacheKey = obj as CredentialCache.CredentialCacheKey;
				return credentialCacheKey != null && this.hash == credentialCacheKey.hash;
			}

			public override string ToString()
			{
				return string.Concat(new object[]
				{
					this.absPath,
					" : ",
					this.authType,
					" : len=",
					this.len
				});
			}
		}

		private class CredentialCacheForHostKey
		{
			private string host;

			private int port;

			private string authType;

			private int hash;

			internal CredentialCacheForHostKey(string host, int port, string authType)
			{
				this.host = host;
				this.port = port;
				this.authType = authType;
				this.hash = host.GetHashCode() + port.GetHashCode() + authType.GetHashCode();
			}

			public string Host
			{
				get
				{
					return this.host;
				}
			}

			public int Port
			{
				get
				{
					return this.port;
				}
			}

			public string AuthType
			{
				get
				{
					return this.authType;
				}
			}

			public override int GetHashCode()
			{
				return this.hash;
			}

			public override bool Equals(object obj)
			{
				CredentialCache.CredentialCacheForHostKey credentialCacheForHostKey = obj as CredentialCache.CredentialCacheForHostKey;
				return credentialCacheForHostKey != null && this.hash == credentialCacheForHostKey.hash;
			}

			public override string ToString()
			{
				return this.host + " : " + this.authType;
			}
		}
	}
}
