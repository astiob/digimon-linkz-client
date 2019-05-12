using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

namespace UnityEngine
{
	public class WWW : CustomYieldInstruction, IDisposable
	{
		private UnityWebRequest _uwr;

		private AssetBundle _assetBundle;

		private Dictionary<string, string> _responseHeaders;

		public WWW(string url)
		{
			this._uwr = UnityWebRequest.Get(url);
			this._uwr.SendWebRequest();
		}

		public WWW(string url, WWWForm form)
		{
			this._uwr = UnityWebRequest.Post(url, form);
			this._uwr.chunkedTransfer = false;
			this._uwr.SendWebRequest();
		}

		public WWW(string url, byte[] postData)
		{
			this._uwr = new UnityWebRequest(url, "POST");
			this._uwr.chunkedTransfer = false;
			UploadHandler uploadHandler = new UploadHandlerRaw(postData);
			uploadHandler.contentType = "application/x-www-form-urlencoded";
			this._uwr.uploadHandler = uploadHandler;
			this._uwr.downloadHandler = new DownloadHandlerBuffer();
			this._uwr.SendWebRequest();
		}

		[Obsolete("This overload is deprecated. Use UnityEngine.WWW.WWW(string, byte[], System.Collections.Generic.Dictionary<string, string>) instead.")]
		public WWW(string url, byte[] postData, Hashtable headers)
		{
			string method = (postData != null) ? "POST" : "GET";
			this._uwr = new UnityWebRequest(url, method);
			this._uwr.chunkedTransfer = false;
			UploadHandler uploadHandler = new UploadHandlerRaw(postData);
			uploadHandler.contentType = "application/x-www-form-urlencoded";
			this._uwr.uploadHandler = uploadHandler;
			this._uwr.downloadHandler = new DownloadHandlerBuffer();
			IEnumerator enumerator = headers.Keys.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					this._uwr.SetRequestHeader((string)obj, (string)headers[obj]);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			this._uwr.SendWebRequest();
		}

		public WWW(string url, byte[] postData, Dictionary<string, string> headers)
		{
			string method = (postData != null) ? "POST" : "GET";
			this._uwr = new UnityWebRequest(url, method);
			this._uwr.chunkedTransfer = false;
			UploadHandler uploadHandler = new UploadHandlerRaw(postData);
			uploadHandler.contentType = "application/x-www-form-urlencoded";
			this._uwr.uploadHandler = uploadHandler;
			this._uwr.downloadHandler = new DownloadHandlerBuffer();
			foreach (KeyValuePair<string, string> keyValuePair in headers)
			{
				this._uwr.SetRequestHeader(keyValuePair.Key, keyValuePair.Value);
			}
			this._uwr.SendWebRequest();
		}

		internal WWW(string url, string name, Hash128 hash, uint crc)
		{
			this._uwr = UnityWebRequest.GetAssetBundle(url, new CachedAssetBundle(name, hash), crc);
			this._uwr.SendWebRequest();
		}

		public static string EscapeURL(string s)
		{
			return WWW.EscapeURL(s, Encoding.UTF8);
		}

		public static string EscapeURL(string s, Encoding e)
		{
			return UnityWebRequest.EscapeURL(s, e);
		}

		public static string UnEscapeURL(string s)
		{
			return WWW.UnEscapeURL(s, Encoding.UTF8);
		}

		public static string UnEscapeURL(string s, Encoding e)
		{
			return UnityWebRequest.UnEscapeURL(s, e);
		}

		public static WWW LoadFromCacheOrDownload(string url, int version)
		{
			return WWW.LoadFromCacheOrDownload(url, version, 0u);
		}

		public static WWW LoadFromCacheOrDownload(string url, int version, uint crc)
		{
			Hash128 hash = new Hash128(0u, 0u, 0u, (uint)version);
			return WWW.LoadFromCacheOrDownload(url, hash, crc);
		}

		public static WWW LoadFromCacheOrDownload(string url, Hash128 hash)
		{
			return WWW.LoadFromCacheOrDownload(url, hash, 0u);
		}

		public static WWW LoadFromCacheOrDownload(string url, Hash128 hash, uint crc)
		{
			return new WWW(url, "", hash, crc);
		}

		public static WWW LoadFromCacheOrDownload(string url, CachedAssetBundle cachedBundle, uint crc = 0u)
		{
			return new WWW(url, cachedBundle.name, cachedBundle.hash, crc);
		}

		public AssetBundle assetBundle
		{
			get
			{
				if (this._assetBundle == null)
				{
					if (!this.WaitUntilDoneIfPossible())
					{
						return null;
					}
					if (this._uwr.isNetworkError)
					{
						return null;
					}
					DownloadHandlerAssetBundle downloadHandlerAssetBundle = this._uwr.downloadHandler as DownloadHandlerAssetBundle;
					if (downloadHandlerAssetBundle != null)
					{
						this._assetBundle = downloadHandlerAssetBundle.assetBundle;
					}
					else
					{
						byte[] bytes = this.bytes;
						if (bytes == null)
						{
							return null;
						}
						this._assetBundle = AssetBundle.LoadFromMemory(bytes);
					}
				}
				return this._assetBundle;
			}
		}

