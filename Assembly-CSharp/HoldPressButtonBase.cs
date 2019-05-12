using System;
using System.Collections.Generic;
using UnityEngine;

public class HoldPressButtonBase : MonoBehaviour
{
	[Tooltip("押しっぱなし判定の間隔（この間隔毎にActionHoldPressが呼ばれる）")]
	public float intervalAction = 0.2f;

	[Tooltip("押し始めてから指定秒数後に呼ばれる関数")]
	public float waitPressCall = 0.2f;

	public List<EventDelegate> onClick = new List<EventDelegate>();

	public List<EventDelegate> onStartPress = new List<EventDelegate>();

	public List<EventDelegate> onHoldPress = new List<EventDelegate>();

	public List<EventDelegate> onHoldWaitPress = new List<EventDelegate>();

	public List<EventDelegate> onDisengagePress = new List<EventDelegate>();

	protected float nextTime;

	protected float waitTimeCount;

	[SerializeField]
	protected bool pressed;

	protected bool isCalledonHoldWaitPress;

	public bool isPressing
	{
		get
		{
			return this.pressed;
		}
	}

	protected void PressCount()
	{
		if (this.nextTime < Time.unscaledTime)
		{
			this.nextTime = Time.unscaledTime + this.intervalAction;
			EventDelegate.Execute(this.onHoldPress);
		}
		if (this.waitTimeCount + this.waitPressCall < Time.unscaledTime && !this.isCalledonHoldWaitPress)
		{
			EventDelegate.Execute(this.onHoldWaitPress);
			this.isCalledonHoldWaitPress = true;
		}
	}

	protected void OnClick()
	{
		if (!base.isActiveAndEnabled)
		{
			return;
		}
		if (this.isCalledonHoldWaitPress)
		{
			return;
		}
		EventDelegate.Execute(this.onClick);
	}

	protected void OnPressStartEnd(bool onPressed)
	{
		this.nextTime = Time.unscaledTime + this.intervalAction;
		if (!onPressed)
		{
			this.pressed = onPressed;
			EventDelegate.Execute(this.onDisengagePress);
		}
		else
		{
			this.isCalledonHoldWaitPress = false;
			this.waitTimeCount = Time.unscaledTime;
			this.OnPressAdvanced();
			EventDelegate.Execute(this.onStartPress);
			this.pressed = onPressed;
		}
	}

	protected virtual void OnPressAdvanced()
	{
	}
}
