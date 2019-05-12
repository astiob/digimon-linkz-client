using System;
using UnityEngine;

namespace MonsterIcon
{
	public static class MonsterIconTransform
	{
		public static void AttachParts(Transform iconRootTransform, GameObject go)
		{
			Transform transform = go.transform;
			transform.parent = iconRootTransform;
			transform.localScale = Vector3.one;
			transform.localPosition = Vector3.zero;
		}

		public static void AttachParts(Transform iconRootTransform, GameObject go, Vector2 goPosition)
		{
			Transform transform = go.transform;
			transform.parent = iconRootTransform;
			transform.localScale = Vector3.one;
			Vector3 localPosition = transform.localPosition;
			localPosition.x = goPosition.x;
			localPosition.y = goPosition.y;
			transform.localPosition = localPosition;
		}

		public static void AttachParts(Transform iconRootTransform, GameObject go, Vector2 goPosition, Quaternion q)
		{
			Transform transform = go.transform;
			transform.parent = iconRootTransform;
			transform.localScale = Vector3.one;
			transform.localRotation = q;
			Vector3 localPosition = transform.localPosition;
			localPosition.x = goPosition.x;
			localPosition.y = goPosition.y;
			transform.localPosition = localPosition;
		}

		public static void SetSize(Transform iconRootTransform, int iconSize)
		{
			if (0 >= iconSize || iconSize == 130)
			{
				return;
			}
			Vector3 localScale = iconRootTransform.localScale;
			localScale.x *= (float)iconSize / 130f;
			localScale.y *= (float)iconSize / 130f;
			iconRootTransform.localScale = localScale;
		}
	}
}
