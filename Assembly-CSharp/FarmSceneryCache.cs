using FarmData;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class FarmSceneryCache : MonoBehaviour
{
	private static FarmScenery farmSceneryReference;

	private static FarmSceneryCache cache;

	private bool existCached;

	[CompilerGenerated]
	private static Action <>f__mg$cache0;

	public static void SetCacheAction(FarmScenery scenery)
	{
		if (null == FarmSceneryCache.farmSceneryReference)
		{
			FarmSceneryCache.farmSceneryReference = scenery;
			if (FarmSceneryCache.<>f__mg$cache0 == null)
			{
				FarmSceneryCache.<>f__mg$cache0 = new Action(FarmSceneryCache.ExecutionCache);
			}
			GUIMain.SetAction_FadeBlackLoadScene(FarmSceneryCache.<>f__mg$cache0);
		}
	}

	public static void ExecutionCache()
	{
		if (null != FarmSceneryCache.farmSceneryReference)
		{
			GameObject gameObject = new GameObject("FarmSceneryCache");
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.parent = null;
			gameObject.name = "FarmSceneryCache";
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			FarmRoot instance = FarmRoot.Instance;
			if (null != instance)
			{
				instance.ClearSettingFarmObject();
			}
			FarmSceneryCache.cache = gameObject.AddComponent<FarmSceneryCache>();
			FarmSceneryCache.cache.Execution(FarmSceneryCache.farmSceneryReference);
			FarmSceneryCache.farmSceneryReference = null;
		}
	}

	public static void ClearCache()
	{
		FarmSceneryCache.farmSceneryReference = null;
		if (null != FarmSceneryCache.cache)
		{
			UnityEngine.Object.Destroy(FarmSceneryCache.cache.gameObject);
			FarmSceneryCache.cache = null;
		}
	}

	public static FarmSceneryCache GetCache()
	{
		return FarmSceneryCache.cache;
	}

	private void Execution(FarmScenery scenery)
	{
		if (!this.existCached)
		{
			this.existCached = true;
			List<GameObject> list = new List<GameObject>();
			List<GameObject> decorationList = new List<GameObject>();
			Transform transform = scenery.transform;
			this.GetCacheFacilityObject(transform, list, decorationList);
			Transform transform2 = base.transform;
			for (int i = 0; i < list.Count; i++)
			{
				list[i].transform.parent = transform2;
			}
			base.gameObject.SetActive(false);
		}
	}

	private void GetCacheFacilityObject(Transform sceneryTransform, List<GameObject> facilityList, List<GameObject> decorationList)
	{
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		List<GameObject> list3 = new List<GameObject>();
		for (int i = 0; i < sceneryTransform.childCount; i++)
		{
			Transform child = sceneryTransform.GetChild(i);
			if (!(null == child))
			{
				FarmObject component = child.GetComponent<FarmObject>();
				if (null == component)
				{
					list3.Add(child.gameObject);
				}
				else
				{
					FacilityM facilityMaster = FarmDataManager.GetFacilityMaster(component.facilityID);
					if (facilityMaster != null)
					{
						if (facilityMaster.IsFacility())
						{
							if (!list.Contains(component.facilityID) && component.facilityID != 1)
							{
								list.Add(component.facilityID);
								facilityList.Add(component.gameObject);
							}
							else
							{
								list3.Add(component.gameObject);
							}
						}
						else if (!list2.Contains(component.facilityID))
						{
							list2.Add(component.facilityID);
							decorationList.Add(component.gameObject);
						}
						else
						{
							list3.Add(component.gameObject);
						}
					}
				}
			}
		}
		for (int j = 0; j < list3.Count; j++)
		{
			UnityEngine.Object.Destroy(list3[j]);
		}
	}

	public FarmObject GetCacheFarmObject(int facilityId)
	{
		FarmObject result = null;
		Transform transform = base.transform;
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (null != child)
			{
				FarmObject component = child.GetComponent<FarmObject>();
				if (null != component && component.facilityID == facilityId)
				{
					result = component;
					break;
				}
			}
		}
		return result;
	}
}
