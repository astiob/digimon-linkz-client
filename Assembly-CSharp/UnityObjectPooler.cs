using System;
using System.Collections.Generic;

public class UnityObjectPooler<Object>
{
	private const string SplitKey = "#";

	private Dictionary<string, Object> objects = new Dictionary<string, Object>();

	public Object GetObject(string key)
	{
		if (this.objects.ContainsKey(key))
		{
			return this.objects[key];
		}
		Debug.LogWarning(key + "に対応するオブジェクトが存在しません.");
		return default(Object);
	}

	public Object GetObject(string masterKey, string subKey)
	{
		if (this.objects.ContainsKey(masterKey + "#" + subKey))
		{
			return this.objects[masterKey + "#" + subKey];
		}
		Debug.LogWarning(masterKey + "#" + subKey + "に対応するオブジェクトが存在しません.");
		return default(Object);
	}

	public void Add(string key, Object g)
	{
		if (this.objects.ContainsKey(key))
		{
			return;
		}
		if (g == null)
		{
			Debug.LogWarning("追加されたオブジェクトがnullです. (" + key + ")");
		}
		this.objects.Add(key, g);
	}

	public void Add(string masterKey, string subKey, Object g)
	{
		if (this.objects.ContainsKey(masterKey + "#" + subKey))
		{
			return;
		}
		if (g == null)
		{
			Debug.LogWarning(string.Concat(new string[]
			{
				"追加されたオブジェクトがnullです. (",
				masterKey,
				"#",
				subKey,
				")"
			}));
		}
		this.objects.Add(masterKey + "#" + subKey, g);
	}

	public Object[] GetAllObject()
	{
		List<Object> list = new List<Object>();
		foreach (KeyValuePair<string, Object> keyValuePair in this.objects)
		{
			if (keyValuePair.Value != null)
			{
				list.Add(keyValuePair.Value);
			}
		}
		return list.ToArray();
	}
}
