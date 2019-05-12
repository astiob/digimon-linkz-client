using LitJson;
using Save;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextureManager : MonoBehaviour
{
	private static TextureManager _instance;

	private string cachePath = Application.persistentDataPath + "/TextureCache.txt";

	private List<TextureManager.TextureInfo> infoList = new List<TextureManager.TextureInfo>();

	private PersistentFile persistentFile = new PersistentFile(false);

	private TextureManager.SaveData saveData = new TextureManager.SaveData();

	public static TextureManager instance
	{
		get
		{
			if (TextureManager._instance == null)
			{
				GameObject gameObject = new GameObject("TextureManager");
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				TextureManager._instance = gameObject.AddComponent<TextureManager>();
				TextureManager._instance.StartCoroutine(TextureManager._instance.persistentFile.Read(TextureManager._instance.cachePath, delegate(bool isSuccess, string data)
				{
					TextureManager._instance.isLoadSaveData = true;
					if (isSuccess)
					{
						TextureManager._instance.saveData = JsonMapper.ToObject<TextureManager.SaveData>(data);
					}
				}));
			}
			return TextureManager._instance;
		}
	}

	public bool isLoadSaveData { get; private set; }

	public Coroutine Load(string path, Action<Texture2D> callback, float timeoutSeconds = 30f, bool isCache = true)
	{
		TextureManager.TextureInfo textureInfo = null;
		for (int i = 0; i < this.infoList.Count; i++)
		{
			if (this.infoList[i].path == path)
			{
				textureInfo = this.infoList[i];
				break;
			}
		}
		bool flag = textureInfo == null;
		if (textureInfo != null)
		{
			if (textureInfo.coroutine != null)
			{
				base.StopCoroutine(textureInfo.coroutine);
				this.infoList.Remove(textureInfo);
				flag = true;
			}
			else if (callback != null)
			{
				callback(textureInfo.texture2D);
			}
		}
		if (flag)
		{
			TextureManager.TextureInfo textureInfo2 = this.LoadFile(path, callback, timeoutSeconds, isCache);
			if (isCache)
			{
				this.infoList.Add(textureInfo2);
			}
			return textureInfo2.coroutine;
		}
		return null;
	}

	private TextureManager.TextureInfo LoadFile(string path, Action<Texture2D> callback, float timeoutSeconds = 30f, bool isCache = true)
	{
		TextureManager.TextureInfo info = new TextureManager.TextureInfo();
		info.path = path;
		TextureManager.SaveData.CacheInfo cacheInfo = this.CreateCacheInfo(path);
		Action<Texture2D> callback2 = delegate(Texture2D loadTex)
		{
			info.texture2D = loadTex;
			info.coroutine = null;
			if (callback != null)
			{
				callback(info.texture2D);
			}
		};
		if (this.Containts(cacheInfo))
		{
			info.coroutine = this.LoadPersistentFile(path, cacheInfo, callback2);
		}
		else
		{
			info.coroutine = this.LoadWWWHelper(path, cacheInfo, callback2, timeoutSeconds, isCache);
		}
		return info;
	}

	private Coroutine LoadPersistentFile(string path, TextureManager.SaveData.CacheInfo cacheInfo, Action<Texture2D> callback)
	{
		return base.StartCoroutine(this.persistentFile.Read(cacheInfo.path, delegate(bool isSuccess, string data)
		{
			Texture2D texture2D = null;
			if (isSuccess)
			{
				byte[] data2 = Convert.FromBase64String(data);
				texture2D = new Texture2D(2, 2, TextureFormat.ARGB32, false);
				texture2D.LoadImage(data2);
			}
			if (callback != null)
			{
				callback(texture2D);
			}
		}));
	}

	private Coroutine LoadWWWHelper(string path, TextureManager.SaveData.CacheInfo cacheInfo, Action<Texture2D> callback, float timeoutSeconds, bool isCache)
	{
		WWWHelper wwwhelper = new WWWHelper(path, null, null, timeoutSeconds);
		return base.StartCoroutine(wwwhelper.StartRequest(delegate(Texture2D loadTex, string errorString, WWWHelper.TimeOut isTimeout)
		{
			if (!string.IsNullOrEmpty(errorString))
			{
				global::Debug.Log(errorString);
				if (callback != null)
				{
					callback(null);
				}
				return;
			}
			if (isTimeout == WWWHelper.TimeOut.YES)
			{
				global::Debug.Log("www time out\nURL : " + path);
				if (callback != null)
				{
					callback(null);
				}
				return;
			}
			if (loadTex != null && isCache)
			{
				string jsonText = Convert.ToBase64String(loadTex.EncodeToPNG());
				this.StartCoroutine(this.persistentFile.Write(cacheInfo.path, jsonText, delegate(bool isSuccess)
				{
				}));
				bool flag = true;
				bool flag2 = false;
				foreach (TextureManager.SaveData.CacheInfo cacheInfo2 in this.saveData.cacheInfoList)
				{
					if (cacheInfo2.path == cacheInfo.path)
					{
						flag = false;
						if (cacheInfo2.version < cacheInfo.version)
						{
							cacheInfo2.version = cacheInfo.version;
							flag2 = true;
						}
						break;
					}
				}
				if (flag)
				{
					this.saveData.cacheInfoList.Add(cacheInfo);
					flag2 = true;
				}
				if (flag2)
				{
					while (this.saveData.cacheInfoList.Count > ConstValue.CACHE_TEXTURE_COUNT)
					{
						this.Delete(this.saveData.cacheInfoList[0].path);
						this.saveData.cacheInfoList.RemoveAt(0);
					}
					string jsonText2 = JsonMapper.ToJson(this.saveData);
					this.StartCoroutine(this.persistentFile.Write(this.cachePath, jsonText2, null));
				}
			}
			if (callback != null)
			{
				callback(loadTex);
			}
		}));
	}

	private TextureManager.SaveData.CacheInfo CreateCacheInfo(string path)
	{
		TextureManager.SaveData.CacheInfo cacheInfo = new TextureManager.SaveData.CacheInfo();
		string[] array = path.Split(new string[]
		{
			"?v="
		}, StringSplitOptions.RemoveEmptyEntries);
		cacheInfo.version = ((array.Length <= 1) ? 0 : int.Parse(array[1]));
		array = array[0].Split(new char[]
		{
			'/'
		}, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 2; i < array.Length; i++)
		{
			TextureManager.SaveData.CacheInfo cacheInfo2 = cacheInfo;
			cacheInfo2.path += array[i];
		}
		cacheInfo.path = Application.persistentDataPath + "/" + cacheInfo.path;
		return cacheInfo;
	}

	private bool Containts(string path)
	{
		return File.Exists(path);
	}

	private bool Containts(TextureManager.SaveData.CacheInfo cacheInfo)
	{
		foreach (TextureManager.SaveData.CacheInfo cacheInfo2 in this.saveData.cacheInfoList)
		{
			if (cacheInfo2.path == cacheInfo.path && cacheInfo2.version == cacheInfo.version)
			{
				return File.Exists(cacheInfo2.path);
			}
		}
		return false;
	}

	private void Delete(string path)
	{
		if (this.Containts(path))
		{
			File.Delete(path);
		}
	}

	private class TextureInfo
	{
		public string path = string.Empty;

		public Texture2D texture2D;

		public Coroutine coroutine;
	}

	private class SaveData
	{
		public List<TextureManager.SaveData.CacheInfo> cacheInfoList = new List<TextureManager.SaveData.CacheInfo>();

		public class CacheInfo
		{
			public string path = string.Empty;

			public int version;
		}
	}
}
