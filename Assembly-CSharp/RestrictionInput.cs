using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class RestrictionInput
{
	private static List<GameObject> displayObjects = new List<GameObject>();

	public static bool isDisableBackKeySetting;

	public static void StartLoad(Loading.LoadingType loadingType, bool isMask)
	{
		if (loadingType == Loading.LoadingType.LARGE)
		{
			if (isMask)
			{
				RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON);
			}
			else
			{
				RestrictionInput.StartLoad(RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF);
			}
		}
		else if (isMask)
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON);
		}
		else
		{
			RestrictionInput.StartLoad(RestrictionInput.LoadType.SMALL_IMAGE_MASK_OFF);
		}
	}

	public static void StartLoad(RestrictionInput.LoadType loadType)
	{
		GUIMain.BarrierON(null);
		if (!RestrictionInput.isDisableBackKeySetting)
		{
			GUIManager.ExtBackKeyReady = false;
		}
		switch (loadType)
		{
		case RestrictionInput.LoadType.LARGE_IMAGE_MASK_ON:
			Loading.Display(Loading.LoadingType.LARGE, true);
			break;
		case RestrictionInput.LoadType.LARGE_IMAGE_MASK_OFF:
			Loading.Display(Loading.LoadingType.LARGE, false);
			break;
		case RestrictionInput.LoadType.SMALL_IMAGE_MASK_ON:
			Loading.Display(Loading.LoadingType.SMALL, true);
			break;
		case RestrictionInput.LoadType.SMALL_IMAGE_MASK_OFF:
			Loading.Display(Loading.LoadingType.SMALL, false);
			break;
		}
	}

	public static void SetDisplayObject(GameObject go)
	{
		if (RestrictionInput.displayObjects != null && null != go)
		{
			RestrictionInput.displayObjects.Add(go);
		}
	}

	public static void DeleteDisplayObject()
	{
		if (0 < RestrictionInput.displayObjects.Count)
		{
			for (int i = 0; i < RestrictionInput.displayObjects.Count; i++)
			{
				if (null != RestrictionInput.displayObjects[i])
				{
					UnityEngine.Object.Destroy(RestrictionInput.displayObjects[i]);
				}
			}
			RestrictionInput.displayObjects.Clear();
		}
	}

	public static void EndLoad()
	{
		if (Loading.IsShow())
		{
			Loading.Invisible();
		}
		if (!RestrictionInput.isDisableBackKeySetting)
		{
			GUIManager.ExtBackKeyReady = true;
		}
		GUIMain.BarrierOFF();
	}

	public static void SuspensionLoad()
	{
		Loading.Invisible();
		GUIManager.ExtBackKeyReady = true;
	}

	public static void ResumeLoad()
	{
		GUIManager.ExtBackKeyReady = false;
		Loading.ResumeDisplay();
	}

	public enum LoadType
	{
		LARGE_IMAGE_MASK_ON,
		LARGE_IMAGE_MASK_OFF,
		SMALL_IMAGE_MASK_ON,
		SMALL_IMAGE_MASK_OFF
	}
}
