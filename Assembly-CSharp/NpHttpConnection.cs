using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpHttpConnection
{
	private MonoBehaviour mMonoBehaviour;

	private Action<WWW> mOnHttpResponse;

	private Action mOnTimeOut;

	private bool mIsTimeOut;

	public NpHttpConnection(MonoBehaviour monoBehaviour, Action<WWW> onHttpResponse, Action onTimeOut)
	{
		this.mMonoBehaviour = monoBehaviour;
		this.mOnHttpResponse = onHttpResponse;
		this.mOnTimeOut = onTimeOut;
		this.TimeOut = 30f;
	}

	public float TimeOut { get; set; }

	public IEnumerator PostRequest(string url, Dictionary<string, object> post, Dictionary<string, string> headers)
	{
		WWWForm form = new WWWForm();
		WWW www = null;
		if (post != null)
		{
			foreach (KeyValuePair<string, object> keyValuePair in post)
			{
				form.AddField(keyValuePair.Key, keyValuePair.Value.ToString());
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
		float requestTime = Time.realtimeSinceStartup;
		while (!www.isDone)
		{
			if (this.TimeOut > 0f && Time.realtimeSinceStartup - requestTime >= this.TimeOut)
			{
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
