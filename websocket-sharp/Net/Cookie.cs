using System;
using System.Globalization;
using System.Text;

namespace WebSocketSharp.Net
{
	[Serializable]
	public sealed class Cookie
	{
		private static char[] _reservedCharsForName = new char[]
		{
			' ',
			'=',
			';',
			',',
			'\n',
			'\r',
			'\t'
		};

		private static char[] _reservedCharsForValue = new char[]
		{
			';',
			','
		};

		private string _comment;

		private Uri _commentUri;

		private bool _discard;

		private string _domain;

		private DateTime _expires;

		private bool _httpOnly;

		private string _name;

		private string _path;

		private string _port;

		private int[] _ports;

		private bool _secure;

		private DateTime _timestamp;

		private string _value;

		private int _version;

		public Cookie()
		{
			this._comment = string.Empty;
			this._domain = string.Empty;
			this._expires = DateTime.MinValue;
			this._name = string.Empty;
			this._path = string.Empty;
			this._port = string.Empty;
			this._ports = new int[0];
			this._timestamp = DateTime.Now;
			this._value = string.Empty;
			this._version = 0;
		}

		public Cookie(string name, string value) : this()
		{
			this.Name = name;
			this.Value = value;
		}

		public Cookie(string name, string value, string path) : this(name, value)
		{
			this.Path = path;
		}

		public Cookie(string name, string value, string path, string domain) : this(name, value, path)
		{
			this.Domain = domain;
		}

		internal bool ExactDomain { get; set; }

		internal int MaxAge
		{
			get
			{
				if (this._expires == DateTime.MinValue)
				{
					return 0;
				}
				DateTime d = (this._expires.Kind == DateTimeKind.Local) ? this._expires : this._expires.ToLocalTime();
				TimeSpan t = d - DateTime.Now;
				return (!(t > TimeSpan.Zero)) ? 0 : ((int)t.TotalSeconds);
			}
		}

		internal int[] Ports
		{
			get
			{
				return this._ports;
			}
		}

		public string Comment
		{
			get
			{
				return this._comment;
			}
			set
			{
				this._comment = (value ?? string.Empty);
			}
		}

		public Uri CommentUri
		{
			get
			{
				return this._commentUri;
			}
			set
			{
				this._commentUri = value;
			}
		}

		public bool Discard
		{
			get
			{
				return this._discard;
			}
			set
			{
				this._discard = value;
			}
		}

		public string Domain
		{
			get
			{
				return this._domain;
			}
			set
			{
				if (value.IsNullOrEmpty())
				{
					this._domain = string.Empty;
					this.ExactDomain = true;
				}
				else
				{
					this._domain = value;
					this.ExactDomain = (value[0] != '.');
				}
			}
		}

		public bool Expired
		{
			get
			{
				return this._expires != DateTime.MinValue && this._expires <= DateTime.Now;
			}
			set
			{
				this._expires = ((!value) ? DateTime.MinValue : DateTime.Now);
			}
		}

		public DateTime Expires
		{
			get
			{
				return this._expires;
			}
			set
			{
				this._expires = value;
			}
		}

