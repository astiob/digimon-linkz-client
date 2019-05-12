using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WWWHelper
{
	protected bool dispose;

	protected string url;

	protected Dictionary<string, string> headers;

	protected byte[] postData;

	protected int version;

	protected uint crc;

	protected float timeoutSeconds = 20f;

	public WWWHelper(string url, byte[] postData, Dictionary<string, string> headers, float timeoutSeconds)
	{
		this.url = url;
		this.postData = postData;
		if (headers == null)
		{
			this.headers = new Dictionary<string, string>();
		}
		else
		{
			this.headers = headers;
		}
		this.timeoutSeconds = timeoutSeconds;
	}

	protected WWWHelper()
	{
	}

	public void WWWDispose()
	{
		this.dispose = true;
	}

	public IEnumerator StartRequest(Action<string, string, WWWHelper.TimeOut> onFinished)
	{
		IEnumerator ie = this.StartRequest(delegate(WWW www, WWWHelper.TimeOut isTimeOut)
		{
			if (onFinished != null)
			{
				if (isTimeOut == WWWHelper.TimeOut.NO)
				{
					string arg = (!string.IsNullOrEmpty(www.text)) ? www.text.Trim() : string.Empty;
					onFinished(arg, www.error, isTimeOut);
				}
				else
				{
					onFinished(string.Empty, "Time Out", isTimeOut);
				}
			}
		});
		while (ie.MoveNext())
		{
			object obj = ie.Current;
			yield return obj;
		}
		yield break;
	}

	public IEnumerator StartRequest(Action<Texture2D, string, WWWHelper.TimeOut> onFinished)
	{
		IEnumerator ie = this.StartRequest(delegate(WWW www, WWWHelper.TimeOut isTimeOut)
		{
			if (onFinished != null)
			{
				if (isTimeOut == WWWHelper.TimeOut.NO)
				{
					Texture2D texture2D = null;
					byte[] bytes = www.bytes;
					if (string.IsNullOrEmpty(www.error) && bytes != null)
					{
						texture2D = new Texture2D(2, 2, TextureFormat.ARGB32, false, false);
						texture2D.LoadImage(bytes, false);
					}
					onFinished(texture2D, www.error, isTimeOut);
				}
				else
				{
					onFinished(null, "Time Out", isTimeOut);
				}
			}
		});
		while (ie.MoveNext())
		{
			object obj = ie.Current;
			yield return obj;
		}
		yield break;
	}

	public IEnumerator StartRequest(Action<byte[], string, WWWHelper.TimeOut> onFinished)
	{
		IEnumerator ie = this.StartRequest(delegate(WWW www, WWWHelper.TimeOut isTimeOut)
		{
			if (onFinished != null)
			{
				if (isTimeOut == WWWHelper.TimeOut.NO)
				{
					byte[] arg = (!string.IsNullOrEmpty(www.error)) ? null : www.bytes;
					onFinished(arg, www.error, isTimeOut);
				}
				else
				{
					onFinished(null, "Time Out", isTimeOut);
				}
			}
		});
		while (ie.MoveNext())
		{
			object obj = ie.Current;
			yield return obj;
		}
		yield break;
	}

	private IEnumerator StartRequest(Action<WWW, WWWHelper.TimeOut> onFinished)
	{
		WWW www = new WWW(this.url, this.postData, this.headers);
		float startTime = Time.realtimeSinceStartup;
		this.dispose = false;
		while (!this.dispose && !www.isDone)
		{
			if (0f < this.timeoutSeconds && Time.realtimeSinceStartup - startTime >= this.timeoutSeconds)
			{
				if (onFinished != null)
				{
					onFinished(www, WWWHelper.TimeOut.YES);
				}
				www.Dispose();
				yield break;
			}
			yield return null;
		}
		if (!this.dispose)
		{
			onFinished(www, WWWHelper.TimeOut.NO);
		}
		www.Dispose();
		yield break;
	}

	public static WWWHelper LoadFromCacheOrDownload(string url, int version, uint crc, float timeoutSeconds)
	{
		return new WWWHelper
		{
			url = url,
			version = version,
			crc = crc,
			timeoutSeconds = timeoutSeconds
		};
	}

	public IEnumerator StartDownloadAssetBundle(Action<WWW> onStarted, Action<WWW> onFinished)
	{
		Hash128 versionHash = new Hash128(0u, 0u, 0u, (uint)this.version);
		WWW www = WWW.LoadFromCacheOrDownload(this.url, versionHash, this.crc);
		if (onStarted != null)
		{
			onStarted(www);
		}
		float startTime = Time.realtimeSinceStartup;
		this.dispose = false;
		bool isErrorWriteFile = false;
		while (!this.dispose && !www.isDone)
		{
			if (0f < this.timeoutSeconds && Time.realtimeSinceStartup - startTime >= this.timeoutSeconds)
			{
				if (onFinished != null)
				{
					onFinished(null);
				}
				yield break;
			}
			if (isErrorWriteFile)
			{
				if (onFinished != null)
				{
					onFinished(null);
				}
				yield break;
			}
			yield return null;
		}
		if (!this.dispose)
		{
			onFinished(www);
		}
		yield break;
	}

	public enum TimeOut
	{
		NO,
		YES
	}
}
