using System;
using System.Collections.Specialized;
using System.IO;

namespace Firebase.Platform
{
	internal abstract class FirebaseHttpRequest
	{
		protected readonly Uri _url;

		protected string _action;

		public static readonly string HeaderContentLength = "CONTENT-LENGTH";

		public static readonly string HeaderContentType = "CONTENT-TYPE";

		public static readonly string HeaderRange = "RANGE";

		public static readonly string HeaderUserAgent = "USER-AGENT";

		public static readonly string HeaderStatus = "STATUS";

		public static readonly int Timeout = 10000;

		public static readonly int StatusNetworkUnavailable = -2;

		public static readonly int StatusOk = 200;

		protected FirebaseHttpRequest(Uri url)
		{
			this._url = url;
		}

		public virtual bool IsConnected
		{
			get
			{
				return true;
			}
		}

		public abstract Stream OutputStream { get; }

		public abstract int ResponseCode { get; }

		public abstract NameValueCollection ResponseHeaderFields { get; }

		public abstract long ResponseContentLength { get; }

		public abstract Stream InputStream { get; }

		public abstract Stream ErrorStream { get; }

		public abstract void SetRequestProperty(string key, string value);

		public void SetRequestMethod(string action)
		{
			this._action = action;
		}
	}
}