		[Obsolete("Obsolete msg (UnityUpgradable) -> * UnityEngine.WWW.GetAudioClip()", true)]
		public Object audioClip
		{
			get
			{
				return null;
			}
		}

		public byte[] bytes
		{
			get
			{
				byte[] result;
				if (!this.WaitUntilDoneIfPossible())
				{
					result = new byte[0];
				}
				else if (this._uwr.isNetworkError)
				{
					result = new byte[0];
				}
				else
				{
					DownloadHandler downloadHandler = this._uwr.downloadHandler;
					if (downloadHandler == null)
					{
						result = new byte[0];
					}
					else
					{
						result = downloadHandler.data;
					}
				}
				return result;
			}
		}

		[Obsolete("WWW.size is obsolete. Please use WWW.bytesDownloaded instead")]
		public int size
		{
			get
			{
				return this.bytesDownloaded;
			}
		}

		public int bytesDownloaded
		{
			get
			{
				return (int)this._uwr.downloadedBytes;
			}
		}

		public string error
		{
			get
			{
				string result;
				if (!this._uwr.isDone)
				{
					result = null;
				}
				else if (this._uwr.isNetworkError)
				{
					result = this._uwr.error;
				}
				else if (this._uwr.responseCode >= 400L)
				{
					result = string.Format("{0} {1}", this._uwr.responseCode, this.GetStatusCodeName(this._uwr.responseCode));
				}
				else
				{
					result = null;
				}
				return result;
			}
		}

		public bool isDone
		{
			get
			{
				return this._uwr.isDone;
			}
		}

		public float progress
		{
			get
			{
				float num = this._uwr.downloadProgress;
				if (num < 0f)
				{
					num = 0f;
				}
				return num;
			}
		}

		public Dictionary<string, string> responseHeaders
		{
			get
			{
				Dictionary<string, string> result;
				if (!this.isDone)
				{
					result = new Dictionary<string, string>();
				}
				else
				{
					if (this._responseHeaders == null)
					{
						this._responseHeaders = this._uwr.GetResponseHeaders();
						if (this._responseHeaders != null)
						{
							this._responseHeaders["STATUS"] = string.Format("HTTP/1.1 {0} {1}", this._uwr.responseCode, this.GetStatusCodeName(this._uwr.responseCode));
						}
						else
						{
							this._responseHeaders = new Dictionary<string, string>();
						}
					}
					result = this._responseHeaders;
				}
				return result;
			}
		}

		[Obsolete("Please use WWW.text instead. (UnityUpgradable) -> text", true)]
		public string data
		{
			get
			{
				return this.text;
			}
		}

		public string text
		{
			get
			{
				string result;
				if (!this.WaitUntilDoneIfPossible())
				{
					result = "";
				}
				else if (this._uwr.isNetworkError)
				{
					result = "";
				}
				else
				{
					DownloadHandler downloadHandler = this._uwr.downloadHandler;
					if (downloadHandler == null)
					{
						result = "";
					}
					else
					{
						result = downloadHandler.text;
					}
				}
				return result;
			}
		}

		private Texture2D CreateTextureFromDownloadedData(bool markNonReadable)
		{
			Texture2D result;
			if (!this.WaitUntilDoneIfPossible())
			{
				result = new Texture2D(2, 2);
			}
			else if (this._uwr.isNetworkError)
			{
				result = null;
			}
			else
			{
				DownloadHandler downloadHandler = this._uwr.downloadHandler;
				if (downloadHandler == null)
				{
					result = null;
				}
				else
				{
					Texture2D texture2D = new Texture2D(2, 2);
					texture2D.LoadImage(downloadHandler.data, markNonReadable);
					result = texture2D;
				}
			}
			return result;
		}

		public Texture2D texture
		{
			get
			{
				return this.CreateTextureFromDownloadedData(false);
			}
		}

		public Texture2D textureNonReadable
		{
			get
			{
				return this.CreateTextureFromDownloadedData(true);
			}
		}

		public void LoadImageIntoTexture(Texture2D texture)
		{
			if (this.WaitUntilDoneIfPossible())
			{
				if (this._uwr.isNetworkError)
				{
					Debug.LogError("Cannot load image: download failed");
				}
				else
				{
					DownloadHandler downloadHandler = this._uwr.downloadHandler;
					if (downloadHandler == null)
					{
						Debug.LogError("Cannot load image: internal error");
					}
					else
					{
						texture.LoadImage(downloadHandler.data, false);
					}
				}
			}
		}

