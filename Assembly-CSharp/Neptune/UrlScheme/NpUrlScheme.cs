using JsonFx.Json;
using System;
using System.Collections.Generic;

namespace Neptune.UrlScheme
{
	public class NpUrlScheme
	{
		public static bool IsUrlSchemeAction()
		{
			return NpUrlSchemeAndroid.IsUrlSchemeAction();
		}

		public static string GetScheme()
		{
			string empty = string.Empty;
			return NpUrlSchemeAndroid.GetScheme();
		}

		public static string GetHost()
		{
			string empty = string.Empty;
			return NpUrlSchemeAndroid.GetHost();
		}

		public static string GetPath()
		{
			string empty = string.Empty;
			return NpUrlSchemeAndroid.GetPath();
		}

		public static Dictionary<string, object> GetQueryParam()
		{
			string value = string.Empty;
			value = NpUrlSchemeAndroid.GetQueryParamJson();
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (!string.IsNullOrEmpty(value))
			{
				dictionary = JsonReader.Deserialize<Dictionary<string, object>>(value);
			}
			return (dictionary.Count > 0) ? dictionary : null;
		}

		public static void Clear()
		{
			NpUrlSchemeAndroid.Clear();
		}
	}
}
