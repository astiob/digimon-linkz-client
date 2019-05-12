using System;
using System.Collections.Specialized;
using System.Text;
using WebSocketSharp.Net;

namespace WebSocketSharp
{
	internal class HandshakeResponse : HandshakeBase
	{
		private string _code;

		private string _reason;

		private HandshakeResponse()
		{
		}

		public HandshakeResponse(HttpStatusCode code)
		{
			int num = (int)code;
			this._code = num.ToString();
			this._reason = code.GetDescription();
			NameValueCollection headers = base.Headers;
			headers["Server"] = "websocket-sharp/1.0";
			if (code == HttpStatusCode.SwitchingProtocols)
			{
				headers["Upgrade"] = "websocket";
				headers["Connection"] = "Upgrade";
			}
		}

		public AuthenticationChallenge AuthChallenge
		{
			get
			{
				string text = base.Headers["WWW-Authenticate"];
				return (text == null || text.Length <= 0) ? null : AuthenticationChallenge.Parse(text);
			}
		}

		public CookieCollection Cookies
		{
			get
			{
				return base.Headers.GetCookies(true);
			}
		}

		public bool IsUnauthorized
		{
			get
			{
				return this._code == "401";
			}
		}

		public bool IsWebSocketResponse
		{
			get
			{
				NameValueCollection headers = base.Headers;
				return base.ProtocolVersion >= HttpVersion.Version11 && this._code == "101" && headers.Contains("Upgrade", "websocket") && headers.Contains("Connection", "Upgrade");
			}
		}

		public string Reason
		{
			get
			{
				return this._reason;
			}
			private set
			{
				this._reason = value;
			}
		}

		public string StatusCode
		{
			get
			{
				return this._code;
			}
			private set
			{
				this._code = value;
			}
		}

		public static HandshakeResponse CreateCloseResponse(HttpStatusCode code)
		{
			HandshakeResponse handshakeResponse = new HandshakeResponse(code);
			handshakeResponse.Headers["Connection"] = "close";
			return handshakeResponse;
		}

		public static HandshakeResponse Parse(string[] headerParts)
		{
			string[] array = headerParts[0].Split(new char[]
			{
				' '
			}, 3);
			if (array.Length != 3)
			{
				throw new ArgumentException("Invalid status line: " + headerParts[0]);
			}
			WebHeaderCollection webHeaderCollection = new WebHeaderCollection();
			for (int i = 1; i < headerParts.Length; i++)
			{
				webHeaderCollection.SetInternal(headerParts[i], true);
			}
			return new HandshakeResponse
			{
				Headers = webHeaderCollection,
				ProtocolVersion = new Version(array[0].Substring(5)),
				Reason = array[2],
				StatusCode = array[1]
			};
		}

		public void SetCookies(CookieCollection cookies)
		{
			if (cookies == null || cookies.Count == 0)
			{
				return;
			}
			NameValueCollection headers = base.Headers;
			foreach (Cookie cookie in cookies.Sorted)
			{
				headers.Add("Set-Cookie", cookie.ToResponseString());
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.AppendFormat("HTTP/{0} {1} {2}{3}", new object[]
			{
				base.ProtocolVersion,
				this._code,
				this._reason,
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
