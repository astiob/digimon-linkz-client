using System;
using System.Text;

namespace System
{
	/// <summary>Provides a custom constructor for uniform resource identifiers (URIs) and modifies URIs for the <see cref="T:System.Uri" /> class.</summary>
	/// <filterpriority>2</filterpriority>
	public class UriBuilder
	{
		private string scheme;

		private string host;

		private int port;

		private string path;

		private string query;

		private string fragment;

		private string username;

		private string password;

		private System.Uri uri;

		private bool modified;

		/// <summary>Initializes a new instance of the <see cref="T:System.UriBuilder" /> class.</summary>
		public UriBuilder() : this(System.Uri.UriSchemeHttp, "localhost")
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.UriBuilder" /> class with the specified URI.</summary>
		/// <param name="uri">A URI string. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="uri" /> is null. </exception>
		/// <exception cref="T:System.UriFormatException">
		///   <paramref name="uri" /> is a zero length string or contains only spaces.-or- The parsing routine detected a scheme in an invalid form.-or- The parser detected more than two consecutive slashes in a URI that does not use the "file" scheme.-or- <paramref name="uri" /> is not a valid URI. </exception>
		public UriBuilder(string uri) : this(new System.Uri(uri))
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.UriBuilder" /> class with the specified <see cref="T:System.Uri" /> instance.</summary>
		/// <param name="uri">An instance of the <see cref="T:System.Uri" /> class. </param>
		/// <exception cref="T:System.NullReferenceException">
		///   <paramref name="uri" /> is null. </exception>
		public UriBuilder(System.Uri uri)
		{
			this.scheme = uri.Scheme;
			this.host = uri.Host;
			this.port = uri.Port;
			this.path = uri.AbsolutePath;
			this.query = uri.Query;
			this.fragment = uri.Fragment;
			this.username = uri.UserInfo;
			int num = this.username.IndexOf(':');
			if (num != -1)
			{
				this.password = this.username.Substring(num + 1);
				this.username = this.username.Substring(0, num);
			}
			else
			{
				this.password = string.Empty;
			}
			this.modified = true;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.UriBuilder" /> class with the specified scheme and host.</summary>
		/// <param name="schemeName">An Internet access protocol. </param>
		/// <param name="hostName">A DNS-style domain name or IP address. </param>
		public UriBuilder(string schemeName, string hostName)
		{
			this.Scheme = schemeName;
			this.Host = hostName;
			this.port = -1;
			this.Path = string.Empty;
			this.query = string.Empty;
			this.fragment = string.Empty;
			this.username = string.Empty;
			this.password = string.Empty;
			this.modified = true;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.UriBuilder" /> class with the specified scheme, host, and port.</summary>
		/// <param name="scheme">An Internet access protocol. </param>
		/// <param name="host">A DNS-style domain name or IP address. </param>
		/// <param name="portNumber">An IP port number for the service. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="portNumber" /> is less than 0 or greater than 65,535. </exception>
		public UriBuilder(string scheme, string host, int portNumber) : this(scheme, host)
		{
			this.Port = portNumber;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.UriBuilder" /> class with the specified scheme, host, port number, and path.</summary>
		/// <param name="scheme">An Internet access protocol. </param>
		/// <param name="host">A DNS-style domain name or IP address. </param>
		/// <param name="port">An IP port number for the service. </param>
		/// <param name="pathValue">The path to the Internet resource. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="port" /> is less than 0 or greater than 65,535. </exception>
		public UriBuilder(string scheme, string host, int port, string pathValue) : this(scheme, host, port)
		{
			this.Path = pathValue;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.UriBuilder" /> class with the specified scheme, host, port number, path and query string or fragment identifier.</summary>
		/// <param name="scheme">An Internet access protocol. </param>
		/// <param name="host">A DNS-style domain name or IP address. </param>
		/// <param name="port">An IP port number for the service. </param>
		/// <param name="path">The path to the Internet resource. </param>
		/// <param name="extraValue">A query string or fragment identifier. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="extraValue" /> is neither null nor <see cref="F:System.String.Empty" />, nor does a valid fragment identifier begin with a number sign (#), nor a valid query string begin with a question mark (?). </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="port" /> is less than 0 or greater than 65,535. </exception>
		public UriBuilder(string scheme, string host, int port, string pathValue, string extraValue) : this(scheme, host, port, pathValue)
		{
			if (extraValue == null || extraValue.Length == 0)
			{
				return;
			}
			if (extraValue[0] == '#')
			{
				this.Fragment = extraValue.Remove(0, 1);
			}
			else
			{
				if (extraValue[0] != '?')
				{
					throw new ArgumentException("extraValue");
				}
				this.Query = extraValue.Remove(0, 1);
			}
		}

		/// <summary>Gets or sets the fragment portion of the URI.</summary>
		/// <returns>The fragment portion of the URI. The fragment identifier ("#") is added to the beginning of the fragment.</returns>
		/// <filterpriority>2</filterpriority>
		public string Fragment
		{
			get
			{
				return this.fragment;
			}
			set
			{
				this.fragment = value;
				if (this.fragment == null)
				{
					this.fragment = string.Empty;
				}
				else if (this.fragment.Length > 0)
				{
					this.fragment = "#" + value.Replace("%23", "#");
				}
				this.modified = true;
			}
		}

		/// <summary>Gets or sets the Domain Name System (DNS) host name or IP address of a server.</summary>
		/// <returns>The DNS host name or IP address of the server.</returns>
		/// <filterpriority>1</filterpriority>
		public string Host
		{
			get
			{
				return this.host;
			}
			set
			{
				this.host = ((value != null) ? value : string.Empty);
				this.modified = true;
			}
		}

		/// <summary>Gets or sets the password associated with the user that accesses the URI.</summary>
		/// <returns>The password of the user that accesses the URI.</returns>
		/// <filterpriority>1</filterpriority>
		public string Password
		{
			get
			{
				return this.password;
			}
			set
			{
				this.password = ((value != null) ? value : string.Empty);
				this.modified = true;
			}
		}

		/// <summary>Gets or sets the path to the resource referenced by the URI.</summary>
		/// <returns>The path to the resource referenced by the URI.</returns>
		/// <filterpriority>1</filterpriority>
		public string Path
		{
			get
			{
				return this.path;
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					this.path = "/";
				}
				else
				{
					this.path = System.Uri.EscapeString(value.Replace('\\', '/'), false, true, true);
				}
				this.modified = true;
			}
		}

		/// <summary>Gets or sets the port number of the URI.</summary>
		/// <returns>The port number of the URI.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The port cannot be set to a value less than 0 or greater than 65,535. </exception>
		/// <filterpriority>1</filterpriority>
		public int Port
		{
			get
			{
				return this.port;
			}
			set
			{
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.port = value;
				this.modified = true;
			}
		}

		/// <summary>Gets or sets any query information included in the URI.</summary>
		/// <returns>The query information included in the URI.</returns>
		/// <filterpriority>1</filterpriority>
		public string Query
		{
			get
			{
				return this.query;
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					this.query = string.Empty;
				}
				else
				{
					this.query = "?" + value;
				}
				this.modified = true;
			}
		}

		/// <summary>Gets or sets the scheme name of the URI.</summary>
		/// <returns>The scheme of the URI.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The scheme cannot be set to an invalid scheme name. </exception>
		/// <filterpriority>1</filterpriority>
		public string Scheme
		{
			get
			{
				return this.scheme;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				int num = value.IndexOf(':');
				if (num != -1)
				{
					value = value.Substring(0, num);
				}
				this.scheme = value.ToLower();
				this.modified = true;
			}
		}

		/// <summary>Gets the <see cref="T:System.Uri" /> instance constructed by the specified <see cref="T:System.UriBuilder" /> instance.</summary>
		/// <returns>A <see cref="T:System.Uri" /> that contains the URI constructed by the <see cref="T:System.UriBuilder" />.</returns>
		/// <exception cref="T:System.UriFormatException">The URI constructed by the <see cref="T:System.UriBuilder" /> properties is invalid. </exception>
		/// <filterpriority>1</filterpriority>
		public System.Uri Uri
		{
			get
			{
				if (!this.modified)
				{
					return this.uri;
				}
				this.uri = new System.Uri(this.ToString(), true);
				this.modified = false;
				return this.uri;
			}
		}

		/// <summary>The user name associated with the user that accesses the URI.</summary>
		/// <returns>The user name of the user that accesses the URI.</returns>
		/// <filterpriority>1</filterpriority>
		public string UserName
		{
			get
			{
				return this.username;
			}
			set
			{
				this.username = ((value != null) ? value : string.Empty);
				this.modified = true;
			}
		}

		/// <summary>Compares an existing <see cref="T:System.Uri" /> instance with the contents of the <see cref="T:System.UriBuilder" /> for equality.</summary>
		/// <returns>true if <paramref name="rparam" /> represents the same <see cref="T:System.Uri" /> as the <see cref="T:System.Uri" /> constructed by this <see cref="T:System.UriBuilder" /> instance; otherwise, false.</returns>
		/// <param name="rparam">The object to compare with the current instance. </param>
		/// <filterpriority>2</filterpriority>
		public override bool Equals(object rparam)
		{
			return rparam != null && this.Uri.Equals(rparam.ToString());
		}

		/// <summary>Returns the hash code for the URI.</summary>
		/// <returns>The hash code generated for the URI.</returns>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override int GetHashCode()
		{
			return this.Uri.GetHashCode();
		}

		/// <summary>Returns the display string for the specified <see cref="T:System.UriBuilder" /> instance.</summary>
		/// <returns>The string that contains the unescaped display string of the <see cref="T:System.UriBuilder" />.</returns>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.scheme);
			stringBuilder.Append("://");
			if (this.username != string.Empty)
			{
				stringBuilder.Append(this.username);
				if (this.password != string.Empty)
				{
					stringBuilder.Append(":" + this.password);
				}
				stringBuilder.Append('@');
			}
			stringBuilder.Append(this.host);
			if (this.port > 0)
			{
				stringBuilder.Append(":" + this.port);
			}
			if (this.path != string.Empty && stringBuilder[stringBuilder.Length - 1] != '/' && this.path.Length > 0 && this.path[0] != '/')
			{
				stringBuilder.Append('/');
			}
			stringBuilder.Append(this.path);
			stringBuilder.Append(this.query);
			stringBuilder.Append(this.fragment);
			return stringBuilder.ToString();
		}
	}
}
