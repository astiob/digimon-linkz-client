using LitJson;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using WebAPIRequest;

public class WebAPIJsonParse : MonoBehaviour
{
	private static readonly char[] TRIMS_CHAR_CODE = new char[]
	{
		':',
		'"',
		' ',
		'\t'
	};

	private static StringBuilder strBuilder = new StringBuilder();

	public static string CreateJsonData(string apiId, object data)
	{
		return string.Concat(new string[]
		{
			"{\"",
			apiId,
			"\":",
			JsonMapper.ToJson(data),
			"}"
		});
	}

	public static string CreateJsonData(List<RequestBase> requestList)
	{
		WebAPIJsonParse.strBuilder.Length = 0;
		WebAPIJsonParse.strBuilder.Append('{');
		for (int i = 0; i < requestList.Count; i++)
		{
			if (0 < i)
			{
				WebAPIJsonParse.strBuilder.Append(',');
			}
			WebAPIJsonParse.strBuilder.Append('"');
			WebAPIJsonParse.strBuilder.Append(requestList[i].apiId);
			WebAPIJsonParse.strBuilder.Append("\":");
			WebAPIJsonParse.strBuilder.Append(JsonMapper.ToJson(requestList[i].param));
		}
		WebAPIJsonParse.strBuilder.Append('}');
		return WebAPIJsonParse.strBuilder.ToString();
	}

	public static void GetResponseData<RecvDataT>(string responseBody, out RecvDataT responseData) where RecvDataT : WebAPI.RecvBaseData, new()
	{
		try
		{
			if (string.IsNullOrEmpty(responseBody))
			{
				responseData = Activator.CreateInstance<RecvDataT>();
			}
			else
			{
				responseData = JsonMapper.ToObject<RecvDataT>(responseBody);
			}
		}
		catch (JsonException ex)
		{
			throw ex;
		}
		catch (Exception ex2)
		{
			throw new JsonException(ex2.ToString());
		}
	}

	public static Dictionary<string, string> GetResponseList(List<RequestBase> requestList, string json)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (!string.IsNullOrEmpty(json))
		{
			int num = json.IndexOf("resData");
			if (num == -1)
			{
				throw new JsonException("json parse error : GetJsonData");
			}
			while (json.Length > num && json[num] != '{')
			{
				num++;
			}
			num++;
			while (json.Length > num)
			{
				int num2 = num;
				while (json.Length > num2 && json[num2] != '{')
				{
					num2++;
				}
				if (json.Length <= num2)
				{
					break;
				}
				for (int i = dictionary.Count; i < requestList.Count; i++)
				{
					int num3 = json.IndexOf("null", num, num2 - num);
					if (num3 != -1)
					{
						dictionary.Add(requestList[i].apiId, string.Empty);
						num = num3 + "null".Length;
					}
				}
				for (int j = dictionary.Count; j < requestList.Count; j++)
				{
					if (json.IndexOf(requestList[j].apiId, num, num2 - num) != -1)
					{
						int num4 = WebAPIJsonParse.SkipBody(json, num2 + 1);
						dictionary.Add(requestList[j].apiId, json.Substring(num2, num4 - num2).Trim());
						num = num4;
						break;
					}
				}
				if (num2 == num)
				{
					num = WebAPIJsonParse.SkipBody(json, num2 + 1);
					global::Debug.LogWarning("WebAPIJsonParse.GetResponseList : Not Found Response");
				}
			}
		}
		return dictionary;
	}

	public static string GetString(string json, string key)
	{
		int num = json.IndexOf(key);
		if (num == -1)
		{
			throw new JsonException("json parse error : GetString Not Key = " + key);
		}
		int num2 = num + key.Length + 1;
		num = json.IndexOf(',', num2);
		if (num == -1)
		{
			throw new JsonException("json parse error : GetString Not Key = " + key);
		}
		string text = json.Substring(num2, num - num2);
		return text.Trim(WebAPIJsonParse.TRIMS_CHAR_CODE);
	}

	public static int GetInt(string json, string key)
	{
		int num = json.IndexOf(key);
		if (num == -1)
		{
			throw new JsonException("json parse error : GetInt Not Key = " + key);
		}
		int num2 = num + key.Length + 1;
		num = json.IndexOf(',', num2);
		if (num == -1)
		{
			num = json.IndexOf('}', num2);
			if (num == -1)
			{
				throw new JsonException("json parse error : GetInt Not Key = " + key);
			}
		}
		string text = json.Substring(num2, num - num2);
		text = text.Trim(WebAPIJsonParse.TRIMS_CHAR_CODE);
		return int.Parse(text);
	}

	public static string GetBody(string json)
	{
		string result;
		try
		{
			int num = json.IndexOf("resData");
			num = json.IndexOf(":", num);
			num++;
			if ("null" == json.Substring(num, "null".Length))
			{
				result = string.Empty;
			}
			else
			{
				num = json.IndexOf(":", num);
				num++;
				int num2 = WebAPIJsonParse.SkipBody(json, num + 1);
				string text = json.Substring(num, num2 - num);
				if ("null" == text.Substring(0, "null".Length))
				{
					result = string.Empty;
				}
				else
				{
					result = text.Trim();
				}
			}
		}
		catch (Exception ex)
		{
			throw new JsonException(ex.ToString());
		}
		return result;
	}

	private static int SkipBody(string json, int startScope)
	{
		int num = 1;
		int num2 = startScope;
		while (0 < num)
		{
			int i = num2;
			while (json.Length > i)
			{
				if (json[i] == '"')
				{
					i = WebAPIJsonParse.SkipString(json, i);
				}
				if (json[i] == '}')
				{
					break;
				}
				i++;
			}
			if (json.Length <= i)
			{
				throw new JsonException("json parse error : SkipBody");
			}
			num--;
			while (i >= num2)
			{
				if (json[num2] == '"')
				{
					num2 = WebAPIJsonParse.SkipString(json, num2);
				}
				if (json[num2] == '{')
				{
					num++;
				}
				num2++;
			}
		}
		return num2;
	}

	private static int SkipString(string json, int startString)
	{
		int num = startString;
		if (json[startString] == '"')
		{
			num++;
			bool flag = false;
			while (!flag && json.Length > num)
			{
				if (json[num] == '\\')
				{
					num++;
				}
				else if (json[num] == '"')
				{
					flag = true;
				}
				num++;
			}
		}
		return num;
	}
}
