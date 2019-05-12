using System;
using System.Collections.Generic;
using UnityEngine;

public class UnityObjectPooler
{
	public const string SplitKey = "#";

	private Dictionary<string, GameObject> objects = new Dictionary<string, GameObject>();

	public Dictionary<string, GameObject> Value
	{
		get
		{
			return this.objects;
		}
		set
		{
			this.objects = value;
		}
	}

	public bool ContainsValue(GameObject value)
	{
		return this.objects.ContainsValue(value);
	}

	public bool ContainsKey(string key)
	{
		return this.objects.ContainsKey(key);
	}

	public string[] ContainsExtractKeys(GameObject value)
	{
		List<string> list = new List<string>();
		string[] array = new string[this.objects.Keys.Count];
		this.objects.Keys.CopyTo(array, 0);
		for (int i = 0; i < array.Length; i++)
		{
			if (this.objects[array[i]] == value)
			{
				list.Add(array[i]);
			}
		}
		return list.ToArray();
	}

	public GameObject GetObject(string key)
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.objects)
		{
			if (keyValuePair.Key.Equals(key))
			{
				return keyValuePair.Value;
			}
		}
		global::Debug.LogWarning(key + "に対応するオブジェクトが存在しません.");
		return null;
	}

	public GameObject GetObject(string masterKey, string subKey)
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.objects)
		{
			if (keyValuePair.Key.Equals(masterKey + "#" + subKey))
			{
				return keyValuePair.Value;
			}
		}
		global::Debug.LogWarning(masterKey + "#" + subKey + "に対応するオブジェクトが存在しません.");
		return null;
	}

	public void Add(string key, GameObject g)
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.objects)
		{
			if (keyValuePair.Key.Equals(key))
			{
				global::Debug.LogWarning(key + "は既に使用されています.");
				return;
			}
		}
		if (g == null)
		{
			global::Debug.LogWarning("追加されたオブジェクトがnullです. (" + key + ")");
		}
		this.objects.Add(key, g);
	}

	public void Add(string masterKey, string subKey, GameObject g)
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.objects)
		{
			if (keyValuePair.Key.Equals(masterKey + "#" + subKey))
			{
				global::Debug.LogWarning(masterKey + "#" + subKey + "は既に使用されています.");
				return;
			}
		}
		if (g == null)
		{
			global::Debug.LogWarning(string.Concat(new string[]
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

	public void Replace(string key, GameObject g)
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.objects)
		{
			if (keyValuePair.Key.Equals(key))
			{
				this.objects.Remove(key);
			}
		}
		this.objects.Add(key, g);
	}

	public void Replace(string masterKey, string subKey, GameObject g)
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
			global::Debug.LogWarning(key + "は存在しません.");
			return;
		}
		this.objects.Remove(key);
	}

	public void Remove(string masterKey, string subKey)
	{
		if (!this.objects.ContainsKey(masterKey + "#" + subKey))
		{
			global::Debug.LogWarning(masterKey + "#" + subKey + "は存在しません.");
			return;
		}
		this.objects.Remove(masterKey + "#" + subKey);
	}

	public Dictionary<string, GameObject> Objects
	{
		get
		{
			return this.objects;
		}
	}
}
