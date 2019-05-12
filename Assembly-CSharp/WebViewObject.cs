using System;
using UnityEngine;

public class WebViewObject : MonoBehaviour
{
	private Action<string> callback;

	private AndroidJavaObject webView;

	private Vector2 offset;

	public void Init(Action<string> cb = null)
	{
		this.callback = cb;
		this.offset = new Vector2(0f, 0f);
		this.webView = new AndroidJavaObject("net.gree.unitywebview.WebViewPlugin", new object[0]);
		this.webView.Call("Init", new object[]
		{
			base.name
		});
	}

	private void OnDestroy()
	{
		if (this.webView == null)
		{
			return;
		}
		this.webView.Call("Destroy", new object[0]);
	}

	public void SetMargins(int x, int y, int width, int height)
	{
	}

	public void SetVisibility(bool v)
	{
		if (this.webView == null)
		{
			return;
		}
		this.webView.Call("SetVisibility", new object[]
		{
			v
		});
	}

	public void LoadURL(string url)
	{
		if (this.webView == null)
		{
			return;
		}
		this.webView.Call("LoadURL", new object[]
		{
			url
		});
	}

	public void EvaluateJS(string js)
	{
		if (this.webView == null)
		{
			return;
		}
		this.webView.Call("LoadURL", new object[]
		{
			"javascript:" + js
		});
	}

	public void CallFromJS(string message)
	{
		if (this.callback != null)
		{
			this.callback(message);
		}
	}
}
