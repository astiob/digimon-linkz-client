using System;
using System.Collections.Generic;

public class UnityObjectPooler<Object>
{
	public const string SplitKey = "#";

	private Dictionary<string, Object> objects = new Dictionary<string, Object>();

	public Object this[string key]
	{
		get
		{
			return this.objects[key];
		}
		set
		{
			this.objects[key] = value;
		}
	}

	public bool ContainsValue(Object value)
	{
		return this.objects.ContainsValue(value);
	}

	public bool ContainsKey(string key)
	{
		return this.objects.ContainsKey(key);
	}

	public string[] ContainsExtractKeys(Object value)
	{
		List<string> list = new List<string>();
		string[] array = new string[this.objects.Keys.Count];
		this.objects.Keys.CopyTo(array, 0);
		for (int i = 0; i < array.Length; i++)
		{
			Object @object = this.objects[array[i]];
			if (@object.Equals(value))
			{
				list.Add(array[i]);
			}
		}
		return list.ToArray();
	}

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
			Debug.LogWarning(key + "は既に使用されています.");
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
			Debug.LogWarning(masterKey + "#" + subKey + "は既に使用されています.");
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

	public void Replace(string key, Object g)
	{
		if (this.objects.ContainsKey(key))
		{
			this.objects.Remove(key);
		}
		this.objects.Add(key, g);
	}

	public void Replace(string masterKey, string subKey, Object g)
	{
		if (this.objects.ContainsKey(masterKey + "#" + subKey))
		{
			this.objects.Remove(masterKey + "#" + subKey);
		}
		this.objects.Add(masterKey + "#" + subKey, g);
	}

	public void Remove(string key)
	{
		if (!this.objects.ContainsKey(key))
		{
			Debug.LogWarning(key + "は存在しません.");
			return;
		}
		this.objects.Remove(key);
	}

	public void Remove(string masterKey, string subKey)
	{
		if (!this.objects.ContainsKey(masterKey + "#" + subKey))
		{
			Debug.LogWarning(masterKey + "#" + subKey + "は存在しません.");
			return;
		}
		this.objects.Remove(masterKey + "#" + subKey);
	}

	public Dictionary<string, Object> Objects
	{
		get
		{
			return this.objects;
		}
	}

	public Object[] GetAllObject(bool includeNull = false)
	{
		List<Object> list = new List<Object>();
		foreach (KeyValuePair<string, Object> keyValuePair in this.objects)
		{
			if (includeNull || (!includeNull && keyValuePair.Value != null))
			{
				list.Add(keyValuePair.Value);
			}
		}
		return list.ToArray();
	}
}
