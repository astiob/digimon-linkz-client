using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using WebSocketSharp.Net;

namespace WebSocketSharp
{
	internal class HandshakeRequest : HandshakeBase
	{
		private string _method;

		private NameValueCollection _queryString;

		private string _rawUrl;

		private Uri _uri;

		private HandshakeRequest()
		{
		}

		public HandshakeRequest(string uriString)
		{
			this._uri = uriString.ToUri();
			this._rawUrl = ((!this._uri.IsAbsoluteUri) ? uriString : this._uri.PathAndQuery);
			this._method = "GET";
			NameValueCollection headers = base.Headers;
			headers["User-Agent"] = "websocket-sharp/1.0";
			headers["Upgrade"] = "websocket";
			headers["Connection"] = "Upgrade";
		}

		public AuthenticationResponse AuthResponse
		{
			get
			{
				string text = base.Headers["Authorization"];
				return (text == null || text.Length <= 0) ? null : AuthenticationResponse.Parse(text);
			}
		}

		public CookieCollection Cookies
		{
			get
			{
				return base.Headers.GetCookies(false);
			}
		}

		public string HttpMethod
		{
			get
			{
				return this._method;
			}
			private set
			{
				this._method = value;
			}
		}

		public bool IsWebSocketRequest
		{
			get
			{
				NameValueCollection headers = base.Headers;
				return this._method == "GET" && base.ProtocolVersion >= HttpVersion.Version11 && headers.Contains("Upgrade", "websocket") && headers.Contains("Connection", "Upgrade");
			}
		}

		public NameValueCollection QueryString
		{
			get
			{
				if (this._queryString == null)
				{
					this._queryString = new NameValueCollection();
					int num = this.RawUrl.IndexOf('?');
					if (num > 0)
					{
						string text = this.RawUrl.Substring(num + 1);
						string[] array = text.Split(new char[]
						{
							'&'
						});
						foreach (string nameAndValue in array)
						{
							KeyValuePair<string, string> nameAndValue2 = nameAndValue.GetNameAndValue("=");
							if (nameAndValue2.Key != null)
							{
								string name = nameAndValue2.Key.UrlDecode();
								string val = nameAndValue2.Value.UrlDecode();
								this._queryString.Add(name, val);
							}
						}
					}
				}
				return this._queryString;
			}
		}

		public string RawUrl
		{
			get
			{
				return this._rawUrl;
			}
			private set
			{
				this._rawUrl = value;
			}
		}

		public Uri RequestUri
		{
			get
			{
				return this._uri;
			}
			private set
			{
				this._uri = value;
			}
		}

		public static HandshakeRequest Parse(string[] headerParts)
		{
			string[] array = headerParts[0].Split(new char[]
			{
				' '
			}, 3);
			if (array.Length != 3)
			{
				throw new ArgumentException("Invalid request line: " + headerParts[0]);
			}
			WebHeaderCollection webHeaderCollection = new WebHeaderCollection();
			for (int i = 1; i < headerParts.Length; i++)
			{
				webHeaderCollection.SetInternal(headerParts[i], false);
			}
			return new HandshakeRequest
			{
				Headers = webHeaderCollection,
				HttpMethod = array[0],
				ProtocolVersion = new Version(array[2].Substring(5)),
				RawUrl = array[1],
				RequestUri = array[1].ToUri()
			};
		}

		public void SetCookies(CookieCollection cookies)
		{
			if (cookies == null || cookies.Count == 0)
			{
				return;
			}
			Cookie[] array = cookies.Sorted.ToArray<Cookie>();
			StringBuilder stringBuilder = new StringBuilder(array[0].ToString(), 64);
			for (int i = 1; i < array.Length; i++)
			{
				if (!array[i].Expired)
				{
					stringBuilder.AppendFormat("; {0}", array[i].ToString());
				}
			}
			base.Headers["Cookie"] = stringBuilder.ToString();
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.AppendFormat("{0} {1} HTTP/{2}{3}", new object[]
			{
				this._method,
				this._rawUrl,
				base.ProtocolVersion,
				"\r\n"
			});
			NameValueCollection headers = base.Headers;
			foreach (string text in headers.AllKeys)
			{
				stringBuilder.AppendFormat("{0}: {1}{2}", text, headers[text], "\r\n");
			}
			stringBuilder.Append("\r\n");
			string entityBody = base.EntityBody;
			if (entityBody.Length > 0)
			{
				stringBuilder.Append(entityBody);
			}
			return stringBuilder.ToString();
		}
	}
}
