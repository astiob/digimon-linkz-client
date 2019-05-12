using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("GUI/DepthController")]
public class DepthController : MonoBehaviour
{
	private bool ADD_MODE = true;

	private int _myDepthNow;

	private int _depth;

	private int _addValue;

	private int _minValue = int.MaxValue;

	private int _maxValue = int.MinValue;

	private UIWidget _uiWidget;

	private bool _isCheckWidgetDepth;

	private List<GameObject> _gameObjectList = new List<GameObject>();

	private Dictionary<int, List<UIWidget>> _childWidget = new Dictionary<int, List<UIWidget>>();

	private void Awake()
	{
		this._uiWidget = base.GetComponent<UIWidget>();
		this.InitWidgetDepth(base.transform);
		if (Application.isPlaying)
		{
			return;
		}
		if (this.ADD_MODE)
		{
			if (this._uiWidget != null)
			{
				this._myDepthNow = this._uiWidget.depth;
			}
		}
		else if (this._uiWidget != null)
		{
			this._myDepthNow = this._uiWidget.depth;
			this._depth = this._uiWidget.depth;
			this.SetWidgetDepth(base.transform, this._depth);
		}
	}

	public void CheckWidgetDepth(Transform tm)
	{
		this._gameObjectList.Clear();
		GameObject gameObject = tm.gameObject;
		while (!gameObject.activeInHierarchy)
		{
			if (!gameObject.activeSelf)
			{
				this._gameObjectList.Add(gameObject);
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
				if (!this._childWidget.ContainsKey(depthControllerHash.originDepth))
				{
					this._childWidget.Add(depthControllerHash.originDepth, new List<UIWidget>());
				}
				this._childWidget[depthControllerHash.originDepth].Add(component);
			}
			this._minValue = Mathf.Min(this._minValue, component.depth);
			this._maxValue = Mathf.Max(this._maxValue, component.depth);
		}
		foreach (GameObject gameObject2 in this._gameObjectList)
		{
			gameObject2.SetActive(false);
		}
		IEnumerator enumerator2 = tm.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				object obj = enumerator2.Current;
				Transform tm2 = (Transform)obj;
				this.CheckWidgetDepth(tm2);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator2 as IDisposable)) != null)
			{
				disposable.Dispose();
			}
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
			if (component != null && this._myDepthNow != component.depth)
			{
				this._addValue = component.depth - this._myDepthNow;
				component.depth = this._myDepthNow;
				this.AddWidgetDepth(base.transform, this._addValue);
				this._myDepthNow = component.depth;
			}
		}
		else
		{
			UIWidget component2 = base.gameObject.GetComponent<UIWidget>();
			if (component2 != null && this._myDepthNow != component2.depth)
			{
				this._myDepthNow = component2.depth;
				this._depth = component2.depth;
				this.SetWidgetDepth(base.transform, this._depth);
			}
		}
	}

	private void InitWidgetDepth(Transform t)
	{
		if (!this._isCheckWidgetDepth)
		{
			this._minValue = int.MaxValue;
			this._maxValue = int.MinValue;
			this.CheckWidgetDepth(t);
			this._isCheckWidgetDepth = true;
		}
	}

	public void SetWidgetDepth(Transform tm, int depth)
	{
		this.InitWidgetDepth(tm);
		List<int> list = new List<int>(this._childWidget.Keys);
		for (int i = 0; i < list.Count; i++)
		{
			int num = list[i];
			for (int j = 0; j < this._childWidget[num].Count; j++)
			{
				int num2 = this._maxValue - this._minValue;
				this._childWidget[num][j].depth = num2 * depth + num;
			}
		}
	}

	public void SetWidgetManualDepth(Transform tm, int depth)
	{
		this.InitWidgetDepth(tm);
		List<int> list = new List<int>(this._childWidget.Keys);
		for (int i = 0; i < list.Count; i++)
		{
			int key = list[i];
			for (int j = 0; j < this._childWidget[key].Count; j++)
			{
				int num = this._maxValue - this._minValue;
				DepthControllerHash component = this._childWidget[key][j].GetComponent<DepthControllerHash>();
				this._childWidget[key][j].manualDepth = num * depth + component.originDepth;
				this._childWidget[key][j].SetDepthFromManualDepth();
			}
		}
	}

	public void AddWidgetDepth(Transform tm, int add)
	{
		UIWidget component = tm.gameObject.GetComponent<UIWidget>();
		if (component != null)
		{
			component.depth += add;
		}
		IEnumerator enumerator = tm.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform tm2 = (Transform)obj;
				this.AddWidgetDepth(tm2, add);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	public void SetWidgetDepth(int depth)
	{
		this.SetWidgetDepth(base.transform, depth);
	}

	public void SetWidgetManualDepth(int depth)
	{
		this.SetWidgetManualDepth(base.transform, depth);
	}

	public void AddWidgetDepth(int add)
	{
		this.AddWidgetDepth(base.transform, add);
	}

	public UIPanel GetPanel()
	{
		if (this._uiWidget == null)
		{
			return null;
		}
		if (this._uiWidget.panel == null)
		{
			return this._uiWidget.transform.parent.GetComponent<UIPanel>();
		}
		return this._uiWidget.panel;
	}

	public int GetManualDepth()
	{
		if (this._uiWidget == null)
		{
			return 0;
		}
		return this._uiWidget.manualDepth;
	}

	public int GetDepth()
	{
		if (this._uiWidget == null)
		{
			return 0;
		}
		return this._uiWidget.depth;
	}

	public static void SetWidgetDepth_Static(Transform tm, int depth)
	{
		UIWidget component = tm.gameObject.GetComponent<UIWidget>();
		if (component != null)
		{
			int add = depth - component.depth;
			DepthController.AddWidgetDepth_Static(tm, add);
		}
	}

	public static void AddWidgetDepth_Static(Transform tm, int add)
	{
		UIWidget component = tm.gameObject.GetComponent<UIWidget>();
		if (component != null)
		{
			component.depth += add;
		}
		IEnumerator enumerator = tm.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform tm2 = (Transform)obj;
				DepthController.AddWidgetDepth_Static(tm2, add);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	public static void RefreshWidgetDrawCall(Transform tm)
	{
		UIWidget component = tm.gameObject.GetComponent<UIWidget>();
		if (component != null)
		{
			component.SetDirty();
		}
		IEnumerator enumerator = tm.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform tm2 = (Transform)obj;
				DepthController.RefreshWidgetDrawCall(tm2);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}
}
