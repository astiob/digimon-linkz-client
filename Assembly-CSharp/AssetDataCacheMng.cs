using System;
using System.Collections.Generic;
using UnityEngine;

public class AssetDataCacheMng : MonoBehaviour
{
	private int LOAD_ASYNC_MAX = 2;

	private static int load_async_ct;

	private static int load_async_not_op_ct;

	private Dictionary<string, AssetDataCacheMng.CacheInfo> cacheInfoTable;

	private static AssetDataCacheMng instance;

	protected virtual void Awake()
	{
		this.cacheInfoTable = new Dictionary<string, AssetDataCacheMng.CacheInfo>();
		AssetDataCacheMng.load_async_ct = 0;
		AssetDataCacheMng.load_async_not_op_ct = 0;
		AssetDataCacheMng.instance = this;
	}

	protected virtual void Update()
	{
		if (AssetDataCacheMng.load_async_not_op_ct > 0)
		{
			foreach (string text in this.cacheInfoTable.Keys)
			{
				if (this.cacheInfoTable[text].state == 0)
				{
					if (AssetDataCacheMng.load_async_ct >= this.LOAD_ASYNC_MAX)
					{
						break;
					}
					AssetDataCacheMng.load_async_ct++;
					AssetDataCacheMng.load_async_not_op_ct--;
					this.cacheInfoTable[text].state = 1;
					AssetDataMng.Instance().LoadObjectASync(text, new Action<UnityEngine.Object>(this.cacheInfoTable[text].cachedCB));
				}
			}
		}
	}

	protected virtual void OnDestroy()
	{
		AssetDataCacheMng.instance = null;
	}

	public static AssetDataCacheMng Instance()
	{
		return AssetDataCacheMng.instance;
	}

	public Dictionary<string, AssetDataCacheMng.CacheInfo> GetCacheInfoDic()
	{
		return this.cacheInfoTable;
	}

	public void RegisterCache(string path, AssetDataCacheMng.CACHE_TYPE type = AssetDataCacheMng.CACHE_TYPE.NORMAL, bool immediate = false)
	{
		if (this.cacheInfoTable.ContainsKey(path))
		{
			global::Debug.LogError("===================================####### AssetDataCacheMng:RegisterCache " + path + " -> 既に登録済み");
		}
		else
		{
			AssetDataCacheMng.CacheInfo cacheInfo = new AssetDataCacheMng.CacheInfo();
			cacheInfo.type = type;
			if (immediate)
			{
				cacheInfo.obj = AssetDataMng.Instance().LoadObject(path, null, true);
				cacheInfo.state = 2;
			}
			else if (AssetDataCacheMng.load_async_ct < this.LOAD_ASYNC_MAX)
			{
				AssetDataCacheMng.load_async_ct++;
				cacheInfo.state = 1;
				AssetDataMng.Instance().LoadObjectASync(path, new Action<UnityEngine.Object>(cacheInfo.cachedCB));
			}
			else
			{
				cacheInfo.state = 0;
				AssetDataCacheMng.load_async_not_op_ct++;
			}
			this.cacheInfoTable.Add(path, cacheInfo);
		}
	}

	public void DeleteCache(string path, AssetDataCacheMng.CACHE_TYPE type = AssetDataCacheMng.CACHE_TYPE.NORMAL)
	{
		foreach (string text in this.cacheInfoTable.Keys)
		{
			if (text == path && this.cacheInfoTable[text].type == type)
			{
				if (this.cacheInfoTable[text].state == 2)
				{
					this.cacheInfoTable.Remove(text);
					break;
				}
				global::Debug.LogError("===================================####### AssetDataCacheMng:DeleteCache " + path + " -> 読み込み中のキャッシュを消そうとしてます、禁止事項");
			}
		}
	}

