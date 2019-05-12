using System;
using UnityEngine;

public class GUIBase : MonoBehaviour
{
	[SerializeField]
	private bool resident_;

	private bool startFlag_;

	private Vector3 firstPos;

	public bool resident
	{
		get
		{
			return this.resident_;
		}
		set
		{
			this.resident_ = value;
		}
	}

	public bool startFlag
	{
		get
		{
			return this.startFlag_;
		}
	}

	protected virtual void Awake()
	{
		Vector3 localPosition = base.transform.localPosition;
		localPosition.x = 0f;
		localPosition.y = 0f;
		base.transform.localPosition = localPosition;
	}

	protected virtual void Start()
	{
		this.firstPos = base.transform.position;
		GUIManager.AddGUIBase(this);
		this.startFlag_ = true;
	}

	public void ReturnPos()
	{
		base.transform.position = this.firstPos;
	}

	protected virtual void Update()
	{
	}

	public virtual void ShowGUI()
	{
	}

	public virtual void HideGUI()
	{
	}

	public virtual void OnDestroy()
	{
		GUIManager.DeleteGUIBase(this);
		this.startFlag_ = false;
	}
}
