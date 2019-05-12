using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neptune
{
	public class OAuthConnect
	{
		private MonoBehaviour mMonoBehaviour;

		private Action<WWW> mOnHttpResponse;

		private Action mOnTimeOut;

		private float mTimeout;

		private bool mIsTimeOut;

		public OAuthConnect(MonoBehaviour monoBehaviour, Action<WWW> onHttpResponse)
		{
			this.mMonoBehaviour = monoBehaviour;
			this.mOnHttpResponse = onHttpResponse;
		}

		public OAuthConnect(MonoBehaviour monoBehaviour, Action<WWW> onHttpResponse, float timeout, Action onTimeOut)
		{
			this.mMonoBehaviour = monoBehaviour;
			this.mOnHttpResponse = onHttpResponse;
			this.mTimeout = timeout;
			this.mOnTimeOut = onTimeOut;
		}

		public IEnumerator OAuthPostRequest(string url, Dictionary<string, object> post, Dictionary<string, string> headers)
		{
			WWWForm form = new WWWForm();
			WWW www = null;
			if (post != null)
			{
				foreach (KeyValuePair<string, object> p in post)
				{
					form.AddField(p.Key, p.Value.ToString());
				}
				www = new WWW(url, form.data, headers);
			}
			else
			{
				www = new WWW(url, null, headers);
			}
			yield return this.mMonoBehaviour.StartCoroutine(this.WaitForRequest(www));
			if (this.mOnHttpResponse != null && !this.mIsTimeOut)
			{
				this.mOnHttpResponse(www);
			}
			yield break;
		}

		private IEnumerator WaitForRequest(WWW www)
		{
			float requestTime = Time.time;
			while (!www.isDone)
			{
				if (this.mTimeout > 0f && Time.time - requestTime >= this.mTimeout)
				{
					global::Debug.LogWarning("#=#=# TimeOut");
					if (this.mOnTimeOut != null)
					{
						if (www != null)
						{
							www.Dispose();
						}
						this.mIsTimeOut = true;
						this.mOnTimeOut();
					}
					break;
				}
				yield return null;
			}
			yield return null;
			yield break;
		}
	}
}
