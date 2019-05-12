using System;
using UnityEngine;

public class UITweenController : MonoBehaviour
{
	[SerializeField]
	private bool _afterObjectDisable;

	private UITweener[] _uiTweeners = new UITweener[0];

	private bool[] _isFinished = new bool[0];

	private bool _isFinishedFlag;

	public bool afterObjectDisable
	{
		get
		{
			return this._afterObjectDisable;
		}
		set
		{
			this._afterObjectDisable = value;
		}
	}

	public bool isFinished
	{
		get
		{
			return this._isFinishedFlag;
		}
	}

	public bool GetObject()
	{
		UITweenController.Callback callback = new UITweenController.Callback(this.OnFinishedAnimation);
		this._uiTweeners = base.GetComponentsInChildren<UITweener>();
		this._isFinished = new bool[this._uiTweeners.Length];
		for (int i = 0; i < this._isFinished.Length; i++)
		{
			EventDelegate eventDelegate = new EventDelegate(this, callback.Method.Name);
			eventDelegate.parameters[0] = new EventDelegate.Parameter(i);
			this._uiTweeners[i].onFinished.Add(eventDelegate);
		}
		return this._uiTweeners.Length > 0;
	}

	private void OnEnable()
	{
		for (int i = 0; i < this._isFinished.Length; i++)
		{
			this._isFinished[i] = false;
		}
	}

	private void Update()
	{
		this._isFinishedFlag = false;
		for (int i = 0; i < this._isFinished.Length; i++)
		{
			if (!this._isFinished[i])
			{
				return;
			}
		}
		this._isFinishedFlag = true;
		this.AfterObjectDisabledFunction();
	}

	private void OnFinishedAnimation(int index)
	{
		this._isFinished[index] = true;
	}

	protected virtual void AfterObjectDisabledFunction()
	{
		base.gameObject.SetActive(false);
	}

	private delegate void Callback(int x);
}
