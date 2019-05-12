using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HoldPressButton : HoldPressButtonBase
{
	public static HoldPressButton current;

	private bool _isDragged;

	private Vector2? _draggedPosisiton;

	public bool isDragged
	{
		get
		{
			return this._isDragged;
		}
	}

	private void Update()
	{
		if (!this.pressed)
		{
			return;
		}
		if (!base.isActiveAndEnabled)
		{
			this.nextTime = Time.unscaledTime;
			return;
		}
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

	private void OnPress(bool onPressed)
	{
		if (!base.isActiveAndEnabled)
		{
			return;
		}
		base.OnPressStartEnd(onPressed);
	}

	protected override void OnPressAdvanced()
	{
		this._draggedPosisiton = null;
		this._isDragged = false;
	}

	private void OnDrag(Vector2 delta)
	{
		if (!base.isActiveAndEnabled)
		{
			return;
		}
		Vector2? draggedPosisiton = this._draggedPosisiton;
		if (draggedPosisiton == null)
		{
			this._draggedPosisiton = new Vector2?(delta);
		}
		if (this._draggedPosisiton.Value != delta)
		{
			this._isDragged = true;
		}
		else
		{
			this._isDragged = false;
		}
	}
}
