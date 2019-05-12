using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityExtension
{
	public static class GameObjectExtension
	{
		public static string GetObjectPath(Transform parentObject, Transform childObject)
		{
			if (parentObject.root.GetInstanceID() != childObject.root.GetInstanceID())
			{
				return null;
			}
			List<string> list = new List<string>();
			Transform transform = childObject;
			do
			{
				list.Insert(0, transform.name);
				transform = transform.parent;
				if (transform == null)
				{
					break;
				}
				if (transform == parentObject)
				{
					break;
				}
			}
			while (!(childObject.root == transform));
			list.Insert(0, parentObject.name);
			return StringExtension.CreatePath(list.ToArray());
		}

		public static T GetComponentEvenIfDeactive<T>(GameObject gameObject)
		{
			List<GameObject> list = new List<GameObject>();
			GameObject gameObject2 = gameObject;
			while (gameObject2 != null && !gameObject2.activeInHierarchy)
			{
				if (!gameObject2.activeSelf)
				{
					list.Add(gameObject2);
				}
				gameObject2.SetActive(true);
				gameObject2 = gameObject2.transform.parent.gameObject;
			}
			T component = gameObject.GetComponent<T>();
			foreach (GameObject gameObject3 in list)
			{
				gameObject3.SetActive(false);
			}
			return component;
		}
	}
}
