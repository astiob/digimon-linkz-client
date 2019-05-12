using System;
using System.Collections.Generic;

namespace UnityEngine.Networking.Match
{
	/// <summary>
	///   <para>A response object base.</para>
	/// </summary>
	public abstract class ResponseBase
	{
		public abstract void Parse(object obj);

		internal string ParseJSONString(string name, object obj, IDictionary<string, object> dictJsonObj)
		{
			if (dictJsonObj.TryGetValue(name, out obj))
			{
				return obj as string;
			}
			throw new FormatException(name + " not found in JSON dictionary");
		}

		internal short ParseJSONInt16(string name, object obj, IDictionary<string, object> dictJsonObj)
		{
			if (dictJsonObj.TryGetValue(name, out obj))
			{
				return Convert.ToInt16(obj);
			}
			throw new FormatException(name + " not found in JSON dictionary");
		}

		internal int ParseJSONInt32(string name, object obj, IDictionary<string, object> dictJsonObj)
		{
			if (dictJsonObj.TryGetValue(name, out obj))
			{
				return Convert.ToInt32(obj);
			}
			throw new FormatException(name + " not found in JSON dictionary");
		}

		internal long ParseJSONInt64(string name, object obj, IDictionary<string, object> dictJsonObj)
		{
			if (dictJsonObj.TryGetValue(name, out obj))
			{
				return Convert.ToInt64(obj);
			}
			throw new FormatException(name + " not found in JSON dictionary");
		}

		internal ushort ParseJSONUInt16(string name, object obj, IDictionary<string, object> dictJsonObj)
		{
			if (dictJsonObj.TryGetValue(name, out obj))
			{
				return Convert.ToUInt16(obj);
			}
			throw new FormatException(name + " not found in JSON dictionary");
		}

		internal uint ParseJSONUInt32(string name, object obj, IDictionary<string, object> dictJsonObj)
		{
			if (dictJsonObj.TryGetValue(name, out obj))
			{
				return Convert.ToUInt32(obj);
			}
			throw new FormatException(name + " not found in JSON dictionary");
		}

		internal ulong ParseJSONUInt64(string name, object obj, IDictionary<string, object> dictJsonObj)
		{
			if (dictJsonObj.TryGetValue(name, out obj))
			{
				return Convert.ToUInt64(obj);
			}
			throw new FormatException(name + " not found in JSON dictionary");
		}

		internal bool ParseJSONBool(string name, object obj, IDictionary<string, object> dictJsonObj)
		{
			if (dictJsonObj.TryGetValue(name, out obj))
			{
				return Convert.ToBoolean(obj);
			}
			throw new FormatException(name + " not found in JSON dictionary");
		}

		internal DateTime ParseJSONDateTime(string name, object obj, IDictionary<string, object> dictJsonObj)
		{
			throw new FormatException(name + " DateTime not yet supported");
		}

		internal List<string> ParseJSONListOfStrings(string name, object obj, IDictionary<string, object> dictJsonObj)
		{
			if (dictJsonObj.TryGetValue(name, out obj))
			{
				List<object> list = obj as List<object>;
				if (list != null)
				{
					List<string> list2 = new List<string>();
					foreach (object obj2 in list)
					{
						IDictionary<string, object> dictionary = (IDictionary<string, object>)obj2;
						foreach (KeyValuePair<string, object> keyValuePair in dictionary)
						{
							string item = (string)keyValuePair.Value;
							list2.Add(item);
						}
					}
					return list2;
				}
			}
			throw new FormatException(name + " not found in JSON dictionary");
		}

		internal List<T> ParseJSONList<T>(string name, object obj, IDictionary<string, object> dictJsonObj) where T : ResponseBase, new()
		{
			if (dictJsonObj.TryGetValue(name, out obj))
			{
				List<object> list = obj as List<object>;
				if (list != null)
				{
					List<T> list2 = new List<T>();
					foreach (object obj2 in list)
					{
						IDictionary<string, object> obj3 = (IDictionary<string, object>)obj2;
						T item = Activator.CreateInstance<T>();
						item.Parse(obj3);
						list2.Add(item);
					}
					return list2;
				}
			}
			throw new FormatException(name + " not found in JSON dictionary");
		}
	}
}
