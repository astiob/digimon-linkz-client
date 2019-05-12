using System;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

public class BattleEffectManager
{
	private static BattleEffectManager _instance;

	private static Dictionary<string, EffectPool> _pool;

	private static Camera _camera;

	private BattleEffectManager()
	{
		BattleEffectManager._pool = new Dictionary<string, EffectPool>();
	}

	public static BattleEffectManager Instance
	{
		get
		{
			BattleEffectManager result;
			if ((result = BattleEffectManager._instance) == null)
			{
				result = (BattleEffectManager._instance = new BattleEffectManager());
			}
			return result;
		}
	}

	public void SetCamera(Camera cam)
	{
		BattleEffectManager._camera = cam;
	}

	public void Destroy()
	{
		this.DisposeAllPool();
		BattleEffectManager._instance = null;
	}

	public bool SetPool(string key, Transform parent, GameObject prefab)
	{
		if (string.IsNullOrEmpty(key))
		{
			return false;
		}
		EffectParamsGeneric component = prefab.GetComponent<EffectParamsGeneric>();
		if (component != null)
		{
			BattleEffectManager._pool.AddOrReplace(key, new EffectPool(key, parent, component));
		}
		return true;
	}

	public void PreloadAsync(string key, int preloadCount, int threshold)
	{
		if (string.IsNullOrEmpty(key))
		{
			return;
		}
		if (!BattleEffectManager._pool.ContainsKey(key))
		{
			return;
		}
		BattleEffectManager._pool[key].PreloadAsync(preloadCount, threshold).Subscribe<Unit>();
	}

	private EffectPool GetPool(string key)
	{
		if (string.IsNullOrEmpty(key))
		{
			global::Debug.LogWarning(string.Format("引数不正", key));
		}
		return (!BattleEffectManager._pool.ContainsKey(key)) ? new EffectPool() : BattleEffectManager._pool[key];
	}

	public EffectParamsGeneric GetEffect(string key)
	{
		EffectPool pool = this.GetPool(key);
		EffectParamsGeneric effectParamsGeneric = pool.Rent();
		effectParamsGeneric.InitializeFast(BattleEffectManager._camera);
		return effectParamsGeneric;
	}

	public void ReturnEffect(EffectParamsGeneric effect)
	{
		if (effect == null)
		{
			global::Debug.LogWarning("引数不正 [ReturnEffect]");
			return;
		}
		this.GetPool(effect.PoolKey).Return(effect);
	}

	public void ReturnEffect(EffectParamsGeneric[] effects)
	{
		foreach (EffectParamsGeneric effectParamsGeneric in effects)
		{
			if (effectParamsGeneric != null)
			{
				this.ReturnEffect(effectParamsGeneric);
			}
		}
	}

	public void ClearPool(string key, bool callOnBeforeRent = false)
	{
		this.GetPool(key).Clear(false);
	}

	public void DisposePool(string key, bool callOnBeforeRent = false)
	{
		this.GetPool(key).Dispose();
	}

	public void ClearAllPool()
	{
		foreach (KeyValuePair<string, EffectPool> keyValuePair in BattleEffectManager._pool)
		{
			this.ClearPool(keyValuePair.Key, false);
		}
	}

	public void DisposeAllPool()
	{
		foreach (KeyValuePair<string, EffectPool> keyValuePair in BattleEffectManager._pool)
		{
			this.DisposePool(keyValuePair.Key, false);
		}
	}

	public int Count(string key)
	{
		if (string.IsNullOrEmpty(key))
		{
			global::Debug.LogWarning("引数不正.");
			return -1;
		}
		return this.GetPool(key).Count;
	}

	[Conditional("UNITY_EDITOR")]
	public void DrawLog()
	{
	}
}
