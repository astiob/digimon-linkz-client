using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleStateMachineInternal
{
	public class BattleObjectPooler
	{
		private static Transform cacheRoot = null;

		private static BattleObjectPoolerManager manager = null;

		private static Dictionary<string, CameraParams> cameraParamsCache = new Dictionary<string, CameraParams>();

		private static Dictionary<string, List<HitEffectParams>> hitEffectParamsCache = new Dictionary<string, List<HitEffectParams>>();

		private static Dictionary<string, List<AlwaysEffectParams>> alwaysEffectParamsCache = new Dictionary<string, List<AlwaysEffectParams>>();

		private static bool onInitialized = false;

		private static bool _isCheckEnable = false;

		public static void CallInitialize()
		{
			if (!BattleObjectPooler.onInitialized)
			{
				BattleObjectPooler.Initialize();
				BattleObjectPooler.onInitialized = true;
			}
			BattleObjectPooler.CheckAndRemoveNull();
		}

		[RuntimeInitializeOnLoadMethod]
		private static void Initialize()
		{
			if (BattleObjectPooler.cacheRoot != null)
			{
				return;
			}
			BattleObjectPooler.cacheRoot = new GameObject("BattleObjectPooler").transform;
			BattleObjectPooler.manager = BattleObjectPooler.cacheRoot.gameObject.AddComponent<BattleObjectPoolerManager>();
			BattleObjectPooler.manager.CheckEnable = BattleObjectPooler.isCheckEnable;
			UnityEngine.Object.DontDestroyOnLoad(BattleObjectPooler.cacheRoot.gameObject);
		}

		public static bool isCheckEnable
		{
			get
			{
				return BattleObjectPooler._isCheckEnable;
			}
			set
			{
				BattleObjectPooler._isCheckEnable = value;
				BattleObjectPooler.manager.CheckEnable = BattleObjectPooler.isCheckEnable;
			}
		}

		public static void AddHitEffectParams(string id, HitEffectParams hitEffect)
		{
			if (!BattleObjectPooler.hitEffectParamsCache.ContainsKey(id))
			{
				BattleObjectPooler.hitEffectParamsCache.Add(id, new List<HitEffectParams>());
			}
			BattleObjectPooler.hitEffectParamsCache[id].Add(hitEffect);
		}

		public static void AddAlwaysEffectParams(string id, AlwaysEffectParams alwaysEffect)
		{
			if (!BattleObjectPooler.alwaysEffectParamsCache.ContainsKey(id))
			{
				BattleObjectPooler.alwaysEffectParamsCache.Add(id, new List<AlwaysEffectParams>());
			}
			BattleObjectPooler.alwaysEffectParamsCache[id].Add(alwaysEffect);
		}

		public static bool TryGetHitEffectParams(string id, int index, out HitEffectParams hitEffect)
		{
			if (BattleObjectPooler.hitEffectParamsCache.ContainsKey(id) && BattleObjectPooler.hitEffectParamsCache[id].Count > index)
			{
				hitEffect = BattleObjectPooler.hitEffectParamsCache[id][index];
				return true;
			}
			hitEffect = null;
			return false;
		}

		public static bool TryGetAlwaysEffectParams(string id, int index, out AlwaysEffectParams alwaysEffect)
		{
			if (BattleObjectPooler.alwaysEffectParamsCache.ContainsKey(id) && BattleObjectPooler.alwaysEffectParamsCache[id].Count > index)
			{
				alwaysEffect = BattleObjectPooler.alwaysEffectParamsCache[id][index];
				return true;
			}
			alwaysEffect = null;
			return false;
		}

		private static void CheckAndRemoveNull()
		{
			List<string> list = new List<string>();
			string[] array = new string[BattleObjectPooler.cameraParamsCache.Keys.Count];
			if (BattleObjectPooler.cameraParamsCache.ContainsValue(null))
			{
				array = new string[BattleObjectPooler.cameraParamsCache.Keys.Count];
				BattleObjectPooler.cameraParamsCache.Keys.CopyTo(array, 0);
				list.AddRange(array);
				foreach (KeyValuePair<string, CameraParams> keyValuePair in BattleObjectPooler.cameraParamsCache)
				{
					if (keyValuePair.Value != null)
					{
						list.Remove(keyValuePair.Key);
					}
				}
				foreach (string key in array)
				{
					BattleObjectPooler.cameraParamsCache.Remove(key);
				}
			}
			list.Clear();
			array = new string[BattleObjectPooler.hitEffectParamsCache.Keys.Count];
			BattleObjectPooler.hitEffectParamsCache.Keys.CopyTo(array, 0);
			foreach (KeyValuePair<string, List<HitEffectParams>> keyValuePair2 in BattleObjectPooler.hitEffectParamsCache)
			{
				if (keyValuePair2.Value.Count <= 0)
				{
					list.Add(keyValuePair2.Key);
				}
				else
				{
					keyValuePair2.Value.Remove(null);
				}
			}
			foreach (string key2 in array)
			{
				BattleObjectPooler.hitEffectParamsCache.Remove(key2);
			}
			list.Clear();
			array = new string[BattleObjectPooler.alwaysEffectParamsCache.Keys.Count];
			BattleObjectPooler.alwaysEffectParamsCache.Keys.CopyTo(array, 0);
			foreach (KeyValuePair<string, List<AlwaysEffectParams>> keyValuePair3 in BattleObjectPooler.alwaysEffectParamsCache)
			{
				if (keyValuePair3.Value.Count <= 0)
				{
					list.Add(keyValuePair3.Key);
				}
				else
				{
					keyValuePair3.Value.Remove(null);
				}
			}
			foreach (string key3 in array)
			{
				BattleObjectPooler.alwaysEffectParamsCache.Remove(key3);
			}
		}

		public static void SetAllDeactive()
		{
			BattleObjectPooler.CheckAndRemoveNull();
			foreach (KeyValuePair<string, CameraParams> keyValuePair in BattleObjectPooler.cameraParamsCache)
			{
				keyValuePair.Value.transform.SetParent(BattleObjectPooler.cacheRoot);
				keyValuePair.Value.gameObject.SetActive(false);
			}
			foreach (KeyValuePair<string, List<HitEffectParams>> keyValuePair2 in BattleObjectPooler.hitEffectParamsCache)
			{
				foreach (HitEffectParams hitEffectParams in keyValuePair2.Value)
				{
					hitEffectParams.transform.SetParent(BattleObjectPooler.cacheRoot);
					hitEffectParams.gameObject.SetActive(false);
				}
			}
			foreach (KeyValuePair<string, List<AlwaysEffectParams>> keyValuePair3 in BattleObjectPooler.alwaysEffectParamsCache)
			{
				foreach (AlwaysEffectParams alwaysEffectParams in keyValuePair3.Value)
				{
					alwaysEffectParams.transform.SetParent(BattleObjectPooler.cacheRoot);
					alwaysEffectParams.gameObject.SetActive(false);
				}
			}
		}

		public static bool ContainHitEffectParams(HitEffectParams c)
		{
			foreach (KeyValuePair<string, List<HitEffectParams>> keyValuePair in BattleObjectPooler.hitEffectParamsCache)
			{
				if (keyValuePair.Value.Contains(c))
				{
					return true;
				}
			}
			return false;
		}

		public static bool ContainAlwaysEffectParams(AlwaysEffectParams c)
		{
			foreach (KeyValuePair<string, List<AlwaysEffectParams>> keyValuePair in BattleObjectPooler.alwaysEffectParamsCache)
			{
				if (keyValuePair.Value.Contains(c))
				{
					return true;
				}
			}
			return false;
		}

		public static void AllUnloadAssets()
		{
			BattleObjectPooler.CheckAndRemoveNull();
			foreach (KeyValuePair<string, CameraParams> keyValuePair in BattleObjectPooler.cameraParamsCache)
			{
				if (keyValuePair.Value.gameObject != null)
				{
					UnityEngine.Object.Destroy(keyValuePair.Value.gameObject);
				}
			}
			BattleObjectPooler.cameraParamsCache.Clear();
			foreach (KeyValuePair<string, List<HitEffectParams>> keyValuePair2 in BattleObjectPooler.hitEffectParamsCache)
			{
				foreach (HitEffectParams hitEffectParams in keyValuePair2.Value)
				{
					if (hitEffectParams.gameObject != null)
					{
						UnityEngine.Object.Destroy(hitEffectParams.gameObject);
					}
				}
			}
			BattleObjectPooler.hitEffectParamsCache.Clear();
			foreach (KeyValuePair<string, List<AlwaysEffectParams>> keyValuePair3 in BattleObjectPooler.alwaysEffectParamsCache)
			{
				foreach (AlwaysEffectParams alwaysEffectParams in keyValuePair3.Value)
				{
					if (alwaysEffectParams.gameObject != null)
					{
						UnityEngine.Object.Destroy(alwaysEffectParams.gameObject);
					}
				}
			}
			BattleObjectPooler.alwaysEffectParamsCache.Clear();
			Resources.UnloadUnusedAssets();
			GC.Collect();
		}
	}
}