	public void RegisterCacheType(List<string> pathL, AssetDataCacheMng.CACHE_TYPE type, bool immediate = false)
	{
		foreach (string key in this.cacheInfoTable.Keys)
		{
			if (this.cacheInfoTable[key].type == type)
			{
				this.cacheInfoTable[key].flg = false;
			}
		}
		for (int i = 0; i < pathL.Count; i++)
		{
			if (this.cacheInfoTable.ContainsKey(pathL[i]))
			{
				if (this.cacheInfoTable[pathL[i]].type != type)
				{
					global::Debug.LogError("===================================####### AssetDataCacheMng:RegisterCache " + pathL[i] + " -> 論理的 不整合(type違い)");
				}
				else
				{
					this.cacheInfoTable[pathL[i]].flg = true;
				}
			}
			else
			{
				AssetDataCacheMng.CacheInfo cacheInfo = new AssetDataCacheMng.CacheInfo();
				cacheInfo.type = type;
				if (immediate)
				{
					cacheInfo.obj = AssetDataMng.Instance().LoadObject(pathL[i], null, true);
					cacheInfo.state = 2;
				}
				else if (AssetDataCacheMng.load_async_ct < this.LOAD_ASYNC_MAX)
				{
					AssetDataCacheMng.load_async_ct++;
					cacheInfo.state = 1;
					AssetDataMng.Instance().LoadObjectASync(pathL[i], new Action<UnityEngine.Object>(cacheInfo.cachedCB));
				}
				else
				{
					cacheInfo.state = 0;
					AssetDataCacheMng.load_async_not_op_ct++;
				}
				cacheInfo.flg = true;
				this.cacheInfoTable.Add(pathL[i], cacheInfo);
			}
		}
		List<string> list = new List<string>();
		foreach (string text in this.cacheInfoTable.Keys)
		{
			if (this.cacheInfoTable[text].type == type && !this.cacheInfoTable[text].flg)
			{
				list.Add(text);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			foreach (string text2 in this.cacheInfoTable.Keys)
			{
				if (text2 == list[i])
				{
					if (this.cacheInfoTable[text2].state != 2)
					{
						global::Debug.LogError("===================================####### AssetDataCacheMng:RegisterCache " + text2 + " -> 読み込み中のキャッシュを消そうとしてます、禁止事項");
					}
					else
					{
						this.cacheInfoTable.Remove(text2);
					}
					break;
				}
			}
		}
	}

	public void DeleteCacheType(AssetDataCacheMng.CACHE_TYPE type)
	{
		List<string> list = new List<string>();
		foreach (string text in this.cacheInfoTable.Keys)
		{
			if (this.cacheInfoTable[text].type == type)
			{
				list.Add(text);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			foreach (string text2 in this.cacheInfoTable.Keys)
			{
				if (text2 == list[i])
				{
					if (this.cacheInfoTable[text2].state != 2)
					{
						global::Debug.LogError("===================================####### AssetDataCacheMng:DeleteCache " + text2 + " -> 読み込み中のキャッシュを消そうとしてます、禁止事項");
					}
					else
					{
						this.cacheInfoTable.Remove(text2);
					}
					break;
				}
			}
		}
	}

	public bool IsCacheAllReady()
	{
		foreach (string key in this.cacheInfoTable.Keys)
		{
			if (this.cacheInfoTable[key].state != 2)
			{
				return false;
			}
		}
		return true;
	}

	public bool IsCacheAllReadyType(AssetDataCacheMng.CACHE_TYPE type)
	{
		foreach (string key in this.cacheInfoTable.Keys)
		{
			if (this.cacheInfoTable[key].type == type && this.cacheInfoTable[key].state != 2)
			{
				return false;
			}
		}
		return true;
	}

	public bool IsCacheExist(string path)
	{
		if (!this.cacheInfoTable.ContainsKey(path))
		{
			return false;
		}
		if (this.cacheInfoTable[path].state != 2)
		{
			global::Debug.LogError("===================================####### AssetDataCacheMng:IsCacheExist " + path + " -> 読み込み中のキャッシュです、禁止事項");
			return false;
		}
		return true;
	}

	public UnityEngine.Object GetCache(string path)
	{
		if (!this.cacheInfoTable.ContainsKey(path))
		{
			return null;
		}
		if (this.cacheInfoTable[path].state != 2)
		{
			global::Debug.LogError("===================================####### AssetDataCacheMng:GetCache " + path + " -> 読み込み中のキャッシュです、禁止事項");
			return null;
		}
		return this.cacheInfoTable[path].obj;
	}

	public enum CACHE_TYPE
	{
		INVALID = -1,
		NORMAL,
		CHARA_PARTY,
		BATTLE_COMMON
	}

	public class CacheInfo
	{
		public AssetDataCacheMng.CACHE_TYPE type;

		public int state;

		public UnityEngine.Object obj;

		public bool flg;

		public void cachedCB(UnityEngine.Object _obj)
		{
			this.state = 2;
			this.obj = _obj;
			AssetDataCacheMng.load_async_ct--;
		}
	}
}
