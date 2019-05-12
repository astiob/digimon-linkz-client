using System;
using UnityEngine;

public class SendMoveCollider : GUICollider
{
	[SerializeField]
	private GUICollider parentCollider;

	public override void OnTouchBegan(Touch touch, Vector2 pos)
	{
		base.OnTouchBegan(touch, pos);
		if (this.parentCollider != null)
		{
			this.parentCollider.OnTouchBegan(touch, pos);
		}
	}

	public override void OnTouchMoved(Touch touch, Vector2 pos)
	{
		base.OnTouchMoved(touch, pos);
		if (this.parentCollider != null)
		{
			this.parentCollider.OnTouchMoved(touch, pos);
		}
	}

	public override void OnTouchEnded(Touch touch, Vector2 pos, bool flag)
	{
		base.OnTouchEnded(touch, pos, flag);
		if (this.parentCollider != null)
		{
			this.parentCollider.OnTouchEnded(touch, pos, flag);
		}
	}
}
