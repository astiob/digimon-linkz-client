using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UITweener))]
public class UITweenEventSystem : MonoBehaviour
{
	public List<EventDelegate> onFinished = new List<EventDelegate>();

	private UITweener tweener;

	private void Awake()
	{
		this.tweener = base.GetComponent<UITweener>();
		EventDelegate.Add(this.tweener.onFinished, new EventDelegate.Callback(this.CallDelegates));
	}

	private void CallDelegates()
	{
		if (this.tweener.tweenFactor == 0f)
		{
			EventDelegate.Execute(this.onFinished);
		}
	}
}
