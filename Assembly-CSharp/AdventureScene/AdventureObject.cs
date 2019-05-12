using System;
using UnityEngine;

namespace AdventureScene
{
	public static class AdventureObject
	{
		public static Transform FindLocator(Transform t, string locatorName)
		{
			if (null != t)
			{
				if (t.name == locatorName)
				{
					return t;
				}
				for (int i = 0; i < t.childCount; i++)
				{
					Transform transform = AdventureObject.FindLocator(t.GetChild(i), locatorName);
					if (null != transform)
					{
						return transform;
					}
				}
			}
			return null;
		}

		public static bool SetLocator(Transform objectTransform, Transform locatorTransform, string locatorName, bool isFollowingFlag)
		{
			bool result = false;
			Transform transform = AdventureObject.FindLocator(locatorTransform, locatorName);
			if (null != transform)
			{
				if (isFollowingFlag)
				{
					objectTransform.parent = transform;
				}
				else
				{
					objectTransform.position = transform.position;
					objectTransform.rotation = transform.rotation;
				}
				objectTransform.localPosition = Vector3.zero;
				objectTransform.localRotation = Quaternion.identity;
				objectTransform.localScale = Vector3.one;
				result = true;
			}
			else
			{
				global::Debug.Log("ロケータ設定失敗");
			}
			return result;
		}
	}
}
