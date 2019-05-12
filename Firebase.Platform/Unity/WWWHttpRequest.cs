using Firebase.Platform;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using UnityEngine;

namespace Firebase.Unity
{
	internal class WWWHttpRequest : FirebaseHttpRequest
	{
		private Dictionary<string, string> _headers = new Dictionary<string, string>();

		private NameValueCollection _responseheaders = new NameValueCollection();

		private MemoryStream _requestBody = new MemoryStream();

		private string _error;

		private bool _executed;

		private byte[] _requestBodyBytes;

		private byte[] _responseBodyBytes;

		private int _responseCode = FirebaseHttpRequest.StatusNetworkUnavailable;

		private long _responseLength;

		public WWWHttpRequest(Uri url) : base(url)
		{
		}

		public override Stream OutputStream
		{
			get
			{
				return this._requestBody;
			}
		}

		public override int ResponseCode
		{
			get
			{
				this.EnsureExecuted();
				return this._responseCode;
			}
		}

		public override NameValueCollection ResponseHeaderFields
		{
			get
			{
				this.EnsureExecuted();
				return this._responseheaders;
			}
		}

		public override long ResponseContentLength
		{
			get
			{
				this.EnsureExecuted();
				return this._responseLength;
			}
		}

		public override Stream InputStream
		{
			get
			{
				this.EnsureExecuted();
				if (this._responseBodyBytes != null)
				{
					return new MemoryStream(this._responseBodyBytes);
				}
				return null;
			}
		}

		public override Stream ErrorStream
		{
			get
			{
				this.EnsureExecuted();
				if (this._responseBodyBytes != null)
				{
					return new MemoryStream(this._responseBodyBytes);
				}
				return null;
			}
		}

		public override void SetRequestProperty(string key, string value)
		{
			this._headers[key] = value;
		}

		private void EnsureExecuted()
		{
			if (!this._headers.ContainsKey("X-HTTP-Method-Override") && !string.Equals(this._action, "POST", StringComparison.OrdinalIgnoreCase) && !string.Equals(this._action, "GET", StringComparison.OrdinalIgnoreCase))
			{
				this._headers["X-HTTP-Method-Override"] = this._action;
			}
			if (!this._executed)
			{
				this._responseCode = 0;
				Services.Logging.LogMessage(PlatformLogLevel.Verbose, "Requesting " + this._url.ToString());
				this._executed = true;
				this._requestBodyBytes = this._requestBody.ToArray();
				Services.Logging.LogMessage(PlatformLogLevel.Verbose, "Waiting for result of " + this._url.ToString());
				UnitySynchronizationContext.Instance.SendCoroutine(() => this.SendUnityRequest(), -1);
				if (!string.IsNullOrEmpty(this._error))
				{
					Services.Logging.LogMessage(PlatformLogLevel.Error, this._error);
				}
				if (this._responseCode == 0)
				{
					Services.Logging.LogMessage(PlatformLogLevel.Verbose, "Response timed out waiting for result");
					this._responseCode = FirebaseHttpRequest.StatusNetworkUnavailable;
				}
			}
		}

		private IEnumerator SendUnityRequest()
		{
			if (this._requestBodyBytes == null || this._requestBodyBytes.Length == 0)
			{
				if (string.Equals(this._action, "POST", StringComparison.OrdinalIgnoreCase))
				{
					this._requestBodyBytes = Encoding.ASCII.GetBytes("\n");
				}
				else
				{
					this._requestBodyBytes = null;
				}
			}
			Services.Logging.LogMessage(PlatformLogLevel.Verbose, "About to send message to " + this._action + " -> " + this._url.ToString());
			foreach (KeyValuePair<string, string> keyValuePair in this._headers)
			{
				Services.Logging.LogMessage(PlatformLogLevel.Verbose, keyValuePair.Key + ":" + keyValuePair.Value);
			}
			if (this._requestBodyBytes != null)
			{
				Services.Logging.LogMessage(PlatformLogLevel.Verbose, "body size:" + this._requestBodyBytes.Length.ToString());
			}
			else
			{
				Services.Logging.LogMessage(PlatformLogLevel.Debug, "body:{null}");
			}
			if (string.Equals(this._action, "GET", StringComparison.OrdinalIgnoreCase) && !this._headers.ContainsKey("Content-Type"))
			{
				this._headers["Content-Type"] = string.Empty;
			}
			WWW www = new WWW(this._url.ToString(), this._requestBodyBytes, this._headers);
			yield return www;
			this.TryParseResponse(www);
			this._responseBodyBytes = www.bytes;
			this._error = www.error;
			yield break;
		}

		private void TryParseResponse(WWW www)
		{
			if (this._responseCode == 0 && www.responseHeaders != null)
			{
				foreach (KeyValuePair<string, string> keyValuePair in www.responseHeaders)
				{
					this._responseheaders[keyValuePair.Key.ToUpper()] = keyValuePair.Value;
				}
				if (this._responseheaders[FirebaseHttpRequest.HeaderStatus] != null)
				{
					this._responseCode = WWWHttpRequest.ParseIntHeader(this._responseheaders[FirebaseHttpRequest.HeaderStatus]);
				}
				if (this._responseheaders[FirebaseHttpRequest.HeaderContentLength] != null)
				{
					this._responseLength = (long)WWWHttpRequest.ParseIntHeader(this._responseheaders[FirebaseHttpRequest.HeaderContentLength]);
				}
			}
		}

		private static int ParseIntHeader(string statusLine)
		{
			int result = 0;
			string[] array = statusLine.Split(new char[]
			{
				' '
			});
			if (array.Length >= 3 && !int.TryParse(array[1], out result))
			{
				Services.Logging.LogMessage(PlatformLogLevel.Error, "invalid header: " + array[1]);
			}
			return result;
		}
	}
}
