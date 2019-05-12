using System;
using UnityEngine;

public sealed class DebugLogScreen : Singleton<DebugLogScreen>
{
	private void Awake()
	{
		UILabel component = base.GetComponent<UILabel>();
		component.enabled = false;
		Transform transform = base.transform;
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			child.gameObject.SetActive(false);
		}
	}

	public void Print(string noop)
	{
	}
}
