using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("GUI/DepthController")]
[ExecuteInEditMode]
public class DepthController : MonoBehaviour
{
	private int myDepthNow;

	private int depth;

	private int add;

	private bool ADD_MODE = true;

	private int min = int.MaxValue;

	private int max = int.MinValue;

	private bool isCheckWidgetDepth;

	private List<GameObject> gameObjectList = new List<GameObject>();

	public void CheckWidgetDepth(Transform tm)
	{
		this.gameObjectList.Clear();
		GameObject gameObject = tm.gameObject;
		while (!gameObject.activeInHierarchy)
		{
			if (!gameObject.activeSelf)
			{
				this.gameObjectList.Add(gameObject);
			}
			gameObject.SetActive(true);
			gameObject = gameObject.transform.parent.gameObject;
		}
		UIWidget component = tm.GetComponent<UIWidget>();
		if (component != null)
		{
			if (!tm.GetComponent<DepthControllerHash>())
			{
				DepthControllerHash depthControllerHash = tm.gameObject.AddComponent<DepthControllerHash>();
				depthControllerHash.originDepth = component.depth;
			}
			this.min = Mathf.Min(this.min, component.depth);
			this.max = Mathf.Max(this.max, component.depth);
		}
		foreach (GameObject gameObject2 in this.gameObjectList)
		{
			gameObject2.SetActive(false);
		}
		foreach (object obj in tm)
		{
			Transform tm2 = (Transform)obj;
			this.CheckWidgetDepth(tm2);
		}
	}

	private void Awake()
	{
		UIWidget component = base.GetComponent<UIWidget>();
		if (Application.isPlaying)
		{
			return;
		}
		if (this.ADD_MODE)
		{
			if (component != null)
			{
				this.myDepthNow = component.depth;
			}
		}
		else if (component != null)
		{
			this.myDepthNow = component.depth;
			this.depth = component.depth;
			this.SetWidgetDepth(base.transform, this.depth);
		}
	}

	private void Update()
	{
		if (Application.isPlaying)
		{
			return;
		}
		if (this.ADD_MODE)
		{
			UIWidget component = base.gameObject.GetComponent<UIWidget>();
			if (component != null && this.myDepthNow != component.depth)
			{
				this.add = component.depth - this.myDepthNow;
				component.depth = this.myDepthNow;
				this.AddWidgetDepth(base.transform, this.add);
				this.myDepthNow = component.depth;
			}
		}
		else
		{
			UIWidget component2 = base.gameObject.GetComponent<UIWidget>();
			if (component2 != null && this.myDepthNow != component2.depth)
			{
				this.myDepthNow = component2.depth;
				this.depth = component2.depth;
				this.SetWidgetDepth(base.transform, this.depth);
			}
		}
	}

	private void SetWidgetDepthInternal(Transform tm, int depth)
	{
		UIWidget component = tm.GetComponent<UIWidget>();
		DepthControllerHash component2 = tm.GetComponent<DepthControllerHash>();
		int num = this.max - this.min;
		if (component != null && component2 != null)
		{
			component.depth = num * depth + component2.originDepth;
		}
		foreach (object obj in tm)
		{
			Transform tm2 = (Transform)obj;
			this.SetWidgetDepthInternal(tm2, depth);
		}
	}

	public void SetWidgetDepth(Transform tm, int depth)
	{
		if (!this.isCheckWidgetDepth)
		{
			this.min = int.MaxValue;
			this.max = int.MinValue;
			this.CheckWidgetDepth(tm);
			this.isCheckWidgetDepth = true;
		}
		this.SetWidgetDepthInternal(tm, depth);
	}

	public void AddWidgetDepth(Transform tm, int add)
	{
		UIWidget component = tm.gameObject.GetComponent<UIWidget>();
		if (component != null)
		{
			component.depth += add;
		}
		foreach (object obj in tm)
		{
			Transform tm2 = (Transform)obj;
			this.AddWidgetDepth(tm2, add);
		}
	}

	public void SetWidgetDepth(int depth)
	{
		this.SetWidgetDepth(base.transform, depth);
	}

	public void AddWidgetDepth(int add)
	{
		this.AddWidgetDepth(base.transform, add);
	}

	public static void SetWidgetDepth_2(Transform tm, int depth)
	{
		UIWidget component = tm.gameObject.GetComponent<UIWidget>();
		if (component != null)
		{
			int num = depth - component.depth;
			DepthController.AddWidgetDepth_2(tm, num);
		}
	}

	public static void AddWidgetDepth_2(Transform tm, int add)
	{
		UIWidget component = tm.gameObject.GetComponent<UIWidget>();
		if (component != null)
		{
			component.depth += add;
		}
		foreach (object obj in tm)
		{
			Transform tm2 = (Transform)obj;
			DepthController.AddWidgetDepth_2(tm2, add);
		}
	}

	public static void RefreshWidgetDrawCall(Transform tm)
	{
		UIWidget component = tm.gameObject.GetComponent<UIWidget>();
		if (component != null)
		{
			component.SetDirty();
		}
		foreach (object obj in tm)
		{
			Transform tm2 = (Transform)obj;
			DepthController.RefreshWidgetDrawCall(tm2);
		}
	}
}