		public bool HttpOnly
		{
			get
			{
				return this._httpOnly;
			}
			set
			{
				this._httpOnly = value;
			}
		}

		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				string message;
				if (!Cookie.canSetName(value, out message))
				{
					throw new CookieException(message);
				}
				this._name = value;
			}
		}

		public string Path
		{
			get
			{
				return this._path;
			}
			set
			{
				this._path = (value ?? string.Empty);
			}
		}

		public string Port
		{
			get
			{
				return this._port;
			}
			set
			{
				if (value.IsNullOrEmpty())
				{
					this._port = string.Empty;
					this._ports = new int[0];
					return;
				}
				if (!value.IsEnclosedIn('"'))
				{
					throw new CookieException("The value of Port attribute must be enclosed in double quotes.");
				}
				string arg;
				if (!Cookie.tryCreatePorts(value, out this._ports, out arg))
				{
					throw new CookieException(string.Format("The value specified for a Port attribute contains an invalid value: {0}", arg));
				}
				this._port = value;
			}
		}

		public bool Secure
		{
			get
			{
				return this._secure;
			}
			set
			{
				this._secure = value;
			}
		}

		public DateTime TimeStamp
		{
			get
			{
				return this._timestamp;
			}
		}

		public string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				string message;
				if (!Cookie.canSetValue(value, out message))
				{
					throw new CookieException(message);
				}
				this._value = ((value.Length <= 0) ? "\"\"" : value);
			}
		}

		public int Version
		{
			get
			{
				return this._version;
			}
			set
			{
				if (value < 0 || value > 1)
				{
					throw new ArgumentOutOfRangeException("value", "Must be 0 or 1.");
				}
				this._version = value;
			}
		}

		private static bool canSetName(string name, out string message)
		{
			if (name.IsNullOrEmpty())
			{
				message = "Name must not be null or empty.";
				return false;
			}
			if (name[0] == '$' || name.Contains(Cookie._reservedCharsForName))
			{
				message = "The value specified for a Name contains an invalid character.";
				return false;
			}
			message = string.Empty;
			return true;
		}

		private static bool canSetValue(string value, out string message)
		{
			if (value == null)
			{
				message = "Value must not be null.";
				return false;
			}
			if (value.Contains(Cookie._reservedCharsForValue) && !value.IsEnclosedIn('"'))
			{
				message = "The value specified for a Value contains an invalid character.";
				return false;
			}
			message = string.Empty;
			return true;
		}

		private static int hash(int i, int j, int k, int l, int m)
		{
			return i ^ (j << 13 | j >> 19) ^ (k << 26 | k >> 6) ^ (l << 7 | l >> 25) ^ (m << 20 | m >> 12);
		}

		private string toResponseStringVersion0()
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.AppendFormat("{0}={1}", this._name, this._value);
			if (this._expires != DateTime.MinValue)
			{
				stringBuilder.AppendFormat("; Expires={0}", this._expires.ToUniversalTime().ToString("ddd, dd'-'MMM'-'yyyy HH':'mm':'ss 'GMT'", CultureInfo.CreateSpecificCulture("en-US")));
			}
			if (!this._path.IsNullOrEmpty())
			{
				stringBuilder.AppendFormat("; Path={0}", this._path);
			}
			if (!this._domain.IsNullOrEmpty())
			{
				stringBuilder.AppendFormat("; Domain={0}", this._domain);
			}
			if (this._secure)
			{
				stringBuilder.Append("; Secure");
			}
			if (this._httpOnly)
			{
				stringBuilder.Append("; HttpOnly");
			}
			return stringBuilder.ToString();
		}

		private string toResponseStringVersion1()
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.AppendFormat("{0}={1}; Version={2}", this._name, this._value, this._version);
			if (this._expires != DateTime.MinValue)
			{
				stringBuilder.AppendFormat("; Max-Age={0}", this.MaxAge);
			}
			if (!this._path.IsNullOrEmpty())
			{
				stringBuilder.AppendFormat("; Path={0}", this._path);
			}
			if (!this._domain.IsNullOrEmpty())
			{
				stringBuilder.AppendFormat("; Domain={0}", this._domain);
			}
			if (!this._port.IsNullOrEmpty())
			{
				if (this._port == "\"\"")
				{
					stringBuilder.Append("; Port");
				}
				else
				{
					stringBuilder.AppendFormat("; Port={0}", this._port);
				}
			}
			if (!this._comment.IsNullOrEmpty())
			{
				stringBuilder.AppendFormat("; Comment={0}", this._comment.UrlEncode());
			}
			if (this._commentUri != null)
			{
				stringBuilder.AppendFormat("; CommentURL={0}", this._commentUri.OriginalString.Quote());
			}
			if (this._discard)
			{
				stringBuilder.Append("; Discard");
			}
			if (this._secure)
			{
				stringBuilder.Append("; Secure");
			}
			return stringBuilder.ToString();
		}

		private static bool tryCreatePorts(string value, out int[] result, out string parseError)
		{
			string[] array = value.Trim(new char[]
			{
				'"'
			}).Split(new char[]
			{
				','
			});
			int[] array2 = new int[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = int.MinValue;
				string text = array[i].Trim();
				if (text.Length != 0)
				{
					if (!int.TryParse(text, out array2[i]))
					{
						result = new int[0];
						parseError = text;
						return false;
					}
				}
			}
			result = array2;
			parseError = string.Empty;
			return true;
		}

		internal string ToRequestString(Uri uri)
		{
			if (this._name.Length == 0)
			{
				return string.Empty;
			}
			if (this._version == 0)
			{
				return string.Format("{0}={1}", this._name, this._value);
			}
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.AppendFormat("$Version={0}; {1}={2}", this._version, this._name, this._value);
			if (!this._path.IsNullOrEmpty())
			{
				stringBuilder.AppendFormat("; $Path={0}", this._path);
			}
			else if (uri != null)
			{
				stringBuilder.AppendFormat("; $Path={0}", uri.GetAbsolutePath());
			}
			else
			{
				stringBuilder.Append("; $Path=/");
			}
			bool flag = uri == null || uri.Host != this._domain;
			if (flag && !this._domain.IsNullOrEmpty())
			{
				stringBuilder.AppendFormat("; $Domain={0}", this._domain);
			}
			if (!this._port.IsNullOrEmpty())
			{
				if (this._port == "\"\"")
				{
					stringBuilder.Append("; $Port");
				}
				else
				{
					stringBuilder.AppendFormat("; $Port={0}", this._port);
				}
			}
			return stringBuilder.ToString();
		}

		internal string ToResponseString()
		{
			return (this._name.Length <= 0) ? string.Empty : ((this._version != 0) ? this.toResponseStringVersion1() : this.toResponseStringVersion0());
		}

		public override bool Equals(object comparand)
		{
			Cookie cookie = comparand as Cookie;
			return cookie != null && this._name.Equals(cookie.Name, StringComparison.InvariantCultureIgnoreCase) && this._value.Equals(cookie.Value, StringComparison.InvariantCulture) && this._path.Equals(cookie.Path, StringComparison.InvariantCulture) && this._domain.Equals(cookie.Domain, StringComparison.InvariantCultureIgnoreCase) && this._version == cookie.Version;
		}

		public override int GetHashCode()
		{
			return Cookie.hash(StringComparer.InvariantCultureIgnoreCase.GetHashCode(this._name), this._value.GetHashCode(), this._path.GetHashCode(), StringComparer.InvariantCultureIgnoreCase.GetHashCode(this._domain), this._version);
		}

		public override string ToString()
		{
			return this.ToRequestString(null);
		}
	}
}
