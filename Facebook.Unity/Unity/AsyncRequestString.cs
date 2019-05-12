using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facebook.Unity
{
	internal class AsyncRequestString : MonoBehaviour
	{
		private Uri url;

		private HttpMethod method;

		private IDictionary<string, string> formData;

		private WWWForm query;

		private FacebookDelegate<IGraphResult> callback;

		internal static void Post(Uri url, Dictionary<string, string> formData = null, FacebookDelegate<IGraphResult> callback = null)
		{
			AsyncRequestString.Request(url, HttpMethod.POST, formData, callback);
		}

		internal static void Get(Uri url, Dictionary<string, string> formData = null, FacebookDelegate<IGraphResult> callback = null)
		{
			AsyncRequestString.Request(url, HttpMethod.GET, formData, callback);
		}

		internal static void Request(Uri url, HttpMethod method, WWWForm query = null, FacebookDelegate<IGraphResult> callback = null)
		{
			ComponentFactory.AddComponent<AsyncRequestString>().SetUrl(url).SetMethod(method).SetQuery(query).SetCallback(callback);
		}

		internal static void Request(Uri url, HttpMethod method, IDictionary<string, string> formData = null, FacebookDelegate<IGraphResult> callback = null)
		{
			ComponentFactory.AddComponent<AsyncRequestString>().SetUrl(url).SetMethod(method).SetFormData(formData).SetCallback(callback);
		}

		internal IEnumerator Start()
		{
			WWW www;
			if (this.method == HttpMethod.GET)
			{
				string text = (!this.url.AbsoluteUri.Contains("?")) ? "?" : "&";
				if (this.formData != null)
				{
					foreach (KeyValuePair<string, string> keyValuePair in this.formData)
					{
						text += string.Format("{0}={1}&", Uri.EscapeDataString(keyValuePair.Key), Uri.EscapeDataString(keyValuePair.Value));
					}
				}
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary["User-Agent"] = Constants.GraphApiUserAgent;
				www = new WWW(this.url + text, null, dictionary);
			}
			else
			{
				if (this.query == null)
				{
					this.query = new WWWForm();
				}
				if (this.method == HttpMethod.DELETE)
				{
					this.query.AddField("method", "delete");
				}
				if (this.formData != null)
				{
					foreach (KeyValuePair<string, string> keyValuePair2 in this.formData)
					{
						this.query.AddField(keyValuePair2.Key, keyValuePair2.Value);
					}
				}
				this.query.headers["User-Agent"] = Constants.GraphApiUserAgent;
				www = new WWW(this.url.AbsoluteUri, this.query);
			}
			yield return www;
			if (this.callback != null)
			{
				this.callback(new GraphResult(www));
			}
			www.Dispose();
			UnityEngine.Object.Destroy(this);
			yield break;
		}

		internal AsyncRequestString SetUrl(Uri url)
		{
			this.url = url;
			return this;
		}

		internal AsyncRequestString SetMethod(HttpMethod method)
		{
			this.method = method;
			return this;
		}

		internal AsyncRequestString SetFormData(IDictionary<string, string> formData)
		{
			this.formData = formData;
			return this;
		}

		internal AsyncRequestString SetQuery(WWWForm query)
		{
			this.query = query;
			return this;
		}

		internal AsyncRequestString SetCallback(FacebookDelegate<IGraphResult> callback)
		{
			this.callback = callback;
			return this;
		}
	}
}
