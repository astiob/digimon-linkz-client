using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace WebSocketSharp.Net
{
	[ComVisible(true)]
	[Serializable]
	public class WebHeaderCollection : NameValueCollection, ISerializable
	{
		private static readonly Dictionary<string, HttpHeaderInfo> headers = new Dictionary<string, HttpHeaderInfo>(StringComparer.InvariantCultureIgnoreCase)
		{
			{
				"Accept",
				new HttpHeaderInfo
				{
					Name = "Accept",
					Type = (HttpHeaderType.Request | HttpHeaderType.Restricted | HttpHeaderType.MultiValue)
				}
			},
			{
				"AcceptCharset",
				new HttpHeaderInfo
				{
					Name = "Accept-Charset",
					Type = (HttpHeaderType.Request | HttpHeaderType.MultiValue)
				}
			},
			{
				"AcceptEncoding",
				new HttpHeaderInfo
				{
					Name = "Accept-Encoding",
					Type = (HttpHeaderType.Request | HttpHeaderType.MultiValue)
				}
			},
			{
				"AcceptLanguage",
				new HttpHeaderInfo
				{
					Name = "Accept-language",
					Type = (HttpHeaderType.Request | HttpHeaderType.MultiValue)
				}
			},
			{
				"AcceptRanges",
				new HttpHeaderInfo
				{
					Name = "Accept-Ranges",
					Type = (HttpHeaderType.Response | HttpHeaderType.MultiValue)
				}
			},
			{
				"Age",
				new HttpHeaderInfo
				{
					Name = "Age",
					Type = HttpHeaderType.Response
				}
			},
			{
				"Allow",
				new HttpHeaderInfo
				{
					Name = "Allow",
					Type = (HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.MultiValue)
				}
			},
			{
				"Authorization",
				new HttpHeaderInfo
				{
					Name = "Authorization",
					Type = (HttpHeaderType.Request | HttpHeaderType.MultiValue)
				}
			},
			{
				"CacheControl",
				new HttpHeaderInfo
				{
					Name = "Cache-Control",
					Type = (HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.MultiValue)
				}
			},
			{
				"Connection",
				new HttpHeaderInfo
				{
					Name = "Connection",
					Type = (HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.Restricted | HttpHeaderType.MultiValue)
				}
			},
			{
				"ContentEncoding",
				new HttpHeaderInfo
				{
					Name = "Content-Encoding",
					Type = (HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.MultiValue)
				}
			},
			{
				"ContentLanguage",
				new HttpHeaderInfo
				{
					Name = "Content-Language",
					Type = (HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.MultiValue)
				}
			},
			{
				"ContentLength",
				new HttpHeaderInfo
				{
					Name = "Content-Length",
					Type = (HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.Restricted)
				}
			},
			{
				"ContentLocation",
				new HttpHeaderInfo
				{
					Name = "Content-Location",
					Type = (HttpHeaderType.Request | HttpHeaderType.Response)
				}
			},
			{
				"ContentMd5",
				new HttpHeaderInfo
				{
					Name = "Content-MD5",
					Type = (HttpHeaderType.Request | HttpHeaderType.Response)
				}
			},
			{
				"ContentRange",
				new HttpHeaderInfo
				{
					Name = "Content-Range",
					Type = (HttpHeaderType.Request | HttpHeaderType.Response)
				}
			},
			{
				"ContentType",
				new HttpHeaderInfo
				{
					Name = "Content-Type",
					Type = (HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.Restricted)
				}
			},
			{
				"Cookie",
				new HttpHeaderInfo
				{
					Name = "Cookie",
					Type = HttpHeaderType.Request
				}
			},
			{
				"Cookie2",
				new HttpHeaderInfo
				{
					Name = "Cookie2",
					Type = HttpHeaderType.Request
				}
			},
			{
				"Date",
				new HttpHeaderInfo
				{
					Name = "Date",
					Type = (HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.Restricted)
				}
			},
			{
				"Expect",
				new HttpHeaderInfo
				{
					Name = "Expect",
					Type = (HttpHeaderType.Request | HttpHeaderType.Restricted | HttpHeaderType.MultiValue)
				}
			},
			{
				"Expires",
				new HttpHeaderInfo
				{
					Name = "Expires",
					Type = (HttpHeaderType.Request | HttpHeaderType.Response)
				}
			},
			{
				"ETag",
				new HttpHeaderInfo
				{
					Name = "ETag",
					Type = HttpHeaderType.Response
				}
			},
			{
				"From",
				new HttpHeaderInfo
				{
					Name = "From",
					Type = HttpHeaderType.Request
				}
			},
			{
				"Host",
				new HttpHeaderInfo
				{
					Name = "Host",
					Type = (HttpHeaderType.Request | HttpHeaderType.Restricted)
				}
			},
			{
				"IfMatch",
				new HttpHeaderInfo
				{
					Name = "If-Match",
					Type = (HttpHeaderType.Request | HttpHeaderType.MultiValue)
				}
			},
			{
				"IfModifiedSince",
				new HttpHeaderInfo
				{
					Name = "If-Modified-Since",
					Type = (HttpHeaderType.Request | HttpHeaderType.Restricted)
				}
			},
			{
				"IfNoneMatch",
				new HttpHeaderInfo
				{
					Name = "If-None-Match",
					Type = (HttpHeaderType.Request | HttpHeaderType.MultiValue)
				}
			},
			{
				"IfRange",
				new HttpHeaderInfo
				{
					Name = "If-Range",
					Type = HttpHeaderType.Request
				}
			},
			{
				"IfUnmodifiedSince",
				new HttpHeaderInfo
				{
					Name = "If-Unmodified-Since",
					Type = HttpHeaderType.Request
				}
			},
			{
				"KeepAlive",
				new HttpHeaderInfo
				{
					Name = "Keep-Alive",
					Type = (HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.MultiValue)
				}
			},
			{
				"LastModified",
				new HttpHeaderInfo
				{
					Name = "Last-Modified",
					Type = (HttpHeaderType.Request | HttpHeaderType.Response)
				}
			},
			{
				"Location",
				new HttpHeaderInfo
				{
					Name = "Location",
					Type = HttpHeaderType.Response
				}
			},
			{
				"MaxForwards",
				new HttpHeaderInfo
				{
					Name = "Max-Forwards",
					Type = HttpHeaderType.Request
				}
			},
			{
				"Pragma",
				new HttpHeaderInfo
				{
					Name = "Pragma",
					Type = (HttpHeaderType.Request | HttpHeaderType.Response)
				}
			},
			{
				"ProxyConnection",
				new HttpHeaderInfo
				{
					Name = "Proxy-Connection",
					Type = (HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.Restricted)
				}
			},
			{
				"ProxyAuthenticate",
				new HttpHeaderInfo
				{
					Name = "Proxy-Authenticate",
					Type = (HttpHeaderType.Response | HttpHeaderType.MultiValue)
				}
			},
			{
				"ProxyAuthorization",
				new HttpHeaderInfo
				{
					Name = "Proxy-Authorization",
					Type = HttpHeaderType.Request
				}
			},
			{
				"Public",
				new HttpHeaderInfo
				{
					Name = "Public",
					Type = (HttpHeaderType.Response | HttpHeaderType.MultiValue)
				}
			},
			{
				"Range",
				new HttpHeaderInfo
				{
					Name = "Range",
					Type = (HttpHeaderType.Request | HttpHeaderType.Restricted | HttpHeaderType.MultiValue)
				}
			},
			{
				"Referer",
				new HttpHeaderInfo
				{
					Name = "Referer",
					Type = (HttpHeaderType.Request | HttpHeaderType.Restricted)
				}
			},
			{
				"RetryAfter",
				new HttpHeaderInfo
				{
					Name = "Retry-After",
					Type = HttpHeaderType.Response
				}
			},
			{
				"SecWebSocketAccept",
				new HttpHeaderInfo
				{
					Name = "Sec-WebSocket-Accept",
					Type = (HttpHeaderType.Response | HttpHeaderType.Restricted)
				}
			},
			{
				"SecWebSocketExtensions",
				new HttpHeaderInfo
				{
					Name = "Sec-WebSocket-Extensions",
					Type = (HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.Restricted | HttpHeaderType.MultiValueInRequest)
				}
			},
			{
				"SecWebSocketKey",
				new HttpHeaderInfo
				{
					Name = "Sec-WebSocket-Key",
					Type = (HttpHeaderType.Request | HttpHeaderType.Restricted)
				}
			},
			{
				"SecWebSocketProtocol",
				new HttpHeaderInfo
				{
					Name = "Sec-WebSocket-Protocol",
					Type = (HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.MultiValueInRequest)
				}
			},
			{
				"SecWebSocketVersion",
				new HttpHeaderInfo
				{
					Name = "Sec-WebSocket-Version",
					Type = (HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.Restricted | HttpHeaderType.MultiValueInResponse)
				}
			},
			{
				"Server",
				new HttpHeaderInfo
				{
					Name = "Server",
					Type = HttpHeaderType.Response
				}
			},
			{
				"SetCookie",
				new HttpHeaderInfo
				{
					Name = "Set-Cookie",
					Type = (HttpHeaderType.Response | HttpHeaderType.MultiValue)
				}
			},
			{
				"SetCookie2",
				new HttpHeaderInfo
				{
					Name = "Set-Cookie2",
					Type = (HttpHeaderType.Response | HttpHeaderType.MultiValue)
				}
			},
			{
				"Te",
				new HttpHeaderInfo
				{
					Name = "TE",
					Type = HttpHeaderType.Request
				}
			},
			{
				"Trailer",
				new HttpHeaderInfo
				{
					Name = "Trailer",
					Type = (HttpHeaderType.Request | HttpHeaderType.Response)
				}
			},
			{
				"TransferEncoding",
				new HttpHeaderInfo
				{
					Name = "Transfer-Encoding",
					Type = (HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.Restricted | HttpHeaderType.MultiValue)
				}
			},
			{
				"Translate",
				new HttpHeaderInfo
				{
					Name = "Translate",
					Type = HttpHeaderType.Request
				}
			},
			{
				"Upgrade",
				new HttpHeaderInfo
				{
					Name = "Upgrade",
					Type = (HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.MultiValue)
				}
			},
			{
				"UserAgent",
				new HttpHeaderInfo
				{
					Name = "User-Agent",
					Type = (HttpHeaderType.Request | HttpHeaderType.Restricted)
				}
			},
			{
				"Vary",
				new HttpHeaderInfo
				{
					Name = "Vary",
					Type = (HttpHeaderType.Response | HttpHeaderType.MultiValue)
				}
			},
			{
				"Via",
				new HttpHeaderInfo
				{
					Name = "Via",
					Type = (HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.MultiValue)
				}
			},
			{
				"Warning",
				new HttpHeaderInfo
				{
					Name = "Warning",
					Type = (HttpHeaderType.Request | HttpHeaderType.Response | HttpHeaderType.MultiValue)
				}
			},
			{
				"WwwAuthenticate",
				new HttpHeaderInfo
				{
					Name = "WWW-Authenticate",
					Type = (HttpHeaderType.Response | HttpHeaderType.Restricted | HttpHeaderType.MultiValue)
				}
			}
		};

		private bool internallyCreated;

		private HttpHeaderType state;

		internal WebHeaderCollection(bool internallyCreated)
		{
			this.internallyCreated = internallyCreated;
			this.state = HttpHeaderType.Unspecified;
		}

		protected WebHeaderCollection(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			if (serializationInfo == null)
			{
				throw new ArgumentNullException("serializationInfo");
			}
			try
			{
				this.internallyCreated = serializationInfo.GetBoolean("InternallyCreated");
				this.state = (HttpHeaderType)serializationInfo.GetInt32("State");
				int @int = serializationInfo.GetInt32("Count");
				for (int i = 0; i < @int; i++)
				{
					base.Add(serializationInfo.GetString(i.ToString()), serializationInfo.GetString((@int + i).ToString()));
				}
			}
			catch (SerializationException ex)
			{
				throw new ArgumentException(ex.Message, "serializationInfo", ex);
			}
		}

		public WebHeaderCollection()
		{
			this.internallyCreated = false;
			this.state = HttpHeaderType.Unspecified;
		}

		[PermissionSet(SecurityAction.LinkDemand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		public override string[] AllKeys
		{
			get
			{
				return base.AllKeys;
			}
		}

		public override int Count
		{
			get
			{
				return base.Count;
			}
		}

		public string this[HttpRequestHeader header]
		{
			get
			{
				return this.Get(WebHeaderCollection.Convert(header));
			}
			set
			{
				this.Add(header, value);
			}
		}

		public string this[HttpResponseHeader header]
		{
			get
			{
				return this.Get(WebHeaderCollection.Convert(header));
			}
			set
			{
				this.Add(header, value);
			}
		}

		public override NameObjectCollectionBase.KeysCollection Keys
		{
			get
			{
				return base.Keys;
			}
		}

		private void Add(string name, string value, bool ignoreRestricted)
		{
			Action<string, string> act;
			if (ignoreRestricted)
			{
				act = new Action<string, string>(this.AddWithoutCheckingNameAndRestricted);
			}
			else
			{
				act = new Action<string, string>(this.AddWithoutCheckingName);
			}
			this.DoWithCheckingState(act, WebHeaderCollection.CheckName(name), value, true);
		}

		private void AddWithoutCheckingName(string name, string value)
		{
			this.DoWithoutCheckingName(new Action<string, string>(base.Add), name, value);
		}

		private void AddWithoutCheckingNameAndRestricted(string name, string value)
		{
			base.Add(name, WebHeaderCollection.CheckValue(value));
		}

		private static int CheckColonSeparated(string header)
		{
			int num = header.IndexOf(':');
			if (num == -1)
			{
				throw new ArgumentException("No colon found.", "header");
			}
			return num;
		}

		private static HttpHeaderType CheckHeaderType(string name)
		{
			HttpHeaderInfo httpHeaderInfo;
			return WebHeaderCollection.TryGetHeaderInfo(name, out httpHeaderInfo) ? ((!httpHeaderInfo.IsRequest || httpHeaderInfo.IsResponse) ? ((httpHeaderInfo.IsRequest || !httpHeaderInfo.IsResponse) ? HttpHeaderType.Unspecified : HttpHeaderType.Response) : HttpHeaderType.Request) : HttpHeaderType.Unspecified;
		}

		private static string CheckName(string name)
		{
			if (name.IsNullOrEmpty())
			{
				throw new ArgumentNullException("name");
			}
			name = name.Trim();
			if (!WebHeaderCollection.IsHeaderName(name))
			{
				throw new ArgumentException("Contains invalid characters.", "name");
			}
			return name;
		}

		private void CheckRestricted(string name)
		{
			if (!this.internallyCreated && WebHeaderCollection.ContainsInRestricted(name, true))
			{
				throw new ArgumentException("This header must be modified with the appropiate property.");
			}
		}

		private void CheckState(bool response)
		{
			if (this.state == HttpHeaderType.Unspecified)
			{
				return;
			}
			if (response && this.state == HttpHeaderType.Request)
			{
				throw new InvalidOperationException("This collection has already been used to store the request headers.");
			}
			if (!response && this.state == HttpHeaderType.Response)
			{
				throw new InvalidOperationException("This collection has already been used to store the response headers.");
			}
		}

		private static string CheckValue(string value)
		{
			if (value.IsNullOrEmpty())
			{
				return string.Empty;
			}
			value = value.Trim();
			if (value.Length > 65535)
			{
				throw new ArgumentOutOfRangeException("value", "The length must not be greater than 65535.");
			}
			if (!WebHeaderCollection.IsHeaderValue(value))
			{
				throw new ArgumentException("Contains invalid characters.", "value");
			}
			return value;
		}

		private static string Convert(string key)
		{
			HttpHeaderInfo httpHeaderInfo;
			return (!WebHeaderCollection.headers.TryGetValue(key, out httpHeaderInfo)) ? string.Empty : httpHeaderInfo.Name;
		}

		private static bool ContainsInRestricted(string name, bool response)
		{
			HttpHeaderInfo httpHeaderInfo;
			return WebHeaderCollection.TryGetHeaderInfo(name, out httpHeaderInfo) && httpHeaderInfo.IsRestricted(response);
		}

		private void DoWithCheckingState(Action<string, string> act, string name, string value, bool setState)
		{
			HttpHeaderType httpHeaderType = WebHeaderCollection.CheckHeaderType(name);
			if (httpHeaderType == HttpHeaderType.Request)
			{
				this.DoWithCheckingState(act, name, value, false, setState);
			}
			else if (httpHeaderType == HttpHeaderType.Response)
			{
				this.DoWithCheckingState(act, name, value, true, setState);
			}
			else
			{
				act(name, value);
			}
		}

		private void DoWithCheckingState(Action<string, string> act, string name, string value, bool response, bool setState)
		{
			this.CheckState(response);
			act(name, value);
			if (setState)
			{
				this.SetState(response);
			}
		}

		private void DoWithoutCheckingName(Action<string, string> act, string name, string value)
		{
			this.CheckRestricted(name);
			act(name, WebHeaderCollection.CheckValue(value));
		}

		private static HttpHeaderInfo GetHeaderInfo(string name)
		{
			return WebHeaderCollection.headers.Values.Cast<HttpHeaderInfo>().Where((HttpHeaderInfo info) => info.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault<HttpHeaderInfo>();
		}

		private void RemoveWithoutCheckingName(string name, string unuse)
		{
			this.CheckRestricted(name);
			base.Remove(name);
		}

		private void SetState(bool response)
		{
			if (this.state == HttpHeaderType.Unspecified)
			{
				this.state = ((!response) ? HttpHeaderType.Request : HttpHeaderType.Response);
			}
		}

		private void SetWithoutCheckingName(string name, string value)
		{
			this.DoWithoutCheckingName(new Action<string, string>(base.Set), name, value);
		}

		private static bool TryGetHeaderInfo(string name, out HttpHeaderInfo info)
		{
			info = WebHeaderCollection.GetHeaderInfo(name);
			return info != null;
		}

		internal static string Convert(HttpRequestHeader header)
		{
			return WebHeaderCollection.Convert(header.ToString());
		}

		internal static string Convert(HttpResponseHeader header)
		{
			return WebHeaderCollection.Convert(header.ToString());
		}

		internal static bool IsHeaderName(string name)
		{
			return !name.IsNullOrEmpty() && name.IsToken();
		}

		internal static bool IsHeaderValue(string value)
		{
			return value.IsText();
		}

		internal static bool IsMultiValue(string headerName, bool response)
		{
			HttpHeaderInfo httpHeaderInfo;
			return !headerName.IsNullOrEmpty() && WebHeaderCollection.TryGetHeaderInfo(headerName, out httpHeaderInfo) && httpHeaderInfo.IsMultiValue(response);
		}

		internal void RemoveInternal(string name)
		{
			base.Remove(name);
		}

		internal void SetInternal(string header, bool response)
		{
			int num = WebHeaderCollection.CheckColonSeparated(header);
			this.SetInternal(header.Substring(0, num), header.Substring(num + 1), response);
		}

		internal void SetInternal(string name, string value, bool response)
		{
			value = WebHeaderCollection.CheckValue(value);
			if (WebHeaderCollection.IsMultiValue(name, response))
			{
				base.Add(name, value);
			}
			else
			{
				base.Set(name, value);
			}
		}

		internal string ToStringMultiValue(bool response)
		{
			StringBuilder sb = new StringBuilder();
			this.Count.Times(delegate(int i)
			{
				string key = this.GetKey(i);
				if (WebHeaderCollection.IsMultiValue(key, response))
				{
					foreach (string arg in this.GetValues(i))
					{
						sb.AppendFormat("{0}: {1}\r\n", key, arg);
					}
				}
				else
				{
					sb.AppendFormat("{0}: {1}\r\n", key, this.Get(i));
				}
			});
			return sb.Append("\r\n").ToString();
		}

		protected void AddWithoutValidate(string headerName, string headerValue)
		{
			this.Add(headerName, headerValue, true);
		}

		public void Add(string header)
		{
			if (header.IsNullOrEmpty())
			{
				throw new ArgumentNullException("header");
			}
			int num = WebHeaderCollection.CheckColonSeparated(header);
			this.Add(header.Substring(0, num), header.Substring(num + 1));
		}

		public void Add(HttpRequestHeader header, string value)
		{
			this.DoWithCheckingState(new Action<string, string>(this.AddWithoutCheckingName), WebHeaderCollection.Convert(header), value, false, true);
		}

		public void Add(HttpResponseHeader header, string value)
		{
			this.DoWithCheckingState(new Action<string, string>(this.AddWithoutCheckingName), WebHeaderCollection.Convert(header), value, true, true);
		}

		public override void Add(string name, string value)
		{
			this.Add(name, value, false);
		}

		public override void Clear()
		{
			base.Clear();
			this.state = HttpHeaderType.Unspecified;
		}

		public override string Get(int index)
		{
			return base.Get(index);
		}

		public override string Get(string name)
		{
			return base.Get(name);
		}

		public override IEnumerator GetEnumerator()
		{
			return base.GetEnumerator();
		}

		public override string GetKey(int index)
		{
			return base.GetKey(index);
		}

		public override string[] GetValues(string header)
		{
			string[] values = base.GetValues(header);
			return (values != null && values.Length != 0) ? values : null;
		}

		public override string[] GetValues(int index)
		{
			string[] values = base.GetValues(index);
			return (values != null && values.Length != 0) ? values : null;
		}

		[PermissionSet(SecurityAction.LinkDemand, XML = "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			if (serializationInfo == null)
			{
				throw new ArgumentNullException("serializationInfo");
			}
			serializationInfo.AddValue("InternallyCreated", this.internallyCreated);
			serializationInfo.AddValue("State", (int)this.state);
			int count = this.Count;
			serializationInfo.AddValue("Count", count);
			count.Times(delegate(int i)
			{
				serializationInfo.AddValue(i.ToString(), this.GetKey(i));
				serializationInfo.AddValue((count + i).ToString(), this.Get(i));
			});
		}

		public static bool IsRestricted(string headerName)
		{
			return WebHeaderCollection.IsRestricted(headerName, false);
		}

		public static bool IsRestricted(string headerName, bool response)
		{
			return WebHeaderCollection.ContainsInRestricted(WebHeaderCollection.CheckName(headerName), response);
		}

		public override void OnDeserialization(object sender)
		{
		}

		public void Remove(HttpRequestHeader header)
		{
			this.DoWithCheckingState(new Action<string, string>(this.RemoveWithoutCheckingName), WebHeaderCollection.Convert(header), null, false, false);
		}

		public void Remove(HttpResponseHeader header)
		{
			this.DoWithCheckingState(new Action<string, string>(this.RemoveWithoutCheckingName), WebHeaderCollection.Convert(header), null, true, false);
		}

		public override void Remove(string name)
		{
			this.DoWithCheckingState(new Action<string, string>(this.RemoveWithoutCheckingName), WebHeaderCollection.CheckName(name), null, false);
		}

		public void Set(HttpRequestHeader header, string value)
		{
			this.DoWithCheckingState(new Action<string, string>(this.SetWithoutCheckingName), WebHeaderCollection.Convert(header), value, false, true);
		}

		public void Set(HttpResponseHeader header, string value)
		{
			this.DoWithCheckingState(new Action<string, string>(this.SetWithoutCheckingName), WebHeaderCollection.Convert(header), value, true, true);
		}

		public override void Set(string name, string value)
		{
			this.DoWithCheckingState(new Action<string, string>(this.SetWithoutCheckingName), WebHeaderCollection.CheckName(name), value, true);
		}

		public byte[] ToByteArray()
		{
			return Encoding.UTF8.GetBytes(this.ToString());
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			this.Count.Times(delegate(int i)
			{
				sb.AppendFormat("{0}: {1}\r\n", this.GetKey(i), this.Get(i));
			});
			return sb.Append("\r\n").ToString();
		}
	}
}
