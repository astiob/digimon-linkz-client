using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetDataCacheBuffer
{
	protected int LOAD_AND_CACHE_OBJ_MAX = 40;

	private List<AssetDataCacheBuffer.CacheObjData> objList;

	private int cacheIdx;

	private Dictionary<string, int> objListDic;

	private string path_bk = string.Empty;

	public AssetDataCacheBuffer(int cacheSize)
	{
		this.LOAD_AND_CACHE_OBJ_MAX = cacheSize;
	}

	public UnityEngine.Object LoadAndCacheObj(string path, Action<UnityEngine.Object> actCB = null)
	{
		if (this.objList == null)
		{
			this.objList = new List<AssetDataCacheBuffer.CacheObjData>();
			this.objListDic = new Dictionary<string, int>();
			for (int i = 0; i < this.LOAD_AND_CACHE_OBJ_MAX; i++)
			{
				AssetDataCacheBuffer.CacheObjData cacheObjData = new AssetDataCacheBuffer.CacheObjData();
				cacheObjData.path = string.Empty;
				cacheObjData.obj = null;
				this.objList.Add(cacheObjData);
			}
			this.cacheIdx = 0;
		}
		UnityEngine.Object @object = this.FindCacheObj(path);
		if (@object == null)
		{
			if (actCB != null)
			{
				AppCoroutine.Start(this.DoLoadObjectASync(path, actCB), true);
				return null;
			}
			@object = AssetDataMng.Instance().LoadObject(path, null, true);
			this.AddCacheObj(path, @object);
			return @object;
		}
		else
		{
			if (actCB != null)
			{
				actCB(@object);
				return null;
			}
			return @object;
		}
	}

	private IEnumerator DoLoadObjectASync(string path, Action<UnityEngine.Object> actCB)
	{
		while (this.path_bk != string.Empty)
		{
			yield return null;
		}
		this.path_bk = path;
		UnityEngine.Object obj = this.FindCacheObj(path);
		if (obj == null)
		{
			AssetDataMng.Instance().LoadObjectASync(path, delegate(UnityEngine.Object _obj)
			{
				this.AddCacheObj(path, _obj);
				actCB(_obj);
				this.path_bk = string.Empty;
			});
		}
		else
		{
			actCB(obj);
			this.path_bk = string.Empty;
		}
		yield break;
	}

	public UnityEngine.Object FindCacheObj(string path)
	{
		if (this.objListDic.ContainsKey(path))
		{
			int index = this.objListDic[path];
			return this.objList[index].obj;
		}
		return null;
	}

	public void AddCacheObj(string path, UnityEngine.Object obj)
	{
		if (this.objList[this.cacheIdx].path != string.Empty)
		{
			if (this.objListDic.ContainsKey(this.objList[this.cacheIdx].path))
			{
				this.objListDic.Remove(this.objList[this.cacheIdx].path);
			}
			else
			{
				global::Debug.LogError("============================= 深刻なエラー");
			}
		}
		this.objList[this.cacheIdx].path = path;
		this.objList[this.cacheIdx].obj = obj;
		this.objListDic.Add(path, this.cacheIdx);
		this.cacheIdx++;
		if (this.cacheIdx >= this.LOAD_AND_CACHE_OBJ_MAX)
		{
			this.cacheIdx = 0;
		}
	}

	private bool DbgExist(string path)
	{
		for (int i = 0; i < this.objList.Count; i++)
		{
			if (this.objList[i].path == path)
			{
				return true;
			}
		}
		return false;
	}

	public class CacheObjData
	{
		public string path;

		public UnityEngine.Object obj;
	}
}
