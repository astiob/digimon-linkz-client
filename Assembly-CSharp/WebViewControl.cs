using Neptune.OAuth;
using Neptune.WebView;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WebViewControl : MonoBehaviour, INpWebViewListener
{
	private Rect webViewRect = default(Rect);

	private Vector2 webViewPositionOffset = new Vector2(0f, 0f);

	private Vector2 screenScale = new Vector2(1f, 1f);

	private Camera orthoCamera;

	private string urlParameter;

	private NpWebView webViewObject;

	private bool isOpen;

	private void InitializeWebViewInstance()
	{
		if (this.webViewObject == null)
		{
			this.webViewObject = new NpWebView();
			this.webViewObject.SetHardwareAccelerated(true);
			this.webViewObject.SetNpWebViewListener(base.gameObject, this);
		}
	}

	private void GetScreenPoint(out Vector3 leftBottom, out Vector3 rightTop)
	{
		int num = Mathf.CeilToInt(this.webViewRect.y + this.webViewPositionOffset.y);
		int num2 = Mathf.CeilToInt(this.webViewRect.y - this.webViewRect.height + this.webViewPositionOffset.y);
		int num3 = Mathf.CeilToInt(this.webViewRect.x + this.webViewPositionOffset.x);
		int num4 = Mathf.CeilToInt(this.webViewRect.x + this.webViewRect.width + this.webViewPositionOffset.x);
		Vector3 zero = Vector3.zero;
		zero.x = (float)num3 * this.screenScale.x;
		zero.y = (float)num2 * this.screenScale.y;
		leftBottom = this.orthoCamera.WorldToScreenPoint(zero);
		zero.x = (float)num4 * this.screenScale.x;
		zero.y = (float)num * this.screenScale.y;
		rightTop = this.orthoCamera.WorldToScreenPoint(zero);
	}

	private List<int> GetWebViewMargin(Vector3 leftBottom, Vector3 rightTop)
	{
		float num = (float)Screen.width;
		float num2 = (float)Screen.height;
		int item = (int)leftBottom.x;
		int item2 = (int)(num2 - rightTop.y);
		int item3 = (int)(num - rightTop.x);
		int item4 = (int)leftBottom.y;
		return new List<int>
		{
			item,
			item2,
			item3,
			item4
		};
	}

	private void SetWebViewSize(List<int> margin)
	{
		int num = this.orthoCamera.pixelWidth - (margin[0] + margin[2]);
		int num2 = this.orthoCamera.pixelHeight - (margin[1] + margin[3]);
		this.webViewObject.ReSize((float)margin[0], (float)margin[1], (float)num, (float)num2);
	}

	private Dictionary<string, string> GetRequestHeader(string url)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary = NpOAuth.Instance.RequestHeaderDic("GET", url, null);
		dictionary["Content-Type"] = "application/x-www-form-urlencoded";
		dictionary["X-AppVer"] = WebAPIPlatformValue.GetAppVersion();
		dictionary["X-TimeZone"] = global::TimeZone.GetTimezoneName();
		dictionary["X-Lang"] = CountrySetting.GetCountryCode(CountrySetting.CountryCode.EN);
		return dictionary;
	}

	private string GetUrlParameter(string url)
	{
		string result = string.Empty;
		if (!string.IsNullOrEmpty(this.urlParameter))
		{
			if (url.IndexOf('?') == -1)
			{
				result = "?" + this.urlParameter;
			}
			else
			{
				result = "&" + this.urlParameter;
			}
		}
		return result;
	}

	public void OnShouldOverrideUrlLoading(string url)
	{
	}

	public void OnPageStarted(string url)
	{
	}

	public void OnPageFinished(string url)
	{
	}

	public void OnReceivedError(string url)
	{
	}

	public void SetWebViewRect(float x, float y, float width, float height)
	{
		this.webViewRect.x = x;
		this.webViewRect.y = y;
		this.webViewRect.width = width;
		this.webViewRect.height = height;
	}

	public void SetWebViewPositionOffset(float x, float y)
	{
		this.webViewPositionOffset.x = x;
		this.webViewPositionOffset.y = y;
	}

	public void SetScreenScale(float x, float y)
	{
		this.screenScale.x = x;
		this.screenScale.y = y;
	}

	public void SetOrthoCamera(Camera camera)
	{
		this.orthoCamera = camera;
	}

	public void SetWebViewPosition()
	{
		this.InitializeWebViewInstance();
		Vector3 leftBottom;
		Vector3 rightTop;
		this.GetScreenPoint(out leftBottom, out rightTop);
		List<int> webViewMargin = this.GetWebViewMargin(leftBottom, rightTop);
		this.SetWebViewSize(webViewMargin);
	}

	public void SetUrlParameter(string parameter)
	{
		this.urlParameter = parameter;
	}

	public void OpenWebView(string url)
	{
		if (this.webViewObject != null)
		{
			if (this.isOpen)
			{
				this.isOpen = false;
				this.webViewObject.Close();
			}
			url += this.GetUrlParameter(url);
			this.webViewObject.Open(url, this.GetRequestHeader(url), string.Empty);
			this.isOpen = true;
		}
	}

	public void DeleteWebView()
	{
		if (this.webViewObject != null)
		{
			this.isOpen = false;
			this.webViewObject.Close();
			this.webViewObject.Destroy();
			this.webViewObject = null;
		}
	}
}
