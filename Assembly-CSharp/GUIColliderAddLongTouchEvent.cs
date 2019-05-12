using System;
using UnityEngine;

public class GUIColliderAddLongTouchEvent : GUICollider
{
	[SerializeField]
	private float longTouchTime;

	private float touchBeganTime;

	private bool isPressing;

	private bool isLongTouched;

	protected Action actionLongPress;

	private void ReadyLongTouchParam()
	{
		this.touchBeganTime = Time.realtimeSinceStartup;
		this.isPressing = true;
		this.isLongTouched = false;
	}

	public override void OnTouchBegan(Touch touch, Vector2 pos)
	{
		if (GUICollider.IsAllColliderDisable() || !base.activeCollider)
		{
			return;
		}
		base.OnTouchBegan(touch, pos);
		this.ReadyLongTouchParam();
	}

	public override void OnTouchEnded(Touch touch, Vector2 pos, bool flag)
	{
		if (GUICollider.IsAllColliderDisable() || !base.activeCollider)
		{
			return;
		}
		this.isPressing = false;
		if (!this.isLongTouched)
		{
			base.OnTouchEnded(touch, pos, flag);
		}
		else
		{
			base.isTouching = false;
		}
		this.isLongTouched = false;
	}

	protected override void Update()
	{
		base.Update();
		if (this.isPressing)
		{
			float num = Time.realtimeSinceStartup - this.touchBeganTime;
			if (num >= this.longTouchTime)
			{
				this.isLongTouched = true;
				this.isPressing = false;
				if (this.actionLongPress != null)
				{
					this.actionLongPress();
				}
			}
		}
	}
}
