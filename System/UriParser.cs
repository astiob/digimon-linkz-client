using System;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
	/// <summary>Parses a new URI scheme. This is an abstract class.</summary>
	public abstract class UriParser
	{
		private static object lock_object = new object();

		private static Hashtable table;

		internal string scheme_name;

		private int default_port;

		private static readonly System.Text.RegularExpressions.Regex uri_regex = new System.Text.RegularExpressions.Regex("^(([^:/?#]+):)?(//([^/?#]*))?([^?#]*)(\\?([^#]*))?(#(.*))?");

		private static readonly System.Text.RegularExpressions.Regex auth_regex = new System.Text.RegularExpressions.Regex("^(([^@]+)@)?(.*?)(:([0-9]+))?$");

		private static System.Text.RegularExpressions.Match ParseAuthority(System.Text.RegularExpressions.Group g)
		{
			return System.UriParser.auth_regex.Match(g.Value);
		}

		/// <summary>Gets the components from a URI.</summary>
		/// <returns>A string that contains the components.</returns>
		/// <param name="uri">The URI to parse.</param>
		/// <param name="components">The <see cref="T:System.UriComponents" /> to retrieve from <paramref name="uri" />.</param>
		/// <param name="format">One of the <see cref="T:System.UriFormat" /> values that controls how special characters are escaped.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="uriFormat" /> is invalid.- or -<paramref name="uriComponents" /> is not a combination of valid <see cref="T:System.UriComponents" /> values. </exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="uri" /> requires user-driven parsing- or -<paramref name="uri" /> is not an absolute URI. Relative URIs cannot be used with this method.</exception>
		protected internal virtual string GetComponents(System.Uri uri, System.UriComponents components, System.UriFormat format)
		{
			if (format < System.UriFormat.UriEscaped || format > System.UriFormat.SafeUnescaped)
			{
				throw new ArgumentOutOfRangeException("format");
			}
			System.Text.RegularExpressions.Match match = System.UriParser.uri_regex.Match(uri.OriginalString);
			string value = this.scheme_name;
			int defaultPort = this.default_port;
			if (value == null || value == "*")
			{
				value = match.Groups[2].Value;
				defaultPort = System.Uri.GetDefaultPort(value);
			}
			else if (string.Compare(value, match.Groups[2].Value, true) != 0)
			{
				throw new SystemException("URI Parser: scheme mismatch: " + value + " vs. " + match.Groups[2].Value);
			}
			System.UriComponents uriComponents = components;
			switch (uriComponents)
			{
			case System.UriComponents.Scheme:
				return value;
			case System.UriComponents.UserInfo:
				return System.UriParser.ParseAuthority(match.Groups[4]).Groups[2].Value;
			default:
			{
				if (uriComponents == System.UriComponents.Path)
				{
					return this.Format(this.IgnoreFirstCharIf(match.Groups[5].Value, '/'), format);
				}
				if (uriComponents == System.UriComponents.Query)
				{
					return this.Format(match.Groups[7].Value, format);
				}
				if (uriComponents == System.UriComponents.Fragment)
				{
					return this.Format(match.Groups[9].Value, format);
				}
				if (uriComponents != System.UriComponents.StrongPort)
				{
					if (uriComponents == System.UriComponents.SerializationInfoString)
					{
						components = System.UriComponents.AbsoluteUri;
					}
					System.Text.RegularExpressions.Match match2 = System.UriParser.ParseAuthority(match.Groups[4]);
					StringBuilder stringBuilder = new StringBuilder();
					if ((components & System.UriComponents.Scheme) != (System.UriComponents)0)
					{
						stringBuilder.Append(value);
						stringBuilder.Append(System.Uri.GetSchemeDelimiter(value));
					}
					if ((components & System.UriComponents.UserInfo) != (System.UriComponents)0)
					{
						stringBuilder.Append(match2.Groups[1].Value);
					}
					if ((components & System.UriComponents.Host) != (System.UriComponents)0)
					{
						stringBuilder.Append(match2.Groups[3].Value);
					}
					if ((components & System.UriComponents.StrongPort) != (System.UriComponents)0)
					{
						System.Text.RegularExpressions.Group group = match2.Groups[4];
						stringBuilder.Append((!group.Success) ? (":" + defaultPort) : group.Value);
					}
					if ((components & System.UriComponents.Port) != (System.UriComponents)0)
					{
						string value2 = match2.Groups[5].Value;
						if (value2 != null && value2 != string.Empty && value2 != defaultPort.ToString())
						{
							stringBuilder.Append(match2.Groups[4].Value);
						}
					}
					if ((components & System.UriComponents.Path) != (System.UriComponents)0)
					{
						stringBuilder.Append(match.Groups[5]);
					}
					if ((components & System.UriComponents.Query) != (System.UriComponents)0)
					{
						stringBuilder.Append(match.Groups[6]);
					}
					if ((components & System.UriComponents.Fragment) != (System.UriComponents)0)
					{
						stringBuilder.Append(match.Groups[8]);
					}
					return this.Format(stringBuilder.ToString(), format);
				}
				System.Text.RegularExpressions.Group group2 = System.UriParser.ParseAuthority(match.Groups[4]).Groups[5];
				return (!group2.Success) ? defaultPort.ToString() : group2.Value;
			}
			case System.UriComponents.Host:
				return System.UriParser.ParseAuthority(match.Groups[4]).Groups[3].Value;
			case System.UriComponents.Port:
			{
				string value3 = System.UriParser.ParseAuthority(match.Groups[4]).Groups[5].Value;
				if (value3 != null && value3 != string.Empty && value3 != defaultPort.ToString())
				{
					return value3;
				}
				return string.Empty;
			}
			}
		}

		/// <summary>Initialize the state of the parser and validate the URI.</summary>
		/// <param name="uri">The T:System.Uri to validate.</param>
		/// <param name="parsingError">Validation errors, if any.</param>
		protected internal virtual void InitializeAndValidate(System.Uri uri, out System.UriFormatException parsingError)
		{
			if (uri.Scheme != this.scheme_name && this.scheme_name != "*")
			{
				parsingError = new System.UriFormatException("The argument Uri's scheme does not match");
			}
			else
			{
				parsingError = null;
			}
		}

		/// <summary>Determines whether <paramref name="baseUri" /> is a base URI for <paramref name="relativeUri" />.</summary>
		/// <returns>true if <paramref name="baseUri" /> is a base URI for <paramref name="relativeUri" />; otherwise, false.</returns>
		/// <param name="baseUri">The base URI.</param>
		/// <param name="relativeUri">The URI to test.</param>
		protected internal virtual bool IsBaseOf(System.Uri baseUri, System.Uri relativeUri)
		{
			if (System.Uri.Compare(baseUri, relativeUri, System.UriComponents.Scheme | System.UriComponents.UserInfo | System.UriComponents.Host | System.UriComponents.Port, System.UriFormat.Unescaped, StringComparison.InvariantCultureIgnoreCase) != 0)
			{
				return false;
			}
			string localPath = baseUri.LocalPath;
			int length = localPath.LastIndexOf('/') + 1;
			return string.Compare(localPath, 0, relativeUri.LocalPath, 0, length, StringComparison.InvariantCultureIgnoreCase) == 0;
		}

		/// <summary>Indicates whether a URI is well-formed.</summary>
		/// <returns>true if <paramref name="uri" /> is well-formed; otherwise, false.</returns>
		/// <param name="uri">The URI to check.</param>
		protected internal virtual bool IsWellFormedOriginalString(System.Uri uri)
		{
			return uri.IsWellFormedOriginalString();
		}

		/// <summary>Invoked by a <see cref="T:System.Uri" /> constructor to get a <see cref="T:System.UriParser" /> instance</summary>
		/// <returns>A <see cref="T:System.UriParser" /> for the constructed <see cref="T:System.Uri" />.</returns>
		protected internal virtual System.UriParser OnNewUri()
		{
			return this;
		}

		/// <summary>Invoked by the Framework when a <see cref="T:System.UriParser" /> method is registered.</summary>
		/// <param name="schemeName">The scheme that is associated with this <see cref="T:System.UriParser" />.</param>
		/// <param name="defaultPort">The port number of the scheme.</param>
		[MonoTODO]
		protected virtual void OnRegister(string schemeName, int defaultPort)
		{
		}

		/// <summary>Called by <see cref="T:System.Uri" /> constructors and <see cref="Overload:System.Uri.TryCreate" /> to resolve a relative URI.</summary>
		/// <returns>The string of the resolved relative <see cref="T:System.Uri" />.</returns>
		/// <param name="baseUri">A base URI.</param>
		/// <param name="relativeUri">A relative URI.</param>
		/// <param name="parsingError">Errors during the resolve process, if any.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="baseUri" /> parameter is not an absolute <see cref="T:System.Uri" />- or -<paramref name="baseUri" /> parameter requires user-driven parsing.</exception>
		[MonoTODO]
		protected internal virtual string Resolve(System.Uri baseUri, System.Uri relativeUri, out System.UriFormatException parsingError)
		{
			throw new NotImplementedException();
		}

		internal string SchemeName
		{
			get
			{
				return this.scheme_name;
			}
			set
			{
				this.scheme_name = value;
			}
		}

		internal int DefaultPort
		{
			get
			{
				return this.default_port;
			}
			set
			{
				this.default_port = value;
			}
		}

		private string IgnoreFirstCharIf(string s, char c)
		{
			if (s.Length == 0)
			{
				return string.Empty;
			}
			if (s[0] == c)
			{
				return s.Substring(1);
			}
			return s;
		}

		private string Format(string s, System.UriFormat format)
		{
			if (s.Length == 0)
			{
				return string.Empty;
			}
			switch (format)
			{
			case System.UriFormat.UriEscaped:
				return System.Uri.EscapeString(s, false, true, true);
			case System.UriFormat.Unescaped:
				return System.Uri.Unescape(s, false);
			case System.UriFormat.SafeUnescaped:
				s = System.Uri.Unescape(s, false);
				return s;
			default:
				throw new ArgumentOutOfRangeException("format");
			}
		}

		private static void CreateDefaults()
		{
			if (System.UriParser.table != null)
			{
				return;
			}
			Hashtable hashtable = new Hashtable();
			System.UriParser.InternalRegister(hashtable, new DefaultUriParser(), System.Uri.UriSchemeFile, -1);
			System.UriParser.InternalRegister(hashtable, new DefaultUriParser(), System.Uri.UriSchemeFtp, 21);
			System.UriParser.InternalRegister(hashtable, new DefaultUriParser(), System.Uri.UriSchemeGopher, 70);
			System.UriParser.InternalRegister(hashtable, new DefaultUriParser(), System.Uri.UriSchemeHttp, 80);
			System.UriParser.InternalRegister(hashtable, new DefaultUriParser(), System.Uri.UriSchemeHttps, 443);
			System.UriParser.InternalRegister(hashtable, new DefaultUriParser(), System.Uri.UriSchemeMailto, 25);
			System.UriParser.InternalRegister(hashtable, new DefaultUriParser(), System.Uri.UriSchemeNetPipe, -1);
			System.UriParser.InternalRegister(hashtable, new DefaultUriParser(), System.Uri.UriSchemeNetTcp, -1);
			System.UriParser.InternalRegister(hashtable, new DefaultUriParser(), System.Uri.UriSchemeNews, 119);
			System.UriParser.InternalRegister(hashtable, new DefaultUriParser(), System.Uri.UriSchemeNntp, 119);
			System.UriParser.InternalRegister(hashtable, new DefaultUriParser(), "ldap", 389);
			object obj = System.UriParser.lock_object;
			lock (obj)
			{
				if (System.UriParser.table == null)
				{
					System.UriParser.table = hashtable;
				}
			}
		}

		/// <summary>Indicates whether the parser for a scheme is registered.</summary>
		/// <returns>true if <paramref name="schemeName" /> has been registered; otherwise, false.</returns>
		/// <param name="schemeName">The scheme name to check.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="schemeName" /> parameter is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="schemeName" /> parameter is not valid. </exception>
		public static bool IsKnownScheme(string schemeName)
		{
			if (schemeName == null)
			{
				throw new ArgumentNullException("schemeName");
			}
			if (schemeName.Length == 0)
			{
				throw new ArgumentOutOfRangeException("schemeName");
			}
			System.UriParser.CreateDefaults();
			string key = schemeName.ToLower(CultureInfo.InvariantCulture);
			return System.UriParser.table[key] != null;
		}

		private static void InternalRegister(Hashtable table, System.UriParser uriParser, string schemeName, int defaultPort)
		{
			uriParser.SchemeName = schemeName;
			uriParser.DefaultPort = defaultPort;
			if (uriParser is System.GenericUriParser)
			{
				table.Add(schemeName, uriParser);
			}
			else
			{
				table.Add(schemeName, new DefaultUriParser
				{
					SchemeName = schemeName,
					DefaultPort = defaultPort
				});
			}
			uriParser.OnRegister(schemeName, defaultPort);
		}

		/// <summary>Associates a scheme and port number with a <see cref="T:System.UriParser" />.</summary>
		/// <param name="uriParser">The URI parser to register.</param>
		/// <param name="schemeName">The name of the scheme that is associated with this parser.</param>
		/// <param name="defaultPort">The default port number for the specified scheme.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="uriParser" /> parameter is null- or -<paramref name="schemeName" /> parameter is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="schemeName" /> parameter is not valid- or -<paramref name="defaultPort" /> parameter is not valid. The <paramref name="defaultPort" /> parameter must be not be less than zero or greater than 65,534.</exception>
		public static void Register(System.UriParser uriParser, string schemeName, int defaultPort)
		{
			if (uriParser == null)
			{
				throw new ArgumentNullException("uriParser");
			}
			if (schemeName == null)
			{
				throw new ArgumentNullException("schemeName");
			}
			if (defaultPort < -1 || defaultPort >= 65535)
			{
				throw new ArgumentOutOfRangeException("defaultPort");
			}
			System.UriParser.CreateDefaults();
			string text = schemeName.ToLower(CultureInfo.InvariantCulture);
			if (System.UriParser.table[text] != null)
			{
				string text2 = Locale.GetText("Scheme '{0}' is already registred.");
				throw new InvalidOperationException(text2);
			}
			System.UriParser.InternalRegister(System.UriParser.table, uriParser, text, defaultPort);
		}

		internal static System.UriParser GetParser(string schemeName)
		{
			if (schemeName == null)
			{
				return null;
			}
			System.UriParser.CreateDefaults();
			string key = schemeName.ToLower(CultureInfo.InvariantCulture);
			return (System.UriParser)System.UriParser.table[key];
		}
	}
}
