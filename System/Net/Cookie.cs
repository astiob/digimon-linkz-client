using System;
using System.Collections;
using System.Globalization;
using System.Text;

namespace System.Net
{
	/// <summary>Provides a set of properties and methods that are used to manage cookies. This class cannot be inherited.</summary>
	[Serializable]
	public sealed class Cookie
	{
		private string comment;

		private System.Uri commentUri;

		private bool discard;

		private string domain;

		private DateTime expires;

		private bool httpOnly;

		private string name;

		private string path;

		private string port;

		private int[] ports;

		private bool secure;

		private DateTime timestamp;

		private string val;

		private int version;

		private static char[] reservedCharsName = new char[]
		{
			' ',
			'=',
			';',
			',',
			'\n',
			'\r',
			'\t'
		};

		private static char[] portSeparators = new char[]
		{
			'"',
			','
		};

		private static string tspecials = "()<>@,;:\\\"/[]?={} \t";

		private bool exact_domain;

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Cookie" /> class.</summary>
		public Cookie()
		{
			this.expires = DateTime.MinValue;
			this.timestamp = DateTime.Now;
			this.domain = string.Empty;
			this.name = string.Empty;
			this.val = string.Empty;
			this.comment = string.Empty;
			this.port = string.Empty;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Cookie" /> class with a specified <see cref="P:System.Net.Cookie.Name" /> and <see cref="P:System.Net.Cookie.Value" />.</summary>
		/// <param name="name">The name of a <see cref="T:System.Net.Cookie" />. The following characters must not be used inside <paramref name="name" />: equal sign, semicolon, comma, newline (\n), return (\r), tab (\t), and space character. The dollar sign character ("$") cannot be the first character. </param>
		/// <param name="value">The value of a <see cref="T:System.Net.Cookie" />. The following characters must not be used inside <paramref name="value" />: semicolon, comma. </param>
		/// <exception cref="T:System.Net.CookieException">The <paramref name="name" /> parameter is null. -or- The <paramref name="name" /> parameter is of zero length. -or- The <paramref name="name" /> parameter contains an invalid character.-or- The <paramref name="value" /> parameter is null .-or - The <paramref name="value" /> parameter contains a string not enclosed in quotes that contains an invalid character. </exception>
		public Cookie(string name, string value) : this()
		{
			this.Name = name;
			this.Value = value;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Cookie" /> class with a specified <see cref="P:System.Net.Cookie.Name" />, <see cref="P:System.Net.Cookie.Value" />, and <see cref="P:System.Net.Cookie.Path" />.</summary>
		/// <param name="name">The name of a <see cref="T:System.Net.Cookie" />. The following characters must not be used inside <paramref name="name" />: equal sign, semicolon, comma, newline (\n), return (\r), tab (\t), and space character. The dollar sign character ("$") cannot be the first character. </param>
		/// <param name="value">The value of a <see cref="T:System.Net.Cookie" />. The following characters must not be used inside <paramref name="value" />: semicolon, comma. </param>
		/// <param name="path">The subset of URIs on the origin server to which this <see cref="T:System.Net.Cookie" /> applies. The default value is "/". </param>
		/// <exception cref="T:System.Net.CookieException">The <paramref name="name" /> parameter is null. -or- The <paramref name="name" /> parameter is of zero length. -or- The <paramref name="name" /> parameter contains an invalid character.-or- The <paramref name="value" /> parameter is null .-or - The <paramref name="value" /> parameter contains a string not enclosed in quotes that contains an invalid character.</exception>
		public Cookie(string name, string value, string path) : this(name, value)
		{
			this.Path = path;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Cookie" /> class with a specified <see cref="P:System.Net.Cookie.Name" />, <see cref="P:System.Net.Cookie.Value" />, <see cref="P:System.Net.Cookie.Path" />, and <see cref="P:System.Net.Cookie.Domain" />.</summary>
		/// <param name="name">The name of a <see cref="T:System.Net.Cookie" />. The following characters must not be used inside <paramref name="name" />: equal sign, semicolon, comma, newline (\n), return (\r), tab (\t), and space character. The dollar sign character ("$") cannot be the first character. </param>
		/// <param name="value">The value of a <see cref="T:System.Net.Cookie" /> object. The following characters must not be used inside <paramref name="value" />: semicolon, comma. </param>
		/// <param name="path">The subset of URIs on the origin server to which this <see cref="T:System.Net.Cookie" /> applies. The default value is "/". </param>
		/// <param name="domain">The optional internet domain for which this <see cref="T:System.Net.Cookie" /> is valid. The default value is the host this <see cref="T:System.Net.Cookie" /> has been received from. </param>
		/// <exception cref="T:System.Net.CookieException">The <paramref name="name" /> parameter is null. -or- The <paramref name="name" /> parameter is of zero length. -or- The <paramref name="name" /> parameter contains an invalid character.-or- The <paramref name="value" /> parameter is null .-or - The <paramref name="value" /> parameter contains a string not enclosed in quotes that contains an invalid character.</exception>
		public Cookie(string name, string value, string path, string domain) : this(name, value, path)
		{
			this.Domain = domain;
		}

		/// <summary>Gets or sets a comment that the server can add to a <see cref="T:System.Net.Cookie" />.</summary>
		/// <returns>An optional comment to document intended usage for this <see cref="T:System.Net.Cookie" />.</returns>
		public string Comment
		{
			get
			{
				return this.comment;
			}
			set
			{
				this.comment = ((value != null) ? value : string.Empty);
			}
		}

		/// <summary>Gets or sets a URI comment that the server can provide with a <see cref="T:System.Net.Cookie" />.</summary>
		/// <returns>An optional comment that represents the intended usage of the URI reference for this <see cref="T:System.Net.Cookie" />. The value must conform to URI format.</returns>
		public System.Uri CommentUri
		{
			get
			{
				return this.commentUri;
			}
			set
			{
				this.commentUri = value;
			}
		}

		/// <summary>Gets or sets the discard flag set by the server.</summary>
		/// <returns>true if the client is to discard the <see cref="T:System.Net.Cookie" /> at the end of the current session; otherwise, false. The default is false.</returns>
		public bool Discard
		{
			get
			{
				return this.discard;
			}
			set
			{
				this.discard = value;
			}
		}

		/// <summary>Gets or sets the URI for which the <see cref="T:System.Net.Cookie" /> is valid.</summary>
		/// <returns>The URI for which the <see cref="T:System.Net.Cookie" /> is valid.</returns>
		public string Domain
		{
			get
			{
				return this.domain;
			}
			set
			{
				if (Cookie.IsNullOrEmpty(value))
				{
					this.domain = string.Empty;
					this.ExactDomain = true;
				}
				else
				{
					this.domain = value;
					this.ExactDomain = (value[0] != '.');
				}
			}
		}

		internal bool ExactDomain
		{
			get
			{
				return this.exact_domain;
			}
			set
			{
				this.exact_domain = value;
			}
		}

		/// <summary>Gets or sets the current state of the <see cref="T:System.Net.Cookie" />.</summary>
		/// <returns>true if the <see cref="T:System.Net.Cookie" /> has expired; otherwise, false. The default is false.</returns>
		public bool Expired
		{
			get
			{
				return this.expires <= DateTime.Now && this.expires != DateTime.MinValue;
			}
			set
			{
				if (value)
				{
					this.expires = DateTime.Now;
				}
			}
		}

		/// <summary>Gets or sets the expiration date and time for the <see cref="T:System.Net.Cookie" /> as a <see cref="T:System.DateTime" />.</summary>
		/// <returns>The expiration date and time for the <see cref="T:System.Net.Cookie" /> as a <see cref="T:System.DateTime" /> instance.</returns>
		public DateTime Expires
		{
			get
			{
				return this.expires;
			}
			set
			{
				this.expires = value;
			}
		}

		/// <summary>Determines whether a page script or other active content can access this cookie.</summary>
		/// <returns>Boolean value that determines whether a page script or other active content can access this cookie.</returns>
		public bool HttpOnly
		{
			get
			{
				return this.httpOnly;
			}
			set
			{
				this.httpOnly = value;
			}
		}

		/// <summary>Gets or sets the name for the <see cref="T:System.Net.Cookie" />.</summary>
		/// <returns>The name for the <see cref="T:System.Net.Cookie" />.</returns>
		/// <exception cref="T:System.Net.CookieException">The value specified for a set operation is null or the empty string- or -The value specified for a set operation contained an illegal character. The following characters must not be used inside the <see cref="P:System.Net.Cookie.Name" /> property: equal sign, semicolon, comma, newline (\n), return (\r), tab (\t), and space character. The dollar sign character ("$") cannot be the first character.</exception>
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (Cookie.IsNullOrEmpty(value))
				{
					throw new CookieException("Name cannot be empty");
				}
				if (value[0] == '$' || value.IndexOfAny(Cookie.reservedCharsName) != -1)
				{
					this.name = string.Empty;
					throw new CookieException("Name contains invalid characters");
				}
				this.name = value;
			}
		}

		/// <summary>Gets or sets the URIs to which the <see cref="T:System.Net.Cookie" /> applies.</summary>
		/// <returns>The URIs to which the <see cref="T:System.Net.Cookie" /> applies.</returns>
		public string Path
		{
			get
			{
				return (this.path != null) ? this.path : string.Empty;
			}
			set
			{
				this.path = ((value != null) ? value : string.Empty);
			}
		}

		/// <summary>Gets or sets a list of TCP ports that the <see cref="T:System.Net.Cookie" /> applies to.</summary>
		/// <returns>The list of TCP ports that the <see cref="T:System.Net.Cookie" /> applies to.</returns>
		/// <exception cref="T:System.Net.CookieException">The value specified for a set operation could not be parsed or is not enclosed in double quotes. </exception>
		public string Port
		{
			get
			{
				return this.port;
			}
			set
			{
				if (Cookie.IsNullOrEmpty(value))
				{
					this.port = string.Empty;
					return;
				}
				if (value[0] != '"' || value[value.Length - 1] != '"')
				{
					throw new CookieException("The 'Port'='" + value + "' part of the cookie is invalid. Port must be enclosed by double quotes.");
				}
				this.port = value;
				string[] array = this.port.Split(Cookie.portSeparators);
				this.ports = new int[array.Length];
				for (int i = 0; i < this.ports.Length; i++)
				{
					this.ports[i] = int.MinValue;
					if (array[i].Length != 0)
					{
						try
						{
							this.ports[i] = int.Parse(array[i]);
						}
						catch (Exception e)
						{
							throw new CookieException("The 'Port'='" + value + "' part of the cookie is invalid. Invalid value: " + array[i], e);
						}
					}
				}
				this.Version = 1;
			}
		}

		internal int[] Ports
		{
			get
			{
				return this.ports;
			}
		}

		/// <summary>Gets or sets the security level of a <see cref="T:System.Net.Cookie" />.</summary>
		/// <returns>true if the client is only to return the cookie in subsequent requests if those requests use Secure Hypertext Transfer Protocol (HTTPS); otherwise, false. The default is false.</returns>
		public bool Secure
		{
			get
			{
				return this.secure;
			}
			set
			{
				this.secure = value;
			}
		}

		/// <summary>Gets the time when the cookie was issued as a <see cref="T:System.DateTime" />.</summary>
		/// <returns>The time when the cookie was issued as a <see cref="T:System.DateTime" />.</returns>
		public DateTime TimeStamp
		{
			get
			{
				return this.timestamp;
			}
		}

		/// <summary>Gets or sets the <see cref="P:System.Net.Cookie.Value" /> for the <see cref="T:System.Net.Cookie" />.</summary>
		/// <returns>The <see cref="P:System.Net.Cookie.Value" /> for the <see cref="T:System.Net.Cookie" />.</returns>
		public string Value
		{
			get
			{
				return this.val;
			}
			set
			{
				if (value == null)
				{
					this.val = string.Empty;
					return;
				}
				this.val = value;
			}
		}

		/// <summary>Gets or sets the version of HTTP state maintenance to which the cookie conforms.</summary>
		/// <returns>The version of HTTP state maintenance to which the cookie conforms.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified for a version is not allowed. </exception>
		public int Version
		{
			get
			{
				return this.version;
			}
			set
			{
				if (value < 0 || value > 10)
				{
					this.version = 0;
				}
				else
				{
					this.version = value;
				}
			}
		}

		/// <summary>Overrides the <see cref="M:System.Object.Equals(System.Object)" /> method.</summary>
		/// <returns>Returns true if the <see cref="T:System.Net.Cookie" /> is equal to <paramref name="comparand" />. Two <see cref="T:System.Net.Cookie" /> instances are equal if their <see cref="P:System.Net.Cookie.Name" />, <see cref="P:System.Net.Cookie.Value" />, <see cref="P:System.Net.Cookie.Path" />, <see cref="P:System.Net.Cookie.Domain" />, and <see cref="P:System.Net.Cookie.Version" /> properties are equal. <see cref="P:System.Net.Cookie.Name" /> and <see cref="P:System.Net.Cookie.Domain" /> string comparisons are case-insensitive.</returns>
		/// <param name="comparand">A reference to a <see cref="T:System.Net.Cookie" />. </param>
		public override bool Equals(object obj)
		{
			Cookie cookie = obj as Cookie;
			return cookie != null && string.Compare(this.name, cookie.name, true, CultureInfo.InvariantCulture) == 0 && string.Compare(this.val, cookie.val, false, CultureInfo.InvariantCulture) == 0 && string.Compare(this.Path, cookie.Path, false, CultureInfo.InvariantCulture) == 0 && string.Compare(this.domain, cookie.domain, true, CultureInfo.InvariantCulture) == 0 && this.version == cookie.version;
		}

		/// <summary>Overrides the <see cref="M:System.Object.GetHashCode" /> method.</summary>
		/// <returns>The 32-bit signed integer hash code for this instance.</returns>
		public override int GetHashCode()
		{
			return Cookie.hash(CaseInsensitiveHashCodeProvider.DefaultInvariant.GetHashCode(this.name), this.val.GetHashCode(), this.Path.GetHashCode(), CaseInsensitiveHashCodeProvider.DefaultInvariant.GetHashCode(this.domain), this.version);
		}

		private static int hash(int i, int j, int k, int l, int m)
		{
			return i ^ (j << 13 | j >> 19) ^ (k << 26 | k >> 6) ^ (l << 7 | l >> 25) ^ (m << 20 | m >> 12);
		}

		/// <summary>Overrides the <see cref="M:System.Object.ToString" /> method.</summary>
		/// <returns>Returns a string representation of this <see cref="T:System.Net.Cookie" /> object that is suitable for including in a HTTP Cookie: request header.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public override string ToString()
		{
			return this.ToString(null);
		}

		internal string ToString(System.Uri uri)
		{
			if (this.name.Length == 0)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(64);
			if (this.version > 0)
			{
				stringBuilder.Append("$Version=").Append(this.version).Append("; ");
			}
			stringBuilder.Append(this.name).Append("=").Append(this.val);
			if (this.version == 0)
			{
				return stringBuilder.ToString();
			}
			if (!Cookie.IsNullOrEmpty(this.path))
			{
				stringBuilder.Append("; $Path=").Append(this.path);
			}
			else if (uri != null)
			{
				stringBuilder.Append("; $Path=/").Append(this.path);
			}
			bool flag = uri == null || uri.Host != this.domain;
			if (flag && !Cookie.IsNullOrEmpty(this.domain))
			{
				stringBuilder.Append("; $Domain=").Append(this.domain);
			}
			if (this.port != null && this.port.Length != 0)
			{
				stringBuilder.Append("; $Port=").Append(this.port);
			}
			return stringBuilder.ToString();
		}

		internal string ToClientString()
		{
			if (this.name.Length == 0)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(64);
			if (this.version > 0)
			{
				stringBuilder.Append("Version=").Append(this.version).Append(";");
			}
			stringBuilder.Append(this.name).Append("=").Append(this.val);
			if (this.path != null && this.path.Length != 0)
			{
				stringBuilder.Append(";Path=").Append(this.QuotedString(this.path));
			}
			if (this.domain != null && this.domain.Length != 0)
			{
				stringBuilder.Append(";Domain=").Append(this.QuotedString(this.domain));
			}
			if (this.port != null && this.port.Length != 0)
			{
				stringBuilder.Append(";Port=").Append(this.port);
			}
			return stringBuilder.ToString();
		}

		private string QuotedString(string value)
		{
			if (this.version == 0 || this.IsToken(value))
			{
				return value;
			}
			return "\"" + value.Replace("\"", "\\\"") + "\"";
		}

		private bool IsToken(string value)
		{
			int length = value.Length;
			for (int i = 0; i < length; i++)
			{
				char c = value[i];
				if (c < ' ' || c >= '\u007f' || Cookie.tspecials.IndexOf(c) != -1)
				{
					return false;
				}
			}
			return true;
		}

		private static bool IsNullOrEmpty(string s)
		{
			return s == null || s.Length == 0;
		}
	}
}
