using System;
using System.Collections;
using System.Collections.Generic;

namespace com.adjust.sdk
{
	public class AdjustUtils
	{
		public static string KeyAdid = "adid";

		public static string KeyMessage = "message";

		public static string KeyNetwork = "network";

		public static string KeyAdgroup = "adgroup";

		public static string KeyCampaign = "campaign";

		public static string KeyCreative = "creative";

		public static string KeyWillRetry = "willRetry";

		public static string KeyTimestamp = "timestamp";

		public static string KeyEventToken = "eventToken";

		public static string KeyClickLabel = "clickLabel";

		public static string KeyTrackerName = "trackerName";

		public static string KeyTrackerToken = "trackerToken";

		public static string KeyJsonResponse = "jsonResponse";

		public static int ConvertLogLevel(AdjustLogLevel? logLevel)
		{
			if (logLevel == null)
			{
				return -1;
			}
			return (int)logLevel.Value;
		}

		public static int ConvertBool(bool? value)
		{
			if (value == null)
			{
				return -1;
			}
			if (value.Value)
			{
				return 1;
			}
			return 0;
		}

		public static double ConvertDouble(double? value)
		{
			if (value == null)
			{
				return -1.0;
			}
			return value.Value;
		}

		public static string ConvertListToJson(List<string> list)
		{
			if (list == null)
			{
				return null;
			}
			JSONArray jsonarray = new JSONArray();
			foreach (string aData in list)
			{
				jsonarray.Add(new JSONData(aData));
			}
			return jsonarray.ToString();
		}

		public static string GetJsonResponseCompact(Dictionary<string, object> dictionary)
		{
			string text = string.Empty;
			if (dictionary == null)
			{
				return text;
			}
			int num = 0;
			text += "{";
			foreach (KeyValuePair<string, object> keyValuePair in dictionary)
			{
				string text2 = keyValuePair.Value as string;
				if (text2 != null)
				{
					if (++num > 1)
					{
						text += ",";
					}
					string text3 = text;
					text = string.Concat(new string[]
					{
						text3,
						"\"",
						keyValuePair.Key,
						"\":\"",
						text2,
						"\""
					});
				}
				else
				{
					Dictionary<string, object> dictionary2 = keyValuePair.Value as Dictionary<string, object>;
					if (++num > 1)
					{
						text += ",";
					}
					text = text + "\"" + keyValuePair.Key + "\":";
					text += AdjustUtils.GetJsonResponseCompact(dictionary2);
				}
			}
			text += "}";
			return text;
		}

		public static string GetJsonString(JSONNode node, string key)
		{
			if (node == null)
			{
				return null;
			}
			JSONData jsondata = node[key] as JSONData;
			if (jsondata == null)
			{
				return null;
			}
			return jsondata.Value;
		}

		public static void WriteJsonResponseDictionary(JSONClass jsonObject, Dictionary<string, object> output)
		{
			IEnumerator enumerator = jsonObject.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					KeyValuePair<string, JSONNode> keyValuePair = (KeyValuePair<string, JSONNode>)obj;
					JSONClass asObject = keyValuePair.Value.AsObject;
					string key = keyValuePair.Key;
					if (asObject == null)
					{
						string value = keyValuePair.Value.Value;
						output.Add(key, value);
					}
					else
					{
						Dictionary<string, object> dictionary = new Dictionary<string, object>();
						output.Add(key, dictionary);
						AdjustUtils.WriteJsonResponseDictionary(asObject, dictionary);
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}
	}
}
