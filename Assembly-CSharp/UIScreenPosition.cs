using System;
using System.Collections.Generic;
using UnityEngine;

public class UIScreenPosition : MonoBehaviour
{
	public bool localMode;

	public int currentIndex;

	public List<UIScreenPosition.UIScreenPositionObject> uiScreenPositionObjects = new List<UIScreenPosition.UIScreenPositionObject>();

	private void Awake()
	{
		if (this.uiScreenPositionObjects.Count <= 0)
		{
			this.uiScreenPositionObjects.Add(new UIScreenPosition.UIScreenPositionObject(base.gameObject, this.localMode));
		}
	}

	private void OnEnable()
	{
		this.ApplyPosition();
	}

	private void OnDisable()
	{
		this.ApplyPosition();
	}

	private void Start()
	{
		this.ApplyPosition();
	}

	public void ApplyPosition()
	{
		if (this.uiScreenPositionObjects.Count > this.currentIndex && this.uiScreenPositionObjects[this.currentIndex] != null)
		{
			this.uiScreenPositionObjects[this.currentIndex].ApplyPosition(base.transform, this.localMode);
		}
	}

	public void SetScreenPosition(int index)
	{
		this.currentIndex = index;
		this.ApplyPosition();
	}

	public int Length
	{
		get
		{
			return this.uiScreenPositionObjects.Count;
		}
	}

	[Serializable]
	public class UIScreenPositionObject
	{
		public bool useManualPosition;

		public Transform transform;

		public Vector2 position;

		public UIScreenPositionObject(GameObject g, bool localMode)
		{
			this.useManualPosition = false;
			this.transform = null;
			if (!localMode)
			{
				this.position = g.transform.position;
			}
			else
			{
				this.position = g.transform.localPosition;
			}
		}

		public void ApplyPosition(Transform pos, bool localMode)
		{
			if (!localMode)
			{
				if (this.useManualPosition && this.transform != null)
				{
					pos.position = this.transform.position;
				}
				else
				{
					pos.position = this.position;
				}
			}
			else if (this.useManualPosition && this.transform != null)
			{
				pos.localPosition = this.transform.localPosition;
			}
			else
			{
				pos.localPosition = this.position;
			}
		}
	}
}
