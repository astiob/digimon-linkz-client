using System;
using System.Collections.Generic;
using UnityEngine;

namespace Neptune.WebView
{
	public class NpWebView
	{
		private string mGameObjectName = string.Empty;

		[Obsolete("This constructor has been deprecated.", false)]
		public NpWebView(GameObject gameObject, INpWebViewListener listener)
		{
			this.mGameObjectName = gameObject.name;
			global::Debug.Log(string.Format("CallBackGoName = {0}", this.mGameObjectName));
		}

		public NpWebView()
		{
			this.mGameObjectName = string.Empty;
		}

		public bool IsIndicatorHiden
		{
			get
			{
				return NpWebViewAndroid.IsIndicator;
			}
			set
			{
				NpWebViewAndroid.IsIndicator = value;
			}
		}

		public void SetNpWebViewListener(GameObject gameObject, INpWebViewListener listener)
		{
			if (gameObject == null)
			{
				global::Debug.LogError("#=#=# SetNpWebViewListener : gameObject is null.");
				return;
			}
			if (listener == null)
			{
				global::Debug.LogError("#=#=# SetNpWebViewListener : listener is null.");
				return;
			}
			this.mGameObjectName = gameObject.name;
		}

		public void SetActionListener(GameObject gameObject, INpActionListener listener)
		{
			this.mGameObjectName = gameObject.name;
		}

		public void Open(string path, Dictionary<string, string> header, string param)
		{
			if (header == null)
			{
				header = new Dictionary<string, string>();
				header.Add("Authorization", string.Empty);
				header.Add("x-guid", string.Empty);
				header.Add("x-uuid", string.Empty);
				header.Add("country-code", string.Empty);
				header.Add("x-appVer", string.Empty);
				header.Add("x-device", string.Empty);
				header.Add("x-osver", string.Empty);
				header.Add("x-ostype", string.Empty);
			}
			NpWebViewAndroid.Open(path, header, param, this.mGameObjectName);
		}

		public void Close()
		{
			NpWebViewAndroid.Close();
		}

		public void SetActive(bool active)
		{
			NpWebViewAndroid.SetActive(active);
		}

		public void Destroy()
		{
			NpWebViewAndroid.Destroy();
		}

		public void ReSize(float x, float y, float width, float height)
		{
			NpWebViewAndroid.ReSize(x, y, width, height);
		}

		public void ReLoad()
		{
			NpWebViewAndroid.ReLoad();
		}

		public void IsBound(bool bound)
		{
		}

		public void RemoveAllChache()
		{
		}

		public void IsScroll(bool scroll)
		{
		}

		public void NextPage()
		{
			NpWebViewAndroid.NextPage();
		}

		public void BackPage()
		{
			NpWebViewAndroid.BackPage();
		}

		public bool IsAccess(NpWebViewAccessType type)
		{
			return NpWebViewAndroid.IsAccess((int)type);
		}

		public void SetBackgroundColor(float r, float g, float b, float a)
		{
			NpWebViewAndroid.SetBackgroundColor(r, g, b, a);
		}

		public void SetExternalBrowserUrl(List<string> externalBrowserUrlList)
		{
			if (externalBrowserUrlList == null)
			{
				global::Debug.Log("NpWebViewController : externalBrowserUrlList is Null.");
				return;
			}
			NpWebViewAndroid.SetExternalBrowserUrl(externalBrowserUrlList);
		}

		public void ClearExternalBrowserUrl()
		{
			NpWebViewAndroid.ClearExternalBrowserUrl();
		}

		public void SetHardwareAccelerated(bool enabled)
		{
			NpWebViewAndroid.SetHardwareAccelerated(enabled);
		}
	}
}