		public ThreadPriority threadPriority { get; set; }

		public float uploadProgress
		{
			get
			{
				float num = this._uwr.uploadProgress;
				if (num < 0f)
				{
					num = 0f;
				}
				return num;
			}
		}

		public string url
		{
			get
			{
				return this._uwr.url;
			}
		}

		public override bool keepWaiting
		{
			get
			{
				return !this._uwr.isDone;
			}
		}

		public void Dispose()
		{
			this._uwr.Dispose();
		}

		internal Object GetAudioClipInternal(bool threeD, bool stream, bool compressed, AudioType audioType)
		{
			return WebRequestWWW.InternalCreateAudioClipUsingDH(this._uwr.downloadHandler, this._uwr.url, stream, compressed, audioType);
		}

		public AudioClip GetAudioClip()
		{
			return this.GetAudioClip(true, false, AudioType.UNKNOWN);
		}

		public AudioClip GetAudioClip(bool threeD)
		{
			return this.GetAudioClip(threeD, false, AudioType.UNKNOWN);
		}

		public AudioClip GetAudioClip(bool threeD, bool stream)
		{
			return this.GetAudioClip(threeD, stream, AudioType.UNKNOWN);
		}

		public AudioClip GetAudioClip(bool threeD, bool stream, AudioType audioType)
		{
			return (AudioClip)this.GetAudioClipInternal(threeD, stream, false, audioType);
		}

		public AudioClip GetAudioClipCompressed()
		{
			return this.GetAudioClipCompressed(false, AudioType.UNKNOWN);
		}

		public AudioClip GetAudioClipCompressed(bool threeD)
		{
			return this.GetAudioClipCompressed(threeD, AudioType.UNKNOWN);
		}

		public AudioClip GetAudioClipCompressed(bool threeD, AudioType audioType)
		{
			return (AudioClip)this.GetAudioClipInternal(threeD, false, true, audioType);
		}

		private bool WaitUntilDoneIfPossible()
		{
			bool result;
			if (this._uwr.isDone)
			{
				result = true;
			}
			else if (this.url.StartsWith("file://", StringComparison.OrdinalIgnoreCase))
			{
				while (!this._uwr.isDone)
				{
				}
				result = true;
			}
			else
			{
				Debug.LogError("You are trying to load data from a www stream which has not completed the download yet.\nYou need to yield the download or wait until isDone returns true.");
				result = false;
			}
			return result;
		}

		private string GetStatusCodeName(long statusCode)
		{
			if (statusCode >= 400L && statusCode <= 416L)
			{
				switch ((int)(statusCode - 400L))
				{
				case 0:
					return "Bad Request";
				case 1:
					return "Unauthorized";
				case 2:
					return "Payment Required";
				case 3:
					return "Forbidden";
				case 4:
					return "Not Found";
				case 5:
					return "Method Not Allowed";
				case 6:
					return "Not Acceptable";
				case 7:
					return "Proxy Authentication Required";
				case 8:
					return "Request Timeout";
				case 9:
					return "Conflict";
				case 10:
					return "Gone";
				case 11:
					return "Length Required";
				case 12:
					return "Precondition Failed";
				case 13:
					return "Request Entity Too Large";
				case 14:
					return "Request-URI Too Long";
				case 15:
					return "Unsupported Media Type";
				case 16:
					return "Requested Range Not Satisfiable";
				}
			}
			if (statusCode >= 200L && statusCode <= 206L)
			{
				switch ((int)(statusCode - 200L))
				{
				case 0:
					return "OK";
				case 1:
					return "Created";
				case 2:
					return "Accepted";
				case 3:
					return "Non-Authoritative Information";
				case 4:
					return "No Content";
				case 5:
					return "Reset Content";
				case 6:
					return "Partial Content";
				}
			}
			if (statusCode >= 300L && statusCode <= 307L)
			{
				switch ((int)(statusCode - 300L))
				{
				case 0:
					return "Multiple Choices";
				case 1:
					return "Moved Permanently";
				case 2:
					return "Found";
				case 3:
					return "See Other";
				case 4:
					return "Not Modified";
				case 5:
					return "Use Proxy";
				case 7:
					return "Temporary Redirect";
				}
			}
			if (statusCode >= 500L && statusCode <= 505L)
			{
				switch ((int)(statusCode - 500L))
				{
				case 0:
					return "Internal Server Error";
				case 1:
					return "Not Implemented";
				case 2:
					return "Bad Gateway";
				case 3:
					return "Service Unavailable";
				case 4:
					return "Gateway Timeout";
				case 5:
					return "HTTP Version Not Supported";
				}
			}
			string result;
			if (statusCode != 41L)
			{
				result = "";
			}
			else
			{
				result = "Expectation Failed";
			}
			return result;
		}
	}
}
