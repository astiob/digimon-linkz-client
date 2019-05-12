using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAnchor : MonoBehaviour
{
	[SerializeField]
	private string _key;

	[SerializeField]
	private Transform _anchor;

	public static bool GetAnchorWithKey(Transform ObjectRoot, string Key, out ObjectAnchor OutObjectAnchor)
	{
		bool activeSelf = ObjectRoot.gameObject.activeSelf;
		ObjectRoot.gameObject.SetActive(true);
		List<ObjectAnchor> list = new List<ObjectAnchor>(ObjectRoot.GetComponentsInChildren<ObjectAnchor>());
		foreach (ObjectAnchor objectAnchor in list)
		{
			if (objectAnchor.key == Key)
			{
				OutObjectAnchor = objectAnchor;
				ObjectRoot.gameObject.SetActive(activeSelf);
				return true;
			}
		}
		OutObjectAnchor = null;
		return false;
	}

	public string key
	{
		get
		{
			return this._key;
		}
	}

	public Transform anchor
	{
		get
		{
			return this._anchor;
		}
	}
}
