using System;

namespace Neptune.WebView
{
	public interface INpWebViewListener
	{
		void OnShouldOverrideUrlLoading(string url);

		void OnPageStarted(string url);

		void OnPageFinished(string url);

		void OnReceivedError(string url);
	}
}
