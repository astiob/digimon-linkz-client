using System;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Scripting;

namespace UnityEngineInternal
{
	internal static class WebRequestUtils
	{
		private static Regex domainRegex = new Regex("^\\s*\\w+(?:\\.\\w+)+(\\/.*)?$");

		[RequiredByNativeCode]
		internal static string RedirectTo(string baseUri, string redirectUri)
		{
			Uri uri;
			if (redirectUri[0] == '/')
			{
				uri = new Uri(redirectUri, UriKind.Relative);
			}
			else
			{
				uri = new Uri(redirectUri, UriKind.RelativeOrAbsolute);
			}
			string absoluteUri;
			if (uri.IsAbsoluteUri)
			{
				absoluteUri = uri.AbsoluteUri;
			}
			else
			{
				Uri baseUri2 = new Uri(baseUri, UriKind.Absolute);
				Uri uri2 = new Uri(baseUri2, uri);
				absoluteUri = uri2.AbsoluteUri;
			}
			return absoluteUri;
		}

		internal static string MakeInitialUrl(string targetUrl, string localUrl)
		{
			bool prependingProtocol = false;
			Uri uri = new Uri(localUrl);
			Uri uri2 = null;
			if (targetUrl[0] == '/')
			{
				uri2 = new Uri(uri, targetUrl);
				prependingProtocol = true;
			}
			if (uri2 == null && WebRequestUtils.domainRegex.IsMatch(targetUrl))
			{
				targetUrl = uri.Scheme + "://" + targetUrl;
				prependingProtocol = true;
			}
			FormatException ex = null;
			try
			{
				if (uri2 == null && targetUrl[0] != '.')
				{
					uri2 = new Uri(targetUrl);
				}
			}
			catch (FormatException ex2)
			{
				ex = ex2;
			}
			if (uri2 == null)
			{
				try
				{
					uri2 = new Uri(uri, targetUrl);
					prependingProtocol = true;
				}
				catch (FormatException)
				{
					throw ex;
				}
			}
			return WebRequestUtils.MakeUriString(uri2, targetUrl, prependingProtocol);
		}

		internal static string MakeUriString(Uri targetUri, string targetUrl, bool prependingProtocol)
		{
			string result;
			if (targetUrl.StartsWith("file://", StringComparison.OrdinalIgnoreCase))
			{
				if (targetUrl.Contains("%"))
				{
					byte[] bytes = Encoding.UTF8.GetBytes(targetUrl);
					byte[] bytes2 = WWWTranscoder.URLDecode(bytes);
					result = Encoding.UTF8.GetString(bytes2);
				}
				else
				{
					result = targetUrl;
				}
			}
			else if (targetUrl.Contains("%"))
			{
				result = targetUri.OriginalString;
			}
			else
			{
				string scheme = targetUri.Scheme;
				if (!prependingProtocol && targetUrl.Length >= scheme.Length + 2 && targetUrl[scheme.Length + 1] != '/')
				{
					StringBuilder stringBuilder = new StringBuilder(scheme, targetUrl.Length);
					stringBuilder.Append(':');
					stringBuilder.Append(targetUri.PathAndQuery);
					stringBuilder.Append(targetUri.Fragment);
					result = stringBuilder.ToString();
				}
				else
				{
					result = targetUri.AbsoluteUri;
				}
			}
			return result;
		}
	}
}
